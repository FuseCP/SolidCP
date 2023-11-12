In order to debug SolidCP.EnterpriseServer please:

- For net48, setup a "solidcp.enterpriseserver" site that can be run on iisexpress with iisexpress /site:solidcp.enterpriseserver 
  and that runs on http or https. http is only supported for localhost and LAN access.

  In order to configure IIS Express for this on my machine I'd have to run the following commands:
  
  IisExpressAdminCmd setupSslUrl -url:https://localhost:9019 -UseSelfSigned
  appcmd add site /name:solidcp.enterpriseserver /bindings:https/*:9019:localhost /physicalPath:c:\github\solidcp\solidcp\sources\solidcp.server


  Unfortunately it's not possible to debug NET Framework projects directrly in Visual Studio for SDK style projects. In order to debug, you have to start iisexpress via the command shell and attach to the process within Visual Studio.

- For net6 & WSL set the correct certificate settings in launchSettings.json or appsettings.json to configure the
   server certificate WCF should use, or run the server only over http.

The net 6 version of SolidCP.EnterpriseServer resides in the directory bin\net.core.
