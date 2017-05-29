~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
SolidCP Readme - SolidCP-Auto-Upgrade.ps1
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

The SolidCP Auto Upgrade script needs to be run from your 
SolidCP Enterprise Server.
The script assumes that your SolidCP Portal Server is also 
installed on the same server as your Enterprise Server.
You MUST ensure that you are able to access the UNC share 
on each remote server that forms part of your deployment 
BEFORE running the script.

There is an option to run the test to make sure your SolidCP 
Enterprise Server can reach the UNC Path on each server listed 
in your SolidCP Enterprise Server Database.


~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
Upgrade Process
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

The SolidCP Enterprise Server will be backed up along with 
the Database.
Once this is complete it will upgrade your SolidCP Enterprise 
Server and Database as well as adding the additional keys 
into your web.config file for any new features.

Then the SolidCP Portal will be backed up.
Once this is complete it will upgrade your SolidCP Portal and 
add any additional keys into your web.config file for any new
features.

The script will read through the database on the SolidCP 
Enterprise Server and all servers that form part of your 
deployment will be updated.
Each SolidCP Server will be backed up (files will be saved on 
the Enterprise Server), the SolidCP server will be upgraded 
and any new keys will be added into the web.config for the 
server, then it will move onto the next SolidCP server in the 
database and repeat the backup and upgrade process until all 
servers have been updated.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
Handy Notes
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

The script can be used as an easy way to test a BETA release 
on your deployment, if things do not work as expected you can 
run the script again to revert back to the latest stable 
release.
If things did not go as expected you can use the files from 
the backup to restore each server individually to a state before 
running the upgrade script.
Don't forget you can also restore the database to the previous 
version as well.

You now have the ability to exclude servers from the SQL query
as well as include additional servers.
You need to create 2 files in the same directory as the script
and call them:-
SolidCP-Auto-Upgrade-Exclude-Servers.txt
SolidCP-Auto-Upgrade-Include-Servers.txt

IP Addresses whould be on thier own line, no commas or quotes
are required as delimiters as it is read in as an array
(i.e. each line is an item in the array)
