#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using SolidCP.Web.Services;
using SolidCP.Providers.WebAppGallery;
using SolidCP.EnterpriseServer;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif

namespace SolidCP.EnterpriseServer.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //[Policy(*EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesApplicationsInstaller
    {
        [WebMethod]
        [OperationContract]
        int InstallApplication(InstallationInfo inst);
        [WebMethod]
        [OperationContract]
        List<ApplicationInfo> GetApplications(int packageId, string categoryId);
        [WebMethod]
        [OperationContract]
        List<ApplicationCategory> GetCategories();
        [WebMethod]
        [OperationContract]
        ApplicationInfo GetApplication(int packageId, string applicationId);
        [WebMethod]
        [OperationContract]
        GalleryWebAppStatus RequestGalleryWebAppInstall(int packageId, string webAppId);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esApplicationsInstaller : SolidCP.EnterpriseServer.esApplicationsInstaller, IesApplicationsInstaller
    {
    }
}
#endif