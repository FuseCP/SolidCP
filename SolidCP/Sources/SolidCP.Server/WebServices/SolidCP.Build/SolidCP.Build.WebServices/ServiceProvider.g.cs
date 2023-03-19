#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers;
using SolidCP.Server.Utils;
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
    public interface IServiceProvider
    {
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string[] Install();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SettingPair[] GetProviderDefaultSettings();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void Uninstall();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool IsInstalled();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ChangeServiceItemsState(SoapServiceProviderItem[] items, bool enabled);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteServiceItems(SoapServiceProviderItem[] items);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ServiceProviderItemDiskSpace[] GetServiceItemsDiskSpace(SoapServiceProviderItem[] items);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ServiceProviderItemBandwidth[] GetServiceItemsBandwidth(SoapServiceProviderItem[] items, DateTime since);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class ServiceProvider : SolidCP.Server.ServiceProvider, IServiceProvider
    {
    }
}
#endif