using Microsoft.Web.Services3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Web.Services
{
#if NETFRAMEWORK
	public class UserNamePasswordValidator: System.IdentityModel.Selectors.UserNamePasswordValidator
#else
	public class UserNamePasswordValidator : CoreWCF.IdentityModel.Selectors.UserNamePasswordValidator
#endif
	{
#if NETFRAMEWORK
		public override void Validate(string userName, string password)
#else
		public override ValueTask ValidateAsync(string userName, string password)

#endif
		{
			var service = OperationContext.Current.InstanceContext.GetServiceInstance();
			var contract = service.GetType().GetInterfaces()
				.FirstOrDefault(t => t.GetCustomAttribute<ServiceContractAttribute>() != null);

			if (contract != null)
			{
				var policy = contract.GetCustomAttribute<PolicyAttribute>();
				if (policy != null)
				{
					if (policy.Policy == "ServerPolicy" && ValidateServer != null)
					{
						if (!ValidateServer(password)) throw new FaultException("Invalid server password");
					}
					else if (policy.Policy == "EnterpriseServerPolicy" && ValidateEnterpriseServer != null)
						if (!ValidateEnterpriseServer(userName, password)) throw new FaultException("Invalid user");
				}
			}

#if !NETFRAMEWORK
			return ValueTask.CompletedTask;
 #endif
		}

		public static Func<string, bool> ValidateServer;
		public static Func<string, string, bool> ValidateEnterpriseServer;

	}
}
