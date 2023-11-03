#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesWebApplicationGallery", Namespace = "http://tempuri.org/")]
    public interface IesWebApplicationGallery
    {
        [OperationContract(Action = "http://tempuri.org/IesWebApplicationGallery/InitFeeds", ReplyAction = "http://tempuri.org/IesWebApplicationGallery/InitFeedsResponse")]
        void InitFeeds(int packageId);
        [OperationContract(Action = "http://tempuri.org/IesWebApplicationGallery/InitFeeds", ReplyAction = "http://tempuri.org/IesWebApplicationGallery/InitFeedsResponse")]
        System.Threading.Tasks.Task InitFeedsAsync(int packageId);
        [OperationContract(Action = "http://tempuri.org/IesWebApplicationGallery/SetResourceLanguage", ReplyAction = "http://tempuri.org/IesWebApplicationGallery/SetResourceLanguageResponse")]
        void SetResourceLanguage(int packageId, string resourceLanguage);
        [OperationContract(Action = "http://tempuri.org/IesWebApplicationGallery/SetResourceLanguage", ReplyAction = "http://tempuri.org/IesWebApplicationGallery/SetResourceLanguageResponse")]
        System.Threading.Tasks.Task SetResourceLanguageAsync(int packageId, string resourceLanguage);
        [OperationContract(Action = "http://tempuri.org/IesWebApplicationGallery/GetGalleryLanguages", ReplyAction = "http://tempuri.org/IesWebApplicationGallery/GetGalleryLanguagesResponse")]
        SolidCP.Providers.ResultObjects.GalleryLanguagesResult GetGalleryLanguages(int packageId);
        [OperationContract(Action = "http://tempuri.org/IesWebApplicationGallery/GetGalleryLanguages", ReplyAction = "http://tempuri.org/IesWebApplicationGallery/GetGalleryLanguagesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryLanguagesResult> GetGalleryLanguagesAsync(int packageId);
        [OperationContract(Action = "http://tempuri.org/IesWebApplicationGallery/GetGalleryApplicationsByServiceId", ReplyAction = "http://tempuri.org/IesWebApplicationGallery/GetGalleryApplicationsByServiceIdResponse")]
        SolidCP.Providers.ResultObjects.GalleryApplicationsResult GetGalleryApplicationsByServiceId(int serviceId);
        [OperationContract(Action = "http://tempuri.org/IesWebApplicationGallery/GetGalleryApplicationsByServiceId", ReplyAction = "http://tempuri.org/IesWebApplicationGallery/GetGalleryApplicationsByServiceIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryApplicationsResult> GetGalleryApplicationsByServiceIdAsync(int serviceId);
        [OperationContract(Action = "http://tempuri.org/IesWebApplicationGallery/GetGalleryApplications", ReplyAction = "http://tempuri.org/IesWebApplicationGallery/GetGalleryApplicationsResponse")]
        SolidCP.Providers.ResultObjects.GalleryApplicationsResult GetGalleryApplications(int packageId, string categoryId);
        [OperationContract(Action = "http://tempuri.org/IesWebApplicationGallery/GetGalleryApplications", ReplyAction = "http://tempuri.org/IesWebApplicationGallery/GetGalleryApplicationsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryApplicationsResult> GetGalleryApplicationsAsync(int packageId, string categoryId);
        [OperationContract(Action = "http://tempuri.org/IesWebApplicationGallery/GetInstaledApplications", ReplyAction = "http://tempuri.org/IesWebApplicationGallery/GetInstaledApplicationsResponse")]
        SolidCP.Providers.ResultObjects.GalleryApplicationsResult GetInstaledApplications(int packageId, string categoryId);
        [OperationContract(Action = "http://tempuri.org/IesWebApplicationGallery/GetInstaledApplications", ReplyAction = "http://tempuri.org/IesWebApplicationGallery/GetInstaledApplicationsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryApplicationsResult> GetInstaledApplicationsAsync(int packageId, string categoryId);
        [OperationContract(Action = "http://tempuri.org/IesWebApplicationGallery/GetGalleryApplicationsFiltered", ReplyAction = "http://tempuri.org/IesWebApplicationGallery/GetGalleryApplicationsFilteredResponse")]
        SolidCP.Providers.ResultObjects.GalleryApplicationsResult GetGalleryApplicationsFiltered(int packageId, string pattern);
        [OperationContract(Action = "http://tempuri.org/IesWebApplicationGallery/GetGalleryApplicationsFiltered", ReplyAction = "http://tempuri.org/IesWebApplicationGallery/GetGalleryApplicationsFilteredResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryApplicationsResult> GetGalleryApplicationsFilteredAsync(int packageId, string pattern);
        [OperationContract(Action = "http://tempuri.org/IesWebApplicationGallery/GetGalleryCategories", ReplyAction = "http://tempuri.org/IesWebApplicationGallery/GetGalleryCategoriesResponse")]
        SolidCP.Providers.ResultObjects.GalleryCategoriesResult GetGalleryCategories(int packageId);
        [OperationContract(Action = "http://tempuri.org/IesWebApplicationGallery/GetGalleryCategories", ReplyAction = "http://tempuri.org/IesWebApplicationGallery/GetGalleryCategoriesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryCategoriesResult> GetGalleryCategoriesAsync(int packageId);
        [OperationContract(Action = "http://tempuri.org/IesWebApplicationGallery/GetGalleryApplicationDetails", ReplyAction = "http://tempuri.org/IesWebApplicationGallery/GetGalleryApplicationDetailsResponse")]
        SolidCP.Providers.ResultObjects.GalleryApplicationResult GetGalleryApplicationDetails(int packageId, string applicationId);
        [OperationContract(Action = "http://tempuri.org/IesWebApplicationGallery/GetGalleryApplicationDetails", ReplyAction = "http://tempuri.org/IesWebApplicationGallery/GetGalleryApplicationDetailsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryApplicationResult> GetGalleryApplicationDetailsAsync(int packageId, string applicationId);
        [OperationContract(Action = "http://tempuri.org/IesWebApplicationGallery/GetGalleryApplicationParams", ReplyAction = "http://tempuri.org/IesWebApplicationGallery/GetGalleryApplicationParamsResponse")]
        SolidCP.Providers.ResultObjects.DeploymentParametersResult GetGalleryApplicationParams(int packageId, string applicationId);
        [OperationContract(Action = "http://tempuri.org/IesWebApplicationGallery/GetGalleryApplicationParams", ReplyAction = "http://tempuri.org/IesWebApplicationGallery/GetGalleryApplicationParamsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.DeploymentParametersResult> GetGalleryApplicationParamsAsync(int packageId, string applicationId);
        [OperationContract(Action = "http://tempuri.org/IesWebApplicationGallery/Install", ReplyAction = "http://tempuri.org/IesWebApplicationGallery/InstallResponse")]
        SolidCP.Providers.ResultObjects.StringResultObject Install(int packageId, string webAppId, string siteName, string virtualDir, SolidCP.Providers.WebAppGallery.DeploymentParameter[] /*List*/ parameters, string languageId);
        [OperationContract(Action = "http://tempuri.org/IesWebApplicationGallery/Install", ReplyAction = "http://tempuri.org/IesWebApplicationGallery/InstallResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringResultObject> InstallAsync(int packageId, string webAppId, string siteName, string virtualDir, SolidCP.Providers.WebAppGallery.DeploymentParameter[] /*List*/ parameters, string languageId);
        [OperationContract(Action = "http://tempuri.org/IesWebApplicationGallery/GetGalleryApplicationStatus", ReplyAction = "http://tempuri.org/IesWebApplicationGallery/GetGalleryApplicationStatusResponse")]
        SolidCP.Providers.WebAppGallery.GalleryWebAppStatus GetGalleryApplicationStatus(int packageId, string webAppId);
        [OperationContract(Action = "http://tempuri.org/IesWebApplicationGallery/GetGalleryApplicationStatus", ReplyAction = "http://tempuri.org/IesWebApplicationGallery/GetGalleryApplicationStatusResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.WebAppGallery.GalleryWebAppStatus> GetGalleryApplicationStatusAsync(int packageId, string webAppId);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esWebApplicationGalleryAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesWebApplicationGallery
    {
        public void InitFeeds(int packageId)
        {
            Invoke("SolidCP.EnterpriseServer.esWebApplicationGallery", "InitFeeds", packageId);
        }

        public async System.Threading.Tasks.Task InitFeedsAsync(int packageId)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esWebApplicationGallery", "InitFeeds", packageId);
        }

        public void SetResourceLanguage(int packageId, string resourceLanguage)
        {
            Invoke("SolidCP.EnterpriseServer.esWebApplicationGallery", "SetResourceLanguage", packageId, resourceLanguage);
        }

        public async System.Threading.Tasks.Task SetResourceLanguageAsync(int packageId, string resourceLanguage)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esWebApplicationGallery", "SetResourceLanguage", packageId, resourceLanguage);
        }

        public SolidCP.Providers.ResultObjects.GalleryLanguagesResult GetGalleryLanguages(int packageId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.GalleryLanguagesResult>("SolidCP.EnterpriseServer.esWebApplicationGallery", "GetGalleryLanguages", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryLanguagesResult> GetGalleryLanguagesAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.GalleryLanguagesResult>("SolidCP.EnterpriseServer.esWebApplicationGallery", "GetGalleryLanguages", packageId);
        }

        public SolidCP.Providers.ResultObjects.GalleryApplicationsResult GetGalleryApplicationsByServiceId(int serviceId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.GalleryApplicationsResult>("SolidCP.EnterpriseServer.esWebApplicationGallery", "GetGalleryApplicationsByServiceId", serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryApplicationsResult> GetGalleryApplicationsByServiceIdAsync(int serviceId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.GalleryApplicationsResult>("SolidCP.EnterpriseServer.esWebApplicationGallery", "GetGalleryApplicationsByServiceId", serviceId);
        }

        public SolidCP.Providers.ResultObjects.GalleryApplicationsResult GetGalleryApplications(int packageId, string categoryId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.GalleryApplicationsResult>("SolidCP.EnterpriseServer.esWebApplicationGallery", "GetGalleryApplications", packageId, categoryId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryApplicationsResult> GetGalleryApplicationsAsync(int packageId, string categoryId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.GalleryApplicationsResult>("SolidCP.EnterpriseServer.esWebApplicationGallery", "GetGalleryApplications", packageId, categoryId);
        }

        public SolidCP.Providers.ResultObjects.GalleryApplicationsResult GetInstaledApplications(int packageId, string categoryId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.GalleryApplicationsResult>("SolidCP.EnterpriseServer.esWebApplicationGallery", "GetInstaledApplications", packageId, categoryId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryApplicationsResult> GetInstaledApplicationsAsync(int packageId, string categoryId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.GalleryApplicationsResult>("SolidCP.EnterpriseServer.esWebApplicationGallery", "GetInstaledApplications", packageId, categoryId);
        }

        public SolidCP.Providers.ResultObjects.GalleryApplicationsResult GetGalleryApplicationsFiltered(int packageId, string pattern)
        {
            return Invoke<SolidCP.Providers.ResultObjects.GalleryApplicationsResult>("SolidCP.EnterpriseServer.esWebApplicationGallery", "GetGalleryApplicationsFiltered", packageId, pattern);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryApplicationsResult> GetGalleryApplicationsFilteredAsync(int packageId, string pattern)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.GalleryApplicationsResult>("SolidCP.EnterpriseServer.esWebApplicationGallery", "GetGalleryApplicationsFiltered", packageId, pattern);
        }

        public SolidCP.Providers.ResultObjects.GalleryCategoriesResult GetGalleryCategories(int packageId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.GalleryCategoriesResult>("SolidCP.EnterpriseServer.esWebApplicationGallery", "GetGalleryCategories", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryCategoriesResult> GetGalleryCategoriesAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.GalleryCategoriesResult>("SolidCP.EnterpriseServer.esWebApplicationGallery", "GetGalleryCategories", packageId);
        }

        public SolidCP.Providers.ResultObjects.GalleryApplicationResult GetGalleryApplicationDetails(int packageId, string applicationId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.GalleryApplicationResult>("SolidCP.EnterpriseServer.esWebApplicationGallery", "GetGalleryApplicationDetails", packageId, applicationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryApplicationResult> GetGalleryApplicationDetailsAsync(int packageId, string applicationId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.GalleryApplicationResult>("SolidCP.EnterpriseServer.esWebApplicationGallery", "GetGalleryApplicationDetails", packageId, applicationId);
        }

        public SolidCP.Providers.ResultObjects.DeploymentParametersResult GetGalleryApplicationParams(int packageId, string applicationId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.DeploymentParametersResult>("SolidCP.EnterpriseServer.esWebApplicationGallery", "GetGalleryApplicationParams", packageId, applicationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.DeploymentParametersResult> GetGalleryApplicationParamsAsync(int packageId, string applicationId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.DeploymentParametersResult>("SolidCP.EnterpriseServer.esWebApplicationGallery", "GetGalleryApplicationParams", packageId, applicationId);
        }

        public SolidCP.Providers.ResultObjects.StringResultObject Install(int packageId, string webAppId, string siteName, string virtualDir, SolidCP.Providers.WebAppGallery.DeploymentParameter[] /*List*/ parameters, string languageId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.StringResultObject>("SolidCP.EnterpriseServer.esWebApplicationGallery", "Install", packageId, webAppId, siteName, virtualDir, parameters.ToList(), languageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringResultObject> InstallAsync(int packageId, string webAppId, string siteName, string virtualDir, SolidCP.Providers.WebAppGallery.DeploymentParameter[] /*List*/ parameters, string languageId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.StringResultObject>("SolidCP.EnterpriseServer.esWebApplicationGallery", "Install", packageId, webAppId, siteName, virtualDir, parameters, languageId);
        }

        public SolidCP.Providers.WebAppGallery.GalleryWebAppStatus GetGalleryApplicationStatus(int packageId, string webAppId)
        {
            return Invoke<SolidCP.Providers.WebAppGallery.GalleryWebAppStatus>("SolidCP.EnterpriseServer.esWebApplicationGallery", "GetGalleryApplicationStatus", packageId, webAppId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.WebAppGallery.GalleryWebAppStatus> GetGalleryApplicationStatusAsync(int packageId, string webAppId)
        {
            return await InvokeAsync<SolidCP.Providers.WebAppGallery.GalleryWebAppStatus>("SolidCP.EnterpriseServer.esWebApplicationGallery", "GetGalleryApplicationStatus", packageId, webAppId);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esWebApplicationGallery : SolidCP.Web.Client.ClientBase<IesWebApplicationGallery, esWebApplicationGalleryAssemblyClient>, IesWebApplicationGallery
    {
        public void InitFeeds(int packageId)
        {
            base.Client.InitFeeds(packageId);
        }

        public async System.Threading.Tasks.Task InitFeedsAsync(int packageId)
        {
            await base.Client.InitFeedsAsync(packageId);
        }

        public void SetResourceLanguage(int packageId, string resourceLanguage)
        {
            base.Client.SetResourceLanguage(packageId, resourceLanguage);
        }

        public async System.Threading.Tasks.Task SetResourceLanguageAsync(int packageId, string resourceLanguage)
        {
            await base.Client.SetResourceLanguageAsync(packageId, resourceLanguage);
        }

        public SolidCP.Providers.ResultObjects.GalleryLanguagesResult GetGalleryLanguages(int packageId)
        {
            return base.Client.GetGalleryLanguages(packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryLanguagesResult> GetGalleryLanguagesAsync(int packageId)
        {
            return await base.Client.GetGalleryLanguagesAsync(packageId);
        }

        public SolidCP.Providers.ResultObjects.GalleryApplicationsResult GetGalleryApplicationsByServiceId(int serviceId)
        {
            return base.Client.GetGalleryApplicationsByServiceId(serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryApplicationsResult> GetGalleryApplicationsByServiceIdAsync(int serviceId)
        {
            return await base.Client.GetGalleryApplicationsByServiceIdAsync(serviceId);
        }

        public SolidCP.Providers.ResultObjects.GalleryApplicationsResult GetGalleryApplications(int packageId, string categoryId)
        {
            return base.Client.GetGalleryApplications(packageId, categoryId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryApplicationsResult> GetGalleryApplicationsAsync(int packageId, string categoryId)
        {
            return await base.Client.GetGalleryApplicationsAsync(packageId, categoryId);
        }

        public SolidCP.Providers.ResultObjects.GalleryApplicationsResult GetInstaledApplications(int packageId, string categoryId)
        {
            return base.Client.GetInstaledApplications(packageId, categoryId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryApplicationsResult> GetInstaledApplicationsAsync(int packageId, string categoryId)
        {
            return await base.Client.GetInstaledApplicationsAsync(packageId, categoryId);
        }

        public SolidCP.Providers.ResultObjects.GalleryApplicationsResult GetGalleryApplicationsFiltered(int packageId, string pattern)
        {
            return base.Client.GetGalleryApplicationsFiltered(packageId, pattern);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryApplicationsResult> GetGalleryApplicationsFilteredAsync(int packageId, string pattern)
        {
            return await base.Client.GetGalleryApplicationsFilteredAsync(packageId, pattern);
        }

        public SolidCP.Providers.ResultObjects.GalleryCategoriesResult GetGalleryCategories(int packageId)
        {
            return base.Client.GetGalleryCategories(packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryCategoriesResult> GetGalleryCategoriesAsync(int packageId)
        {
            return await base.Client.GetGalleryCategoriesAsync(packageId);
        }

        public SolidCP.Providers.ResultObjects.GalleryApplicationResult GetGalleryApplicationDetails(int packageId, string applicationId)
        {
            return base.Client.GetGalleryApplicationDetails(packageId, applicationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryApplicationResult> GetGalleryApplicationDetailsAsync(int packageId, string applicationId)
        {
            return await base.Client.GetGalleryApplicationDetailsAsync(packageId, applicationId);
        }

        public SolidCP.Providers.ResultObjects.DeploymentParametersResult GetGalleryApplicationParams(int packageId, string applicationId)
        {
            return base.Client.GetGalleryApplicationParams(packageId, applicationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.DeploymentParametersResult> GetGalleryApplicationParamsAsync(int packageId, string applicationId)
        {
            return await base.Client.GetGalleryApplicationParamsAsync(packageId, applicationId);
        }

        public SolidCP.Providers.ResultObjects.StringResultObject Install(int packageId, string webAppId, string siteName, string virtualDir, SolidCP.Providers.WebAppGallery.DeploymentParameter[] /*List*/ parameters, string languageId)
        {
            return base.Client.Install(packageId, webAppId, siteName, virtualDir, parameters, languageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringResultObject> InstallAsync(int packageId, string webAppId, string siteName, string virtualDir, SolidCP.Providers.WebAppGallery.DeploymentParameter[] /*List*/ parameters, string languageId)
        {
            return await base.Client.InstallAsync(packageId, webAppId, siteName, virtualDir, parameters, languageId);
        }

        public SolidCP.Providers.WebAppGallery.GalleryWebAppStatus GetGalleryApplicationStatus(int packageId, string webAppId)
        {
            return base.Client.GetGalleryApplicationStatus(packageId, webAppId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.WebAppGallery.GalleryWebAppStatus> GetGalleryApplicationStatusAsync(int packageId, string webAppId)
        {
            return await base.Client.GetGalleryApplicationStatusAsync(packageId, webAppId);
        }
    }
}
#endif