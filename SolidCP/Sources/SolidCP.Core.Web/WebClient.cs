using System;
using System.Collections.Generic;
using System.Text;
using CoreWCF;
using CoreWCF.Channels;
using CoreWCF.Description;
using CoreWCF.Configuration;
using System.ServiceModel;

namespace SolidCP.Core.Build
{
	public class WebClient
	{

		interface ISomeContract
		{
			void SomeMethod();
		}
		// web service client
		public class TestClient
		{

			public enum Protocols { BasicHttp, BasicHttps, WSHttp, WSHttps, NetTcp, NetTcpSsl, gRPC, gRPCSsl, gRPCWeb, gRPCWebSsl, Assembly }

			public Protocols Protocol { get; set; }
			public Credentials Credentials { get; set; }
			string _url;
			public string Url {
				get { return _url; }
				set
				{
					_url = value;
					if (_url.StartsWith("http://"))
					{
						if (_url.EndsWith("/basic")) Protocol = Protocols.BasicHttp;
						else if (_url.EndsWith("/ws")) Protocol = Protocols.WSHttp;
						else if (_url.EndsWith("grpc")) Protocol = Protocols.gRPC;
						else if (_url.EndsWith("gprc/web")) Protocol = Protocols.gRPCWeb;
						else throw new NotSupportedException("illegal protocol");
					}
					else if (_url.StartsWith("https://"))
					{
						if (_url.EndsWith("/basic")) Protocol = Protocols.BasicHttps;
						else if (_url.EndsWith("/ws")) Protocol = Protocols.WSHttps;
						else if (_url.EndsWith("grpc")) Protocol = Protocols.gRPCSsl;
						else if (_url.EndsWith("gprc/web")) Protocol = Protocols.gRPCWebSsl;
						else throw new NotSupportedException("illegal protocol");
					}
					else if (_url.StartsWith("net.tcp://"))
					{
						if (_url.EndsWith("/ssl")) Protocol = Protocols.NetTcpSsl;
						else Protocol = Protocols.NetTcp;
					}
					else if (_url.StartsWith("assembly://")) Protocol = Protocols.Assembly;
					else throw new NotSupportedException("illegal protocol");
				}
			}
			public object SoapHeader { get; set; }

			bool IsWCF => Protocol < Protocols.gRPC;
			bool IsgPRC => Protocol >= Protocols.gRPC && Protocol < Protocols.Assembly;
			bool IsAssembly => Protocol == Protocols.Assembly;
			bool IsSsl => Protocol == Protocols.BasicHttps || Protocol == Protocols.WSHttps || Protocol == Protocols.NetTcpSsl || Protocol == Protocols.gRPCSsl
				|| Protocol == Protocols.gRPCWebSsl;

			ISomeContract Client;
			public TestClient() {	}
			public TestClient(string url): this() { Url = url; }
			void InitClient()
			{
				if (IsWCF)
				{
					if (Protocol == Protocols.BasicHttp)
					{
						var bindig = new BasicHttpBinding(BasicHttpSecurityMode.None);
						var endpoint = new EnpointAddress();
						var factory = new Channel
					}
				}
			}
		public method.ReturnType @method.Name @method.ParameterList {
			InitClient();
			try
			{
				@m.ReturnToken base.@method.Name(@method.Arguments);
			}

								}
	}
}
