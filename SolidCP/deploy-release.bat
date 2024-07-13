@echo off
RMDIR /S /Q "Bin"
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S bin') DO RMDIR /S /Q "%%G"
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S obj') DO RMDIR /S /Q "%%G"
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S bin_dotnet') DO RMDIR /S /Q "%%G"

IF not defined MsBuildSwitches ( Set MsBuildSwitches=/m)
IF not defined SolidCPVersion ( Set SolidCPVersion=1.4.5)
IF not defined SolidCPFileVersion ( Set SolidCPFileVersion=1.4.9)
IF not defined Configuration ( Set Configuration=Release)
 
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
echo "MsBuildSwitches: %MsBuildSwitches%"
%SCPMSBuild% build.xml /target:Deploy /p:BuildConfiguration=%Configuration% /p:Version="%SolidCPVersion%" /p:FileVersion="%SolidCPFileVersion%" /p:VersionLabel="%SolidCPFileVersion%" /v:n %MsBuildSwitches% /fileLogger /flp:verbosity=normal /p:VisualStudioVersion=%SCPVSVer%