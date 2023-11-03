#if !Client
using System;
using System.Collections.Generic;
using SolidCP.Web.Services;
using SolidCP.Providers.WebAppGallery;
using SolidCP.Providers.ResultObjects;
using SolidCP.EnterpriseServer;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif

namespace SolidCP.EnterpriseServer.Services
{
    // wcf service contract
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [Policy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://tempuri.org/")]
    public interface IesWebApplicationGallery
    {
        [WebMethod]
        [OperationContract]
        void InitFeeds(int packageId);
        [WebMethod]
        [OperationContract]
        void SetResourceLanguage(int packageId, string resourceLanguage);
        [WebMethod]
        [OperationContract]
        GalleryLanguagesResult GetGalleryLanguages(int packageId);
        [WebMethod]
        [OperationContract]
        GalleryApplicationsResult GetGalleryApplicationsByServiceId(int serviceId);
        [WebMethod]
        [OperationContract]
        GalleryApplicationsResult GetGalleryApplications(int packageId, string categoryId);
        [WebMethod]
        [OperationContract]
        GalleryApplicationsResult GetInstaledApplications(int packageId, string categoryId);
        [WebMethod]
        [OperationContract]
        GalleryApplicationsResult GetGalleryApplicationsFiltered(int packageId, string pattern);
        [WebMethod]
        [OperationContract]
        GalleryCategoriesResult GetGalleryCategories(int packageId);
        [WebMethod]
        [OperationContract]
        GalleryApplicationResult GetGalleryApplicationDetails(int packageId, string applicationId);
        [WebMethod]
        [OperationContract]
        DeploymentParametersResult GetGalleryApplicationParams(int packageId, string applicationId);
        [WebMethod]
        [OperationContract]
        StringResultObject Install(int packageId, string webAppId, string siteName, string virtualDir, List<DeploymentParameter> parameters, string languageId);
        [WebMethod(Description = "Returns Web Application Gallery application status, such as Downloaded, Downloading, Failed or NotDownloaded. Throws an ApplicationException if WAG module is not available on the target server.")]
        [OperationContract]
        GalleryWebAppStatus GetGalleryApplicationStatus(int packageId, string webAppId);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esWebApplicationGallery : SolidCP.EnterpriseServer.esWebApplicationGallery, IesWebApplicationGallery
    {
    }
}
#endif