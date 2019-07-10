@echo off
setlocal enabledelayedexpansion

for /f "usebackq tokens=*" %%i in (`%~dp0%\vswhere.exe -latest -prerelease -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do (
  "%%i" %*
  exit /b !errorlevel!
)
