*************************************************************************************************************************
  Guacamole branding extension for SolidCP

  This extension, guacamole-brand-solidcp.jar, modifies the login screen for Guacamole to simply display the SolidCP
  logo. It removes other visible content including username and password fields.  You can customize this extension 
  as you wish.

  This extension is optional. It is not required for the SolidCP authentication provider to function correctly.

  If you wish to replace the SolidCP logo with your own logo, replace the \src\resources\images\solidcp\logo.png
  file with your own and create a new guacamole-brand-solidcp.jar file.

  If you are using other authentication providers along with the SolidCP authentication provider then you may need
  to restore the ability to login manually.  This can be achieved by removing the custom html pages from the 
  guac-manifest.json file and creating a new guacamole-brand-solidcp.jar file.
  
  A jar is simply a zip file with a .jar extension.

*************************************************************************************************************************

  1) Modifying the extension
	- Copy the brand-ext folder and its contents to your computer
	- Modify the src files to meet your needs
	- Zip the contents contained in the src folder
	- Name the zip file guacamole-brand-solidcp.jar

  2) Installing the extension
	- Copy the guacamole-brand-solidcp.jar to the GUACAMOLE_HOME/extensions/ folder (ie /etc/guacamole/extensions/)
	- Set ownership for the tomcat user
		# sudo chown tomcat:tomcat /etc/guacamole/extensions/guacamole-brand-solidcp.jar
	- Restart guacamole and tomcat
		# sudo systemctl restart guacd tomcat9

