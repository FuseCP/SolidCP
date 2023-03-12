using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using SolidCP.Providers;
#if !NETFRAMEWORK
using CoreWCF;
using CoreWCF.Description;
using CoreWCF.Channels;
using CoreWCF.Dispatcher;
#else
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
#endif


namespace SolidCP.Web.Services
{
	public class SoapHeaderMessageInspector : IDispatchMessageInspector, IEndpointBehavior
	{

		public const string Namespace = "http://solidcp.com/headers/";

		public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
		{
		}

		public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
		{
			var instance = instanceContext.GetServiceInstance();
			var instanceType = instance.GetType();
			var action = request.Headers.Action;
			var operationName = action.Substring(action.LastIndexOf("/") + 1);
			var contract = instanceType.GetInterfaces()
				.FirstOrDefault(intf => intf.GetCustomAttribute<ServiceContractAttribute>() != null);
			var method = contract.GetMethod(operationName);
			var soapattr = method.GetCustomAttribute<SoapHeaderAttribute>();
			if (soapattr != null)
			{
				var p = instanceType.GetProperty(soapattr.Field, BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				if (p != null)
				{
					int hpos = request.Headers.FindHeader(p.PropertyType.Name, $"{Namespace}{p.PropertyType.Name}");
					var getHeaderMethod = request.Headers.GetType().GetMethod("GetHeader", new Type[] { typeof(string), typeof(string) });
					getHeaderMethod = getHeaderMethod.MakeGenericMethod(p.PropertyType);
					var header = getHeaderMethod.Invoke(request.Headers, new object[] { p.PropertyType.Name, $"{Namespace}{p.PropertyType.Name}" });
					p.SetValue(instance, header);
				} else {
					var f = instanceType.GetField(soapattr.Field, BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
					if (f != null)
					{
						int hpos = request.Headers.FindHeader(f.FieldType.Name, $"{Namespace}{f.FieldType.Name}");
						var getHeaderMethod = request.Headers.GetType().GetMethod("GetHeader", new Type[] { typeof(string), typeof(string) });
						getHeaderMethod = getHeaderMethod.MakeGenericMethod(f.FieldType);
						var header = getHeaderMethod.Invoke(request.Headers, new object[] { f.FieldType.Name, $"{Namespace}{f.FieldType.Name}" });
						f.SetValue(instance, header);
					}
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
			var inspector = new SoapHeaderMessageInspector();
			foreach (var ed in channelDispatcher.Endpoints)
			{
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
