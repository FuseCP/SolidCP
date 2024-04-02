#if NETFRAMEWORK

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Web;
using System.Web.Routing;
using System.Web.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using SolidCP.Web.Services;

[assembly:SolidCP.Web.Services.HttpHandlerTypes(new Type[] { typeof(SolidCP.Server.VNCTunnelHandlerNetFX) })]

namespace SolidCP.Server
{
    public class VNCTunnelHandlerNetFX : IHttpHandler, IRouteHandler, IRoutedHttpHandler
    {

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

        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
            {
                providerType = context.Request.QueryString["providerType"];
                vncParameters = context.Request.QueryString["parameters"];

                context.AcceptWebSocketRequest(VNCTunnelWebSocket);
            } else {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }

        private async Task VNCTunnelWebSocket(AspNetWebSocketContext socketContext)
        {
            var buffer = new byte[1024 * 4];
            var result = await socketContext.WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            Task.Run(async () =>
            {
                while (!result.CloseStatus.HasValue)
                {
                    var count = await PveSocket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                    await socketContext.WebSocket.SendAsync(new ArraySegment<byte>(buffer, 0, count), WebSocketMessageType.Binary, true, CancellationToken.None);
                }
            });

            while (!result.CloseStatus.HasValue)
            {
                if (result.MessageType == WebSocketMessageType.Binary)
                {
                    // forward data to PveSocket
                    await PveSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), SocketFlags.None);
                }
                result = await socketContext.WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await socketContext.WebSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return this;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
#endif