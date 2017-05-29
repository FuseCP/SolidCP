using System.Collections.Generic;
using SolidCP.Providers.HostedSolution;
using SolidCP.WebDav.Core.Security.Authentication.Principals;
using SolidCP.WebDav.Core.Security.Authorization.Enums;

namespace SolidCP.WebDav.Core.Interfaces.Security
{
    public interface IWebDavAuthorizationService
    {
        bool HasAccess(ScpPrincipal principal, string path);
        WebDavPermissions GetPermissions(ScpPrincipal principal, string path);
        IEnumerable<ExchangeAccount> GetUserSecurityGroups(ScpPrincipal principal);
    }
}