using System;
using SolidCP.EnterpriseServer.Base.HostedSolution;
using SolidCP.WebDav.Core.Security.Authentication.Principals;

namespace SolidCP.WebDav.Core.Interfaces.Managers
{
    public interface IAccessTokenManager
    {
        WebDavAccessToken CreateToken(ScpPrincipal principal, string filePath);
        WebDavAccessToken GetToken(int id);
        WebDavAccessToken GetToken(Guid guid);
        void ClearExpiredTokens();
    }
}