#if !Client
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
using System.ServiceModel;

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [ServiceContract]
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

    // wcf service
    public class StatisticsServerService : StatisticsServer, IStatisticsServer
    {
        public new StatsServer[] GetServers()
        {
            return base.GetServers();
        }

        public new string GetSiteId(string siteName)
        {
            return base.GetSiteId(siteName);
        }

        public new string[] GetSites()
        {
            return base.GetSites();
        }

        public new StatsSite GetSite(string siteId)
        {
            return base.GetSite(siteId);
        }

        public new string AddSite(StatsSite site)
        {
            return base.AddSite(site);
        }

        public new void UpdateSite(StatsSite site)
        {
            base.UpdateSite(site);
        }

        public new void DeleteSite(string siteId)
        {
            base.DeleteSite(siteId);
        }
    }
}
#endif