using Renci.SshNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace SolidCP.Providers.OS
{
    /// <summary>
    /// A class supporting uri's for TunnelSockes's. The class has public read and writable properties Query, QueryString, Port and Tunnel
    /// The Url property that gets or sets the url string always reflects the setting in the Query, QueryString, Port and Tunnel
    /// properties. Query is a StringDictionary of all query variables, whereas QueryString is just the string of the query url part.
    /// Tunnel is a query string parameter named "tunnel=...". The tunnel parameter is used to specify special options for the
    /// TunnelSocket in the url, like if the tunnel is a fallback tunnel or a listener tunnel.
    /// Fallback TunnelSockets support upgrading their url according the url of the TunnelSocket's connected websockets url.
    /// That way, instead of creating a tunnel from the Portal to EnterpriseServer and then to Server, each client can upgrade it's
    /// url and the Portal can directly connect to the url that was able on the Server if it is possible to reach that url from the 
    /// Portal.
    /// A listener TunnelSocket is a TunnelSocket that listens on a local port with a Socket. When calling ConnectAsync, the
    /// TunnelSocket will start listening on the port specified in the url.
    /// </summary>
    public class TunnelUri
    {
        string url = null;

        public TunnelUri(string url)
        {
            if (!string.IsNullOrEmpty(url)) Url = url;
            else this.url = url;
        }
        public string Username { get; protected set; }
        public string Password { get; protected set; }
        public string Host { get; protected set; }
        public string DnsSafeHost { get; protected set; }
        public string IdnHost { get; protected set; }
        public bool IsDefaultPort => Port == 22;
        public string Path { get; protected set; }

        [XmlIgnore, IgnoreDataMember]
        public StringDictionary Query { get; set; } = new StringDictionary();

        [XmlIgnore, IgnoreDataMember]
        public string Tunnel
        {
            get => Query["tunnel"];
            set => Query["tunnel"] = value;
        }

        [XmlIgnore, IgnoreDataMember]
        public bool IsListener
        {
            get => Tunnel == "listener";
            set => Tunnel = value ? "listener" : null;
        }

        [XmlIgnore, IgnoreDataMember]
        public bool IsFallback
        {
            get => Tunnel == "fallback";
            set => Tunnel = value ? "fallback" : null;
        }

        public virtual string Url {
            get => !string.IsNullOrEmpty(url) ? Regex.Replace(url , @"\?.*?$", QueryString, RegexOptions.Singleline) : url;
            set
            {
                if (url != value)
                {
                    url = value;
                    if (string.IsNullOrEmpty(url))
                    {
                        Username = Password = Host = DnsSafeHost = IdnHost = Path = QueryString = null;
                        Tunnel = null;
                        port = 0;
                    }
                    else
                    {
                        var uri = new Uri(url);
                        var userInfo = uri.UserInfo.Split(':');
                        Username = userInfo[0];
                        if (userInfo.Length > 1) Password = userInfo[1];
                        else Password = null;
                        Host = uri.Host;
                        DnsSafeHost = uri.DnsSafeHost;
                        IdnHost = uri.IdnHost;
                        port = uri.Port == 0 ? 22 : uri.Port;
                        Path = uri.AbsolutePath;
                        QueryString = uri.Query;
                    }
                }
            }
        }

        int port = 0;
        [XmlIgnore, IgnoreDataMember]
        public int Port
        {
            get => port;
            set
            {
                if (port != value)
                {
                    port = value;
                    var uri = new Uri(Url ?? "");
                    Url = $"{uri.Scheme}://{(!string.IsNullOrEmpty(uri.UserInfo) ? $"{uri.UserInfo}@" : "")}{uri.Host}{(port != 0 ? $":{port}" : "")}{uri.PathAndQuery}";
                }
            }
        }

        [XmlIgnore, IgnoreDataMember]
        public virtual string QueryString
        {
            get
            {
                var str = new StringBuilder();
                foreach (DictionaryEntry par in Query)
                {
                    if (str.Length > 0) str.Append("&");
                    str.Append(par.Key);
                    if ((string)par.Value != "")
                    {
                        str.Append("=");
                        str.Append(par.Value);
                    }
                }
                return str.ToString();
            }
            set
            {
                Query.Clear();
                foreach (var par in value.Split('&', ';'))
                {
                    var tokens = par.Split('=');
                    if (tokens.Length > 1) Query.Add(tokens[0].Trim(), tokens[1].Trim());
                    else Query.Add(tokens[0].Trim(), "");
                }
            }
        }

        public virtual string RawUrl
        {
            get
            {
                var copy = new TunnelUri(Url);
                copy.Tunnel = null;
                return copy.Url;
            }
        }
        public override string ToString() => Url;
    }
}
