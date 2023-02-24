#if Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;
using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Server.Utils;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.WebAppGallery;
using SolidCP.Providers.Common;
using SolidCP.Providers.HeliconZoo;
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
    /// Summary description for HeliconZoo
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
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

    public class HeliconZoo
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