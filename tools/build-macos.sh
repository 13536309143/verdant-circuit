#!/usr/bin/env bash
set -euo pipefail
verdant_project_dir="$(cd "$(dirname "$0")/.." && pwd)"
verdant_version="0.10.2"
verdant_release_dir="$verdant_project_dir/Builds/release"
if [[ -z "${VERDANT_UNITY_EDITOR:-}" || ! -x "$VERDANT_UNITY_EDITOR" ]]; then
  echo 'Set VERDANT_UNITY_EDITOR to your Unity Editor executable.' >&2; exit 1
fi
mkdir -p "$verdant_release_dir"
"$VERDANT_UNITY_EDITOR" -batchmode -quit -projectPath "$verdant_project_dir" \
  -executeMethod VerdantLocalizationSetup.Setup -logFile "$verdant_release_dir/localization.log"
"$VERDANT_UNITY_EDITOR" -batchmode -quit -projectPath "$verdant_project_dir" \
  -executeMethod VerdantRelease.Verify -logFile "$verdant_release_dir/verify.log"
export VERDANT_BUILD_PATH="$verdant_release_dir/HeXu-$verdant_version.app"
"$VERDANT_UNITY_EDITOR" -batchmode -quit -projectPath "$verdant_project_dir" \
  -buildTarget StandaloneOSX -executeMethod VerdantRelease.BuildMac -logFile "$verdant_release_dir/build-mac.log"
ditto -c -k --sequesterRsrc --keepParent "$VERDANT_BUILD_PATH" "$verdant_release_dir/HeXu-macOS-$verdant_version.zip"
(cd "$verdant_release_dir" && shasum -a 256 "HeXu-macOS-$verdant_version.zip" > "SHA256SUMS-macOS-$verdant_version.txt")
echo "Completed: $verdant_release_dir"
