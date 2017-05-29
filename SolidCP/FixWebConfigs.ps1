#
# tool to fix Development WebConfigs after merge
# you MUST set $devroot to the base of your source tree... script will tell you if you've not set it right
#
$devroot = "C:\Rich"
#
if (-not (Get-Item "$devroot\SolidCP\Sources\SolidCP.WebPortal\Web.config") ) {
    $wshell = New-Object -ComObject Wscript.Shell
    $wshell.Popup("devroot is invalid",0,"Done",0x1) 
    exit
}


#Base (working) Install
[xml]$SolidPortal = Get-Content -Path "C:\SolidCP\Portal\Web.config"
[xml]$SolidEnterprise = Get-Content -Path "C:\SolidCP\Enterprise Server\Web.config"
[xml]$SolidServer = Get-Content -Path "C:\SolidCP\Server\Web.config"

#Dev (post merge) files
[xml]$devPortal = Get-Content -Path "$devroot\SolidCP\Sources\SolidCP.WebPortal\Web.config"
[xml]$devEnterprise = Get-Content -Path "$devroot\SolidCP\Sources\SolidCP.EnterpriseServer\Web.config"
[xml]$devServer = Get-Content -Path "$devroot\SolidCP\Sources\SolidCP.Server\Web.config"

# Portal Web Config - session validation key
$SolidPortal.configuration.appSettings.add | ForEach { if ( $_.key -eq "SessionValidationKey" ) {$hold = $_.value } }
$devPortal.configuration.appSettings.add | ForEach { if ( $_.key -eq "SessionValidationKey" ) { $_.value = $hold } }

# Enterprise WebConfig - connection string and crypto key
$devEnterprise.configuration.connectionStrings.add.connectionString = $SolidEnterprise.configuration.connectionStrings.add.connectionString
$SolidEnterprise.configuration.appSettings.add | ForEach { if ( $_.key -eq "SolidCP.CryptoKey" ) {$hold = $_.value } }
$devEnterprise.configuration.appSettings.add | ForEach { if ( $_.key -eq "SolidCP.CryptoKey" ) { $_.value = $hold } }

# Server WebConfig - server password
$devServer.configuration.'SolidCP.server'.security.password.value = $SolidServer.configuration.'SolidCP.server'.security.password.value

# backup existing files w/ timestamp just in case of overwrite, so they can be restored if something really goes wrong
$t = get-date -format "yyyyMMddhhmss"
Rename-Item "$devroot\SolidCP\Sources\SolidCP.WebPortal\Web.config"        "$devroot\SolidCP\Sources\SolidCP.WebPortal\Web.config-Git-$t"
Rename-Item "$devroot\SolidCP\Sources\SolidCP.EnterpriseServer\Web.config" "$devroot\SolidCP\Sources\SolidCP.EnterpriseServer\Web.config-Git-$t"
Rename-Item "$devroot\SolidCP\Sources\SolidCP.Server\Web.config"           "$devroot\SolidCP\Sources\SolidCP.Server\Web.config-Git-$t"

# write out new configs
$devPortal.save("$devroot\SolidCP\Sources\SolidCP.WebPortal\Web.config")
$devEnterprise.save("$devroot\SolidCP\Sources\SolidCP.EnterpriseServer\Web.config")
$devServer.Save("$devroot\SolidCP\Sources\SolidCP.Server\Web.config")