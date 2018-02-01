@echo off
RMDIR /S /Q "Bin"
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S bin') DO RMDIR /S /Q "%%G"
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S obj') DO RMDIR /S /Q "%%G"

IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe" (
	Set SCPMSBuild="%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe"
	Set SCPVSVer=15.0
	Echo Found VS 2017 Community
) ELSE IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\MSBuild.exe" (
	Set SCPMSBuild="%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\MSBuild.exe"
	Set SCPVSVer=15.0
	Echo Found VS 2017 Professional
) ELSE IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe" (
	Set SCPMSBuild="%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe"
	Set SCPVSVer=15.0
	Echo Found VS 2017 Enterprise
) ELSE IF EXIST "%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" (
	Set SCPMSBuild="%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe"
	Set SCPVSVer=14.0
	Echo Found VS 2015
)

%SCPMSBuild% build.xml /target:Deploy /p:BuildConfiguration=Debug /p:Version="1.4.0" /p:FileVersion="1.4.0" /p:VersionLabel="1.4.0" /v:n /fileLogger /m /p:VisualStudioVersion=%SCPVSVer%