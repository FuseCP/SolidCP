<####################################################################################################
SolidSCP - Simple server setup menu

v1.0    24th May 2016:       First release of the SolidCP Install Script
v1.0.1  15th June 2016       Added Exchange 2016 and Silent Installation to the script
v1.0.2  16th June 2016       Fixes on Option 4 (removed [TAB] and replaced with [SPACE] so PowerShell doesn't try to Auto Complete
v1.0.3  22nd June 2016       Added domain membership checking as well as various bug fixes for File Permission Hardening
v1.0.4  23nd June 2016       Added MariaDB Database Server as well as various fixes
v1.0.5  25th June 2016       New menu structure added for simplicity
v1.0.6  28th June 2016       Additional deployment options for Active Directory
v1.0.7  04th July 2016       Added XML function and improved the File Permission Hardening for the XML files in IIS for .NET
v1.0.8  05th August 2016     Added IP Address checking for Firewall Rules - allow UNC access for SolidCP upgrade via Portal or Auto-Upgrade Script
v1.0.9  09th August 2016     Added VC++ 2012 runtime for PHP and option for phpMyAdmin, added submenu for SCP ES and Portal installation options - Thanks to S.Brown
v1.1.0  2nd  September 2016: Added web.config file updates to the script so the new features are added
v1.1.1  10th October 2016:   Added IIS SSL Hardening and fixed PHP v7.0 installation bug
v1.1.2  19th October 2016:   Added Dynamic Download Support - will download the latest files from the SolidCP XML List
v1.1.3  20th October 2016:   Added Tools Menu
v1.1.4  22nd October 2016:   Added Exchange DAG Creation
v1.1.5  3rd  November 2016:  Added Mail Enable Silent Installation
v1.1.6  6th  November 2016:  Added extra permissions to resolve issues with Web Server that are joined to a domain
v1.1.7  14th November 2016:  Added WebDav to the script so you can configure your Cloud Sorage Folders in SolidCP
v1.1.8  18th November 2016:  Resolved an issue with the SSL Hardening for Windows Servers on the Ciphers
v1.1.9  21st November 2016:  Added MailCleaner Move Spam to Junk Folder rule
v1.2.0  25th November 2016:  Added Remote Desktop Services to the script to fully provision your RDS Deployment
v1.2.1  29th November 2016:  Added LetsEncrypt support to the script
v1.2.2  6th  December 2016:  Resolved issue with phpMyAdmin installation and the SSL Hardening functions
v1.2.3  28th December 2016:  Added PreLoad for all SolidCP Components to speed up the load times on the web interface
v1.2.4  17th January  2017:  Increased the Exchange Send and Receive Connectors sizes to the maximum as they are low by default, they can now be set via Mailbox Plans in the portal
v1.2.5  5th  February 2017:  Improved the installation speed on Microsoft Windows Server 2016
v1.2.6  9th  March    2017:  Fixed installation issues on some Exchange Server 2016 setups


Written By Marc Banyard for the SolidCP Project (c) 2016 SolidCP

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
$host.UI.RawUI.BufferSize = New-Object -TypeName System.Management.Automation.Host.Size -ArgumentList (120, 50)
$host.UI.RawUI.WindowSize = New-Object -TypeName System.Management.Automation.Host.Size -ArgumentList (120, 50)
$Host.UI.RawUI.WindowTitle = "$([Environment]::UserName): --  SolidCP Server Configuration Script  --"
CLS
Write-Host "
        ****************************************
        *                                      *
        *        Welcome to the SolidCP        *
        *          Automated Installer         *
        *                                      *
        *       Please be patient whilst       *
        *          the menu is loaded          *
        *                                      *
        ****************************************" -ForegroundColor Green
####################################################################################################
# Additional items used in the below functions, they are stored as variables for easy use later    #
####################################################################################################
$dIPV4 = ((Test-Connection $env:computername -count 1).IPv4address.IPAddressToString)       # Get IPV4 Address
$dIPV6 = ((Resolve-DnsName -Name $env:COMPUTERNAME -Type AAAA).IPAddress -notlike 'fe80:*') # Get IPV6 Address
$dDomainMember     = ((gwmi win32_computersystem).partofdomain -eq $true)                   # Check if machine is joined to a domain
$dFQDNthisMachine  = ([System.Net.Dns]::GetHostByName(($env:computerName)).HostName)        # Get the FQDN of this machine
$dRootDN           = (([ADSI]"LDAP://RootDSE").rootDomainNamingContext)                     # Get the Root Distinguished Name from Active Directory (i.e. DC=SolidCP,DC=com)
$dDomainName       = $env:USERDNSDOMAIN  # Store the Domain Name (if joined) as a variable to use later
$dComputerName     = $env:computername   # Store the Computer Name as a variable to use later
$dLoggedInUserName = $env:USERNAME       # Store the Logged On User Name as a variable to use later
####################################################################################################
####################################################################################################

# Editable features are below this line

$SolidCPhstSpace   = "$env:SystemDrive\HostingSpaces"  # Hosting Spaces Directory Location (including Drive)
$InstallSNMPchk    = $true                  # Enable or disable by $true|$false
$SNMPsysContact    = "Please Change Me"     # SNMP Contact Name
$SNMPsysLocation   = "Please Change Me"     # SNMP Server Location
$dMsSQLpassword    = ""                     # Enter the password for your MS SQL "sa" User
$dMySQLvMariaDB    = "MariaDB"              # Choose between "MySQL|MariaDB" for the Database Type
$dMariaMySQLpasswd = ""                     # Enter the password for your MariaDB or MySQL "root" User
$dInstalPhpMyAdmin = ""                     # Specify if phpMyAdmin is to be installed on the Web Servers that have MySQL/MariaDB $true|$false
$dPhpMyAdminPort   = "80"                   # phpMyAdmin TCP Port for website (if installed on Web or Database Server)
$dPhpMyAdminUserNm = "pma"                  # phpMyAdmin Username
$dPhpMyAdminPassWd = "YourPassword"         # phpMyAdmin Password
$dPhpMyAdminHostNm = "pma"                  # Set the FQDN (Hostname) to be used for phpMyAdmin - Default = ServerFQDNpma (i.e. web1pma.solidcp.com)
$dEnableIISsmtpSvc = ""                     # Enable or Disable the SMTP Virtual Server in IIS $true|$false - Leave blank to prompt
$dExchangeOrgNme   = "HostedExchange"       # Set the Exchange Organisation Name
$dExchangeFQDNsndC = ""                     # Set the FQDN of the Exchange Send Connector
$dOutlookAnywhFQDN = ""                     # Set the FQDN for Outlook Anywhere
$dExchangeAutoDisc = ""                     # Set the FQDN for Exchange Autodiscover - If this is empty it will use your Outlook Anywhere FQDN
$dRDSconnBroker    = ""                     # Set the FQDN for your Remote Desktop Connection Broker
$dRDSGatewayFQDN   = ""                     # Set the FQDN for your RD Gateway cluster
$dSolidCp_OU_Name  = "SolidCP"              # Set the Active Directory Organisational Unit Name
$dSCPhstd_OU_Name  = "Hosted"               # Set the Active Directory Organisational Unit Name for your Hosted Accounts (Exchange / SharePoint / RDP)
$dExchangeInstal   = "D:"                   # Set the installation directory of where your Exchange files are located (i.e. D: for the DVD)
$dExchangeImportUserFirst = "SolidCP"                # Exchange Import/Export Active Directory Account First Name
$dExchangeImportUserLast  = "Exchange Administrator" # Exchange Import/Export Active Directory Account Last Name
$dExchangeImportLogonName = "ExchangeAdminAcc"       # Exchange Import/Export Active Directory Account Username
$dExchangeImportFolderNme = "C:\_pstImports"         # Exchange PST Imports Folder
$dExchangeExportFolderNme = "C:\_pstExports"         # Exchange PST Exports Folder
$dSolidCPportalPortNumber = "80"            # SolidCP Web Portal - Port Number
$dSolidCPportalIPaddress  = $dIPV4          # SolidCP Web Portal - IP Address
$dSolidCPportalPassword   = "YourPassword"  # SolidCP Web Portal - serveradmin Password
$dSolidCPserverPassword   = "YourPassword"  # SolidCP Server - Password
$dSolidCPCloudPortlPasswd = "YourPassword"  # SolidCP Cloud Storage Server - Password
$dRDSourganisationalUnit  = "RemoteDesktopServers"  # Remote Desktop Services Organisational Unit
$dSolidCPEnterpriseSvrIP  = ""              # Enterprise Server IP Address (i.e. 192.168.1.1)
$dSolidCPEnterpriseSvrURL = ""              # Enterprise Server URL - FULL URL with Port (i.e. http://192.168.1.1:9002)
$dSCPdomainName           = ""              # FQDN used for Active Directory (i.e. hosted.solidcp.com)
$dSCPfirewallUNCshareIPs  = ""              # IPV4 Addresses seperated by semicolon for UNC Access to servers (192.168.1.1; 192.168.2.0/24)
$dSCPfirewallRDPaccess    = ""              # IPV4 Addresses seperated by semicolon for RDP Access to servers (192.168.1.1; 192.168.2.0/24)
$dSCPqualitySSLlabsRating = "A"             # Qualys SSL Labs Rating - https://www.ssllabs.com/ssltest/
$ddSCPqualySSLScheduleTsk = $true           # Setup an Automated Script to check for new SSL Fixes on your web servers by $true|$false
$dMailEnableDirectory     = "C:\Program Files (x86)\Mail Enable" # Specify the installation directory where MailEnable needs to be installed to on the machine
$dMailEnableContactName   = "SolidCP"       # Mail Enable Company Name (who it needs to be registered to
$dWebDavStoragePath       = "C:\CloudStorage"                    # The path for the SolidCP WebDav Storage
$dWebDavStoragePortNumber = "80"                                 # The SolidCP Cloud Storage Portal (WebDav Front End) Port Number


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
# Language section, this is needed as Microsoft have different account names depending on language #
####################################################################################################
$dLocalAdministratorSID  = (Get-WmiObject -Query "SELECT SID FROM Win32_UserAccount WHERE LocalAccount = 'True'" | Select-Object -First 1 -ExpandProperty SID); # Local Administrator SID
$dDomainAdministratorSID = (Get-WmiObject -Query "SELECT SID FROM Win32_UserAccount WHERE LocalAccount = 'False'" | Select-Object -First 1 -ExpandProperty SID); # Domain Administrator SID
$dMachineSID             = ((Get-WmiObject -Query "SELECT SID FROM Win32_UserAccount WHERE LocalAccount = 'True'" | Select-Object -First 1 -ExpandProperty SID) -replace ".{4}$"); # Local Machine SID
$dDomainSID              = ((Get-WmiObject -Query "SELECT SID FROM Win32_UserAccount WHERE LocalAccount = 'False'" | Select-Object -First 1 -ExpandProperty SID) -replace ".{4}$"); # Domain SID
$dLoggedInLocally        = ( ((Get-WMIObject -class Win32_ComputerSystem | select username).username) -eq ("$env:COMPUTERNAME\$env:USERNAME") ) # Check if logged in locally
$dLangAdministratorName  = (([wmi]"Win32_SID.SID='$dLocalAdministratorSID'").AccountName); # Administrator
$dLangCreatorOwnerName   = (([wmi]"Win32_SID.SID='S-1-3-0'").AccountName);                 # CREATOR OWNER
$dLangSystemName         = (([wmi]"Win32_SID.SID='S-1-5-18'").AccountName);                # SYSTEM
$dLangLocalServiceName   = (([wmi]"Win32_SID.SID='S-1-5-19'").AccountName);                # LOCAL SERVICE
$dLangNetworkServiceName = (([wmi]"Win32_SID.SID='S-1-5-20'").AccountName);                # NETWORK SERVICE
$dLangAdministratorGroup = (([wmi]"Win32_SID.SID='S-1-5-32-544'").AccountName);            # Administrators
$dLangUsersGroup         = (([wmi]"Win32_SID.SID='S-1-5-32-545'").AccountName);            # Users
$dLangPreWin2000Group    = (([wmi]"Win32_SID.SID='S-1-5-32-554'").AccountName);            # Pre-Windows 2000 Compatible Access
if ($dDomainMember) { # Only do the following if the server is a member of a domain
	$dLangDomainAdminsGroup       = (([wmi]"Win32_SID.SID='$dDomainSID-512'").AccountName);          # Domain Admins
	$dLangDomainAdministratorName = (([wmi]"Win32_SID.SID='$dDomainAdministratorSID'").AccountName); # Administrator
	$dLangDomainEnterpriseAdmins  = (([wmi]"Win32_SID.SID='$dDomainSID-519'").AccountName);          # Enterprise Admins
}
$dSCPiisSSLratingURLa     = "http://installer.solidcp.com/Files/XML/SSL/IIS_SSL_Hardening_A.xml"     # XML Feed for A Rating from Qualys SSL Labs for your IIS Server
$dSCPiisSSLratingURLb     = "http://installer.solidcp.com/Files/XML/SSL/IIS_SSL_Hardening_B.xml"     # XML Feed for B Rating from Qualys SSL Labs for your IIS Server
$dSCPiisSSLDateCheckFile  = "C:\SolidCP\SSL_Fix_DO_NOT_DELETE.txt"                                   # This is the location of the file that stores the last date the IIS SSL Security was updated on the server
$dSCPFileURL              = "http://installer.solidcp.com/Files/XML/Downloads/Download-Links.xml"    # XML Feed for files that are downloaded as part of the PowerShell Auto Installation Script
$dSCPFileDownloadLinks    = ([xml](New-Object System.Net.WebClient).DownloadString("$dSCPFileURL"))  # Download the XML Feed for files and store as a variable
(Import-Module ServerManager) | Out-Null                                                             # Import the "ServerManager" module
####################################################################################################
# Main menu starts here
Function SolidCPmenu() {
	do {
		do {
		cls
			Write-Host "`n`tSolidCP Setup Menu`n" -ForegroundColor Magenta
			Write-Host "`t  Please select the type of server you are installing SolidCP onto`n" -ForegroundColor Cyan
	$menu = @"
	    1. SolidCP Exterprise Server / Portal
	    2. Microsoft DNS Server
	    3. Microsoft Web Server
	    4. Database Servers (MS SQL, MariaDB, MySQL)
	    5. AwStats Server
	    6. Active Directory (Domain Controllers)
	    7. Email Servers
	    8. Microsoft Remote Desktop Services
	    9. Cloud Storage Portal (WebDav)

	    T. Tools

	    X. Quit and exit
"@
	$menuOptions = '^[1-9tx]+$'

			Write-Host $menu -ForegroundColor Cyan
			$choice = Read-Host "`n`tEnter Option From Above Menu"
			$ok = $choice -match $menuOptions
		
			if ( -not $ok) { Write-Host "Invalid selection" ; start-Sleep -Seconds 2 }
		} until ( $ok )

		switch -Regex ( $choice ) {
			"1" { MainMenu_Option_1 }
			"2" { dInstallConfirm "Microsoft DNS Server" $function:MainMenu_Option_2 ; $choice="X" }
			"3" { MainMenu_Option_3 }
			"4" { MainMenu_Option_4 }
			"5" { dInstallConfirm "AwStats Server" $function:MainMenu_Option_5 ; $choice="X" }
			"6" { dInstallConfirm "Microsoft Active Directory Requirements" $function:MainMenu_Option_6 ; $choice="X" }
			"7" { MainMenu_Option_7 }
			"8" { MainMenu_Option_8 }
			"9" { dInstallConfirm "SolidCP Cloud Storage Portal (WebDav)" $function:MainMenu_Option_9 ; $choice="X" }
			"T" { MainMenu_Option_T }
			"X" { MainMenu_Option_X }
		}
	} until ( $choice -match "X" )
}
# Main menu end

####################################################################################################################################################################################
####################################################################################################################################################################################
# Functions for the Main Menu Options are below here
####################################################################################################################################################################################
# Main Menu - Option 1 - Present Sub Menu for SolidCP Enterprise Server and Portal Server deployment types
Function MainMenu_Option_1() { # Function to show the user a Sub Menu for the SolidCP Enterprise Server and Portal Server deployment options
	do {
		do {
		cls
			Write-Host "`n`tSolidCP Setup Menu - Enterprise Server and Portal Server`n" -ForegroundColor Magenta
			Write-Host "`t  Please select the Enterprise Server and Portal Server deployment you want to install`n`n" -ForegroundColor Cyan
	$menu = @"
	    A. SolidCP Enterprise Server and Portal Server (single Machine)
	    B. SolidCP Enterprise Server ONLY
	    C. SolidCP Portal Server ONLY

	    X. Exit back to Main Menu
"@
	$menuOptions = '^[a-cx]+$'

			Write-Host $menu -ForegroundColor Cyan
			$choice1 = Read-Host "`n`tEnter Option From Above Menu"
			$ok = $choice1 -match $menuOptions

			if ( -not $ok) { Write-Host "Invalid selection" ; start-Sleep -Seconds 2 }
		} until ( $ok )

		switch -Regex ( $choice1 ) {
			"A" { dInstallConfirm "SolidCP Enterprise Server and Portal Server (single Machine)" $function:MainMenu_Option_1_A }
			"B" { dInstallConfirm "SolidCP Enterprise Server ONLY" $function:MainMenu_Option_1_B }
			"C" { dInstallConfirm "SolidCP Portal Server ONLY" $function:MainMenu_Option_1_C }
		}
	} until ( $choice1 -match "X" )
}

####################################################################################################################################################################################
# Main Menu - Option 1 - Sub Menu A - Install SolidCP Enterpeise Server and Portal Server
Function MainMenu_Option_1_A() { # Function to call multiple functions to install SolidCP Enterprise Server and Portal
	CheckSolidCPdomainUser "Enterprise"
	CheckSolidCPdomainUser "Portal"
	dCheckIPaddressesSet
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	dCheckMsSQLpassword
	Write-Host "`n Installing the 'SolidCP Enterprise Server and Portal'" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	InstallIISforSolidCP
	InstallIISforESandPortal
	HardenDotNETforIIS
	HardenFilePermissions
	IIS_SMTP_VirtualServer -Enable
	InstallSQLsvr2014withTools -Password $dMsSQLpassword
	InstallDisablePasswdComplex
	InstallSolidCPInstaller
	InstallSolidCPcomponent -Enterprise
	InstallSolidCPcomponent -Portal
	HardenIIS_SSL
	EnableWindowsUpdates
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 1 - Sub Menu B - Install SolidCP Enterpeise Server ONLY
Function MainMenu_Option_1_B() { # Function to call multiple functions to install SolidCP Enterprise Server and Portal
	CheckSolidCPdomainUser "Enterprise"
	dCheckIPaddressesSet
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	dCheckMsSQLpassword
	Write-Host "`n Installing the 'SolidCP Enterprise Server'" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	InstallIISforSolidCP
	InstallIISforESandPortal
	HardenDotNETforIIS
	HardenFilePermissions
	IIS_SMTP_VirtualServer -Enable
	InstallSQLsvr2014withTools -Password $dMsSQLpassword
	InstallDisablePasswdComplex
	InstallSolidCPInstaller
	InstallSolidCPcomponent -Enterprise
	HardenIIS_SSL
	EnableWindowsUpdates
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 1 - Sub Menu C - Install SolidCP Portal Server ONLY
Function MainMenu_Option_1_C() { # Function to call multiple functions to install SolidCP Enterprise Server and Portal
	CheckSolidCPdomainUser "Portal"
	dCheckIPaddressesSet
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	Write-Host "`nInstalling the 'SolidCP Portal Server'" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	InstallIISforSolidCP
	InstallIISforESandPortal
	HardenDotNETforIIS
	HardenFilePermissions
	InstallDisablePasswdComplex
	InstallSolidCPInstaller
	InstallSolidCPcomponent -Portal
	HardenIIS_SSL
	EnableWindowsUpdates
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 2 - Install Microsoft DNS Server for SolidCP
Function MainMenu_Option_2() { # Function to call multiple functions to install Features for Microsoft DNS Server
	CheckSolidCPdomainUser "Server"
	dCheckIPaddressesSet
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	Write-Host "Installing a 'Microsoft DNS Server'" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	InstallIISforSolidCP
	InstallDNSServer
	HardenDotNETforIIS
	HardenFilePermissions
	InstallDisablePasswdComplex
	InstallSolidCPInstaller
	InstallSolidCPcomponent -Server
	HardenIIS_SSL
	EnableWindowsUpdates
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 3 - Present Sub Menu for Microsoft IIS Server deployment types
Function MainMenu_Option_3() { # Function to show the user a Sub Menu for the Microsoft IIS Server deployment options
	do {
		do {
		cls
			Write-Host "`n`tSolidCP Setup Menu - Microsoft Web Server`n" -ForegroundColor Magenta
			Write-Host "`t  Please select the Microsoft Web Server deployment you want to install`n`n" -ForegroundColor Cyan
			Write-Host "`t  (IIS, FTP, PHP, PHP Manager, Python, Perl, MariaDB or MySQL, MS SQL)`n`n" -ForegroundColor Cyan
	$menu = @"
	    A. Microsoft Web Server - Fully Automated
	    B. Microsoft Web Server - (NO Database) Fully Automated
	    C. Microsoft Web Server Prompted
	    D. Microsoft Web Server (Basic IIS & FTP Server)

	    X. Exit back to Main Menu
"@
	$menuOptions = '^[a-dx]+$'

			Write-Host $menu -ForegroundColor Cyan
			$choice3 = Read-Host "`n`tEnter Option From Above Menu"
			$ok = $choice3 -match $menuOptions
		
			if ( -not $ok) { Write-Host "Invalid selection" ; start-Sleep -Seconds 2 }
		} until ( $ok )
		
		switch -Regex ( $choice3 ) {
			"A" { dInstallConfirm "IIS, FTP, PHP, Python, Perl, MariaDB or MySQL and MS SQL (Automated)" $function:MainMenu_Option_3_A }
			"B" { dInstallConfirm "IIS, FTP, PHP, Python, Perl - Without Database Servers (Automated)" $function:MainMenu_Option_3_B }
			"C" { dInstallConfirm "this server as a Web Server" $function:MainMenu_Option_3_C }
			"D" { dInstallConfirm "this server as a Web Server (IIS and FTP)" $function:MainMenu_Option_3_D }
		}
	} until ( $choice3 -match "X" )
}

####################################################################################################################################################################################
# Main Menu - Option 3 - Sub Menu A - Install Microsoft IIS Web Server for SolidCP
Function MainMenu_Option_3_A() { # Function to call multiple functions to install IIS, FTP, PHP, Python, Perl, MariaDB or MySQL and MS SQL
	CheckSolidCPdomainUser "Server"
	dCheckIPaddressesSet
	IIS_SMTP_InstallCheck
	dAskInstallPhpMyAdmin
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	dCheckMsSQLpassword
	dCheckMariaMySQLpassword
	Write-Host "Installing a 'Microsoft Web Server' (IIS, FTP, PHP, Python, Perl, $dMySQLvMariaDB and MS SQL)" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	InstallIISforSolidCP
	InstallIISforHosting
	HardenDotNETforIIS
	HardenFilePermissions
	InstallFTPforHosting
	InstallWebPI_PHP
	InstallWebPI_SQLdriverPHP
	InstallWebPI_WinCachePHP
	InstallWebPI_Python
	InstallWebPI_URLreWrite
	InstallWebPI_WebDeploy
	InstallActivePerl
	InstallMariaDB_MySQL $dMariaMySQLpasswd
	InstallPhpMyAdmin
	InstallSQLsvr2014withTools -Password $dMsSQLpassword
	InstallDisablePasswdComplex
	IIS_SMTP_VirtualServer
	InstallSolidCPInstaller
	InstallSolidCPcomponent -Server
	AddWebServerToDomainIISacco
	HardenIIS_SSL
	InstallLetsEncryptACMESharp
	EnableWindowsUpdates
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 3 - Sub Menu B - Install Microsoft IIS Web Server without Database Servers for SolidCP
Function MainMenu_Option_3_B() { # Function to call multiple functions to install IIS, FTP, PHP, Python, Perl
	CheckSolidCPdomainUser "Server"
	dCheckIPaddressesSet
	IIS_SMTP_InstallCheck
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	Write-Host "Installing a 'Microsoft Web Server' (IIS, FTP, PHP, Python, Perl)" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	InstallIISforSolidCP
	InstallIISforHosting
	HardenDotNETforIIS
	HardenFilePermissions
	InstallFTPforHosting
	InstallWebPI_PHP
	InstallWebPI_SQLdriverPHP
	InstallWebPI_WinCachePHP
	InstallWebPI_Python
	InstallWebPI_URLreWrite
	InstallWebPI_WebDeploy
	InstallActivePerl
	InstallDisablePasswdComplex
	IIS_SMTP_VirtualServer
	InstallSolidCPInstaller
	InstallSolidCPcomponent -Server
	AddWebServerToDomainIISacco
	HardenIIS_SSL
	InstallLetsEncryptACMESharp
	EnableWindowsUpdates
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 3 - Sub Menu C - Install Microsoft IIS Web Server for SolidCP - Prompted
Function MainMenu_Option_3_C() { # Function to call multiple functions to install Features for IIS, FTP, PHP, Python, Perl, MySQL and MS SQL
	CheckSolidCPdomainUser "Server"
	dCheckIPaddressesSet
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	dCheckMsSQLpassword
	dCheckMariaMySQLpassword
	Write-Host "Installing a 'Microsoft Web Server - Prompted'" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	InstallIISforSolidCP
	InstallIISforHosting
	dInstallConfirmNoCLS "Harden .NET for IIS" $function:HardenDotNETforIIS                            # Prompt to Harden .NET for IIS
	dInstallConfirmNoCLS "Harden the File Permissions" $function:HardenFilePermissions                 # Prompt to harden the file permissions
	dInstallConfirmNoCLS "Microsoft FTP Server" $function:InstallFTPforHosting                         # Prompt to install Microsoft FTP Server
	dInstallConfirmNoCLS "PHP for IIS" $function:InstallWebPI_PHP                                      # Prompt to install PHP for IIS
	dInstallConfirmNoCLS "SQL Server Driver for PHP" $function:InstallWebPI_SQLdriverPHP               # Prompt to install SQL Server Driver for PHP
	dInstallConfirmNoCLS "WinCache for PHP" $function:InstallWebPI_WinCachePHP                         # Prompt to install WinCache for PHP
	dInstallConfirmNoCLS "Python v3.4" $function:InstallWebPI_Python                                   # Prompt to install Python v3.4
	dInstallConfirmNoCLS "URL Rewrite Module" $function:InstallWebPI_URLreWrite                        # Prompt to install URL Rewrite Module
	dInstallConfirmNoCLS "Web Deploy module" $function:InstallWebPI_WebDeploy                          # Prompt to install Web Deploy Module
	dInstallConfirmNoCLS "Active Perl v5.22.1" $function:InstallActivePerl                             # Prompt to install Active Perl v5.22.1
	dInstallConfirmNoCLS "$dMySQLvMariaDB Database Server" $function:InstallMariaDB_MySQL              # Prompt to install MariaDB or MySQL Database Server
	dInstallConfirmNoCLS "Microsoft SQL Server 2014 Express" $function:InstallSQLsvr2014noTools        # Prompt to install Microsoft SQL Server 2014 Express
	dInstallConfirmNoCLS "SQL Server 2014 Management Tools" $function:InstallSQLsvr2014mgmtTools       # Prompt to install Microsoft SQL Server 2014 Express Management Tools
	dInstallConfirmNoCLS "Disable Password Complexity (Local)" $function:InstallDisablePasswdComplex   # Prompt to disable local password complexity
	IIS_SMTP_InstallCheck                                                                              # Prompt to enable the SMTP Virtual Server in IIS
	IIS_SMTP_VirtualServer                                                                             # Install the SMTP Virtual Server if required
	dInstallConfirmNoCLS "SolidCP Server" $function:InstallSolidCPcomponentServer                      # Prompt to download and install the SOlidCP Installation Application and Server component
	dInstallConfirmNoCLS "IIS SSL Hardening for security" $function:HardenIIS_SSL                      # Prompt to fix the SSL issues in IIS to resolve various issues regarding SSL Certificates
	dInstallConfirmNoCLS "LetsEncrypt (ACMESharp)" $function:InstallLetsEncryptACMESharp               # Prompt to install LetsEncrypt for free SSL Certificates
	dInstallConfirmNoCLS "Windows Updates" $function:EnableWindowsUpdates                              # Prompt to enable Windows Updates on this machine
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 3 - Sub Menu D - Install Microsoft IIS Web Server for SolidCP
Function MainMenu_Option_3_D() { # Function to call multiple functions to install IIS, FTP for Microsoft Web Server ONLY
	CheckSolidCPdomainUser "Server"
	dCheckIPaddressesSet
	IIS_SMTP_InstallCheck
	dAskInstallPhpMyAdmin
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	dCheckMsSQLpassword
	dCheckMariaMySQLpassword
	Write-Host "Installing a 'Microsoft Web Server' (IIS, FTP)" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	InstallIISforSolidCP
	InstallIISforHosting
	HardenDotNETforIIS
	HardenFilePermissions
	InstallFTPforHosting
	InstallWebPI_URLreWrite
	InstallWebPI_WebDeploy
	InstallDisablePasswdComplex
	IIS_SMTP_VirtualServer
	InstallSolidCPInstaller
	InstallSolidCPcomponent -Server
	AddWebServerToDomainIISacco
	HardenIIS_SSL
	EnableWindowsUpdates
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 4 - Present Sub Menu for Database Server deployment types
Function MainMenu_Option_4() { # Function to show the user a Sub Menu for the Database Server deployment options
	do {
		do {
		cls
			Write-Host "`n`tSolidCP Setup Menu - Database Server`n" -ForegroundColor Magenta
			Write-Host "`t  Please select the Database Server deployment you want to install`n`n" -ForegroundColor Cyan
	$menu = @"
	    A. FULL Database Server - (Microsoft SQL Server 2014, MariaDB or MySQL)
	    B. Microsoft SQL Server 2014 Express With Tools
	    C. Microsoft SQL Server 2014 Express Database ONLY (No Tools)
	    D. MariaDB Server or MySQL Server

	    X. Exit back to Main Menu
"@
	$menuOptions = '^[a-dx]+$'

			Write-Host $menu -ForegroundColor Cyan
			$choice4 = Read-Host "`n`tEnter Option From Above Menu"
			$ok = $choice4 -match $menuOptions
		
			if ( -not $ok) { Write-Host "Invalid selection" ; start-Sleep -Seconds 2 }
		} until ( $ok )
		
		switch -Regex ( $choice4 ) {
			"A" { dInstallConfirm "FULL Database Server - (Microsoft SQL Server 2014, MariaDB or MySQL)" $function:MainMenu_Option_4_A }
			"B" { dInstallConfirm "Microsoft SQL Server 2014 Express With Tools" $function:MainMenu_Option_4_B }
			"C" { dInstallConfirm "Microsoft SQL Server 2014 Express Database ONLY (No Tools)" $function:MainMenu_Option_4_C }
			"D" { dInstallConfirm "MariaDB Server or MySQL Server" $function:MainMenu_Option_4_D }
		}
	} until ( $choice4 -match "X" )
}

####################################################################################################################################################################################
# Main Menu - Option 4 - Sub Menu A - Install Full Database Server for SolidCP
Function MainMenu_Option_4_A() { # Function to call multiple functions to install IIS, MariaDB or MySQL and MS SQL
	CheckSolidCPdomainUser "Server"
	dCheckIPaddressesSet
	dAskInstallPhpMyAdmin
	CLS
	dCheckMsSQLpassword
	dCheckMariaMySQLpassword
	Write-Host "Installing the FULL Database Server - (Microsoft SQL Server 2014, $dMySQLvMariaDB Database Server)" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	InstallIISforSolidCP
	InstallIISforHosting
	HardenDotNETforIIS
	HardenFilePermissions
	InstallMariaDB_MySQL $dMariaMySQLpasswd
	InstallPhpMyAdmin
	InstallSQLsvr2014withTools -Password $dMsSQLpassword
	InstallDisablePasswdComplex
	InstallSolidCPInstaller
	InstallSolidCPcomponent -Server
	HardenIIS_SSL
	EnableWindowsUpdates
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 4 - Sub Menu B - Install Microsoft SQL Express Database Server with Tools for SolidCP
Function MainMenu_Option_4_B() { # Function to call multiple functions to install IIS, MS SQL and Tools
	CheckSolidCPdomainUser "Server"
	dCheckIPaddressesSet
	CLS
	dCheckMsSQLpassword
	Write-Host "Installing the Microsoft SQL Server 2014 Express Database ONLY (No Tools))" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	InstallIISforSolidCP
	InstallIISforHosting
	HardenDotNETforIIS
	HardenFilePermissions
	InstallSQLsvr2014noTools $dMsSQLpassword
	InstallDisablePasswdComplex
	InstallSolidCPInstaller
	InstallSolidCPcomponent -Server
	HardenIIS_SSL
	EnableWindowsUpdates
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 4 - Sub Menu C - Install Microsoft SQL Database Server ONLY (NO Tools) for SolidCP
Function MainMenu_Option_4_C() { # Function to call multiple functions to install IIS and MS SQL
	CheckSolidCPdomainUser "Server"
	dCheckIPaddressesSet
	CLS
	dCheckMsSQLpassword
	Write-Host "Installing the Microsoft SQL Server 2014 Express with Tools)" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	InstallIISforSolidCP
	InstallIISforHosting
	HardenDotNETforIIS
	HardenFilePermissions
	InstallSQLsvr2014withTools -Password $dMsSQLpassword
	InstallDisablePasswdComplex
	InstallSolidCPInstaller
	InstallSolidCPcomponent -Server
	HardenIIS_SSL
	EnableWindowsUpdates
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 4 - Sub Menu D - Install MariaDB or MySQL Database Server for SolidCP
Function MainMenu_Option_4_D() { # Function to call multiple functions to install MariaDB or MySQL
	CheckSolidCPdomainUser "Server"
	dCheckIPaddressesSet
	dAskInstallPhpMyAdmin
	CLS
	dCheckMariaMySQLpassword
	Write-Host "Installing the $dMySQLvMariaDB Database Server)" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	InstallIISforSolidCP
	InstallIISforHosting
	HardenDotNETforIIS
	HardenFilePermissions
	InstallMariaDB_MySQL $dMariaMySQLpasswd
	InstallPhpMyAdmin
	InstallDisablePasswdComplex
	InstallSolidCPInstaller
	InstallSolidCPcomponent -Server
	HardenIIS_SSL
	EnableWindowsUpdates
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 5 - Install AwStats Server for SolidCP
Function MainMenu_Option_5() { # Function to call multiple functions to install AwStats
	CheckSolidCPdomainUser "Server"
	dCheckIPaddressesSet
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	Write-Host "Installing the 'AwStats Server'" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	InstallIISforSolidCP
	InstallAwStats
	HardenDotNETforIIS
	HardenFilePermissions
	InstallActivePerl
	InstallDisablePasswdComplex
	InstallSolidCPInstaller
	InstallSolidCPcomponent -Server
	HardenIIS_SSL
	EnableWindowsUpdates
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 6 - Install Microsoft Active Directory
Function MainMenu_Option_6() { # Function to call multiple functions to install Active Directory Requirements
	dCheckIPaddressesSet
	CLS
	Write-Host "`n`n`n`n`n`n`n`n`n`n`n`n"  # This brings the text down by 12 lines to allow fo the Windows Features installation overlay
	Write-Host "Installing the required windows features for 'Microsoft Active Directory (Domain Controllers)'" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	InstallActiveDirectory
	EnableWindowsUpdates
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 7 - Present Sub Menu for Email Server deployment types
Function MainMenu_Option_7() { # Function to show the user a Sub Menu for the Email Server deployment options
	do {
		do {
		cls
			Write-Host "`n`tSolidCP Setup Menu - Email Servers`n" -ForegroundColor Magenta
			Write-Host "`t  Please select the Email Server deployment you want to install`n`n" -ForegroundColor Cyan
	$menu = @"
	    A. Microsoft Exchange Server 2016 - CAS & Mailbox Roles
	    B. Microsoft Exchange Server 2016 - File Share Witness - DAG ONLY
	    C. Microsoft Exchange Server 2013 - Mailbox Server ONLY
	    D. Microsoft Exchange Server 2013 - CAS Server ONLY
	    E. MailEnable Standard Email Server (POP3 & SMTP)
	    F. Microsoft Exchange Server 2016 - Import and Export Server

	    N. Microsoft Exchange - Create Exchange Failover Cluster
	    O. Microsoft Exchange - Configure Outlook Anywhere
	    P. Microsoft Exchange - Configure HTTP to HTTPS Redirection
	    Q. Microsoft Exchange - Extend the Active Directory Schema
	    R. Microsoft Exchange - Prepare Active Directory
	    S. Microsoft Exchange - Create ContentSubmitters Security Group
	    T. Microsoft Exchange - Create Exchange DAG
	    U. Microsoft Exchange - Add Servers to Exchange DAG
	    V. Microsoft Exchange - Apply Post Upgrade Fixes
	    W. Microsoft Exchange - Fix Failed Database Content Indexes

	    X. Exit back to Main Menu
"@
	$menuOptions = '^[a-en-x]+$'

			Write-Host $menu -ForegroundColor Cyan
			$choice7 = Read-Host "`n`tEnter Option From Above Menu"
			$ok = $choice7 -match $menuOptions
		
			if ( -not $ok) { Write-Host "Invalid selection" ; start-Sleep -Seconds 2 }
		} until ( $ok )
		
		switch -Regex ( $choice7 ) {
			"A" { dInstallConfirmDomain "Microsoft Exchange Server 2016 - CAS & Mailbox Roles" $function:MainMenu_Option_7_A }
			"B" { dInstallConfirmDomain "Microsoft Exchange Server 2016 - File Share Witness - DAG ONLY" $function:MainMenu_Option_7_B }
			"C" { dInstallConfirmDomain "Microsoft Exchange Server 2013 - Mailbox Server ONLY" $function:MainMenu_Option_7_C }
			"D" { dInstallConfirmDomain "Microsoft Exchange Server 2013 - CAS Server ONLY" $function:MainMenu_Option_7_D }
			"E" { dInstallConfirm "MailEnable Standard Email Server" $function:MainMenu_Option_7_E }
			"F" { dInstallConfirmDomain "Microsoft Exchange Server 2016 - PST Import Server" $function:MainMenu_Option_7_F }
			"N" { dInstallConfirmDomain "Microsoft Exchange - Create Exchange Failover Cluster" $function:MainMenu_Option_7_N }
			"O" { MainMenu_Option_7_O }
			"P" { dInstallConfirmDomain "Microsoft Exchange - Configure HTTP to HTTPS Redirection" $function:MainMenu_Option_7_P }
			"Q" { dInstallConfirmDomain "Microsoft Exchange - Extend the Active Directory Schema" $function:MainMenu_Option_7_Q }
			"R" { dInstallConfirmDomain "Microsoft Exchange - Prepare Active Directory" $function:MainMenu_Option_7_R }
			"S" { dInstallConfirmDomain "Microsoft Exchange - Create ContentSubmitters Security Group" $function:MainMenu_Option_7_S }
			"T" { dInstallConfirmDomain "Microsoft Exchange - Create Exchange DAG" $function:MainMenu_Option_7_T }
			"U" { dInstallConfirmDomain "Microsoft Exchange - Add Servers to Exchange DAG" $function:MainMenu_Option_7_U }
			"V" { dInstallConfirmDomain "Microsoft Exchange - Post Upgrade Fixes" $function:MainMenu_Option_7_V }
			"W" { dInstallConfirmDomain "Microsoft Exchange - Fix Failed Database Content Indexes" $function:MainMenu_Option_7_W }
		}
	} until ( $choice7 -match "X" )
}

####################################################################################################################################################################################
# Main Menu - Option 7 - Sub Menu A - Install Microsoft Exchange 2016 for SolidCP
Function MainMenu_Option_7_A() { # Function to call multiple functions to install Microsoft Exchange 2016 MBX Role Requirements
	CheckSolidCPdomainUser "Server"
	dCheckIPaddressesSet
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	Write-Host "Installing the 'Microsoft Exchange 2016 Server Role (Mailbox and CAS)'" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	Exchange2016_PreCheck
	InstallCommonFeatures
	InstallIISforSolidCP
	ExchangeMBX2016
	dInstallConfirmNoCLS "SolidCP Installer Application" $function:InstallSolidCPInstaller  # Prompt to download and install the SOlidCP Installation Application
	InstallSolidCPcomponent -Server -NoIP                                                   # Install the Server component ONLY of the installer is installed
	Exchange2016_PostCheck
	HardenIIS_SSL
	EnableWindowsUpdates
	Write-Host "`n`t You must reboot this machine before you can use this Exchange Server" -ForegroundColor Yellow
	dRebootConfirm
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 7 - Sub Menu B - Install Microsoft Exchange File Share Witness (DAG ONLY Role) for SolidCP
Function MainMenu_Option_7_B() { # Function to call multiple functions to install Microsoft Exchange File Share Witness - DAG ONLY Role Requirements
	dCheckIPaddressesSet
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	Write-Host "Installing the 'Microsoft Exchange (File Share Witness - DAG ONLY)'" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	InstallExchangeFSW
	EnableWindowsUpdates
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 7 - Sub Menu C - Install Microsoft Exchange 2013 CAS Role for SolidCP
Function MainMenu_Option_7_C() { # Function to call multiple functions to install Microsoft Exchange CAS Role Requirements
	CheckSolidCPdomainUser "Server"
	dCheckIPaddressesSet
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	Write-Host "Installing the 'Microsoft Exchange (CAS Server Role)'" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	InstallIISforSolidCP
	InstallExchangeCAS
	InstallSolidCPInstaller
	InstallSolidCPcomponent -Server
	HardenIIS_SSL
	EnableWindowsUpdates
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 7 - Sub Menu D - Install Microsoft Exchange 2013 Mailbox Role for SolidCP
Function MainMenu_Option_7_D() { # Function to call multiple functions to install Microsoft Exchange Mailbox Role Requirements
	dCheckIPaddressesSet
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	Write-Host "Installing the 'Microsoft Exchange (Mailbox Server Role)'" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	InstallIISforSolidCP
	InstallExchangeMBX
	HardenIIS_SSL
	EnableWindowsUpdates
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 7 - Sub Menu E - Install MailEnable Email Server for SolidCP
Function MainMenu_Option_7_E() { # Function to call multiple functions to install Install MailEnable Email Server
	CheckSolidCPdomainUser "Server"
	dCheckIPaddressesSet
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	Write-Host "Installing the 'Install MailEnable Email Server'" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	InstallIISforSolidCP
	InstallMailEnable
	InstallSolidCPInstaller
	InstallSolidCPcomponent -Server
	HardenIIS_SSL
	EnableWindowsUpdates
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 7 - Sub Menu F - Install Microsoft Exchange 2016 for SolidCP
Function MainMenu_Option_7_F() { # Function to call multiple functions to install Microsoft Exchange 2016 MBX Role Requirements
	dCheckIPaddressesSet
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	Write-Host "Installing the 'Microsoft Exchange 2016 Server Role (Mailbox and CAS)'" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	InstallIISforSolidCP
	HardenDotNETforIIS
	HardenFilePermissions
	InstallDisablePasswdComplex
	Exchange2016PreReqMgmtTools
	InstallDotNET452
	Exchange2016InstallTools
	Exchange2016ImportExportSvr
	HardenIIS_SSL
	EnableWindowsUpdates
	Write-Host "`n`t You must reboot this machine before you can use this Exchange Server" -ForegroundColor Yellow
	dRebootConfirm
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 7 - Sub Menu N - Create a new Failover Cluster for Exchange 2016
Function MainMenu_Option_7_N() { # Function to Create a new Failover Cluster for Exchange 2016
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	Write-Host "'Microsoft Exchange 2016 Server' Create a new Failover Cluster" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	CreateNewExchangeFailoverCluster
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 7 - Sub Menu O - Configure Outlook Anywhere for Exchange 2016 on this server
Function MainMenu_Option_7_O() { # Function to configure Outlook Anywhere for Exchange 2016
	do {
		do {
		cls
			Write-Host "`n`t'Microsoft Exchange 2016 Server' Configure Outlook Anywhere" -ForegroundColor Magenta
			Write-Host "`t  Please select the Outlook Anywhere deployment you want to install`n`n" -ForegroundColor Cyan
	$menu = @"
	    A. Configure Outlook Anywhere on this server ONLY
	    B. Configure Outlook Anywhere on ALL Exchange Servers

	    X. Exit back to Main Menu
"@
	$menuOptions = '^[a-bx]+$'

			Write-Host $menu -ForegroundColor Cyan
			$choice7O = Read-Host "`n`tEnter Option From Above Menu"
			$ok = $choice7O -match $menuOptions
		
			if ( -not $ok) { Write-Host "Invalid selection" ; start-Sleep -Seconds 2 }
		} until ( $ok )
		
		switch -Regex ( $choice7O ) {
			"A" {
				Exchange2016_Set_OOA
				Restart_IIS
				dPressAnyKeyToExit
			}
			"B" {
				Exchange2016_Set_OOA -All
				Restart_IIS
				dPressAnyKeyToExit
			}
		}
	} until ( $choice7O -match "X" )
}

####################################################################################################################################################################################
# Main Menu - Option 7 - Sub Menu P - Configure the HTTP to HTTPS Redirection for Exchange 2016 on this server
Function MainMenu_Option_7_P() { # Function to configure the HTTP to HTTPS Redirection for Exchange 2016
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	Write-Host "'Microsoft Exchange 2016 Server' Configure HTTP to HTTPS Redirection for Exchange 2016" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	CreateExch2016_HttpToHttps403
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 7 - Sub Menu Q - Extend the Active Directory Schema for Exchange 2016 installation
Function MainMenu_Option_7_Q() { # Function to Extend the Active Directory Schema
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	Write-Host "'Microsoft Exchange 2016 Server' Extend the Active Directory Schema for Exchange 2016 installation" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	Exchange2016PrepareSchema
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 7 - Sub Menu R - Prepare Active Directory for Microsoft Exchange Server for Exchange 2016
Function MainMenu_Option_7_R() { # Function to Prepare Active Directory for Microsoft Exchange Server
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	Write-Host "'Microsoft Exchange 2016 Server' Prepare Active Directory for Microsoft Exchange Server" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	Exchange2016PrepareAD
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 7 - Sub Menu S - Create Exchange ContentSubmitters Security Group for Exchange 2016
Function MainMenu_Option_7_S() { # Function to Create Exchange ContentSubmitters Security Group
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	Write-Host "'Microsoft Exchange 2016 Server' Create Exchange ContentSubmitters Security Group" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	CreateExchangeContentSubmittersGroup
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 7 - Sub Menu T - Create a new Database Availability Group for Exchange 2016
Function MainMenu_Option_7_T() { # Function to Create a new DAG for Exchange 2016
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	Write-Host "'Microsoft Exchange 2016 Server' Create a new Database Avilability Group (DAG)" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	CreateExchangeContentSubmittersGroup
	CreateNewDAG
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 7 - Sub Menu U - Add Exchange 2016 Servers to existing Database Availability Group
Function MainMenu_Option_7_U() { # Function to Add Exchange 2016 Servers to existing DAG
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	Write-Host "Add 'Microsoft Exchange 2016 Server' to existing Database Avilability Group (DAG)" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	CreateExchangeContentSubmittersGroup
	AddDAGMember
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 7 - Sub Menu V - Check Microsoft Exchange 2016 after Service Pack or Cumulative Updates
Function MainMenu_Option_7_V() { # Function to check the Microsoft Exchange 2016 Server after doing Exchange Updates
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	Write-Host "Checking 'Microsoft Exchange 2016 Server' after Windows or Exchange Updates" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	Exchange2016_UpgradeCheck
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 7 - Sub Menu W - Microsoft Exchange - Fix Failed Database Content Indexes for SolidCP
Function MainMenu_Option_7_W() { # Function to call multiple functions to Microsoft Exchange - Fix Failed Database Content Indexes
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	Write-Host "Microsoft Exchange - Fixing the Failed Database Content Indexes" -ForegroundColor Magenta
	Write-Host "`tNothing done, this will be added soon" -ForegroundColor Green ; start-Sleep -Seconds 2
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 8 - Present Sub Menu for Microsoft Remote Desktop Services deployment types
Function MainMenu_Option_8() { # Function to show the user a Sub Menu for the Microsoft Exchange Server deployment options
	do {
		do {
		cls
			Write-Host "`n`tSolidCP Setup Menu - Remote Desktop Services`n" -ForegroundColor Magenta
			Write-Host "`t  Please select the Microsoft Remote Desktop Services deployment you want to install`n`n" -ForegroundColor Cyan
	$menu = @"
	    A. RD Connection Broker
	    B. RD Licencing Server
	    C. RD Gateway
	    D. RD Web Access
	    E. RD Session Host

	    W. HTTP to HTTPS Redirection for RD Web Access

	    X. Exit back to Main Menu
"@
	$menuOptions = '^[a-eWx]+$'

			Write-Host $menu -ForegroundColor Cyan
			$choice8 = Read-Host "`n`tEnter Option From Above Menu"
			$ok = $choice8 -match $menuOptions
		
			if ( -not $ok) { Write-Host "Invalid selection" ; start-Sleep -Seconds 2 }
		} until ( $ok )
		
		switch -Regex ( $choice8 ) {
			"A" { dInstallConfirm "Microsoft Remote Desktop Services - RD Connection Broker" $function:MainMenu_Option_8_A }
			"B" { dInstallConfirm "Microsoft Remote Desktop Services - RD Licencing Server" $function:MainMenu_Option_8_B }
			"C" { dInstallConfirm "Microsoft Remote Desktop Services - RD Gateway" $function:MainMenu_Option_8_C }
			"D" { dInstallConfirm "Microsoft Remote Desktop Services - RD Web Access" $function:MainMenu_Option_8_D }
			"E" { dInstallConfirm "Microsoft Remote Desktop Services - RD Session Host" $function:MainMenu_Option_8_E }
			"W" { dInstallConfirm "HTTP to HTTPS Redirection for RD Web Access" $function:MainMenu_Option_8_W }
		}
	} until ( $choice8 -match "X" )
}

####################################################################################################################################################################################
# Main Menu - Option 8 - Sub Menu A - Install Microsoft Remote Desktop Services RD Connection Broker and RD Web Access Requirements for SolidCP
Function MainMenu_Option_8_A() { # Function to call multiple functions to install Microsoft Remote Desktop Services RD Connection Broker and RD Web Access
	dCheckIPaddressesSet
	dCheckRDSconnectionBroker
	dCheckRDGatewayFQDN
	CLS
	Write-Host "`n`n`n`n`n`n`n`n`n`n`n`n`n`n`n`n"  # This brings the text down by 16 lines to allow fo the Windows Features installation overlay
	Write-Host "Installing the 'Microsoft Remote Desktop - RD Connection Broker and RD Web Access'" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	CreateRDSConnectionBrokersGroup
	InstallSQLsvr2012NativeClnt
	InstallSQLsvr2012PowerShellTools
	HardenDotNETforIIS
	#HardenFilePermissions
	HardenIIS_SSL
	InstallRDSconnectionBroker
	EnableWindowsUpdates
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 8 - Sub Menu B - Install Microsoft Remote Desktop Services Licencing Server Requirements for SolidCP
Function MainMenu_Option_8_B() { # Function to call multiple functions to install Microsoft Remote Desktop Services Licencing Server Requirements
	dCheckIPaddressesSet
	dCheckRDSconnectionBroker
	dCheckRDGatewayFQDN
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	dCheckMsSQLpassword
	Write-Host "Installing the 'Microsoft Remote Desktop - RD Licencing Server'" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	InstallSQLsvr2014withTools -Password $dMsSQLpassword
	HardenDotNETforIIS
	#HardenFilePermissions
	HardenIIS_SSL
	InstallRDLicencingServer
	EnableWindowsUpdates
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 8 - Sub Menu C - Install Microsoft Remote Desktop Services RD Gateway Server Requirements for SolidCP
Function MainMenu_Option_8_C() { # Function to call multiple functions to install Microsoft Remote Desktop Services RD Gateway
	dCheckIPaddressesSet
	dCheckRDSconnectionBroker
	dCheckRDGatewayFQDN
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	Write-Host "Installing the 'Microsoft Remote Desktop - RD Gateway'" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	HardenDotNETforIIS
	#HardenFilePermissions
	HardenIIS_SSL
	InstallRDGateway
	InstallSolidCPInstaller
	InstallSolidCPcomponent -Server
	EnableWindowsUpdates
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 8 - Sub Menu D - Install Microsoft Remote Desktop Services RD Web Access Server Requirements for SolidCP
Function MainMenu_Option_8_D() { # Function to call multiple functions to install Microsoft Remote Desktop Services RD Web Access
	dCheckIPaddressesSet
	dCheckRDSconnectionBroker
	dCheckRDGatewayFQDN
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	Write-Host "Installing the 'Microsoft Remote Desktop - RD Web Access'" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	HardenDotNETforIIS
	#HardenFilePermissions
	HardenIIS_SSL
	InstallRDWebAccess
	EnableWindowsUpdates
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 8 - Sub Menu E - Install Microsoft Remote Desktop Services RD Session Host Server Requirements for SolidCP
Function MainMenu_Option_8_E() { # Function to call multiple functions to install Microsoft Remote Desktop Services RD Session Host
	dCheckIPaddressesSet
	dCheckRDSconnectionBroker
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	Write-Host "Installing the 'Microsoft Remote Desktop - RD Session Host'" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	HardenDotNETforIIS
	#HardenFilePermissions
	HardenIIS_SSL
	InstallRDSessionHost
	EnableWindowsUpdates
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 8 - Sub Menu W - Configure the HTTP to HTTPS Redirection for the RD Web Access Server
Function MainMenu_Option_8_W() { # Function to call multiple functions to Configure the HTTP to HTTPS Redirection for the RD Web Access Server
	dCheckIPaddressesSet
	dCheckRDSconnectionBroker
	dCheckRDGatewayFQDN
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	Write-Host "Configuring the 'HTTP to HTTPS Redirection' for the RD Web Access role" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	ConfigureRDwebHTTPtoHTTPSredirect
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option 9 - Install SolidCP Cloud Storage Portal
Function MainMenu_Option_9() { # Function to call multiple functions to install Cloud Storage Portal
	dCheckIPaddressesSet
	CLS
	Write-Host "`n`n`n`n`n`n`n`n"  # This brings the text down by 8 lines to allow fo the Windows Features installation overlay
	Write-Host "Installing the 'Cloud Storage Portal'" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallCommonFeatures
	InstallIISforSolidCP
	InstallWebDavFeatures
	InstallSolidCPInstaller
	CheckWebDavStorageFQDN -FQDN
	InstallSolidCPcomponent -WebDav
	InstallWebDavStorageRootWebsite
	InstallSolidCPcomponent -Server
	HardenIIS_SSL
	EnableWindowsUpdates
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option T - Present Sub Menu for Tools
Function MainMenu_Option_T() { # Function to show the user a Sub Menu for the Tools offered by SolidCP
	do {
		do {
		cls
			Write-Host "`n`tSolidCP Setup Menu - Tools`n" -ForegroundColor Magenta
			Write-Host "`t  Please select the required Tool you want to run on this machine`n`n" -ForegroundColor Cyan
	$menu = @"
	    A. Harden File Permissions
	    B. Harden Microsoft .NET
	    C. Fix SSL Issues for Microsoft IIS
	    D. Install LetsEncrypt for Microsoft IIS

	    X. Exit back to Main Menu
"@
	$menuOptions = '^[a-dx]+$'

			Write-Host $menu -ForegroundColor Cyan
			$choice8 = Read-Host "`n`tEnter Option From Above Menu"
			$ok = $choice8 -match $menuOptions
		
			if ( -not $ok) { Write-Host "Invalid selection" ; start-Sleep -Seconds 2 }
		} until ( $ok )
		
		switch -Regex ( $choice8 ) {
			"A" { dInstallConfirm "Harden File Permissions" $function:MainMenu_Option_T_A }
			"B" { dInstallConfirm "Harden Microsoft .NET" $function:MainMenu_Option_T_B }
			"C" { dInstallConfirm "Fix SSL Issues for Microsoft IIS" $function:MainMenu_Option_T_C }
			"D" { dInstallConfirm "Install LetsEncrypt for Microsoft IIS" $function:MainMenu_Option_T_D }
		}
	} until ( $choice8 -match "X" )
}

####################################################################################################################################################################################
# Main Menu - Option T - Sub Menu A - Harden the File Permissions
Function MainMenu_Option_T_A() { # Function to call multiple functions to harden the file permissions on this machine
	CLS
	Write-Host "SolifCP - Harden the File Permissions on this machine" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	HardenFilePermissions
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option T - Sub Menu B - Harden Microsoft .NET
Function MainMenu_Option_T_B() { # Function to call multiple functions to Harden Microsoft .NET
	CLS
	Write-Host "SolifCP - Harden Microsoft .NET" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	HardenDotNETforIIS
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option T - Sub Menu C - Fix SSL Issues for Microsoft IIS
Function MainMenu_Option_T_C() { # Function to call multiple functions to fixFix SSL Issues for Microsoft IIS
	CLS
	Write-Host "SolifCP - Fix SSL Issues for Microsoft IIS" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	HardenIIS_SSL
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option T - Sub Menu D - Install LetsEncrypt for Microsoft IIS
Function MainMenu_Option_T_D() { # Function to call multiple functions to Install LetsEncrypt for Microsoft IIS
	CLS
	Write-Host "SolifCP - Install LetsEncrypt for Microsoft IIS" -ForegroundColor Magenta
	Write-Host "`tStarting server setup for SolidCP`n" -ForegroundColor Gray
	InstallLetsEncryptACMESharp
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Main Menu - Option X - Exit the SolidCP Menu
Function MainMenu_Option_X() { # Function to Exit this menu
	CLS
	Write-Host "`n`t ******************************" -ForegroundColor Green
	Write-Host "`t *                            *" -ForegroundColor Green
	Write-Host "`t *          Quitting          *" -ForegroundColor Green
	Write-Host "`t *                            *" -ForegroundColor Green
	Write-Host "`t *    NO Changes were made    *" -ForegroundColor Green
	Write-Host "`t *                            *" -ForegroundColor Green
	Write-Host "`t ******************************" -ForegroundColor Green
	dPressAnyKeyToExit
}

####################################################################################################################################################################################
# Functions for the Main Menu Options are above here
####################################################################################################################################################################################
####################################################################################################################################################################################



########################################################################################################################################################################################################
# General Functions start below here #
######################################


####################################################################################################################################################################################
Function dInstallConfirm($dInstallName, $sFunctionToRun)        # Function to prompt for confirmation to install application called from a function
{ # Usage - dInstallConfirm "SQL Server" $function:dTestScript
	CLS
	$choice = ""
	while ($choice -notmatch "[y|n]") { CLS ; $choice = read-host "`n Do you want to install $dInstallName`? (Y/N)" }
	if ($choice -eq "y") {
		$($sFunctionToRun.Invoke())
		Write-Host "`t $dInstallName has been installed" -ForegroundColor Green ; start-Sleep -Seconds 2
	} else {
		write-host "`t $dInstallName installation cancelled by user" -ForegroundColor Red    ; start-Sleep -Seconds 2
	}
}


####################################################################################################################################################################################
Function dInstallConfirmNoCLS($dInstallName, $sFunctionToRun)   # Function to prompt for confirmation to install application called from a function without cleaning the screen
{ # Usage - dInstallConfirmNoCLS "SQL Server" $function:dTestScript
	$choice = ""
	while ($choice -notmatch "[y|n]") { $choice = read-host "`n Do you want to install $dInstallName`? (Y/N)" }
	if ($choice -eq "y") {
		$($sFunctionToRun.Invoke())
		Write-Host "`t $dInstallName has been installed" -ForegroundColor Green ; start-Sleep -Seconds 2
	} else {
		write-host "`t $dInstallName installation cancelled by user" -ForegroundColor Red    ; start-Sleep -Seconds 2
	}
}


####################################################################################################################################################################################
Function dInstallConfirmDomain($dInstallName, $sFunctionToRun)  # Function to prompt for confirmation to install application called from a function if Domain Checking is required
{ # Usage - dInstallConfirmDomain "Exchange 2016" $function:dTestScript
	CLS
	if (!$dDomainMember) {
		Write-Host "`n`t *************************************************" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *   This machine is NOT a member of a domain    *" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *   This server MUST be joined to a domain and  *" -ForegroundColor Yellow
		Write-Host "`t *    you need to login with a domain account    *" -ForegroundColor Yellow
		Write-Host "`t *  that is a member of the Domain Admins group  *" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *    Please join this server to a domain and    *" -ForegroundColor Yellow
		Write-Host "`t *      log back on with an account that is      *" -ForegroundColor Yellow
		Write-Host "`t *      a member of the Domain Admins group      *" -ForegroundColor Yellow
		Write-Host "`t *           and run this script again           *" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *************************************************" -ForegroundColor Yellow
	}elseif ( ($dDomainMember) -and (!(CheckGroupMembers "$dLangDomainAdminsGroup" "$dLoggedInUserName" "Domain")) ) {
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
	}else{
		$choice = ""
		while ($choice -notmatch "[y|n]") { CLS ; $choice = read-host "`n`tDo you want to install $dInstallName`? (Y/N)" }
		if ($choice -eq "y") {
			$($sFunctionToRun.Invoke())
			Write-Host "`t $dInstallName has been installed" -ForegroundColor Green ; start-Sleep -Seconds 2
		} else {
			write-host "`t $dInstallName installation cancelled by user" -ForegroundColor Red ; start-Sleep -Seconds 2
		}
	}
}


####################################################################################################################################################################################
Function dRebootConfirm()                                   # Function to prompt for confirmation to reboot this machine
{
	$choice = ""
	while ($choice -notmatch "[y|n]") { $choice = read-host "`n`tDo you want to reboot this machine`? (Y/N)" }
	if ($choice -eq "y") {
		Write-Host "`n`t Rebooting this machine" -ForegroundColor Yellow ; start-Sleep -Seconds 2
		Restart-Computer
	} else {
		write-host "`n`t You MUST Reboot this machine" -ForegroundColor Red ; start-Sleep -Seconds 2
	}
}


####################################################################################################################################################################################
Function dCheckMsSQLpassword()                              # Function to prompt for confirmation to view the password for the Microsoft SQL Server that have just been entered
{
	if (!$dMsSQLpassword) { # Ask for a password if none has been specified for the MariaDB or MySQL installation
		Write-Host "`t*************************************************" -ForegroundColor Red
		Write-Host "`t*                                               *" -ForegroundColor Red
		Write-Host "`t*          YOUR ATTENTION IS REQUIRED!          *" -ForegroundColor Red
		Write-Host "`t*                                               *" -ForegroundColor Red
		Write-Host "`t*     You need to enter a password for your     *" -ForegroundColor Red
		Write-Host "`t*              Microsft SQL Server              *" -ForegroundColor Red
		Write-Host "`t*                                               *" -ForegroundColor Red
		Write-Host "`t*************************************************" -ForegroundColor Red
		$responseMsSQL = Read-host "`n`tEnter the SA password you want to use for your Microsoft SQL Server" -AsSecureString
		$script:dMsSQLpassword = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($responseMsSQL))

		# if a password has been entered ask the user if they want to view it
		$choiceCheckMsSQLpassword = ""
		while ($choiceCheckMsSQLpassword -notmatch "[y|n]") {
			$choiceCheckMsSQLpassword = read-host "`tDo you want to view the SA Password you have entered for your Microsoft SQL Server? (Y/N)"
		}
		if ($choiceCheckMsSQLpassword -eq "y") {
			Write-Host "`n The SA Password entered for your Microsoft SQL Server was - $dMsSQLpassword" -ForegroundColor Green
			dPressAnyKeyToContinue
			CLS
		}
	}
}


####################################################################################################################################################################################
Function dCheckMariaMySQLpassword()                         # Function to check the password for the MariaDB or MySQL Server that have just been entered, if not ask for it
{
	if (!$dMySQLvMariaDB) { # Check if MariaDB or MySQL has been specified, if not ask which one needs to be installed
		$choiceCheckMariaMySQL = ""
		while ($choiceCheckMariaMySQL -notmatch "[1|2]") { # Present the user with a menu to choose MariaDB or MySQL
			CLS
			Write-Host "`n`tPlease select the Database Type you want to install`n" -ForegroundColor Cyan
			Write-Host "`t 1. MariaDB Database Server" -ForegroundColor Cyan
			Write-Host "`t 2. MySQL Database Server" -ForegroundColor Cyan
			$choiceCheckMariaMySQL = read-host "`n`tEnter Option From Above Menu" 
		}
		if ($choiceCheckMariaMySQL -eq "1") { # Options for MariaDB
			$script:dMySQLvMariaDB = "MariaDB"
			Write-Host "`t The MariaDB Database will be installed on this machine" -ForegroundColor Green
		}
		if ($choiceCheckMariaMySQL -eq "2") { # Options for MariaDB
			$script:dMySQLvMariaDB = "MySQL"
			Write-Host "`t The MySQL Database will be installed on this machine" -ForegroundColor Green
		}
	}

	if (!$dMariaMySQLpasswd) { # Ask for a password if none has been specified for the MariaDB or MySQL installation
		if     ($dMySQLvMariaDB -eq "MariaDB") { $dMariaMySQLTxt = " MariaDB Server" } # Set Attention text for MariaDB
		elseif ($dMySQLvMariaDB -eq "MySQL"  ) { $dMariaMySQLTxt = "  MySQL Server " } # Set Attention text for MySQL
		Write-Host "`t*************************************************" -ForegroundColor Red
		Write-Host "`t*                                               *" -ForegroundColor Red
		Write-Host "`t*          YOUR ATTENTION IS REQUIRED!          *" -ForegroundColor Red
		Write-Host "`t*                                               *" -ForegroundColor Red
		Write-Host "`t*     You need to enter a password for your     *" -ForegroundColor Red
		Write-Host "`t*                $dMariaMySQLTxt                *" -ForegroundColor Red
		Write-Host "`t*                                               *" -ForegroundColor Red
		Write-Host "`t*************************************************" -ForegroundColor Red
		$responseMySQL = Read-host "`n`tEnter the ROOT password you want to use for your $dMySQLvMariaDB Server" -AsSecureString
		$script:dMariaMySQLpasswd = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($responseMySQL))

		# if a password has been entered ask the user if they want to view it
		$choiceCheckMariaMySQLpassword = ""
		while ($choiceCheckMariaMySQLpassword -notmatch "[y|n]") {
			$choiceCheckMariaMySQLpassword = read-host "`tDo you want to view the ROOT Password you have just entered for your $dMySQLvMariaDB Server? (Y/N)"
		}
		if ($choiceCheckMariaMySQLpassword -eq "y") {
			Write-Host "`t The ROOT Password entered for your $dMySQLvMariaDB Server was - $dMariaMySQLpasswd" -ForegroundColor Green
			dPressAnyKeyToContinue
			CLS
		}
	}
}


####################################################################################################################################################################################
Function dCheckIPaddressesSet() {                            # Function to check if the IP Addresses have been set at the top of this script
	if (!$dSolidCPEnterpriseSvrIP) { # If the Enterprise Server IP Address has not been set ask the user for one
		do {
			cls
			Write-Host "`n`t *************************************************" -ForegroundColor Yellow
			Write-Host "`t *                                               *" -ForegroundColor Yellow
			Write-Host "`t *     You have not specified the IP Address     *" -ForegroundColor Yellow
			Write-Host "`t *      for your SolidCP Enterprise Server!      *" -ForegroundColor Yellow
			Write-Host "`t *                                               *" -ForegroundColor Yellow
			Write-Host "`t *      You can set this at the top of this      *" -ForegroundColor Yellow
			Write-Host "`t *           script before running it.           *" -ForegroundColor Yellow
			Write-Host "`t *                                               *" -ForegroundColor Yellow
			Write-Host "`t *   It is a requirement to open the firewall    *" -ForegroundColor Yellow
			Write-Host "`t *   for UNC access from the Enterprise Server   *" -ForegroundColor Yellow
			Write-Host "`t *     on each server if you want to use the     *" -ForegroundColor Yellow
			Write-Host "`t *    Upgrade feature from the SolidCP Portal    *" -ForegroundColor Yellow
			Write-Host "`t *        or from the SolidCP PowerShell         *" -ForegroundColor Yellow
			Write-Host "`t *              Auto-Upgrade Script              *" -ForegroundColor Yellow
			if (!$dSCPfirewallUNCshareIPs) {
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *      We would also reccomend setting the      *" -ForegroundColor Yellow
				Write-Host "`t *           `$dSCPfirewallUNCshareIPs            *" -ForegroundColor Yellow
				Write-Host "`t *   with any additional IP Addresses that you   *" -ForegroundColor Yellow
				Write-Host "`t *    would like to access the UNC path from     *" -ForegroundColor Yellow
			}
			if (!$dSCPfirewallRDPaccess) {
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *     You can also specify IP Addresses in      *" -ForegroundColor Yellow
				Write-Host "`t *            `$dSCPfirewallRDPaccess             *" -ForegroundColor Yellow
				Write-Host "`t *    so you can access this server remotely     *" -ForegroundColor Yellow
				Write-Host "`t *              via Remote Desktop               *" -ForegroundColor Yellow
			}
			Write-Host "`t *                                               *" -ForegroundColor Yellow
			Write-Host "`t *************************************************" -ForegroundColor Yellow

			$choice = Read-Host "`n`tDo you want to enter the IP Address of your SolidCP Enterprise Server`? (Y/N)"
			$ok = $choice -match '[y|n]'

			if ( -not $ok) { Write-Host "`t Invalid selection" -ForegroundColor Yellow ; start-Sleep -Seconds 1 }
		} until ( $ok )

		switch -Regex ( $choice ) {
			"Y" {
				do { # Ask for the Enterprise Server IP Address (can be single IP or in CIDR format)
					$ok_EntSvrIP = ""
					$script:dSolidCPEnterpriseSvrIP = Read-Host "`n`tPlease enter the SolidCP Enterprise Server IPV4 Address"
					$ok_EntSvrIP = CheckIP -IPV4 "$dSolidCPEnterpriseSvrIP"
					if ( -not $ok_EntSvrIP) { Write-Host "`t Invalid selection" -ForegroundColor Yellow ; start-Sleep -Seconds 1 }
				} until ( $ok_EntSvrIP )
				Write-Host "`t You entered `"$dSolidCPEnterpriseSvrIP`"" -ForegroundColor Green
			}
			"N" { Write-Host "`n`tYou will need to manually set the firewall rules to allow UNC Access to this machine from your Enterprise Server" -ForegroundColor Yellow }
		}
	}
	if (!$dSCPfirewallRDPaccess) { # If the Remote Desktop access IP Address has not been set ask the user for one
		do { # Ask if user wants to allow Remote Desktop access to this machine
			$choice_UNC = Read-Host "`n`tDo you want to allow Remote Desktop Access to this server`? (Y/N)"
			$ok_UNC = $choice_UNC -match '[y|n]'
			if ( -not $ok_UNC) { Write-Host "`t Invalid selection" -ForegroundColor Yellow ; start-Sleep -Seconds 1 }
		} until ( $ok_UNC )

		switch -Regex ( $choice_UNC ) {

			"Y" {
				do { # Ask for the IP Address (can be single IP or in CIDR format) that RDP access will be allowed from
					$ok_RDP_IP = ""
					$script:dSCPfirewallRDPaccess = Read-Host "`n`tPlease enter the IPV4 Address to allow RDP access from"
					$ok_RDP_IP = CheckIP -IPV4 "$dSCPfirewallRDPaccess"
					if ( -not $ok_RDP_IP) { Write-Host "`tInvalid selection" -ForegroundColor Yellow ; start-Sleep -Seconds 1 }
				} until ( $ok_RDP_IP )
				Write-Host "`t You entered `"$dSCPfirewallRDPaccess`"" -ForegroundColor Green
			}

			"N" { Write-Host "`n`tYou will need to manually set the firewall rules to allow RDP Access to this machine" -ForegroundColor Yellow }
		}
	}
	if ($dSolidCPEnterpriseSvrIP) {ValidateIP -IPaddress "$dSolidCPEnterpriseSvrIP"} # Check that the Enterprise Server IP Address is valid
	if ($dSCPfirewallUNCshareIPs) {ValidateIP -IPaddress "$dSCPfirewallUNCshareIPs"} # Check that the UNC Share IP Address(es) are valid
	if ($dSCPfirewallRDPaccess)   {ValidateIP -IPaddress "$dSCPfirewallRDPaccess"}   # Check that the Remote Desktop IP Address(es) are valid
}


####################################################################################################################################################################################
Function dPressAnyKeyToExit()                               # Function to press any key to exit
{
	if ($psISE) { # Check if running Powershell ISE
		Add-Type -AssemblyName System.Windows.Forms
		[System.Windows.Forms.MessageBox]::Show("Press any key to exit")
		exit
	}else{
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
Function Get-OSName()                                       # Function to get the Operating System Name and check against an input string
{
	Param(
		[string]$Check       # Check if the OS Version matches this parameter
	)
	if ($Check) {
		(((Get-WmiObject Win32_OperatingSystem).Caption) -match $Check)
	}else{
		(Get-WmiObject Win32_OperatingSystem).Caption
	}
}


####################################################################################################################################################################################
Function dAskInstallPhpMyAdmin()                            # Function to ask if the user wants to install phpMyAdmin on this machine
{
	if ($dInstalPhpMyAdmin -eq "" -or $dInstalPhpMyAdmin -eq $null) { # Oly ask if the user wants to install phpMyAdmin if it has not been set globally
		$choice = ""
		while ($choice -notmatch "[y|n]") { $choice = read-host "`n Do you want to install phpMyAdmin on this machine`? (Y/N)" }
		if ($choice -eq "y") {
			$Script:dInstalPhpMyAdmin  ="$true"
			if (!($dPhpMyAdminPort)) {
				Write-Host "`t You have not defined the port you would like to use for phpMyAdmin" -ForegroundColor Yellow
				Write-Host "`t Please set the `"`$dPhpMyAdminPort`" variable at the top of this script" -ForegroundColor Yellow
			}
			if (!($dPhpMyAdminUserNm)) {
				Write-Host "`t You have not defined the phpMyAdmin Username for your Administrator User" -ForegroundColor Yellow
				Write-Host "`t Please set the `"`$dPhpMyAdminUserNm`" variable at the top of this script" -ForegroundColor Yellow
			}
			if (!($dPhpMyAdminPassWd)) {
				Write-Host "`t You have not defined the phpMyAdmin Password for your Administrator user" -ForegroundColor Yellow
				Write-Host "`t Please set the `"`$dPhpMyAdminPassWd`" variable at the top of this script" -ForegroundColor Yellow
			}
			if (!($dPhpMyAdminHostNm)) {
				Write-Host "`t You have not defined the FQDN you would like to use for phpMyAdmin" -ForegroundColor Yellow
				Write-Host "`t Please set the `"`$dPhpMyAdminHostNm`" variable at the top of this script" -ForegroundColor Yellow
			}
			if ( (!($dPhpMyAdminPort)) -or (!($dPhpMyAdminUserNm)) -or (!($dPhpMyAdminPassWd)) -or (!($dPhpMyAdminHostNm)) ) {
				dPressAnyKeyToExit
			}
		}
	}
}


####################################################################################################################################################################################
Function CheckIP()                                          # Function for check if IP Address is valid (IPV4 and IPV6)
{
	Param(
		[string]$IPV4,             # IPV4 address to check (Single IP or CIDR notation)
		[string]$IPV6              # IPV6 address to check (Single IP or CIDR notation)
	)

	if ($IPV4 -and (!$IPV6)) { # If the IPV4 address has been specified, check to make sure it is valid
		$IPV4 -match '^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])(\/([1-9]|[1-2][0-9]|3[0-2]))?$'
	}elseif ($IPV6 -and (!$IPV4)) { # If the IPV6 address has been specified, check to make sure it is valid
		$IPV6 -match '^s*((([0-9A-Fa-f]{1,4}:){7}([0-9A-Fa-f]{1,4}|:))|(([0-9A-Fa-f]{1,4}:){6}(:[0-9A-Fa-f]{1,4}|((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3})|:))|(([0-9A-Fa-f]{1,4}:){5}(((:[0-9A-Fa-f]{1,4}){1,2})|:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3})|:))|(([0-9A-Fa-f]{1,4}:){4}(((:[0-9A-Fa-f]{1,4}){1,3})|((:[0-9A-Fa-f]{1,4})?:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){3}(((:[0-9A-Fa-f]{1,4}){1,4})|((:[0-9A-Fa-f]{1,4}){0,2}:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){2}(((:[0-9A-Fa-f]{1,4}){1,5})|((:[0-9A-Fa-f]{1,4}){0,3}:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){1}(((:[0-9A-Fa-f]{1,4}){1,6})|((:[0-9A-Fa-f]{1,4}){0,4}:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:))|(:(((:[0-9A-Fa-f]{1,4}){1,7})|((:[0-9A-Fa-f]{1,4}){0,5}:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:)))(%.+)?s*(\/([1-9]|[0-9][0-9]|1[0-1][0-9]|12[0-8]))?$'
	}else{ # Otherwise return false
		$false
	}
}


####################################################################################################################################################################################
Function ValidateIP()                                       # Function for check if IP Address is valid (IPV4 and IPV6) - Display error and exit if there is an issue
{
	Param(
		[string]$IPaddress         # IP Aaddress to check (Single IP or CIDR notation) - IPV4 or IPV6
	)

	if ( ($IPaddress -match '\.') -and ($IPaddress -notmatch '\:') ) { # Check the string for an IPV4 address
		foreach ( $IP4 in ($IPaddress.Replace(' ','').Split(';')) ) {
			if (!(CheckIP -IPV4 $IP4)) {
				Write-Host "`t The IPV4 Address `"$IP4`" is invalid" -ForegroundColor Yellow
				$result = $true
			}
		}
	}elseif ( ($IPaddress -match '\:') -and ($IPaddress -notmatch '\.') ) { # Check the string for an IPV6 address
		foreach ( $IP6 in ($IPaddress.Replace(' ','').Split(';')) ) {
			if (!(CheckIP -IPV6 $IP6)) {
				#Write-Host "IPV6 = $IP"
				Write-Host "`t The IPV6 Address `"$IP6`" is invalid" -ForegroundColor Yellow
				$result = $true
			}
		}
	}elseif ( (($IPaddress -match '\:') -and ($IPaddress -match '\.')) -or (!$IPaddress) ) { # Check if an IPV4 and IPV6 address or if nothing has been entered
		Write-Host "`t You can`'t mix IPV4 and IPV6 addresses" -ForegroundColor Yellow
		$result = $true
	}
	if ($Result) { # Promot the user to press any key then exit is there has been an invalid IP Address entered
		dPressAnyKeyToExit
	}
}


####################################################################################################################################################################################
Function SolidCPFileDownload($dProduct)                     # Function to get the Download URL, Folder Name and File Name from the SolidCP Installer Site for the required file
{
	Return ($dSCPFileDownloadLinks.SelectNodes("//Feed/Files/Downloads/$dProduct"))
}


####################################################################################################################################################################################
Function EnableWindowsUpdates()                             # Function to enable Windows Updates on this machine
{
	# Enable Windows Updates (Notify ONLY) on the machine
	Write-Host "`tEnabling Windows Updates on this machine" -ForegroundColor Cyan
	Set-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update" -Name IsOOBEInProgress -value 0 -Type DWord
	Set-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update" -Name AUOptions -value 2 -Type DWord
	Set-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update" -Name IncludeRecommendedUpdates -value 1 -Type DWord
	Set-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update" -Name CachedAUOptions -value 2 -Type DWord
	(new-object -c "microsoft.update.servicemanager").addservice2("7971f918-a847-4430-9279-4a52d1efe18d",7,"") | Out-Null
	# Alert the user that they need to install ALL windows updates once the server has restarted
	Write-Host "`n`t *************************************************" -ForegroundColor Yellow
	Write-Host "`t *                                               *" -ForegroundColor Yellow
	Write-Host "`t *     Please ensure ALL Windows Updates are     *" -ForegroundColor Yellow
	Write-Host "`t *      installed before adding this server      *" -ForegroundColor Yellow
	Write-Host "`t *          to your SolidCP Deployment.          *" -ForegroundColor Yellow
	Write-Host "`t *                                               *" -ForegroundColor Yellow
	Write-Host "`t *************************************************" -ForegroundColor Yellow
}


####################################################################################################################################################################################
Function InstallCommonFeatures()                            # Function for common features on the servers
{
	# Start installing the Common Features required on the machine
	Write-Host "`tChecking the common features required for SolidCP" -ForegroundColor Cyan
	(Import-Module ServerManager) | Out-Null
	# Install .NET Framework 3.5 on the server
	(Add-WindowsFeature -Name Net-Framework-Core) | Out-Null
	# Install PowerShell (current) as well as the older v2 for compatability
	(Add-WindowsFeature -Name PowerShell, PowerShell-V2) | Out-Null
	# Install SNMP and configure it on this machine as long as the $InstallSNMPchk is set as true
	if ($InstallSNMPchk) {
		# Install and Configure SNMP on the server so we can monitor it
		(Add-WindowsFeature -Name SNMP-Service -IncludeAllSubFeature -IncludeManagementTools) | Out-Null
		if (Get-Itemproperty "HKLM:\SYSTEM\CurrentControlSet\Services\SNMP\Parameters\PermittedManagers" | Select-Object -ExpandProperty 1 -ErrorAction Stop) {
			Remove-Itemproperty -Path HKLM:\SYSTEM\CurrentControlSet\Services\SNMP\Parameters\PermittedManagers -Name 1
		}
		Set-ItemProperty -Path HKLM:\SYSTEM\CurrentControlSet\Services\SNMP\Parameters\RFC1156Agent -Name sysContact -Value "$SNMPsysContact"
		Set-ItemProperty -Path HKLM:\SYSTEM\CurrentControlSet\Services\SNMP\Parameters\RFC1156Agent -Name sysLocation -Value "$SNMPsysLocation"
		Set-ItemProperty -Path HKLM:\SYSTEM\CurrentControlSet\Services\SNMP\Parameters\RFC1156Agent -Name sysServices -Value 79 -Type DWord
		Set-ItemProperty -Path HKLM:\SYSTEM\CurrentControlSet\Services\SNMP\Parameters\ValidCommunities -Name public -Value 4 -Type DWord
	}
	# Set the Start Button to display All Apps rather than the silly start menu
	Set-ItemProperty -Path HKCU:\Software\Microsoft\Windows\CurrentVersion\Explorer\StartPage -Name MakeAllAppsDefault -Value 1 -Type DWord
	# Install the Telnet Client and TFTP Client as we might need them at a later date
	(Add-WindowsFeature -Name Telnet-Client) | Out-Null
	(Add-WindowsFeature -Name TFTP-Client) | Out-Null
	# Disable IE Enhanced Security Configuration for Administrators of this server only
	if ((Get-ItemProperty "HKLM:\SOFTWARE\Microsoft\Active Setup\Installed Components\{A509B1A7-37EF-4b3f-8CFC-4F3A74704073}").IsInstalled -ne 0 ) {
		Set-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Active Setup\Installed Components\{A509B1A7-37EF-4b3f-8CFC-4F3A74704073}" -Name IsInstalled -Value 0 -Type DWord
		Stop-Process -Name Explorer
	}
	# Configure the firewall to allow Ping in on IPV4 and IPV6
	Enable-NetFirewallRule -DisplayName "File and Printer Sharing (Echo Request - ICMPv4-In)"
	Enable-NetFirewallRule -DisplayName "File and Printer Sharing (Echo Request - ICMPv6-In)"
	# Add the firewall rule in for UNC Access to this machine from the IP Addresses specified in the user defined section unless it already exists
	if ( !(Get-NetFirewallRule | where DisplayName -EQ "Allow SolidCP UNC Share") ) {
		if ($dSCPfirewallUNCshareIPs) { $IPV4addresses_UNC = $dSCPfirewallUNCshareIPs.replace(' ','').split(';') } # Build the array of IP Addresses
		if ($dSolidCPEnterpriseSvrIP) { $IPV4addresses_UNC += "$dSolidCPEnterpriseSvrIP" }                         # Add the Enterprise Server IP Address to the array
		if ($IPV4addresses_UNC) { # If the array is not empty then create and enable the firewall rule
			(New-NetFirewallRule -DisplayName "Allow SolidCP UNC Share" -Description "Firewall rule to allow remote UNC access to the default share from the SolidCP Enterprise Server" -Protocol "TCP" -LocalPort "445" -RemoteAddress $IPV4addresses_UNC -WarningAction SilentlyContinue) | Out-Null
			(Enable-NetFirewallRule -DisplayName "Allow SolidCP UNC Share") | Out-Null
		}
	}
	# Add the firewall rule in for Remote Desktop Access to this machine from the IP Addresses specified in the user defined section
	if ($dSCPfirewallRDPaccess) { $IPV4addresses_RDP = $dSCPfirewallRDPaccess.replace(' ','').split(';') } # Build the array of IP Addresses
	if ($IPV4addresses_RDP) { # If the array is not empty then create and enable the firewall rule and enable Remote Desktop access
		(Set-NetFirewallRule -DisplayName "Remote Desktop - User Mode (TCP-In)" -RemoteAddress $IPV4addresses_RDP) | Out-Null
		(Enable-NetFirewallRule -DisplayName "Remote Desktop - User Mode (TCP-In)") | Out-Null
		set-ItemProperty -Path "HKLM:System\CurrentControlSet\Control\Terminal Server"-name "fDenyTSConnections" -Value 0  
		set-ItemProperty -Path "HKLM:System\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp" -name "UserAuthentication" -Value 1    
	}
}


####################################################################################################################################################################################
Function InstallIISforSolidCP()         # Function to install SolidCP IIS (Basic Requirements)
{
	Write-Host "`tInstalling the features required for SolidCP to run on IIS Server" -ForegroundColor Cyan
	Write-Host "`t  ****************************************" -ForegroundColor Green
	Write-Host "`t  *                                      *" -ForegroundColor Green
	Write-Host "`t  *  Please be patient while we install  *" -ForegroundColor Green
	Write-Host "`t  *    the required Winodws Features     *" -ForegroundColor Green
	Write-Host "`t  *                                      *" -ForegroundColor Green
	Write-Host "`t  ****************************************" -ForegroundColor Green
	# Install the features required by SolidCP Server to run on the machine, this is the minimum requirements for SolidCP to be installed on the server.
	(Add-WindowsFeature -Name Web-Server, Web-WebServer, Web-Common-Http, Web-Default-Doc, Web-Dir-Browsing, Web-Http-Errors, Web-Static-Content, Web-Http-Redirect, Web-Health, Web-Http-Logging, Web-Log-Libraries, Web-Http-Tracing, Web-Performance, Web-Stat-Compression, Web-Security, Web-Filtering, Web-Client-Auth, Web-Windows-Auth, Web-App-Dev, Web-Net-Ext, Web-Net-Ext45, Web-Asp-Net, Web-Asp-Net45, Web-ISAPI-Ext, Web-ISAPI-Filter, Web-Mgmt-Tools, Web-Mgmt-Console, Web-Scripting-Tools) | Out-Null
}


####################################################################################################################################################################################
Function InstallIISforHosting()         # Function to install additional IIS features for Web Server (Customer Hosting Requirements)
{
	Write-Host "`tInstalling the features required for Microsoft IIS Server (Web Hosting)" -ForegroundColor Cyan
	# Install the additional features required for a customer web server or shared hosting server that will be public facing
	(Add-WindowsFeature -Name Web-ODBC-Logging, Web-Request-Monitor, Web-Basic-Auth, Web-IP-Security, Web-ASP, Web-CGI, Web-Mgmt-Compat, Web-Metabase, Web-Lgcy-Mgmt-Console, Web-Lgcy-Scripting, Web-WMI, SMTP-Server) | Out-Null
	# Import the PowerShell Web Administration Module
	Import-Module WebAdministration
	# Configure all logging options to be enabled for websites in IIS
	Set-WebConfigurationProperty -Filter System.Applicationhost/Sites/SiteDefaults/logfile -Name LogExtFileFlags -Value "Date,Time,ClientIP,UserName,SiteName,ComputerName,ServerIP,Method,UriStem,UriQuery,HttpStatus,Win32Status,BytesSent,BytesRecv,TimeTaken,ServerPort,UserAgent,Cookie,Referer,ProtocolVersion,Host,HttpSubStatus"
	if ($dDomainMember) {
		# Create a domain group called "SCP_IUSRS_ComputerName" within the Web Server Organisational Unit in Active Directory
		CreateSolidCPdomainOU -Web_Server
	}
}


####################################################################################################################################################################################
Function InstallFTPforHosting()         # Function to install additional IIS features for FTP Server (Customer Hosting Requirements)
{
	Write-Host "`tInstalling the features required for Microsoft FTP Server" -ForegroundColor Cyan
	# Install the additional features required for a customer FTP server or shared hosting server that will be public facing
	(Add-WindowsFeature -Name Web-Ftp-Server, Web-Ftp-Service) | Out-Null
	Write-Host "`tConfiguring the FTP Server on this machine" -ForegroundColor Cyan
	# Check if the FTP Server is installed on this machine, if it check to make sure the Default FTP Site is not setup, then set it up corectly for SolidCP
	If ( ((Get-WindowsFeature *Web-Ftp-Service*).Installed[0]) -And !(Test-Path "IIS:\Sites\Default FTP Site" -pathType container) ) {
		# Create a local group called "SCPFtpUsers" on the local web server unless it already exists
		CreateLocalUserOrGroup "SCPFtpUsers" "SolidCP FTP Users Group  ********* DO NOT DELETE *********" "Group"
		# Create a domain group called "SCPFtpUsers_ComputerName" within the FTP Server Organisational Unit in Active Directory
		if ($dDomainMember) {
			CreateSolidCPdomainOU -FTP_Server
		}
		# Create the "ftproot" folder in "C:\inetpub" if it doesnt exist
		if(!(Test-Path "$env:SystemDrive\inetpub\ftproot")) {New-Item "$env:SystemDrive\inetpub\ftproot" -itemType directory}
		# Grant permissions for the "C:\inetpub\ftproot" folder to the SCPFtpUsers Group (Read ONLY Access)
		SetAccessToFolder "C:\inetpub\ftproot" "SCPFtpUsers" "Read" "Allow" "Local"
		if ($dDomainMember) {
			SetAccessToFolder "C:\inetpub\ftproot" "SCPFtpUsers_$env:COMPUTERNAME" "Read" "Allow" "Domain"
		}
		# Create the FTP Site for SolidCP only if it does not alreayd exist
		if (!(Test-Path "IIS:\Sites\Default FTP Site" -pathType container)) { # Setup the SolifCP FTP Site only if it doesn't already exist
			Import-Module WebAdministration
			$bindings = '@{protocol="ftp";bindingInformation="'+ $dIPV4 +':21:"}'	# Setup the Bindings for the FTP Site with the IP Address gathered above
			New-Item "IIS:\Sites\Default FTP Site" -bindings $bindings -physicalPath "%SystemDrive%\inetpub\ftproot" -Verbose:$false | Out-Null # Create site with directory
			# Ensure the settings are correct for the FTP Server to enure security
			Set-ItemProperty "IIS:\Sites\Default FTP Site" -Name ftpServer.security.ssl.controlChannelPolicy -Value 0 # Allow SSL connections
			Set-ItemProperty "IIS:\Sites\Default FTP Site" -Name ftpServer.security.ssl.dataChannelPolicy -Value 0    # Allow SSL connections
			Set-ItemProperty "IIS:\Sites\Default FTP Site" -Name ftpServer.security.authentication.basicAuthentication.enabled -Value $true # Enable Basic Authentication
			Set-ItemProperty "IIS:\Sites\Default FTP Site" -Name ftpServer.security.authentication.anonymousAuthentication.enabled -Value $false # Disable Anonymous Authentication
			Set-ItemProperty "IIS:\Sites\Default FTP Site" -Name ftpserver.userisolation.mode -Value 0  # Set USer Isolation
			Set-ItemProperty "IIS:\Sites\Default FTP Site" -Name ftpserver.logFile.directory -Value "%SystemDrive%\inetpub\logs\LogFiles"  # Set Log File Directory
			# Configure all logging options to be enabled for FTP Sites in IIS
			Set-WebConfigurationProperty -Filter "system.applicationHost/sites/siteDefaults/ftpServer/logFile" -Name LogExtFileFlags -Value "Date,Time,ClientIP,UserName,SiteName,ComputerName,ServerIP,Method,UriStem,FtpStatus,Win32Status,BytesSent,BytesRecv,TimeTaken,ServerPort,Host,FtpSubStatus,Session,FullPath,Info,ClientPort"
			# Give Authorization to SCPFtpUsers and grant "read" privileges in IIS (Default FTP Site)
			Add-WebConfiguration "/system.ftpServer/security/authorization" -value @{accessType="Allow";roles="SCPFtpUsers";permissions="Read";users=""} -PSPath IIS:\ -location "Default FTP Site"
			if ($dDomainMember) {
				Add-WebConfiguration "/system.ftpServer/security/authorization" -value @{accessType="Allow";roles="$env:USERDOMAIN\SCPFtpUsers_$env:COMPUTERNAME";permissions="Read";users=""} -PSPath IIS:\ -location "Default FTP Site"
			}
			Restart-WebItem "IIS:\Sites\Default FTP Site"  # Restart the FTP site for all changes to take effect
		}
	}
}


####################################################################################################################################################################################
Function CreateLocalUserOrGroup($dName, $dDescription, $dType)                  # Create a Local User or Group if it does not exist
{ # Usage - CreateLocalUserOrGroup "Local [User|Group] Name" "Description" "User|Group"
	if (!([ADSI]::Exists("WinNT://$env:computername/$dName"))) {
		invoke-command {$cn=[ADSI]"WinNT://$env:computername";$name=$cn.Create("$dType", "$dName");$name.SetInfo();$name.description=("$dDescription");$name.SetInfo()}
	}
}


####################################################################################################################################################################################
Function AddLocalUserToLocalGroup($dGroupName, $dUserName)                      # Add a Local User to a Local Group if it is not already a member
{ # Usage - AddLocalUserToLocalGroup "Local Group Name" "Local User Name"
	if (!([ADSI]::Exists("WinNT://$env:computername/$dGroupName"))) {
		Write-Host "`t Local Group `"$env:computername\$dGroupName`" does not exist`!" -ForegroundColor Red
	}elseif (!([ADSI]::Exists("WinNT://$env:computername/$dUserName"))) {
		Write-Host "`t Local User `"$env:computername\$dUserName`" does not exist`!" -ForegroundColor Red
	}else{
		if (( @( ([ADSI]"WinNT://$env:computername/$dGroupName").psbase.Invoke("Members") ) | ForEach-Object{$_.GetType().InvokeMember("Name", "GetProperty", $null, $_, $null);}) -notcontains $dUserName) {
			invoke-command {$group = [ADSI]"WinNT://$env:computername/$dGroupName,group";$group.Add("WinNT://$env:computername/$dUserName");$group.SetInfo()}
		}
	}
}


####################################################################################################################################################################################
Function AddDomainUserToLocalGroup($dGroupName, $dUserName)                     # Add a Domain User to a Local Group if it is not already a member
{ # Usage - AddDomainUserToLocalGroup "Local Group Name" "Domain User Name"
	if (!([ADSI]::Exists("WinNT://$env:computername/$dGroupName"))) {
		Write-Host "`t Local Group `"$env:computername\$dGroupName`" does not exist`!" -ForegroundColor Red
	}elseif (!([ADSI]::Exists("WinNT://$env:USERDNSDOMAIN/$dUserName"))) {
		Write-Host "`t Domain User `"$env:USERDNSDOMAIN\$dUserName`" does not exist`!" -ForegroundColor Red
	}else{
		if (( @( ([ADSI]"WinNT://$env:computername/$dGroupName").psbase.Invoke("Members") ) | ForEach-Object{$_.GetType().InvokeMember("Name", "GetProperty", $null, $_, $null);}) -notcontains $dUserName) {
			invoke-command {$group = [ADSI]"WinNT://$env:computername/$dGroupName,group";$group.Add("WinNT://$env:USERDNSDOMAIN/$dUserName");$group.SetInfo()}
		}
	}
}


####################################################################################################################################################################################
Function AddDomainUserToDomainGroup($dGroupName, $dUserName)                    # Add a Domain User to a Domain Group if it is not already a member
{ # Usage - AddDomainUserToDomainGroup "Domain Group Name" "Domain User Name"
	if (!([ADSI]::Exists("WinNT://$env:USERDNSDOMAIN/$dGroupName"))) {
		Write-Host "`t Domain Group `"$env:USERDNSDOMAIN\$dGroupName`" does not exist`!" -ForegroundColor Red
	}elseif (!([ADSI]::Exists("WinNT://$env:USERDNSDOMAIN/$dUserName"))) {
		Write-Host "`t Domain User `"$env:USERDNSDOMAIN\$dUserName`" does not exist`!" -ForegroundColor Red
	}else{
		if (( @( ([ADSI]"WinNT://$env:USERDNSDOMAIN/$dGroupName").psbase.Invoke("Members") ) | ForEach-Object{$_.GetType().InvokeMember("Name", "GetProperty", $null, $_, $null);}) -notcontains $dUserName) {
			invoke-command {$group = [ADSI]"WinNT://$env:USERDNSDOMAIN/$dGroupName,group";$group.Add("WinNT://$env:USERDNSDOMAIN/$dUserName");$group.SetInfo()}
		}
	}
}


####################################################################################################################################################################################
Function CopyGroupMembers($dSourceGroup, $dDestGroup, $dGroupType)              # Copy Group Members from one group to another
{ # Usage - CopyGroupMembers "From Group Name" "To Group Name" "Local|Domain"
	$MemberNames = @()
	if ($dGroupType -eq "local") { # If Local group is specified
		if ( ([ADSI]::Exists("WinNT://$env:computername/$dSourceGroup")) -and ([ADSI]::Exists("WinNT://$env:computername/$dDestGroup")) ) { # Check if the Local groups exist
			# Get the members for the Source Group and store them in an array
			$dMembers = ( @( ([ADSI]"WinNT://$env:computername/$dSourceGroup").psbase.Invoke("Members") ) | ForEach-Object{$_.GetType().InvokeMember('adspath', 'GetProperty', $null, $_, $null);} )
			foreach ($dMember in $dMembers) {
				# Add the user if it is not already a member of the destination group
				if (( @( ([ADSI]"WinNT://$env:computername/$dDestGroup,group").psbase.Invoke("Members") ) | ForEach-Object{$_.GetType().InvokeMember("adspath", "GetProperty", $null, $_, $null);}) -notcontains "$dMember") {
					invoke-command {$group = [ADSI]"WinNT://$env:computername/$dDestGroup,group";$group.Add("$dMember");$group.SetInfo()}
				}
			}
		}
	}elseif ( ($dGroupType -eq "domain") -and ($dDomainMember) ) { # If Domain group is specified
		if ( (([ADSISearcher]"(sAMAccountName=$dSourceGroup)").FindOne()) -and (([ADSISearcher]"(sAMAccountName=$dDestGroup)").FindOne()) ) { # Check if the Domain group exists
			# Get the members for the Source Group and store them in an array
			$dMembers = ( @( ([ADSI]"WinNT://$env:USERDNSDOMAIN/$dSourceGroup").psbase.Invoke("Members") ) | ForEach-Object{$_.GetType().InvokeMember('adspath', 'GetProperty', $null, $_, $null);} )
			foreach ($dMember in $dMembers) {
				# Add the user if it is not already a member of the destination group
				if (( @( ([ADSI]"WinNT://$env:USERDNSDOMAIN/$dDestGroup").psbase.Invoke("Members") ) | ForEach-Object{$_.GetType().InvokeMember("adspath", "GetProperty", $null, $_, $null);}) -notcontains "$dMember") {
					invoke-command {$group = [ADSI]"WinNT://$env:USERDNSDOMAIN/$dDestGroup,group";$group.Add("$dMember");$group.SetInfo()}
				}
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
Function GetMembersOfGroup ($dGroupName, $dIdentifier)                          # Get all members of a group (Local or Domain) and return the required variables   
{ # Usage - GetMembersOfGroup "Administrators" "SID|Name|Caption|Domain"
	if (!($dIdentifier)) {$dIdentifier = "Name"} # If the Identifier is not specified then set it as the Name
	(Get-CimInstance -ClassName win32_group -Filter "name = '$dGroupName'" | Get-CimAssociatedInstance -Association win32_groupuser).$dIdentifier
}


####################################################################################################################################################################################
Function CreateDomainComputerObject()                                           # Create a Domain Computer Object if it does not already exist
{ # Usage - CreateDomainComputerObject -Name "ComputerName" -OU "SolidCP" -Disabled
	Param(
		[string]$Name,    # Computer Name to be added to Active Directory
		[string]$OU,      # Organisational Unit in Active Directory where to add the new Computer Object
		[switch]$Disabled # Disable the new Computer Object when creating it
	)
	if ($OU) {
		$ComputerOU  = (([ADSI]“LDAP://OU=$OU,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)“).distinguishedName)
	}else{
		$ComputerOU  = (([ADSI]“LDAP://CN=Computers,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)“).distinguishedName)
	}
	if (!([string]::IsNullOrEmpty($ComputerOU))) { # Check of the Organisational Unit exists
		if ( !([adsi]::Exists("LDAP://CN=$Name,$ComputerOU")) ) { # Check if the Computer Object exists
			$dNewComputer = ([ADSI]"LDAP://$ComputerOU").create("Computer", "CN=$Name")
			$dNewComputer.put(“sAMAccountName”,($Name + "$"))
			$dNewComputer.put(“userAccountControl”,4128)
			# Disable the Computer Object if required
			if ($Disabled) { $dNewComputer.InvokeSet("AccountDisabled", $true) }
			$dNewComputer.setInfo()
			# Check if the New OU has been created before continuing
			do { $dCeckIfComputerCreated = ([adsi]::Exists("LDAP://CN=$Name,$ComputerOU")) } until ($dCeckIfComputerCreated -eq $true)
			Write-Host "`t Created a New Computer Object Unit Called `"$Name`"" -ForegroundColor Green
		}else{
			Write-Host "`t The Computer Object Unit Called `"$Name`" already exists`!" -ForegroundColor Red
		}
	}else{
		Write-Host "`t The Organisational Unit Called `"$OU`" does not exist`!" -ForegroundColor Red
	}
}


####################################################################################################################################################################################
Function AddDomainComputerToComputerObject()                                    # Add a Domain Computer to a Domain Computer Object
{ # Usage - AddDomainComputerToComputerObject -Computer "ComputerName" -OU "SolidCP" -Add "ComputerNameToAdd"
	Param(
		[string]$Computer, # Domain Computer Object in Active Directory
		[string]$OU,       # Organisational Unit in Active Directory where the Computer Object is located
		[string]$Add       # Domain Computer Object to be added
	)
	if ($OU) {
		$ComputerOU  = (([ADSI]“LDAP://OU=$OU,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)“).distinguishedName)
	}else{
		$ComputerOU  = (([ADSI]“LDAP://CN=Computers,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)“).distinguishedName)
	}
	if (!([string]::IsNullOrEmpty($ComputerOU))) { # Check of the Organisational Unit exists
		if ([adsi]::Exists("LDAP://CN=$Computer,$ComputerOU")) { # Check if the Computer Object exists
			Import-Module ActiveDirectory
			$Path = "AD:\CN=$Computer,$ComputerOU"
			$Acl = Get-Acl -Path $Path
			$Ace = New-Object System.DirectoryServices.ActiveDirectoryAccessRule([System.Security.Principal.NTAccount]"$Add$", "GenericAll", "Allow")
			$Acl.AddAccessRule($Ace)
			Set-Acl -Path $Path -AclObject $Acl
			# Check if the New OU has been created before continuing
			do { $dCeckIfComputerAdded = (((((Get-Acl -Path "AD:\CN=$Computer,$ComputerOU").Access).IdentityReference).Value) -contains "$env:USERDOMAIN\$Add$") } until ($dCeckIfComputerAdded -eq $true)
			Write-Host "`t `"$Add`" was added to the Computer Object Unit Called `"$Computer`"" -ForegroundColor Green
		}else{
			Write-Host "`t The Computer Object Unit Called `"$Computer`" does not exist`!" -ForegroundColor Red
		}
	}else{
		Write-Host "`t The Organisational Unit Called `"$OU`" does not exist`!" -ForegroundColor Red
	}
}


####################################################################################################################################################################################
Function CheckDomainComputerObject()                                            # Check if a Domain Computer Object already exists
{ # Usage - CheckDomainComputerObject -Name "ComputerName" -OU "SolidCP"
	Param(
		[string]$Name,    # Computer Name to be checked in Active Directory
		[string]$OU       # Organisational Unit in Active Directory where to check the Computer Object
	)
	if ($OU) {
		$ComputerOU  = (([ADSI]“LDAP://OU=$OU,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)“).distinguishedName)
	}else{
		$ComputerOU  = (([ADSI]“LDAP://CN=Computers,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)“).distinguishedName)
	}
	if ([adsi]::Exists("LDAP://CN=$Name,$ComputerOU") ) { # Check if the Computer Object exists
		return $true
	}else{
		return $false
	}
}


####################################################################################################################################################################################
Function SetAccessToFolder ($dFolder, $dName, $dAccessRights, $dType, $dGroupType) # Set access for a User or Group to a folder if the user, group or folder exists 
{ # Usage - SetAccessToFolder "C:\HostingSpaces" "User|Group" "Read|ReadAndExecute|Change|Modify|ListFolderContents|FullControl" "Allow|Deny" "Local|Domain"
	if ( ($dGroupType -eq "local") -and (!([ADSI]::Exists("WinNT://$env:computername/$dName"))) ) {
		Write-Host "`t The Local User or Group `"$env:computername\$dName`" does not exist`!" -ForegroundColor Red
	}elseif ( ($dGroupType -eq "domain") -and (!(([ADSISearcher]"(sAMAccountName=$dName)").FindOne())) ) {
		Write-Host "`t The Domain User or Group `"$env:USERDOMAIN\$dName`" does not exist`!" -ForegroundColor Red
	}elseif (!(Test-Path "$dFolder")) {
		Write-Host "`t The Folder does not exist`!" -ForegroundColor Red
	}elseif (!($dGroupType)) {
		Write-Host "`t You need to specify Local or Domain for the Group Type`!" -ForegroundColor Red
	}else{
		$AccessRule = New-Object System.Security.AccessControl.FileSystemAccessRule("$dName", "$dAccessRights", [system.security.accesscontrol.InheritanceFlags]"ContainerInherit, ObjectInherit", [system.security.accesscontrol.PropagationFlags]"None", "$dType")
		$Acl        = (Get-Item "$dFolder").GetAccessControl('Access')
		$Acl.AddAccessRule($AccessRule)
		Set-ACL -Path "$dFolder" -AclObject $Acl
		invoke-command {$AccessRule} | Out-Null
	}
}


####################################################################################################################################################################################
Function SetAccessToFolderNoChk ($dFolder, $dName, $dAccessRights, $dType)      # Set access for a User or Group to a folder if the folder exists (doesn't check the user or group)
{ # Usage - SetAccessToFolderNoChk "C:\HostingSpaces" "User|Group" "Read|ReadAndExecute|Change|Modify|ListFolderContents|FullControl" "Allow|Deny"
	if (!(Test-Path "$dFolder")) {
		Write-Host "`t The Folder does not exist`!" -ForegroundColor Red
	}else{
		$AccessRule = New-Object System.Security.AccessControl.FileSystemAccessRule("$dName", "$dAccessRights", [system.security.accesscontrol.InheritanceFlags]"ContainerInherit, ObjectInherit", [system.security.accesscontrol.PropagationFlags]"None", "$dType")
		$Acl        = (Get-Item "$dFolder").GetAccessControl('Access')
		$Acl.AddAccessRule($AccessRule)
		Set-ACL -Path "$dFolder" -AclObject $Acl
		invoke-command {$AccessRule} | Out-Null
	}
}


####################################################################################################################################################################################
Function RemoveAccessToFolder ($dFolder, $dName, $dAccessRights, $dType)        # Remove access for a User or Group to a folder if the user, group or folder exists
{ # Usage - RemoveAccessToFolder "C:\HostingSpaces" "User|Group" "Read|ReadAndExecute|Change|Modify|ListFolderContents|FullControl" "Allow|Deny"
	if (!([ADSI]::Exists("WinNT://$env:computername/$dName"))) {
		Write-Host "`t The Local User or Group `"$dName`" does not exist`!" -ForegroundColor Red
	}elseif (!(Test-Path "$dFolder")) {
		Write-Host "`t The Folder does not exist`!" -ForegroundColor Red
	}else{
		$AccessRule = New-Object System.Security.AccessControl.FileSystemAccessRule($dName,$dAccessRights, 'ContainerInherit,ObjectInherit', 'None', "$dType")
		$Acl        = (Get-Item "$dFolder").GetAccessControl('Access')
		$Acl.RemoveAccessRuleAll($AccessRule)
		Set-Acl -Path "$dFolder" -AclObject $Acl
	}
}


####################################################################################################################################################################################
Function RemoveAccessToFolderNoChk ($dFolder, $dName, $dAccessRights, $dType)   # Remove access for a User or Group to a folder if the user, group or folder exists
{ # Usage - RemoveAccessToFolderNoChk "C:\HostingSpaces" "User|Group" "Read|ReadAndExecute|Change|Modify|ListFolderContents|FullControl" "Allow|Deny"
	if (!(Test-Path "$dFolder")) {
		Write-Host "`t The Folder does not exist`!" -ForegroundColor Red
	}else{
		$AccessRule = New-Object System.Security.AccessControl.FileSystemAccessRule($dName,$dAccessRights, 'ContainerInherit,ObjectInherit', 'None', "$dType")
		$Acl        = (Get-Item "$dFolder").GetAccessControl('Access')
		$Acl.RemoveAccessRuleAll($AccessRule)
		Set-Acl -Path "$dFolder" -AclObject $Acl
	}
}


####################################################################################################################################################################################
Function SetAccessToFiles ($dFileLocation, $dName, $dAccessRights, $dType)      # Set access for a User or Group to a files within a folder if the user or group exists 
{ # Usage - SetAccessToFiles "C:\HostingSpaces\*.txt" "User|Group" "Read|ReadAndExecute|Change|Modify|ListFolderContents|FullControl" "Allow|Deny"
	if (!([ADSI]::Exists("WinNT://$env:computername/$dName"))) {
		Write-Host "`t The Local User or Group `"$dName`" does not exist`!" -ForegroundColor Red
	}else{
		Get-ChildItem $dFileLocation |  ForEach {
			$AccessRule = New-Object  System.Security.AccessControl.FileSystemAccessRule("$dName","$dAccessRights","$dType")
			$Acl        = Get-Acl "$_"
			$Acl.SetAccessRule($AccessRule)
			Set-Acl -Path "$_" -AclObject $Acl
		}
	}
}


####################################################################################################################################################################################
Function RemoveAccessToFiles ($dFileLocation, $dName, $dAccessRights, $dType)   # Grant access for a User or Group to a folder if the user or group exists 
{ # Usage - RemoveAccessToFiles "C:\HostingSpaces\*.txt" "User|Group" "Read|ReadAndExecute|Change|Modify|ListFolderContents|FullControl" "Allow|Deny"
	if (!([ADSI]::Exists("WinNT://$env:computername/$dName"))) {
		Write-Host "`t The Local User or Group `"$dName`" does not exist`!" -ForegroundColor Red
	}else{
		Get-ChildItem $dFileLocation |  ForEach {
			$AccessRule = New-Object  System.Security.AccessControl.FileSystemAccessRule($dName,"$dAccessRights","$dType")
			$Acl        = Get-Acl "$_"
			$Acl.RemoveAccessRuleAll($AccessRule)
			Set-Acl -Path "$_" -AclObject $Acl
		}
	}
}


####################################################################################################################################################################################
Function CheckAccessToFolder()      # Check access for a User or Group on a folder if the folder exists
{ # Usage - CheckAccessToFolder -Folder "C:\HostingSpaces" -User "User|Group" [-Local -Domain] -Type "Allow|Deny" -AccessRights "Read|ReadAndExecute|Change|Modify|ListFolderContents|FullControl"
	Param(
		[string]$Folder,            # Folder Location to check the Access Rights
		[string]$User,              # User or Group to check the Access Rights of
		[switch]$Local,             # Check against Local users or groups
		[switch]$Domain,            # Check against Domain users or groups
		[switch]$System,            # Check against System users or groups
		[switch]$NTauth,            # Check against NT Authority users or groups
		[string]$AccessRights,      # Access Rights to check
		[string]$Type               # Access Permissions to check
	)
	if (!(Test-Path "$Folder")) {
		Write-Host "`t The Folder does not exist`!" -ForegroundColor Red
	}elseif ( ($Local) -and (!([ADSI]::Exists("WinNT://$env:computername/$User"))) ) {
		Write-Host "`t The Local User or Group `"$env:computername\$User`" does not exist`!" -ForegroundColor Red
	}elseif ( ($Domain) -and (!(([ADSISearcher]"(sAMAccountName=$User)").FindOne())) ) {
		Write-Host "`t The Domain User or Group `"$env:USERDOMAIN\$User`" does not exist`!" -ForegroundColor Red
	}elseif ( (!($Local)) -and (!($System)) -and (!($NTauth)) -and (!($Domain)) ) {
		Write-Host "`t You need to specify Local or Domain for the Group Type`!" -ForegroundColor Red
	}elseif (!($AccessRights)) {
		Write-Host "`t You need to specify Access Rights to check against`!" -ForegroundColor Red
	}elseif (!($Type)) {
		Write-Host "`t You need to specify Permission to check against (Allow or Deny)`!" -ForegroundColor Red
	}else{
		if ($Local -or $System -or $NTauth) {
			$UserSID = (((New-Object System.Security.Principal.NTAccount("$User")).Translate([System.Security.Principal.SecurityIdentifier])).Value)
			$UserFull = ([wmi]"Win32_SID.SID='$UserSID'")
		}elseif ($Domain) {
			$UserSID = (((New-Object System.Security.Principal.NTAccount("$env:USERDOMAIN", "$User")).Translate([System.Security.Principal.SecurityIdentifier])).Value)
			$UserFull = ([wmi]"Win32_SID.SID='$UserSID'")
		}
		foreach ($UserName in (((Get-Item "$Folder").GetAccessControl('Access')).Access)) {
			if ($UserName.IdentityReference -eq "$($UserFull.ReferencedDomainName)\$($UserFull.AccountName)") {
				foreach ($FileSystemRights in ($UserName.FileSystemRights -split ",")) {
					if ( ($FileSystemRights -eq $AccessRights) -and ($UserName.AccessControlType -eq $Type) ) {
						$result = $true
					}
				}
			}
		}
		if ($result) {
			return $true
		}else{
			return $false
		}
	}
}


####################################################################################################################################################################################
Function DisableFolderInheritance ($dFolder)    # Disable inheritance on the folder specified
{ # Usage - DisableFolderInheritance "C:\HostingSpaces"
if (!(Test-Path "$dFolder")) {
		Write-Host "`t The `"$dFolder`" folder does not exist`!" -ForegroundColor Red
	}else{
		$acl = Get-ACL -Path $dFolder
		$acl.SetAccessRuleProtection($True, $True)
		Set-Acl -Path $dFolder -AclObject $acl | Out-Null
	}
}


####################################################################################################################################################################################
Function EnableFolderInheritance ($dFolder)     # Enable inheritance on the folder specified
{ # Usage - EnableFolderInheritance "C:\HostingSpaces"
if (!(Test-Path "$dFolder")) {
		Write-Host "`t The `"$dFolder`" folder does not exist`!" -ForegroundColor Red
	}else{
		$acl = Get-ACL -Path $dFolder
		$acl.SetAccessRuleProtection($false, $false)
		Set-Acl -Path $dFolder -AclObject $acl | Out-Null
	}
}


####################################################################################################################################################################################
Function CheckFolderInheritance()               # Check inheritance on the folder specified
{ # Usage - CheckFolderInheritance -Folder "C:\HostingSpaces"
	Param(
		[string]$Folder            # Folder Location to check Inheritance on
	)
	if (!(Test-Path "$Folder")) {
		Write-Host "`t The `"$Folder`" folder does not exist`!" -ForegroundColor Red
	}else{
		foreach ($User in ((Get-ACL -Path "$Folder").Access)) {
			if ($User.IsInherited -eq $true) {
				$InheritanceEnabled = $true
			}elseif ($User.IsInherited -eq $false) {
				$InheritanceDisabled = $true
			}
		}
		if ( ($InheritanceEnabled) -and (!($InheritanceDisabled)) ) {
			return $true
		}elseif ( ($InheritanceDisabled) -and (!($InheritanceEnabled)) ) {
			return $false
		}elseif ( ($InheritanceEnabled) -and ($InheritanceDisabled) ) {
			return $error
		}
	}
}


####################################################################################################################################################################################
Function CreateSolidCPdomainOU()        # Function to create the SolidCP Organisational units
{
	Param(
		[switch]$Web_Server,    # Setup the WebServer Organisational Units in Active Directory
		[switch]$FTP_Server,    # Setup the FTP Server Organisational Units in Active Directory
		[switch]$Hosted         # Setup the Hosted Organisational Units in Active Directory (For Exchange, SharePoint, RDP etc)
	)
	# Check if this machine is joined to a domain before continuing
	if ($dDomainMember) {
		# Add the Remote Server Administration Tools for PowerShell to this server if it is a member of a domain
		(Add-WindowsFeature -Name RSAT-AD-PowerShell) | Out-Null
		# Check to make sure the Organisational Unit has been set at the top of the script, if not then ask for one
		if (!($dSolidCp_OU_Name)) {
			$choice = ""
			Write-Host "`n`tIt is reccomended that you have a Dedicated Organisational Unit for SolidCP" -ForegroundColor Yellow
			while ($choice -notmatch "[y|n]") {
				$choice = Read-Host "`n`t Do you wan to install a new Organisational Unit for SolidCP`? (Y/N)"
			}
			if ($choice -eq "y") {
				$script:dSolidCp_OU_Name = Read-Host "`n`t Please enter the name for the Organisational Unit"
				if (!($dSolidCp_OU_Name)) {$script:dSolidCp_OU_Name = "SolidCP"}
			}
		}
		# Check to see if the SolidCP Organisational Unit exists in Active Directory, if not then create it
		if (!([adsi]::Exists("LDAP://OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)"))) {
			New-ADOrganizationalUnit -Name "$dSolidCp_OU_Name" -Path "$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)" -Description "SolidCP Organisational Unit"
			do { $dCheckWebServersOUcreated = ([adsi]::Exists("LDAP://OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)")) } until ($dCheckWebServersOUcreated -eq $true)
			Write-Host "`t Organisational Unit `"$dSolidCp_OU_Name`" created in Active Directory" -ForegroundColor Green
		}
		if ($Web_Server) {
			# Check to see if the WebServers Organisational Unit is created in the SolidCP Organisational Unit created above
			if (!([adsi]::Exists("LDAP://OU=WebServers,OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)"))) {
				New-ADOrganizationalUnit -Name "WebServers" -Path "OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)" -Description "SolidCP Web Servers"
				do { $dCheckWebServersOUcreated = ([adsi]::Exists("LDAP://OU=WebServers,OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)")) } until ($dCheckWebServersOUcreated -eq $true)
				Write-Host "`t Organisational Unit `"$dSolidCp_OU_Name\WebServers`" created in Active Directory" -ForegroundColor Green
			}
			# Create an Organisational Unit with the Server Name in the WebServers Organisational Unit created above if it doesn't already exist
			if (!([adsi]::Exists("LDAP://OU=$env:COMPUTERNAME,OU=WebServers,OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)"))) {
				New-ADOrganizationalUnit -Name "$env:COMPUTERNAME" -Path "OU=WebServers,OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)" -Description "SolidCP Organisational Unit for $env:COMPUTERNAME"
				do { $dCheckWebServerOUcreated1 = ([adsi]::Exists("LDAP://OU=$env:COMPUTERNAME,OU=WebServers,OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)")) } until ($dCheckWebServerOUcreated1 -eq $true)
				Write-Host "`t Organisational Unit `"$dSolidCp_OU_Name\WebServers\$env:COMPUTERNAME`" created in Active Directory" -ForegroundColor Green
			}
			# Create an Organisational Unit called Users in the Server Name Organisational Unit created above if it doesn't already exist
			if (!([adsi]::Exists("LDAP://OU=Users,OU=$env:COMPUTERNAME,OU=WebServers,OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)"))) {
				New-ADOrganizationalUnit -Name "Users" -Path "OU=$env:COMPUTERNAME,OU=WebServers,OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)" -Description "SolidCP Organisational Unit for $env:COMPUTERNAME Web Users"
				do { $dCheckWebServerOUcreated2 = ([adsi]::Exists("LDAP://OU=Users,OU=$env:COMPUTERNAME,OU=WebServers,OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)")) } until ($dCheckWebServerOUcreated2 -eq $true)
				Write-Host "`t Organisational Unit `"$dSolidCp_OU_Name\WebServers\$env:COMPUTERNAME\Users`" created in Active Directory" -ForegroundColor Green
			}
			# Create the SCP_IUSRS_ComputerName Security Group in the WebServers Organisational Unit if it doesn't already exist
			if (!([bool](Get-ADGroup -LDAPFilter "(sAMAccountName=SCP_IUSRS_$env:COMPUTERNAME)"))) {
				New-ADGroup -Name "SCP_IUSRS_$env:COMPUTERNAME" -SamAccountName "SCP_IUSRS_$env:COMPUTERNAME" -GroupCategory Security -GroupScope Global -Description "SolidCP System Group for $env:COMPUTERNAME" -Path "OU=$env:COMPUTERNAME,OU=WebServers,OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)"
				do { $dCheckWebServerUsercreated = ([bool](([ADSISearcher]"(sAMAccountName=SCP_IUSRS_$env:COMPUTERNAME)").FindOne())) } until ($dCheckWebServerUsercreated -eq $true)
				Write-Host "`t The `"SCP_IUSRS_$env:COMPUTERNAME`" Security Group has been created in Active Directory" -ForegroundColor Green
			}
		}elseif ($FTP_Server) {
			# Check to see if the FtpServers Organisational Unit is created in the SolidCP Organisational Unit created above
			if (!([adsi]::Exists("LDAP://OU=FtpServers,OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)"))) {
				New-ADOrganizationalUnit -Name "FtpServers" -Path "OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)" -Description "SolidCP Web Servers"
				do { $dCheckFtpServersOUcreated = ([adsi]::Exists("LDAP://OU=FtpServers,OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)")) } until ($dCheckFtpServersOUcreated -eq $true)
				Write-Host "`t Organisational Unit `"$dSolidCp_OU_Name\FtpServers`" created in Active Directory" -ForegroundColor Green
			}
			# Create an Organisational Unit with the Server Name in the FtpServers Organisational Unit created above if it doesn't already exist
			if (!([adsi]::Exists("LDAP://OU=$env:COMPUTERNAME,OU=FtpServers,OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)"))) {
				New-ADOrganizationalUnit -Name "$env:COMPUTERNAME" -Path "OU=FtpServers,OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)" -Description "SolidCP Organisational Unit for $env:COMPUTERNAME"
				do { $dCheckFtpServerOUcreated1 = ([adsi]::Exists("LDAP://OU=$env:COMPUTERNAME,OU=FtpServers,OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)")) } until ($dCheckFtpServerOUcreated1 -eq $true)
				Write-Host "`t Organisational Unit `"$dSolidCp_OU_Name\FtpServers\$env:COMPUTERNAME`" created in Active Directory" -ForegroundColor Green
			}
			# Create an Organisational Unit called Users in the Server Name Organisational Unit created above if it doesn't already exist
			if (!([adsi]::Exists("LDAP://OU=Users,OU=$env:COMPUTERNAME,OU=FtpServers,OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)"))) {
				New-ADOrganizationalUnit -Name "Users" -Path "OU=$env:COMPUTERNAME,OU=FtpServers,OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)" -Description "SolidCP Organisational Unit for $env:COMPUTERNAME Web Users"
				do { $dCheckFtpServerOUcreated2 = ([adsi]::Exists("LDAP://OU=Users,OU=$env:COMPUTERNAME,OU=FtpServers,OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)")) } until ($dCheckFtpServerOUcreated2 -eq $true)
				Write-Host "`t Organisational Unit `"$dSolidCp_OU_Name\FtpServers\$env:COMPUTERNAME\Users`" created in Active Directory" -ForegroundColor Green
			}
			# Create the SCPFtpUsers_ComputerName Security Group in the FtpServers Organisational Unit if it doesn't already exist
			if (!([bool](Get-ADGroup -LDAPFilter "(sAMAccountName=SCPFtpUsers_$env:COMPUTERNAME)"))) {
				New-ADGroup -Name "SCPFtpUsers_$env:COMPUTERNAME" -SamAccountName "SCPFtpUsers_$env:COMPUTERNAME" -GroupCategory Security -GroupScope Global -Description "SolidCP System Group for $env:COMPUTERNAME" -Path "OU=$env:COMPUTERNAME,OU=FtpServers,OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)"
				do { $dCheckFtpServerUsercreated = ([bool](([ADSISearcher]"(sAMAccountName=SCPFtpUsers_$env:COMPUTERNAME)").FindOne())) } until ($dCheckFtpServerUsercreated -eq $true)
				Write-Host "`t The `"SCPFtpUsers_$env:COMPUTERNAME`" Security Group has been created in Active Directory" -ForegroundColor Green
			}
		}elseif ($Hosted) {
			# Check to see if the Hosted Organisational Unit is created in the SolidCP Organisational Unit created above
			if (!($dSCPhstd_OU_Name)) {$dSCPhstd_OU_Name = "Hosted"}
			if (!([adsi]::Exists("LDAP://OU=$dSCPhstd_OU_Name,OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)"))) {
				New-ADOrganizationalUnit -Name "$dSCPhstd_OU_Name" -Path "OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)" -Description "SolidCP Web Servers"
				do { $dCheckHostedOUcreated = ([adsi]::Exists("LDAP://OU=$dSCPhstd_OU_Name,OU=$dSolidCp_OU_Name,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)")) } until ($dCheckHostedOUcreated -eq $true)
				Write-Host "`t Organisational Unit `"$dSolidCp_OU_Name\$dSCPhstd_OU_Name`" created in Active Directory" -ForegroundColor Green
			}
		}
	}
}


####################################################################################################################################################################################
Function dCreateNewOU () # Function to create new Organisational Unit if it does not already exist
{ # Usage - dCreateNewOU - Name "OU=New OU Name" -Description "OU Description" -DiableInheritance
	Param(
		[string]$Name,              # Name for the New Organisational Unit
		[string]$Description,       # Description for the New Organisational Unit
		[switch]$DiableInheritance, # If this switch is set it will Disable Inheritance for the New Organisational Unit
		        $RemoveUserFromACL  # Remove User (Name or SID) from the ACL for the New Organisational Unit (Single User or Array of Users)
	)
	(Import-Module ActiveDirectory) | Out-Null
	$RootDN = (([ADSI]"LDAP://RootDSE").rootDomainNamingContext)
	if ( !($Name) ) { $Name = Read-Host "`n Please enter the Name for the new Organisational Unit you want to create" }
	if ( !([adsi]::Exists("LDAP://OU=$Name,$RootDN")) ) {
		$dNewOU = ([ADSI]"LDAP://$RootDN").create("organizationalUnit", "OU=$Name") ; $dNewOU.setInfo()
		# Set the Description for the new OU if it has been specified
		if ($Description) { $dNewOU.description=("$Description");$dNewOU.SetInfo() }
		# Check if the New OU has been created before continuing
		do { $dCeckIfOUcreated = ([adsi]::Exists("LDAP://OU=$Name,$RootDN")) } until ($dCeckIfOUcreated -eq $true)
		Write-Host "`t Created a New Organisational Unit Called `"$Name`"" -ForegroundColor Green
		# Disable Inheritance if requested
		if ($DiableInheritance) {
			$acl1 = Get-ACL -Path "AD:\OU=$Name,$RootDN"
			$acl1.SetAccessRuleProtection($True, $True)
			Set-Acl -Path "AD:\OU=$Name,$RootDN" -AclObject $acl1 | Out-Null
			Write-Host "`t Disabling Inheritance on the `"$Name`" Organisational Unit" -ForegroundColor Green
		}
		# Remove access 
		if ($RemoveUserFromACL) {
			foreach ($user in $RemoveUserFromACL) {
				$acl = Get-Acl -Path "AD:\OU=$Name,$RootDN"
				foreach($acc in $acl.access ) {
					if(($acc.IdentityReference.Value) -eq "$user") {
						($acl.RemoveAccessRule($acc)) | Out-Null
						(Set-Acl -Path "AD:\OU=$Name,$RootDN" -AclObject $acl -ErrorAction Stop) | Out-Null
					}
				}
				Write-Host "`t Removing access for `"$user`" on the `"$Name`" Organisational Unit" -ForegroundColor Green
			}
		}
	}
}


####################################################################################################################################################################################
Function AskToAdd()                                         # Function to ask to add a value to something
{
	Param(
		[string]$Value,      # The value to ask the question on
		[string]$Question    # The question to ask
	)
	while ($AddChoice -notmatch "[Y|N]") {
		$AddChoice = Read-Host "`tAdd `"$Value`" to $Question`? (Y/N)"
		if ($AddChoice -eq "Y") {
			return $Value
		}
	}
}


####################################################################################################################################################################################
Function IIS_SMTP_InstallCheck()                            # Ask if user wants to enable the SMTP Virtual Server in IIS
{
	if ([string]::IsNullOrEmpty($dEnableIISsmtpSvc)) {
		$choice = ""
		while ($choice -notmatch "[y|n]") { $choice = read-host "`n Do you want to enable the Virtual SMTP Server in IIS`? (Y/N)" }
		if ($choice -eq "y") {
			$Script:dEnableIISsmtpSvc = $true
		}
	}
}


####################################################################################################################################################################################
Function IIS_SMTP_VirtualServer()                           # Configure and enable the SMTP Virtual Server in IIS for access from 127.0.0.1 ONLY
{
	Param(
		[switch]$Enable   # Ignore checks and enable the SMTP Virtual Server in IIS
	)

	if ($Enable -or $dEnableIISsmtpSvc) {
		# Install the IIS 6 WMI Compatibility so we can set the SMTP Virtual Server in IIS
		(Add-WindowsFeature -Name Web-WMI) | Out-Null
		Write-Host "`tConfiguring the SMTP Virtual Server in IIS for relay from 127.0.0.1" -ForegroundColor Green
		# Enable logging for the SMTP Virtual Server in IIS
		$smtpserversetting = get-wmiobject -namespace "root\MicrosoftIISv2" -computername localhost -Query "Select * from IIsSmtpServerSetting"
		$smtpserversetting.LogExtFileBytesRecv       = $true
		$smtpserversetting.LogExtFileBytesSent       = $true
		$smtpserversetting.LogExtFileClientIp        = $true
		$smtpserversetting.LogExtFileComputerName    = $true
		$smtpserversetting.LogExtFileCookie          = $true
		$smtpserversetting.LogExtFileDate            = $true
		$smtpserversetting.LogExtFileFlags           = "4194303"
		$smtpserversetting.LogExtFileHost            = $true
		$smtpserversetting.LogExtFileHttpStatus      = $true
		$smtpserversetting.LogExtFileHttpSubStatus   = $true
		$smtpserversetting.LogExtFileMethod          = $true
		$smtpserversetting.LogExtFileProtocolVersion = $true
		$smtpserversetting.LogExtFileReferer         = $true
		$smtpserversetting.LogExtFileServerIp        = $true
		$smtpserversetting.LogExtFileServerPort      = $true
		$smtpserversetting.LogExtFileSiteName        = $true
		$smtpserversetting.LogExtFileTime            = $true
		$smtpserversetting.LogExtFileTimeTaken       = $true
		$smtpserversetting.LogExtFileUriQuery        = $true
		$smtpserversetting.LogExtFileUriStem         = $true
		$smtpserversetting.LogExtFileUserAgent       = $true
		$smtpserversetting.LogExtFileUserName        = $true
		$smtpserversetting.LogExtFileWin32Status     = $true
		$smtpserversetting.LogFilePeriod             = "1"
		$smtpserversetting.LogFileTruncateSize       = "19922944"
		$smtpserversetting.LogType                   = "1"
		($smtpserversetting.put()) | Out-Null
		# Configure the Reverse DNS for the SMTP Virtual Server in IIS
		$smtpserversetting.DoMasquerade             = $true
		$smtpserversetting.FullyQualifiedDomainName = $dFQDNthisMachine
		$smtpserversetting.MasqueradeDomain         = $dFQDNthisMachine
		($smtpserversetting.put()) | Out-Null
		# Allow AuthAnonymous access from 127.0.0.1 in the SMTP Virtual Server in IIS (Access >> Authentication)
		$smtpserversetting.AuthAnonymous = $true
		$smtpserversetting.AuthBasic     = $false
		$smtpserversetting.AuthNTLM      = $false
		# Enable Relay access for 127.0.0.1 in the SMTP Virtual Server in IIS (Access >> Relay Restrictions)
		$smtpserversetting.RelayForAuth  = "-1"
		$smtpserversetting.RelayIpList   = @( 24, 0, 0, 128, 32, 0, 0, 128, 60, 0, 0, 128, 68, 0, 0, 128, 1, 0, 0, 0, 76, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 76, 0, 0, 128, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 255, 255, 127, 0, 0, 1 )
		($smtpserversetting.put()) | Out-Null
		# Allow connections from 127.0.0.1 in the SMTP Virtual Server in IIS (Access >> Connection Control)
		$GetSMTP = Get-CimInstance -Namespace "root\MicrosoftIISv2" -Class "IIsIPSecuritySetting" -Filter "Name ='SmtpSvc/1'"
		if ($GetSMTP.GrantByDefault) {
			$GetSMTP.GrantByDefault = $false
		}
		if ($GetSMTP.ipgrant -notcontains "127.0.0.1, 255.255.255.255") {
			$GetSMTP.ipgrant += @("127.0.0.1, 255.255.255.255";)
		}
		Set-CimInstance -InputObject $GetSMTP
		# Set the SMTP Virtual Server to automatically start
		$smtpserversetting.ServerAutoStart = $true
		($smtpserversetting.put()) | Out-Null
		# Start the SMTP Virtual Server unless it is already running
		if ((Get-Service "SMTPSVC").Status -ne "running") {
			(Start-Service 'SMTPSVC' -WarningAction SilentlyContinue) | Out-Null
		}
		# Set the SMTP Virtual Server to automatically start if
		if ((Get-WmiObject -Class Win32_Service -Property StartMode -Filter "Name='SMTPSVC'").StartMode -ne "Auto") {
			Set-Service -Name "SMTPSVC" -StartupType Automatic
		}
	}
}


####################################################################################################################################################################################
Function ModifyRegistryKeys()           # Function to modify registry paths and keys only if they exist
{
	Param(
		[string]$Path,  # Specify the Registry Path
		[string]$Name,  # Specify Key Name
		[string]$Value, # Specify Key Value
		[string]$Type,  # Specify Key Type - Optional
		[switch]$Remove # Remove the Specified Registry Path or Key
	)

	if ($Remove) {
		if ($Path -and $Name) {
			if (Get-Itemproperty "$Path" | Select-Object -ExpandProperty "$Name" -ErrorAction SilentlyContinue) {
				(Remove-Itemproperty -LiteralPath "$Path" -Name "$Name" -Force) | Out-Null
			}
		}elseif ($Path -and (!($Name))) {
			if (Test-Path $Path) {
				(Remove-Item -Path $Path -Force) | Out-Null
			}
		}
	}else{
		if (!(Test-Path $Path )) {
			((New-Item -Path $Path -Force) | Out-Null) ; do {$dTestRegPath = (Test-Path $Path)} until ($dTestRegPath)
		}
		if ((Test-Path "$Path") -and ($Name)) {
			if ( (((Get-ItemProperty -LiteralPath $Path).psbase.members) | WHERE {$_.Name -eq "$Name"}).Name ) { # If key exists then modify it
				if ( (((Get-ItemProperty -LiteralPath $Path).psbase.members) | WHERE {$_.Name -eq "$Name"}).Value -is [system.array] ) { # Check if the key is an array
					$dKeyValue = ([Microsoft.Win32.RegistryKey]::OpenBaseKey('LocalMachine', 'Registry32')).OpenSubKey((($Path -replace "HKLM`:", "").Substring(1)), $true)
					if ( ($dKeyValue.GetValue($Name)) -ne ($Value -split ", ") ) {
						$dKeyValue.SetValue($Name, [string[]]($Value -split ", "), $Type)
					}
				}else{ # The key is not an array
					if ( ((((Get-ItemProperty -LiteralPath "$Path").psbase.members) | WHERE {$_.Name -eq "$Name"}).Value) -ne "$Value" ) { # modify it if the value if different
						(Set-ItemProperty -LiteralPath "$Path" -Name "$Name" -Value "$Value") | Out-Null
					}
				}
			}else{ # Add the key as it doesn't exist
				if ($Type) {
					(New-ItemProperty -LiteralPath "$Path" -Name "$Name" -Value "$Value" -Type "$Type") | Out-Null
				}else{
					(New-ItemProperty -LiteralPath "$Path" -Name "$Name" -Value "$Value") | Out-Null
				}
			}
		}
	}
}


####################################################################################################################################################################################
Function HardenIIS_SSL()           # Function to Check if the IIS SSL Registry fixes have already been applied
{
	Write-Host "`tChecking the SSL Security on this machine" -ForegroundColor Cyan
	if (!($dSCPiisSSLDateCheckFile)) { # Set the location of the IIS SSL file used to store the date of the last update unless it has already been definied in the user variables
		$dLocalDateFile = "C:\SolidCP\SSL_Fix_DO_NOT_DELETE.txt"
	}else{
		$dLocalDateFile = $dSCPiisSSLDateCheckFile
	}
	if ($dSCPqualitySSLlabsRating -eq "B") { # Check if B Rating is required for IIS SSL Security
		$dRemoteXMLdata = ([xml](New-Object System.Net.WebClient).DownloadString("$dSCPiisSSLratingURLb"))
	}else{ # Otherwise use A Rating for best IIS SSL Security
		$dRemoteXMLdata = ([xml](New-Object System.Net.WebClient).DownloadString("$dSCPiisSSLratingURLa"))
	}
	$dCurrentDate   = (Get-Date -Format "yyyy-MM-dd HH:mm")

	if (Test-Path "$dLocalDateFile") { # Check if the local file exists
		if ( ([IO.File]::ReadAllText("$dLocalDateFile")) -lt ( ($dRemoteXMLdata.SelectNodes("//RegistryKeys")).Updated) ) {
			$dApplySSLfix = $true
		}
	}else{ # The file does not exist
		$dApplySSLfix = $true
	}

	if ($dApplySSLfix) { # Update the local file with todays date so we know the updates have been applied
		Write-Host "`t Updating the SSL Security on this machine" -ForegroundColor Green
		foreach ( $Key in ($dRemoteXMLdata.SelectNodes("//RegistryKeys/RegEntry/Key")) ) {
			if ($Key.attributes['Remove'].value) { # Check if the key is to be removed
				if ($Key.attributes['Name'].value -ne $null) { # Remove the Key Name if specified
					ModifyRegistryKeys -Path ($Key.attributes['Path'].value -replace "/", "$([char]0x2215)") -Name $Key.attributes['Name'].value -Remove
				}else{ # Remove the Key Path if no name is specified
					ModifyRegistryKeys -Path ($Key.attributes['Path'].value -replace "/", "$([char]0x2215)") -Remove
				}
			}elseif ($Key.attributes['Array'].value) { # Check if the Value is an Array
				if ($Key.attributes['Type'].value) { # Check if the Registry Type is specified
					ModifyRegistryKeys -Path ($Key.attributes['Path'].value -replace "/", "$([char]0x2215)") -Name $Key.attributes['Name'].value -Value $Key.attributes['Value'].value -Type $Key.attributes['Type'].value
				}else{
					ModifyRegistryKeys -Path ($Key.attributes['Path'].value -replace "/", "$([char]0x2215)") -Name $Key.attributes['Name'].value -Value $Key.attributes['Value'].value
				}
			}else{ # Otherwise treat as default 
				if ($Key.attributes['Type'].value) { # Check if the Registry Type is specified
					ModifyRegistryKeys -Path ($Key.attributes['Path'].value -replace "/", "$([char]0x2215)") -Name $Key.attributes['Name'].value -Value $Key.attributes['Value'].value -Type $Key.attributes['Type'].value
				}else{
					ModifyRegistryKeys -Path ($Key.attributes['Path'].value -replace "/", "$([char]0x2215)") -Name $Key.attributes['Name'].value -Value $Key.attributes['Value'].value
				}
			}
		}
		# Remove all existing Ciphers to ensure the corect ones are active in the registry
		((gci ('HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers')).PsPath | foreach { if($_){Remove-Item $_ -Force} }) | Out-Null
		$InsecureCiphers = ($dRemoteXMLdata.SelectNodes("//RegistryKeys/Ciphers/InsecureCiphers"))
		foreach ($InsecureCipher in $InsecureCiphers.Name) { # Update the Insecure Ciphers in the Registry
			$key = (Get-Item HKLM:\).OpenSubKey('SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers', $true).CreateSubKey($insecureCipher)
			New-ItemProperty -path "HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\$insecureCipher" -name 'Enabled' -value '0' -PropertyType 'DWord' -Force | Out-Null
			$key.close()
		}
		$SecureCiphers   = ($dRemoteXMLdata.SelectNodes("//RegistryKeys/Ciphers/SecureCiphers"))
		foreach ($SecureCipher in $SecureCiphers.Name) { # Update the Secure Ciphers in the Registry
			$key = (Get-Item HKLM:\).OpenSubKey('SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers', $true).CreateSubKey($secureCipher)
			New-ItemProperty -path "HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\$secureCipher" -name 'Enabled' -value '0xffffffff' -PropertyType 'DWord' -Force | Out-Null
			$key.close()
		}
		# Get the correct Cipher Suite Orders for this server depending upon Operating System Version
		if ([System.Version](Get-WmiObject -class Win32_OperatingSystem).Version -lt [System.Version]'10.0') {
			$cipherSuitesOrder = @($dRemoteXMLdata.SelectNodes("//RegistryKeys/CipherOrder2012R2andBelow/Order").Name)
		}else{
			$cipherSuitesOrder  = @($dRemoteXMLdata.SelectNodes("//RegistryKeys/CipherOrder2016/Order").Name)
		}
		if ($cipherSuitesOrder) { # Set the Cipher Suites Order
			New-Item 'HKLM:\SOFTWARE\Policies\Microsoft\Cryptography\Configuration\SSL\00010002' -ErrorAction SilentlyContinue
			New-ItemProperty -path 'HKLM:\SOFTWARE\Policies\Microsoft\Cryptography\Configuration\SSL\00010002' -name 'Functions' -value $([string]::join(',', $cipherSuitesOrder)) -PropertyType 'String' -Force | Out-Null
			New-Item 'HKLM:\SYSTEM\CurrentControlSet\Control\Cryptography\Configuration\Local\SSL\00010002' -ErrorAction SilentlyContinue
			New-ItemProperty -path 'HKLM:\SYSTEM\CurrentControlSet\Control\Cryptography\Configuration\Local\SSL\00010002' -name 'Functions' -value $cipherSuitesOrder -PropertyType 'MultiString' -Force | Out-Null
		}
		if ( !(Test-Path ($dLocalDateFile -replace ($dLocalDateFile.Split("\")[-1]))) ) { # Create the directory if it doesn't exist
			(New-Item ($dLocalDateFile -replace ($dLocalDateFile.Split("\")[-1])) -ItemType "Directory") | Out-Null
		}
		$dCurrentDate | Out-File -FilePath "$dLocalDateFile"
		if ((Get-Service "World Wide Web Publishing Service").Status -ne "running") { # Start IIS if it is not already running
			(Start-Service 'World Wide Web Publishing Service' -WarningAction SilentlyContinue) | Out-Null
			start-Sleep -Seconds 5
		}elseif ((Get-Service "World Wide Web Publishing Service").Status -eq "running") { # ReStart IIS if it is already running
			(Restart-Service 'World Wide Web Publishing Service' -Force -WarningAction SilentlyContinue) | Out-Null
			start-Sleep -Seconds 5
		}
		if ($ddSCPqualySSLScheduleTsk) { # Create the Scheduled Task if enabled in the user settings at the top of this script
			Write-Host "`t Creating the SSL Security Check as a Scheduled Task" -ForegroundColor Green
			CreateIISsslScheduledTask
		}
		Write-Host "`t The SSL Security has been updated on this machine" -ForegroundColor Green
		start-Sleep -Seconds 5
	}
}


####################################################################################################################################################################################
Function CreateIISsslScheduledTask()           # Function to create a Scheduled Task to automatically Check if the IIS SSL Registry fixes have already been applied
{
	$dSCPiisSSLAutoScript = @'
<####################################################################################################
SolidSCP - IIS SSL Hardening

v1.0    4th  October 2016:   First release of the SolidCP IIS SSL Hardeing Script

Written By Marc Banyard for the SolidCP Project (c) 2016 SolidCP

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
CLS
Write-Host "
        ****************************************
        *                                      *
        *        Welcome to the SolidCP        *
        *       IIS SSL Hardening Script       *
        *                                      *
        *       Please be patient whilst       *
        *        the server is patched         *
        *                                      *
        ****************************************" -ForegroundColor Green
####################################################################################################################################################################################
Function ModifyRegistryKeys()           # Function to modify registry paths and keys only if they exist
{
    Param(
        [string]$Path,  # Specify the Registry Path
        [string]$Name,  # Specify Key Name
        [string]$Value, # Specify Key Value
        [string]$Type,  # Specify Key Type - Optional
        [switch]$Remove # Remove the Specified Registry Path or Key
    )

    if ($Remove) {
        if ($Path -and $Name) {
            if (Get-Itemproperty "$Path" | Select-Object -ExpandProperty "$Name" -ErrorAction SilentlyContinue) {
                (Remove-Itemproperty -LiteralPath "$Path" -Name "$Name" -Force) | Out-Null
            }
        }elseif ($Path -and (!($Name))) {
            if (Test-Path $Path) {
                (Remove-Item -Path $Path -Force) | Out-Null
            }
        }
    }else{
        if (!(Test-Path $Path )) {
            ((New-Item -Path $Path -Force) | Out-Null) ; do {$dTestRegPath = (Test-Path $Path)} until ($dTestRegPath)
        }
        if ((Test-Path "$Path") -and ($Name)) {
            if ( (((Get-ItemProperty -LiteralPath $Path).psbase.members) | WHERE {$_.Name -eq "$Name"}).Name ) { # If key exists then modify it
                if ( (((Get-ItemProperty -LiteralPath $Path).psbase.members) | WHERE {$_.Name -eq "$Name"}).Value -is [system.array] ) { # Check if the key is an array
                    $dKeyValue = ([Microsoft.Win32.RegistryKey]::OpenBaseKey('LocalMachine', 'Registry32')).OpenSubKey((($Path -replace "HKLM`:", "").Substring(1)), $true)
                    if ( ($dKeyValue.GetValue($Name)) -ne ($Value -split ", ") ) {
                        $dKeyValue.SetValue($Name, [string[]]($Value -split ", "), $Type)
                    }
                }else{ # The key is not an array
                    if ( ((((Get-ItemProperty -LiteralPath "$Path").psbase.members) | WHERE {$_.Name -eq "$Name"}).Value) -ne "$Value" ) { # modify it if the value if different
                        (Set-ItemProperty -LiteralPath "$Path" -Name "$Name" -Value "$Value") | Out-Null
                    }
                }
            }else{ # Add the key as it doesn't exist
                if ($Type) {
                    (New-ItemProperty -LiteralPath "$Path" -Name "$Name" -Value "$Value" -Type "$Type") | Out-Null
                }else{
                    (New-ItemProperty -LiteralPath "$Path" -Name "$Name" -Value "$Value") | Out-Null
                }
            }
        }
    }
}


####################################################################################################################################################################################
Function HardenIIS_SSL()           # Function to Check if the IIS SSL Registry fixes have already been applied
{
    $dLocalDateFile = "_SCP_Date_Check_File_"
    $dRemoteXMLdata = ([xml](New-Object System.Net.WebClient).DownloadString("_XML_Installer_Site_"))
    $dCurrentDate   = (Get-Date -Format "yyyy-MM-dd HH:mm")

    if (Test-Path "$dLocalDateFile") { # Check if the local file exists
        if ( ([IO.File]::ReadAllText("$dLocalDateFile")) -lt ( ($dRemoteXMLdata.SelectNodes("//RegistryKeys")).Updated) ) {
            $dApplySSLfix = $true
        }
    }else{ # The file does not exist
        $dApplySSLfix = $true
    }

    if ($dApplySSLfix) { # Update the local file with todays date so we know the updates have been applied
        foreach ( $Key in ($dRemoteXMLdata.SelectNodes("//RegistryKeys/RegEntry/Key")) ) {
            if ($Key.attributes['Remove'].value) { # Check if the key is to be removed
                if ($Key.attributes['Name'].value -ne $null) { # Remove the Key Name if specified
                    ModifyRegistryKeys -Path ($Key.attributes['Path'].value -replace "/", "$([char]0x2215)") -Name $Key.attributes['Name'].value -Remove
                }else{ # Remove the Key Path if no name is specified
                    ModifyRegistryKeys -Path ($Key.attributes['Path'].value -replace "/", "$([char]0x2215)") -Remove
                }
            }elseif ($Key.attributes['Array'].value) { # Check if the Value is an Array
                if ($Key.attributes['Type'].value) { # Check if the Registry Type is specified
                    ModifyRegistryKeys -Path ($Key.attributes['Path'].value -replace "/", "$([char]0x2215)") -Name $Key.attributes['Name'].value -Value $Key.attributes['Value'].value -Type $Key.attributes['Type'].value
                }else{
                    ModifyRegistryKeys -Path ($Key.attributes['Path'].value -replace "/", "$([char]0x2215)") -Name $Key.attributes['Name'].value -Value $Key.attributes['Value'].value
                }
            }else{ # Otherwise treat as default 
                if ($Key.attributes['Type'].value) { # Check if the Registry Type is specified
                    ModifyRegistryKeys -Path ($Key.attributes['Path'].value -replace "/", "$([char]0x2215)") -Name $Key.attributes['Name'].value -Value $Key.attributes['Value'].value -Type $Key.attributes['Type'].value
                }else{
                    ModifyRegistryKeys -Path ($Key.attributes['Path'].value -replace "/", "$([char]0x2215)") -Name $Key.attributes['Name'].value -Value $Key.attributes['Value'].value
                }
            }
        }
        # Remove all existing Ciphers to ensure the corect ones are active in the registry
        ((gci ('HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers')).PsPath | foreach { if($_){Remove-Item $_ -Force} }) | Out-Null
        $InsecureCiphers = ($dRemoteXMLdata.SelectNodes("//RegistryKeys/Ciphers/InsecureCiphers"))
        foreach ($InsecureCipher in $InsecureCiphers.Name) { # Update the Insecure Ciphers in the Registry
            $key = (Get-Item HKLM:\).OpenSubKey('SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers', $true).CreateSubKey($insecureCipher)
            New-ItemProperty -path "HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\$insecureCipher" -name 'Enabled' -value '0' -PropertyType 'DWord' -Force | Out-Null
            $key.close()
        }
        $SecureCiphers   = ($dRemoteXMLdata.SelectNodes("//RegistryKeys/Ciphers/SecureCiphers"))
        foreach ($SecureCipher in $SecureCiphers.Name) { # Update the Secure Ciphers in the Registry
            $key = (Get-Item HKLM:\).OpenSubKey('SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers', $true).CreateSubKey($secureCipher)
            New-ItemProperty -path "HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Ciphers\$secureCipher" -name 'Enabled' -value '0xffffffff' -PropertyType 'DWord' -Force | Out-Null
            $key.close()
        }
        # Get the correct Cipher Suite Orders for this server depending upon Operating System Version
        if ([System.Version](Get-WmiObject -class Win32_OperatingSystem).Version -lt [System.Version]'10.0') {
            $cipherSuitesOrder = @($dRemoteXMLdata.SelectNodes("//RegistryKeys/CipherOrder2012R2andBelow/Order").Name)
        }else{
            $cipherSuitesOrder  = @($dRemoteXMLdata.SelectNodes("//RegistryKeys/CipherOrder2016/Order").Name)
        }
        if ($cipherSuitesOrder) { # Set the Cipher Suites Order
            New-Item 'HKLM:\SOFTWARE\Policies\Microsoft\Cryptography\Configuration\SSL\00010002' -ErrorAction SilentlyContinue
            New-ItemProperty -path 'HKLM:\SOFTWARE\Policies\Microsoft\Cryptography\Configuration\SSL\00010002' -name 'Functions' -value $([string]::join(',', $cipherSuitesOrder)) -PropertyType 'String' -Force | Out-Null
            New-Item 'HKLM:\SYSTEM\CurrentControlSet\Control\Cryptography\Configuration\Local\SSL\00010002' -ErrorAction SilentlyContinue
            New-ItemProperty -path 'HKLM:\SYSTEM\CurrentControlSet\Control\Cryptography\Configuration\Local\SSL\00010002' -name 'Functions' -value $cipherSuitesOrder -PropertyType 'MultiString' -Force | Out-Null
        }
        if ( !(Test-Path ($dLocalDateFile -replace ($dLocalDateFile.Split("\")[-1]))) ) { # Create the directory if it doesn't exist
            (New-Item ($dLocalDateFile -replace ($dLocalDateFile.Split("\")[-1])) -ItemType "Directory") | Out-Null
        }
        $dCurrentDate | Out-File -FilePath "$dLocalDateFile"
        Write-Host "`t The SSL Security has been updated on this machine" -ForegroundColor Green
        start-Sleep -Seconds 5
    }
}

HardenIIS_SSL

'@

	if (!($dSCPiisSSLDateCheckFile)) { # Set the location of the IIS SSL file used to store the date of the last update unless it has already been definied in the user variables
		$dLocalDateFile = "C:\SolidCP\SSL_Fix_DO_NOT_DELETE.txt"
	}else{
		$dLocalDateFile = $dSCPiisSSLDateCheckFile
	}
	if ($dSCPqualitySSLlabsRating -eq "B") { # Check if B Rating is required for IIS SSL Security
		$dRemoteXMLfile = $dSCPiisSSLratingURLb
	}else{ # Otherwise use A Rating for best IIS SSL Security
		$dRemoteXMLfile = $dSCPiisSSLratingURLa
	}
	if (!($dDomainMember)) { # Check to see if the machine is NOT joined to a domain
		if (CheckGroupMembers "$dLangAdministratorGroup" "$dLoggedInUserName" "Local") { # Logged in user is a Local Administrator
			$dSSLtaskUser = $dLocalAdministratorSID
		}
	}elseif ( ($dDomainMember) -and (!($dLoggedInLocally)) ) {
		if (CheckGroupMembers "$dLangDomainAdminsGroup" "$dLoggedInUserName" "Domain") { # Logged in user is a Domain Administrator
			$dSSLtaskUser = $dDomainAdministratorSID
		}
	}

	# Create the IIS SSL Hardening PowerShell Script on the Server so we can run it as a Scheduled Task
	(New-Item "C:\SolidCP\IIS_SSL_Hardening.ps1" -type file -force -value "$((($dSCPiisSSLAutoScript -replace "_XML_Installer_Site_", $dRemoteXMLfile) -replace "_SCP_Date_Check_File_", $dLocalDateFile) -replace "`n", "`r`n")") | Out-Null
	# Create the IIS SSL Hardening Scheduled Task as XML Format so we can import it
	(New-Item "C:\SolidCP\IIS_SSL_Hardening-Scheduled-Task-Import.xml" -type file -force -value "<?xml version=`"1.0`" encoding=`"UTF-16`"?>`r`n<Task version=`"1.2`" xmlns=`"http://schemas.microsoft.com/windows/2004/02/mit/task`">`r`n  <RegistrationInfo>`r`n    <Date>2015-01-01T00:00:00.000000</Date>`r`n    <Author>SolidCP</Author>`r`n    <Description>PowerShell file that runs Daily to fix any new SSL Security Issues from and XML file on the SolidCP Installer Site.</Description>`r`n  </RegistrationInfo>`r`n  <Triggers>`r`n    <CalendarTrigger>`r`n      <Repetition>`r`n        <Interval>P1D</Interval>`r`n        <Duration>P1D</Duration>`r`n        <StopAtDurationEnd>false</StopAtDurationEnd>`r`n      </Repetition>`r`n      <StartBoundary>2015-01-01T00:00:00</StartBoundary>`r`n      <ExecutionTimeLimit>PT1H</ExecutionTimeLimit>`r`n      <Enabled>true</Enabled>`r`n      <ScheduleByDay>`r`n        <DaysInterval>1</DaysInterval>`r`n      </ScheduleByDay>`r`n    </CalendarTrigger>`r`n  </Triggers>`r`n  <Principals>`r`n    <Principal id=`"Author`">`r`n      <RunLevel>HighestAvailable</RunLevel>`r`n      <UserId>$dSSLtaskUser</UserId>`r`n      <LogonType>S4U</LogonType>`r`n    </Principal>`r`n  </Principals>`r`n  <Settings>`r`n    <MultipleInstancesPolicy>IgnoreNew</MultipleInstancesPolicy>`r`n    <DisallowStartIfOnBatteries>false</DisallowStartIfOnBatteries>`r`n    <StopIfGoingOnBatteries>false</StopIfGoingOnBatteries>`r`n    <AllowHardTerminate>true</AllowHardTerminate>`r`n    <StartWhenAvailable>true</StartWhenAvailable>`r`n    <RunOnlyIfNetworkAvailable>false</RunOnlyIfNetworkAvailable>`r`n    <IdleSettings>`r`n      <StopOnIdleEnd>true</StopOnIdleEnd>`r`n      <RestartOnIdle>false</RestartOnIdle>`r`n    </IdleSettings>`r`n    <AllowStartOnDemand>true</AllowStartOnDemand>`r`n    <Enabled>true</Enabled>`r`n    <Hidden>false</Hidden>`r`n    <RunOnlyIfIdle>false</RunOnlyIfIdle>`r`n    <WakeToRun>false</WakeToRun>`r`n    <ExecutionTimeLimit>PT12H</ExecutionTimeLimit>`r`n    <Priority>7</Priority>`r`n    <RestartOnFailure>`r`n      <Interval>PT6H</Interval>`r`n      <Count>4</Count>`r`n    </RestartOnFailure>`r`n  </Settings>`r`n  <Actions Context=`"Author`">`r`n    <Exec>`r`n      <Command>powershell.exe</Command>`r`n      <Arguments>-file `"C:\SolidCP\IIS_SSL_Hardening.ps1`"</Arguments>`r`n      <WorkingDirectory>C:\SolidCP</WorkingDirectory>`r`n    </Exec>`r`n  </Actions>`r`n</Task>`r`n") | Out-Null
	# Create the task and import the settings from the XML file we created above
	(schtasks /create /XML C:\SolidCP\IIS_SSL_Hardening-Scheduled-Task-Import.xml /tn "SolidCP - Check SSL Security") | Out-Null
	# Remove the file used to import the Scheduled Task
	(Remove-Item "C:\SolidCP\IIS_SSL_Hardening-Scheduled-Task-Import.xml" -Force) | Out-Null
}


####################################################################################################################################################################################
Function AddWebServerToDomainIISacco()  # Add the SCPServer-ComputerName account to the Domain Group called "IIS_IUSRS" only if the SCP Server component is installed
{
	if (!($dDomainMember)) { # Check to see if the machine is NOT joined to a domain
		if (!(Test-Path "C:\SolidCP\Server")) { # Make sure the Server Component is installed
			if ([bool](([ADSISearcher]"(sAMAccountName=SCPServer-$env:COMPUTERNAME)").FindOne())) { # Make sure the SCPServer-ComputerName Domain Account exists
				(AddDomainUserToDomainGroup "IIS_IUSRS" "SCPServer-$env:computerName") | Out-Null    # Add the "SCPServer-[ComputerName]" User to the Domain Group called "IIS_IUSRS"
			}
		}
	}
}


####################################################################################################################################################################################
Function InstallDisablePasswdComplex()  # Disable Password Complexity Policy – Local Security Policy
{
	Write-Host "`tDisabling Password Complexity Policy – Local Security Policy" -ForegroundColor Cyan
	(secedit /export /cfg c:\secpol.cfg) | Out-Null;
	((gc C:\secpol.cfg).replace("PasswordComplexity = 1", "PasswordComplexity = 0") | Out-File C:\secpol.cfg) | Out-Null;
	(secedit /configure /db c:\windows\security\local.sdb /cfg c:\secpol.cfg /areas SECURITYPOLICY) | Out-Null;
	rm -force c:\secpol.cfg -confirm:$false
}


####################################################################################################################################################################################
Function InstallIISforESandPortal()     # Function to install additional IIS features required by the SolidCP Enterprise Server and Portal
{
	Write-Host "`tInstalling the features required for the SolidCP Enterprise Server and Portal" -ForegroundColor Cyan
	# Install the additional features required for SolidCP to be able to send emails from the Enterpeise Server
	(Add-WindowsFeature -Name Web-Lgcy-Mgmt-Console, SMTP-Server) | Out-Null
	# Change the "Default Website" port from 80 to 8080 so that we can install SolidCP on Port 80
	if ( (Get-WebBinding -Name 'Default Web Site').bindingInformation -eq "*:80:" ) {
		Set-WebBinding -Name 'Default Web Site' -BindingInformation "*:80:" -PropertyName Port -Value 8080
	}
}


####################################################################################################################################################################################
Function InstallDNSServer()         # Function to install features for Microsoft DNS Server
{
	Write-Host "`tInstalling the features required for Microsoft DNS Server" -ForegroundColor Cyan
	# Install the additional features required for the Microsoft DNS Server
	(Add-WindowsFeature -Name DNS, RSAT-DNS-Server) | Out-Null
	# Remove the DNS Root Hints for security and to stop the DNS Servers being used in Amplification Attacks
	Get-DnsServerRootHint | Remove-DnsServerRootHint -force
	Set-DnsServerRecursion -Enable $false
	Set-DnsServerCache -LockingPercent 100
	(ReStart-Service "DNS Server") | Out-Null
}


####################################################################################################################################################################################
Function SplitStringReverseJoin()                           # Function to split a string and reverse it then join back together with parameters
{
	Param(
		[string]$String,        # The string to work with
		[string]$Split,         # The value to split the string with
		[string]$Join           # The value to join the string after reversing it
	)
	$StringSplit = $String.split("$Split")
	[array]::reverse($StringSplit)
	return $StringSplit -join "$Join"
}


####################################################################################################################################################################################
Function AddRDSserversToServerManager()                     # Function to add all of the RDS Servers to the Server Manager
{
	Param(
		[string]$OU           # The Organisational Unit in Active Directory to get a list of servers from
	)
	# Check to make sure the OU is not empty
	if (!($OU)) {
		Write-Host "`t You must specify the `"`$dRDSourganisationalUnit`" variable at the top of this script" -ForegroundColor Yellow
		dPressAnyKeyToExit
	# Check if the OU Exists in Active Directory
	}elseif ([adsi]::Exists("LDAP://OU=$(SplitStringReverseJoin -String $OU -Split '\\' -Join ",OU="),$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)")) {
		# Check if there are actually any servers in the specified OU
		if (((Get-ADComputer -LDAPFilter "(name=*)" -SearchBase "OU=$(SplitStringReverseJoin -String $OU -Split '\\' -Join ",OU="),$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)").Name).count -gt 0) {
			# Get a list of all servers in the specified Organisational Unit
			$ServerList = (Get-ADComputer -LDAPFilter "(name=*)" -SearchBase "OU=$(SplitStringReverseJoin -String $OU -Split '\\' -Join ",OU="),$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)").Name
			# Close the Server Manager if it is running on this machine
			get-process ServerManager  -ErrorAction SilentlyContinue | stop-process –force
			if (!(Test-Path "$env:APPDATA\Microsoft\Windows\ServerManager\ServerList.xml")) {
				if (!(Test-Path "$env:APPDATA\Microsoft\Windows\ServerManager\")) {
					(md -Path "$env:APPDATA\Microsoft\Windows\ServerManager\" -Force) | Out-Null
				}
				@"
<?xml version="1.0" encoding="utf-8"?>
<ServerList xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" localhostName="$([System.Net.Dns]::GetHostByName(($env:computerName)).HostName)" xmlns="urn:serverpool-schema">
    <ServerInfo name="$([System.Net.Dns]::GetHostByName(($env:computerName)).HostName)" status="2" lastUpdateTime="0001-01-01T00:00:00" locale="en-GB" />
</ServerList>
"@ -replace '\n', "`r`n" | Out-File -FilePath "$env:APPDATA\Microsoft\Windows\ServerManager\ServerList.xml" -Encoding ascii
			}
			# Get a list of all the servers in the specified Organisational Unit
			$file = get-item "$env:APPDATA\Microsoft\Windows\ServerManager\ServerList.xml"
			copy-item –path $file –destination $file-backup –force
			$xml = [xml] (get-content $file )
			foreach ($Server in $ServerList) {
				if ($xml.ServerList.ServerInfo.name -notcontains "$([System.Net.Dns]::GetHostByName(($Server)).HostName)") {
					$newserver = @($xml.ServerList.ServerInfo)[0].clone()
					$newserver.name = "$([System.Net.Dns]::GetHostByName(($Server)).HostName)"
					$newserver.lastUpdateTime = "0001-01-01T00:00:00"
					$newserver.status = "2"
					Write-Host "`t Adding the `"$Server`" server to the Server Manager" -ForegroundColor Green
					($xml.ServerList.AppendChild($newserver)) | Out-Null
				}
			}
			($xml.Save($file.FullName)) | Out-Null
			Write-Host "`t Starting Server Manager" -ForegroundColor Green
			(Start-Process ServerManager -WindowStyle Minimized -ErrorAction SilentlyContinue) | Out-Null
			Start-Sleep -Seconds 60
		}else{
			Write-Host "`t There are no servers in the `"$OU`" Organisational Unit`!" -ForegroundColor Yellow
			Write-Host "`t and add ALL Remote Desktop Servers that will form you RDS deployment to it" -ForegroundColor Yellow
			dPressAnyKeyToExit
		}
	}else{
		Write-Host "`t The `"$OU`" Organisational Unit does not exist in Active Directory`!" -ForegroundColor Yellow
		Write-Host "`t Please create the `"$OU`" Organisational Unit in Active Directory" -ForegroundColor Yellow
		Write-Host "`t and add ALL Remote Desktop Servers that will form you RDS deployment to it" -ForegroundColor Yellow
		dPressAnyKeyToExit
	}
}


####################################################################################################################################################################################
Function dCheckRDSconnectionBroker()                        # Function to check if the RDS Connection Broker has been set at the top of this script
{
	if (!($dRDSconnBroker)) {
		Write-Host "`t You have not defined the FQDN for your RDS Connection Broker" -ForegroundColor Yellow
		Write-Host "`t Please set the `"`$dRDSconnBroker`" variable at the top of this script" -ForegroundColor Yellow
		dPressAnyKeyToExit
	}
}


####################################################################################################################################################################################
Function dCheckRDGatewayFQDN()                              # Function to check if the RDS Gateway FQDN has been set at the top of this script
{
	if (!($dRDSGatewayFQDN)) {
		Write-Host "`t You have not defined the FQDN for your RD Gateway for Client Access" -ForegroundColor Yellow
		Write-Host "`t Please set the `"`$dRDSGatewayFQDN`" variable at the top of this script" -ForegroundColor Yellow
		dPressAnyKeyToExit
	}
	if (!([bool](Test-Connection "$dRDSGatewayFQDN" -ErrorAction SilentlyContinue))) { # Test to make sure the RD Gateway FQDN has been configured in DNS
		Write-Host "`t You need to configure the `"RD Gateway FQDN`" to point to your RD Gateway Servers" -ForegroundColor Yellow
		Write-Host "`t Make sure `"$dRDSGatewayFQDN`"" -ForegroundColor Yellow
		Write-Host "`t Resolves to all of the RD Gateway Servers IP Addresses" -ForegroundColor Yellow
		Write-Host "`t This will ensure high availability for your RD Gateway Servers" -ForegroundColor Yellow
	}
}


####################################################################################################################################################################################
Function CheckRDSinstalledRole()                            # Function to check if certain RDS Roles are installed on this machine
{
	Param(
		[switch]$ConnectionBroker, # Check if the RD Connection Broker Role is installed on this machine
		[switch]$WebAccess,        # Check if the RD Web Access Role is installed on this machine
		[switch]$Gateway,          # Check if the RD Gateway Role is installed on this machine
		[switch]$Licencing,        # Check if the RD Licencing Role is installed on this machine
		[switch]$RDserver          # Check if the RD Session Host Role is installed on this machine
	)

	(Import-Module RemoteDesktop) | Out-Null
	if ($ConnectionBroker) {
		[bool](Get-RDServer -ConnectionBroker "$dRDSconnBroker" -ErrorAction SilentlyContinue  | `WHERE {(($_.Roles -match "RDS-CONNECTION-BROKER") -and ($_.Server -eq $([System.Net.Dns]::GetHostByName($env:computerName).HostName)))})
	}elseif ($WebAccess) {
		[bool](Get-RDServer -ConnectionBroker "$dRDSconnBroker" -ErrorAction SilentlyContinue  | `WHERE {(($_.Roles -match "RDS-WEB-ACCESS") -and ($_.Server -eq $([System.Net.Dns]::GetHostByName($env:computerName).HostName)))})
	}elseif ($Gateway) {
		[bool](Get-RDServer -ConnectionBroker "$dRDSconnBroker" -ErrorAction SilentlyContinue  | `WHERE {(($_.Roles -match "RDS-GATEWAY") -and ($_.Server -eq $([System.Net.Dns]::GetHostByName($env:computerName).HostName)))})
	}elseif ($Licencing) {
		[bool](Get-RDServer -ConnectionBroker "$dRDSconnBroker" -ErrorAction SilentlyContinue  | `WHERE {(($_.Roles -match "RDS-LICENSING") -and ($_.Server -eq $([System.Net.Dns]::GetHostByName($env:computerName).HostName)))})
	}elseif ($RDserver) {
		[bool](Get-RDServer -ConnectionBroker "$dRDSconnBroker" -ErrorAction SilentlyContinue  | `WHERE {(($_.Roles -match "RDS-RD-SERVER") -and ($_.Server -eq $([System.Net.Dns]::GetHostByName($env:computerName).HostName)))})
	}

}


####################################################################################################################################################################################
Function CreateRDSConnectionBrokersGroup()                  # Create the "RDS Connection Brokers" security group if it does not exist and add this server to it
{
	# Install the AD PowerShell tools unless it is already installed
	if (!(Get-WindowsFeature RSAT-AD-PowerShell).Installed) {
		Write-Host "`t Installing the PowerShell Active Directory tools" -ForegroundColor Green
		(Add-WindowsFeature -Name RSAT-AD-PowerShell) | Out-Null
	}
	# Import the Active Directory PowerShell Module
	(Import-Module ActiveDirectory) | Out-Null
	# Create the "RDS Connection Brokers" Group in Active Directory if it doesn't exist
	if (!([bool]((Get-ADGroup -filter {name -eq "RDS Connection Brokers"}).Name))) {
		# Create the "RDS Connection Brokers" security group and add the "Administrators" and "Network Service" groups to it
		Write-Host "`t Creating the `"RDS Connection Brokers`" Security Group in Active Directory" -ForegroundColor Green
		(New-ADGroup -Name "RDS Connection Brokers" -SamAccountName "RDS Connection Brokers" -GroupCategory "Security" -GroupScope "Universal" -DisplayName "RDS Connection Brokers" -Description "Required for Microsoft Remote Desktop Services") | Out-Null
		# Check to make sure the group is created before adding members to it
		do { $dCheckRDSConnectionBrokersGroup = ([ADSI]("WinNT://$env:USERDNSDOMAIN/RDS Connection Brokers")) } until ($dCheckRDSConnectionBrokersGroup -ne $null)
		start-sleep -Seconds "25"
	}
	# Add this server to the "RDS Connection Brokers" Group in Active Directory if it is not already a mamber and the group exists
	if ( ([bool]((Get-ADGroup -filter {name -eq "RDS Connection Brokers"}).Name)) -and (!([bool](Get-ADGroupMember -Identity "RDS Connection Brokers" | where {$_.name -eq "$env:COMPUTERNAME"}))) ) {
		Write-Host "`t Adding this server to the `"RDS Connection Brokers`" Security Group" -ForegroundColor Green
		(Add-ADGroupMember -Identity "RDS Connection Brokers" -Members "$((Get-ADComputer -Identity $env:COMPUTERNAME).SamAccountName)") | Out-Null
	}
}


####################################################################################################################################################################################
Function InstallRDSconnectionBroker()                       # Function to install the RD Connection Broker on this machine
{
	# Install the AD PowerShell tools unless it is already installed
	if (!(Get-WindowsFeature RSAT-AD-PowerShell).Installed) {
		Write-Host "`t Installing the PowerShell Active Directory tools" -ForegroundColor Green
		(Add-WindowsFeature -Name RSAT-AD-PowerShell) | Out-Null
	}
	# Import the Active Directory PowerShell Module
	(Import-Module ActiveDirectory) | Out-Null
	# Import the Remote Desktop PowerShell Module
	(Import-Module RemoteDesktop) | Out-Null
	# Add all of the RDS Servers to the Server Manager on this machine, if none are in the OU exit this script.
	AddRDSserversToServerManager -OU $dRDSourganisationalUnit
	# Check to make sure the RD Connection Broker is not already installed on this machine
	if (!(CheckRDSinstalledRole -ConnectionBroker)) {
		# Check if there is already an RDS Connection Broker configured
		if ([bool](Get-RDServer -ConnectionBroker "$dRDSconnBroker" -ErrorAction SilentlyContinue)) {
			# Check to make sure this server is not already a RD Connection Broker
			if (! [bool](Get-RDServer -ConnectionBroker "$dRDSconnBroker" -ErrorAction SilentlyContinue | WHERE {( ($_.Server -eq "$dFQDNthisMachine") -and ($_.Roles -match "RDS-CONNECTION-BROKER") )}) ) {
				# Check to make sure the RD Connection Broker is Highly Available before installing another RD Connection Broker
				if ([bool](Get-RDConnectionBrokerHighAvailability -ConnectionBroker "$dRDSconnBroker")) {
					# RD Connection Broker is already on the network, add this server as an additional RD Connection Broker Server
					Write-Host "`t Adding this server as an `"RD Connection Broker`" Server" -ForegroundColor Green
					(Add-RDServer -Server "$dFQDNthisMachine" -Role "RDS-CONNECTION-BROKER" -ConnectionBroker "$dRDSconnBroker") | Out-Null
				}else{
					Write-Host "`t You must configure `"High Availability`" on your existing RD Connection Broker" -ForegroundColor Yellow
					Write-Host "`t Please convert your existing RD Connection Broker and re-run this script" -ForegroundColor Yellow
					dPressAnyKeyToExit
				}
			}

			# Check if this server will be an "RD Web Access" Server
			if (!(CheckRDSinstalledRole -WebAccess)) {
				$choiceRDSwa = ""
				while ($choiceRDSwa -notmatch "[y|n]") { $choiceRDSwa = read-host "`tWill this server be an `"RD Web Access`" Server`? (Y/N)" }
				if ($choiceRDSwa -eq "y") {
					Write-Host "`t Adding this server as an `"RD Web Access`" Server" -ForegroundColor Green
					(Add-RDServer -Server "$dFQDNthisMachine" -Role "RDS-WEB-ACCESS" -ConnectionBroker "$dRDSconnBroker") | Out-Null
				}
			}
			# Check if this server will be an "RD Gateway" server
			if (!(CheckRDSinstalledRole -Gateway)) {
				$choiceRDSgw = ""
				while ($choiceRDSgw -notmatch "[y|n]") { $choiceRDSgw = read-host "`tWill this server be an `"RD Gateway`" Server`? (Y/N)" }
				if ($choiceRDSgw -eq "y") {
					Write-Host "`t Adding this server as an `"RD Gateway`" Server" -ForegroundColor Green
					(Add-RDServer -Server "$dFQDNthisMachine" -Role "RDS-GATEWAY" -ConnectionBroker "$dRDSconnBroker" -GatewayExternalFqdn "$dRDSGatewayFQDN") | Out-Null
					# Install the SolidCP Installer and then install the SolidCP Server component as this is required on the RD Gateway server only
					InstallSolidCPInstaller
					InstallSolidCPcomponent -Server
				}
			}
			Write-Host "`t This server will automatically reboot to complete the installation" -ForegroundColor Yellow
			Start-Sleep -Seconds 240
			Restart-Computer
		}else{ # No RD Connection Broker is configure, This server will be configured as the first RD Connection Broker
			# Double check to make sure this is the first RD Connection Broker in this deployment
			if ($dRDSconnBroker -eq $dFQDNthisMachine) {
				# Set the server as the " RD Connection Broker Server"
				$script:dRDSconnBrokerServer = $dFQDNthisMachine

				# Check if this server will also be an "RD Web Access" Server
				$choiceRDSwa = ""
				while ($choiceRDSwa -notmatch "[y|n]") { $choiceRDSwa = read-host "`tWill this server be an `"RD Web Access`" Server`? (Y/N)" }
				if ($choiceRDSwa -eq "n") {
					do { $script:dRDwebAccessServer = Read-Host "`tPlease enter the FQDN of the `"RD Web Access`" Server" }
					until (!([string]::IsNullOrEmpty($dRDwebAccessServer)))
				} else {
					$script:dRDwebAccessServer = $dFQDNthisMachine
				}

				# Check if this server will also be an "RD Session Host" Server
				$choiceRDScb = ""
				while ($choiceRDScb -notmatch "[y|n]") { $choiceRDScb = read-host "`tWill this server be an `"RD Session Host`" Server`? (Y/N)" }
				if ($choiceRDScb -eq "n") {
					do { $script:dRDSessionHostServer = Read-Host "`tPlease enter the FQDN of the `"RD Session Host`" Server" }
					until (!([string]::IsNullOrEmpty($dRDSessionHostServer)))
				} else {
					$script:dRDSessionHostServer = $dFQDNthisMachine
				}

				# Test to make sure each machine is reachable on the network before continuing
				if ( ([bool](Test-Connection "$dRDSconnBrokerServer" -Count 2 -ErrorAction SilentlyContinue)) -and ([bool](Test-Connection "$dRDwebAccessServer" -Count 2 -ErrorAction SilentlyContinue)) -and ([bool](Test-Connection "$dRDSessionHostServer" -Count 2 -ErrorAction SilentlyContinue)) ) {
					Write-Host "`t RD Connection Broker = $dRDSconnBrokerServer" -ForegroundColor Green
					Write-Host "`t RD Web Access = $dRDwebAccessServer" -ForegroundColor Green
					Write-Host "`t RD Session Host = $dRDSessionHostServer" -ForegroundColor Green

					# Configure the RD Session Deployment with the first RD Connection Broker, RD Web Access Server and RD Session Host
					Write-Host "`t    ****************************************" -ForegroundColor Green
					Write-Host "`t    *                                      *" -ForegroundColor Green
					Write-Host "`t    *     This will take quite a while     *" -ForegroundColor Green
					Write-Host "`t    *           to fully complete          *" -ForegroundColor Green
					Write-Host "`t    *                                      *" -ForegroundColor Green
					Write-Host "`t    *  Please be patient while we install  *" -ForegroundColor Green
					Write-Host "`t    *       Remote Desktop Services        *" -ForegroundColor Green
					Write-Host "`t    *                                      *" -ForegroundColor Green
					Write-Host "`t    ****************************************" -ForegroundColor Green
					Write-Host "`t Starting the installation of the Remote Desktop Connection Broker" -ForegroundColor Green
					Write-Host "`t The `"RD Session Host`" will reboot automatically once the roles have been installed" -ForegroundColor Yellow
					Write-Host "`t If not please reboot the server once this one has rebooted" -ForegroundColor Yellow
					(New-SessionDeployment –ConnectionBroker "$(($dRDSconnBrokerServer).ToLower())" –WebAccessServer "$(($dRDwebAccessServer).ToLower())" –SessionHost "$(($dRDSessionHostServer).ToUpper())") | Out-Null

					# If this server is not a RD Session Host server we can ask if it needs to be an RD Gateway server
					if ($dRDSessionHostServer -ne $dFQDNthisMachine) {
						$choiceRDSgw = ""
						while ($choiceRDSgw -notmatch "[y|n]") { $choiceRDSgw = read-host "`tWill this server be an `"RD Gateway`" Server`? (Y/N)" }
						if ($choiceRDSgw -eq "y") {
							Write-Host "`t Adding this server as an `"RD Gateway`" Server" -ForegroundColor Green
							(Add-RDServer -Server "$(($dFQDNthisMachine).ToLower())" -Role "RDS-GATEWAY" -ConnectionBroker "$dRDSconnBroker" -GatewayExternalFqdn "$dRDSGatewayFQDN") | Out-Null
							# Install the SolidCP Installer and then install the SolidCP Server component as this is required on the RD Gateway server only
							InstallSolidCPInstaller
							InstallSolidCPcomponent -Server
						}
					}
					Write-Host "`t This server will automatically reboot to complete the installation" -ForegroundColor Yellow
					start-sleep -Seconds 240
					Restart-Computer
				}else{ # Alert the user that one of the machines was not reachable
					if (!([bool](Test-Connection "$dRDSconnBrokerServer" -Count 2 -ErrorAction SilentlyContinue))) {
						Write-Host "`t`"$dRDSconnBrokerServer`" is not reachable from this server`!" -ForegroundColor Yellow
						Write-Host "`tPlease check the firewall settings and run this script again" -ForegroundColor Yellow
						dPressAnyKeyToExit
					}
					if (!([bool](Test-Connection "$dRDwebAccessServer" -Count 2 -ErrorAction SilentlyContinue))) {
						Write-Host "`t`"$dRDwebAccessServer`" is not reachable from this server`!" -ForegroundColor Yellow
						Write-Host "`tPlease check the firewall settings and run this script again" -ForegroundColor Yellow
						dPressAnyKeyToExit
					}
					if (!([bool](Test-Connection "$dRDSessionHostServer" -Count 2 -ErrorAction SilentlyContinue))) {
						Write-Host "`t`"$dRDSessionHostServer`" is not reachable from this server`!" -ForegroundColor Yellow
						Write-Host "`tPlease check the firewall settings and run this script again" -ForegroundColor Yellow
						dPressAnyKeyToExit
					}
				}
			}else{ # The server needst to be rebooted before the RD Connection Broker role can be installed
				Write-Host "`t This server will automatically reboot" -ForegroundColor Yellow
				Write-Host "`t Please re-run this script again once the server has rebooted" -ForegroundColor Yellow
				start-sleep -Seconds 10
				Restart-Computer
			}
		}
	}
}


####################################################################################################################################################################################
Function InstallRDLicencingServer()                         # Function to install the RD Licencing Server on this machine
{
	if ([bool](Test-Connection "$dRDSGatewayFQDN" -ErrorAction SilentlyContinue)) { # Test to make sure the RD Gateway FQDN has been configured in DNS
		# Install the AD PowerShell tools unless it is already installed
		if (!(Get-WindowsFeature RSAT-AD-PowerShell).Installed) {
			Write-Host "`t Installing the PowerShell Active Directory tools" -ForegroundColor Green
			(Add-WindowsFeature -Name RSAT-AD-PowerShell) | Out-Null
		}
		# Import the Active Directory PowerShell Module
		(Import-Module ActiveDirectory) | Out-Null
		# Import the Remote Desktop PowerShell Module
		(Import-Module RemoteDesktop) | Out-Null
		# Add all of the RDS Servers to the Server Manager on this machine, if none are in the OU exit this script.
		AddRDSserversToServerManager -OU $dRDSourganisationalUnit
		# Check to make sure the RD Licencing Server is not already installed on this machine
		if (!(CheckRDSinstalledRole -Licencing)) {
			# Check if there is already an RDS Connection Broker configured
			if ([bool](Get-RDServer -ConnectionBroker "$dRDSconnBroker" -ErrorAction SilentlyContinue)) {
				# Add the "RDS Connection Brokers" domain group to the SQL Server as a DB Creator
				Write-Host "`t Adding the `"$env:USERDOMAIN\RDS Connection Brokers`" Security Group to SQL Server" -ForegroundColor Green
				[System.Reflection.Assembly]::LoadWithPartialName('Microsoft.SqlServer.SMO') | out-null
				$dSQLserver = New-Object ('Microsoft.SqlServer.Management.Smo.Server') "$env:COMPUTERNAME"
				if (!($dSQLserver.Logins.Contains("$env:USERDOMAIN\RDS Connection Brokers"))) {
					$dSQLserverUser = New-Object -TypeName Microsoft.SqlServer.Management.Smo.Login $env:COMPUTERNAME, "$env:USERDOMAIN\RDS Connection Brokers"
					$dSQLserverUser.LoginType = "WindowsUser"
					$dSQLserverUser.Create()
					$dSQLserverRole = $dSQLserver.Roles | where {$_.Name -eq "sysadmin"}
					$dSQLserverRole.AddMember($dSQLserverUser.Name)
				}
				Write-Host "`t    ****************************************" -ForegroundColor Green
				Write-Host "`t    *                                      *" -ForegroundColor Green
				Write-Host "`t    *     This will take quite a while     *" -ForegroundColor Green
				Write-Host "`t    *           to fully complete          *" -ForegroundColor Green
				Write-Host "`t    *                                      *" -ForegroundColor Green
				Write-Host "`t    *  Please be patient while we install  *" -ForegroundColor Green
				Write-Host "`t    *       Remote Desktop Services        *" -ForegroundColor Green
				Write-Host "`t    *                                      *" -ForegroundColor Green
				Write-Host "`t    ****************************************" -ForegroundColor Green
				Write-Host "`t Configuring this server as an `"RD Licencing Server`"" -ForegroundColor Green
				# Add this server as an RD Licencing Server to an existing deployment
				#(Add-RDServer -Server "$(($dFQDNthisMachine).ToLower())" -Role "RDS-LICENSING" -ConnectionBroker "$dRDSconnBroker") | Out-Null
				(Add-RDServer -Server "$(($dFQDNthisMachine).ToLower())" -Role "RDS-LICENSING" -ConnectionBroker "$dRDSconnBroker") | Out-Null

				# Alert the user that the Licencing Server has been installed, but they now need to Manually activate it and add the licences via the Licencing Manager GUI
				Write-Host "`t The Licencing Server has been installed" -ForegroundColor Yellow
				Write-Host "`t You will now need to manually `"Activate`" this server and add the RDS Licences" -ForegroundColor Yellow
				Write-Host "`n`t Please open the `"Licencing Manager GUI`" to do this before continuing" -ForegroundColor Yellow
				Write-Host "`n`t Press any key once you have completed the above" -ForegroundColor Yellow
				dPressAnyKeyToContinue
				dPressAnyKeyToContinue
				# Once the Licencing Server has been activated and the CAL's added we can continue
				Write-Host "`t Setting the Licencing Mode as `"Per User`"" -ForegroundColor Green
				(Set-RDLicenseConfiguration -LicenseServer "$(($dFQDNthisMachine).ToLower())" -Mode "PerUser" -ConnectionBroker "$dRDSconnBroker" -Force) | Out-Null

				# Don't continue until the Licencing Model has been configured as "PerUser"
				do {Start-Sleep -Milliseconds 500} until ((Get-RDLicenseConfiguration -ConnectionBroker "$dRDSconnBroker" -ErrorAction SilentlyContinue).Mode -eq "PerUser")
				# Check if High Availability is to be configured for the RDS deployment
				$choiceRDSha = ""
				while ($choiceRDSha -notmatch "[y|n]") { $choiceRDSha = read-host "`tWould you like to enable `"RD Connection Broker High Availability`"`? (Y/N)" }
				if ($choiceRDSha -eq "y") {
					# Now configure the High Availability for the RDS deployment
					Write-Host "`t Configuring RD Connection Broker High Availability" -ForegroundColor Green
					#(Set-RDConnectionBrokerHighAvailability -ConnectionBroker "$dRDSconnBroker" -DatabaseConnectionString "DRIVER=SQL Server Native Client 11.0;SERVER=$env:COMPUTERNAME;Trusted_Connection=Yes;APP=Remote Desktop Services Connection Broker;DATABASE=RDConnectionBroker" -DatabaseFilePath "C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\RDConnectionBroker.mdf" -ClientAccessName "$dRDSGatewayFQDN" -ErrorAction SilentlyContinue) | Out-Null
					(Set-RDConnectionBrokerHighAvailability -ConnectionBroker "$dRDSconnBroker" -DatabaseConnectionString "DRIVER=SQL Server Native Client 11.0;SERVER=$env:COMPUTERNAME\SQLExpress,1433;Trusted_Connection=Yes;APP=Remote Desktop Services Connection Broker;DATABASE=RDConnectionBroker" -DatabaseFilePath "C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\RDConnectionBroker.mdf" -ClientAccessName "$dRDSGatewayFQDN" -ErrorAction SilentlyContinue) | Out-Null
				}
				Write-Host "`t This server will automatically reboot to complete the installation" -ForegroundColor Yellow
				Start-Sleep -Seconds 240
				Restart-Computer
			}else{
				Write-Host "`t You need to configure an RD Connection Broker before you can add an RD Licencing Server" -ForegroundColor Yellow
			}
		}
	}else{ # Warn the user that the DNS Round Robin has not been setup to point to the RD Gateway Servers
		Write-Host "`t You need to configure the `"RD Gateway FQDN`" to point to your RD Gateway Servers" -ForegroundColor Yellow
		Write-Host "`t Make sure `"$dRDSGatewayFQDN`"" -ForegroundColor Yellow
		Write-Host "`t Resolves to all of the RD Gateway Servers IP Addresses" -ForegroundColor Yellow
		Write-Host "`t This will ensure high availability for your RD Gateway Servers" -ForegroundColor Yellow
	}
}


####################################################################################################################################################################################
Function InstallRDGateway()                                 # Function to install the RD Gateway on this machine
{
	if ([bool](Compare-Object ([System.Net.Dns]::GetHostAddresses("$dFQDNthisMachine").IPAddressToString -notlike 'fe80:*') ([System.Net.Dns]::GetHostAddresses("$dRDSGatewayFQDN").IPAddressToString -notlike 'fe80:*') -IncludeEqual -ExcludeDifferent -PassThru)) {
		# Install the AD PowerShell tools unless it is already installed
		if (!(Get-WindowsFeature RSAT-AD-PowerShell).Installed) {
			Write-Host "`t Installing the PowerShell Active Directory tools" -ForegroundColor Green
			(Add-WindowsFeature -Name RSAT-AD-PowerShell) | Out-Null
		}
		# Import the Active Directory PowerShell Module
		(Import-Module ActiveDirectory) | Out-Null
		# Import the Remote Desktop PowerShell Module
		(Import-Module RemoteDesktop) | Out-Null
		# Add all of the RDS Servers to the Server Manager on this machine, if none are in the OU exit this script.
		AddRDSserversToServerManager -OU $dRDSourganisationalUnit
		# Check to make sure the RD Gateway is not already installed on this machine
		if (!(CheckRDSinstalledRole -Gateway)) {
			# Check if there is already an RDS Connection Broker configured
			if ([bool](Get-RDServer -ConnectionBroker "$dRDSconnBroker" -ErrorAction SilentlyContinue)) {
				# Add this server as an RD Gateway Server to an existing deployment
				Write-Host "`t Adding this server as an `"RD Gateway`" Server" -ForegroundColor Green
				(Add-RDServer -Server "$(($dFQDNthisMachine).ToLower())" -Role "RDS-GATEWAY" -ConnectionBroker "$dRDSconnBroker" -GatewayExternalFqdn "$dRDSGatewayFQDN") | Out-Null
				Write-Host "`t This server will automatically reboot to complete the installation" -ForegroundColor Yellow
				Start-Sleep -Seconds 240
				Restart-Computer
			}else{
				Write-Host "`t You need to configure an RD Connection Broker before you can add an RD Gateway" -ForegroundColor Yellow
			}
		}
	}else{
		Write-Host "`t You need to add this server to the DNS Round Robin entry for `"$dRDSGatewayFQDN`"" -ForegroundColor Yellow
		Write-Host "`t Please run this script again once you have created the DNS Entries" -ForegroundColor Yellow
		dPressAnyKeyToExit
	}
}


####################################################################################################################################################################################
Function InstallRDWebAccess()                               # Function to install the RD WebAccess on this machine
{
	# Install the AD PowerShell tools unless it is already installed
	if (!(Get-WindowsFeature RSAT-AD-PowerShell).Installed) {
		Write-Host "`t Installing the PowerShell Active Directory tools" -ForegroundColor Green
		(Add-WindowsFeature -Name RSAT-AD-PowerShell) | Out-Null
	}
	# Import the Active Directory PowerShell Module
	(Import-Module ActiveDirectory) | Out-Null
	# Import the Remote Desktop PowerShell Module
	(Import-Module RemoteDesktop) | Out-Null
	# Add all of the RDS Servers to the Server Manager on this machine, if none are in the OU exit this script.
	AddRDSserversToServerManager -OU $dRDSourganisationalUnit
	# Check to make sure the RD Gateway is not already installed on this machine
	if (!(CheckRDSinstalledRole -WebAccess)) {
		# Check if there is already an RDS Connection Broker configured
		if ([bool](Get-RDServer -ConnectionBroker "$dRDSconnBroker" -ErrorAction SilentlyContinue)) {
			# Add this server as an RD Gateway Server to an existing deployment
			Write-Host "`t Adding this server as an `"RD Web Access`" Server" -ForegroundColor Green
			(Add-RDServer -Server "$(($dFQDNthisMachine).ToLower())" -Role "RDS-WEB-ACCESS" -ConnectionBroker "$dRDSconnBroker") | Out-Null
			Write-Host "`t This server will automatically reboot to complete the installation" -ForegroundColor Yellow
			Start-Sleep -Seconds 240
			Restart-Computer
		}else{
			Write-Host "`t You need to configure an RD Connection Broker before you can add an RD Web Access Server" -ForegroundColor Yellow
		}
	}
}


####################################################################################################################################################################################
Function InstallRDSessionHost()                             # Function to install the RD Session Host on this machine
{
	# Import the Remote Desktop PowerShell Module
	(Import-Module RemoteDesktop) | Out-Null
	# Check to make sure the RD Gateway is not already installed on this machine
	if (!(CheckRDSinstalledRole -RDserver)) {
		# Check if there is already an RDS Connection Broker configured
		if ([bool](Get-RDServer -ConnectionBroker "$dRDSconnBroker" -ErrorAction SilentlyContinue)) {
			# Add this server as an RD Session Host Server to an existing deployment
			if (!(Get-WindowsFeature RDS-RD-Server).Installed) {
				Write-Host "`t Adding the `"RD Session Host`" Windows Feature" -ForegroundColor Green
				(Add-WindowsFeature -Name RDS-RD-Server -WarningAction SilentlyContinue) | Out-Null
				Write-Host "`t This server will automatically reboot" -ForegroundColor Yellow
				Write-Host "`t Please re-run this script again once the server has rebooted" -ForegroundColor Yellow
				Start-Sleep -Seconds 10
				Restart-Computer
			}elseif ((Get-WindowsFeature RDS-RD-Server).InstallState -eq "InstallPending") {
				Write-Host "`t This server requires a reboot" -ForegroundColor Yellow
				Write-Host "`t Please re-run this script again once the server has rebooted" -ForegroundColor Yellow
				Start-Sleep -Seconds 10
				Restart-Computer
			}else{
				Write-Host "`t Adding this server as an `"RD Session Host`" Server" -ForegroundColor Green
				(Add-RDServer -Server "$(($dFQDNthisMachine).ToLower())" -Role "RDS-RD-SERVER" -ConnectionBroker "$dRDSconnBroker") | Out-Null
				Write-Host "`t This server will automatically reboot to complete the installation" -ForegroundColor Yellow
				Start-Sleep -Seconds 60
				Restart-Computer
			}
		}else{
			Write-Host "`t You need to configure an RD Connection Broker before you can add an RD Session Host" -ForegroundColor Yellow
		}
	}
}


####################################################################################################################################################################################
Function ConfigureRDwebHTTPtoHTTPSredirect()                # Function to install the RD Session Host on this machine
{
	# Import the Remote Desktop PowerShell Module
	(Import-Module RemoteDesktop) | Out-Null
	# Check to make sure the RD Gateway is not already installed on this machine
	if (CheckRDSinstalledRole -WebAccess) {
		# Set the HTTP to HTTPS Redirection for the RDWeb
		Write-Host "`t Applying the HTTP to HTTPS Redirection for the RD Web Access" -ForegroundColor Green
		# Import the PowerShell Web Administration Module
		Import-Module WebAdministration
		if ($dRDSGatewayFQDN) {
			Set-WebConfiguration -Filter "system.webServer/httpRedirect" "IIS:\sites\Default Web Site" -Value @{enabled="true";destination="https://$dRDSGatewayFQDN/RDWeb";exactDestination="false";childOnly="true";httpResponseStatus="Found"}
		}elseif ( (!($dRDSGatewayFQDN)) -and ($dFQDNthisMachine) ){
			Set-WebConfiguration -Filter "system.webServer/httpRedirect" "IIS:\sites\Default Web Site" -Value @{enabled="true";destination="https://$dFQDNthisMachine/RDWeb";exactDestination="false";childOnly="true";httpResponseStatus="Found"}
		}
	}
}


####################################################################################################################################################################################
Function InstallActiveDirectory()   # Function to install features for Active Directory
{
	# ********** Domain Member, but signed in locally **********
	if ( ($dDomainMember) -and ($dLoggedInLocally) ) {
		Write-Host "`n`t *************************************************" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *      This server is a member of a domain      *" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *    You need to login with a domain account    *" -ForegroundColor Yellow
		Write-Host "`t *          to promote this server to a          *" -ForegroundColor Yellow
		Write-Host "`t *               Domain Controller               *" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *  Please log back on with an account that is   *" -ForegroundColor Yellow
		Write-Host "`t *      a member of the Domain Admins group      *" -ForegroundColor Yellow
		Write-Host "`t *           and run this script again           *" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *************************************************" -ForegroundColor Yellow
	}else{
		Write-Host "`tInstalling the features required for Active Directory" -ForegroundColor Cyan
		# Install the features required for this server to run Active Directory (Domain Controllers)
		(Add-WindowsFeature -Name AD-Domain-Services, RSAT-AD-Tools, RSAT-AD-PowerShell, RSAT-ADDS, RSAT-AD-AdminCenter, RSAT-ADDS-Tools, WDS-AdminPack -IncludeManagementTools) | Out-Null

		# Ask the user if they want to configure Active Directory on this server
		$InstallAD = ""; while ($InstallAD -notmatch "[y|n]") { $InstallAD = read-host "`t Do you want to configure Active Directory on this server`? (Y/N)" }
		if ($InstallAD -eq "n") { # If the user answers No to the question, let them know they must manually promote this server to a Domain Controller
			Write-Host "`n`t *************************************************" -ForegroundColor Yellow
			Write-Host "`t *                                               *" -ForegroundColor Yellow
			Write-Host "`t *   The Windows Features for Active Directory   *" -ForegroundColor Yellow
			Write-Host "`t *       have been successfully installed        *" -ForegroundColor Yellow
			Write-Host "`t *                                               *" -ForegroundColor Yellow
			Write-Host "`t *    You will need to manually promote this     *" -ForegroundColor Yellow
			Write-Host "`t *         server to a Domain Controller         *" -ForegroundColor Yellow
			Write-Host "`t *                                               *" -ForegroundColor Yellow
			Write-Host "`t *         We reccomend having at least          *" -ForegroundColor Yellow
			Write-Host "`t *      2 Domain Controllers for redundancy      *" -ForegroundColor Yellow
			Write-Host "`t *                                               *" -ForegroundColor Yellow
			Write-Host "`t *************************************************" -ForegroundColor Yellow
		}elseif ($InstallAD -eq "y") { # Configure Active Directory if the user answers Yes to the question
			# ********** First Domain Controller **********
			if (!$dDomainMember) {
				# Check with the user if they want to configure this server as the First Domain Controller
				$InstallADprimary = ""
				while ($InstallADprimary -notmatch "[y|n]") {
					Write-Host "`t This server is NOT a member of a domain!" -ForegroundColor Yellow
					$InstallADprimary = read-host "`t Do you want to configure this server as a Primary Domain Controller`? (Y/N)"
				}
				if ($InstallADprimary -eq "y") { # Configure this server as the Primary Domain Controller if the user answers Yes to the question
					# If a Domain Name has not been defined in the user variables ask the user to enter one
					if (!$dSCPdomainName) { $dSCPdomainName = Read-Host "Please enter the Domain Name you would like to use for Active Directory" }
					Write-Host "`t Configuring this server as the first Domain Controller`n" -ForegroundColor Green
					# Import the Active Directory Deployment Module
					Import-Module ADDSDeployment
					# Configure this server as the First Domain Controller in a new forest
					(Install-ADDSForest -CreateDnsDelegation:$false -DomainName "$dSCPdomainName" -InstallDns:$true -NoRebootOnCompletion:$true -Force:$true -WarningAction SilentlyContinue) | Out-Null
					Write-Host "`n`t *************************************************" -ForegroundColor Yellow
					Write-Host "`t *                                               *" -ForegroundColor Yellow
					Write-Host "`t *       This server has been successfully       *" -ForegroundColor Yellow
					Write-Host "`t *        promoted to a Domain Controller        *" -ForegroundColor Yellow
					Write-Host "`t *                                               *" -ForegroundColor Yellow
					Write-Host "`t *         We reccomend having at least          *" -ForegroundColor Yellow
					Write-Host "`t *      2 Domain Controllers for redundancy      *" -ForegroundColor Yellow
					Write-Host "`t *                                               *" -ForegroundColor Yellow
					Write-Host "`t *                                               *" -ForegroundColor Yellow
					Write-Host "`t *        The server will now be rebooted        *" -ForegroundColor Yellow
					Write-Host "`t *                                               *" -ForegroundColor Yellow
					Write-Host "`t *************************************************" -ForegroundColor Yellow
					Write-Host "`n`t Press any key to reboot this server" -ForegroundColor Green
					$dRestartConfirm = $host.ui.RawUI.ReadKey("NoEcho,IncludeKeyDown")
					if ($dRestartConfirm) {Restart-Computer}
				}
			# ********** Additional Domain Controller **********
			}elseif ( ($dDomainMember) -and (!$dLoggedInLocally) ) {
				# Check with the user if they want to configure this server as an Additional Domain Controller
				$InstallADsecondary = ""; while ($InstallADsecondary -notmatch "[y|n]") {
					Write-Host "`t This server is a member of a domain!" -ForegroundColor Yellow
					$InstallADsecondary = read-host "`t Do you want to configure this server as an Additional Domain Controller`? (Y/N)"
				}
				if ($InstallADsecondary -eq "y") { # Configure this server as an Additional Domain Controller if the user answers Yes to the question
					Write-Host "`t Configuring this server as an additional Domain Controller`n" -ForegroundColor Green
					# Import the Active Directory Deployment Module
					Import-Module ADDSDeployment
					# Configure this server as an additional Domain Controller in an existing forest
					if (CheckGroupMembers "$dLangDomainAdminsGroup" "$dLoggedInUserName" "Domain") {
						(Install-ADDSDomainController -CreateDnsDelegation:$false  -DomainName "$env:USERDNSDOMAIN" -InstallDns:$true -NoRebootOnCompletion:$true -Force:$true -WarningAction SilentlyContinue) | Out-Null
					}else{
						(Install-ADDSDomainController -CreateDnsDelegation:$false  -DomainName "$env:USERDNSDOMAIN" -InstallDns:$true -NoRebootOnCompletion:$true -Force:$true -Credential (Get-Credential "$env:USERDOMAIN\$dLangDomainAdministratorName") -WarningAction SilentlyContinue) | Out-Null
					}
					Write-Host "`n`t *************************************************" -ForegroundColor Yellow
					Write-Host "`t *                                               *" -ForegroundColor Yellow
					Write-Host "`t *       This server has been successfully       *" -ForegroundColor Yellow
					Write-Host "`t *  promoted to an additional Domain Controller  *" -ForegroundColor Yellow
					Write-Host "`t *                                               *" -ForegroundColor Yellow
					Write-Host "`t *                                               *" -ForegroundColor Yellow
					Write-Host "`t *        The server will now be rebooted        *" -ForegroundColor Yellow
					Write-Host "`t *                                               *" -ForegroundColor Yellow
					Write-Host "`t *************************************************" -ForegroundColor Yellow
					Write-Host "`n`t Press any key to reboot this server" -ForegroundColor Green
					$dRestartConfirm = $host.ui.RawUI.ReadKey("NoEcho,IncludeKeyDown")
					if ($dRestartConfirm) {Restart-Computer}
				}
			}
		}
	}
}


####################################################################################################################################################################################
Function InstallActivePerl()            # Function to install Perl - ActiverPerl v5.22.1
{
	# Check if Perl is installed, if not then install the correct version for the server
	if ( !(Test-Path "C:\Perl64") -or (Test-Path "C:\Perl\") ) {
		# Perl is not installed on this server so go ahead and install it
		Write-Host "`tDetermining your Operating System Type and installing the correct version of Perl" -ForegroundColor Cyan
		# Get the latest download information from the SolidCP Installer site
		($ActivePerl_x64 = (SolidCPFileDownload "ActivePerl_x64")) | Out-Null
		($ActivePerl_x32 = (SolidCPFileDownload "ActivePerl_x32")) | Out-Null
		# Create the Perl ActivePerl Directory in our Installation Files folder ready for downloading
		(md -Path "C:\_Install Files\$($ActivePerl_x64.FolderName)" -Force) | Out-Null ; cd "C:\_Install Files\$($ActivePerl_x64.FolderName)\"
		# Import the PowerShell Web Administration Module
		Import-Module WebAdministration
		# Download the files required for Perl, then install them. This downloads the correct version for the OS and then installs it.
		if ([Environment]::Is64BitProcess) {
			Write-Host "`t Downloading the 64bit version of Perl" -ForegroundColor Green
			(New-Object System.Net.WebClient).DownloadFile("$($ActivePerl_x64.DownloadURL)", "C:\_Install Files\$($ActivePerl_x64.FolderName)\$($ActivePerl_x64.FileName)") | Out-Null
			Write-Host "`t Installing the 64bit version of Perl" -ForegroundColor Green
			(Start-Process -FilePath "C:\_Install Files\$($ActivePerl_x64.FolderName)\$($ActivePerl_x64.FileName)" -Argumentlist "/install /silent /norestart" -Wait -Passthru).ExitCode | Out-Null
			# Add the Perl ISAPI and CGI Restrictions for Perl (64 bit)
			Write-Host "`t Adding the Perl ISAPI and CGI Restrictions in IIS" -ForegroundColor Green
			Add-WebConfiguration -pspath 'MACHINE/WEBROOT/APPHOST' -filter 'system.webServer/security/isapiCgiRestriction' -value @{description='Perl FastCGI' ; path= "C:\Perl64\bin\perl.exe `"%s`" %s" ; allowed='True'}
			# Add the Perl Handler Mappings for Perl (64 bit)
			Write-Host "`t Adding the Perl Handler Mappings in IIS" -ForegroundColor Green
			New-WebHandler -Name "Perl_Script_Map_PL" -Path "*.pl" -Verb '*' -Modules CgiModule -ScriptProcessor 'C:\Perl64\bin\perl.exe "%s" %s' -ResourceType File
			New-WebHandler -Name "Perl_Script_Map_CGI" -Path "*.cgi" -Verb '*' -Modules CgiModule -ScriptProcessor 'C:\Perl64\bin\perl.exe "%s" %s' -ResourceType File
		}else{
			Write-Host "`t Downloading the 32bit version of Perl" -ForegroundColor Green
			(New-Object System.Net.WebClient).DownloadFile("$($ActivePerl_x32.DownloadURL)", "C:\_Install Files\$($ActivePerl_x32.FolderName)\$($ActivePerl_x32.FileName)") | Out-Null
			Write-Host "`t Installing the 32bit version of Perl" -ForegroundColor Green
			(Start-Process -FilePath "C:\_Install Files\$($ActivePerl_x32.FolderName)\$($ActivePerl_x32.FileName)" -Argumentlist "/install /silent /norestart" -Wait -Passthru).ExitCode | Out-Null
			# Add the Perl ISAPI and CGI Restrictions for Perl (32 bit)
			Write-Host "`t Adding the Perl ISAPI and CGI Restrictions in IIS" -ForegroundColor Green
			Add-WebConfiguration -pspath 'MACHINE/WEBROOT/APPHOST' -filter 'system.webServer/security/isapiCgiRestriction' -value @{description='Perl FastCGI' ; path= "C:\Perl\bin\perl.exe `"%s`" %s" ; allowed='True'}
			# Add the Perl Handler Mappings for Perl (32 bit)
			Write-Host "`t Adding the Perl Handler Mappings in IIS" -ForegroundColor Green
			New-WebHandler -Name "Perl_Script_Map_PL" -Path "*.pl" -Verb '*' -Modules CgiModule -ScriptProcessor 'C:\Perl\bin\perl.exe "%s" %s' -ResourceType File
			New-WebHandler -Name "Perl_Script_Map_CGI" -Path "*.cgi" -Verb '*' -Modules CgiModule -ScriptProcessor 'C:\Perl\bin\perl.exe "%s" %s' -ResourceType File
		}
	cd "\"
	}
}


####################################################################################################################################################################################
Function InstallWebPI()                                 # Function to install Microsoft Web Platform Installer
{
	# Check if WebPI is installed, if not then install the correct version for the server
	if ( !(Test-Path "C:\Program Files\Microsoft\Web Platform Installer\") ) {
		# WebPI is not installed on this server so go ahead and install it
		Write-Host "`tDetermining your Operating System Type and installing the correct version of WebPI" -ForegroundColor Cyan
		# Get the latest download information from the SolidCP Installer site
		($WebPI_x64 = (SolidCPFileDownload "WebPI_x64")) | Out-Null
		($WebPI_x32 = (SolidCPFileDownload "WebPI_x32")) | Out-Null
		# Create the WebPI Directory in our Installation Files folder ready for downloading
		(md -Path "C:\_Install Files\$($WebPI_x64.FolderName)" -Force) | Out-Null ; cd "C:\_Install Files\$($WebPI_x64.FolderName)\"
		if ([Environment]::Is64BitProcess){ # Download and install the 64 bit version if the operating system is 64 bit.
			Write-Host "`t Downloading the 64bit version of Web Platform Installer" -ForegroundColor Green
			(New-Object System.Net.WebClient).DownloadFile("$($WebPI_x64.DownloadURL)", "C:\_Install Files\$($WebPI_x64.FolderName)\$($WebPI_x64.FileName)") | Out-Null
			Write-Host "`t installing the 64bit version of Web Platform Installer" -ForegroundColor Green
			(Start-Process -FilePath "C:\_Install Files\$($WebPI_x64.FolderName)\$($WebPI_x64.FileName)" -Argumentlist "/passive" -Wait -Passthru).ExitCode | Out-Null
		}else{								# Download and install the 32 bit version if the operating system is 32 bit.
			Write-Host "`t Downloading the 32bit version of Web Platform Installer" -ForegroundColor Green
			(New-Object System.Net.WebClient).DownloadFile("$($WebPI_x32.DownloadURL)", "C:\_Install Files\$($WebPI_x32.FolderName)\$($WebPI_x32.FileName)") | Out-Null
			Write-Host "`t installing the 32bit version of Web Platform Installer" -ForegroundColor Green
			(Start-Process -FilePath "C:\_Install Files\$($WebPI_x32.FolderName)\$($WebPI_x32.FileName)" -Argumentlist "/passive" -Wait -Passthru).ExitCode | Out-Null
		}
		cd "C:\"
		start-Sleep -Seconds 10
	}
}


####################################################################################################################################################################################
Function InstallWebPI_PHP()                             # Function to install PHP from the WebPI Command Line
{
	# Check if VC++ 2012 32bit run times are installed - PHP doesnt work without them, if not then install them
	if ( !(Test-Path "HKLM:\SOFTWARE\Classes\Installer\Dependencies\{33d1fd90-4274-48a1-9bc1-97e33d9c2d6f}") ) {
		Write-Host "`tInstalling PHP runtime support" -ForegroundColor Cyan
		# Get the latest download information from the SolidCP Installer site
		($WebPI_PHP_Runtime_Support = (SolidCPFileDownload "WebPI_PHP_Runtime_Support")) | Out-Null
		if (!(Test-Path "C:\_Install Files\$($WebPI_PHP_Runtime_Support.FolderName)")) { (md -Path "C:\_Install Files\$($WebPI_PHP_Runtime_Support.FolderName)" -Force) | Out-Null }
		cd "C:\_Install Files\$($WebPI_PHP_Runtime_Support.FolderName)\"
		if (!(Test-Path "C:\_Install Files\$($WebPI_PHP_Runtime_Support.FolderName)\$($WebPI_PHP_Runtime_Support.FileName)")) {
			Write-Host "`t Downloading the Visual C++ 2012 Runtime (32bit)" -ForegroundColor Green
			(New-Object System.Net.WebClient).DownloadFile("$($WebPI_PHP_Runtime_Support.DownloadURL)", "C:\_Install Files\$($WebPI_PHP_Runtime_Support.FolderName)\$($WebPI_PHP_Runtime_Support.FileName)")
		}
		Write-Host "`t Installing the Visual C++ 2012 Runtime (32bit)" -ForegroundColor Green
		((Start-Process -FilePath "C:\_Install Files\$($WebPI_PHP_Runtime_Support.FolderName)\$($WebPI_PHP_Runtime_Support.FileName)" -ArgumentList "/install /silent /norestart" -Wait -Passthru).ExitCode) | Out-Null
		cd "\"
	}

	# Check if WebPI is installed, if not then install the correct version for the server
	if ( !(Test-Path "C:\Program Files\Microsoft\Web Platform Installer\") ) { InstallWebPI }
	# Install PHP and PHP Manager for IIS if it is not already installed
	Write-Host "`tWebPI is installing PHP (v5.3, v5.4, v5.5, v5.6, v7.0) and PHP Manager for IIS" -ForegroundColor Cyan
	cd "C:\Program Files\Microsoft\Web Platform Installer\"
	if ( !(Test-Path "C:\Program Files (x86)\PHP\v5.3\") ) { Write-Host "`t Installing PHP v5.3" -ForegroundColor Green ; (invoke-command {.\WebPICMD.exe /Install /Products:PHP53 /AcceptEULA}) | Out-Null }
	if ( !(Test-Path "C:\Program Files (x86)\PHP\v5.4\") ) { Write-Host "`t Installing PHP v5.4" -ForegroundColor Green ; (invoke-command {.\WebPICMD.exe /Install /Products:PHP54 /AcceptEULA}) | Out-Null }
	if ( !(Test-Path "C:\Program Files (x86)\PHP\v5.5\") ) { Write-Host "`t Installing PHP v5.5" -ForegroundColor Green ; (invoke-command {.\WebPICMD.exe /Install /Products:PHP55 /AcceptEULA}) | Out-Null }
	if ( !(Test-Path "C:\Program Files (x86)\PHP\v5.6\") ) { Write-Host "`t Installing PHP v5.6" -ForegroundColor Green ; (invoke-command {.\WebPICMD.exe /Install /Products:PHP56 /AcceptEULA}) | Out-Null }
	if ( !(Test-Path "C:\Program Files (x86)\PHP\v7.0\") ) {
		Write-Host "`t Installing PHP v7.0" -ForegroundColor Green
		do {
			# Install the Visual C++ 2015 Runtime (32bit) for PHP v7.0
			($WebPI_PHP_Runtime_Support_2015 = (SolidCPFileDownload "WebPI_PHP_Runtime_Support_2015")) | Out-Null
			if (!(Test-Path "C:\_Install Files\$($WebPI_PHP_Runtime_Support_2015.FolderName)")) { (md -Path "C:\_Install Files\$($WebPI_PHP_Runtime_Support_2015.FolderName)" -Force) | Out-Null }
			cd "C:\_Install Files\$($WebPI_PHP_Runtime_Support_2015.FolderName)\"
			if (!(Test-Path "C:\_Install Files\$($WebPI_PHP_Runtime_Support_2015.FolderName)\$($WebPI_PHP_Runtime_Support_2015.FileName)")) {
				Write-Host "`t Downloading the Visual C++ 2015 Runtime (32bit)" -ForegroundColor Green
				(New-Object System.Net.WebClient).DownloadFile("$($WebPI_PHP_Runtime_Support_2015.DownloadURL)", "C:\_Install Files\$($WebPI_PHP_Runtime_Support_2015.FolderName)\$($WebPI_PHP_Runtime_Support_2015.FileName)")
			}
			Write-Host "`t Installing the Visual C++ 2015 Runtime (32bit)" -ForegroundColor Green
			((Start-Process -FilePath "C:\_Install Files\$($WebPI_PHP_Runtime_Support_2015.FolderName)\$($WebPI_PHP_Runtime_Support_2015.FileName)" -ArgumentList "/install /silent /norestart" -Wait -Passthru).ExitCode) | Out-Null
			cd "C:\Program Files\Microsoft\Web Platform Installer\"
			# Install PHP v7.0 using WebPI
			(invoke-command {.\WebPICMD.exe /Install /Products:PHP70 /AcceptEULA}) | Out-Null
			$dPHPv7InstallTry = $dPHPv7InstallTry + 1
			start-Sleep -Seconds 5
		}
		until ( (Test-Path "C:\Program Files (x86)\PHP\v7.0\") -or ($dPHPv7InstallTry -eq 10) )
	}
	if ( !(Test-Path "C:\Program Files\PHP Manager 1.2 for IIS 7\") ) { Write-Host "`t Installing PHP Manager for IIS" -ForegroundColor Green ; (invoke-command {.\WebPICMD.exe /Install /Products:PHPManager /AcceptEULA}) | Out-Null }
	cd "\"
}


####################################################################################################################################################################################
Function InstallWebPI_SQLdriverPHP()                    # Function to install the SQL Driver for PHP from the WebPI Command Line
{
	# Check if WebPI is installed, if not then install the correct version for the server
	if ( !(Test-Path "C:\Program Files\Microsoft\Web Platform Installer\") ) { InstallWebPI }
	# Install SQL Drivers for PHP if they are not already installed
	Write-Host "`tWebPI is installing the SQL Driver for PHP" -ForegroundColor Cyan
	cd "C:\Program Files\Microsoft\Web Platform Installer\"
	if ( (Test-Path "C:\Program Files (x86)\PHP\v7.0\") -and (!(Test-Path "C:\Program Files (x86)\PHP\v5.3\ext\php_sqlsrv.dll")) ) { Write-Host "`t Installing the SQL Driver for PHP v5.3" -ForegroundColor Green ; (invoke-command {.\WebPICMD.exe /Install /Products:SQLDriverPHP53IIS /AcceptEULA}) | Out-Null }
	if ( (Test-Path "C:\Program Files (x86)\PHP\v7.0\") -and (!(Test-Path "C:\Program Files (x86)\PHP\v5.4\ext\php_sqlsrv.dll")) ) { Write-Host "`t Installing the SQL Driver for PHP v5.4" -ForegroundColor Green ; (invoke-command {.\WebPICMD.exe /Install /Products:SQLDriverPHP54IIS /AcceptEULA}) | Out-Null }
	if ( (Test-Path "C:\Program Files (x86)\PHP\v7.0\") -and (!(Test-Path "C:\Program Files (x86)\PHP\v5.5\ext\php_sqlsrv.dll")) ) { Write-Host "`t Installing the SQL Driver for PHP v5.5" -ForegroundColor Green ; (invoke-command {.\WebPICMD.exe /Install /Products:SQLDriverPHP55IIS /AcceptEULA}) | Out-Null }
	if ( (Test-Path "C:\Program Files (x86)\PHP\v7.0\") -and (!(Test-Path "C:\Program Files (x86)\PHP\v5.6\ext\php_sqlsrv.dll")) ) { Write-Host "`t Installing the SQL Driver for PHP v5.6" -ForegroundColor Green ; (invoke-command {.\WebPICMD.exe /Install /Products:SQLDriverPHP56IIS /AcceptEULA}) | Out-Null }
	if ( (Test-Path "C:\Program Files (x86)\PHP\v7.0\") -and (!(Test-Path "C:\Program Files (x86)\PHP\v7.0\ext\php_sqlsrv_7_ts_x86.dll")) ) {
		Write-Host "`t Installing the SQL Driver for PHP v7.0" -ForegroundColor Green
		# Get the latest download information from the SolidCP Installer site
		($WebPI_PHP7_SQL_Driver = (SolidCPFileDownload "WebPI_PHP7_SQL_Driver")) | Out-Null
		if ( !(Test-Path "C:\_Install Files\$($WebPI_PHP7_SQL_Driver.FolderName)") ) { (md -Path "C:\_Install Files\$($WebPI_PHP7_SQL_Driver.FolderName)" -Force) | Out-Null }
		if ( (((Get-ChildItem "C:\_Install Files\$($WebPI_PHP7_SQL_Driver.FolderName)" | Measure-Object).Count) -eq "0") ) {
			(New-Object System.Net.WebClient).DownloadFile("$($WebPI_PHP7_SQL_Driver.DownloadURL)", "C:\_Install Files\$($WebPI_PHP7_SQL_Driver.FolderName)\$($WebPI_PHP7_SQL_Driver.FileName)")
			(Add-Type -assembly "system.io.compression.filesystem" -ErrorAction SilentlyContinue) | Out-Null
			[io.compression.zipfile]::ExtractToDirectory("C:\_Install Files\$($WebPI_PHP7_SQL_Driver.FolderName)\$($WebPI_PHP7_SQL_Driver.FileName)", "C:\_Install Files\$($WebPI_PHP7_SQL_Driver.FolderName)")
		}
		Copy-Item "C:\_Install Files\$($WebPI_PHP7_SQL_Driver.FolderName)\php_sqlsrv_4.0.6_x86\*_x86.dll" "C:\Program Files (x86)\PHP\v7.0\ext\"
		if ( !(Get-Content "C:\Program Files (x86)\PHP\v7.0\php.ini" | Select-String "extension=php_sqlsrv_7_ts_x86.dll" -Quiet) ) {
			";extension=php_sqlsrv_7_ts_x86.dll" |  Out-file -Encoding "ascii" -Append -filePath "C:\Program Files (x86)\PHP\v7.0\php.ini"
		}
		if ( !(Get-Content "C:\Program Files (x86)\PHP\v7.0\php.ini" | Select-String "extension=php_sqlsrv_7_nts_x86.dll" -Quiet) ) {
			";extension=php_sqlsrv_7_nts_x86.dll" |  Out-file -Encoding "ascii" -Append -filePath "C:\Program Files (x86)\PHP\v7.0\php.ini"
		}
	}
	cd "\"
}


####################################################################################################################################################################################
Function InstallWebPI_WinCachePHP()                     # Function to install the WinCache for PHP from the WebPI Command Line
{
	# Check if WebPI is installed, if not then install the correct version for the server
	if ( !(Test-Path "C:\Program Files\Microsoft\Web Platform Installer\") ) { InstallWebPI }
	# Install WinCache for PHP if they are not already installed
	Write-Host "`tWebPI is installing WinCache for PHP" -ForegroundColor Cyan
	cd "C:\Program Files\Microsoft\Web Platform Installer\"
	if ( (Test-Path "C:\Program Files (x86)\PHP\v7.0\") -and (!(Test-Path "C:\Program Files (x86)\PHP\v5.3\ext\php_wincache.dll")) ) { Write-Host "`t Installing WinCache for PHP v5.3" -ForegroundColor Green ; (invoke-command {.\WebPICMD.exe /Install /Products:WinCache53 /AcceptEULA}) | Out-Null }
	if ( (Test-Path "C:\Program Files (x86)\PHP\v7.0\") -and (!(Test-Path "C:\Program Files (x86)\PHP\v5.4\ext\php_wincache.dll")) ) { Write-Host "`t Installing WinCache for PHP v5.4" -ForegroundColor Green ; (invoke-command {.\WebPICMD.exe /Install /Products:WinCache54 /AcceptEULA}) | Out-Null }
	if ( (Test-Path "C:\Program Files (x86)\PHP\v7.0\") -and (!(Test-Path "C:\Program Files (x86)\PHP\v5.5\ext\php_wincache.dll")) ) { Write-Host "`t Installing WinCache for PHP v5.5" -ForegroundColor Green ; (invoke-command {.\WebPICMD.exe /Install /Products:WinCache55 /AcceptEULA}) | Out-Null }
	if ( (Test-Path "C:\Program Files (x86)\PHP\v7.0\") -and (!(Test-Path "C:\Program Files (x86)\PHP\v5.6\ext\php_wincache.dll")) ) { Write-Host "`t Installing WinCache for PHP v5.6" -ForegroundColor Green ; (invoke-command {.\WebPICMD.exe /Install /Products:WinCache56 /AcceptEULA}) | Out-Null }
	if ( (Test-Path "C:\Program Files (x86)\PHP\v7.0\") -and (!(Test-Path "C:\Program Files (x86)\PHP\v7.0\ext\php_wincache.dll")) ) { Write-Host "`t Installing WinCache for PHP v7.0" -ForegroundColor Green ; (invoke-command {.\WebPICMD.exe /Install /Products:WinCache70x86 /AcceptEULA}) | Out-Null }
	cd "\"
}


####################################################################################################################################################################################
Function InstallWebPI_Python()                          # Function to install Python v3.4 from the WebPI Command Line
{
	# Check if WebPI is installed, if not then install the correct version for the server
	if ( !(Test-Path "C:\Program Files\Microsoft\Web Platform Installer\") ) { InstallWebPI }
	# Install Python v3.4 if it is not already installed
	Write-Host "`tWebPI is installing Python v3.4 for IIS" -ForegroundColor Cyan
	cd "C:\Program Files\Microsoft\Web Platform Installer\"
	if ( !(Test-Path "C:\Python34\") ) { Write-Host "`t Installing Python v3.4" -ForegroundColor Green ; (invoke-command {.\WebPICMD.exe /Install /Products:Python34 /AcceptEULA}) | Out-Null }
	cd "\"
}


####################################################################################################################################################################################
Function InstallWebPI_URLreWrite()                      # Function to install the URL Rewrite module from the WebPI Command Line
{
	# Check if WebPI is installed, if not then install the correct version for the server
	if ( !(Test-Path "C:\Program Files\Microsoft\Web Platform Installer\") ) { InstallWebPI }
	# Install URL ReWrite for IIS if it is not already installed
	Write-Host "`tWebPI is installing the URL ReWrite module for IIS" -ForegroundColor Cyan
	cd "C:\Program Files\Microsoft\Web Platform Installer\"
	if ( !(Test-Path "HKLM:\SOFTWARE\Microsoft\IIS Extensions\URL Rewrite\") ) { Write-Host "`t Installing the URL ReWrite module for IIS" -ForegroundColor Green ; (invoke-command {.\WebPICMD.exe /Install /Products:UrlRewrite2 /AcceptEULA}) | Out-Null }
	cd "\"
}


####################################################################################################################################################################################
Function InstallWebPI_WebDeploy()                       # Function to install the Web Deploy module from the WebPI Command Line
{
	# Check if WebPI is installed, if not then install the correct version for the server
	if ( !(Test-Path "C:\Program Files\Microsoft\Web Platform Installer\") ) { InstallWebPI }
	# Install WebDeploy if it is not already installed
	Write-Host "`tWebPI is installing the Web Deploy module for IIS" -ForegroundColor Cyan
	cd "C:\Program Files\Microsoft\Web Platform Installer\"
	if ( !(Test-Path "C:\Program Files\IIS\Microsoft Web Deploy V3\") ) { Write-Host "`t Installing the Web Deploy module for IIS" -ForegroundColor Green ; (invoke-command {.\WebPICMD.exe /Install /Products:WDeployNoSMO /AcceptEULA}) | Out-Null }
	cd "\"
}


####################################################################################################################################################################################
Function InstallMariaDB_MySQL($dMariaMySQLpasswd)       # Function to install the MariaDB or MySQL Database Server
{
	# Run the function to check which Database Server (MariaDB or MySQL) needs to be installed and request a password unless it has been defined
	dCheckMariaMySQLpassword

	if ($dMySQLvMariaDB -eq "MariaDB") { # Install MariaDB if required
		if ( !(Test-Path "C:\Program Files\MariaDB 10.1\") ) { # Check if the MariaDB Database Server is already installed
			Write-Host "`tDownloading the MariaDB Database Server" -ForegroundColor Cyan
			# Get the latest download information from the SolidCP Installer site
			($MariaDB_x64 = (SolidCPFileDownload "MariaDB_x64")) | Out-Null
			# Create the MariaDB Database Server Directory in our Installation Files folder ready for downloading
			(md -Path "C:\_Install Files\$($MariaDB_x64.FolderName)" -Force) | Out-Null ; cd "C:\_Install Files\$($MariaDB_x64.FolderName)\"
			# Download MariaDB Database Server
			(New-Object System.Net.WebClient).DownloadFile("$($MariaDB_x64.DownloadURL)", "C:\_Install Files\$($MariaDB_x64.FolderName)\$($MariaDB_x64.FileName)")
			# Install the MariaDB Database Server x64 on the Server
			if ( !(Test-Path "C:\Program Files\MariaDB 10.1\") ) { # Check if the MariaDB Database Server is already installed
				Write-Host "`t Installing MariaDB Database Server" -ForegroundColor Green
				((Start-Process -FilePath "C:\_Install Files\$($MariaDB_x64.FolderName)\$($MariaDB_x64.FileName)" -Argumentlist "/QN PASSWORD=$dMariaMySQLpasswd ALLOWREMOTEROOTACCESS=1 UTF8=1 SERVICENAME=MariaDB /passive" -Wait -Passthru).ExitCode) | Out-Null
			}
			cd "\"
		}
	}elseif ($dMySQLvMariaDB -eq "MySQL") { # Install MySQL if required
		Write-Host "`tInstalling the MySQL Database Server" -ForegroundColor Cyan
		cd "C:\Program Files\Microsoft\Web Platform Installer\"
		if ( !(Test-Path "C:\Program Files\MySQL\MySQL Server 5.1\") ) { # Check if the MySQL Database Server is already installed
			# Install the MySQL Database Server
			Write-Host "`t Installing the MySQL Database Server" -ForegroundColor Green
			(invoke-command {.\WebPICMD.exe /Install /Products:MySQL /AcceptEULA /MySQLPassword:$dMariaMySQLpasswd}) | Out-Null
		}
		# Add the firewall rule to open Port 3306 for inbound management of the MySQL Server 5.1 only if it doesn't exist
		if ( !(Get-NetFirewallRule | where DisplayName -EQ "MySQL Server - Port 3306") ) {
			Write-Host "`tOpening port 3306 for the MySQL Database Server on the Firewall" -ForegroundColor Cyan
			(New-NetFirewallRule -DisplayName "MySQL Server - Port 3306" -Direction Inbound –LocalPort 3306 -Protocol TCP -Action Allow) | Out-Null
		}
		cd "\"
	}
	# Check if the MySQL Connector Net v6.9.7 is already installed (also works for MarioaDB), if not then install it via the Web Platform Installer
	Write-Host "`t Checking for MySQL Connector" -ForegroundColor Green
	if ( !(Test-Path "C:\Program Files (x86)\MySQL\MySQL Connector Net 6.9.7\") ) {
		# Install the MySQL Connector Net v6.9.7
		cd "C:\Program Files\Microsoft\Web Platform Installer\"
		Write-Host "`t Installing the MySQL Connector Net v6.9.7" -ForegroundColor Green ; (invoke-command {.\WebPICMD.exe /Install /Products:MySQLConnector /AcceptEULA}) | Out-Null
		cd "\"
	}
}


####################################################################################################################################################################################
Function InstallSQLsvr2014withTools()                   # Function to install SQL Server 2014 Express (with tools)
{
	Param(
		[string]$Password,        # Specify the Password for the SA (SQL Admin) user
		[switch]$DefaultInstance  # Use the Default Instance Name of "MSSQLSERVER"
	)

	if ( ($DefaultInstance -and (!(Test-Path "C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\"))) -or (!(Test-Path "C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\")) ) {
		# Run the function to request a password for the Microsoft SQL Server unless it has been defined
		if (!($Password)) {
			dCheckMsSQLpassword
		}
		# Check to make sure the SQL Server requirements are installed on this server
		if (!(Get-WindowsFeature Net-Framework-Core).Installed) {
			Write-Host "`t Installing .NET Framework 3.5" -ForegroundColor Green
			(Add-WindowsFeature -Name Net-Framework-Core) | Out-Null
		}
		Write-Host "`tDownloading SQL Server 2014 Express (with tools)" -ForegroundColor Cyan
		Write-Host "`t  ****************************************" -ForegroundColor Green
		Write-Host "`t  *                                      *" -ForegroundColor Green
		Write-Host "`t  *     This will take quite a while     *" -ForegroundColor Green
		Write-Host "`t  *             to download              *" -ForegroundColor Green
		Write-Host "`t  *                                      *" -ForegroundColor Green
		Write-Host "`t  ****************************************" -ForegroundColor Green
		# Get the latest download information from the SolidCP Installer site
		($Microsoft_SQL_Server_2014_With_Tools = (SolidCPFileDownload "Microsoft_SQL_Server_2014_With_Tools")) | Out-Null
		# Create the SQL Server 2014 Express with Tools Directory in our Installation Files folder ready for downloading
		if (!(Test-Path "C:\_Install Files\$($Microsoft_SQL_Server_2014_With_Tools.FolderName)")) {
			(md -Path "C:\_Install Files\$($Microsoft_SQL_Server_2014_With_Tools.FolderName)" -Force) | Out-Null
			cd "C:\_Install Files\$($Microsoft_SQL_Server_2014_With_Tools.FolderName)"
		}
		# Download SQL Server 2014 Express with Tools x64 from Microsoft unless it is already on this machine
		if (!(Test-Path "C:\_Install Files\$($Microsoft_SQL_Server_2014_With_Tools.FolderName)\$($Microsoft_SQL_Server_2014_With_Tools.FileName)")) {
			(New-Object System.Net.WebClient).DownloadFile("$($Microsoft_SQL_Server_2014_With_Tools.DownloadURL)", "C:\_Install Files\$($Microsoft_SQL_Server_2014_With_Tools.FolderName)\$($Microsoft_SQL_Server_2014_With_Tools.FileName)")
		}
		# Install the SQL Server 2014 Express with Tools x64 on the Server
		Write-Host "`t Extracting and Installing SQL Server 2014 Express (with tools)" -ForegroundColor Green
		Write-Host "`t    ****************************************" -ForegroundColor Green
		Write-Host "`t    *                                      *" -ForegroundColor Green
		Write-Host "`t    *     This will take quite a while     *" -ForegroundColor Green
		Write-Host "`t    *           to fully complete          *" -ForegroundColor Green
		Write-Host "`t    *                                      *" -ForegroundColor Green
		Write-Host "`t    *  Please be patient while we install  *" -ForegroundColor Green
		Write-Host "`t    *       SQL Server 2014 Express        *" -ForegroundColor Green
		Write-Host "`t    *        With Management Tools         *" -ForegroundColor Green
		Write-Host "`t    *                                      *" -ForegroundColor Green
		Write-Host "`t    ****************************************" -ForegroundColor Green
		if ($DefaultInstance) {
			((Start-Process -FilePath "C:\_Install Files\$($Microsoft_SQL_Server_2014_With_Tools.FolderName)\$($Microsoft_SQL_Server_2014_With_Tools.FileName)" -Argumentlist "/Q /IACCEPTSQLSERVERLICENSETERMS=1 /ADDCURRENTUSERASSQLADMIN=True /ACTION=Install /INSTANCEID=MSSQLSERVER /INSTANCENAME=MSSQLSERVER /TCPENABLED=1 /UPDATEENABLED=True /FEATURES=SQLENGINE,REPLICATION,CONN,BC,SDK,SSMS,ADV_SSMS,SNAC_SDK,Tools /SECURITYMODE=SQL /SAPWD=$Password /INDICATEPROGRESS=False /SQMREPORTING=False /AGTSVCSTARTUPTYPE=Automatic /SQLSVCSTARTUPTYPE=Automatic /SQLCOLLATION=`"SQL_Latin1_General_CP1_CI_AS`" /BROWSERSVCSTARTUPTYPE=Automatic" -Wait -Passthru).ExitCode) | Out-Null
		}else{
			((Start-Process -FilePath "C:\_Install Files\$($Microsoft_SQL_Server_2014_With_Tools.FolderName)\$($Microsoft_SQL_Server_2014_With_Tools.FileName)" -Argumentlist "/Q /IACCEPTSQLSERVERLICENSETERMS=1 /ADDCURRENTUSERASSQLADMIN=True /ACTION=Install /INSTANCEID=SQLEXPRESS /INSTANCENAME=SQLExpress /TCPENABLED=1 /UPDATEENABLED=True /FEATURES=SQLENGINE,REPLICATION,CONN,BC,SDK,SSMS,ADV_SSMS,SNAC_SDK,Tools /SECURITYMODE=SQL /SAPWD=$Password /INDICATEPROGRESS=False /SQMREPORTING=False /AGTSVCSTARTUPTYPE=Automatic /SQLSVCSTARTUPTYPE=Automatic /SQLCOLLATION=`"SQL_Latin1_General_CP1_CI_AS`" /BROWSERSVCSTARTUPTYPE=Automatic" -Wait -Passthru).ExitCode) | Out-Null
		}

		InstallSQLsvr2012NativeClnt
		InstallSQLsvr2012PowerShellTools
		InstallMSSQL_TCPports
		InstallFirewall_MSSQL
	}
}


####################################################################################################################################################################################
Function InstallSQLsvr2014noTools()                     # Function to install SQL Server 2014 Express ONLY (NO Tools)
{
	Param(
		[string]$Password,        # Specify the Password for the SA (SQL Admin) user
		[switch]$DefaultInstance  # Use the Default Instance Name of "MSSQLSERVER"
	)

	if ( !(Test-Path "C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\") ) {
		# Run the function to request a password for the Microsoft SQL Server unless it has been defined
		if (!($Password)) {
			dCheckMsSQLpassword
		}
		# Check to make sure the SQL Server requirements are installed on this server
		if (!(Get-WindowsFeature Net-Framework-Core).Installed) {
			Write-Host "`t Installing .NET Framework 3.5" -ForegroundColor Green
			(Add-WindowsFeature -Name Net-Framework-Core) | Out-Null
		}
		Write-Host "`tDownloading SQL Server 2014 Express" -ForegroundColor Cyan
		Write-Host "`t  ****************************************" -ForegroundColor Green
		Write-Host "`t  *                                      *" -ForegroundColor Green
		Write-Host "`t  *     This will take quite a while     *" -ForegroundColor Green
		Write-Host "`t  *             to download              *" -ForegroundColor Green
		Write-Host "`t  *                                      *" -ForegroundColor Green
		Write-Host "`t  ****************************************" -ForegroundColor Green
		# Get the latest download information from the SolidCP Installer site
		($Microsoft_SQL_Server_2014_No_Tools = (SolidCPFileDownload "Microsoft_SQL_Server_2014_No_Tools")) | Out-Null
		# Create the SQL Server 2014 Express Directory in our Installation Files folder ready for downloading
		if (!(Test-Path "C:\_Install Files\$($Microsoft_SQL_Server_2014_No_Tools.FolderName)")) {
			(md -Path "C:\_Install Files\$($Microsoft_SQL_Server_2014_No_Tools.FolderName)" -Force) | Out-Null
			cd "C:\_Install Files\$($Microsoft_SQL_Server_2014_No_Tools.FolderName)"
		}
		# Download SQL Server 2014 Express x64 from Microsoft
		if (!(Test-Path "C:\_Install Files\$($Microsoft_SQL_Server_2014_No_Tools.FolderName)\$($Microsoft_SQL_Server_2014_No_Tools.FileName)")) {
			(New-Object System.Net.WebClient).DownloadFile("$($Microsoft_SQL_Server_2014_No_Tools.DownloadURL)", "C:\_Install Files\$($Microsoft_SQL_Server_2014_No_Tools.FolderName)\$($Microsoft_SQL_Server_2014_No_Tools.FileName)")
		}
		# Install the SQL Server 2014 Express x64 on the Server
		Write-Host "`t Extracting and Installing SQL Server 2014 Express" -ForegroundColor Green
		Write-Host "`t    ****************************************" -ForegroundColor Green
		Write-Host "`t    *                                      *" -ForegroundColor Green
		Write-Host "`t    *     This will take quite a while     *" -ForegroundColor Green
		Write-Host "`t    *           to fully complete          *" -ForegroundColor Green
		Write-Host "`t    *                                      *" -ForegroundColor Green
		Write-Host "`t    *  Please be patient while we install  *" -ForegroundColor Green
		Write-Host "`t    *       SQL Server 2014 Express        *" -ForegroundColor Green
		Write-Host "`t    *                                      *" -ForegroundColor Green
		Write-Host "`t    ****************************************" -ForegroundColor Green
		if ($DefaultInstance) {
			((Start-Process -FilePath "C:\_Install Files\$($Microsoft_SQL_Server_2014_No_Tools.FolderName)\$($Microsoft_SQL_Server_2014_No_Tools.FileName)" -Argumentlist "/Q /IACCEPTSQLSERVERLICENSETERMS=1 /ADDCURRENTUSERASSQLADMIN=True /ACTION=Install /INSTANCEID=MSSQLSERVER /INSTANCENAME=MSSQLSERVER /TCPENABLED=1 /UPDATEENABLED=True /FEATURES=SQLENGINE,REPLICATION,CONN,BC,SDK,SNAC_SDK /SECURITYMODE=SQL /SAPWD=$Password /INDICATEPROGRESS=False /SQMREPORTING=False /AGTSVCSTARTUPTYPE=Automatic /SQLSVCSTARTUPTYPE=Automatic /SQLCOLLATION=`"SQL_Latin1_General_CP1_CI_AS`" /BROWSERSVCSTARTUPTYPE=Automatic" -Wait -Passthru).ExitCode) | Out-Null
		}else{
			((Start-Process -FilePath "C:\_Install Files\$($Microsoft_SQL_Server_2014_No_Tools.FolderName)\$($Microsoft_SQL_Server_2014_No_Tools.FileName)" -Argumentlist "/Q /IACCEPTSQLSERVERLICENSETERMS=1 /ADDCURRENTUSERASSQLADMIN=True /ACTION=Install /INSTANCEID=SQLEXPRESS /INSTANCENAME=SQLExpress /TCPENABLED=1 /UPDATEENABLED=True /FEATURES=SQLENGINE,REPLICATION,CONN,BC,SDK,SNAC_SDK /SECURITYMODE=SQL /SAPWD=$Password /INDICATEPROGRESS=False /SQMREPORTING=False /AGTSVCSTARTUPTYPE=Automatic /SQLSVCSTARTUPTYPE=Automatic /SQLCOLLATION=`"SQL_Latin1_General_CP1_CI_AS`" /BROWSERSVCSTARTUPTYPE=Automatic" -Wait -Passthru).ExitCode) | Out-Null
		}

		InstallSQLsvr2012NativeClnt
		InstallSQLsvr2012PowerShellTools
		InstallMSSQL_TCPports
		InstallFirewall_MSSQL
	}
}


####################################################################################################################################################################################
Function InstallSQLsvr2014mgmtTools()                   # Function to install SQL Server 2014 Express Management Tools
{
	if ( !(Test-Path "C:\Program Files (x86)\Microsoft SQL Server\120\Tools\Binn\ManagementStudio\Ssms.exe") ) {
		# Check to make sure the SQL Server requirements are installed on this server
		if (!(Get-WindowsFeature Net-Framework-Core).Installed) {
			Write-Host "`t Installing .NET Framework 3.5" -ForegroundColor Green
			(Add-WindowsFeature -Name Net-Framework-Core) | Out-Null
		}
		Write-Host "`tDownloading SQL Server 2014 Express Management Tools" -ForegroundColor Cyan
		Write-Host "`t  ****************************************" -ForegroundColor Green
		Write-Host "`t  *                                      *" -ForegroundColor Green
		Write-Host "`t  *     This will take quite a while     *" -ForegroundColor Green
		Write-Host "`t  *             to download              *" -ForegroundColor Green
		Write-Host "`t  *                                      *" -ForegroundColor Green
		Write-Host "`t  ****************************************" -ForegroundColor Green
		# Get the latest download information from the SolidCP Installer site
		($Microsoft_SQL_Server_2014_Tools_ONLY = (SolidCPFileDownload "Microsoft_SQL_Server_2014_Tools_ONLY")) | Out-Null
		# Create the SQL Server 2014 Management Tools Directory in our Installation Files folder ready for downloading
		if (!(Test-Path "C:\_Install Files\$($Microsoft_SQL_Server_2014_Tools_ONLY.FolderName)")) {
			(md -Path "C:\_Install Files\$($Microsoft_SQL_Server_2014_Tools_ONLY.FolderName)" -Force) | Out-Null
			cd "C:\_Install Files\$($Microsoft_SQL_Server_2014_Tools_ONLY.FolderName)"
		}
		# Download SQL Server 2014 Express Management Tools x64 from Microsoft
		if (!(Test-Path "C:\_Install Files\$($Microsoft_SQL_Server_2014_Tools_ONLY.FolderName)\$($Microsoft_SQL_Server_2014_Tools_ONLY.FileName)")) {
			(New-Object System.Net.WebClient).DownloadFile("$($Microsoft_SQL_Server_2014_Tools_ONLY.DownloadURL)", "C:\_Install Files\$($Microsoft_SQL_Server_2014_Tools_ONLY.FolderName)\$($Microsoft_SQL_Server_2014_Tools_ONLY.FileName)")
		}
		# Install the SQL Server 2014 Management Tools x64 on the Server
		Write-Host "`t Extracting and Installing SQL Server 2014 Express Management Tools" -ForegroundColor Green
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
		((Start-Process -FilePath "C:\_Install Files\$($Microsoft_SQL_Server_2014_Tools_ONLY.FolderName)\$($Microsoft_SQL_Server_2014_Tools_ONLY.FileName)" -Argumentlist "/Q /IACCEPTSQLSERVERLICENSETERMS=1 /ACTION=Install /UPDATEENABLED=True /FEATURES=SSMS,ADV_SSMS,Tools /INDICATEPROGRESS=False" -Wait -Passthru).ExitCode) | Out-Null
	}
}


####################################################################################################################################################################################
Function InstallSQLsvr2012NativeClnt()                  # Function to install SQL Server 2012 Native Client
{
	if ( !(Test-Path "HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server\SQLNCLI11\CurrentVersion") ) { 
		# Stopping the SQL Server Service
		if ([bool](Get-Service -Name 'MSSQL*' -ErrorAction SilentlyContinue)) {
			Write-Host "`tStopping the SQL Server ready for the SQL Server 2012 Native Client Installation" -ForegroundColor Cyan
			(Stop-Service 'MSSQL*' -Force -WarningAction SilentlyContinue) | Out-Null
		}
		Write-Host "`t Downloading the SQL Server 2012 Native Client" -ForegroundColor Green
		# Get the latest download information from the SolidCP Installer site
		($Microsoft_SQL_Server_2012_Native_Client = (SolidCPFileDownload "Microsoft_SQL_Server_2012_Native_Client")) | Out-Null
		# Create the SQL Server 2012 Native Client Directory in our Installation Files folder ready for downloading
		if (!(Test-Path "C:\_Install Files\$($Microsoft_SQL_Server_2012_Native_Client.FolderName)")) {
			(md -Path "C:\_Install Files\$($Microsoft_SQL_Server_2012_Native_Client.FolderName)" -Force) | Out-Null
			cd "C:\_Install Files\$($Microsoft_SQL_Server_2012_Native_Client.FolderName)"
		}
		# Download the SQL Server Native Client from Microsoft
		if (!(Test-Path "C:\_Install Files\$($Microsoft_SQL_Server_2012_Native_Client.FolderName)\$($Microsoft_SQL_Server_2012_Native_Client.FileName)")) {
			(New-Object System.Net.WebClient).DownloadFile("$($Microsoft_SQL_Server_2012_Native_Client.DownloadURL)", "C:\_Install Files\$($Microsoft_SQL_Server_2012_Native_Client.FolderName)\$($Microsoft_SQL_Server_2012_Native_Client.FileName)")
		}
		# Install the SQL Server Native Client on the Server
		Write-Host "`t Installing the SQL Server 2012 Native Client" -ForegroundColor Green
		((Start-Process -FilePath "C:\_Install Files\$($Microsoft_SQL_Server_2012_Native_Client.FolderName)\$($Microsoft_SQL_Server_2012_Native_Client.FileName)" -ArgumentList "/qb IACCEPTSQLNCLILICENSETERMS=YES" -Wait -Passthru).ExitCode) | Out-Null
		# Start the SQL Server Service
		if ([bool](Get-Service -Name 'MSSQL*' -ErrorAction SilentlyContinue)) {
			Write-Host "`t Starting the SQL Server again as we have just installed the SQL Server 2012 Native Client" -ForegroundColor Green
			(Start-Service 'MSSQL*' -WarningAction SilentlyContinue) | Out-Null
		}
	}
}


####################################################################################################################################################################################
Function InstallSQLsvr2012PowerShellTools()             # Function to install the SQL Server 2012 PowerShell Tools
{
	if ( (!(Test-Path "HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server 2012 Redist\SQL Server System CLR Types\1033\CurrentVersion")) -or (!(Test-Path "HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server 2012 Redist\SharedManagementObjects\1033\CurrentVersion")) -or (!(Test-Path "HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server 2012 Redist\PowerShell\1033\CurrentVersion")) ) {
		# Stopping the SQL Server Service
		if ([bool](Get-Service -Name 'MSSQL*' -ErrorAction SilentlyContinue)) {
			Write-Host "`tStopping the SQL Server ready for the SQL Server 2012 PowerShell Tools Installation" -ForegroundColor Cyan
			(Stop-Service 'MSSQL*' -Force -WarningAction SilentlyContinue) | Out-Null
		}
	}
	if (!(Test-Path "HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server 2012 Redist\SQL Server System CLR Types\1033\CurrentVersion")) {
		($Microsoft_SQL_Server_2012_System_CLR_Types = (SolidCPFileDownload "Microsoft_SQL_Server_2012_System_CLR_Types")) | Out-Null
		if (!(Test-Path "C:\_Install Files\$($Microsoft_SQL_Server_2012_System_CLR_Types.FolderName)")) {
			(md -Path "C:\_Install Files\$($Microsoft_SQL_Server_2012_System_CLR_Types.FolderName)" -Force) | Out-Null
			cd "C:\_Install Files\$($Microsoft_SQL_Server_2012_System_CLR_Types.FolderName)"
		}
		if (!(Test-Path "C:\_Install Files\$($Microsoft_SQL_Server_2012_System_CLR_Types.FolderName)\$($Microsoft_SQL_Server_2012_System_CLR_Types.FileName)")) {
			Write-Host "`t Downloading the System CLR Types for SQL Server 2012" -ForegroundColor Green
			(New-Object System.Net.WebClient).DownloadFile("$($Microsoft_SQL_Server_2012_System_CLR_Types.DownloadURL)", "C:\_Install Files\$($Microsoft_SQL_Server_2012_System_CLR_Types.FolderName)\$($Microsoft_SQL_Server_2012_System_CLR_Types.FileName)")
		}
		Write-Host "`t Installing the System CLR Types for SQL Server 2012" -ForegroundColor Green
		((Start-Process -FilePath "C:\_Install Files\$($Microsoft_SQL_Server_2012_System_CLR_Types.FolderName)\$($Microsoft_SQL_Server_2012_System_CLR_Types.FileName)" -ArgumentList "/qb IACCEPTSQLNCLILICENSETERMS=YES" -Wait -Passthru).ExitCode) | Out-Null
	}
	if (!(Test-Path "HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server 2012 Redist\SharedManagementObjects\1033\CurrentVersion")) {
		($Microsoft_SQL_Server_2012_Shared_Management_Objects = (SolidCPFileDownload "Microsoft_SQL_Server_2012_Shared_Management_Objects")) | Out-Null
		if (!(Test-Path "C:\_Install Files\$($Microsoft_SQL_Server_2012_Shared_Management_Objects.FolderName)")) {
			(md -Path "C:\_Install Files\$($Microsoft_SQL_Server_2012_Shared_Management_Objects.FolderName)" -Force) | Out-Null
			cd "C:\_Install Files\$($Microsoft_SQL_Server_2012_Shared_Management_Objects.FolderName)"
		}
		if (!(Test-Path "C:\_Install Files\$($Microsoft_SQL_Server_2012_Shared_Management_Objects.FolderName)\$($Microsoft_SQL_Server_2012_Shared_Management_Objects.FileName)")) {
			Write-Host "`t Downloading the Shared Management Objects for SQL Server 2012" -ForegroundColor Green
			(New-Object System.Net.WebClient).DownloadFile("$($Microsoft_SQL_Server_2012_Shared_Management_Objects.DownloadURL)", "C:\_Install Files\$($Microsoft_SQL_Server_2012_Shared_Management_Objects.FolderName)\$($Microsoft_SQL_Server_2012_Shared_Management_Objects.FileName)")
		}
		Write-Host "`t Installing the Shared Management Objects for SQL Server 2012" -ForegroundColor Green
		((Start-Process -FilePath "C:\_Install Files\$($Microsoft_SQL_Server_2012_Shared_Management_Objects.FolderName)\$($Microsoft_SQL_Server_2012_Shared_Management_Objects.FileName)" -ArgumentList "/qb IACCEPTSQLNCLILICENSETERMS=YES" -Wait -Passthru).ExitCode) | Out-Null
	}
	if (!(Test-Path "HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server 2012 Redist\PowerShell\1033\CurrentVersion")) {
		($Microsoft_SQL_Server_2012_PowerShell_Tools = (SolidCPFileDownload "Microsoft_SQL_Server_2012_PowerShell_Tools")) | Out-Null
		if (!(Test-Path "C:\_Install Files\$($Microsoft_SQL_Server_2012_PowerShell_Tools.FolderName)")) {
			(md -Path "C:\_Install Files\$($Microsoft_SQL_Server_2012_PowerShell_Tools.FolderName)" -Force) | Out-Null
			cd "C:\_Install Files\$($Microsoft_SQL_Server_2012_PowerShell_Tools.FolderName)"
		}
		if (!(Test-Path "C:\_Install Files\$($Microsoft_SQL_Server_2012_PowerShell_Tools.FolderName)\$($Microsoft_SQL_Server_2012_PowerShell_Tools.FileName)")) {
			Write-Host "`t Downloading the SQL Server 2012 PowerShell Tools" -ForegroundColor Green
			(New-Object System.Net.WebClient).DownloadFile("$($Microsoft_SQL_Server_2012_PowerShell_Tools.DownloadURL)", "C:\_Install Files\$($Microsoft_SQL_Server_2012_PowerShell_Tools.FolderName)\$($Microsoft_SQL_Server_2012_PowerShell_Tools.FileName)")
		}
		Write-Host "`t Installing the SQL Server 2012 PowerShell Tools" -ForegroundColor Green
		((Start-Process -FilePath "C:\_Install Files\$($Microsoft_SQL_Server_2012_PowerShell_Tools.FolderName)\$($Microsoft_SQL_Server_2012_PowerShell_Tools.FileName)" -ArgumentList "/qb IACCEPTSQLNCLILICENSETERMS=YES" -Wait -Passthru).ExitCode) | Out-Null
	}
	if ( (!(Test-Path "HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server 2012 Redist\SQL Server System CLR Types\1033\CurrentVersion")) -or (!(Test-Path "HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server 2012 Redist\SharedManagementObjects\1033\CurrentVersion")) -or (!(Test-Path "HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server 2012 Redist\PowerShell\1033\CurrentVersion")) ) {
		# Start the SQL Server Service
		if ([bool](Get-Service -Name 'MSSQL*' -ErrorAction SilentlyContinue)) {
			Write-Host "`t Starting the SQL Server again as we have just installed the SQL Server 2012 PowerShell Tools" -ForegroundColor Green
			(Start-Service 'MSSQL*' -WarningAction SilentlyContinue) | Out-Null
		}
	}
}


####################################################################################################################################################################################
Function InstallPowerShellGallery()                     # Function to install the PowerShell Gallery used to install various external software
{
	if ($PSVersionTable.PSVersion.Major -match "3|4") {
		if (!(Test-Path "C:\Program Files\WindowsPowerShell\Modules\PowerShellGet")) {
			($PowerShell_Gallery = (SolidCPFileDownload "PowerShell_Gallery")) | Out-Null
			if (!(Test-Path "C:\_Install Files\$($PowerShell_Gallery.FolderName)")) {
				(md -Path "C:\_Install Files\$($PowerShell_Gallery.FolderName)" -Force) | Out-Null
				cd "C:\_Install Files\$($PowerShell_Gallery.FolderName)"
			}
			if (!(Test-Path "C:\_Install Files\$($PowerShell_Gallery.FolderName)\$($PowerShell_Gallery.FileName)")) {
				Write-Host "`t Downloading the PowerShell Gallery module" -ForegroundColor Green
				(New-Object System.Net.WebClient).DownloadFile("$($PowerShell_Gallery.DownloadURL)", "C:\_Install Files\$($PowerShell_Gallery.FolderName)\$($PowerShell_Gallery.FileName)")
			}
			Write-Host "`t Installing the PowerShell Gallery module" -ForegroundColor Green
			((Start-Process -FilePath "C:\_Install Files\$($PowerShell_Gallery.FolderName)\$($PowerShell_Gallery.FileName)" -ArgumentList "/qb IACCEPTSQLNCLILICENSETERMS=YES" -Wait -Passthru).ExitCode) | Out-Null
		}
	}
}


####################################################################################################################################################################################
Function InstallLetsEncryptACMESharp()                      # Function to get the Download URL, Folder Name and File Name from the SolidCP Installer Site for the required file
{
	if (!(Test-Path "C:\Program Files\WindowsPowerShell\Modules\ACMESharp")) {
		# Install the PowerShell Gallery (PowerShellGet) if required
		InstallPowerShellGallery
		if ($PSVersionTable.PSVersion.Major -ge "3") {
			# Install the NuGet Package Management unless it is already installed
			if (!(Test-Path "C:\Program Files\PackageManagement\ProviderAssemblies\nuget")) {
				Import-Module PowerShellGet -DisableNameChecking
				Write-Host "`t Installing the NuGet Package Manager" -ForegroundColor Green
				(Install-PackageProvider -Name NuGet -MinimumVersion 2.8.5.201 -Force) | Out-Null
			}
			# Install ACMESharp (LetsEncrypt) unless it is already installed
			if (!(Test-Path "C:\Program Files\WindowsPowerShell\Modules\ACMESharp")) {
				Write-Host "`t Installing LetsEncrypt (ACMESharp) on this machine" -ForegroundColor Green
				(Install-Module -Name ACMESharp -AllowClobber -Confirm:$false -Force) | Out-Null
			}
		}
	}
}


####################################################################################################################################################################################
Function InstallMSSQL_TCPports()                        # Function to the TCP Ports and Named Pipes for the Microsoft SQL Database Server
{
	$dEnableSQLports = @'
	# Setup the enviroment to enable the ports
	$wmi = New-Object ('Microsoft.SqlServer.Management.Smo.Wmi.ManagedComputer')
	$Tcp = $wmi.GetSmoObject("ManagedComputer[@Name='$env:COMPUTERNAME']/ ServerInstance[@Name='SQLExpress']/ServerProtocol[@Name='Tcp']")

	# Enable TCP on the SQL Server
	$Tcp.IsEnabled = $true
	$TCP.alter()

	# Enabled TCP, Disbale Dynamic Ports and Set TCP Port as 1433 for all IP Addresses
	foreach ($ipAddress in $Tcp.IPAddresses)
	{
		if ($ipAddress.IPAddressProperties["Active"].Value) {
			$ipAddress.IPAddressProperties["Enabled"].Value=$True
		}
		$ipAddress.IPAddressProperties["TcpDynamicPorts"].Value = ""
		$ipAddress.IPAddressProperties["TcpPort"].Value = "1433"
	}
	$Tcp.Alter()

	# Enable the named pipes protocol for the default instance.
	$NamedPipes = $wmi.GetSmoObject("ManagedComputer[@Name='$env:COMPUTERNAME']/ ServerInstance[@Name='SQLExpress']/ServerProtocol[@Name='Np']")
	$NamedPipes.IsEnabled = $true
	$NamedPipes.Alter()

	# Exit the SQL PowerShell Enviroment
	exit
'@

	$dEnableSQLports2 = @'
$wmi = New-Object ('Microsoft.SqlServer.Management.Smo.Wmi.ManagedComputer')
($wmi.GetSmoObject("ManagedComputer[@Name='$env:COMPUTERNAME']/ServerInstance[@Name='SQLExpress']/ ServerProtocol[@Name='Tcp']/IPAddress[@Name='IPAll']").IPAddressProperties["TcpDynamicPorts"].Value = "")
($wmi.GetSmoObject("ManagedComputer[@Name='$env:COMPUTERNAME']/ServerInstance[@Name='SQLExpress']/ ServerProtocol[@Name='Tcp']/IPAddress[@Name='IPAll']").IPAddressProperties["TcpPort"].Value = "1433")
($wmi.GetSmoObject("ManagedComputer[@Name='$env:COMPUTERNAME']/ServerInstance[@Name='SQLExpress']/ ServerProtocol[@Name='Tcp']")).Alter();
exit
'@

	Write-Host "`tEnabling TCP and Named Pipes for the Microsoft SQL Database Server" -ForegroundColor Cyan
	# Register the SQL PowerShell Modules
	start-Sleep -Seconds 25
	(set-alias installutil $env:windir\microsoft.net\framework\v2.0.50727\installutil) | Out-Null
	(installutil -i “C:\Program Files (x86)\Microsoft SQL Server\120\Tools\PowerShell\Modules\SQLPS\Microsoft.SqlServer.Management.PSProvider.dll”) | Out-Null
	(installutil -i “C:\Program Files (x86)\Microsoft SQL Server\120\Tools\PowerShell\Modules\SQLPS\Microsoft.SqlServer.Management.PSSnapins.dll”) | Out-Null
	# Open the SQL PowerShell Enviroment and run the above command
	cd "C:\Program Files (x86)\Microsoft SQL Server\120\Tools\Binn"
	invoke-expression "cmd /c start sqlps -Command {$dEnableSQLports}"
	cd "\"
	start-Sleep -Seconds 25
	# Restart the SQL Server Service
	Write-Host "`t Restarting the SQL Database Server" -ForegroundColor Yellow
	(Restart-Service 'MSSQL*' -Force -WarningAction SilentlyContinue) | Out-Null
	Write-Host "`t Restarted successfully" -ForegroundColor Green
	start-Sleep -Seconds 1
}


####################################################################################################################################################################################
Function InstallFirewall_MSSQL()                        # Function to install the Firewall Rule for the Microsoft SQL Database Server
{
	Write-Host "`tChecking the Microsoft SQL Database Server Ports on the Firewall" -ForegroundColor Cyan
	# Check if the OLD SQL Server rule exists, it it does then delete it
	if (Get-NetFirewallRule | where DisplayName -EQ "Microsoft SQL Server - Port 1433") {
		Write-Host "`t Removing the old `"Microsoft SQL Server - Port 1433`" Firewall rule" -ForegroundColor Green
		(Remove-NetFirewallRule -DisplayName "Microsoft SQL Server - Port 1433" -Confirm:$false) | Out-Null
	}
	# Add the firewall rule to open Port 1433 for inbound management of the Microsoft SQL Server only if it doesn't exist
	if ( !(Get-NetFirewallRule | where DisplayName -EQ "Microsoft SQL Server - TCP Port 1433") ) {
		(New-NetFirewallRule -DisplayName "Microsoft SQL Server - TCP Port 1433" -Direction Inbound –LocalPort "1433-1434" -Protocol TCP -Action Allow) | Out-Null
		Write-Host "`t The SQL Server TCP Port `"1433`" Firewall rule has been added successfully" -ForegroundColor Green
	}else{
		(Set-NetFirewallRule -DisplayName "Microsoft SQL Server - TCP Port 1433" -Direction Inbound -LocalPort "1433-1434" -Protocol TCP -Action Allow) | Out-Null
		Write-Host "`t The SQL Server TCP Port `"1433`" Firewall rule has been modified successfully" -ForegroundColor Green
	}
	if ( !(Get-NetFirewallRule | where DisplayName -EQ "Microsoft SQL Server - UDP Port 1433") ) {
		(New-NetFirewallRule -DisplayName "Microsoft SQL Server - UDP Port 1433" -Direction Inbound –LocalPort "1433-1434" -Protocol UDP -Action Allow) | Out-Null
		Write-Host "`t The SQL Server UDP Port `"1433`" Firewall rule has been added successfully" -ForegroundColor Green
	}else{
		(Set-NetFirewallRule -DisplayName "Microsoft SQL Server - UDP Port 1433" -Direction Inbound -LocalPort "1433-1434" -Protocol UDP -Action Allow) | Out-Null
		Write-Host "`t The SQL Server UDP Port `"1433`" Firewall rule has been modified successfully" -ForegroundColor Green
	}
}


####################################################################################################################################################################################
Function Harden_MSSQL_UserIsolation()                   # Function to harden the Microsoft SQL Database Server so that users can only see thier own database from SQL Server Managemnt Studio
{
	if (Test-Path "C:\Program Files\Microsoft SQL Server\MSSQL*\MSSQL\DATA\") {
		Write-Host "`t Configuring SQL Server Management Studio User Isolation" -ForegroundColor Green
		push-location
		# Import the SQL PowerShell Module
		Import-Module SQLPS -DisableNameChecking
		Invoke-Sqlcmd -Query "USE MASTER`n GO`n DENY VIEW ANY DATABASE TO PUBLIC`n GO`n" -ServerInstance "$env:COMPUTERNAME\$((Get-Item "C:\Program Files\Microsoft SQL Server\MSSQL*").Name.Split(".")[-1])"
		Pop-Location
	}
}


####################################################################################################################################################################################
Function Harden_MSSQL_UserIsolationUnDo()               # Function to undo the Microsoft SQL Database Server hardening from the function above
{
	if (Test-Path "C:\Program Files\Microsoft SQL Server\MSSQL*\MSSQL\DATA\") {
		Write-Host "`t Un-Doing SQL Server Management Studio User Isolation" -ForegroundColor Green
		push-location
		# Import the SQL PowerShell Module
		Import-Module SQLPS -DisableNameChecking
		Invoke-Sqlcmd -Query "USE MASTER`n GO`n GRANT VIEW ANY DATABASE TO PUBLIC`n GO`n" -ServerInstance "$env:COMPUTERNAME\$((Get-Item "C:\Program Files\Microsoft SQL Server\MSSQL*").Name.Split(".")[-1])"
		Pop-Location
	}
}


####################################################################################################################################################################################
Function InstallPhpMyAdmin()                            # Function to install phpMyAdmin on a server so customers can securely administer thier MySQL or MariaDB databases
{
	if ( (!(Test-Path "C:\phpMyAdmin\")) -and ($dInstalPhpMyAdmin) ) { # Install phpMyAdmin - current version if not already installed
		Write-Host "`tInstalling the features required for phpMyAdmin" -ForegroundColor Cyan
		# Install the additional IIS Requirements for phpMyAdmin
		(Add-WindowsFeature -Name Web-CGI) | Out-Null
		# Import the PowerShell Web Administration Module
		Import-Module WebAdministration
		# Configure all logging options to be enabled for websites in IIS
		Set-WebConfigurationProperty -Filter System.Applicationhost/Sites/SiteDefaults/logfile -Name LogExtFileFlags -Value "Date,Time,ClientIP,UserName,SiteName,ComputerName,ServerIP,Method,UriStem,UriQuery,HttpStatus,Win32Status,BytesSent,BytesRecv,TimeTaken,ServerPort,UserAgent,Cookie,Referer,ProtocolVersion,Host,HttpSubStatus"
		# Get the latest download information from the SolidCP Installer site
		($PHP_MyAdmin = (SolidCPFileDownload "PHP_MyAdmin")) | Out-Null
		# Create the phpMyAdmin Directory in our Installation Files folder ready for downloading if it doesn't already exist
		if (!(Test-Path "C:\_Install Files\$($PHP_MyAdmin.FolderName)")) { (md -Path "C:\_Install Files\$($PHP_MyAdmin.FolderName)" -Force) | Out-Null }
		# Download the files required for phpMyAdmin
		Write-Host "`t Downloading the latest version of phpMyAdmin" -ForegroundColor Green
		(New-Object System.Net.WebClient).DownloadFile("$($PHP_MyAdmin.DownloadURL)", "C:\_Install Files\$($PHP_MyAdmin.FolderName)\$($PHP_MyAdmin.FileName)")
		# Unzip the downloaded files to the C:\phpMyAdmin directory
		Write-Host "`t Extracting and installing phpMyAdmin from the downloaded files" -ForegroundColor Green
		(Add-Type -assembly "system.io.compression.filesystem" -ErrorAction SilentlyContinue) | Out-Null
		[io.compression.zipfile]::ExtractToDirectory("C:\_Install Files\$($PHP_MyAdmin.FolderName)\$($PHP_MyAdmin.FileName)", "C:\phpMyAdmin")
		(Move-Item "$((Get-ChildItem "C:\phpMyAdmin" | WHERE {$_.Name -like "phpMyAdmin-*"}).FullName)\DCO" "C:\phpMyAdmin") | Out-Null
		(Move-Item "$((Get-ChildItem "C:\phpMyAdmin" | WHERE {$_.Name -like "phpMyAdmin-*"}).FullName)\CONTRIBUTING.md" "C:\phpMyAdmin") | Out-Null
		(Move-Item "$((Get-ChildItem "C:\phpMyAdmin" | WHERE {$_.Name -like "phpMyAdmin-*"}).FullName)\LICENSE" "C:\phpMyAdmin") | Out-Null
		(Move-Item "$((Get-ChildItem "C:\phpMyAdmin" | WHERE {$_.Name -like "phpMyAdmin-*"}).FullName)\README" "C:\phpMyAdmin") | Out-Null
		(Move-Item "$((Get-ChildItem "C:\phpMyAdmin" | WHERE {$_.Name -like "phpMyAdmin-*"}).FullName)\RELEASE-DATE-*" "C:\phpMyAdmin") | Out-Null
		(Move-Item "$((Get-ChildItem "C:\phpMyAdmin" | WHERE {$_.Name -like "phpMyAdmin-*"}).FullName)\sql" "C:\phpMyAdmin") | Out-Null
		(Move-Item "$((Get-ChildItem "C:\phpMyAdmin" | WHERE {$_.Name -like "phpMyAdmin-*"}).FullName)\config.sample.inc.php" "C:\phpMyAdmin") | Out-Null
		(Move-Item "$((Get-ChildItem "C:\phpMyAdmin" | WHERE {$_.Name -like "phpMyAdmin-*"}).FullName)\doc\make.bat" "C:\phpMyAdmin") | Out-Null
		(Move-Item "$((Get-ChildItem "C:\phpMyAdmin" | WHERE {$_.Name -like "phpMyAdmin-*"}).FullName)\doc\Makefile" "C:\phpMyAdmin") | Out-Null
		(Move-Item "$((Get-ChildItem "C:\phpMyAdmin" | WHERE {$_.Name -like "phpMyAdmin-*"}).FullName)\*" "C:\phpMyAdmin\wwwroot\") | Out-Null
		(Remove-Item "$((Get-ChildItem "C:\phpMyAdmin" | WHERE {$_.Name -like "phpMyAdmin-*"}).FullName)" -Recurse) | Out-Null
		(md -Path "C:\phpMyAdmin\data") | Out-Null
		(md -Path "C:\phpMyAdmin\logs") | Out-Null

		# Create the phpMyAdmin configuration File Template file
		Write-Host "`t Creating phpMyAdmin secured configuration file" -ForegroundColor Green
		$randomSecret = (([guid]::NewGuid()).ToString().replace('-','').substring(0,15) + ([guid]::NewGuid()).ToString().replace('-','').substring(0,17))
		# update the config
		$newConfig = @"
<?php
//SolidCP phpMyAdmin Configuration
`$cfg['blowfish_secret'] = '$randomSecret';

`$i = 1;
`$cfg['Servers'][`$i]['auth_type'] = 'cookie';
`$cfg['Servers'][`$i]['host'] = 'localhost';
`$cfg['Servers'][`$i]['connect_type'] = 'tcp';
`$cfg['Servers'][`$i]['compress'] = false;
`$cfg['Servers'][`$i]['AllowNoPassword'] = false;

`$cfg['Servers'][`$i]['pmadb'] = 'phpmyadmin';
`$cfg['Servers'][`$i]['bookmarktable'] = 'pma__bookmark';
`$cfg['Servers'][`$i]['relation'] = 'pma__relation';
`$cfg['Servers'][`$i]['table_info'] = 'pma__table_info';
`$cfg['Servers'][`$i]['table_coords'] = 'pma__table_coords';
`$cfg['Servers'][`$i]['pdf_pages'] = 'pma__pdf_pages';
`$cfg['Servers'][`$i]['column_info'] = 'pma__column_info';
`$cfg['Servers'][`$i]['history'] = 'pma__history';
`$cfg['Servers'][`$i]['table_uiprefs'] = 'pma__table_uiprefs';
`$cfg['Servers'][`$i]['tracking'] = 'pma__tracking';
`$cfg['Servers'][`$i]['userconfig'] = 'pma__userconfig';
`$cfg['Servers'][`$i]['recent'] = 'pma__recent';
`$cfg['Servers'][`$i]['favorite'] = 'pma__favorite';
`$cfg['Servers'][`$i]['users'] = 'pma__users';
`$cfg['Servers'][`$i]['usergroups'] = 'pma__usergroups';
`$cfg['Servers'][`$i]['navigationhiding'] = 'pma__navigationhiding';
`$cfg['Servers'][`$i]['savedsearches'] = 'pma__savedsearches';
`$cfg['Servers'][`$i]['central_columns'] = 'pma__central_columns';
`$cfg['Servers'][`$i]['designer_settings'] = 'pma__designer_settings';
`$cfg['Servers'][`$i]['export_templates'] = 'pma__export_templates';
`$cfg['Servers'][`$i]['hide_db'] = '(performance_schema|information_schema|phpmyadmin|mysql|test');

`$cfg['SaveDir'] = '';
`$cfg['docSQLDir'] = 'docsql';
`$cfg['ShowPhpInfo'] = true;
`$cfg['ShowChgPassword'] = false;
`$cfg['LoginCookieRecall'] = false;
`$cfg['LoginCookieValidity'] = 1800;
`$cfg['AllowAnywhereRecording'] = true;
`$cfg['DefaultCharSet'] = 'iso-8859-1';
`$cfg['RecordingEngine'] = 'iconv';
`$cfg['IconvExtraParams'] = '//TRANSLIT';
`$cfg['GD2Available'] = 'no';
`$cfg['CheckConfigurationPermissions'] = FALSE;
`$cfg['ShowAll'] = true;

`$cfg['UploadDir'] = '';
`$cfg['SaveDir'] = '';

`$cfg['RowActionType'] = 'both';
?>
"@

		# Save the phpMyAdmin Config File
		$newConfig | Out-File C:\phpMyAdmin\wwwroot\config.inc.php
		# Configure the phpMyAdmin Database
		Write-Host  "`t Configuring the phpMyAdmin Database permissions for this server" -ForegroundColor Green
		([System.Reflection.Assembly]::LoadWithPartialName("MySql.Data")) | Out-Null
		$myconnection = New-Object MySql.Data.MySqlClient.MySqlConnection
		$myconnection.ConnectionString = "server=localhost;userid=root;password=$dMariaMySQLpasswd;pooling=false"
		$myconnection.Open()
		$mycommand = New-Object MySql.Data.MySqlClient.MySqlCommand
		$mycommand.Connection = $myconnection
		$mycommand.CommandText = "CREATE USER '$dPhpMyAdminUserNm'@'localhost' IDENTIFIED BY '$dPhpMyAdminPassWd';"
		$myreader = $mycommand.ExecuteReader()
		$myreader.close();
		$mycommand.CommandText = "GRANT SELECT (Host, User, Select_priv, Insert_priv, Update_priv, Delete_priv, Create_priv, Drop_priv, Reload_priv, Shutdown_priv, Process_priv,    File_priv, Grant_priv, References_priv, Index_priv, Alter_priv, Show_db_priv, Super_priv, Create_tmp_table_priv, Lock_tables_priv, Execute_priv, Repl_slave_priv, Repl_client_priv) ON mysql.user TO '$dPhpMyAdminUserNm'@'localhost';"
		$myreader = $mycommand.ExecuteReader()
		$myreader.close();
		$mycommand.CommandText = "GRANT SELECT ON mysql.db TO '$dPhpMyAdminUserNm'@'localhost';"
		$myreader = $mycommand.ExecuteReader()
		$myreader.close();
		$mycommand.CommandText = "GRANT SELECT ON mysql.host TO '$dPhpMyAdminUserNm'@'localhost';"
		$myreader = $mycommand.ExecuteReader()
		$myreader.close();
		$mycommand.CommandText = "GRANT SELECT (Host, Db, User, Table_name, Table_priv, Column_priv) ON mysql.tables_priv TO '$dPhpMyAdminUserNm'@'localhost';"
		$myreader = $mycommand.ExecuteReader()
		$myreader.close();
		$mycommand.CommandText = ([IO.File]::ReadAllText("C:\phpMyAdmin\sql\create_tables.sql"))
		$myreader = $mycommand.ExecuteReader()
		$myreader.close()
		$mycommand.CommandText = "GRANT SELECT, INSERT, DELETE, UPDATE ON phpmyadmin.* TO '$dPhpMyAdminUserNm'@'localhost';"
		$myreader = $mycommand.ExecuteReader()
		$myreader.close();	
		$myconnection.close()
		Write-Host "`t phpMyAdmin Database created successfully" -ForegroundColor Green

		# Change the bindings on the default website to Port 8080 so we can install phpmyadmin on Port 80 unless it has already been done and only if port 80 is specified for the phpMyAdmin website so it does not conflict
		if ( (Get-WebBinding -Name 'Default Web Site').bindingInformation -eq "*:$dPhpMyAdminPort`:" ) {
			Set-WebBinding -Name 'Default Web Site' -BindingInformation "*:$dPhpMyAdminPort`:" -PropertyName Port -Value 8080
		}
		# Create the AppPool for phpMyAdmin
		Write-Host "`t Creating the phpMyAdmin Application Pool in IIS" -ForegroundColor Green
		(New-WebAppPool -Name "SolidCP phpMyAdmin .NET v4" -Force) | Out-Null
		# Create the new website for phpMyAdmin
		Write-Host "`t Creating the phpMyAdmin website in IIS" -ForegroundColor Green
		(New-Website -Name "SolidCP phpMyAdmin" -Port $dPhpMyAdminPort -HostHeader "$(($env:computerName).ToLower() + $dPhpMyAdminHostNm + (([System.Net.Dns]::GetHostByName(($env:computerName)).HostName) -replace (($env:computerName).ToLower()), ''))" -IPAddress "$dIPV4" -PhysicalPath "C:\phpMyAdmin\wwwroot\" -ApplicationPool "SolidCP phpMyAdmin .NET v4" -Force) | Out-Null
		(Set-ItemProperty "IIS:\Sites\SolidCP phpMyAdmin" -name logfile.directory "C:\phpMyAdmin\logs\" -Force) | Out-Null
		# Grant Read & Execute permissions on wwwroot folder and Logs Folder
		SetAccessToFolderNoChk "C:\phpMyAdmin\wwwroot" "IIS AppPool\SolidCP phpMyAdmin .NET v4" "ReadAndExecute" "Allow";
		SetAccessToFolderNoChk "C:\phpMyAdmin\wwwroot" "BUILTIN\IIS_IUSRS" "ReadAndExecute" "Allow";
		SetAccessToFolderNoChk "C:\phpMyAdmin\wwwroot" "BUILTIN\$dLangUsersGroup" "ReadAndExecute" "Allow";
		SetAccessToFolderNoChk "C:\phpMyAdmin\logs" "NT SERVICE\TrustedInstaller" "FullControl" "Allow";
		# Set the "preloadEnabled" value to True to speed up loading on the phpMyAdmin site
		if (Test-Path "IIS:\Sites\SolidCP phpMyAdmin" -pathType container) {
			if ((Get-ItemProperty "IIS:\Sites\SolidCP phpMyAdmin" -Name applicationDefaults.preloadEnabled).Value -eq $false) {
				Set-ItemProperty "IIS:\Sites\SolidCP phpMyAdmin" -Name applicationDefaults.preloadEnabled -Value $true
			}
		}
		#SetAccessToFolderNoChk "C:\phpMyAdmin\wwwroot\config.inc.php" "IIS AppPool\SolidCP phpMyAdmin .NET v4" "FullControl" "Allow";
		(Restart-Service 'World Wide Web Publishing Service' -Force -WarningAction SilentlyContinue) | Out-Null
		start-Sleep -Seconds 5
		# Add the firewall rule to open the Port for phpMyAdmin only if it doesn't exist
		if ( !(Get-NetFirewallRule | where DisplayName -EQ "SolidCP phpMyAdmin - Port $dPhpMyAdminPort") ) {
			Write-Host "`tOpening port $dPhpMyAdminPort for phpMyAdmin on the Firewall" -ForegroundColor Cyan
			(New-NetFirewallRule -DisplayName "SolidCP phpMyAdmin - Port $dPhpMyAdminPort" -Direction Inbound –LocalPort $dPhpMyAdminPort -Protocol TCP -Action Allow) | Out-Null
			Write-Host "`t Firewall rule added successfully" -ForegroundColor Green
		}
	}
}


####################################################################################################################################################################################
Function InstallWebDavFeatures()                            # Function to install WebDav features
{
	if (Test-Path $("\\" + ($dSolidCPEnterpriseSvrIP) + "\c$\SolidCP\Enterprise Server\Web.config")) {
		Write-Host "`tInstalling the features required for Cloud Storage Portal" -ForegroundColor Cyan
		# Install the features required by the SolidCP Cloud Storage Portal on the server.
		(Add-WindowsFeature -Name Web-DAV-Publishing, FS-Resource-Manager, Search-Service, Web-Basic-Auth) | Out-Null
		# Create a local group called "Administrators File Access" on the serverif it does not exist
		CreateLocalUserOrGroup "Administrators File Access" "Local Administrators File Access - SCP Harden IIS  ********* DO NOT DELETE *********" "Group"
	}else{
		Write-Host "`n`t *************************************************" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *          The firewall check to your           *" -ForegroundColor Yellow
		Write-Host "`t *     SolidCP Enterprise Server has failed      *" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *    UNC Access is required from this server    *" -ForegroundColor Yellow
		Write-Host "`t *       to your SolidCP Enterprise Server       *" -ForegroundColor Yellow
		Write-Host "`t *                                               *" -ForegroundColor Yellow
		Write-Host "`t *************************************************" -ForegroundColor Yellow
		dPressAnyKeyToExit
	}
}


####################################################################################################################################################################################
Function CheckWebDavStorageFQDN()                           # Function to install the WebDav Storage Root Website
{
	Param(
		[switch]$FQDN           # Use the FQDN of the machine rather than asking
	)
	if ($FQDN) { # If the FQDN Switch has been specified then use the FQDN of this machine and do not prompt the user for a choice
		$script:dWebDavStorageHostName = $dFQDNthisMachine
	}else{
		if (!($dWebDavStorageHostName)) { # As the user for to choose the FQDN for the WebDav Server
			do {
				Write-Host "`n`tPlease select the FQDN Deployment for the `"SolidCP WebDav Storage`" website" -ForegroundColor Cyan
				Write-Host "`t  A. Use `"$dFQDNthisMachine`"`n`t  B. Enter my own FQDN" -ForegroundColor Cyan
				$choiceWebDavFQDN = Read-Host "`tEnter Option From Above Menu"
				$ok = $choiceWebDavFQDN -match '^[a-b]+$'
				if ( -not $ok) { Write-Host "`t Invalid selection" -ForegroundColor Yellow ; start-Sleep -Seconds 2 }
			} until ( $ok )
			switch -Regex ( $choiceWebDavFQDN ) {
				"A" {$script:dWebDavStorageHostName = $dFQDNthisMachine}
				"B" {do { $script:dWebDavStorageHostName = Read-Host "`tPlease enter the required FQDN for the `"SolidCP WebDav Storage`" website" } until (!([string]::IsNullOrEmpty($dWebDavStorageHostName)))}
			}
		}
	}
}


####################################################################################################################################################################################
Function InstallWebDavStorageRootWebsite()                  # Function to install the WebDav Storage Root Website
{
	# Check to make sure the WebDav Storage FQDN has been set
	if ($dWebDavStorageHostName) {
		# Import the PowerShell Web Administration Module
		Import-Module WebAdministration
		if ( (!(Test-Path "IIS:\Sites\$dWebDavStorageHostName")) -and (!(Test-Path "IIS:\AppPools\$dWebDavStorageHostName" )) ) {
			if (!($dWebDavStoragePath)) { # Specify the WebDav Storage Location
				do { $script:dWebDavStoragePath = Read-Host "`tPlease enter the WebDav Storage Path (i.e. C:\CloudStorage)" } until (!([string]::IsNullOrEmpty($dWebDavStoragePath)))
			}
			Write-Host "`tCreating the `"SolidCP WebDav Storage`" website in IIS" -ForegroundColor Cyan
			# Create the WebDav Storage Directory on the server if it doesn't already exist
			if (!(Test-Path "$dWebDavStoragePath")) { (md -Path "$dWebDavStoragePath" -Force) | Out-Null }
			# Create the AppPool for WebDav Storage only if it doesn't exist
			if (!(Test-Path "IIS:\AppPools\$dWebDavStorageHostName" )) {
				(New-WebAppPool -Name "$dWebDavStorageHostName" -Force) | Out-Null
				# Get the Application Pool for the SolidCP Cloud Storage Portal ready to make changes
				$pool = Get-Item "IIS:\AppPools\$dWebDavStorageHostName"
				$pool.autoStart = 'True'                                # Enable Auto-Start
				$pool.startMode = 'AlwaysRunning'                       # Set Start Mode as Always Running
				$pool.processModel.idleTimeout = [TimeSpan]::Zero       # Set Idele TimeOut as 0 Minutes
				$pool.recycling.periodicRestart.time = [TimeSpan]::Zero # Set Regular time interval as 0 Minutes
				$pool | Set-Item                                        # Set all of the above for the SolidCP Cloud Storage Portal Application Pool
				# Remove any schedules for the SolidCP Cloud Storage Portal Application Pool and set the time to be 3.00am as it needs to be recycled once a day
				Clear-ItemProperty "IIS:\AppPools\$dWebDavStorageHostName" -Name recycling.periodicRestart.schedule
				Set-ItemProperty "IIS:\AppPools\$dWebDavStorageHostName" -Name recycling.periodicRestart.schedule -Value @{value='03:00:00'}
				# Set the "preloadEnabled" value to True to speed up loading on the WebDav site
				Set-ItemProperty "IIS:\Sites\$dWebDavStorageHostName" -Name applicationDefaults.preloadEnabled -Value $true
			}
			# Create the new website for WebDav Storage
			(New-Website -Name "$dWebDavStorageHostName" -Port 80 -IPAddress "*" -PhysicalPath "$dWebDavStoragePath" -HostHeader "$dWebDavStorageHostName" -ApplicationPool "$dWebDavStorageHostName" -Force) | Out-Null
			# Enable WebDav Authoring on the site
			Set-WebConfiguration system.webServer/webdav/authoring -PSPath "IIS:\" -Location "$dWebDavStorageHostName" -Value @{enabled="True"}
			# Go to "Handler Mappings" then Edit Feature Permissions and disable Script
			Set-WebConfiguration "system.webServer/handlers/@AccessPolicy" -PSPath "IIS:\" -Location "$dWebDavStorageHostName" -value "Read"
			# enable Basic Auth on the website
			Set-WebConfiguration system.webServer/security/authentication/windowsAuthentication    -PSPath IIS:\ -Location "$dWebDavStorageHostName" -Value @{enabled="True"}  -WarningAction SilentlyContinue
			Set-WebConfiguration system.webServer/security/authentication/basicAuthentication      -PSPath IIS:\ -Location "$dWebDavStorageHostName" -Value @{enabled="False"} -WarningAction SilentlyContinue
			Set-WebConfiguration system.webServer/security/authentication/digestAuthentication     -PSPath IIS:\ -Location "$dWebDavStorageHostName" -Value @{enabled="False"} -WarningAction SilentlyContinue
			Set-WebConfiguration system.webServer/security/authentication/anonymousAuthentication  -PSPath IIS:\ -Location "$dWebDavStorageHostName" -Value @{enabled="False"} -WarningAction SilentlyContinue
			Set-WebConfiguration system.webServer/security/authentication/iisClientCertificateMapp -PSPath IIS:\ -Location "$dWebDavStorageHostName" -Value @{enabled="False"} -WarningAction SilentlyContinue
			Set-WebConfiguration system.webServer/security/authentication/ingAuthentication        -PSPath IIS:\ -Location "$dWebDavStorageHostName" -Value @{enabled="False"} -WarningAction SilentlyContinue
			Set-WebConfiguration system.webServer/security/authentication/clientCertificateMapping -PSPath IIS:\ -Location "$dWebDavStorageHostName" -Value @{enabled="False"} -WarningAction SilentlyContinue
			Set-WebConfiguration system.webServer/security/authentication/Authentication           -PSPath IIS:\ -Location "$dWebDavStorageHostName" -Value @{enabled="False"} -WarningAction SilentlyContinue
			# Restart IIS on the server
			(Restart-Service 'World Wide Web Publishing Service' -Force -WarningAction SilentlyContinue) | Out-Null
			Write-Host "`t Successfully created the `"SolidCP WebDav Storage`" website" -ForegroundColor Green
			# Ensure the Windows Search Service startup type is Automatic and start the service
			if ((Get-WmiObject -Class Win32_Service -Property StartMode -Filter "Name='WSearch'").StartMode -ne "Automatic") {
				(Set-Service -Name "WSearch" -StartupType Automatic -Status Running) | Out-Null
			}
			# Ensure the Windows Search Service status is started
			if ((Get-Service 'Windows Search').Status -eq "Running") {
				(Start-Service 'Windows Search' -Force -WarningAction SilentlyContinue) | Out-Null
			}
		}
	}
}


####################################################################################################################################################################################
Function dCheckPendingRestart()                             # Function to check if a restart is pending on this machine
{
	# Check if a restart is pending on this machine
	if (!([string]::IsNullOrEmpty( ((Get-WindowsFeature | where-object {$_.Installed -eq $True}).InstallState | Where-Object {$_ -eq "InstallPending"}) ))) {
		Write-Host "`tA Restart is pending on this machine" -ForegroundColor Yellow
		Write-Host "`tPlease run this script again once the server has rebooted" -ForegroundColor Green
		if ($psISE) { # Check if running Powershell ISE
			Add-Type -AssemblyName System.Windows.Forms
			[System.Windows.Forms.MessageBox]::Show("Press any key to reboot")
			Restart-Computer
			Exit
		}else{
			Write-Host "`n`tPress any key to reboot..." -ForegroundColor Yellow
			$x = $host.ui.RawUI.ReadKey("NoEcho,IncludeKeyDown")
			Restart-Computer
			Exit
		}
	}
}


####################################################################################################################################################################################
Function CreateExchangeContentSubmittersGroup()                                 # Create the "ContentSubmitters" security group if it does not exist - Exchange DAG Fix (Content Index failing)
{
	# Import the Active Directory PowerShell Module
	(Import-Module ActiveDirectory) | Out-Null
	# Load the PowerShell Exchange Snapin
	add-pssnapin *exchange* -erroraction SilentlyContinue
	# Check if the "Microsoft Exchange Security Groups" Organisational Unit exists in Active Directory
	if ([adsi]::Exists("LDAP://OU=Microsoft Exchange Security Groups,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)")) {
		# Check if the "ContentSubmitters" Group exists in the "Microsoft Exchange Security Groups" Organisational Unit in Active Directory
		if (! ([adsi]::Exists("LDAP://CN=ContentSubmitters,OU=Microsoft Exchange Security Groups,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)")) ) {
			# Create the "ContentSubmitters" security group and add the "Administrators" and "Network Service" groups to it
			Write-Host "`t Creating the `"ContentSubmitters`" Security Group for Exchange (DAG Fix)" -ForegroundColor Green
			(New-ADGroup -Name "ContentSubmitters" -SamAccountName "ContentSubmitters" -GroupCategory "Security" -GroupScope "Universal" -DisplayName "ContentSubmitters" -Path "OU=Microsoft Exchange Security Groups,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)" -Description "Required for Microsoft DAG to fix Content Index Failing issues") | Out-Null
			# Check to make sure the group is created before adding members to it
			do { $dCheckContentSubmitters = (([ADSI]("WinNT://$env:USERDNSDOMAIN/ContentSubmitters")).Path) } until ($dCheckContentSubmitters -ne $null)
			start-sleep -Seconds "25"
			Write-Host "`t Adding `"Network Service`" to the `"ContentSubmitters`" Security Group" -ForegroundColor Green
			(Add-ADPermission -ID "ContentSubmitters" -User “Network Service” -AccessRights "GenericAll") | Out-Null
			Write-Host "`t Adding `"Administrators`" to the `"ContentSubmitters`" Security Group" -ForegroundColor Green
			(Add-ADPermission -Identity "ContentSubmitters" -User “Administrators” -AccessRights "GenericAll") | Out-Null
		}
	}
}


####################################################################################################################################################################################
function Get-ExchangeOrgName{           # Function to get the Exchange Organisation Name
	$domain = [System.DirectoryServices.ActiveDirectory.Domain]::GetCurrentDomain() 
	$ddn = $domain.GetDirectoryEntry().distinguishedName 
	$config = [ADSI]"LDAP://CN=Microsoft Exchange,CN=Services,CN=Configuration,$ddn" 
	$orgName = $config.psbase.children | where {$_.objectClass -eq 'msExchOrganizationContainer'} 
	$orgName.name 
}


####################################################################################################################################################################################
Function CheckExchangeDatabase()        # Function to Get and Check Exchange Databases and Mailboxes exist
{
	Param(
		[string]$DatabaseName,  # Specify the Database Name to check against
		[string]$DBname_Like,   # Specify a Search Query to check the Database Name against
		[string]$User_Exists,   # Check if a Exchange Mailbox exists
		[string]$User_Database, # Check if a Exchange Mailbox exists and return the Database Name where it is located
		[switch]$ThisServer     # Get the Database Name on this server
	)
	# Load the PowerShell Exchange Snapin
	add-pssnapin *exchange* -erroraction SilentlyContinue
	# Search the Exchange Servers Database list to see if the Database Name exists
	if ($DatabaseName) { 
		foreach ($Name in ((Get-MailboxDatabase).name)) {
			if ($Name -eq $DatabaseName) { $true }
		}
	# Search the Exchange Servers Database list to see if the Database Name exists based upon the search query
	}elseif ($DBname_Like) {
		foreach ($Name in ((Get-MailboxDatabase).name)) {
			if ($Name -like $DBname_Like) { $Name }
		}
	# Check if a Mailbox exists based upon the mailbox name
	}elseif ($User_Exists) {
		if ((Get-mailbox -identity "$User_Exists" -ErrorAction SilentlyContinue) -ne $null) {
			$true
		}
	# Check if a Mailbox exists based upon the mailbox name and return the Mailbox Database Name
	}elseif ($User_Database) {
		if (Get-mailbox -identity "$User_Database" -ErrorAction SilentlyContinue) {
			((Get-mailbox -identity "$User_Database" -ErrorAction SilentlyContinue).Database.Name)
		}
	# Search the Exchange Servers and list all existing Database Names
	}elseif ($ThisServer) {
		((Get-MailboxDatabase -Server $env:COMPUTERNAME).name)
	}else{
		((Get-MailboxDatabase).name)
	}
}


####################################################################################################################################################################################
Function GetExchangeNonDAGmembers()                         # Get the Exchange Servers that are not members of any DAG's
{
	# Load the PowerShell Exchange Snapin
	add-pssnapin *exchange* -erroraction SilentlyContinue
	if ( ((Get-ExchangeServer).Name) -ne $null ) {
		if ( ((Get-Cluster -Domain "$env:USERDNSDOMAIN" | Get-ClusterNode).Name) -eq $null) {
			( ((Get-ExchangeServer).Name) | Sort-Object )
		}else{
			( ((( (Compare-Object -ReferenceObject ((Get-ExchangeServer).Name) -DifferenceObject ((Get-Cluster -Domain "$env:USERDNSDOMAIN" | Get-ClusterNode).Name)) ) | Where {$_.sideIndicator -eq "<="} | select inputobject | Where {$_.inputobject -ne ""} | Where {$_.inputobject -ne $null }).inputobject) | Sort-Object )
		}
	}
}


####################################################################################################################################################################################
Function CreateNewDAG()                                     # Function for create a new Database Availability Group (DAG) for Exchange 2016
{
	if ($dDomainMember) {
		# Check to make sure the Exchange ContentSubmitters Security Group is created
		CreateExchangeContentSubmittersGroup
		while ([string]::IsNullOrEmpty($NewDAGName)) { $NewDAGName = Read-Host "`tEnter the name for the new DAG" }
		if (!(CheckDomainComputerObject -Name "$NewDAGName")) { # Check if the DAG does not exist
			while ([string]::IsNullOrEmpty($NewDAGFSW)) { $NewDAGFSW = Read-Host "`tEnter the name of your File Share Witness Server (Machine Name ONLY)" }
			if (CheckDomainComputerObject -Name $NewDAGFSW) { # Check if the File Share Witness Server exists in the Computers container
				# Create the new DAG as a Computer Name Object (CNO) in Active Directory and make sure it is Disabled - Pre-Staging
				CreateDomainComputerObject -Name "$NewDAGName" -Disabled
				# Get a list of Exchange Servers that are not members of any DAG's and ask if each one is to be added to this DAG
				$ChosenDAGmembers = @()
				foreach ($ExchangeServerName in (GetExchangeNonDAGmembers) ) {
					$ChosenDAGmembers += AskToAdd -Value "$ExchangeServerName" -Question "this DAG"
				}
				# Add the chosen Exchange Servers to the Computer Object we created above
				foreach ($ChosenServer in $ChosenDAGmembers) {
					AddDomainComputerToComputerObject -Computer "$NewDAGName" -Add "$ChosenServer"
				}
				# Ask for the IPV4 address for the new DAG
				while ((CheckIP -IPV4 "$DAGipv4") -ne $true) {
					$DAGipv4 = Read-Host "`tPlease enter the IPV4 Address for this new DAG"
				}
				if ($DAGipv4 -match "/") {
					$DAGipv4 = (($DAGipv4 -split "/")[0])
				}
				# Ask for the IPV6 address for the new DAG
				while ($IPV6DAGchoice -notmatch "[Y|N]") {
					$IPV6DAGchoice = Read-Host "`tWould you like to enable IPV6 for this DAG`? (Y/N)"
				}
				if ($IPV6DAGchoice -eq "Y") {
					while ((CheckIP -IPV6 "$DAGipv6") -ne $true) {
						$DAGipv6 = Read-Host "`tPlease enter the IPV6 Address for this new DAG"
					}
					if ($DAGipv6 -match "/") {
						$DAGipv6 = (($DAGipv6 -split "/")[0])
					}
				}
				#Create the new DAG in Exchange
				add-pssnapin *exchange* -erroraction SilentlyContinue
				(New-DatabaseAvailabilityGroup -Name "$NewDAGName" -DatabaseAvailabilityGroupIPAddresses "$DAGipv4" -WitnessServer "$NewDAGFSW" -WitnessDirectory "C:\DAGFileShareWitnesses\$NewDAGName") | Out-Null
				# Check if the new DAG has been created on Exchange before continuing
				do { $dCeckIfDAGcreated = (((Get-DatabaseAvailabilityGroup).Name) -contains "$NewDAGName") } until ($dCeckIfDAGcreated -eq $true)
				Write-Host "`t `"$NewDAGName`" created successfully" -ForegroundColor Green
				# Add each chosen server to the newly created DAG
				foreach ($ChosenServer in $ChosenDAGmembers ) {
					Write-Host "`t Adding `"$ChosenServer`" to the `"$NewDAGName`" DAG" -ForegroundColor Green
					Add-DatabaseAvailabilityGroupServer -Identity "$NewDAGName" -MailboxServer "$ChosenServer"
				}
				# Add the IPV6 address to the DAG if it has been set above
				if ($DAGipv6) {
					# Create a new IPv6 Cluster IP Resource
					(Add-ClusterResource -Name "IPv6 Cluster Address" -ResourceType "IPv6 Address" -Group "Cluster Group") | Out-Null
					# Set the properties for the newly created IP Address resource
					(Get-ClusterResource "IPv6 Cluster Address" | Set-ClusterParameter –Multiple @{“Network"="Cluster Network 1"; "Address"= "$DAGipv6";"PrefixLength"="$(((Get-NetIPAddress –AddressFamily IPv6 | Select-Object PrefixLength,IPAddress) -match "$((Resolve-DnsName -Name $env:COMPUTERNAME -Type AAAA).IPAddress -notlike 'fe80:*')").PrefixLength)"}) | Out-Null
					(Stop-ClusterResource "Cluster Name") | Out-Null
					(Set-ClusterResourceDependency "Cluster Name" "[Ipv6 Cluster Address] or [Cluster IP Address]") | Out-Null
					(Start-ClusterResource "Cluster Name") | Out-Null
					Write-Host "`t IPV6 Address added to the DAG `"$NewDAGName`"" -ForegroundColor Green
				}
			}else{ # The File Share Witness Server does not exist
				Write-Host "`t The FSW Server `"$NewDAGFSW`" does not exist in the `"Computers`" container in Active Directory" -ForegroundColor Yellow
			}
		}else{ # The DAG already exists
			Write-Host "`t The DAG `"$NewDAGName`" already exists`!" -ForegroundColor Yellow
		}
	}else{
		write-host "`n`tYou need to be logged in as a Domain User who is a member of the " -ForegroundColor Red
		start-Sleep -Seconds 5
	}
}


####################################################################################################################################################################################
Function CreateNewExchangeFailoverCluster()                 # Function for create a new Failover Cluster for Exchange 2016
{
	if ($dDomainMember) {
		while ([string]::IsNullOrEmpty($NewClusterName)) { $NewClusterName = Read-Host "`tEnter the name for the new Failover Cluster" }
		if (!(CheckDomainComputerObject -Name "$NewClusterName")) { # Check if the Failover Cluster does not exist
			# Get a list of Exchange Servers that are not members of any Cluster's and ask if each one is to be added to this Cluster
			$ChosenClustermembers = @()
			foreach ($ExchangeServerName in (GetExchangeNonDAGmembers) ) {
				$ChosenClustermembers += AskToAdd -Value "$ExchangeServerName" -Question "this Failover Cluster"
			}
			# Ask for the IPV4 address for the new Cluster
			while ((CheckIP -IPV4 "$Clusteripv4") -ne $true) { $Clusteripv4 = Read-Host "`tPlease enter the IPV4 Address for this new Failover Cluster" }
			if ($Clusteripv4 -match "/") { $Clusteripv4 = (($Clusteripv4 -split "/")[0]) }
			# Ask for the IPV6 address for the new Cluster
			while ($IPV6Clusterchoice -notmatch "[Y|N]") { $IPV6Clusterchoice = Read-Host "`tWould you like to enable IPV6 for this Failover Cluster`? (Y/N)" }
			if ($IPV6Clusterchoice -eq "Y") {
				while ((CheckIP -IPV6 "$Clusteripv6") -ne $true) { $Clusteripv6 = Read-Host "`tPlease enter the IPV6 Address for this new Failover Cluster" }
				if ($Clusteripv6 -match "/") { $Clusteripv6 = (($Clusteripv6 -split "/")[0]) }
			}
			# Install the Windows Failover Clustering Feature on the selected machines
			foreach ($dNode in $ChosenClustermembers) {
				(Add-WindowsFeature -ComputerName $dNode -Name Failover-Clustering) | Out-Null
			}
			#Create the new Failover Cluster for Exchange
			(New-Cluster -Name "$NewClusterName" -Node $ChosenClustermembers -StaticAddress "$Clusteripv4" -NoStorage -WarningAction SilentlyContinue) | Out-Null
			# Check if the new Failover Cluster has been created on Exchange before continuing
			do { $dCeckIfClustercreated = (((Get-Cluster -Domain "$env:USERDNSDOMAIN").Name) -contains "$NewClusterName") } until ($dCeckIfClustercreated -eq $true)
			Write-Host "`t The cluster `"$NewClusterName`" has been created successfully" -ForegroundColor Green
			# Add the IPV6 address to the Cluster if it has been set above
			if ($Clusteripv6) {
				# Create a new IPv6 Failover Cluster IP Resource
				(Add-ClusterResource -Name "IPv6 Cluster Address" -ResourceType "IPv6 Address" -Group "Cluster Group") | Out-Null
				# Set the properties for the newly created IP Address resource
				(Get-ClusterResource "IPv6 Cluster Address" | Set-ClusterParameter –Multiple @{“Network"="Cluster Network 1"; "Address"= "$Clusteripv6";"PrefixLength"="$(((Get-NetIPAddress –AddressFamily IPv6 | Select-Object PrefixLength,IPAddress) -match "$((Resolve-DnsName -Name $env:COMPUTERNAME -Type AAAA).IPAddress -notlike 'fe80:*')").PrefixLength)"}) | Out-Null
				(Stop-ClusterResource "Cluster Name") | Out-Null
				(Set-ClusterResourceDependency "Cluster Name" "[Ipv6 Cluster Address] or [Cluster IP Address]") | Out-Null
				if (Get-OSName -Check "2016") { (Get-ClusterResource | Where {$_.ResourceType -eq "IPv6 Address"} | Where {$_.Name -ne "IPv6 Cluster Address"} | Remove-ClusterResource -Force) | Out-Null }
				(Start-ClusterResource "Cluster Name") | Out-Null
				Write-Host "`t IPV6 Address added to the Cluster `"$NewClusterName`"" -ForegroundColor Green
			}
		}else{ # The Cluster already exists
			Write-Host "`t The Failover Cluster `"$NewClusterName`" already exists`!" -ForegroundColor Yellow
		}
	}else{
		write-host "`n`tYou need to be logged in as a Domain User who is a member of the " -ForegroundColor Red
		start-Sleep -Seconds 5
	}
}


####################################################################################################################################################################################
Function AddDAGMember()                                     # Function add an Exchange Server to an Exchange Database Availability Group (DAG) for Exchange 2016
{
	if ($dDomainMember) {
		$NewDAGmemberChoice = ""
		while ($NewDAGmemberChoice -notmatch "[Y|N]") { $NewDAGmemberChoice = read-host "`tWould you like to add an additional server to an existing DAG for Exchange`? (Y/N)" }
		if ($NewDAGmemberChoice -eq "y") {
			# Check to make sure the Exchange ContentSubmitters Security Group is created
			CreateExchangeContentSubmittersGroup
			# Import the Microsoft Exchange PowerShell Snap In
			add-pssnapin *exchange* -erroraction SilentlyContinue
			Write-Host "`tExisting Database Availability Groups setup on your Exchange Enviroment" -ForegroundColor Yellow
			foreach ($dDAG in ((Get-DatabaseAvailabilityGroup).Name)) {
				Write-Host "`t  $dDAG" -ForegroundColor Yellow
			}
			while ([string]::IsNullOrEmpty($DAGName)) { $DAGName = Read-Host "`tEnter the name of the DAG you want to add Exchange Servers to from the list above" }
			if (CheckDomainComputerObject -Name "$DAGName") { # Check if the DAG exists
				# Get a list of Exchange Servers that are not members of any DAG's and ask if each one is to be added to this DAG
				$ChosenDAGmembers = @()
				foreach ($ExchangeServerName in (GetExchangeNonDAGmembers) ) {
					$ChosenDAGmembers += AskToAdd -Value "$ExchangeServerName" -Question "this DAG"
				}
				# Add the chosen Exchange Servers to the Computer Object for the chosen DAG
				foreach ($ChosenServer in $ChosenDAGmembers) {
					AddDomainComputerToComputerObject -Computer "$DAGName" -Add "$ChosenServer"
				}
				# Add each chosen server to the chosen DAG
				foreach ($ChosenServer in $ChosenDAGmembers ) {
					Write-Host "`t Adding `"$ChosenServer`" to the `"$DAGName`" DAG" -ForegroundColor Green
					Add-DatabaseAvailabilityGroupServer -Identity "$DAGName" -MailboxServer "$ChosenServer"
				}
			}else{ # The DAG does not exist
				Write-Host "`t The DAG `"$DAGName`" does not exist`!" -ForegroundColor Yellow
			}
		}
	}
}


####################################################################################################################################################################################
Function AddThisServerToDAG()                               # Function add this Exchange Server to an Exchange Database Availability Group (DAG) for Exchange 2016
{
	if ($dDomainMember) {
		$AddThisServerToDAGchoice = ""
		while ($AddThisServerToDAGchoice -notmatch "[Y|N]") { $AddThisServerToDAGchoice = read-host "`tWould you like to add this server to an existing DAG for Exchange`? (Y/N)" }
		if ($AddThisServerToDAGchoice -eq "y") {
			# Check to make sure the Exchange ContentSubmitters Security Group is created
			CreateExchangeContentSubmittersGroup
			# Import the Microsoft Exchange PowerShell Snap In
			add-pssnapin *exchange* -erroraction SilentlyContinue
			Write-Host "`tExisting Database Availability Groups setup on your Exchange Enviroment" -ForegroundColor Yellow
			foreach ($dDAG in ((Get-DatabaseAvailabilityGroup).Name)) {
				Write-Host "`t  $dDAG" -ForegroundColor Yellow
			}
			while ([string]::IsNullOrEmpty($DAGName)) { $DAGName = Read-Host "`tEnter the name of the DAG you want to add Exchange Servers to from the list above" }
			if (CheckDomainComputerObject -Name "$DAGName") { # Check if the DAG exists
				# Add this Exchange Server to the Computer Object for the chosen DAG
				AddDomainComputerToComputerObject -Computer "$DAGName" -Add "$env:COMPUTERNAME"
				# Add this Exchange Sserver to the chosen DAG
				Write-Host "`t Adding `"$env:COMPUTERNAME`" to the `"$DAGName`" DAG" -ForegroundColor Green
				Add-DatabaseAvailabilityGroupServer -Identity "$DAGName" -MailboxServer "$env:COMPUTERNAME"
			}else{ # The DAG does not exist
				Write-Host "`t The DAG `"$DAGName`" does not exist`!" -ForegroundColor Yellow
			}
		}
	}
}


####################################################################################################################################################################################
Function InstallExchangeCAS()           # Function to install the features required for the Microsoft Exchange CAS Server Role
{
	Write-Host "`tInstalling the features required for Microsoft Exchange Server (CAS Role)" -ForegroundColor Cyan
	(Add-WindowsFeature -Name AS-HTTP-Activation, Web-Request-Monitor, Web-Basic-Auth, Web-Digest-Auth, Web-Dyn-Compression, Web-Lgcy-Mgmt-Console, Web-Mgmt-Service, Web-Metabase, Web-WMI, NET-Framework-45-Features, Desktop-Experience, RSAT-Clustering, RSAT-ADDS, RPC-over-HTTP-proxy, Windows-Identity-Foundation, WAS-Process-Model, Web-IP-Security, NLB, RSAT-NLB) | Out-Null
}


####################################################################################################################################################################################
Function InstallExchangeMBX()           # Function to install the features required for the Microsoft Exchange Mailbox Server Role
{
	Write-Host "`tInstalling the features required for Microsoft Exchange Server (Mailbox Role)" -ForegroundColor Cyan
	(Add-WindowsFeature -Name AS-HTTP-Activation, Web-Request-Monitor, Web-Basic-Auth, Web-Digest-Auth, Web-Dyn-Compression, Web-Lgcy-Mgmt-Console, Web-Mgmt-Service, Web-Metabase, Web-WMI, NET-Framework-45-Features, Desktop-Experience, RSAT-Clustering, RSAT-ADDS, RPC-over-HTTP-proxy, Windows-Identity-Foundation, WAS-Process-Model, RSAT-Clustering-CmdInterface) | Out-Null
}


####################################################################################################################################################################################
Function Exchange2016_PreCheck()  # Function to check Exchange 2016 Installation Options have all been set prior to installing
{
	# Check to see if the Exchange Installation Directory has already been set, if not then ask for one to be entered
	if (!$dExchangeInstal) {
		while ( ([string]::IsNullOrEmpty($dExchangeInstal)) -or (!(Test-Path "$dExchangeInstal")) ) {
			$script:dExchangeInstal = Read-Host "`n`tEnter the location of the Exchange setup files you would like to use for this installation"
		}
		if ($dExchangeInstal.Substring($dExchangeInstal.Length -1, 1) -eq "\") { $script:dExchangeInstal = ($dExchangeInstal.Substring(0, $dExchangeInstal.Length -1)) }
		Write-Host "`t Exchange will be installed from `"$dExchangeInstal`"" -ForegroundColor Green
	}
	#Check if the Exchange Installation Directory has files in it
	if ( (!(Test-Path "$dExchangeInstal")) -or (((Get-ChildItem "$dExchangeInstal" | Measure-Object).Count) -eq "0") ) {
		Write-Host "`tThe Exchange Installation Directory does not contain any files`!" -ForegroundColor Yellow
		Write-Host "`tPlease check the directory you specified is correct and run this script again" -ForegroundColor Yellow
		dPressAnyKeyToExit
	}
	# Check if there are already any Exchange Servers on the domain
	if ( [string]::IsNullOrEmpty(([ADSI]("LDAP://CN=ms-Exch-Schema-Version-Pt," + ([ADSI]"LDAP://RootDSE").schemaNamingContext)).rangeUpper) ) {
		# Check to see if the Exchange Organisation Name has already been set, if not then ask for one to be entered
		if (!($dExchangeOrgNme)) {
			while ([string]::IsNullOrEmpty($dExchangeOrgNme)) {
				$script:dExchangeOrgNme = Read-Host "`n`tEnter the Exchange Organisation Name you would like to use for this installation"
			}
		}
		Write-Host "`t Your Exchange Organisation Name will be `"$dExchangeOrgNme`"" -ForegroundColor Green
		# Check if a new Organisational Unit needs to be created for SolidCP
		if (!($dSolidCp_OU_Name)) {
			$choice = ""
			Write-Host "`n`tIt is reccomended that you have a Dedicated Organisational Unit for SolidCP" -ForegroundColor Yellow
			while ($choice -notmatch "[y|n]") {
				$choice = Read-Host "`n`t Do you wan to install a new Organisational Unit for SolidCP`? (Y/N)"
			}
			if ($choice -eq "y") {
				$script:dSolidCp_OU_Name = Read-Host "`n`t Please enter the name for the Organisational Unit"
				if (!($dSolidCp_OU_Name)) {$script:dSolidCp_OU_Name = "SolidCP"}
			}
		}
		Write-Host "`t Your Dedicated Organisational Unit will be `"$dSolidCp_OU_Name`"" -ForegroundColor Green
		##dCreateNewOU -Name "$dSolidCp_OU_Name" -Description "SolidCP Organisational Unit for Hosted Customers" -DiableInheritance -RemoveUserFromACL "S-1-5-32-554"
		#dCreateNewOU -Name "$dSolidCp_OU_Name" -Description "SolidCP Organisational Unit for Hosted Customers"
		CreateSolidCPdomainOU -Hosted
	}else{ # Otherwise get the Exchange Organisation Name from Active Directory
		$script:dExchangeOrgNme = Get-ExchangeOrgName
		Write-Host "`t Your Exchange Organisation Name is `"$dExchangeOrgNme`"" -ForegroundColor Green
	}
	# Ask for the Exchange Database Name on this server
	if (!$dExchangeDatabaseName) {
		while ([string]::IsNullOrEmpty($dExchangeDatabaseName)) {
			$script:dExchangeDatabaseName = Read-Host "`n`tEnter the Exchange Database Name you would like to use on this server"
		}
		Write-Host "`t The Exchange Database Name will be `"$dExchangeDatabaseName`"`n" -ForegroundColor Green
	}
}


####################################################################################################################################################################################
Function Exchange2016PreReq()           # Function to install the features required for the Microsoft Exchange 2016
{
	Write-Host "`tInstalling the features required for Microsoft Exchange 2016 Server (Mailbox Role)" -ForegroundColor Cyan
	Write-Host "`t  ****************************************" -ForegroundColor Green
	Write-Host "`t  *                                      *" -ForegroundColor Green
	Write-Host "`t  *  Please be patient while we install  *" -ForegroundColor Green
	Write-Host "`t  *    the required Winodws Features     *" -ForegroundColor Green
	Write-Host "`t  *                                      *" -ForegroundColor Green
	Write-Host "`t  ****************************************" -ForegroundColor Green
	# Install Windows Features for Server 2012 ONLY
	if (Get-OSName -Check "2012") { (Add-WindowsFeature -Name AS-HTTP-Activation, Desktop-Experience -WarningAction SilentlyContinue) | Out-Null }
	# Install Windows Features for Server 2012 and Server 2016
	(Add-WindowsFeature -Name Web-Request-Monitor, Web-Basic-Auth, Web-Digest-Auth, Web-Dyn-Compression, Web-Lgcy-Mgmt-Console, Web-Mgmt-Service, Web-Metabase, Web-WMI, NET-Framework-45-Features, RSAT-Clustering, RSAT-ADDS, RPC-over-HTTP-proxy, Windows-Identity-Foundation, WAS-Process-Model, RSAT-Clustering-CmdInterface, RSAT-Clustering-Mgmt, RSAT-Clustering-PowerShell, RSAT-AD-PowerShell, GPMC -WarningAction SilentlyContinue) | Out-Null
	# Check if a restart is pending on this machine after installing the required features for Exchange 2016
	dCheckPendingRestart
	# Create a local group called "Administrators File Access" on the serverif it does not exist
	CreateLocalUserOrGroup "Administrators File Access" "Local Administrators File Access - SCP Harden IIS  ********* DO NOT DELETE *********" "Group"
}


####################################################################################################################################################################################
Function Exchange2016PreReqMgmtTools()  # Function to install the features required for the Microsoft Exchange 2016 Management Tools
{
	Write-Host "`tInstalling the features required for Microsoft Exchange 2016 Server (Mailbox Role)" -ForegroundColor Cyan
	Write-Host "`t  ****************************************" -ForegroundColor Green
	Write-Host "`t  *                                      *" -ForegroundColor Green
	Write-Host "`t  *  Please be patient while we install  *" -ForegroundColor Green
	Write-Host "`t  *    the required Winodws Features     *" -ForegroundColor Green
	Write-Host "`t  *                                      *" -ForegroundColor Green
	Write-Host "`t  ****************************************" -ForegroundColor Green
	# Install Windows Features for Server 2012 ONLY
	if (Get-OSName -Check "2012") { (Add-WindowsFeature -Name AS-HTTP-Activation, Desktop-Experience -WarningAction SilentlyContinue) | Out-Null }
	# Install Windows Features for Server 2012 and Server 2016
	(Add-WindowsFeature -Name Web-Request-Monitor, Web-Basic-Auth, Web-Digest-Auth, Web-Dyn-Compression, Web-Lgcy-Mgmt-Console, Web-Mgmt-Service, Web-Metabase, Web-WMI, NET-Framework-45-Features, RSAT-ADDS, RPC-over-HTTP-proxy, Windows-Identity-Foundation, WAS-Process-Model, RSAT-AD-PowerShell, GPMC -WarningAction SilentlyContinue) | Out-Null
	# Check if a restart is pending on this machine after installing the required features for Exchange 2016
	dCheckPendingRestart
	# Create a local group called "Administrators File Access" on the serverif it does not exist
	CreateLocalUserOrGroup "Administrators File Access" "Local Administrators File Access - SCP Harden IIS  ********* DO NOT DELETE *********" "Group"
}


####################################################################################################################################################################################
Function InstallDotNET452()             # Function to install the .NET Framework v4.5.2
{
	# Get the latest download information from the SolidCP Installer site
	($Exchange_2016_DotNET_4_5_2 = (SolidCPFileDownload "Exchange_2016_DotNET_4_5_2")) | Out-Null
	# Create the Exchange 2016 Pre-Requisites Directory in our Installation Files folder ready for downloading if it doesn't already exist
	if (!(Test-Path "C:\_Install Files\$($Exchange_2016_DotNET_4_5_2.FolderName)")) { (md -Path "C:\_Install Files\$($Exchange_2016_DotNET_4_5_2.FolderName)" -Force) | Out-Null }
	# Check if Microsoft .NET Framework v4.5.2 is installed, if not then download and install it
	if ( (Get-Itemproperty "HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" | Select-Object -ExpandProperty "Release") -le "379892" ) {
		# Download the Microsoft .NET Framework v4.5.2
		if (!(Test-Path "C:\_Install Files\$($Exchange_2016_DotNET_4_5_2.FolderName)\$($Exchange_2016_DotNET_4_5_2.FileName)")) {
			Write-Host "`t Downloading the Exchange 2016 Pre-Requisites (Microsoft .NET Framework v4.5.2)" -ForegroundColor Green
			(New-Object System.Net.WebClient).DownloadFile("$($Exchange_2016_DotNET_4_5_2.DownloadURL)", "C:\_Install Files\$($Exchange_2016_DotNET_4_5_2.FolderName)\$($Exchange_2016_DotNET_4_5_2.FileName)")
		}
		# Install the Microsoft .NET Framework v4.5.2
		Write-Host "`t Installing the Pre-Requisites for Exchange 2016 (Microsoft .NET Framework v4.5.2)" -ForegroundColor Green
		(Start-Process -FilePath "C:\_Install Files\$($Exchange_2016_DotNET_4_5_2.FolderName)\$($Exchange_2016_DotNET_4_5_2.FileName)" -Argumentlist "/passive" -Wait -Passthru).ExitCode | Out-Null
	}
}


####################################################################################################################################################################################
Function InstallExchangeUCMA4()         # Function to install the Unified Communications Managed API 4.0 Runtime
{
	# Get the latest download information from the SolidCP Installer site
	($Exchange_2016_UC_API_4_0 = (SolidCPFileDownload "Exchange_2016_UC_API_4_0")) | Out-Null
	# Create the Exchange 2016 Pre-Requisites Directory in our Installation Files folder ready for downloading if it doesn't already exist
	if (!(Test-Path "C:\_Install Files\$($Exchange_2016_UC_API_4_0.FolderName)")) { (md -Path "C:\_Install Files\$($Exchange_2016_UC_API_4_0.FolderName)" -Force) | Out-Null }
	# Check if Unified Communications Managed API 4.0 Runtime is installed, if not then download and install it
	if ( !(Test-Path "C:\Program Files\Microsoft UCMA 4.0")) {
		# Download the Microsoft .NET Framework v4.5.2
		if (!(Test-Path "C:\_Install Files\$($Exchange_2016_UC_API_4_0.FolderName)\$($Exchange_2016_UC_API_4_0.FileName)")) {
			Write-Host "`t Downloading the Exchange 2016 Pre-Requisites (Unified Communications Managed API 4.0 Runtime)" -ForegroundColor Green
			(New-Object System.Net.WebClient).DownloadFile("$($Exchange_2016_UC_API_4_0.DownloadURL)", "C:\_Install Files\$($Exchange_2016_UC_API_4_0.FolderName)\$($Exchange_2016_UC_API_4_0.FileName)")
		}
		# Install the Unified Communications Managed API 4.0 Runtime
		Write-Host "`t Installing the Exchange 2016 Pre-Requisites (Unified Communications Managed API 4.0 Runtime)" -ForegroundColor Green
		(Start-Process -FilePath "C:\_Install Files\$($Exchange_2016_UC_API_4_0.FolderName)\$($Exchange_2016_UC_API_4_0.FileName)" -Argumentlist "/passive" -Wait -Passthru).ExitCode | Out-Null
	}
}


####################################################################################################################################################################################
Function Exchange2016PrepareSchema()     # Function to Extend the Active Directory Schema for the Microsoft Exchange 2016 Mailbox Server Role
{
	# Check to see if Exchange is already installed on the domain
	if ( (((([ADSI]("LDAP://CN=ms-Exch-Schema-Version-Pt," + ([ADSI]"LDAP://RootDSE").schemaNamingContext)).rangeUpper).Value) -lt "15317") -and (!(Test-Path "C:\Program Files\Microsoft\Exchange Server\V15")) ) {
		# Extend the Active Directory Schema for Exchange 2016 installation
		Write-Host "`t Extending the Active Directory Schema for Microsoft Exchange" -ForegroundColor Green
		((Start-Process -FilePath "$dExchangeInstal\Setup.exe" -Argumentlist "/PrepareSchema /IAcceptExchangeServerLicenseTerms" -Wait -Passthru).ExitCode) | Out-Null
		# Check to make sure the Active Directory Schema has been extended for Microsoft Exchange
		do { $dCheckExchgSchemaVer = ((([ADSI]("LDAP://CN=ms-Exch-Schema-Version-Pt," + ([ADSI]"LDAP://RootDSE").schemaNamingContext)).rangeUpper).Value) } until ($dCheckExchgSchemaVer -ge "15317")
		Write-Host "`t  The Active Directory Schema has been extended successfully" -ForegroundColor Green
	}
}


####################################################################################################################################################################################
Function Exchange2016PrepareAD()         # Function to Prepare the Active Directory Forest for the Microsoft Exchange 2016 Mailbox Server Role
{
	if ( (((([ADSI]("LDAP://CN=ms-Exch-Schema-Version-Pt," + ([ADSI]"LDAP://RootDSE").schemaNamingContext)).rangeUpper).Value) -ge "15317") -and (((([ADSI]("LDAP://CN=Services,CN=Configuration,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)")).psbase.children) | where {$_.objectClass -eq 'MSExchConfigurationContainer'}).Path -eq $null) -and (!(Test-Path "C:\Program Files\Microsoft\Exchange Server\V15")) ) {
		if (((([ADSI]("LDAP://CN=Microsoft Exchange System Objects,$(([ADSI]"LDAP://RootDSE").defaultNamingContext)")).objectVersion).Value) -lt "13236") {
			# Prepare Active Directory for Exchange 2016 installation
			Write-Host "`t Preparing Active Directory for Microsoft Exchange" -ForegroundColor Green
			((Start-Process -FilePath "$dExchangeInstal\Setup.exe" -Argumentlist "/PrepareAD /OrganizationName:`"$dExchangeOrgNme`" /IAcceptExchangeServerLicenseTerms" -Wait -Passthru).ExitCode) | Out-Null
			# Check to make sure Active Directory has been prepared for Microsoft Exchange
			$dElapsedExchgPrepareAD = [System.Diagnostics.Stopwatch]::StartNew() # Start a timer as the check below does not complete on some systems
			do { $dCheckExchgPrepareAD = (((([ADSI]("LDAP://CN=Services,CN=Configuration,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)")).psbase.children) | where {$_.objectClass -eq 'MSExchConfigurationContainer'}).Path) }
			until (($dCheckExchgPrepareAD -ne $null) -or ($dElapsedExchgPrepareAD.Elapsed.TotalMinutes.ToString() -ge "6"))
			$dElapsedExchgPrepareAD = "" # Clear the timer as the task has completed
			Write-Host "`t  Active Directory has been prepared for Microsoft Exchange successfully" -ForegroundColor Green
		}
		if (((([ADSI]("LDAP://CN=Microsoft Exchange System Objects,$(([ADSI]"LDAP://RootDSE").defaultNamingContext)")).objectVersion).Value) -ge "13236") {
			# Prepare Active Directory Domains for Exchange 2016 installation
			Write-Host "`t Preparing the Active Directory Domains" -ForegroundColor Green
			((Start-Process -FilePath "$dExchangeInstal\Setup.exe" -Argumentlist "/PrepareAllDomains /IAcceptExchangeServerLicenseTerms" -Wait -Passthru).ExitCode) | Out-Null
			Write-Host "`t  The Active Directory Domains have been prepared for Microsoft Exchange successfully" -ForegroundColor Green
		}
	}
}


####################################################################################################################################################################################
Function Exchange2016InstallMailbox()    # Function to Prepare Active Directory Domains for the Microsoft Exchange 2016 Mailbox Server Role
{
	if (((([ADSI]("LDAP://CN=ms-Exch-Schema-Version-Pt," + ([ADSI]"LDAP://RootDSE").schemaNamingContext)).rangeUpper).Value) -ge "15317") {
		if (((([ADSI]("LDAP://CN=Microsoft Exchange System Objects,$(([ADSI]"LDAP://RootDSE").defaultNamingContext)")).objectVersion).Value) -ge "13236") {
			if (((([ADSI]("LDAP://CN=Services,CN=Configuration,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)")).psbase.children) | where {$_.objectClass -eq 'MSExchConfigurationContainer'}).Path -ne $null) {
				if ((([ADSI]("LDAP://CN=Microsoft Exchange,CN=Services,CN=Configuration,$(([ADSI]"LDAP://RootDSE").defaultNamingContext)")).psbase.children[0].objectVersion) -ge "16211") {
					if (!(Test-Path "C:\Program Files\Microsoft\Exchange Server\V15")) {
						# Install the Exchange 2016 Mailbox server on this machine
						Write-Host "`t Installing the Exchange Mailbox Server Role" -ForegroundColor Green
						Write-Host "`t    ****************************************" -ForegroundColor Green
						Write-Host "`t    *                                      *" -ForegroundColor Green
						Write-Host "`t    *     This will take quite a while     *" -ForegroundColor Green
						Write-Host "`t    *           to fully complete          *" -ForegroundColor Green
						Write-Host "`t    *                                      *" -ForegroundColor Green
						Write-Host "`t    *  Please be patient while we install  *" -ForegroundColor Green
						Write-Host "`t    *      Exchange 2016 Mailbox Role      *" -ForegroundColor Green
						Write-Host "`t    *                                      *" -ForegroundColor Green
						Write-Host "`t    ****************************************" -ForegroundColor Green
						((Start-Process -FilePath "$dExchangeInstal\Setup.exe" -Argumentlist "/mode:Install /role:Mailbox /MdbName:`"$dExchangeDatabaseName`" /IAcceptExchangeServerLicenseTerms" -Wait -Passthru).ExitCode) | Out-Null
						# Install the Exchange 2016 Management Tools on this machine
						Write-Host "`t Installing the Exchange Management Tools" -ForegroundColor Green
						((Start-Process -FilePath "$dExchangeInstal\Setup.exe" -Argumentlist "/Role:ManagementTools /IAcceptExchangeServerLicenseTerms" -Wait -Passthru).ExitCode) | Out-Null
					}
				}
			}
		}
	}
}


####################################################################################################################################################################################
Function Exchange2016InstallTools()     # Function to install the Microsoft Exchange 2016 Management Tools ONLY
{
	if (((([ADSI]("LDAP://CN=ms-Exch-Schema-Version-Pt," + ([ADSI]"LDAP://RootDSE").schemaNamingContext)).rangeUpper).Value) -ge "15317") {
		if (((([ADSI]("LDAP://CN=Microsoft Exchange System Objects,$(([ADSI]"LDAP://RootDSE").defaultNamingContext)")).objectVersion).Value) -ge "13236") {
			if (((([ADSI]("LDAP://CN=Services,CN=Configuration,$(([ADSI]"LDAP://RootDSE").rootDomainNamingContext)")).psbase.children) | where {$_.objectClass -eq 'MSExchConfigurationContainer'}).Path -ne $null) {
				if ((([ADSI]("LDAP://CN=Microsoft Exchange,CN=Services,CN=Configuration,$(([ADSI]"LDAP://RootDSE").defaultNamingContext)")).psbase.children[0].objectVersion) -ge "16211") {
					if (!(Test-Path "C:\Program Files\Microsoft\Exchange Server\V15")) {
						# Install the Legacy Web Management Console
						(Install-WindowsFeature Web-Lgcy-Mgmt-Console) | Out-Null
						# Install the Exchange 2016 Tools on this machine
						Write-Host "`t Installing the Exchange Management Tools" -ForegroundColor Green
						((Start-Process -FilePath "$dExchangeInstal\Setup.exe" -Argumentlist "/Role:ManagementTools /IAcceptExchangeServerLicenseTerms" -Wait -Passthru).ExitCode) | Out-Null
					}
				}
			}
		}
	}
}


####################################################################################################################################################################################
Function Exchange2016ImportExportSvr()  # Function to setup the Microsoft Exchange 2016 Import Export PST Server
{
	# Check if there are already any Exchange 2016 Servers on the domain
	if (((([ADSI]("LDAP://CN=ms-Exch-Schema-Version-Pt," + ([ADSI]"LDAP://RootDSE").schemaNamingContext)).rangeUpper).Value) -ge "15317") {
		# Import the Active Directory PowerShell Module
		(Import-Module ActiveDirectory) | Out-Null
		# Load the Microsoft Exchange PowerShell SnapIn
		add-pssnapin *exchange* -erroraction SilentlyContinue
		# Create a new Active Directory Group if it doesn't already exist
		if (!([bool](Get-ADGroup -Filter {name -eq "Exchange Mailbox Import Export"}))) {
			Write-Host "`t Creating the `"Exchange Mailbox Import Export`" Security Group" -ForegroundColor Green
			New-ADGroup -Name "Exchange Mailbox Import Export" -GroupScope "Universal" -GroupCategory "Security" -Path "OU=Microsoft Exchange Security Groups,$(([ADSI]"LDAP://RootDSE").defaultNamingContext)" -Description "This group will provide access to mailbox import and export cmdlets within entire Exchange Organization."
		}
		# Create a new Active Directory User which will be used for the Exchange PST Import and Export if it doesn't already exist
		if (!([bool](Get-ADUser -Filter {SamAccountName -eq "$dExchangeImportLogonName"}))) {
			$dExchangeImportPassword = Read-Host "Enter a Password for the `"$dExchangeImportLogonName`" user" -AsSecureString
			Write-Host "`t Creating the `"$dExchangeImportLogonName`" User Account" -ForegroundColor Green
			New-ADUser -Name "$dExchangeImportUserFirst $dExchangeImportUserLast" -SamAccountName "$dExchangeImportLogonName" -DisplayName "$dExchangeImportUserFirst $dExchangeImportUserLast" -AccountPassword $dExchangeImportPassword -Enabled $true  -Description "Account to Import and Export PST Files to Exchange" -CannotChangePassword $true -PasswordNeverExpires $true -GivenName "$dExchangeImportUserFirst" -Surname "$dExchangeImportUserLast" -UserPrincipalName "$dExchangeImportLogonName@$dSCPdomainName"
		}
		# Add the user to the "Exchange Mailbox Import Export" group unless it is already a member
		if (!((Get-ADGroupMember -identity "Exchange Mailbox Import Export" -Recursive).SamAccountName -match "$dExchangeImportLogonName")) {
			Write-Host "`t Adding the user to the `"Exchange Mailbox Import Export`" Security Group" -ForegroundColor Green
			Add-ADGroupMember -identity "Exchange Mailbox Import Export" -Members "$dExchangeImportLogonName"
		}
		# Add the user to the "Organization Management" group unless it is already a member
		if (!((Get-ADGroupMember -identity "Organization Management" -Recursive).SamAccountName -match "$dExchangeImportLogonName")) {
			Write-Host "`t Adding the user to the `"Organization Management`" Security Group" -ForegroundColor Green
			Add-ADGroupMember -identity "Organization Management" -Members "$dExchangeImportLogonName"
		}
		# Create the _pstImports folder and shares it
		if (!(Test-Path "$dExchangeImportFolderNme")) {
			Write-Host "`t Creating the `"$dExchangeImportFolderNme`" folder and sharing it" -ForegroundColor Green
			(New-Item -ItemType Directory -Path "$dExchangeImportFolderNme") | Out-Null
			# Set the permissions on the "_pstImports" directory for full access on the "Exchange Trusted Subsystem" security Group
			$acl = Get-Acl -Path "$dExchangeImportFolderNme"
			$acl.SetAccessRule($(New-Object -TypeName System.Security.AccessControl.FileSystemAccessRule -ArgumentList "Exchange Trusted Subsystem", 'FullControl', 'ContainerInherit, ObjectInherit', 'None', 'Allow'))
			$acl | Set-Acl -Path "$dExchangeImportFolderNme"
			(New-SMBShare -Name "$(($dExchangeImportFolderNme -split('\\'))[-1])" -Path "$dExchangeImportFolderNme" -FullAccess "Exchange Trusted Subsystem") | Out-Null
		}
		# Create the _pstExports folder and shares it
		if (!(Test-Path "$dExchangeExportFolderNme")) {
			Write-Host "`t Creating the `"$dExchangeExportFolderNme`" folder and sharing it" -ForegroundColor Green
			(New-Item -ItemType Directory -Path "$dExchangeExportFolderNme") | Out-Null
			# Set the permissions on the "_pstImports" directory for full access on the "Exchange Trusted Subsystem" security Group
			$acl = Get-Acl -Path "$dExchangeExportFolderNme"
			$acl.SetAccessRule($(New-Object -TypeName System.Security.AccessControl.FileSystemAccessRule -ArgumentList "Exchange Trusted Subsystem", 'FullControl', 'ContainerInherit, ObjectInherit', 'None', 'Allow'))
			$acl | Set-Acl -Path "$dExchangeExportFolderNme"
			(New-SMBShare -Name "$(($dExchangeExportFolderNme -split('\\'))[-1])" -Path "$dExchangeExportFolderNme" -FullAccess "Exchange Trusted Subsystem") | Out-Null
		}
		# Assign the "Exchange Mailbox Import Export" Security Group permissions to be able to import and export PST Files into exchange unless it already has permission
		if (!([bool]((Get-ManagementRoleAssignment -role "Mailbox Import Export").RoleAssigneeName -match "Exchange Mailbox Import Export"))) {
			Write-Host "`t Adding permissions to the `"Exchange Mailbox Import Export`" Security Group" -ForegroundColor Green
			(New-ManagementRoleAssignment -Role "Mailbox Import Export" -SecurityGroup "$dSCPdomainName\Exchange Mailbox Import Export") | Out-Null
		}
	}
}


####################################################################################################################################################################################
Function ExchangeMBX2016()              # Function to install the features required for the Microsoft Exchange 2016 Mailbox Server Role
{
	# Run the Pre-Requisites Function
	Exchange2016PreReq
	# Install the .NET Framework v4.5.2
	InstallDotNET452
	# Install the Unified Communications Managed API 4.0 Runtime
	InstallExchangeUCMA4
	# Check to see if the Exchange Installation options have been set
	Exchange2016_PreCheck
	# Extend the Active Directory Schema for Exchange 2016 installation 
	Exchange2016PrepareSchema
	# Prepare Active Directory for Microsoft Exchange Server
	Exchange2016PrepareAD
	# Install the Exchange Server 2016 Mailbox Role and Tools
	Exchange2016InstallMailbox
}


####################################################################################################################################################################################
Function Exchange2016_PostCheck()  # Function to check Exchange 2016 Installation after installation
{
	# Check if there are already any Exchange 2016 Servers on the domain
	if (((([ADSI]("LDAP://CN=ms-Exch-Schema-Version-Pt," + ([ADSI]"LDAP://RootDSE").schemaNamingContext)).rangeUpper).Value) -ge "15317") {
		# Load the Microsoft Exchange PowerShell SnapIn
		add-pssnapin *exchange* -erroraction SilentlyContinue
		$dInstalledDBname = ((Get-MailboxDatabase -Server $env:COMPUTERNAME).name)
		Write-Host "`n`t Congratulations Microsoft Exchange has been successfully configured on this server" -ForegroundColor Green
		if ($dExchangeDatabaseName -eq  $dInstalledDBname){
			Write-Host "`t The Database Name on this server is `"$dExchangeDatabaseName`"" -ForegroundColor Green
		}else{
			Write-Host "`t The Database Name you entered was `"$dExchangeDatabaseName`"" -ForegroundColor Yellow
			Write-Host "`t The Database Name Installed on this server is `"$dInstalledDBname`"" -ForegroundColor Yellow
		}
		# Check if the Default Public Folder Database is created
		if ( !((Get-Mailbox -PublicFolder).Name) ) {
			# Ask the user if they want to install a Public Folder Database on this server
			$choice = ""
			Write-Host "`n`t You do not have any Public Folder Databases installed" -ForegroundColor Yellow
			Write-Host "`t It is a requirement of SolidCP that at least 1 Public Folder Database is configured" -ForegroundColor Yellow
			while ($choice -notmatch "[y|n]") {
				$choice = Read-Host "`n`t Do you want to install a Public Folder Database on this server`? (Y/N)"
			}
			if ($choice -eq "y") {
				$dPublicFolderDatabase = Read-Host "`n`t Please enter the name for the Public Folder Database"
				if (!($dPublicFolderDatabase)) {$dPublicFolderDatabase = "SolidCP-PF-Mailbox"}
				Write-Host "`t The Public Folder Database will be called `"$dPublicFolderDatabase`"" -ForegroundColor Green
				# Check if the Exchange Organisation Name is set, if not then get it
				if ( !($dExchangeOrgNme) ) { $dExchangeOrgNme = (([ADSI]("LDAP://CN=Microsoft Exchange,CN=Services," + (Get-ADRootDSE).configurationNamingContext)).Children[0].name) }
				# Reset the Default Public Folder Database Primary Hierarchy if it has not been deleted corectly
				$dDNquery = ("CN=$dExchangeOrgNme,CN=Microsoft Exchange,CN=Services," + (Get-ADRootDSE).configurationNamingContext) 
				if ( (([ADSI]("LDAP://$dDNquery")).msExchDefaultPublicFolderMailbox) -split ";" -contains "00000000-0000-0000-0000-000000000000") {
					$orgName = ([ADSI]("LDAP://$dDNquery"))
					$orgName.PutEx(1,"msExchDefaultPublicFolderMailbox",0)
					$orgName.SetInfo()
				}
				# Create the Default Public Folder Database for Primary Hierarchy
				(New-Mailbox -Name "$dPublicFolderDatabase" –PublicFolder) | Out-Null
			} else {
				write-host "`n`t You MUST install a Public Folder Database for SolidCP to function correctly" -ForegroundColor Yellow
			}
		}
		if ( (Test-Path "C:\Program Files (x86)\SolidCP Installer") -and  (Test-Path "C:\Program Files\Microsoft\Exchange Server\V15") ) {
			# Add the SCPServer User to the Domain Groups called "Enterprise Admins" and "Organization Management"
			(AddDomainUserToDomainGroup "$dLangDomainEnterpriseAdmins" "SCPServer-$env:computerName") | Out-Null    # Add the "SCPServer-[ComputerName]" User to the Domain Group called "Enterprise Admins"
			(AddDomainUserToDomainGroup "Organization Management" "SCPServer-$env:computerName") | Out-Null         # Add the "SCPServer-[ComputerName]" User to the Domain Group called "Organization Management"
		}
		# Check to make sure the default options for SolidCP are configured corectly
		Exchange2016_UpgradeCheck
	}
}


####################################################################################################################################################################################
Function Exchange2016_Set_OOA()       # Function to configure the Outlook Anywhere URL's for Exchange 2016
{
	Param(
		[switch]$All       # Set the Outlook AnyWhere URLs on Al Servers in the Exchange Organisation
	)
	# Check if there are already any Exchange 2016 Servers on the domain
	if ( (((([ADSI]("LDAP://CN=ms-Exch-Schema-Version-Pt," + ([ADSI]"LDAP://RootDSE").schemaNamingContext)).rangeUpper).Value) -ge "15317") -and (Test-Path "C:\Program Files\Microsoft\Exchange Server\V15") ) {
		# Import the Active Directory PowerShell Module
		(Import-Module ActiveDirectory) | Out-Null
		# Load the Microsoft Exchange PowerShell SnapIn
		add-pssnapin *exchange* -erroraction SilentlyContinue
		if ($All) {
			do { $ExchgSvrList = ((Get-ExchangeServer | where {$_.IsClientAccessServer} ).Name) } until ($ExchgSvrList)
		}else{
			$ExchgSvrList = $env:COMPUTERNAME
		}
		if (!($dOutlookAnywhFQDN)) {
			do { $dOutlookAnywhFQDN = Read-Host "`tPlease enter the required FQDN for Outlook Anywhere" }
			until (!([string]::IsNullOrEmpty($dOutlookAnywhFQDN)))
		}
		if (!($dExchangeAutoDisc)) {
			$dExchangeAutoDisc = $dOutlookAnywhFQDN
		}
		if (Resolve-DnsName -Name "$dOutlookAnywhFQDN" -ErrorAction SilentlyContinue) {
			foreach ($Server in $ExchgSvrList) {
				if ((Get-ExchangeServer -Identity $Server -ErrorAction SilentlyContinue).IsClientAccessServer) {
					Write-Host "`t Configuring the Exchange URLs on `"$Server`"" -ForegroundColor Green
					(Get-OutlookAnywhere             -Server $Server | Set-OutlookAnywhere -ExternalHostname $dOutlookAnywhFQDN -InternalHostname $dOutlookAnywhFQDN -ExternalClientsRequireSsl $true -InternalClientsRequireSsl $true -DefaultAuthenticationMethod "NTLM" -SSLOffloading $false) |Out-Null
					(Get-OwaVirtualDirectory         -Server $Server | Set-OwaVirtualDirectory         -ExternalUrl "https://$dOutlookAnywhFQDN/owa"                         -InternalUrl "https://$dOutlookAnywhFQDN/owa" -LogonFormat "PrincipalName" -DefaultClientLanguage "$((Get-Culture).LCID)" -LogonAndErrorLanguage "$((Get-Culture).LCID)" -WarningAction SilentlyContinue) |Out-Null
					(Get-EcpVirtualDirectory         -Server $Server | Set-EcpVirtualDirectory         -ExternalUrl "https://$dOutlookAnywhFQDN/ecp"                         -InternalUrl "https://$dOutlookAnywhFQDN/ecp" -WarningAction SilentlyContinue) |Out-Null
					(Get-ActiveSyncVirtualDirectory  -Server $Server | Set-ActiveSyncVirtualDirectory  -ExternalUrl "https://$dOutlookAnywhFQDN/Microsoft-Server-ActiveSync" -InternalUrl "https://$dOutlookAnywhFQDN/Microsoft-Server-ActiveSync") |Out-Null
					(Get-WebServicesVirtualDirectory -Server $Server | Set-WebServicesVirtualDirectory -ExternalUrl "https://$dOutlookAnywhFQDN/EWS/Exchange.asmx"           -InternalUrl "https://$dOutlookAnywhFQDN/EWS/Exchange.asmx") |Out-Null
					(Get-OabVirtualDirectory         -Server $Server | Set-OabVirtualDirectory         -ExternalUrl "https://$dOutlookAnywhFQDN/OAB"                         -InternalUrl "https://$dOutlookAnywhFQDN/OAB" -WarningAction SilentlyContinue) |Out-Null
					(Get-MapiVirtualDirectory        -Server $Server | Set-MapiVirtualDirectory        -ExternalUrl "https://$dOutlookAnywhFQDN/mapi"                        -InternalUrl "https://$dOutlookAnywhFQDN/mapi" -IISAuthenticationMethods Negotiate) |Out-Null
					(Get-ClientAccessService       -Identity $Server | Set-ClientAccessService -AutoDiscoverServiceInternalUri "https://$dExchangeAutoDisc/Autodiscover/Autodiscover.xml") |Out-Null
					(Get-PowerShellVirtualDirectory  -Server $Server | Set-PowerShellVirtualDirectory  -ExternalUrl "https://$dOutlookAnywhFQDN/PowerShell"                  -InternalUrl "https://$dOutlookAnywhFQDN/PowerShell" -WindowsAuthentication $true) |Out-Null
					if (!((Get-OrganizationConfig).MapiHttpEnabled)) {(Set-OrganizationConfig -MapiHttpEnabled $true)}
				}
			}
			if ($dSCPdomainName) {
				while ($dWildcardSSLchoice -notmatch "[y|n]") {
					$dWildcardSSLchoice = Read-Host "`n`t Will you be using a Wildcard SSL Certificate for Exchange`? (Y/N)"
				}
				if ($dWildcardSSLchoice -eq "y") {
					(Set-OutlookProvider EXCH -CertPrincipalName "msstd:*.$dSCPdomainName" -WarningAction SilentlyContinue) | Out-Null
					(set-POPSettings -X509CertificateName "$dOutlookAnywhFQDN" -WarningAction SilentlyContinue) | Out-Null
					(set-IMAPSettings -X509CertificateName "$dOutlookAnywhFQDN" -WarningAction SilentlyContinue) | Out-Null
					(Restart-Service 'MSExchangePop3*' -WarningAction SilentlyContinue) | Out-Null
					(Restart-Service 'MSExchangeImap4*' -WarningAction SilentlyContinue) | Out-Null
				}
			}
		}
	}
}


####################################################################################################################################################################################
Function Exchange2016_Set_SSLredir()           # Function to configure the SSL Redirect URL for Exchange 2016
{
	# Check if there are already any Exchange 2016 Servers on the domain
	if ( (((([ADSI]("LDAP://CN=ms-Exch-Schema-Version-Pt," + ([ADSI]"LDAP://RootDSE").schemaNamingContext)).rangeUpper).Value) -ge "15317") -and (Test-Path "C:\Program Files\Microsoft\Exchange Server\V15") ) {
		# Import the Active Directory PowerShell Module
		(Import-Module ActiveDirectory) | Out-Null
		# Load the Microsoft Exchange PowerShell SnapIn
		add-pssnapin *exchange* -erroraction SilentlyContinue
		if (!($dOutlookAnywhFQDN)) {
			do { $dOutlookAnywhFQDN = Read-Host "`tPlease enter the required FQDN for Outlook Anywhere" }
			until (!([string]::IsNullOrEmpty($dOutlookAnywhFQDN)))
		}
		# Import the PowerShell Web Administration Module
		Import-Module WebAdministration
		# Set the SSL settings for the Default Website and Virtual Directories
		Write-Host "`t Configuring SSL Settings for Microsoft Exchange on this server" -ForegroundColor Green
		Set-WebConfigurationProperty -pspath ‘MACHINE/WEBROOT/APPHOST’ -filter “system.webServer/security/access” -location ‘Default Web Site’ -name “sslFlags” -value “$null”
		Set-WebConfigurationProperty -pspath ‘MACHINE/WEBROOT/APPHOST’ -filter “system.webServer/security/access” -location ‘Default Web Site/Autodiscover’ -name “sslFlags” -value “Ssl,Ssl128”
		Set-WebConfigurationProperty -pspath ‘MACHINE/WEBROOT/APPHOST’ -filter “system.webServer/security/access” -location ‘Default Web Site/ecp’ -name “sslFlags” -value “Ssl,Ssl128”
		Set-WebConfigurationProperty -pspath ‘MACHINE/WEBROOT/APPHOST’ -filter “system.webServer/security/access” -location ‘Default Web Site/EWS’ -name “sslFlags” -value “Ssl,Ssl128”
		Set-WebConfigurationProperty -pspath ‘MACHINE/WEBROOT/APPHOST’ -filter “system.webServer/security/access” -location ‘Default Web Site/mapi’ -name “sslFlags” -value “Ssl,Ssl128”
		Set-WebConfigurationProperty -pspath ‘MACHINE/WEBROOT/APPHOST’ -filter “system.webServer/security/access” -location ‘Default Web Site/Microsoft-Server-ActiveSync’ -name “sslFlags” -value “Ssl,Ssl128”
		Set-WebConfigurationProperty -pspath ‘MACHINE/WEBROOT/APPHOST’ -filter “system.webServer/security/access” -location ‘Default Web Site/OAB’ -name “sslFlags” -value “Ssl,Ssl128”
		Set-WebConfigurationProperty -pspath ‘MACHINE/WEBROOT/APPHOST’ -filter “system.webServer/security/access” -location ‘Default Web Site/owa’ -name “sslFlags” -value “Ssl,Ssl128”
		Set-WebConfigurationProperty -pspath ‘MACHINE/WEBROOT/APPHOST’ -filter “system.webServer/security/access” -location ‘Default Web Site/PowerShell’ -name “sslFlags” -value “SslNegotiateCert”
		Set-WebConfigurationProperty -pspath ‘MACHINE/WEBROOT/APPHOST’ -filter “system.webServer/security/access” -location ‘Default Web Site/Rpc’ -name “sslFlags” -value “Ssl,Ssl128”
		# Set the HTTP Redirect on the Default Website and remove it on all Virtual Directories
		Write-Host "`t Configuring HTTP to HTTPS Redirection for Microsoft Exchange on this server" -ForegroundColor Green
		Set-WebConfiguration -Filter "system.webServer/httpRedirect" "IIS:\sites\Default Web Site" -Value @{enabled="true";destination="https://$dOutlookAnywhFQDN/owa";exactDestination="false";childOnly="true";httpResponseStatus="Found"}
		Set-WebConfiguration -Filter "system.webServer/httpRedirect" "IIS:\sites\Default Web Site/Autodiscover" -Value @{destination="$null";enabled="false";exactDestination="false";childOnly="true";httpResponseStatus="Found"}
		Set-WebConfiguration -Filter "system.webServer/httpRedirect" "IIS:\sites\Default Web Site/ecp" -Value @{destination="$null";enabled="false";exactDestination="false";childOnly="true";httpResponseStatus="Found"}
		Set-WebConfiguration -Filter "system.webServer/httpRedirect" "IIS:\sites\Default Web Site/EWS" -Value @{destination="$null";enabled="false";exactDestination="false";childOnly="true";httpResponseStatus="Found"}
		Set-WebConfiguration -Filter "system.webServer/httpRedirect" "IIS:\sites\Default Web Site/mapi" -Value @{destination="$null";enabled="false";exactDestination="false";childOnly="true";httpResponseStatus="Found"}
		Set-WebConfiguration -Filter "system.webServer/httpRedirect" "IIS:\sites\Default Web Site/Microsoft-Server-ActiveSync" -Value @{destination="$null";enabled="false";exactDestination="false";childOnly="true";httpResponseStatus="Found"}
		Set-WebConfiguration -Filter "system.webServer/httpRedirect" "IIS:\sites\Default Web Site/OAB" -Value @{destination="$null";enabled="false";exactDestination="false";childOnly="true";httpResponseStatus="Found"}
		Set-WebConfiguration -Filter "system.webServer/httpRedirect" "IIS:\sites\Default Web Site/owa" -Value @{destination="$null";enabled="false";exactDestination="false";childOnly="true";httpResponseStatus="Found"}
		Set-WebConfiguration -Filter "system.webServer/httpRedirect" "IIS:\sites\Default Web Site/PowerShell" -Value @{destination="$null";enabled="false";exactDestination="false";childOnly="true";httpResponseStatus="Found"}
		Set-WebConfiguration -Filter "system.webServer/httpRedirect" "IIS:\sites\Default Web Site/Rpc" -Value @{destination="$null";enabled="false";exactDestination="false";childOnly="true";httpResponseStatus="Found"}
		# Set a Scheduled Task to run on startup to ensure the HTTP to HTTPS Redirection works after a reboot
		CreateIISHTTPtoHTTPSScheduledTask
	}
}


####################################################################################################################################################################################
Function CreateExch2016_HttpToHttps403()       # Function to create the HTTP to HTTPS Redirection for Exchange using the Error 403.4 Error Page in IIS
{
	# Check if there are already any Exchange 2016 Servers on the domain
	if ( (((([ADSI]("LDAP://CN=ms-Exch-Schema-Version-Pt," + ([ADSI]"LDAP://RootDSE").schemaNamingContext)).rangeUpper).Value) -ge "15317") -and (Test-Path "C:\Program Files\Microsoft\Exchange Server\V15") ) {
		# Specify the Exchange web.config file for the Default Web Site
		$dExchangeServerWebConfig = "C:\inetpub\wwwroot\web.config"
		# Check if the web.config file exists
		if (Test-Path "$dExchangeServerWebConfig ") {
			if (!($dOutlookAnywhFQDN)) {
				do { $dOutlookAnywhFQDN = Read-Host "`tPlease enter the required FQDN for Outlook Anywhere" }
				until (!([string]::IsNullOrEmpty($dOutlookAnywhFQDN)))
			}
			Write-Host "`t Applying the HTTPS 403.4 redirect" -ForegroundColor Green
			ModifyXML "$dExchangeServerWebConfig" "Comment" "//configuration/location/system.webServer/modules/remove[@name='CustomErrorModule']"
			ModifyXML "$dExchangeServerWebConfig" "Add" "//configuration" "system.webServer"
			ModifyXML "$dExchangeServerWebConfig" "Add" "//configuration/system.webServer" "httpErrors"
			ModifyXML "$dExchangeServerWebConfig" "Add" "//configuration/system.webServer/httpErrors" "error" @( ("statusCode","403"), ("subStatusCode","4"), ("path","https://$dOutlookAnywhFQDN/owa/"), ("responseMode","Redirect") )
			CreateIISHTTPtoHTTPSScheduledTask
		}
	}
}


####################################################################################################################################################################################
Function CreateIISHTTPtoHTTPSScheduledTask()   # Function to create a Scheduled Task to configure the HTTP to HTTPS Redirection on Exchange Servers
{
	$dExchangeServerWebConfig = "C:\inetpub\wwwroot\web.config"
	# Check if there are already any Exchange 2016 Servers on the domain
	if (((([ADSI]("LDAP://CN=ms-Exch-Schema-Version-Pt," + ([ADSI]"LDAP://RootDSE").schemaNamingContext)).rangeUpper).Value) -ge "15317") {
		# Check if Exchange is installed on this machine
		if (Test-Path "C:\Program Files\Microsoft\Exchange Server\V15") {
			# Check if the web.config file exists
			if (Test-Path "$dExchangeServerWebConfig ") {
				$dSCPiisHTTPSAutoScript = @'
<####################################################################################################
SolidSCP - Exchange HTTP to HTTPS Redirection

v1.0    31st October 2016:   First release of the SolidCP Exchange HTTP to HTTPS Redirection Script

Written By Marc Banyard for the SolidCP Project (c) 2016 SolidCP

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
CLS
####################################################################################################################################################################################
function CheckXMLnode ($dFilePath, $dXMLNode, $dXMLname, $dXMLvalue)
{
	[xml]$xml = Get-Content -Path "$dFilePath"
	if ([string]::IsNullOrEmpty($dXMLvalue)) {
		if ($xml.selectNodes("$dXMLNode").$dXMLname) { $true } else { $false }
	}else{
		if ($xml.selectNodes("$dXMLNode").$dXMLname -contains "$dXMLvalue") { $true } else { $false }
	}
}
####################################################################################################################################################################################
function ModifyXML([String] $dFilePath, $dAction, [String] $dNodePath, $dElement, $dValue)
{
	[xml]$xml = Get-Content -Path "$dFilePath"
	if ($dAction -eq "Add") {
		$Child = $xml.CreateElement("$dElement")
		if ($dValue -is [System.Array]) {
			if ($dValue[0] -is [System.Array]) {
				$dCheckNode = "$dNodePath/$dElement"
				if ( !(CheckXMLnode "$dFilePath" "$dCheckNode" $dValue[0][0] $dValue[0][1]) ) {
					foreach($value in $dValue) {
						$Child.SetAttribute($value[0],$value[1])
						$xml.selectNodes("$dNodePath").AppendChild($Child) | Out-Null
					}
				}
			}else{
				$dCheckNode = "$dNodePath/$dElement"
				if ( !(CheckXMLnode "$dFilePath" "$dCheckNode" $dValue[0] $dValue[1]) ) {
					$Child.SetAttribute($dValue[0],$dValue[1])
					$xml.selectNodes("$dNodePath").AppendChild($Child) | Out-Null
				}
			}
		}
		elseif ($dValue -isnot [System.Array]) {
			if ( !(CheckXMLnode "$dFilePath" "$dNodePath" "$dElement" "$dValue") ) {
				$Child.InnerText = "$dValue"
				$xml.selectNodes("$dNodePath").AppendChild($Child) | Out-Null
			}
		}
		$xml.Save("$dFilePath") | Out-Null
	}elseif ($dAction -eq "Comment") {
		$xml.SelectNodes("$dNodePath") | ForEach-Object {
			$Comment = $xml.CreateComment($_.OuterXml);
			$_.ParentNode.ReplaceChild($Comment, $_) | Out-Null
		}
		$xml.Save("$dFilePath");
	}
}

ModifyXML "_SolidCP_Exchange_Web_Config_File_" "Comment" "//configuration/location/system.webServer/modules/remove[@name='CustomErrorModule']"
ModifyXML "_SolidCP_Exchange_Web_Config_File_" "Add" "//configuration" "system.webServer"
ModifyXML "_SolidCP_Exchange_Web_Config_File_" "Add" "//configuration/system.webServer" "httpErrors"
ModifyXML "_SolidCP_Exchange_Web_Config_File_" "Add" "//configuration/system.webServer/httpErrors" "error" @( ("statusCode","403"), ("subStatusCode","4"), ("path","https://_SolidCP_Exchange_OWA_URL_/owa/"), ("responseMode","Redirect") )

'@

				if (!($dDomainMember)) { # Check to see if the machine is NOT joined to a domain
					if (CheckGroupMembers "$dLangAdministratorGroup" "$dLoggedInUserName" "Local") { # Logged in user is a Local Administrator
						$dIISredirectTaskUser = $dLocalAdministratorSID
					}
				}elseif ( ($dDomainMember) -and (!($dLoggedInLocally)) ) {
					if (CheckGroupMembers "$dLangDomainAdminsGroup" "$dLoggedInUserName" "Domain") { # Logged in user is a Domain Administrator
						$dIISredirectTaskUser = $dDomainAdministratorSID
					}
				}
				# Check if the SolidCP directory exists, if not then create it
				if (!(Test-Path "C:\SolidCP")) { (md -Path "C:\SolidCP" -Force) | Out-Null }
				# Create the IIS SSL Hardening PowerShell Script on the Server so we can run it as a Scheduled Task
				( (($dSCPiisHTTPSAutoScript -replace "_SolidCP_Exchange_Web_Config_File_", $dExchangeServerWebConfig) -replace "_SolidCP_Exchange_OWA_URL_", $dOutlookAnywhFQDN) | Out-File -Encoding "ascii" -FilePath "C:\SolidCP\Exchange_HTTP_to_HTTPS_Redirection.ps1" ) | Out-Null
				# Create the IIS SSL Hardening Scheduled Task as XML Format so we can import it
				(New-Item "C:\SolidCP\Exchange_HTTP_Redirect-Scheduled-Task-Import.xml" -type file -force -value "<?xml version=`"1.0`" encoding=`"UTF-16`"?>`r`n<Task version=`"1.2`" xmlns=`"http://schemas.microsoft.com/windows/2004/02/mit/task`">`r`n  <RegistrationInfo>`r`n    <Date>2015-01-01T00:00:00.000000</Date>`r`n    <Author>SolidCP</Author>`r`n    <Description>PowerShell file that runs on Startup to fix the Microsoft Exchange HTTP to HTTPS settings in IIS.</Description>`r`n  </RegistrationInfo>`r`n  <Triggers>`r`n    <BootTrigger>`r`n      <Enabled>true</Enabled>`r`n    </BootTrigger>`r`n  </Triggers>`r`n  <Principals>`r`n    <Principal id=`"Author`">`r`n      <RunLevel>HighestAvailable</RunLevel>`r`n      <UserId>$dIISredirectTaskUser</UserId>`r`n      <LogonType>S4U</LogonType>`r`n    </Principal>`r`n  </Principals>`r`n  <Settings>`r`n    <MultipleInstancesPolicy>IgnoreNew</MultipleInstancesPolicy>`r`n    <DisallowStartIfOnBatteries>false</DisallowStartIfOnBatteries>`r`n    <StopIfGoingOnBatteries>false</StopIfGoingOnBatteries>`r`n    <AllowHardTerminate>true</AllowHardTerminate>`r`n    <StartWhenAvailable>true</StartWhenAvailable>`r`n    <RunOnlyIfNetworkAvailable>false</RunOnlyIfNetworkAvailable>`r`n    <IdleSettings>`r`n      <StopOnIdleEnd>true</StopOnIdleEnd>`r`n      <RestartOnIdle>false</RestartOnIdle>`r`n    </IdleSettings>`r`n    <AllowStartOnDemand>true</AllowStartOnDemand>`r`n    <Enabled>true</Enabled>`r`n    <Hidden>false</Hidden>`r`n    <RunOnlyIfIdle>false</RunOnlyIfIdle>`r`n    <WakeToRun>false</WakeToRun>`r`n    <ExecutionTimeLimit>PT12H</ExecutionTimeLimit>`r`n    <Priority>7</Priority>`r`n    <RestartOnFailure>`r`n      <Interval>PT6H</Interval>`r`n      <Count>4</Count>`r`n    </RestartOnFailure>`r`n  </Settings>`r`n  <Actions Context=`"Author`">`r`n    <Exec>`r`n      <Command>powershell.exe</Command>`r`n      <Arguments>-file `"C:\SolidCP\Exchange_HTTP_to_HTTPS_Redirection.ps1`"</Arguments>`r`n      <WorkingDirectory>C:\SolidCP</WorkingDirectory>`r`n    </Exec>`r`n  </Actions>`r`n</Task>`r`n") | Out-Null
				# Create the task and import the settings from the XML file we created above
				(schtasks /create /XML C:\SolidCP\Exchange_HTTP_Redirect-Scheduled-Task-Import.xml /tn "SolidCP - Fix Exchange HTTP Redirect") | Out-Null
				# Remove the file used to import the Scheduled Task
				(Remove-Item "C:\SolidCP\Exchange_HTTP_Redirect-Scheduled-Task-Import.xml" -Force) | Out-Null
			}
		}
	}
}


####################################################################################################################################################################################
Function Restart_IIS()                # Function to configure the Outlook Anywhere URL's for Exchange 2016
{
	# Restart IIS on the server
	if ((Get-Service "World Wide Web Publishing Service").Status -ne "running") { # Start IIS if it is not already running
		(Start-Service 'World Wide Web Publishing Service' -WarningAction SilentlyContinue) | Out-Null
		Write-Host "`t Starting IIS on this server" -ForegroundColor Green
		start-Sleep -Seconds 5
	}elseif ((Get-Service "World Wide Web Publishing Service").Status -eq "running") { # ReStart IIS if it is already running
		(Restart-Service 'World Wide Web Publishing Service' -Force -WarningAction SilentlyContinue) | Out-Null
		Write-Host "`t Restarting IIS on this server" -ForegroundColor Green
		start-Sleep -Seconds 5
	}
}


####################################################################################################################################################################################
Function Exchange2016_UpgradeCheck()  # Function to check Exchange 2016 Installation after Upgrades
{
	# Check if there are already any Exchange 2016 Servers on the domain
	if ( (((([ADSI]("LDAP://CN=ms-Exch-Schema-Version-Pt," + ([ADSI]"LDAP://RootDSE").schemaNamingContext)).rangeUpper).Value) -ge "15317") -and (Test-Path "C:\Program Files\Microsoft\Exchange Server\V15") ) {
		# Load the Microsoft Exchange PowerShell SnapIn
		add-pssnapin *exchange* -erroraction SilentlyContinue
		# Enable Automatic Replies
		if ((Get-RemoteDomain Default).AutoReplyEnabled -eq $false) {
			Write-Host "`t Enabling Auto Replies on the Exchange Server" -ForegroundColor Green
			Set-RemoteDomain Default -AutoReplyEnabled $true
		}
		# Enable Automatic Forwards
		if ((Get-RemoteDomain Default).AutoForwardEnabled -eq $false) {
			Write-Host "`t Enabling Auto Forwards on the Exchange Server" -ForegroundColor Green
			Set-RemoteDomain Default –AutoForwardEnabled $true
		}
		# Enable OOF for Outlook 2003 and previous (for Exchange 2007 and 2010 support)
		if ((Get-RemoteDomain Default).AllowedOOFType -eq "External") {
			Write-Host "`t Enabling Legacy Out Of Office Types on the Exchange Server" -ForegroundColor Green
			Set-RemoteDomain Default –AllowedOOFType "ExternalLegacy"
		}
		# Enable Meeting Forward Notifications
		if ((Get-RemoteDomain Default).MeetingForwardNotificationEnabled -eq $false) {
			Write-Host "`t Enabling Meeting Forward Notifications on the Exchange Server" -ForegroundColor Green
			Set-RemoteDomain Default -MeetingForwardNotificationEnabled $true
		}
		# Create the Exchange Securit Group "ContentSubmitters" if it does not alreay exist
		CreateExchangeContentSubmittersGroup
		# Check if there is a Send Connector configured, if not ask the user if they want one
		do { $SendConnectorChoice = Read-Host "`tWould you like to enable the Exchange Send Connector on this server`? (Y/N)" }
		until ($SendConnectorChoice -match "[y|n]")
		if ($SendConnectorChoice -eq "y") {
			if ( [string]::IsNullOrEmpty((Get-SendConnector).enabled) ) { # No Send Connectors are configured
				if (!($dExchangeFQDNsndC)) {
					do { $dExchangeFQDNsndC = Read-Host "`tPlease enter the FQDN for the Exchange Send Connector you would like to use" }
					until (!([string]::IsNullOrEmpty($dExchangeFQDNsndC)))
				}
				Write-Host "`t Configuring the SolidCP Send Connector on this Exchange Server" -ForegroundColor Green
				(New-SendConnector -Internet -Name "SolidCP Outbound Internet Email" -AddressSpaces "*" -UseExternalDNSServersEnabled $true -SourceTransportServers $env:COMPUTERNAME -MaxMessageSize "unlimited" -Fqdn "$dExchangeFQDNsndC") | Out-Null
			}elseif ( ((Get-SendConnector).enabled) -and ((((Get-SendConnector).Identity).Name) -eq "SolidCP Outbound Internet Email") ) { # Edit the SolidCP Send Connector
				Write-Host "`t Adding this server to the Default Send Connector on the Exchange Server" -ForegroundColor Green
				(Set-SendConnector -Identity "SolidCP Outbound Internet Email" -SourceTransportServers @{Add="$env:COMPUTERNAME"}) | Out-Null
			}
		}
		# Set the Transport Config Maximum Send and Receive message sizes as unlimited unless they are already set
		if ((!(Get-TransportConfig).MaxReceiveSize.IsUnlimited) -or (!(Get-TransportConfig).MaxSendSize.IsUnlimited)) {
			Write-Host "`t Configuring the Transport Config message sizes on this Exchange Server" -ForegroundColor Green
			(Set-TransportConfig -MaxSendSize unlimited -MaxReceiveSize unlimited) | Out-Null
		}
		# Set the Default Receive Connector perameters
		if ((Get-ReceiveConnector -Identity "$env:COMPUTERNAME\Default $env:COMPUTERNAME").enabled) {
			Write-Host "`t Configuring the Default Receive Connector on this Exchange Server" -ForegroundColor Green
			(Set-ReceiveConnector -Identity "$env:COMPUTERNAME\Default $env:COMPUTERNAME" -MaxMessageSize "2047MB") | Out-Null
		}
		# Set the Client Proxy Receive Connector perameters
		if ((Get-ReceiveConnector -Identity "$env:COMPUTERNAME\Client Proxy $env:COMPUTERNAME").enabled) {
			Write-Host "`t Configuring the Client Proxy Receive Connector on this Exchange Server" -ForegroundColor Green
			(Set-ReceiveConnector -Identity "$env:COMPUTERNAME\Client Proxy $env:COMPUTERNAME" -MaxMessageSize "2047MB") | Out-Null
		}
		# Set the Default Frontend Receive Connector perameters
		if ((Get-ReceiveConnector -Identity "$env:COMPUTERNAME\Default Frontend $env:COMPUTERNAME").enabled) {
			Write-Host "`t Configuring the Default Frontend Receive Connector on this Exchange Server" -ForegroundColor Green
			(Set-ReceiveConnector -Identity "$env:COMPUTERNAME\Default Frontend $env:COMPUTERNAME" -MaxMessageSize "2047MB" -PermissionGroups "AnonymousUsers, ExchangeServers, ExchangeLegacyServers") | Out-Null
		}
		# Set the Outbound Proxy Frontend Receive Connector perameters
		if ((Get-ReceiveConnector -Identity "$env:COMPUTERNAME\Outbound Proxy Frontend $env:COMPUTERNAME").enabled) {
			Write-Host "`t Configuring the Outbound Proxy Frontend Receive Connector on this Exchange Server" -ForegroundColor Green
			(Set-ReceiveConnector -Identity "$env:COMPUTERNAME\Outbound Proxy Frontend $env:COMPUTERNAME" -MaxMessageSize "2047MB") | Out-Null
		}
		# Set the Client Frontend Receive Connector perameters
		if ((Get-ReceiveConnector -Identity "$env:COMPUTERNAME\Client Frontend $env:COMPUTERNAME").enabled) {
			Write-Host "`t Configuring the Client Frontend Receive Connector on this Exchange Server" -ForegroundColor Green
			(Set-ReceiveConnector -Identity "$env:COMPUTERNAME\Client Frontend $env:COMPUTERNAME" -MaxMessageSize "2047MB") | Out-Null
		}
		# Check to make sure ALL receive Connectors are configured with the maximum values
		foreach ($SendConnector in (Get-ReceiveConnector)) {
			if ($SendConnector.MaxMessageSize -notmatch "$("{0:N0}" -f (2047 * 1048576)) bytes") {
				(Set-ReceiveConnector -Identity $SendConnector.Identity -MaxMessageSize "2047MB") | Out-Null
				Write-Host "`t Updating the `"$($SendConnector.Identity)`" Receive Connector" -ForegroundColor Green
			}
		}
		# Add the a rule to Exchange to move spam marked by MailCleaner to the users Junk Folder in Exchange only if it doesn't already exist
		if (!([bool](Get-TransportRule | Where {$_.Name -eq "MailCleaner - Move Spam to Junk Folder"}))) {
			Write-Host "`t Adding `"Move Spam marked by MailCleaner to Junk Folder`" rule in Exchange" -ForegroundColor Green
			(New-TransportRule -Name "MailCleaner - Move Spam to Junk Folder" -HeaderMatchesMessageHeader "X-MailCleaner-SpamCheck" -HeaderMatchesPatterns "spam" -SetSCL $(((Get-OrganizationConfig).SCLJunkThreshold) + 1) -ExceptIfHeaderMatchesMessageHeader "X-MailCleaner-SpamCheck" -ExceptIfHeaderMatchesPatterns "not spam" -Priority 0) | Out-Null
		}
		# Restart IIS on this server to apply the above changes
		Restart_IIS
		# If a DAG exists then see if this server needs to be added to one
		if ((Get-DatabaseAvailabilityGroup).Name) {
			AddThisServerToDAG
		}
	}
}


####################################################################################################################################################################################
Function InstallExchangeFSW()           # Function to install the features required for the Microsoft Exchange File Share Witness Server Role
{
	Write-Host "`tInstalling the features required for Microsoft Exchange Server (File Share Witness Role)" -ForegroundColor Cyan
	(Add-WindowsFeature -Name FS-FileServer) | Out-Null
	# Add the Domain Groups called "Exchange Trusted Subsystem" and "Organization Management" to the Local Group "Administrators"
	(AddDomainUserToLocalGroup "Administrators" "Exchange Trusted Subsystem") | Out-Null # Add the Domain "Exchange Trusted Subsystem" Group to the Local Group called "Administrators"
	(AddDomainUserToLocalGroup "Administrators" "Organization Management") | Out-Null    # Add the Domain "Organization Management" Group to the Local Group called "Administrators"
}


####################################################################################################################################################################################
Function InstallMailEnable()           # Function to install the features required for the MailEnable Email Server
{
	if (!($dMailEnableDirectory)) {
		$dMailEnableDirectory = "C:\Program Files (x86)\Mail Enable"
	}
	if ( ( !(Test-Path "$dMailEnableDirectory") ) -and ( !(Test-Path "HKLM:\SOFTWARE\Wow6432Node\Mail Enable\Mail Enable") ) ) { # Only install MailEnable if it isn't already installed
		Write-Host "`tInstalling the features required for MailEnable Email Server" -ForegroundColor Cyan
		Write-Host "`t  ****************************************" -ForegroundColor Green
		Write-Host "`t  *                                      *" -ForegroundColor Green
		Write-Host "`t  *  Please be patient while we install  *" -ForegroundColor Green
		Write-Host "`t  *    the required Winodws Features     *" -ForegroundColor Green
		Write-Host "`t  *                                      *" -ForegroundColor Green
		Write-Host "`t  ****************************************" -ForegroundColor Green
		(Add-WindowsFeature -Name Web-Request-Monitor, Web-Basic-Auth, Web-Metabase, Web-WMI, WAS-Process-Model, Web-ASP, Web-CGI, Web-Mgmt-Compat, Web-Lgcy-Mgmt-Console, Web-IP-Security, RSAT, WAS, WAS-NET-Environment, WAS-Config-APIs) | Out-Null
		# Set the correct Registry Entries for MailEnable Standard to enable the Silent Install
		if (($dIPV4) -and (!($dIPV6))) {
			$dMailEnableIPs = "$dIPV4"
		}elseif ((!($dIPV4)) -and ($dIPV6)) {
			$dMailEnableIPs = "$dIPV6"
		}elseif ($dIPV4 -and $dIPV6) {
			$dMailEnableIPs = "$dIPV6;$dIPV4"
		}
		(New-Item -Path "HKLM:\SOFTWARE\Wow6432Node\Mail Enable\Mail Enable" -Force -ErrorAction SilentlyContinue) | Out-Null
		do {$dTestRegPath = (Test-Path "HKLM:\SOFTWARE\Wow6432Node\Mail Enable\Mail Enable")} until ($dTestRegPath)
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "Program Directory" -Value "$dMailEnableDirectory" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "Activity Log Directory" -Value "$dMailEnableDirectory\Logging" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "Debug Log File Directory" -Value "$dMailEnableDirectory\Logging" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "W3C Logging Directory" -Value "$dMailEnableDirectory\Logging" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "Configuration Directory" -Value "$dMailEnableDirectory\CONFIG" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "Application Directory" -Value "$dMailEnableDirectory\Bin" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "Bad Mail Directory" -Value "$dMailEnableDirectory\Bad Mail" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "Data Directory" -Value "$dMailEnableDirectory" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "Quarantine Directory" -Value "$dMailEnableDirectory\Quarantine" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "Mail Root" -Value "$dMailEnableDirectory\POSTOFFICES" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "Update Statistics" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "Backup Index" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "Index Notification Timeout" -Value "300000" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "Version" -Value "9.51" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "IPv6 Status" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "Mailbox Size Calc Mode" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "Store Event Notification Mode" -Value "4" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "Index Commit Mode" -Value "4" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "Service Notification Mode" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "Create Emails for All Domains" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "Admin Winsock Enabled" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "Authoring Version" -Value "2" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "Framework Min Version" -Value "v2.0.50727" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "Use MIME Library" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "AutoResponder Restriction State" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "iPhone Automatic Configuration Enabled" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable" -Name "System Messages Enabled" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-Item -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\LS" -Force -ErrorAction SilentlyContinue) | Out-Null
		do {$dTestRegPath = (Test-Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\LS")} until ($dTestRegPath)
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\LS" -Name "Transfer Count" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\LS" -Name "Pickup Count" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\LS" -Name "Outbound Queue Item Max Age" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\LS" -Name "Outbound Queue Length" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\LS" -Name "Inbound Queue Length" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-Item -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\POP" -Force -ErrorAction SilentlyContinue) | Out-Null
		do {$dTestRegPath = (Test-Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\POP")} until ($dTestRegPath)
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\POP" -Name "Inbound Queue Length" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-Item -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SF" -Force -ErrorAction SilentlyContinue) | Out-Null
		do {$dTestRegPath = (Test-Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SF")} until ($dTestRegPath) 
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SF" -Name "Transfer Count" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SF" -Name "Pickup Count" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SF" -Name "Outbound Queue Item Max Age" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SF" -Name "Outbound Queue Length" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SF" -Name "Inbound Queue Length" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-Item -Path "HKLM:\SOFTWARE\Wow6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Force -ErrorAction SilentlyContinue) | Out-Null
		do {$dTestRegPath = (Test-Path "HKLM:\SOFTWARE\Wow6432Node\Mail Enable\Mail Enable\Connectors\SMTP")} until ($dTestRegPath)
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Additional Ports" -Value "26,0,0,0" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Alternate Port Authentication Mode" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Alternate Port Relay Mode" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Alternate Port SSL Enabled" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Bad Mail Notification Sender Address" -Value "POSTMASTER@$(("$env:COMPUTERNAME.$env:USERDNSDOMAIN").ToLower())" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Concurrent Outbound Limit" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "CRAM-MD5 Authentication Enabled" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Disable catchalls" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Drop Folder Status" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Enforce inbound message size" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Enforce Inbound Recipient Limits" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Force Outbound Interface" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Host Name" -Value "$(("$env:COMPUTERNAME.$env:USERDNSDOMAIN").ToLower())" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Inbound Server Bindings" -Value "$dMailEnableIPs" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Limit Outbound Message Size" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Listen Port Authentication Mode" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Listen Port Relay Mode" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Listen Port SSL Enabled" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Local DNS Server List" -Value "$((Get-WMIObject -Class "Win32_NetworkAdapterConfiguration" -Filter "IPEnabled=TRUE").DNSServerSearchOrder)" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Local Domain Name" -Value "$(("$env:COMPUTERNAME.$env:USERDNSDOMAIN").ToLower())" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Local Senders Must Authenticate" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Maximum Outbound Message Size" -Value "10484736" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Maximum Recv Threads" -Value "320" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "NDR copy to" -Value "" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "NDR in inbound" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "No Delay Notification Generation" -Value "2" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "No NDR Generation" -Value "3" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Outbound Failed Message Lifetime" -Value "48" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Outbound Interface" -Value "$dIPV4" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Outbound TLS Mode" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Require PTR" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Resolve Sender Domain" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Restrict Concurrent Outbound" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Restrict Inbound Message" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Sender must be valid" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Subsequent Message Retry Interval" -Value "30" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Transfer Count" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Pickup Count" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Outbound Queue Item Max Age" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Outbound Queue Length" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Outbound Delivery Count" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Inbound Queue Length" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP" -Name "Inbound Delivery Count" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-Item -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Counters" -Force -ErrorAction SilentlyContinue) | Out-Null
		do {$dTestRegPath = (Test-Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Counters")} until ($dTestRegPath)
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Counters" -Name "XBL Message Detections" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Counters" -Name "XBL Message Scans" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Counters" -Name "RDNS Tested" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Counters" -Name "RDNS Messages Detected" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-Item -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Extensions\AUTH" -Force -ErrorAction SilentlyContinue) | Out-Null
		do {$dTestRegPath = (Test-Path "HKLM:\SOFTWARE\Wow6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Extensions\AUTH")} until ($dTestRegPath)
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Extensions\AUTH" -Name "Code" -Value "AUTH NTLM" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Extensions\AUTH" -Name "Data" -Value "LOGIN" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-Item -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Extensions\DATA" -Force -ErrorAction SilentlyContinue) | Out-Null
		do {$dTestRegPath = (Test-Path "HKLM:\SOFTWARE\Wow6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Extensions\DATA")} until ($dTestRegPath)
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Extensions\DATA" -Name "Code" -Value "SIZE" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Extensions\DATA" -Name "Data" -Value "157286400" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-Item -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Logging\W3C" -Force -ErrorAction SilentlyContinue) | Out-Null
		do {$dTestRegPath = (Test-Path "HKLM:\SOFTWARE\Wow6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Logging\W3C")} until ($dTestRegPath)
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Logging\W3C" -Name "date" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Logging\W3C" -Name "time" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Logging\W3C" -Name "c-ip" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Logging\W3C" -Name "account" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Logging\W3C" -Name "s-ip" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Logging\W3C" -Name "s-port" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Logging\W3C" -Name "cs-method" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Logging\W3C" -Name "cs-uri-stem" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Logging\W3C" -Name "cs-uri-query" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Logging\W3C" -Name "s-computername" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Logging\W3C" -Name "sc-bytes" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Logging\W3C" -Name "cs-bytes" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Logging\W3C" -Name "time-taken" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Logging\W3C" -Name "agent" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-Item -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\IMAP" -Force -ErrorAction SilentlyContinue) | Out-Null
		do {$dTestRegPath = (Test-Path "HKLM:\SOFTWARE\Wow6432Node\Mail Enable\Mail Enable\Services\IMAP")} until ($dTestRegPath)
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\IMAP" -Name "Alternate Port" -Value "993" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\IMAP" -Name "Alternate Port Enabled" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\IMAP" -Name "Alternate Port SSL Enabled" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\IMAP" -Name "Extensions" -Value "IMAP4 AUTH=LOGIN AUTH=CRAM-MD5 IDLE CHILDREN UIDPLUS AUTH=NTLM" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\IMAP" -Name "Inbound Server Bindings" -Value "$dMailEnableIPs" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\IMAP" -Name "IPv6 Status" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\IMAP" -Name "Limit Receive Threads" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\IMAP" -Name "Listen Port" -Value "143" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\IMAP" -Name "Listen Port SSL Enabled" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\IMAP" -Name "Public Folders Enabled" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\IMAP" -Name "W3C Logging Rollover Frequency" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-Item -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\POP" -Force -ErrorAction SilentlyContinue) | Out-Null
		do {$dTestRegPath = (Test-Path "HKLM:\SOFTWARE\Wow6432Node\Mail Enable\Mail Enable\Services\POP")} until ($dTestRegPath)
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\POP" -Name "Alternate Port Enabled" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\POP" -Name "Alternate Port SSL Enabled" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\POP" -Name "APOP Enabled" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\POP" -Name "Extensions" -Value "" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\POP" -Name "Inbound Server Bindings" -Value "$dMailEnableIPs" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\POP" -Name "IPv6 Status" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\POP" -Name "Listen Port SSL Enabled" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-Item -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\POP\Logging\W3C" -Force -ErrorAction SilentlyContinue) | Out-Null
		do {$dTestRegPath = (Test-Path "HKLM:\SOFTWARE\Wow6432Node\Mail Enable\Mail Enable\Services\POP\Logging\W3C")} until ($dTestRegPath)
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\POP\Logging\W3C" -Name "date" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\POP\Logging\W3C" -Name "time" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\POP\Logging\W3C" -Name "c-ip" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\POP\Logging\W3C" -Name "s-ip" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\POP\Logging\W3C" -Name "s-port" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\POP\Logging\W3C" -Name "cs-method" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\POP\Logging\W3C" -Name "cs-uri-stem" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\POP\Logging\W3C" -Name "cs-uri-query" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\POP\Logging\W3C" -Name "s-computername" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\POP\Logging\W3C" -Name "sc-bytes" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\POP\Logging\W3C" -Name "cs-bytes" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\POP\Logging\W3C" -Name "time-taken" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\POP\Logging\W3C" -Name "agent" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-Item -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL\Options" -Force -ErrorAction SilentlyContinue) | Out-Null
		do {$dTestRegPath = (Test-Path "HKLM:\SOFTWARE\Wow6432Node\Mail Enable\Mail Enable\Services\WEBMAIL\Options")} until ($dTestRegPath)
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL\Options" -Name "Auto Response" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL\Options" -Name "BannerStatus" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL\Options" -Name "CanEditDisplayName" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL\Options" -Name "Default Timezone" -Value "GMT Standard Time" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL\Options" -Name "Help Base" -Value "http://www.mailenable.com/Help" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL\Options" -Name "Help Status" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL\Options" -Name "Hide Client IP" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL\Options" -Name "Import Status" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL\Options" -Name "Login Details" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL\Options" -Name "Mailbox Redirection" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL\Options" -Name "Media Player Enabled" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-Item -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Providers\Authentication" -Force -ErrorAction SilentlyContinue) | Out-Null
		do {$dTestRegPath = (Test-Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Providers\Authentication")} until ($dTestRegPath)
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Providers\Authentication" -Name "Default Account" -Value "$(("$env:COMPUTERNAME.$env:USERDNSDOMAIN").ToLower())" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Providers\Authentication" -Name "Default Account Enabled" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-Item -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Registration" -Force -ErrorAction SilentlyContinue) | Out-Null
		do {$dTestRegPath = (Test-Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Registration")} until ($dTestRegPath)
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Registration" -Name "Name" -Value "$dMailEnableContactName" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Registration" -Name "Company" -Value "$dMailEnableContactName" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-Item -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WebAdmin" -Force -ErrorAction SilentlyContinue) | Out-Null
		do {$dTestRegPath = (Test-Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WebAdmin")} until ($dTestRegPath)
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WebAdmin" -Name "SiteId" -Value "Default Web Site" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-Item -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL" -Force -ErrorAction SilentlyContinue) | Out-Null
		do {$dTestRegPath = (Test-Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL")} until ($dTestRegPath)
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL" -Name "SiteId" -Value "Default Web Site" -Force -ErrorAction SilentlyContinue) | Out-Null
		(New-Item -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Updates"  -Force -ErrorAction SilentlyContinue) | Out-Null
		do {$dTestRegPath = (Test-Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Updates")} until ($dTestRegPath)
		(New-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Updates" -Name "Updates Ready" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		# Change the bindings on the default website to Port 8080 so we can install MailEnable WebMail on Port 80 unless it has already been done
		if ( (Get-WebBinding -Name 'Default Web Site').bindingInformation -eq "*:80:" ) {
			Set-WebBinding -Name 'Default Web Site' -BindingInformation "*:80:" -PropertyName Port -Value 8080
		}
		# Get the latest download information from the SolidCP Installer site
		($MailEnable = (SolidCPFileDownload "MailEnable")) | Out-Null
		# Create the MailEnable Directory in our Installation Files folder ready for downloading if it doesn't already exist
		if (!(Test-Path "C:\_Install Files\$($MailEnable.FolderName)")) { (md -Path "C:\_Install Files\$($MailEnable.FolderName)" -Force) | Out-Null }
		# Download the files required for MailEnable
		Write-Host "`t Downloading the latest version of MailEnable" -ForegroundColor Green
		(New-Object System.Net.WebClient).DownloadFile("$($MailEnable.DownloadURL)", "C:\_Install Files\$($MailEnable.FolderName)\$($MailEnable.FileName)")
		# Install the MailEnable Email Server
		Write-Host "`t Installing the MailEnable Email Server" -ForegroundColor Green
		Write-Host "`t    ****************************************" -ForegroundColor Green
		Write-Host "`t    *                                      *" -ForegroundColor Green
		Write-Host "`t    *     This will take quite a while     *" -ForegroundColor Green
		Write-Host "`t    *          to fully complete           *" -ForegroundColor Green
		Write-Host "`t    *                                      *" -ForegroundColor Green
		Write-Host "`t    *  Please be patient while we install  *" -ForegroundColor Green
		Write-Host "`t    *     MailEnable Standard Edition      *" -ForegroundColor Green
		Write-Host "`t    *                                      *" -ForegroundColor Green
		Write-Host "`t    ****************************************" -ForegroundColor Green
		$dMailEnableInstallProcess = Start-Process -FilePath "C:\_Install Files\$($MailEnable.FolderName)\$($MailEnable.FileName)" -ArgumentList "/s /B" -Passthru
		do {start-sleep -Milliseconds 500}
		until ($dMailEnableInstallProcess.HasExited)
		Write-Host "`t Configuring the MailEnable Email Server" -ForegroundColor Green
		(Set-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Extensions\AUTH" -Name "Code" -Value "AUTH NTLM" -Force -ErrorAction SilentlyContinue) | Out-Null
		(Set-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Connectors\SMTP\Logging\W3C" -Name "time-taken" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(Set-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL\Options" -Name "Auto Response" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(Set-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL\Options" -Name "BannerStatus" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(Set-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL\Options" -Name "CanEditDisplayName" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(Set-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL\Options" -Name "Default Timezone" -Value "GMT Standard Time" -Force -ErrorAction SilentlyContinue) | Out-Null
		(Set-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL\Options" -Name "Help Base" -Value "http://www.mailenable.com/Help" -Force -ErrorAction SilentlyContinue) | Out-Null
		(Set-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL\Options" -Name "Help Status" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(Set-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL\Options" -Name "Hide Client IP" -Value "1" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(Set-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL\Options" -Name "Import Status" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(Set-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL\Options" -Name "Login Details" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(Set-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL\Options" -Name "Mailbox Redirection" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		(Set-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Mail Enable\Mail Enable\Services\WEBMAIL\Options" -Name "Media Player Enabled" -Value "0" -Type DWord -Force -ErrorAction SilentlyContinue) | Out-Null
		# Start the MailEnable Services on the server
		Write-Host "`t Starting the MailEnable Email Services" -ForegroundColor Green
		(Start-Service "MEIMAPS"  -WarningAction SilentlyContinue) | Out-Null
		(Start-Service "MELCS"    -WarningAction SilentlyContinue) | Out-Null
		(Start-Service "MEMTAS"   -WarningAction SilentlyContinue) | Out-Null
		(Start-Service "MEDMS"    -WarningAction SilentlyContinue) | Out-Null
		(Start-Service "MEPOPS"   -WarningAction SilentlyContinue) | Out-Null
		(Start-Service "MEPOCS"   -WarningAction SilentlyContinue) | Out-Null
		(Start-Service "MESMTPCS" -WarningAction SilentlyContinue) | Out-Null
		# Create the desktop shortcut if it doesn't already exist
		if (!(Test-Path "$env:PUBLIC\Desktop\MailEmable.lnk")) {
			$MailEnableShortcut = ((New-Object -comObject WScript.Shell).CreateShortcut("$env:PUBLIC\Desktop\MailEmable.lnk"))
			$MailEnableShortcut.TargetPath = "$dMailEnableDirectory\Admin\MailEnableAdmin.msc"
			$MailEnableShortcut.WorkingDirectory = "$dMailEnableDirectory\Admin\"
			$MailEnableShortcut.IconLocation = "$dMailEnableDirectory\Bin\MailEnable.ico"
			$MailEnableShortcut.Save()
		}
		# Import the PowerShell Web Administration Module
		Import-Module WebAdministration
		# Configure the Webmail and Web Admin sites
		Set-WebConfigurationProperty -Filter '/system.applicationHost/sites/site[@name="MailEnable WebMail"]' -PSPath "IIS:\" -Name "Bindings" -Value (@{protocol="http";bindingInformation="*:80:"})
		# Create a local group called "Administrators File Access" on the serverif it does not exist
		CreateLocalUserOrGroup "Administrators File Access" "Local Administrators File Access - SCP Harden IIS  ********* DO NOT DELETE *********" "Group"
		# Add the Local "Administrator" to the Local Group called "Administrators File Access" unless it is already a mmember of the group
		AddLocalUserToLocalGroup "Administrators File Access" "$dLangAdministratorName"
		# Copy over the members of the Local "Administrators" group over to the Local "Administrators File Access" group
		if ((@(Compare-Object -ReferenceObject (GetMembersOfGroup "Administrators" "Name") -DifferenceObject (GetMembersOfGroup "Administrators File Access" "Name") -SyncWindow 0).length) -ne "0") {
			CopyGroupMembers "Administrators" "Administrators File Access" "Local"
		}
		# Set the "preloadEnabled" value to True to speed up loading on the MailEnable webmail site
		if (Test-Path "IIS:\Sites\MailEnable WebMail" -pathType container) {
			if ((Get-ItemProperty "IIS:\Sites\MailEnable WebMail" -Name applicationDefaults.preloadEnabled).Value -eq $false) {
				Set-ItemProperty "IIS:\Sites\MailEnable WebMail" -Name applicationDefaults.preloadEnabled -Value $true
			}
		}
	}
}


####################################################################################################################################################################################
Function InstallAwStats()               # Function for downloading and installing AwStats
{
	if ( ( !(Test-Path "C:\AWStats\wwwroot\") ) -and ( !(Test-Path "HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\AWStats") ) ) { 
		Write-Host "`tInstalling the features required for AwStats" -ForegroundColor Cyan
		# Install the additional IIS Requirements for AwStats
		(Add-WindowsFeature -Name Web-CGI) | Out-Null
		# Import the PowerShell Web Administration Module
		Import-Module WebAdministration
		# Configure all logging options to be enabled for websites in IIS
		Set-WebConfigurationProperty -Filter System.Applicationhost/Sites/SiteDefaults/logfile -Name LogExtFileFlags -Value "Date,Time,ClientIP,UserName,SiteName,ComputerName,ServerIP,Method,UriStem,UriQuery,HttpStatus,Win32Status,BytesSent,BytesRecv,TimeTaken,ServerPort,UserAgent,Cookie,Referer,ProtocolVersion,Host,HttpSubStatus"
		# Get the latest download information from the SolidCP Installer site
		($AwStats_WebApp = (SolidCPFileDownload "AwStats_WebApp")) | Out-Null
		# Create the AwStats Directory in our Installation Files folder ready for downloading if it doesn't already exist
		if (!(Test-Path "C:\_Install Files\$($AwStats_WebApp.FolderName)")) { (md -Path "C:\_Install Files\$($AwStats_WebApp.FolderName)" -Force) | Out-Null }
		# Download the files required for AwStats
		Write-Host "`t Downloading the latest version of AwStats" -ForegroundColor Green
		(New-Object System.Net.WebClient).DownloadFile("$($AwStats_WebApp.DownloadURL)", "C:\_Install Files\$($AwStats_WebApp.FolderName)\$($AwStats_WebApp.FileName)")
		# Get the highest current SolidCP Version from the Installer XML File from the SolidCP Installation Website
		[string]$dSolidCPversion = ((([xml](New-Object System.Net.WebClient).DownloadString("http://installer.solidcp.com/Data/ProductReleasesFeed-1.0.xml")).components.component.releases.release).version | measure -Maximum).Maximum
		# Download the latest SolidCP AwStats Viewer based upon the highest version above
		Write-Host "`t Downloading the latest version of the AwStats Viewer from the SolidCP website" -ForegroundColor Green
		(New-Object System.Net.WebClient).DownloadFile("http://installer.solidcp.com/Files/stable/Tools/SolidCP-AWStatsViewer.zip", "C:\_Install Files\$($AwStats_WebApp.FolderName)\SolidCP-AWStatsViewer.zip")
		# Unzip the downloaded files to the C:\AwStats directory
		Write-Host "`t Extracting and installing AwStats from the downloaded files" -ForegroundColor Green
		(Add-Type -assembly "system.io.compression.filesystem" -ErrorAction SilentlyContinue) | Out-Null
		[io.compression.zipfile]::ExtractToDirectory("C:\_Install Files\$($AwStats_WebApp.FolderName)\$($AwStats_WebApp.FileName)", "C:\AWStats")
		[io.compression.zipfile]::ExtractToDirectory("C:\_Install Files\$($AwStats_WebApp.FolderName)\SolidCP-AWStatsViewer.zip", "C:\AWStats\awstats-develop\wwwroot")
		( (Get-ChildItem "C:\AWStats\" -include *.cvsignore -recurse | foreach ($_) {remove-item $_.fullname}) ) | Out-Null
		(Move-Item "C:\AWStats\awstats-develop\wwwroot\LICENSE.txt" "C:\AWStats") | Out-Null
		(Move-Item "C:\AWStats\awstats-develop\wwwroot\Readme.htm" "C:\AWStats") | Out-Null
		(Move-Item "C:\AWStats\awstats-develop\wwwroot\Web-v2.0.config" "C:\AWStats") | Out-Null
		(Move-Item "C:\AWStats\awstats-develop\docs\*"    "C:\AwStats\docs\") | Out-Null
		(Move-Item "C:\AWStats\awstats-develop\tools\*"   "C:\AwStats\tools\") | Out-Null
		(Move-Item "C:\AWStats\awstats-develop\wwwroot\*" "C:\AwStats\wwwroot\") | Out-Null
		(Remove-Item "C:\AWStats\awstats-develop" -Recurse) | Out-Null
		(md -Path  "C:\AWStats\wwwroot\bin\" -Force) | Out-Null
		(Move-Item "C:\AWStats\wwwroot\*.dll" "C:\AwStats\wwwroot\bin\") | Out-Null
		(Move-Item "C:\AWStats\wwwroot\SolidCP.AWStats.Viewer.dll.config" "C:\AwStats\wwwroot\bin\") | Out-Null
		(md -Path "C:\AWStats\data") | Out-Null
		(md -Path "C:\AWStats\logs") | Out-Null

		# Create the AwStats configuration File Template file
		(New-Item C:\AWStats\AwStats-Configuration-Template-File.txt -type file -force -value "LogFormat = `"%time2 %other %other %other %method %url %other %other %logname %host %other %ua %other %referer %other %code %other %other %bytesd %other %other`"`r`nLogSeparator = `" `"`r`nDNSLookup = 1`r`nDirCgi = `"/awstats/cgi-bin`"`r`nDirIcons = `"/awstats/icon`"`r`nAllowFullYearView=3`r`nAllowToUpdateStatsFromBrowser = 0`r`nUseFramesWhenCGI = 1`r`nShowFlagLinks = `"en fr de it nl es`"`r`nLogFile = `"[LOGS_FOLDER]\u_ex%YY-3%MM-3%DD-3.log`"`r`nDirData = `"C:\AWStats\data`"`r`nSiteDomain = `"[DOMAIN_NAME]`"`r`nHostAliases = [DOMAIN_ALIASES]`r`n") | Out-Null
		# Create the BAT File that will be used for AwStats to run the processing of website stats
		(New-Item C:\AWStats\UpdateStats.bat -type file -force) | Out-Null
		# Create the AwStats Scheduled Job as XML Format so we can import it
		if (!($dDomainMember)) { # Check to see if the machine is NOT joined to a domain
			if (CheckGroupMembers "$dLangAdministratorGroup" "$dLoggedInUserName" "Local") { # Logged in user is a Local Administrator
				$dAwStatsTaskUser = $dLocalAdministratorSID
			}
		}elseif ( ($dDomainMember) -and (!($dLoggedInLocally)) ) {
			if (CheckGroupMembers "$dLangDomainAdminsGroup" "$dLoggedInUserName" "Domain") { # Logged in user is a Domain Administrator
				$dAwStatsTaskUser = $dDomainAdministratorSID
			}
		}
		# Add the user for the Scheduled Task to the local security with "Log on as a batch job"
		if (Test-Path "c:\secpol-awstats-before.cfg") {(rm -force c:\secpol-awstats-before.cfg -confirm:$false -ErrorAction SilentlyContinue) | Out-Null}
		if (Test-Path "c:\secpol-awstats-after.cfg") {(rm -force c:\secpol-awstats-after.cfg -confirm:$false -ErrorAction SilentlyContinue) | Out-Null}
		(secedit /export /cfg c:\secpol-awstats-before.cfg) | Out-Null;
		foreach ($dLine in (Get-Content "c:\secpol-awstats-before.cfg")) {
			$dValSplit = $dLine.Split('=')
			if ( ($dValSplit[0] -match "SeBatchLogonRight") -and ($dValSplit[1] -notmatch "$dAwStatsTaskUser") ) {
				"$($dValSplit[0])= *$dAwStatsTaskUser,$(($dValSplit[1]).trim(' '))" | Out-File -Append -FilePath "c:\secpol-awstats-after.cfg"
			}else{
				$dLine | Out-File -Append -FilePath "c:\secpol-awstats-after.cfg"
			}
		}
		(secedit /configure /db c:\windows\security\local.sdb /cfg c:\secpol-awstats-after.cfg /areas USER_RIGHTS) | Out-Null;
		(rm -force c:\secpol-awstats-before.cfg -confirm:$false -ErrorAction SilentlyContinue) | Out-Null
		(rm -force c:\secpol-awstats-after.cfg -confirm:$false -ErrorAction SilentlyContinue) | Out-Null
		# Create the AwStats scheduled task file ready to import
		(New-Item C:\AWStats\AwStats-Scheduled-Task-Import.xml -type file -force -value "<?xml version=`"1.0`" encoding=`"UTF-16`"?>`r`n<Task version=`"1.2`" xmlns=`"http://schemas.microsoft.com/windows/2004/02/mit/task`">`r`n  <RegistrationInfo>`r`n    <Date>2015-01-01T00:00:00.000000</Date>`r`n    <Author>SolidCP</Author>`r`n    <Description>Batch file that runs every 1 hour to poll each website that has Advanced Stats enabled on it.</Description>`r`n  </RegistrationInfo>`r`n  <Triggers>`r`n    <CalendarTrigger>`r`n      <Repetition>`r`n        <Interval>PT1H</Interval>`r`n        <Duration>P1D</Duration>`r`n        <StopAtDurationEnd>false</StopAtDurationEnd>`r`n      </Repetition>`r`n      <StartBoundary>2015-01-01T00:00:00</StartBoundary>`r`n      <ExecutionTimeLimit>PT1H</ExecutionTimeLimit>`r`n      <Enabled>true</Enabled>`r`n      <ScheduleByDay>`r`n        <DaysInterval>1</DaysInterval>`r`n      </ScheduleByDay>`r`n    </CalendarTrigger>`r`n  </Triggers>`r`n  <Principals>`r`n    <Principal id=`"Author`">`r`n      <RunLevel>HighestAvailable</RunLevel>`r`n      <UserId>$dAwStatsTaskUser</UserId>`r`n      <LogonType>S4U</LogonType>`r`n    </Principal>`r`n  </Principals>`r`n  <Settings>`r`n    <MultipleInstancesPolicy>IgnoreNew</MultipleInstancesPolicy>`r`n    <DisallowStartIfOnBatteries>false</DisallowStartIfOnBatteries>`r`n    <StopIfGoingOnBatteries>false</StopIfGoingOnBatteries>`r`n    <AllowHardTerminate>true</AllowHardTerminate>`r`n    <StartWhenAvailable>true</StartWhenAvailable>`r`n    <RunOnlyIfNetworkAvailable>false</RunOnlyIfNetworkAvailable>`r`n    <IdleSettings>`r`n      <StopOnIdleEnd>true</StopOnIdleEnd>`r`n      <RestartOnIdle>false</RestartOnIdle>`r`n    </IdleSettings>`r`n    <AllowStartOnDemand>true</AllowStartOnDemand>`r`n    <Enabled>true</Enabled>`r`n    <Hidden>false</Hidden>`r`n    <RunOnlyIfIdle>false</RunOnlyIfIdle>`r`n    <WakeToRun>true</WakeToRun>`r`n    <ExecutionTimeLimit>PT12H</ExecutionTimeLimit>`r`n    <Priority>7</Priority>`r`n    <RestartOnFailure>`r`n      <Interval>PT10M</Interval>`r`n      <Count>5</Count>`r`n    </RestartOnFailure>`r`n  </Settings>`r`n  <Actions Context=`"Author`">`r`n    <Exec>`r`n      <Command>C:\AWStats\UpdateStats.bat</Command>`r`n      <WorkingDirectory>C:\AWStats</WorkingDirectory>`r`n    </Exec>`r`n  </Actions>`r`n</Task>`r`n") | Out-Null
		# Create the task and import the settings from the XML file we created above
		(schtasks /create /XML C:\AWStats\AwStats-Scheduled-Task-Import.xml /tn "AwStats - Update Stats") | Out-Null
		# Change the bindings on the default website to Port 8080 so we can install AwStats on Port 80 unless it has already been done
		if ( (Get-WebBinding -Name 'Default Web Site').bindingInformation -eq "*:80:" ) {
			Set-WebBinding -Name 'Default Web Site' -BindingInformation "*:80:" -PropertyName Port -Value 8080
			# Another way to do the above is as per the line below
			#Set-WebConfigurationProperty "/system.applicationHost/sites/site[@name='Default Web Site']/bindings/binding[@protocol='http']" -name bindingInformation -value '*:8080:'
		}
		# Create the AppPool for AwStats
		Write-Host "`t Creating the AwStats Application Pool in IIS" -ForegroundColor Green
		(New-WebAppPool -Name "SolidCP AwStats .NET v4" -Force) | Out-Null
		# Create the new website for AwStats
		Write-Host "`t Creating the AwStats website in IIS" -ForegroundColor Green
		(New-Website -Name "SolidCP AwStats" -Port 80 -IPAddress "*" -PhysicalPath "C:\AWStats\wwwroot" -ApplicationPool "SolidCP AwStats .NET v4" -Force) | Out-Null
		(Set-ItemProperty "IIS:\Sites\SolidCP AwStats" -name logfile.directory "C:\AWStats\logs" -Force) | Out-Null
		# Grant Read & Execute permissions on wwwroot folder and Logs Folder
		SetAccessToFolderNoChk "C:\AWStats\wwwroot" "IIS AppPool\SolidCP AwStats .NET v4" "ReadAndExecute" "Allow";
		SetAccessToFolderNoChk "C:\AWStats\wwwroot" "BUILTIN\IIS_IUSRS" "ReadAndExecute" "Allow";
		SetAccessToFolderNoChk "C:\AWStats\wwwroot" "BUILTIN\$dLangUsersGroup" "ReadAndExecute" "Allow";
		SetAccessToFolderNoChk "C:\AWStats\logs" "NT SERVICE\TrustedInstaller" "FullControl" "Allow";
		(Restart-Service 'World Wide Web Publishing Service' -Force -WarningAction SilentlyContinue) | Out-Null
		start-Sleep -Seconds 5
		Write-Host "`t Adding the Registry Entries for AwStats" -ForegroundColor Green
		InstallAwStatsRegKeys | Out-Null
		# Update the AWstats web.config file to hold the corect values for the connection to the Enterprise Server for authentication
		ModifyXML "C:\AWStats\wwwroot\web.config" "Update" "//configuration/appSettings/add[@key='AWStats.ConfigFileAuthenticationProvider.DataFolder']/@value"    "C:\AWStats\data"
		# Check if the SolidCP Enterprise Server URL has been set, if not ask for it
		if (!($dSolidCPEnterpriseSvrURL)) {
			Write-Host "`t You have not set the `"`$dSolidCPEnterpriseSvrURL`" variable at the top of this script" -ForegroundColor Yellow
			do { $script:dSolidCPEnterpriseSvrURL = Read-Host "`tPlease enter the SolidCP Enterprise Server URL (i.e. http://PublicIP:9002)" }
			until (!([string]::IsNullOrEmpty($dSolidCPEnterpriseSvrURL)))
		}
		ModifyXML "C:\AWStats\wwwroot\web.config" "Update" "//configuration/appSettings/add[@key='AWStats.SolidCPAuthenticationProvider.EnterpriseServer']/@value" "$dSolidCPEnterpriseSvrURL"
		# Make the required changes ONLY if the SolidCP AwStats site exists
		if (Test-Path "IIS:\Sites\SolidCP AwStats" -pathType container) {
			# Get the Application Pool for the SolidCP Enterprise Server ready to make changes
			$pool = Get-Item 'IIS:\AppPools\SolidCP AwStats .NET v4'
			$pool.autoStart = 'True'                                # Enable Auto-Start
			$pool.startMode = 'AlwaysRunning'                       # Set Start Mode as Always Running
			$pool.processModel.idleTimeout = [TimeSpan]::Zero       # Set Idele TimeOut as 0 Minutes
			$pool.recycling.periodicRestart.time = [TimeSpan]::Zero # Set Regular time interval as 0 Minutes
			$pool | Set-Item                                        # Set all of the above for the SolidCP AwStats Application Pool
			# Remove any schedules for the SolidCP AwStats Application Pool and set the time to be 3.00am as it needs to be recycled once a day
			Clear-ItemProperty "IIS:\AppPools\SolidCP AwStats .NET v4" -Name recycling.periodicRestart.schedule
			Set-ItemProperty "IIS:\AppPools\SolidCP AwStats .NET v4" -Name recycling.periodicRestart.schedule -Value @{value='03:00:00'}
			# Set the "preloadEnabled" value to True to speed up loading on the AwStats site
			if ((Get-ItemProperty "IIS:\Sites\SolidCP AwStats" -Name applicationDefaults.preloadEnabled).Value -eq $false) {
				Set-ItemProperty "IIS:\Sites\SolidCP AwStats" -Name applicationDefaults.preloadEnabled -Value $true
			}
		}
	}
}

####################################################################################################################################################################################
Function InstallAwStatsRegKeys()        # Function for downloading and installing AwStats
{
	if (!(Test-Path "HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\AWStats")) {
		(New-Item -Path "HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\AWStats" -Force) ; start-Sleep -Seconds 2
	}
	if (Test-Path "HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\AWStats") {
		New-ItemProperty -Path "HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\AWStats" -Name "DisplayName" -Value "AWStats"
		New-ItemProperty -Path "HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\AWStats" -Name "UninstallString" -Value "C:\AWStats\uninstall.exe"
		New-ItemProperty -Path "HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\AWStats" -Name "Publisher" -Value "Laurent Destailleur"
		New-ItemProperty -Path "HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\AWStats" -Name "DisplayIcon" -Value "C:\AWStats\docs\awstats.ico"
		New-ItemProperty -Path "HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\AWStats" -Name "URLInfoAbout" -Value "http://awstats.sourceforge.net"
		New-ItemProperty -Path "HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\AWStats" -Name "Comments" -Value "copyright 2000/2015 Laurent Destailleur"
		New-ItemProperty -Path "HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\AWStats" -Name "HelpLink" -Value "http://awstats.sourceforge.net/docs/index.html"
		New-ItemProperty -Path "HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\AWStats" -Name "URLUpdateInfo" -Value "http://awstats.sourceforge.net"
	}
}


####################################################################################################################################################################################
function CopyAndFormatXMLfile($filePath)    # Function to copy file and format the file into standard XML format so we can process it consistently
{
	[xml]$xml = Get-Content -Path "$filePath"
	# Save the file
	$xml.Save((split-path $filePath) + "\cust_" + (split-path $filePath -Leaf));
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
Function HardenFilePermissions()            # Function to harden the file permissions on this Server to make the server secure
{
	Write-Host "`tChecking File Permissions on this machine" -ForegroundColor Cyan
	# Create a local group called "Administrators File Access" on the serverif it does not exist
	CreateLocalUserOrGroup "Administrators File Access" "Local Administrators File Access - SCP Harden IIS  ********* DO NOT DELETE *********" "Group"
	# Add the Local "Administrator" to the Local Group called "Administrators File Access" unless it is already a mmember of the group
	AddLocalUserToLocalGroup "Administrators File Access" "$dLangAdministratorName"
	# If the machine is a member of a domain then add the "Domain Admins" group to the Local Group called "Administrators File Access" unless it is already a mmember of the group
	if ($dDomainMember) {AddDomainUserToLocalGroup "Administrators File Access" "$dLangDomainAdminsGroup"}
	# Create the "C:\HostingSpaces" folder if it doesnt exist
	if(!(Test-Path $SolidCPhstSpace)) {
		(New-Item $SolidCPhstSpace -itemType directory) | Out-Null
	}
	# Take ownership of the "C:\HostingSpaces" directory and add the "Administrators" and "Administrators File Access" Groups (Full Access) and Remove inheritance
	if(Test-Path $SolidCPhstSpace) {
		# Take owenership of the "C:\HostingSpaces" folder with the Local "Administrators" Group
		if (((get-acl "$SolidCPhstSpace").owner) -ne "BUILTIN\Administrators") {
			Write-Host "`t Taking ownership of the HostingSpaces directory" -ForegroundColor Green
			Invoke-Command {takeown /A /F $SolidCPhstSpace /R /D Y} | Out-Null;
		}
		# Grant Full Access to the "c:\HostingSpaces" folder for the local group "Administrators"
		if (!(CheckAccessToFolder -Folder "$SolidCPhstSpace" -User "$dLangAdministratorGroup" -Local -AccessRights "FullControl" -Type "Allow")) {
			Write-Host "`t Granting access to the HostingSpaces directory for the Administrators group" -ForegroundColor Green
			SetAccessToFolderNoChk "$SolidCPhstSpace" "$dLangAdministratorGroup" "FullControl" "Allow";
		}
		# Grant Full Access to the "c:\HostingSpaces" folder for the local group "Administrators File Access"
		if (!(CheckAccessToFolder -Folder "$SolidCPhstSpace" -User "Administrators File Access" -Local -AccessRights "FullControl" -Type "Allow")) {
			Write-Host "`t Granting access to the HostingSpaces directory for the Administrators File Access group" -ForegroundColor Green
			SetAccessToFolderNoChk "$SolidCPhstSpace" "Administrators File Access" "FullControl" "Allow";
		}
		# Grant Full Access to the "c:\HostingSpaces" folder for the domain group "Domain Admins"
		if ($dDomainMember) {
			if (!(CheckAccessToFolder -Folder "$SolidCPhstSpace" -User "$dLangDomainAdminsGroup" -Domain -AccessRights "FullControl" -Type "Allow")) {
				Write-Host "`t Granting access to the HostingSpaces directory for the Domain Admins group" -ForegroundColor Green
				SetAccessToFolderNoChk "$SolidCPhstSpace" "$dLangDomainAdminsGroup" "FullControl" "Allow"
			}
		}
		# Disable inheritance on the "c:\HostingSpaces" folder
		if (CheckFolderInheritance -Folder "$SolidCPhstSpace") {
			Write-Host "`t Disabling Inheritance on the HostingSpaces directory" -ForegroundColor Green
			DisableFolderInheritance "$SolidCPhstSpace";
		}
		# Remove "Allow" permissions to the "C:\HostingSpaces" folder for the "Users", "CREATOR OWNER" and "SYSTEM" Groups
		if (CheckAccessToFolder -Folder "$SolidCPhstSpace" -User "$dLangUsersGroup" -Local -AccessRights "FullControl" -Type "Allow") {
			Write-Host "`t Removing permissions on the HostingSpaces directory for $dLangUsersGroup" -ForegroundColor Green
			RemoveAccessToFolder "$SolidCPhstSpace" "$dLangUsersGroup" "FullControl" "Allow";
		}
		if (CheckAccessToFolder -Folder "$SolidCPhstSpace" -User "$dLangCreatorOwnerName" -System -AccessRights "FullControl" -Type "Allow") {
			Write-Host "`t Removing permissions on the HostingSpaces directory for $dLangCreatorOwnerName" -ForegroundColor Green
			RemoveAccessToFolderNoChk "$SolidCPhstSpace" "$dLangCreatorOwnerName" "FullControl" "Allow";
		}
		if (CheckAccessToFolder -Folder "$SolidCPhstSpace" -User "$dLangSystemName" -System -AccessRights "FullControl" -Type "Allow") {
			Write-Host "`t Removing permissions on the HostingSpaces directory for $dLangSystemName" -ForegroundColor Green
			RemoveAccessToFolderNoChk "$SolidCPhstSpace" "$dLangSystemName" "FullControl" "Allow";
		}
		# Remove Special permissions to the "C:\HostingSpaces" folder for the "Administrator" User as it has access through the Administrators group
		if (CheckAccessToFolder -Folder "$SolidCPhstSpace" -User "$dLangAdministratorName" -Local -AccessRights "FullControl" -Type "Allow") {
			Write-Host "`t Removing permissions on the HostingSpaces directory for $dLangAdministratorName" -ForegroundColor Green
			RemoveAccessToFolderNoChk "$SolidCPhstSpace" "$dLangAdministratorName" "FullControl" "Allow";
		}
	}
	# Remove "Allow" permissions to the "C:\" drive to the "Users" Group
	if (CheckAccessToFolder -Folder "C:\" -User "$dLangUsersGroup" -Local -AccessRights "FullControl" -Type "Allow") {
		Write-Host "`t Removing access to the C Drive for the Users group" -ForegroundColor Green
		RemoveAccessToFolder "C:\" "$dLangUsersGroup" "FullControl" "Allow";
	}
	# Grant permissions for the "C:\" drive to the "Local Service" and "Network Service" Groups (Read & Execute, List Folder Contents, Read)
	if (!(CheckAccessToFolder -Folder "C:\" -User "$dLangLocalServiceName" -NTauth -AccessRights "ReadAndExecute" -Type "Allow")) {
		Write-Host "`t Allowing access to the C Drive for the Local Service group" -ForegroundColor Green
		SetAccessToFolderNoChk "C:\" "$dLangLocalServiceName" "ReadAndExecute" "Allow";
	}
	if (!(CheckAccessToFolder -Folder "C:\" -User "$dLangNetworkServiceName" -NTauth -AccessRights "ReadAndExecute" -Type "Allow")) {
		Write-Host "`t Allowing access to the C Drive for the Network Service group" -ForegroundColor Green
		SetAccessToFolderNoChk "C:\" "$dLangNetworkServiceName" "ReadAndExecute" "Allow";
	}
	# Grant Full Access for the "C:\" drive for the local group "Administrators File Access"
	if (!(CheckAccessToFolder -Folder "C:\" -User "Administrators File Access" -Local -AccessRights "FullControl" -Type "Allow")) {
		Write-Host "`t Allowing access to the C Drive for the Administrators File Access group" -ForegroundColor Green
		SetAccessToFolderNoChk "C:\" "Administrators File Access" "FullControl" "Allow";
	}
	# Grant Full Access for the "C:\" drive for the domain group "Domain Admins"
	if ($dDomainMember) {
		if (!(CheckAccessToFolder -Folder "C:\" -User "$dLangDomainAdminsGroup" -Domain -AccessRights "FullControl" -Type "Allow")) {
			Write-Host "`t Allowing access to the C Drive for the Domain Admins group" -ForegroundColor Green
			SetAccessToFolderNoChk "C:\" "$dLangDomainAdminsGroup" "FullControl" "Allow"
		}
	}
	# Take ownership for *.exe files in the "C:\Windows\System32" and "C:\Windows\SysWOW64" directories (EXE Files ONLY)
	#HardenFilePermissions_1  # Commented out as we are still testing the SQL Backups needing permissions to some files
	# Deny permissions for *.exe files in the "C:\Windows\System32" folder for the "IIS_IUSRS" Group (Deny All)
	#HardenFilePermissions_2  # Commented out as we are still testing the SQL Backups needing permissions to some files
	# Copy over the members of the Local "Administrators" group over to the Local "Administrators File Access" group
	if ((@(Compare-Object -ReferenceObject (GetMembersOfGroup "Administrators" "Name") -DifferenceObject (GetMembersOfGroup "Administrators File Access" "Name") -SyncWindow 0).length) -ne "0") {
		CopyGroupMembers "Administrators" "Administrators File Access" "Local"
	}
}


####################################################################################################################################################################################
Function HardenFilePermissions_1()          # Function to Take Ownership of *.exe files in the System32 and SysWOW64 folders
{
	# Take ownership for *.exe files in the "C:\Windows\System32" and "C:\Windows\SysWOW64" directories (EXE Files ONLY)
	Write-Host "`t Taking ownership for all executables files in the System32 directory" -ForegroundColor Green
	Invoke-Command {takeown /A /F c:\windows\system32\*.exe} | Out-Null;
	Write-Host "`t Taking ownership for all executables files in the SysWOW64 directory" -ForegroundColor Green
	Invoke-Command {takeown /A /F c:\windows\SysWOW64\*.exe} | Out-Null;
}


####################################################################################################################################################################################
Function HardenFilePermissions_2()          # Function to Deny permissions for *.exe files in the "C:\Windows\System32" folder for the "IIS_IUSRS" Group (Deny All)
{
	# Deny permissions for *.exe files in the "C:\Windows\System32" folder for the "IIS_IUSRS" Group (Deny All)
	Write-Host "`t Deny access for all executables files in the System32 directory for the IIS_IUSRS group" -ForegroundColor Green
	Invoke-Command {cacls c:\windows\system32\*.exe /E /D IIS_IUSRS} | Out-Null;
	# Deny permissions for *.exe files in the "C:\Windows\SysWOW64" folder for the "IIS_IUSRS" Group (Deny All)
	Write-Host "`t Deny access for all executables files in the SysWOW64 directory for the IIS_IUSRS group" -ForegroundColor Green
	Invoke-Command {cacls c:\windows\SysWOW64\*.exe /E /D IIS_IUSRS} | Out-Null;
}


####################################################################################################################################################################################
Function HardenFilePermissions_2_UNDO()     # Function to REMOVE the Deny permissions for *.exe files in the "C:\Windows\System32" folder for the "IIS_IUSRS" Group (Deny All)
{
	# This will REVOKE the Deny permissions for *.exe files in the "C:\Windows\System32" folder for the "IIS_IUSRS" Group (Deny All)
	Invoke-Command {icacls  c:\windows\system32\*.exe /remove:d IIS_IUSRS} | Out-Null;
	# This will REVOKE the Deny permissions for *.exe files in the "C:\Windows\SysWOW64" folder for the "IIS_IUSRS" Group (Deny All)
	Invoke-Command {icacls  c:\windows\SysWOW64\*.exe /remove:d IIS_IUSRS} | Out-Null;
}


####################################################################################################################################################################################
Function HardenDotNETforIIS()               # Function to harden .NET for IIS to make the server secure
{
	if ((Get-Service "W3SVC" -ErrorAction SilentlyContinue).Name) {
		Write-Host "`tChecking Microsoft .NET on this machine" -ForegroundColor Cyan
		$dNET2_32_dir = "C:\Windows\Microsoft.NET\Framework\v2.0.50727\CONFIG"
		if (!(Test-Path "$dNET2_32_dir\cust_web_mediumtrust.config")) {   # .NET v2 (32 Bit)
			Write-Host "`t Hardening IIS for .NET v2 (32 Bit)" -ForegroundColor Green
			Copy-Item "$dNET2_32_dir\web_mediumtrust.config" "$dNET2_32_dir\cust_web_mediumtrust.config"
			ModifyXML "$dNET2_32_dir\cust_web_mediumtrust.config" "Comment" "//SecurityClasses/SecurityClass[@Name='EnvironmentPermission']"
			ModifyXML "$dNET2_32_dir\cust_web_mediumtrust.config" "Comment" "//SecurityClasses/SecurityClass[@Name='PrintingPermission']"
			ModifyXML "$dNET2_32_dir\cust_web_mediumtrust.config" "Add" "//SecurityClasses" "SecurityClass" @( ("Name","OleDbPermission"), ("Description","System.Data.OleDb.OleDbPermission, System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089") )
			ModifyXML "$dNET2_32_dir\cust_web_mediumtrust.config" "Comment" "//NamedPermissionSets/PermissionSet/IPermission[@class='EnvironmentPermission']"
			ModifyXML "$dNET2_32_dir\cust_web_mediumtrust.config" "Comment" "//NamedPermissionSets/PermissionSet/IPermission[@class='PrintingPermission']"
			ModifyXML "$dNET2_32_dir\cust_web_mediumtrust.config" "Comment" "//NamedPermissionSets/PermissionSet/IPermission[@class='WebPermission']"
			ModifyXML "$dNET2_32_dir\cust_web_mediumtrust.config" "Add" "//PermissionSet[@Name='ASP.Net']" "IPermission" @( ("class","WebPermission"), ("version","1"), ("Unrestricted", "true") )
			ModifyXML "$dNET2_32_dir\cust_web_mediumtrust.config" "Add" "//PermissionSet[@Name='ASP.Net']" "IPermission" @( ("class","OleDbPermission"), ("version","1"), ("Unrestricted", "true") )
			ModifyXML "$dNET2_32_dir\web.config" "Update" "//location[@allowOverride='true']/@allowOverride" "false"
			ModifyXML "$dNET2_32_dir\web.config" "Add" "//system.web/securityPolicy" "trustLevel" @( ("name","Custom"), ("policyFile", "cust_web_mediumtrust.config") )
		}
		$dNET2_64_dir = "C:\Windows\Microsoft.NET\Framework64\v2.0.50727\CONFIG"
		if (!(Test-Path "$dNET2_64_dir\cust_web_mediumtrust.config")) { # .NET v2 (64 Bit)
			Write-Host "`t Hardening IIS for .NET v2 (64 Bit)" -ForegroundColor Green
			Copy-Item "$dNET2_64_dir\web_mediumtrust.config" "$dNET2_64_dir\cust_web_mediumtrust.config"
			ModifyXML "$dNET2_64_dir\cust_web_mediumtrust.config" "Comment" "//SecurityClasses/SecurityClass[@Name='EnvironmentPermission']"
			ModifyXML "$dNET2_64_dir\cust_web_mediumtrust.config" "Comment" "//SecurityClasses/SecurityClass[@Name='PrintingPermission']"
			ModifyXML "$dNET2_64_dir\cust_web_mediumtrust.config" "Add" "//SecurityClasses" "SecurityClass" @( ("Name","OleDbPermission"), ("Description","System.Data.OleDb.OleDbPermission, System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089") )
			ModifyXML "$dNET2_64_dir\cust_web_mediumtrust.config" "Comment" "//NamedPermissionSets/PermissionSet/IPermission[@class='EnvironmentPermission']"
			ModifyXML "$dNET2_64_dir\cust_web_mediumtrust.config" "Comment" "//NamedPermissionSets/PermissionSet/IPermission[@class='PrintingPermission']"
			ModifyXML "$dNET2_64_dir\cust_web_mediumtrust.config" "Comment" "//NamedPermissionSets/PermissionSet/IPermission[@class='WebPermission']"
			ModifyXML "$dNET2_64_dir\cust_web_mediumtrust.config" "Add" "//PermissionSet[@Name='ASP.Net']" "IPermission" @( ("class","WebPermission"), ("version","1"), ("Unrestricted", "true") )
			ModifyXML "$dNET2_64_dir\cust_web_mediumtrust.config" "Add" "//PermissionSet[@Name='ASP.Net']" "IPermission" @( ("class","OleDbPermission"), ("version","1"), ("Unrestricted", "true") )
			ModifyXML "$dNET2_64_dir\web.config" "Update" "//location[@allowOverride='true']/@allowOverride" "false"
			ModifyXML "$dNET2_64_dir\web.config" "Add" "//system.web/securityPolicy" "trustLevel" @( ("name","Custom"), ("policyFile", "cust_web_mediumtrust.config") )
		}
		$dNET4_32_dir = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\Config"
		if (!(Test-Path "$dNET4_32_dir\cust_web_mediumtrust.config")) {   # .NET v4 (32 Bit)
			Write-Host "`t Hardening IIS for .NET v4 (32 Bit)" -ForegroundColor Green
			Copy-Item "$dNET4_32_dir\web_mediumtrust.config" "$dNET4_32_dir\cust_web_mediumtrust.config"
			ModifyXML "$dNET4_32_dir\cust_web_mediumtrust.config" "Comment" "//SecurityClasses/SecurityClass[@Name='EnvironmentPermission']"
			ModifyXML "$dNET4_32_dir\cust_web_mediumtrust.config" "Comment" "//SecurityClasses/SecurityClass[@Name='PrintingPermission']"
			ModifyXML "$dNET4_32_dir\cust_web_mediumtrust.config" "Add" "//SecurityClasses" "SecurityClass" @( ("Name","OleDbPermission"), ("Description","System.Data.OleDb.OleDbPermission, System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089") )
			ModifyXML "$dNET4_32_dir\cust_web_mediumtrust.config" "Comment" "//NamedPermissionSets/PermissionSet/IPermission[@class='EnvironmentPermission']"
			ModifyXML "$dNET4_32_dir\cust_web_mediumtrust.config" "Comment" "//NamedPermissionSets/PermissionSet/IPermission[@class='PrintingPermission']"
			ModifyXML "$dNET4_32_dir\cust_web_mediumtrust.config" "Add" "//PermissionSet[@Name='ASP.Net']" "IPermission" @( ("class","OleDbPermission"), ("version","1"), ("Unrestricted", "true") )
			ModifyXML "$dNET4_32_dir\web.config" "Update" "//location[@allowOverride='true']/@allowOverride" "false"
			ModifyXML "$dNET4_32_dir\web.config" "Add" "//system.web/securityPolicy" "trustLevel" @( ("name","Custom"), ("policyFile", "cust_web_mediumtrust.config") )
		}
		$dNET4_64_dir = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Config"
		if (!(Test-Path "$dNET4_64_dir\cust_web_mediumtrust.config")) { # .NET v4 (64 Bit)
			Write-Host "`t Hardening IIS for .NET v4 (64 Bit)" -ForegroundColor Green
			Copy-Item "$dNET4_64_dir\web_mediumtrust.config" "$dNET4_64_dir\cust_web_mediumtrust.config"
			ModifyXML "$dNET4_64_dir\cust_web_mediumtrust.config" "Comment" "//SecurityClasses/SecurityClass[@Name='EnvironmentPermission']"
			ModifyXML "$dNET4_64_dir\cust_web_mediumtrust.config" "Comment" "//SecurityClasses/SecurityClass[@Name='PrintingPermission']"
			ModifyXML "$dNET4_64_dir\cust_web_mediumtrust.config" "Add" "//SecurityClasses" "SecurityClass" @( ("Name","OleDbPermission"), ("Description","System.Data.OleDb.OleDbPermission, System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089") )
			ModifyXML "$dNET4_64_dir\cust_web_mediumtrust.config" "Comment" "//NamedPermissionSets/PermissionSet/IPermission[@class='EnvironmentPermission']"
			ModifyXML "$dNET4_64_dir\cust_web_mediumtrust.config" "Comment" "//NamedPermissionSets/PermissionSet/IPermission[@class='PrintingPermission']"
			ModifyXML "$dNET4_64_dir\cust_web_mediumtrust.config" "Add" "//PermissionSet[@Name='ASP.Net']" "IPermission" @( ("class","OleDbPermission"), ("version","1"), ("Unrestricted", "true") )
			ModifyXML "$dNET4_64_dir\web.config" "Update" "//location[@allowOverride='true']/@allowOverride" "false"
			ModifyXML "$dNET4_64_dir\web.config" "Add" "//system.web/securityPolicy" "trustLevel" @( ("name","Custom"), ("policyFile", "cust_web_mediumtrust.config") )
		}
	}
}


####################################################################################################################################################################################
Function dEntSvrQuerySQL ($dQueryString)    # Function to query the Enterprise Server Database directly
{ # Usage - (dEntSvrQuerySQL "[Query String]").ValueRequired
  # Usage - (dEntSvrQuerySQL "SELECT [Username], [Password] FROM [Users] WHERE [Username] = 'SCPWebDavPeer'").Password
	$SqlConnection = New-Object System.Data.SqlClient.SqlConnection
	$SqlConnection.ConnectionString = (( ([xml](Get-Content ( "\\" + ($dSolidCPEnterpriseSvrIP) + "\c$\SolidCP\Enterprise Server\Web.config" ) )).configuration.connectionStrings.add.connectionString) -replace "localhost", $dSolidCPEnterpriseSvrIP)
	$SqlConnection.Open()
	$SqlCmd = New-Object System.Data.SqlClient.SqlCommand
	$SqlCmd.CommandText = $dQueryString
	$SqlCmd.Connection = $SqlConnection
	$SqlDataset = New-Object System.Data.DataSet
	(New-Object System.Data.sqlclient.sqlDataAdapter $SqlCmd).Fill($SqlDataset) | Out-Null
	$SqlConnection.Close()
	$SqlDataset.Tables
}


####################################################################################################################################################################################
Function InstallSolidCPInstaller()          # Function to download and install the latest SolidCP Installer from the SolidCP Website
{
	if (!(Test-Path "C:\Program Files (x86)\SolidCP Installer")) {
		# Create the SolidCP Directory in our Installation Files folder ready for downloading if it doesn't already exist
		if (!(Test-Path "C:\_Install Files\SolidCP")) { (md -Path "C:\_Install Files\SolidCP" -Force) | Out-Null }
		# Get the highest current SolidCP Version from the Installer XML File from the SolidCP Installation Website
		[string]$dSolidCPversion = ((([xml](New-Object System.Net.WebClient).DownloadString("http://installer.solidcp.com/Data/ProductReleasesFeed-1.0.xml")).components.component.releases.release).version | measure -Maximum).Maximum
		# Download the latest SolidCP Installer and save it to the "C:\_Install Files\SolidCP\" directory
		Write-Host "`tDownloading the latest SolidCP Installer" -ForegroundColor Cyan
		(New-Object System.Net.WebClient).DownloadFile("http://installer.solidcp.com/Files/$dSolidCPversion/SolidCPInstaller.msi", "C:\_Install Files\SolidCP\SolidCPInstaller-v$dSolidCPversion.msi")
		# Install the SolidCP Installer on this machine
		Write-Host "`t Installing the latest SolidCP Installer on this machine" -ForegroundColor Green
		((Start-Process -FilePath "C:\_Install Files\SolidCP\SolidCPInstaller-v$dSolidCPversion.msi" -ArgumentList "/qb IACCEPTSQLNCLILICENSETERMS=YES" -Wait -Passthru).ExitCode) | Out-Null
	}
}


####################################################################################################################################################################################
Function CheckSolidCPdomainUser($dComponent)                        # Function to check if the SolidCP user account exists in Active Directory
{# Usage - CheckSolidCPdomainUser Enterprise|Portal|Server|WebDav
	if ($dDomainMember) {
		if ($dComponent -eq "Enterprise") { # Check if the SolidCP Enterprise Server user account exists in Active Directory
			if ([adsi]::Exists("LDAP://CN=SCPEnterprise,CN=Users,$dRootDN")) {
				Write-Host "`n`t *************************************************" -ForegroundColor Yellow
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *       Oops, There seems to be a problem!      *" -ForegroundColor Yellow
				Write-Host "`t *        The `"SCPEnterprise`" user account       *" -ForegroundColor Yellow
				Write-Host "`t *       already exists in Active Directory      *" -ForegroundColor Yellow
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *    We can't install a new Enterprise Server   *" -ForegroundColor Yellow
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *  Either remove the user from Active Directory *" -ForegroundColor Yellow
				Write-Host "`t *           and run this script again           *" -ForegroundColor Yellow
				Write-Host "`t *          or manually configure a new          *" -ForegroundColor Yellow
				Write-Host "`t *           SolidCP Enterprise Server           *" -ForegroundColor Yellow
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *************************************************" -ForegroundColor Yellow
				dPressAnyKeyToExit
			}
		}elseif ($dComponent -eq "Portal") { # Check if the SolidCP Portal user account exists in Active Directory
			if ([adsi]::Exists("LDAP://CN=SCPPortal,CN=Users,$dRootDN")) {
				Write-Host "`n`t *************************************************" -ForegroundColor Yellow
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *       Oops, There seems to be a problem!      *" -ForegroundColor Yellow
				Write-Host "`t *          The `"SCPPortal`" user account         *" -ForegroundColor Yellow
				Write-Host "`t *       already exists in Active Directory      *" -ForegroundColor Yellow
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *      We can't install a new Portal Server     *" -ForegroundColor Yellow
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *  Either remove the user from Active Directory *" -ForegroundColor Yellow
				Write-Host "`t *           and run this script again           *" -ForegroundColor Yellow
				Write-Host "`t *          or manually configure a new          *" -ForegroundColor Yellow
				Write-Host "`t *             SolidCP Portal Server             *" -ForegroundColor Yellow
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *************************************************" -ForegroundColor Yellow
				dPressAnyKeyToExit
			}
		}elseif ($dComponent -eq "Server") { # Check if the SolidCP Server user account exists in Active Directory
			if ([adsi]::Exists("LDAP://CN=SCPServer-$env:computerName,CN=Users,$dRootDN")) {
				Write-Host "`n`t *************************************************" -ForegroundColor Yellow
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *       Oops, There seems to be a problem!      *" -ForegroundColor Yellow
				Write-Host "`t *   The `"SCPServer-computerName`" user account   *" -ForegroundColor Yellow
				Write-Host "`t *       already exists in Active Directory      *" -ForegroundColor Yellow
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *     Please check your Active Directory for    *" -ForegroundColor Yellow
				Write-Host "`t *     a user account in the following format    *" -ForegroundColor Yellow
				Write-Host "`t *           `"SCPServer-computerName`"            *" -ForegroundColor Yellow
				Write-Host "`t *           and run this script again           *" -ForegroundColor Yellow
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *************************************************" -ForegroundColor Yellow
				dPressAnyKeyToExit
			}
		}elseif ($dComponent -eq "WebDav") { # Check if the SolidCP WebDav user account exists in Active Directory
			if ([adsi]::Exists("LDAP://CN=SCPWebDav-$env:computerName,CN=Users,$dRootDN")) {
				Write-Host "`n`t *************************************************" -ForegroundColor Yellow
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *       Oops, There seems to be a problem!      *" -ForegroundColor Yellow
				Write-Host "`t *   The `"SCPWebDav-computerName`" user account   *" -ForegroundColor Yellow
				Write-Host "`t *       already exists in Active Directory      *" -ForegroundColor Yellow
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *     Please check your Active Directory for    *" -ForegroundColor Yellow
				Write-Host "`t *     a user account in the following format    *" -ForegroundColor Yellow
				Write-Host "`t *           `"SCPWebDav-computerName`"            *" -ForegroundColor Yellow
				Write-Host "`t *           and run this script again           *" -ForegroundColor Yellow
				Write-Host "`t *                                               *" -ForegroundColor Yellow
				Write-Host "`t *************************************************" -ForegroundColor Yellow
				dPressAnyKeyToExit
			}
		}
	}
}


####################################################################################################################################################################################
function GenerateIISMachineKey($dFilePath)                          # Function to generate and add the Machine Key used in IIS only if it doesn't already exist
{# Usage - GenerateIISMachineKey "C:\SolidCP\Server\web.config"
	if (Test-Path "$dFilePath") {
		if (!(CheckXMLnode $dFilePath "//configuration/system.web" "machineKey")) { # Add the Machine Key
			function BinaryToHex {
				[CmdLetBinding()]
				param($bytes)
				process {
					$builder = new-object System.Text.StringBuilder
					foreach ($b in $bytes) {
						$builder = $builder.AppendFormat([System.Globalization.CultureInfo]::InvariantCulture, "{0:X2}", $b)
					}
					$builder
				}
			}
			$decryptionObject = new-object System.Security.Cryptography.AesCryptoServiceProvider
			$decryptionObject.GenerateKey()
			$decryptionKey = BinaryToHex($decryptionObject.Key)
			$decryptionObject.Dispose()
			$validationObject = new-object System.Security.Cryptography.HMACSHA1
			$validationKey = BinaryToHex($validationObject.Key)
			$validationObject.Dispose()
			Write-Host "`t IIS Machine Key added to `"$dFilePath`"" -ForegroundColor Green
			(ModifyXML $dFilePath "Add" "//configuration/system.web" "machineKey" @( ("decryption","AES"), ("decryptionKey","$decryptionKey,IsolateApps"), ("validation","SHA1"), ("validationKey","$validationKey,IsolateApps") )) | Out-Null
		}
	}
}


####################################################################################################################################################################################
Function InstallSolidCPcomponentServer()                            # Function to install the SolidCP Installer and the SolidCP Server component
{
	InstallSolidCPInstaller
	InstallSolidCPcomponent -Server
	AddWebServerToDomainIISacco
}


####################################################################################################################################################################################
Function InstallSolidCPcomponent()  # Function to install the required SolidCP Component from the SolidCP Installer
{
	Param(
		[switch]$Enterprise, # Switch to install the Enterprise Server Component
		[switch]$Portal,     # Switch to install the Portal Component
		[switch]$Server,     # Switch to install the Server Component
		[switch]$WebDav,     # Switch to install the WebDav (Cloud Storage Portal) Component
		[string]$Pass,       # The SolidCP Component Password
		[switch]$NoIP        # Switch use ALL IP Addresses as the Binding in IIS for the chosen component
	)
	######################################################################################################################
	# Variables - these are all of the variables that can be passed to the SolidCP.SilentInstaller.exe file
	#
	# webip     # General - The IP Address that you want the SolidCP Component to be bound to in IIS
	# webport   # General - The Port Number that you want the SolidCP Component to be bound to in IIS
	# webdom    # General - The FQDN that you want the SolidCP Component to be bound to in IIS
	# uname     # General - The Service Name account that you want to use for your SolidCP deployment
	# upassw    # General - The Service Name password that you want to use for your SolidCP deployment
	# udomaim   # General - The Domain Name that you want to use for your SolidCP deployment - *** If this is used then the above username and password will be added to Active Directory ***
	# passw     # Server Password - Used on the Server and Enterprise Server Components ONLY
	# dbname    # Enterprise Server - Database Name
	# dbserver  # Enterprise Server - Database Server (IP or Hostname)
	# dbadmin   # Enterprise Server - Database Username
	# dbapassw  # Enterprise Server - Database Password
	# esurl     # Portal Server - Enterprise Server URL - Used on the Portal Installation Component ONLY
	# cname     # "Server"|"Enterprise Server"|"Portal"|"WebDavPortal" - This is the component you want to install
	#
	# Install the SolidCP Server component from the SolidCP.SilentInstaller.exe file with the corasponding arguments above
	######################################################################################################################
	if (Test-Path "C:\Program Files (x86)\SolidCP Installer") {
		if ($NoIP) {
			$dIPAddres = "*"
		}else{
			$dIPAddres = $dIPV4
		}
		# Import the PowerShell Web Administration Module
		Import-Module WebAdministration
		# Install the additional IIS Application Initialization to speed up website loading times
		(Add-WindowsFeature -Name Web-AppInit -ErrorAction SilentlyContinue) | Out-Null
		if ($Enterprise) {   # Install the SolidCP Enterprise Server component
			if (!(Test-Path "C:\SolidCP\Enterprise Server")) { # Check to see if it is already installed, if not then install it
				if ($Pass) {$dSolidCPportalPassword = $Pass} # set the password if it has been specified as part of the function
				if (!($dSolidCPportalPassword)) { # Ask the user to enter the password they want to for the WebDav component (for communication between the Server and the Enterprise Server)
					$dSolidCPportalPassword = Read-Host "Please enter the password you want to use for the SolidCP Portal Component (Web GUI ServerAdmin account)"
				}
				Write-Host "`tDownloading and Installing the SolidCP `"Enterprise Server`" component" -ForegroundColor Cyan
				if ($dDomainMember) { # If this server is joined to a domain then install the SolidCP Enterprise Server component Service Account in Active Directory
					$dRandomPasswordEntSvr = [guid]::NewGuid() # Generate a random password for the Domain SolidCP Enterprise Server Service Account
					(Start-Process -FilePath "C:\Program Files (x86)\SolidCP Installer\SolidCP.SilentInstaller.exe" -Argumentlist "/cname:`"Enterprise Server`" /passw:`"$dSolidCPportalPassword`" /webip:`"*`" /webport:`"9002`" /udomaim:`"$env:USERDNSDOMAIN`" /uname:`"SCPEnterprise`" /upassw:`"$dRandomPasswordEntSvr`"" -Wait -Passthru).ExitCode | Out-Null
				}else{ # If the server is not joined to a domain then install the SolidCP Enterprise Server component normally
					(Start-Process -FilePath "C:\Program Files (x86)\SolidCP Installer\SolidCP.SilentInstaller.exe" -Argumentlist "/cname:`"Enterprise Server`" /passw:`"$dSolidCPportalPassword`" /webip:`"*`" /webport:`"9002`"" -Wait -Passthru).ExitCode | Out-Null
				}
				# Set the correct values in the "bin\SolidCP.SchedulerService.exe.config" file so they match the ones in the "web.config" file for the Connection String and the CryptoKey
				ModifyXML "C:\SolidCP\Enterprise Server\bin\SolidCP.SchedulerService.exe.config" "Update" "//configuration/connectionStrings/add[@name='EnterpriseServer']/@connectionString" (ModifyXML "C:\SolidCP\Enterprise Server\web.config" "Get" "//configuration/connectionStrings/add[@name='EnterpriseServer']/@connectionString")
				ModifyXML "C:\SolidCP\Enterprise Server\bin\SolidCP.SchedulerService.exe.config" "Update" "//configuration/appSettings/add[@key='SolidCP.CryptoKey']/@value"                  (ModifyXML "C:\SolidCP\Enterprise Server\web.config" "Get" "//configuration/appSettings/add[@key='SolidCP.CryptoKey']/@value")
				# Set the Machine Key in IIS for the SolidCP Enterprise Server Component
				GenerateIISMachineKey "C:\SolidCP\Enterprise Server\web.config"
				# Make the required changes ONLY if the SolidCP Enterprise Server site exists
				if (Test-Path "IIS:\Sites\SolidCP Enterprise Server" -pathType container) {
					# Get the Application Pool for the SolidCP Enterprise Server ready to make changes
					$pool = Get-Item 'IIS:\AppPools\SolidCP Enterprise Server Pool'
					$pool.autoStart = 'True'                                # Enable Auto-Start
					$pool.startMode = 'AlwaysRunning'                       # Set Start Mode as Always Running
					$pool.processModel.idleTimeout = [TimeSpan]::Zero       # Set Idele TimeOut as 0 Minutes
					$pool.recycling.periodicRestart.time = [TimeSpan]::Zero # Set Regular time interval as 0 Minutes
					$pool | Set-Item                                        # Set all of the above for the SolidCP Enterprise Server Application Pool
					# Remove any schedules for the SolidCP Enterprise Server Application Pool and set the time to be 3.00am as it needs to be recycled once a day
					Clear-ItemProperty "IIS:\AppPools\SolidCP Enterprise Server Pool" -Name recycling.periodicRestart.schedule
					Set-ItemProperty "IIS:\AppPools\SolidCP Enterprise Server Pool" -Name recycling.periodicRestart.schedule -Value @{value='03:00:00'}
					# Disable Anonymous Authentication on the SolidCP Enterprise Server website
					#Get-WebConfiguration "system.webServer/security/authentication/anonymousAuthentication" -PSPath IIS:\ -Location "SolidCP Enterprise Server" -Value @{enabled="False"}
					# Disable Windows Authentication on the SolidCP Enterprise Server website
					Set-WebConfiguration system.webServer/security/authentication/windowsAuthentication -PSPath IIS:\ -Location "SolidCP Enterprise Server" -Value @{enabled="False"}
					# Set the "preloadEnabled" value to True to speed up loading on the SolidCP component
					if ((Get-ItemProperty "IIS:\Sites\SolidCP Enterprise Server" -Name applicationDefaults.preloadEnabled).Value -eq $false) {
						Set-ItemProperty "IIS:\Sites\SolidCP Enterprise Server" -Name applicationDefaults.preloadEnabled -Value $true
					}
				}
			}
		}elseif ($Portal) {  # Install the SolidCP Portal component
			if (!(Test-Path "C:\SolidCP\Portal")) { # Check to see if it is already installed, if not then install it
				if (!($dSolidCPEnterpriseSvrURL)) { # Chec to see if the Enterpeise Server URL is defined
					if ( (Test-Path "C:\SolidCP\Enterprise Server") -and (Get-WebBinding -Name 'SolidCP Enterprise Server') ) { # Check to see if the Enterprise Server is installed on this machine
						# Find the SolidCP Enterprise Server URL and port so that it can be used in the portal installation only if it
						if (((Get-WebBinding -Name 'SolidCP Enterprise Server').bindingInformation).substring(((Get-WebBinding -Name 'SolidCP Enterprise Server').bindingInformation).length -1,1) -eq "`:") {
							$dSolidCPEnterpriseSvrURL = ((Get-WebBinding -Name 'SolidCP Enterprise Server').protocol) + "://" + ((((Get-WebBinding -Name 'SolidCP Enterprise Server').bindingInformation).substring(0,((Get-WebBinding -Name 'SolidCP Enterprise Server').bindingInformation).length -1) -replace '[*]','127.0.0.1') -replace '[\[\]]','')
						}else{
							$dSolidCPEnterpriseSvrURL = ((Get-WebBinding -Name 'SolidCP Enterprise Server').protocol) + "://" + ((((Get-WebBinding -Name 'SolidCP Enterprise Server').bindingInformation) -replace '[*]','127.0.0.1') -replace '[\[\]]','')
						}
					}else{ # Otherwise ask for the Enterprise Server URL
							$dSolidCPEnterpriseSvrURL = Read-Host "Please enter the SolidCP Enterprise Server URL (i.e. http://127.0.0.1:9002)"
					}
				}
				Write-Host "`tDownloading and Installing the SolidCP `"Portal`" component" -ForegroundColor Cyan
				if ($dDomainMember) { # If this server is joined to a domain then install the SolidCP Portal component Service Account in Active Directory
					$dRandomPasswordPortal = [guid]::NewGuid() # Generate a random password for the Domain SolidCP Portal Service Account
					(Start-Process -FilePath "C:\Program Files (x86)\SolidCP Installer\SolidCP.SilentInstaller.exe" -Argumentlist "/cname:`"Portal`" /webip:`"$dSolidCPportalIPaddress`" /webport:`"$dSolidCPportalPortNumber`" /esurl:`"$dSolidCPEnterpriseSvrURL`" /udomaim:`"$env:USERDNSDOMAIN`" /uname:`"SCPPortal`" /upassw:`"$dRandomPasswordPortal`"" -Wait -Passthru).ExitCode | Out-Null
				}else{ # If the server is not joined to a domain then install the SolidCP Portal component normally
					(Start-Process -FilePath "C:\Program Files (x86)\SolidCP Installer\SolidCP.SilentInstaller.exe" -Argumentlist "/cname:`"Portal`" /webip:`"$dSolidCPportalIPaddress`" /webport:`"$dSolidCPportalPortNumber`" /esurl:`"$dSolidCPEnterpriseSvrURL`"" -Wait -Passthru).ExitCode | Out-Null
				}
				# Set the correct icon for the Desktop shortcut
				if ( (Test-Path "$env:USERPROFILE\Desktop\Login to SolidCP.url") -and (Test-Path "C:\SolidCP\Portal\favicon.ico") ) {
					($objShortcut = ((((New-Object -comObject Shell.Application).NameSpace(0X0)).ParseName("$env:USERPROFILE\Desktop\Login to SolidCP.url")).GetLink)) | Out-Null
					($objShortcut.SetIconLocation("C:\SolidCP\Portal\favicon.ico",0)) | Out-Null
					($objShortcut.Save()) | Out-Null
				}
				# Set the correct icon for the Start Menu shortcut
				if ( (Test-Path "$env:APPDATA\Microsoft\Windows\Start Menu\Programs\SolidCP Software\Login to SolidCP.url") -and (Test-Path "C:\SolidCP\Portal\favicon.ico") ) {
					($objShortcut = ((((New-Object -comObject Shell.Application).NameSpace(0X0)).ParseName("$env:APPDATA\Microsoft\Windows\Start Menu\Programs\SolidCP Software\Login to SolidCP.url")).GetLink)) | Out-Null
					($objShortcut.SetIconLocation("C:\SolidCP\Portal\favicon.ico",0)) | Out-Null
					($objShortcut.Save()) | Out-Null
				}
				# Set the Machine Key in IIS for the SolidCP Portal Component
				GenerateIISMachineKey "C:\SolidCP\Portal\web.config"
				# Make the required changes ONLY if the SolidCP Portal site exists
				if (Test-Path "IIS:\Sites\SolidCP Portal" -pathType container) {
					# Get the Application Pool for the SolidCP Portal ready to make changes
					$pool = Get-Item 'IIS:\AppPools\SolidCP Portal Pool'
					$pool.autoStart = 'True'                                # Enable Auto-Start
					$pool.startMode = 'AlwaysRunning'                       # Set Start Mode as Always Running
					$pool.processModel.idleTimeout = [TimeSpan]::Zero       # Set Idele TimeOut as 0 Minutes
					$pool.recycling.periodicRestart.time = [TimeSpan]::Zero # Set Regular time interval as 0 Minutes
					$pool | Set-Item                                        # Set all of the above for the SolidCP Portal Application Pool
					# Remove any schedules for the SolidCP Portal Application Pool and set the time to be 3.00am as it needs to be recycled once a day
					Clear-ItemProperty "IIS:\AppPools\SolidCP Portal Pool" -Name recycling.periodicRestart.schedule
					Set-ItemProperty "IIS:\AppPools\SolidCP Portal Pool" -Name recycling.periodicRestart.schedule -Value @{value='03:00:00'}
					# Disable Anonymous Authentication on the SolidCP Portal website
					#Get-WebConfiguration "system.webServer/security/authentication/anonymousAuthentication" -PSPath IIS:\ -Location "SolidCP Portal" -Value @{enabled="False"}
					# Disable Windows Authentication on the SolidCP Portal website
					Set-WebConfiguration system.webServer/security/authentication/windowsAuthentication -PSPath IIS:\ -Location "SolidCP Portal" -Value @{enabled="False"}
					# Set the "preloadEnabled" value to True to speed up loading on the SolidCP component
					if ((Get-ItemProperty "IIS:\Sites\SolidCP Portal" -Name applicationDefaults.preloadEnabled).Value -eq $false) {
						Set-ItemProperty "IIS:\Sites\SolidCP Portal" -Name applicationDefaults.preloadEnabled -Value $true
					}
				}
			}
		}elseif ($Server) {  # Install the SolidCP Server component
			if (!(Test-Path "C:\SolidCP\Server")) { # Check to see if it is already installed, if not then install it
				Write-Host "`tDownloading and Installing the SolidCP `"Server`" component" -ForegroundColor Cyan
				if ($Pass) {$dSolidCPserverPassword = $Pass} # set the password if it has been specified as part of the function
				if (!($dSolidCPserverPassword)) { # Ask the user to enter the password they want to for the WebDav component (for communication between the Server and the Enterprise Server)
					$dSolidCPserverPassword = Read-Host "Please enter the password you want to use for the SolidCP Server Component"
				}
				# Create a local group called "Administrators File Access" on the serverif it does not exist
				CreateLocalUserOrGroup "Administrators File Access" "Local Administrators File Access - SCP Harden IIS  ********* DO NOT DELETE *********" "Group"
				# Add the Local "Administrator" to the Local Group called "Administrators File Access" unless it is already a mmember of the group
				AddLocalUserToLocalGroup "Administrators File Access" "$dLangAdministratorName"
				if ($dDomainMember) { # If this server is joined to a domain then install the SolidCP Server component Service Account in Active Directory
					$dRandomPasswordServer = [guid]::NewGuid() # Generate a random password for the Domain SolidCP Server Service Account
					(Start-Process -FilePath "C:\Program Files (x86)\SolidCP Installer\SolidCP.SilentInstaller.exe" -Argumentlist "/cname:`"Server`" /passw:`"$dSolidCPserverPassword`" /webip:`"$dIPAddres`" /webport:`"9003`" /udomaim:`"$env:USERDNSDOMAIN`" /uname:`"SCPServer-$env:computerName`" /upassw:`"$dRandomPasswordServer`"" -Wait -Passthru).ExitCode | Out-Null
					(AddDomainUserToLocalGroup "Administrators File Access" "$dLangDomainAdminsGroup") | Out-Null      # Add the "Domain Admins" User to the new group called "Administrators File Access"
					(AddDomainUserToLocalGroup "Administrators File Access" "SCPServer-$env:computerName") | Out-Null  # Add the "SCPServer-[ComputerName]" User to the new group called "Administrators File Access"
					(AddDomainUserToDomainGroup "$dLangDomainAdminsGroup" "SCPServer-$env:computerName") | Out-Null    # Add the "SCPServer-[ComputerName]" User to the Domain Group called "Domain Admins"
					(AddDomainUserToDomainGroup "$dLangAdministratorGroup" "SCPServer-$env:computerName") | Out-Null   # Add the "SCPServer-[ComputerName]" User to the Domain Group called "Administrators"
					## May need to add the following groups in Active Directory due to the File Permissions that are set to harden the server
					#(AddDomainUserToDomainGroup "IIS_IUSRS" "SCPServer-$env:computerName") | Out-Null    # Add the "SCPServer-[ComputerName]" User to the Domain Group called "IIS_IUSRS"
				}else{ # If the server is not joined to a domain then install the SolidCP Server component normally
					(Start-Process -FilePath "C:\Program Files (x86)\SolidCP Installer\SolidCP.SilentInstaller.exe" -Argumentlist "/cname:`"Server`" /passw:`"$dSolidCPserverPassword`" /webip:`"$dIPAddres`" /webport:`"9003`"" -Wait -Passthru).ExitCode | Out-Null
					(AddLocalUserToLocalGroup "Administrators File Access" "SCPServer") | Out-Null                     # Add the "SCPServer" User to the new group called "Administrators File Access"
				}
				# Add the firewall rule to open Port 9003 for the SolidCP Enterprise Server only if the Enterprise Server IP is set also add RDP Allowed IP's
				if ($dSCPfirewallRDPaccess) { $dSolidCPServerPort9003ips = $dSCPfirewallRDPaccess.replace(' ','').split(';') } # Build the array of IP Addresses
				if ($dSolidCPEnterpriseSvrIP) { $dSolidCPServerPort9003ips += "$dSolidCPEnterpriseSvrIP" }                     # Add the Enterprise Server IP Address to the array
				if ($dSolidCPServerPort9003ips) { # If the array is not empty then create and enable the firewall rule with the correct IPs
					if ((Get-NetFirewallRule | where DisplayName -EQ "SolidCP Server").DisplayName -eq "SolidCP Server") {
						Write-Host "`t Restricting port 9003 for SolidCP Server on the Firewall to the Enterprise Server IP Address" -ForegroundColor Green
						(Set-NetFirewallRule -DisplayName "SolidCP Server" -RemoteAddress $dSolidCPServerPort9003ips) | Out-Null
						Write-Host "`t Firewall rule modified successfully" -ForegroundColor Green
					}else {
						Write-Host "`t Opening port 9003 for SolidCP Server on the Firewall" -ForegroundColor Green
						(New-NetFirewallRule -DisplayName "SolidCP Server" -Direction Inbound –LocalPort "9003" -Protocol TCP -Action Allow -RemoteAddress $dSolidCPServerPort9003ips -WarningAction SilentlyContinue) | Out-Null
						Write-Host "`t Firewall rule added successfully" -ForegroundColor Green
					}
				}
				# Set the Machine Key in IIS for the SolidCP Server Component
				GenerateIISMachineKey "C:\SolidCP\Server\web.config"
				# Make the required changes ONLY if the SolidCP Server site exists
				if (Test-Path "IIS:\Sites\SolidCP Server" -pathType container) {
					# Get the Application Pool for the SolidCP Server ready to make changes
					$pool = Get-Item 'IIS:\AppPools\SolidCP Server Pool'
					$pool.autoStart = 'True'                                # Enable Auto-Start
					$pool.startMode = 'AlwaysRunning'                       # Set Start Mode as Always Running
					$pool.processModel.idleTimeout = [TimeSpan]::Zero       # Set Idele TimeOut as 0 Minutes
					$pool.recycling.periodicRestart.time = [TimeSpan]::Zero # Set Regular time interval as 0 Minutes
					$pool | Set-Item                                        # Set all of the above for the SolidCP Server Application Pool
					# Remove any schedules for the SolidCP Server Application Pool and set the time to be 3.00am as it needs to be recycled once a day
					Clear-ItemProperty "IIS:\AppPools\SolidCP Server Pool" -Name recycling.periodicRestart.schedule
					Set-ItemProperty "IIS:\AppPools\SolidCP Server Pool" -Name recycling.periodicRestart.schedule -Value @{value='03:00:00'}
					# Disable Anonymous Authentication on the SolidCP Server website
					#Get-WebConfiguration "system.webServer/security/authentication/anonymousAuthentication" -PSPath IIS:\ -Location "SolidCP Server" -Value @{enabled="False"}
					# Disable Windows Authentication on the SolidCP Server website
					Set-WebConfiguration system.webServer/security/authentication/windowsAuthentication -PSPath IIS:\ -Location "SolidCP Server" -Value @{enabled="False"}
					# Set the "preloadEnabled" value to True to speed up loading on the SolidCP component
					if ((Get-ItemProperty "IIS:\Sites\SolidCP Server" -Name applicationDefaults.preloadEnabled).Value -eq $false) {
						Set-ItemProperty "IIS:\Sites\SolidCP Server" -Name applicationDefaults.preloadEnabled -Value $true
					}
				}
				# if SQL has been installed then make sure the SQL Server Service Account has full permissions on the SCPServer's AppData Temp folder
				if ((Test-Path "C:\Users\SCPServer-$env:computerName\AppData\Local") -and ([bool](Get-Service "MSSQL**").Status)) {
					if (!([bool]((Get-Acl -Path "C:\Users\SCPServer-$env:computerName\AppData\Local\Temp").Access | WHERE {(($_.IdentityReference -eq "$((Get-WmiObject Win32_Service -ComputerName localhost -Filter "name Like 'MSSQL%'").StartName)") -and ($_.FileSystemRights -eq "FullControl"))}))) {
						Write-Host "`t Fixing the Microsoft SQL Server Backup Permissions" -ForegroundColor Green
						$acl = Get-Acl -Path "C:\Users\SCPServer-$env:computerName\AppData\Local\Temp"
						$acl.SetAccessRule($(New-Object -TypeName System.Security.AccessControl.FileSystemAccessRule -ArgumentList "$((Get-WmiObject Win32_Service -ComputerName localhost -Filter "name Like 'MSSQL%'").StartName)", 'FullControl', 'ContainerInherit, ObjectInherit', 'None', 'Allow'))
						$acl | Set-Acl -Path "C:\Users\SCPServer-$env:computerName\AppData\Local\Temp"
					}
				}
			}
		}elseif ($WebDav) {  # Install the SolidCP WebDav (Cloud Storage Portal) component
			if (!(Test-Path "C:\SolidCP\Cloud Storage Portal")) { # Check to see if it is already installed, if not then install it
				if (!($dSolidCPEnterpriseSvrURL)) { # Check if the Enterprise Server URL has been set, if not ask for it
					$dSolidCPEnterpriseSvrURL = Read-Host "`tPlease enter the SolidCP Enterprise Server URL (i.e. http://PublicIP:9002)"
				}
				if (!($dSolidCPEnterpriseSvrIP)) { # Check if the Enterprise Server IP has been set, if not ask for it
					$dSolidCPEnterpriseSvrIP = Read-Host "`tPlease enter the SolidCP Enterprise Server IP Address ONLY (i.e. 192.168.1.1)"
				}
				if (!($dWebDavStorageHostName)) { # As the user for to choose the FQDN for the WebDav Server
					do {
						Write-Host "`n`tPlease select the FQDN Deployment for the Cloud Storage Portal" -ForegroundColor Cyan
						Write-Host "`t  A. Use `"$dFQDNthisMachine`"`n`t  B. Enter my own FQDN" -ForegroundColor Cyan
						$choiceWebDavFQDN = Read-Host "`tEnter Option From Above Menu"
						$ok = $choiceWebDavFQDN -match '^[a-b]+$'
						if ( -not $ok) { Write-Host "`t Invalid selection" -ForegroundColor Yellow ; start-Sleep -Seconds 2 }
					} until ( $ok )
					switch -Regex ( $choiceWebDavFQDN ) {
						"A" {$script:dWebDavStorageHostName = $dFQDNthisMachine}
						"B" {do { $script:dWebDavStorageHostName = Read-Host "`tPlease enter the required FQDN for the Cloud Storage Portal" } until (!([string]::IsNullOrEmpty($dWebDavStorageHostName)))}
					}
				}
				Write-Host "`tDownloading and Installing the SolidCP `"Cloud Storage Portal`" component" -ForegroundColor Cyan
				# Change the bindings on the default website to Port 8080 if the required WebDav Port is 80
				if ($dWebDavStoragePortNumber -eq "80") {
					if ( (Get-WebBinding -Name 'Default Web Site').bindingInformation -eq "*:80:" ) {
						Set-WebBinding -Name 'Default Web Site' -BindingInformation "*:80:" -PropertyName Port -Value 8080
					}
				}
				if ($dDomainMember) { # If this server is joined to a domain then install the SolidCP WebDav (Cloud Storage Portal) component Service Account in Active Directory
					$dRandomPasswordWebDav = [guid]::NewGuid() # Generate a random password for the Domain SolidCP WebDav Service Account
					(Start-Process -FilePath "C:\Program Files (x86)\SolidCP Installer\SolidCP.SilentInstaller.exe" -Argumentlist "/cname:`"WebDavPortal`" /webip:`"*`" /webport:`"$dWebDavStoragePortNumber`" /esurl:`"$dSolidCPEnterpriseSvrURL`" /udomaim:`"$env:USERDNSDOMAIN`" /uname:`"SCPWebDav-$env:computerName`" /upassw:`"$dRandomPasswordWebDav`"" -Wait -Passthru).ExitCode | Out-Null
					(AddDomainUserToLocalGroup "Administrators File Access" "SCPWebDav-$env:computerName") | Out-Null  # Add the "SCPWebDav-[ComputerName]" User to the new group called "Administrators File Access"
					(AddDomainUserToDomainGroup "$dLangDomainAdminsGroup" "SCPWebDav-$env:computerName") | Out-Null    # Add the "SCPWebDav-[ComputerName]" User to the group "Domain Admins"
				}else{ # If the server is not joined to a domain then install the SolidCP SCPWebDav (Cloud Storage Portal) component normally
					(Start-Process -FilePath "C:\Program Files (x86)\SolidCP Installer\SolidCP.SilentInstaller.exe" -Argumentlist "/cname:`"WebDavPortal`" /webip:`"*`" /webport:`"$dWebDavStoragePortNumber`" /esurl:`"$dSolidCPEnterpriseSvrURL`"" -Wait -Passthru).ExitCode | Out-Null
					(AddLocalUserToLocalGroup "Administrators File Access" "SCPWebDav") | Out-Null                     # Add the "SCPWebDav" User to the new group called "Administrators File Access"
				}
				# Check if the SolidCP Cloud Storage Portal web.config file is there, if it is then modify it as below
				if (Test-Path "C:\SolidCP\Cloud Storage Portal\Web.config") {
					# Set the values in the web.config file for the Cloud Storage Portal (WebDav)
					[xml]$myXML = Get-Content "C:\SolidCP\Cloud Storage Portal\Web.config"
					($myXML.configuration.appSettings.add | ? { $_.key -eq "SolidCP.CryptoKey" }).value = (([xml](Get-Content ( "\\" + ($dSolidCPEnterpriseSvrIP) + "\c$\SolidCP\Enterprise Server\Web.config" ) )).configuration.appSettings.add | ? { $_.key -eq "SolidCP.CryptoKey" }).value
					$myXML.configuration.webDavExplorerConfigurationSettings.userDomain.value= $env:USERDNSDOMAIN.ToLower()
					$myXML.configuration.webDavExplorerConfigurationSettings.webdavRoot.value = "http://" + $dWebDavStorageHostName + "/"
					$myXML.configuration.webDavExplorerConfigurationSettings.enterpriseServer.url = $dSolidCPEnterpriseSvrURL
					$myXML.configuration.webDavExplorerConfigurationSettings.SolidCPConstantUser.login = "serveradmin"
					$myXML.configuration.webDavExplorerConfigurationSettings.SolidCPConstantUser.password = (dEntSvrQuerySQL "SELECT [Username], [Password] FROM [Users] WHERE [Username] = 'serveradmin'").Password
					$myXML.Save("C:\SolidCP\Cloud Storage Portal\Web.config")
				}
				# Set the Machine Key in IIS for the SolidCP Cloud Storage Portal Component
				GenerateIISMachineKey "C:\SolidCP\Cloud Storage Portal\web.config"
				# Make the required changes ONLY if the SolidCP Cloud Storage Portal site exists
				if (Test-Path "IIS:\Sites\SolidCP Cloud Storage Portal" -pathType container) {
					# Get the Application Pool for the SolidCP Cloud Storage Portal ready to make changes
					$pool = Get-Item 'IIS:\AppPools\SolidCP Cloud Storage Portal Pool'
					$pool.autoStart = 'True'                                # Enable Auto-Start
					$pool.startMode = 'AlwaysRunning'                       # Set Start Mode as Always Running
					$pool.processModel.idleTimeout = [TimeSpan]::Zero       # Set Idele TimeOut as 0 Minutes
					$pool.recycling.periodicRestart.time = [TimeSpan]::Zero # Set Regular time interval as 0 Minutes
					$pool | Set-Item                                        # Set all of the above for the SolidCP Cloud Storage Portal Application Pool
					# Remove any schedules for the SolidCP Cloud Storage Portal Application Pool and set the time to be 3.00am as it needs to be recycled once a day
					Clear-ItemProperty "IIS:\AppPools\SolidCP Cloud Storage Portal Pool" -Name recycling.periodicRestart.schedule
					Set-ItemProperty "IIS:\AppPools\SolidCP Cloud Storage Portal Pool" -Name recycling.periodicRestart.schedule -Value @{value='03:00:00'}
					# Disable Anonymous Authentication on the SolidCP Cloud Storage Portal website
					#Get-WebConfiguration "system.webServer/security/authentication/anonymousAuthentication" -PSPath IIS:\ -Location "SolidCP Cloud Storage Portal" -Value @{enabled="False"}
					# Disable Windows Authentication on the SolidCP Cloud Storage Portal website
					Set-WebConfiguration system.webServer/security/authentication/windowsAuthentication -PSPath IIS:\ -Location "SolidCP Cloud Storage Portal" -Value @{enabled="False"}
					# Set the "preloadEnabled" value to True to speed up loading on the SolidCP component
					if ((Get-ItemProperty "IIS:\Sites\SolidCP Cloud Storage Portal" -Name applicationDefaults.preloadEnabled).Value -eq $false) {
						Set-ItemProperty "IIS:\Sites\SolidCP Cloud Storage Portal" -Name applicationDefaults.preloadEnabled -Value $true
					}
				}
			}
		}
	}
}


####################################################################################################################################################################################
####################################################################################################################################################################################
# Run the SolidCP Installation Menu as long as the logged in user is member of the Local "Administrators" group of the "Domain Admins" group
if (!($dDomainMember)) { # Check to see if the machine is NOT joined to a domain
	if (CheckGroupMembers "$dLangAdministratorGroup" "$dLoggedInUserName" "Local") { # Run the SOlidCP Menu if the logged in user is a Local Administrator
		Write-Host "`n`t This machine is NOT Joined to domain and you are logged in as Local Administrator Account" -ForegroundColor Green
		Write-Host "`t The SolidCP menu is being loaded" -ForegroundColor Green
		SolidCPmenu
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
		Write-Host "`t The SolidCP menu is being loaded" -ForegroundColor Green
		SolidCPmenu
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

