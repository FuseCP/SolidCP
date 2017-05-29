using SolidCP.WebDav.Core.Entities.Account;
using SolidCP.WebDav.Core.Entities.Account.Enums;
using SolidCP.WebDav.Core.Helper;
using SolidCP.WebDav.Core.Interfaces.Managers.Users;
using SolidCP.WebDav.Core.Scp.Framework;

namespace SolidCP.WebDav.Core.Managers.Users
{
    public class UserSettingsManager : IUserSettingsManager
    {
        public UserPortalSettings GetUserSettings(int accountId)
        {
            string xml = SCP.Services.EnterpriseStorage.GetWebDavPortalUserSettingsByAccountId(accountId);

            if (string.IsNullOrEmpty(xml))
            {
                return new UserPortalSettings();
            }

            return SerializeHelper.Deserialize<UserPortalSettings>(xml);
        }

        public void UpdateSettings(int accountId, UserPortalSettings settings)
        {
            var xml = SerializeHelper.Serialize(settings);

            SCP.Services.EnterpriseStorage.UpdateWebDavPortalUserSettings(accountId, xml);
        }

        public void ChangeWebDavViewType(int accountId, FolderViewTypes type)
        {
            var settings = GetUserSettings(accountId);

            settings.WebDavViewType = type;

            UpdateSettings(accountId, settings);
        }
    }
}