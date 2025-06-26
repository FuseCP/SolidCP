:: If you want to generate files, you need:
:: - .NET Framework (SDK) version 4.8
:: - Put your future functions into your "*.asmx.cs" files, then rebuild your project.
:: - Install SolidCP Enterprise Server and Server (and, of course, update them with your latest compiled binaries).
:: - I recommend generating one file at a time (or in small groups) to prevent conflicts.
::   Delete "REM" before the lines corresponding to the files you want to generate. 
:: - After generating the files, you need to replace manually System.Web.Services.Protocols.SoapHttpClientProtocol with Microsoft.Web.Services3.WebServicesClientProtocol (though this is probably unnecessary).
SET WSDL="%ProgramFiles(x86)%\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\wsdl.exe"
SET WSE_CLEAN=..\Tools\WseClean.exe
SET SERVER_URL=http://127.0.0.1:9002

REM %WSDL% %SERVER_URL%/esApplicationsInstaller.asmx /out:.\SolidCP.EnterpriseServer.Client\ApplicationsInstallerProxy.cs /namespace:SolidCP.EnterpriseServer.ApplicationsInstaller /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\ApplicationsInstallerProxy.cs

REM %WSDL% %SERVER_URL%/esAuditLog.asmx /out:.\SolidCP.EnterpriseServer.Client\AuditLogProxy.cs /namespace:SolidCP.EnterpriseServer.AuditLog /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\AuditLogProxy.cs

REM %WSDL% %SERVER_URL%/esAuthentication.asmx /out:.\SolidCP.EnterpriseServer.Client\AuthenticationProxy.cs /namespace:SolidCP.EnterpriseServer.Authentication /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\AuthenticationProxy.cs

REM %WSDL% %SERVER_URL%/esBackup.asmx /out:.\SolidCP.EnterpriseServer.Client\BackupProxy.cs /namespace:SolidCP.EnterpriseServer.Backup /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\BackupProxy.cs

REM %WSDL% %SERVER_URL%/esBlackBerry.asmx /out:.\SolidCP.EnterpriseServer.Client\BlackBerryProxy.cs /namespace:SolidCP.EnterpriseServer.BlackBerry /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\BlackBerryProxy.cs

REM %WSDL% %SERVER_URL%/esComments.asmx /out:.\SolidCP.EnterpriseServer.Client\CommentsProxy.cs /namespace:SolidCP.EnterpriseServer.Comments /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\CommentsProxy.cs

REM %WSDL% %SERVER_URL%/esCRM.asmx /out:.\SolidCP.EnterpriseServer.Client\CRMProxy.cs /namespace:SolidCP.EnterpriseServer.CRM /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\CRMProxy.cs

REM %WSDL% %SERVER_URL%/esDatabaseServers.asmx /out:.\SolidCP.EnterpriseServer.Client\DatabaseServersProxy.cs /namespace:SolidCP.EnterpriseServer.DatabaseServers /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\DatabaseServersProxy.cs

REM %WSDL% %SERVER_URL%/esEnterpriseStorage.asmx /out:.\SolidCP.EnterpriseServer.Client\EnterpriseStorageProxy.cs /namespace:SolidCP.EnterpriseServer.EnterpriseStorage /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\EnterpriseStorageProxy.cs

REM %WSDL% %SERVER_URL%/esExchangeServer.asmx /out:.\SolidCP.EnterpriseServer.Client\ExchangeServerProxy.cs /namespace:SolidCP.EnterpriseServer.ExchangeServer /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\ExchangeServerProxy.cs

REM %WSDL% %SERVER_URL%/esFiles.asmx /out:.\SolidCP.EnterpriseServer.Client\FilesProxy.cs /namespace:SolidCP.EnterpriseServer.Files /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\FilesProxy.cs

REM %WSDL% %SERVER_URL%/esFtpServers.asmx /out:.\SolidCP.EnterpriseServer.Client\FtpServersProxy.cs /namespace:SolidCP.EnterpriseServer.FtpServers /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\FtpServersProxy.cs

REM %WSDL% %SERVER_URL%/esHeliconZoo.asmx /out:.\SolidCP.EnterpriseServer.Client\HeliconZooProxy.cs /namespace:SolidCP.EnterpriseServer.HeliconZoo /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\HeliconZooProxy.cs

REM %WSDL% %SERVER_URL%/esHostedSharePointServers.asmx /out:.\SolidCP.EnterpriseServer.Client\HostedSharePointServersProxy.cs /namespace:SolidCP.EnterpriseServer.HostedSharePointServers /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\HostedSharePointServersProxy.cs

REM %WSDL% %SERVER_URL%/esHostedSharePointServersEnt.asmx /out:.\SolidCP.EnterpriseServer.Client\HostedSharePointServersEntProxy.cs /namespace:SolidCP.EnterpriseServer.HostedSharePointServersEnt /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\HostedSharePointServersEntProxy.cs

REM %WSDL% %SERVER_URL%/esImport.asmx /out:.\SolidCP.EnterpriseServer.Client\ImportProxy.cs /namespace:SolidCP.EnterpriseServer.Import /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\ImportProxy.cs

REM %WSDL% %SERVER_URL%/esLync.asmx /out:.\SolidCP.EnterpriseServer.Client\LyncProxy.cs /namespace:SolidCP.EnterpriseServer.Lync /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\LyncProxy.cs

REM %WSDL% %SERVER_URL%/esMailServers.asmx /out:.\SolidCP.EnterpriseServer.Client\MailServersProxy.cs /namespace:SolidCP.EnterpriseServer.MailServers /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\MailServersProxy.cs

REM %WSDL% %SERVER_URL%/esOCS.asmx /out:.\SolidCP.EnterpriseServer.Client\OCSProxy.cs /namespace:SolidCP.EnterpriseServer.OCS /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\OCSProxy.cs

REM %WSDL% %SERVER_URL%/esOperatingSystems.asmx /out:.\SolidCP.EnterpriseServer.Client\OperatingSystemsProxy.cs /namespace:SolidCP.EnterpriseServer.OperatingSystems /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\OperatingSystemsProxy.cs

REM %WSDL% %SERVER_URL%/esOrganizations.asmx /out:.\SolidCP.EnterpriseServer.Client\OrganizationProxy.cs /namespace:SolidCP.EnterpriseServer.Organizations /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\OrganizationProxy.cs

REM %WSDL% %SERVER_URL%/esPackages.asmx /out:.\SolidCP.EnterpriseServer.Client\PackagesProxy.cs /namespace:SolidCP.EnterpriseServer.Packages /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\PackagesProxy.cs

REM %WSDL% %SERVER_URL%/esRemoteDesktopServices.asmx /out:.\SolidCP.EnterpriseServer.Client\RemoteDesktopServicesProxy.cs /namespace:SolidCP.EnterpriseServer.RemoteDesktopServices /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\RemoteDesktopServicesProxy.cs

REM %WSDL% %SERVER_URL%/esScheduler.asmx /out:.\SolidCP.EnterpriseServer.Client\SchedulerProxy.cs /namespace:SolidCP.EnterpriseServer.Scheduler /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\SchedulerProxy.cs

REM %WSDL% %SERVER_URL%/esServers.asmx /out:.\SolidCP.EnterpriseServer.Client\ServersProxy.cs /namespace:SolidCP.EnterpriseServer.Servers /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\ServersProxy.cs

REM %WSDL% %SERVER_URL%/esSfB.asmx /out:.\SolidCP.EnterpriseServer.Client\SfBProxy.cs /namespace:SolidCP.EnterpriseServer.SfB /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\SfBProxy.cs

REM %WSDL% %SERVER_URL%/esSharePointServers.asmx /out:.\SolidCP.EnterpriseServer.Client\SharePointServersProxy.cs /namespace:SolidCP.EnterpriseServer.SharePointServers /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\SharePointServersProxy.cs

REM %WSDL% %SERVER_URL%/esStatisticsServers.asmx /out:.\SolidCP.EnterpriseServer.Client\StatisticsServersProxy.cs /namespace:SolidCP.EnterpriseServer.StatisticsServers /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\StatisticsServersProxy.cs

REM %WSDL% %SERVER_URL%/esStorageSpaces.asmx /out:.\SolidCP.EnterpriseServer.Client\StorageSpacesProxy.cs /namespace:SolidCP.EnterpriseServer.StorageSpaces /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\StorageSpacesProxy.cs

REM %WSDL% %SERVER_URL%/esSystem.asmx /out:.\SolidCP.EnterpriseServer.Client\SystemProxy.cs /namespace:SolidCP.EnterpriseServer.System /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\SystemProxy.cs

REM %WSDL% %SERVER_URL%/esTasks.asmx /out:.\SolidCP.EnterpriseServer.Client\TasksProxy.cs /namespace:SolidCP.EnterpriseServer.Tasks /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\TasksProxy.cs

REM %WSDL% %SERVER_URL%/esUsers.asmx /out:.\SolidCP.EnterpriseServer.Client\UsersProxy.cs /namespace:SolidCP.EnterpriseServer.Users /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\UsersProxy.cs

REM %WSDL% %SERVER_URL%/esVirtualizationServer.asmx /out:.\SolidCP.EnterpriseServer.Client\VirtualizationServerProxy.cs /namespace:SolidCP.EnterpriseServer.VirtualizationServer /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\VirtualizationServerProxy.cs

REM %WSDL% %SERVER_URL%/esVirtualizationServer2012.asmx /out:.\SolidCP.EnterpriseServer.Client\VirtualizationServerProxy2012.cs /namespace:SolidCP.EnterpriseServer.VirtualizationServer2012 /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\VirtualizationServerProxy2012.cs

REM %WSDL% %SERVER_URL%/esVirtualizationServerForPrivateCloud.asmx /out:.\SolidCP.EnterpriseServer.Client\VirtualizationServerProxyForPrivateCloud.cs /namespace:SolidCP.EnterpriseServer.VirtualizationServerForPrivateCloud /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\VirtualizationServerProxyForPrivateCloud.cs

REM %WSDL% %SERVER_URL%/esVirtualizationServerProxmox.asmx /out:.\SolidCP.EnterpriseServer.Client\VirtualizationServerProxyProxmox.cs /namespace:SolidCP.EnterpriseServer.VirtualizationServerProxmox /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\VirtualizationServerProxyProxmox.cs

REM %WSDL% %SERVER_URL%/esWebApplicationGallery.asmx /out:.\SolidCP.EnterpriseServer.Client\WebApplicationGalleryProxy.cs /namespace:SolidCP.EnterpriseServer.WebApplicationGallery /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\WebApplicationGalleryProxy.cs

REM %WSDL% %SERVER_URL%/esWebServers.asmx /out:.\SolidCP.EnterpriseServer.Client\WebServersProxy.cs /namespace:SolidCP.EnterpriseServer.WebServers /protocol:Soap
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\WebServersProxy.cs
