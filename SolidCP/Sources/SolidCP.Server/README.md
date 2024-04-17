In order to debug SolidCP.Server please:

Debugging with net48 when running SolidCP.Server on Windows:

- Debugging with IIS:
  Setup a IIS Website for SolidCP.Server pointing to the SolidCP.Server folder. The Application Pool should run with
  Admin privileges, this is best done by using the builtin SYSTEM account.
  To debug, attack to IIS with Visual Studio running as Administrator

- Debugging with IIS Express:
  Setup a "solidcp.server" site that can be run on iisexpress with iisexpress /site:solidcp.server 
  and that runs on http or https. Http is only supported for localhost and LAN access.

  In order to configure IIS Express for this on my machine I'd have to run the following commands:
  
  IisExpressAdminCmd setupSslUrl -url:https://localhost:9019 -UseSelfSigned
  appcmd add site /name:solidcp.server /bindings:http/*:9018:localhost,https/*:9019:localhost /physicalPath:c:\github\solidcp\solidcp\sources\solidcp.server

  Unfortunately it's not possible to debug NET Framework projects directly in Visual Studio for SDK style projects.
  In order to debug, you have to start iisexpress via the command shell and attach to the process within Visual Studio.

Debugging on Linux:

- For net8 & WSL set the correct certificate settings in launchSettings.json or appsettings.json to configure the
  server certificate WCF should use, or run the server only over http.
  If the certificate is set or you only specified http url's in appsettings.json, you can debug the server from within
  Visual Stuido by clickng on the WSL debug button, to debug SolidCP.Server in WSL.


The net8 version of the SolidCP.Server resides in the directory bin_dotnet. It can be started by excuting the command

dotnet SolidCP.Server.dll

or on Linux:

sudo dotnet SolidCP.Server.dll

as the Server needs to run as root user.