SET WSDL="C:\Program Files (x86)\Microsoft WSE\v3.0\Tools\WseWsdl3.exe"
SET WSE_CLEAN=..\Tools\WseClean.exe
SET SERVER_URL=http://127.0.0.1:9002

REM %WSDL% %SERVER_URL%/esApplicationsInstaller.asmx /out:.\SolidCP.EnterpriseServer.Client\ApplicationsInstallerProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\ApplicationsInstallerProxy.cs

REM %WSDL% %SERVER_URL%/esAuditLog.asmx /out:.\SolidCP.EnterpriseServer.Client\AuditLogProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\AuditLogProxy.cs

REM %WSDL% %SERVER_URL%/esAuthentication.asmx /out:.\SolidCP.EnterpriseServer.Client\AuthenticationProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\AuthenticationProxy.cs

REM %WSDL% %SERVER_URL%/esBackup.asmx /out:.\SolidCP.EnterpriseServer.Client\BackupProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\BackupProxy.cs

REM %WSDL% %SERVER_URL%/esBlackBerry.asmx /out:.\SolidCP.EnterpriseServer.Client\BlackBerryProxy.cs /namespace:SolidCP.EnterpriseServer.HostedSolution /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\BlackBerryProxy.cs

REM %WSDL% %SERVER_URL%/esComments.asmx /out:.\SolidCP.EnterpriseServer.Client\CommentsProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\CommentsProxy.cs

REM %WSDL% %SERVER_URL%/esCRM.asmx /out:.\SolidCP.EnterpriseServer.Client\CRMProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\CRMProxy.cs

REM %WSDL% %SERVER_URL%/esDatabaseServers.asmx /out:.\SolidCP.EnterpriseServer.Client\DatabaseServersProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\DatabaseServersProxy.cs

REM %WSDL% %SERVER_URL%/esExchangeServer.asmx /out:.\SolidCP.EnterpriseServer.Client\ExchangeServerProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\ExchangeServerProxy.cs

REM %WSDL% %SERVER_URL%/esExchangeHostedEdition.asmx /out:.\SolidCP.EnterpriseServer.Client\ExchangeHostedEditionProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\ExchangeHostedEditionProxy.cs

REM %WSDL% %SERVER_URL%/esFiles.asmx /out:.\SolidCP.EnterpriseServer.Client\FilesProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\FilesProxy.cs

REM %WSDL% %SERVER_URL%/esHostedSharePointServers.asmx /out:.\SolidCP.EnterpriseServer.Client\HostedSharePointServersProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\HostedSharePointServersProxy.cs

REM %WSDL% %SERVER_URL%/esHostedSharePointServersEnt.asmx /out:.\SolidCP.EnterpriseServer.Client\HostedSharePointServersEntProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\HostedSharePointServersEntProxy.cs

REM %WSDL% %SERVER_URL%/esImport.asmx /out:.\SolidCP.EnterpriseServer.Client\ImportProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\ImportProxy.cs

REM %WSDL% %SERVER_URL%/esOCS.asmx /out:.\SolidCP.EnterpriseServer.Client\OCSProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\OCSProxy.cs

REM %WSDL% %SERVER_URL%/esOperatingSystems.asmx /out:.\SolidCP.EnterpriseServer.Client\OperatingSystemsProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\OperatingSystemsProxy.cs

%WSDL% %SERVER_URL%/esOrganizations.asmx /out:.\SolidCP.EnterpriseServer.Client\OrganizationProxy.cs /namespace:SolidCP.EnterpriseServer.HostedSolution /type:webClient
%WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\OrganizationProxy.cs

REM %WSDL% %SERVER_URL%/esPackages.asmx /out:.\SolidCP.EnterpriseServer.Client\PackagesProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\PackagesProxy.cs

REM %WSDL% %SERVER_URL%/esScheduler.asmx /out:.\SolidCP.EnterpriseServer.Client\SchedulerProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\SchedulerProxy.cs

REM %WSDL% %SERVER_URL%/esServers.asmx /out:.\SolidCP.EnterpriseServer.Client\ServersProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\ServersProxy.cs

REM %WSDL% %SERVER_URL%/esSharePointServers.asmx /out:.\SolidCP.EnterpriseServer.Client\SharePointServersProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\SharePointServersProxy.cs

REM %WSDL% %SERVER_URL%/esSystem.asmx /out:.\SolidCP.EnterpriseServer.Client\SystemProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\SystemProxy.cs

REM %WSDL% %SERVER_URL%/esTasks.asmx /out:.\SolidCP.EnterpriseServer.Client\TasksProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\TasksProxy.cs

REM %WSDL% %SERVER_URL%/esUsers.asmx /out:.\SolidCP.EnterpriseServer.Client\UsersProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\UsersProxy.cs

REM %WSDL% %SERVER_URL%/esVirtualizationServer.asmx /out:.\SolidCP.EnterpriseServer.Client\VirtualizationServerProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\VirtualizationServerProxy.cs

REM %WSDL% %SERVER_URL%/esVirtualizationServer2012.asmx /out:.\SolidCP.EnterpriseServer.Client\VirtualizationServerProxy2012.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\VirtualizationServerProxy2012.cs

REM %WSDL% %SERVER_URL%/esWebApplicationGallery.asmx /out:.\SolidCP.EnterpriseServer.Client\WebApplicationGalleryProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\WebApplicationGalleryProxy.cs

REM %WSDL% %SERVER_URL%/esWebServers.asmx /out:.\SolidCP.EnterpriseServer.Client\WebServersProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\WebServersProxy.cs

REM %WSDL% %SERVER_URL%/esVirtualizationServerForPrivateCloud.asmx /out:.\SolidCP.EnterpriseServer.Client\VirtualizationServerProxyForPrivateCloud.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\VirtualizationServerProxyForPrivateCloud.cs

REM %WSDL% %SERVER_URL%/esLync.asmx /out:.\SolidCP.EnterpriseServer.Client\LyncProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\LyncProxy.cs

REM %WSDL% %SERVER_URL%/esHeliconZoo.asmx /out:.\SolidCP.EnterpriseServer.Client\HeliconZooProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\HeliconZooProxy.cs

REM %WSDL% %SERVER_URL%/esRemoteDesktopServices.asmx /out:.\SolidCP.EnterpriseServer.Client\RemoteDesktopServicesProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\RemoteDesktopServicesProxy.cs

REM %WSDL% %SERVER_URL%/esEnterpriseStorage.asmx /out:.\SolidCP.EnterpriseServer.Client\EnterpriseStorageProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\EnterpriseStorageProxy.cs

REM %WSDL% %SERVER_URL%/esStorageSpaces.asmx /out:.\SolidCP.EnterpriseServer.Client\StorageSpacesProxy.cs /namespace:SolidCP.EnterpriseServer /type:webClient
REM %WSE_CLEAN% .\SolidCP.EnterpriseServer.Client\StorageSpacesProxy.cs



