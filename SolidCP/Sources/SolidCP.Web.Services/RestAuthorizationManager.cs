#if NETFRAMEWORK
using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Net;
using System.Reflection;

namespace SolidCP.Web.Services {
	public class RestAuthorizationManager : ServiceAuthorizationManager
	{
		/// <summary>  
		/// Method source sample taken from here: http://bit.ly/1hUa1LR  
		/// </summary>  
		protected override bool CheckAccessCore(OperationContext operationContext)
		{
			string bindingName = operationContext.EndpointDispatcher.ChannelDispatcher.BindingName;
			if (WebOperationContext.Current == null || string.Compare(bindingName, "webHttpBinding", true) != 0) return true;

			var instance = operationContext.InstanceContext.GetServiceInstance();
			PolicyAttribute policy = null;
			var insttype = instance.GetType();
			while (insttype != null && policy == null)
			{
				policy = insttype.GetCustomAttribute<PolicyAttribute>();
				if (policy == null)
				{
					insttype = insttype.BaseType;
				}
			}
			var isEncrypted = policy != null;
			var isAuthenticated = isEncrypted && policy.Policy != "CommonPolicy";

			if (!isAuthenticated) return true;

	        //Extract the Authorization header, and parse out the credentials converting the Base64 string:  
			var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
			if ((authHeader != null) && (authHeader != string.Empty))
			{
				var svcCredentials = System.Text.ASCIIEncoding.ASCII
					.GetString(Convert.FromBase64String(authHeader.Substring(6)))
					.Split(':');
				var user = new
				{
					Name = svcCredentials[0],
					Password = svcCredentials[1]
				};
				var validator = new UserNamePasswordValidator();
				validator.Policy = policy;
				try
				{
					validator.Validate(user.Name, user.Password);
					return true;

				} catch (FaultException)
				{
					return false;
				}
			}
			else
			{
				//No authorization header was provided, so challenge the client to provide before proceeding:  
				WebOperationContext.Current.OutgoingResponse.Headers.Add("WWW-Authenticate: Basic realm=\"SolidCP\"");
				//Throw an exception with the associated HTTP status code equivalent to HTTP status 401  
				throw new WebFaultException(HttpStatusCode.Unauthorized);
			}
		}
	}
}
#endif