using Renci.SshNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace SolidCP.Providers.OS
{

    public enum TunnelOption { None, Fallback, Listener, WebSocket }

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
    [DataContract]
    [KnownType(typeof(SshUri))]
    public class TunnelUri
    {
        string url = null;

        public TunnelUri() { }
        public TunnelUri(string url) => Url = url;

        string scheme = null;
        public string Scheme
        {
            get => scheme;
            set
            {
                if (scheme != value)
                {
                    scheme = value;
                    if (!url.StartsWith($"{scheme}://")) url = Regex.Replace(url, @"^[a-zA-Z.-]+(?=://)", scheme);
                }
            }
        }
        public string Username { get; protected set; }
        public string Password { get; protected set; }
        public string Host { get; protected set; }
        public string DnsSafeHost { get; protected set; }
        public string IdnHost { get; protected set; }
        public bool IsDefaultPort => Port == 22;
        public string Path { get; protected set; }

        public QueryStringDictionary Query { get; set; } = new QueryStringDictionary();

        public TunnelOption Tunnel
        {
            get
            {
                var tunnel = Query["tunnel"];
                TunnelOption option = TunnelOption.None;
                Enum.TryParse(tunnel, true, out option);
                return option;
            }
            set => Query["tunnel"] = value == TunnelOption.None ? null : value.ToString().ToLower();
        }

        public bool IsListener
        {
            get => Tunnel == TunnelOption.Listener;
            set => Tunnel = value ? TunnelOption.Listener : TunnelOption.None;
        }

        public bool IsFallback
        {
            get => Tunnel == TunnelOption.Fallback;
            set => Tunnel = value ? TunnelOption.Fallback : TunnelOption.None;
        }

        [DataMember]
        public virtual string Url
        {
            get
            {
                if (string.IsNullOrEmpty(url)) return url;
                if (url.Contains('?')) return Regex.Replace(url, @"(?<=\?).*?$", QueryString, RegexOptions.Singleline);
                if (Query.Count > 0) return $"{url}?{QueryString}";
                return url;
            }
            set
            {
                if (url != value)
                {
                    url = value;
                    if (string.IsNullOrEmpty(url))
                    {
                        Username = Password = Host = DnsSafeHost = IdnHost = Path = QueryString = null;
                        Tunnel = TunnelOption.None;
                        port = 0;
                    }
                    else
                    {
                        var uri = new Uri(url);
                        scheme = uri.Scheme;
                        var userInfo = uri.UserInfo.Split(':');
                        Username = userInfo[0];
                        if (userInfo.Length > 1) Password = userInfo[1];
                        else Password = null;
                        Host = uri.Host;
                        DnsSafeHost = uri.DnsSafeHost;
                        IdnHost = uri.IdnHost;
                        port = uri.Port == 0 ? 22 : uri.Port;
                        Path = uri.AbsolutePath;
                        Query = new QueryStringDictionary(url);
                    }
                }
            }
        }

        int port = 0;
        public int Port
        {
            get => port;
            set
            {
                if (port != value)
                {
                    port = value;
                    url = Regex.Replace(url, "(?<=^[a-zA-Z.-]+://(?:[^@]+@)?[a-zA-Z0-9.-]+)(?::[0-9]+)?(/|$)", $":{port}$1");
                }
            }
        }

        public virtual string QueryString
        {
            get => Query.QueryString;
            set => Query.QueryString = value;
        }

        public virtual string RawUrl
        {
            get
            {
                var copy = new TunnelUri(Url);
                copy.Tunnel = TunnelOption.None;
                return copy.Url;
            }
        }
        public override string ToString() => Url;
    }
}
