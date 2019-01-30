@echo off
set YYYYMMDD=%DATE:~6,4%%DATE:~3,2%%DATE:~0,2%
set Logfile=Log.txt
set logpath="C:\Program Files\dns\Logs\"
IF EXIST %logpath%%YYYYMMDD%_Log.txt set logfile=%YYYYMMDD%_Log.txt
ECHO . >> %logpath%%Logfile%
ECHO ######################### >> %logpath%%Logfile%
ECHO [%DATE% %TIME%] >> %logpath%%Logfile%
ECHO ######################### >> %logpath%%Logfile%

REM check if the first parameter is "reconfig" or "reload"
IF "%~1"=="reconfig" (
	CALL :RECONFIG
	GOTO :END
)
IF "%~1"=="reload" (
	CALL :RELOAD %1 %2
	GOTO :END
)
CALL :RECONFIG
CALL :RELOAD
GOTO :END

:RECONFIG
REM ##########################################
REM at reconfig, we need to regenerate the local .forward/.reverse (to catch the new zone), transfer it to the primaries/secondaries
REM and then issue a rdnc reconfig there (so that bind picks up these changes)
REM ##########################################
ECHO Doing RECONFIG
ECHO Generate primary configs
ECHO Generate primary configs >> %logpath%%Logfile%
powershell.exe -file "C:\Program Files\dns\etc\gen_named.forward.ps1" >> %logpath%%Logfile% 2>&1
powershell.exe -file "C:\Program Files\dns\etc\gen_named.reverse.ps1" >> %logpath%%Logfile% 2>&1
ECHO Transfer primary configs
ECHO Transfer primary configs >> %logpath%%Logfile%
"C:\Program Files (x86)\Putty\PSCP.EXE" -i "C:\Program Files\dns\myprivatekey.ppk" "C:\Program Files\dns\etc\named.conf.forward_out.txt" named@primary:/etc/bind/named.conf.forward >> %logpath%%Logfile% 2>&1
"C:\Program Files (x86)\Putty\PSCP.EXE" -i "C:\Program Files\dns\myprivatekey.ppk" "C:\Program Files\dns\etc\named.conf.forward.signed_out.txt" named@primary:/etc/bind/named.conf.forward.signed >> %logpath%%Logfile% 2>&1
"C:\Program Files (x86)\Putty\PSCP.EXE" -i "C:\Program Files\dns\myprivatekey.ppk" "C:\Program Files\dns\etc\named.conf.reverse_out.txt" named@primary:/etc/bind/named.conf.reverse >> %logpath%%Logfile% 2>&1

ECHO Generate secondary configs
ECHO Generate secondary configs >> %logpath%%Logfile%
powershell.exe -file "C:\Program Files\dns\etc\secondary_gen_named.forward.ps1" >> %logpath%%Logfile% 2>&1
powershell.exe -file "C:\Program Files\dns\etc\secondary_gen_named.reverse.ps1" >> %logpath%%Logfile% 2>&1
ECHO Transfer secondary configs
ECHO Transfer secondary configs >> %logpath%%Logfile%
"C:\Program Files (x86)\Putty\PSCP.EXE" -i "C:\Program Files\dns\myprivatekey.ppk" "C:\Program Files\dns\etc\secondary_named.conf.forward_out.txt" named@secondary1:/etc/bind/named.conf.forward  >> %logpath%%Logfile% 2>&1
"C:\Program Files (x86)\Putty\PSCP.EXE" -i "C:\Program Files\dns\myprivatekey.ppk" "C:\Program Files\dns\etc\secondary_named.conf.forward.signed_out.txt" named@secondary1:/etc/bind/named.conf.forward.signed  >> %logpath%%Logfile% 2>&1
"C:\Program Files (x86)\Putty\PSCP.EXE" -i "C:\Program Files\dns\myprivatekey.ppk" "C:\Program Files\dns\etc\secondary_named.conf.reverse_out.txt" named@secondary1:/etc/bind/named.conf.reverse >> %logpath%%Logfile% 2>&1
"C:\Program Files (x86)\Putty\PSCP.EXE" -i "C:\Program Files\dns\myprivatekey.ppk" "C:\Program Files\dns\etc\secondary_named.conf.forward_out.txt" named@secondary2:/etc/bind/named.conf.forward  >> %logpath%%Logfile% 2>&1
"C:\Program Files (x86)\Putty\PSCP.EXE" -i "C:\Program Files\dns\myprivatekey.ppk" "C:\Program Files\dns\etc\secondary_named.conf.forward.signed_out.txt" named@secondary2:/etc/bind/named.conf.forward.signed  >> %logpath%%Logfile% 2>&1
"C:\Program Files (x86)\Putty\PSCP.EXE" -i "C:\Program Files\dns\myprivatekey.ppk" "C:\Program Files\dns\etc\secondary_named.conf.reverse_out.txt" named@secondary2:/etc/bind/named.conf.reverse >> %logpath%%Logfile% 2>&1

REM first reconfig local server, then remote servers (only picks up new/deleted zones)
ECHO Issue rndc reconfig locally
ECHO Issue rndc reconfig locally >> %logpath%%Logfile%
"C:\Program Files\dns\bin\rndc.exe" reconfig >> %logpath%%Logfile% 2>&1
ECHO Issue rndc reconfig remote
ECHO Issue rndc reconfig remote >> %logpath%%Logfile%
"C:\Program Files (x86)\Putty\plink.EXE" -i "C:\Program Files\dns\myprivatekey.ppk" named@primary "rndc reconfig" >> %logpath%%Logfile% 2>&1
"C:\Program Files (x86)\Putty\plink.EXE" -i "C:\Program Files\dns\myprivatekey.ppk" named@secondary1 "rndc reconfig" >> %logpath%%Logfile% 2>&1
"C:\Program Files (x86)\Putty\plink.EXE" -i "C:\Program Files\dns\myprivatekey.ppk" named@secondary2 "rndc reconfig" >> %logpath%%Logfile% 2>&1
ECHO Done with RECONFIG
GOTO :EOF

:RELOAD
REM ##########################################
REM at reload, we need to sign the updated zone (if it is a signed one - handled by signzone itself)
REM and then issue a "rndc reload [zone]" (to signal reloading all/only the changed zone)
REM ##########################################
ECHO Doing RELOAD %2
ECHO Resign %2
ECHO Resign %2 >> %logpath%%Logfile%
powershell.exe -file "C:\Program Files\dns\zones\signzone.ps1" %2 >> %logpath%%Logfile% 2>&1
REM first reload local server, then remote servers (picks up changes in all zones/in zone %2)
REM in theory, the reload of the remote servers should not be necessary - they should get signaled about the updated zone with the new serial
REM - this also works in reality :-)
ECHO Issue rndc reload %2 locally
ECHO Issue rndc reload %2 locally >> %logpath%%Logfile%
"C:\Program Files\dns\bin\rndc.exe" reload %2 >> %logpath%%Logfile% 2>&1
ECHO Done with RELOAD %2
GOTO :EOF

:END
REN %logpath%%Logfile% %YYYYMMDD%_Log.txt 2>&1
