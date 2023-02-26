#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
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

		public ServiceHost(object singletonInstance, params Uri[] baseAdresses) : base(object singletonInstance, params Uri[] baseAdresses)
		{
			AddEndpoints(singletonInstance.GetType(), baseAdresses);
		}
		void AddEndpoints(Type serviceType, Uri[] baseAdresses)
		{
			var contract = serviceType
				.GetInterfaces()
				.Where(i => i.GetCustomAttributes(false).OfType<ServiceContractAttribute>().Any())
				.FirstOrDefault();

			if (contract == null) throw new NotSupportedException();

			foreach (var adr in baseAdresses.Select(uri => uri.AbsoluteUri) {
				if (adr.StartsWith("http://"))
				{
					AddServiceEndpoint(contract, new BasicHttpBinding(BasicHttpSecurityMode.None), $"{adr}/basic");
					AddServiceEndpoint(contract, new NetHttpBinding(BasicHttpSecurityMode.None), $"{adr}/net");
					AddServiceEndpoint(contract, new WSHttpBinding(SecurityMode.None), $"{adr}/ws");
				}
				else if (adr.StartsWith("https://"))
				{
					AddServiceEndpoint(contract, new BasicHttpBinding(BasicHttpSecurityMode.TransportWithMessageCredential), $"{adr}/basic");
					AddServiceEndpoint(contract, new NetHttpBinding(BasicHttpSecurityMode.TransportWithMessageCredential), $"{adr}/net");
					AddServiceEndpoint(contract, new WSHttpBinding(SecurityMode.TransportWithMessageCredential), $"{adr}/ws");
				}
				else if (adr.StartsWith("net.tcp://"))
				{
					AddServiceEndpoint(contract, new NetTcpBinding(SecurityMode.None), adr);
					AddServiceEndpoint(contract, new NetTcpBinding(SecurityMode.TransportWithMessageCredential), $"{adr}/ssl");
				}
				else if (adr.StartsWith("net.pipe://"))
				{
					AddServiceEndpoint(contract, new NetNamedPipeBinding(NetNamedPipeSecurityMode.None), adr);
					AddServiceEndpoint(contract, new NetNamedPipeBinding(NetNamedPipeSecurityMode.Transport), $"{adr}/ssl");
				}
				Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true });
			}
		}

	}
}
#endif