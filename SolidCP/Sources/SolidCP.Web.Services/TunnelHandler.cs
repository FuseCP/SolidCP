using System;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
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
    {
        public string Route => "Tunnel";

        public async Task<TunnelSocket> GetTunnel(string caller, string method, string arguments, bool encrypted)
        {
            var service = new TunnelService(caller);
            return await service.Service.GetTunnel(method, arguments, encrypted);
        }
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
                string caller, method, args, argsx;
                try
                {
                    caller = context.Request.Query["caller"];
                    method = context.Request.Query["method"];
                    args = context.Request.Query["args"];
                    argsx = context.Request.Query["argsx"];
                    if (string.IsNullOrEmpty(args)) args = argsx;
                    if (string.IsNullOrEmpty(caller) || string.IsNullOrEmpty(method) || string.IsNullOrEmpty(args))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return;
                }

                using (var webSocket = await context.WebSockets.AcceptWebSocketAsync())
                {
                    var tunnel = new TunnelSocket(webSocket);
                    var dest = await GetTunnel(caller, method, args, !string.IsNullOrEmpty(argsx));
                    if (dest != null)
                    {
                        await tunnel.ProvideUpgradeTunnelSocketAsync(dest);
                        await tunnel.Transmit(dest);
                    } else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.FailedDependency;
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
        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
            {
                string caller, method, args, argsx;
                try
                {
                    caller = context.Request.QueryString["caller"];
                    method = context.Request.QueryString["method"];
                    args = context.Request.QueryString["args"];
                    argsx = context.Request.QueryString["argsx"];
                    if (string.IsNullOrEmpty(args)) args = argsx;
                } catch (Exception ex)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return;
                }
                
                context.AcceptWebSocketRequest(async webSocketContext =>
                {
                    var tunnel = new TunnelSocket(webSocketContext.WebSocket);
                    await tunnel.Transmit(await GetTunnel(caller, method, args, !string.IsNullOrEmpty(argsx)));
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