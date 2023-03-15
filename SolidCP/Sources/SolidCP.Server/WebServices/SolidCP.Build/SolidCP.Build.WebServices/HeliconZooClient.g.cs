#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("ServerPolicy")]
    [SolidCP.Providers.SoapHeader]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IHeliconZoo", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IHeliconZoo
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHeliconZoo/GetEngines", ReplyAction = "http://smbsaas/solidcp/server/IHeliconZoo/GetEnginesResponse")]
        SolidCP.Providers.HeliconZoo.HeliconZooEngine[] GetEngines();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHeliconZoo/GetEngines", ReplyAction = "http://smbsaas/solidcp/server/IHeliconZoo/GetEnginesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HeliconZoo.HeliconZooEngine[]> GetEnginesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHeliconZoo/SetEngines", ReplyAction = "http://smbsaas/solidcp/server/IHeliconZoo/SetEnginesResponse")]
        void SetEngines(SolidCP.Providers.HeliconZoo.HeliconZooEngine[] userEngines);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHeliconZoo/SetEngines", ReplyAction = "http://smbsaas/solidcp/server/IHeliconZoo/SetEnginesResponse")]
        System.Threading.Tasks.Task SetEnginesAsync(SolidCP.Providers.HeliconZoo.HeliconZooEngine[] userEngines);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHeliconZoo/IsEnginesEnabled", ReplyAction = "http://smbsaas/solidcp/server/IHeliconZoo/IsEnginesEnabledResponse")]
        bool IsEnginesEnabled();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHeliconZoo/IsEnginesEnabled", ReplyAction = "http://smbsaas/solidcp/server/IHeliconZoo/IsEnginesEnabledResponse")]
        System.Threading.Tasks.Task<bool> IsEnginesEnabledAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHeliconZoo/SwithEnginesEnabled", ReplyAction = "http://smbsaas/solidcp/server/IHeliconZoo/SwithEnginesEnabledResponse")]
        void SwithEnginesEnabled(bool enabled);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHeliconZoo/SwithEnginesEnabled", ReplyAction = "http://smbsaas/solidcp/server/IHeliconZoo/SwithEnginesEnabledResponse")]
        System.Threading.Tasks.Task SwithEnginesEnabledAsync(bool enabled);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHeliconZoo/GetEnabledEnginesForSite", ReplyAction = "http://smbsaas/solidcp/server/IHeliconZoo/GetEnabledEnginesForSiteResponse")]
        string[] GetEnabledEnginesForSite(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHeliconZoo/GetEnabledEnginesForSite", ReplyAction = "http://smbsaas/solidcp/server/IHeliconZoo/GetEnabledEnginesForSiteResponse")]
        System.Threading.Tasks.Task<string[]> GetEnabledEnginesForSiteAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHeliconZoo/SetEnabledEnginesForSite", ReplyAction = "http://smbsaas/solidcp/server/IHeliconZoo/SetEnabledEnginesForSiteResponse")]
        void SetEnabledEnginesForSite(string siteId, string[] engineNames);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHeliconZoo/SetEnabledEnginesForSite", ReplyAction = "http://smbsaas/solidcp/server/IHeliconZoo/SetEnabledEnginesForSiteResponse")]
        System.Threading.Tasks.Task SetEnabledEnginesForSiteAsync(string siteId, string[] engineNames);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHeliconZoo/IsWebCosoleEnabled", ReplyAction = "http://smbsaas/solidcp/server/IHeliconZoo/IsWebCosoleEnabledResponse")]
        bool IsWebCosoleEnabled();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHeliconZoo/IsWebCosoleEnabled", ReplyAction = "http://smbsaas/solidcp/server/IHeliconZoo/IsWebCosoleEnabledResponse")]
        System.Threading.Tasks.Task<bool> IsWebCosoleEnabledAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHeliconZoo/SetWebCosoleEnabled", ReplyAction = "http://smbsaas/solidcp/server/IHeliconZoo/SetWebCosoleEnabledResponse")]
        void SetWebCosoleEnabled(bool enabled);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHeliconZoo/SetWebCosoleEnabled", ReplyAction = "http://smbsaas/solidcp/server/IHeliconZoo/SetWebCosoleEnabledResponse")]
        System.Threading.Tasks.Task SetWebCosoleEnabledAsync(bool enabled);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class HeliconZooAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IHeliconZoo
    {
        public SolidCP.Providers.HeliconZoo.HeliconZooEngine[] GetEngines()
        {
            return Invoke<SolidCP.Providers.HeliconZoo.HeliconZooEngine[]>("SolidCP.Server.HeliconZoo", "GetEngines");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HeliconZoo.HeliconZooEngine[]> GetEnginesAsync()
        {
            return await InvokeAsync<SolidCP.Providers.HeliconZoo.HeliconZooEngine[]>("SolidCP.Server.HeliconZoo", "GetEngines");
        }

        public void SetEngines(SolidCP.Providers.HeliconZoo.HeliconZooEngine[] userEngines)
        {
            Invoke("SolidCP.Server.HeliconZoo", "SetEngines", userEngines);
        }

        public async System.Threading.Tasks.Task SetEnginesAsync(SolidCP.Providers.HeliconZoo.HeliconZooEngine[] userEngines)
        {
            await InvokeAsync("SolidCP.Server.HeliconZoo", "SetEngines", userEngines);
        }

        public bool IsEnginesEnabled()
        {
            return Invoke<bool>("SolidCP.Server.HeliconZoo", "IsEnginesEnabled");
        }

        public async System.Threading.Tasks.Task<bool> IsEnginesEnabledAsync()
        {
            return await InvokeAsync<bool>("SolidCP.Server.HeliconZoo", "IsEnginesEnabled");
        }

        public void SwithEnginesEnabled(bool enabled)
        {
            Invoke("SolidCP.Server.HeliconZoo", "SwithEnginesEnabled", enabled);
        }

        public async System.Threading.Tasks.Task SwithEnginesEnabledAsync(bool enabled)
        {
            await InvokeAsync("SolidCP.Server.HeliconZoo", "SwithEnginesEnabled", enabled);
        }

        public string[] GetEnabledEnginesForSite(string siteId)
        {
            return Invoke<string[]>("SolidCP.Server.HeliconZoo", "GetEnabledEnginesForSite", siteId);
        }

        public async System.Threading.Tasks.Task<string[]> GetEnabledEnginesForSiteAsync(string siteId)
        {
            return await InvokeAsync<string[]>("SolidCP.Server.HeliconZoo", "GetEnabledEnginesForSite", siteId);
        }

        public void SetEnabledEnginesForSite(string siteId, string[] engineNames)
        {
            Invoke("SolidCP.Server.HeliconZoo", "SetEnabledEnginesForSite", siteId, engineNames);
        }

        public async System.Threading.Tasks.Task SetEnabledEnginesForSiteAsync(string siteId, string[] engineNames)
        {
            await InvokeAsync("SolidCP.Server.HeliconZoo", "SetEnabledEnginesForSite", siteId, engineNames);
        }

        public bool IsWebCosoleEnabled()
        {
            return Invoke<bool>("SolidCP.Server.HeliconZoo", "IsWebCosoleEnabled");
        }

        public async System.Threading.Tasks.Task<bool> IsWebCosoleEnabledAsync()
        {
            return await InvokeAsync<bool>("SolidCP.Server.HeliconZoo", "IsWebCosoleEnabled");
        }

        public void SetWebCosoleEnabled(bool enabled)
        {
            Invoke("SolidCP.Server.HeliconZoo", "SetWebCosoleEnabled", enabled);
        }

        public async System.Threading.Tasks.Task SetWebCosoleEnabledAsync(bool enabled)
        {
            await InvokeAsync("SolidCP.Server.HeliconZoo", "SetWebCosoleEnabled", enabled);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class HeliconZoo : SolidCP.Web.Client.ClientBase<IHeliconZoo, HeliconZooAssemblyClient>, IHeliconZoo
    {
        public SolidCP.Providers.HeliconZoo.HeliconZooEngine[] GetEngines()
        {
            return base.Client.GetEngines();
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HeliconZoo.HeliconZooEngine[]> GetEnginesAsync()
        {
            return await base.Client.GetEnginesAsync();
        }

        public void SetEngines(SolidCP.Providers.HeliconZoo.HeliconZooEngine[] userEngines)
        {
            base.Client.SetEngines(userEngines);
        }

        public async System.Threading.Tasks.Task SetEnginesAsync(SolidCP.Providers.HeliconZoo.HeliconZooEngine[] userEngines)
        {
            await base.Client.SetEnginesAsync(userEngines);
        }

        public bool IsEnginesEnabled()
        {
            return base.Client.IsEnginesEnabled();
        }

        public async System.Threading.Tasks.Task<bool> IsEnginesEnabledAsync()
        {
            return await base.Client.IsEnginesEnabledAsync();
        }

        public void SwithEnginesEnabled(bool enabled)
        {
            base.Client.SwithEnginesEnabled(enabled);
        }

        public async System.Threading.Tasks.Task SwithEnginesEnabledAsync(bool enabled)
        {
            await base.Client.SwithEnginesEnabledAsync(enabled);
        }

        public string[] GetEnabledEnginesForSite(string siteId)
        {
            return base.Client.GetEnabledEnginesForSite(siteId);
        }

        public async System.Threading.Tasks.Task<string[]> GetEnabledEnginesForSiteAsync(string siteId)
        {
            return await base.Client.GetEnabledEnginesForSiteAsync(siteId);
        }

        public void SetEnabledEnginesForSite(string siteId, string[] engineNames)
        {
            base.Client.SetEnabledEnginesForSite(siteId, engineNames);
        }

        public async System.Threading.Tasks.Task SetEnabledEnginesForSiteAsync(string siteId, string[] engineNames)
        {
            await base.Client.SetEnabledEnginesForSiteAsync(siteId, engineNames);
        }

        public bool IsWebCosoleEnabled()
        {
            return base.Client.IsWebCosoleEnabled();
        }

        public async System.Threading.Tasks.Task<bool> IsWebCosoleEnabledAsync()
        {
            return await base.Client.IsWebCosoleEnabledAsync();
        }

        public void SetWebCosoleEnabled(bool enabled)
        {
            base.Client.SetWebCosoleEnabled(enabled);
        }

        public async System.Threading.Tasks.Task SetWebCosoleEnabledAsync(bool enabled)
        {
            await base.Client.SetWebCosoleEnabledAsync(enabled);
        }
    }
}
#endif