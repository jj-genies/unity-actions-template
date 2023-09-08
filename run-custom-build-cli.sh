#!/bin/bash

/Applications/Unity/Hub/Editor/2021.3.26f1//Unity.app/Contents/MacOS/Unity \
-batchmode \
-nographics \
-quitTimeout 21600 \
-username $UNITY_USERNAME \
-password $UNITY_PASSWORD \
-serial $UNITY_SERIAL \
-projectPath /Users/josevazquez/repos/UnityActionsTemplate/. \
-platform "StandaloneOSX" \
-executeMethod Genies.Editor.Build.HeadlessDynamicBuildScript.HeadlessBuild \
-profile default 
