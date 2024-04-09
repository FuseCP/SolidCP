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
using System.Threading.Tasks;

namespace SolidCP.Providers.OS
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
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
                    return  EnterpriseServerClient;
                }
                throw new NotSupportedException("Unknown assembly in AssemblyBinding.");
            }
        }

        public virtual string CryptoKey => "";
        public virtual string EncryptString(string secret) => new CryptoUtility(CryptoKey).Encrypt(secret);

        string[] TypeNames(Type[] types) => types?.Select(type => type?.FullName).ToArray();
        public string Serialize(string methodName, bool encrypted, params object[] args)
        {
            var mem = new MemoryStream();
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
                .FirstOrDefault(m => {
                    if (m.Method.Name != methodName) return false;

                    if (m.ParameterTypes.Length != types.Length) return false;

                    for (int i = 0; i < m.ParameterTypes.Length; i++)
                    {
                        if (types[i] != null)
                        {
                            if (m.ParameterTypes[i] != types[i] && !types[i].IsSubclassOf(m.ParameterTypes[i])) return false;
                        }
                    }
                    return true;
                });
            if (method == null) throw new ArgumentException($"Could not find method {methodName} with correct parameters");

            var typeSerializer = new DataContractSerializer(typeof(string[]));
            typeSerializer.WriteObject(mem, TypeNames(method.ParameterTypes));
            typeSerializer.WriteObject(mem, TypeNames(types));
            var knownTypes = method.ParameterTypes.Concat(types)
                .Where(type => type != null)
                .Distinct();
            var serializer = new DataContractSerializer(typeof(object[]), knownTypes);
            var argsWithCredentials = new string[] { Username, Password }
                .Concat(args)
                .ToArray();
            serializer.WriteObject(mem, argsWithCredentials);
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
        public bool IsSecure => (ServerUrl?.StartsWith("https://") ?? false) || (ServerUrl?.StartsWith("ssh://") ?? false) ||
            DnsService.IsHostLAN(new Uri(ServerUrl ?? "").Host);

        public virtual async Task<TunnelSocket> GetSocket(string method, params object[] args)
        {
            if (!IsAssemblyBinding)
            {
                var url = $"{ServerUrl}/Tunnel";
                var uri = new SshUri(url);
                var scheme = uri.Scheme;
                if (scheme == "ssh") uri.Protocol = uri.Protocol ?? "ws";
                else if (scheme == "https") uri.Scheme = "wss";
                else if (scheme == "http") uri.Scheme = "ws";
                else throw new NotSupportedException($"The protocol {scheme} is not supported on GetSocket");
                uri.Query["caller"] = GetType().FullName;
                uri.Query["mehtod"] = method;
                uri.Query[$"args{(!IsSecure ? "x" : "")}"] = Serialize(method, !IsSecure, args);
                var tunnel = new TunnelSocket(uri.Url);
                tunnel.IsFallback = true;
                return tunnel;
            }
            else
            {

                if (this.GetType() == typeof(TunnelClient))
                {
                    return await Client.GetSocket(method, args);
                }
                else
                {
                    var service = new TunnelService(GetType().FullName);
                    return await service.GetSocket(method, Username, Password, args);
                    
                    /*
                    var types = args.Select(arg => arg?.GetType()).ToArray();

                    var service = Service;
                    var methodInfos = service.GetType().GetMethods()
                        .Where(m =>
                        {
                            if (m.Name != method || !m.IsPublic) return false;

                            var pars = m.GetParameters();
                            if (pars.Length != types.Length) return false;

                            for (int i = 0; i < pars.Length; i++)
                            {
                                if (pars[i].ParameterType != types[i] && types[i] != null && !types[i].IsSubclassOf(pars[i].ParameterType))
                                    return false;
                            }

                            return true;
                        })
                        .ToArray();

                    if (methodInfos.Length == 0) throw new Exception($"No method {method} found with the correct signature");
                    if (methodInfos.Length > 1) throw new Exception($"Cannot determin which method {method} to use");

                    var methodInfo = methodInfos[0];

                    return await (Task<TunnelSocket>)methodInfo?.Invoke(service, args);
                    */
                }

            }
        }
    }

    public abstract class EnterpriseServerTunnelClientBase : TunnelClient
    {
        public abstract Task<TunnelSocket> GetPveVncWebSocketAsync(int serviceItemId);
    }

    public abstract class ServerTunnelClientBase : TunnelClient
    {
        public abstract Task<TunnelSocket> GetPveVncWebSocketAsync(string vmId, ServiceProviderSettings providerSettings);
    }
}