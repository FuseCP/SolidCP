using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.IO;
using System.Text;
using Compat.Runtime.Serialization;
using System.Net;
using System.Net.WebSockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SolidCP.Providers.OS
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class TunnelClientAttribute : Attribute
    {
        public Type Client { get; set; }
        public TunnelClient Instance => (TunnelClient)Activator.CreateInstance(Client);

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
            .Where(a => a.GetName().Name == "SolidCP.Server")
            .SelectMany(a => a.GetCustomAttributes(typeof(TunnelClientAttribute), false))
            .OfType<TunnelClientAttribute>()
            .Select(a => (ServerTunnelClientBase)CopyTo(a.Instance))
            .FirstOrDefault();

        public EnterpriseServerTunnelClientBase EnterpriseServerClient => AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.GetName().Name == "SolidCP.Server")
            .SelectMany(a => a.GetCustomAttributes(typeof(TunnelClientAttribute), false))
            .OfType<TunnelClientAttribute>()
            .Select(a => (EnterpriseServerTunnelClientBase)CopyTo(a.Instance))
            .FirstOrDefault();

        public virtual string CryptoKey => "";
        public virtual string EncryptString(string secret) => new CryptoUtility(CryptoKey).Encrypt(secret);
        public string Serialize(bool encrypted, params object[] args)
        {
            var formatter = new NetDataContractSerializer();
            var mem = new MemoryStream();
            formatter.Serialize(mem, args);
            var base64 = Convert.ToBase64String(mem.ToArray());
            if (encrypted) base64 = EncryptString(base64);
            return base64;
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
        public bool IsSecure => (ServerUrl?.StartsWith("https://") ?? false) || (ServerUrl?.StartsWith("ssh://") ?? false);

        public virtual async Task<TunnelSocket> GetSocket(string method, params object[] args)
        {
            if (!IsAssemblyBinding)
            {
                args = new object[] { Username, Password }.Concat(args).ToArray();
                var url = $"{ServerUrl}?caller={WebUtility.UrlEncode(GetType().Name)}&method={WebUtility.UrlEncode(method)}&args{(!IsSecure ? "x" : "")}={WebUtility.UrlEncode(Serialize(!IsSecure, args))}";
                var tunnel = new TunnelSocket(url);
                tunnel.IsFallback = true;
                return tunnel;
            }
            else
            {
                if (this.GetType() == typeof(TunnelClient))
                {
                    if (ServerUrl == "assembly://SolidCP.Server")
                    {
                        if (!IsServerLoaded) Assembly.Load("SolidCP.Server");
                        var client = ServerClient;
                        return await client.GetSocket(method, args);
                    }
                    else if (ServerUrl == "assembly://SolidCP.EnterpriseServer")
                    {
                        if (!IsEnterpriseServerLoaded) Assembly.Load("SolidCP.EnterpriseServer");
                        var client = EnterpriseServerClient;
                        return await client.GetSocket(method, args);
                    }
                    throw new NotSupportedException("Unknown assembly in AssemblyBinding.");
                }
                else
                {
                    var types = args.Select(arg => arg?.GetType()).ToArray();
                    if (types.Any(type => type == null)) throw new ArgumentException("Cannot derive argument type because it is null.");

                    var methodInfo = this.GetType().GetMethod(method, types);
                    if (methodInfo == null) throw new ArgumentException("Method not found");
                    return await (Task<TunnelSocket>)methodInfo?.Invoke(this, args);
                }

            }
        }
    }

    public abstract class EnterpriseServerTunnelClientBase : TunnelClient
    {
        public abstract Task<TunnelSocket> GetPveVncWebSocketAsync(int serviceId, int packageId, int serviceItemId);
    }

    public abstract class ServerTunnelClientBase : TunnelClient
    {
        public abstract Task<TunnelSocket> GetPveVncWebSocketAsync(string vmId, ServiceProviderSettings providerSettings);
    }
}