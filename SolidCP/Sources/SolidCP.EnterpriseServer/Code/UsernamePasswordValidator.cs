using System.ServiceModel;
#if !NETFRAMEWORK

#endif

namespace SolidCP.EnterpriseServer
{
	public class UsernamePasswordValidator
	{

		public static bool Validate(string username, string password)
		{
			using (var controller = new Controllers())
			{
				UserInfo user = controller.UserController.GetUserByUsernamePassword(username, password, Web.Services.Server.UserHostAddress);

				if (user == null) return false;

				controller.SecurityContext.SetThreadPrincipal(user);
			}
			return true;
		}


		public static void Init() { SolidCP.Web.Services.UserNamePasswordValidator.ValidateEnterpriseServer = Validate; }
	}
}
