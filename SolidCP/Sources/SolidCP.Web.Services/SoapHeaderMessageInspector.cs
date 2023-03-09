using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using System.Collections.ObjectModel;

namespace SolidCP.Web.Services
{
	public class SoapHeaderMessageInspector : IDispatchMessageInspector, IEndpointBehavior
	{
		public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
		{
			throw new NotImplementedException();
		}

		public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
		{
			var instance = instanceContext.GetServiceInstance();
			var action = request.Headers.Action;
			var operationName = action.Substring(action.LastIndexOf("/") + 1);
			var contract = instance.GetType().GetInterfaces()
				.Select(intf => intf.GetCustomAttribute<ServiceContractAttribute>())
				.FirstOrDefault(intf => intf != null);
			var method = contract.GetType().GetMethod(operationName);
			var soapattr = method.GetCustomAttribute<SoapHeaderAttribute>();
			if (soapattr != null)
			{
				var instanceType = instance.GetType();
				var p = instanceType.GetProperty(soapattr.Field);
				if (p != null)
				{
					int hpos = request.Headers.FindHeader(p.PropertyType.Name, $"http://temuri.org/{p.PropertyType.Name}");
					var getHeaderMethod = request.Headers.GetType().GetMethod("GetHeader", new Type[] { typeof(string), typeof(string) });
					getHeaderMethod.MakeGenericMethod(p.PropertyType);
					var header = getHeaderMethod.Invoke(request.Headers, new object[] { p.PropertyType.Name, $"http://temuri.org/{p.PropertyType.Name}" });
					p.SetValue(instance, header);
				}
			}
			return null;
		}

		public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
		{
		}

		public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
		{
			var channelDispatcher = endpointDispatcher.ChannelDispatcher;
			if (channelDispatcher == null) return;
			foreach (var ed in channelDispatcher.Endpoints)
			{
				var inspector = new SoapHeaderMessageInspector();
				ed.DispatchRuntime.MessageInspectors.Add(inspector);
			}
		}

		public void BeforeSendReply(ref Message reply, object correlationState)
		{
		}

		public void Validate(ServiceEndpoint endpoint)
		{
		}
	}
}
