using SolidCP.EnterpriseServer;
using SolidCP.Providers.HostedSolution;
using System;
using System.Collections;

namespace SolidCP.EnterpriseServer.MailTemplates
{
    public class TemplateHashtable : Hashtable
    {
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

        public TemplateHashtable(UserInfo user) : this(user, null)
        {
        }

        public TemplateHashtable(UserInfo user, int orgId) : this(user, new int?(orgId))
        {
        }

        private TemplateHashtable(UserInfo user, int? orgId)
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
                OrganizationGeneralSettings organizationGeneralSettings = OrganizationController.GetOrganizationGeneralSettings(orgId.Value);
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
    }
}
