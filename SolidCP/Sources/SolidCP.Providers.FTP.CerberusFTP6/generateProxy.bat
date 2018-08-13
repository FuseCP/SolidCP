SET WSDL="C:\Program Files (x86)\Microsoft WSE\v3.0\Tools\WseWsdl3.exe"
SET WSE_CLEAN=..\..\Tools\WseClean.exe

%WSDL% http://localhost:9001/wsdl/Cerberus.wsdl /out:.\CerberusFTP6Proxy.cs /namespace:SolidCP.Providers.FTP.CerberusFTP6Proxy /type:webClient /fields
REM %WSE_CLEAN% .\MSPControl.Server.Client\AutoDiscoveryProxy.cs