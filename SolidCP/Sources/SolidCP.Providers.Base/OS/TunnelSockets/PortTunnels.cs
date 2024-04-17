using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidCP.Providers.OS
{
    public class TunnelApplication
    {
        public string Name;
        public IPAddress Client;
    }

    public class ListeningTask
    {
        // CancellationToken Cancel = CancellationToken.None;
        public Task Task;
        public TunnelSocket Incoming, Tunnel;
        public int ListeningPort = -1;
        public DateTime LastActivity => Incoming.LastActivity;
    }

    /// <summary>
    /// Used to forward a local port on the SolidCP.WebPanel server to a port on a server where SolidCP.Server is running.
    /// The forwarding is done over WebSockets.
    /// </summary>
    public class PortTunnels
    {
        public readonly static ConcurrentDictionary<TunnelApplication, Task<ListeningTask>> Listeners = new ConcurrentDictionary<TunnelApplication, Task<ListeningTask>>();
        public static async Task<ListeningTask> Listen(string applicationName, IPAddress clientAddress, Task<TunnelSocket> tunnel)
        {
            var application = new TunnelApplication()
            {
                Name = applicationName,
                Client = clientAddress
            };
            return await Listeners.GetOrAdd(application, async app =>
            {
                var incoming = new TunnelSocket(clientAddress);
                var port = await incoming.ListenAsync(clientAddress);
                var task = Task.Factory.StartNew(async () => await incoming.Transmit(await tunnel));
                var listeningTask = new ListeningTask()
                {
                    Incoming = incoming,
                    ListeningPort = port,
                    Tunnel = await tunnel,
                    Task = task
                };
                return listeningTask;
            });
        }
    }
}
