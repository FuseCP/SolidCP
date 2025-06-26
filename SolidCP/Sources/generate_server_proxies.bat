:: If you want to generate files, you need:
:: - .NET Framework (SDK) version 4.8
:: - Put your future functions into your "*.asmx.cs" files, then rebuild your project.
:: - Install SolidCP Enterprise Server and Server (and, of course, update them with your latest compiled binaries).
:: - I recommend generating one file at a time (or in small groups) to prevent conflicts.
::   Delete "REM" before the lines corresponding to the files you want to generate. 
:: - After generating the files, you need to replace manually System.Web.Services.Protocols.SoapHttpClientProtocol with Microsoft.Web.Services3.WebServicesClientProtocol (though this is probably unnecessary).
SET WSDL="%ProgramFiles(x86)%\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\wsdl.exe"
SET WSE_CLEAN=..\Tools\WseClean.exe
SET SERVER_URL=http://127.0.0.1:9003

REM %WSDL% %SERVER_URL%/AutoDiscovery.asmx /out:.\SolidCP.Server.Client\AutoDiscoveryProxy.cs /namespace:SolidCP.AutoDiscovery /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\AutoDiscoveryProxy.cs

::Perhaps the BlackBerryProxy file has a problem with the namespace. After generation, you will know exactly
REM %WSDL% %SERVER_URL%/BlackBerry.asmx /out:.\SolidCP.Server.Client\BlackBerryProxy.cs /namespace:SolidCP.Providers.HostedSolution /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\BlackBerryProxy.cs

REM %WSDL% %SERVER_URL%/CRM.asmx /out:.\SolidCP.Server.Client\CRMProxy.cs /namespace:SolidCP.Providers.CRM /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\CRMProxy.cs

REM %WSDL% %SERVER_URL%/DatabaseServer.asmx /out:.\SolidCP.Server.Client\DatabaseServerProxy.cs /namespace:SolidCP.Providers.Database /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\DatabaseServerProxy.cs

REM %WSDL% %SERVER_URL%/DNSServer.asmx /out:.\SolidCP.Server.Client\DnsServerProxy.cs /namespace:SolidCP.Providers.DNS /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\DnsServerProxy.cs

REM %WSDL% %SERVER_URL%/ExchangeServer.asmx /out:.\SolidCP.Server.Client\ExchangeServerProxy.cs /namespace:SolidCP.Providers.Exchange /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\ExchangeServerProxy.cs

REM %WSDL% %SERVER_URL%/EnterpriseStorage.asmx /out:.\SolidCP.Server.Client\EnterpriseStorageProxy.cs /namespace:SolidCP.Providers.EnterpriseStorage /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\EnterpriseStorageProxy.cs

REM %WSDL% %SERVER_URL%/FTPServer.asmx /out:.\SolidCP.Server.Client\FtpServerProxy.cs /namespace:SolidCP.Providers.FTP /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\FtpServerProxy.cs

REM %WSDL% %SERVER_URL%/HeliconZoo.asmx /out:.\SolidCP.Server.Client\HeliconZooProxy.cs /namespace:SolidCP.Providers.HeliconZoo /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\HeliconZooProxy.cs

::Perhaps the HostedSharePointServerProxy file has a problem with the namespace. After generation, you will know exactly
REM %WSDL% %SERVER_URL%/HostedSharePointServer.asmx /out:.\SolidCP.Server.Client\HostedSharePointServerProxy.cs /namespace:SolidCP.Providers.HostedSolution /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\HostedSharePointServerProxy.cs

::Perhaps the HostedSharePointServerEntProxy file has a problem with the namespace. After generation, you will know exactly
REM %WSDL% %SERVER_URL%/HostedSharePointServerEnt.asmx /out:.\SolidCP.Server.Client\HostedSharePointServerEntProxy.cs /namespace:SolidCP.Providers.HostedSolution /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\HostedSharePointServerEntProxy.cs

REM %WSDL% %SERVER_URL%/LyncServer.asmx /out:.\SolidCP.Server.Client\LyncServerProxy.cs /namespace:SolidCP.Providers.Lync /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\LyncServerProxy.cs

REM %WSDL% %SERVER_URL%/MailServer.asmx /out:.\SolidCP.Server.Client\MailServerProxy.cs /namespace:SolidCP.Providers.Mail /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\MailServerProxy.cs

::Perhaps the OCSEdgeServerProxy file has a problem with the namespace. After generation, you will know exactly
REM %WSDL% %SERVER_URL%/OCSEdgeServer.asmx /out:.\SolidCP.Server.Client\OCSEdgeServerProxy.cs /namespace:SolidCP.Providers.OCS /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\OCSEdgeServerProxy.cs

::Perhaps the OCSServerProxy file has a problem with the namespace. After generation, you will know exactly
REM %WSDL% %SERVER_URL%/OCSServer.asmx /out:.\SolidCP.Server.Client\OCSServerProxy.cs /namespace:SolidCP.Providers.OCS /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\OCSServerProxy.cs

REM %WSDL% %SERVER_URL%/OperatingSystem.asmx /out:.\SolidCP.Server.Client\OperatingSystemProxy.cs /namespace:SolidCP.Providers.OS /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\OperatingSystemProxy.cs

::Perhaps the Organizations file has a problem with the namespace. After generation, you will know exactly
REM %WSDL% %SERVER_URL%/Organizations.asmx /out:.\SolidCP.Server.Client\OrganizationProxy.cs /namespace:SolidCP.Providers.HostedSolution /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\OrganizationProxy.cs

REM %WSDL% %SERVER_URL%/RemoteDesktopServices.asmx /out:.\SolidCP.Server.Client\RemoteDesktopServicesProxy.cs /namespace:SolidCP.Providers.RemoteDesktopServices /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\RemoteDesktopServicesProxy.cs

REM %WSDL% %SERVER_URL%/ServiceProvider.asmx /out:.\SolidCP.Server.Client\ServiceProviderProxy.cs /namespace:SolidCP.Providers /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\ServiceProviderProxy.cs

REM %WSDL% %SERVER_URL%/SfBServer.asmx /out:.\SolidCP.Server.Client\SfBServerProxy.cs /namespace:SolidCP.SfB /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\SfBServerProxy.cs

REM %WSDL% %SERVER_URL%/SharePointServer.asmx /out:.\SolidCP.Server.Client\SharePointServerProxy.cs /namespace:SolidCP.Providers.SharePoint /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\SharePointServerProxy.cs

REM %WSDL% %SERVER_URL%/StatisticsServer.asmx /out:.\SolidCP.Server.Client\StatisticsServerProxy.cs /namespace:SolidCP.Providers.Statistics /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\StatisticsServerProxy.cs

REM %WSDL% %SERVER_URL%/StorageSpaceServices.asmx /out:.\SolidCP.Server.Client\StorageSpacesProxy.cs /namespace:SolidCP.Providers.StorageSpaces /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\StorageSpacesProxy.cs

REM %WSDL% %SERVER_URL%/VirtualizationServer.asmx /out:.\SolidCP.Server.Client\VirtualizationServerProxy.cs /namespace:SolidCP.Providers.Virtualization /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\VirtualizationServerProxy.cs

REM %WSDL% %SERVER_URL%/VirtualizationServer2012.asmx /out:.\SolidCP.Server.Client\VirtualizationServerProxy2012.cs /namespace:SolidCP.Providers.Virtualization2012 /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\VirtualizationServerProxy2012.cs

REM %WSDL% %SERVER_URL%/VirtualizationServerForPrivateCloud.asmx /out:.\SolidCP.Server.Client\VirtualizationServerForPrivateCloudProxy.cs /namespace:SolidCP.Providers.VirtualizationForPC /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\VirtualizationServerForPrivateCloudProxy.cs

REM %WSDL% %SERVER_URL%/VirtualizationServerProxmox.asmx /out:.\SolidCP.Server.Client\VirtualizationServerProxyProxmox.cs /namespace:SolidCP.Providers.VirtualizationProxmox /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\VirtualizationServerProxyProxmox.cs

REM %WSDL% %SERVER_URL%/WebServer.asmx /out:.\SolidCP.Server.Client\WebServerProxy.cs /namespace:SolidCP.Providers.Web /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\WebServerProxy.cs

REM %WSDL% %SERVER_URL%/WindowsServer.asmx /out:.\SolidCP.Server.Client\WindowsServerProxy.cs /namespace:SolidCP.Server /protocol:Soap /fields
REM %WSE_CLEAN% .\SolidCP.Server.Client\WindowsServerProxy.cs
