#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
//using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;
using SolidCP.Providers;
using SolidCP.Server.Utils;
using SolidCP.Server;
using System.ServiceModel;
using System.ServiceModel.Activation;

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
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ServiceProvider : SolidCP.Server.ServiceProvider, IServiceProvider
    {
        public new string[] Install()
        {
            return base.Install();
        }

        public new SettingPair[] GetProviderDefaultSettings()
        {
            return base.GetProviderDefaultSettings();
        }

        public new void Uninstall()
        {
            base.Uninstall();
        }

        public new bool IsInstalled()
        {
            return base.IsInstalled();
        }

        public new void ChangeServiceItemsState(SoapServiceProviderItem[] items, bool enabled)
        {
            base.ChangeServiceItemsState(items, enabled);
        }

        public new void DeleteServiceItems(SoapServiceProviderItem[] items)
        {
            base.DeleteServiceItems(items);
        }

        public new ServiceProviderItemDiskSpace[] GetServiceItemsDiskSpace(SoapServiceProviderItem[] items)
        {
            return base.GetServiceItemsDiskSpace(items);
        }

        public new ServiceProviderItemBandwidth[] GetServiceItemsBandwidth(SoapServiceProviderItem[] items, DateTime since)
        {
            return base.GetServiceItemsBandwidth(items, since);
        }
    }
}
#endif