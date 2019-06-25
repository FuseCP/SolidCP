using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Net.Security;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Xml;

namespace SolidCP.WebDav.Core
{
    namespace Client
    {
        public interface IResource : IItemContent, IHierarchyItem, IConnectionSettings
        {
            bool CheckedOut { get; }
            bool VersionControlled { get; }
        }

        public class WebDavResource : IResource
        {
            // IResource
            private Uri _baseUri;
            private bool _checkedOut = false;
            private string _comment = "";
            private long _contentLength;
            private DateTime _creationDate = new DateTime(0);
            private string _creatorDisplayName = "";
            private ICredentials _credentials = new NetworkCredential();
            private Uri _href;
            private ItemType _itemType;
            private DateTime _lastModified = new DateTime(0);
            private Property[] _properties = {};
            private int _timeOut = 30000;
            private bool _versionControlled = false;

            public WebDavResource()
            {
                SendChunked = false;
                AllowWriteStreamBuffering = false;
            }

            public WebDavResource(ICredentials credentials, IHierarchyItem item)
            {
                SendChunked = false;
                AllowWriteStreamBuffering = false;

                IsRootItem = item.IsRootItem;
                SetCredentials(credentials);
                SetHierarchyItem(item);
            }

            public Uri BaseUri
            {
                get { return _baseUri; }
            }

            public bool CheckedOut
            {
                get { return _checkedOut; }
            }

            public bool VersionControlled
            {
                get { return _versionControlled; }
            }

            // IItemContent

            public long ContentLength
            {
                get { return _contentLength; }
                set { _contentLength = value; }
            }

            public string ContentType
            {
                get
                {
                    {
                        var property = _properties.FirstOrDefault(x => x.Name.Name == "getcontenttype");
                        return property == null ? MediaTypeNames.Application.Octet : property.StringValue;
                    }
                }
            }

            public string Summary { get; set; }

            /// <summary>
            ///     Downloads content of the resource to a file specified by filename
            /// </summary>
            /// <param name="filename">Full path of a file to be downloaded to</param>
            public void Download(string filename)
            {
                var webClient = new WebClient();
                webClient.DownloadFile(_href, filename);
            }

            public byte[] Download()
            {
                try
                {
                    var webClient = new WebClient();
                    return webClient.DownloadData(_href);
                }
                catch (WebException exception)
                {
                    throw;
                }
            }

            /// <summary>
            ///     Uploads content of a file specified by filename to the server
            /// </summary>
            /// <param name="filename">Full path of a file to be uploaded from</param>
            public void Upload(string filename)
            {
                var credentials = (NetworkCredential) _credentials;
                string auth = "Basic " +
                              Convert.ToBase64String(
                                  Encoding.Default.GetBytes(credentials.UserName + ":" + credentials.Password));
                var webClient = new WebClient();
                webClient.Credentials = credentials;
                webClient.Headers.Add("Authorization", auth);
                webClient.UploadFile(Href, "PUT", filename);
            }

            /// <summary>
            ///     Uploads content of a file specified by filename to the server
            /// </summary>
            /// <param name="data">Posted file data to be uploaded</param>
            public void Upload(byte[] data)
            {
                var credentials = (NetworkCredential)_credentials;
                string auth = "Basic " +
                              Convert.ToBase64String(
                                  Encoding.Default.GetBytes(credentials.UserName + ":" + credentials.Password));
                var webClient = new WebClient();
                webClient.Credentials = credentials;
                webClient.Headers.Add("Authorization", auth);
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                webClient.UploadData(Href, "PUT", data);
            }

            /// <summary>
            ///     Loads content of the resource from WebDAV server.
            /// </summary>
            /// <returns>Stream to read resource content.</returns>
            public Stream GetReadStream()
            {
                var credentials = (NetworkCredential) _credentials;
                string auth = "Basic " +
                              Convert.ToBase64String(
                                  Encoding.Default.GetBytes(credentials.UserName + ":" + credentials.Password));
                var webClient = new WebClient();
                webClient.Credentials = credentials;
                webClient.Headers.Add("Authorization", auth);
                //TODO Disable SSL
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate{ return true; });

                return webClient.OpenRead(_href);
            }

            /// <summary>
            ///     Saves resource's content to WebDAV server.
            /// </summary>
            /// <param name="contentLength">Length of data to be written.</param>
            /// <returns>Stream to write resource content.</returns>
            public Stream GetWriteStream(long contentLength)
            {
                return GetWriteStream("application/octet-stream", contentLength);
            }

            /// <summary>
            ///     Saves resource's content to WebDAV server.
            /// </summary>
            /// <param name="contentType">Media type of the resource.</param>
            /// <param name="contentLength">Length of data to be written.</param>
            /// <returns>Stream to write resource content.</returns>
            public Stream GetWriteStream(string contentType, long contentLength)
            {
                var tcpClient = new TcpClient(Href.Host, Href.Port);
                if (tcpClient.Connected)
                {
                    var credentials = (NetworkCredential) _credentials;
                    string auth = "Basic " +
                                  Convert.ToBase64String(
                                      Encoding.Default.GetBytes(credentials.UserName + ":" + credentials.Password));

                    try
                    {
                        if (TimeOut != Timeout.Infinite)
                        {
                            tcpClient.SendTimeout = TimeOut;
                            tcpClient.ReceiveTimeout = TimeOut;
                        }
                        else
                        {
                            tcpClient.SendTimeout = 0;
                            tcpClient.ReceiveTimeout = 0;
                        }
                    }
                    catch (SocketException e)
                    {
                        tcpClient.SendTimeout = 0;
                        tcpClient.ReceiveTimeout = 0;
                    }
                    NetworkStream networkStream = tcpClient.GetStream();
                    if (networkStream.CanTimeout)
                    {
                        try
                        {
                            networkStream.WriteTimeout = TimeOut;
                            networkStream.ReadTimeout = TimeOut;
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    byte[] methodBuffer = Encoding.UTF8.GetBytes("PUT " + Href.AbsolutePath + " HTTP/1.1\r\n");
                    byte[] hostBuffer = Encoding.UTF8.GetBytes("Host: " + Href.Host + "\r\n");
                    byte[] contentLengthBuffer = Encoding.UTF8.GetBytes("Content-Length: " + contentLength + "\r\n");
                    byte[] authorizationBuffer = Encoding.UTF8.GetBytes("Authorization: " + auth + "\r\n");
                    byte[] connectionBuffer = Encoding.UTF8.GetBytes("Connection: Close\r\n\r\n");
                    networkStream.Write(methodBuffer, 0, methodBuffer.Length);
                    networkStream.Write(hostBuffer, 0, hostBuffer.Length);
                    networkStream.Write(contentLengthBuffer, 0, contentLengthBuffer.Length);
                    networkStream.Write(authorizationBuffer, 0, authorizationBuffer.Length);
                    networkStream.Write(connectionBuffer, 0, connectionBuffer.Length);

                    return networkStream;
                }

                throw new IOException("could not connect to server");
            }

            // IHierarchyItem

            public string Comment
            {
                get { return _comment; }
            }

            public DateTime CreationDate
            {
                get { return _creationDate; }
            }

            public string CreatorDisplayName
            {
                get { return _creatorDisplayName; }
            }

            public string DisplayName
            {
                get
                {
                    string displayName = _href.ToString().Trim('/').Replace(_baseUri.ToString().Trim('/'), "");
                    displayName = Regex.Replace(displayName, "\\/$", "");
                    Match displayNameMatch = Regex.Match(displayName, "([\\/]+)$");
                    if (displayNameMatch.Success)
                    {
                        displayName = displayNameMatch.Groups[1].Value;
                    }
                    return HttpUtility.UrlDecode(displayName.Trim('/'));
                }
            }

            public long AllocatedSpace { get; set; }
            public bool IsRootItem { get; set; }

            public Uri Href
            {
                get { return _href; }
                set { SetHref(value.ToString(), new Uri(value.Scheme + "://" + value.Host + value.Segments[0] + value.Segments[1])); }
            }

            public ItemType ItemType
            {
                get { return _itemType; }
                set { _itemType = value; }
            }

            public DateTime LastModified
            {
                get { return _lastModified; }
            }

            public Property[] Properties
            {
                get { return _properties; }
            }

            // IHierarchyItem Methods
            /// <summary>
            ///     Retrieves all custom properties exposed by the item.
            /// </summary>
            /// <returns>This method returns the array of custom properties exposed by the item.</returns>
            public Property[] GetAllProperties()
            {
                return _properties;
            }

            /// <summary>
            ///     Returns names of all custom properties exposed by this item.
            /// </summary>
            /// <returns></returns>
            public PropertyName[] GetPropertyNames()
            {
                return _properties.Select(p => p.Name).ToArray();
            }

            /// <summary>
            ///     Retrieves values of specific properties.
            /// </summary>
            /// <param name="names"></param>
            /// <returns>Array of requested properties with values.</returns>
            public Property[] GetPropertyValues(PropertyName[] names)
            {
                return (from p in _properties from pn in names where pn.Equals(p.Name) select p).ToArray();
            }

            /// <summary>
            ///     Deletes this item.
            /// </summary>
            public void Delete()
            {
                var credentials = (NetworkCredential) _credentials;
                string auth = "Basic " +
                              Convert.ToBase64String(
                                  Encoding.Default.GetBytes(credentials.UserName + ":" + credentials.Password));
                WebRequest webRequest = WebRequest.Create(Href);
                webRequest.Method = "DELETE";
                webRequest.Credentials = credentials;
                webRequest.Headers.Add("Authorization", auth);
                using (WebResponse webResponse = webRequest.GetResponse())
                {
                    using (Stream responseStream = webResponse.GetResponseStream())
                    {
                        var buffer = new byte[8192];
                        string result = "";
                        int bytesRead = 0;
                        do
                        {
                            bytesRead = responseStream.Read(buffer, 0, buffer.Length);
                            if (bytesRead > 0)
                            {
                                result += Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            }
                        } while (bytesRead > 0);
                    }
                }
            }


            /// <summary>
            ///     Lock this item.
            /// </summary>
            public string Lock()
            {
                var credentials = (NetworkCredential)_credentials;
                string lockToken = string.Empty;


                string lockXml =string.Format( "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                                 "<D:lockinfo xmlns:D='DAV:'>" +
                                 "<D:lockscope><D:exclusive/></D:lockscope>" +
                                 "<D:locktype><D:write/></D:locktype>" +
                                 "<D:owner>{0}</D:owner>" +
                                 "</D:lockinfo>", ScpContext.User.Login);

                string auth = "Basic " +
                              Convert.ToBase64String(
                                  Encoding.Default.GetBytes(credentials.UserName + ":" + credentials.Password));

                WebRequest webRequest = WebRequest.Create(Href);

                webRequest.Method = "LOCK";
                webRequest.Credentials = credentials;
                webRequest.Headers.Add("Authorization", auth);
                webRequest.PreAuthenticate = true;
                webRequest.ContentType = "application/xml";

                // Retrieve the request stream.
                using (Stream requestStream = webRequest.GetRequestStream())
                {
                    // Write the lock XML to the destination.
                    requestStream.Write(Encoding.UTF8.GetBytes(lockXml), 0, lockXml.Length);
                }

                using (WebResponse webResponse = webRequest.GetResponse())
                {
                    lockToken = webResponse.Headers["Lock-Token"];
                }

                return lockToken;
            }

            /// <summary>
            ///     Lock this item.
            /// </summary>
            public void UnLock()
            {
                WebRequest webRequest = WebRequest.Create(Href);

                webRequest.Method = "UNLOCK";
                webRequest.Credentials = _credentials;
                webRequest.PreAuthenticate = true;

                webRequest.Headers.Add(@"Lock-Token", Properties.First(x => x.Name.Name == "locktoken").StringValue);

                using (WebResponse webResponse = webRequest.GetResponse())
                {
                    //TODO unlock
                }
            }

            public bool AllowWriteStreamBuffering { get; set; }
            public bool SendChunked { get; set; }

            public int TimeOut
            {
                get { return _timeOut; }
                set { _timeOut = value; }
            }

            /// <summary>
            ///     For internal use only.
            /// </summary>
            /// <param name="comment"></param>
            public void SetComment(string comment)
            {
                _comment = comment;
            }

            /// <summary>
            ///     For internal use only.
            /// </summary>
            /// <param name="comment"></param>
            public void SetCreationDate(string creationDate)
            {
                _creationDate = DateTime.Parse(creationDate);
            }

            /// <summary>
            ///     For internal use only.
            /// </summary>
            /// <param name="comment"></param>
            public void SetCreationDate(DateTime creationDate)
            {
                _creationDate = creationDate;
            }

            /// <summary>
            ///     For internal use only.
            /// </summary>
            /// <param name="comment"></param>
            public void SetCreatorDisplayName(string creatorDisplayName)
            {
                _creatorDisplayName = creatorDisplayName;
            }

            /// <summary>
            ///     For internal use only.
            /// </summary>
            /// <param name="comment"></param>
            public void SetHref(string href, Uri baseUri)
            {
                _href = new Uri(href);
                _baseUri = baseUri;
            }

            /// <summary>
            ///     For internal use only.
            /// </summary>
            /// <param name="comment"></param>
            public void SetHref(Uri href)
            {
                _href = href;

                var baseUrl = href.ToString().Remove(href.ToString().Length - href.ToString().Trim('/').Split('/').Last().Length);

                _baseUri = new Uri(baseUrl);
            }

            /// <summary>
            ///     For internal use only.
            /// </summary>
            /// <param name="comment"></param>
            public void SetLastModified(string lastModified)
            {
                _lastModified = DateTime.Parse(lastModified);
            }

            /// <summary>
            ///     For internal use only.
            /// </summary>
            /// <param name="comment"></param>
            public void SetLastModified(DateTime lastModified)
            {
                _lastModified = lastModified;
            }

            /// <summary>
            ///     For internal use only.
            /// </summary>
            /// <param name="comment"></param>
            public void SetItemType(ItemType type)
            {
                _itemType = type;
            }

            /// <summary>
            ///     For internal use only.
            /// </summary>
            /// <param name="comment"></param>
            public void SetProperty(Property property)
            {
                if (property.Name.Name == "resourcetype" && property.StringValue != String.Empty)
                {
                    var XmlDoc = new XmlDocument();
                    try
                    {
                        XmlDoc.LoadXml(property.StringValue);
                        property.StringValue = XmlDoc.DocumentElement.LocalName;
                        switch (property.StringValue)
                        {
                            case "collection":
                                _itemType = ItemType.Folder;
                                break;

                            default:
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }

                bool propertyFound = false;
                foreach (Property prop in _properties)
                {
                    if (prop.Name.Equals(property.Name))
                    {
                        prop.StringValue = property.StringValue;
                        propertyFound = true;
                    }
                }

                if (!propertyFound)
                {
                    var newProperties = new Property[_properties.Length + 1];
                    for (int i = 0; i < _properties.Length; i++)
                    {
                        newProperties[i] = _properties[i];
                    }
                    if (property.Name.Name == "getcontentlength")
                    {
                        try
                        {
                            _contentLength = Convert.ToInt64(property.StringValue);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    newProperties[_properties.Length] = property;
                    _properties = newProperties;
                }
            }

            /// <summary>
            ///     For internal use only.
            /// </summary>
            /// <param name="comment"></param>
            public void SetProperty(PropertyName propertyName, string value)
            {
                SetProperty(new Property(propertyName, value));
            }

            /// <summary>
            ///     For internal use only.
            /// </summary>
            /// <param name="comment"></param>
            public void SetProperty(string name, string nameSpace, string value)
            {
                SetProperty(new Property(name, nameSpace, value));
            }

            /// <summary>
            ///     For internal use only.
            /// </summary>
            /// <param name="comment"></param>
            public void SetProperties(Property[] properties)
            {
                foreach (Property property in properties)
                {
                    SetProperty(property);
                }
            }

            /// <summary>
            ///     For internal use only.
            /// </summary>
            /// <param name="comment"></param>
            public void SetCredentials(ICredentials credentials)
            {
                _credentials = credentials;
            }

            // IConnectionSettings

            /// <summary>
            ///     For internal use only.
            /// </summary>
            /// <param name="comment"></param>
            public void SetHierarchyItem(IHierarchyItem item)
            {
                SetComment(item.Comment);
                SetCreationDate(item.CreationDate);
                SetCreatorDisplayName(item.CreatorDisplayName);
                SetHref(item.Href);
                SetLastModified(item.LastModified);
                SetProperties(item.Properties);
                SetItemType(item.ItemType);
            }
        }
    }
}