using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer
{
	public class UsernamePasswordValidator
	{
		static ConcurrentDictionary<string, string> Users = new ConcurrentDictionary<string, string>();
		static ConcurrentDictionary<string, Task> GetUserTasks = new ConcurrentDictionary<string, Task>();
		public static bool Validate(string username, string password)
		{
			using (var controller = new Controllers())
			{
				string cachePassword = null;
				if (Users.TryGetValue(username, out cachePassword) && password == cachePassword)
				{
					GetUserTasks.GetOrAdd(username, (username) => Task.Run(async () =>
					{
						await Task.Delay(3000);

						UserInfo user = controller.UserController.GetUserByUsernamePassword(username, password, Web.Services.Server.UserHostAddress);
						if (user == null) Users.TryRemove(username, out cachePassword);
						Task task;
						GetUserTasks.TryRemove(username, out task);
					}));

					return true;
				}

				UserInfo user = controller.UserController.GetUserByUsernamePassword(username, password, Web.Services.Server.UserHostAddress);

				if (user == null)
				{
					Users.TryRemove(username, out cachePassword);
					return false;
				}

				Users.AddOrUpdate(username, password, (user, pwd) => password);

				controller.SecurityContext.SetThreadPrincipal(user);
			}
			return true;
		}


		public static void Init() { SolidCP.Web.Services.UserNamePasswordValidator.ValidateEnterpriseServer = Validate; }
	}
}
