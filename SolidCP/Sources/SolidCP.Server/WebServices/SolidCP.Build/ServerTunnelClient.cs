using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidCP.Providers;
using SolidCP.Providers.OS;
using SolidCP.Providers.Virtualization;

[assembly:TunnelClient(typeof(SolidCP.Server.Client.ServerTunnelClient))]

namespace SolidCP.Server.Client
{
    public class ServerTunnelClient: ServerTunnelClientBase
    {
        public virtual string GetCryptoKey()
        {
            if (string.IsNullOrEmpty(ServerUrl)) throw new ArgumentException("ServerUrl must be set.");

            var serviceProviderClient = new ServiceProvider();
            serviceProviderClient.Url = ServerUrl;
            serviceProviderClient.Credentials.UserName = "";
            serviceProviderClient.Credentials.Password = Password;
            return serviceProviderClient.GetCryptoKey();
        }

        public override string CryptoKey => GetCryptoKey();

        public override async Task<TunnelSocket> GetPveVncWebSocketAsync(string vmId, ProxmoxVncCredentials credentials, RemoteServerSettings serverSettings, ServiceProviderSettings providerSettings)
        {
            return await GetTunnel(nameof(GetPveVncWebSocketAsync), vmId, credentials, serverSettings, providerSettings);
        }
    }
}
