using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http.Routing;
using SolidCP.EnterpriseServer.Base.HostedSolution;
using SolidCP.WebDav.Core;
using SolidCP.WebDav.Core.Config;
using SolidCP.WebDavPortal.UI.Routes;

namespace SolidCP.WebDavPortal.Extensions
{
    public static class UrlHelperExtensions
    {
        public static String GenerateWopiUrl(this System.Web.Mvc.UrlHelper urlHelper, WebDavAccessToken token, string path)
        {
            var urlPart = urlHelper.HttpRouteUrl(OwaRouteNames.CheckFileInfo, new { accessTokenId = token.Id });

            return GenerateWopiUrl(token, urlPart, path);
        }

        public static String GenerateWopiUrl(this UrlHelper urlHelper, WebDavAccessToken token, string path)
        {
            var urlPart = urlHelper.Route(OwaRouteNames.CheckFileInfo, new { accessTokenId = token.Id });

            return GenerateWopiUrl(token, urlPart, path);
        }

        private static string GenerateWopiUrl(WebDavAccessToken token, string  urlPart, string path)
        {
            var url = new Uri(HttpContext.Current.Request.Url, urlPart).ToString();

            string wopiSrc = HttpUtility.UrlDecode(url);

            return string.Format("{0}&access_token={1}", wopiSrc, token.AccessToken.ToString("N"));
        }
    }
}