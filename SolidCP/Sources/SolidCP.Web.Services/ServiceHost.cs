#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SolidCP.Web.Services
{
	public enum Protocols { BasicHttp, BasicHttps, NetHttp, NetHttps, WSHttp, WSHttps, NetTcp, NetTcpSsl, gRPC, gRPCSsl, gRPCWeb, gRPCWebSsl, Assembly }

	public class ServiceHost : System.ServiceModel.ServiceHost
	{
		public ServiceHost() : base() { }
		public ServiceHost(Type serviceType, params Uri[] baseAdresses) : base(serviceType, baseAdresses)
		{
			AddEndpoints(serviceType, baseAdresses);
		}

		public ServiceHost(object singletonInstance, params Uri[] baseAdresses) : base(singletonInstance, baseAdresses)
		{
			AddEndpoints(singletonInstance.GetType(), baseAdresses);
		}

		bool HasApi(string adr, string api) => Regex.IsMatch(adr, $"/{api}/[a-zA-Z0-9_]+(?:\\?|$)");

		void AddEndpoint(Type contract, Binding binding, string address)
		{
			var endpoint = AddServiceEndpoint(contract, binding, address);
			endpoint.EndpointBehaviors.Add(new SoapHeaderMessageInspector());
		}
		void AddEndpoints(Type serviceType, Uri[] baseAdresses)
		{
			var contract = serviceType
				.GetInterfaces()
				.FirstOrDefault(i => i.GetCustomAttributes(false).OfType<ServiceContractAttribute>().Any());

			if (contract == null) throw new NotSupportedException();

			var isAuthenticated = contract.GetCustomAttributes(false).OfType<Microsoft.Web.Services3.PolicyAttribute>().Any();
			if (isAuthenticated)
			{
				Credentials.UserNameAuthentication.UserNamePasswordValidationMode = System.ServiceModel.Security.UserNamePasswordValidationMode.Custom;
				Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new SolidCP.Web.Services.UserNamePasswordValidator();
			}

			foreach (var adr in baseAdresses.Select(uri => uri.AbsoluteUri)) {
				if (adr.StartsWith("http://"))
				{

					if (HasApi(adr, "basic"))
					{
						if (isAuthenticated) throw new NotSupportedException("Api not supported.");
						else AddEndpoint(contract, new BasicHttpBinding(BasicHttpSecurityMode.None), adr);
					}
					else if (HasApi(adr, "net"))
					{
						if (isAuthenticated) throw new NotSupportedException("Api not supported.");
						AddEndpoint(contract, new NetHttpBinding(BasicHttpSecurityMode.None), adr);
					}
					else if (HasApi(adr, "ws"))
					{
						if (isAuthenticated) AddEndpoint(contract, new WSHttpBinding(SecurityMode.Message), adr);
						else AddEndpoint(contract, new WSHttpBinding(SecurityMode.None), adr);
					}
					else
					{
						if (isAuthenticated) AddEndpoint(contract, new WSHttpBinding(SecurityMode.Message), adr);
						else AddEndpoint(contract, new NetHttpBinding(BasicHttpSecurityMode.None), adr);
					}
				}
				else if (adr.StartsWith("https://"))
				{
					if (HasApi(adr, "basic"))
						AddEndpoint(contract, new BasicHttpBinding(BasicHttpSecurityMode.TransportWithMessageCredential), adr);
					else if (HasApi(adr, "ws"))
						AddEndpoint(contract, new WSHttpBinding(SecurityMode.TransportWithMessageCredential), adr);
					else
						AddEndpoint(contract, new NetHttpBinding(BasicHttpSecurityMode.TransportWithMessageCredential), adr);
				}
				else if (adr.StartsWith("net.tcp://"))
				{
					if (HasApi(adr, "ssl"))
						AddEndpoint(contract, new NetTcpBinding(SecurityMode.TransportWithMessageCredential), adr);
					else AddEndpoint(contract, new NetTcpBinding(SecurityMode.Message), adr);
				}

#if NETFRAMEWORK
				else if (adr.StartsWith("net.pipe://"))
				{
					if (HasApi(adr, "ssl"))
						AddEndpoint(contract, new NetNamedPipeBinding(NetNamedPipeSecurityMode.Transport), adr);
					else
					{
						if (isAuthenticated) throw new NotSupportedException("Api not supported.");
						else AddEndpoint(contract, new NetNamedPipeBinding(NetNamedPipeSecurityMode.None), adr);
					}
				}
#endif
				
				Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true });
			}
		}

	}
}
#endif