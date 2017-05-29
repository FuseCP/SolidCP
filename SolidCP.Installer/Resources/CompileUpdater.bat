@ECHO OFF
set basedir=
IF %1.==. GOTO NoArg
set basedir=%1
:NoArg
set sourcedir=%basedir%SolidCP.Updater\bin
set targetdir=%basedir%SolidCP.Installer

"%basedir%ILMerge.exe" "%sourcedir%\SolidCP.Updater.exe" "%sourcedir%\..\Lib\Ionic.Zip.Reduced.dll" /out:%targetdir%\Updater.exe
del %targetdir%\Updater.pdb 