using System.ServiceModel;
using SolidCP.Web.Services;

namespace SolidCP.Server
{
	public class PasswordValidator
	{

		public static bool Validate(string password) => password == Settings.Password;

		public static void Init()
		{
			SolidCP.Web.Services.UserNamePasswordValidator.ValidateServer = Validate;
		}

	}
}
