@echo off
setlocal
set PATH=%PATH%;%ProgramFiles(x86)%\Microsoft SDKs\F#\3.1\Framework\v4.0
set PATH=%PATH%;%ProgramFiles(x86)%\Microsoft SDKs\F#\3.0\Framework\v4.0
set PATH=%PATH%;%ProgramFiles%\Microsoft SDKs\F#\3.1\Framework\v4.0
set PATH=%PATH%;%ProgramFiles%\Microsoft SDKs\F#\3.0\Framework\v4.0
set PATH=%PATH%;tools
nuget install IntelliFactory.Build -pre -nocache -excludeversion -o packages
fsi --exec build.fsx %*