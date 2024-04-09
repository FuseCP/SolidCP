using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SolidCP.Providers.OS
{

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class TunnelServiceAttribute : Attribute
    {
        public Type Client { get; set; }
        public TunnelService Instance => (TunnelService)Activator.CreateInstance(Client);

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
            ServerService : throw new Exception("Invalid"));

        public virtual string CryptoKey => "";

        public string Username { get; private set; }
        public string Password { get; private set; }
        public virtual string DecryptString(string secret) => new CryptoUtility(CryptoKey).Decrypt(secret);

        public virtual object[] Deserialize(string method, string args, bool encrypted)
        {
            if (encrypted) args = DecryptString(args);
            var bytes = Convert.FromBase64String(args);
            var mem = new MemoryStream(bytes);
            var typeSerializer = new DataContractSerializer(typeof(Type[]));
            var parameterTypes = (Type[])typeSerializer.ReadObject(mem);
            var types = (Type[])typeSerializer.ReadObject(mem);
            var methodInfo = this.GetType().GetMethod(method, parameterTypes);
            if (methodInfo == null) throw new ArgumentException("Method not found");

            bool invalidTypes = false;
            if (types.Length != parameterTypes.Length) invalidTypes = true;
            else
            {
                for (int i = 0; i < types.Length; i++)
                {
                    if (types[i] != null && 
                        (types[i] != parameterTypes[i] ||
                        !types[i].IsSubclassOf(parameterTypes[i]))) {
                        invalidTypes = true;
                        break;
                    }
                }
            }
            if (invalidTypes) throw new ArgumentException("Invalid parameter types");

            var knownTypes = parameterTypes.Concat(types)
                .Where(type => type != null)
                .Distinct();
            var serializer = new DataContractSerializer(typeof(object[]), knownTypes);
            return (object[])serializer.ReadObject(mem);
        }

        public virtual void Authenticate(string user, string password) => throw new NotSupportedException("Authentication is not supported in the base TunnelService class.");

        public TunnelService() { }
        public TunnelService(string callerType) : this() { CallerType = callerType; }

        public virtual async Task<TunnelSocket> GetSocket(string method, string arguments, bool encrypted)
        {
            var args = Deserialize(method, arguments, encrypted);
            return await GetSocket(method, args);
        }

        public virtual async Task<TunnelSocket> GetSocket(string method, object[] arguments)
        {

            if (GetType() == typeof(TunnelService))
            {
                return await Service.GetSocket(method, arguments);
            }
            else
            {
                Username = arguments[0] as string;
                Password = arguments[1] as string;
                Service.Authenticate(Username, Password);

                var args = arguments.Skip(2).ToArray();
                var types = args.Select(arg => arg?.GetType()).ToArray();

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

                return await (Task<TunnelSocket>)methodInfo?.Invoke(this, args);
            }
        }
    }
    public abstract class ServerTunnelServiceBase : TunnelService
    {
        public override TunnelService Service => ServerService;
        public abstract Task<TunnelSocket> GetPveVncWebSocketAsync(string vmId, ServiceProviderSettings providerSettings);
    }

    public abstract class EnterpriseServerTunnelServiceBase : TunnelService
    {
        public override TunnelService Service => EnterpriseServerService;
        public abstract Task<TunnelSocket> GetPveVncWebSocketAsync(int serviceItemId);
    }
}