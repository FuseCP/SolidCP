using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;
using SolidCP.Providers.Virtualization;

namespace SolidCP.Providers.OS
{

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class TunnelServiceAttribute : Attribute
    {
        public Type Client { get; set; }
        static TunnelService instance = null;
        public TunnelService Instance => instance ?? (instance = (TunnelService)Activator.CreateInstance(Client));

        public TunnelServiceAttribute(Type client) { Client = client; }
    }


    public class TunnelService
    {
        public bool IsServerLoaded => AppDomain.CurrentDomain.GetAssemblies()
            .Any(a => a.GetName().Name == "SolidCP.Server");
        public bool IsEnterpriseServerLoaded => AppDomain.CurrentDomain.GetAssemblies()
            .Any(a => a.GetName().Name == "SolidCP.EnterpriseServer");
        public bool IsPortalLoaded => AppDomain.CurrentDomain.GetAssemblies()
            .Any(a => a.GetName().Name == "SolidCP.WebPortal");

        public ServerTunnelServiceBase ServerService => AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.GetName().Name == "SolidCP.Server")
            .SelectMany(a => a.GetCustomAttributes(typeof(TunnelServiceAttribute), false))
            .OfType<TunnelServiceAttribute>()
            .Select(a => (ServerTunnelServiceBase)a.Instance)
            .FirstOrDefault();

        public EnterpriseServerTunnelServiceBase EnterpriseServerService => AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.GetName().Name == "SolidCP.EnterpriseServer")
            .SelectMany(a => a.GetCustomAttributes(typeof(TunnelServiceAttribute), false))
            .OfType<TunnelServiceAttribute>()
            .Select(a => (EnterpriseServerTunnelServiceBase)a.Instance)
            .FirstOrDefault();

        public string CallerType { get; set; } = null;
        public virtual TunnelService Service => CallerType.StartsWith("SolidCP.EnterpriseServer.Client") ?
            EnterpriseServerService : (CallerType.StartsWith("SolidCP.Server.Client") ?
            ServerService : throw new Exception("Invalid caller type"));

        public virtual string CryptoKey => "";

        public string Username { get; private set; }
        public string Password { get; private set; }

        public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(120);

        public virtual byte[] DecryptData(ArraySegment<byte> secret) => new Cryptor(CryptoKey).Decrypt(secret);

        Type[] TypesFromNames(IEnumerable<string?> types) => types
            .Select(type => type != null ? Type.GetType(type) : null)
            .ToArray();

        IEnumerable<string?> ReadStrings(BinaryReader reader)
        {
            var line = reader.ReadString();
            while (!string.IsNullOrEmpty(line)) {
                yield return line == "!" ? null : line;
                line = reader.ReadString();
            }
        }

        Type[] ReadTypes(BinaryReader reader) => TypesFromNames(ReadStrings(reader));

        public virtual object[] Deserialize(string method, byte[] args)
        {
            try
            {
                var encrypted = args[0] != 0;
                MemoryStream mem;
                if (encrypted)
                {
                    args = DecryptData(new ArraySegment<byte>(args, 1, args.Length - 1));
                    mem = new MemoryStream(args);
                } else {
                    mem = new MemoryStream(args);
                    mem.Seek(1, SeekOrigin.Begin);
                }

                Type[] parameterTypes, types;
                using (var binaryReader = new BinaryReader(mem, Encoding.UTF8, true))
                {
                    parameterTypes = ReadTypes(binaryReader);
                    types = ReadTypes(binaryReader);
                }
                var methodInfo = this.GetType().GetMethod(method, parameterTypes);
                if (methodInfo == null) throw new ArgumentException($"Method {method} not found");

                // check if types are equal or subtype of parameterTypes
                bool invalidTypes = false;
                if (types.Length != parameterTypes.Length) invalidTypes = true;
                else
                {
                    for (int i = 0; i < types.Length; i++)
                    {
                        if (types[i] != null && types[i] != parameterTypes[i] && !types[i].IsSubclassOf(parameterTypes[i]))                        {
                            invalidTypes = true;
                            break;
                        }
                    }
                }
                if (invalidTypes) throw new ArgumentException($"Invalid parameter types for method {method}");

                var knownTypes = parameterTypes
                    .Concat(types)
                    .Concat(new Type[] { typeof(string) })
                    .Where(type => type != null)
                    .Distinct();

                var argsWithCredentials = TunnelSocket.Deserialize<object[]>(mem, knownTypes);

                if (argsWithCredentials.Length < 3 ||
                    !(argsWithCredentials[0] is long) ||
                    !(argsWithCredentials[1] == null || argsWithCredentials[1] is string) ||
                    !(argsWithCredentials[2] == null || argsWithCredentials[2] is string))
                    throw new AccessViolationException("No credentials specified");
                // check timestamp
                var timeStamp = (long)argsWithCredentials[0];
                if (new DateTime(timeStamp).Add(RequestTimeout) < DateTime.Now) throw new AccessViolationException("Request is too old");

                Username = argsWithCredentials[1] as string;
                Password = argsWithCredentials[2] as string;

                return argsWithCredentials.Skip(3).ToArray();
            }
            catch (Exception ex)
            {
                throw new SerializationException(ex.Message, ex);
            }
        }

        public virtual void Authenticate(string user, string password) => throw new NotSupportedException("Authentication is not supported in the base TunnelService class.");

        public TunnelService() { }
        public TunnelService(string callerType) : this() { CallerType = callerType; }

        public virtual async Task<TunnelSocket> GetTunnel(string method, byte[] arguments)
        {
            var args = Deserialize(method, arguments);
            return await GetTunnel(method, args);
        }

        public virtual async Task<TunnelSocket> GetTunnel(string method, string username, string password, object[] arguments)
        {
            Username = username;
            Password = password;
            return await GetTunnel(method, arguments);
        }

        public virtual async Task<TunnelSocket> GetTunnel(string method, object[] arguments)
        {

            if (GetType() == typeof(TunnelService))
            {
                Service.Username = Username;
                Service.Password = Password;
                return await Service.GetTunnel(method, arguments);
            }
            else
            {
                Service.Authenticate(Username, Password);

                var types = arguments.Select(arg => arg?.GetType()).ToArray();

                var methodInfos = GetType().GetMethods()
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
                if (methodInfos.Length > 1) throw new Exception($"Cannot determine which method {method} to use");

                var methodInfo = methodInfos[0];

                return await (Task<TunnelSocket>)methodInfo?.Invoke(this, arguments);
            }
        }
    }
    public abstract class ServerTunnelServiceBase : TunnelService
    {
        public override TunnelService Service => ServerService;
        public abstract Task<TunnelSocket> GetPveVncWebSocketAsync(string vmId, VncCredentials credentials, RemoteServerSettings serverSettings, ServiceProviderSettings providerSettings);
    }

    public abstract class EnterpriseServerTunnelServiceBase : TunnelService
    {
        public override TunnelService Service => EnterpriseServerService;
        public abstract Task<TunnelSocket> GetPveVncWebSocketAsync(int serviceItemId, VncCredentials credentials);
    }
}