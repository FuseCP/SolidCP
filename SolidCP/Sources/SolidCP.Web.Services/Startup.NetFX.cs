using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
#if NETFRAMEWORK
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using System.Web.Configuration;
using SwaggerWcf;
#endif
//using Microsoft.Web.Infrastructure;
using System.ComponentModel;

//[assembly: PreApplicationStartMethod(typeof(SolidCP.Web.Services.StartupFX), "Start")]
namespace SolidCP.Web.Services
{
	public static class StartupNetFX
	{

		static int initialized = 0;
		public static void Start()
		{
#if NETFRAMEWORK
			if (Interlocked.CompareExchange(ref initialized, 1, 0) != 0) return;

			AddServiceRoutes(ServiceTypes.Types.Select(srvc => srvc.Service));
			//SvcVirtualPathProvider.SetupSvcServices(webServices);
			//DictionaryVirtualPathProvider.Startup();

			// set Log trace switch, as it is not working
#endif
		}

		static void AddServiceRoutes(IEnumerable<Type> services)
		{
#if NETFRAMEWORK
			foreach (var service in services)
			{
				RouteTable.Routes.Add(new ServiceRoute(service.Name, new ServiceHostFactory(), service));
				RouteTable.Routes.Add(new ServiceRoute($"basic/{service.Name}", new ServiceHostFactory(), service));
				RouteTable.Routes.Add(new ServiceRoute($"ws/{service.Name}", new ServiceHostFactory(), service));
				RouteTable.Routes.Add(new ServiceRoute($"net/{service.Name}", new ServiceHostFactory(), service));
				RouteTable.Routes.Add(new ServiceRoute($"tcp/{service.Name}", new ServiceHostFactory(), service));
                RouteTable.Routes.Add(new ServiceRoute($"pipe/{service.Name}", new ServiceHostFactory(), service));
				RouteTable.Routes.Add(new ServiceRoute($"tcp/ssl/{service.Name}", new ServiceHostFactory(), service));
                RouteTable.Routes.Add(new ServiceRoute($"pipe/ssl/{service.Name}", new ServiceHostFactory(), service));
                RouteTable.Routes.Add(new ServiceRoute($"api/{service.Name}", new ServiceHostFactory(), service));
			}
			RouteTable.Routes.Add(new ServiceRoute("api-docs", new WebServiceHostFactory(), typeof(SwaggerWcfEndpoint)));

			var tunnelHandler = new TunnelHandlerNetFX();
			RouteTable.Routes.Add(new Route(tunnelHandler.Route, tunnelHandler));
#endif
		}
	}
}