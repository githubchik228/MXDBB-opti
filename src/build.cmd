@echo off
setlocal
set FW=C:\Windows\Microsoft.NET\Framework64\v4.0.30319
set WPF=%FW%\WPF
cd /d "%~dp0"
"%FW%\csc.exe" /nologo /target:winexe /out:"MXDBB Opti.exe" /win32manifest:app.manifest /reference:"%WPF%\PresentationFramework.dll" /reference:"%WPF%\PresentationCore.dll" /reference:"%WPF%\WindowsBase.dll" /reference:"%FW%\System.Xaml.dll" /reference:System.dll /reference:System.Core.dll Program.cs App.cs AssemblyInfo.cs
if %errorlevel%==0 (echo BUILD OK) else (echo BUILD FAILED %errorlevel%)
pause
