<####################################################################################################
SolidSCP - Upgrade Menu

v1.0    14th July 2016:      First release of the SolidCP Upgrade Script
v1.1    2nd  August 2016:    Added dynamic Database & Folder detection to enable upgrade on older WSP or DNP installations
v1.2    30th August 2016:    Added dynamic Database Server detection to work with external Database Servers
v1.3    2nd  September 2016: Added web.config file updates to the script so the new features are added
v1.4    4th  September 2016: Added SQLPS detection for users who have the Database on a different machine to the Enterprise Server
v1.5    5th  September 2016: Added version update to the "SolidCP.Installer.exe.config" file so the manual installer shows the corect version if the application is opened
v1.6    6th  September 2016: Additional improvements to the backup of the database and the update of the web.config file for the Portal to ensure they are done in the correct order
v1.7    28th September 2016: Resolved various issues with the SQL Backup, also improved the component backups to save space and added in additional options to the menu for finer granularity when it comes to upgrading the components.
v1.8    16th January   2017: Improved the component backups to save time and to remove old files that are no longer in use by SolidCP. Added timer to show run time of this script
v1.9	27th May 2017:		 Removal of LE Files from the project when the update is ran
V2.0	17th May 2018		 Added support for CRM2016 and the asp.net server folders

Written By Marc Banyard for the SolidCP Project (c) 2016 SolidCP
Updated By Trevor Robinson.

The script needs to be run from the server that holds your Enterprise Server
as the script will query the database to get the servers that form part of your
SolidCP setup and upgrade each one in turn.

Copyright (c) 2016, SolidCP
SolidCP is distributed under the Creative Commons Share-alike license

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

- Redistributions of source code must  retain  the  above copyright notice, this
  list of conditions and the following disclaimer.

- Redistributions in binary form  must  reproduce the  above  copyright  notice,
  this list of conditions  and  the  following  disclaimer in  the documentation
  and/or other materials provided with the distribution.

- Neither the name of  SolidCP  nor the names of its contributors may be used to
  endorse or  promote  products  derived  from  this  software  without specific
  prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

All Code provided as is and used at your own risk.
####################################################################################################>
#Requires -RunAsAdministrator
# Set the window size as Server 2016 comes up small
$host.UI.RawUI.BufferSize  = New-Object -TypeName System.Management.Automation.Host.Size -ArgumentList (120, 50)
$host.UI.RawUI.WindowSize  = New-Object -TypeName System.Management.Automation.Host.Size -ArgumentList (120, 50)
$Host.UI.RawUI.WindowTitle = "$([Environment]::UserName): --  SolidCP - Auto Upgrade Script  --"
Write-Host "
        ****************************************
        *                                      *
        *        Welcome to the SolidCP        *
        *          Automated Upgrader          *
        *                                      *
        *       Please be patient whilst       *
        *          the menu is loaded          *
        *                                      *
        ****************************************" -ForegroundColor Green
####################################################################################################
####################################################################################################

# Editable features are below this line

$SCP_Portal_Svr_IP = "" # IP Address of the Portal component if not running on the Enterprise Server

# Editable features are above this line

####################################################################################################
#
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
#                                                                                                  #
# DO NOT EDIT ANYTHING BELOW THIS LINE                                                             #
#                                                                                                  #
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
# General settings - do not modify them or anyting else below this line                            #
$SCP_Installer_Site = "http://installer.solidcp.com" # SolidCP Installer Site
Add-Type -assembly "system.io.compression.filesystem"
Import-Module WebAdministration
if ([bool](Get-ChildItem IIS:\Sites -ErrorAction SilentlyContinue | Where-Object {(($_.Name -match "SolidCP|WebsitePanel|DotNetPanel") -and ($_.Name -match "Portal|Enterprise Server| Server"))})) {
	$SCP_EntSvr_Dir     = ((Get-ChildItem IIS:\Sites -ErrorAction SilentlyContinue | Where-Object {$_.Name -match "SolidCP Enterprise Server|WebsitePanel Enterprise Server|DotNetPanel Enterprise Server"} -ErrorAction SilentlyContinue).physicalPath) # SolidCP Enterprise Server Files Location
	$SCP_EntSvr_WebName = ((Get-ChildItem IIS:\Sites -ErrorAction SilentlyContinue | Where-Object {$_.Name -match "SolidCP Enterprise Server|WebsitePanel Enterprise Server|DotNetPanel Enterprise Server"} -ErrorAction SilentlyContinue).name)         # SolidCP Enterprise Server IIS Website Name
	$SCP_Portal_Dir     = ((Get-ChildItem IIS:\Sites -ErrorAction SilentlyContinue | Where-Object {$_.Name -match "SolidCP Portal|WebsitePanel Portal|DotNetPanel Portal"} -ErrorAction SilentlyContinue).physicalPath)                                  # SolidCP Portal Files Location
	$SCP_Portal_WebName = ((Get-ChildItem IIS:\Sites -ErrorAction SilentlyContinue | Where-Object {$_.Name -match "SolidCP Portal|WebsitePanel Portal|DotNetPanel Portal"} -ErrorAction SilentlyContinue).name)                                          # SolidCP Portal IIS Website Name
	$SCP_EntSvr_WebCfg  = ([xml](Get-Content "$SCP_EntSvr_Dir\Web.config"))
	$SCP_EntSvr_ConStr  = ($SCP_EntSvr_WebCfg.configuration.connectionStrings.add.connectionString)
	$SCP_EntSvr_CryptoK = ($SCP_EntSvr_WebCfg.SelectSingleNode("//configuration/appSettings/add[@key='SolidCP.CryptoKey']").value)
	$SCP_Database_Name  = ( $SCP_EntSvr_ConStr | Select-String 'Database=(?<ic>[^;]+?);' | ForEach-Object  {$_.Matches} | ForEach-Object {$_.Groups["ic"].Value} ) # Get the SolidCP Database Name from the Enterprise Server Connection String in the web.config file
	$SCP_Database_Servr = ( $SCP_EntSvr_ConStr | Select-String 'server=(?<ic>[^;]+?);' | ForEach-Object  {$_.Matches} | ForEach-Object {$_.Groups["ic"].Value} ) # Get the SolidCP Database Server from the Enterprise Server Connection String in the web.config file
}
$SCP_Backup_Time    = [System.DateTime]::Now.ToString("yyyy-MM-dd - (HH.mm tt)")
$dIPV4              = ((Test-Connection $env:computername -count 1).IPv4address.IPAddressToString)
####################################################################################################
# Additional items used in the below functions, they are stored as variables for easy use later    #
####################################################################################################
$dDomainMember     = ((gwmi win32_computersystem).partofdomain -eq $true)                   # Check if machine is joined to a domain
$dFQDNthisMachine  = ([System.Net.Dns]::GetHostByName(($env:computerName)).HostName)        # Get the FQDN of this machine
$dDomainName       = $env:USERDNSDOMAIN  # Store the Domain Name (if joined) as a variable to use later
$dComputerName     = $env:computername   # Store the Computer Name as a variable to use later
$dLoggedInUserName = $env:USERNAME       # Store the Logged On User Name as a variable to use later
$dLocalAdministratorSID  = (Get-WmiObject -Query "SELECT SID FROM Win32_UserAccount WHERE LocalAccount = 'True'" | Select-Object -First 1 -ExpandProperty SID); # Local Administrator SID
$dDomainAdministratorSID = (Get-WmiObject -Query "SELECT SID FROM Win32_UserAccount WHERE LocalAccount = 'False'" | Select-Object -First 1 -ExpandProperty SID); # Domain Administrator SID
$dMachineSID             = ((Get-WmiObject -Query "SELECT SID FROM Win32_UserAccount WHERE LocalAccount = 'True'" | Select-Object -First 1 -ExpandProperty SID) -replace ".{4}$"); # Local Machine SID
$dDomainSID              = ((Get-WmiObject -Query "SELECT SID FROM Win32_UserAccount WHERE LocalAccount = 'False'" | Select-Object -First 1 -ExpandProperty SID) -replace ".{4}$"); # Domain SID
$dLoggedInLocally  = ( ((Get-WMIObject -class Win32_ComputerSystem | select username).username) -eq ("$env:COMPUTERNAME\$env:USERNAME") ) # Check if logged in locally
$dLangAdministratorGroup = (([wmi]"Win32_SID.SID='S-1-5-32-544'").AccountName);            # Administrators
if ($dDomainMember) { # Only do the following if the server is a member of a domain
	$dLangDomainAdminsGroup       = (([wmi]"Win32_SID.SID='$dDomainSID-512'").AccountName);          # Domain Admins
	$dLangDomainAdministratorName = (([wmi]"Win32_SID.SID='$dDomainAdministratorSID'").AccountName); # Administrator
	$dLangDomainEnterpriseAdmins  = (([wmi]"Win32_SID.SID='$dDomainSID-519'").AccountName);          # Enterprise Admins
}
$dExcludedIPaddressesFile  = "SolidCP-Auto-Upgrade-Exclude-Servers.txt" # File name to contain a list of IP Addresses to exclude from the SolidCP Upgrade
$dIncludedIPaddressesFile  = "SolidCP-Auto-Upgrade-Include-Servers.txt" # File name to contain a list of additional IP Addresses to include the SolidCP Upgrade
####################################################################################################
####################################################################################################
####################################################################################################################################################################################
Function SCPupgradeMenu() # Ask the user if they want to use the Stable Release or if they want to use the BETA release
{
	DNPversionCheck
	SCPcheckIfEnterpriseServer
	dSQLPScheckInstalled
	GetSCPserverIPaddresses

	$choice = ""
	while ($choice -notmatch "[1|2|3|9|x]") {
		$SCP_Stable_Version      = ((([xml](New-Object System.Net.WebClient).DownloadString("$SCP_Installer_Site/Data/ProductReleasesFeed.xml")).SelectNodes("//release").version) | measure -Maximum).Maximum               # SolidCP Current Stable Version
		$SCP_BETA_Version        = ((([xml](New-Object System.Net.WebClient).DownloadString("$SCP_Installer_Site/Data/ProductReleasesFeed.Beta.xml")).SelectNodes("//release").version) | measure -Maximum).Maximum          # SolidCP Current BETA Version
		$SCP_Prev_Stable_Version = ((([xml](New-Object System.Net.WebClient).DownloadString("$SCP_Installer_Site/Data/ProductReleasesFeed.xml")).SelectNodes("//release").version) | select -Unique | sort -Descending)["1"] # SolidCP Previous Stable Version

		cls
		Write-Host "`n`tSolidCP Upgrade Menu`n" -ForegroundColor Magenta
		Write-Host "`t`tPlease select version of SolidCP you would like to upgrade your deployment to`n" -ForegroundColor Cyan
		Write-Host "`t`t 1. SolidCP v$SCP_Stable_Version - `"Stable`"" -ForegroundColor Cyan
		Write-Host "`t`t 2. SolidCP v$SCP_BETA_Version -  `"BETA`"" -ForegroundColor Cyan
		Write-Host "`t`t 3. SolidCP Test Remote Servers UNC Path (Firewall Test)" -ForegroundColor Cyan
		Write-Host "`n`t`t 9. SolidCP v$SCP_Prev_Stable_Version - `"Previous Stable Version`"" -ForegroundColor Cyan
		Write-Host "`n`t`t X. Exit this menu" -ForegroundColor Cyan
		$choice = Read-Host "`n`tEnter Option From Above Menu"
	}
	if ($choice -eq "1") {
		Write-Host "`n`tPreparing to Upgrade your SolidCP servers to the latest Stable release (v$SCP_Stable_Version)" -ForegroundColor Green
		$script:SCP_Version = "$SCP_Stable_Version"
		UpgradeSCPChoseComponent
	}
	elseif ($choice -eq "2") {
		Write-Host "`n`tPreparing to Upgrade your SolidCP servers to the latest BETA release (v$SCP_BETA_Version)" -ForegroundColor Green
		$script:SCP_Version = "$SCP_BETA_Version"
		UpgradeSCPChoseComponent
	}
	elseif ($choice -eq "3") {
		Write-Host "`n`tTest SolidCP Remote servers UNC Path" -ForegroundColor Cyan
		UpgradeSCPcheckUNCpath -IPs $SCP_ServerIPs
		dPressAnyKeyToContinue
		SCPupgradeMenu
	}
	elseif ($choice -eq "9") {
		Write-Host "`n`tPreparing to Upgrade your SolidCP servers to the previous Stable release (v$SCP_Prev_Stable_Version)" -ForegroundColor Green
		$script:SCP_Version = "$SCP_Prev_Stable_Version"
		UpgradeSCPChoseComponent
	}
	elseif ($choice -eq "x") {
		exit
	}
	dPressAnyKeyToExit
}


####################################################################################################################################################################################
function UpgradeSCPChoseComponent() # Function to download the files from the SolidCP Installer site for the SolidCP upgrade
{
	GetSCPserverIPaddresses
	do {
		do {
		cls
		Write-Host "`n`tSolidCP Upgrade Menu`n" -ForegroundColor Magenta
		Write-Host "`t`tPlease select SolidCP Components you would like to upgrade`n" -ForegroundColor Cyan
	$menu = @"
	    A. All components on All SolidCP Servers
	    B. SolidCP Enterprise Server Component
	    C. SolidCP Portal Component
	    D. SolidCP Server Component (This Server ONLY)
	    E. SolidCP Server Component (ALL Servers)
	    F. SolidCP Cloud Storage Portal Component (This Server ONLY)
	    G. SolidCP Cloud Storage Portal Component (ALL Servers)

	    X. Exit back to Main Menu
"@
	$menuOptions = '^[a-gx]+$'

			Write-Host $menu -ForegroundColor Cyan
			$choice1 = Read-Host "`n`tEnter Option From Above Menu"
			$ok = $choice1 -match $menuOptions

			if ( -not $ok) { Write-Host "Invalid selection" ; start-Sleep -Seconds 2 }
		} until ( $ok )

		switch -Regex ( $choice1 ) {
			"A" {
					UpgradeSCPDownloadFiles
					UpgradeSCPentSvr
					UpgradeSCPPortal
					UpgradeSCPserver -IPs $SCP_ServerIPs
					UpgradeSCPwebDav -IPs $SCP_ServerIPs
					UpgradeSCPPortalPost
					dPressAnyKeyToExit
				}

			"B" {
					UpgradeSCPDownloadFiles
					UpgradeSCPentSvr
					dPressAnyKeyToExit
				}

			"C" {
					UpgradeSCPDownloadFiles
					UpgradeSCPPortal
					UpgradeSCPPortalPost
					dPressAnyKeyToExit
				}

			"D" {
					UpgradeSCPDownloadFiles
					UpgradeSCPserver -IPs "127.0.0.1"
					dPressAnyKeyToExit
				}

			"E" {
					UpgradeSCPDownloadFiles
					UpgradeSCPserver -IPs $SCP_ServerIPs
					dPressAnyKeyToExit
				}

			"F" {
					UpgradeSCPDownloadFiles
					UpgradeSCPwebDav -IPs "127.0.0.1"
					dPressAnyKeyToExit
				}

			"G" {
					UpgradeSCPDownloadFiles
					UpgradeSCPwebDav -IPs $SCP_ServerIPs
					dPressAnyKeyToExit
				}
		}
	} until ( $choice1 -match "X" )
}


####################################################################################################################################################################################
function GetSCPserverIPaddresses()  # Function to get a list of the SolidCP Server IP Addresses to be upgraded
{
	if (Test-Path ".\$dExcludedIPaddressesFile") {
		$dSCPexcludeServerList = Get-Content ".\$dExcludedIPaddressesFile"  # Array with IP Addresses to Exclude from the upgrade
	}
	if (Test-Path ".\$dIncludedIPaddressesFile") {
		$dSCPincludeServerList = Get-Content ".\$dIncludedIPaddressesFile"  # Array with IP Addresses to Include with the upgrade
	}
	# Get the list of IP Addresses from the SolidCP Database and store tham as an array, also add the additional IP Addresses and remove the ones to be excluded
	push-location ; ($script:SCP_ServerIPs = ((Invoke-SQLCmd -query "SELECT [ServerUrl] FROM [$SCP_Database_Name].[dbo].[Servers] WHERE [VirtualServer]='0'" -Server $SCP_Database_Servr).ServerUrl -replace "^[^_]*\/\/|:.*|\/.*", "" ) + $dSCPincludeServerList | WHERE {$_} | Select -Unique | WHERE {$dSCPexcludeServerList -notcontains $_}) | Out-Null ; Pop-Location
}


####################################################################################################################################################################################
function UpgradeSCPcheckUNCpath()   # Function to test the UNC Path to each server is accessable
{
	Param(
		[String[]]$IPs        # Specify the Server IPs that are to be checked
	)
	foreach ($RemoteServer in $IPs) { # Loop through each server in the $IPs Array
		if (Test-Path "\\$RemoteServer\c$") {
			Write-Host "`t $([System.Net.Dns]::gethostentry("$RemoteServer").HostName.split('.')[0]) `($RemoteServer`) - UNC Test successful" -ForegroundColor Green
		}else{
			Write-Host "`t Unable to connect to $RemoteServer - Check Firewall Settings" -ForegroundColor Yellow
			# Add the IP Address to the excluded IP Addresses
			Write-Host "`t $RemoteServer has been added to the `"$dExcludedIPaddressesFile`" file" -ForegroundColor Yellow
			$RemoteServer | Out-File -FilePath "$dExcludedIPaddressesFile" -Append -Encoding ascii
		}
	}
}


####################################################################################################################################################################################
function UpgradeSCPDownloadFiles() # Function to download the files from the SolidCP Installer site for the SolidCP upgrade
{
	if ($SCP_Version) {
		$script:SCP_UpdateDir      = "C:\Program Files (x86)\SolidCP Installer\Manual Updates\$SCP_Backup_Time - (Before v$SCP_Version)"
		# SolidCP - Download files and prepare the upgrade
		if (!(Test-Path "$SCP_UpdateDir\Updates")) {
			################################################################################
			# SolidCP - Get the Values to update the "SolidCP.Installer.exe.config" later
			$script:SCP_XML_EntSvr = ([xml](New-Object System.Net.WebClient).DownloadString("$SCP_Installer_Site/Data/ProductReleasesFeed.xml")).SelectNodes("//component[@name='Enterprise Server']/releases/release[@available='true'][@version='$SCP_Version']")
			$script:SCP_XML_Portal = ([xml](New-Object System.Net.WebClient).DownloadString("$SCP_Installer_Site/Data/ProductReleasesFeed.xml")).SelectNodes("//component[@name='Portal']/releases/release[@available='true'][@version='$SCP_Version']")
			$script:SCP_XML_Server = ([xml](New-Object System.Net.WebClient).DownloadString("$SCP_Installer_Site/Data/ProductReleasesFeed.xml")).SelectNodes("//component[@name='Server']/releases/release[@available='true'][@version='$SCP_Version']")
			$script:SCP_XML_WebDav = ([xml](New-Object System.Net.WebClient).DownloadString("$SCP_Installer_Site/Data/ProductReleasesFeed.xml")).SelectNodes("//component[@name='Cloud Storage Portal']/releases/release[@available='true'][@version='$SCP_Version']")
			# Create the directory to download the updates to
			(New-Item -ItemType Directory -Path "$SCP_UpdateDir\Updates" -Force) | Out-Null
			# Check if user wants to download the files from the installer site - Mainly for Developers what want to build the source on thier machine and update the servers from that
			$choiceDownload = ""
			while ($choiceDownload -notmatch "[y|n]") { $choiceDownload = read-host "`n`tWould you like to download the update files from the SolidCP website`"`? (Y/N)" }
			if ($choiceDownload -eq "y") {
				# Start a timer to see how long the script takes to upgrade all of the servers
				$script:StopWatch = [System.Diagnostics.Stopwatch]::StartNew()
				# Download the Manual-Update.zip file from the SolidCP Installer website
				Write-Host "`t Downloading the Files ready for updating" -ForegroundColor Green
				Invoke-WebRequest -Uri ("$SCP_Installer_Site/Files/$SCP_Version/Manual-Update.zip") -OutFile "$SCP_UpdateDir\Updates\Manual-Update-$SCP_Version.zip" -PassThru -UseBasicParsing  | out-null
			}else{
				# Developers can manually place the installation files that they have built in the directory specified on screen
				Write-Host "`tPlace the `"Manual-Update.zip`" file in the following directory" -ForegroundColor Yellow
				Write-Host "$SCP_UpdateDir\" -ForegroundColor Yellow
				do {Start-Sleep -Milliseconds 500} until (Test-Path "$SCP_UpdateDir\Manual-Update.zip")
				# Start a timer to see how long the script takes to upgrade all of the servers once the file has been copied to the server
				$script:StopWatch = [System.Diagnostics.Stopwatch]::StartNew()
				# Move the file to the corect location
				(Move-Item "$SCP_UpdateDir\Manual-Update.zip" "$SCP_UpdateDir\Updates\Manual-Update-$SCP_Version.zip") | Out-Null
			}
			# Unzip the files
			Write-Host "`t Extracting the Files ready for updating" -ForegroundColor Green
			[io.compression.zipfile]::ExtractToDirectory("$SCP_UpdateDir\Updates\Manual-Update-$SCP_Version.zip", "$SCP_UpdateDir\Updates") | Out-Null
			# Update the SQL Update File with the SolidCP Database Name
			(Get-Content "$SCP_UpdateDir\Updates\update_db.sql").replace('${install.database}', "$SCP_Database_Name") | Set-Content "$SCP_UpdateDir\Updates\update_db.sql"
			# Remove the downloaded ZIP File to save space
			(Remove-Item "$SCP_UpdateDir\Updates\Manual-Update-$SCP_Version.zip" -Force) | Out-Null
		}
	}
}


####################################################################################################################################################################################
function UpgradeSCPentSvr() # Function to upgrade the SolidCP Enterprise Server Component
{
	if (Test-Path "$SCP_EntSvr_Dir") { # Upgrade the Enterprise Server
		if (!(IsFolderEmpty -Path "$SCP_UpdateDir\Updates\EnterpriseServer\")) { # Check if the Enterprise Server Update Folder has any files in it before upgrading
			if (([bool](Get-WebSite -Name "$SCP_EntSvr_WebName" -ErrorAction SilentlyContinue)) -and ($SCP_EntSvr_WebName -ne $null)) { # Check if the Enterprise Server website exists
				# Start the Enterprise Server upgrade
				Write-Host "`n`tStarting the `"SolidCP Enterprise Server`" upgrade" -ForegroundColor Cyan

				# Stop the Enterprise Server Website
				Write-Host "`t Stopping the `"$SCP_EntSvr_WebName`" website" -ForegroundColor Green
				(Stop-WebSite "$SCP_EntSvr_WebName") | Out-Null

				# Restart IIS on this server to ensure none of the files are locked - ensures clean upgrade
				Write-Host "`t Restarting IIS on this server for a clean upgrade" -ForegroundColor Green
				(Restart-Service 'W3SVC' -WarningAction SilentlyContinue -Force) | Out-Null

				# Stop the Enterprise Server Scheduler service
				Write-Host "`t Stopping the `"$SCP_EntSvr_WebName`" Scheduler service" -ForegroundColor Green
				if ( ((Get-Service -Name "SolidCP Scheduler"      -ErrorAction SilentlyContinue).Status) -eq "Running" ) {$SchedularServiceName = "SolidCP";      (Stop-Service "SolidCP Scheduler"      -Force -WarningAction SilentlyContinue)}
				if ( ((Get-Service -Name "WebsitePanel Scheduler" -ErrorAction SilentlyContinue).Status) -eq "Running" ) {$SchedularServiceName = "WebsitePanel"; (Stop-Service "WebsitePanel Scheduler" -Force -WarningAction SilentlyContinue)}
				if ( ((Get-Service -Name "DotNetPanel Scheduler"  -ErrorAction SilentlyContinue).Status) -eq "Running" ) {$SchedularServiceName = "DotNetPanel";  (Stop-Service "DotNetPanel Scheduler"  -Force -WarningAction SilentlyContinue)}

				# Backup the Enterprise Server files
				Write-Host "`t Creating a backup of the `"Enterprise Server`" files" -ForegroundColor Green
				if (!(Test-Path "$SCP_UpdateDir\Enterprise Server - Backup")) {(New-Item -ItemType Directory -Path "$SCP_UpdateDir\Enterprise Server - Backup" -Force) | Out-Null}
				[System.IO.Compression.ZipFile]::CreateFromDirectory($SCP_EntSvr_Dir, "$SCP_UpdateDir\Enterprise Server - Backup\Files.zip")
				#Copy-Item -Path "$SCP_EntSvr_Dir\*" -Destination "$SCP_UpdateDir\Enterprise Server - Backup" -Recurse -ErrorAction SilentlyContinue | Out-Null

				# Backup the Enterprise Server Database
				if ($SCP_Database_Servr -and $SCP_Database_Name -and $SCP_UpdateDir -and $SCP_Backup_Time) {
					Write-Host "`t Creating a backup of the `"Enterprise Server`" Database" -ForegroundColor Green
					# Create the SQL Database backup directory if it doesn't exist
					if (!(Test-Path "$SCP_UpdateDir\Enterprise Server - Database")) {
						(New-Item -ItemType Directory -Path "$SCP_UpdateDir\Enterprise Server - Database" -Force) | Out-Null
					}
					# Set the permissions on the SQL Database backup directory for full access
					$acl = Get-Acl -Path "$SCP_UpdateDir\Enterprise Server - Database"
					$acl.SetAccessRule($(New-Object -TypeName System.Security.AccessControl.FileSystemAccessRule -ArgumentList 'Everyone', 'FullControl', 'ContainerInherit, ObjectInherit', 'None', 'Allow'))
					$acl | Set-Acl -Path "$SCP_UpdateDir\Enterprise Server - Database"
					# Create temporary Share for SQL Backup
					New-SMBShare -Name "SCPUpgrade$" -Path "$SCP_UpdateDir\Enterprise Server - Database" -FullAccess "Everyone" -Temporary | Out-Null
					# Backup the SQL Database
					push-location
					# Import the SQL PowerShell Module
					Import-Module SQLPS -DisableNameChecking
					if (([System.Net.Dns]::gethostentry("$($SCP_Database_Servr.Split("\")[0])").HostName) -match $env:USERDNSDOMAIN) { # Check if local or domain
						Backup-SqlDatabase -ServerInstance "$SCP_Database_Servr" -Database "$SCP_Database_Name" -BackupFile "\\$dIPV4\SCPUpgrade$\$SCP_Database_Name - $SCP_Backup_Time.bak"
					}else{ # If the SQL Server is not local or on the same domain then prompt for the SQL Admin users credentials
						Backup-SqlDatabase -ServerInstance "$SCP_Database_Servr" -Database "$SCP_Database_Name" -BackupFile "\\$dIPV4\SCPUpgrade$\$SCP_Database_Name - $SCP_Backup_Time.bak" -Credential (Get-Credential "sa")
					}
					Pop-Location
					do {Start-Sleep -Milliseconds 500} until (Test-Path "$SCP_UpdateDir\Enterprise Server - Database\$SCP_Database_Name - $SCP_Backup_Time.bak")
					# Zip the backup to save space
					[System.IO.Compression.ZipFile]::CreateFromDirectory("$SCP_UpdateDir\Enterprise Server - Database", "$SCP_UpdateDir\Enterprise Server - Backup\Database.zip")
					# Remove the temporary Share for SQL Backup
					(Remove-SmbShare -Name "SCPUpgrade$" -Force) | Out-Null
					# Remove the SQL Database backup directory as it is no longer required
					(Remove-Item "$SCP_UpdateDir\Enterprise Server - Database" -Recurse -Force -confirm:$false) | Out-Null
					Write-Host "`t The `"Enterprise Server`" Database has been backed up successfully" -ForegroundColor Green
				}

				# Remove old Enterprise Server Files that are no longer in use or will be replaced by the upgraded files
				Write-Host "`t Preparing the existing `"Enterprise Server`" files for upgrading" -ForegroundColor Green
				if (Test-Path "$SCP_UpdateDir\Updates\EnterpriseServer\setup\delete.txt") {
					foreach ($SCP_File_Tidy in (Get-Content "$SCP_UpdateDir\Updates\EnterpriseServer\setup\delete.txt")) {
						if (Test-Path "$SCP_EntSvr_Dir\$SCP_File_Tidy") {
							if ((Get-Item "$SCP_EntSvr_Dir\$SCP_File_Tidy") -is [System.IO.DirectoryInfo]) { # check if item is a directory and delete files within it
								(Remove-Item -Path "$SCP_EntSvr_Dir\$SCP_File_Tidy\*" -Force -Recurse -Confirm:$false -ErrorAction SilentlyContinue) | Out-Null
							}else{ # otherwise delete the specified file
								(Remove-Item -Path "$SCP_EntSvr_Dir\$SCP_File_Tidy" -Force -Confirm:$false -ErrorAction SilentlyContinue) | Out-Null
							}
						}
					}
				}

				# Upgrade the Enterprise Server files
				Write-Host "`t Upgrading the `"Enterprise Server`" files" -ForegroundColor Green
				Copy-Item -Path "$SCP_UpdateDir\Updates\EnterpriseServer\*" -Exclude "delete.txt" -Destination "$SCP_EntSvr_Dir\" -Recurse -Force | Out-Null
				(Remove-Item -Path "$SCP_EntSvr_Dir\setup\update_db.sql" -Force -Confirm:$false -ErrorAction SilentlyContinue) | Out-Null

				# Upgrade the Enterprise Server databaseM
				if (Test-Path "$SCP_UpdateDir\Updates\update_db.sql") {
					Write-Host "`t Upgrading the `"Enterprise Server`" database" -ForegroundColor Green
					push-location ; Invoke-Sqlcmd -InputFile "$SCP_UpdateDir\Updates\update_db.sql" -ServerInstance "$SCP_Database_Servr" -Database "$SCP_Database_Name" | Out-Null ; Pop-Location
				}

				# Update the web.config file from Website Panel to SolidCP
				ModifyXML "$SCP_EntSvr_Dir\web.config" "Update" "//configuration/appSettings/add[@key='WebsitePanel.CryptoKey']/@key" "SolidCP.CryptoKey"
				ModifyXML "$SCP_EntSvr_Dir\web.config" "Update" "//configuration/appSettings/add[@key='WebsitePanel.EncryptionEnabled']/@key" "SolidCP.EncryptionEnabled"
				ModifyXML "$SCP_EntSvr_Dir\web.config" "Update" "//configuration/appSettings/add[@key='WebsitePanel.EnterpriseServer.WebApplicationsPath']/@key" "SolidCP.EnterpriseServer.WebApplicationsPath"
				ModifyXML "$SCP_EntSvr_Dir\web.config" "Update" "//configuration/appSettings/add[@key='WebsitePanel.EnterpriseServer.ServerRequestTimeout']/@key" "SolidCP.EnterpriseServer.ServerRequestTimeout"
				ModifyXML "$SCP_EntSvr_Dir\web.config" "Add" "//configuration/appSettings" "add" @( ("key","SolidCP.AltConnectionString"), ("value","ConnectionString") )
				ModifyXML "$SCP_EntSvr_Dir\web.config" "Add" "//configuration/appSettings" "add" @( ("key","SolidCP.AltCryptoKey"), ("value","CryptoKey") )
				ModifyXML "$SCP_EntSvr_Dir\web.config" "Update" "//configuration/microsoft.web.services3/security/securityTokenManager/add[@type='WebsitePanel.EnterpriseServer.ServiceUsernameTokenManager, WebsitePanel.EnterpriseServer']/@type" "SolidCP.EnterpriseServer.ServiceUsernameTokenManager, SolidCP.EnterpriseServer.Code"
				#ModifyXML "$SCP_EntSvr_Dir\bin\SolidCP.EnterpriseServer.dll.config" "Update" "//configuration/connectionStrings/add[@name='EnterpriseServer']/@connectionString" "$SCP_EntSvr_ConStr"
				#ModifyXML "$SCP_EntSvr_Dir\bin\SolidCP.EnterpriseServer.dll.config" "Update" "//configuration/appSettings/add[@key='SolidCP.CryptoKey']/@value" "$SCP_EntSvr_CryptoK"
				ModifyXML "$SCP_EntSvr_Dir\bin\SolidCP.SchedulerService.exe.config" "Update" "//configuration/connectionStrings/add[@name='EnterpriseServer']/@connectionString" "$SCP_EntSvr_ConStr"
				ModifyXML "$SCP_EntSvr_Dir\bin\SolidCP.SchedulerService.exe.config" "Update" "//configuration/appSettings/add[@key='SolidCP.CryptoKey']/@value" "$SCP_EntSvr_CryptoK"
				Write-Host "`t The `"SolidCP Enterprise Server`" web.config file has been updated" -ForegroundColor Green

				# Start the Enterprise Server Scheduler service
				if ($SchedularServiceName) {
					Write-Host "`t Starting the `"$SchedularServiceName Enterprise Server`" Scheduler service" -ForegroundColor Green
					(Start-Service "$SchedularServiceName Scheduler" -WarningAction SilentlyContinue) | Out-Null
				}

				# Start the Enterprise Server Website
				Write-Host "`t Starting the `"$SCP_EntSvr_WebName`" website" -ForegroundColor Green
				(Start-WebSite "$SCP_EntSvr_WebName") | Out-Null

				# Wake the Enterprise Server so it is more responsive after the upgrade
				try {(Invoke-WebRequest "http://$([System.Net.Dns]::gethostentry("$dIPV4").HostName):9002" -DisableKeepAlive -UseBasicParsing -Method head) | Out-Null} catch {}
				try {(Invoke-WebRequest "http://127.0.0.1:9002" -DisableKeepAlive -UseBasicParsing -Method head) | Out-Null} catch {}

				# Upgrade complete
				Write-Host "`t  The `"SolidCP Enterprise Server`" has been upgraded" -ForegroundColor Green
			}else{
				Write-Host "`tThe `"SolidCP Enterprise Server`" website was not found on this server" -ForegroundColor Yellow
			}
		}else{
			Write-Host "`t There are no `"SolidCP Enterprise Server`" updates required on this server" -ForegroundColor Yellow
		}
	}else{
		Write-Host "`tThe `"SolidCP Enterprise Server`" was not found on this server" -ForegroundColor Yellow
	}
}


####################################################################################################################################################################################
function UpgradeSCPPortal() # Function to upgrade the SolidCP Portal Component
{
	if (Test-Path "$SCP_Portal_Dir") { # Upgrade the Portal if the path exists
		if (!(IsFolderEmpty -Path "$SCP_UpdateDir\Updates\Portal\")) { # Check if the Portal Update Folder has any files in it before upgrading
			if (([bool](Get-WebSite -Name "$SCP_Portal_WebName" -ErrorAction SilentlyContinue)) -and ($SCP_Portal_WebName -ne $null)) { # Check if the SolidCP Portal website exists
				# Start the Portal upgrade
				Write-Host "`n`tStarting the `"SolidCP Portal`" upgrade" -ForegroundColor Cyan

				# Stop the Portal Website
				Write-Host "`t Stopping the `"$SCP_Portal_WebName`" website" -ForegroundColor Green
				(Stop-WebSite "$SCP_Portal_WebName") | Out-Null

				# Backup the Portal files
				Write-Host "`t Creating a backup of the `"Portal`" files" -ForegroundColor Green
				if (!(Test-Path "$SCP_UpdateDir\Portal - Backup")) {(New-Item -ItemType Directory -Path "$SCP_UpdateDir\Portal - Backup" -Force) | Out-Null}
				[System.IO.Compression.ZipFile]::CreateFromDirectory($SCP_Portal_Dir, "$SCP_UpdateDir\Portal - Backup\Files.zip")
				#Copy-Item -Path "$SCP_Portal_Dir\*" -Destination "$SCP_UpdateDir\Portal - Backup" -Recurse -ErrorAction SilentlyContinue | Out-Null

				# Remove old Portal Files that are no longer in use or will be replaced by the upgraded files
				Write-Host "`t Preparing the existing `"Portal`" files for upgrading" -ForegroundColor Green
				if (Test-Path "$SCP_UpdateDir\Updates\Portal\setup\delete.txt") {
					foreach ($SCP_File_Tidy in (Get-Content "$SCP_UpdateDir\Updates\Portal\setup\delete.txt")) {
						if (Test-Path "$SCP_Portal_Dir\$SCP_File_Tidy") {
							if ((Get-Item "$SCP_Portal_Dir\$SCP_File_Tidy") -is [System.IO.DirectoryInfo]) { # check if item is a directory and delete files within it
								(Remove-Item -Path "$SCP_Portal_Dir\$SCP_File_Tidy\*" -Force -Recurse -Confirm:$false -ErrorAction SilentlyContinue) | Out-Null
							}else{ # otherwise delete the specified file
								(Remove-Item -Path "$SCP_Portal_Dir\$SCP_File_Tidy" -Force -Confirm:$false -ErrorAction SilentlyContinue) | Out-Null
							}
						}
					}
				}

				# Upgrade the Portal files
				Write-Host "`t Upgrading the `"Portal`" files" -ForegroundColor Green
				Copy-Item -Path "$SCP_UpdateDir\Updates\Portal\*" -Exclude "delete.txt" -Destination "$SCP_Portal_Dir\" -Recurse -Force | Out-Null

				# Update the web.config to change the "xmlns" to "xmlns-temp" otherwise we have issues when parsing the XML file
				(Get-Content "$SCP_Portal_Dir\web.config") -replace " xmlns=`"", " xmlns-temp=`"" | Set-Content "$SCP_Portal_Dir\web.config"
				# Update the web.config file from Website Panel to SolidCP
				ModifyXML "$SCP_Portal_Dir\web.config" "Update" "//configuration/appSettings/add[@key='WebPortal.ThemeProvider'][@value='WebsitePanel.Portal.WebPortalThemeProvider, WebsitePanel.Portal.Modules']/@value" "SolidCP.Portal.WebPortalThemeProvider, SolidCP.Portal.Modules"
				ModifyXML "$SCP_Portal_Dir\web.config" "Update" "//configuration/appSettings/add[@key='WebPortal.PageTitleProvider'][@value='WebsitePanel.Portal.WebPortalPageTitleProvider, WebsitePanel.Portal.Modules']/@value" "SolidCP.Portal.WebPortalPageTitleProvider, SolidCP.Portal.Modules"
				ModifyXML "$SCP_Portal_Dir\web.config" "Update" "//configuration/system.web/siteMap[@defaultProvider='WebsitePanelSiteMapProvider'][@enabled='true']/@defaultProvider" "SolidCPSiteMapProvider"
				ModifyXML "$SCP_Portal_Dir\web.config" "Update" "//configuration/system.web/siteMap/providers/add[@name='WebsitePanelSiteMapProvider'][@type='WebsitePanel.WebPortal.WebsitePanelSiteMapProvider, WebsitePanel.WebPortal'][@securityTrimmingEnabled='true']/@name" "SolidCPSiteMapProvider"
				ModifyXML "$SCP_Portal_Dir\web.config" "Update" "//configuration/system.web/siteMap/providers/add[@name='SolidCPSiteMapProvider'][@type='WebsitePanel.WebPortal.WebsitePanelSiteMapProvider, WebsitePanel.WebPortal'][@securityTrimmingEnabled='true']/@type" "SolidCP.WebPortal.SolidCPSiteMapProvider, SolidCP.WebPortal"
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//system.web/siteMap/providers" "remove" @("name","MySqlSiteMapProvider")
				if ( !(CheckXMLnode "$SCP_Portal_Dir\web.config" "//configuration/system.web/httpHandlers/add[@verb='*'][@path='AjaxHandler.ashx'][@type='SolidCP.WebPortal.SolidCPAjaxHandler, SolidCP.WebPortal']" "type") ) {
					ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/system.web/httpHandlers" "add" @( ("verb", ".*"), ("path", "AjaxHandler.ashx"), ("type", "SolidCP.WebPortal.SolidCPAjaxHandler, SolidCP.WebPortal") )
					ModifyXML "$SCP_Portal_Dir\web.config" "Update" "//configuration/system.web/httpHandlers/add[@path='AjaxHandler.ashx'][@type='SolidCP.WebPortal.SolidCPAjaxHandler, SolidCP.WebPortal']/@verb" "*"
				}
				ModifyXML "$SCP_Portal_Dir\web.config" "Update" "//configuration/system.web/authentication[@mode='Forms']/forms[@name='.WEBSITEPANELPORTALAUTHASPX'][@protection='All'][@timeout='30'][@path='/'][@requireSSL='false'][@slidingExpiration='true'][@cookieless='UseDeviceProfile'][@domain=''][@enableCrossAppRedirects='false']/@name" ".SolidCPPORTALAUTHASPX"
				ModifyXML "$SCP_Portal_Dir\web.config" "Update" "//configuration/system.web/compilation[@debug='true'][@targetFramework='4.0']/@debug" "false"
				ModifyXML "$SCP_Portal_Dir\web.config" "Delete" "//configuration/system.webServer/modules"
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/system.webServer" 'staticContent'
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/system.webServer/staticContent" "remove" @("fileExtension",".woff")
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/system.webServer/staticContent" "remove" @("fileExtension",".woff2")
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/system.webServer/staticContent" "mimeMap" @( ("fileExtension",".woff"), ("mimeType","application/x-font-woff") )
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/system.webServer/staticContent" "mimeMap" @( ("fileExtension",".woff2"), ("mimeType","application/font-woff2") )
				# Update the web.config file to make sure it is up to date with the new Mailcleaner (Ignore SSL Check) Settings
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration" 'system.net'
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/system.net" "settings"
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/system.net/settings" "servicePointManager" @( ("checkCertificateName","false"), ("checkCertificateRevocationList","false") )
				# Update the web.config file to make sure it is up to date with the new Settings for v1.1.0 of SolidCP
				if (!(CheckXMLnode "$SCP_Portal_Dir\Web.config" "//configuration" "configSections")) {
					(Get-Content "$SCP_Portal_Dir\web.config") -replace "<configuration>", "<configuration>`n  <configSections>`n  </configSections>" | Set-Content "$SCP_Portal_Dir\web.config"
				}
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/configSections" "sectionGroup" @("name","jsEngineSwitcher")
				(Get-Content "$SCP_Portal_Dir\web.config") -replace "    <sectionGroup name=`"jsEngineSwitcher`" />", "    <sectionGroup name=`"jsEngineSwitcher`">`n    </sectionGroup>" | Set-Content "$SCP_Portal_Dir\web.config"
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/configSections/sectionGroup[@name='jsEngineSwitcher']" "section" @( ("name","core"), ("type","JavaScriptEngineSwitcher.Core.Configuration.CoreConfiguration, JavaScriptEngineSwitcher.Core") )
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/configSections/sectionGroup[@name='jsEngineSwitcher']" "section" @( ("name","msie"), ("type","JavaScriptEngineSwitcher.Msie.Configuration.MsieConfiguration, JavaScriptEngineSwitcher.Msie") )
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/configSections" "sectionGroup" @("name","bundleTransformer")
				(Get-Content "$SCP_Portal_Dir\web.config") -replace "    <sectionGroup name=`"bundleTransformer`" />", "    <sectionGroup name=`"bundleTransformer`">`n    </sectionGroup>" | Set-Content "$SCP_Portal_Dir\web.config"
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/configSections/sectionGroup[@name='bundleTransformer']" "section" @( ("name","core"), ("type","BundleTransformer.Core.Configuration.CoreSettings, BundleTransformer.Core") )
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/configSections/sectionGroup[@name='bundleTransformer']" "section" @( ("name","less"), ("type","BundleTransformer.Less.Configuration.LessSettings, BundleTransformer.Less") )
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/system.web/pages[@theme='Default'][@validateRequest='false'][@controlRenderingCompatibilityVersion='3.5'][@clientIDMode='AutoID']/controls" "add" @( ("tagPrefix","CPCC"), ("namespace","CPCC"), ("assembly","CPCC") )
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/system.webServer/handlers" "add" @( ("name","LessAssetHandler"), ("path","`*.less"), ("verb","GET"), ("type","BundleTransformer.Less.HttpHandlers.LessAssetHandler, BundleTransformer.Less"), ("resourceType","File"), ("preCondition","") )
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration" "jsEngineSwitcher" @("xmlns-temp","http`:`/`/tempuri.org`/JavaScriptEngineSwitcher.Configuration.xsd")
				(Get-Content "$SCP_Portal_Dir\web.config") -replace "  <jsEngineSwitcher xmlns-temp=`"http://tempuri.org/JavaScriptEngineSwitcher.Configuration.xsd`" />", "  <jsEngineSwitcher xmlns-temp=`"http://tempuri.org/JavaScriptEngineSwitcher.Configuration.xsd`">`n  </jsEngineSwitcher>" | Set-Content "$SCP_Portal_Dir\web.config"
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/jsEngineSwitcher[@xmlns-temp='http://tempuri.org/JavaScriptEngineSwitcher.Configuration.xsd']" "core"
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/jsEngineSwitcher[@xmlns-temp='http://tempuri.org/JavaScriptEngineSwitcher.Configuration.xsd']/core" "engines"
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/jsEngineSwitcher[@xmlns-temp='http://tempuri.org/JavaScriptEngineSwitcher.Configuration.xsd']/core/engines" "add" @( ("name","MsieJsEngine"), ("type","JavaScriptEngineSwitcher.Msie.MsieJsEngine, JavaScriptEngineSwitcher.Msie") )
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration" "bundleTransformer" @("xmlns-temp","http://tempuri.org/BundleTransformer.Configuration.xsd")
				(Get-Content "$SCP_Portal_Dir\web.config") -replace "  <bundleTransformer xmlns-temp=`"http://tempuri.org/BundleTransformer.Configuration.xsd`" />", "  <bundleTransformer xmlns-temp=`"http://tempuri.org/BundleTransformer.Configuration.xsd`">`n  </bundleTransformer>" | Set-Content "$SCP_Portal_Dir\web.config"
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']" "core"
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core" "css"
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/css" "translators"
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/css/translators" "add" @( ("name","NullTranslator"), ("type","BundleTransformer.Core.Translators.NullTranslator, BundleTransformer.Core"), ("enabled","false") )
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/css/translators" "add" @( ("name","LessTranslator"), ("type","BundleTransformer.Less.Translators.LessTranslator, BundleTransformer.Less") )
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/css" "postProcessors"
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/css/postProcessors" "add" @( ("name","UrlRewritingCssPostProcessor"), ("type","BundleTransformer.Core.PostProcessors.UrlRewritingCssPostProcessor, BundleTransformer.Core"), ("useInDebugMode","false") )
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/css" "minifiers"
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/css/minifiers" "add" @( ("name","NullMinifier"), ("type","BundleTransformer.Core.Minifiers.NullMinifier, BundleTransformer.Core") )
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/css" "fileExtensions"
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/css/fileExtensions" "add" @( ("fileExtension",".css"), ("assetTypeCode","Css") )
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/css/fileExtensions" "add" @( ("fileExtension",".less"), ("assetTypeCode","Less") )
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core" "js"
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/js" "translators"
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/js/translators" "add" @( ("name","NullTranslator"), ("type","BundleTransformer.Core.Translators.NullTranslator, BundleTransformer.Core"), ("enabled","false") )
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/js" "minifiers"
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/js/minifiers" "add" @( ("name","NullMinifier"), ("type","BundleTransformer.Core.Minifiers.NullMinifier, BundleTransformer.Core") )
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/js" "fileExtensions"
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/js/fileExtensions" "add" @( ("fileExtension",".js"), ("assetTypeCode","JavaScript") )
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']" "less" @( ("useNativeMinification","true"), ("ieCompat","true"), ("strictMath","false"), ("strictUnits","false"), ("dumpLineNumbers","None"), ("javascriptEnabled","true") )
				(Get-Content "$SCP_Portal_Dir\web.config") -replace "    <less useNativeMinification=`"true`" ieCompat=`"true`" strictMath=`"false`" strictUnits=`"false`" dumpLineNumbers=`"None`" javascriptEnabled=`"true`" />", "    <less useNativeMinification=`"true`" ieCompat=`"true`" strictMath=`"false`" strictUnits=`"false`" dumpLineNumbers=`"None`" javascriptEnabled=`"true`">`n    </less>" | Set-Content "$SCP_Portal_Dir\web.config"
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/less[@useNativeMinification='true'][@ieCompat='true'][@strictMath='false'][@strictUnits='false'][@dumpLineNumbers='None'][@javascriptEnabled='true']" "jsEngine" @("name","MsieJsEngine")
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration" "runtime"
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/runtime" "assemblyBinding" @("xmlns-temp","urn:schemas-microsoft-com:asm.v1")
				(Get-Content "$SCP_Portal_Dir\web.config") -replace "    <assemblyBinding xmlns-temp=`"urn:schemas-microsoft-com:asm.v1`" />", "    <assemblyBinding xmlns-temp=`"urn:schemas-microsoft-com:asm.v1`">`n    </assemblyBinding>" | Set-Content "$SCP_Portal_Dir\web.config"
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/runtime/assemblyBinding[@xmlns-temp='urn:schemas-microsoft-com:asm.v1']" "dependentAssembly"
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/runtime/assemblyBinding[@xmlns-temp='urn:schemas-microsoft-com:asm.v1']/dependentAssembly" "assemblyIdentity" @( ("name","Newtonsoft.Json"), ("publicKeyToken","30ad4fe6b2a6aeed"), ("culture","neutral") )
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/runtime/assemblyBinding[@xmlns-temp='urn:schemas-microsoft-com:asm.v1']/dependentAssembly" "bindingRedirect" @( ("oldVersion","0.0.0.0-9.0.0.0"), ("newVersion","9.0.0.0") )
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/runtime/assemblyBinding[@xmlns-temp='urn:schemas-microsoft-com:asm.v1']" "dependentAssembly"
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/runtime/assemblyBinding[@xmlns-temp='urn:schemas-microsoft-com:asm.v1']/dependentAssembly" "assemblyIdentity" @( ("name","WebGrease"), ("publicKeyToken","31bf3856ad364e35"), ("culture","neutral") )
				ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/runtime/assemblyBinding[@xmlns-temp='urn:schemas-microsoft-com:asm.v1']/dependentAssembly" "bindingRedirect" @( ("oldVersion","0.0.0.0-1.5.2.14234"), ("newVersion","1.5.2.14234") )
				# Add the edditional "<dependentAssembly>" tags in the Runtime section and remove any additional charichter returns from the end of the file
				((Get-Content "$SCP_Portal_Dir\web.config" -Raw) -replace '        <bindingRedirect oldVersion="0\.0\.0\.0-9\.0\.0\.0" newVersion="9\.0\.0\.0" \/>[\r\n]+        <assemblyIdentity name="WebGrease" publicKeyToken="31bf3856ad364e35" culture="neutral" \/>', "        <bindingRedirect oldVersion=`"0.0.0.0-9.0.0.0`" newVersion=`"9.0.0.0`" />`r`n      </dependentAssembly>`r`n      <dependentAssembly>`r`n        <assemblyIdentity name=`"WebGrease`" publicKeyToken=`"31bf3856ad364e35`" culture=`"neutral`" />" -replace '</configuration>[\r\n]+', "</configuration>") | Set-Content "$SCP_Portal_Dir\web.config"
				# Update the web.config to change the "xmlns-temp" back to "xmlns" now we have finished parsing the XML file
				(Get-Content "$SCP_Portal_Dir\web.config") -replace " xmlns-temp=`"", " xmlns=`"" | Set-Content "$SCP_Portal_Dir\web.config"
				Write-Host "`t The `"SolidCP Portal`" web.config file has been updated" -ForegroundColor Green

				# Delete the old css files from the themes styles directory
				if (Test-Path "$SCP_Portal_Dir\App_Themes\Default\Styles\bootstrap.min.css") {Remove-Item -Path "$SCP_Portal_Dir\App_Themes\Default\Styles\bootstrap.min.css" -Force}
				if (Test-Path "$SCP_Portal_Dir\App_Themes\Default\Styles\menus.css")         {Remove-Item -Path "$SCP_Portal_Dir\App_Themes\Default\Styles\menus.css" -Force}
				# Delete files which should not be in the project
				if (Test-Path "$SCP_Portal_Dir\DesktopModules\SolidCP\ExchangeServer\UserControls\MSO365\MSO365Address.ascx") {Remove-Item -Recurse -Path "$SCP_Portal_Dir\DesktopModules\SolidCP\ExchangeServer\UserControls\MSO365" -Force}
				if (Test-Path "$SCP_Portal_Dir\DesktopModules\SolidCP\ExchangeServer\UserControls\Locations\LocationAddress.ascx") {Remove-Item -Recurse -Path "$SCP_Portal_Dir\DesktopModules\SolidCP\ExchangeServer\UserControls\Locations\" -Force}
				if (Test-Path "$SCP_Portal_Dir\DesktopModules\SolidCP\ProviderControls\SpamExperts_Settings.ascx") {Remove-Item -Path "$SCP_Portal_Dir\DesktopModules\SolidCP\ProviderControls\SpamExperts_Settings.ascx" -Force}
				if (Test-Path "$SCP_Portal_Dir\DesktopModules\SolidCP\ProviderControls\App_LocalResources\SpamExperts_Settings.ascx.resx") {Remove-Item -Path "$SCP_Portal_Dir\DesktopModules\SolidCP\ProviderControls\App_LocalResources\SpamExperts_Settings.ascx.resx" -Force}
				if (Test-Path "$SCP_Portal_Dir\DesktopModules\SolidCP\ScheduleTaskControls\LetsEncryptRenewalView.ascx") {Remove-Item -Path "$SCP_Portal_Dir\DesktopModules\SolidCP\ScheduleTaskControls\LetsEncryptRenewalView.ascx" -Force}
				if (Test-Path "$SCP_Portal_Dir\DesktopModules\SolidCP\SettingsLetsEncryptRenewalAdminNotificationLetter.ascx") {Remove-Item -Path "$SCP_Portal_Dir\DesktopModules\SolidCP\SettingsLetsEncryptRenewalAdminNotificationLetter.ascx" -Force}
				if (Test-Path "$SCP_Portal_Dir\DesktopModules\SolidCP\App_LocalResources\SettingsLetsEncryptRenewalAdminNotificationLetter.ascx.resx") {Remove-Item -Path "$SCP_Portal_Dir\DesktopModules\SolidCP\App_LocalResources\SettingsLetsEncryptRenewalAdminNotificationLetter.ascx.resx" -Force}
				if (Test-Path "$SCP_Portal_Dir\DesktopModules\SolidCP\SettingsLetsEncryptRenewalNotificationLetter.ascx") {Remove-Item -Path "$SCP_Portal_Dir\DesktopModules\SolidCP\SettingsLetsEncryptRenewalNotificationLetter.ascx" -Force}
				if (Test-Path "$SCP_Portal_Dir\DesktopModules\SolidCP\App_LocalResources\SettingsLetsEncryptRenewalNotificationLetter.ascx.resx") {Remove-Item -Path "$SCP_Portal_Dir\DesktopModules\SolidCP\App_LocalResources\SettingsLetsEncryptRenewalNotificationLetter.ascx.resx" -Force}
				
				# Upgrade complete
				Write-Host "`t  The `"SolidCP Portal`" has been upgraded" -ForegroundColor Green
			}else{
				Write-Host "`tThe `"SolidCP Portal`" website was not found on this server" -ForegroundColor Yellow
			}
		}else{
			Write-Host "`t There are no `"SolidCP Server`" updates required on this server" -ForegroundColor Yellow
		}
	}else{
		Write-Host "`tThe `"SolidCP Portal`" was not found on this server" -ForegroundColor Yellow
	}
}


####################################################################################################################################################################################
function UpgradeSCPPortalPost() # Function to Start the SolidCP Portal website after the upgrade has been completed
{
	if (Test-Path "$SCP_Portal_Dir") { # Upgrade the Portal if the path exists
		if (!(IsFolderEmpty -Path "$SCP_UpdateDir\Updates\Portal\")) { # Check if the Portal Update Folder has any files in it before upgrading
			if ([bool](Get-WebSite "$SCP_Portal_WebName")) { # Check if the SolidCP Portal website exists
				# Start the Portal Website
				Write-Host "`n`t Starting the `"$SCP_Portal_WebName`" website" -ForegroundColor Green
				(Start-WebSite "$SCP_Portal_WebName") | Out-Null

				# Wake the SolidCP Portal so it is more responsive after the upgrade
				try {(Invoke-WebRequest "http://$([System.Net.Dns]::gethostentry("$dIPV4").HostName):80" -DisableKeepAlive -UseBasicParsing -Method head) | Out-Null} catch {}
				try {(Invoke-WebRequest "http://$([System.Net.Dns]::gethostentry("$dIPV4").HostName):9001" -DisableKeepAlive -UseBasicParsing -Method head) | Out-Null} catch {}
				try {(Invoke-WebRequest "https://$([System.Net.Dns]::gethostentry("$dIPV4").HostName):443" -DisableKeepAlive -UseBasicParsing -Method head) | Out-Null} catch {}

				# Upgrade complete
				Write-Host "`t  The `"SolidCP Portal`" has been started" -ForegroundColor Green
			}
		}
	}
}


####################################################################################################################################################################################
function UpgradeSCPserver() # Function to upgrade the SolidCP Server Component
{
	Param(
		[String[]]$IPs        # Specify the Server IPs that are to be upgraded
	)
	if ($IPs) { # Check to make sure there are servers in the $IPs Array
		if (!(IsFolderEmpty -Path "$SCP_UpdateDir\Updates\Server\")) { # Check if the Server Update Folder has any files in it before upgrading
			if (!(Test-Path "$SCP_UpdateDir\Server - Backups")) {(New-Item -ItemType Directory -Path "$SCP_UpdateDir\Server - Backups" -Force) | Out-Null}
			foreach ($SCP_RemoteServer in $IPs) { # Loop through each server in the $IPs Array
				if (Test-Path "\\$SCP_RemoteServer\c$") { # Check to make sure the servers UNC Default Share is accessable
					foreach ($RemoteServer in (Get-ChildItem (Get-ChildItem -Path "\\$SCP_RemoteServer\c$\" -Include ("WebsitePanel", "SolidCP", "DotNetPanel")).FullName -Directory)) {
						If ($RemoteServer.name -eq "Server" -Or $RemoteServer.name -eq "Server asp.net v4.5" -Or $RemoteServer.name -eq "Server asp.net v2.0") {
							$SCP_Server_Dir  = $RemoteServer.FullName
							$SCP_Server_FQDN = $([System.Net.Dns]::gethostentry("$SCP_RemoteServer").HostName)
							$SCP_Server_Name = $SCP_Server_FQDN.split('.')[0]

							# Start the Server upgrade
							Write-Host "`n`tStarting the `"SolidCP Server`" upgrade on `"$SCP_Server_FQDN`"" -ForegroundColor Cyan
							# Backup the Server files
							Write-Host "`t Creating a backup of the `"Server`" files" -ForegroundColor Green
							[System.IO.Compression.ZipFile]::CreateFromDirectory($SCP_Server_Dir, "$SCP_UpdateDir\Server - Backups\$SCP_Server_Name.zip")
							#if (!(Test-Path "$SCP_UpdateDir\Server - Backups\$SCP_Server_Name")) {(New-Item -ItemType Directory -Path "$SCP_UpdateDir\Server - Backups\$SCP_Server_Name" -Force) | Out-Null}
							#Copy-Item -Path "$SCP_Server_Dir\*" -Destination "$SCP_UpdateDir\Server - Backups\$SCP_Server_Name" -Recurse -ErrorAction SilentlyContinue | Out-Null

							# Remove old Server Files that are no longer in use or will be replaced by the upgraded files
							Write-Host "`t Preparing the existing `"Server`" files for upgrading" -ForegroundColor Green
							if (Test-Path "$SCP_UpdateDir\Updates\Server\setup\delete.txt") {
								foreach ($SCP_File_Tidy in (Get-Content "$SCP_UpdateDir\Updates\Server\setup\delete.txt")) {
									if (Test-Path "$SCP_Server_Dir\$SCP_File_Tidy") {
										if ((Get-Item "$SCP_Server_Dir\$SCP_File_Tidy") -is [System.IO.DirectoryInfo]) { # check if item is a directory and delete files within it
											(Remove-Item -Path "$SCP_Server_Dir\$SCP_File_Tidy\*" -Force -Recurse -Confirm:$false -ErrorAction SilentlyContinue) | Out-Null
										}else{ # otherwise delete the specified file
											(Remove-Item -Path "$SCP_Server_Dir\$SCP_File_Tidy" -Force -Confirm:$false -ErrorAction SilentlyContinue) | Out-Null
										}
									}
								}
							}

							# Remove some files which should have not been included
							if (Test-Path "$SCP_Server_Dir\EmailSecurity.asmx") {Remove-Item -Path "$SCP_Server_Dir\EmailSecurity.asmx" -Force}
							if (Test-Path "$SCP_Server_Dir\srvLetsEncrypt.asmx") {Remove-Item -Path "$SCP_Server_Dir\srvLetsEncrypt.asmx" -Force}
							if (Test-Path "$SCP_Server_Dir\bin\Filters\SolidCP.Providers.EmailSecurity.SpamExperts.dll") {Remove-Item -Path "$SCP_Server_Dir\bin\Filters\SolidCP.Providers.EmailSecurity.SpamExperts.dll" -Force}

							# Upgrade the Server files
							Write-Host "`t Upgrading the `"Server`" files" -ForegroundColor Green
							Copy-Item -Path "$SCP_UpdateDir\Updates\Server\*" -Exclude "delete.txt" -Destination "$SCP_Server_Dir\" -Recurse -Force | Out-Null

							# Update the web.config file from Website Panel to SolidCP
							((Get-Content "$SCP_Server_Dir\web.config").replace('WebsitePanel', 'SolidCP') | Set-Content "$SCP_Server_Dir\web.config")
							((Get-Content "$SCP_Server_Dir\web.config").replace('websitepanel', 'SolidCP') | Set-Content "$SCP_Server_Dir\web.config")
							ModifyXML "$SCP_Server_Dir\web.config" "Add" "//configuration/appSettings" "add" @( ("key", "SolidCP.Exchange.ClearQueryBaseDN"), ("value", "false") )
							ModifyXML "$SCP_Server_Dir\web.config" "Add" "//configuration/appSettings" "add" @( ("key", "SolidCP.Exchange.enableSP2abp"), ("value", "false") )
							ModifyXML "$SCP_Server_Dir\web.config" "Add" "//configuration/appSettings" "add" @( ("key", "SCVMMServerName"), ("value", "") )
							ModifyXML "$SCP_Server_Dir\web.config" "Add" "//configuration/appSettings" "add" @( ("key", "SCVMMServerPort"), ("value", "") )
							ModifyXML "$SCP_Server_Dir\web.config" "Add" "//configuration/system.web" "compilation" @( ("debug", "true"), ("targetFramework", "4.0") )
							ModifyXML "$SCP_Server_Dir\web.config" "Add" "//configuration/system.web" "pages" @( ("controlRenderingCompatibilityVersion", "3.5"), ("clientIDMode", "AutoID") )
							ModifyXML "$SCP_Server_Dir\web.config" "Add" "//configuration" "runtime"
							if ( !(CheckXMLnode "$SCP_Server_Dir\web.config" "//configuration/runtime" "assemblyBinding") ) {
								ModifyXML "$SCP_Server_Dir\web.config" "Add" "//configuration/runtime" "assemblyBinding" @("xmlns", "urn:schemas-microsoft-com:asm.v1")
								((Get-Content "$SCP_Server_Dir\web.config").replace('    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1" />', "    <assemblyBinding xmlns=`"urn:schemas-microsoft-com:asm.v1`">`n      <probing privatePath=`"bin/Crm2011;bin/Crm2013;bin/Exchange2013;bin/Exchange2016;bin/Sharepoint2013;bin/Sharepoint2016;bin/Lync2013;bin/SfB2015;bin/Lync2013HP;bin/Dns2012;bin/IceWarp;bin/IIs80;bin/IIs100;bin/HyperV2012R2;bin/HyperVvmm;bin/Crm2015;bin/Crm2016;bin/Filters`" />`n    </assemblyBinding>") | Set-Content "$dFilePath1")
							}
							# Update the web.config file to make sure it is up to date with the new Settings
							[xml]$SCP_Server_XML = Get-Content -Path "$SCP_Server_Dir\web.config"
							$SCP_Server_XML.configuration.runtime.assemblyBinding.probing.privatePath = "bin/Crm2011;bin/Crm2013;bin/Exchange2013;bin/Exchange2016;bin/Sharepoint2013;bin/Sharepoint2016;bin/Lync2013;bin/SfB2015;bin/Lync2013HP;bin/Dns2012;bin/IceWarp;bin/IIs80;bin/IIs100;bin/HyperV2012R2;bin/HyperVvmm;bin/Crm2015;bin/Crm2016;bin/Filters"
							$SCP_Server_XML.Save("$SCP_Server_Dir\web.config") | Out-Null
							Write-Host "`t The `"SolidCP Server`" web.config file has been updated" -ForegroundColor Green

							# Wake the SolidCP Server so it is more responsive after the upgrade
							try {(Invoke-WebRequest "http://$($SCP_Server_FQDN):9003" -DisableKeepAlive -UseBasicParsing -Method head) | Out-Null} catch {}

							# Upgrade complete
							Write-Host "`t  The `"SolidCP Server`" has been upgraded on `"$SCP_Server_FQDN`"" -ForegroundColor Green
						}
					}
				}else{
					Write-Host "`t Unable to connect to $SCP_RemoteServer - Check Firewall Settings" -ForegroundColor Yellow
					# Add the IP Address to the excluded IP Addresses
					Write-Host "`t $RemoteServer has been added to the `"$dExcludedIPaddressesFile`" file" -ForegroundColor Yellow
					$RemoteServer | Out-File -FilePath "$dExcludedIPaddressesFile" -Append -Encoding ascii
				}
			}
		}else{
			Write-Host "`t There are no `"SolidCP Server`" updates required on your servers" -ForegroundColor Yellow
		}
	}else{
		Write-Host "`t No `"SolidCP Servers`" are configured on your Portal" -ForegroundColor Yellow
	}
}


####################################################################################################################################################################################
function UpgradeSCPwebDav() # Function to upgrade the SolidCP Cloud Storage Portal (WebDAV) Component
{
	Param(
		[String[]]$IPs        # Specify the Cloud Storage Portal IPs that are to be upgraded
	)
	if ($IPs) { # Check to make sure there are IP Addresses in the $IPs Array
		if (!(IsFolderEmpty -Path "$SCP_UpdateDir\Updates\WebDavPortal\")) { # Check if the WebDAV Update Folder has any files in it before upgrading
			if (!(Test-Path "$SCP_UpdateDir\WebDAV - Backups")) {(New-Item -ItemType Directory -Path "$SCP_UpdateDir\WebDAV - Backups" -Force) | Out-Null}
			foreach ($SCP_RemoteWebDAV in $IPs) { # Loop through each IP Address in the $IPs Array
				if (Test-Path "\\$SCP_RemoteWebDAV\c$") { # Check to make sure the WebDAVs UNC Default Share is accessable
					foreach ($RemoteWebDAV in (Get-ChildItem (Get-ChildItem -Path "\\$SCP_RemoteWebDAV\c$\" -Include ("WebsitePanel", "SolidCP")).FullName -Directory)) {
						If ($RemoteWebDAV.name -eq "Cloud Storage Portal") {
							$SCP_WebDAV_Dir  = $RemoteWebDAV.FullName
							$SCP_WebDAV_FQDN = $([System.Net.Dns]::gethostentry("$SCP_RemoteWebDAV").HostName)
							$SCP_WebDAV_Name = $SCP_WebDAV_FQDN.split('.')[0]

							# Start the Cloud Storage Portal upgrade
							Write-Host "`n`tStarting the `"SolidCP Cloud Storage Portal`" upgrade on `"$SCP_WebDAV_FQDN`"" -ForegroundColor Cyan
							# Backup the Cloud Storage Portal files
							Write-Host "`t Creating a backup of the `"Cloud Storage Portal`" files" -ForegroundColor Green
							[System.IO.Compression.ZipFile]::CreateFromDirectory($SCP_WebDAV_Dir, "$SCP_UpdateDir\WebDAV - Backups\$SCP_WebDAV_Name.zip")

							# Remove old Cloud Storage Portal Files that are no longer in use or will be replaced by the upgraded files
							Write-Host "`t Preparing the existing `"Cloud Storage Portal`" files for upgrading" -ForegroundColor Green
							if (Test-Path "$SCP_UpdateDir\Updates\WebDavPortal\setup\delete.txt") {
								foreach ($SCP_File_Tidy in (Get-Content "$SCP_UpdateDir\Updates\WebDavPortal\setup\delete.txt")) {
									if (Test-Path "$SCP_WebDAV_Dir\$SCP_File_Tidy") {
										if ((Get-Item "$SCP_WebDAV_Dir\$SCP_File_Tidy") -is [System.IO.DirectoryInfo]) { # check if item is a directory and delete files within it
											(Remove-Item -Path "$SCP_WebDAV_Dir\$SCP_File_Tidy\*" -Force -Recurse -Confirm:$false -ErrorAction SilentlyContinue) | Out-Null
										}else{ # otherwise delete the specified file
											(Remove-Item -Path "$SCP_WebDAV_Dir\$SCP_File_Tidy" -Force -Confirm:$false -ErrorAction SilentlyContinue) | Out-Null
										}
									}
								}
							}

							# Upgrade the Cloud Storage Portal files
							Write-Host "`t Upgrading the `"SolidCP Cloud Storage Portal`" files" -ForegroundColor Green
							Copy-Item -Path "$SCP_UpdateDir\Updates\WebDavPortal\*" -Exclude "delete.txt" -Destination "$SCP_WebDAV_Dir\" -Recurse -Force | Out-Null

							# Wake the SolidCP Cloud Storage Portal so it is more responsive after the upgrade
							try {(Invoke-WebRequest "http://$($SCP_WebDAV_FQDN):9004" -DisableKeepAlive -UseBasicParsing -Method head) | Out-Null} catch {}

							# Upgrade complete
							Write-Host "`t  The `"SolidCP Cloud Storage Portal`" has been upgraded on `"$SCP_WebDAV_FQDN`"" -ForegroundColor Green
						}
					}
				}else{
					Write-Host "`t Unable to connect to $SCP_RemoteWebDAV - Check Firewall Settings" -ForegroundColor Yellow
					# Add the IP Address to the excluded IP Addresses
					Write-Host "`t $RemoteWebDAV has been added to the `"$dExcludedIPaddressesFile`" file" -ForegroundColor Yellow
					$RemoteWebDAV | Out-File -FilePath "$dExcludedIPaddressesFile" -Append -Encoding ascii
				}
			}
		}else{
			Write-Host "`n`tThere are no `"SolidCP Cloud Storage Portal`" updates required on your servers" -ForegroundColor Yellow
		}
	}else{
		Write-Host "`t No `"SolidCP Cloud Storage Portals`" are configured on your Portal" -ForegroundColor Yellow
	}
}


####################################################################################################################################################################################
function DNPversionCheck() # Check if DNP is installed, if so advise a manual update first before using the upgrade script
{
	if ((Get-ChildItem IIS:\Sites | Where-Object {$_.Name -like "* Enterprise Server*"}).name -like "DotNetPanel*") {
		# Check the Database to make sure it has been upgraded to SolidCP if DotNetPanel is detected
		push-location ; ($SCP_Database_Check = (Invoke-SQLCmd -query "SELECT SolidCPdatabase = ( COUNT([DatabaseVersion])) FROM [$SCP_Database_Name].[dbo].[Versions] WHERE BuildDate >= '2016' AND DatabaseVersion >= '1.0.1'" -Server $SCP_Database_Servr).SolidCPdatabase) | Out-Null ; Pop-Location
		if ($SCP_Database_Check -eq '0') { # Show a warning message to the user if DNP is detected and has not been upgraded to SCP
			Write-Host "`n`t *************************************************" -ForegroundColor Yellow
			Write-Host "`t *                                               *" -ForegroundColor Yellow
			Write-Host "`t *     You have a VERY old version installed     *" -ForegroundColor Yellow
			Write-Host "`t *                                               *" -ForegroundColor Yellow
			Write-Host "`t *     We reccomend manually upgrading before    *" -ForegroundColor Yellow
			Write-Host "`t *         attempting to run this script!        *" -ForegroundColor Yellow
			Write-Host "`t *                                               *" -ForegroundColor Yellow
			Write-Host "`t *           Please see www.solidcp.com          *" -ForegroundColor Yellow
			Write-Host "`t *              for more information             *" -ForegroundColor Yellow
			Write-Host "`t *                                               *" -ForegroundColor Yellow
			Write-Host "`t *************************************************" -ForegroundColor Yellow
			dPressAnyKeyToExit
		}
	}
}


####################################################################################################################################################################################
function SCPcheckIfEnterpriseServer() # Check if the script is being run on the Enterprise Server, if not then advise end user
{
	if (! ((Get-ChildItem IIS:\Sites | Where-Object {$_.Name -like "* Enterprise Server*"}).name -match "SolidCP|WebsitePanel|DotNetPanel") ) {
		Write-Host "`n`t *************************************************" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *     The Enterprise Server component is not    *" -ForegroundColor Yellow
		Write-Host "`t *           installed on this machine           *" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *     You need to run this script from your     *" -ForegroundColor Yellow
		Write-Host "`t *               Enterprise Server               *" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *************************************************" -ForegroundColor Yellow
		dPressAnyKeyToExit
	}
}


####################################################################################################################################################################################
Function IsFolderEmpty()
{
	Param(
		[String]$Path        # Specify the folder to test if it is empty
	)
	if ($Path) {
		if ((Get-ChildItem $Path | Measure-Object).Count -gt 0) {
			return $false
		}else{
			return $true
		}
	}
}


####################################################################################################################################################################################
function CheckXMLnode ($dFilePath, $dXMLNode, $dXMLname, $dXMLvalue) # Function to check if a node exists in an XML file with specific values
{ # Usage - CheckXMLnode $dFilePath "//SecurityClasses" "SecurityClass" "name-Custom"
  # Usage - CheckXMLnode $dFilePath "//SecurityClass" "Name" "Custom"
	[xml]$xml = Get-Content -Path "$dFilePath"
	if ([string]::IsNullOrEmpty($dXMLvalue)) {
		if ($xml.selectNodes("$dXMLNode").$dXMLname) { $true } else { $false }
	}else{
		if ($xml.selectNodes("$dXMLNode").$dXMLname -contains "$dXMLvalue") { $true } else { $false }
	}
}


####################################################################################################################################################################################
function ModifyXML([String] $dFilePath, $dAction, [String] $dNodePath, $dElement, $dValue) # Function to Add, Comment  or UnComment XML nodes in an XML File
{ # Usage - ModifyXML $dFilePath "Add" "//TestParent" "TestChild" "TestValue"
  # Usage - ModifyXML $dFilePath "Add" "//TestParent" "TestChild" @("Attribute","Value")
  # Usage - ModifyXML $dFilePath "Add" "//TestParent" "TestChild" @( ("Attribute1","Value 1"), ("Attribute2","Value 2"), ("Attribute3","Value 3"), ("Attribute4","Value 4"), ("Attribute5","Value 5") )
  # Usage - ModifyXML $dFilePath "Delete" "//TestParent/TestChild"
  # Usage - ModifyXML $dFilePath "Delete" "//TestChild[@TestAttribute]"
  # Usage - ModifyXML $dFilePath "Update" "//TestChild" "Updated Value"
  # Usage - ModifyXML $dFilePath "Update" "//TestChild[@TestAttribute='Original Value']/@TestAttribute" "New Value"
  # Usage - ModifyXML $dFilePath "Get" "//TestParent/TestChild"
  # Usage - ModifyXML $dFilePath "Get" "//TestChild[@TestAttribute]/@TestAttribute"
  # Usage - ModifyXML $dFilePath "Comment" "//TestParent/TestChild"
  # Usage - ModifyXML $dFilePath "Comment" "//TestChild[@TestAttribute='Value']"
  # Usage - ModifyXML $dFilePath "UnComment" "/TestChild"
  # Usage - ModifyXML $dFilePath "UnComment" 'TestAttribute="Value"'
	[xml]$xml = Get-Content -Path "$dFilePath"
	if ($dAction -eq "Add") {
		$Child = $xml.CreateElement("$dElement")
		if ($dValue -is [System.Array]) {
			if ($dValue[0] -is [System.Array]) { # The Attribute are in a Multi Dimensional Array
				$dCheckNode = "$dNodePath/$dElement"
				if ( !(CheckXMLnode "$dFilePath" "$dCheckNode" $dValue[0][0] $dValue[0][1]) ) {
					foreach($value in $dValue) {
						$Child.SetAttribute($value[0],$value[1])
						$xml.selectNodes("$dNodePath").AppendChild($Child) | Out-Null
					}
				}
			}else{ # The Attributes are in a Single Level Array
				$dCheckNode = "$dNodePath/$dElement"
				if ( !(CheckXMLnode "$dFilePath" "$dCheckNode" $dValue[0] $dValue[1]) ) {
					$Child.SetAttribute($dValue[0],$dValue[1])
					$xml.selectNodes("$dNodePath").AppendChild($Child) | Out-Null
				}
			}
		}
		elseif ($dValue -isnot [System.Array]) { # Simple Element with a Value
			if ( !(CheckXMLnode "$dFilePath" "$dNodePath" "$dElement" "$dValue") ) {
				$Child.InnerText = "$dValue"
				$xml.selectNodes("$dNodePath").AppendChild($Child) | Out-Null
			}
		}
		$xml.Save("$dFilePath") | Out-Null
	}elseif ($dAction -eq "Update") {
		$dNode = $xml.SelectNodes($dNodePath)
		foreach ($node in $dNode) {
			if ($node -ne $null) {
				if ( $node.NodeType -eq "Element") { $node.InnerXml = $dElement }
				else { $node.Value = $dElement }
			}
		}
		$xml.Save("$dFilePath") | Out-Null
	}elseif ($dAction -eq "Get") {
		$dNode = $xml.SelectNodes($dNodePath)
		if ($dNode -ne $null) {
			if ( $dNode.NodeType -eq "Element") { $dNode.InnerXml }
			else { $dNode.Value }
		}
	}elseif ($dAction -eq "Delete") {
		if ( ($xml.selectNodes("$dNodePath")).ParentNode.Name ) {
			($xml | select-xml -xpath ("//" + ($xml.selectNodes("$dNodePath")).ParentNode.Name) | ForEach-Object {$_.node.removechild((select-xml -xpath $dNodePath  -xml $xml).node)}) | Out-Null
			$xml.Save("$dFilePath");
		}
	}elseif ($dAction -eq "Comment") {
		$xml.SelectNodes("$dNodePath") | ForEach-Object {
			$Comment = $xml.CreateComment($_.OuterXml);
			$_.ParentNode.ReplaceChild($Comment, $_) | Out-Null
		}
		$xml.Save("$dFilePath");
	}elseif ($dAction -eq "UnComment") {
		$xml.SelectNodes("//comment()") | ForEach-Object {     
			($_.InnerText | convertto-xml).SelectNodes("/descendant::*[contains(text(), '$dNodePath')]") | ForEach-Object { 
				$UnComment = $_;
				(Get-Content "$dFilePath") | ForEach-Object { $_.Replace("<!--" + $UnComment.InnerText + "-->", $UnComment.InnerText) } | Set-Content "$dFilePath"
			}
		}
	}
}


####################################################################################################################################################################################
Function CheckGroupMembers($dGroupName, $dUserName, $dGroupType)                # Check if a Local Users is a member of a Group (Local or Domain) - returns True or False
{ # Usage - CheckGroupMembers "Local Group Name" "Local User Name" "Local|Domain"
	$MemberNames = @()
	if ($dGroupType -eq "local") { # If Local group is specified
		if ([ADSI]::Exists("WinNT://$env:computername/$dGroupName")) { # Check if the Local group exists
			$dMembers = @( ([ADSI]"WinNT://$env:computername/$dGroupName").psbase.Invoke("Members") ) | ForEach-Object{$_.GetType().InvokeMember("Name", "GetProperty", $null, $_, $null);}
		}
	}elseif ( ($dGroupType -eq "domain") -and ($dDomainMember) ) { # If Domain group is specified
		if (([ADSISearcher]"(sAMAccountName=$dGroupName)").FindOne()) { # Check if the Domain group exists
			$dMembers = @( ([ADSI]"WinNT://$env:USERDNSDOMAIN/$dGroupName").psbase.Invoke("Members") ) | ForEach-Object{$_.GetType().InvokeMember("Name", "GetProperty", $null, $_, $null);}
		}
	}
	($dMembers -contains "$dUserName")
}


####################################################################################################################################################################################
Function dPressAnyKeyToExit()                               # Function to press any key to exit
{
	if ($psISE) { # Check if running Powershell ISE
		if ($StopWatch.IsRunning) {
			$script:StopWatch.Stop()
			Write-Host "`n`t It took $(($StopWatch.Elapsed.TotalSeconds).ToString("#.##")) Seconds to upgrade your SolidCP server" -ForegroundColor Green
		}
		Add-Type -AssemblyName System.Windows.Forms
		[System.Windows.Forms.MessageBox]::Show("Press any key to exit")
		exit
	}else{
		if ($StopWatch.IsRunning) {
			$script:StopWatch.Stop()
			Write-Host "`n`t It took $(($StopWatch.Elapsed.TotalSeconds).ToString("#.##")) Seconds to upgrade your SolidCP server" -ForegroundColor Green
		}
		Write-Host "`n`tPress any key to exit..." -ForegroundColor Yellow
		$x = $host.ui.RawUI.ReadKey("NoEcho,IncludeKeyDown")
		exit
	}
}


####################################################################################################################################################################################
Function dPressAnyKeyToContinue()                           # Function to press any key to exit
{
	if ($psISE) { # Check if running Powershell ISE
		Add-Type -AssemblyName System.Windows.Forms
		[System.Windows.Forms.MessageBox]::Show("Press any key to continue")
	}else{
		Write-Host "`n`tPress any key to continue..." -ForegroundColor Yellow
		$x = $host.ui.RawUI.ReadKey("NoEcho,IncludeKeyDown")
	}
}


####################################################################################################################################################################################
Function InstallSQLsvr2014mgmtTools()                   # Function to install SQL Server 2014 Express Management Tools
{
	if ( !(Test-Path "C:\Program Files (x86)\Microsoft SQL Server\120\Tools\Binn\ManagementStudio\Ssms.exe") ) { 
		Write-Host "`tDownloading SQL Server 2014 Express Management Tools" -ForegroundColor Cyan
		# Create the SQL Server 2014 Express with Tools Directory in our Installation Files folder ready for downloading
		(md -Path 'C:\_Install Files\SQL Server 2014 Express Management Tools' -Force) | Out-Null ; cd 'C:\_Install Files\SQL Server 2014 Express Management Tools\'
		# Download SQL Server 2014 Express Management Tools x64 from Microsoft
		(New-Object System.Net.WebClient).DownloadFile("https://download.microsoft.com/download/E/A/E/EAE6F7FC-767A-4038-A954-49B8B05D04EB/MgmtStudio%2064BIT/SQLManagementStudio_x64_ENU.exe", "C:\_Install Files\SQL Server 2014 Express Management Tools\SQLManagementStudio_x64_ENU.exe")

		# Install the SQL Server 2014 Express with Tools x64 on the Server
		Write-Host "`t Extracting and Installing SQL Server 2014 Express Management Tools`n" -ForegroundColor Green
		Write-Host "`t    ****************************************" -ForegroundColor Green
		Write-Host "`t    *                                      *" -ForegroundColor Green
		Write-Host "`t    *     This will take quite a while     *" -ForegroundColor Green
		Write-Host "`t    *           to fully complete          *" -ForegroundColor Green
		Write-Host "`t    *                                      *" -ForegroundColor Green
		Write-Host "`t    *  Please be patient while we install  *" -ForegroundColor Green
		Write-Host "`t    *       SQL Server 2014 Express        *" -ForegroundColor Green
		Write-Host "`t    *        Management Tools ONLY         *" -ForegroundColor Green
		Write-Host "`t    *                                      *" -ForegroundColor Green
		Write-Host "`t    ****************************************" -ForegroundColor Green
		((Start-Process -FilePath 'C:\_Install Files\SQL Server 2014 Express Management Tools\SQLManagementStudio_x64_ENU.exe' -Argumentlist "/Q /IACCEPTSQLSERVERLICENSETERMS=1 /ACTION=Install /UPDATEENABLED=True /FEATURES=SSMS,ADV_SSMS,Tools /INDICATEPROGRESS=False" -Wait -Passthru).ExitCode) | Out-Null
	}
}


####################################################################################################################################################################################
Function dSQLPScheckInstalled()                             # Function to check if the SQL PowerShell Module is installed and loaded, if not then download the minimum requirements to use SQLPS
{
	if (!(Get-Module -ListAvailable -Name SQLPS)) {
		Write-Host "`n`t *************************************************" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *   SQLPS seems to be missing on this machine   *" -ForegroundColor Yellow
		Write-Host "`t *       we will now install SQLPS for you       *" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *  Please be patient as this will take several  *" -ForegroundColor Yellow
		Write-Host "`t *         minutes to install to install         *" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *************************************************" -ForegroundColor Yellow
		if (!(Test-Path "C:\_Install Files\SQLPS")) {(md -Path 'C:\_Install Files\SQLPS' -Force) | Out-Null ; cd 'C:\_Install Files\SQLPS\'}
		if ([Environment]::Is64BitProcess) {
			Write-Host "`t Downloading the 64bit version of SQL Server PowerShell Tools" -ForegroundColor Green
			# Download and install the Visual C++ Redistributable Packages for Visual Studio 2013 (x64)
			if ( !(Test-Path "HKLM\SOFTWARE\Classes\Installer\Dependencies\{050d4fc8-5d48-4b8f-8972-47c82c46020f}") ) {
				(New-Object System.Net.WebClient).DownloadFile("https://download.microsoft.com/download/2/E/6/2E61CFA4-993B-4DD4-91DA-3737CD5CD6E3/vcredist_x64.exe", "C:\_Install Files\SQLPS\vcredist_x64.exe") | Out-Null
				(Start-Process -FilePath 'C:\_Install Files\SQLPS\vcredist_x64.exe' -Argumentlist "/passive" -Wait -Passthru).ExitCode | Out-Null
			}
			# Microsoft® System CLR Types for Microsoft® SQL Server® 2012
			(New-Object System.Net.WebClient).DownloadFile("http://download.microsoft.com/download/F/E/D/FEDB200F-DE2A-46D8-B661-D019DFE9D470/ENU/x64/SQLSysClrTypes.msi", "C:\_Install Files\SQLPS\1 - SQLSysClrTypes.msi") | Out-Null
			# Microsoft® SQL Server® 2012 Shared Management Objects
			(New-Object System.Net.WebClient).DownloadFile("http://download.microsoft.com/download/F/E/D/FEDB200F-DE2A-46D8-B661-D019DFE9D470/ENU/x64/SharedManagementObjects.msi", "C:\_Install Files\SQLPS\2 - SharedManagementObjects.msi") | Out-Null
			# Microsoft® Windows PowerShell Extensions for Microsoft® SQL Server® 2012
			(New-Object System.Net.WebClient).DownloadFile("http://download.microsoft.com/download/F/E/D/FEDB200F-DE2A-46D8-B661-D019DFE9D470/ENU/x64/PowerShellTools.MSI", "C:\_Install Files\SQLPS\3 - PowerShellTools.msi") | Out-Null
			# Microsoft® OLEDB Provider for DB2 v4.0 for Microsoft® SQL Server® 2012
			(New-Object System.Net.WebClient).DownloadFile("http://download.microsoft.com/download/F/E/D/FEDB200F-DE2A-46D8-B661-D019DFE9D470/ENU/x64/DB2OLEDBV4.msi", "C:\_Install Files\SQLPS\4 - DB2OLEDBV4.msi") | Out-Null
			Write-Host "`t Installing the 64bit version of SQL Server PowerShell Tools" -ForegroundColor Green
			(Start-Process -FilePath 'C:\_Install Files\SQLPS\1 - SQLSysClrTypes.msi' -Argumentlist "/passive" -Wait -Passthru).ExitCode | Out-Null
			(Start-Process -FilePath 'C:\_Install Files\SQLPS\2 - SharedManagementObjects.msi' -Argumentlist "/passive" -Wait -Passthru).ExitCode | Out-Null
			(Start-Process -FilePath 'C:\_Install Files\SQLPS\3 - PowerShellTools.msi' -Argumentlist "/passive" -Wait -Passthru).ExitCode | Out-Null
			(Start-Process -FilePath 'C:\_Install Files\SQLPS\4 - DB2OLEDBV4.msi' -Argumentlist "/passive" -Wait -Passthru).ExitCode | Out-Null
		}else{
			Write-Host "`t Downloading the 32bit version of SQL Server PowerShell Tools" -ForegroundColor Green
			# Download and install the Visual C++ Redistributable Packages for Visual Studio 2013 (x64)
			if ( !(Test-Path "HKLM\SOFTWARE\Classes\Installer\Dependencies\{f65db027-aff3-4070-886a-0d87064aabb1}") ) {
				(New-Object System.Net.WebClient).DownloadFile("https://download.microsoft.com/download/2/E/6/2E61CFA4-993B-4DD4-91DA-3737CD5CD6E3/vcredist_x86.exe", "C:\_Install Files\SQLPS\vcredist_x86.exe") | Out-Null
				(Start-Process -FilePath 'C:\_Install Files\SQLPS\vcredist_x86.exe' -Argumentlist "/passive" -Wait -Passthru).ExitCode | Out-Null
			}
			# Microsoft® System CLR Types for Microsoft® SQL Server® 2012
			(New-Object System.Net.WebClient).DownloadFile("http://download.microsoft.com/download/F/E/D/FEDB200F-DE2A-46D8-B661-D019DFE9D470/ENU/x86/SQLSysClrTypes.msi", "C:\_Install Files\SQLPS\1 - SQLSysClrTypes.msi") | Out-Null
			# Microsoft® SQL Server® 2012 Shared Management Objects
			(New-Object System.Net.WebClient).DownloadFile("http://download.microsoft.com/download/F/E/D/FEDB200F-DE2A-46D8-B661-D019DFE9D470/ENU/x86/SharedManagementObjects.msi", "C:\_Install Files\SQLPS\2 - SharedManagementObjects.msi") | Out-Null
			# Microsoft® Windows PowerShell Extensions for Microsoft® SQL Server® 2012
			(New-Object System.Net.WebClient).DownloadFile("http://download.microsoft.com/download/F/E/D/FEDB200F-DE2A-46D8-B661-D019DFE9D470/ENU/x86/PowerShellTools.msi", "C:\_Install Files\SQLPS\3 - PowerShellTools.msi") | Out-Null
			# Microsoft® OLEDB Provider for DB2 v4.0 for Microsoft® SQL Server® 2012
			(New-Object System.Net.WebClient).DownloadFile("http://download.microsoft.com/download/F/E/D/FEDB200F-DE2A-46D8-B661-D019DFE9D470/ENU/x86/DB2OLEDBV4.msi", "C:\_Install Files\SQLPS\4 - DB2OLEDBV4.msi") | Out-Null
			Write-Host "`t Installing the 32bit version of SQL Server PowerShell Tools" -ForegroundColor Green
			(Start-Process -FilePath 'C:\_Install Files\SQLPS\1 - SQLSysClrTypes.msi' -Argumentlist "/passive" -Wait -Passthru).ExitCode | Out-Null
			(Start-Process -FilePath 'C:\_Install Files\SQLPS\2 - SharedManagementObjects.msi' -Argumentlist "/passive" -Wait -Passthru).ExitCode | Out-Null
			(Start-Process -FilePath 'C:\_Install Files\SQLPS\3 - PowerShellTools.msi' -Argumentlist "/passive" -Wait -Passthru).ExitCode | Out-Null
			(Start-Process -FilePath 'C:\_Install Files\SQLPS\4 - DB2OLEDBV4.msi' -Argumentlist "/passive" -Wait -Passthru).ExitCode | Out-Null
		}
		#(set-alias installutil $env:windir\microsoft.net\framework\v2.0.50727\installutil) | Out-Null
		#(installutil -i “C:\Program Files\Microsoft SQL Server\110\Tools\PowerShell\Modules\SQLPS\Microsoft.SqlServer.Management.PSProvider.dll”) | Out-Null
		#(installutil -i “C:\Program Files\Microsoft SQL Server\110\Tools\PowerShell\Modules\SQLPS\Microsoft.SqlServer.Management.PSSnapins.dll”) | Out-Null
		# Test to make sure the SQLPS module is now loaded, if not then load it
		Write-Host "`n`t *************************************************" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *   SQLPS has been installed on this machine    *" -ForegroundColor Yellow
		Write-Host "`t *          this script will now exit            *" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *   Please re-run the script again to upgrade   *" -ForegroundColor Yellow
		Write-Host "`t *            your SolidCP deployment            *" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *      You may need to reboot this server       *" -ForegroundColor Yellow
		Write-Host "`t *       before running this script again        *" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *************************************************" -ForegroundColor Yellow
		dPressAnyKeyToExit
	}elseif ((Get-Module -ListAvailable -Name SQLPS).ExportedCommands -eq "") {
		(Add-PSSnapin SqlServerCmdletSnapin1*) | Out-Null
		(Add-PSSnapin SqlServerProviderSnapin1*) | Out-Null
	}
}


####################################################################################################################################################################################
function PowerShellVerCheck() # Check if PowerShell v3 or above is installed, if not advise a manual update first before using the upgrade script
{
	if (($Host.Version).Major -le 2) {
		Write-Host "`n`t *************************************************" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *     You need PowerShell Version 3 or above    *" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *     We reccomend manually upgrading before    *" -ForegroundColor Yellow
		Write-Host "`t *         attempting to run this script!        *" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *    Please note you cannot use Powershell 3    *" -ForegroundColor Yellow
		Write-Host "`t *               with Exchange 2010              *" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *           Please see www.solidcp.com          *" -ForegroundColor Yellow
		Write-Host "`t *              for more information             *" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *************************************************" -ForegroundColor Yellow
		dPressAnyKeyToExit
	}
}


####################################################################################################################################################################################
####################################################################################################################################################################################
# Run Check to make sure PowerShell v3 is installed as this is a requirement for the script to run
if (($Host.Version).Major -le 2) {
	Write-Host "`n`t *************************************************" -ForegroundColor Yellow
	Write-Host "`t *                                               *" -ForegroundColor Yellow
	Write-Host "`t *     You need PowerShell Version 3 or above    *" -ForegroundColor Yellow
	Write-Host "`t *                                               *" -ForegroundColor Yellow
	Write-Host "`t *     We reccomend manually upgrading before    *" -ForegroundColor Yellow
	Write-Host "`t *         attempting to run this script!        *" -ForegroundColor Yellow
	Write-Host "`t *                                               *" -ForegroundColor Yellow
	Write-Host "`t *    Please note you cannot use Powershell 3    *" -ForegroundColor Yellow
	Write-Host "`t *               with Exchange 2010              *" -ForegroundColor Yellow
	Write-Host "`t *                                               *" -ForegroundColor Yellow
	Write-Host "`t *           Please see www.solidcp.com          *" -ForegroundColor Yellow
	Write-Host "`t *              for more information             *" -ForegroundColor Yellow
	Write-Host "`t *                                               *" -ForegroundColor Yellow
	Write-Host "`t *************************************************" -ForegroundColor Yellow
	dPressAnyKeyToExit
}else{
	# Run the SolidCP Installation Menu as long as the logged in user is member of the Local "Administrators" group of the "Domain Admins" group
	if (Test-Path "$SCP_EntSvr_Dir") { # Check to make sure the script is being run on the Enterprise Server
		if (!($dDomainMember)) { # Check to see if the machine is NOT joined to a domain
			if (CheckGroupMembers "$dLangAdministratorGroup" "$dLoggedInUserName" "Local") { # Run the SOlidCP Menu if the logged in user is a Local Administrator
				Write-Host "`n`t This machine is NOT Joined to domain and you are logged in as Local Administrator Account" -ForegroundColor Green
				Write-Host "`t The SolidCP Upgrade menu is being loaded" -ForegroundColor Green
				SCPupgradeMenu
			}elseif (!(CheckGroupMembers "$dLangAdministratorGroup" "$dLoggedInUserName" "Local")) { # The logged in user is NOT a Local Administrator
				Write-Host "`n`t *************************************************" -ForegroundColor Yellow
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *         You dont seem to be logged in         *" -ForegroundColor Yellow
				Write-Host "`t *         with an Administrative account        *" -ForegroundColor Yellow
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *   Please log back on with an account that is  *" -ForegroundColor Yellow
				Write-Host "`t *      a member of the Administrators group     *" -ForegroundColor Yellow
				Write-Host "`t *           and run this script again           *" -ForegroundColor Yellow
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *************************************************" -ForegroundColor Yellow
				dPressAnyKeyToExit
			}
		}elseif ( ($dDomainMember) -and ($dLoggedInLocally) ) {
			Write-Host "`n`t *************************************************" -ForegroundColor Yellow
			Write-Host "`t *                                               *" -ForegroundColor Yellow
			Write-Host "`t *     This machine is a member of a domain      *" -ForegroundColor Yellow
			Write-Host "`t *  and you are legged in with a local account   *" -ForegroundColor Yellow
			Write-Host "`t *                                               *" -ForegroundColor Yellow
			Write-Host "`t *    You need to login with a domain account    *" -ForegroundColor Yellow
			Write-Host "`t *                                               *" -ForegroundColor Yellow
			Write-Host "`t *  Please log back on with an account that is   *" -ForegroundColor Yellow
			Write-Host "`t *      a member of the Domain Admins group      *" -ForegroundColor Yellow
			Write-Host "`t *           and run this script again           *" -ForegroundColor Yellow
			Write-Host "`t *                                               *" -ForegroundColor Yellow
			Write-Host "`t *************************************************" -ForegroundColor Yellow
			dPressAnyKeyToExit
		}elseif ( ($dDomainMember) -and (!($dLoggedInLocally)) ) {
			if (CheckGroupMembers "$dLangDomainAdminsGroup" "$dLoggedInUserName" "Domain") { # Run the SOlidCP Menu if the logged in user is a Domain Administrator
				Write-Host "`n`t This machine is Joined to domain and you are logged in as Domain Administrator Account" -ForegroundColor Green
				Write-Host "`t The SolidCP Upgrade menu is being loaded" -ForegroundColor Green
				SCPupgradeMenu
			}elseif (!(CheckGroupMembers "$dLangDomainAdminsGroup" "$dLoggedInUserName" "Domain")) { # The logged in user is NOT a Domain Administrator
				Write-Host "`n`t *************************************************" -ForegroundColor Yellow
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *     This machine is a member of a domain      *" -ForegroundColor Yellow
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *    You need to login with a domain account    *" -ForegroundColor Yellow
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *  Please log back on with an account that is   *" -ForegroundColor Yellow
				Write-Host "`t *      a member of the Domain Admins group      *" -ForegroundColor Yellow
				Write-Host "`t *           and run this script again           *" -ForegroundColor Yellow
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *************************************************" -ForegroundColor Yellow
				dPressAnyKeyToExit
			}
		}else{ # Show this error if the above conditions are not met, it should never apear
			CLS
			Write-Host "`n`t *************************************************" -ForegroundColor Yellow
			Write-Host "`t *                                               *" -ForegroundColor Yellow
			Write-Host "`t *     Oops, An unexpected error has occurred    *" -ForegroundColor Yellow
			Write-Host "`t *      We apologize for this inconvenience.     *" -ForegroundColor Yellow
			Write-Host "`t *                                               *" -ForegroundColor Yellow
			Write-Host "`t *    Please contact SolidCP Technical Support   *" -ForegroundColor Yellow
			Write-Host "`t *            on support@solidcp.com             *" -ForegroundColor Yellow
			Write-Host "`t *                                               *" -ForegroundColor Yellow
			Write-Host "`t *      Please let them know the error was       *" -ForegroundColor Yellow
			Write-Host "`t *      with the SolidCP PowerShell Script       *" -ForegroundColor Yellow
			Write-Host "`t *                                               *" -ForegroundColor Yellow
			Write-Host "`t *************************************************" -ForegroundColor Yellow
			Write-Host "`n`n`t ==============  DEBUG INFORMATION  ==============" -ForegroundColor Green
			Write-Host "`t Joined to domain     = $dDomainMember" -ForegroundColor Green
			Write-Host "`t FQDN of this machine = $dFQDNthisMachine" -ForegroundColor Green
			Write-Host "`t Domain Name          = $dDomainName" -ForegroundColor Green
			Write-Host "`t Computer Name        = $dComputerName" -ForegroundColor Green
			Write-Host "`t Logged in User Name  = $dLoggedInUserName" -ForegroundColor Green
			Write-Host "`t Logged in Locally    = $dLoggedInLocally" -ForegroundColor Green
			dPressAnyKeyToExit
		}
	}else{
		Write-Host "`n`t *************************************************" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *      You MUST run this script from your       *" -ForegroundColor Yellow
		Write-Host "`t *           SolidCP Enterprise Server           *" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *    Please log on to your Enterpeise Server    *" -ForegroundColor Yellow
		Write-Host "`t *        and run this script from there         *" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *************************************************" -ForegroundColor Yellow
		dPressAnyKeyToExit
	}
}
