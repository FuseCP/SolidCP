using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using SolidCP.Providers;

namespace SolidCP.Web.Clients
{
	public class SoapHeaderClientMessageInspector : IClientMessageInspector
	{
		public const string Namespace = "http://solidcp.com/headers/";

		public ClientBase Client;
		public object SoapHeader => Client.SoapHeader;
		
		public void AfterReceiveReply(ref Message reply, object correlationState)
		{
		}

		public object BeforeSendRequest(ref Message request, IClientChannel channel)
		{
			if (SoapHeader != null || Client.Credentials != null && Client.Credentials.Password != null && 
				(Client.IsSecureProtocol || Client.IsLocal))
			{
				// Prepare the request message copy to be modified
				MessageBuffer buffer = request.CreateBufferedCopy(Int32.MaxValue);
				request = buffer.CreateMessage();
			}

			if (SoapHeader != null)
			{
				var header = MessageHeader.CreateHeader(SoapHeader.GetType().Name, $"{Namespace}{SoapHeader.GetType().Name}", SoapHeader);
				request.Headers.Add(header);
				Client.SoapHeader = null;
			}
			if (Client.Credentials != null && Client.Credentials.Password != null && Client.IsAuthenticated && (Client.IsSecureProtocol || Client.IsLocal))
			{
				var cred = new Credentials { Username = Client.Credentials.UserName, Password = Client.Credentials.Password };
				var header = MessageHeader.CreateHeader(nameof(Credentials), $"{Namespace}{nameof(Credentials)}", cred);
				request.Headers.Add(header);
				// Client.Credentials.UserName = Client.Credentials.Password = null;
			}
			return null;
		}
	}

	public class SoapHeaderClientBehavior : IEndpointBehavior
	{
		public ClientBase Client;

		public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
		{
		}

		public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
		{
			clientRuntime.ClientMessageInspectors.Add(new SoapHeaderClientMessageInspector() { Client = this.Client });
		}

		public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
		{
		}

		public void Validate(ServiceEndpoint endpoint)
		{
		}
	}
}
