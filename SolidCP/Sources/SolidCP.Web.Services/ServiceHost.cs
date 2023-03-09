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

		void AddEndpoint(Type contract, Binding binding, string address)
		{
			var endpoint = AddServiceEndpoint(contract, binding, address);
			endpoint.EndpointBehaviors.Add(new SoapHeaderMessageInspector());
		}
		void AddEndpoints(Type serviceType, Uri[] baseAdresses)
		{
			var contract = serviceType
				.GetInterfaces()
				.Where(i => i.GetCustomAttributes(false).OfType<ServiceContractAttribute>().Any())
				.FirstOrDefault();

			if (contract == null) throw new NotSupportedException();

			Credentials.UserNameAuthentication.UserNamePasswordValidationMode = System.ServiceModel.Security.UserNamePasswordValidationMode.Custom;
			Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new UserNamePasswordValidator();

			foreach (var adr in baseAdresses.Select(uri => uri.AbsoluteUri)) {
				if (adr.StartsWith("http://"))
				{
				
					if (Regex.IsMatch(adr, "/basic/[a-zA-Z0-9_]+(?:\\?|$)"))
						AddEndpoint(contract, new BasicHttpBinding(BasicHttpSecurityMode.None), adr);
					else if (Regex.IsMatch(adr, "/ws/[a-zA-Z0-9_]+(?:\\?|$)"))
						AddEndpoint(contract, new WSHttpBinding(SecurityMode.None), adr);
					else
						AddEndpoint(contract, new NetHttpBinding(BasicHttpSecurityMode.None), adr);
				}
				else if (adr.StartsWith("https://"))
				{
					if (Regex.IsMatch(adr, "/basic/[a-zA-Z0-9_]+(?:\\?|$)"))
						AddEndpoint(contract, new BasicHttpBinding(BasicHttpSecurityMode.TransportWithMessageCredential), adr);
					else if (Regex.IsMatch(adr, "/ws/[a-zA-Z0-9_]+(?:\\?|$)"))
						AddEndpoint(contract, new WSHttpBinding(SecurityMode.TransportWithMessageCredential), adr);
					else
						AddEndpoint(contract, new NetHttpBinding(BasicHttpSecurityMode.TransportWithMessageCredential), adr);
				}
				else if (adr.StartsWith("net.tcp://"))
				{
					if (Regex.IsMatch(adr, "/ssl/[a-zA-Z0-9_]+(?:\\?|$)"))
						AddEndpoint(contract, new NetTcpBinding(SecurityMode.TransportWithMessageCredential), adr);						
					else AddEndpoint(contract, new NetTcpBinding(SecurityMode.None), adr);
				}

#if NETFRAMEWORK
				else if (adr.StartsWith("net.pipe://"))
				{
					if (Regex.IsMatch(adr, "/ssl/[a-zA-Z0-9_]+(?:\\?|$)"))
						AddEndpoint(contract, new NetNamedPipeBinding(NetNamedPipeSecurityMode.Transport), adr);
					else AddEndpoint(contract, new NetNamedPipeBinding(NetNamedPipeSecurityMode.None), adr);
				}
#endif
				
				Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true });
			}
		}

	}
}
#endif