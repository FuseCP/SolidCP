# Created by Trevor Robinson
# SolidCP 2018
Import-Module activedirectory

#This script is designed to setup dSHeuristics for the AD Domain.

# This is the OU in the format of OU=Customers,DC=scp1,DC=local
$CustomerOU = "OU=Customers,DC=scp1,DC=local"

# Get SubOU
$SubOUs = Get-ADOrganizationalUnit -SearchBase $CustomerOU -Filter * -SearchScope OneLevel

######################## Do not edit below ########################

#Some Varibles we need multiple times
$ADDomain = Get-ADDomain
$ADDistinguishedName = $ADDomain.DistinguishedName
$AuthUsers = [System.Security.Principal.SecurityIdentifier] ("S-1-5-11") 
$ACLtypeAllow = [System.Security.AccessControl.AccessControlType]::Allow
$ACLinheritanceAll = [System.DirectoryServices.ActiveDirectorySecurityInheritance]::SelfAndChildren
$ACLinheritanceNone = [System.DirectoryServices.ActiveDirectorySecurityInheritance]::None
$ACLRightsListObject = [System.DirectoryServices.ActiveDirectoryRights]::ListObject
$ACLRightsListChildren = [System.DirectoryServices.ActiveDirectoryRights]::ListChildren
$ACLRightsGenericRead = [System.DirectoryServices.ActiveDirectoryRights]::GenericRead


# Doing the required checks to see List Object Mode is enabled on AD
$dsconfig = "CN=Directory Service,CN=Windows NT,CN=Services,CN=Configuration," + $ADDistinguishedName
$dSHeuristics = (Get-ADObject $dsconfig -Properties dSHeuristics).dSHeuristics
IF (!($dSHeuristics)) {
        Write-Host "Setting the dSHeuristics value to 001"
        Set-ADObject $dsconfig -Replace @{dSHeuristics="001"}
    } elseif ($dSHeuristics -eq "001") {
        Write-Host "dSHeuristics has already been set to 001"
    } else {
        Write-Host "The value for dSHeuristics is currently set to $dSHeuristics"
        Write-Host "WE are not going to adjust this setting to enable List Object Mode"
        Write-Host "Please check this value before proceeding"
        Write-Host ""
        [void](Read-Host 'Press Enter to continue…')

    }

#Remove Authenticated Users from "Pre-Windows 2000 Compatible Access"
Write-Host 'Checking Authenticated Users are part of "Pre-Windows 2000 Compatible Access" group'
$Pre2000group = Get-ADGroup ("S-1-5-32-554")
$AuthUsersGroup = Get-ADGroupMember -Identity $Pre2000group | Where-Object {$_.SID -eq "S-1-5-11"}
If($AuthUsersGroup) {
    Write-Host '.. Removing'
    # Using WinNT due to limits on the Authenticated Users
    $Pre2000groupadsi = [ADSI]("WinNT://" + $ADDomain.DNSRoot + "/" + $Pre2000group.Name)
    $Pre2000groupadsi.remove("WinNT://NT Authority/" + $AuthUsersGroup.Name)
} else {
    Write-Host '.. It has already been removed'
}




Write-Host "Going through the Hosted Orgs"
#Setting the values on the OUs
Foreach ($SubOU in $SubOUs[1..$($SubOUs.count)]){
    $SubOUname = $SubOU.Name
    $SubOUDistName = $SubOU.DistinguishedName
    Write-Host ".OU: $SubOUDistName"
    
    #Set ACL for the org OU
    ## Removal Rules
    $SubOUpath = "AD:\" + $SubOUDistName
    $SubOUacl = Get-ACL -Path $SubOUpath
    $SubOUace1 = New-Object System.DirectoryServices.ActiveDirectoryAccessRule $AuthUsers,$ACLRightsListObject,$ACLtypeAllow,$ACLinheritanceNone
    $SubOUacl.RemoveAccessRule($SubOUace1) >$null 2>&1
    $SubOUace2 = New-Object System.DirectoryServices.ActiveDirectoryAccessRule $AuthUsers,$ACLRightsListChildren,$ACLtypeAllow,$ACLinheritanceNone
    $SubOUacl.RemoveAccessRule($SubOUace2) >$null 2>&1
    ## Add Rules
    $SubOUID = [System.Security.Principal.NTAccount]$SubOUName
    $SubOUace3 = New-Object System.DirectoryServices.ActiveDirectoryAccessRule $SubOUID,$ACLRightsListObject,$ACLtypeAllow,$ACLinheritanceAll
    $SubOUacl.AddAccessRule($SubOUace3)  >$null 2>&1
    $SubOUace4 = New-Object System.DirectoryServices.ActiveDirectoryAccessRule $SubOUID,$ACLRightsListChildren,$ACLtypeAllow,$ACLinheritanceAll
    $SubOUacl.AddAccessRule($SubOUace4) >$null 2>&1
    Set-Acl -Path $SubOUpath -AclObject $SubOUacl

    #We Will check for any SubOU. This is due to RDS creating them and we want them to be checked.
    $SubOUs2 = Get-ADOrganizationalUnit -SearchBase $SubOUDistName -Filter * -SearchScope OneLevel
    Foreach ($SubOU2 in $SubOUs2){
        $SubOU2DistName = $SubOU2.DistinguishedName
        Write-Host ".. SubOU found $SubOU2DistName"
        #Set ACL for the org OU
        ## Removal
        $SubOU2path = "AD:\" + $SubOU2DistName
        $SubOU2acl = Get-ACL -Path $SubOU2path
        $SubOU2ace1 = New-Object System.DirectoryServices.ActiveDirectoryAccessRule $AuthUsers,$ACLRightsListObject,$ACLtypeAllow,$ACLinheritanceNone
        $SubOU2acl.RemoveAccessRule($SubOU2ace1) >$null 2>&1
        $SubOU2ace2 = New-Object System.DirectoryServices.ActiveDirectoryAccessRule $AuthUsers,$ACLRightsListChildren,$ACLtypeAllow,$ACLinheritanceNone
        $SubOU2acl.RemoveAccessRule($SubOU2ace2) >$null 2>&1
        Set-Acl -Path $SubOU2path -AclObject $SubOU2acl
    }

    #Finally now we have the OU security set we need to check for computer objects to give them read rights.
    $SubOUComputers = Get-ADComputer -SearchBase $SubOU.DistinguishedName  -Filter *
    Foreach ($Computer in $SubOUComputers){
        $ComputerDistName = $Computer.DistinguishedName
        Write-Host ".. SubOU Computer Found $ComputerDistName"
        #Set ACL for the org OU
        ## Removal
        $ComputerName = $Computer.Name + "$"
        $ComputerID = [System.Security.Principal.NTAccount]$ComputerName
        $Computeracl = Get-ACL -Path $SubOUpath
        $Computerace = New-Object System.DirectoryServices.ActiveDirectoryAccessRule $ComputerID,$ACLRightsGenericRead,$ACLtypeAllow,$ACLinheritanceAll
        $Computeracl.AddAccessRule($Computerace) >$null 2>&1
        Set-Acl -Path $SubOUpath -AclObject $Computeracl
    }




}