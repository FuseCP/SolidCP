using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Renci.SshNet;
using System.Xml;

namespace SolidCP.Providers.OS
{
    /// <summary>
    /// Implements a Socket, that can be a connetion over a WebSocket, a Socket or an SshTunnel. The WebSocket supports
    /// an initial fallback protocol to automatically upgrade the WebSocket to a upgrade url, if the url is reachable from
    /// the server. This way Portal can create a TunnelSocket to a service directly on the Server without creating a tunnel
    /// from Portal to EnterpriseServer and then to Server, if that service is also reachable from Portal.
    /// </summary>
    [DataContract]
    public class TunnelSocket : IDisposable, IAsyncDisposable
    {
        public Socket BaseSocket { get; set; }
        public WebSocket BaseWebSocket { get; set; }
        public SshTunnel BaseSshTunnel { get; set; }

        public TunnelSocket() { }
        public TunnelSocket(Socket socket) : this()
        {
            BaseSocket = socket;
            Url = "";
        }

        public TunnelSocket(WebSocket socket) : this()
        {
            BaseWebSocket = socket;
            Url = "";
        }
        public TunnelSocket(IPAddress address) : this(new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
        {
            Url = $"tcp://{address}";
        }
        public TunnelSocket(IPAddress address, int port) : this(new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
        {
            Url = $"tcp://{address}:{port}";
        }
        public TunnelSocket(string url) : this() => Url = url;

        public TunnelSocket Clone
        {
            get
            {
                var clone = (TunnelSocket)Activator.CreateInstance(GetType());
                clone.Url = Url;
                clone.UpgradeTunnelSocket = UpgradeTunnelSocket;
                clone.BaseSocket = BaseSocket;
                clone.BaseWebSocket = BaseWebSocket;
                clone.BaseSshTunnel = BaseSshTunnel;
                foreach (var cookie in Cookies) clone.Cookies.Add(cookie);
                foreach (DictionaryEntry header in HttpHeaders) clone.HttpHeaders.Add((string)header.Key, (string)header.Value);
                return clone;
            }
        }

        public void CopyFrom(TunnelSocket from)
        {
            Url = from.Url;
            UpgradeTunnelSocket = from.UpgradeTunnelSocket;
            BaseSocket = from.BaseSocket;
            BaseWebSocket = from.BaseWebSocket;
            BaseSshTunnel = from.BaseSshTunnel;
            foreach (var cookie in from.Cookies) Cookies.Add(cookie);
            foreach (DictionaryEntry header in from.HttpHeaders) HttpHeaders.Add((string)header.Key, (string)header.Value);
        }

        public TunnelUri Uri { get; set; }
        public bool IsWebSocket => url.StartsWith("ws://") || url.StartsWith("wss://") || IsWebSocketOverSsh ||
             BaseWebSocket != null;
        public bool IsSocket => url.StartsWith("tcp://") || url.StartsWith("udp://") || BaseSocket != null;
        public bool IsSshTunnel => url.StartsWith("ssh://") || BaseSshTunnel != null;
        public bool IsWebSocketOverSsh
        {
            get => IsSshTunnel && Uri.Tunnel == TunnelOption.WebSocket;
            set
            {
                if (IsSshTunnel) Uri.Tunnel = TunnelOption.WebSocket;
            }
        }
        public bool IsSecure => url.StartsWith("wss://") || IsSshTunnel || url.StartsWith("https://");

        string url;
        [DataMember]
        public string Url
        {
            // Replace the url's QueryString
            get => Uri?.Url;
            set => Uri = (url = value).StartsWith("ssh://") ? new SshUri(url) : new TunnelUri(url);
        }

        public bool ValidateCertificate { get; set; } = true;
        public SshUri SshUri => (Uri is SshUri sshUri) ? sshUri : null;

        public string RawUrl => Uri?.RawUrl;

        public async Task<string> GetSshWebSocketUrlAsync()
        {
            // get the path stripped of the ssh remote host part
            var tunnel = await GetSshTunnelAsync();
            return SshUri.AccessUrl(tunnel.Loopback, tunnel.ForwardedPort.BoundPort);
        }
        public void UseSshWebSocket(string protocol)
        {
            if (IsWebSocket && IsSshTunnel && (protocol == "http" || protocol == "https" || protocol == "ws" ||
                 protocol == "wss" || string.IsNullOrEmpty(protocol)))
            {
                if (protocol == "http") protocol = "ws";
                else if (protocol == "https") protocol = "wss";
                SshUri.Protocol = protocol;
            }
            else throw new NotSupportedException("TunnelSocket is no WebSocket or invalid protocol");
        }

        public int Port
        {
            get => Uri.Port;
            set => Uri.Port = value;
        }

        public async Task<IPAddress> GetIPAddressAsync() => await DnsService.GetFirstIPAddressAsync(new Uri(url).Host);

        [DataMember]
        public TimeSpan IdleTimeout { get; set; } = TimeSpan.FromMinutes(15);
        [DataMember]
        public TimeSpan ConnectTimeout { get; set; } = TimeSpan.FromSeconds(120); //TimeSpan.FromSeconds(10);
        [DataMember]
        public TimeSpan WriteTimeout { get; set; } = TimeSpan.FromSeconds(120); //TimeSpan.FromSeconds(10);
        [DataMember]
        public int MaxPendingConncections { get; set; } = 20;
        [DataMember]
        public bool CanUpgrade { get; set; } = true;

        [DataMember]
        public List<Cookie> Cookies { get; set; } = new List<Cookie>();
        [DataMember]
        public StringDictionary HttpHeaders { get; private set; } = new StringDictionary();
        [DataMember]
        public byte[] Arguments { get; set; } = null;
        bool AlwaysTrustCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;

        bool validateCertificateSet = false;
        public bool SetRemoteValidationCallback()
        {
            if (IsWebSocket && BaseWebSocket != null && OSInfo.IsCore && !ValidateCertificate && !validateCertificateSet &&
                 IsSecure && BaseWebSocket is ClientWebSocket clientWebSocket)
            {
                // Set BaseWebSocket.Options.RemoteCertificateValidationCallback += AlwaysTrustCertificate with reflection,
                // because this property is not present in NET Standard 2.0
                validateCertificateSet = false;
                var remoteCertificateValidationCallback = typeof(ClientWebSocketOptions).GetProperty("RemoteCertificateValidationCallback");
                if (remoteCertificateValidationCallback != null)
                {
                    var delegateType = Type.GetType("System.Net.Security.RemoteCertificateValidationCallback, System.Net.Security");
                    if (delegateType != null)
                    {
                        var method = GetType().GetMethod(nameof(AlwaysTrustCertificate), BindingFlags.NonPublic | BindingFlags.Instance);

                        var alwaysTrust = Delegate.CreateDelegate(delegateType, this, method);

                        Delegate validateDelegate = (Delegate)remoteCertificateValidationCallback?.GetValue(clientWebSocket.Options);
                        if (validateDelegate == null) validateDelegate = alwaysTrust;
                        else validateDelegate = Delegate.Combine(validateDelegate, alwaysTrust);

                        remoteCertificateValidationCallback?.SetValue(clientWebSocket.Options, validateDelegate);
                        validateCertificateSet = true;
                    }
                }
            }
            return validateCertificateSet;
        }

        public ClientWebSocket GetClientWebSocket()
        {
            try
            {
                var baseWebSocket = new ClientWebSocket();
                foreach (var cookie in Cookies)
                {
                    if (baseWebSocket.Options.Cookies == null) baseWebSocket.Options.Cookies = new CookieContainer();
                    baseWebSocket.Options.Cookies.Add(cookie);
                }
                foreach (DictionaryEntry item in HttpHeaders)
                {
                    baseWebSocket.Options.SetRequestHeader((string)item.Key, (string)item.Value);
                }

                BaseWebSocket = baseWebSocket;

                SetRemoteValidationCallback();

                return baseWebSocket;
            }
            catch (Exception ex)
            {
                throw new IOException(ex.Message, ex);
            }
        }
        async Task InitSocketAsync()
        {
            if (Url == null) throw new ArgumentNullException("Url cannot be null");
            var scheme = Uri.Scheme;
            if (scheme == System.Uri.UriSchemeHttp || scheme == System.Uri.UriSchemeHttps ||
                 scheme == "ws" || scheme == "wss")
            {
                GetClientWebSocket();
            }
            else if (scheme == "ssh")
            {
                if (IsWebSocketOverSsh)
                {
                    GetClientWebSocket();
                }
                else
                {
                    var tunnel = await GetSshTunnelAsync();
                    BaseSocket = new Socket(tunnel.Loopback.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                }
            }
            else if (scheme == "tcp")
            {
                BaseSocket = new Socket((await GetIPAddressAsync()).AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            }
            else if (scheme == "udp")
            {
                BaseSocket = new Socket((await GetIPAddressAsync()).AddressFamily, SocketType.Stream, ProtocolType.Udp);
            }
        }

        public async Task<SshTunnel> GetSshTunnelAsync()
        {
            if (BaseSshTunnel == null)
            {
                if (!Url.StartsWith("ssh://")) throw new ArgumentException("Not a ssh url");
                BaseSshTunnel = new SshTunnel(Url);
                BaseSshTunnel.ConnectTimeout = ConnectTimeout;
                await BaseSshTunnel.CreateAsync();
            }
            return BaseSshTunnel;
        }

        public DateTime LastActivity { get; set; } = DateTime.Now;

        public bool IsFallback
        {
            get => Uri.IsFallback;
            set => Uri.IsFallback = value;
        }
        public bool IsListener
        {
            get => Uri.IsListener;
            set => Uri.IsListener = value;
        }

        public async Task SendAsync(ArraySegment<byte> data, WebSocketMessageType messageType, bool endOfMessage)
        {
            LastActivity = DateTime.Now;
            if (IsSocket) await BaseSocket.SendAsync(data, SocketFlags.None);
            if (IsWebSocket) await BaseWebSocket.SendAsync(data, messageType, endOfMessage, CancellationToken.None);
            LastActivity = DateTime.Now;
        }

        public async Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> data)
        {
            if (IsSocket)
            {
                int n = -1;
                if (BaseSocket.Connected) n = await BaseSocket.ReceiveAsync(data, SocketFlags.None);
                WebSocketCloseStatus? status = null;
                if (n <= 0 && !BaseSocket.Connected)
                {
                    status = WebSocketCloseStatus.EndpointUnavailable;
                }
                LastActivity = DateTime.Now;
                return new WebSocketReceiveResult(n, WebSocketMessageType.Binary, true, status, "");
            }

            if (IsWebSocket)
            {
                var result = await BaseWebSocket.ReceiveAsync(data, new CancellationTokenSource(IdleTimeout).Token);
                LastActivity = DateTime.Now;
                return result;
            }

            throw new ArgumentException("No base socket specified.");
        }

        public async Task CloseAsync(WebSocketCloseStatus status, string statusDescription)
        {
            if (IsSocket && BaseSocket.Connected) BaseSocket.Close();
            if (IsWebSocket && BaseWebSocket.State == WebSocketState.Open || BaseWebSocket.State == WebSocketState.Connecting ||
                 !BaseWebSocket.CloseStatus.HasValue) await BaseWebSocket.CloseAsync(status, statusDescription, CancellationToken.None);
            if (IsSshTunnel) BaseSshTunnel.Dispose();
        }

        public async Task Transmit(TunnelSocket dest)
        {
            if (!dest.IsConnected) await dest.ConnectAsync();

            var destResult = new WebSocketReceiveResult(0, WebSocketMessageType.Binary, true);
            var listenerResult = new WebSocketReceiveResult(0, WebSocketMessageType.Binary, true);

            var Lock = new SemaphoreSlim(1, 1);

            Task closeDestTask = null;
            Exception destException = null, exception = null;

            // listen on the dest tunnel and send to main tunnel on another thread.
            var receivingTask = Task.Run(async () =>
            {
                try
                {
                    WebSocketReceiveResult listenerResultSender, destResultSender;
                    var buffer = new byte[1024 * 4];

                    destResultSender = await dest.ReceiveAsync(new ArraySegment<byte>(buffer));

                    await Lock.WaitAsync();
                    destResult = destResultSender;
                    listenerResultSender = listenerResult;
                    Lock.Release();

                    while (!listenerResultSender.CloseStatus.HasValue && !destResultSender.CloseStatus.HasValue)
                    {
                        if (destResult.Count > 0) await SendAsync(new ArraySegment<byte>(buffer, 0, destResultSender.Count), destResultSender.MessageType, destResultSender.EndOfMessage);

                        destResultSender = await dest.ReceiveAsync(new ArraySegment<byte>(buffer));

                        await Lock.WaitAsync();
                        destResult = destResultSender;
                        listenerResultSender = listenerResult;
                        Lock.Release();
                    }
                }
                catch (Exception ex)
                {
                    await Lock.WaitAsync();
                    destException = ex;
                    Lock.Release();
                }

                Task closeDestTaskSender = null;
                await Lock.WaitAsync();
                try
                {
                    if (dest.IsConnected && closeDestTask == null) closeDestTask = closeDestTaskSender = dest.CloseAsync(destResult.CloseStatus.Value, destResult.CloseStatusDescription);
                }
                finally
                {
                    Lock.Release();
                }

                if (closeDestTaskSender != null) await closeDestTaskSender;
            });

            try
            {
                // listen on the main tunnel and send to the dest tunnel
                WebSocketReceiveResult listenerResultReceiver, destResultReceiver;
                var buffer = new byte[1024 * 4];

                listenerResultReceiver = await ReceiveAsync(new ArraySegment<byte>(buffer));

                await Lock.WaitAsync();
                listenerResult = listenerResultReceiver;
                destResultReceiver = destResult;
                Lock.Release();

                while (!listenerResultReceiver.CloseStatus.HasValue && !destResultReceiver.CloseStatus.HasValue)
                {
                    if (listenerResultReceiver.Count > 0) await dest.SendAsync(new ArraySegment<byte>(buffer, 0, listenerResultReceiver.Count), listenerResultReceiver.MessageType, listenerResultReceiver.EndOfMessage);

                    listenerResultReceiver = await ReceiveAsync(new ArraySegment<byte>(buffer));

                    await Lock.WaitAsync();
                    listenerResult = listenerResultReceiver;
                    destResultReceiver = destResult;
                    Lock.Release();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            WebSocketCloseStatus closeStatus;
            string closeStatusDescription;

            await Lock.WaitAsync();
            try
            {
                closeStatus = listenerResult.CloseStatus ?? destResult.CloseStatus.Value;
                closeStatusDescription = listenerResult.CloseStatus.HasValue ? listenerResult.CloseStatusDescription : destResult.CloseStatusDescription;
            }
            finally
            {
                Lock.Release();
            }

            if (IsConnected) await CloseAsync(closeStatus, closeStatusDescription);
            Task closeDestTaskReceiver = null;
            await Lock.WaitAsync();
            try
            {
                if (dest.IsConnected && closeDestTask == null) closeDestTask = closeDestTaskReceiver = dest.CloseAsync(closeStatus, closeStatusDescription);
                dest.Dispose();
            }
            finally
            {
                Lock.Release();
            }

            await (closeDestTaskReceiver ?? Task.CompletedTask);

            await Lock.WaitAsync();
            try
            {
                if (exception != null) throw new IOException(exception.Message, exception);
                else if (destException != null) throw new IOException(destException.Message, destException);
            }
            finally
            {
                Lock.Release();
            }
        }

        public bool IsConnected
        {
            get
            {
                var tunnelConnected = IsSshTunnel ? BaseSshTunnel.Client.IsConnected && BaseSshTunnel.ForwardedPort.IsStarted : true;
                if (IsSocket) return BaseSocket.Connected && tunnelConnected;
                if (IsWebSocket) return BaseWebSocket?.State == WebSocketState.Open && tunnelConnected;
                return false;
            }
        }

        public virtual async Task<string> ReceiveMessageAsync()
        {
            if (!IsWebSocket) throw new NotSupportedException("ReceiveMessageAsync is only supported on WebSockets");
            const int BufferSize = 1024;
            var buffer = new byte[BufferSize];
            var mem = new MemoryStream();
            WebSocketReceiveResult result = await BaseWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), new CancellationTokenSource(IdleTimeout).Token);
            await mem.WriteAsync(buffer, 0, result.Count);
            while (!result.CloseStatus.HasValue && !result.EndOfMessage && result.MessageType == WebSocketMessageType.Text)
            {
                result = await BaseWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), new CancellationTokenSource(IdleTimeout).Token);
                await mem.WriteAsync(buffer, 0, result.Count);
            }
            if (result.MessageType != WebSocketMessageType.Text)
            {
                throw new NotSupportedException("This WebSocket does not support the fallback protocol");
            }
            return Encoding.UTF8.GetString(mem.ToArray());
        }

        public virtual async Task SendMessageAsync(string message)
        {
            if (!IsWebSocket) throw new NotSupportedException("SendMessageAsync is only supported on WebSockets");
            var buffer = Encoding.UTF8.GetBytes(message);
            await BaseWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public virtual async Task<T> ReceiveObjectAsync<T>(IEnumerable<Type> knownTypes = null) where T : class
        {
            if (!IsWebSocket) throw new NotSupportedException("ReceiveObjectAsync is only supported on WebSockets");
            var mem = await ReceiveData();
            return Deserialize<T>(mem, knownTypes);
        }

        public virtual async Task SendObjectAsync<T>(T obj, IEnumerable<Type> knownTypes = null)
        {
            if (!IsWebSocket) throw new NotSupportedException("SendObjectAsync is only supported on WebSockets");
            var buffer = Serialize<T>(obj, knownTypes);
            await BaseWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Binary, true, CancellationToken.None);
        }

        public static T Deserialize<T>(byte[] data, IEnumerable<Type> knownTypes = null) where T : class
        {
            var mem = new MemoryStream(data);
            return Deserialize<T>(mem, knownTypes);
        }
        public static T Deserialize<T>(Stream stream, IEnumerable<Type> knownTypes = null) where T : class
        {
            if (knownTypes == null) knownTypes = new Type[0];
            var serializer = new DataContractSerializer(typeof(T), knownTypes);
            var reader = XmlDictionaryReader.CreateBinaryReader(stream, new XmlDictionaryReaderQuotas()
            {
                MaxArrayLength = 64 * 1024,
                MaxBytesPerRead = 64 * 1024,
                MaxDepth = 256,
                MaxNameTableCharCount = 2048,
                MaxStringContentLength = 64 * 1024
            });
            return serializer.ReadObject(reader) as T;
        }

        public static void Serialize<T>(T obj, Stream stream, IEnumerable<Type> knownTypes = null)
        {
            if (knownTypes == null) knownTypes = new Type[0];
            var serializer = new DataContractSerializer(typeof(T), knownTypes);
            var writer = XmlDictionaryWriter.CreateBinaryWriter(stream);
            try
            {
                serializer.WriteObject(writer, obj);
                writer.Flush();
            }
            catch (Exception ex)
            {
                throw new SerializationException(ex.Message, ex);
            }
        }

        public static byte[] Serialize<T>(T obj, IEnumerable<Type> knownTypes = null)
        {
            var mem = new MemoryStream();
            Serialize<T>(obj, mem, knownTypes);
            return mem.ToArray();
        }

        public virtual async Task<MemoryStream> ReceiveData()
        {
            if (!IsWebSocket) throw new NotSupportedException("ReceiveData is only supported on WebSockets");
            const int BufferSize = 1024;
            var buffer = new byte[BufferSize];
            var mem = new MemoryStream();
            WebSocketReceiveResult result = await BaseWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), new CancellationTokenSource(IdleTimeout).Token);
            await mem.WriteAsync(buffer, 0, result.Count);
            while (!result.CloseStatus.HasValue && !result.EndOfMessage && result.MessageType == WebSocketMessageType.Binary)
            {
                result = await BaseWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), new CancellationTokenSource(IdleTimeout).Token);
                await mem.WriteAsync(buffer, 0, result.Count);
            }
            if (result.MessageType != WebSocketMessageType.Binary)
            {
                throw new NotSupportedException("This WebSocket does not support the fallback protocol");
            }

            mem.Seek(0, SeekOrigin.Begin);
            return mem;
        }

        public virtual async Task SendData(byte[] data)
        {
            if (!IsWebSocket) throw new NotSupportedException("SendData is only supported on WebSockets");
            await BaseWebSocket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Binary, true, CancellationToken.None);

        }

        const string GetUpgradeTunnelSocketMessage = nameof(GetUpgradeTunnelSocketMessage);
        const string UseUpgradeTunnelSocketMessage = nameof(UseUpgradeTunnelSocketMessage);

        public bool SupportsUpgradeTunnelSocketProtocol { get; set; } = false;

        public TunnelSocket UpgradeTunnelSocket { get; set; } = null;
        public bool HasUpgradeTunnelSocket => UpgradeTunnelSocket != null && UpgradeTunnelSocket != this;

        public async Task UseUpgradeTunnelSocketAsync()
        {
            if (HasUpgradeTunnelSocket)
            {
                try
                {
                    await UpgradeTunnelSocket.ConnectAsync();
                }
                catch (Exception ex)
                {

                }
                if (UpgradeTunnelSocket.IsConnected)
                {
                    // fire and forget CloseAsync to close current connection
                    var closeTask = Clone.CloseAsync(WebSocketCloseStatus.NormalClosure, "Use upgrade tunnel");
                    CopyFrom(UpgradeTunnelSocket);
                    UpgradeTunnelSocket = null;
                }
                else
                {
                    // Tell child fallback TunnelSocket to use it's UpgradeTunnelSocket
                    if (IsFallback)
                    {
                        await SendMessageAsync(UseUpgradeTunnelSocketMessage);
                    }
                }
                UpgradeTunnelSocket = null;
            }
            else
            {
                if (IsFallback)
                {
                    await SendMessageAsync(UseUpgradeTunnelSocketMessage);
                }
            }
        }
        public virtual async Task<TunnelSocket> RequestUpgradeTunnelSocketAsync(bool autoconnect = true)
        {
            if (IsFallback)
            {
                if (autoconnect && !IsConnected) await ConnectAsync(false);

                await SendMessageAsync(GetUpgradeTunnelSocketMessage);
                var upgradeTunnelSocket = await ReceiveObjectAsync<TunnelSocket>();
                if (upgradeTunnelSocket != null && (!CanUpgrade || !upgradeTunnelSocket.CanUpgrade || upgradeTunnelSocket.IsConnected || DnsService.IsHostLoopbackOrUnknown(upgradeTunnelSocket.Uri.Host) &&
                     !DnsService.IsHostLoopback(Uri.Host)))
                {
                    // if upgrade host is a loopback or unknown address we cannot use it as upgrade tunnel
                    upgradeTunnelSocket = null;
                }
                UpgradeTunnelSocket = upgradeTunnelSocket;
            }
            return UpgradeTunnelSocket ?? (CanUpgrade ? this : null);
        }

        public virtual async Task ProvideUpgradeTunnelSocketAsync(TunnelSocket destinationSocket)
        {
            SupportsUpgradeTunnelSocketProtocol = true;

            string message = await ReceiveMessageAsync();
            if (message == GetUpgradeTunnelSocketMessage)
            {

                var upgradeTunnelSocket = await destinationSocket.RequestUpgradeTunnelSocketAsync();

                // Send destination UpgradeTunnelSocket so the client can try connecting to it directly himself
                await SendObjectAsync(upgradeTunnelSocket);

                // Wait for the response
                message = await ReceiveMessageAsync();

                if (message == UseUpgradeTunnelSocketMessage)
                {
                    await destinationSocket.UseUpgradeTunnelSocketAsync();
                }
            }
            else
            {
                // error, close sockets
                await CloseAsync(WebSocketCloseStatus.InvalidMessageType, "This WebSocket does not support the fallback url protocol");
            }
        }

        bool isConnected = false;

        SemaphoreSlim ConnectLock = new SemaphoreSlim(1, 1);
        public bool HasConnectBeenCalled
        {
            get
            {
                ConnectLock.Wait();
                try
                {
                    return isConnected;
                }
                finally
                {
                    ConnectLock.Release();
                }
            }
        }

        public async Task ConnectAsync(bool upgradeWhenAvailable = true)
        {
            await ConnectLock.WaitAsync();
            try
            {
                if (IsConnected || isConnected) return;
                isConnected = true; // only call ConnectAsync once if it's called concurrently
            }
            finally
            {
                ConnectLock.Release();
            }

            try
            {
                await InitSocketAsync();
                if (IsSshTunnel) await GetSshTunnelAsync();
                if (IsSocket)
                {
                    ProtocolType protocol = ProtocolType.Tcp;
                    var url = Url;
                    if (url.StartsWith("tcp://")) protocol = ProtocolType.Tcp;
                    else if (url.StartsWith("udp://")) protocol = ProtocolType.Udp;
                    else throw new NotSupportedException("This url scheme is not supported");
                    var ip = DnsService.GetFirstIPAddress(Uri.Host);

                    if (ip == default) throw new IOException($"Could not resolve host {Uri.Host}");

                    var port = Uri.Port;

                    if (port != 0)
                    {
                        await ConnectAsync(ip, port, protocol);
                    }
                    else
                    {
                        await ListenAsync(ip, protocol);
                    }
                }
                else if (IsWebSocket)
                {
                    if (BaseWebSocket is ClientWebSocket clientWebSocket)
                    {
                        var url = Url;
                        if (IsWebSocketOverSsh) url = await GetSshWebSocketUrlAsync();

                        var setValidationCallback = !ValidateCertificate && !SetRemoteValidationCallback();
                        if (setValidationCallback) ServicePointManager.ServerCertificateValidationCallback += AlwaysTrustCertificate;

                        try
                        {
                            await clientWebSocket.ConnectAsync(new System.Uri(url), new CancellationTokenSource(ConnectTimeout).Token);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        finally
                        {
                            if (setValidationCallback) ServicePointManager.ServerCertificateValidationCallback -= AlwaysTrustCertificate;
                        }
                        if (Arguments != null) await SendData(Arguments);

                        if (IsFallback && upgradeWhenAvailable)
                        {
                            var upgradeTunnel = await RequestUpgradeTunnelSocketAsync(false);
                            await UseUpgradeTunnelSocketAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new IOException(ex.Message, ex);
            }
        }

        public async Task ConnectAsync(IPAddress address, int port, ProtocolType protocolType = ProtocolType.Tcp)
        {
            try
            {
                var socket = BaseSocket;
                if (socket == null || socket.AddressFamily != address.AddressFamily)
                {
                    socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                }
                await socket.ConnectAsync(new IPEndPoint(address, port));
                BaseSocket = socket;
            }
            catch (Exception ex)
            {
                throw new IOException(ex.Message, ex);
            }
        }

        public async Task ConnectAsync(int port, AddressFamily addressFamily)
        {
            if (addressFamily == AddressFamily.InterNetwork) await ConnectAsync(IPAddress.Loopback, port);
            else if (addressFamily == AddressFamily.InterNetworkV6) await ConnectAsync(IPAddress.IPv6Loopback, port);
            throw new NotSupportedException("This addres family is not supported.");
        }

        public async Task<int> ListenAsync(IPAddress address, int port, ProtocolType protocol = ProtocolType.Tcp)
        {
            try
            {
                await ConnectAsync(address, port, protocol);
                BaseSocket.Listen(MaxPendingConncections);
                return port;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> ListenAsync(IPAddress address, ProtocolType protocol = ProtocolType.Tcp)
        {
            const int MinPort = 1024;
            const int MaxPort = UInt16.MaxValue;
            const int Retries = 20;
            int port = 0, n = 0;
            do
            {
                port = new Random().Next(MinPort + 1, MaxPort);
                port = await ListenAsync(address, port, protocol);
                if (port != 0) return port;

            } while (n++ < Retries);

            return 0;
        }
        public async Task<int> ListenAsync()
        {
            var uri = new Uri(url);
            var ip = DnsService.GetFirstIPAddress(uri.Host);

            if (ip == default) throw new IOException($"Cannot resolve host {uri.Host}");

            var port = uri.Port;
            if (port != 0) return await ListenAsync(ip, port);
            else return await ListenAsync(ip);
        }

        bool isDisposed = false;

        public event EventHandler Disposing;
        public void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
                BaseSocket?.Close();
                BaseSocket?.Dispose();
                BaseWebSocket?.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None)
                     .ContinueWith(task => BaseWebSocket?.Dispose());
                BaseSshTunnel?.Disconnect();
                BaseSshTunnel?.Dispose();
                Disposing?.Invoke(this, EventArgs.Empty);
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (!isDisposed)
            {
                isDisposed = true;
                BaseSocket?.Close();
                BaseSocket?.Dispose();
                await BaseWebSocket?.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                BaseWebSocket?.Dispose();
                BaseSshTunnel?.Disconnect();
                BaseSshTunnel?.Dispose();
                Disposing?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
