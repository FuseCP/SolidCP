:If you want to generate files, you need:
:.NET Framework (SDK) version 2.0 (x64) https://www.microsoft.com/en-us/download/details.aspx?id=15354 (manully unzip setup.exe and click install.msi)
:(WSE) 3.0 for Microsoft .NET https://www.microsoft.com/en-us/download/details.aspx?id=14089
: Put your future functions in your "*.asmx.cs" files, and then rebuild your project
: Install SolidCP EnterpriseServer && Server (and ofcourse update it with last your bin libs)
: Works on the OS Windows 2012R2 (possibly on Windows 10/2016)
: I recommend generating one file at a time (or group of files), to prevent conflicts. (delete "REM" before the files you want to generate) <--- delete this comment after generating all the files.
SET WSDL="%ProgramFiles(x86)%\Microsoft WSE\v3.0\Tools\WseWsdl3.exe"
SET WSE_CLEAN=..\Tools\WseClean.exe
SET SERVER_URL=http://127.0.0.1:9003

REM %WSDL% %SERVER_URL%/AutoDiscovery.asmx /out:.\SolidCP.Server.Client\AutoDiscoveryProxy.cs /namespace:SolidCP.AutoDiscovery /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\AutoDiscoveryProxy.cs

REM %WSDL% %SERVER_URL%/BlackBerry.asmx /out:.\SolidCP.Server.Client\BlackBerryProxy.cs /namespace:SolidCP.Providers.HostedSolution /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\BlackBerryProxy.cs

REM %WSDL% %SERVER_URL%/CRM.asmx /out:.\SolidCP.Server.Client\CRMProxy.cs /namespace:SolidCP.Providers.CRM /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\CRMProxy.cs

REM %WSDL% %SERVER_URL%/DatabaseServer.asmx /out:.\SolidCP.Server.Client\DatabaseServerProxy.cs /namespace:SolidCP.Providers.Database /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\DatabaseServerProxy.cs

REM %WSDL% %SERVER_URL%/DNSServer.asmx /out:.\SolidCP.Server.Client\DnsServerProxy.cs /namespace:SolidCP.Providers.DNS /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\DnsServerProxy.cs

REM %WSDL% %SERVER_URL%/ExchangeServer.asmx /out:.\SolidCP.Server.Client\ExchangeServerProxy.cs /namespace:SolidCP.Providers.Exchange /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\ExchangeServerProxy.cs

REM %WSDL% %SERVER_URL%/EnterpriseStorage.asmx /out:.\SolidCP.Server.Client\EnterpriseStorageProxy.cs /namespace:SolidCP.Providers.EnterpriseStorage /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\EnterpriseStorageProxy.cs

REM %WSDL% %SERVER_URL%/FTPServer.asmx /out:.\SolidCP.Server.Client\FtpServerProxy.cs /namespace:SolidCP.Providers.EnterpriseStorage /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\FtpServerProxy.cs

REM %WSDL% %SERVER_URL%/HeliconZoo.asmx /out:.\SolidCP.Server.Client\HeliconZooProxy.cs /namespace:SolidCP.Providers.HeliconZoo /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\HeliconZooProxy.cs

REM %WSDL% %SERVER_URL%/HostedSharePointServer.asmx /out:.\SolidCP.Server.Client\HostedSharePointServerProxy.cs /namespace:SolidCP.Providers.HostedSolution /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\HostedSharePointServerProxy.cs

REM %WSDL% %SERVER_URL%/HostedSharePointServerEnt.asmx /out:.\SolidCP.Server.Client\HostedSharePointServerEntProxy.cs /namespace:SolidCP.Providers.HostedSolution /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\HostedSharePointServerEntProxy.cs

REM %WSDL% %SERVER_URL%/LyncServer.asmx /out:.\SolidCP.Server.Client\LyncServerProxy.cs /namespace:SolidCP.Providers.Lync /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\LyncServerProxy.cs

REM %WSDL% %SERVER_URL%/MailServer.asmx /out:.\SolidCP.Server.Client\MailServerProxy.cs /namespace:SolidCP.Providers.Lync /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\MailServerProxy.cs

REM %WSDL% %SERVER_URL%/OCSEdgeServer.asmx /out:.\SolidCP.Server.Client\OCSEdgeServerProxy.cs /namespace:SolidCP.Providers.OCS /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\OCSEdgeServerProxy.cs

REM %WSDL% %SERVER_URL%/OCSServer.asmx /out:.\SolidCP.Server.Client\OCSServerProxy.cs /namespace:SolidCP.Providers.OCS /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\OCSServerProxy.cs

REM %WSDL% %SERVER_URL%/OperatingSystem.asmx /out:.\SolidCP.Server.Client\OperatingSystemProxy.cs /namespace:SolidCP.Providers.OS /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\OperatingSystemProxy.cs

REM %WSDL% %SERVER_URL%/Organizations.asmx /out:.\SolidCP.Server.Client\OrganizationProxy.cs /namespace:SolidCP.Providers.HostedSolution /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\OrganizationProxy.cs

REM %WSDL% %SERVER_URL%/RemoteDesktopServices.asmx /out:.\SolidCP.Server.Client\RemoteDesktopServicesProxy.cs /namespace:SolidCP.Providers.RemoteDesktopServices /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\RemoteDesktopServicesProxy.cs

REM %WSDL% %SERVER_URL%/ServiceProvider.asmx /out:.\SolidCP.Server.Client\ServiceProviderProxy.cs /namespace:SolidCP.Providers /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\ServiceProviderProxy.cs

REM %WSDL% %SERVER_URL%/SfBServer.asmx /out:.\SolidCP.Server.Client\SfBServerProxy.cs /namespace:SolidCP.Providers /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\SfBServerProxy.cs

REM %WSDL% %SERVER_URL%/SharePointServer.asmx /out:.\SolidCP.Server.Client\SharePointServerProxy.cs /namespace:SolidCP.Providers.SharePoint /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\SharePointServerProxy.cs

REM %WSDL% %SERVER_URL%/StatisticsServer.asmx /out:.\SolidCP.Server.Client\StatisticsServerProxy.cs /namespace:SolidCP.Providers.StorageSpaces /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\StatisticsServerProxy.cs

REM %WSDL% %SERVER_URL%/StorageSpaceServices.asmx /out:.\SolidCP.Server.Client\StorageSpacesProxy.cs /namespace:SolidCP.Providers.StorageSpaces /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\StorageSpacesProxy.cs

REM %WSDL% %SERVER_URL%/VirtualizationServer.asmx /out:.\SolidCP.Server.Client\VirtualizationServerProxy.cs /namespace:SolidCP.Providers.Virtualization /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\VirtualizationServerProxy.cs

REM %WSDL% %SERVER_URL%/VirtualizationServer2012.asmx /out:.\SolidCP.Server.Client\VirtualizationServerProxy2012.cs /namespace:SolidCP.Providers.Virtualization2012 /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\VirtualizationServerProxy2012.cs

REM %WSDL% %SERVER_URL%/VirtualizationServerForPrivateCloud.asmx /out:.\SolidCP.Server.Client\VirtualizationServerForPrivateCloudProxy.cs /namespace:SolidCP.Providers.VirtualizationForPC /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\VirtualizationServerForPrivateCloudProxy.cs

REM %WSDL% %SERVER_URL%/VirtualizationServerProxmox.asmx /out:.\SolidCP.Server.Client\VirtualizationServerProxyProxmox.cs /namespace:SolidCP.Providers.VirtualizationProxmox /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\VirtualizationServerProxyProxmox.cs

REM %WSDL% %SERVER_URL%/WebServer.asmx /out:.\SolidCP.Server.Client\WebServerProxy.cs /namespace:SolidCP.Providers.Web /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\WebServerProxy.cs

REM %WSDL% %SERVER_URL%/WindowsServer.asmx /out:.\SolidCP.Server.Client\WindowsServerProxy.cs /namespace:SolidCP.Server /type:webClient /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\WindowsServerProxy.cs
