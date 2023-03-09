#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace SolidCP.Web.Services
{

	public class SvcFile : VirtualFile
	{
		public Type Service { get; set; }

		public SvcFile(Type service, string name) : base(Paths.Absolute($"~/{name}"), DictionaryVirtualPathProvider.Current)
		{
			Service = service;
		}
		public override Stream Open()
		{
			var text = $"<% @ServiceHost Service = \"{Service.FullName}\" Factory = \"SolidCP.Web.Services.ServiceHostFactory, SolidCP.Web.Services\" %>";
			return new MemoryStream(Encoding.UTF8.GetBytes(text));
		}
	}
	public class SvcVirtualPathProvider
	{
		public static void SetupSvcServices(IEnumerable<Type> services)
		{
			foreach (var service in services)
			{
				var name = service.Name;
				if (name.EndsWith("Service")) name = name.Substring(0, name.Length - "Service".Length); 
				var file = new SvcFile(service, $"{name}.svc");
				DictionaryVirtualPathProvider.Current.Add(file);
			}
			//DynamicModuleUtility.RegisterModule(typeof(SvcHttpModule));
		}
	}
}
#endif