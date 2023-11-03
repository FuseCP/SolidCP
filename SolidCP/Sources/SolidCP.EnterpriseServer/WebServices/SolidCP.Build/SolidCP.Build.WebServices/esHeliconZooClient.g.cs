#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesHeliconZoo", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesHeliconZoo
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/GetEngines", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/GetEnginesResponse")]
        SolidCP.Providers.HeliconZoo.HeliconZooEngine[] GetEngines(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/GetEngines", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/GetEnginesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HeliconZoo.HeliconZooEngine[]> GetEnginesAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/SetEngines", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/SetEnginesResponse")]
        void SetEngines(int serviceId, SolidCP.Providers.HeliconZoo.HeliconZooEngine[] userEngines);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/SetEngines", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/SetEnginesResponse")]
        System.Threading.Tasks.Task SetEnginesAsync(int serviceId, SolidCP.Providers.HeliconZoo.HeliconZooEngine[] userEngines);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/IsEnginesEnabled", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/IsEnginesEnabledResponse")]
        bool IsEnginesEnabled(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/IsEnginesEnabled", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/IsEnginesEnabledResponse")]
        System.Threading.Tasks.Task<bool> IsEnginesEnabledAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/SwithEnginesEnabled", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/SwithEnginesEnabledResponse")]
        void SwithEnginesEnabled(int serviceId, bool enabled);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/SwithEnginesEnabled", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/SwithEnginesEnabledResponse")]
        System.Threading.Tasks.Task SwithEnginesEnabledAsync(int serviceId, bool enabled);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/GetAllowedHeliconZooQuotasForPackage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/GetAllowedHeliconZooQuotasForPackageResponse")]
        SolidCP.Providers.HeliconZoo.ShortHeliconZooEngine[] GetAllowedHeliconZooQuotasForPackage(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/GetAllowedHeliconZooQuotasForPackage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/GetAllowedHeliconZooQuotasForPackageResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HeliconZoo.ShortHeliconZooEngine[]> GetAllowedHeliconZooQuotasForPackageAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/GetEnabledEnginesForSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/GetEnabledEnginesForSiteResponse")]
        string[] GetEnabledEnginesForSite(string siteId, int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/GetEnabledEnginesForSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/GetEnabledEnginesForSiteResponse")]
        System.Threading.Tasks.Task<string[]> GetEnabledEnginesForSiteAsync(string siteId, int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/SetEnabledEnginesForSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/SetEnabledEnginesForSiteResponse")]
        void SetEnabledEnginesForSite(string siteId, int packageId, string[] engines);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/SetEnabledEnginesForSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/SetEnabledEnginesForSiteResponse")]
        System.Threading.Tasks.Task SetEnabledEnginesForSiteAsync(string siteId, int packageId, string[] engines);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/IsWebCosoleEnabled", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/IsWebCosoleEnabledResponse")]
        bool IsWebCosoleEnabled(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/IsWebCosoleEnabled", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/IsWebCosoleEnabledResponse")]
        System.Threading.Tasks.Task<bool> IsWebCosoleEnabledAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/SetWebCosoleEnabled", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/SetWebCosoleEnabledResponse")]
        void SetWebCosoleEnabled(int serviceId, bool enabled);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/SetWebCosoleEnabled", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHeliconZoo/SetWebCosoleEnabledResponse")]
        System.Threading.Tasks.Task SetWebCosoleEnabledAsync(int serviceId, bool enabled);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esHeliconZooAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesHeliconZoo
    {
        public SolidCP.Providers.HeliconZoo.HeliconZooEngine[] GetEngines(int serviceId)
        {
            return Invoke<SolidCP.Providers.HeliconZoo.HeliconZooEngine[]>("SolidCP.EnterpriseServer.esHeliconZoo", "GetEngines", serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HeliconZoo.HeliconZooEngine[]> GetEnginesAsync(int serviceId)
        {
            return await InvokeAsync<SolidCP.Providers.HeliconZoo.HeliconZooEngine[]>("SolidCP.EnterpriseServer.esHeliconZoo", "GetEngines", serviceId);
        }

        public void SetEngines(int serviceId, SolidCP.Providers.HeliconZoo.HeliconZooEngine[] userEngines)
        {
            Invoke("SolidCP.EnterpriseServer.esHeliconZoo", "SetEngines", serviceId, userEngines);
        }

        public async System.Threading.Tasks.Task SetEnginesAsync(int serviceId, SolidCP.Providers.HeliconZoo.HeliconZooEngine[] userEngines)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esHeliconZoo", "SetEngines", serviceId, userEngines);
        }

        public bool IsEnginesEnabled(int serviceId)
        {
            return Invoke<bool>("SolidCP.EnterpriseServer.esHeliconZoo", "IsEnginesEnabled", serviceId);
        }

        public async System.Threading.Tasks.Task<bool> IsEnginesEnabledAsync(int serviceId)
        {
            return await InvokeAsync<bool>("SolidCP.EnterpriseServer.esHeliconZoo", "IsEnginesEnabled", serviceId);
        }

        public void SwithEnginesEnabled(int serviceId, bool enabled)
        {
            Invoke("SolidCP.EnterpriseServer.esHeliconZoo", "SwithEnginesEnabled", serviceId, enabled);
        }

        public async System.Threading.Tasks.Task SwithEnginesEnabledAsync(int serviceId, bool enabled)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esHeliconZoo", "SwithEnginesEnabled", serviceId, enabled);
        }

        public SolidCP.Providers.HeliconZoo.ShortHeliconZooEngine[] GetAllowedHeliconZooQuotasForPackage(int packageId)
        {
            return Invoke<SolidCP.Providers.HeliconZoo.ShortHeliconZooEngine[]>("SolidCP.EnterpriseServer.esHeliconZoo", "GetAllowedHeliconZooQuotasForPackage", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HeliconZoo.ShortHeliconZooEngine[]> GetAllowedHeliconZooQuotasForPackageAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.Providers.HeliconZoo.ShortHeliconZooEngine[]>("SolidCP.EnterpriseServer.esHeliconZoo", "GetAllowedHeliconZooQuotasForPackage", packageId);
        }

        public string[] GetEnabledEnginesForSite(string siteId, int packageId)
        {
            return Invoke<string[]>("SolidCP.EnterpriseServer.esHeliconZoo", "GetEnabledEnginesForSite", siteId, packageId);
        }

        public async System.Threading.Tasks.Task<string[]> GetEnabledEnginesForSiteAsync(string siteId, int packageId)
        {
            return await InvokeAsync<string[]>("SolidCP.EnterpriseServer.esHeliconZoo", "GetEnabledEnginesForSite", siteId, packageId);
        }

        public void SetEnabledEnginesForSite(string siteId, int packageId, string[] engines)
        {
            Invoke("SolidCP.EnterpriseServer.esHeliconZoo", "SetEnabledEnginesForSite", siteId, packageId, engines);
        }

        public async System.Threading.Tasks.Task SetEnabledEnginesForSiteAsync(string siteId, int packageId, string[] engines)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esHeliconZoo", "SetEnabledEnginesForSite", siteId, packageId, engines);
        }

        public bool IsWebCosoleEnabled(int serviceId)
        {
            return Invoke<bool>("SolidCP.EnterpriseServer.esHeliconZoo", "IsWebCosoleEnabled", serviceId);
        }

        public async System.Threading.Tasks.Task<bool> IsWebCosoleEnabledAsync(int serviceId)
        {
            return await InvokeAsync<bool>("SolidCP.EnterpriseServer.esHeliconZoo", "IsWebCosoleEnabled", serviceId);
        }

        public void SetWebCosoleEnabled(int serviceId, bool enabled)
        {
            Invoke("SolidCP.EnterpriseServer.esHeliconZoo", "SetWebCosoleEnabled", serviceId, enabled);
        }

        public async System.Threading.Tasks.Task SetWebCosoleEnabledAsync(int serviceId, bool enabled)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esHeliconZoo", "SetWebCosoleEnabled", serviceId, enabled);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esHeliconZoo : SolidCP.Web.Client.ClientBase<IesHeliconZoo, esHeliconZooAssemblyClient>, IesHeliconZoo
    {
        public SolidCP.Providers.HeliconZoo.HeliconZooEngine[] GetEngines(int serviceId)
        {
            return base.Client.GetEngines(serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HeliconZoo.HeliconZooEngine[]> GetEnginesAsync(int serviceId)
        {
            return await base.Client.GetEnginesAsync(serviceId);
        }

        public void SetEngines(int serviceId, SolidCP.Providers.HeliconZoo.HeliconZooEngine[] userEngines)
        {
            base.Client.SetEngines(serviceId, userEngines);
        }

        public async System.Threading.Tasks.Task SetEnginesAsync(int serviceId, SolidCP.Providers.HeliconZoo.HeliconZooEngine[] userEngines)
        {
            await base.Client.SetEnginesAsync(serviceId, userEngines);
        }

        public bool IsEnginesEnabled(int serviceId)
        {
            return base.Client.IsEnginesEnabled(serviceId);
        }

        public async System.Threading.Tasks.Task<bool> IsEnginesEnabledAsync(int serviceId)
        {
            return await base.Client.IsEnginesEnabledAsync(serviceId);
        }

        public void SwithEnginesEnabled(int serviceId, bool enabled)
        {
            base.Client.SwithEnginesEnabled(serviceId, enabled);
        }

        public async System.Threading.Tasks.Task SwithEnginesEnabledAsync(int serviceId, bool enabled)
        {
            await base.Client.SwithEnginesEnabledAsync(serviceId, enabled);
        }

        public SolidCP.Providers.HeliconZoo.ShortHeliconZooEngine[] GetAllowedHeliconZooQuotasForPackage(int packageId)
        {
            return base.Client.GetAllowedHeliconZooQuotasForPackage(packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HeliconZoo.ShortHeliconZooEngine[]> GetAllowedHeliconZooQuotasForPackageAsync(int packageId)
        {
            return await base.Client.GetAllowedHeliconZooQuotasForPackageAsync(packageId);
        }

        public string[] GetEnabledEnginesForSite(string siteId, int packageId)
        {
            return base.Client.GetEnabledEnginesForSite(siteId, packageId);
        }

        public async System.Threading.Tasks.Task<string[]> GetEnabledEnginesForSiteAsync(string siteId, int packageId)
        {
            return await base.Client.GetEnabledEnginesForSiteAsync(siteId, packageId);
        }

        public void SetEnabledEnginesForSite(string siteId, int packageId, string[] engines)
        {
            base.Client.SetEnabledEnginesForSite(siteId, packageId, engines);
        }

        public async System.Threading.Tasks.Task SetEnabledEnginesForSiteAsync(string siteId, int packageId, string[] engines)
        {
            await base.Client.SetEnabledEnginesForSiteAsync(siteId, packageId, engines);
        }

        public bool IsWebCosoleEnabled(int serviceId)
        {
            return base.Client.IsWebCosoleEnabled(serviceId);
        }

        public async System.Threading.Tasks.Task<bool> IsWebCosoleEnabledAsync(int serviceId)
        {
            return await base.Client.IsWebCosoleEnabledAsync(serviceId);
        }

        public void SetWebCosoleEnabled(int serviceId, bool enabled)
        {
            base.Client.SetWebCosoleEnabled(serviceId, enabled);
        }

        public async System.Threading.Tasks.Task SetWebCosoleEnabledAsync(int serviceId, bool enabled)
        {
            await base.Client.SetWebCosoleEnabledAsync(serviceId, enabled);
        }
    }
}
#endif