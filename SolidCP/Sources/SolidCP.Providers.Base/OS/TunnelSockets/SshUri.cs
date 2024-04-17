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
    /// <summary>
    /// A class supporting uri's for SshTunnel's. The syntax of the url's for SshTunnels is as follows:
    /// 
    /// ssh://&lt;username&gt:&lt;password&gt;@&lt;&lt;sshhost&gt;:&lt;sshport&gt;/[[&lt;localport&gt;:]&lt;remotehost&gt;:]&lt;remoteport&gt[/&lt;path&gt;][?... protocol=&lt;protocol&gt; ... keyfiles=&lt;comma separated list of key files&gt; ... ]
    /// 
    /// The parameters are as follows:
    /// - username: The username used to connect via ssh
    /// - password: The password used to connect via ssh
    /// - sshhost: The host to connect to via ssh
    /// - sshport: The port to use to connect via ssh
    /// - localport: The local port where the ssh tunnel will listen on
    /// - remotehost: When specified the tunnel will go the the remotehost from sshost when not specified this will connect
    ///   to the loopback address on the sshhost
    /// - The port on the remotehost or sshhost to connect to
    /// - protocol: The protocol to use when connecting over the SshTunnel, either http or https. When connecting to servers via 
    ///   ordinary SolidCP.Web.Clients access, this parameter is not needed and the protocol will be determined automatically and
    ///   will default to http. If you still want to connect using https, specify this parameter like protocol=https in the
    ///   ServerUrl
    /// - keyfile: A comma separated list of key files used to connect via ssh, if no password is specified
    /// 
    /// This class derives from TunnelUri, and as such inherits it's url manipulation properties Query, QueryString, Port and Tunnel
    /// by which you can change the url very easily.
    /// </summary>
    [DataContract]
    public class SshUri: TunnelUri
    {
        string url = null;

        public string RemoteForwardHost { get; protected set; }
        public uint LocalForwardPort { get; protected set; }
        public uint RemoteForwardPort { get; protected set; }

        public string Protocol
        {
            get => Query["protocol"];
            set => Query["protocol"] = value;
        }

        public PrivateKeyFile[] Keys { get; protected set; } = new PrivateKeyFile[0];

        public SshUri() { }
        public SshUri(string url) => Url = url;

        [DataMember]
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
                uri.Tunnel = TunnelOption.None;
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
