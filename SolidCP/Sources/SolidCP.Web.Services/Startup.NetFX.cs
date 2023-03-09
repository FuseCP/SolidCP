#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Services;
using System.Web.Routing;
using Microsoft.Web.Infrastructure;
using System.ComponentModel;

//[assembly: PreApplicationStartMethod(typeof(SolidCP.Web.Services.StartupFX), "Start")]
namespace SolidCP.Web.Services
{
	public static class StartupFX
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
		public static void Start()
		{

			lock (startLock)
			{
				if (wasCalled) return;
				wasCalled = true;
			}

			Assembly eserver = null, server = null;

			try
			{
				eserver = Assembly.Load("SolidCP.EnterpriseServer");
			} catch { }
			try
			{
				server = Assembly.Load("SolidCP.Server");
			} catch { }
			ServiceAssemblies = new Assembly[]
			{
				eserver, server
			}
			.Where(a => a != null)
			.ToArray();

			var attributeType = Assembly.Load("SolidCP.Web.Services").GetType("System.Web.Services.WebServiceAttribute");

			var types = ServiceAssemblies
				.SelectMany(a => a.GetLoadableTypes())
				.ToArray();
			var webServices = types
					.Where(t => t.GetCustomAttribute(attributeType) != null &&
						(t.GetInterfaces().Any(i => i.GetCustomAttribute<ServiceContractAttribute>() != null)))
					.ToArray();
			AddServiceRoutes(webServices);
			//SvcVirtualPathProvider.SetupSvcServices(webServices);
			//DictionaryVirtualPathProvider.Startup();
		}

		static void AddServiceRoutes(Type[] services)
		{
			foreach (var service in services)
			{
				RouteTable.Routes.Add(new ServiceRoute(service.Name, new ServiceHostFactory(), service));
				RouteTable.Routes.Add(new ServiceRoute($"/basic/{service.Name}", new ServiceHostFactory(), service));
				RouteTable.Routes.Add(new ServiceRoute($"/ws/{service.Name}", new ServiceHostFactory(), service));
				RouteTable.Routes.Add(new ServiceRoute($"/net/{service.Name}", new ServiceHostFactory(), service));
				RouteTable.Routes.Add(new ServiceRoute($"/ssl/{service.Name}", new ServiceHostFactory(), service));
			}
		}
	}
}
#endif