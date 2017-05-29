Supported WHMCS version: 6.x and 7.x (versions <= 5.x are not supported and not working anymore)
Supported PHP version: >= 5.4 and 7.x
If you are running WHMCS 5.x (which is not recommended), you can find the latest compatible plugin here:
http://installer.solidcp.com/Files/plugins/SolidCP.WHMCS-v5-Module.zip (this module is not maintained anymore)

Installation instruction:
- Unpack the zip file
- Upload to your WHMCS root directory, overwrite existing files
- In Admin Panel go to "System" -> "Addon Modules"
- Activate "SolidCP Module"
- Configure "SolidCP Module" and grant access to Full Administrator -> Save changes
- In the main menu go to "Addons" -> "SolidCP Module"
- Adjust your settings.
- Add a new server to "System" -> "Products/Services" -> "Servers"
- Configure your products/services with the "SolidCP" module in Module Settings tab.
- Done!

Update instruction:
Please note, that due to structure changes in the database, there is no possibility to use old modules (< 1.1.0) after a successful migration.
- Make a full backup of your entire WHMCS database.
- Unpack the zip file
- Upload to your WHMCS root directory, overwrite existing files
- In Admin Panel go to "System" -> "Addon Modules"
- Activate "SolidCP Module"
- Configurate "SolidCP Module" and grant access to "Full Administrator"
- In the main menu go to "Addons" -> "SolidCP Module"
- Follow migration instructions.
- When migration is finished, the Settings page will be shown automatically.
- Adjust your settings and check if Addons and Configurable Options were migrated successfully (when used in previous version)
- Done!