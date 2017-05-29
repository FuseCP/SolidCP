using Ninject;
using System.Web.SessionState;
using SolidCP.WebDav.Core.Interfaces.Managers;
using SolidCP.WebDav.Core.Interfaces.Managers.Users;
using SolidCP.WebDav.Core.Interfaces.Owa;
using SolidCP.WebDav.Core.Interfaces.Security;
using SolidCP.WebDav.Core.Interfaces.Services;
using SolidCP.WebDav.Core.Interfaces.Storages;
using SolidCP.WebDav.Core.Managers;
using SolidCP.WebDav.Core.Managers.Users;
using SolidCP.WebDav.Core.Owa;
using SolidCP.WebDav.Core.Security.Authentication;
using SolidCP.WebDav.Core.Security.Authorization;
using SolidCP.WebDav.Core.Security.Cryptography;
using SolidCP.WebDav.Core.Services;
using SolidCP.WebDav.Core.Storages;
using SolidCP.WebDavPortal.DependencyInjection.Providers;

namespace SolidCP.WebDavPortal.DependencyInjection
{
    public class PortalDependencies
    {
        public static void Configure(IKernel kernel)
        {
            kernel.Bind<HttpSessionState>().ToProvider<HttpSessionStateProvider>();
            kernel.Bind<ICryptography>().To<CryptoUtils>();
            kernel.Bind<IAuthenticationService>().To<FormsAuthenticationService>();
            kernel.Bind<IWebDavManager>().To<WebDavManager>();
            kernel.Bind<IAccessTokenManager>().To<AccessTokenManager>();
            kernel.Bind<IWopiServer>().To<WopiServer>();
            kernel.Bind<IWopiFileManager>().To<CobaltSessionManager>();
            kernel.Bind<IWebDavAuthorizationService>().To<WebDavAuthorizationService>();
            kernel.Bind<ICobaltManager>().To<CobaltManager>();
            kernel.Bind<ITtlStorage>().To<CacheTtlStorage>();
            kernel.Bind<IUserSettingsManager>().To<UserSettingsManager>();
            kernel.Bind<ISmsDistributionService>().To<TwillioSmsDistributionService>();
            kernel.Bind<ISmsAuthenticationService>().To<SmsAuthenticationService>();
        }
    }
}