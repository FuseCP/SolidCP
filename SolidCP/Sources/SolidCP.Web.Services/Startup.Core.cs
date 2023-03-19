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
using System.Diagnostics.Eventing.Reader;
using Microsoft.Extensions.Configuration;

namespace SolidCP.Web.Services
{
    public static class CoreWebServicesApp
	{
		public static int? HttpPort = null;
		public static int? HttpsPort = null;
		public static int? NetTcpPort = null;
		// Only used on case that UseRequestHeadersForMetadataAddressBehavior is not used
		public static string? HttpHost = null;
		public static string? HttpsHost = null;
		public static string? NetTcpHost = null;
		public static StoreLocation StoreLocation = StoreLocation.LocalMachine;
		public static StoreName StoreName = StoreName.My;
		public static X509FindType FindType = X509FindType.FindBySubjectName;
		public static object Name = "localhost";

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
			var urls = builder.Configuration["applicationUrls"];
			foreach (var url in urls.Split(';'))
			{
				var uri = new Uri(url);
				if (uri.Scheme == "http")
				{
					HttpPort = uri.Port;
					HttpHost = uri.Host;
				}
				else if (uri.Scheme == "https")
				{
					HttpsPort = uri.Port;
					HttpsHost = uri.Host;
				}
				else if (uri.Scheme == "net.tcp")
				{
					NetTcpPort = uri.Port;
					NetTcpHost = uri.Host;
				}
			}
			StoreLocation = builder.Configuration.GetValue<StoreLocation?>("ServerCertificate:StoreLocation") ?? StoreLocation.LocalMachine;
			StoreName = builder.Configuration.GetValue<StoreName?>("ServerCertificate:StoreName") ?? StoreName.My;
			FindType = builder.Configuration.GetValue<X509FindType?>("ServerCertificate:FindType") ?? X509FindType.FindBySubjectName;
			Name = builder.Configuration.GetValue<object?>("ServerCertificate:Name") ?? "localhost";

			builder.Services.AddRazorPages();
			//builder.Services.AddHttpContextAccessor();
  			ConfigureServices(builder.Services);

			if (NetTcpPort.HasValue) builder.WebHost.UseNetTcp(NetTcpPort.Value);

				/*.UseKestrel(options =>
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
			}) */

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

		public static IServiceBuilder AddServiceEndpoint(this IServiceBuilder builder, Type service, Type contract, Binding binding, Uri address)
		{
			builder.AddServiceEndpoint(service, contract, binding, address, address, conf => conf.EndpointBehaviors.Add(new SoapHeaderMessageInspector()));
			return builder;
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
					.Where(ws => ws.Contract != null);

                builder.ConfigureAllServiceHostBase(host => {
                    host.Credentials.ServiceCertificate
                        .SetCertificate(StoreLocation, StoreName, FindType, Name);
                    host.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = CoreWCF.Security.UserNamePasswordValidationMode.Custom;
                    host.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new UserNamePasswordValidator();
                    var behavior = host.Description.Behaviors.Find<ServiceDebugBehavior>();
                    if (behavior != null) behavior.IncludeExceptionDetailInFaults = true;
                });

                foreach (var ws in webServices)
				{
                    var policy = ws.Contract.GetCustomAttributes(false).OfType<Services.PolicyAttribute>().FirstOrDefault();
                    var isAuthenticated = policy != null;

                    var service = builder.AddService(ws.Service, serviceOptions =>
                    {
                    });

                    BasicHttpBinding basicHttpBinding;
					WSHttpBinding wsHttpBinding;
					NetHttpBinding netHttpBinding;
					NetTcpBinding netTcpBinding = null;
					Uri basicUri, wsHttpUri, netHttpUri, netTcpUri = null, defaultUri;
					if (isAuthenticated)
					{
						if (HttpsPort.HasValue)
						{
							basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.TransportWithMessageCredential);
							basicUri = new Uri($"https://{HttpsHost}:{HttpsPort}/basic/{ws.Service.Name}");
							wsHttpBinding = new WSHttpBinding(CoreWCF.SecurityMode.TransportWithMessageCredential);
							wsHttpUri = new Uri($"https://{HttpsHost}:{HttpsPort}/ws/{ws.Service.Name}");
							netHttpBinding = new NetHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
							netHttpUri = new Uri($"https://{HttpsHost}:{HttpsPort}/net/{ws.Service.Name}");
							defaultUri = new Uri($"https://{HttpsHost}:{HttpsPort}/{ws.Service.Name}");
							service.AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, basicUri)
								.AddServiceEndpoint(ws.Service, ws.Contract, wsHttpBinding, wsHttpUri)
								.AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, netHttpUri)
								.AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, defaultUri);
						}
						if (NetTcpPort.HasValue)
						{
							netTcpBinding = new NetTcpBinding(CoreWCF.SecurityMode.TransportWithMessageCredential);
							netTcpUri = new Uri($"net.tcp://{NetTcpHost}:{NetTcpPort}/net.tcp/ssl/{ws.Service.Name}");
							service.AddServiceEndpoint(ws.Service, ws.Contract, netTcpBinding, netTcpUri);
						}
					}
					else
					{
						if (HttpPort.HasValue)
						{
							basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
							basicUri = new Uri($"http://{HttpHost}:{HttpPort}/basic/{ws.Service.Name}");
							wsHttpBinding = new WSHttpBinding(CoreWCF.SecurityMode.None);
							wsHttpUri = new Uri($"http://{HttpHost}:{HttpPort}/ws/{ws.Service.Name}");
							netHttpBinding = new NetHttpBinding(BasicHttpSecurityMode.None);
							netHttpUri = new Uri($"http://{HttpHost}:{HttpPort}/net/{ws.Service.Name}");
							defaultUri = new Uri($"http://{HttpHost}:{HttpPort}/{ws.Service.Name}");
							service.AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, basicUri)
								.AddServiceEndpoint(ws.Service, ws.Contract, wsHttpBinding, wsHttpUri)
								.AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, netHttpUri)
								.AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, defaultUri);
						}
						if (HttpsPort.HasValue)
						{
							basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.TransportWithMessageCredential);
							basicUri = new Uri($"https://{HttpsHost}:{HttpsPort}/basic/{ws.Service.Name}");
							wsHttpBinding = new WSHttpBinding(CoreWCF.SecurityMode.TransportWithMessageCredential);
							wsHttpUri = new Uri($"https://{HttpsHost}:{HttpsPort}/ws/{ws.Service.Name}");
							netHttpBinding = new NetHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
							netHttpUri = new Uri($"https://{HttpsHost}:{HttpsPort}/net/{ws.Service.Name}");
							defaultUri = new Uri($"https://{HttpsHost}:{HttpsPort}/{ws.Service.Name}");
							service.AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, basicUri)
								.AddServiceEndpoint(ws.Service, ws.Contract, wsHttpBinding, wsHttpUri)
								.AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, netHttpUri)
								.AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, defaultUri);
						}
						if (NetTcpPort.HasValue)
						{
							netTcpBinding = new NetTcpBinding(CoreWCF.SecurityMode.None);
							netTcpUri = new Uri($"net.tcp://{NetTcpHost}:{NetTcpPort}/net.tcp/{ws.Service.Name}");
							service.AddServiceEndpoint(ws.Service, ws.Contract, netTcpBinding, netTcpUri);
						}
					}
                }

                // Configure WSDL to be available
                var serviceMetadataBehavior = app.ApplicationServices.GetRequiredService<ServiceMetadataBehavior>();
				serviceMetadataBehavior.HttpGetEnabled = serviceMetadataBehavior.HttpsGetEnabled = true;
			});
			
		}
	}
}
#endif
