Import-Module WebAdministration

cd ..
$path = pwd
cd Tools

New-Website -Name "SolidCP Server" -Port 9003 -PhysicalPath "$path\Sources\SolidCP.Server" -ErrorAction SilentlyContinue
New-Website -Name "SolidCP Enterprise Server" -Port 9002 -PhysicalPath "$path\Sources\SolidCP.EnterpriseServer" -ErrorAction SilentlyContinue
New-Website -Name "SolidCP Portal" -Port 9001 -PhysicalPath "$path\Sources\SolidCP.WebPortal" -ErrorAction SilentlyContinue
	
	
	