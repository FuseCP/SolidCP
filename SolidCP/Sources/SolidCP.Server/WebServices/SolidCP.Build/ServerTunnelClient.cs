using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidCP.Providers;
using SolidCP.Providers.OS;

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
            return serviceProviderClient.GetCryptoKey();
        }

        public override string CryptoKey => GetCryptoKey();

        public override async Task<TunnelSocket> GetPveVNCWebSocket(string vmId, ServiceProviderSettings providerSettings)
        {
            return await GetSocket(nameof(GetPveVNCWebSocket), vmId, providerSettings);
        }
    }
}
