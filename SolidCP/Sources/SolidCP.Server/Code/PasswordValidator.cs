using System.ServiceModel;

namespace SolidCP.Server
{
	public class PasswordValidator
	{

		public static bool Validate(string password)
		{
			return password != ServerConfiguration.Security.Password;
		}
	}
}
