using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SolidCP.Providers.OS
{
    /// <summary>
    /// Basic DNS services
    /// </summary>
    public class DnsService
    {
        static ConcurrentDictionary<string, Task<IPAddress[]>> ipforhost = new ConcurrentDictionary<string, Task<IPAddress[]>>();
        public static async Task<IPAddress> GetFirstIPAddressAsync(string host) => (await GetIPAddressesAsync(host))
                .OrderBy(ip => ip.AddressFamily) // Get IPv4 address first
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork || ip.AddressFamily == AddressFamily.InterNetworkV6);

        public static async Task<IPAddress> GetFirstIPV4AddressAsync(string host) => (await GetIPAddressesAsync(host))
             .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        public static async Task<IPAddress> GetFirstIPV6AddressAsync(string host) => (await GetIPAddressesAsync(host))
             .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetworkV6);
        public static async Task<IPAddress[]> GetIPAddressesAsync(string host)
        {
            IPAddress ip;
            if (!IPAddress.TryParse(host, out ip))
            {
                var ips = await ipforhost.GetOrAdd(host, async host =>
                {
                    var dnsips = await Dns.GetHostAddressesAsync(host);
                    //if (dnsips.Length == 0) throw new ArgumentException($"Could not resolve ip for {host}");
                    return dnsips;
                });
                return ips;
            }
            return new IPAddress[] { ip };
        }

        public static IPAddress[] GetIPAddresses(string host)
        {
            IPAddress ip;
            if (!IPAddress.TryParse(host, out ip))
            {
                var ipsTask = ipforhost.GetOrAdd(host, host =>
                {
                    var dnsips = Dns.GetHostAddresses(host);
                    //if (dnsips.Length == 0) throw new ArgumentException($"Could not resolve ip for {host}");
                    return Task.FromResult(dnsips);
                });
                // avoid deadlock on ipsTask.Result
                if (ipsTask.IsCompleted) return ipsTask.Result;
                else
                {
                    var dnsips = Dns.GetHostAddresses(host);
                    //if (dnsips.Length == 0) throw new ArgumentException($"Could not resolve ip for {host}");
                    ipsTask = Task.FromResult(dnsips);
                    ipforhost.AddOrUpdate(host, ipsTask, (host, ips) => ipsTask);
                    return dnsips;
                }
            }
            return new IPAddress[] { ip };
        }

        public static IPAddress GetFirstIPAddress(string host) => GetIPAddresses(host)
             .OrderBy(ip => ip.AddressFamily) // Get IPv4 address first
             .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork || ip.AddressFamily == AddressFamily.InterNetworkV6);

        public static IPAddress GetFirstIPV4Address(string host) => GetIPAddresses(host)
             .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        public static IPAddress GetFirstIPV6Address(string host) => GetIPAddresses(host)
             .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetworkV6);

        public static bool IsHostLoopback(string host)
        {
            if (string.IsNullOrEmpty(host)) return true;

            IPAddress ip;
            var isHostIP = IPAddress.TryParse(host, out ip);
            isHostIP = isHostIP && (ip.AddressFamily == AddressFamily.InterNetwork || ip.AddressFamily == AddressFamily.InterNetworkV6);
            if (host == "localhost" || isHostIP && IPAddress.IsLoopback(ip)) return true;

            if (!isHostIP)
            {
                IPAddress[] ips = GetIPAddresses(host);
                return ips
                     .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork || ip.AddressFamily == AddressFamily.InterNetworkV6)
                     .Any(ip => IPAddress.IsLoopback(ip));
            }
            return false;
        }

        public static bool IsHostLoopbackOrUnknown(string host)
        {
            if (string.IsNullOrEmpty(host)) return true;

            IPAddress ip;
            var isHostIP = IPAddress.TryParse(host, out ip);
            isHostIP = isHostIP && (ip.AddressFamily == AddressFamily.InterNetwork || ip.AddressFamily == AddressFamily.InterNetworkV6);
            if (host == "localhost" || isHostIP && IPAddress.IsLoopback(ip)) return true;

            if (!isHostIP)
            {
                IPAddress[] ips = GetIPAddresses(host);
                return !ips.Any() || ips
                     .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork || ip.AddressFamily == AddressFamily.InterNetworkV6)
                     .Any(ip => IPAddress.IsLoopback(ip));
            }
            return false;
        }

        public static bool IsHostIpV4(string host)
        {
            if (host == null) return true;

            IPAddress ip;
            var isHostIP = IPAddress.TryParse(host, out ip);
            if (isHostIP && ip.AddressFamily == AddressFamily.InterNetwork) return true;
            IPAddress[] ips = GetIPAddresses(host);
            return ips.Any(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }

        public static bool IsLANAddress(string adr)
        {
            return Regex.IsMatch(adr, @"(^127\.)|(^192\.168\.)|(^10\.)|(^172\.1[6-9]\.)|(^172\.2[0-9]\.)|(^172\.3[0-1]\.)|(^\[?::1\]?$)|(^\[?[fF][cCdD])", RegexOptions.Singleline);
        }

        public static bool IsHostLAN(string host)
        {
            if (host == null) return true;

            IPAddress hostip;
            var isHostIP = IPAddress.TryParse(host, out hostip);
            isHostIP = isHostIP && (hostip.AddressFamily == AddressFamily.InterNetwork || hostip.AddressFamily == AddressFamily.InterNetworkV6);
            if (host == "localhost" || host.StartsWith("127.0.0.") && isHostIP || host == "::1" || host == "[::1]" ||
                 isHostIP && IsLANAddress(host)) return true;

            if (!isHostIP)
            {
                IPAddress[] ips = GetIPAddresses(host);
                return ips
                     .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork || ip.AddressFamily == AddressFamily.InterNetworkV6)
                     .All(ip => IsLANAddress(ip.ToString()));
            }
            return false;
        }
    }
}
