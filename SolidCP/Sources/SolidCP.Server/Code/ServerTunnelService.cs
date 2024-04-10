using System.Net;
using System.Threading.Tasks;
using SolidCP.Web.Services;
using SolidCP.Providers;
using SolidCP.Providers.OS;

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
        public override async Task<TunnelSocket> GetPveVncWebSocketAsync(string vmId, ServiceProviderSettings providerSettings)
        {
            using (var proxmox = new VirtualizationServerProxmox())
            {
                proxmox.ProviderSettings = providerSettings;
                proxmox.ServerSettings = new RemoteServerSettings();
                return await proxmox.GetPveVncWebSocketAsync(vmId);
            }
        }
    }
}
