@echo off
RMDIR /S /Q "Bin"
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S bin') DO RMDIR /S /Q "%%G"
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S obj') DO RMDIR /S /Q "%%G"
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S bin_dotnet') DO RMDIR /S /Q "%%G"


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
%SCPMSBuild% build.xml /target:Deploy /p:BuildConfiguration=Release /p:Version="1.4.5" /p:FileVersion="1.4.9" /p:VersionLabel="1.4.9" /v:n /fileLogger /flp:verbosity=normal /p:VisualStudioVersion=%SCPVSVer%