Write-Host -ForegroundColor Green "
Ensure that you have created the EnterpriseServer Database
by executing test-createDB.bat and compiled SolidCP by
executing build-test.bat .

Configuration:

SolidCP Portal:

URL: http://localhost:9001
Login: serveradmin
Password: 1234


SolidCP Enterprise Server:
URL: http://127.0.0.1:9002
Database Login: SolidCP
Database Password: Password12


SolidCP Server:
URL: http://localhost:9003
Password: Password12
"

start http://localhost:9001

Read-Host "Press a key"


	
	
	