using SolidCP.EnterpriseServer;
using SolidCP.Providers.HostedSolution;
using System;
using System.Collections;

namespace SolidCP.EnterpriseServer.MailTemplates
{
	public class TemplateHashtable : Hashtable, IDisposable
	{
		ControllerBase Provider;
		UserController userController;
		protected UserController UserController => userController ??= new UserController(Provider);
		public TemplateHashtable(ControllerBase provider) { Provider = provider; }

		public string LogoUrl
		{
			get
			{
				string str;
				object item = this["logoUrl"];
				if (item != null)
				{
					str = item.ToString();
				}
				else
				{
					str = null;
				}
				return str;
			}
			set
			{
				this["logoUrl"] = value;
			}
		}

		public UserInfo User
		{
			get
			{
				return (UserInfo)this["user"];
			}
			set
			{
				this["user"] = value;
			}
		}

		public TemplateHashtable(UserInfo user, ControllerBase provider = null) : this(user, null, provider)
		{
		}

		public TemplateHashtable(UserInfo user, int orgId, ControllerBase provider = null) : this(user, new int?(orgId), provider)
		{
		}

		private TemplateHashtable(UserInfo user, int? orgId, ControllerBase provider = null): this(provider)
		{
			string organizationLogoUrl;
			string str;
			if (user != null)
			{
				this.User = user;
				UserSettings userSettings = UserController.GetUserSettings(user.UserId, "SolidCPPolicy");
				if (!string.IsNullOrEmpty(userSettings["LogoImageURL"]))
				{
					this.LogoUrl = userSettings["LogoImageURL"];
				}
			}
			if (orgId.HasValue)
			{
				OrganizationGeneralSettings organizationGeneralSettings;

				using (var organizationController = new OrganizationController())
				{
					organizationGeneralSettings = organizationController.GetOrganizationGeneralSettings(orgId.Value);
				}
				if (organizationGeneralSettings != null)
				{
					organizationLogoUrl = organizationGeneralSettings.OrganizationLogoUrl;
				}
				else
				{
					organizationLogoUrl = null;
				}
				if (!string.IsNullOrEmpty(organizationLogoUrl))
				{
					if (organizationGeneralSettings != null)
					{
						str = organizationGeneralSettings.OrganizationLogoUrl;
					}
					else
					{
						str = null;
					}
					this.LogoUrl = str;
				}
			}
		}

		bool isDisposed = false;
		public void Dispose()
		{
			if (!isDisposed)
			{
				isDisposed = true;
				userController?.Dispose();
			}
		}
	}
}
