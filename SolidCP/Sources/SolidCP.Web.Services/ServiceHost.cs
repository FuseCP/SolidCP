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
using System.Xml;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using SolidCP.Web.Services;

namespace SolidCP.Web.Services
{
	public enum Protocols { BasicHttp, BasicHttps, NetHttp, NetHttps, WSHttp, WSHttps, NetTcp, NetTcpSsl, RESTHttp, RESTHttps, gRPC, gRPCSsl, gRPCWeb, gRPCWebSsl, Assembly }

	public class ServiceHost : System.ServiceModel.ServiceHost
	{

		public const int KB = Configuration.KB;
		public const int MB = Configuration.MB;
		public const int MaxReceivedMessageSize = Configuration.MaxReceivedMessageSize;
		public const int MaxBufferSize = Configuration.MaxBufferSize;
		public const int MaxBytesPerRead = Configuration.MaxBytesPerRead;
		public const int MaxDepth = Configuration.MaxDepth;
		public const int MaxArrayLength = Configuration.MaxArrayLength;
		public const int MaxStringContentLength = Configuration.MaxStringContentLength;
		public const int MaxNameTableCharCount = Configuration.MaxNameTableCharCount;

		public ServiceHost() : base() { }
		public ServiceHost(Type serviceType, params Uri[] baseAddresses) : base(serviceType, baseAddresses)
		{
			AddEndpoints(serviceType, baseAddresses);
		}

		public ServiceHost(object singletonInstance, params Uri[] baseAddresses) : base(singletonInstance, baseAddresses)
		{
			AddEndpoints(singletonInstance.GetType(), baseAddresses);
		}

		bool HasApi(string adr, string api) => Regex.IsMatch(adr, $"/{api}/[a-zA-Z0-9_]+(?:\\?|$)");
		bool IsHttp(string adr) => adr.StartsWith("http://", StringComparison.OrdinalIgnoreCase);
		bool IsHttps(string adr) => adr.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
		bool IsNetTcp(string adr) => adr.StartsWith("net.tcp://", StringComparison.OrdinalIgnoreCase);
		bool IsPipe(string adr) => adr.StartsWith("pipe://", StringComparison.OrdinalIgnoreCase);

		bool IsLocal(string adr)
		{
			var host = new Uri(adr).Host;
			var hostIsIP = Regex.IsMatch(host, @"^[0.9]{1,3}(?:\.[0-9]{1,3}){3}$", RegexOptions.Singleline) || Regex.IsMatch(host, @"^[0-9a-fA-F:]+$", RegexOptions.Singleline);
			return host == "localhost" || host == "127.0.0.1" || host == "::1" ||
				hostIsIP && Regex.IsMatch(host, @"(^127\.)|(^192\.168\.)|(^10\.)|(^172\.1[6-9]\.)|(^172\.2[0-9]\.)|(^172\.3[0-1]\.)|(^::1$)|(^[fF][cCdD])", RegexOptions.Singleline) || // local network ip
				IsPipe(adr);
		}
		const bool AllowInsecureHttp = PolicyAttribute.AllowInsecureHttp;
		const bool UseMessageSecurityOverHttp = PolicyAttribute.UseMessageSecurityOverHttp;

		void AddEndpoint(Type contract, Binding binding, string address)
		{
			//binding.CloseTimeout = binding.OpenTimeout = binding.ReceiveTimeout = binding.SendTimeout = TimeSpan.FromMinutes(10);
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
			} else if (binding is NetNamedPipeBinding pipeBinding)
			{
				pipeBinding.MaxReceivedMessageSize = MaxReceivedMessageSize;
				pipeBinding.MaxBufferSize = MaxBufferSize;
				pipeBinding.ReaderQuotas = readerQuotas;
			}
			var endpoint = AddServiceEndpoint(contract, binding, address);
			endpoint.EndpointBehaviors.Add(new SoapHeaderMessageInspector());
		}
		void AddWebEndpoint(Type contract, WebHttpBinding binding, string address)
		{
			//binding.CloseTimeout = binding.OpenTimeout = binding.ReceiveTimeout = binding.SendTimeout = TimeSpan.FromMinutes(10);
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
			binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
			if (Authorization.ServiceAuthorizationManager == null || !(Authorization.ServiceAuthorizationManager is RestAuthorizationManager))
			{
				Authorization.ServiceAuthorizationManager = new RestAuthorizationManager();
			}
			var endpoint = AddServiceEndpoint(contract, binding, address);
			var messageInspector = new SoapHeaderMessageInspector();
			var behavior = new WebHttpBehavior();
			behavior.AutomaticFormatSelectionEnabled = true;
			behavior.HelpEnabled = true;
			endpoint.EndpointBehaviors.Add(messageInspector);
			endpoint.EndpointBehaviors.Add(behavior);
		}

		void AddEndpoints(Type serviceType, Uri[] baseAdresses)
		{
			var contract = serviceType
				.GetInterfaces()
				.FirstOrDefault(i => i.GetCustomAttributes(false).OfType<ServiceContractAttribute>().Any());

			if (contract == null) throw new NotSupportedException("The type has no ServiceContract attribute.");

			var policy = contract.GetCustomAttributes(false).OfType<PolicyAttribute>().FirstOrDefault();
			var isEncrypted = policy != null;
			var isAuthenticated = isEncrypted && policy.Policy != PolicyAttribute.Encrypted;

			Credentials.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;
			Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new UserNamePasswordValidator() { Policy = policy };
			var behavior = Description.Behaviors.Find<ServiceDebugBehavior>();
			if (behavior != null) behavior.IncludeExceptionDetailInFaults = true;
			else Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

			//Credentials.ServiceCertificate.SetCertificate(
			//	StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, "localhost");

			foreach (var adr in baseAdresses.Select(uri => uri.AbsoluteUri))
			{
				if (IsHttp(adr))
				{
					if (HasApi(adr, "api"))
					{
						if (!isEncrypted || IsLocal(adr) || AllowInsecureHttp)
						{
							if (!isAuthenticated) AddWebEndpoint(contract, new WebHttpBinding(WebHttpSecurityMode.None) { Name = "rest.none" }, adr);
							else AddWebEndpoint(contract, new WebHttpBinding(WebHttpSecurityMode.TransportCredentialOnly) { Name = "rest.credential" }, adr);
						}
					}
					else if (HasApi(adr, "basic"))
					{
						if (!isEncrypted || IsLocal(adr) || AllowInsecureHttp)
						{
							AddEndpoint(contract, new BasicHttpBinding(BasicHttpSecurityMode.None) { Name = "basic.none" }, adr);
						}
					}
					else if (HasApi(adr, "net"))
					{
						if (!isEncrypted || IsLocal(adr) || AllowInsecureHttp)
						{
							var netHttpBinding = new NetHttpBinding(BasicHttpSecurityMode.None) { Name = "net.none" };
							netHttpBinding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Never;
							AddEndpoint(contract, netHttpBinding, adr);
						}
					}
					else if (HasApi(adr, "ws"))
					{
						if (isEncrypted && UseMessageSecurityOverHttp)
						{
							var wsHttpBinding = new WSHttpBinding(SecurityMode.Message) { Name = "ws.message" };
							AddEndpoint(contract, wsHttpBinding, adr);
						}
						else if (!isEncrypted || IsLocal(adr) || AllowInsecureHttp)
						{
							AddEndpoint(contract, new WSHttpBinding(SecurityMode.None) { Name = "ws.none" }, adr);
						}
					}
					else
					{
						if (!isEncrypted || IsLocal(adr) || AllowInsecureHttp)
						{
							AddEndpoint(contract, new BasicHttpBinding(BasicHttpSecurityMode.None) { Name = "net.none" }, adr);
						}
					}
				}
				else if (IsHttps(adr))
				{
					if (HasApi(adr, "api"))
					{
						var binding = new WebHttpBinding(WebHttpSecurityMode.Transport);
						binding.Name = "rest.transport";
						binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
						AddWebEndpoint(contract, binding, adr);
					}
					else if (HasApi(adr, "basic"))
					{
						var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
						binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
						binding.Name = "basic.transport";
						AddEndpoint(contract, binding, adr);
					}
					else if (HasApi(adr, "ws"))
					{
						var binding = new WSHttpBinding(SecurityMode.Transport);
						binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
						binding.Name = "ws.transportwithmessage";
						AddEndpoint(contract, binding, adr);
					}
					else if (HasApi(adr, "net"))
					{
						var binding = new NetHttpBinding(BasicHttpSecurityMode.Transport);
						binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
						binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Never;
						binding.Name = "net.transport";
						AddEndpoint(contract, binding, adr);
					}
					else
					{
						var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
						binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
						binding.Name = "basic.transport";
						AddEndpoint(contract, binding, adr);
					}
				}
				else if (adr.StartsWith("net.tcp://", StringComparison.OrdinalIgnoreCase))
				{
					if (HasApi(adr, "tcp/ssl"))
					{
						var binding = new NetTcpBinding(SecurityMode.Transport);
						binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
						binding.Name = "nettcp.transport";
						AddEndpoint(contract, binding, adr);
					}
					else if (HasApi(adr, "tcp"))
					{
						if (!isEncrypted || IsLocal(adr) || AllowInsecureHttp)
						{
							AddEndpoint(contract, new NetTcpBinding(SecurityMode.None) { Name = "nettcp.none" }, adr);
						}
					}
				}
				else if (adr.StartsWith("net.pipe://", StringComparison.OrdinalIgnoreCase))
				{
					if (HasApi(adr, "pipe/ssl"))
					{
						AddEndpoint(contract, new NetNamedPipeBinding(NetNamedPipeSecurityMode.Transport) { Name = "pipe.transport" }, adr);
					}
					else if (HasApi(adr, "pipe"))
					{
						AddEndpoint(contract, new NetNamedPipeBinding(NetNamedPipeSecurityMode.None) { Name = "pipe.none" }, adr);
					}
				}

				var meta = Description.Behaviors.OfType<ServiceMetadataBehavior>().FirstOrDefault();
				if (meta == null)
				{
					meta = new ServiceMetadataBehavior();
					Description.Behaviors.Add(meta);
				}
				if (IsHttp(adr)) meta.HttpGetEnabled = true;
				else if (IsHttps(adr)) meta.HttpsGetEnabled = true;

				meta.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;

				/* if (IsHttp(adr)) AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), $"{adr}/mex");
				if (IsHttps(adr)) AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpsBinding(), $"{adr}/mex");
				if (IsNetTcp(adr)) AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexTcpBinding(), $"{adr}/mex");
				if (IsPipe(adr)) AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexNamedPipeBinding(), $"{adr}/mex");
				*/
			}
		}

	}
}
#endif