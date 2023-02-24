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
#if NET6_0
using CoreWCF;
#endif
#if !NET6_0
using System.ServiceModel;
#endif

namespace SolidCP.Server.Client
{
    /// <summary>
    /// Summary description for ServiceProvider
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
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

    public class ServiceProvider
    {
        ChannelFactory<T> _Factory { get; set; }

        public Credentials Credentials { get; set; }

        public object SoapHeader { get; set; }

        void Test()
        {
            try
            {
                var client = _Factory.CreateChannel();
                client.MyServiceOperation();
                ((ICommunicationObject)client).Close();
                _Factory.Close();
            }
            catch
            {
                (client as ICommunicationObject)?.Abort();
            }
        }
    }
}
#endif