using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using SolidCP.Providers;
using SolidCP.Server.Client;

namespace SolidCP.EnterpriseServer
{
    public class EnterpriseServerTunnelService: EnterpriseServerTunnelServiceBase
    {
        public override string CryptoKey => CryptoUtility.SHA1($"{CryptoUtils.CryptoKey}{DateTime.Now.Ticks}");

        public override void Authenticate(string user, string password) => UsernamePasswordValidator.Validate(user, password);

        public override async Task<TunnelSocket> GetPveApiSocket(int serviceId)
        {
            // get service
            ServiceInfo service = ServerController.GetServiceInfo(serviceId);

            // Verfiy user has access to service 
            var user = UserController.GetUser(Username);
            var serviceItems = PackageController.GetServiceItems(serviceId);
            var packages = PackageController.GetPackages(user.UserId);
            var ownsService = serviceItems
                .Join(packages, serviceItem => serviceItem.PackageId, package => package.PackageId,
                    (serviceItem, package) => package.UserId)
                .Any(userId => userId == user.UserId);
            if (!ownsService) throw new AccessViolationException("The current user has no access to this service.");
           
            return await GetPveApiSocket(service);
        }
        private async Task<TunnelSocket> GetPveApiSocket(ServiceInfo service)
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
            var password = CryptoUtils.SHA1(server.Password);

            return await GetPveApiSocket(serverUrl, password, providerSettings); 
        }
        private async Task<TunnelSocket> GetPveApiSocket(string serverUrl, string password, ServiceProviderSettings providerSettings) {
            var client = new ServerTunnelClient() { ServerUrl = serverUrl, Password = password };

            return await client.GetPveApiSocket(providerSettings);
        }
    }
}
