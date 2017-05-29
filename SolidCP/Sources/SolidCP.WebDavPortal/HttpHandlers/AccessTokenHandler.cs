using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SolidCP.WebDav.Core.Interfaces.Managers;
using SolidCP.WebDav.Core.Interfaces.Security;
using SolidCP.WebDav.Core.Security.Cryptography;
using SolidCP.WebDav.Core.Scp.Framework;

namespace SolidCP.WebDavPortal.HttpHandlers
{
    public class AccessTokenHandler : DelegatingHandler
    {
        private const string Bearer = "Bearer ";

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Contains("Authorization"))
            {
                var tokenString = request.Headers.GetValues("Authorization").First();
                if (!string.IsNullOrEmpty(tokenString) && tokenString.StartsWith(Bearer))
                {
                    try
                    {
                        var accessToken = tokenString.Substring(Bearer.Length - 1);

                        var tokenManager = DependencyResolver.Current.GetService<IAccessTokenManager>();

                        var guid = Guid.Parse(accessToken);
                        tokenManager.ClearExpiredTokens();

                        var token = tokenManager.GetToken(guid);

                        if (token != null)
                        {
                            var authenticationService = DependencyResolver.Current.GetService<IAuthenticationService>();
                            var cryptography = DependencyResolver.Current.GetService<ICryptography>();


                            var user = SCP.Services.ExchangeServer.GetAccount(token.ItemId, token.AccountId);

                            authenticationService.LogIn(user.UserPrincipalName, cryptography.Decrypt(token.AuthData));
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return await
                base.SendAsync(request, cancellationToken);
        }
    }
}