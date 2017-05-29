using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using SolidCP.WebDav.Core;
using SolidCP.WebDav.Core.Client;
using SolidCP.WebDav.Core.Config;
using SolidCP.WebDavPortal.Extensions;
using SolidCP.WebDavPortal.UI.Routes;

namespace SolidCP.WebDavPortal.FileOperations
{
    public class FileOpenerManager
    {
        private readonly IDictionary<string, FileOpenerType> _operationTypes = new Dictionary<string, FileOpenerType>();

        private readonly Lazy<IDictionary<string, FileOpenerType>> _officeOperationTypes = new Lazy<IDictionary<string, FileOpenerType>>(
            () =>
            {
                if (WebDavAppConfigManager.Instance.OfficeOnline.IsEnabled)
                {
                    return 
                        WebDavAppConfigManager.Instance.OfficeOnline.ToDictionary(x => x.Extension,
                            y => FileOpenerType.OfficeOnline);
                }

                return new Dictionary<string, FileOpenerType>();
            });

        public FileOpenerManager()
        {
            _operationTypes.AddRange(
                    WebDavAppConfigManager.Instance.FileOpener.ToDictionary(x => x.Extension,
                        y => FileOpenerType.Open));
        }

        public string GetUrl(IHierarchyItem item, UrlHelper urlHelper)
        {
            var opener = this[Path.GetExtension(item.DisplayName)];
            string href = "/";

            switch (opener)
            {
                case FileOpenerType.OfficeOnline:
                {
                    var pathPart = item.Href.AbsolutePath.Replace("/" + ScpContext.User.OrganizationId, "").TrimStart('/');
                    href = string.Concat(urlHelper.RouteUrl(FileSystemRouteNames.EditOfficeOnline, new { org = ScpContext.User.OrganizationId, pathPart = "" }), pathPart);
                    break;
                }
                default:
                {
                    href = item.Href.LocalPath;
                    break;
                }
            }

            return href;
        }

        public bool GetIsTargetBlank(IHierarchyItem item)
        {
            var opener = this[Path.GetExtension(item.DisplayName)];
            var result = false;

            switch (opener)
            {
                case FileOpenerType.OfficeOnline:
                {
                    result = true;
                    break;
                }
                    case FileOpenerType.Open:
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public string GetMimeType(string extension)
        {
            var opener = WebDavAppConfigManager.Instance.FileOpener.FirstOrDefault(x => x.Extension.ToLowerInvariant() == extension.ToLowerInvariant());

            if (opener == null)
            {
                return MediaTypeNames.Application.Octet;
            }

            return opener.MimeType;
        }

        public FileOpenerType this[string fileExtension]
        {
            get
            {
                FileOpenerType result;
                if (_officeOperationTypes.Value.TryGetValue(fileExtension, out result) && CheckBrowserSupport())
                {
                    return result;
                }

                if (_operationTypes.TryGetValue(fileExtension, out result))
                {
                    return result;
                }

                return FileOpenerType.Download;
            }
        }

        private bool CheckBrowserSupport()
        {
            var request = HttpContext.Current.Request;
            int supportedVersion;

            string key = string.Empty;

            foreach (var supportedKey in WebDavAppConfigManager.Instance.OwaSupportedBrowsers.Keys)
            {
                if (supportedKey.Split(';').Contains(request.Browser.Browser))
                {
                    key = supportedKey;
                    break;
                }
            }

            if (WebDavAppConfigManager.Instance.OwaSupportedBrowsers.TryGetValue(key, out supportedVersion) == false)
            {
                return false;
            }

            return supportedVersion <= request.Browser.MajorVersion;
        }
    }
}