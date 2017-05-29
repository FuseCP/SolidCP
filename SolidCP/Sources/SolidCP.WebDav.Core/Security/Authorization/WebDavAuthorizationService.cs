using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cobalt;
using SolidCP.EnterpriseServer.Base.HostedSolution;
using SolidCP.Providers.HostedSolution;
using SolidCP.WebDav.Core.Config;
using SolidCP.WebDav.Core.Extensions;
using SolidCP.WebDav.Core.Interfaces.Security;
using SolidCP.WebDav.Core.Security.Authentication.Principals;
using SolidCP.WebDav.Core.Security.Authorization.Enums;
using SolidCP.WebDav.Core.Scp.Framework;

namespace SolidCP.WebDav.Core.Security.Authorization
{
    public class WebDavAuthorizationService : IWebDavAuthorizationService
    {
        public bool HasAccess(ScpPrincipal principal, string path)
        {
            path = path.RemoveLeadingFromPath(principal.OrganizationId);

            var permissions = GetPermissions(principal, path);

            return permissions.HasFlag(WebDavPermissions.Read) || permissions.HasFlag(WebDavPermissions.Write);
        }

        public WebDavPermissions GetPermissions(ScpPrincipal principal, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return WebDavPermissions.Read;
            }

            var resultPermissions = WebDavPermissions.Empty;

            var rootFolder = GetRootFolder(path);

            var userGroups = GetUserSecurityGroups(principal);

            var permissions = GetFolderEsPermissions(principal, rootFolder);

            foreach (var permission in permissions)
            {
                if ((!permission.IsGroup
                        && (permission.DisplayName == principal.UserName || permission.DisplayName == principal.DisplayName))
                    || (permission.IsGroup && userGroups.Any(x => x.DisplayName == permission.DisplayName)))
                {
                    if (permission.Access.ToLowerInvariant().Contains("read"))
                    {
                        resultPermissions |= WebDavPermissions.Read;
                    }

                    if (permission.Access.ToLowerInvariant().Contains("write"))
                    {
                        resultPermissions |= WebDavPermissions.Write;
                    }
                }
            }

            var owaEditFolders = GetOwaFoldersWithEditPermission(principal);

            if (owaEditFolders.Contains(rootFolder))
            {
                resultPermissions |= WebDavPermissions.OwaEdit;
            }
            else
            {
                resultPermissions |= WebDavPermissions.OwaRead;
            }

            return resultPermissions;
        }

        private string GetRootFolder(string path)
        {
            return path.Split(new[]{'/'}, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        private IEnumerable<ESPermission> GetFolderEsPermissions(ScpPrincipal principal, string rootFolderName)
        {
            var dictionary = HttpContext.Current.Session != null ?HttpContext.Current.Session[WebDavAppConfigManager.Instance.SessionKeys.WebDavRootFoldersPermissions] as
                Dictionary<string, IEnumerable<ESPermission>> : null;

            if (dictionary == null)
            {
                dictionary = new Dictionary<string, IEnumerable<ESPermission>>();

                var rootFolders = SCP.Services.EnterpriseStorage.GetEnterpriseFoldersPaged(principal.ItemId, false,false, false,"","",0, int.MaxValue).PageItems;

                foreach (var rootFolder in rootFolders)
                {
                    var permissions = SCP.Services.EnterpriseStorage.GetEnterpriseFolderPermissions(principal.ItemId, rootFolder.Name);

                    dictionary.Add(rootFolder.Name, permissions);
                }

                if (HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session[WebDavAppConfigManager.Instance.SessionKeys.WebDavRootFoldersPermissions] = dictionary;
                }
            }

            return dictionary.ContainsKey(rootFolderName) ? dictionary[rootFolderName] : new ESPermission[0];
        }

        public IEnumerable<ExchangeAccount> GetUserSecurityGroups(ScpPrincipal principal)
        {
            var groups = HttpContext.Current.Session != null ? HttpContext.Current.Session[WebDavAppConfigManager.Instance.SessionKeys.UserGroupsKey] as IEnumerable<ExchangeAccount> : null;

            if (groups == null)
            {
                 groups = SCP.Services.Organizations.GetSecurityGroupsByMember(principal.ItemId, principal.AccountId);

                if (HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session[WebDavAppConfigManager.Instance.SessionKeys.UserGroupsKey] = groups;
                }
            }

            return groups ?? new ExchangeAccount[0];
        }

        private IEnumerable<string> GetOwaFoldersWithEditPermission(ScpPrincipal principal)
        {
            var folders = HttpContext.Current.Session != null ? HttpContext.Current.Session[WebDavAppConfigManager.Instance.SessionKeys.OwaEditFoldersSessionKey] as IEnumerable<string> : null;

            if (folders != null)
            {
                return folders;
            }

            var accountsIds = new List<int>();

            accountsIds.Add(principal.AccountId);

            var groups = GetUserSecurityGroups(principal);

            accountsIds.AddRange(groups.Select(x=>x.AccountId));

            try
            {
                folders = ScpContext.Services.EnterpriseStorage.GetUserEnterpriseFolderWithOwaEditPermission(principal.ItemId, accountsIds.ToArray());
            }
            catch (Exception)
            {
                //TODO remove try catch when es &portal will be updated
                return new List<string>();
            }


            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session[WebDavAppConfigManager.Instance.SessionKeys.OwaEditFoldersSessionKey] = folders;
            }

            return folders;
        }
    }
}