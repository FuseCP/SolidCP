@echo off
RMDIR /S /Q "Bin"
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S bin') DO RMDIR /S /Q "%%G"
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S obj') DO RMDIR /S /Q "%%G"
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S bin_dotnet') DO RMDIR /S /Q "%%G"


IF defined MSBUILD_SWITCHES (
	Set MsBuildSwitches = "%MSBUILD_SWITCHES%"
) ELSE (
	Set MsBuildSwitches = " /m "
)
IF defined SOLIDCP_VERSION (
	Set SolidCPVersion = "%SOLIDCP_VERSION%"
) ELSE (
	Set SolidCPVersion = "1.4.5"
)
IF defined SOLIDCP_FILEVERSION (
	Set SolidCPFileVersion = "%SOLIDCP_FILEVERSION%"
) ELSE (
	Set SolidCPFileVersion = "1.4.9"
)

IF EXIST "%ProgramFiles%\Microsoft Visual Studio\2022\Community\MSBuild\Current\bin\MSBuild.exe" (
	Set SCPMSBuild="%ProgramFiles%\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
	Set SCPVSVer=17.0
	Echo Found VS 2022 Community
	GOTO Build 
 )
IF EXIST "%ProgramFiles%\Microsoft Visual Studio\2022\Professional\MSBuild\Current\bin\MSBuild.exe" (
	Set SCPMSBuild="%ProgramFiles%\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
	Set SCPVSVer=17.0
	Echo Found VS 2022 Professional
	GOTO Build 
 )
IF EXIST "%ProgramFiles%\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\bin\MSBuild.exe" (
	Set SCPMSBuild="%ProgramFiles%\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe"
	Set SCPVSVer=17.0
	Echo Found VS 2022 Enterprise
	GOTO Build 
 )
IF EXIST "%ProgramFiles%\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\MSBuild.exe" (
	Set SCPMSBuild="%ProgramFiles%\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\MSBuild.exe"
	Set SCPVSVer=17.0
	Echo Found VS 2022 Preview
	GOTO Build 
 )
IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" (
	Set SCPMSBuild="%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
	Set SCPVSVer=16.0
	Echo Found VS 2019 Community
	GOTO Build 
 )
IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe" (
	Set SCPMSBuild="%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe"
	Set SCPVSVer=16.0
	Echo Found VS 2019 Professional
	GOTO Build 
 )
IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe" (
	Set SCPMSBuild="%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe"
	Set SCPVSVer=16.0
	Echo Found VS 2019 Enterprise
	GOTO Build 
 )
IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe" (
	Set SCPMSBuild="%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe"
	Set SCPVSVer=15.0
	Echo Found VS 2017 Community
	GOTO Build 
 )
IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\MSBuild.exe" (
	Set SCPMSBuild="%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\MSBuild.exe"
	Set SCPVSVer=15.0
	Echo Found VS 2017 Professional
	GOTO Build 
 )
IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe" (
	Set SCPMSBuild="%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe"
	Set SCPVSVer=15.0
	Echo Found VS 2017 Enterprise
	GOTO Build 
 )
IF EXIST "%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" (
	Set SCPMSBuild="%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe"
	Set SCPVSVer=14.0
	Echo Found VS 2015
	GOTO Build 
 )

:Build
%SCPMSBuild%  build.xml /target:Build /p:BuildConfiguration=Debug /p:Version="%SolidCPVersion%" /p:FileVersion="%SolidCPFileVersion%" /p:VersionLabel="%SolidCPFileVersion%" /v:n %MsBuildSwitches% /fileLogger /flp:verbosity=normal /p:VisualStudioVersion=%SCPVSVer%