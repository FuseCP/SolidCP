using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Net;
using SolidCP.Providers.OS;
using SolidCP.EnterpriseServer.Client;
using SolidCP.Portal;

namespace SolidCP.WebPortal
{
    public class VncWebSocketHandler : IHttpHandler, IRouteHandler
    {
        public bool IsReusable => true;

        public IHttpHandler GetHttpHandler(RequestContext requestContext) => this;

        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
            {
                var query = context.Request.QueryString;
                var user = query["user"];
                var password = query["password"];
                if (string.IsNullOrEmpty(password))
                {
                    var authTicket = PortalUtils.AuthTicket;
                    if (authTicket != null)
                    {
                        password = authTicket.UserData.Substring(0, authTicket.UserData.IndexOf(Environment.NewLine));
                    } else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        return;
                    }
                }
                var item = query["item"];
                int itemId;

                if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(item) || !int.TryParse(item, out itemId))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                } else
                {
                    context.AcceptWebSocketRequest(async socketContext =>
                    {
                        var incoming = new TunnelSocket(socketContext.WebSocket);
                        var esclient = new EnterpriseServerTunnelClient();
                        esclient.Username = user;
                        esclient.Password = password;
                        esclient.ServerUrl = PortalConfiguration.SiteSettings["EnterpriseServer"];
                        try
                        {
                            var outgoing = await esclient.GetPveVncWebSocketAsync(itemId);
                            await incoming.Transmit(outgoing);
                        } catch (Exception ex)
                        {
                        }
                    });
                }

            } else
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }

        }

        public static void Init()
        {
            RouteTable.Routes.Add(new Route("novnc/websocket", new VncWebSocketHandler()));
        }
    }
}