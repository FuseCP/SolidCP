using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;
#if !NETFRAMEWORK
using Grpc.Core;
using Grpc.Core.Interceptors;
using ProtoBuf.Grpc.Client;
using Grpc.Net.Client;
#endif

namespace SolidCP.Web.Client
{

	public enum Protocols { BasicHttp, BasicHttps, NetHttp, NetHttps, WSHttp, WSHttps, NetTcp, NetTcpSsl, NetPipe, NetPipeSsl, gRPC, gRPCSsl, gRPCWeb, gRPCWebSsl, Assembly }

	public class UserNamePasswordCredentials
	{
		public string UserName { get; set; }
		public string Password { get; set; }
	}

	public class ClientBase : IDisposable
	{
		Protocols protocol = Protocols.NetHttp;
		public Protocols Protocol
		{
			get => protocol;
			set
			{
				if (value != protocol)
				{
					url = url
						.Strip("basic")
						.Strip("net")
						.Strip("ws")
						.Strip("gprc")
						.Strip("gprc/web")
						.Strip("ssl")
						.Strip("nettcp");
						//.Strip("nettcp/ssl");
					
					if (value == Protocols.NetTcp && IsEncrypted) value = Protocols.NetTcpSsl;

					if (value == Protocols.BasicHttp) url = url.SetScheme("http").SetApi("basic");
					else if (value == Protocols.BasicHttps) url = url.SetScheme("https").SetApi("basic");
					else if (value == Protocols.NetHttp) url = url.SetScheme("http").SetApi("net");
					else if (value == Protocols.NetHttps) url = url.SetScheme("https").SetApi("net");
					else if (value == Protocols.WSHttp) url = url.SetScheme("http").SetApi("ws");
					else if (value == Protocols.WSHttps) url = url.SetScheme("https").SetApi("ws");
					else if (value == Protocols.NetTcp) url = url.SetScheme("net.tcp").SetApi("nettcp");
					else if (value == Protocols.NetTcpSsl) url = url.SetScheme("net.tcp").SetApi("nettcp");
					else if (value == Protocols.gRPC) url = url.SetScheme("http").SetApi("grpc");
					else if (value == Protocols.gRPCSsl) url = url.SetScheme("https").SetApi("grpc");
					else if (value == Protocols.gRPCWeb) url = url.SetScheme("http").SetApi("grpc/web");
					else if (value == Protocols.gRPCWebSsl) url = url.SetScheme("https").SetApi("grpc/web");
					else if (value == Protocols.Assembly) url = url.SetScheme("assembly");
				}
				protocol = value;
			}
		}
		public UserNamePasswordCredentials Credentials { get; set; } = new UserNamePasswordCredentials();
		public object SoapHeader { get; set; } = null;

		public V Header<V>() => (V)SoapHeader;
		public TimeSpan? Timeout { get; set; } = null;

		protected string url;
		public string Url
		{
			get { return url; }
			set
			{
				url = value;
				if (url.StartsWith("http://"))
				{
					if (url.HasApi("basic")) protocol = Protocols.BasicHttp;
					else if (url.HasApi("net")) protocol = Protocols.NetHttp;
					else if (url.HasApi("ws")) protocol = Protocols.WSHttp;
					else if (url.HasApi("grpc")) protocol = Protocols.gRPC;
					else if (url.HasApi("grpc/web")) protocol = Protocols.gRPCWeb;
					else if (IsEncrypted) Protocol = Protocols.WSHttp;
					else Protocol = Protocols.BasicHttp;
				}
				else if (url.StartsWith("https://"))
				{
					if (url.HasApi("basic")) protocol = Protocols.BasicHttps;
					else if (url.HasApi("net")) protocol = Protocols.NetHttps;
					else if (url.HasApi("ws")) protocol = Protocols.WSHttps;
					else if (url.HasApi("grpc")) protocol = Protocols.gRPCSsl;
					else if (url.HasApi("grpc/web")) protocol = Protocols.gRPCWebSsl;
					else Protocol = Protocols.BasicHttps;
				}
				else if (url.StartsWith("net.tcp://"))
				{
					if (url.HasApi("nettcp"))
					{
						if (IsEncrypted) Protocol = Protocols.NetTcpSsl;
						else Protocol = Protocols.NetTcp;
					}
				}
#if NETFRAMEWORK
				else if (url.StartsWith("net.pipe://"))
				{
					if (url.HasApi("ssl")) Protocol = Protocols.NetPipeSsl;
					else Protocol = Protocols.NetPipe;
				}
#endif
				else if (url.StartsWith("assembly://")) Protocol = Protocols.Assembly;
				else throw new NotSupportedException("illegal protocol");
			}
		}

		public bool IsWCF => Protocol < Protocols.gRPC;
		public bool IsGRPC => Protocol >= Protocols.gRPC && Protocol < Protocols.Assembly;
		public bool IsAssembly => Protocol == Protocols.Assembly;
		public bool IsSsl => Protocol == Protocols.BasicHttps || Protocol == Protocols.WSHttps || Protocol == Protocols.NetHttps || Protocol == Protocols.NetTcpSsl || Protocol == Protocols.NetPipeSsl ||
			Protocol == Protocols.gRPCSsl || Protocol == Protocols.gRPCWebSsl;
		public bool IsSecureProtocol => IsSsl || Protocol == Protocols.WSHttp;

		public bool IsEncrypted => this.GetType().GetInterfaces()
					.FirstOrDefault(i => i.GetCustomAttribute<ServiceContractAttribute>() != null)
					?.GetCustomAttribute<HasPolicyAttribute>() != null;

		public bool IsAuthenticated
		{
			get
			{
				var serviceInterface = this.GetType().GetInterfaces()
					.FirstOrDefault(i => i.GetCustomAttribute<ServiceContractAttribute>() != null);
				var policy = serviceInterface?.GetCustomAttribute<HasPolicyAttribute>()?.Policy;
				return policy != null && policy != "CommonPolicy";
			}
		}
		public bool HasSoapHeaders => this.GetType().GetInterfaces()
			.FirstOrDefault(i => i.GetCustomAttribute<ServiceContractAttribute>() != null)
			?.GetCustomAttribute<SolidCP.Providers.SoapHeaderAttribute>()
			!= null;

		public virtual void Close() { }
		public void Dispose()
		{
			Close();
		}
	}
	static class StringExtensions
	{
		public static string Strip(this string url, string api)
		{
			url = Regex.Replace(url, $"/{api}/", "/");
			url = Regex.Replace(url, $"/{api}(?=\\?|$)", "");
			return url;
		}
		public static string SetScheme(this string url, string scheme) => Regex.Replace(url, "^[a-zA-Z.]://", $"{scheme}://");
		public static string SetApi(this string url, string api)
		{
			var parts = url.Split('?');
			url = parts[0];
			if (url[url.Length-1] == '/') url = url.Substring(0, url.Length - 1);
			url = Regex.Replace(url, "(/(?:net|ws|basic|ssl|nettcp|pipe|grpc|grpc/web))(?=(?:/[a-zA-Z0-9_]+)?$)|((?<!/(?:net|ws|basic|ssl|nettcp|pipe|grpc|grpc/web)(?:/[a-zA-Z0-9_]+)?)$)", $"/{api}");

			if (parts.Length > 1) return $"{url}?{parts[1]}";
			else return url;
		}

		public static bool HasApi(this string url, string api) => Regex.IsMatch(url, $"/{api}(?:/|$|\\?)");
	}
	// web service client
	public class ClientBase<T, U> : ClientBase
		where T : class
		where U : ClientAssemblyBase, T, new()
	{


#if NETCOREAPP
		static Dictionary<string, GrpcChannel> GrpcPool = new Dictionary<string, GrpcChannel>();
#endif
		static readonly Dictionary<string, ChannelFactory<T>> FactoryPool = new Dictionary<string, ChannelFactory<T>>();
		ChannelFactory<T> factory;

		T client = null;

        protected T Client
		{
			get
			{

				var serviceurl = $"{url}/{this.GetType().Name}";

                /*
				if (IsWCF)
				{
					serviceurl = url
					   .Strip("basic")
					   .Strip("net")
					   .Strip("ws")
					   .Strip("gprc")
					   .Strip("gprc/web")
					   .Strip("ssl")
					   .Strip("net.tcp/ssl")
					   .Strip("net.tcp");

					serviceurl = $"{serviceurl}/{this.GetType().Name}";
					switch (Protocol)
					{
						case Protocols.BasicHttp:
						case Protocols.BasicHttps: serviceurl = $"{serviceurl}/basic"; break;
						case Protocols.NetHttp:
						case Protocols.NetHttps: serviceurl = $"{serviceurl}/net"; break;
						case Protocols.WSHttp:
						case Protocols.WSHttps: serviceurl = $"{serviceurl}/ws"; break;
						case Protocols.NetTcp:
						case Protocols.NetTcpSsl: serviceurl = $"{serviceurl}/nettcp"; break;
						case Protocols.NetPipe:
						case Protocols.NetPipeSsl: serviceurl = $"{serviceurl}/pipe"; break;
					}
				}
				*/

				// TODO set the credentials on client
                if (client != null)
				{
					if (client is IClientChannel chan)
					{
						// reuse client if it uses the same address and credentials
						if (chan.RemoteAddress.Uri.AbsoluteUri != serviceurl)
						{
							client = null;
						}
						else
						{
							if (chan.State != CommunicationState.Opened && chan.State != CommunicationState.Opening)
							{
								if (chan.State == CommunicationState.Faulted || chan.State == CommunicationState.Closed) client = null;
								else chan.Open();
							}
							return client;
						}
					}
					else return client;
				}


				if (IsWCF)
				{
					var isEncrypted = IsEncrypted;

					Binding binding = null;
					switch (Protocol)
					{
						case Protocols.BasicHttp:
							if (isEncrypted)
							{
								//throw new NotSupportedException("This api is not supported on this service.");
								binding = new BasicHttpBinding(BasicHttpSecurityMode.Message);
							}
							else binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
							break;
						case Protocols.BasicHttps: 
							if (isEncrypted)
							{
								var basics = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
								binding = basics;
							} else
							{
                                var basics = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
                                binding = basics;
                            }
                            break;
						case Protocols.NetHttp:
							if (isEncrypted)
							{
								// throw new NotSupportedException("This api is not supported on this service.");
								binding = new NetHttpBinding(BasicHttpSecurityMode.Message);
							} else binding = new NetHttpBinding(BasicHttpSecurityMode.None);
							break;
						case Protocols.NetHttps: 
							if (isEncrypted)
							{
								var nets = new NetHttpBinding(BasicHttpSecurityMode.Transport);
								binding = nets;
                            } else
							{
                                var nets = new NetHttpBinding(BasicHttpSecurityMode.Transport);
                                binding = nets;
                            }

                            break;
						case Protocols.WSHttp:
							if (isEncrypted)
							{
								//throw new NotSupportedException("This api is not supported on this service.");
								var ws = new WSHttpBinding(SecurityMode.Message);
								ws.Security.Message.ClientCredentialType = MessageCredentialType.None;
								ws.Security.Message.NegotiateServiceCredential = true;
								ws.Security.Message.EstablishSecurityContext = true;
								binding = ws;
							}
							else binding = new WSHttpBinding(SecurityMode.None);
							break;
						case Protocols.WSHttps: 
							if (isEncrypted)
							{
								var wss = new WSHttpBinding(SecurityMode.Transport);
								binding = wss;
							} else
							{
                                var wss = new WSHttpBinding(SecurityMode.Transport);
                                binding = wss;
                            }
                            break;
						case Protocols.NetTcp:
						case Protocols.NetTcpSsl:
							if (isEncrypted)
							{
								var nettcp = new NetTcpBinding(SecurityMode.Transport);
								binding = nettcp;
							}
							else binding = new NetTcpBinding(SecurityMode.Transport);
							break;
#if NETFRAMEWORK
						case Protocols.NetPipe: 
							
							binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None); break;
						case Protocols.NetPipeSsl: binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.Transport); break;
#endif
					}
					binding.ReceiveTimeout = Timeout ?? TimeSpan.FromSeconds(120);
					binding.SendTimeout = Timeout ?? TimeSpan.FromSeconds(120);

					var endpoint = new EndpointAddress(serviceurl);

					lock (FactoryPool)
					{
						if (!FactoryPool.TryGetValue(serviceurl, out factory))
						{
							factory = new ChannelFactory<T>(binding, endpoint);
						}
						else
						{
							FactoryPool[url] = null;
						}
					}
					if (SoapHeader != null || Credentials != null && Credentials.Password != null && IsSsl)
					{
						foreach (var b in factory.Endpoint.EndpointBehaviors.ToArray())
						{
							if (b is SoapHeaderClientBehavior) factory.Endpoint.EndpointBehaviors.Remove(b);
						}
						factory.Endpoint.EndpointBehaviors.Add(new SoapHeaderClientBehavior() { Client = this });
					}
					client = factory.CreateChannel();
					((IClientChannel)client).OperationTimeout = Timeout ?? TimeSpan.FromSeconds(120);
				}
#if !NETFRAMEWORK
				else if (IsGRPC)
				{
					// TODO soap header & username credentials

					throw new NotSupportedException("gRPC is not yet supported.");

					GrpcChannel gchannel;
					if (!GrpcPool.TryGetValue(url, out gchannel))
					{
						GrpcPool[url] = gchannel = GrpcChannel.ForAddress(url);
					}
					client = gchannel.CreateGrpcService<T>();
				}
#endif
				else if (IsAssembly)
				{
					var assemblyClient = new U();
					assemblyClient.AssemblyName = url.Substring("assembly://".Length);
					client = assemblyClient;
				}
				else throw new NotSupportedException("Unsupported protocol in SolidCP.Web.Clients.ClientBase");
				if (client is IClientChannel channel) channel.Open();
				return client;
			}
		}

		public override void Close()
		{
			lock (FactoryPool)
			{
				FactoryPool[url] = factory;
				factory = null;
			}
			if (client != null && client is IClientChannel channel) channel.Close();
		}


		public ClientBase() { }
		public ClientBase(string url) : this()
		{
			Url = url;
		}
	}
}
