using System;
using System.Net;

namespace SolidCP.WebDav.Core
{
    /// <summary>
    /// WebDav.Client Namespace.
    /// </summary>
    /// <see cref="http://doc.webdavsystem.com/ITHit.WebDAV.Client.html"/>
    namespace Client
    {
        public class WebDavSession
        {
            public NetworkCredential Credentials { get; set; }

            /// <summary>
            ///     Returns IFolder corresponding to path.
            /// </summary>
            /// <param name="path">Path to the folder.</param>
            /// <returns>Folder corresponding to requested path.</returns>
            public IFolder OpenFolder(string path)
            {
                var folder = new WebDavFolder();
                folder.SetCredentials(Credentials);
                folder.Open(path);
                return folder;
            }

            /// <summary>
            ///     Returns IFolder corresponding to path.
            /// </summary>
            /// <param name="path">Path to the folder.</param>
            /// <returns>Folder corresponding to requested path.</returns>
            public IFolder OpenFolder(Uri path)
            {
                var folder = new WebDavFolder();
                folder.SetCredentials(Credentials);
                folder.Open(path);
                return folder;
            }

            /// <summary>
            ///     Returns IFolder corresponding to path.
            /// </summary>
            /// <param name="path">Path to the folder.</param>
            /// <returns>Folder corresponding to requested path.</returns>
            public IFolder OpenFolderPaged(string path)
            {
                var folder = new WebDavFolder();
                folder.SetCredentials(Credentials);
                folder.OpenPaged(path);
                return folder;
            }

            /// <summary>
            ///     Returns IResource corresponding to path.
            /// </summary>
            /// <param name="path">Path to the resource.</param>
            /// <returns>Resource corresponding to requested path.</returns>
            public IResource OpenResource(string path)
            {
                return OpenResource(new Uri(path));
            }

            /// <summary>
            ///     Returns IResource corresponding to path.
            /// </summary>
            /// <param name="path">Path to the resource.</param>
            /// <returns>Resource corresponding to requested path.</returns>
            public IResource OpenResource(Uri path)
            {
                IFolder folder = OpenFolder(path);
                return folder.GetResource(path.Segments[path.Segments.Length - 1]);
            }
        }
    }
}