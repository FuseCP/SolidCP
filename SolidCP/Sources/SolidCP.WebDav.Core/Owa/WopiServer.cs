using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Cobalt;
using SolidCP.EnterpriseServer.Base.HostedSolution;
using SolidCP.WebDav.Core.Client;
using SolidCP.WebDav.Core.Config;
using SolidCP.WebDav.Core.Entities.Owa;
using SolidCP.WebDav.Core.Interfaces.Managers;
using SolidCP.WebDav.Core.Interfaces.Owa;
using SolidCP.WebDav.Core.Interfaces.Security;
using SolidCP.WebDav.Core.Security.Authentication.Principals;
using SolidCP.WebDav.Core.Security.Authorization.Enums;

namespace SolidCP.WebDav.Core.Owa
{
    public class WopiServer : IWopiServer
    {
        private readonly IWebDavManager _webDavManager;
        private readonly IAccessTokenManager _tokenManager;
        private readonly IWebDavAuthorizationService _webDavAuthorizationService;
        private readonly IWopiFileManager _fileManager;


        public WopiServer(IWebDavManager webDavManager, IAccessTokenManager tokenManager, IWebDavAuthorizationService webDavAuthorizationService, IWopiFileManager fileManager)
        {
            _webDavManager = webDavManager;
            _tokenManager = tokenManager;
            _webDavAuthorizationService = webDavAuthorizationService;
            _fileManager = fileManager;
        }

        public CheckFileInfo GetCheckFileInfo(WebDavAccessToken token)
        {
            var resource = _webDavManager.GetResource(token.FilePath);

            var permissions = _webDavAuthorizationService.GetPermissions(ScpContext.User, token.FilePath);

            var readOnly = permissions.HasFlag(WebDavPermissions.Write) == false || permissions.HasFlag(WebDavPermissions.OwaEdit) == false;

            var cFileInfo = new CheckFileInfo
            {
                BaseFileName = resource == null ? token.FilePath.Split('/').Last() : resource.DisplayName.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault(),
                OwnerId = ScpContext.User.Login,
                Size = resource == null ? 0 : resource.ContentLength,
                Version = DateTime.Now.ToString("s"),
                SupportsCoauth = true,
                SupportsCobalt = true,
                SupportsFolders = true,
                SupportsLocks = true,
                SupportsScenarioLinks = false,
                SupportsSecureStore = false,
                SupportsUpdate = true,
                UserCanWrite = !readOnly,
                ReadOnly = readOnly,
                RestrictedWebViewOnly = false,
                CloseButtonClosesWindow = true
            };

            if (resource != null)
            {
                cFileInfo.ClientUrl = _webDavManager.GetFileUrl(token.FilePath);
            }

            return cFileInfo;
        }

        public byte[] GetFileBytes(int accessTokenId)
        {
            var token = _tokenManager.GetToken(accessTokenId);

            if (_webDavManager.FileExist(token.FilePath))
            {
                return _webDavManager.GetFileBytes(token.FilePath);
            }

            var cobaltFile = _fileManager.Get(token.FilePath) ?? _fileManager.Create(accessTokenId);

            var stream = new MemoryStream();

            new GenericFda(cobaltFile.CobaltEndpoint, null).GetContentStream().CopyTo(stream);

            return stream.ToArray();
        }
    }
}