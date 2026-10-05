@echo off
setlocal EnableExtensions

set ROOT=%~dp0..
set SRC=%ROOT%\src
set REPORT=%SRC%\release\release-report.txt

if not exist "%SRC%\MXDBB Opti.exe" (
  echo BUILD: FAIL
  exit /b 1
)

if not exist "%ROOT%\README.md" (
  echo README: FAIL
  exit /b 1
)

set FAIL=0
> "%REPORT%" echo MXDBB Opti Release Check
>>"%REPORT%" echo =========================
>>"%REPORT%" echo.

call :checkfile "src\Program.cs"
call :checkfile "src\App.cs"
call :checkfile "src\Advanced.cs"
call :checkfile "src\EmbeddedLogo.cs"
call :checkfile "src\AssemblyInfo.cs"
call :checkfile "src\app.manifest"
call :checkfile "src\build.cmd"
call :checkfile ".github\workflows\build.yml"

if exist "%SRC%\bull.png" (
  >>"%REPORT%" echo RESOURCE bull.png: PASS
) else (
  >>"%REPORT%" echo RESOURCE bull.png: WARN - embedded logo fallback is used
)

if exist "%SRC%\icon.ico" (
  >>"%REPORT%" echo RESOURCE icon.ico: PASS
) else (
  >>"%REPORT%" echo RESOURCE icon.ico: WARN - executable may use default icon
)

>>"%REPORT%" echo.
if "%FAIL%"=="0" (
  >>"%REPORT%" echo RELEASE CHECK: PASS
  echo RELEASE CHECK: PASS
  exit /b 0
) else (
  >>"%REPORT%" echo RELEASE CHECK: FAIL
  echo RELEASE CHECK: FAIL
  exit /b 1
)

:checkfile
if exist "%ROOT%\%~1" (
  >>"%REPORT%" echo FILE %~1: PASS
) else (
  >>"%REPORT%" echo FILE %~1: FAIL
  set FAIL=1
)
exit /b 0
