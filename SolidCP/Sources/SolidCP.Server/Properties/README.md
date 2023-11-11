In order to debug SolidCP.Server please:

- For net48, setup a "solidcp.server" site that can be run on iisexpress with iisexpress /site:solidcp.server 
  and that runs on https
  In order to configure IIS Express for this on my machine I'd have to run the following commands:
  
  IisExpressAdminCmd setupSslUrl -url:https://localhost:9018 -UseSelfSigned
  appcmd add site /name:solidcp.server /bindings:https/*:9018:localhost /physicalPath:c:\github\solidcp\solidcp\sources\solidcp.server

- For net6 & WSL set the correct certificate settings in launchSettings.json or appsettings.json to configure the
   server certificate WCF should use.
