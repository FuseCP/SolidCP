using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;

namespace SolidCP.Web.Client
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
			if (SoapHeader != null)
			{
				// Prepare the request message copy to be modified
				MessageBuffer buffer = request.CreateBufferedCopy(Int32.MaxValue);
				request = buffer.CreateMessage();

				var header = MessageHeader.CreateHeader(SoapHeader.GetType().Name, $"{Namespace}{SoapHeader.GetType().Name}", SoapHeader);
				request.Headers.Add(header);
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
