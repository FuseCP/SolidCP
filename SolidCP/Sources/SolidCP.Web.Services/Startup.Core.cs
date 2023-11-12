#if !NETFRAMEWORK
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using CoreWCF;
using CoreWCF.Description;
using CoreWCF.Channels;
using CoreWCF.Configuration;

using System.Security.Cryptography.X509Certificates;
using SolidCP.Web.Services;
using System.Diagnostics.Eventing.Reader;
using Microsoft.Extensions.Configuration;
using System.Runtime.Intrinsics.X86;
using System.IO;

namespace SolidCP.Web.Services
{
	public static class StartupCore
	{

		public static void Log(string msg)
		{
			Console.WriteLine(msg);
			if (Debugger.IsAttached) Debugger.Log(1, "SolidCP", msg);
		}
		public static void Error(string msg)
		{
			var col = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Log(msg);
			Console.ForegroundColor = col;
		}

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
		public static string? Name = null;
		public static string? CertificateFile = null;
		public static string? CertificatePassword = null;
		public static string Password;
		public static string ProbingPaths = "";
		public static string AllowedHosts = "0.0.0.0";
		public static bool IsLocalService = false;
		public static X509Certificate2 Certificate = null;

		public static void Init(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			var ProbingPaths = builder.Configuration["probingPaths"];
			AssemblyLoaderNetCore.Init();
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
			Name = builder.Configuration.GetValue<string?>("ServerCertificate:Name") ?? null;
			CertificateFile = builder.Configuration.GetValue<string?>("ServerCertificate:File");
			CertificatePassword = builder.Configuration.GetValue<string?>("ServerCertificate:Password");
			Password = builder.Configuration.GetValue<string?>("Server:Password") ?? String.Empty;
			AllowedHosts = builder.Configuration.GetValue<string?>("AllowedHosts") ?? "0.0.0.0";

			IsLocalService = AllowedHosts.Split(';')
				.All(host => host == "localhost" || host == "127.0.0.1" || host == "::1" ||
					Regex.IsMatch(host, "^192\\.168\\.[0-9]+\\.[0-9]+$")); // local network ip

			builder.Services.AddRazorPages();
			//builder.Services.AddHttpContextAccessor();
			ConfigureServices(builder.Services);

			if (NetTcpPort.HasValue) builder.WebHost.UseNetTcp(NetTcpPort.Value);

			builder.WebHost.UseKestrel(options =>
			{
				if (HttpPort.HasValue) options.ListenAnyIP(HttpPort.Value, listenOptions =>
				{
					if (Debugger.IsAttached) listenOptions.UseConnectionLogging();
				});
				if (HttpsPort.HasValue) options.ListenAnyIP(HttpsPort.Value, listenOptions =>
				{
					listenOptions.UseHttps(listenOptions =>
					{
						if (string.IsNullOrEmpty(CertificateFile) || string.IsNullOrEmpty(CertificatePassword))
						{
							if (!string.IsNullOrEmpty(Name))
							{
								X509Store store = new X509Store(StoreName, StoreLocation);
								store.Open(OpenFlags.ReadOnly);
								Certificate = store.Certificates.Find(FindType, Name, false).FirstOrDefault();
								if (Certificate != null) Log($"Use certificate {Certificate.GetNameInfo(X509NameType.SimpleName, false)} {Certificate.FriendlyName} found in {StoreName} at {StoreLocation}");
								else Error($"Certificate for {Name} not found in {StoreName} at {StoreLocation}");
							}
						}
						else
						{
							var file = new DirectoryInfo(CertificateFile).FullName;
							if (File.Exists(file))
							{
								var certs = new X509Certificate2Collection();
								certs.Import(file, CertificatePassword, X509KeyStorageFlags.DefaultKeySet);
								Certificate = certs.FirstOrDefault();
								if (Certificate != null) Log($"Use certificate {Certificate.SubjectName} from {file}.");
								else Error($"The certificate {file} was not found.");
							}
						}

						// if (Certificate == null) return;

						listenOptions.ServerCertificate = Certificate;

					});

					if (Debugger.IsAttached) listenOptions.UseConnectionLogging();
				});
			});

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
				.AddServiceModelMetadata();
			//.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();
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
				var webServices = StartupNetFX.GetWebServices()
					.Select(ws => new
					{
						Service = ws,
						Contract = ws.GetInterfaces().FirstOrDefault(i => i.GetCustomAttribute<ServiceContractAttribute>() != null)
					})
					.Where(ws => ws.Contract != null);

				builder.ConfigureAllServiceHostBase(host =>
				{
					host.Credentials.ServiceCertificate
						.SetCertificate(StoreLocation, StoreName, FindType, Name);
					var behavior = host.Description.Behaviors.Find<ServiceDebugBehavior>();
					if (behavior != null) behavior.IncludeExceptionDetailInFaults = true;
					else
					{
						var debugBehavior = new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true };
						host.Description.Behaviors.Add(debugBehavior);
					}
				});

				var isLocal = StartupCore.IsLocalService;

				foreach (var ws in webServices)
				{
					var policy = ws.Contract.GetCustomAttributes(false).OfType<Services.PolicyAttribute>().FirstOrDefault();
					var isEncrypted = policy != null;

					BasicHttpBinding basicHttpBinding;
					WSHttpBinding wsHttpBinding;
					NetHttpBinding netHttpBinding;
					NetTcpBinding netTcpBinding = null, netTcpSslBinding = null;
					Uri basicUri, wsHttpUri, netHttpUri, netTcpUri = null, netTcpSslUri, defaultUri;

					builder.AddService(ws.Service, options =>
					{
						//if (HttpPort.HasValue) options.BaseAddresses.Add(new Uri($"http://{HttpHost}:{HttpPort}/{ws.Service.Name}"));
						//if (HttpsPort.HasValue) options.BaseAddresses.Add(new Uri($"https://{HttpsHost}:{HttpsPort}/{ws.Service.Name}"));
						//if (NetTcpPort.HasValue) options.BaseAddresses.Add(new Uri($"tcp.net://{NetTcpHost}:{NetTcpPort}/{ws.Service.Name}"));
					});

					if (isEncrypted)
					{
						if (HttpPort.HasValue && isLocal)
						{
							basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
							basicUri = new Uri($"http://{HttpHost}:{HttpPort}/basic/{ws.Service.Name}");
							wsHttpBinding = new WSHttpBinding(CoreWCF.SecurityMode.None);
							wsHttpUri = new Uri($"http://{HttpHost}:{HttpPort}/ws/{ws.Service.Name}");
							netHttpBinding = new NetHttpBinding(BasicHttpSecurityMode.None);
							netHttpUri = new Uri($"http://{HttpHost}:{HttpPort}/net/{ws.Service.Name}");
							defaultUri = new Uri($"http://{HttpHost}:{HttpPort}/{ws.Service.Name}");
							builder.AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, defaultUri)
								.AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, basicUri)
								.AddServiceEndpoint(ws.Service, ws.Contract, wsHttpBinding, wsHttpUri)
								.AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, netHttpUri);
						}
						if (HttpsPort.HasValue)
						{
							basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
							basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
							basicUri = new Uri($"https://{HttpsHost}:{HttpsPort}/basic/{ws.Service.Name}");
							wsHttpBinding = new WSHttpBinding(CoreWCF.SecurityMode.Transport);
							wsHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
							wsHttpUri = new Uri($"https://{HttpsHost}:{HttpsPort}/ws/{ws.Service.Name}");
							netHttpBinding = new NetHttpBinding(BasicHttpSecurityMode.Transport);
							netHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
							netHttpUri = new Uri($"https://{HttpsHost}:{HttpsPort}/net/{ws.Service.Name}");
							defaultUri = new Uri($"https://{HttpsHost}:{HttpsPort}/{ws.Service.Name}");
							builder.AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, defaultUri)
								.AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, basicUri)
								.AddServiceEndpoint(ws.Service, ws.Contract, wsHttpBinding, wsHttpUri)
								.AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, netHttpUri);
						}
						if (NetTcpPort.HasValue)
						{
							if (Certificate != null)
							{
								netTcpSslBinding = new NetTcpBinding(CoreWCF.SecurityMode.Transport);
								netTcpSslBinding.Security.Message.ClientCredentialType = MessageCredentialType.None;
								netTcpSslBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
								netTcpSslUri = new Uri($"net.tcp://{NetTcpHost}:{NetTcpPort}/tcp/ssl/{ws.Service.Name}");
								builder.AddServiceEndpoint(ws.Service, ws.Contract, netTcpSslBinding, netTcpSslUri);
							}
							else
							{
								if (isLocal)
								{
									netTcpSslBinding = new NetTcpBinding(CoreWCF.SecurityMode.None);
									netTcpUri = new Uri($"net.tcp://{NetTcpHost}:{NetTcpPort}/tcp/{ws.Service.Name}");
									builder.AddServiceEndpoint(ws.Service, ws.Contract, netTcpBinding, netTcpUri);
								}
							}
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
							builder.AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, defaultUri)
								.AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, basicUri)
								.AddServiceEndpoint(ws.Service, ws.Contract, wsHttpBinding, wsHttpUri)
								.AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, netHttpUri);
						}
						if (HttpsPort.HasValue)
						{
							basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
							basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
							basicUri = new Uri($"https://{HttpsHost}:{HttpsPort}/basic/{ws.Service.Name}");
							wsHttpBinding = new WSHttpBinding(CoreWCF.SecurityMode.Transport);
							wsHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
							wsHttpUri = new Uri($"https://{HttpsHost}:{HttpsPort}/ws/{ws.Service.Name}");
							netHttpBinding = new NetHttpBinding(BasicHttpSecurityMode.Transport);
							netHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
							netHttpUri = new Uri($"https://{HttpsHost}:{HttpsPort}/net/{ws.Service.Name}");
							defaultUri = new Uri($"https://{HttpsHost}:{HttpsPort}/{ws.Service.Name}");
							builder.AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, defaultUri)
								.AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, basicUri)
								.AddServiceEndpoint(ws.Service, ws.Contract, wsHttpBinding, wsHttpUri)
								.AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, netHttpUri);
						}
						if (NetTcpPort.HasValue)
						{
							netTcpBinding = new NetTcpBinding(CoreWCF.SecurityMode.None);
							netTcpUri = new Uri($"net.tcp://{NetTcpHost}:{NetTcpPort}/tcp/{ws.Service.Name}");
							builder.AddServiceEndpoint(ws.Service, ws.Contract, netTcpBinding, netTcpUri);
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
