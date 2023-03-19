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
#if NETFRAMEWORK
using Microsoft.Web.Services3;
using System.ServiceModel;
#else
using CoreWCF;
#endif
using System.Security.Cryptography.X509Certificates;
using SolidCP.Web.Services;

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
			/* var app = WebHost.CreateDefaultBuilder(args)
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
			app.Run(); */

			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddRazorPages();
			//builder.Services.AddHttpContextAccessor();
  			ConfigureServices(builder.Services);

            builder.WebHost.UseKestrel(options =>
			{
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
			.UseNetTcp(NETTCP_PORT);

			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseDefaultFiles()
				.UseStaticFiles()
				.UseRouting();

			app.MapRazorPages();

			ConfigureWCF(app);

			app.Run();
		}

		public static void ConfigureServices(IServiceCollection services)
		{
			//Enable CoreWCF Services, with metadata (WSDL) support
			services
				.AddServiceModelServices()
				.AddServiceModelMetadata()
				.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();
		}

		public static void ConfigureWCF(IApplicationBuilder app)
		{
			app.UseServiceModel(builder =>
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

                /*builder.ConfigureAllServiceHostBase(host => {
                    host.Credentials.ServiceCertificate
                        .SetCertificate("localhost", StoreLocation.LocalMachine, StoreName.My);
                    host.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = CoreWCF.Security.UserNamePasswordValidationMode.Custom;
                    host.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new UserNamePasswordValidator();
                });*/

                foreach (var ws in webServices)
				{
					BasicHttpBinding basicHttpBinding;
					WSHttpBinding wsHttpBinding;
					NetHttpBinding netHttpBinding;
					NetTcpBinding netTcpBinding;
					Uri basicUri, wsHttpUri, netHttpUri, netTcpUri, defaultUri;
					if (ws.IsAuth) {
						basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.TransportWithMessageCredential);
						basicUri = new Uri($"https://{HOST_IN_WSDL}:{HTTPS_PORT}/basic/{ws.Service.Name}");
						wsHttpBinding = new WSHttpBinding(CoreWCF.SecurityMode.TransportWithMessageCredential);
						wsHttpUri = new Uri($"https://{HOST_IN_WSDL}:{HTTPS_PORT}/ws/{ws.Service.Name}");
						netHttpBinding = new NetHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
						netHttpUri = new Uri($"https://{HOST_IN_WSDL}:{HTTPS_PORT}/net/{ws.Service.Name}");
						netTcpBinding = new NetTcpBinding(CoreWCF.SecurityMode.TransportWithMessageCredential);
						netTcpUri = new Uri($"net.tcp://{HOST_IN_WSDL}:{NETTCP_PORT}/net.tcp/ssl/{ws.Service.Name}");
                        defaultUri = new Uri($"https://{HOST_IN_WSDL}:{HTTPS_PORT}/{ws.Service.Name}");
					}
					else {
						basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
						basicUri = new Uri($"http://{HOST_IN_WSDL}:{HTTP_PORT}/basic/{ws.Service.Name}");
						wsHttpBinding = new WSHttpBinding(CoreWCF.SecurityMode.None);
						wsHttpUri = new Uri($"http://{HOST_IN_WSDL}:{HTTP_PORT}/ws/{ws.Service.Name}");
						netHttpBinding = new NetHttpBinding(BasicHttpSecurityMode.None);
						netHttpUri = new Uri($"http://{HOST_IN_WSDL}:{HTTP_PORT}/net/{ws.Service.Name}");
						netTcpBinding = new NetTcpBinding(CoreWCF.SecurityMode.None);
						netTcpUri = new Uri($"net.tcp://{HOST_IN_WSDL}:{NETTCP_PORT}/net.tcp/{ws.Service.Name}");
						defaultUri = new Uri($"http://{HOST_IN_WSDL}:{HTTP_PORT}/{ws.Service.Name}");
                    }


					// Add the Calculator Service
					builder.AddService(ws.Service, serviceOptions =>
						{
							/* serviceOptions.BaseAddresses.Add(new Uri($"http://{HOST_IN_WSDL}:{HTTP_PORT}"));
							serviceOptions.BaseAddresses.Add(new Uri($"https://{HOST_IN_WSDL}:{HTTPS_PORT}"));
							serviceOptions.BaseAddresses.Add(new Uri($"net.tcp://{HOST_IN_WSDL}:{NETTCP_PORT}")); */
						})
						.AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, basicUri, basicUri)
						.AddServiceEndpoint(ws.Service, ws.Contract, wsHttpBinding, wsHttpUri, wsHttpUri)
						.AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, netHttpUri, netHttpUri)
						.AddServiceEndpoint(ws.Service, ws.Contract, netTcpBinding, netTcpUri, netTcpUri)
						.AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, defaultUri, defaultUri);
				}

				// Configure WSDL to be available
				var serviceMetadataBehavior = app.ApplicationServices.GetRequiredService<ServiceMetadataBehavior>();
				serviceMetadataBehavior.HttpGetEnabled = serviceMetadataBehavior.HttpsGetEnabled = true;
			});
			
		}
	}
}
#endif
