using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace SolidCP.EnterpriseServer
{
	public class UserCacheEntry
	{
		public DateTime LastAccess;
		public string Password;
		public UserInfo User;
	}

	public class UserCache : ConcurrentDictionary<string, UserCacheEntry>
	{
		const int MaxEntries = 20;

		public new bool TryGetValue(string key, out UserCacheEntry entry)
		{
			entry = null;
			if (base.TryGetValue(key, out entry))
			{
				entry.LastAccess = DateTime.Now;
				return true;
			}
			return false;
		}

		public new void AddOrUpdate(string key, UserInfo user, string password)
		{
			var entry = new UserCacheEntry
			{
				LastAccess = DateTime.Now,
				User = user,
				Password = password
			};
			base.AddOrUpdate(key, entry, (k, v) => entry);
			if (Count > MaxEntries)
			{
				Task.Run(() =>
				{
					var oldEntries = Values
						.OrderByDescending(e => e.LastAccess)
						.Skip(MaxEntries);
					foreach (var oldEntry in oldEntries) base.TryRemove(oldEntry.User.Username, out _);
				});
			}
		}
	}

	public class UsernamePasswordValidator
	{
		static UserCache Users = new UserCache();
		static ConcurrentDictionary<string, Task> GetUserTasks = new ConcurrentDictionary<string, Task>();

		public static bool Validate(string username, string password)
		{
			using (var controller = new Controllers())
			{
				UserCacheEntry cachedUser;
				var hostAddress = Web.Services.Server.UserHostAddress;

				if (Users.TryGetValue(username, out cachedUser) && password == cachedUser.Password)
				{
					GetUserTasks.GetOrAdd(username, (username) => Task.Run(async () =>
					{
						await Task.Delay(3000);

						UserInfo user = controller.UserController.GetUserByUsernamePassword(username, password, hostAddress, false);
						if (user == null) Users.TryRemove(username, out cachedUser);
						else Users.AddOrUpdate(username, user, password);
						Task task;
						GetUserTasks.TryRemove(username, out task);
					}));

					controller.SecurityContext.SetThreadPrincipal(cachedUser.User);
					return true;
				}

				UserInfo user = controller.UserController.GetUserByUsernamePassword(username, password, hostAddress, false);

				if (user == null)
				{
					Users.TryRemove(username, out cachedUser);
					return false;
				}

				Users.AddOrUpdate(username, user, password);
				controller.SecurityContext.SetThreadPrincipal(user);
			}
			return true;
		}

		public static void Init() { SolidCP.Web.Services.UserNamePasswordValidator.ValidateEnterpriseServer = Validate; }
	}
}
