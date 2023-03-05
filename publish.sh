#!/usr/bin/env bash

readonly VERSION="0.1"
readonly TARGETS=(osx.13-arm64 osx-x64 linux-x64 linux-arm64 win-x64 win-arm64)
readonly PUBLISH_PATH="Warb/bin/Release/net7.0"
readonly RELEASE_PATH="release"

for t in "${TARGETS[@]}"; do
  dotnet publish -r $t --configuration release
done

mkdir -p $RELEASE_PATH

working_dir=$(pwd)
cd $RELEASE_PATH
rm -rf *
full_release_path=$(pwd)
cd ..

for t in "${TARGETS[@]}"; do
  cd "${PUBLISH_PATH}/${t}/publish"
  zip -r "$full_release_path/${VERSION}-${t}.zip" .
  cd $working_dir
done;