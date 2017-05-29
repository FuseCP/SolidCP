~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
SolidCP Readme - SolidCP-Configuration.ps1
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

The SolidCP Configuration script has been designed to speed 
up the installation process as well as automate as much as 
possible to ensure mistakes are not made when initially 
setting up your environment or adding new servers to it.

The script has been tested on Windows Server 2012 R2 
although most of it was written for Server 2008 R2 machines.
It should work on any version of server running PowerShell 
v4, but preferably PowerShell v4.


~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
Assumptions
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

We have presumed that you will be using the script to setup 
a new SolidCP deployment on new machines that are NOT in 
production, but it can be used to add additional machines 
to your SolidCP deployment.
Networking is configured on the machine and you are able to 
get on the internet.
ALL Windows Updates need to be done BEFORE running the 
script.


~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
Before you begin
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Once you have downloaded it you will need to unzip it.
Right click on the extracted and click Properties then click 
the Unblock button at the bottom of the General Tab.

When this has been done you will then need to define the 
variables for your deployment in the file, to do this right 
click on the file and select Edit, this will open the file 
in the PowerShell ISE editor.
Scroll down to the section labelled Editable features are 
below this line and make sure you have set the variables as 
per your required deployment, while you are doing this it 
would be worth setting all of them so the file is setup 
ready to use for the deployment of your servers.
Save the file and exit the PowerShell ISE editor and we are 
now ready to use it to deploy your servers.

There are some configuration options that can be set at the 
top of the script, please edit the script and set them as 
required for your deployment.


~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
Domain Controllers
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

If you are planning on running Enterprise Services (such as 
Exchange or SharePoint) you will need to have Domain 
Controllers setup and configured for your SolidCP deployment.

If you plan on adding Enterprise Services at a later date we 
would recommend setting up your Domain Controllers now as 
this will save you time and hassle when you do come to deploy 
them.

If you are using Domain Controllers ensure ALL servers are 
joined to your Active Directory Domain EXCEPT for your 
Authoritative DNS Servers


~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
Running the script
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

If your machine is joined to a domain ensure you are logged 
in with a Domain Administrative account before trying to run 
the script, if you are no the script will point this out to 
you and not let you continue.

To run the script simply right click on the 
SolidCP-Configuration.ps1 and choose "Run with PowerShell"
The script will do some checks to ensure you are logged in 
with sufficient permissions and then present a menu to you.
Simply select the option from the menu that you wish the 
server to perform, answer any questions or prompts, then sit 
back and let the script install everything on the server for 
you.

Once the script has completed reboot the machine and install 
any additional updates that are required because of the 
components or features that have been installed.


~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
Problems or Errors
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

The script has been written to include as much information 
as possible in relation to error reporting, if you do 
encounter any errors or issues we welcome your feedback so 
we can look into them for you and resolve them as quickly as 
possible.
The script will also produce a small debug section which we 
may ask you to email into us so we can fix the issue or let 
you know what you have done wrong.
Issues should be emailed into support@solidcp.com so our 
developers can troubleshoot this, we would also ask for your 
contact details so that we can contact you for any additional 
information should it be required.