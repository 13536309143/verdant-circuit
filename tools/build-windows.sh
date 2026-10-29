#!/usr/bin/env bash
# macOS host; install Windows Build Support (Mono) for this Editor first.
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
export VERDANT_WINDOWS_BUILD_DIR="$verdant_release_dir/HeXu-Windows-x64-$verdant_version"
"$VERDANT_UNITY_EDITOR" -batchmode -quit -projectPath "$verdant_project_dir" \
  -buildTarget StandaloneWindows64 -executeMethod VerdantRelease.BuildWindows -logFile "$verdant_release_dir/build-windows.log"
(cd "$verdant_release_dir" && /usr/bin/zip -qr "HeXu-Windows-x64-$verdant_version.zip" "HeXu-Windows-x64-$verdant_version")
(cd "$verdant_release_dir" && shasum -a 256 "HeXu-Windows-x64-$verdant_version.zip" > "SHA256SUMS-Windows-$verdant_version.txt")
echo "Completed: $verdant_release_dir"
