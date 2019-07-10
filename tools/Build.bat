@echo off

pushd %~dp0%
pushd ..

pushd src

REM .\.paket\paket.exe restore

dotnet restore --force

REM dotnet build -c Debug -p:Platform=x86
REM dotnet build -c Debug -p:Platform=x64

..\tools\msbuild.bat Alex.sln

popd
popd
popd
