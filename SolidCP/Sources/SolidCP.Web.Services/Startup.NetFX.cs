using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
#if NETFRAMEWORK
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
#else
using CoreWCF;
#endif
//using Microsoft.Web.Infrastructure;
using System.ComponentModel;

//[assembly: PreApplicationStartMethod(typeof(SolidCP.Web.Services.StartupFX), "Start")]
namespace SolidCP.Web.Services
{
	public static class StartupNetFX
	{

		public static Assembly[] ServiceAssemblies { get; set; }

		public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
		{
			if (assembly == null) throw new ArgumentNullException(nameof(assembly));
			try
			{
				return assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException e)
			{
				return e.Types.Where(t => t != null);
			}
		}

		static object startLock = new object();
		static bool wasCalled = false;

		public static Type[] GetWebServices()
		{
			Assembly eserver = null, server = null;

			try
			{
				eserver = Assembly.Load("SolidCP.EnterpriseServer");
			}
			catch { }
			try
			{
				server = Assembly.Load("SolidCP.Server");
			}
			catch { }
			ServiceAssemblies = new Assembly[]
			{
				eserver, server
			}
			.Where(a => a != null)
			.ToArray();

			var types = ServiceAssemblies
				.SelectMany(a => a.GetLoadableTypes())
				.ToArray();
			var webServices = types

/* Unmerged change from project 'SolidCP.Web.Services (net48)'
Before:
					.Where(t => t.GetCustomAttribute<System.Web.Services.WebServiceAttribute>() != null &&
After:
					.Where(t => t.GetCustomAttribute<Services.WebServiceAttribute>() != null &&
*/
					.Where(t => t.GetCustomAttribute<WebServiceAttribute>() != null &&
						(t.GetInterfaces().Any(i => i.GetCustomAttribute<ServiceContractAttribute>() != null)))
					.ToArray();
			return webServices;
		}

		public static void Start()
		{

			lock (startLock)
			{
				if (wasCalled) return;
				wasCalled = true;
			}

			AddServiceRoutes(GetWebServices());
			//SvcVirtualPathProvider.SetupSvcServices(webServices);
			//DictionaryVirtualPathProvider.Startup();
		}

		static void AddServiceRoutes(Type[] services)
		{
#if NETFRAMEWORK
			foreach (var service in services)
			{
				RouteTable.Routes.Add(new ServiceRoute(service.Name, new ServiceHostFactory(), service));
				RouteTable.Routes.Add(new ServiceRoute($"basic/{service.Name}", new ServiceHostFactory(), service));
				RouteTable.Routes.Add(new ServiceRoute($"ws/{service.Name}", new ServiceHostFactory(), service));
				RouteTable.Routes.Add(new ServiceRoute($"net/{service.Name}", new ServiceHostFactory(), service));
				RouteTable.Routes.Add(new ServiceRoute($"nettcp/{service.Name}", new ServiceHostFactory(), service));
                RouteTable.Routes.Add(new ServiceRoute($"pipe/{service.Name}", new ServiceHostFactory(), service));
            }
#endif
        }
	}
}