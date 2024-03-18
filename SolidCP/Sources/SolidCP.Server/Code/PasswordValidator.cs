using System.ServiceModel;
using SolidCP.Web.Services;

namespace SolidCP.Server
{
	public class PasswordValidator
	{

		public static bool Validate(string password)
		{
#if NETFRAMEWORK
			return password == ServerConfiguration.Security.Password;
#else
			return password == StartupCore.Password;
#endif

		}

		public static void Init()
		{
			SolidCP.Web.Services.UserNamePasswordValidator.ValidateServer = Validate;
		}

	}
}
