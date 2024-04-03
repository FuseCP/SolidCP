using System;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SolidCP.Providers;
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

        public static ConcurrentDictionary<string, Task<TunnelSocket>> Sockets = new ConcurrentDictionary<string, Task<TunnelSocket>>();

        public async Task<TunnelSocket> GetSocket(string caller, string method, string arguments)
        {
            var key = $"{caller}/{method}/{arguments}";
            var socket = Sockets.GetOrAdd(key, async key =>
            {
                var service = new TunnelService(caller);
                return await service.Service.GetSocket(method, arguments);
            });
            return await socket;
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
                string caller, method, arguments;
                try
                {
                    caller = context.Request.Query["caller"];
                    method = context.Request.Query["method"];
                    arguments = context.Request.Query["args"];
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return;
                }

                using (var webSocket = await context.WebSockets.AcceptWebSocketAsync())
                {
                    var tunnel = new TunnelSocket(webSocket);
                    await tunnel.Transmit(await GetSocket(caller, method, arguments));
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
                string caller, method, arguments;
                try
                {
                    caller = context.Request.QueryString["caller"];
                    method = context.Request.QueryString["method"];
                    arguments = context.Request.QueryString["arguments"];
                } catch (Exception ex)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return;
                }
                
                context.AcceptWebSocketRequest(async context =>
                {
                    var tunnel = new TunnelSocket(context.WebSocket);
                    await tunnel.Transmit(await GetSocket(caller, method, arguments));
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