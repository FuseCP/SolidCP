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
using SolidCP.Providers;
using SolidCP.Providers.OS;

namespace SolidCP.Web.Services
{
	public static class StartupCore
	{
		public static bool IsUnixSystemd => Directory.Exists("/run/systemd/system");

		public const int KB = Configuration.KB;
		public const int MB = Configuration.MB;
		public const int MaxReceivedMessageSize = Configuration.MaxReceivedMessageSize;
		public const int MaxBufferSize = Configuration.MaxBufferSize;
		public const int MaxBytesPerRead = Configuration.MaxBytesPerRead;
		public const int MaxDepth = Configuration.MaxDepth;
		public const int MaxArrayLength = Configuration.MaxArrayLength;
		public const int MaxStringContentLength = Configuration.MaxStringContentLength;
		public const int MaxNameTableCharCount = Configuration.MaxNameTableCharCount;
		public const bool AllowInsecureHttp = Configuration.AllowInsecureHttp;

		public static bool IsIIS => OSInfo.IsWindows && NativeMethods.IsAspNetCoreModuleLoaded();

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

		static int? HttpPort = null;
		static int? HttpsPort = null;
		static int? NetTcpPort = null;
		static string HttpHost = null;
		static string HttpsHost = null;
		static string NetTcpHost = null;
		static StoreLocation StoreLocation = StoreLocation.LocalMachine;
		static StoreName StoreName = StoreName.My;
		static X509FindType FindType = X509FindType.FindBySubjectName;
		static string Name = null;
		static string CertificateFile = null;
		static string CertificatePassword = null;
		static string Password;
		static string KeyFile = null;
		static string ProbingPaths = "";
		static string AllowedHosts = "0.0.0.0";
		static bool IsLocalService = false;
		static TraceLevel TraceLevel = TraceLevel.Off;
		static X509Certificate2 Certificate = null;
		static string DataProviderType = null;
		static string WebApplicationsPath = null;
		static int? ServerRequestTimeout = null;
		static string ConnectionString = null;
		static string ProviderName = null;
		static string AltConnectionString = null;
		static string AltProviderName = null;
		static bool AlwaysUseEntityFramework = false;
		static string CryptoKey = null;
		static string AltCryptoKey = null;
		static bool? EncryptionEnabled = null;
		static string ExposeWebServices = null;

		public static void Init(string[] args)
		{

			var builder = WebApplication.CreateBuilder(args);
			Configuration.ProbingPaths = builder.Configuration["probingPaths"];
			AssemblyLoaderNetCore.Init();
			var urls = builder.Configuration["applicationUrls"];
			foreach (var url in urls.Split(';'))
			{
				var uri = new Uri(url);
				if (uri.Scheme == "http")
				{
					Configuration.HttpPort = HttpPort = uri.Port;
					Configuration.HttpHost = HttpHost = uri.Host;
				}
				else if (uri.Scheme == "https")
				{
					Configuration.HttpsPort = HttpsPort = uri.Port;
					Configuration.HttpsHost = HttpsHost = uri.Host;
				}
				else if (uri.Scheme == "net.tcp")
				{
					Configuration.NetTcpPort = uri.Port;
					Configuration.NetTcpHost = uri.Host;
				}
			}
			Configuration.StoreLocation = StoreLocation = builder.Configuration.GetValue<StoreLocation?>("ServerCertificate:StoreLocation") ?? StoreLocation.LocalMachine;
			Configuration.StoreName = StoreName = builder.Configuration.GetValue<StoreName?>("ServerCertificate:StoreName") ?? StoreName.My;
			Configuration.FindType = FindType = builder.Configuration.GetValue<X509FindType?>("ServerCertificate:FindType") ?? X509FindType.FindBySubjectName;
			Configuration.Name = Name = builder.Configuration.GetValue<string>("ServerCertificate:Name") ?? null;
			Configuration.CertificateFile = CertificateFile = builder.Configuration.GetValue<string>("ServerCertificate:File");
			Configuration.CertificatePassword = CertificatePassword = builder.Configuration.GetValue<string>("ServerCertificate:Password");
			Configuration.Password = Password = builder.Configuration.GetValue<string>("Server:Password") ?? String.Empty;
			Configuration.AllowedHosts = AllowedHosts = builder.Configuration.GetValue<string>("AllowedHosts") ?? "0.0.0.0";
			Configuration.TraceLevel = TraceLevel = builder.Configuration.GetValue<TraceLevel?>("TraceLevel") ?? TraceLevel.Off;
			Configuration.KeyFile = KeyFile = builder.Configuration.GetValue<string>("ServerCertificate:KeyFile");
			Configuration.ExposeWebServices = ExposeWebServices = builder.Configuration.GetValue<string>("exposeWebServices") ?? "";
			Configuration.DataProviderType = DataProviderType = builder.Configuration.GetValue<string>("EnterpriseServer:DataProviderType");
			Configuration.WebApplicationsPath = WebApplicationsPath = builder.Configuration.GetValue<string>("EnterpriseServer:WebApplicationPath");
			Configuration.ServerRequestTimeout = ServerRequestTimeout = builder.Configuration.GetValue<int?>("EnterpriseServer:ServerRequestTimeout") ?? -1;
			Configuration.ConnectionString = ConnectionString = builder.Configuration.GetValue<string>("EnterpriseServer:ConnectionString");
			Configuration.ProviderName = ProviderName = builder.Configuration.GetValue<string>("EnterpriseServer:ProviderName");
			Configuration.AltConnectionString = AltConnectionString = builder.Configuration.GetValue<string>("EnterpriseServer:AltConnectionString");
			Configuration.AltProviderName = AltProviderName = builder.Configuration.GetValue<string>("EnterpriseServer:AltProviderName");
			Configuration.AlwaysUseEntityFramework = AlwaysUseEntityFramework = builder.Configuration.GetValue<bool?>("EnterpriseServer:AlwaysUseEntityFramework") ?? false;
			Configuration.CryptoKey = CryptoKey = builder.Configuration.GetValue<string>("EnterpriseServer:CryptoKey");
			Configuration.AltCryptoKey = AltCryptoKey = builder.Configuration.GetValue<string>("EnterpriseServer:AltCryptoKey");
			Configuration.EncryptionEnabled = EncryptionEnabled = builder.Configuration.GetValue<bool?>("EnterpriseServer:EncryptionEnabled");

			if (TraceLevel != TraceLevel.Off)
			{
				var listener = OSInfo.Current.DefaultTraceListener;
				Trace.Listeners.Add(listener);
				Log($"Trace level set to {TraceLevel}");
			}
			Configuration.IsLocalService = Configuration.AllowedHosts.Split(';')
				.All(host => DnsService.IsHostLAN(host)); // local network ip

			builder.Services.AddRazorPages();
			builder.Services.AddHttpContextAccessor();
			ConfigureServices(builder.Services);

			if (NetTcpPort.HasValue)
			{
				builder.WebHost.UseNetTcp(NetTcpPort.Value);
				Log($"Listen on net.tcp port {NetTcpPort.Value}");
			}

			if (IsIIS)
			{
				builder.Services.Configure<IISServerOptions>(options =>
				{
					options.AllowSynchronousIO = true;
				});
				builder.WebHost.UseIIS();
			}
			else
			{
				builder.WebHost.UseKestrel(options =>
				{
					options.AllowSynchronousIO = true;

					if (OSInfo.IsUnix && IsUnixSystemd) options.UseSystemd();

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
			}

			Server.ContentRoot = builder.Environment.ContentRootPath;
			Server.WebRoot = builder.Environment.WebRootPath;

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

			var tunnelHandler = new TunnelHandlerCore();
			tunnelHandler.Init(app);

			Server.UseWebForms?.Invoke(app);

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
			var readerQuotas = new XmlDictionaryReaderQuotas
			{
				MaxBytesPerRead = MaxBytesPerRead,
				MaxDepth = MaxDepth,
				MaxArrayLength = MaxArrayLength,
				MaxStringContentLength = MaxStringContentLength,
				MaxNameTableCharCount = MaxNameTableCharCount
			};
			if (binding is BasicHttpBinding basicBinding)
			{
				basicBinding.MaxReceivedMessageSize = MaxReceivedMessageSize;
				basicBinding.MaxBufferSize = MaxBufferSize;
				basicBinding.ReaderQuotas = readerQuotas;
			} else if (binding is WSHttpBinding wsBinding)
			{
				wsBinding.MaxReceivedMessageSize = MaxReceivedMessageSize;
				wsBinding.ReaderQuotas = readerQuotas;
			} else if (binding is NetHttpBinding netBinding)
			{
				netBinding.MaxReceivedMessageSize = MaxReceivedMessageSize;
				netBinding.MaxBufferSize = MaxBufferSize;
				netBinding.ReaderQuotas = readerQuotas;
			} else if (binding is NetTcpBinding tcpBinding)
			{
				tcpBinding.MaxReceivedMessageSize = MaxReceivedMessageSize;
				tcpBinding.MaxBufferSize = MaxBufferSize;
				tcpBinding.ReaderQuotas = readerQuotas;
			}
			builder.AddServiceEndpoint(service, contract, binding, address, address, conf => conf.EndpointBehaviors.Add(new SoapHeaderMessageInspector()));
			return builder;
		}
		public static IServiceBuilder AddServiceWebEndpoint(this IServiceBuilder builder, Type service, Type contract, WebHttpBinding binding, Uri address)
		{
			var readerQuotas = new XmlDictionaryReaderQuotas
			{
				MaxBytesPerRead = MaxBytesPerRead,
				MaxDepth = MaxDepth,
				MaxArrayLength = MaxArrayLength,
				MaxStringContentLength = MaxStringContentLength,
				MaxNameTableCharCount = MaxNameTableCharCount
			};
			binding.MaxReceivedMessageSize = MaxReceivedMessageSize;
			binding.MaxBufferSize = MaxBufferSize;
			binding.ReaderQuotas = readerQuotas;
			builder.AddServiceWebEndpoint(service, contract, binding, address, address, behavior =>
			{
				behavior.AutomaticFormatSelectionEnabled = true;
				//behavior.DefaultBodyStyle = CoreWCF.Web.WebMessageBodyStyle.WrappedRequest;
				behavior.HelpEnabled = true;
			});
			return builder;
		}

		public static void ConfigureWCF(IApplicationBuilder app)
		{
			app.UseMiddleware<SwaggerMiddleware>();
			app.UseSwaggerUI();

			app.UseServiceModel(builder =>
			{
				var webServices = ServiceTypes.Types;

				var isLocal = Configuration.IsLocalService;

				foreach (var ws in webServices)
				{
					var policy = ws.Contract.GetCustomAttributes(false).OfType<Services.PolicyAttribute>().FirstOrDefault();
					var isEncrypted = policy != null;
					var isAuthenticated = isEncrypted && policy.Policy != PolicyAttribute.Encrypted;

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
							webHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
							webHttpUri = new Uri($"http://{HttpHost}:{HttpPort}/api/{ws.Service.Name}");
							defaultUri = new Uri($"http://{HttpHost}:{HttpPort}/{ws.Service.Name}");
							builder.AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, defaultUri)
												.AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, basicUri)
												.AddServiceEndpoint(ws.Service, ws.Contract, wsHttpBinding, wsHttpUri)
												.AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, netHttpUri)
												.AddServiceWebEndpoint(ws.Service, ws.Contract, webHttpBinding, webHttpUri);
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
							webHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
							webHttpUri = new Uri($"https://{HttpsHost}:{HttpsPort}/api/{ws.Service.Name}");
							defaultUri = new Uri($"https://{HttpsHost}:{HttpsPort}/{ws.Service.Name}");
							builder.AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, defaultUri)
												.AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, basicUri)
												.AddServiceEndpoint(ws.Service, ws.Contract, wsHttpBinding, wsHttpUri)
												.AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, netHttpUri)
												.AddServiceWebEndpoint(ws.Service, ws.Contract, webHttpBinding, webHttpUri);
						}
						if (NetTcpPort.HasValue)
						{
							if (Certificate != null)
							{
								netTcpSslBinding = new NetTcpBinding(SecurityMode.Transport);
								netTcpSslBinding.Security.Message.ClientCredentialType = MessageCredentialType.None;
								netTcpSslBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
								netTcpSslUri = new Uri($"net.tcp://{NetTcpHost}:{NetTcpPort}/tcp/ssl/{ws.Service.Name}");
								builder.AddServiceEndpoint(ws.Service, ws.Contract, netTcpSslBinding, netTcpSslUri);
							}
							if (isLocal || AllowInsecureHttp)
							{
								netTcpBinding = new NetTcpBinding(SecurityMode.None);
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
							webHttpUri = new Uri($"http://{HttpHost}:{HttpPort}/api/{ws.Service.Name}");
							defaultUri = new Uri($"http://{HttpHost}:{HttpPort}/{ws.Service.Name}");
							builder.AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, defaultUri)
											.AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, basicUri)
											.AddServiceEndpoint(ws.Service, ws.Contract, wsHttpBinding, wsHttpUri)
											.AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, netHttpUri)
											.AddServiceWebEndpoint(ws.Service, ws.Contract, webHttpBinding, webHttpUri);
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
							webHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
							webHttpUri = new Uri($"https://{HttpsHost}:{HttpsPort}/api/{ws.Service.Name}");
							defaultUri = new Uri($"https://{HttpsHost}:{HttpsPort}/{ws.Service.Name}");
							builder.AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, defaultUri)
												.AddServiceEndpoint(ws.Service, ws.Contract, basicHttpBinding, basicUri)
												.AddServiceEndpoint(ws.Service, ws.Contract, wsHttpBinding, wsHttpUri)
												.AddServiceEndpoint(ws.Service, ws.Contract, netHttpBinding, netHttpUri)
												.AddServiceWebEndpoint(ws.Service, ws.Contract, webHttpBinding, webHttpUri);
						}
						if (NetTcpPort.HasValue)
						{
							if (Certificate != null)
							{
								netTcpSslBinding = new NetTcpBinding(SecurityMode.Transport);
								netTcpSslBinding.Security.Message.ClientCredentialType = MessageCredentialType.None;
								netTcpSslBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
								netTcpSslUri = new Uri($"net.tcp://{NetTcpHost}:{NetTcpPort}/tcp/ssl/{ws.Service.Name}");
								builder.AddServiceEndpoint(ws.Service, ws.Contract, netTcpSslBinding, netTcpSslUri);
							}
							netTcpBinding = new NetTcpBinding(SecurityMode.None);
							netTcpUri = new Uri($"net.tcp://{NetTcpHost}:{NetTcpPort}/tcp/{ws.Service.Name}");
							builder.AddServiceEndpoint(ws.Service, ws.Contract, netTcpBinding, netTcpUri);
						}
					}
				}

				// Configure WSDL to be available
				var serviceMetadataBehavior = app.ApplicationServices.GetRequiredService<ServiceMetadataBehavior>();
				serviceMetadataBehavior.HttpGetEnabled = HttpPort.HasValue;
				serviceMetadataBehavior.HttpsGetEnabled = HttpsPort.HasValue;

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
					host.Authorization.ServiceAuthorizationManager = new RestAuthorizationManager();
					foreach (var endpoint in host.Description.Endpoints)
					{
						if (endpoint.Binding is WebHttpBinding)
						{
							endpoint.EndpointBehaviors.Add(new SoapHeaderMessageInspector());
						}
					}
				});
			});
		}
	}
}
#endif
