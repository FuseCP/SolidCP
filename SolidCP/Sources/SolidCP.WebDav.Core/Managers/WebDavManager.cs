using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Serialization;
using SolidCP.Providers.OS;
using SolidCP.WebDav.Core.Client;
using SolidCP.WebDav.Core.Config;
using SolidCP.WebDav.Core.Exceptions;
using SolidCP.WebDav.Core.Extensions;
using SolidCP.WebDav.Core.Interfaces.Managers;
using SolidCP.WebDav.Core.Interfaces.Security;
using SolidCP.WebDav.Core.Resources;
using SolidCP.WebDav.Core.Security.Cryptography;
using SolidCP.WebDav.Core.Scp.Framework;

namespace SolidCP.WebDav.Core.Managers
{
    public class WebDavManager : IWebDavManager
    {
        private readonly ICryptography _cryptography;
        private readonly WebDavSession _webDavSession;
        private readonly IWebDavAuthorizationService _webDavAuthorizationService;

        // private bool _isRoot = true; --- compiler indicates never used
        private IFolder _currentFolder;

        public WebDavManager(ICryptography cryptography, IWebDavAuthorizationService webDavAuthorizationService)
        {
            _cryptography = cryptography;
            _webDavAuthorizationService = webDavAuthorizationService;        

            _webDavSession = new WebDavSession();
        }

        public IEnumerable<IHierarchyItem> OpenFolder(string pathPart)
        {
            IHierarchyItem[] children;

            if (string.IsNullOrWhiteSpace(pathPart))
            {
                children = ConnectToWebDavServer().Select(x => new WebDavResource
                {
                    Href = new Uri(x.Url), 
                    ItemType = ItemType.Folder,
                    ContentLength = x.Size * 1024 * 1024,
                    AllocatedSpace = (long)x.FRSMQuotaMB * 1024 * 1024, 
                    IsRootItem = true
                }).ToArray();
            }
            else
            {
                if (_currentFolder == null || _currentFolder.Path.ToString() != pathPart)
                {
                    _webDavSession.Credentials = new NetworkCredential(ScpContext.User.Login,
                        _cryptography.Decrypt(ScpContext.User.EncryptedPassword),
                        WebDavAppConfigManager.Instance.UserDomain);

                    _currentFolder = _webDavSession.OpenFolder(string.Format("{0}{1}/{2}", WebDavAppConfigManager.Instance.WebdavRoot, ScpContext.User.OrganizationId, pathPart.TrimStart('/')));
                }

                children = FilterResult(_currentFolder.GetChildren()).ToArray();
            }

            List<IHierarchyItem> sortedChildren = children.Where(x => x.ItemType == ItemType.Folder).OrderBy(x => x.DisplayName).ToList();
            sortedChildren.AddRange(children.Where(x => x.ItemType != ItemType.Folder).OrderBy(x => x.DisplayName));

            return sortedChildren;
        }

        public IEnumerable<IHierarchyItem> SearchFiles(int itemId, string pathPart, string searchValue, string uesrPrincipalName, bool recursive)
        {
            pathPart = (pathPart ?? string.Empty).Replace("/","\\");

            SystemFile[] items;


            if (string.IsNullOrWhiteSpace(pathPart))
            {
                var rootItems = ConnectToWebDavServer().Select(x => x.Name).ToList();
                rootItems.Insert(0, string.Empty);

                items = ScpContext.Services.EnterpriseStorage.SearchFiles(itemId, rootItems.ToArray(), searchValue, uesrPrincipalName, recursive);
            }
            else
            {
                items = ScpContext.Services.EnterpriseStorage.SearchFiles(itemId, new []{pathPart}, searchValue, uesrPrincipalName, recursive);
            }

            var resources = Convert(items, new Uri(WebDavAppConfigManager.Instance.WebdavRoot));


            return FilterResult(resources);
        }

        public bool IsFile(string path)
        {
            string folder = GetFileFolderPath(path);

            if (string.IsNullOrWhiteSpace(folder))
            {
                return false;
            }

            var resourceName = GetResourceName(path);

            OpenFolder(folder);

            IResource resource = _currentFolder.GetResource(resourceName);

            return resource.ItemType != ItemType.Folder;
        }


        public byte[] GetFileBytes(string path)
        {
            try
            {
                string folder = GetFileFolderPath(path);

                var resourceName = GetResourceName(path);

                OpenFolder(folder);

                IResource resource = _currentFolder.GetResource(resourceName);

                Stream stream = resource.GetReadStream();
                byte[] fileBytes = ReadFully(stream);

                return fileBytes;
            }
            catch (InvalidOperationException exception)
            {
                throw new ResourceNotFoundException("Resource not found", exception);
            }
        }

        public void UploadFile(string path, HttpPostedFileBase file)
        {
            var resource = new WebDavResource();

            var fileUrl = new Uri(WebDavAppConfigManager.Instance.WebdavRoot)
                .Append(ScpContext.User.OrganizationId)
                .Append(path)
                .Append(Path.GetFileName(file.FileName));

            resource.SetHref(fileUrl);
            resource.SetCredentials(new NetworkCredential(ScpContext.User.Login,  _cryptography.Decrypt(ScpContext.User.EncryptedPassword)));

            file.InputStream.Seek(0, SeekOrigin.Begin);
            var bytes = ReadFully(file.InputStream);

            resource.Upload(bytes);
        }

        public void UploadFile(string path, byte[] bytes)
        {
            var resource = new WebDavResource();

            var fileUrl = new Uri(WebDavAppConfigManager.Instance.WebdavRoot)
                .Append(ScpContext.User.OrganizationId)
                .Append(path);

            resource.SetHref(fileUrl);
            resource.SetCredentials(new NetworkCredential(ScpContext.User.Login, _cryptography.Decrypt(ScpContext.User.EncryptedPassword)));

            resource.Upload(bytes);
        }

        public void UploadFile(string path, Stream stream)
        {
            var resource = new WebDavResource();

            var fileUrl = new Uri(WebDavAppConfigManager.Instance.WebdavRoot)
                .Append(ScpContext.User.OrganizationId)
                .Append(path);

            resource.SetHref(fileUrl);
            resource.SetCredentials(new NetworkCredential(ScpContext.User.Login, _cryptography.Decrypt(ScpContext.User.EncryptedPassword)));

            var bytes = ReadFully(stream);

            resource.Upload(bytes);
        }

        public void LockFile(string path)
        {
            var resource = new WebDavResource();

            var fileUrl = new Uri(WebDavAppConfigManager.Instance.WebdavRoot)
                .Append(ScpContext.User.OrganizationId)
                .Append(path);

            resource.SetHref(fileUrl);
            resource.SetCredentials(new NetworkCredential(ScpContext.User.Login, _cryptography.Decrypt(ScpContext.User.EncryptedPassword)));

            resource.Lock();
        }

        public void DeleteResource(string path)
        {
            path = RemoveLeadingFromPath(path, "office365");
            path = RemoveLeadingFromPath(path, "view");
            path = RemoveLeadingFromPath(path, "edit");
            path = RemoveLeadingFromPath(path, ScpContext.User.OrganizationId);

            string folderPath = GetFileFolderPath(path);

            OpenFolder(folderPath);

            var resourceName = GetResourceName(path);

            IResource resource = _currentFolder.GetResource(resourceName);

            if (resource.ItemType == ItemType.Folder && GetFoldersItemsCount(path) > 0)
            {
                throw new WebDavException(string.Format(WebDavResources.FolderIsNotEmptyFormat, resource.DisplayName));
            }

            resource.Delete();
        }

        public IResource GetResource(string path)
        {
            try
            {
                string folder = GetFileFolderPath(path);

                var resourceName = GetResourceName(path);

                OpenFolder(folder);

                return _currentFolder.GetResource(resourceName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool FileExist(string path)
        {
            try
            {
                string folder = GetFileFolderPath(path);

                var resourceName = GetResourceName(path);

                OpenFolder(folder);

                var resource = _currentFolder.GetResource(resourceName);

                return resource != null;
            }
#pragma warning disable 0168
            catch (InvalidOperationException exception)
#pragma warning restore 0168
            {
                return false;
            }
        }

        public string GetFileUrl(string path)
        {
            try
            {
                string folder = GetFileFolderPath(path);

                var resourceName = GetResourceName(path);

                OpenFolder(folder);

                IResource resource =  _currentFolder.GetResource(resourceName);
                return resource.Href.ToString();
            }
            catch (InvalidOperationException exception)
            {
                throw new ResourceNotFoundException("Resource not found", exception);
            }
        }

        private IList<SystemFile> ConnectToWebDavServer()
        {
            var rootFolders = new List<SystemFile>();
            var user = ScpContext.User;

            var folders = SCP.Services.EnterpriseStorage.GetEnterpriseFoldersPaged(user.ItemId, true, false, false, "", "", 0, int.MaxValue).PageItems;

            foreach (var folder in folders)
            {
                if (_webDavAuthorizationService.HasAccess(user, Uri.UnescapeDataString(new Uri(folder.Url).PathAndQuery)))
                {
                    rootFolders.Add(folder);
                }
            }

            return rootFolders;
        }

        private int GetFoldersItemsCount(string path)
        {
            var items = OpenFolder(path);

            return items.Count();
        }

        #region Helpers

        private string RemoveLeadingFromPath(string pathPart, string toRemove)
        {
            return pathPart.StartsWith('/' + toRemove) ? pathPart.Substring(toRemove.Length + 1) : pathPart;
        }

        private IEnumerable<WebDavResource> Convert(IEnumerable<SystemFile> files, Uri baseUri)
        {
            var convertResult = new List<WebDavResource>();

            var credentials = new NetworkCredential(ScpContext.User.Login,
                _cryptography.Decrypt(ScpContext.User.EncryptedPassword),
                WebDavAppConfigManager.Instance.UserDomain);

            foreach (var file in files)
            {
                 var webDavitem = new WebDavResource();

                webDavitem.SetCredentials(credentials);

                webDavitem.SetHref(baseUri.Append(ScpContext.User.OrganizationId).Append(file.RelativeUrl.Replace("\\","/")));

                webDavitem.SetItemType(file.IsDirectory? ItemType.Folder : ItemType.Resource);
                webDavitem.SetLastModified(file.Changed);
                webDavitem.ContentLength = file.Size;
                webDavitem.AllocatedSpace = file.FRSMQuotaMB;
                webDavitem.Summary = file.Summary;

                convertResult.Add(webDavitem);
            }

            return convertResult;
        }

        private byte[] ReadFully(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, read);
                return ms.ToArray();
            }
        }

        public void WriteTo(Stream sourceStream, Stream targetStream)
        {
            byte[] buffer = new byte[16 * 1024];
            int n;
            while ((n = sourceStream.Read(buffer, 0, buffer.Length)) != 0)
                targetStream.Write(buffer, 0, n);
        }

        public string GetFileFolderPath(string path)
        {
            path = path.TrimEnd('/');

            if (string.IsNullOrEmpty(path) || !path.Contains('/'))
            {
                return string.Empty;
            }

            string fileName = path.Split('/').Last();
            int index = path.LastIndexOf(fileName, StringComparison.InvariantCultureIgnoreCase);
            string folder = string.IsNullOrEmpty(fileName)? path : path.Remove(index - 1, fileName.Length + 1);

            return folder;
        }

        private string GetResourceName(string path)
        {
            path = path.TrimEnd('/');

            if (string.IsNullOrEmpty(path) || !path.Contains('/'))
            {
                return string.Empty;
            }

            return path.Split('/').Last(); ;
        }

        private IEnumerable<IHierarchyItem> FilterResult(IEnumerable<IHierarchyItem> items)
        {
            var result = items.ToList();

            foreach (var item in items)
            {
                foreach (var itemToIgnore in WebDavAppConfigManager.Instance.FilesToIgnore)
                {
                    var regex = new Regex(itemToIgnore.Regex);

                    Match match = regex.Match(item.DisplayName.Trim('/'));

                    if (match.Success && result.Contains(item))
                    {
                        result.Remove(item);

                        break;
                    }
                }
            }

            return result;
        }

        #endregion
    }
}