using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.DirectoryServices;
using SolidCP.Providers;
using SolidCP.Providers.OS;
using SolidCP.Providers.Virtualization;
using SolidCP.EnterpriseServer;
using SolidCP.Server.Client;

[assembly:TunnelService(typeof(SolidCP.EnterpriseServer.EnterpriseServerTunnelService))]

namespace SolidCP.EnterpriseServer
{
    public class EnterpriseServerTunnelService: EnterpriseServerTunnelServiceBase
    {
        Controllers Controllers = new Controllers();
        PackageController PackageController => Controllers.PackageController;
        ServerController ServerController => Controllers.ServerController;
        UserController UserController => Controllers.UserController;

        static string cryptoKey = null;
        public override string CryptoKey => cryptoKey ?? (cryptoKey = Cryptor.SHA256($"{CryptoUtils.CryptoKey}{DateTime.Now.Ticks}"));

        public override void Authenticate(string user, string password) => UsernamePasswordValidator.Validate(user, password);
        
        public override async Task<TunnelSocket> GetPveVncWebSocketAsync(int serviceItemId, VncCredentials credentials)
        {
            var serviceItem = PackageController.GetPackageItem(serviceItemId) as VirtualMachine;
            if (serviceItem == null) throw new AccessViolationException("Service item not found.");

            // get service
            ServiceInfo service = ServerController.GetServiceInfo(serviceItem.ServiceId);
            if (service == null) throw new AccessViolationException("Service not found.");

            var package = PackageController.GetPackage(serviceItem.PackageId);
            if (package == null) throw new AccessViolationException("Package not found.");
            
            // Verfiy user has access to service 
            var user = UserController.GetUser(Username);
            if (package.UserId != user.UserId && user.Role != UserRole.Administrator) throw new AccessViolationException("The current user has no access to this service.");

            var vmId = serviceItem.VirtualMachineId;

            return await GetPveVNCWebSocket(vmId, credentials, service);
        }
        private async Task<TunnelSocket> GetPveVNCWebSocket(string vmId, VncCredentials credentials, ServiceInfo service)
        {
            if (service == null)
                throw new Exception($"Service with ID {service.ServiceId} was not found");

            var providerSettings = new ServiceProviderSettings();
            // set service settings
            StringDictionary serviceSettings = ServerController.GetServiceSettings(service.ServiceId);
            foreach (string key in serviceSettings.Keys)
                providerSettings.Settings[key] = serviceSettings[key];

            // get provider
            ProviderInfo provider = ServerController.GetProvider(service.ProviderId);
            providerSettings.ProviderGroupID = provider.GroupId;
            providerSettings.ProviderCode = provider.ProviderName;
            providerSettings.ProviderName = provider.DisplayName;
            providerSettings.ProviderType = provider.ProviderType;

            ServerInfo server = ServerController.GetServerById(service.ServerId);
            
            var serverSettings = new RemoteServerSettings();
            serverSettings.ADEnabled = server.ADEnabled;
            // TODO support AutheticationTypes on Linux
            serverSettings.ADAuthenticationType = AuthenticationTypes.Secure;

            AuthenticationTypes adAuthenticationType;
            if (Enum.TryParse<AuthenticationTypes>(server.ADAuthenticationType, true, out adAuthenticationType))
                serverSettings.ADAuthenticationType = adAuthenticationType;

            serverSettings.ADRootDomain = server.ADRootDomain;
            serverSettings.ADUsername = server.ADUsername;
            serverSettings.ADPassword = server.ADPassword;
            serverSettings.ADParentDomain = server.ADParentDomain;
            serverSettings.ADParentDomainController = server.ADParentDomainController;

            var serverUrl = CryptoUtils.DecryptServerUrl(server.ServerUrl);

            return await GetPveVNCWebSocket(serverUrl, server.Password, server.PasswordIsSHA256, vmId, credentials, serverSettings, providerSettings); 
        }
        private async Task<TunnelSocket> GetPveVNCWebSocket(string serverUrl, string password, bool sha256Password, string vmId, VncCredentials credentials, RemoteServerSettings serverSettings, ServiceProviderSettings providerSettings) {
            password = sha256Password ? CryptoUtils.SHA256(password) : CryptoUtils.SHA1(password);
            var client = new ServerTunnelClient() { ServerUrl = serverUrl, Password = password };

            return await client.GetPveVncWebSocketAsync(vmId, credentials, serverSettings, providerSettings);
        }
    }
}
