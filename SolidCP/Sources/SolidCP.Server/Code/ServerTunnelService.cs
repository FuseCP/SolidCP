using System.Net;
using System.Threading.Tasks;
using SolidCP.Web.Services;
using SolidCP.Providers;

[assembly:TunnelService(typeof(SolidCP.Server.ServerTunnelService))]

namespace SolidCP.Server
{
    public class ServerTunnelService: ServerTunnelServiceBase
    {
        public override string CryptoKey => Settings.CryptoKey;
        public override void Authenticate(string user, string password)
        {
            PasswordValidator.Validate(password);
        }
        public override async Task<TunnelSocket> GetPveVNCWebSocket(string vmId, ServiceProviderSettings providerSettings)
        {
            var proxmox = new VirtualizationServerProxmox();
            proxmox.ProviderSettings = providerSettings;
            return await proxmox.GetPveVNCWebSocket(vmId);
        }
    }
}
