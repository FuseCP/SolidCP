using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using SolidCP.Providers;
using SolidCP.Providers.OS;
using SolidCP.Server.Client;

namespace SolidCP.EnterpriseServer
{
    public class EnterpriseServerTunnelService: EnterpriseServerTunnelServiceBase
    {
        public override string CryptoKey => CryptoUtility.SHA1($"{CryptoUtils.CryptoKey}{DateTime.Now.Ticks}");

        public override void Authenticate(string user, string password) => UsernamePasswordValidator.Validate(user, password);
        
        public override async Task<TunnelSocket> GetPveVncWebSocketAsync(int serviceId, int packageId, int serviceItemId)
        {
            // get service
            ServiceInfo service = ServerController.GetServiceInfo(serviceId);
            if (service == null) throw new AccessViolationException("Service not found.");

            var package = PackageController.GetPackage(packageId);
            if (package == null) throw new AccessViolationException("Package not found.");
            
            // Verfiy user has access to service 
            var user = UserController.GetUser(Username);
            if (package.UserId != user.UserId) throw new AccessViolationException("The current user has no access to this service.");

            var serviceItem = PackageController.GetPackageItem(serviceItemId);
            if (serviceItem == null) throw new AccessViolationException("Service item not found.");
            
            if (serviceItem.ServiceId != serviceId || serviceItem.PackageId != packageId)
                throw new AccessViolationException("The current user has no access to this service");

            var vmId = serviceItem["VirtualMachineId"];

            return await GetPveVNCWebSocket(vmId, service);
        }
        private async Task<TunnelSocket> GetPveVNCWebSocket(string vmId, ServiceInfo service)
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

            var serverUrl = CryptoUtils.DecryptServerUrl(server.ServerUrl);

            return await GetPveVNCWebSocket(serverUrl, server.Password, vmId, providerSettings); 
        }
        private async Task<TunnelSocket> GetPveVNCWebSocket(string serverUrl, string password, string vmId, ServiceProviderSettings providerSettings) {
            password = CryptoUtils.SHA1(password);
            var client = new ServerTunnelClient() { ServerUrl = serverUrl, Password = password };

            return await client.GetPveVncWebSocketAsync(vmId, providerSettings);
        }
    }
}
