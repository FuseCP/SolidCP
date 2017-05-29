using System;
using SolidCP.EnterpriseServer.Base.HostedSolution;
using SolidCP.WebDav.Core.Interfaces.Managers;
using SolidCP.WebDav.Core.Security.Authentication.Principals;
using SolidCP.WebDav.Core.Scp.Framework;

namespace SolidCP.WebDav.Core.Managers
{
    public class AccessTokenManager : IAccessTokenManager
    {
        public WebDavAccessToken CreateToken(ScpPrincipal principal, string filePath)
        {
            var token = new WebDavAccessToken();

            token.AccessToken = Guid.NewGuid();
            token.AccountId = principal.AccountId;
            token.ItemId = principal.ItemId;
            token.AuthData = principal.EncryptedPassword;
            token.ExpirationDate = DateTime.Now.AddHours(3);
            token.FilePath = filePath;

            token.Id = SCP.Services.EnterpriseStorage.AddWebDavAccessToken(token);

            return token;
        }

        public WebDavAccessToken GetToken(int id)
        {
            return SCP.Services.EnterpriseStorage.GetWebDavAccessTokenById(id);
        }

        public WebDavAccessToken GetToken(Guid guid)
        {
            return SCP.Services.EnterpriseStorage.GetWebDavAccessTokenByAccessToken(guid);
        }

        public void ClearExpiredTokens()
        {
            SCP.Services.EnterpriseStorage.DeleteExpiredWebDavAccessTokens();
        }
    }
}