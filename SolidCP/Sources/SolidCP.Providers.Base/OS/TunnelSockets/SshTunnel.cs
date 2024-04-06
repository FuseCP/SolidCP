using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;

namespace SolidCP.Providers.OS
{
    public class SshTunnel : IDisposable
    {
        public string AccessUrl {
            get {
                // Delete keyfiles query parameter
                var accessUrl = Regex.Replace(Url ?? "", @"(?<=\?.*?)(?:(?<=\?)|&)keyfiles=[^&$]*(?=&|$)", "");
                // Replace authority and remote port path
                const string ReplaceSshAuthorityRegex = @"(?<=^ssh://)(?<sshlogin>[^/]*)/(?:(?<localport>[0-9]+):)?(?:(?<host>\[[0-9a-fA-F:]+\]|[0-9a-zA-Z_.-]+):)?(?<remoteport>[0-9]+)";
                accessUrl = Regex.Replace(accessUrl, ReplaceSshAuthorityRegex, $"{Loopback}:{Port.BoundPort}");
                return accessUrl;
            }
        }

        public SshClient Client;
        public ForwardedPortLocal Port;
        public string Url;
        public Exception Exception, ConnectException;
        public bool IsConnecting;
        public TimeSpan ConnectTimeout = TimeSpan.FromSeconds(15);
        public IPAddress Loopback;
        public SshTunnel(string url)
        {
            Url = url;
            Client = null;
            Exception = null;
            ConnectException = null;
            Port = null;
            IsConnecting = true;
        }

        public static void ParseSshUrl(string url, out string username, out string password, out string sshhost, out int sshport,
            out uint localport, out string remotehost, out uint remoteport, out string[] keyfiles)
        {
            var uri = new Uri(url);
            var userToken = uri.UserInfo.Split(':');
            username = userToken.First();
            password = userToken.Skip(1).FirstOrDefault();
            sshhost = uri.Host;
            sshport = uri.Port;
            const string ParseSshUrlRegex = @"^ssh://(?<sshlogin>[^/]*)/(?:(?<localport>[0-9]+):)?(?:(?<host>\[[0-9a-fA-F:]+\]|[0-9a-zA-Z_.-]+):)?(?<remoteport>[0-9]+)(?:/(?<path>[^?$]*))?(?:$|(?:(?:\?|\?.*&)keyfiles=(?<keys>.*?)(?:&|$))?)";

            var match = Regex.Match(url, ParseSshUrlRegex, RegexOptions.Singleline);
            localport = match.Groups["localport"].Success ? uint.Parse(match.Groups["localport"].Value) : uint.MaxValue;
            remoteport = uint.Parse(match.Groups["remoteport"].Value);
            remotehost = match.Groups["host"].Success ? match.Groups["host"].Value : null;
            keyfiles = WebUtility.UrlDecode(match.Groups["keys"].Success ? match.Groups["keys"].Value : "").Split(';');
        }
        private async Task InitAsync()
        {
            string username, password, sshhost, remotehost;
            int sshport;
            uint localport, remoteport;
            string[] keyfilesFileNames;

            ParseSshUrl(Url, out username, out password, out sshhost, out sshport, out localport, out remotehost,
                out remoteport, out keyfilesFileNames);

            var keyfiles = keyfilesFileNames.Select(file => new PrivateKeyFile(file)).ToArray();

            var uri = new Uri(Url);

            var sshhostip = await DnsService.GetFirstIPAddressAsync(sshhost);
            if (remotehost == null)
            {
                if (sshhostip.AddressFamily == AddressFamily.InterNetwork)
                {
                    remotehost = IPAddress.Loopback.ToString();
                    Loopback = IPAddress.Loopback;
                }
                else
                {
                    remotehost = IPAddress.IPv6Loopback.ToString();
                    Loopback = IPAddress.IPv6Loopback;
                }
            }
            sshhost = sshhostip.ToString();

            if (string.IsNullOrEmpty(password))
            {
                Client = new SshClient(sshhost, uri.Port != -1 ? uri.Port : 22, username, keyfiles);
            }
            else
            {
                Client = new SshClient(sshhost, uri.Port != -1 ? uri.Port : 22, username, password);
            }

            if (localport == uint.MaxValue) Port = new ForwardedPortLocal(Loopback.ToString(), remotehost, remoteport);
            else Port = new ForwardedPortLocal(Loopback.ToString(), localport, remotehost, remoteport);
        }

        private void Init()
        {
            string username, password, sshhost, remotehost;
            int sshport;
            uint localport, remoteport;
            string[] keyfilesFileNames;

            ParseSshUrl(Url, out username, out password, out sshhost, out sshport, out localport, out remotehost,
                out remoteport, out keyfilesFileNames);

            var keyfiles = keyfilesFileNames.Select(file => new PrivateKeyFile(file)).ToArray();

            var uri = new Uri(Url);

            var sshhostip = DnsService.GetFirstIPAddress(sshhost);
            if (remotehost == null)
            {
                if (sshhostip.AddressFamily == AddressFamily.InterNetwork)
                {
                    remotehost = IPAddress.Loopback.ToString();
                    Loopback = IPAddress.Loopback;
                }
                else
                {
                    remotehost = IPAddress.IPv6Loopback.ToString();
                    Loopback = IPAddress.IPv6Loopback;
                }
            }
            sshhost = sshhostip.ToString();

            if (string.IsNullOrEmpty(password))
            {
                Client = new SshClient(sshhost, uri.Port != -1 ? uri.Port : 22, username, keyfiles);
            }
            else
            {
                Client = new SshClient(sshhost, uri.Port != -1 ? uri.Port : 22, username, password);
            }

            if (localport == uint.MaxValue) Port = new ForwardedPortLocal(Loopback.ToString(), remotehost, remoteport);
            else Port = new ForwardedPortLocal(Loopback.ToString(), localport, remotehost, remoteport);
        }

        public bool Create()
        {
            Init();

            try
            {
                IsConnecting = true;
                Client.Connect();
                Client.AddForwardedPort(Port);
                Port.Start();
                IsConnecting = false;
                Client.ErrorOccurred += Restart;
                Port.Exception += Restart;

                Trace.TraceInformation($"SSH Tunnel on {Url} started.");
                return true;
            }
            catch (Exception ex)
            {
                ConnectException = ex;
                IsConnecting = false;
                Disconnect();

                Trace.TraceError($"Failed to connect SSH Tunnel to {Url}: {ex}");
                return false;
            }
        }

        public async Task CreateAsync()
        {
            await InitAsync();

            try
            {
                IsConnecting = true;
                await Client.ConnectAsync(new CancellationTokenSource(ConnectTimeout).Token);
                await Task.Run(() =>
                {
                    Client.AddForwardedPort(Port);
                    Port.Start();
                });
                IsConnecting = false;
                Client.ErrorOccurred += Restart;
                Port.Exception += Restart;
                Trace.TraceInformation($"SSH Tunnel on {Url} started.");
            }
            catch (Exception ex)
            {
                ConnectException = ex;
                IsConnecting = false;
                Disconnect();

                Trace.TraceError($"Failed to connect SSH Tunnel to {Url}: {ex}");
            }
        }

        bool isRestarting = false;
        public void Restart(object sender, ExceptionEventArgs args)
        {
            lock (this)
            {
                if (isRestarting || Exception == args.Exception || ConnectException != null) return;
                Exception = args.Exception;
                isRestarting = true;
                IsConnecting = true;
                Client.ErrorOccurred -= Restart;
                Port.Exception -= Restart;

                Trace.TraceError($"Exception on SSH Tunnel {Url}: {Exception}");

                Disconnect();

                ConnectException = null;
                try
                {
                    Client.Connect();
                    Port.Start();
                }
                catch (Exception ex)
                {
                    ConnectException = ex;
                    isRestarting = false;
                    Disconnect();

                    Trace.TraceError($"Failed to reconnect SSH Tunnel to {Url}: {ex}");
                }
                isRestarting = IsConnecting = false;
                Client.ErrorOccurred += Restart;
                Port.Exception += Restart;
            }
        }

        bool isDisposed = false;
        public void Disconnect()
        {
            lock (this)
            {
                if (Port.IsStarted)
                {
                    try
                    {
                        Port.Stop();
                    }
                    catch { }
                }
                if (Client.IsConnected)
                {
                    try
                    {
                        Client.Disconnect();
                    }
                    catch { }
                }
            }
        }
        public void Dispose()
        {
            Disconnect();
            if (!isDisposed)
            {
                isDisposed = true;
                Client.Dispose();
            }
        }
    }
}
