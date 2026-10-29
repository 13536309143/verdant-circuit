#!/usr/bin/env bash
set -euo pipefail
verdant_project_dir="$(cd "$(dirname "$0")/.." && pwd)"
verdant_version="0.10.1"
verdant_release_dir="$verdant_project_dir/Builds/release"
if [[ -z "${VERDANT_UNITY_EDITOR:-}" || ! -x "$VERDANT_UNITY_EDITOR" ]]; then
  echo '请将 VERDANT_UNITY_EDITOR 指向 Unity.app/Contents/MacOS/Unity。' >&2
  exit 1
fi
mkdir -p "$verdant_release_dir"
"$VERDANT_UNITY_EDITOR" -batchmode -quit -projectPath "$verdant_project_dir" \
  -executeMethod VerdantRelease.Verify -logFile "$verdant_release_dir/verify.log"
export VERDANT_BUILD_PATH="$verdant_release_dir/HeXu-$verdant_version.app"
"$VERDANT_UNITY_EDITOR" -batchmode -quit -projectPath "$verdant_project_dir" \
  -buildTarget StandaloneOSX -executeMethod VerdantRelease.BuildMac \
  -logFile "$verdant_release_dir/build.log"
ditto -c -k --sequesterRsrc --keepParent "$VERDANT_BUILD_PATH" \
  "$verdant_release_dir/HeXu-macOS-$verdant_version.zip"
(cd "$verdant_release_dir" && shasum -a 256 "HeXu-macOS-$verdant_version.zip" > SHA256SUMS.txt)
echo "完成：$verdant_release_dir"
