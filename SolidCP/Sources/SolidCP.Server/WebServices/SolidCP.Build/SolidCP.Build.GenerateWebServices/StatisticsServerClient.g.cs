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
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IStatisticsServer", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IStatisticsServer
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStatisticsServer/GetServers", ReplyAction = "http://smbsaas/solidcp/server/IStatisticsServer/GetServersResponse")]
        StatsServer[] GetServers();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStatisticsServer/GetServers", ReplyAction = "http://smbsaas/solidcp/server/IStatisticsServer/GetServersResponse")]
        System.Threading.Tasks.Task<StatsServer[]> GetServersAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStatisticsServer/GetSiteId", ReplyAction = "http://smbsaas/solidcp/server/IStatisticsServer/GetSiteIdResponse")]
        string GetSiteId(string siteName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStatisticsServer/GetSiteId", ReplyAction = "http://smbsaas/solidcp/server/IStatisticsServer/GetSiteIdResponse")]
        System.Threading.Tasks.Task<string> GetSiteIdAsync(string siteName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStatisticsServer/GetSites", ReplyAction = "http://smbsaas/solidcp/server/IStatisticsServer/GetSitesResponse")]
        string[] GetSites();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStatisticsServer/GetSites", ReplyAction = "http://smbsaas/solidcp/server/IStatisticsServer/GetSitesResponse")]
        System.Threading.Tasks.Task<string[]> GetSitesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStatisticsServer/GetSite", ReplyAction = "http://smbsaas/solidcp/server/IStatisticsServer/GetSiteResponse")]
        StatsSite GetSite(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStatisticsServer/GetSite", ReplyAction = "http://smbsaas/solidcp/server/IStatisticsServer/GetSiteResponse")]
        System.Threading.Tasks.Task<StatsSite> GetSiteAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStatisticsServer/AddSite", ReplyAction = "http://smbsaas/solidcp/server/IStatisticsServer/AddSiteResponse")]
        string AddSite(StatsSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStatisticsServer/AddSite", ReplyAction = "http://smbsaas/solidcp/server/IStatisticsServer/AddSiteResponse")]
        System.Threading.Tasks.Task<string> AddSiteAsync(StatsSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStatisticsServer/UpdateSite", ReplyAction = "http://smbsaas/solidcp/server/IStatisticsServer/UpdateSiteResponse")]
        void UpdateSite(StatsSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStatisticsServer/UpdateSite", ReplyAction = "http://smbsaas/solidcp/server/IStatisticsServer/UpdateSiteResponse")]
        System.Threading.Tasks.Task UpdateSiteAsync(StatsSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStatisticsServer/DeleteSite", ReplyAction = "http://smbsaas/solidcp/server/IStatisticsServer/DeleteSiteResponse")]
        void DeleteSite(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStatisticsServer/DeleteSite", ReplyAction = "http://smbsaas/solidcp/server/IStatisticsServer/DeleteSiteResponse")]
        System.Threading.Tasks.Task DeleteSiteAsync(string siteId);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class StatisticsServerAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IStatisticsServer
    {
        public StatsServer[] GetServers()
        {
            return (StatsServer[])Invoke("SolidCP.Server.StatisticsServer", "GetServers");
        }

        public async System.Threading.Tasks.Task<StatsServer[]> GetServersAsync()
        {
            return await InvokeAsync<StatsServer[]>("SolidCP.Server.StatisticsServer", "GetServers");
        }

        public string GetSiteId(string siteName)
        {
            return (string)Invoke("SolidCP.Server.StatisticsServer", "GetSiteId", siteName);
        }

        public async System.Threading.Tasks.Task<string> GetSiteIdAsync(string siteName)
        {
            return await InvokeAsync<string>("SolidCP.Server.StatisticsServer", "GetSiteId", siteName);
        }

        public string[] GetSites()
        {
            return (string[])Invoke("SolidCP.Server.StatisticsServer", "GetSites");
        }

        public async System.Threading.Tasks.Task<string[]> GetSitesAsync()
        {
            return await InvokeAsync<string[]>("SolidCP.Server.StatisticsServer", "GetSites");
        }

        public StatsSite GetSite(string siteId)
        {
            return (StatsSite)Invoke("SolidCP.Server.StatisticsServer", "GetSite", siteId);
        }

        public async System.Threading.Tasks.Task<StatsSite> GetSiteAsync(string siteId)
        {
            return await InvokeAsync<StatsSite>("SolidCP.Server.StatisticsServer", "GetSite", siteId);
        }

        public string AddSite(StatsSite site)
        {
            return (string)Invoke("SolidCP.Server.StatisticsServer", "AddSite", site);
        }

        public async System.Threading.Tasks.Task<string> AddSiteAsync(StatsSite site)
        {
            return await InvokeAsync<string>("SolidCP.Server.StatisticsServer", "AddSite", site);
        }

        public void UpdateSite(StatsSite site)
        {
            Invoke("SolidCP.Server.StatisticsServer", "UpdateSite", site);
        }

        public async System.Threading.Tasks.Task UpdateSiteAsync(StatsSite site)
        {
            await InvokeAsync("SolidCP.Server.StatisticsServer", "UpdateSite", site);
        }

        public void DeleteSite(string siteId)
        {
            Invoke("SolidCP.Server.StatisticsServer", "DeleteSite", siteId);
        }

        public async System.Threading.Tasks.Task DeleteSiteAsync(string siteId)
        {
            await InvokeAsync("SolidCP.Server.StatisticsServer", "DeleteSite", siteId);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class StatisticsServer : SolidCP.Web.Client.ClientBase<IStatisticsServer, StatisticsServerAssemblyClient>, IStatisticsServer
    {
        public StatsServer[] GetServers()
        {
            return base.Client.GetServers();
        }

        public async System.Threading.Tasks.Task<StatsServer[]> GetServersAsync()
        {
            return await base.Client.GetServersAsync();
        }

        public string GetSiteId(string siteName)
        {
            return base.Client.GetSiteId(siteName);
        }

        public async System.Threading.Tasks.Task<string> GetSiteIdAsync(string siteName)
        {
            return await base.Client.GetSiteIdAsync(siteName);
        }

        public string[] GetSites()
        {
            return base.Client.GetSites();
        }

        public async System.Threading.Tasks.Task<string[]> GetSitesAsync()
        {
            return await base.Client.GetSitesAsync();
        }

        public StatsSite GetSite(string siteId)
        {
            return base.Client.GetSite(siteId);
        }

        public async System.Threading.Tasks.Task<StatsSite> GetSiteAsync(string siteId)
        {
            return await base.Client.GetSiteAsync(siteId);
        }

        public string AddSite(StatsSite site)
        {
            return base.Client.AddSite(site);
        }

        public async System.Threading.Tasks.Task<string> AddSiteAsync(StatsSite site)
        {
            return await base.Client.AddSiteAsync(site);
        }

        public void UpdateSite(StatsSite site)
        {
            base.Client.UpdateSite(site);
        }

        public async System.Threading.Tasks.Task UpdateSiteAsync(StatsSite site)
        {
            await base.Client.UpdateSiteAsync(site);
        }

        public void DeleteSite(string siteId)
        {
            base.Client.DeleteSite(siteId);
        }

        public async System.Threading.Tasks.Task DeleteSiteAsync(string siteId)
        {
            await base.Client.DeleteSiteAsync(siteId);
        }
    }
}
#endif