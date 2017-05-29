using System;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using SolidCP.WebDav.Core.Config;
using SolidCP.WebDav.Core.Exceptions;
using System.Reflection;

namespace SolidCP.WebDav.Core
{
    namespace Client
    {
        public interface IFolder : IHierarchyItem
        {
            IResource CreateResource(string name);
            IFolder CreateFolder(string name);
            IHierarchyItem[] GetChildren();
            IResource GetResource(string name);
            Uri Path { get; }
        }

        public class WebDavFolder : WebDavHierarchyItem, IFolder
        {
            private IHierarchyItem[] _children = new IHierarchyItem[0];
            private Uri _path;

            public Uri Path { get { return _path; } }

            /// <summary>
            ///     The constructor
            /// </summary>
            public WebDavFolder()
            {
            }

            /// <summary>
            ///     The constructor
            /// </summary>
            /// <param name="path">Path to the folder.</param>
            public WebDavFolder(string path)
            {
                _path = new Uri(path);
            }

            /// <summary>
            ///     The constructor
            /// </summary>
            /// <param name="path">Path to the folder.</param>
            public WebDavFolder(Uri path)
            {
                _path = path;
            }

            /// <summary>
            ///     Creates a resource with a specified name.
            /// </summary>
            /// <param name="name">Name of the new resource.</param>
            /// <returns>Newly created resource.</returns>
            public IResource CreateResource(string name)
            {
                var resource = new WebDavResource();
                try
                {
                    resource.SetHref(new Uri(Href.AbsoluteUri + name));
                    var credentials = (NetworkCredential) _credentials;
                    string auth = "Basic " +
                                  Convert.ToBase64String(
                                      Encoding.Default.GetBytes(credentials.UserName + ":" + credentials.Password));
                    var request = (HttpWebRequest) WebRequest.Create(resource.Href);
                    request.Method = "PUT";
                    request.Credentials = credentials;
                    request.ContentType = "text/xml";
                    request.Accept = "text/xml";
                    request.Headers["translate"] = "f";
                    request.Headers.Add("Authorization", auth);
                    using (var response = (HttpWebResponse) request.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.Created ||
                            response.StatusCode == HttpStatusCode.NoContent)
                        {
                            Open(Href);
                            resource = (WebDavResource) GetResource(name);
                            resource.SetCredentials(_credentials);
                        }
                    }
                }
                catch (AmbiguousMatchException)
                {
                }

                return resource;
            }

            /// <summary>
            ///     Creates new folder with specified name as child of this one.
            /// </summary>
            /// <param name="name">Name of the new folder.</param>
            /// <returns>IFolder</returns>
            public IFolder CreateFolder(string name)
            {
                var folder = new WebDavFolder();
                try
                {
                    var credentials = (NetworkCredential) _credentials;
                    var request = (HttpWebRequest) WebRequest.Create(Href.AbsoluteUri + name);
                    request.Method = "MKCOL";
                    request.Credentials = credentials;
                    string auth = "Basic " +
                                  Convert.ToBase64String(
                                      Encoding.Default.GetBytes(credentials.UserName + ":" + credentials.Password));
                    request.Headers.Add("Authorization", auth);

                    using (var response = (HttpWebResponse) request.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.Created ||
                            response.StatusCode == HttpStatusCode.NoContent)
                        {
                            folder.SetCredentials(_credentials);
                            folder.Open(Href.AbsoluteUri + name + "/");
                        }
                    }
                }
                catch (AmbiguousMatchException)
                {
                }

                return folder;
            }

            /// <summary>
            ///     Returns children of this folder.
            /// </summary>
            /// <returns>Array that include child folders and resources.</returns>
            public IHierarchyItem[] GetChildren()
            {
                return _children;
            }

            /// <summary>
            ///     Gets the specified resource from server.
            /// </summary>
            /// <param name="name">Name of the resource.</param>
            /// <returns>Resource corresponding to requested name.</returns>
            public IResource GetResource(string name)
            {
                IHierarchyItem item =
                    _children.Single(i => i.DisplayName.ToLowerInvariant().Trim('/') == name.ToLowerInvariant().Trim('/'));
                var resource = new WebDavResource();
                resource.SetCredentials(_credentials);
                resource.SetHierarchyItem(item);
                return resource;
            }

            /// <summary>
            ///     Opens the folder.
            /// </summary>
            public void Open()
            {
                var request = (HttpWebRequest)WebRequest.Create(_path);
                request.PreAuthenticate = true;
                request.Method = "PROPFIND";
                request.ContentType = "application/xml";
                request.Headers["Depth"] = "1";
                //TODO Disable SSL
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });

                var credentials = (NetworkCredential)_credentials;
                if (credentials != null && credentials.UserName != null)
                {
                    //request.Credentials = credentials;
                    string auth = "Basic " +
                                  Convert.ToBase64String(
                                      Encoding.Default.GetBytes(credentials.UserName + ":" + credentials.Password));
                    request.Headers.Add("Authorization", auth);
                }
                try
                {
                    using (var response = (HttpWebResponse) request.GetResponse())
                    {
                        using (var responseStream = new StreamReader(response.GetResponseStream()))
                        {
                            string responseString = responseStream.ReadToEnd();
                            ProcessResponse(responseString);
                        }
                    }
                }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        throw new UnauthorizedException();
                    }
                    throw e;
                }
            }

            /// <summary>
            ///     Opens the folder
            /// </summary>
            /// <param name="path">Path of the folder to open.</param>
            public void Open(string path)
            {
                _path = new Uri(path);
                Open();
            }

            /// <summary>
            ///     Opens the folder
            /// </summary>
            /// <param name="path">Path of the folder to open.</param>
            public void Open(Uri path)
            {
                _path = path;
                Open();
            }

            public void OpenPaged(string path)
            {
                _path = new Uri(path);
                OpenPaged();
            }

            public void OpenPaged()
            {
                var request = (HttpWebRequest)WebRequest.Create(_path);
                //request.PreAuthenticate = true;
                request.Method = "SEARCH";

                //TODO Disable SSL
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });

                var credentials = (NetworkCredential)_credentials;
                if (credentials != null && credentials.UserName != null)
                {
                    request.Credentials = _credentials;

                    string auth = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(credentials.UserName + ":" + credentials.Password));
                    request.Headers.Add("Authorization", auth);
                }

                var strQuery = "<?xml version=\"1.0\"?><D:searchrequest xmlns:D = \"DAV:\" >"
                        + "<D:sql>SELECT \"DAV:displayname\" FROM \"" + _path + "\""
                        + "WHERE \"DAV:ishidden\" = false"
                        + "</D:sql></D:searchrequest>";

                try
                {
                   var bytes = Encoding.UTF8.GetBytes(strQuery);

                    request.ContentLength = bytes.Length;

                    using (var requestStream = request.GetRequestStream())
                    {
                        // Write the SQL query to the request stream.
                        requestStream.Write(bytes, 0, bytes.Length);
                    }

                    request.ContentType = "text/xml";

                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        using (var responseStream = new StreamReader(response.GetResponseStream()))
                        {
                            string responseString = responseStream.ReadToEnd();
                            ProcessResponse(responseString);
                        }
                    }
                }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        throw new UnauthorizedException();
                    }
                    throw e;
                }
            }

            /// <summary>
            ///     Processes the response from the server.
            /// </summary>
            /// <param name="response">The raw response from the server.</param>
            private void ProcessResponse(string response)
            {
                try
                {
                    var XmlDoc = new XmlDocument();
                    XmlDoc.LoadXml(response);
                    XmlNodeList XmlResponseList = XmlDoc.GetElementsByTagName("D:response");
                    if (XmlResponseList.Count == 0)
                    {
                        XmlResponseList = XmlDoc.GetElementsByTagName("d:response");
                    }
                    var children = new WebDavResource[XmlResponseList.Count];
                    int counter = 0;
                    foreach (XmlNode XmlCurrentResponse in XmlResponseList)
                    {
                        var item = new WebDavResource();
                        item.SetCredentials(_credentials);

                        foreach (XmlNode XmlCurrentNode in XmlCurrentResponse.ChildNodes)
                        {
                            switch (XmlCurrentNode.LocalName)
                            {
                                case "href":
                                    string href = XmlCurrentNode.InnerText;
                                    if (!Regex.Match(href, "^https?:\\/\\/").Success)
                                    {
                                        href = _path.Scheme + "://" + _path.Host + href;
                                    }
                                    item.SetHref(href, _path);
                                    break;

                                case "propstat":
                                    foreach (XmlNode XmlCurrentPropStatNode in XmlCurrentNode)
                                    {
                                        switch (XmlCurrentPropStatNode.LocalName)
                                        {
                                            case "prop":
                                                foreach (XmlNode XmlCurrentPropNode in XmlCurrentPropStatNode)
                                                {
                                                    switch (XmlCurrentPropNode.LocalName)
                                                    {
                                                        case "creationdate":
                                                            item.SetCreationDate(XmlCurrentPropNode.InnerText);
                                                            break;

                                                        case "getcontentlanguage":
                                                            item.SetProperty(
                                                                new Property(
                                                                    new PropertyName("getcontentlanguage",
                                                                        XmlCurrentPropNode.NamespaceURI),
                                                                    XmlCurrentPropNode.InnerText));
                                                            break;

                                                        case "getcontentlength":
                                                            item.SetProperty(
                                                                new Property(
                                                                    new PropertyName("getcontentlength",
                                                                        XmlCurrentPropNode.NamespaceURI),
                                                                    XmlCurrentPropNode.InnerText));
                                                            break;
                                                        case "getcontenttype":
                                                            item.SetProperty(
                                                                new Property(
                                                                    new PropertyName("getcontenttype",
                                                                        XmlCurrentPropNode.NamespaceURI),
                                                                    XmlCurrentPropNode.InnerText));
                                                            break;

                                                        case "getlastmodified":
                                                            item.SetLastModified(XmlCurrentPropNode.InnerText);
                                                            break;

                                                        case "resourcetype":
                                                            item.SetProperty(
                                                                new Property(
                                                                    new PropertyName("resourcetype",
                                                                        XmlCurrentPropNode.NamespaceURI),
                                                                    XmlCurrentPropNode.InnerXml));
                                                            break;
                                                        //case "lockdiscovery":
                                                        //{
                                                        //    if (XmlCurrentPropNode.HasChildNodes == false)
                                                        //    {
                                                        //        break;
                                                        //    }

                                                        //    foreach (XmlNode activeLockNode in XmlCurrentPropNode.FirstChild)
                                                        //    {
                                                        //        switch (activeLockNode.LocalName)
                                                        //        {
                                                        //            case "owner":
                                                        //                item.SetProperty(
                                                        //                    new Property(
                                                        //                        new PropertyName("owner",
                                                        //                            activeLockNode.NamespaceURI),
                                                        //                        activeLockNode.InnerXml));
                                                        //                break;
                                                        //            case "locktoken":
                                                        //                var lockTokenNode = activeLockNode.FirstChild;
                                                        //                item.SetProperty(
                                                        //                    new Property(
                                                        //                        new PropertyName("locktoken",
                                                        //                            lockTokenNode.NamespaceURI),
                                                        //                        lockTokenNode.InnerXml));
                                                        //                break;
                                                        //        }
                                                        //    }
                                                        //    break;
                                                        //}
                                                    }
                                                }
                                                break;

                                            case "status":
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }

                        if (item.DisplayName != String.Empty)
                        {
                            children[counter] = item;
                            counter++;
                        }
                        else
                        {
                            SetItemType(ItemType.Folder);
                            SetHref(item.Href.AbsoluteUri, item.Href);
                            SetCreationDate(item.CreationDate);
                            SetComment(item.Comment);
                            SetCreatorDisplayName(item.CreatorDisplayName);
                            SetLastModified(item.LastModified);

                            foreach (Property property in item.Properties)
                            {
                                SetProperty(property);
                            }
                        }
                    }

                    int childrenCount = 0;
                    foreach (IHierarchyItem item in children)
                    {
                        if (item != null)
                        {
                            childrenCount++;
                        }
                    }
                    _children = new IHierarchyItem[childrenCount];

                    counter = 0;
                    foreach (IHierarchyItem item in children)
                    {
                        if (item != null)
                        {
                            _children[counter] = item;
                            counter++;
                        }
                    }
                }
                catch (AmbiguousMatchException)
                {
                }
            }
        }
    }
}