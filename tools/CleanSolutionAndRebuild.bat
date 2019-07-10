@echo off

pushd %~dp0%
pushd ..
powershell .\tools\CleanSolution.ps1 -Force

pushd src

REM .\.paket\paket.exe restore

dotnet restore --force

REM dotnet build -c Debug -p:Platform=x86
REM dotnet build -c Debug -p:Platform=x64

REM ..\tools\msbuild.bat Alex.sln /t:Build /p:Configuration=Debug;Platform=x86 /m
..\tools\msbuild.bat Alex.sln /t:Build /p:Configuration=Debug;Platform=x64 /m

popd
popd
popd
