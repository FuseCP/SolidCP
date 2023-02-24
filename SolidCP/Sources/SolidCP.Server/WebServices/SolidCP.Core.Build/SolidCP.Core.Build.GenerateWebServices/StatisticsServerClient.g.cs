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
using SolidCP.Providers.Statistics;
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
    /// Summary description for StatisticsServer
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/server/")]
    public interface IStatisticsServer
    {

        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        StatsServer[] GetServers();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string GetSiteId(string siteName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string[] GetSites();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        StatsSite GetSite(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string AddSite(StatsSite site);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateSite(StatsSite site);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteSite(string siteId);
    }

    public class StatisticsServer
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