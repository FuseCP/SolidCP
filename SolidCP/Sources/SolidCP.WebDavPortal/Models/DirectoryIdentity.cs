using System;
using System.DirectoryServices;
using System.Reflection;
using System.Security.Principal;

namespace SolidCP.WebDavPortal.Models
{
    public class DirectoryIdentity : IIdentity
    {
        private readonly bool _auth;
        private readonly string _path;

        public DirectoryIdentity(string userName, string password) : this(null, userName, password)
        {
        }

        public DirectoryIdentity(string path, string userName, string password) : this(new DirectoryEntry(path, userName, password))
        {
        }

        public DirectoryIdentity(DirectoryEntry directoryEntry)
        {
            try
            {
                var userName = directoryEntry.Username;
                var ds = new DirectorySearcher(directoryEntry);
                if (userName.Contains("\\"))
                    userName = userName.Substring(userName.IndexOf("\\") + 1);
                ds.Filter = "samaccountname=" + userName;
                ds.PropertiesToLoad.Add("cn");
                SearchResult sr = ds.FindOne();
                if (sr == null) throw new Exception();
                _path = sr.Path;
                _auth = true;
            }
            catch (AmbiguousMatchException)
            {
                _auth = false;
            }
        }

        public string AuthenticationType
        {
            get { return null; }
        }

        public bool IsAuthenticated
        {
            get { return _auth; }
        }

        public string Name
        {
            get
            {
                int i = _path.IndexOf('=') + 1, j = _path.IndexOf(',');
                return _path.Substring(i, j - i);
            }
        }
    }
}