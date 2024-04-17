using System;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
#if NETFRAMEWORK
using System.ServiceModel;
using System.ServiceModel.Web;
#else
using CoreWCF;
using CoreWCF.Web;
#endif

namespace SolidCP.Web.Services {
	public class RestAuthorizationManager : ServiceAuthorizationManager
	{
		bool HasApi(string adr, string api) => Regex.IsMatch(adr, $"/{api}/[a-zA-Z0-9_]+(?:\\?|$)");

		/// <summary>  
		/// Method source sample taken from here: http://bit.ly/1hUa1LR  
		/// </summary>  
#if NETFRAMEWORK
		protected override bool CheckAccessCore(OperationContext operationContext)
#else
		protected override async ValueTask<bool> CheckAccessCoreAsync(OperationContext operationContext)
#endif
		{
			string endpointUri = operationContext.EndpointDispatcher.EndpointAddress.Uri.AbsolutePath;
			if (WebOperationContext.Current == null || !HasApi(endpointUri, "api"))
			{
#if NETFRAMEWORK
				return base.CheckAccessCore(operationContext);
#else
				return await base.CheckAccessCoreAsync(operationContext);
#endif
			}

			var match = Regex.Match(endpointUri, "(?<=/api/)[a-zA-Z0-9_]+(?=\\?|$)", RegexOptions.Singleline);
			if (!match.Success) throw new NotSupportedException("Error parsing endpoint address.");
			var typeName = match.Value;
			var type = ServiceTypes.Types[typeName];
			var contract = type?.Contract;
			PolicyAttribute policy = contract?.GetCustomAttribute<PolicyAttribute>();
			var isEncrypted = policy != null;
			var isAuthenticated = isEncrypted && policy.Policy != PolicyAttribute.Encrypted;

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
#if NETFRAMEWORK
					validator.Validate(user.Name, user.Password);
#else
					validator.ValidateAsync(user.Name, user.Password).AsTask().Wait();
#endif
					return true;

				} catch (Exception ex)
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