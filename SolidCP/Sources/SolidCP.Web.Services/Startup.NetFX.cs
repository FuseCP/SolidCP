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
using Microsoft.Web.Infrastructure;

[assembly: PreApplicationStartMethod(typeof(SolidCP.Web.Services.Startup), "Start")]
namespace SolidCP.Web.Services
{
	public class Startup
	{
		public void Start()
		{
			var assemblys = new Assembly[]
			{
				Assembly.Load("SolidCP.EnterpriseServer"),
				Assembly.Load("SolidCP.Server")
			}
			.Where(a => a != null)
			.ToArray();

			var attributeType = Assembly.Load("SolidCP.Web.Services").GetType("System.Web.Services.WebServiceAttribute");
			var webServices = assemblys
				.SelectMany(a => a.DefinedTypes
					.Where(t => t.GetCustomAttribute(attributeType) != null)
					.Select(ws => new
					{
						Service = ws,
						Contract = ws.GetInterfaces().FirstOrDefault(i => i.GetCustomAttribute<ServiceContractAttribute>() != null)
					})
					.Where(ws => ws.Contract != null));

					
		}
	}
}
#endif