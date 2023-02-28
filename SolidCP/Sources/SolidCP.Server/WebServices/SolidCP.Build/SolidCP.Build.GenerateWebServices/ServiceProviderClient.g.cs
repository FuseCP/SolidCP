#if Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;
using SolidCP.Providers;
using SolidCP.Server.Utils;
using SolidCP.Server;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [ServiceContract]
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
}
#endif