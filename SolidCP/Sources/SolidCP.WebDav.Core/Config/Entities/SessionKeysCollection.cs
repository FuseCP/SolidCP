using System.Collections.Generic;
using System.Linq;
using SolidCP.EnterpriseServer.Base.HostedSolution;
using SolidCP.WebDav.Core.Config.WebConfigSections;
using SolidCP.WebDavPortal.WebConfigSections;

namespace SolidCP.WebDav.Core.Config.Entities
{
    public class SessionKeysCollection : AbstractConfigCollection
    {
        private readonly IEnumerable<SessionKeysElement> _sessionKeys;

        public SessionKeysCollection()
        {
            _sessionKeys = ConfigSection.SessionKeys.Cast<SessionKeysElement>();
        }

        public string AuthTicket
        {
            get
            {
                SessionKeysElement sessionKey =
                    _sessionKeys.FirstOrDefault(x => x.Key == SessionKeysElement.AuthTicketKey);
                return sessionKey != null ? sessionKey.Value : null;
            }
        }

        public string WebDavManager
        {
            get
            {
                SessionKeysElement sessionKey =
                    _sessionKeys.FirstOrDefault(x => x.Key == SessionKeysElement.WebDavManagerKey);
                return sessionKey != null ? sessionKey.Value : null;
            }
        }

        public string UserGroupsKey
        {
            get
            {
                SessionKeysElement sessionKey =
                    _sessionKeys.FirstOrDefault(x => x.Key == SessionKeysElement.UserGroupsKey);
                return sessionKey != null ? sessionKey.Value : null;
            }
        }

        public string OwaEditFoldersSessionKey
        {
            get
            {
                SessionKeysElement sessionKey =
                    _sessionKeys.FirstOrDefault(x => x.Key == SessionKeysElement.OwaEditFoldersSessionKey);
                return sessionKey != null ? sessionKey.Value : null;
            }
        }

        public string WebDavRootFoldersPermissions
        {
            get
            {
                SessionKeysElement sessionKey =
                    _sessionKeys.FirstOrDefault(x => x.Key == SessionKeysElement.WebDavRootFolderPermissionsKey);
                return sessionKey != null ? sessionKey.Value : null;
            }
        }

        public string PasswordResetSmsKey
        {
            get
            {
                SessionKeysElement sessionKey =
                    _sessionKeys.FirstOrDefault(x => x.Key == SessionKeysElement.PasswordResetSmsKey);
                return sessionKey != null ? sessionKey.Value : null;
            }
        }

        public string AccountIdKey
        {
            get
            {
                SessionKeysElement sessionKey =
                    _sessionKeys.FirstOrDefault(x => x.Key == SessionKeysElement.AccountIdKey);
                return sessionKey != null ? sessionKey.Value : null;
            }
        }

        public string ResourseRenderCount
        {
            get
            {
                SessionKeysElement sessionKey = _sessionKeys.FirstOrDefault(x => x.Key == SessionKeysElement.ResourseRenderCountKey);
                return sessionKey != null ? sessionKey.Value : null;
            }
        }

        public string ItemId
        {
            get
            {
                SessionKeysElement sessionKey = _sessionKeys.FirstOrDefault(x => x.Key == SessionKeysElement.ItemIdSessionKey);
                return sessionKey != null ? sessionKey.Value : null;
            }
        }
    }
}