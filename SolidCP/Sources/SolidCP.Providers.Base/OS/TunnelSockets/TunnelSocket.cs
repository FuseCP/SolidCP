using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Net;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Compat.Runtime.Serialization;
using Renci.SshNet;

namespace SolidCP.Providers.OS
{
    public class TunnelSocket : IDisposable, IAsyncDisposable
    {
        [XmlIgnore, IgnoreDataMember]
        public Socket BaseSocket { get; set; }
        [XmlIgnore, IgnoreDataMember]
        public WebSocket BaseWebSocket { get; set; }
        [XmlIgnore, IgnoreDataMember]
        public SshTunnel BaseSshTunnel { get; set; }
        public TunnelSocket TunnelOtherEnd { get; set; } = null;

        public TunnelSocket() { }
        public TunnelSocket(Socket socket) : this()
        {
            BaseSocket = socket;
            url = "";
        }

        public TunnelSocket(WebSocket socket) : this()
        {
            BaseWebSocket = socket;
            url = "";
        }
        public TunnelSocket(IPAddress address) : this(new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
        {
            url = $"tcp://{address}";
        }
        public TunnelSocket(IPAddress address, int port) : this(new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
        {
            url = $"tcp://{address}:{port}";
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
                foreach (var header in HttpHeaders) clone.HttpHeaders.Add(header.Key, header.Value);
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
            foreach (var header in from.HttpHeaders) HttpHeaders.Add(header.Key, header.Value);
        }

        public bool IsWebSocket => url.StartsWith("http://") || url.StartsWith("https://") || BaseWebSocket != null;
        public bool IsSocket => url.StartsWith("tcp://") || url.StartsWith("udp://") || BaseSocket != null;
        public bool IsSshTunnel => url.StartsWith("ssh://") || BaseSshTunnel != null;

        string url = null;
        public string Url
        {
            get => url;
            set
            {
                if (url != value)
                {
                    url = value;
                    var uri = new Uri(url ?? "");
                    var query = uri.Query;
                    port = uri.Port;
                    isFallback = Regex.IsMatch(url ?? "", @"(?<=\?.*?)(?:(?<=\?)|&)tunnel=fallback(?:&|$)");
                    isListener = Regex.IsMatch(url ?? "", @"(?<=\?.*?)(?:(?<=\?)|&)tunnel=listener(?:&|$)");
                    var str = new StringBuilder();
                    str.Append(url ?? "");
                    if (isFallback)
                    {
                        if (string.IsNullOrEmpty(query)) str.Append("?");
                        else str.Append("&");
                        str.Append("tunnel=fallback");
                    }
                    if (isListener)
                    {
                        str.Append("&");
                        str.Append("tunnel=listener");
                    }
                    url = str.ToString();
                }
            }
        }
        public string RawUrl => Regex.Replace(Url ?? "", @"(?<=\?.*?)(?:(?<=\?)|&)(?:tunnel=listener|tunnel=fallback)(?=&|$)", "");

        int port = -1;
        [XmlIgnore, IgnoreDataMember]
        public int Port
        {
            get => port;
            set
            {
                if (port != value)
                {
                    port = value;
                    var uri = new Uri(url ?? "");
                    url = $"{uri.Scheme}://{(!string.IsNullOrEmpty(uri.UserInfo) ? $"{uri.UserInfo}@" : "")}{uri.Host}{port}{uri.PathAndQuery}";
                }
            }
        }

        public async Task<IPAddress> GetIPAddressAsync() => await DnsService.GetFirstIPAddressAsync(new Uri(url).Host);

        public TimeSpan IdleTimeout { get; set; } = TimeSpan.FromMinutes(15);
        public TimeSpan ConnectTimeout { get; set; } = TimeSpan.FromSeconds(60);
        public TimeSpan WriteTimeout { get; set; } = TimeSpan.FromSeconds(60);
        public int MaxPendingConncections { get; set; } = 20;
        public List<Cookie> Cookies { get; set; }
        public Dictionary<string, string> HttpHeaders { get; private set; } = new Dictionary<string, string>();

        async Task InitSocketAsync()
        {
            if (url == null) throw new ArgumentNullException("Url cannot be null");
            if (url != Url)
            {
                var uri = new Uri(RawUrl);
                if (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
                {
                    var baseWebSocket = new ClientWebSocket();
                    BaseWebSocket = baseWebSocket;
                    foreach (var cookie in Cookies) baseWebSocket.Options.Cookies.Add(cookie);
                    foreach (var item in HttpHeaders)
                    {
                        baseWebSocket.Options.SetRequestHeader(item.Key, item.Value);
                    }
                }
                else if (uri.Scheme == "tcp" || uri.Scheme == "ssh")
                {
                    BaseSocket = new Socket((await GetIPAddressAsync()).AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                }
                else if (uri.Scheme == "udp")
                {
                    BaseSocket = new Socket((await GetIPAddressAsync()).AddressFamily, SocketType.Stream, ProtocolType.Udp);
                }
            }
        }

        public async Task<SshTunnel> GetSshTunnelAsync()
        {
            if (!Url.StartsWith("ssh://")) throw new ArgumentException("Not a ssh url");
            BaseSshTunnel = new SshTunnel(Url);
            BaseSshTunnel.ConnectTimeout = ConnectTimeout;
            await BaseSshTunnel.CreateAsync();
            return BaseSshTunnel;
        }

        [XmlIgnore, IgnoreDataMember]
        public DateTime LastActivity { get; set; } = DateTime.Now;

        bool isFallback, isListener;
        public bool IsFallback
        {
            get => isFallback;
            set
            {
                if (isFallback == value) return;

                isFallback = value && IsWebSocket;
                if (!isFallback)
                {
                    url = Regex.Replace(url ?? "", @"(?<=\?.*?)(?:(?<=\?)|&)tunnel=fallback(?=&|$)", "");
                }
                else if (!Regex.IsMatch(url, @"(?<=\?.*?)(?:(?<=\?)|&)tunnel=fallback(?:&|$)"))
                {
                    var str = new StringBuilder(url);
                    if (!(url.EndsWith("?") || url.EndsWith("&")))
                    {
                        if (url.Contains('?')) str.Append("&");
                        else str.Append("?");
                    }
                    str.Append("tunnel=fallback");
                }

            }
        }
        public bool IsListener
        {
            get => isListener;
            set
            {
                if (isListener == value) return;

                isListener = value && IsSocket;
                if (!isListener)
                {
                    url = Regex.Replace(url ?? "", @"(?<=\?.*?)(?:(?<=\?)|&)tunnel=listener(?=&|$)", "");
                }
                else if (!Regex.IsMatch(url, @"(?<=\?.*?)(?:(?<=\?)|&)tunnel=listener(?:&|$)"))
                {
                    var str = new StringBuilder(url);
                    if (!(url.EndsWith("?") || url.EndsWith("&")))
                    {
                        if (url.Contains('?')) str.Append("&");
                        else str.Append("?");
                    }
                    str.Append("tunnel=listener");
                }
            }
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
            if (IsSocket) BaseSocket.Close();
            if (IsWebSocket) await BaseWebSocket.CloseAsync(status, statusDescription, CancellationToken.None);
            if (IsSshTunnel) BaseSshTunnel.Dispose();
            if (TunnelOtherEnd != null) await TunnelOtherEnd.CloseAsync(status, statusDescription);
        }

        public async Task Transmit(TunnelSocket dest)
        {
            if (!dest.IsConnected) await dest.ConnectAsync();

            var buffer = new byte[1024 * 4];
            var result = await ReceiveAsync(new ArraySegment<byte>(buffer));
            WebSocketReceiveResult destResult = new WebSocketReceiveResult(0, WebSocketMessageType.Binary, true);

            var receivingTask = Task.Run(async () =>
            {
                destResult = await dest.ReceiveAsync(new ArraySegment<byte>(buffer));

                while (!result.CloseStatus.HasValue && !destResult.CloseStatus.HasValue)
                {
                    if (destResult.Count > 0) await SendAsync(new ArraySegment<byte>(buffer, 0, destResult.Count), destResult.MessageType, destResult.EndOfMessage);
                }

                await dest.CloseAsync(destResult.CloseStatus.Value, destResult.CloseStatusDescription);
            });

            while (!result.CloseStatus.HasValue && !destResult.CloseStatus.HasValue)
            {
                if (result.MessageType == WebSocketMessageType.Binary)
                {
                    // forward data to PveSocket
                    await dest.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage);
                }
                result = await ReceiveAsync(new ArraySegment<byte>(buffer));
            }

            await CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription);
        }

        public bool IsConnected
        {
            get
            {
                if (IsSocket) return BaseSocket.Connected;
                if (IsWebSocket) return BaseWebSocket.State == WebSocketState.Open;
                throw new NotSupportedException("IsConnected not supported on this TunnelSocket");
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
            if (!result.EndOfMessage || result.MessageType != WebSocketMessageType.Text)
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

        public virtual async Task<T> ReceiveObjectAsync<T>()
        {
            if (!IsWebSocket) throw new NotSupportedException("ReceiveObjectAsync is only supported on WebSockets");
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
            if (!result.EndOfMessage || result.MessageType != WebSocketMessageType.Text)
            {
                throw new NotSupportedException("This WebSocket does not support the fallback protocol");
            }
            mem.Seek(0, SeekOrigin.Begin);
            var serializer = new NetDataContractSerializer();
            return (T)serializer.Deserialize(mem);
        }

        public virtual async Task SendObjectAsync<T>(T obj)
        {
            if (!IsWebSocket) throw new NotSupportedException("SendObjectAsync is only supported on WebSockets");
            var mem = new MemoryStream();
            var serializer = new NetDataContractSerializer();
            serializer.Serialize(mem, obj);
            var buffer = mem.ToArray();
            await BaseWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }


        const string GetUpgradeTunnelSocketMessage = nameof(GetUpgradeTunnelSocketMessage);
        const string UseUpgradeTunnelSocketMessage = nameof(UseUpgradeTunnelSocketMessage);

        [XmlIgnore, IgnoreDataMember]
        public bool SupportsUpgradeTunnelSocketProtocol { get; set; } = false;

        [XmlIgnore, IgnoreDataMember]
        public TunnelSocket UpgradeTunnelSocket { get; set; } = null;
        public bool HasUpgradeTunnelSocket => UpgradeTunnelSocket != null;

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
                    var closeTask = Clone.CloseAsync(WebSocketCloseStatus.NormalClosure, "Use new TunnelSocket");
                    CopyFrom(UpgradeTunnelSocket);
                    UpgradeTunnelSocket = null;
                }
                else
                {
                    // Tell child fallback TunnelSocket to use ServerTunnelSocketUrl
                    if (TunnelOtherEnd != null && TunnelOtherEnd.IsFallback)
                    {
                        await TunnelOtherEnd.SendMessageAsync(UseUpgradeTunnelSocketMessage);
                    }
                }
                UpgradeTunnelSocket = null;
            }
        }
        public virtual async Task<TunnelSocket> RequestUpgradeTunnelSocketAsync(bool autoconnect = true)
        {
            if (IsFallback)
            {
                if (autoconnect && !IsConnected) await ConnectAsync();

                await SendMessageAsync(GetUpgradeTunnelSocketMessage);
                UpgradeTunnelSocket = await ReceiveObjectAsync<TunnelSocket>();
            }
            return UpgradeTunnelSocket ?? this;
        }

        public virtual async Task ProvideUpgradeTunnelSocketAsync(TunnelSocket destinationSocket)
        {
            SupportsUpgradeTunnelSocketProtocol = true;

            TunnelOtherEnd = destinationSocket;

            string message = await ReceiveMessageAsync();
            if (message == GetUpgradeTunnelSocketMessage)
            {
                if (!destinationSocket.IsConnected) await destinationSocket.ConnectAsync();

                var upgradeTunnel = await destinationSocket.RequestUpgradeTunnelSocketAsync();
                // Send destination UpgradeTunnelSocket so the client can try connecting to it directly himself
                await SendObjectAsync(upgradeTunnel);
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
                await BaseWebSocket.CloseAsync(WebSocketCloseStatus.InvalidMessageType, "This WebSocket does not support the fallback url protocol", CancellationToken.None);
                await destinationSocket.CloseAsync(WebSocketCloseStatus.InvalidMessageType, "This WebSocket does not support the fallback url protocol");
            }
        }
        public async Task ConnectAsync()
        {
            if (IsConnected) return;

            await InitSocketAsync();
            if (IsSshTunnel) await GetSshTunnelAsync();
            if (IsSocket)
            {
                ProtocolType protocol;
                if (url.StartsWith("tcp://")) protocol = ProtocolType.Tcp;
                else if (url.StartsWith("udp://")) protocol = ProtocolType.Udp;
                else throw new NotSupportedException("This url scheme is not supported");
                var uri = new Uri(url);
                var ip = DnsService.GetFirstIPAddress(uri.Host);
                var port = uri.Port;

                if (port != 0)
                {
                    await ConnectAsync(ip, port);
                }
                else
                {
                    await ListenAsync(ip);
                }
            }
            else if (IsWebSocket)
            {
                if (BaseWebSocket is ClientWebSocket clientWebSocket)
                {
                    await clientWebSocket.ConnectAsync(new Uri(Url), new CancellationTokenSource(ConnectTimeout).Token);

                    if (IsFallback) await RequestUpgradeTunnelSocketAsync(false);
                }
            }
        }

        public async Task ConnectAsync(IPAddress address, int port)
        {
            var socket = BaseSocket;
            if (socket == null || socket.AddressFamily != address.AddressFamily)
            {
                socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            }
            await socket.ConnectAsync(new IPEndPoint(address, port));
            BaseSocket = socket;
        }

        public async Task ConnectAsync(int port, AddressFamily addressFamily)
        {
            if (addressFamily == AddressFamily.InterNetwork) await ConnectAsync(IPAddress.Loopback, port);
            else if (addressFamily == AddressFamily.InterNetworkV6) await ConnectAsync(IPAddress.IPv6Loopback, port);
            throw new NotSupportedException("This addres family is not supported.");
        }

        public async Task<int> ListenAsync(IPAddress address, int port)
        {
            try
            {
                await ConnectAsync(address, port);
                BaseSocket.Listen(MaxPendingConncections);
                return port;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> ListenAsync(IPAddress address)
        {
            const int MinPort = 1024;
            const int MaxPort = UInt16.MaxValue;
            const int Retries = 20;
            int port = 0, n = 0;
            do
            {
                port = new Random().Next(MinPort + 1, MaxPort);
                port = await ListenAsync(address, port);
                if (port != -1) return port;

            } while (n++ < Retries);

            return 0;
        }
        public async Task<int> ListenAsync()
        {
            var uri = new Uri(url);
            var ip = DnsService.GetFirstIPAddress(uri.Host);
            var port = uri.Port;
            if (port != null) return await ListenAsync(ip, port);
            else return await ListenAsync(ip);
        }

        bool isDisposed = false;
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
            }
        }
    }
}
