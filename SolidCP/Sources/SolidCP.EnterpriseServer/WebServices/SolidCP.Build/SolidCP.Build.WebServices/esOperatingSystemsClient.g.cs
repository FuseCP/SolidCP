#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesOperatingSystems", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesOperatingSystems
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/GetRawOdbcSourcesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/GetRawOdbcSourcesPagedResponse")]
        System.Data.DataSet GetRawOdbcSourcesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/GetRawOdbcSourcesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/GetRawOdbcSourcesPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawOdbcSourcesPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/GetInstalledOdbcDrivers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/GetInstalledOdbcDriversResponse")]
        string[] GetInstalledOdbcDrivers(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/GetInstalledOdbcDrivers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/GetInstalledOdbcDriversResponse")]
        System.Threading.Tasks.Task<string[]> GetInstalledOdbcDriversAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/GetOdbcSources", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/GetOdbcSourcesResponse")]
        SolidCP.Providers.OS.SystemDSN[] /*List*/ GetOdbcSources(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/GetOdbcSources", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/GetOdbcSourcesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemDSN[]> GetOdbcSourcesAsync(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/GetOdbcSource", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/GetOdbcSourceResponse")]
        SolidCP.Providers.OS.SystemDSN GetOdbcSource(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/GetOdbcSource", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/GetOdbcSourceResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemDSN> GetOdbcSourceAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/AddOdbcSource", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/AddOdbcSourceResponse")]
        int AddOdbcSource(SolidCP.Providers.OS.SystemDSN item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/AddOdbcSource", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/AddOdbcSourceResponse")]
        System.Threading.Tasks.Task<int> AddOdbcSourceAsync(SolidCP.Providers.OS.SystemDSN item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/UpdateOdbcSource", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/UpdateOdbcSourceResponse")]
        int UpdateOdbcSource(SolidCP.Providers.OS.SystemDSN item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/UpdateOdbcSource", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/UpdateOdbcSourceResponse")]
        System.Threading.Tasks.Task<int> UpdateOdbcSourceAsync(SolidCP.Providers.OS.SystemDSN item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/DeleteOdbcSource", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/DeleteOdbcSourceResponse")]
        int DeleteOdbcSource(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/DeleteOdbcSource", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/DeleteOdbcSourceResponse")]
        System.Threading.Tasks.Task<int> DeleteOdbcSourceAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/CheckFileServicesInstallation", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/CheckFileServicesInstallationResponse")]
        bool CheckFileServicesInstallation(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/CheckFileServicesInstallation", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesOperatingSystems/CheckFileServicesInstallationResponse")]
        System.Threading.Tasks.Task<bool> CheckFileServicesInstallationAsync(int serviceId);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esOperatingSystemsAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesOperatingSystems
    {
        public System.Data.DataSet GetRawOdbcSourcesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esOperatingSystems", "GetRawOdbcSourcesPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawOdbcSourcesPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esOperatingSystems", "GetRawOdbcSourcesPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public string[] GetInstalledOdbcDrivers(int packageId)
        {
            return Invoke<string[]>("SolidCP.EnterpriseServer.esOperatingSystems", "GetInstalledOdbcDrivers", packageId);
        }

        public async System.Threading.Tasks.Task<string[]> GetInstalledOdbcDriversAsync(int packageId)
        {
            return await InvokeAsync<string[]>("SolidCP.EnterpriseServer.esOperatingSystems", "GetInstalledOdbcDrivers", packageId);
        }

        public SolidCP.Providers.OS.SystemDSN[] /*List*/ GetOdbcSources(int packageId, bool recursive)
        {
            return Invoke<SolidCP.Providers.OS.SystemDSN[], SolidCP.Providers.OS.SystemDSN>("SolidCP.EnterpriseServer.esOperatingSystems", "GetOdbcSources", packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemDSN[]> GetOdbcSourcesAsync(int packageId, bool recursive)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemDSN[], SolidCP.Providers.OS.SystemDSN>("SolidCP.EnterpriseServer.esOperatingSystems", "GetOdbcSources", packageId, recursive);
        }

        public SolidCP.Providers.OS.SystemDSN GetOdbcSource(int itemId)
        {
            return Invoke<SolidCP.Providers.OS.SystemDSN>("SolidCP.EnterpriseServer.esOperatingSystems", "GetOdbcSource", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemDSN> GetOdbcSourceAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemDSN>("SolidCP.EnterpriseServer.esOperatingSystems", "GetOdbcSource", itemId);
        }

        public int AddOdbcSource(SolidCP.Providers.OS.SystemDSN item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOperatingSystems", "AddOdbcSource", item);
        }

        public async System.Threading.Tasks.Task<int> AddOdbcSourceAsync(SolidCP.Providers.OS.SystemDSN item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOperatingSystems", "AddOdbcSource", item);
        }

        public int UpdateOdbcSource(SolidCP.Providers.OS.SystemDSN item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOperatingSystems", "UpdateOdbcSource", item);
        }

        public async System.Threading.Tasks.Task<int> UpdateOdbcSourceAsync(SolidCP.Providers.OS.SystemDSN item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOperatingSystems", "UpdateOdbcSource", item);
        }

        public int DeleteOdbcSource(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOperatingSystems", "DeleteOdbcSource", itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteOdbcSourceAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOperatingSystems", "DeleteOdbcSource", itemId);
        }

        public bool CheckFileServicesInstallation(int serviceId)
        {
            return Invoke<bool>("SolidCP.EnterpriseServer.esOperatingSystems", "CheckFileServicesInstallation", serviceId);
        }

        public async System.Threading.Tasks.Task<bool> CheckFileServicesInstallationAsync(int serviceId)
        {
            return await InvokeAsync<bool>("SolidCP.EnterpriseServer.esOperatingSystems", "CheckFileServicesInstallation", serviceId);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esOperatingSystems : SolidCP.Web.Client.ClientBase<IesOperatingSystems, esOperatingSystemsAssemblyClient>, IesOperatingSystems
    {
        public System.Data.DataSet GetRawOdbcSourcesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetRawOdbcSourcesPaged(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawOdbcSourcesPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetRawOdbcSourcesPagedAsync(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public string[] GetInstalledOdbcDrivers(int packageId)
        {
            return base.Client.GetInstalledOdbcDrivers(packageId);
        }

        public async System.Threading.Tasks.Task<string[]> GetInstalledOdbcDriversAsync(int packageId)
        {
            return await base.Client.GetInstalledOdbcDriversAsync(packageId);
        }

        public SolidCP.Providers.OS.SystemDSN[] /*List*/ GetOdbcSources(int packageId, bool recursive)
        {
            return base.Client.GetOdbcSources(packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemDSN[]> GetOdbcSourcesAsync(int packageId, bool recursive)
        {
            return await base.Client.GetOdbcSourcesAsync(packageId, recursive);
        }

        public SolidCP.Providers.OS.SystemDSN GetOdbcSource(int itemId)
        {
            return base.Client.GetOdbcSource(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemDSN> GetOdbcSourceAsync(int itemId)
        {
            return await base.Client.GetOdbcSourceAsync(itemId);
        }

        public int AddOdbcSource(SolidCP.Providers.OS.SystemDSN item)
        {
            return base.Client.AddOdbcSource(item);
        }

        public async System.Threading.Tasks.Task<int> AddOdbcSourceAsync(SolidCP.Providers.OS.SystemDSN item)
        {
            return await base.Client.AddOdbcSourceAsync(item);
        }

        public int UpdateOdbcSource(SolidCP.Providers.OS.SystemDSN item)
        {
            return base.Client.UpdateOdbcSource(item);
        }

        public async System.Threading.Tasks.Task<int> UpdateOdbcSourceAsync(SolidCP.Providers.OS.SystemDSN item)
        {
            return await base.Client.UpdateOdbcSourceAsync(item);
        }

        public int DeleteOdbcSource(int itemId)
        {
            return base.Client.DeleteOdbcSource(itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteOdbcSourceAsync(int itemId)
        {
            return await base.Client.DeleteOdbcSourceAsync(itemId);
        }

        public bool CheckFileServicesInstallation(int serviceId)
        {
            return base.Client.CheckFileServicesInstallation(serviceId);
        }

        public async System.Threading.Tasks.Task<bool> CheckFileServicesInstallationAsync(int serviceId)
        {
            return await base.Client.CheckFileServicesInstallationAsync(serviceId);
        }
    }
}
#endif