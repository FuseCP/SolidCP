using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Net;
using System.Net.WebSockets;
using System.Reflection;
using System.Threading;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Xml;
using System.Threading.Tasks;
using SolidCP.Providers.Virtualization;

namespace SolidCP.Providers.OS
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class TunnelClientAttribute : Attribute
    {
        public Type Client { get; set; }
        public TunnelClient instance = null;
        public TunnelClient Instance => instance ?? (instance = (TunnelClient)Activator.CreateInstance(Client));

        public TunnelClientAttribute(Type client) { Client = client; }
    }

    public class TunnelClient
    {
        public bool IsServerLoaded => AppDomain.CurrentDomain.GetAssemblies()
            .Any(a => a.GetName().Name == "SolidCP.Server");
        public bool IsEnterpriseServerLoaded => AppDomain.CurrentDomain.GetAssemblies()
            .Any(a => a.GetName().Name == "SolidCP.EnterpriseServer");
        public bool IsPortalLoaded => AppDomain.CurrentDomain.GetAssemblies()
            .Any(a => a.GetName().Name == "SolidCP.WebPortal");

        public ServerTunnelClientBase ServerClient => AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.GetName().Name == "SolidCP.Server.Client")
            .SelectMany(a => a.GetCustomAttributes(typeof(TunnelClientAttribute), false))
            .OfType<TunnelClientAttribute>()
            .Select(a => (ServerTunnelClientBase)CopyTo(a.Instance))
            .FirstOrDefault();

        public EnterpriseServerTunnelClientBase EnterpriseServerClient => AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.GetName().Name == "SolidCP.EnterpriseServer.Client")
            .SelectMany(a => a.GetCustomAttributes(typeof(TunnelClientAttribute), false))
            .OfType<TunnelClientAttribute>()
            .Select(a => (EnterpriseServerTunnelClientBase)CopyTo(a.Instance))
            .FirstOrDefault();

        public TunnelClient Client
        {
            get
            {
                if (ServerUrl == "assembly://SolidCP.Server")
                {
                    if (!IsServerLoaded) Assembly.Load("SolidCP.Server");
                    return ServerClient;
                }
                else if (ServerUrl == "assembly://SolidCP.EnterpriseServer")
                {
                    if (!IsEnterpriseServerLoaded) Assembly.Load("SolidCP.EnterpriseServer");
                    return EnterpriseServerClient;
                }
                throw new NotSupportedException("Unknown assembly in AssemblyBinding.");
            }
        }

        public virtual string CryptoKey => "";
        IEnumerable<string> TypeNames(IEnumerable<Type> types) => types?.Select(type => type?.FullName);

        void WriteStrings(BinaryWriter writer, IEnumerable<string?> strings)
        {
            if (strings != null)
            {
                foreach (var str in strings)
                {
                    writer.Write(str ?? "!");
                }
            }
            writer.Write("");
        }
        void WriteTypes(BinaryWriter writer, IEnumerable<Type> types) => WriteStrings(writer, TypeNames(types));

        public byte[] Serialize(string methodName, bool encrypted, params object[] args)
        {
            var mem = new MemoryStream();
            if (encrypted) mem.WriteByte(1);
            else mem.WriteByte(0);

            var types = args.Select(arg => arg?.GetType())
                .ToArray();
            var methods = GetType().GetMethods();
            var method = methods
                .Select(m => new
                {
                    Method = m,
                    ParameterTypes = m.GetParameters()
                    .Select(p => p.ParameterType)
                    .ToArray()
                })
                .FirstOrDefault(m =>
                {
                    if (m.Method.Name != methodName) return false;

                    if (m.ParameterTypes.Length != types.Length) return false;

                    for (int i = 0; i < m.ParameterTypes.Length; i++)
                    {
                        if (types[i] != null && m.ParameterTypes[i] != types[i] && !types[i].IsSubclassOf(m.ParameterTypes[i])) return false;
                    }
                    return true;
                });
            if (method == null) throw new ArgumentException($"Could not find method {methodName} with correct parameters");

            using (var binaryWriter = new BinaryWriter(mem, Encoding.UTF8, true))
            {
                WriteTypes(binaryWriter, method.ParameterTypes);
                WriteTypes(binaryWriter, types);
                binaryWriter.Flush();
            }
            var knownTypes = method.ParameterTypes
                .Concat(types)
                .Concat(new Type[] { typeof(string), typeof(long) })
                .Where(type => type != null)
                .Distinct();
            var argsWithCredentials = new object[] { DateTime.Now.Ticks, Username, Password }
                .Concat(args)
                .ToArray();

            using (var stream = encrypted ? new Cryptor(CryptoKey).EncryptorStream(mem) : mem)
            {
                TunnelSocket.Serialize<object[]>(argsWithCredentials, stream, knownTypes);

                if (stream is CryptoStream crStream) crStream.FlushFinalBlock();
                else stream.Flush();

                return mem.ToArray();
            }
        }

        public virtual string ServerUrl { get; set; }

        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public TunnelClient CopyTo(TunnelClient copy)
        {
            copy.ServerUrl = ServerUrl;
            copy.Username = Username;
            copy.Password = Password;
            return copy;
        }
        public CancellationToken Cancel { get; set; }
        public bool IsAssemblyBinding => ServerUrl?.StartsWith("assembly://") ?? false;
        public bool IsSecure => (ServerUrl?.StartsWith("https://") ?? false) || (ServerUrl?.StartsWith("ssh://") ?? false) ||
            DnsService.IsHostLAN(new Uri(ServerUrl ?? "").Host);

        public virtual async Task<TunnelSocket> GetTunnel(string method, params object[] args)
        {
            if (!IsAssemblyBinding)
            {
                var url = $"{ServerUrl}/Tunnel";
                var tunnel = new TunnelSocket(url);
                var scheme = tunnel.Uri.Scheme;
                if (scheme == "https") tunnel.Uri.Scheme = "wss";
                else if (scheme == "http") tunnel.Uri.Scheme = "ws";
                else if (scheme != "ssh" && scheme != "ws" && scheme != "wss") throw new NotSupportedException($"The protocol {scheme} is not supported on GetSocket");
                tunnel.Uri.Query["caller"] = GetType().FullName;
                tunnel.Uri.Query["method"] = method;
                tunnel.Arguments = Serialize(method, !IsSecure, args);
                tunnel.IsFallback = true;
                return tunnel;
            }
            else
            {

                if (this.GetType() == typeof(TunnelClient))
                {
                    Client.Username = Username;
                    Client.Password = Password;
                    Client.ServerUrl = ServerUrl;
                    return await Client.GetTunnel(method, args);
                }
                else
                {
                    var service = new TunnelService(GetType().FullName);
                    return await service.GetTunnel(method, Username, Password, args);
                }

            }
        }
    }

    public abstract class EnterpriseServerTunnelClientBase : TunnelClient
    {
        public abstract Task<TunnelSocket> GetPveVncWebSocketAsync(int serviceItemId, VncCredentials credentials);
    }

    public abstract class ServerTunnelClientBase : TunnelClient
    {
        public abstract Task<TunnelSocket> GetPveVncWebSocketAsync(string vmId, VncCredentials credentials, RemoteServerSettings serverSettings, ServiceProviderSettings providerSettings);
    }
}