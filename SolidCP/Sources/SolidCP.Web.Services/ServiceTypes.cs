using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
#if NETFRAMEWORK
using System.ServiceModel;
using System.Configuration;
#else
using CoreWCF;
#endif
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Web.Services
{
	public class ServiceType
	{
		public Type Service;
		public Type Contract;

		public ServiceType(Type type)
		{
			Service = type;
			Contract = type.GetInterfaces().FirstOrDefault(i => i.GetCustomAttribute<ServiceContractAttribute>() != null);
		}
	}

	public class ServiceTypes: KeyedCollection<string, ServiceType>
	{

		static string exposeWebServices = null;
		public static string ExposeWebServices
		{
			get
			{
				if (exposeWebServices == null)
				{
#if NETFRAMEWORK
					exposeWebServices = (ConfigurationManager.AppSettings["ExposeWebServices"] ?? "").ToLower();
#else
					exposeWebServices = Configuration.ExposeWebServices.ToLower();
#endif
				}
				return exposeWebServices;
			}
		}

		public static Assembly[] assemblies = null;

        public static Assembly[] ExposedAssemblies
		{
			get
			{
				if (assemblies != null) return assemblies;

				Assembly eserver = null, server = null;

				if (ExposeWebServices == "" || ExposeWebServices == "all" || ExposeWebServices == "true" ||
					ExposeWebServices.Split(';', ',').Any(s => s.Trim() == "enterpriseserver"))
				{
					try
					{
						eserver = Assembly.Load("SolidCP.EnterpriseServer");
					}
					catch { }
				}

				if (ExposeWebServices == "" || ExposeWebServices == "all" || ExposeWebServices == "true" ||
					ExposeWebServices.Split(';', ',').Any(s => s.Trim() == "server"))
				{
					try
					{
						server = Assembly.Load("SolidCP.Server");
					}
					catch { }
				}

				assemblies = new Assembly[]
				{
					eserver, server
				}
				.Where(a => a != null)
				.ToArray();

				return assemblies;
			}
		}

        public bool IsServerLoaded => AppDomain.CurrentDomain.GetAssemblies()
            .Any(a => a.GetName().Name == "SolidCP.Server");
        public bool IsEnterpriseServerLoaded => AppDomain.CurrentDomain.GetAssemblies()
            .Any(a => a.GetName().Name == "SolidCP.EnterpriseServer");
        public bool IsPortalLoaded => AppDomain.CurrentDomain.GetAssemblies()
            .Any(a => a.GetName().Name == "SolidCP.WebPortal");

        public static IEnumerable<Type> GetWebServices()
		{
			var types = ExposedAssemblies
				.SelectMany(a => {
					var attrTypes = a.GetCustomAttribute<WCFServiceTypesAttribute>()?.Types;
					return attrTypes ?? new Type[0];
				});
			return types;
		}

		protected override string GetKeyForItem(ServiceType type) => type.Service.Name;

		static ServiceTypes types = null;
		public static ServiceTypes Types
		{
			get
			{
				if (types == null)
				{
					types = new ServiceTypes();
					foreach (var type in GetWebServices())
					{
						types.Add(new ServiceType(type));
					}
				}
				return types;
			}
		}
	}
}
