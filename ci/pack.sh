#!/bin/bash
set -x
set -o errexit

apt update && apt -y install tar git zstd
mkdir dist replay expansions icon
cp -rf per-platform/$TARGET_PLATFORM/* . || true

ARCHIVE_FILES=(LICENSE cdb config deck expansions icon pack picture puzzle replay sound texture script)
if [[ "$TARGET_PLATFORM" == "darwin" ]]; then
  mv Builds/StandaloneOSX/YGOPro2.app .
  ARCHIVE_FILES=("${ARCHIVE_FILES[@]}" YGOPro2.app)
fi
if [[ "$TARGET_PLATFORM" == "linux" ]]; then
  mv Builds/StandaloneLinux64/* .
  ARCHIVE_FILES=("${ARCHIVE_FILES[@]}" LinuxPlayer_s.debug UnityPlayer_s.debug UnityPlayer.so YGOPro2 YGOPro2_Data)
fi
if [[ "$TARGET_PLATFORM" == "win32" ]]; then
  mv Builds/StandaloneWindows64/* .
  ARCHIVE_FILES=("${ARCHIVE_FILES[@]}" MonoBleedingEdge UnityCrashHandler64.exe UnityPlayer.dll YGOPro2_Data YGOPro2.exe)
fi

ARCHIVE_SUFFIX="zst"

tar -acf "dist/ygopro2-$CI_COMMIT_REF_NAME-$TARGET_PLATFORM-$TARGET_LOCALE.tar.$ARCHIVE_SUFFIX" --exclude='.git*' "${ARCHIVE_FILES[@]}"
