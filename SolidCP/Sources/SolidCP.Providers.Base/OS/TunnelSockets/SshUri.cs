using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Renci.SshNet;

namespace SolidCP.Providers.OS
{
    public class SshUri: TunnelUri
    {
        string url = null;

        public string RemoteForwardHost { get; protected set; }
        public uint LocalForwardPort { get; protected set; }
        public uint RemoteForwardPort { get; protected set; }

        [XmlIgnore, IgnoreDataMember]
        public string Protocol
        {
            get => Query["protocol"];
            set => Query["protocol"] = value;
        }

        public PrivateKeyFile[] Keys { get; protected set; } = new PrivateKeyFile[0];

        public SshUri(string url): base(url) { this.url = url; }
        public override string Url {
            get => base.Url;
            set
            {
                if (url != value)
                {
                    base.Url = url = value;
                    if (string.IsNullOrEmpty(url))
                    {
                        RemoteForwardHost = null;
                        LocalForwardPort = RemoteForwardPort = 0;
                        Keys = new PrivateKeyFile[0];
                    }
                    else
                    {
                        var uri = new Uri(url);

                        var match = Regex.Match(uri.PathAndQuery, @"^/?(?:(?<localport>[0-9]+):)?(?:(?<host>\[[0-9a-fA-F:]+\]|[0-9a-zA-Z_.-]+):)?(?<remoteport>[0-9]+)(?:/(?<path>.*?))?(?:?(?<query>.*?))?$");
                        if (match.Success)
                        {
                            if (match.Groups["localport"].Success)
                            {
                                LocalForwardPort = uint.Parse(match.Groups["localport"].Value);
                            }
                            else LocalForwardPort = 0;

                            if (match.Groups["host"].Success)
                            {
                                RemoteForwardHost = match.Groups["host"].Value;
                            }
                            else RemoteForwardHost = null;

                            if (match.Groups["remoteport"].Success)
                            {
                                RemoteForwardPort = uint.Parse(match.Groups["remoteport"].Value);
                            }
                            else RemoteForwardPort = 0;

                            if (match.Groups["path"].Success) Path = match.Groups["path"].Value;
                            else Path = "";

                            var keys = Query["keyfiles"];
                            if (keys != null)
                            {
                                Keys = keys.Split(',')
                                    .Select(file => new PrivateKeyFile(file))
                                    .ToArray();
                            }
                            else Keys = null;
                        }
                        else throw new ArgumentException("This url is not a valid ssh url.");

                        if (uri.Scheme != "ssh") throw new ArgumentException("This url is not a valid ssh url. Ssh urls must begin with ssh://");
                    }
                }
            }
        }

        public override string RawUrl
        {
            get
            {
                var uri = new SshUri(url);
                uri.Tunnel = null;
                uri.Protocol = null;
                return uri.Url;
            }
        }

        public string AccessUrl(IPAddress loopback, int port, string scheme = null)
        {
            // Replace authority and remote port path
            var accessUrl = Regex.Replace(RawUrl, @"(?<=^[0-9a-zA-Z.-]+://)[^/?$]*", $"{loopback}:{port}");
            scheme = scheme ?? Protocol ?? "http";
            // Replace scheme
            accessUrl = Regex.Replace(accessUrl, @"^[a-zA-Z.-0-9]+(?=://)", scheme);
            // Restore
            return accessUrl;
        }
        public string AccessUrl(IPAddress loopback, uint port, string scheme = null) => AccessUrl(loopback, (int)port, scheme);

    }
}
