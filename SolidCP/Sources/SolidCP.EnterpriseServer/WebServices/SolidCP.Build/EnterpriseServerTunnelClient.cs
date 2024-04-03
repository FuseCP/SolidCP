using System;
using System.Threading.Tasks;
using SolidCP.Providers;

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
        public override async Task<TunnelSocket> GetPveApiSocket(int serviceId)
        {
            return await GetSocket(nameof(GetPveApiSocket), serviceId);
        }
    }
}
