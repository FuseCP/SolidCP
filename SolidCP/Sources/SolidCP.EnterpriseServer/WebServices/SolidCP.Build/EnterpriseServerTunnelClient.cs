using System;
using System.Threading.Tasks;
using SolidCP.Providers.OS;

[assembly:TunnelClient(typeof(SolidCP.EnterpriseServer.Client.EnterpriseServerTunnelClient))]

namespace SolidCP.EnterpriseServer.Client
{
    public class EnterpriseServerTunnelClient : EnterpriseServerTunnelClientBase
    {
        public string GetCryptoKey()
        {
            var system = new esSystem() { Url = ServerUrl };
            system.Credentials.UserName = Username;
            system.Credentials.Password = Password;
            return system.GetCryptoKey();
        }
        public override string CryptoKey => GetCryptoKey();
        public override async Task<TunnelSocket> GetPveVncWebSocketAsync(int serviceItemId)
        {
            return await GetSocket(nameof(GetPveVncWebSocketAsync), serviceItemId);
        }
    }
}
