#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace SolidCP.Web.Services
{
	public class ServiceHostFactory: System.ServiceModel.Activation.ServiceHostFactory
	{
		protected override System.ServiceModel.ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
		{
			return new ServiceHost(serviceType, baseAddresses);
		}

		public override ServiceHostBase CreateServiceHost(string constructorString, Uri[] baseAddresses)
		{
			return CreateServiceHost(Type.GetType(constructorString), baseAddresses);
		}
	}
}
#endif