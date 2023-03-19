#if !NETFRAMEWORK
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CoreWCF;
using CoreWCF.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CoreWCF.Description;
using CoreWCF.Channels;
using System.Diagnostics;
using Microsoft.Web.Services3;

namespace SolidCP.Web.Services
{
	public class CoreWebServicesApp
	{
		public const int HTTP_PORT = 9009;
		public const int HTTPS_PORT = 9010;
		public const int NETTCP_PORT = 9011;
		// Only used on case that UseRequestHeadersForMetadataAddressBehavior is not used
		public const string HOST_IN_WSDL = "localhost";

		public static void Init(string[] args)
		{
			var app = WebHost.CreateDefaultBuilder(args)
				.UseKestrel(options => {
					options.ListenAnyIP(HTTP_PORT);
					options.ListenAnyIP(HTTPS_PORT, listenOptions =>
					{
						listenOptions.UseHttps();
						if (Debugger.IsAttached)
						{
							listenOptions.UseConnectionLogging();
						}
					});
				})
			.UseNetTcp(NETTCP_PORT)
			.UseStartup<CoreWebServicesApp>()
			.Build();
			app.Run();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			//Enable CoreWCF Services, with metadata (WSDL) support
			services
				.AddServiceModelServices()
				.AddServiceModelMetadata()
				.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>()
				.AddRazorPages();
		}

		public void Configure(IApplicationBuilder app)
		{
			app.UseDefaultFiles()
				.UseStaticFiles()
				.UseRouting()
				.UseEndpoints(ep => ep.MapRazorPages())
				.UseServiceModel(builder =>
			{
				var webServices = StartupFX.GetWebServices()
					.Select(ws => new
					{
						Service = ws,
						Contract = ws.GetInterfaces().FirstOrDefault(i => i.GetCustomAttribute<ServiceContractAttribute>() != null)
					})
					.Where(ws => ws.Contract != null)
					.Select(ws => new
					{
						Service = ws.Service,
						Contract = ws.Contract,
						IsAuth = ws.Contract.GetCustomAttribute<PolicyAttribute>() != null
					});


				foreach (var ws in webServices)
				{
					BasicHttpBinding basicHttpBinding;
					WSHttpBinding wsHttpBinding;
					NetHttpBinding netHttpBinding;
					NetTcpBinding netTcpBinding;
					if (ws.IsAuth) {
						basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.TransportWithMessageCredential);
						wsHttpBinding = new WSHttpBinding(SecurityMode.TransportWithMessageCredential);
						netHttpBinding = new NetHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
						netTcpBinding = new NetTcpBinding(SecurityMode.TransportWithMessageCredential);
					} else {
						basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
						wsHttpBinding = new WSHttpBinding(SecurityMode.None);
						netHttpBinding = new NetHttpBinding(BasicHttpSecurityMode.None);
						netTcpBinding = new NetTcpBinding(SecurityMode.None);
					}
					// Add the Calculator Service
					builder.AddService(ws.Service, serviceOptions => { })
						// Add BasicHttpBinding
						.AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, new Uri($"/basic/{ws.Service.Name}"), new Uri($"/basic/{ws.Service.Name}"))
						.AddServiceEndpoint(ws.Service, ws.Contract, wsHttpBinding, new Uri($"/ws/{ws.Service.Name}"), new Uri($"/ws/{ws.Service.Name}"))
						.AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, new Uri($"/net/{ws.Service.Name}"), new Uri($"/net/{ws.Service.Name}"))
						.AddServiceEndpoint(ws.Service, ws.Contract, netTcpBinding, new Uri($"/net.tcp/{ws.Service.Name}"), new Uri($"/net.tcp/{ws.Service.Name}"))
						.AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, new Uri($"/{ws.Service.Name}"), new Uri($"/{ws.Service.Name}"));
				}

				// Configure WSDL to be available
				var serviceMetadataBehavior = app.ApplicationServices.GetRequiredService<CoreWCF.Description.ServiceMetadataBehavior>();
				serviceMetadataBehavior.HttpGetEnabled = serviceMetadataBehavior.HttpsGetEnabled = true;
			});
		}
	}
}
#endif
