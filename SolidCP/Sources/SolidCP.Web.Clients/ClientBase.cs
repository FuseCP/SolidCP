using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.ServiceModel;
using System.Net;
using System.ServiceModel.Channels;
using System.Security.Policy;
using System.Threading.Tasks;
#if NETCOREAPP
using ProtoBuf.Grpc.Client;
using Grpc.Net.Client;
#endif

namespace SolidCP.Web.Client
{

	public enum Protocols { BasicHttp, BasicHttps, NetHttp, NetHttps, WSHttp, WSHttps, NetTcp, NetTcpSsl, NetPipe, NetPipeSsl, gRPC, gRPCSsl, gRPCWeb, gRPCWebSsl, Assembly }

	static class StringExtensions
	{
		public static string Strip(this string url, string api) => url.Replace($"/{api}/", "/");
		public static string SetScheme(this string url, string scheme) => Regex.Replace(url, "[a-zA-Z.]://", $"{scheme}://");
		public static string SetApi(this string url, string api) => Regex.Replace(url, "/(?:net/|ws/|basic/|ssl/|grpc/|grpc/web/)(?=[a-zA-Z0-9_]+\\?|$)", $"/{api}/");

		public static bool HasApi(this string url, string api) => url.Contains($"/{api}/");
	}
	// web service client
	public class ClientBase<T, U>
		where T: class
		where U : T, new()
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
						.Strip("ssl");
					if (value == Protocols.BasicHttp) url = url.SetScheme("http").SetApi("basic");
					else if (value == Protocols.BasicHttps) url = url.SetScheme("https").SetApi("basic");
					else if (value == Protocols.NetHttp) url = url.SetScheme("http").SetApi("net");
					else if (value == Protocols.NetHttps) url = url.SetScheme("https").SetApi("net");
					else if (value == Protocols.WSHttp) url = url.SetScheme("http").SetApi("ws");
					else if (value == Protocols.WSHttps) url = url.SetScheme("https").SetApi("ws");
					else if (value == Protocols.NetTcp) url = url.SetScheme("net.tcp");
					else if (value == Protocols.NetTcpSsl) url = url.SetScheme("net.tcp").SetApi("ssl");
					else if (value == Protocols.gRPC) url = url.SetScheme("http").SetApi("grpc");
					else if (value == Protocols.gRPCSsl) url = url.SetScheme("https").SetApi("grpc");
					else if (value == Protocols.gRPCWeb) url = url.SetScheme("http").SetApi("grpc/web");
					else if (value == Protocols.gRPCWebSsl) url = url.SetScheme("https").SetApi("grpc/web");
					else if (value == Protocols.Assembly) url = url.SetScheme("assembly");
				}
				protocol = value;
			}
		}
		public ICredentials Credentials { get; set; }
		public object SoapHeader { get; set; }

		string url;
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
					else if (url.HasApi("gprc/web")) protocol = Protocols.gRPCWeb;
					else Protocol = Protocols.NetHttp;
				}
				else if (url.StartsWith("https://"))
				{
					if (url.HasApi("basic")) Protocol = Protocols.BasicHttps;
					else if (url.HasApi("net")) Protocol = Protocols.NetHttps;
					else if (url.HasApi("ws")) Protocol = Protocols.WSHttps;
					else if (url.HasApi("grpc")) Protocol = Protocols.gRPCSsl;
					else if (url.HasApi("gprc/web")) Protocol = Protocols.gRPCWebSsl;
					else Protocol = Protocols.NetHttps;
				}
				else if (url.StartsWith("net.tcp://"))
				{
					if (url.HasApi("ssl")) Protocol = Protocols.NetTcpSsl;
					else Protocol = Protocols.NetTcp;
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

		bool IsWCF => Protocol < Protocols.gRPC;
		bool IsGRPC => Protocol >= Protocols.gRPC && Protocol < Protocols.Assembly;
		bool IsAssembly => Protocol == Protocols.Assembly;
		bool IsSsl => Protocol == Protocols.BasicHttps || Protocol == Protocols.WSHttps || Protocol == Protocols.NetTcpSsl || Protocol == Protocols.NetPipeSsl ||
			Protocol == Protocols.gRPCSsl || Protocol == Protocols.gRPCWebSsl;

#if NETCOREAPP
		static Dictionary<string, GrpcChannel> GrpcPool = new Dictionary<string, GrpcChannel>();
#endif
		static Dictionary<string, ChannelFactory<T>> FactoryPool = new Dictionary<string, ChannelFactory<T>>();

		protected T Client
		{
			get
			{
				T client = default(T);
				if (IsWCF)
				{
					Binding binding = null;
					switch (Protocol)
					{
						case Protocols.BasicHttp: binding = new BasicHttpBinding(BasicHttpSecurityMode.None); break;
						case Protocols.BasicHttps: binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportWithMessageCredential); break;
						case Protocols.NetHttp: binding = new NetHttpBinding(BasicHttpSecurityMode.None); break;
						case Protocols.NetHttps: binding = new NetHttpBinding(BasicHttpSecurityMode.TransportWithMessageCredential); break;
						case Protocols.WSHttp: binding = new WSHttpBinding(SecurityMode.None); break;
						case Protocols.WSHttps: binding = new WSHttpBinding(SecurityMode.TransportWithMessageCredential); break;
						case Protocols.NetTcp: binding = new NetTcpBinding(SecurityMode.None); break;
						case Protocols.NetTcpSsl: binding = new NetTcpBinding(SecurityMode.TransportWithMessageCredential); break;
#if NETFRAMEWORK
						case Protocols.NetPipe: binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None); break;
						case Protocols.NetPipeSsl: binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.Transport); break;
#endif
					}
					var endpoint = new EndpointAddress(url);
					ChannelFactory<T> factory;

					if (!FactoryPool.TryGetValue(url, out factory))
					{
						FactoryPool[url] = factory = new ChannelFactory<T>(binding, endpoint);
					}
					client = factory.CreateChannel();
				}
#if NETCOREAPP
				else if (IsGRPC)
				{
					GrpcChannel channel;
					if (!GrpcPool.TryGetValue(url, out channel)) {
						GrpcPool[url] = channel = GrpcChannel.ForAddress(url);
					}
					client = channel.CreateGrpcService<T>();
				}
#endif
				else if (IsAssembly)
				{
					client = new U();
				}
				else throw new NotSupportedException("Unsupported protocol in SolidCP.Web.Clients.ClientBase");
				if (client is IClientChannel) ((IClientChannel)client).Open();
				return client;
			}
		}

		protected void Close(T client)
		{
			if (client is IClientChannel) ((IClientChannel)client).Close();
		}
		

		public ClientBase() { }
		public ClientBase(string url) : this()
		{
			Url = url;
		}
	}
}
