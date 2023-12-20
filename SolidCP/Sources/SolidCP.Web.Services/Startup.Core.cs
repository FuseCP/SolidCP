#if !NETFRAMEWORK
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Linq;
using System.Xml;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using Swashbuckle.AspNetCore.Swagger;
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
using SolidCP.Providers.OS;

namespace SolidCP.Web.Services
{
	public static class StartupCore
	{

		public static void Log(string msg)
		{
			Console.WriteLine(msg);
			if (Debugger.IsAttached) Debugger.Log(1, "SolidCP", msg);
			//Trace.TraceInformation(msg);
		}
		public static void Error(string msg)
		{
			var col = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(msg);
			Console.ForegroundColor = col;
			if (Debugger.IsAttached) Debugger.Log(1, "SolidCP", msg);
			//Trace.TraceError(msg);
		}

		const bool AllowInsecureHttp = PolicyAttribute.AllowInsecureHttp;
		public static int? HttpPort = null;
		public static int? HttpsPort = null;
		public static int? NetTcpPort = null;
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
		public static string KeyFile = null;
		public static string ProbingPaths = "";
		public static string AllowedHosts = "0.0.0.0";
		public static bool IsLocalService = false;
		public static TraceLevel TraceLevel = TraceLevel.Off;
		public static X509Certificate2 Certificate = null;

		public static void Init(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			ProbingPaths = builder.Configuration["probingPaths"];
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
			Name = builder.Configuration.GetValue<string>("ServerCertificate:Name") ?? null;
			CertificateFile = builder.Configuration.GetValue<string>("ServerCertificate:File");
			CertificatePassword = builder.Configuration.GetValue<string>("ServerCertificate:Password");
			Password = builder.Configuration.GetValue<string>("Server:Password") ?? String.Empty;
			AllowedHosts = builder.Configuration.GetValue<string>("AllowedHosts") ?? "0.0.0.0";
			TraceLevel = builder.Configuration.GetValue<TraceLevel?>("TraceLevel") ?? TraceLevel.Off;
			KeyFile = builder.Configuration.GetValue<string>("ServerCertificate:KeyFile");

			if (TraceLevel != TraceLevel.Off && (OSInfo.IsLinux || OSInfo.IsMac))
			{
				TraceListener syslog = (TraceListener)Activator.CreateInstance(Type.GetType("SolidCP.Providers.OS.SyslogTraceListener, SolidCP.Providers.OS.Unix"));
				var level = syslog.GetType().GetProperty("Level");
				level.SetValue(syslog, TraceLevel);
				Trace.Listeners.Add(syslog);
				Log($"Trace level set to {TraceLevel}");
			}
			IsLocalService = AllowedHosts.Split(';')
				 .All(host => host == "localhost" || host == "127.0.0.1" || host == "::1" ||
					  Regex.IsMatch(host, "^192\\.168\\.[0-9]+\\.[0-9]+$")); // local network ip

			builder.Services.AddRazorPages();
			builder.Services.AddHttpContextAccessor();
			ConfigureServices(builder.Services);

			if (NetTcpPort.HasValue)
			{
				builder.WebHost.UseNetTcp(NetTcpPort.Value);
				Log($"Listen on net.tcp port {NetTcpPort.Value}");
			}

			builder.WebHost.UseKestrel(options =>
			{
				options.AllowSynchronousIO = true;

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
									var file = new FileInfo(CertificateFile).FullName;
									if (File.Exists(file))
									{
										if (!string.IsNullOrEmpty(KeyFile))
										{
											var keyFile = new FileInfo(KeyFile).FullName;
											if (File.Exists(keyFile)) CertificatePassword = File.ReadAllText(keyFile);
										}
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
			var a = Assembly.GetEntryAssembly();
			var srvcAssemblies = AppDomain.CurrentDomain.GetAssemblies()
				.Select(assembly => assembly.GetName().Name.Replace('.', ' '))
				.Where(name => name == "SolidCP Server" || name == "SolidCP EnterpriseServer");
			var title = $"{string.Join(" & ", srvcAssemblies.ToArray())} API";

			var ver = a.GetCustomAttribute<AssemblyVersionAttribute>();
			services
				.AddServiceModelServices()
				.AddServiceModelMetadata()
				.AddServiceModelWebServices(o =>
				{
					o.Title = title;
					o.Version = ver?.Version ?? "1.0";
					o.Description = title;
					o.TermsOfService = new("http://solidcp.com/terms");
					o.ContactName = "Contact";
					o.ContactEmail = "support@solidcp.com";
					o.ContactUrl = new("http://solidcp.com/contact");
					o.ExternalDocumentUrl = new("http://solidcp.com/apidoc.pdf");
					o.ExternalDocumentDescription = "Documentation";
				});

			services.AddSingleton(new SwaggerOptions());
			//.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();
		}

		public static IServiceBuilder AddServiceEndpoint(this IServiceBuilder builder, Type service, Type contract, Binding binding, Uri address)
		{
			builder.AddServiceEndpoint(service, contract, binding, address, address, conf => conf.EndpointBehaviors.Add(new SoapHeaderMessageInspector()));
			return builder;
		}

		public static void ConfigureWCF(IApplicationBuilder app)
		{
			app.UseMiddleware<SwaggerMiddleware>();
			app.UseSwaggerUI();

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
						if (Certificate != null) host.Credentials.ServiceCertificate.Certificate = Certificate;
						var behavior = host.Description.Behaviors.Find<ServiceDebugBehavior>();
						if (behavior != null) behavior.IncludeExceptionDetailInFaults = true;
						else
						{
							var debugBehavior = new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true };
							host.Description.Behaviors.Add(debugBehavior);
						}
					});

				var isLocal = StartupCore.IsLocalService;

				var readerQuotas = new XmlDictionaryReaderQuotas
				{
					MaxBytesPerRead = 4096,
					MaxDepth = 32,
					MaxArrayLength = 16384,
					MaxStringContentLength = 16384,
					MaxNameTableCharCount = 16384
				};

				foreach (var ws in webServices)
				{
					var policy = ws.Contract.GetCustomAttributes(false).OfType<Services.PolicyAttribute>().FirstOrDefault();
					var isEncrypted = policy != null;
					var isAuthenticated = isEncrypted && policy.Policy != "CommonPolicy";

					BasicHttpBinding basicHttpBinding;
					WSHttpBinding wsHttpBinding;
					NetHttpBinding netHttpBinding;
					NetTcpBinding netTcpBinding = null, netTcpSslBinding = null;
					WebHttpBinding webHttpBinding;

					Uri basicUri, wsHttpUri, netHttpUri, netTcpUri = null, netTcpSslUri, webHttpUri, defaultUri;

					builder.AddService(ws.Service, options =>
						{
							//if (HttpPort.HasValue) options.BaseAddresses.Add(new Uri($"http://{HttpHost}:{HttpPort}/{ws.Service.Name}"));
							//if (HttpsPort.HasValue) options.BaseAddresses.Add(new Uri($"https://{HttpsHost}:{HttpsPort}/{ws.Service.Name}"));
							//if (NetTcpPort.HasValue) options.BaseAddresses.Add(new Uri($"tcp.net://{NetTcpHost}:{NetTcpPort}/{ws.Service.Name}"));
							options.DebugBehavior.IncludeExceptionDetailInFaults = true;
						});

					if (isEncrypted)
					{
						if (HttpPort.HasValue && (isLocal || AllowInsecureHttp))
						{
							basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
							basicUri = new Uri($"http://{HttpHost}:{HttpPort}/basic/{ws.Service.Name}");
							wsHttpBinding = new WSHttpBinding(CoreWCF.SecurityMode.None);
							wsHttpUri = new Uri($"http://{HttpHost}:{HttpPort}/ws/{ws.Service.Name}");
							netHttpBinding = new NetHttpBinding(BasicHttpSecurityMode.None);
							netHttpBinding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Never;
							netHttpUri = new Uri($"http://{HttpHost}:{HttpPort}/net/{ws.Service.Name}");
							webHttpBinding = new WebHttpBinding(isAuthenticated ? WebHttpSecurityMode.TransportCredentialOnly : WebHttpSecurityMode.None);
							webHttpBinding.MaxReceivedMessageSize = 5242880;
							webHttpBinding.MaxBufferSize = 65536;
							webHttpBinding.ReaderQuotas = readerQuotas;
							webHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
							webHttpUri = new Uri($"http://{HttpHost}:{HttpPort}/api/{ws.Service.Name}");
							defaultUri = new Uri($"http://{HttpHost}:{HttpPort}/{ws.Service.Name}");
							builder.AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, defaultUri)
									 .AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, basicUri)
									 .AddServiceEndpoint(ws.Service, ws.Contract, wsHttpBinding, wsHttpUri)
									 .AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, netHttpUri)
									 .AddServiceWebEndpoint(ws.Service, ws.Contract, webHttpBinding, webHttpUri, webHttpUri, behavior =>
									 {
										 behavior.AutomaticFormatSelectionEnabled = true;
										 behavior.DefaultBodyStyle = CoreWCF.Web.WebMessageBodyStyle.WrappedRequest;
										 behavior.HelpEnabled = true;
									 });
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
							netHttpBinding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Never;
							netHttpUri = new Uri($"https://{HttpsHost}:{HttpsPort}/net/{ws.Service.Name}");
							webHttpBinding = new WebHttpBinding(WebHttpSecurityMode.Transport);
							webHttpBinding.MaxReceivedMessageSize = 5242880;
							webHttpBinding.MaxBufferSize = 65536;
							webHttpBinding.ReaderQuotas = readerQuotas;
							webHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
							webHttpUri = new Uri($"https://{HttpsHost}:{HttpsPort}/api/{ws.Service.Name}");
							defaultUri = new Uri($"https://{HttpsHost}:{HttpsPort}/{ws.Service.Name}");
							builder.AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, defaultUri)
									 .AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, basicUri)
									 .AddServiceEndpoint(ws.Service, ws.Contract, wsHttpBinding, wsHttpUri)
									 .AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, netHttpUri)
									 .AddServiceWebEndpoint(ws.Service, ws.Contract, webHttpBinding, webHttpUri, webHttpUri, behavior =>
									 {
										 behavior.AutomaticFormatSelectionEnabled = true;
										 behavior.DefaultBodyStyle = CoreWCF.Web.WebMessageBodyStyle.WrappedRequest;
										 behavior.HelpEnabled = true;
									 });
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
							if (isLocal || AllowInsecureHttp)
							{
								netTcpBinding = new NetTcpBinding(CoreWCF.SecurityMode.None);
								netTcpUri = new Uri($"net.tcp://{NetTcpHost}:{NetTcpPort}/tcp/{ws.Service.Name}");
								builder.AddServiceEndpoint(ws.Service, ws.Contract, netTcpBinding, netTcpUri);
							}
						}
					}
					else // isEncrypted = false
					{
						if (HttpPort.HasValue)
						{
							basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
							basicUri = new Uri($"http://{HttpHost}:{HttpPort}/basic/{ws.Service.Name}");
							wsHttpBinding = new WSHttpBinding(CoreWCF.SecurityMode.None);
							wsHttpUri = new Uri($"http://{HttpHost}:{HttpPort}/ws/{ws.Service.Name}");
							netHttpBinding = new NetHttpBinding(BasicHttpSecurityMode.None);
							netHttpBinding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Never;
							netHttpUri = new Uri($"http://{HttpHost}:{HttpPort}/net/{ws.Service.Name}");
							webHttpBinding = new WebHttpBinding(WebHttpSecurityMode.None);
							webHttpBinding.MaxReceivedMessageSize = 5242880;
							webHttpBinding.MaxBufferSize = 65536;
							webHttpBinding.ReaderQuotas = readerQuotas;
							webHttpUri = new Uri($"http://{HttpHost}:{HttpPort}/api/{ws.Service.Name}");
							defaultUri = new Uri($"http://{HttpHost}:{HttpPort}/{ws.Service.Name}");
							builder.AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, defaultUri)
								.AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, basicUri)
								.AddServiceEndpoint(ws.Service, ws.Contract, wsHttpBinding, wsHttpUri)
								.AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, netHttpUri)
								.AddServiceWebEndpoint(ws.Service, ws.Contract, webHttpBinding, webHttpUri, webHttpUri, behavior =>
								{
									behavior.AutomaticFormatSelectionEnabled = true;
									behavior.DefaultBodyStyle = CoreWCF.Web.WebMessageBodyStyle.WrappedRequest;
									behavior.HelpEnabled = true;
								});
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
							netHttpBinding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Never;
							netHttpUri = new Uri($"https://{HttpsHost}:{HttpsPort}/net/{ws.Service.Name}");
							webHttpBinding = new WebHttpBinding(WebHttpSecurityMode.Transport);
							webHttpBinding.MaxReceivedMessageSize = 5242880;
							webHttpBinding.MaxBufferSize = 65536;
							webHttpBinding.ReaderQuotas = readerQuotas;
							webHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
							webHttpUri = new Uri($"https://{HttpsHost}:{HttpsPort}/api/{ws.Service.Name}");
							defaultUri = new Uri($"https://{HttpsHost}:{HttpsPort}/{ws.Service.Name}");
							builder.AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, defaultUri)
									 .AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, basicUri)
									 .AddServiceEndpoint(ws.Service, ws.Contract, wsHttpBinding, wsHttpUri)
									 .AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, netHttpUri)
									 .AddServiceWebEndpoint(ws.Service, ws.Contract, webHttpBinding, webHttpUri, webHttpUri, behavior =>
									 {
										 behavior.AutomaticFormatSelectionEnabled = true;
										 behavior.DefaultBodyStyle = CoreWCF.Web.WebMessageBodyStyle.WrappedRequest;
										 behavior.HelpEnabled = true;
									 });
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
							netTcpBinding = new NetTcpBinding(CoreWCF.SecurityMode.None);
							netTcpUri = new Uri($"net.tcp://{NetTcpHost}:{NetTcpPort}/tcp/{ws.Service.Name}");
							builder.AddServiceEndpoint(ws.Service, ws.Contract, netTcpBinding, netTcpUri);
						}
					}
				}

				// Configure WSDL to be available
				var serviceMetadataBehavior = app.ApplicationServices.GetRequiredService<ServiceMetadataBehavior>();
				serviceMetadataBehavior.HttpGetEnabled = HttpPort.HasValue;
				serviceMetadataBehavior.HttpsGetEnabled = HttpsPort.HasValue;
			});

		}
	}
}
#endif
