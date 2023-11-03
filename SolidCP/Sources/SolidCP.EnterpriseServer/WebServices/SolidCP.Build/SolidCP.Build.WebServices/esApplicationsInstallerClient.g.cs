#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesApplicationsInstaller", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesApplicationsInstaller
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesApplicationsInstaller/InstallApplication", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesApplicationsInstaller/InstallApplicationResponse")]
        int InstallApplication(SolidCP.EnterpriseServer.InstallationInfo inst);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesApplicationsInstaller/InstallApplication", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesApplicationsInstaller/InstallApplicationResponse")]
        System.Threading.Tasks.Task<int> InstallApplicationAsync(SolidCP.EnterpriseServer.InstallationInfo inst);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesApplicationsInstaller/GetApplications", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesApplicationsInstaller/GetApplicationsResponse")]
        SolidCP.EnterpriseServer.ApplicationInfo[] /*List*/ GetApplications(int packageId, string categoryId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesApplicationsInstaller/GetApplications", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesApplicationsInstaller/GetApplicationsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ApplicationInfo[]> GetApplicationsAsync(int packageId, string categoryId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesApplicationsInstaller/GetCategories", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesApplicationsInstaller/GetCategoriesResponse")]
        SolidCP.EnterpriseServer.ApplicationCategory[] /*List*/ GetCategories();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesApplicationsInstaller/GetCategories", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesApplicationsInstaller/GetCategoriesResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ApplicationCategory[]> GetCategoriesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesApplicationsInstaller/GetApplication", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesApplicationsInstaller/GetApplicationResponse")]
        SolidCP.EnterpriseServer.ApplicationInfo GetApplication(int packageId, string applicationId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesApplicationsInstaller/GetApplication", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesApplicationsInstaller/GetApplicationResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ApplicationInfo> GetApplicationAsync(int packageId, string applicationId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesApplicationsInstaller/RequestGalleryWebAppInstall", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesApplicationsInstaller/RequestGalleryWebAppInstallResponse")]
        SolidCP.Providers.WebAppGallery.GalleryWebAppStatus RequestGalleryWebAppInstall(int packageId, string webAppId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesApplicationsInstaller/RequestGalleryWebAppInstall", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesApplicationsInstaller/RequestGalleryWebAppInstallResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.WebAppGallery.GalleryWebAppStatus> RequestGalleryWebAppInstallAsync(int packageId, string webAppId);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esApplicationsInstallerAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesApplicationsInstaller
    {
        public int InstallApplication(SolidCP.EnterpriseServer.InstallationInfo inst)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esApplicationsInstaller", "InstallApplication", inst);
        }

        public async System.Threading.Tasks.Task<int> InstallApplicationAsync(SolidCP.EnterpriseServer.InstallationInfo inst)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esApplicationsInstaller", "InstallApplication", inst);
        }

        public SolidCP.EnterpriseServer.ApplicationInfo[] /*List*/ GetApplications(int packageId, string categoryId)
        {
            return Invoke<SolidCP.EnterpriseServer.ApplicationInfo[], SolidCP.EnterpriseServer.ApplicationInfo>("SolidCP.EnterpriseServer.esApplicationsInstaller", "GetApplications", packageId, categoryId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ApplicationInfo[]> GetApplicationsAsync(int packageId, string categoryId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ApplicationInfo[], SolidCP.EnterpriseServer.ApplicationInfo>("SolidCP.EnterpriseServer.esApplicationsInstaller", "GetApplications", packageId, categoryId);
        }

        public SolidCP.EnterpriseServer.ApplicationCategory[] /*List*/ GetCategories()
        {
            return Invoke<SolidCP.EnterpriseServer.ApplicationCategory[], SolidCP.EnterpriseServer.ApplicationCategory>("SolidCP.EnterpriseServer.esApplicationsInstaller", "GetCategories");
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ApplicationCategory[]> GetCategoriesAsync()
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ApplicationCategory[], SolidCP.EnterpriseServer.ApplicationCategory>("SolidCP.EnterpriseServer.esApplicationsInstaller", "GetCategories");
        }

        public SolidCP.EnterpriseServer.ApplicationInfo GetApplication(int packageId, string applicationId)
        {
            return Invoke<SolidCP.EnterpriseServer.ApplicationInfo>("SolidCP.EnterpriseServer.esApplicationsInstaller", "GetApplication", packageId, applicationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ApplicationInfo> GetApplicationAsync(int packageId, string applicationId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ApplicationInfo>("SolidCP.EnterpriseServer.esApplicationsInstaller", "GetApplication", packageId, applicationId);
        }

        public SolidCP.Providers.WebAppGallery.GalleryWebAppStatus RequestGalleryWebAppInstall(int packageId, string webAppId)
        {
            return Invoke<SolidCP.Providers.WebAppGallery.GalleryWebAppStatus>("SolidCP.EnterpriseServer.esApplicationsInstaller", "RequestGalleryWebAppInstall", packageId, webAppId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.WebAppGallery.GalleryWebAppStatus> RequestGalleryWebAppInstallAsync(int packageId, string webAppId)
        {
            return await InvokeAsync<SolidCP.Providers.WebAppGallery.GalleryWebAppStatus>("SolidCP.EnterpriseServer.esApplicationsInstaller", "RequestGalleryWebAppInstall", packageId, webAppId);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esApplicationsInstaller : SolidCP.Web.Client.ClientBase<IesApplicationsInstaller, esApplicationsInstallerAssemblyClient>, IesApplicationsInstaller
    {
        public int InstallApplication(SolidCP.EnterpriseServer.InstallationInfo inst)
        {
            return base.Client.InstallApplication(inst);
        }

        public async System.Threading.Tasks.Task<int> InstallApplicationAsync(SolidCP.EnterpriseServer.InstallationInfo inst)
        {
            return await base.Client.InstallApplicationAsync(inst);
        }

        public SolidCP.EnterpriseServer.ApplicationInfo[] /*List*/ GetApplications(int packageId, string categoryId)
        {
            return base.Client.GetApplications(packageId, categoryId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ApplicationInfo[]> GetApplicationsAsync(int packageId, string categoryId)
        {
            return await base.Client.GetApplicationsAsync(packageId, categoryId);
        }

        public SolidCP.EnterpriseServer.ApplicationCategory[] /*List*/ GetCategories()
        {
            return base.Client.GetCategories();
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ApplicationCategory[]> GetCategoriesAsync()
        {
            return await base.Client.GetCategoriesAsync();
        }

        public SolidCP.EnterpriseServer.ApplicationInfo GetApplication(int packageId, string applicationId)
        {
            return base.Client.GetApplication(packageId, applicationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ApplicationInfo> GetApplicationAsync(int packageId, string applicationId)
        {
            return await base.Client.GetApplicationAsync(packageId, applicationId);
        }

        public SolidCP.Providers.WebAppGallery.GalleryWebAppStatus RequestGalleryWebAppInstall(int packageId, string webAppId)
        {
            return base.Client.RequestGalleryWebAppInstall(packageId, webAppId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.WebAppGallery.GalleryWebAppStatus> RequestGalleryWebAppInstallAsync(int packageId, string webAppId)
        {
            return await base.Client.RequestGalleryWebAppInstallAsync(packageId, webAppId);
        }
    }
}
#endif