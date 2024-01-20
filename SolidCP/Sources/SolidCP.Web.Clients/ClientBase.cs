using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Text.RegularExpressions;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;
using System.CodeDom;
using System.ServiceModel.Description;
using System.Net;
using System.IO;
using Renci.SshNet;
using SolidCP.Providers.OS;

#if !NETFRAMEWORK && gRPC
using Grpc.Core;
using Grpc.Core.Interceptors;
using ProtoBuf.Grpc.Client;
using Grpc.Net.Client;
#endif

namespace SolidCP.Web.Client
{

	public enum Protocols { BasicHttp, NetHttp, WSHttp, BasicHttps, NetHttps, WSHttps, NetTcp, NetTcpSsl, NetPipe, NetPipeSsl, RESTHttp, RESTHttps, gRPC, gRPCSsl, gRPCWeb, gRPCWebSsl, Assembly, Ssh }

	public class UserNamePasswordCredentials
	{
		public string UserName { get; set; }
		public string Password { get; set; }
	}

	public class ClientBase : IDisposable
	{

		public const long MaximumMessageSize = 10 * 1024 * 1024; // 10 MB
		public const bool UseMessageSecurityOverHttp = true;
		public static readonly TimeSpan ReceiveTimeout = TimeSpan.FromSeconds(120);
		public static readonly TimeSpan SendTimeout = TimeSpan.FromSeconds(120);

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
						.Strip("tcp")
						.Strip("tcp/ssl")
						.Strip("pipe")
						.Strip("pipe/ssl");

					SetProtocol(url, ref value);

					url.AssertScheme(value);

					if (value == Protocols.NetTcp && IsEncrypted && !IsLocal) value = Protocols.NetTcpSsl;

					if (value == Protocols.BasicHttp) url = url.SetApi("basic");
					else if (value == Protocols.BasicHttps) url = url.SetApi("basic");
					else if (value == Protocols.NetHttp) url = url.SetApi("net");
					else if (value == Protocols.NetHttps) url = url.SetApi("net");
					else if (value == Protocols.WSHttp) url = url.SetApi("ws");
					else if (value == Protocols.WSHttps) url = url.SetApi("ws");
					else if (value == Protocols.NetTcp) url = url.SetApi("tcp");
					else if (value == Protocols.NetTcpSsl) url = url.SetApi("tcp/ssl");
					else if (value == Protocols.gRPC) url = url.SetApi("grpc");
					else if (value == Protocols.gRPCSsl) url = url.SetApi("grpc");
					else if (value == Protocols.gRPCWeb) url = url.SetApi("grpc/web");
					else if (value == Protocols.gRPCWebSsl) url = url.SetApi("grpc/web");
					else if (value == Protocols.NetPipe) url = url.SetApi("pipe");
					else if (value == Protocols.NetPipeSsl) url = url.SetApi("pipe/ssl");
					else if (value == Protocols.Assembly) url = url.SetScheme("assembly");
					else if (value == Protocols.Ssh) url = url.SetScheme("ssh");
				}
				protocol = value;
			}
		}

		public void SetProtocol(string url, ref Protocols protocol)
		{
			if (url.StartsWith("https://"))
			{
				switch (protocol)
				{
					case Protocols.BasicHttp: protocol = Protocols.BasicHttps; break;
					case Protocols.NetHttp: protocol = Protocols.NetHttps; break;
					case Protocols.WSHttp: protocol = Protocols.WSHttps; break;
					case Protocols.gRPC: protocol = Protocols.gRPCSsl; break;
					case Protocols.gRPCWeb: protocol = Protocols.gRPCWebSsl; break;
					default: break;
				}
			}
			else if (url.StartsWith("http://"))
			{
				switch (protocol)
				{
					case Protocols.BasicHttps: protocol = Protocols.BasicHttp; break;
					case Protocols.NetHttps: protocol = Protocols.NetHttp; break;
					case Protocols.WSHttps: protocol = Protocols.WSHttp; break;
					case Protocols.gRPCSsl: protocol = Protocols.gRPC; break;
					case Protocols.gRPCWebSsl: protocol = Protocols.gRPCWeb; break;
					default: break;
				}
			}
			else if (url.StartsWith("ssh://"))
			{
				switch (protocol)
				{
					case Protocols.BasicHttps: protocol = Protocols.BasicHttp; break;
					case Protocols.NetHttps: protocol = Protocols.NetHttp; break;
					case Protocols.WSHttps: protocol = Protocols.WSHttp; break;
					case Protocols.gRPCSsl: protocol = Protocols.gRPC; break;
					case Protocols.gRPCWebSsl: protocol = Protocols.gRPCWeb; break;
					case Protocols.NetTcpSsl: protocol = Protocols.NetTcp; break;
					default: break;
				}
			}
		}
		public UserNamePasswordCredentials Credentials { get; set; } = new UserNamePasswordCredentials();
		public object SoapHeader { get; set; } = null;

		public V Header<V>() => (V)SoapHeader;
		public TimeSpan? Timeout { get; set; } = null;

		protected string url = null;
		public string Url
		{
			get { return url; }
			set
			{
				url = value;
				if (url.StartsWith("http://"))
				{
					if (IsEncrypted && !IsLocal) throw new NotSupportedException("This protocol is not secure over this connection.");

					if (url.HasApi("basic")) protocol = Protocols.BasicHttp;
					else if (url.HasApi("net")) protocol = Protocols.NetHttp;
					else if (url.HasApi("ws")) protocol = Protocols.WSHttp;
					else if (url.HasApi("grpc")) protocol = Protocols.gRPC;
					else if (url.HasApi("grpc/web")) protocol = Protocols.gRPCWeb;
					else protocol = Protocols.BasicHttp;
				}
				else if (url.StartsWith("https://"))
				{
					if (url.HasApi("basic")) protocol = Protocols.BasicHttps;
					else if (url.HasApi("net")) protocol = Protocols.NetHttps;
					else if (url.HasApi("ws")) protocol = Protocols.WSHttps;
					else if (url.HasApi("grpc")) protocol = Protocols.gRPCSsl;
					else if (url.HasApi("grpc/web")) protocol = Protocols.gRPCWebSsl;
					else protocol = Protocols.BasicHttps;
				}
				else if (url.StartsWith("net.tcp://"))
				{
					if (url.HasApi("tcp/ssl")) Protocol = Protocols.NetTcpSsl;
					else if (url.HasApi("tcp"))
					{
						if (IsEncrypted && !IsLocal) throw new NotSupportedException("This protocol is not secure over this connection.");
						else Protocol = Protocols.NetTcp;
					}
					else throw new NotSupportedException("net.tcp url must include tcp api");
				}
				//#if NETFRAMEWORK
				else if (url.StartsWith("net.pipe://"))
				{
					if (url.HasApi("pipe/ssl")) Protocol = Protocols.NetPipeSsl;
					else if (url.HasApi("pipe"))
					{
						if (IsEncrypted && !IsLocal) throw new NotSupportedException("This protocol is not secure over this connection.");
						else Protocol = Protocols.NetPipe;
					}
					else throw new NotSupportedException("net.pipe url must include pipe api");
				}
				//#endif
				else if (url.StartsWith("assembly://")) Protocol = Protocols.Assembly;
				else if (url.StartsWith("ssh://"))
				{
					if (url.HasApi("basic")) protocol = Protocols.BasicHttp;
					else if (url.HasApi("net")) protocol = Protocols.NetHttp;
					else if (url.HasApi("ws")) protocol = Protocols.WSHttp;
					else if (url.HasApi("grpc")) protocol = Protocols.gRPC;
					else if (url.HasApi("grpc/web")) protocol = Protocols.gRPCWeb;
					else if (url.HasApi("tcp")) protocol = Protocols.NetTcp;
					else if (url.HasApi("tcp/ssl")) protocol = Protocols.NetTcp;
					else protocol = Protocols.BasicHttp;

				}
				else throw new NotSupportedException("illegal protocol");
			}
		}

		public bool IsWCF => Protocol < Protocols.gRPC;
		public bool IsGRPC => Protocol >= Protocols.gRPC && Protocol < Protocols.Assembly;
		public bool IsAssembly => Protocol == Protocols.Assembly;
		public bool IsSsl => Protocol == Protocols.BasicHttps || Protocol == Protocols.WSHttps || Protocol == Protocols.NetHttps || Protocol == Protocols.NetTcpSsl || Protocol == Protocols.NetPipeSsl ||
			Protocol == Protocols.gRPCSsl || Protocol == Protocols.gRPCWebSsl;
		
		public bool IsSsh => Protocol == Protocols.Ssh || url.StartsWith("ssh://");
		public bool IsSecureProtocol => IsSsl || IsSsh;

		public bool IsHttp => Protocol <= Protocols.WSHttp;
		public bool IsHttps => Protocol >= Protocols.BasicHttps && Protocol <= Protocols.WSHttps;
		public bool IsEncrypted => this.GetType().GetInterfaces()
					.FirstOrDefault(i => i.GetCustomAttribute<ServiceContractAttribute>() != null)
					?.GetCustomAttribute<HasPolicyAttribute>() != null;

		static Dictionary<string, System.Net.IPAddress[]> ResolvedHosts = new Dictionary<string, System.Net.IPAddress[]>();

		public bool IsLocalAddress(string adr)
		{
			return Regex.IsMatch(adr, @"(^127\.)|(^192\.168\.)|(^10\.)|(^172\.1[6-9]\.)|(^172\.2[0-9]\.)|(^172\.3[0-1]\.)|(^::1$)|(^[fF][cCdD])", RegexOptions.Singleline);
		}


		public bool IsHostLocal(string host)
		{
			var isHostIP = Regex.IsMatch(host, @"^[0.9]{1,3}(?:\.[0-9]{1,3}){3}$", RegexOptions.Singleline) || Regex.IsMatch(host, @"^[0-9a-fA-F:]+$", RegexOptions.Singleline);
			if (host == "localhost" || host == "127.0.0.1" || host == "::1" ||
				isHostIP && IsLocalAddress(host)) return true;

			if (!isHostIP)
			{
				IPAddress[] ips;
				lock (ResolvedHosts)
				{
					if (!ResolvedHosts.TryGetValue(host, out ips))
					{
						ResolvedHosts.Add(host, ips = Dns.GetHostEntry(host).AddressList);
					}
				}
				return ips
					.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork || ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
					.All(ip => IsLocalAddress(ip.ToString()));
			}
			return false;
		}

		public bool IsLocal
		{
			get
			{
				if (url.StartsWith("ssh://")) return true;
				var host = new Uri(url).Host;
				return url.StartsWith("pipe://", StringComparison.OrdinalIgnoreCase) || IsHostLocal(host);
			}
		}

		public bool IsDefaultApi => !Regex.IsMatch(url, "(?:basic|net|ws|grpc|grpc/ssl|tcp|tcp/ssl|pipe|pipe/ssl)(?:/[A-Za-z0-9_¨]+)?/?(?:\\?|$)");

		public HasPolicyAttribute Policy
		{
			get
			{
				var serviceInterface = this.GetType().GetInterfaces()
					.FirstOrDefault(i => i.GetCustomAttribute<ServiceContractAttribute>() != null);
				return serviceInterface?.GetCustomAttribute<HasPolicyAttribute>();
			}
		}
		public bool IsAuthenticated
		{
			get
			{
				var serviceInterface = this.GetType().GetInterfaces()
					.FirstOrDefault(i => i.GetCustomAttribute<ServiceContractAttribute>() != null);
				var policy = serviceInterface?.GetCustomAttribute<HasPolicyAttribute>()?.Policy;
				return policy != null && policy != HasPolicyAttribute.Encrypted;
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

		public static string AssertScheme(this string url, Protocols protocol)
		{
			if (url.StartsWith("http://") &&
				!(protocol == Protocols.BasicHttp || protocol == Protocols.NetHttp || protocol == Protocols.WSHttp ||
				protocol == Protocols.gRPC || protocol == Protocols.gRPCWeb) ||
				url.StartsWith("https://") &&
				!(protocol == Protocols.BasicHttps || protocol == Protocols.NetHttps || protocol == Protocols.WSHttps ||
				protocol == Protocols.gRPCSsl || protocol == Protocols.gRPCWebSsl) ||
				url.StartsWith("net.tcp://") && !(protocol == Protocols.NetTcp || protocol == Protocols.NetTcpSsl) ||
				url.StartsWith("net.pipe://") && !(protocol == Protocols.NetPipe || protocol == Protocols.NetPipeSsl) ||
				url.StartsWith("assembly://") && protocol != Protocols.Assembly ||
				url.StartsWith("ssh://") && protocol != Protocols.Assembly && protocol != Protocols.NetPipe && protocol != Protocols.NetPipeSsl)
				throw new NotSupportedException("This protocol is not valid for this connection.");
			return url;
		}

		public static string SetApi(this string url, string api)
		{
			var parts = url.Split('?');
			url = parts[0];
			if (url[url.Length - 1] == '/') url = url.Substring(0, url.Length - 1);
			url = Regex.Replace(url, "(/(?:net|ws|basic|ssl|tcp|pipe|tcp/ssl|pipe/ssl|grpc|grpc/web))(?=(?:/[a-zA-Z0-9_]+)?$)|((?<!/(?:net|ws|basic|ssl|tcp|pipe|tcp/ssl|pipe/ssl|grpc|grpc/web)(?:/[a-zA-Z0-9_]+)?)$)", $"/{api}");

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


#if !NETFRAMEWORK && gRPC
		static Dictionary<string, GrpcChannel> GrpcPool = new Dictionary<string, GrpcChannel>();
#endif
		static readonly Dictionary<string, ChannelFactory<T>> FactoryPool = new Dictionary<string, ChannelFactory<T>>();
		ChannelFactory<T> factory;

		T client = null;

		Binding GetNamedPipeBinding(bool secure)
		{
			NetNamedPipeBinding pipe;
			if (secure) pipe = new NetNamedPipeBinding(NetNamedPipeSecurityMode.Transport);
			else pipe = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
			pipe.MaxReceivedMessageSize = MaximumMessageSize;
			return pipe;
		}

		static ConcurrentDictionary<string, SshClient> SshClients = new ConcurrentDictionary<string, SshClient>();
		static object SshLock = new object();
		static bool SshDisposed = false;

		const string ParseSshUrlRegex = @"(?<=^[a-zA-Z.]+://)(?<sshlogin>[^/]*)/(?<localport>[0-9]+:(?<host>\[[0-9a-fA-F:]+\]|[0-9a-zA-Z_-.]+):(?<remoteport>[0-9]+)(?=/|$|?";

		public static SshClient StartSshTunnel(string url)
		{
			lock (SshLock) if (SshDisposed) return null;

			var match = Regex.Match(url, ParseSshUrlRegex, RegexOptions.Singleline);
			var sshlogin = new Uri(match.Groups["sshlogin"].Value);
			var userTokens = sshlogin.UserInfo.Split(':');
			var user = userTokens[0];
			var password = "";
			if (userTokens.Length >= 2) password = userTokens[1];
			var localport = uint.Parse(match.Groups["localport"].Value);
			var host = match.Groups["host"].Value;
			var remoteport = uint.Parse(match.Groups["remoteport"].Value);

			var sshClient = new SshClient(sshlogin.Authority, user, password);
			ThreadPool.QueueUserWorkItem<object>(state =>
			{
				lock (sshClient)
				{
					var forwardedPort = new ForwardedPortLocal("127.0.0.1", localport, host, remoteport);
					sshClient.Connect();
					sshClient.AddForwardedPort(forwardedPort);
					forwardedPort.Start();
					var apppath = AppDomain.CurrentDomain.BaseDirectory;
					var appData = Path.Combine(apppath, "App_Data");
					if (!Directory.Exists(appData)) Directory.CreateDirectory(appData);
					var sshUrlFile = Path.Combine(appData, "SshUrls.txt");
					lock (SshLock) File.WriteAllLines(sshUrlFile, SshClients.Keys.ToArray());
				}
			}, null, false);
			return sshClient;
		}

		public static void StartAllSshTunnels()
		{
			string[] urls = new string[0];
			var apppath = AppDomain.CurrentDomain.BaseDirectory;
			var appData = Path.Combine(apppath, "App_Data");
			var sshUrlFile = Path.Combine(appData, "SshUrls.txt");

			lock (SshLock)
			{
				if (File.Exists(sshUrlFile)) urls = File.ReadAllLines(sshUrlFile);
			}
			foreach (var url in urls)
			{
				lock (SshLock) SshClients.GetOrAdd(url, StartSshTunnel);
			}
		}

		public static void DisposeAllSshTunnels()
		{
			lock (SshLock)
			{
				SshDisposed = true;

				foreach (var sshClient in SshClients.Values)
				{
					foreach (var port in sshClient.ForwardedPorts) port.Stop();
					sshClient.Disconnect();
					sshClient.Dispose();
				}
			}
		}
		protected T Client
		{
			get
			{
				var serviceurl = url;
				if (serviceurl.StartsWith("ssh://"))
				{
					SshClient sshClient;
					lock (SshLock) sshClient = SshClients.GetOrAdd(url, StartSshTunnel); // block until ssh tunnel is ready
					while (!sshClient.IsConnected) lock(sshClient) Thread.Sleep(0);

					if (IsHttp || IsHttps)
					{
						serviceurl = serviceurl.SetScheme("http");
					} else if (Protocol == Protocols.NetTcp || Protocol == Protocols.NetTcpSsl)
					{
						if (Protocol == Protocols.NetTcpSsl) Protocol = Protocols.NetTcp;
						serviceurl = serviceurl.SetScheme("net.tcp");
					}

					Regex.Replace(serviceurl, ParseSshUrlRegex, m => $"127.0.0.1:{m.Groups["localport"].Value}", RegexOptions.Singleline);
				}
				
				serviceurl = $"{serviceurl}/{this.GetType().Name}";

				if (client != null)
				{
					if (client is IClientChannel chan)
					{
						// reuse client if it uses the same address
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
							if (!isEncrypted || IsLocal)
							{
								var basic = new BasicHttpBinding(BasicHttpSecurityMode.None);
								basic.MaxReceivedMessageSize = MaximumMessageSize;
								binding = basic;
							}
							else
							{
								throw new NotSupportedException("This protocol is not secure on this connection.");
							}
							break;
						case Protocols.BasicHttps:
							var basics = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
							basics.MaxReceivedMessageSize = MaximumMessageSize;
							binding = basics;
							break;
						case Protocols.NetHttp:
							if (!isEncrypted || IsLocal)
							{
								var nethttp = new NetHttpBinding(BasicHttpSecurityMode.None);
								nethttp.MaxReceivedMessageSize = MaximumMessageSize;
								binding = nethttp;
							}
							else
							{
								throw new NotSupportedException("This protocol is not secure on this connection.");
							}
							break;
						case Protocols.NetHttps:
							var nethttps = new NetHttpBinding(BasicHttpSecurityMode.Transport);
							nethttps.MaxReceivedMessageSize = MaximumMessageSize;
							binding = nethttps;
							break;
						case Protocols.WSHttp:
							if (isEncrypted && UseMessageSecurityOverHttp)
							{
								var ws = new WSHttpBinding(SecurityMode.Message);
								ws.MaxReceivedMessageSize = MaximumMessageSize;
								binding = ws;
							}
							else if (!isEncrypted || IsLocal)
							{
								var ws = new WSHttpBinding(SecurityMode.None);
								ws.MaxReceivedMessageSize = MaximumMessageSize;
								binding = ws;
							}
							else
							{
								throw new NotSupportedException("This protocol is not secure on this connection.");
							}
							break;
						case Protocols.WSHttps:
							var wss = new WSHttpBinding(SecurityMode.Transport);
							wss.MaxReceivedMessageSize = MaximumMessageSize;
							binding = wss;
							break;
						case Protocols.NetTcp:
							if (!isEncrypted || IsLocal)
							{
								var tcp = new NetTcpBinding(SecurityMode.None);
								tcp.MaxReceivedMessageSize = MaximumMessageSize;
								binding = tcp;
							}
							else
							{
								throw new NotSupportedException("This protocol is not secure on this connection.");
							}
							break;
						case Protocols.NetTcpSsl:
							var tcps = new NetTcpBinding(SecurityMode.Transport);
							tcps.MaxReceivedMessageSize = MaximumMessageSize;
							binding = tcps;
							break;
						case Protocols.NetPipe:
							binding = GetNamedPipeBinding(false);
							break;
						case Protocols.NetPipeSsl:
							binding = GetNamedPipeBinding(true);
							break;
					}
					binding.ReceiveTimeout = Timeout ?? ReceiveTimeout;
					binding.SendTimeout = Timeout ?? SendTimeout;

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
					if (SoapHeader != null || Credentials != null && Credentials.Password != null && (IsSecureProtocol || IsLocal))
					{
						foreach (var b in factory.Endpoint.EndpointBehaviors.ToArray())
						{
							if (b is SoapHeaderClientBehavior) factory.Endpoint.EndpointBehaviors.Remove(b);
						}
						factory.Endpoint.EndpointBehaviors.Add(new SoapHeaderClientBehavior() { Client = this });
					}
					// set certificate validation mode to PeerOrChainTrust
					var clientCredentials = factory.Endpoint.EndpointBehaviors.OfType<ClientCredentials>().FirstOrDefault();
					if (clientCredentials == null)
					{
						clientCredentials = new ClientCredentials();
						factory.Endpoint.EndpointBehaviors.Add(clientCredentials);
					}
					clientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.PeerOrChainTrust;

					client = factory.CreateChannel();
					((IClientChannel)client).OperationTimeout = Timeout ?? TimeSpan.FromSeconds(120);


				}
#if !NETFRAMEWORK && gRPC
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
					assemblyClient.Client = this;
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
