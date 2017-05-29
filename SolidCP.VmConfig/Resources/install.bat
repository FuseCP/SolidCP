@echo off
%systemroot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe SolidCP.VmConfig.exe
del SolidCP.VmConfig.InstallLog
del SolidCP.VmConfig.InstallState
del InstallUtil.InstallLog