#if !NETFRAMEWORK

using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading.Tasks;
using SolidCP.Web.Services;

[assembly:SolidCP.Web.Services.HttpHandlerTypes(new Type[] { typeof(SolidCP.Server.VNCTunnelHandlerCore) })]

namespace SolidCP.Server
{
    public class VNCTunnelHandlerCore: IRoutedHttpHandler
    {
        static bool WebSocketsInitialized = false;

        public string Route => "VNCTunnel";

        public static Dictionary<string, VirtualizationServerProxmox> Proxmox = new Dictionary<string, VirtualizationServerProxmox>();

        public Socket pveSocket = null;
        string providerType, vncParameters;

        public Socket PveSocket
        {
            get
            {
                lock (this)
                {
                    if (pveSocket != null && pveSocket.Connected) return pveSocket;

                    var pveKey = $"{vncParameters}/{providerType}";
                    VirtualizationServerProxmox proxmox;
                    if (!Proxmox.TryGetValue(pveKey, out proxmox))
                    {
                        proxmox = new VirtualizationServerProxmox();
                        Proxmox.Add(pveKey, proxmox);
                    }
                    proxmox.ProviderSettings.ProviderType = providerType;
                    proxmox.ProviderSettings["VNCParameters"] = vncParameters;
                    pveSocket = proxmox.GetPveApiSocket();
                    return pveSocket;
                }
            }
        }

        public void Init(Microsoft.AspNetCore.Builder.WebApplication app) {
            if (!WebSocketsInitialized) {
                app.UseWebSockets();
                WebSocketsInitialized = true;
            }
            app.Map(Route, HandleRequest);
        }

        public async Task HandleRequest(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                providerType = context.Request.Query["providerType"];
                vncParameters = context.Request.Query["parameters"];

                using (var ws = await context.WebSockets.AcceptWebSocketAsync())
                {
                    var buffer = new byte[1024 * 4];
                    var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    Task.Run(async () =>
                    {
                        while (!result.CloseStatus.HasValue)
                        {
                            var count = await PveSocket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                            await ws.SendAsync(new ArraySegment<byte>(buffer, 0, count), WebSocketMessageType.Binary, true, CancellationToken.None);
                        }
                    });

                    while (!result.CloseStatus.HasValue)
                    {
                        if (result.MessageType == WebSocketMessageType.Binary)
                        {
                            // forward data to PveSocket
                            await PveSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), SocketFlags.None);
                        }
                        result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    }

                    await ws.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }

        private async Task VNCTunnelWebSocket(WebSocketAcceptContext socketContext)
        {
        }
    }
}
#endif