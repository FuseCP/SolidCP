#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IAutoDiscovery", Namespace = "http://tempuri.org/")]
    public interface IAutoDiscovery
    {
        [OperationContract(Action = "http://tempuri.org/IAutoDiscovery/IsInstalled", ReplyAction = "http://tempuri.org/IAutoDiscovery/IsInstalledResponse")]
        SolidCP.Providers.Common.BoolResult IsInstalled(string providerName);
        [OperationContract(Action = "http://tempuri.org/IAutoDiscovery/IsInstalled", ReplyAction = "http://tempuri.org/IAutoDiscovery/IsInstalledResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.BoolResult> IsInstalledAsync(string providerName);
        [OperationContract(Action = "http://tempuri.org/IAutoDiscovery/GetServerFilePath", ReplyAction = "http://tempuri.org/IAutoDiscovery/GetServerFilePathResponse")]
        string GetServerFilePath();
        [OperationContract(Action = "http://tempuri.org/IAutoDiscovery/GetServerFilePath", ReplyAction = "http://tempuri.org/IAutoDiscovery/GetServerFilePathResponse")]
        System.Threading.Tasks.Task<string> GetServerFilePathAsync();
        [OperationContract(Action = "http://tempuri.org/IAutoDiscovery/GetServerVersion", ReplyAction = "http://tempuri.org/IAutoDiscovery/GetServerVersionResponse")]
        string GetServerVersion();
        [OperationContract(Action = "http://tempuri.org/IAutoDiscovery/GetServerVersion", ReplyAction = "http://tempuri.org/IAutoDiscovery/GetServerVersionResponse")]
        System.Threading.Tasks.Task<string> GetServerVersionAsync();
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class AutoDiscoveryAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IAutoDiscovery
    {
        public SolidCP.Providers.Common.BoolResult IsInstalled(string providerName)
        {
            return Invoke<SolidCP.Providers.Common.BoolResult>("SolidCP.Server.AutoDiscovery", "IsInstalled", providerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.BoolResult> IsInstalledAsync(string providerName)
        {
            return await InvokeAsync<SolidCP.Providers.Common.BoolResult>("SolidCP.Server.AutoDiscovery", "IsInstalled", providerName);
        }

        public string GetServerFilePath()
        {
            return Invoke<string>("SolidCP.Server.AutoDiscovery", "GetServerFilePath");
        }

        public async System.Threading.Tasks.Task<string> GetServerFilePathAsync()
        {
            return await InvokeAsync<string>("SolidCP.Server.AutoDiscovery", "GetServerFilePath");
        }

        public string GetServerVersion()
        {
            return Invoke<string>("SolidCP.Server.AutoDiscovery", "GetServerVersion");
        }

        public async System.Threading.Tasks.Task<string> GetServerVersionAsync()
        {
            return await InvokeAsync<string>("SolidCP.Server.AutoDiscovery", "GetServerVersion");
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class AutoDiscovery : SolidCP.Web.Client.ClientBase<IAutoDiscovery, AutoDiscoveryAssemblyClient>, IAutoDiscovery
    {
        public SolidCP.Providers.Common.BoolResult IsInstalled(string providerName)
        {
            return base.Client.IsInstalled(providerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.BoolResult> IsInstalledAsync(string providerName)
        {
            return await base.Client.IsInstalledAsync(providerName);
        }

        public string GetServerFilePath()
        {
            return base.Client.GetServerFilePath();
        }

        public async System.Threading.Tasks.Task<string> GetServerFilePathAsync()
        {
            return await base.Client.GetServerFilePathAsync();
        }

        public string GetServerVersion()
        {
            return base.Client.GetServerVersion();
        }

        public async System.Threading.Tasks.Task<string> GetServerVersionAsync()
        {
            return await base.Client.GetServerVersionAsync();
        }
    }
}
#endif