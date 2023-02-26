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

	static class StringExtensions
	{
		public static string Strip(this string url, string api) => url.EndsWith(api) ? url.Substring(0, url.Length - api.Length) : url;
		public static string SetScheme(this string url, string scheme) => Regex.Replace(url, "[a-zA-Z.]://", $"{scheme}://");
		public static string Append(this string url, string api) => url + api;

	}
	// web service client
	public class ClientBase<T, U>
		where T: class
		where U : T, new()
	{

		public enum Protocols { BasicHttp, BasicHttps, NetHttp, NetHttps, WSHttp, WSHttps, NetTcp, NetTcpSsl, gRPC, gRPCSsl, gRPCWeb, gRPCWebSsl, Assembly }

		Protocols protocol = Protocols.NetHttp;
		public Protocols Protocol
		{
			get => protocol;
			set
			{
				if (value != protocol)
				{
					url = url
						.Strip("/basic")
						.Strip("/net")
						.Strip("/ws")
						.Strip("/gprc")
						.Strip("/gprc/web")
						.Strip("/ssl");
					if (value == Protocols.BasicHttp) url = url.SetScheme("http").Append("/basic");
					else if (value == Protocols.BasicHttps) url = url.SetScheme("https").Append("/basic");
					else if (value == Protocols.NetHttp) url = url.SetScheme("http").Append("/net");
					else if (value == Protocols.NetHttps) url = url.SetScheme("https").Append("/net");
					else if (value == Protocols.WSHttp) url = url.SetScheme("http").Append("/ws");
					else if (value == Protocols.WSHttps) url = url.SetScheme("https").Append("/ws");
					else if (value == Protocols.NetTcp) url = url.SetScheme("net.tcp");
					else if (value == Protocols.NetTcpSsl) url = url.SetScheme("net.tcp").Append("/ssl");
					else if (value == Protocols.gRPC) url = url.SetScheme("http").Append("/gprc");
					else if (value == Protocols.gRPCSsl) url = url.SetScheme("https").Append("/gprc");
					else if (value == Protocols.gRPCWeb) url = url.SetScheme("http").Append("/gprc/web");
					else if (value == Protocols.gRPCWebSsl) url = url.SetScheme("https").Append("/gprc/web");
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
					if (url.EndsWith("/basic")) protocol = Protocols.BasicHttp;
					else if (url.EndsWith("/net")) protocol = Protocols.NetHttp;
					else if (url.EndsWith("/ws")) protocol = Protocols.WSHttp;
					else if (url.EndsWith("grpc")) protocol = Protocols.gRPC;
					else if (url.EndsWith("gprc/web")) protocol = Protocols.gRPCWeb;
					else Protocol = Protocols.NetHttp;
				}
				else if (url.StartsWith("https://"))
				{
					if (url.EndsWith("/basic")) Protocol = Protocols.BasicHttps;
					else if (url.EndsWith("/net")) Protocol = Protocols.NetHttps;
					else if (url.EndsWith("/ws")) Protocol = Protocols.WSHttps;
					else if (url.EndsWith("grpc")) Protocol = Protocols.gRPCSsl;
					else if (url.EndsWith("gprc/web")) Protocol = Protocols.gRPCWebSsl;
					else Protocol = Protocols.NetHttps;
				}
				else if (url.StartsWith("net.tcp://"))
				{
					if (url.EndsWith("/ssl")) Protocol = Protocols.NetTcpSsl;
					else Protocol = Protocols.NetTcp;
				}
				else if (url.StartsWith("assembly://")) Protocol = Protocols.Assembly;
				else throw new NotSupportedException("illegal protocol");
			}
		}

		bool IsWCF => Protocol < Protocols.gRPC;
		bool IsGPRC => Protocol >= Protocols.gRPC && Protocol < Protocols.Assembly;
		bool IsAssembly => Protocol == Protocols.Assembly;
		bool IsSsl => Protocol == Protocols.BasicHttps || Protocol == Protocols.WSHttps || Protocol == Protocols.NetTcpSsl || Protocol == Protocols.gRPCSsl
			|| Protocol == Protocols.gRPCWebSsl;

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
					if (Protocol == Protocols.BasicHttp) binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
					else if (Protocol == Protocols.BasicHttps) binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportWithMessageCredential);
					else if (Protocol == Protocols.NetHttp) binding = new NetHttpBinding(BasicHttpSecurityMode.None);
					else if (Protocol == Protocols.NetHttps) binding = new NetHttpBinding(BasicHttpSecurityMode.TransportWithMessageCredential);
					else if (Protocol == Protocols.WSHttp) binding = new WSHttpBinding(SecurityMode.None);
					else if (Protocol == Protocols.WSHttps) binding = new WSHttpBinding(SecurityMode.TransportWithMessageCredential);
					var endpoint = new EndpointAddress(url);
					ChannelFactory<T> factory;

					if (!FactoryPool.TryGetValue(url, out factory))
					{
						FactoryPool[url] = factory = new ChannelFactory<T>(binding, endpoint);
					}
					client = factory.CreateChannel();
				}
#if NETCOREAPP
				else if (IsGPRC)
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
