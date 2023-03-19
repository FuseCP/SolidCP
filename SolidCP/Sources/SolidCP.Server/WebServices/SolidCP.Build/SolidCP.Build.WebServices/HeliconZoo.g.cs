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
using SolidCP.Server.Utils;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.WebAppGallery;
using SolidCP.Providers.Common;
using SolidCP.Providers.HeliconZoo;
using SolidCP.Server;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/server/")]
    public interface IHeliconZoo
    {
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        HeliconZooEngine[] GetEngines();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetEngines(HeliconZooEngine[] userEngines);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool IsEnginesEnabled();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SwithEnginesEnabled(bool enabled);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string[] GetEnabledEnginesForSite(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetEnabledEnginesForSite(string siteId, string[] engineNames);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool IsWebCosoleEnabled();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetWebCosoleEnabled(bool enabled);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class HeliconZoo : SolidCP.Server.HeliconZoo, IHeliconZoo
    {
    }
}
#endif