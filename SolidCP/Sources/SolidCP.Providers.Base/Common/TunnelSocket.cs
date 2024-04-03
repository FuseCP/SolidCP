using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Net;

namespace SolidCP.Providers
{
    public class TunnelSocket
    {
        public Socket BaseSocket { get; set; }
        public WebSocket BaseWebSocket { get; set; }

        public TunnelSocket(Socket socket) => BaseSocket = socket;
        public TunnelSocket(WebSocket socket) => BaseWebSocket = socket;
        public TunnelSocket(IPAddress address) : this(new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp)) { }

        public bool IsWebSocket => BaseWebSocket != null;
        public bool IsSocket => BaseSocket != null;
        public AddressFamily AddressFamily { get; set; }
        public DateTime LastActivity { get; set; }

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
                var result = await BaseWebSocket.ReceiveAsync(data, CancellationToken.None);
                LastActivity = DateTime.Now;
                return result;
            }

            throw new ArgumentException("No base socket specified.");
        }

        public async Task CloseAsync(WebSocketCloseStatus status, string statusDescription)
        {
            if (IsSocket) BaseSocket.Close();
            if (IsWebSocket) await BaseWebSocket.CloseAsync(status, statusDescription, CancellationToken.None);
        }

        public async Task Transmit(TunnelSocket dest)
        {
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

        public async Task ConnectAsync(IPAddress address, int port)
        {
            AddressFamily = AddressFamily;
            var socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
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
                BaseSocket.Listen(20);
                return port;
            } catch (Exception ex)
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
            AddressFamily = address.AddressFamily;
            do
            {
                port = new Random().Next(MinPort + 1, MaxPort);
                port = await ListenAsync(address, port);
                if (port != -1) return port;

            } while (n++ < Retries);
           
            return -1;
        }
    }
}
