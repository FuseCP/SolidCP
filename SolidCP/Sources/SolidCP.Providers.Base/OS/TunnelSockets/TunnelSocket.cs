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
using Compat.Runtime.Serialization;
using Renci.SshNet;

namespace SolidCP.Providers.OS
{
    public class TunnelSocket : IDisposable, IAsyncDisposable
    {

        public Socket BaseSocket { get; set; }
        public WebSocket BaseWebSocket { get; set; }
        public SshTunnel BaseSshTunnel { get; set; }

        public TunnelSocket() { }
        public TunnelSocket(Socket socket) : this() => BaseSocket = socket;
        public TunnelSocket(WebSocket socket) : this() => BaseWebSocket = socket;
        public TunnelSocket(IPAddress address) : this(new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
        {
            url = $"tcp://{address}";
        }
        public TunnelSocket(IPAddress address, int port) : this(new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
        {
            url = $"tcp://{address}:{port}";
        }
        public TunnelSocket(string url): this() => Url = url;

        public bool IsWebSocket => BaseWebSocket != null;
        public bool IsSocket => BaseSocket != null;
        public bool IsSshTunnel => url.StartsWith("ssh://");

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
                        if (string.IsNullOrEmpty(query) && !isFallback) str.Append("?");
                        else str.Append("&");
                        str.Append("tunnel=listener");
                    }
                    url = str.ToString();
                }
            }
        }
        public string RawUrl => Regex.Replace(Url ?? "", @"(?<=\?.*?)(?:(?<=\?)|&)(?:tunnel=listener|tunnel=fallback)(?=&|$)", "");

        int port = -1;
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
        public CookieContainer Cookies { get; set; }
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
                    baseWebSocket.Options.Cookies = Cookies;
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

        public ForwardedPort BaseForwardedPort { get; set; } = null;
        public async Task<SshTunnel> GetSshTunnelAsync()
        {
            if (!Url.StartsWith("ssh://")) throw new ArgumentException("Not a ssh url");
            BaseSshTunnel = new SshTunnel(Url);
            BaseSshTunnel.ConnectTimeout = ConnectTimeout;
            await BaseSshTunnel.CreateAsync();
            return BaseSshTunnel;
        }

        public DateTime LastActivity { get; set; }
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
        }

        public async Task Transmit(TunnelSocket dest)
        {
            // handshake if dest is fallback TunnelSocket


            var buffer = new byte[1024 * 4];
            var result = await ReceiveAsync(new ArraySegment<byte>(buffer));
            WebSocketReceiveResult destResult = new WebSocketReceiveResult(0, WebSocketMessageType.Binary, true);

            Task.Run(async () =>
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

        const string FallbackMessgae = "GetServerTunnelSocketUrl";
        public virtual async Task<string> RequestServerTunnelSocketUrlAsync()
        {
            const int BufferSize = 4 * 1024;
            BaseWebSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(FallbackMessgae)), WebSocketMessageType.Text, true, CancellationToken.None);
            var mem = new MemoryStream();
            var buffer = new byte[BufferSize];
            WebSocketReceiveResult result = await BaseWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), new CancellationTokenSource(IdleTimeout).Token));
            while (!result.CloseStatus.HasValue && !result.EndOfMessage && result.MessageType == WebSocketMessageType.Text)
            {
                await mem.WriteAsync(buffer, 0, result.Count);
                result = await BaseWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), new CancellationTokenSource(IdleTimeout).Token));
            }
            if (!result.EndOfMessage || result.MessageType != WebSocketMessageType.Text)
            {
                await BaseWebSocket.CloseAsync(WebSocketCloseStatus.InvalidMessageType, "This WebSocket does not support the fallback url protocol", CancellationToken.None);
            }
            return Encoding.UTF8.GetString(mem.ToArray());
        }

        public virtual async Task ProvideServerTunnelSocketUrlAsync(TunnelSocket destinationSocket)
        {
            const int BufferSize = 1024;
            var buffer = new byte[BufferSize];
            var mem = new MemoryStream();
            WebSocketReceiveResult result = await BaseWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), new CancellationTokenSource(IdleTimeout).Token));
            await mem.WriteAsync(buffer, 0, result.Count);
            while (!result.CloseStatus.HasValue && !result.EndOfMessage && result.MessageType == WebSocketMessageType.Text)
            {
                result = await BaseWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), new CancellationTokenSource(IdleTimeout).Token));
                await mem.WriteAsync(buffer, 0, result.Count);
            }
            string message = Encoding.UTF8.GetString(mem.ToArray());
            if (message == FallbackMessgae)
            {
                // Send destination socket's url so the client can try opening directly himself
                buffer = Encoding.UTF8.GetBytes(destinationSocket.Url);
                await BaseWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            } else {
                await BaseWebSocket.CloseAsync(WebSocketCloseStatus.InvalidMessageType, "This WebSocket does not support the fallback url protocol", CancellationToken.None);
            }
        }
        public async Task ConnectAsync()
        {
            await InitSocketAsync();
            if (IsSshTunnel) await GetSshTunnelAsync();
            if (IsSocket) {
                ProtocolType protocol;
                if (url.StartsWith("tcp://")) protocol = ProtocolType.Tcp;
                else if (url.StartsWith("udp://")) protocol = ProtocolType.Udp;
                AutoResetEvent finished = new AutoResetEvent(false);
                var args = new SocketAsyncEventArgs();
                args.Completed += (sender, arguments) => finished.Set();
                if (!BaseSocket.ConnectAsync(args)) return;
                finished.WaitOne(ConnectTimeout);
            } else if (IsWebSocket)
            {
                if (BaseWebSocket is ClientWebSocket clientWebSocket)
                {
                    await clientWebSocket.ConnectAsync(new Uri(Url), new CancellationTokenSource(ConnectTimeout).Token);
                    if (IsFallback)
                    {
                        var url = await RequestServerTunnelSocketUrlAsync();
                    }
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
                return -1;
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

            return -1;
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
