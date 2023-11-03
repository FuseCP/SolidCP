#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.HeliconZoo;
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
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesHeliconZoo
    {
        [WebMethod]
        [OperationContract]
        HeliconZooEngine[] GetEngines(int serviceId);
        [WebMethod]
        [OperationContract]
        void SetEngines(int serviceId, HeliconZooEngine[] userEngines);
        [WebMethod]
        [OperationContract]
        bool IsEnginesEnabled(int serviceId);
        [WebMethod]
        [OperationContract]
        void SwithEnginesEnabled(int serviceId, bool enabled);
        [WebMethod]
        [OperationContract]
        ShortHeliconZooEngine[] GetAllowedHeliconZooQuotasForPackage(int packageId);
        [WebMethod]
        [OperationContract]
        string[] GetEnabledEnginesForSite(string siteId, int packageId);
        [WebMethod]
        [OperationContract]
        void SetEnabledEnginesForSite(string siteId, int packageId, string[] engines);
        [WebMethod]
        [OperationContract]
        bool IsWebCosoleEnabled(int serviceId);
        [WebMethod]
        [OperationContract]
        void SetWebCosoleEnabled(int serviceId, bool enabled);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esHeliconZoo : SolidCP.EnterpriseServer.esHeliconZoo, IesHeliconZoo
    {
    }
}
#endif