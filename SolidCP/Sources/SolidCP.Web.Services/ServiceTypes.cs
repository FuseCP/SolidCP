using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
#if NETFRAMEWORK
using System.ServiceModel;
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
		public static Assembly[] Assemblies { get; set; }

		public static IEnumerable<Type> GetWebServices()
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
			Assemblies = new Assembly[]
			{
				eserver, server
			}
			.Where(a => a != null)
			.ToArray();

			var types = Assemblies
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
