#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using SolidCP.Web.Services;

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

		bool HasApi(string adr, string api) => Regex.IsMatch(adr, $"{api}/[a-zA-Z0-9_]+(?:\\?|$)");

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


/* Unmerged change from project 'SolidCP.Web.Services (net48)'
Before:
			var policy = contract.GetCustomAttributes(false).OfType<Microsoft.Web.Services3.PolicyAttribute>().FirstOrDefault();
After:
			var policy = contract.GetCustomAttributes(false).OfType<PolicyAttribute>().FirstOrDefault();
*/
			var policy = contract.GetCustomAttributes(false).OfType<Services.PolicyAttribute>().FirstOrDefault();
			var isAuthenticated = policy != null;


			Credentials.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;
			Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new UserNamePasswordValidator() { Policy = policy };
			var behavior = Description.Behaviors.Find<ServiceDebugBehavior>();
			if (behavior != null) behavior.IncludeExceptionDetailInFaults = true;

			//Credentials.ServiceCertificate.SetCertificate(
			//	StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, "localhost");

			foreach (var adr in baseAdresses.Select(uri => uri.AbsoluteUri)) {
				if (adr.StartsWith("http://"))
				{

					if (HasApi(adr, "basic"))
					{
						if (isAuthenticated)
						{
							// var binding = new BasicHttpBinding(BasicHttpSecurityMode.Message);
							// binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
							// AddEndpoint(contract, binding, adr);
							//throw new NotSupportedException("Api not supported.");
						}
						else AddEndpoint(contract, new BasicHttpBinding(BasicHttpSecurityMode.None) { Name = "basic.none" }, adr);
					}
					else if (HasApi(adr, "net"))
					{
						if (isAuthenticated)
						{
							//var binding = new NetHttpBinding(BasicHttpSecurityMode.Message);
							//binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
							//AddEndpoint(contract, binding, adr);
							//throw new NotSupportedException("Api not supported.");
						}
						else AddEndpoint(contract, new NetHttpBinding(BasicHttpSecurityMode.None) { Name = "net.none" }, adr);
					}
					else if (HasApi(adr, "ws"))
					{
						if (isAuthenticated)
						{
							/* var binding = new WSHttpBinding(SecurityMode.Message);
							binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
							binding.Security.Message.NegotiateServiceCredential = true;
							binding.Name = "ws.message";
							AddEndpoint(contract, binding, adr); */
						}
						else AddEndpoint(contract, new WSHttpBinding(SecurityMode.None) { Name = "ws.none" }, adr);
					}
					else
					{
						if (isAuthenticated)
						{
							/* var binding = new WSHttpBinding(SecurityMode.Message);
							binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
							binding.Security.Message.NegotiateServiceCredential = true;
							binding.Name = "ws.message";
							AddEndpoint(contract, binding, adr); */
						}
						else AddEndpoint(contract, new NetHttpBinding(BasicHttpSecurityMode.None) { Name = "net.none" }, adr);
					}
				}
				else if (adr.StartsWith("https://"))
				{
					if (HasApi(adr, "basic"))
					{
						if (isAuthenticated)
						{
							var binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportWithMessageCredential);
							binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
							binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
							binding.Name = "basic.transportwithmessage";
							AddEndpoint(contract, binding, adr);
						} else
						{
							var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
                            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                            binding.Name = "basic.transport";
							AddEndpoint(contract, binding, adr);
						}
					}
					else if (HasApi(adr, "ws")) {
						if (isAuthenticated)
						{
							var binding = new WSHttpBinding(SecurityMode.TransportWithMessageCredential);
							binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
							binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
							binding.Name = "ws.transportwithmessage";
							AddEndpoint(contract, binding, adr);
						} else
						{
                            var binding = new WSHttpBinding(SecurityMode.Transport);
                            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                            binding.Name = "ws.transportwithmessage";
                            AddEndpoint(contract, binding, adr);
                        }
                    }
					else if (HasApi(adr, "net"))
					{
						if (isAuthenticated)
						{
							var binding = new NetHttpBinding(BasicHttpSecurityMode.TransportWithMessageCredential);
							binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
							binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
							binding.Name = "net.transportwithmessage";
							AddEndpoint(contract, binding, adr);
						} else
						{
                            var binding = new NetHttpBinding(BasicHttpSecurityMode.Transport);
                            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                            binding.Name = "net.transport";
                            AddEndpoint(contract, binding, adr);
                        }
                    }
				}
				else if (adr.StartsWith("net.tcp://"))
				{
					if (HasApi(adr, "nettcp"))
					{
						if (isAuthenticated)
						{
							var binding = new NetTcpBinding(SecurityMode.TransportWithMessageCredential);
							binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
							binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
							binding.Name = "nettcp.transportwithmessage";
							AddEndpoint(contract, binding, adr);
						} else AddEndpoint(contract, new NetTcpBinding(SecurityMode.None) {  Name="nettcp.none" }, adr);
					}
				}
				else if (adr.StartsWith("net.pipe://"))
				{
					if (HasApi(adr, "pipe"))
					{
						if (isAuthenticated) AddEndpoint(contract, new NetNamedPipeBinding(NetNamedPipeSecurityMode.Transport) { Name="pipe.transport" }, adr);
						else AddEndpoint(contract, new NetNamedPipeBinding(NetNamedPipeSecurityMode.None) { Name="pipe.none" }, adr);
					}
				}
				
				if (!Description.Behaviors.OfType<ServiceMetadataBehavior>().Any())
					Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true, HttpsGetEnabled = true });
			}
		}

	}
}
#endif