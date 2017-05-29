using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using SolidCP.WebDavPortal.DependencyInjection;
using SolidCP.WebDavPortal.UI.Routes;

namespace SolidCP.WebDavPortal.App_Start
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            #region Owa

            configuration.Routes.MapHttpRoute(
                name: OwaRouteNames.GetFile,
                routeTemplate: "owa/wopi*/files/{accessTokenId}/contents",
                defaults: new {controller = "Owa", action = "GetFile"});

            configuration.Routes.MapHttpRoute(
                name: OwaRouteNames.CheckFileInfo,
                routeTemplate: "owa/wopi*/files/{accessTokenId}",
                defaults: new {controller = "Owa", action = "CheckFileInfo"});

            #endregion



            configuration.Routes.MapHttpRoute("API Default", "api/{controller}/{id}",
                new { id = RouteParameter.Optional });

            configuration.DependencyResolver = new NinjectDependecyResolver();
        }
    }
}