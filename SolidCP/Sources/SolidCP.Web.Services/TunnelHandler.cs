using System;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SolidCP.Providers;
using SolidCP.Providers.OS;
#if !NETFRAMEWORK
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
#else
using System.Web;
using System.Web.Routing;
using System.Web.WebSockets;
#endif

namespace SolidCP.Web.Services
{

    public class TunnelHandlerBase
#if NETFRAMEWORK
        : HttpTaskAsyncHandler
#endif
    {
        public string Route => "Tunnel";

        public async Task<TunnelSocket> GetTunnel(string caller, string method, byte[] arguments)
        {
            var service = new TunnelService(caller);
            return await service.Service.GetTunnel(method, arguments);
        }

        public async Task Transmit(TunnelSocket listener, TunnelSocket destination)
        {
            try
            {
                await listener.ProvideUpgradeTunnelSocketAsync(destination);
                await listener.Transmit(destination);
            }
            catch (Exception ex)
            {
                if (listener.IsConnected) await listener.CloseAsync(WebSocketCloseStatus.InternalServerError, ex.StackTrace);
                if (destination.IsConnected) await destination.CloseAsync(WebSocketCloseStatus.InternalServerError, ex.StackTrace);
                throw new IOException(ex.Message, ex);
            } finally
            {
                destination.Dispose();
            }
        }

        public async Task<byte[]> ReadArgumentsAsync(TunnelSocket listener) => (await listener.ReceiveData()).ToArray();

#if NETFRAMEWORK
        public override Task ProcessRequestAsync(HttpContext context) => throw new NotImplementedException();
#endif
    }

#if !NETFRAMEWORK
    public class TunnelHandlerCore : TunnelHandlerBase, ITunnelHandler
    {
        static bool WebSocketsInitialized = false;

        public void Init(WebApplication app)
        {
            if (!WebSocketsInitialized)
            {
                app.UseWebSockets();
                WebSocketsInitialized = true;
            }
            app.Map(Route, HandleRequest);
        }

        public async Task HandleRequest(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                string caller, method;
                caller = context.Request.Query["caller"];
                method = context.Request.Query["method"];
                if (string.IsNullOrEmpty(caller) || string.IsNullOrEmpty(method))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return;
                }

                using (var webSocket = await context.WebSockets.AcceptWebSocketAsync())
                {
                    try
                    {
                        var tunnel = new TunnelSocket(webSocket);

                        var args = await ReadArgumentsAsync(tunnel);

                        var dest = await GetTunnel(caller, method, args);
                        if (dest != null)
                        {
                            await Transmit(tunnel, dest);
                        }
                        else
                        {
                                await webSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Cannot get a tunnel", CancellationToken.None);
                        }
                    }
                    catch (Exception ex)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, $"{ex.Message}\n{ex.StackTrace}", CancellationToken.None);
                    }

                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
    }
#else
    public class TunnelHandlerNetFX : TunnelHandlerBase, IHttpHandler, IRouteHandler, ITunnelHandler
    {
        public override void ProcessRequest(HttpContext context) => throw new NotSupportedException("Handler cannot execute synchronously");

        public override async Task ProcessRequestAsync(HttpContext context)
        {
            if (context.IsWebSocketRequest)
            {
                string caller, method, args, argsx;
                caller = context.Request.QueryString["caller"];
                method = context.Request.QueryString["method"];
                if (string.IsNullOrEmpty(caller) || string.IsNullOrEmpty(method))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return;
                }

                context.AcceptWebSocketRequest(async webSocketContext =>
                {
                    var tunnel = new TunnelSocket(webSocketContext.WebSocket);

                    try
                    {
                        var args = await ReadArgumentsAsync(tunnel);

                        var dest = await GetTunnel(caller, method, args);
                        if (dest != null)
                        {
                            await Transmit(tunnel, dest);
                        }
                        else
                        {
                            await tunnel.CloseAsync(WebSocketCloseStatus.InternalServerError, "Cannot get a tunnel");
                        }
                    }
                    catch (Exception ex)
                    {
                        await tunnel.CloseAsync(WebSocketCloseStatus.InternalServerError, $"{ex.Message}\n{ex.StackTrace}");
                    }

                });
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext) => this;
        public bool IsReusable => true;
    }
#endif
}