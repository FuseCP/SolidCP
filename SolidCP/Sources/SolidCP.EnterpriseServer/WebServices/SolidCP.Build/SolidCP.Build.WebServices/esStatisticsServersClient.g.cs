#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesStatisticsServers", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesStatisticsServers
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/GetRawStatisticsSitesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/GetRawStatisticsSitesPagedResponse")]
        System.Data.DataSet GetRawStatisticsSitesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/GetRawStatisticsSitesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/GetRawStatisticsSitesPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawStatisticsSitesPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/GetStatisticsSites", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/GetStatisticsSitesResponse")]
        SolidCP.Providers.Statistics.StatsSite[] /*List*/ GetStatisticsSites(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/GetStatisticsSites", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/GetStatisticsSitesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Statistics.StatsSite[]> GetStatisticsSitesAsync(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/GetServers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/GetServersResponse")]
        SolidCP.Providers.Statistics.StatsServer[] GetServers(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/GetServers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/GetServersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Statistics.StatsServer[]> GetServersAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/GetSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/GetSiteResponse")]
        SolidCP.Providers.Statistics.StatsSite GetSite(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/GetSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/GetSiteResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Statistics.StatsSite> GetSiteAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/AddSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/AddSiteResponse")]
        int AddSite(SolidCP.Providers.Statistics.StatsSite item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/AddSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/AddSiteResponse")]
        System.Threading.Tasks.Task<int> AddSiteAsync(SolidCP.Providers.Statistics.StatsSite item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/UpdateSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/UpdateSiteResponse")]
        int UpdateSite(SolidCP.Providers.Statistics.StatsSite item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/UpdateSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/UpdateSiteResponse")]
        System.Threading.Tasks.Task<int> UpdateSiteAsync(SolidCP.Providers.Statistics.StatsSite item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/DeleteSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/DeleteSiteResponse")]
        int DeleteSite(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/DeleteSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesStatisticsServers/DeleteSiteResponse")]
        System.Threading.Tasks.Task<int> DeleteSiteAsync(int itemId);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esStatisticsServersAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesStatisticsServers
    {
        public System.Data.DataSet GetRawStatisticsSitesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esStatisticsServers", "GetRawStatisticsSitesPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawStatisticsSitesPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esStatisticsServers", "GetRawStatisticsSitesPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Statistics.StatsSite[] /*List*/ GetStatisticsSites(int packageId, bool recursive)
        {
            return Invoke<SolidCP.Providers.Statistics.StatsSite[], SolidCP.Providers.Statistics.StatsSite>("SolidCP.EnterpriseServer.esStatisticsServers", "GetStatisticsSites", packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Statistics.StatsSite[]> GetStatisticsSitesAsync(int packageId, bool recursive)
        {
            return await InvokeAsync<SolidCP.Providers.Statistics.StatsSite[], SolidCP.Providers.Statistics.StatsSite>("SolidCP.EnterpriseServer.esStatisticsServers", "GetStatisticsSites", packageId, recursive);
        }

        public SolidCP.Providers.Statistics.StatsServer[] GetServers(int serviceId)
        {
            return Invoke<SolidCP.Providers.Statistics.StatsServer[]>("SolidCP.EnterpriseServer.esStatisticsServers", "GetServers", serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Statistics.StatsServer[]> GetServersAsync(int serviceId)
        {
            return await InvokeAsync<SolidCP.Providers.Statistics.StatsServer[]>("SolidCP.EnterpriseServer.esStatisticsServers", "GetServers", serviceId);
        }

        public SolidCP.Providers.Statistics.StatsSite GetSite(int itemId)
        {
            return Invoke<SolidCP.Providers.Statistics.StatsSite>("SolidCP.EnterpriseServer.esStatisticsServers", "GetSite", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Statistics.StatsSite> GetSiteAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Statistics.StatsSite>("SolidCP.EnterpriseServer.esStatisticsServers", "GetSite", itemId);
        }

        public int AddSite(SolidCP.Providers.Statistics.StatsSite item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esStatisticsServers", "AddSite", item);
        }

        public async System.Threading.Tasks.Task<int> AddSiteAsync(SolidCP.Providers.Statistics.StatsSite item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esStatisticsServers", "AddSite", item);
        }

        public int UpdateSite(SolidCP.Providers.Statistics.StatsSite item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esStatisticsServers", "UpdateSite", item);
        }

        public async System.Threading.Tasks.Task<int> UpdateSiteAsync(SolidCP.Providers.Statistics.StatsSite item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esStatisticsServers", "UpdateSite", item);
        }

        public int DeleteSite(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esStatisticsServers", "DeleteSite", itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteSiteAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esStatisticsServers", "DeleteSite", itemId);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esStatisticsServers : SolidCP.Web.Client.ClientBase<IesStatisticsServers, esStatisticsServersAssemblyClient>, IesStatisticsServers
    {
        public System.Data.DataSet GetRawStatisticsSitesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetRawStatisticsSitesPaged(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawStatisticsSitesPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetRawStatisticsSitesPagedAsync(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Statistics.StatsSite[] /*List*/ GetStatisticsSites(int packageId, bool recursive)
        {
            return base.Client.GetStatisticsSites(packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Statistics.StatsSite[]> GetStatisticsSitesAsync(int packageId, bool recursive)
        {
            return await base.Client.GetStatisticsSitesAsync(packageId, recursive);
        }

        public SolidCP.Providers.Statistics.StatsServer[] GetServers(int serviceId)
        {
            return base.Client.GetServers(serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Statistics.StatsServer[]> GetServersAsync(int serviceId)
        {
            return await base.Client.GetServersAsync(serviceId);
        }

        public SolidCP.Providers.Statistics.StatsSite GetSite(int itemId)
        {
            return base.Client.GetSite(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Statistics.StatsSite> GetSiteAsync(int itemId)
        {
            return await base.Client.GetSiteAsync(itemId);
        }

        public int AddSite(SolidCP.Providers.Statistics.StatsSite item)
        {
            return base.Client.AddSite(item);
        }

        public async System.Threading.Tasks.Task<int> AddSiteAsync(SolidCP.Providers.Statistics.StatsSite item)
        {
            return await base.Client.AddSiteAsync(item);
        }

        public int UpdateSite(SolidCP.Providers.Statistics.StatsSite item)
        {
            return base.Client.UpdateSite(item);
        }

        public async System.Threading.Tasks.Task<int> UpdateSiteAsync(SolidCP.Providers.Statistics.StatsSite item)
        {
            return await base.Client.UpdateSiteAsync(item);
        }

        public int DeleteSite(int itemId)
        {
            return base.Client.DeleteSite(itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteSiteAsync(int itemId)
        {
            return await base.Client.DeleteSiteAsync(itemId);
        }
    }
}
#endif