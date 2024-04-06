using System;
using System.Threading.Tasks;
using SolidCP.Providers.OS;

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
        public override async Task<TunnelSocket> GetPveVNCWebSocket(int serviceId, int packageId, int serviceItemId)
        {
            return await GetSocket(nameof(GetPveVNCWebSocket), serviceId, packageId, serviceItemId);
        }
    }
}
