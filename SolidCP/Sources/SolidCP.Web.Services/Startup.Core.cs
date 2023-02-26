using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Linq;
#if NET
using CoreWCF;
using CoreWCF.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
#endif

namespace SolidCP.Web.Services
{
	public class CoreWebServicesApp
	{

		static Assembly Server;

		public static void Init(string[] args, Assembly server)
		{
			Server = server;
#if NET
			var app = WebHost.CreateDefaultBuilder(args)
				.UseStartup<CoreWebServicesApp>()
				.Build();
			app.Run();
#endif
		}

#if NET
		public void ConfigureServices(IServiceCollection services)
		{
			//Enable CoreWCF Services, with metadata (WSDL) support
			services.AddServiceModelServices()
				.AddServiceModelMetadata();
		}

		public void Configure(IApplicationBuilder app)
		{
			app.UseServiceModel(builder =>
			{
				var webServices = Server.DefinedTypes
					.Where(t => t.GetCustomAttribute<WebServiceAttribute>() != null)
					.Select(ws => new
					{
						Service = ws,
						Contract = ws.GetInterfaces().FirstOrDefault(i => i.GetCustomAttribute<ServiceContractAttribute>() != null)
					})
					.Where(ws => ws.Contract != null);

				var basicHttpBinding = new BasicHttpBinding(CoreWCF.Channels.BasicHttpSecurityMode.TransportWithMessageCredential);

				foreach (var ws in webServices)
				{
					// Add the Calculator Service
					builder.AddService(ws.Service, serviceOptions => { })
						// Add BasicHttpBinding
						.AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, new Uri($"/{ws.Service.Name}/http"), new Uri($"/{ws.Service.Name}/http"));
				}

				// Configure WSDL to be available
				var serviceMetadataBehavior = app.ApplicationServices.GetRequiredService<CoreWCF.Description.ServiceMetadataBehavior>();
				serviceMetadataBehavior.HttpGetEnabled = true;
			});
		}
#endif
	}
}
