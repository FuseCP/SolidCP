#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesServers", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesServers
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetAllServers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetAllServersResponse")]
        SolidCP.EnterpriseServer.ServerInfo[] /*List*/ GetAllServers();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetAllServers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetAllServersResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServerInfo[]> GetAllServersAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawAllServers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawAllServersResponse")]
        System.Data.DataSet GetRawAllServers();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawAllServers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawAllServersResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawAllServersAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServersResponse")]
        SolidCP.EnterpriseServer.ServerInfo[] /*List*/ GetServers();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServersResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServerInfo[]> GetServersAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawServers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawServersResponse")]
        System.Data.DataSet GetRawServers();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawServers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawServersResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawServersAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServerShortDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServerShortDetailsResponse")]
        SolidCP.EnterpriseServer.ServerInfo GetServerShortDetails(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServerShortDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServerShortDetailsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServerInfo> GetServerShortDetailsAsync(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServerById", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServerByIdResponse")]
        SolidCP.EnterpriseServer.ServerInfo GetServerById(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServerById", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServerByIdResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServerInfo> GetServerByIdAsync(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServerByName", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServerByNameResponse")]
        SolidCP.EnterpriseServer.ServerInfo GetServerByName(string serverName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServerByName", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServerByNameResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServerInfo> GetServerByNameAsync(string serverName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/CheckServerAvailable", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/CheckServerAvailableResponse")]
        int CheckServerAvailable(string serverUrl, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/CheckServerAvailable", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/CheckServerAvailableResponse")]
        System.Threading.Tasks.Task<int> CheckServerAvailableAsync(string serverUrl, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddServerResponse")]
        int AddServer(SolidCP.EnterpriseServer.ServerInfo server, bool autoDiscovery);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddServerResponse")]
        System.Threading.Tasks.Task<int> AddServerAsync(SolidCP.EnterpriseServer.ServerInfo server, bool autoDiscovery);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateServerResponse")]
        int UpdateServer(SolidCP.EnterpriseServer.ServerInfo server);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateServerResponse")]
        System.Threading.Tasks.Task<int> UpdateServerAsync(SolidCP.EnterpriseServer.ServerInfo server);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateServerConnectionPassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateServerConnectionPasswordResponse")]
        int UpdateServerConnectionPassword(int serverId, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateServerConnectionPassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateServerConnectionPasswordResponse")]
        System.Threading.Tasks.Task<int> UpdateServerConnectionPasswordAsync(int serverId, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateServerADPassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateServerADPasswordResponse")]
        int UpdateServerADPassword(int serverId, string adPassword);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateServerADPassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateServerADPasswordResponse")]
        System.Threading.Tasks.Task<int> UpdateServerADPasswordAsync(int serverId, string adPassword);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteServerResponse")]
        int DeleteServer(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteServerResponse")]
        System.Threading.Tasks.Task<int> DeleteServerAsync(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AutoUpdateServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AutoUpdateServerResponse")]
        string[] /*List*/ AutoUpdateServer(int[] /*List*/[] /*List*/ servers, string downloadLink, string fileName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AutoUpdateServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AutoUpdateServerResponse")]
        System.Threading.Tasks.Task<string[]> AutoUpdateServerAsync(int[] /*List*/[] /*List*/ servers, string downloadLink, string fileName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetVirtualServers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetVirtualServersResponse")]
        System.Data.DataSet GetVirtualServers();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetVirtualServers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetVirtualServersResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetVirtualServersAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetAvailableVirtualServices", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetAvailableVirtualServicesResponse")]
        System.Data.DataSet GetAvailableVirtualServices(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetAvailableVirtualServices", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetAvailableVirtualServicesResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetAvailableVirtualServicesAsync(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetVirtualServices", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetVirtualServicesResponse")]
        System.Data.DataSet GetVirtualServices(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetVirtualServices", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetVirtualServicesResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetVirtualServicesAsync(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddVirtualServices", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddVirtualServicesResponse")]
        int AddVirtualServices(int serverId, int[] ids);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddVirtualServices", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddVirtualServicesResponse")]
        System.Threading.Tasks.Task<int> AddVirtualServicesAsync(int serverId, int[] ids);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteVirtualServices", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteVirtualServicesResponse")]
        int DeleteVirtualServices(int serverId, int[] ids);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteVirtualServices", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteVirtualServicesResponse")]
        System.Threading.Tasks.Task<int> DeleteVirtualServicesAsync(int serverId, int[] ids);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateVirtualGroups", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateVirtualGroupsResponse")]
        int UpdateVirtualGroups(int serverId, SolidCP.EnterpriseServer.VirtualGroupInfo[] groups);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateVirtualGroups", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateVirtualGroupsResponse")]
        System.Threading.Tasks.Task<int> UpdateVirtualGroupsAsync(int serverId, SolidCP.EnterpriseServer.VirtualGroupInfo[] groups);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawServicesByServerId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawServicesByServerIdResponse")]
        System.Data.DataSet GetRawServicesByServerId(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawServicesByServerId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawServicesByServerIdResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawServicesByServerIdAsync(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServicesByServerId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServicesByServerIdResponse")]
        SolidCP.EnterpriseServer.ServiceInfo[] /*List*/ GetServicesByServerId(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServicesByServerId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServicesByServerIdResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServiceInfo[]> GetServicesByServerIdAsync(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServicesByServerIdGroupName", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServicesByServerIdGroupNameResponse")]
        SolidCP.EnterpriseServer.ServiceInfo[] /*List*/ GetServicesByServerIdGroupName(int serverId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServicesByServerIdGroupName", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServicesByServerIdGroupNameResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServiceInfo[]> GetServicesByServerIdGroupNameAsync(int serverId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawServicesByGroupId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawServicesByGroupIdResponse")]
        System.Data.DataSet GetRawServicesByGroupId(int groupId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawServicesByGroupId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawServicesByGroupIdResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawServicesByGroupIdAsync(int groupId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawServicesByGroupName", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawServicesByGroupNameResponse")]
        System.Data.DataSet GetRawServicesByGroupName(string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawServicesByGroupName", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawServicesByGroupNameResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawServicesByGroupNameAsync(string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServiceInfo", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServiceInfoResponse")]
        SolidCP.EnterpriseServer.ServiceInfo GetServiceInfo(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServiceInfo", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServiceInfoResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServiceInfo> GetServiceInfoAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddService", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddServiceResponse")]
        int AddService(SolidCP.EnterpriseServer.ServiceInfo service);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddService", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddServiceResponse")]
        System.Threading.Tasks.Task<int> AddServiceAsync(SolidCP.EnterpriseServer.ServiceInfo service);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateService", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateServiceResponse")]
        int UpdateService(SolidCP.EnterpriseServer.ServiceInfo service);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateService", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateServiceResponse")]
        System.Threading.Tasks.Task<int> UpdateServiceAsync(SolidCP.EnterpriseServer.ServiceInfo service);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteService", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteServiceResponse")]
        int DeleteService(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteService", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteServiceResponse")]
        System.Threading.Tasks.Task<int> DeleteServiceAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServiceSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServiceSettingsResponse")]
        string[] GetServiceSettings(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServiceSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServiceSettingsResponse")]
        System.Threading.Tasks.Task<string[]> GetServiceSettingsAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServiceSettingsRDS", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServiceSettingsRDSResponse")]
        string[] GetServiceSettingsRDS(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServiceSettingsRDS", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServiceSettingsRDSResponse")]
        System.Threading.Tasks.Task<string[]> GetServiceSettingsRDSAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateServiceSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateServiceSettingsResponse")]
        int UpdateServiceSettings(int serviceId, string[] settings);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateServiceSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateServiceSettingsResponse")]
        System.Threading.Tasks.Task<int> UpdateServiceSettingsAsync(int serviceId, string[] settings);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/InstallService", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/InstallServiceResponse")]
        string[] InstallService(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/InstallService", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/InstallServiceResponse")]
        System.Threading.Tasks.Task<string[]> InstallServiceAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetProviderServiceQuota", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetProviderServiceQuotaResponse")]
        SolidCP.EnterpriseServer.QuotaInfo GetProviderServiceQuota(int providerId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetProviderServiceQuota", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetProviderServiceQuotaResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.QuotaInfo> GetProviderServiceQuotaAsync(int providerId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetMailServiceSettingsByPackage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetMailServiceSettingsByPackageResponse")]
        string[] GetMailServiceSettingsByPackage(int packageID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetMailServiceSettingsByPackage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetMailServiceSettingsByPackageResponse")]
        System.Threading.Tasks.Task<string[]> GetMailServiceSettingsByPackageAsync(int packageID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetInstalledProviders", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetInstalledProvidersResponse")]
        SolidCP.EnterpriseServer.ProviderInfo[] /*List*/ GetInstalledProviders(int groupId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetInstalledProviders", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetInstalledProvidersResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ProviderInfo[]> GetInstalledProvidersAsync(int groupId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetResourceGroups", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetResourceGroupsResponse")]
        SolidCP.EnterpriseServer.ResourceGroupInfo[] /*List*/ GetResourceGroups();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetResourceGroups", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetResourceGroupsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ResourceGroupInfo[]> GetResourceGroupsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetResourceGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetResourceGroupResponse")]
        SolidCP.EnterpriseServer.ResourceGroupInfo GetResourceGroup(int groupId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetResourceGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetResourceGroupResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ResourceGroupInfo> GetResourceGroupAsync(int groupId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetProvider", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetProviderResponse")]
        SolidCP.EnterpriseServer.ProviderInfo GetProvider(int providerId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetProvider", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetProviderResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ProviderInfo> GetProviderAsync(int providerId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetProviders", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetProvidersResponse")]
        SolidCP.EnterpriseServer.ProviderInfo[] /*List*/ GetProviders();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetProviders", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetProvidersResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ProviderInfo[]> GetProvidersAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetProvidersByGroupId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetProvidersByGroupIdResponse")]
        SolidCP.EnterpriseServer.ProviderInfo[] /*List*/ GetProvidersByGroupId(int groupId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetProvidersByGroupId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetProvidersByGroupIdResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ProviderInfo[]> GetProvidersByGroupIdAsync(int groupId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPackageServiceProvider", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPackageServiceProviderResponse")]
        SolidCP.EnterpriseServer.ProviderInfo GetPackageServiceProvider(int packageId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPackageServiceProvider", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPackageServiceProviderResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ProviderInfo> GetPackageServiceProviderAsync(int packageId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetMailFilterUrl", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetMailFilterUrlResponse")]
        System.String GetMailFilterUrl(int packageId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetMailFilterUrl", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetMailFilterUrlResponse")]
        System.Threading.Tasks.Task<System.String> GetMailFilterUrlAsync(int packageId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetMailFilterUrlByHostingPlan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetMailFilterUrlByHostingPlanResponse")]
        System.String GetMailFilterUrlByHostingPlan(int PlanID, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetMailFilterUrlByHostingPlan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetMailFilterUrlByHostingPlanResponse")]
        System.Threading.Tasks.Task<System.String> GetMailFilterUrlByHostingPlanAsync(int PlanID, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/IsInstalled", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/IsInstalledResponse")]
        SolidCP.Providers.Common.BoolResult IsInstalled(int serverId, int providerId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/IsInstalled", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/IsInstalledResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.BoolResult> IsInstalledAsync(int serverId, int providerId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServerVersion", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServerVersionResponse")]
        string GetServerVersion(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServerVersion", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServerVersionResponse")]
        System.Threading.Tasks.Task<string> GetServerVersionAsync(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServerFilePath", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServerFilePathResponse")]
        string GetServerFilePath(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServerFilePath", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetServerFilePathResponse")]
        System.Threading.Tasks.Task<string> GetServerFilePathAsync(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPrivateNetworVLANsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPrivateNetworVLANsPagedResponse")]
        SolidCP.EnterpriseServer.VLANsPaged GetPrivateNetworVLANsPaged(int serverId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPrivateNetworVLANsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPrivateNetworVLANsPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VLANsPaged> GetPrivateNetworVLANsPagedAsync(int serverId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddPrivateNetworkVLAN", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddPrivateNetworkVLANResponse")]
        SolidCP.Providers.ResultObjects.IntResult AddPrivateNetworkVLAN(int serverId, int vlan, string comments);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddPrivateNetworkVLAN", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddPrivateNetworkVLANResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> AddPrivateNetworkVLANAsync(int serverId, int vlan, string comments);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeletePrivateNetworkVLANs", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeletePrivateNetworkVLANsResponse")]
        SolidCP.Providers.Common.ResultObject DeletePrivateNetworkVLANs(int[] vlans);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeletePrivateNetworkVLANs", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeletePrivateNetworkVLANsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeletePrivateNetworkVLANsAsync(int[] vlans);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddPrivateNetworkVLANsRange", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddPrivateNetworkVLANsRangeResponse")]
        SolidCP.Providers.Common.ResultObject AddPrivateNetworkVLANsRange(int serverId, int startVLAN, int endVLAN, string comments);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddPrivateNetworkVLANsRange", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddPrivateNetworkVLANsRangeResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddPrivateNetworkVLANsRangeAsync(int serverId, int startVLAN, int endVLAN, string comments);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPrivateNetworVLAN", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPrivateNetworVLANResponse")]
        SolidCP.EnterpriseServer.VLANInfo GetPrivateNetworVLAN(int vlanId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPrivateNetworVLAN", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPrivateNetworVLANResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VLANInfo> GetPrivateNetworVLANAsync(int vlanId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdatePrivateNetworVLAN", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdatePrivateNetworVLANResponse")]
        SolidCP.Providers.Common.ResultObject UpdatePrivateNetworVLAN(int vlanId, int serverId, int vlan, string comments);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdatePrivateNetworVLAN", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdatePrivateNetworVLANResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdatePrivateNetworVLANAsync(int vlanId, int serverId, int vlan, string comments);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPackagePrivateNetworkVLANs", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPackagePrivateNetworkVLANsResponse")]
        SolidCP.EnterpriseServer.PackageVLANsPaged GetPackagePrivateNetworkVLANs(int packageId, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPackagePrivateNetworkVLANs", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPackagePrivateNetworkVLANsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageVLANsPaged> GetPackagePrivateNetworkVLANsAsync(int packageId, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeallocatePackageVLANs", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeallocatePackageVLANsResponse")]
        SolidCP.Providers.Common.ResultObject DeallocatePackageVLANs(int packageId, int[] vlanId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeallocatePackageVLANs", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeallocatePackageVLANsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeallocatePackageVLANsAsync(int packageId, int[] vlanId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetUnallottedVLANs", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetUnallottedVLANsResponse")]
        SolidCP.EnterpriseServer.VLANInfo[] /*List*/ GetUnallottedVLANs(int packageId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetUnallottedVLANs", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetUnallottedVLANsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VLANInfo[]> GetUnallottedVLANsAsync(int packageId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AllocatePackageVLANs", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AllocatePackageVLANsResponse")]
        SolidCP.Providers.Common.ResultObject AllocatePackageVLANs(int packageId, string groupName, bool allocateRandom, int vlansNumber, int[] vlanId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AllocatePackageVLANs", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AllocatePackageVLANsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AllocatePackageVLANsAsync(int packageId, string groupName, bool allocateRandom, int vlansNumber, int[] vlanId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetIPAddressesResponse")]
        SolidCP.EnterpriseServer.IPAddressInfo[] /*List*/ GetIPAddresses(SolidCP.EnterpriseServer.IPAddressPool pool, int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.IPAddressInfo[]> GetIPAddressesAsync(SolidCP.EnterpriseServer.IPAddressPool pool, int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetIPAddressesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetIPAddressesPagedResponse")]
        SolidCP.EnterpriseServer.IPAddressesPaged GetIPAddressesPaged(SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetIPAddressesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetIPAddressesPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.IPAddressesPaged> GetIPAddressesPagedAsync(SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetIPAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetIPAddressResponse")]
        SolidCP.EnterpriseServer.IPAddressInfo GetIPAddress(int addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetIPAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetIPAddressResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.IPAddressInfo> GetIPAddressAsync(int addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddIPAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddIPAddressResponse")]
        SolidCP.Providers.ResultObjects.IntResult AddIPAddress(SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string externalIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddIPAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddIPAddressResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> AddIPAddressAsync(SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string externalIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddIPAddressesRange", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddIPAddressesRangeResponse")]
        SolidCP.Providers.Common.ResultObject AddIPAddressesRange(SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string externalIP, string endIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddIPAddressesRange", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddIPAddressesRangeResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddIPAddressesRangeAsync(SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string externalIP, string endIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateIPAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateIPAddressResponse")]
        SolidCP.Providers.Common.ResultObject UpdateIPAddress(int addressId, SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string externalIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateIPAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateIPAddressResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateIPAddressAsync(int addressId, SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string externalIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateIPAddressesResponse")]
        SolidCP.Providers.Common.ResultObject UpdateIPAddresses(int[] addresses, SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string subnetMask, string defaultGateway, string comments, int VLAN);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateIPAddressesAsync(int[] addresses, SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string subnetMask, string defaultGateway, string comments, int VLAN);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteIPAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteIPAddressResponse")]
        SolidCP.Providers.Common.ResultObject DeleteIPAddress(int addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteIPAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteIPAddressResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteIPAddressAsync(int addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteIPAddressesResponse")]
        SolidCP.Providers.Common.ResultObject DeleteIPAddresses(int[] addresses);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteIPAddressesAsync(int[] addresses);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetUnallottedIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetUnallottedIPAddressesResponse")]
        SolidCP.EnterpriseServer.IPAddressInfo[] /*List*/ GetUnallottedIPAddresses(int packageId, string groupName, SolidCP.EnterpriseServer.IPAddressPool pool);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetUnallottedIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetUnallottedIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.IPAddressInfo[]> GetUnallottedIPAddressesAsync(int packageId, string groupName, SolidCP.EnterpriseServer.IPAddressPool pool);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPackageIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPackageIPAddressesResponse")]
        SolidCP.EnterpriseServer.PackageIPAddressesPaged GetPackageIPAddresses(int packageId, int orgId, SolidCP.EnterpriseServer.IPAddressPool pool, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPackageIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPackageIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageIPAddressesPaged> GetPackageIPAddressesAsync(int packageId, int orgId, SolidCP.EnterpriseServer.IPAddressPool pool, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPackageIPAddressesCount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPackageIPAddressesCountResponse")]
        int GetPackageIPAddressesCount(int packageId, int orgId, SolidCP.EnterpriseServer.IPAddressPool pool);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPackageIPAddressesCount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPackageIPAddressesCountResponse")]
        System.Threading.Tasks.Task<int> GetPackageIPAddressesCountAsync(int packageId, int orgId, SolidCP.EnterpriseServer.IPAddressPool pool);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPackageUnassignedIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPackageUnassignedIPAddressesResponse")]
        SolidCP.EnterpriseServer.PackageIPAddress[] /*List*/ GetPackageUnassignedIPAddresses(int packageId, int orgId, SolidCP.EnterpriseServer.IPAddressPool pool);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPackageUnassignedIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetPackageUnassignedIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageIPAddress[]> GetPackageUnassignedIPAddressesAsync(int packageId, int orgId, SolidCP.EnterpriseServer.IPAddressPool pool);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AllocatePackageIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AllocatePackageIPAddressesResponse")]
        SolidCP.Providers.Common.ResultObject AllocatePackageIPAddresses(int packageId, int orgId, string groupName, SolidCP.EnterpriseServer.IPAddressPool pool, bool allocateRandom, int addressesNumber, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AllocatePackageIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AllocatePackageIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AllocatePackageIPAddressesAsync(int packageId, int orgId, string groupName, SolidCP.EnterpriseServer.IPAddressPool pool, bool allocateRandom, int addressesNumber, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AllocateMaximumPackageIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AllocateMaximumPackageIPAddressesResponse")]
        SolidCP.Providers.Common.ResultObject AllocateMaximumPackageIPAddresses(int packageId, string groupName, SolidCP.EnterpriseServer.IPAddressPool pool);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AllocateMaximumPackageIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AllocateMaximumPackageIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AllocateMaximumPackageIPAddressesAsync(int packageId, string groupName, SolidCP.EnterpriseServer.IPAddressPool pool);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeallocatePackageIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeallocatePackageIPAddressesResponse")]
        SolidCP.Providers.Common.ResultObject DeallocatePackageIPAddresses(int packageId, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeallocatePackageIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeallocatePackageIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeallocatePackageIPAddressesAsync(int packageId, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetClusters", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetClustersResponse")]
        SolidCP.EnterpriseServer.ClusterInfo[] /*List*/ GetClusters();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetClusters", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetClustersResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ClusterInfo[]> GetClustersAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddCluster", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddClusterResponse")]
        int AddCluster(SolidCP.EnterpriseServer.ClusterInfo cluster);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddCluster", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddClusterResponse")]
        System.Threading.Tasks.Task<int> AddClusterAsync(SolidCP.EnterpriseServer.ClusterInfo cluster);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteCluster", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteClusterResponse")]
        int DeleteCluster(int clusterId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteCluster", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteClusterResponse")]
        System.Threading.Tasks.Task<int> DeleteClusterAsync(int clusterId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawDnsRecordsByService", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawDnsRecordsByServiceResponse")]
        System.Data.DataSet GetRawDnsRecordsByService(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawDnsRecordsByService", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawDnsRecordsByServiceResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawDnsRecordsByServiceAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawDnsRecordsByServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawDnsRecordsByServerResponse")]
        System.Data.DataSet GetRawDnsRecordsByServer(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawDnsRecordsByServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawDnsRecordsByServerResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawDnsRecordsByServerAsync(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawDnsRecordsByPackage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawDnsRecordsByPackageResponse")]
        System.Data.DataSet GetRawDnsRecordsByPackage(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawDnsRecordsByPackage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawDnsRecordsByPackageResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawDnsRecordsByPackageAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawDnsRecordsByGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawDnsRecordsByGroupResponse")]
        System.Data.DataSet GetRawDnsRecordsByGroup(int groupId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawDnsRecordsByGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawDnsRecordsByGroupResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawDnsRecordsByGroupAsync(int groupId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDnsRecordsByService", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDnsRecordsByServiceResponse")]
        SolidCP.EnterpriseServer.GlobalDnsRecord[] /*List*/ GetDnsRecordsByService(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDnsRecordsByService", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDnsRecordsByServiceResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.GlobalDnsRecord[]> GetDnsRecordsByServiceAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDnsRecordsByServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDnsRecordsByServerResponse")]
        SolidCP.EnterpriseServer.GlobalDnsRecord[] /*List*/ GetDnsRecordsByServer(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDnsRecordsByServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDnsRecordsByServerResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.GlobalDnsRecord[]> GetDnsRecordsByServerAsync(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDnsRecordsByPackage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDnsRecordsByPackageResponse")]
        SolidCP.EnterpriseServer.GlobalDnsRecord[] /*List*/ GetDnsRecordsByPackage(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDnsRecordsByPackage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDnsRecordsByPackageResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.GlobalDnsRecord[]> GetDnsRecordsByPackageAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDnsRecordsByGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDnsRecordsByGroupResponse")]
        SolidCP.EnterpriseServer.GlobalDnsRecord[] /*List*/ GetDnsRecordsByGroup(int groupId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDnsRecordsByGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDnsRecordsByGroupResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.GlobalDnsRecord[]> GetDnsRecordsByGroupAsync(int groupId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDnsRecord", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDnsRecordResponse")]
        SolidCP.EnterpriseServer.GlobalDnsRecord GetDnsRecord(int recordId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDnsRecord", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDnsRecordResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.GlobalDnsRecord> GetDnsRecordAsync(int recordId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddDnsRecord", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddDnsRecordResponse")]
        int AddDnsRecord(SolidCP.EnterpriseServer.GlobalDnsRecord record);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddDnsRecord", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddDnsRecordResponse")]
        System.Threading.Tasks.Task<int> AddDnsRecordAsync(SolidCP.EnterpriseServer.GlobalDnsRecord record);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateDnsRecord", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateDnsRecordResponse")]
        int UpdateDnsRecord(SolidCP.EnterpriseServer.GlobalDnsRecord record);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateDnsRecord", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateDnsRecordResponse")]
        System.Threading.Tasks.Task<int> UpdateDnsRecordAsync(SolidCP.EnterpriseServer.GlobalDnsRecord record);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteDnsRecord", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteDnsRecordResponse")]
        int DeleteDnsRecord(int recordId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteDnsRecord", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteDnsRecordResponse")]
        System.Threading.Tasks.Task<int> DeleteDnsRecordAsync(int recordId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDomainDnsRecords", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDomainDnsRecordsResponse")]
        SolidCP.Providers.DomainLookup.DnsRecordInfo[] /*List*/ GetDomainDnsRecords(int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDomainDnsRecords", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDomainDnsRecordsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.DomainLookup.DnsRecordInfo[]> GetDomainDnsRecordsAsync(int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDomains", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDomainsResponse")]
        SolidCP.EnterpriseServer.DomainInfo[] /*List*/ GetDomains(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDomains", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDomainsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DomainInfo[]> GetDomainsAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDomainsByDomainId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDomainsByDomainIdResponse")]
        SolidCP.EnterpriseServer.DomainInfo[] /*List*/ GetDomainsByDomainId(int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDomainsByDomainId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDomainsByDomainIdResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DomainInfo[]> GetDomainsByDomainIdAsync(int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetMyDomains", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetMyDomainsResponse")]
        SolidCP.EnterpriseServer.DomainInfo[] /*List*/ GetMyDomains(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetMyDomains", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetMyDomainsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DomainInfo[]> GetMyDomainsAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetResellerDomains", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetResellerDomainsResponse")]
        SolidCP.EnterpriseServer.DomainInfo[] /*List*/ GetResellerDomains(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetResellerDomains", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetResellerDomainsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DomainInfo[]> GetResellerDomainsAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDomainsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDomainsPagedResponse")]
        System.Data.DataSet GetDomainsPaged(int packageId, int serverId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDomainsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDomainsPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetDomainsPagedAsync(int packageId, int serverId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDomainResponse")]
        SolidCP.EnterpriseServer.DomainInfo GetDomain(int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDomainResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DomainInfo> GetDomainAsync(int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddDomainResponse")]
        int AddDomain(SolidCP.EnterpriseServer.DomainInfo domain);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddDomainResponse")]
        System.Threading.Tasks.Task<int> AddDomainAsync(SolidCP.EnterpriseServer.DomainInfo domain);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddDomainWithProvisioning", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddDomainWithProvisioningResponse")]
        int AddDomainWithProvisioning(int packageId, string domainName, SolidCP.EnterpriseServer.DomainType domainType, bool createWebSite, int pointWebSiteId, int pointMailDomainId, bool createDnsZone, bool createPreviewDomain, bool allowSubDomains, string hostName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddDomainWithProvisioning", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddDomainWithProvisioningResponse")]
        System.Threading.Tasks.Task<int> AddDomainWithProvisioningAsync(int packageId, string domainName, SolidCP.EnterpriseServer.DomainType domainType, bool createWebSite, int pointWebSiteId, int pointMailDomainId, bool createDnsZone, bool createPreviewDomain, bool allowSubDomains, string hostName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateDomainResponse")]
        int UpdateDomain(SolidCP.EnterpriseServer.DomainInfo domain);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateDomainResponse")]
        System.Threading.Tasks.Task<int> UpdateDomainAsync(SolidCP.EnterpriseServer.DomainInfo domain);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteDomainResponse")]
        int DeleteDomain(int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteDomainResponse")]
        System.Threading.Tasks.Task<int> DeleteDomainAsync(int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DetachDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DetachDomainResponse")]
        int DetachDomain(int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DetachDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DetachDomainResponse")]
        System.Threading.Tasks.Task<int> DetachDomainAsync(int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/EnableDomainDns", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/EnableDomainDnsResponse")]
        int EnableDomainDns(int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/EnableDomainDns", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/EnableDomainDnsResponse")]
        System.Threading.Tasks.Task<int> EnableDomainDnsAsync(int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DisableDomainDns", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DisableDomainDnsResponse")]
        int DisableDomainDns(int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DisableDomainDns", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DisableDomainDnsResponse")]
        System.Threading.Tasks.Task<int> DisableDomainDnsAsync(int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/CreateDomainPreviewDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/CreateDomainPreviewDomainResponse")]
        int CreateDomainPreviewDomain(string hostName, int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/CreateDomainPreviewDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/CreateDomainPreviewDomainResponse")]
        System.Threading.Tasks.Task<int> CreateDomainPreviewDomainAsync(string hostName, int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteDomainPreviewDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteDomainPreviewDomainResponse")]
        int DeleteDomainPreviewDomain(int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteDomainPreviewDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteDomainPreviewDomainResponse")]
        System.Threading.Tasks.Task<int> DeleteDomainPreviewDomainAsync(int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDnsZoneRecords", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDnsZoneRecordsResponse")]
        SolidCP.Providers.DNS.DnsRecord[] GetDnsZoneRecords(int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDnsZoneRecords", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetDnsZoneRecordsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.DNS.DnsRecord[]> GetDnsZoneRecordsAsync(int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawDnsZoneRecords", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawDnsZoneRecordsResponse")]
        System.Data.DataSet GetRawDnsZoneRecords(int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawDnsZoneRecords", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetRawDnsZoneRecordsResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawDnsZoneRecordsAsync(int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddDnsZoneRecord", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddDnsZoneRecordResponse")]
        int AddDnsZoneRecord(int domainId, string recordName, SolidCP.Providers.DNS.DnsRecordType recordType, string recordData, int mxPriority, int srvPriority, int srvWeight, int srvPortNumber);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddDnsZoneRecord", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/AddDnsZoneRecordResponse")]
        System.Threading.Tasks.Task<int> AddDnsZoneRecordAsync(int domainId, string recordName, SolidCP.Providers.DNS.DnsRecordType recordType, string recordData, int mxPriority, int srvPriority, int srvWeight, int srvPortNumber);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateDnsZoneRecord", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateDnsZoneRecordResponse")]
        int UpdateDnsZoneRecord(int domainId, string originalRecordName, string originalRecordData, string recordName, SolidCP.Providers.DNS.DnsRecordType recordType, string recordData, int mxPriority, int srvPriority, int srvWeight, int srvPortNumber);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateDnsZoneRecord", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/UpdateDnsZoneRecordResponse")]
        System.Threading.Tasks.Task<int> UpdateDnsZoneRecordAsync(int domainId, string originalRecordName, string originalRecordData, string recordName, SolidCP.Providers.DNS.DnsRecordType recordType, string recordData, int mxPriority, int srvPriority, int srvWeight, int srvPortNumber);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteDnsZoneRecord", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteDnsZoneRecordResponse")]
        int DeleteDnsZoneRecord(int domainId, string recordName, SolidCP.Providers.DNS.DnsRecordType recordType, string recordData);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteDnsZoneRecord", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/DeleteDnsZoneRecordResponse")]
        System.Threading.Tasks.Task<int> DeleteDnsZoneRecordAsync(int domainId, string recordName, SolidCP.Providers.DNS.DnsRecordType recordType, string recordData);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetTerminalServicesSessions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetTerminalServicesSessionsResponse")]
        SolidCP.Providers.OS.TerminalSession[] GetTerminalServicesSessions(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetTerminalServicesSessions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetTerminalServicesSessionsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.TerminalSession[]> GetTerminalServicesSessionsAsync(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/CloseTerminalServicesSession", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/CloseTerminalServicesSessionResponse")]
        int CloseTerminalServicesSession(int serverId, int sessionId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/CloseTerminalServicesSession", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/CloseTerminalServicesSessionResponse")]
        System.Threading.Tasks.Task<int> CloseTerminalServicesSessionAsync(int serverId, int sessionId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetOSProcesses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetOSProcessesResponse")]
        SolidCP.Providers.OS.OSProcess[] GetOSProcesses(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetOSProcesses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetOSProcessesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.OSProcess[]> GetOSProcessesAsync(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/TerminateOSProcess", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/TerminateOSProcessResponse")]
        int TerminateOSProcess(int serverId, int pid);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/TerminateOSProcess", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/TerminateOSProcessResponse")]
        System.Threading.Tasks.Task<int> TerminateOSProcessAsync(int serverId, int pid);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/CheckLoadUserProfile", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/CheckLoadUserProfileResponse")]
        bool CheckLoadUserProfile(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/CheckLoadUserProfile", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/CheckLoadUserProfileResponse")]
        System.Threading.Tasks.Task<bool> CheckLoadUserProfileAsync(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/EnableLoadUserProfile", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/EnableLoadUserProfileResponse")]
        void EnableLoadUserProfile(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/EnableLoadUserProfile", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/EnableLoadUserProfileResponse")]
        System.Threading.Tasks.Task EnableLoadUserProfileAsync(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/InitWPIFeeds", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/InitWPIFeedsResponse")]
        void InitWPIFeeds(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/InitWPIFeeds", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/InitWPIFeedsResponse")]
        System.Threading.Tasks.Task InitWPIFeedsAsync(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPITabs", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPITabsResponse")]
        SolidCP.Server.WPITab[] GetWPITabs(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPITabs", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPITabsResponse")]
        System.Threading.Tasks.Task<SolidCP.Server.WPITab[]> GetWPITabsAsync(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPIKeywords", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPIKeywordsResponse")]
        SolidCP.Server.WPIKeyword[] GetWPIKeywords(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPIKeywords", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPIKeywordsResponse")]
        System.Threading.Tasks.Task<SolidCP.Server.WPIKeyword[]> GetWPIKeywordsAsync(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPIProducts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPIProductsResponse")]
        SolidCP.Server.WPIProduct[] GetWPIProducts(int serverId, string tabId, string keywordId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPIProducts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPIProductsResponse")]
        System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsAsync(int serverId, string tabId, string keywordId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPIProductsFiltered", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPIProductsFilteredResponse")]
        SolidCP.Server.WPIProduct[] GetWPIProductsFiltered(int serverId, string keywordId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPIProductsFiltered", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPIProductsFilteredResponse")]
        System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsFilteredAsync(int serverId, string keywordId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPIProductById", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPIProductByIdResponse")]
        SolidCP.Server.WPIProduct GetWPIProductById(int serverId, string productdId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPIProductById", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPIProductByIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Server.WPIProduct> GetWPIProductByIdAsync(int serverId, string productdId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPIProductsWithDependencies", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPIProductsWithDependenciesResponse")]
        SolidCP.Server.WPIProduct[] GetWPIProductsWithDependencies(int serverId, string[] products);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPIProductsWithDependencies", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPIProductsWithDependenciesResponse")]
        System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsWithDependenciesAsync(int serverId, string[] products);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/InstallWPIProducts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/InstallWPIProductsResponse")]
        void InstallWPIProducts(int serverId, string[] products);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/InstallWPIProducts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/InstallWPIProductsResponse")]
        System.Threading.Tasks.Task InstallWPIProductsAsync(int serverId, string[] products);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/CancelInstallWPIProducts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/CancelInstallWPIProductsResponse")]
        void CancelInstallWPIProducts(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/CancelInstallWPIProducts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/CancelInstallWPIProductsResponse")]
        System.Threading.Tasks.Task CancelInstallWPIProductsAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPIStatus", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPIStatusResponse")]
        string GetWPIStatus(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPIStatus", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetWPIStatusResponse")]
        System.Threading.Tasks.Task<string> GetWPIStatusAsync(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/WpiGetLogFileDirectory", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/WpiGetLogFileDirectoryResponse")]
        string WpiGetLogFileDirectory(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/WpiGetLogFileDirectory", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/WpiGetLogFileDirectoryResponse")]
        System.Threading.Tasks.Task<string> WpiGetLogFileDirectoryAsync(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/WpiGetLogsInDirectory", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/WpiGetLogsInDirectoryResponse")]
        SolidCP.Providers.SettingPair[] WpiGetLogsInDirectory(int serverId, string Path);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/WpiGetLogsInDirectory", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/WpiGetLogsInDirectoryResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.SettingPair[]> WpiGetLogsInDirectoryAsync(int serverId, string Path);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetOSServices", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetOSServicesResponse")]
        SolidCP.Providers.OS.OSService[] GetOSServices(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetOSServices", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetOSServicesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.OSService[]> GetOSServicesAsync(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/ChangeOSServiceStatus", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/ChangeOSServiceStatusResponse")]
        int ChangeOSServiceStatus(int serverId, string id, SolidCP.Providers.OS.OSServiceStatus status);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/ChangeOSServiceStatus", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/ChangeOSServiceStatusResponse")]
        System.Threading.Tasks.Task<int> ChangeOSServiceStatusAsync(int serverId, string id, SolidCP.Providers.OS.OSServiceStatus status);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetLogNames", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetLogNamesResponse")]
        string[] GetLogNames(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetLogNames", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetLogNamesResponse")]
        System.Threading.Tasks.Task<string[]> GetLogNamesAsync(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetLogEntries", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetLogEntriesResponse")]
        SolidCP.Providers.OS.SystemLogEntry[] GetLogEntries(int serverId, string logName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetLogEntries", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetLogEntriesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemLogEntry[]> GetLogEntriesAsync(int serverId, string logName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetLogEntriesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetLogEntriesPagedResponse")]
        SolidCP.Providers.OS.SystemLogEntriesPaged GetLogEntriesPaged(int serverId, string logName, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetLogEntriesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetLogEntriesPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemLogEntriesPaged> GetLogEntriesPagedAsync(int serverId, string logName, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/ClearLog", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/ClearLogResponse")]
        int ClearLog(int serverId, string logName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/ClearLog", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/ClearLogResponse")]
        System.Threading.Tasks.Task<int> ClearLogAsync(int serverId, string logName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/RebootSystem", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/RebootSystemResponse")]
        int RebootSystem(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/RebootSystem", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/RebootSystemResponse")]
        System.Threading.Tasks.Task<int> RebootSystemAsync(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetMemoryPackageId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetMemoryPackageIdResponse")]
        SolidCP.Providers.OS.Memory GetMemoryPackageId(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetMemoryPackageId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetMemoryPackageIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.Memory> GetMemoryPackageIdAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetMemory", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetMemoryResponse")]
        SolidCP.Providers.OS.Memory GetMemory(int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetMemory", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesServers/GetMemoryResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.Memory> GetMemoryAsync(int serverId);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esServersAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesServers
    {
        public SolidCP.EnterpriseServer.ServerInfo[] /*List*/ GetAllServers()
        {
            return Invoke<SolidCP.EnterpriseServer.ServerInfo[], SolidCP.EnterpriseServer.ServerInfo>("SolidCP.EnterpriseServer.esServers", "GetAllServers");
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServerInfo[]> GetAllServersAsync()
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ServerInfo[], SolidCP.EnterpriseServer.ServerInfo>("SolidCP.EnterpriseServer.esServers", "GetAllServers");
        }

        public System.Data.DataSet GetRawAllServers()
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetRawAllServers");
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawAllServersAsync()
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetRawAllServers");
        }

        public SolidCP.EnterpriseServer.ServerInfo[] /*List*/ GetServers()
        {
            return Invoke<SolidCP.EnterpriseServer.ServerInfo[], SolidCP.EnterpriseServer.ServerInfo>("SolidCP.EnterpriseServer.esServers", "GetServers");
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServerInfo[]> GetServersAsync()
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ServerInfo[], SolidCP.EnterpriseServer.ServerInfo>("SolidCP.EnterpriseServer.esServers", "GetServers");
        }

        public System.Data.DataSet GetRawServers()
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetRawServers");
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawServersAsync()
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetRawServers");
        }

        public SolidCP.EnterpriseServer.ServerInfo GetServerShortDetails(int serverId)
        {
            return Invoke<SolidCP.EnterpriseServer.ServerInfo>("SolidCP.EnterpriseServer.esServers", "GetServerShortDetails", serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServerInfo> GetServerShortDetailsAsync(int serverId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ServerInfo>("SolidCP.EnterpriseServer.esServers", "GetServerShortDetails", serverId);
        }

        public SolidCP.EnterpriseServer.ServerInfo GetServerById(int serverId)
        {
            return Invoke<SolidCP.EnterpriseServer.ServerInfo>("SolidCP.EnterpriseServer.esServers", "GetServerById", serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServerInfo> GetServerByIdAsync(int serverId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ServerInfo>("SolidCP.EnterpriseServer.esServers", "GetServerById", serverId);
        }

        public SolidCP.EnterpriseServer.ServerInfo GetServerByName(string serverName)
        {
            return Invoke<SolidCP.EnterpriseServer.ServerInfo>("SolidCP.EnterpriseServer.esServers", "GetServerByName", serverName);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServerInfo> GetServerByNameAsync(string serverName)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ServerInfo>("SolidCP.EnterpriseServer.esServers", "GetServerByName", serverName);
        }

        public int CheckServerAvailable(string serverUrl, string password)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "CheckServerAvailable", serverUrl, password);
        }

        public async System.Threading.Tasks.Task<int> CheckServerAvailableAsync(string serverUrl, string password)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "CheckServerAvailable", serverUrl, password);
        }

        public int AddServer(SolidCP.EnterpriseServer.ServerInfo server, bool autoDiscovery)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "AddServer", server, autoDiscovery);
        }

        public async System.Threading.Tasks.Task<int> AddServerAsync(SolidCP.EnterpriseServer.ServerInfo server, bool autoDiscovery)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "AddServer", server, autoDiscovery);
        }

        public int UpdateServer(SolidCP.EnterpriseServer.ServerInfo server)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "UpdateServer", server);
        }

        public async System.Threading.Tasks.Task<int> UpdateServerAsync(SolidCP.EnterpriseServer.ServerInfo server)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "UpdateServer", server);
        }

        public int UpdateServerConnectionPassword(int serverId, string password)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "UpdateServerConnectionPassword", serverId, password);
        }

        public async System.Threading.Tasks.Task<int> UpdateServerConnectionPasswordAsync(int serverId, string password)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "UpdateServerConnectionPassword", serverId, password);
        }

        public int UpdateServerADPassword(int serverId, string adPassword)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "UpdateServerADPassword", serverId, adPassword);
        }

        public async System.Threading.Tasks.Task<int> UpdateServerADPasswordAsync(int serverId, string adPassword)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "UpdateServerADPassword", serverId, adPassword);
        }

        public int DeleteServer(int serverId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "DeleteServer", serverId);
        }

        public async System.Threading.Tasks.Task<int> DeleteServerAsync(int serverId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "DeleteServer", serverId);
        }

        public string[] /*List*/ AutoUpdateServer(int[] /*List*/[] /*List*/ servers, string downloadLink, string fileName)
        {
            return Invoke<string[], string>("SolidCP.EnterpriseServer.esServers", "AutoUpdateServer", servers.ToList(), downloadLink, fileName);
        }

        public async System.Threading.Tasks.Task<string[]> AutoUpdateServerAsync(int[] /*List*/[] /*List*/ servers, string downloadLink, string fileName)
        {
            return await InvokeAsync<string[], string>("SolidCP.EnterpriseServer.esServers", "AutoUpdateServer", servers, downloadLink, fileName);
        }

        public System.Data.DataSet GetVirtualServers()
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetVirtualServers");
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetVirtualServersAsync()
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetVirtualServers");
        }

        public System.Data.DataSet GetAvailableVirtualServices(int serverId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetAvailableVirtualServices", serverId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetAvailableVirtualServicesAsync(int serverId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetAvailableVirtualServices", serverId);
        }

        public System.Data.DataSet GetVirtualServices(int serverId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetVirtualServices", serverId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetVirtualServicesAsync(int serverId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetVirtualServices", serverId);
        }

        public int AddVirtualServices(int serverId, int[] ids)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "AddVirtualServices", serverId, ids);
        }

        public async System.Threading.Tasks.Task<int> AddVirtualServicesAsync(int serverId, int[] ids)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "AddVirtualServices", serverId, ids);
        }

        public int DeleteVirtualServices(int serverId, int[] ids)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "DeleteVirtualServices", serverId, ids);
        }

        public async System.Threading.Tasks.Task<int> DeleteVirtualServicesAsync(int serverId, int[] ids)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "DeleteVirtualServices", serverId, ids);
        }

        public int UpdateVirtualGroups(int serverId, SolidCP.EnterpriseServer.VirtualGroupInfo[] groups)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "UpdateVirtualGroups", serverId, groups);
        }

        public async System.Threading.Tasks.Task<int> UpdateVirtualGroupsAsync(int serverId, SolidCP.EnterpriseServer.VirtualGroupInfo[] groups)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "UpdateVirtualGroups", serverId, groups);
        }

        public System.Data.DataSet GetRawServicesByServerId(int serverId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetRawServicesByServerId", serverId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawServicesByServerIdAsync(int serverId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetRawServicesByServerId", serverId);
        }

        public SolidCP.EnterpriseServer.ServiceInfo[] /*List*/ GetServicesByServerId(int serverId)
        {
            return Invoke<SolidCP.EnterpriseServer.ServiceInfo[], SolidCP.EnterpriseServer.ServiceInfo>("SolidCP.EnterpriseServer.esServers", "GetServicesByServerId", serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServiceInfo[]> GetServicesByServerIdAsync(int serverId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ServiceInfo[], SolidCP.EnterpriseServer.ServiceInfo>("SolidCP.EnterpriseServer.esServers", "GetServicesByServerId", serverId);
        }

        public SolidCP.EnterpriseServer.ServiceInfo[] /*List*/ GetServicesByServerIdGroupName(int serverId, string groupName)
        {
            return Invoke<SolidCP.EnterpriseServer.ServiceInfo[], SolidCP.EnterpriseServer.ServiceInfo>("SolidCP.EnterpriseServer.esServers", "GetServicesByServerIdGroupName", serverId, groupName);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServiceInfo[]> GetServicesByServerIdGroupNameAsync(int serverId, string groupName)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ServiceInfo[], SolidCP.EnterpriseServer.ServiceInfo>("SolidCP.EnterpriseServer.esServers", "GetServicesByServerIdGroupName", serverId, groupName);
        }

        public System.Data.DataSet GetRawServicesByGroupId(int groupId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetRawServicesByGroupId", groupId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawServicesByGroupIdAsync(int groupId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetRawServicesByGroupId", groupId);
        }

        public System.Data.DataSet GetRawServicesByGroupName(string groupName)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetRawServicesByGroupName", groupName);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawServicesByGroupNameAsync(string groupName)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetRawServicesByGroupName", groupName);
        }

        public SolidCP.EnterpriseServer.ServiceInfo GetServiceInfo(int serviceId)
        {
            return Invoke<SolidCP.EnterpriseServer.ServiceInfo>("SolidCP.EnterpriseServer.esServers", "GetServiceInfo", serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServiceInfo> GetServiceInfoAsync(int serviceId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ServiceInfo>("SolidCP.EnterpriseServer.esServers", "GetServiceInfo", serviceId);
        }

        public int AddService(SolidCP.EnterpriseServer.ServiceInfo service)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "AddService", service);
        }

        public async System.Threading.Tasks.Task<int> AddServiceAsync(SolidCP.EnterpriseServer.ServiceInfo service)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "AddService", service);
        }

        public int UpdateService(SolidCP.EnterpriseServer.ServiceInfo service)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "UpdateService", service);
        }

        public async System.Threading.Tasks.Task<int> UpdateServiceAsync(SolidCP.EnterpriseServer.ServiceInfo service)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "UpdateService", service);
        }

        public int DeleteService(int serviceId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "DeleteService", serviceId);
        }

        public async System.Threading.Tasks.Task<int> DeleteServiceAsync(int serviceId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "DeleteService", serviceId);
        }

        public string[] GetServiceSettings(int serviceId)
        {
            return Invoke<string[]>("SolidCP.EnterpriseServer.esServers", "GetServiceSettings", serviceId);
        }

        public async System.Threading.Tasks.Task<string[]> GetServiceSettingsAsync(int serviceId)
        {
            return await InvokeAsync<string[]>("SolidCP.EnterpriseServer.esServers", "GetServiceSettings", serviceId);
        }

        public string[] GetServiceSettingsRDS(int serviceId)
        {
            return Invoke<string[]>("SolidCP.EnterpriseServer.esServers", "GetServiceSettingsRDS", serviceId);
        }

        public async System.Threading.Tasks.Task<string[]> GetServiceSettingsRDSAsync(int serviceId)
        {
            return await InvokeAsync<string[]>("SolidCP.EnterpriseServer.esServers", "GetServiceSettingsRDS", serviceId);
        }

        public int UpdateServiceSettings(int serviceId, string[] settings)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "UpdateServiceSettings", serviceId, settings);
        }

        public async System.Threading.Tasks.Task<int> UpdateServiceSettingsAsync(int serviceId, string[] settings)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "UpdateServiceSettings", serviceId, settings);
        }

        public string[] InstallService(int serviceId)
        {
            return Invoke<string[]>("SolidCP.EnterpriseServer.esServers", "InstallService", serviceId);
        }

        public async System.Threading.Tasks.Task<string[]> InstallServiceAsync(int serviceId)
        {
            return await InvokeAsync<string[]>("SolidCP.EnterpriseServer.esServers", "InstallService", serviceId);
        }

        public SolidCP.EnterpriseServer.QuotaInfo GetProviderServiceQuota(int providerId)
        {
            return Invoke<SolidCP.EnterpriseServer.QuotaInfo>("SolidCP.EnterpriseServer.esServers", "GetProviderServiceQuota", providerId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.QuotaInfo> GetProviderServiceQuotaAsync(int providerId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.QuotaInfo>("SolidCP.EnterpriseServer.esServers", "GetProviderServiceQuota", providerId);
        }

        public string[] GetMailServiceSettingsByPackage(int packageID)
        {
            return Invoke<string[]>("SolidCP.EnterpriseServer.esServers", "GetMailServiceSettingsByPackage", packageID);
        }

        public async System.Threading.Tasks.Task<string[]> GetMailServiceSettingsByPackageAsync(int packageID)
        {
            return await InvokeAsync<string[]>("SolidCP.EnterpriseServer.esServers", "GetMailServiceSettingsByPackage", packageID);
        }

        public SolidCP.EnterpriseServer.ProviderInfo[] /*List*/ GetInstalledProviders(int groupId)
        {
            return Invoke<SolidCP.EnterpriseServer.ProviderInfo[], SolidCP.EnterpriseServer.ProviderInfo>("SolidCP.EnterpriseServer.esServers", "GetInstalledProviders", groupId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ProviderInfo[]> GetInstalledProvidersAsync(int groupId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ProviderInfo[], SolidCP.EnterpriseServer.ProviderInfo>("SolidCP.EnterpriseServer.esServers", "GetInstalledProviders", groupId);
        }

        public SolidCP.EnterpriseServer.ResourceGroupInfo[] /*List*/ GetResourceGroups()
        {
            return Invoke<SolidCP.EnterpriseServer.ResourceGroupInfo[], SolidCP.EnterpriseServer.ResourceGroupInfo>("SolidCP.EnterpriseServer.esServers", "GetResourceGroups");
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ResourceGroupInfo[]> GetResourceGroupsAsync()
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ResourceGroupInfo[], SolidCP.EnterpriseServer.ResourceGroupInfo>("SolidCP.EnterpriseServer.esServers", "GetResourceGroups");
        }

        public SolidCP.EnterpriseServer.ResourceGroupInfo GetResourceGroup(int groupId)
        {
            return Invoke<SolidCP.EnterpriseServer.ResourceGroupInfo>("SolidCP.EnterpriseServer.esServers", "GetResourceGroup", groupId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ResourceGroupInfo> GetResourceGroupAsync(int groupId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ResourceGroupInfo>("SolidCP.EnterpriseServer.esServers", "GetResourceGroup", groupId);
        }

        public SolidCP.EnterpriseServer.ProviderInfo GetProvider(int providerId)
        {
            return Invoke<SolidCP.EnterpriseServer.ProviderInfo>("SolidCP.EnterpriseServer.esServers", "GetProvider", providerId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ProviderInfo> GetProviderAsync(int providerId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ProviderInfo>("SolidCP.EnterpriseServer.esServers", "GetProvider", providerId);
        }

        public SolidCP.EnterpriseServer.ProviderInfo[] /*List*/ GetProviders()
        {
            return Invoke<SolidCP.EnterpriseServer.ProviderInfo[], SolidCP.EnterpriseServer.ProviderInfo>("SolidCP.EnterpriseServer.esServers", "GetProviders");
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ProviderInfo[]> GetProvidersAsync()
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ProviderInfo[], SolidCP.EnterpriseServer.ProviderInfo>("SolidCP.EnterpriseServer.esServers", "GetProviders");
        }

        public SolidCP.EnterpriseServer.ProviderInfo[] /*List*/ GetProvidersByGroupId(int groupId)
        {
            return Invoke<SolidCP.EnterpriseServer.ProviderInfo[], SolidCP.EnterpriseServer.ProviderInfo>("SolidCP.EnterpriseServer.esServers", "GetProvidersByGroupId", groupId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ProviderInfo[]> GetProvidersByGroupIdAsync(int groupId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ProviderInfo[], SolidCP.EnterpriseServer.ProviderInfo>("SolidCP.EnterpriseServer.esServers", "GetProvidersByGroupId", groupId);
        }

        public SolidCP.EnterpriseServer.ProviderInfo GetPackageServiceProvider(int packageId, string groupName)
        {
            return Invoke<SolidCP.EnterpriseServer.ProviderInfo>("SolidCP.EnterpriseServer.esServers", "GetPackageServiceProvider", packageId, groupName);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ProviderInfo> GetPackageServiceProviderAsync(int packageId, string groupName)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ProviderInfo>("SolidCP.EnterpriseServer.esServers", "GetPackageServiceProvider", packageId, groupName);
        }

        public System.String GetMailFilterUrl(int packageId, string groupName)
        {
            return Invoke<System.String>("SolidCP.EnterpriseServer.esServers", "GetMailFilterUrl", packageId, groupName);
        }

        public async System.Threading.Tasks.Task<System.String> GetMailFilterUrlAsync(int packageId, string groupName)
        {
            return await InvokeAsync<System.String>("SolidCP.EnterpriseServer.esServers", "GetMailFilterUrl", packageId, groupName);
        }

        public System.String GetMailFilterUrlByHostingPlan(int PlanID, string groupName)
        {
            return Invoke<System.String>("SolidCP.EnterpriseServer.esServers", "GetMailFilterUrlByHostingPlan", PlanID, groupName);
        }

        public async System.Threading.Tasks.Task<System.String> GetMailFilterUrlByHostingPlanAsync(int PlanID, string groupName)
        {
            return await InvokeAsync<System.String>("SolidCP.EnterpriseServer.esServers", "GetMailFilterUrlByHostingPlan", PlanID, groupName);
        }

        public SolidCP.Providers.Common.BoolResult IsInstalled(int serverId, int providerId)
        {
            return Invoke<SolidCP.Providers.Common.BoolResult>("SolidCP.EnterpriseServer.esServers", "IsInstalled", serverId, providerId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.BoolResult> IsInstalledAsync(int serverId, int providerId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.BoolResult>("SolidCP.EnterpriseServer.esServers", "IsInstalled", serverId, providerId);
        }

        public string GetServerVersion(int serverId)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esServers", "GetServerVersion", serverId);
        }

        public async System.Threading.Tasks.Task<string> GetServerVersionAsync(int serverId)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esServers", "GetServerVersion", serverId);
        }

        public string GetServerFilePath(int serverId)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esServers", "GetServerFilePath", serverId);
        }

        public async System.Threading.Tasks.Task<string> GetServerFilePathAsync(int serverId)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esServers", "GetServerFilePath", serverId);
        }

        public SolidCP.EnterpriseServer.VLANsPaged GetPrivateNetworVLANsPaged(int serverId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.EnterpriseServer.VLANsPaged>("SolidCP.EnterpriseServer.esServers", "GetPrivateNetworVLANsPaged", serverId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VLANsPaged> GetPrivateNetworVLANsPagedAsync(int serverId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.VLANsPaged>("SolidCP.EnterpriseServer.esServers", "GetPrivateNetworVLANsPaged", serverId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.ResultObjects.IntResult AddPrivateNetworkVLAN(int serverId, int vlan, string comments)
        {
            return Invoke<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esServers", "AddPrivateNetworkVLAN", serverId, vlan, comments);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> AddPrivateNetworkVLANAsync(int serverId, int vlan, string comments)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esServers", "AddPrivateNetworkVLAN", serverId, vlan, comments);
        }

        public SolidCP.Providers.Common.ResultObject DeletePrivateNetworkVLANs(int[] vlans)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "DeletePrivateNetworkVLANs", vlans);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeletePrivateNetworkVLANsAsync(int[] vlans)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "DeletePrivateNetworkVLANs", vlans);
        }

        public SolidCP.Providers.Common.ResultObject AddPrivateNetworkVLANsRange(int serverId, int startVLAN, int endVLAN, string comments)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "AddPrivateNetworkVLANsRange", serverId, startVLAN, endVLAN, comments);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddPrivateNetworkVLANsRangeAsync(int serverId, int startVLAN, int endVLAN, string comments)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "AddPrivateNetworkVLANsRange", serverId, startVLAN, endVLAN, comments);
        }

        public SolidCP.EnterpriseServer.VLANInfo GetPrivateNetworVLAN(int vlanId)
        {
            return Invoke<SolidCP.EnterpriseServer.VLANInfo>("SolidCP.EnterpriseServer.esServers", "GetPrivateNetworVLAN", vlanId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VLANInfo> GetPrivateNetworVLANAsync(int vlanId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.VLANInfo>("SolidCP.EnterpriseServer.esServers", "GetPrivateNetworVLAN", vlanId);
        }

        public SolidCP.Providers.Common.ResultObject UpdatePrivateNetworVLAN(int vlanId, int serverId, int vlan, string comments)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "UpdatePrivateNetworVLAN", vlanId, serverId, vlan, comments);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdatePrivateNetworVLANAsync(int vlanId, int serverId, int vlan, string comments)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "UpdatePrivateNetworVLAN", vlanId, serverId, vlan, comments);
        }

        public SolidCP.EnterpriseServer.PackageVLANsPaged GetPackagePrivateNetworkVLANs(int packageId, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.EnterpriseServer.PackageVLANsPaged>("SolidCP.EnterpriseServer.esServers", "GetPackagePrivateNetworkVLANs", packageId, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageVLANsPaged> GetPackagePrivateNetworkVLANsAsync(int packageId, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PackageVLANsPaged>("SolidCP.EnterpriseServer.esServers", "GetPackagePrivateNetworkVLANs", packageId, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Common.ResultObject DeallocatePackageVLANs(int packageId, int[] vlanId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "DeallocatePackageVLANs", packageId, vlanId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeallocatePackageVLANsAsync(int packageId, int[] vlanId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "DeallocatePackageVLANs", packageId, vlanId);
        }

        public SolidCP.EnterpriseServer.VLANInfo[] /*List*/ GetUnallottedVLANs(int packageId, string groupName)
        {
            return Invoke<SolidCP.EnterpriseServer.VLANInfo[], SolidCP.EnterpriseServer.VLANInfo>("SolidCP.EnterpriseServer.esServers", "GetUnallottedVLANs", packageId, groupName);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VLANInfo[]> GetUnallottedVLANsAsync(int packageId, string groupName)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.VLANInfo[], SolidCP.EnterpriseServer.VLANInfo>("SolidCP.EnterpriseServer.esServers", "GetUnallottedVLANs", packageId, groupName);
        }

        public SolidCP.Providers.Common.ResultObject AllocatePackageVLANs(int packageId, string groupName, bool allocateRandom, int vlansNumber, int[] vlanId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "AllocatePackageVLANs", packageId, groupName, allocateRandom, vlansNumber, vlanId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AllocatePackageVLANsAsync(int packageId, string groupName, bool allocateRandom, int vlansNumber, int[] vlanId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "AllocatePackageVLANs", packageId, groupName, allocateRandom, vlansNumber, vlanId);
        }

        public SolidCP.EnterpriseServer.IPAddressInfo[] /*List*/ GetIPAddresses(SolidCP.EnterpriseServer.IPAddressPool pool, int serverId)
        {
            return Invoke<SolidCP.EnterpriseServer.IPAddressInfo[], SolidCP.EnterpriseServer.IPAddressInfo>("SolidCP.EnterpriseServer.esServers", "GetIPAddresses", pool, serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.IPAddressInfo[]> GetIPAddressesAsync(SolidCP.EnterpriseServer.IPAddressPool pool, int serverId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.IPAddressInfo[], SolidCP.EnterpriseServer.IPAddressInfo>("SolidCP.EnterpriseServer.esServers", "GetIPAddresses", pool, serverId);
        }

        public SolidCP.EnterpriseServer.IPAddressesPaged GetIPAddressesPaged(SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.EnterpriseServer.IPAddressesPaged>("SolidCP.EnterpriseServer.esServers", "GetIPAddressesPaged", pool, serverId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.IPAddressesPaged> GetIPAddressesPagedAsync(SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.IPAddressesPaged>("SolidCP.EnterpriseServer.esServers", "GetIPAddressesPaged", pool, serverId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.EnterpriseServer.IPAddressInfo GetIPAddress(int addressId)
        {
            return Invoke<SolidCP.EnterpriseServer.IPAddressInfo>("SolidCP.EnterpriseServer.esServers", "GetIPAddress", addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.IPAddressInfo> GetIPAddressAsync(int addressId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.IPAddressInfo>("SolidCP.EnterpriseServer.esServers", "GetIPAddress", addressId);
        }

        public SolidCP.Providers.ResultObjects.IntResult AddIPAddress(SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string externalIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            return Invoke<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esServers", "AddIPAddress", pool, serverId, externalIP, internalIP, subnetMask, defaultGateway, comments, VLAN);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> AddIPAddressAsync(SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string externalIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esServers", "AddIPAddress", pool, serverId, externalIP, internalIP, subnetMask, defaultGateway, comments, VLAN);
        }

        public SolidCP.Providers.Common.ResultObject AddIPAddressesRange(SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string externalIP, string endIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "AddIPAddressesRange", pool, serverId, externalIP, endIP, internalIP, subnetMask, defaultGateway, comments, VLAN);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddIPAddressesRangeAsync(SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string externalIP, string endIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "AddIPAddressesRange", pool, serverId, externalIP, endIP, internalIP, subnetMask, defaultGateway, comments, VLAN);
        }

        public SolidCP.Providers.Common.ResultObject UpdateIPAddress(int addressId, SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string externalIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "UpdateIPAddress", addressId, pool, serverId, externalIP, internalIP, subnetMask, defaultGateway, comments, VLAN);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateIPAddressAsync(int addressId, SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string externalIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "UpdateIPAddress", addressId, pool, serverId, externalIP, internalIP, subnetMask, defaultGateway, comments, VLAN);
        }

        public SolidCP.Providers.Common.ResultObject UpdateIPAddresses(int[] addresses, SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "UpdateIPAddresses", addresses, pool, serverId, subnetMask, defaultGateway, comments, VLAN);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateIPAddressesAsync(int[] addresses, SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "UpdateIPAddresses", addresses, pool, serverId, subnetMask, defaultGateway, comments, VLAN);
        }

        public SolidCP.Providers.Common.ResultObject DeleteIPAddress(int addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "DeleteIPAddress", addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteIPAddressAsync(int addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "DeleteIPAddress", addressId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteIPAddresses(int[] addresses)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "DeleteIPAddresses", addresses);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteIPAddressesAsync(int[] addresses)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "DeleteIPAddresses", addresses);
        }

        public SolidCP.EnterpriseServer.IPAddressInfo[] /*List*/ GetUnallottedIPAddresses(int packageId, string groupName, SolidCP.EnterpriseServer.IPAddressPool pool)
        {
            return Invoke<SolidCP.EnterpriseServer.IPAddressInfo[], SolidCP.EnterpriseServer.IPAddressInfo>("SolidCP.EnterpriseServer.esServers", "GetUnallottedIPAddresses", packageId, groupName, pool);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.IPAddressInfo[]> GetUnallottedIPAddressesAsync(int packageId, string groupName, SolidCP.EnterpriseServer.IPAddressPool pool)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.IPAddressInfo[], SolidCP.EnterpriseServer.IPAddressInfo>("SolidCP.EnterpriseServer.esServers", "GetUnallottedIPAddresses", packageId, groupName, pool);
        }

        public SolidCP.EnterpriseServer.PackageIPAddressesPaged GetPackageIPAddresses(int packageId, int orgId, SolidCP.EnterpriseServer.IPAddressPool pool, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            return Invoke<SolidCP.EnterpriseServer.PackageIPAddressesPaged>("SolidCP.EnterpriseServer.esServers", "GetPackageIPAddresses", packageId, orgId, pool, filterColumn, filterValue, sortColumn, startRow, maximumRows, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageIPAddressesPaged> GetPackageIPAddressesAsync(int packageId, int orgId, SolidCP.EnterpriseServer.IPAddressPool pool, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PackageIPAddressesPaged>("SolidCP.EnterpriseServer.esServers", "GetPackageIPAddresses", packageId, orgId, pool, filterColumn, filterValue, sortColumn, startRow, maximumRows, recursive);
        }

        public int GetPackageIPAddressesCount(int packageId, int orgId, SolidCP.EnterpriseServer.IPAddressPool pool)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "GetPackageIPAddressesCount", packageId, orgId, pool);
        }

        public async System.Threading.Tasks.Task<int> GetPackageIPAddressesCountAsync(int packageId, int orgId, SolidCP.EnterpriseServer.IPAddressPool pool)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "GetPackageIPAddressesCount", packageId, orgId, pool);
        }

        public SolidCP.EnterpriseServer.PackageIPAddress[] /*List*/ GetPackageUnassignedIPAddresses(int packageId, int orgId, SolidCP.EnterpriseServer.IPAddressPool pool)
        {
            return Invoke<SolidCP.EnterpriseServer.PackageIPAddress[], SolidCP.EnterpriseServer.PackageIPAddress>("SolidCP.EnterpriseServer.esServers", "GetPackageUnassignedIPAddresses", packageId, orgId, pool);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageIPAddress[]> GetPackageUnassignedIPAddressesAsync(int packageId, int orgId, SolidCP.EnterpriseServer.IPAddressPool pool)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PackageIPAddress[], SolidCP.EnterpriseServer.PackageIPAddress>("SolidCP.EnterpriseServer.esServers", "GetPackageUnassignedIPAddresses", packageId, orgId, pool);
        }

        public SolidCP.Providers.Common.ResultObject AllocatePackageIPAddresses(int packageId, int orgId, string groupName, SolidCP.EnterpriseServer.IPAddressPool pool, bool allocateRandom, int addressesNumber, int[] addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "AllocatePackageIPAddresses", packageId, orgId, groupName, pool, allocateRandom, addressesNumber, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AllocatePackageIPAddressesAsync(int packageId, int orgId, string groupName, SolidCP.EnterpriseServer.IPAddressPool pool, bool allocateRandom, int addressesNumber, int[] addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "AllocatePackageIPAddresses", packageId, orgId, groupName, pool, allocateRandom, addressesNumber, addressId);
        }

        public SolidCP.Providers.Common.ResultObject AllocateMaximumPackageIPAddresses(int packageId, string groupName, SolidCP.EnterpriseServer.IPAddressPool pool)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "AllocateMaximumPackageIPAddresses", packageId, groupName, pool);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AllocateMaximumPackageIPAddressesAsync(int packageId, string groupName, SolidCP.EnterpriseServer.IPAddressPool pool)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "AllocateMaximumPackageIPAddresses", packageId, groupName, pool);
        }

        public SolidCP.Providers.Common.ResultObject DeallocatePackageIPAddresses(int packageId, int[] addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "DeallocatePackageIPAddresses", packageId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeallocatePackageIPAddressesAsync(int packageId, int[] addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esServers", "DeallocatePackageIPAddresses", packageId, addressId);
        }

        public SolidCP.EnterpriseServer.ClusterInfo[] /*List*/ GetClusters()
        {
            return Invoke<SolidCP.EnterpriseServer.ClusterInfo[], SolidCP.EnterpriseServer.ClusterInfo>("SolidCP.EnterpriseServer.esServers", "GetClusters");
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ClusterInfo[]> GetClustersAsync()
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ClusterInfo[], SolidCP.EnterpriseServer.ClusterInfo>("SolidCP.EnterpriseServer.esServers", "GetClusters");
        }

        public int AddCluster(SolidCP.EnterpriseServer.ClusterInfo cluster)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "AddCluster", cluster);
        }

        public async System.Threading.Tasks.Task<int> AddClusterAsync(SolidCP.EnterpriseServer.ClusterInfo cluster)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "AddCluster", cluster);
        }

        public int DeleteCluster(int clusterId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "DeleteCluster", clusterId);
        }

        public async System.Threading.Tasks.Task<int> DeleteClusterAsync(int clusterId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "DeleteCluster", clusterId);
        }

        public System.Data.DataSet GetRawDnsRecordsByService(int serviceId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetRawDnsRecordsByService", serviceId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawDnsRecordsByServiceAsync(int serviceId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetRawDnsRecordsByService", serviceId);
        }

        public System.Data.DataSet GetRawDnsRecordsByServer(int serverId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetRawDnsRecordsByServer", serverId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawDnsRecordsByServerAsync(int serverId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetRawDnsRecordsByServer", serverId);
        }

        public System.Data.DataSet GetRawDnsRecordsByPackage(int packageId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetRawDnsRecordsByPackage", packageId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawDnsRecordsByPackageAsync(int packageId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetRawDnsRecordsByPackage", packageId);
        }

        public System.Data.DataSet GetRawDnsRecordsByGroup(int groupId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetRawDnsRecordsByGroup", groupId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawDnsRecordsByGroupAsync(int groupId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetRawDnsRecordsByGroup", groupId);
        }

        public SolidCP.EnterpriseServer.GlobalDnsRecord[] /*List*/ GetDnsRecordsByService(int serviceId)
        {
            return Invoke<SolidCP.EnterpriseServer.GlobalDnsRecord[], SolidCP.EnterpriseServer.GlobalDnsRecord>("SolidCP.EnterpriseServer.esServers", "GetDnsRecordsByService", serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.GlobalDnsRecord[]> GetDnsRecordsByServiceAsync(int serviceId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.GlobalDnsRecord[], SolidCP.EnterpriseServer.GlobalDnsRecord>("SolidCP.EnterpriseServer.esServers", "GetDnsRecordsByService", serviceId);
        }

        public SolidCP.EnterpriseServer.GlobalDnsRecord[] /*List*/ GetDnsRecordsByServer(int serverId)
        {
            return Invoke<SolidCP.EnterpriseServer.GlobalDnsRecord[], SolidCP.EnterpriseServer.GlobalDnsRecord>("SolidCP.EnterpriseServer.esServers", "GetDnsRecordsByServer", serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.GlobalDnsRecord[]> GetDnsRecordsByServerAsync(int serverId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.GlobalDnsRecord[], SolidCP.EnterpriseServer.GlobalDnsRecord>("SolidCP.EnterpriseServer.esServers", "GetDnsRecordsByServer", serverId);
        }

        public SolidCP.EnterpriseServer.GlobalDnsRecord[] /*List*/ GetDnsRecordsByPackage(int packageId)
        {
            return Invoke<SolidCP.EnterpriseServer.GlobalDnsRecord[], SolidCP.EnterpriseServer.GlobalDnsRecord>("SolidCP.EnterpriseServer.esServers", "GetDnsRecordsByPackage", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.GlobalDnsRecord[]> GetDnsRecordsByPackageAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.GlobalDnsRecord[], SolidCP.EnterpriseServer.GlobalDnsRecord>("SolidCP.EnterpriseServer.esServers", "GetDnsRecordsByPackage", packageId);
        }

        public SolidCP.EnterpriseServer.GlobalDnsRecord[] /*List*/ GetDnsRecordsByGroup(int groupId)
        {
            return Invoke<SolidCP.EnterpriseServer.GlobalDnsRecord[], SolidCP.EnterpriseServer.GlobalDnsRecord>("SolidCP.EnterpriseServer.esServers", "GetDnsRecordsByGroup", groupId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.GlobalDnsRecord[]> GetDnsRecordsByGroupAsync(int groupId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.GlobalDnsRecord[], SolidCP.EnterpriseServer.GlobalDnsRecord>("SolidCP.EnterpriseServer.esServers", "GetDnsRecordsByGroup", groupId);
        }

        public SolidCP.EnterpriseServer.GlobalDnsRecord GetDnsRecord(int recordId)
        {
            return Invoke<SolidCP.EnterpriseServer.GlobalDnsRecord>("SolidCP.EnterpriseServer.esServers", "GetDnsRecord", recordId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.GlobalDnsRecord> GetDnsRecordAsync(int recordId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.GlobalDnsRecord>("SolidCP.EnterpriseServer.esServers", "GetDnsRecord", recordId);
        }

        public int AddDnsRecord(SolidCP.EnterpriseServer.GlobalDnsRecord record)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "AddDnsRecord", record);
        }

        public async System.Threading.Tasks.Task<int> AddDnsRecordAsync(SolidCP.EnterpriseServer.GlobalDnsRecord record)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "AddDnsRecord", record);
        }

        public int UpdateDnsRecord(SolidCP.EnterpriseServer.GlobalDnsRecord record)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "UpdateDnsRecord", record);
        }

        public async System.Threading.Tasks.Task<int> UpdateDnsRecordAsync(SolidCP.EnterpriseServer.GlobalDnsRecord record)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "UpdateDnsRecord", record);
        }

        public int DeleteDnsRecord(int recordId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "DeleteDnsRecord", recordId);
        }

        public async System.Threading.Tasks.Task<int> DeleteDnsRecordAsync(int recordId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "DeleteDnsRecord", recordId);
        }

        public SolidCP.Providers.DomainLookup.DnsRecordInfo[] /*List*/ GetDomainDnsRecords(int domainId)
        {
            return Invoke<SolidCP.Providers.DomainLookup.DnsRecordInfo[], SolidCP.Providers.DomainLookup.DnsRecordInfo>("SolidCP.EnterpriseServer.esServers", "GetDomainDnsRecords", domainId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.DomainLookup.DnsRecordInfo[]> GetDomainDnsRecordsAsync(int domainId)
        {
            return await InvokeAsync<SolidCP.Providers.DomainLookup.DnsRecordInfo[], SolidCP.Providers.DomainLookup.DnsRecordInfo>("SolidCP.EnterpriseServer.esServers", "GetDomainDnsRecords", domainId);
        }

        public SolidCP.EnterpriseServer.DomainInfo[] /*List*/ GetDomains(int packageId)
        {
            return Invoke<SolidCP.EnterpriseServer.DomainInfo[], SolidCP.EnterpriseServer.DomainInfo>("SolidCP.EnterpriseServer.esServers", "GetDomains", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DomainInfo[]> GetDomainsAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.DomainInfo[], SolidCP.EnterpriseServer.DomainInfo>("SolidCP.EnterpriseServer.esServers", "GetDomains", packageId);
        }

        public SolidCP.EnterpriseServer.DomainInfo[] /*List*/ GetDomainsByDomainId(int domainId)
        {
            return Invoke<SolidCP.EnterpriseServer.DomainInfo[], SolidCP.EnterpriseServer.DomainInfo>("SolidCP.EnterpriseServer.esServers", "GetDomainsByDomainId", domainId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DomainInfo[]> GetDomainsByDomainIdAsync(int domainId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.DomainInfo[], SolidCP.EnterpriseServer.DomainInfo>("SolidCP.EnterpriseServer.esServers", "GetDomainsByDomainId", domainId);
        }

        public SolidCP.EnterpriseServer.DomainInfo[] /*List*/ GetMyDomains(int packageId)
        {
            return Invoke<SolidCP.EnterpriseServer.DomainInfo[], SolidCP.EnterpriseServer.DomainInfo>("SolidCP.EnterpriseServer.esServers", "GetMyDomains", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DomainInfo[]> GetMyDomainsAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.DomainInfo[], SolidCP.EnterpriseServer.DomainInfo>("SolidCP.EnterpriseServer.esServers", "GetMyDomains", packageId);
        }

        public SolidCP.EnterpriseServer.DomainInfo[] /*List*/ GetResellerDomains(int packageId)
        {
            return Invoke<SolidCP.EnterpriseServer.DomainInfo[], SolidCP.EnterpriseServer.DomainInfo>("SolidCP.EnterpriseServer.esServers", "GetResellerDomains", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DomainInfo[]> GetResellerDomainsAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.DomainInfo[], SolidCP.EnterpriseServer.DomainInfo>("SolidCP.EnterpriseServer.esServers", "GetResellerDomains", packageId);
        }

        public System.Data.DataSet GetDomainsPaged(int packageId, int serverId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetDomainsPaged", packageId, serverId, recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetDomainsPagedAsync(int packageId, int serverId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetDomainsPaged", packageId, serverId, recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.EnterpriseServer.DomainInfo GetDomain(int domainId)
        {
            return Invoke<SolidCP.EnterpriseServer.DomainInfo>("SolidCP.EnterpriseServer.esServers", "GetDomain", domainId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DomainInfo> GetDomainAsync(int domainId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.DomainInfo>("SolidCP.EnterpriseServer.esServers", "GetDomain", domainId);
        }

        public int AddDomain(SolidCP.EnterpriseServer.DomainInfo domain)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "AddDomain", domain);
        }

        public async System.Threading.Tasks.Task<int> AddDomainAsync(SolidCP.EnterpriseServer.DomainInfo domain)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "AddDomain", domain);
        }

        public int AddDomainWithProvisioning(int packageId, string domainName, SolidCP.EnterpriseServer.DomainType domainType, bool createWebSite, int pointWebSiteId, int pointMailDomainId, bool createDnsZone, bool createPreviewDomain, bool allowSubDomains, string hostName)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "AddDomainWithProvisioning", packageId, domainName, domainType, createWebSite, pointWebSiteId, pointMailDomainId, createDnsZone, createPreviewDomain, allowSubDomains, hostName);
        }

        public async System.Threading.Tasks.Task<int> AddDomainWithProvisioningAsync(int packageId, string domainName, SolidCP.EnterpriseServer.DomainType domainType, bool createWebSite, int pointWebSiteId, int pointMailDomainId, bool createDnsZone, bool createPreviewDomain, bool allowSubDomains, string hostName)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "AddDomainWithProvisioning", packageId, domainName, domainType, createWebSite, pointWebSiteId, pointMailDomainId, createDnsZone, createPreviewDomain, allowSubDomains, hostName);
        }

        public int UpdateDomain(SolidCP.EnterpriseServer.DomainInfo domain)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "UpdateDomain", domain);
        }

        public async System.Threading.Tasks.Task<int> UpdateDomainAsync(SolidCP.EnterpriseServer.DomainInfo domain)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "UpdateDomain", domain);
        }

        public int DeleteDomain(int domainId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "DeleteDomain", domainId);
        }

        public async System.Threading.Tasks.Task<int> DeleteDomainAsync(int domainId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "DeleteDomain", domainId);
        }

        public int DetachDomain(int domainId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "DetachDomain", domainId);
        }

        public async System.Threading.Tasks.Task<int> DetachDomainAsync(int domainId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "DetachDomain", domainId);
        }

        public int EnableDomainDns(int domainId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "EnableDomainDns", domainId);
        }

        public async System.Threading.Tasks.Task<int> EnableDomainDnsAsync(int domainId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "EnableDomainDns", domainId);
        }

        public int DisableDomainDns(int domainId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "DisableDomainDns", domainId);
        }

        public async System.Threading.Tasks.Task<int> DisableDomainDnsAsync(int domainId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "DisableDomainDns", domainId);
        }

        public int CreateDomainPreviewDomain(string hostName, int domainId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "CreateDomainPreviewDomain", hostName, domainId);
        }

        public async System.Threading.Tasks.Task<int> CreateDomainPreviewDomainAsync(string hostName, int domainId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "CreateDomainPreviewDomain", hostName, domainId);
        }

        public int DeleteDomainPreviewDomain(int domainId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "DeleteDomainPreviewDomain", domainId);
        }

        public async System.Threading.Tasks.Task<int> DeleteDomainPreviewDomainAsync(int domainId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "DeleteDomainPreviewDomain", domainId);
        }

        public SolidCP.Providers.DNS.DnsRecord[] GetDnsZoneRecords(int domainId)
        {
            return Invoke<SolidCP.Providers.DNS.DnsRecord[]>("SolidCP.EnterpriseServer.esServers", "GetDnsZoneRecords", domainId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.DNS.DnsRecord[]> GetDnsZoneRecordsAsync(int domainId)
        {
            return await InvokeAsync<SolidCP.Providers.DNS.DnsRecord[]>("SolidCP.EnterpriseServer.esServers", "GetDnsZoneRecords", domainId);
        }

        public System.Data.DataSet GetRawDnsZoneRecords(int domainId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetRawDnsZoneRecords", domainId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawDnsZoneRecordsAsync(int domainId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esServers", "GetRawDnsZoneRecords", domainId);
        }

        public int AddDnsZoneRecord(int domainId, string recordName, SolidCP.Providers.DNS.DnsRecordType recordType, string recordData, int mxPriority, int srvPriority, int srvWeight, int srvPortNumber)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "AddDnsZoneRecord", domainId, recordName, recordType, recordData, mxPriority, srvPriority, srvWeight, srvPortNumber);
        }

        public async System.Threading.Tasks.Task<int> AddDnsZoneRecordAsync(int domainId, string recordName, SolidCP.Providers.DNS.DnsRecordType recordType, string recordData, int mxPriority, int srvPriority, int srvWeight, int srvPortNumber)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "AddDnsZoneRecord", domainId, recordName, recordType, recordData, mxPriority, srvPriority, srvWeight, srvPortNumber);
        }

        public int UpdateDnsZoneRecord(int domainId, string originalRecordName, string originalRecordData, string recordName, SolidCP.Providers.DNS.DnsRecordType recordType, string recordData, int mxPriority, int srvPriority, int srvWeight, int srvPortNumber)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "UpdateDnsZoneRecord", domainId, originalRecordName, originalRecordData, recordName, recordType, recordData, mxPriority, srvPriority, srvWeight, srvPortNumber);
        }

        public async System.Threading.Tasks.Task<int> UpdateDnsZoneRecordAsync(int domainId, string originalRecordName, string originalRecordData, string recordName, SolidCP.Providers.DNS.DnsRecordType recordType, string recordData, int mxPriority, int srvPriority, int srvWeight, int srvPortNumber)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "UpdateDnsZoneRecord", domainId, originalRecordName, originalRecordData, recordName, recordType, recordData, mxPriority, srvPriority, srvWeight, srvPortNumber);
        }

        public int DeleteDnsZoneRecord(int domainId, string recordName, SolidCP.Providers.DNS.DnsRecordType recordType, string recordData)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "DeleteDnsZoneRecord", domainId, recordName, recordType, recordData);
        }

        public async System.Threading.Tasks.Task<int> DeleteDnsZoneRecordAsync(int domainId, string recordName, SolidCP.Providers.DNS.DnsRecordType recordType, string recordData)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "DeleteDnsZoneRecord", domainId, recordName, recordType, recordData);
        }

        public SolidCP.Providers.OS.TerminalSession[] GetTerminalServicesSessions(int serverId)
        {
            return Invoke<SolidCP.Providers.OS.TerminalSession[]>("SolidCP.EnterpriseServer.esServers", "GetTerminalServicesSessions", serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.TerminalSession[]> GetTerminalServicesSessionsAsync(int serverId)
        {
            return await InvokeAsync<SolidCP.Providers.OS.TerminalSession[]>("SolidCP.EnterpriseServer.esServers", "GetTerminalServicesSessions", serverId);
        }

        public int CloseTerminalServicesSession(int serverId, int sessionId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "CloseTerminalServicesSession", serverId, sessionId);
        }

        public async System.Threading.Tasks.Task<int> CloseTerminalServicesSessionAsync(int serverId, int sessionId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "CloseTerminalServicesSession", serverId, sessionId);
        }

        public SolidCP.Providers.OS.OSProcess[] GetOSProcesses(int serverId)
        {
            return Invoke<SolidCP.Providers.OS.OSProcess[]>("SolidCP.EnterpriseServer.esServers", "GetOSProcesses", serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.OSProcess[]> GetOSProcessesAsync(int serverId)
        {
            return await InvokeAsync<SolidCP.Providers.OS.OSProcess[]>("SolidCP.EnterpriseServer.esServers", "GetOSProcesses", serverId);
        }

        public int TerminateOSProcess(int serverId, int pid)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "TerminateOSProcess", serverId, pid);
        }

        public async System.Threading.Tasks.Task<int> TerminateOSProcessAsync(int serverId, int pid)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "TerminateOSProcess", serverId, pid);
        }

        public bool CheckLoadUserProfile(int serverId)
        {
            return Invoke<bool>("SolidCP.EnterpriseServer.esServers", "CheckLoadUserProfile", serverId);
        }

        public async System.Threading.Tasks.Task<bool> CheckLoadUserProfileAsync(int serverId)
        {
            return await InvokeAsync<bool>("SolidCP.EnterpriseServer.esServers", "CheckLoadUserProfile", serverId);
        }

        public void EnableLoadUserProfile(int serverId)
        {
            Invoke("SolidCP.EnterpriseServer.esServers", "EnableLoadUserProfile", serverId);
        }

        public async System.Threading.Tasks.Task EnableLoadUserProfileAsync(int serverId)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esServers", "EnableLoadUserProfile", serverId);
        }

        public void InitWPIFeeds(int serverId)
        {
            Invoke("SolidCP.EnterpriseServer.esServers", "InitWPIFeeds", serverId);
        }

        public async System.Threading.Tasks.Task InitWPIFeedsAsync(int serverId)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esServers", "InitWPIFeeds", serverId);
        }

        public SolidCP.Server.WPITab[] GetWPITabs(int serverId)
        {
            return Invoke<SolidCP.Server.WPITab[]>("SolidCP.EnterpriseServer.esServers", "GetWPITabs", serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPITab[]> GetWPITabsAsync(int serverId)
        {
            return await InvokeAsync<SolidCP.Server.WPITab[]>("SolidCP.EnterpriseServer.esServers", "GetWPITabs", serverId);
        }

        public SolidCP.Server.WPIKeyword[] GetWPIKeywords(int serverId)
        {
            return Invoke<SolidCP.Server.WPIKeyword[]>("SolidCP.EnterpriseServer.esServers", "GetWPIKeywords", serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIKeyword[]> GetWPIKeywordsAsync(int serverId)
        {
            return await InvokeAsync<SolidCP.Server.WPIKeyword[]>("SolidCP.EnterpriseServer.esServers", "GetWPIKeywords", serverId);
        }

        public SolidCP.Server.WPIProduct[] GetWPIProducts(int serverId, string tabId, string keywordId)
        {
            return Invoke<SolidCP.Server.WPIProduct[]>("SolidCP.EnterpriseServer.esServers", "GetWPIProducts", serverId, tabId, keywordId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsAsync(int serverId, string tabId, string keywordId)
        {
            return await InvokeAsync<SolidCP.Server.WPIProduct[]>("SolidCP.EnterpriseServer.esServers", "GetWPIProducts", serverId, tabId, keywordId);
        }

        public SolidCP.Server.WPIProduct[] GetWPIProductsFiltered(int serverId, string keywordId)
        {
            return Invoke<SolidCP.Server.WPIProduct[]>("SolidCP.EnterpriseServer.esServers", "GetWPIProductsFiltered", serverId, keywordId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsFilteredAsync(int serverId, string keywordId)
        {
            return await InvokeAsync<SolidCP.Server.WPIProduct[]>("SolidCP.EnterpriseServer.esServers", "GetWPIProductsFiltered", serverId, keywordId);
        }

        public SolidCP.Server.WPIProduct GetWPIProductById(int serverId, string productdId)
        {
            return Invoke<SolidCP.Server.WPIProduct>("SolidCP.EnterpriseServer.esServers", "GetWPIProductById", serverId, productdId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIProduct> GetWPIProductByIdAsync(int serverId, string productdId)
        {
            return await InvokeAsync<SolidCP.Server.WPIProduct>("SolidCP.EnterpriseServer.esServers", "GetWPIProductById", serverId, productdId);
        }

        public SolidCP.Server.WPIProduct[] GetWPIProductsWithDependencies(int serverId, string[] products)
        {
            return Invoke<SolidCP.Server.WPIProduct[]>("SolidCP.EnterpriseServer.esServers", "GetWPIProductsWithDependencies", serverId, products);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsWithDependenciesAsync(int serverId, string[] products)
        {
            return await InvokeAsync<SolidCP.Server.WPIProduct[]>("SolidCP.EnterpriseServer.esServers", "GetWPIProductsWithDependencies", serverId, products);
        }

        public void InstallWPIProducts(int serverId, string[] products)
        {
            Invoke("SolidCP.EnterpriseServer.esServers", "InstallWPIProducts", serverId, products);
        }

        public async System.Threading.Tasks.Task InstallWPIProductsAsync(int serverId, string[] products)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esServers", "InstallWPIProducts", serverId, products);
        }

        public void CancelInstallWPIProducts(int serviceId)
        {
            Invoke("SolidCP.EnterpriseServer.esServers", "CancelInstallWPIProducts", serviceId);
        }

        public async System.Threading.Tasks.Task CancelInstallWPIProductsAsync(int serviceId)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esServers", "CancelInstallWPIProducts", serviceId);
        }

        public string GetWPIStatus(int serverId)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esServers", "GetWPIStatus", serverId);
        }

        public async System.Threading.Tasks.Task<string> GetWPIStatusAsync(int serverId)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esServers", "GetWPIStatus", serverId);
        }

        public string WpiGetLogFileDirectory(int serverId)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esServers", "WpiGetLogFileDirectory", serverId);
        }

        public async System.Threading.Tasks.Task<string> WpiGetLogFileDirectoryAsync(int serverId)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esServers", "WpiGetLogFileDirectory", serverId);
        }

        public SolidCP.Providers.SettingPair[] WpiGetLogsInDirectory(int serverId, string Path)
        {
            return Invoke<SolidCP.Providers.SettingPair[]>("SolidCP.EnterpriseServer.esServers", "WpiGetLogsInDirectory", serverId, Path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SettingPair[]> WpiGetLogsInDirectoryAsync(int serverId, string Path)
        {
            return await InvokeAsync<SolidCP.Providers.SettingPair[]>("SolidCP.EnterpriseServer.esServers", "WpiGetLogsInDirectory", serverId, Path);
        }

        public SolidCP.Providers.OS.OSService[] GetOSServices(int serverId)
        {
            return Invoke<SolidCP.Providers.OS.OSService[]>("SolidCP.EnterpriseServer.esServers", "GetOSServices", serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.OSService[]> GetOSServicesAsync(int serverId)
        {
            return await InvokeAsync<SolidCP.Providers.OS.OSService[]>("SolidCP.EnterpriseServer.esServers", "GetOSServices", serverId);
        }

        public int ChangeOSServiceStatus(int serverId, string id, SolidCP.Providers.OS.OSServiceStatus status)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "ChangeOSServiceStatus", serverId, id, status);
        }

        public async System.Threading.Tasks.Task<int> ChangeOSServiceStatusAsync(int serverId, string id, SolidCP.Providers.OS.OSServiceStatus status)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "ChangeOSServiceStatus", serverId, id, status);
        }

        public string[] GetLogNames(int serverId)
        {
            return Invoke<string[]>("SolidCP.EnterpriseServer.esServers", "GetLogNames", serverId);
        }

        public async System.Threading.Tasks.Task<string[]> GetLogNamesAsync(int serverId)
        {
            return await InvokeAsync<string[]>("SolidCP.EnterpriseServer.esServers", "GetLogNames", serverId);
        }

        public SolidCP.Providers.OS.SystemLogEntry[] GetLogEntries(int serverId, string logName)
        {
            return Invoke<SolidCP.Providers.OS.SystemLogEntry[]>("SolidCP.EnterpriseServer.esServers", "GetLogEntries", serverId, logName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemLogEntry[]> GetLogEntriesAsync(int serverId, string logName)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemLogEntry[]>("SolidCP.EnterpriseServer.esServers", "GetLogEntries", serverId, logName);
        }

        public SolidCP.Providers.OS.SystemLogEntriesPaged GetLogEntriesPaged(int serverId, string logName, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.Providers.OS.SystemLogEntriesPaged>("SolidCP.EnterpriseServer.esServers", "GetLogEntriesPaged", serverId, logName, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemLogEntriesPaged> GetLogEntriesPagedAsync(int serverId, string logName, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemLogEntriesPaged>("SolidCP.EnterpriseServer.esServers", "GetLogEntriesPaged", serverId, logName, startRow, maximumRows);
        }

        public int ClearLog(int serverId, string logName)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "ClearLog", serverId, logName);
        }

        public async System.Threading.Tasks.Task<int> ClearLogAsync(int serverId, string logName)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "ClearLog", serverId, logName);
        }

        public int RebootSystem(int serverId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esServers", "RebootSystem", serverId);
        }

        public async System.Threading.Tasks.Task<int> RebootSystemAsync(int serverId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esServers", "RebootSystem", serverId);
        }

        public SolidCP.Providers.OS.Memory GetMemoryPackageId(int packageId)
        {
            return Invoke<SolidCP.Providers.OS.Memory>("SolidCP.EnterpriseServer.esServers", "GetMemoryPackageId", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.Memory> GetMemoryPackageIdAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.Providers.OS.Memory>("SolidCP.EnterpriseServer.esServers", "GetMemoryPackageId", packageId);
        }

        public SolidCP.Providers.OS.Memory GetMemory(int serverId)
        {
            return Invoke<SolidCP.Providers.OS.Memory>("SolidCP.EnterpriseServer.esServers", "GetMemory", serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.Memory> GetMemoryAsync(int serverId)
        {
            return await InvokeAsync<SolidCP.Providers.OS.Memory>("SolidCP.EnterpriseServer.esServers", "GetMemory", serverId);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esServers : SolidCP.Web.Client.ClientBase<IesServers, esServersAssemblyClient>, IesServers
    {
        public SolidCP.EnterpriseServer.ServerInfo[] /*List*/ GetAllServers()
        {
            return base.Client.GetAllServers();
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServerInfo[]> GetAllServersAsync()
        {
            return await base.Client.GetAllServersAsync();
        }

        public System.Data.DataSet GetRawAllServers()
        {
            return base.Client.GetRawAllServers();
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawAllServersAsync()
        {
            return await base.Client.GetRawAllServersAsync();
        }

        public SolidCP.EnterpriseServer.ServerInfo[] /*List*/ GetServers()
        {
            return base.Client.GetServers();
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServerInfo[]> GetServersAsync()
        {
            return await base.Client.GetServersAsync();
        }

        public System.Data.DataSet GetRawServers()
        {
            return base.Client.GetRawServers();
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawServersAsync()
        {
            return await base.Client.GetRawServersAsync();
        }

        public SolidCP.EnterpriseServer.ServerInfo GetServerShortDetails(int serverId)
        {
            return base.Client.GetServerShortDetails(serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServerInfo> GetServerShortDetailsAsync(int serverId)
        {
            return await base.Client.GetServerShortDetailsAsync(serverId);
        }

        public SolidCP.EnterpriseServer.ServerInfo GetServerById(int serverId)
        {
            return base.Client.GetServerById(serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServerInfo> GetServerByIdAsync(int serverId)
        {
            return await base.Client.GetServerByIdAsync(serverId);
        }

        public SolidCP.EnterpriseServer.ServerInfo GetServerByName(string serverName)
        {
            return base.Client.GetServerByName(serverName);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServerInfo> GetServerByNameAsync(string serverName)
        {
            return await base.Client.GetServerByNameAsync(serverName);
        }

        public int CheckServerAvailable(string serverUrl, string password)
        {
            return base.Client.CheckServerAvailable(serverUrl, password);
        }

        public async System.Threading.Tasks.Task<int> CheckServerAvailableAsync(string serverUrl, string password)
        {
            return await base.Client.CheckServerAvailableAsync(serverUrl, password);
        }

        public int AddServer(SolidCP.EnterpriseServer.ServerInfo server, bool autoDiscovery)
        {
            return base.Client.AddServer(server, autoDiscovery);
        }

        public async System.Threading.Tasks.Task<int> AddServerAsync(SolidCP.EnterpriseServer.ServerInfo server, bool autoDiscovery)
        {
            return await base.Client.AddServerAsync(server, autoDiscovery);
        }

        public int UpdateServer(SolidCP.EnterpriseServer.ServerInfo server)
        {
            return base.Client.UpdateServer(server);
        }

        public async System.Threading.Tasks.Task<int> UpdateServerAsync(SolidCP.EnterpriseServer.ServerInfo server)
        {
            return await base.Client.UpdateServerAsync(server);
        }

        public int UpdateServerConnectionPassword(int serverId, string password)
        {
            return base.Client.UpdateServerConnectionPassword(serverId, password);
        }

        public async System.Threading.Tasks.Task<int> UpdateServerConnectionPasswordAsync(int serverId, string password)
        {
            return await base.Client.UpdateServerConnectionPasswordAsync(serverId, password);
        }

        public int UpdateServerADPassword(int serverId, string adPassword)
        {
            return base.Client.UpdateServerADPassword(serverId, adPassword);
        }

        public async System.Threading.Tasks.Task<int> UpdateServerADPasswordAsync(int serverId, string adPassword)
        {
            return await base.Client.UpdateServerADPasswordAsync(serverId, adPassword);
        }

        public int DeleteServer(int serverId)
        {
            return base.Client.DeleteServer(serverId);
        }

        public async System.Threading.Tasks.Task<int> DeleteServerAsync(int serverId)
        {
            return await base.Client.DeleteServerAsync(serverId);
        }

        public string[] /*List*/ AutoUpdateServer(int[] /*List*/[] /*List*/ servers, string downloadLink, string fileName)
        {
            return base.Client.AutoUpdateServer(servers, downloadLink, fileName);
        }

        public async System.Threading.Tasks.Task<string[]> AutoUpdateServerAsync(int[] /*List*/[] /*List*/ servers, string downloadLink, string fileName)
        {
            return await base.Client.AutoUpdateServerAsync(servers, downloadLink, fileName);
        }

        public System.Data.DataSet GetVirtualServers()
        {
            return base.Client.GetVirtualServers();
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetVirtualServersAsync()
        {
            return await base.Client.GetVirtualServersAsync();
        }

        public System.Data.DataSet GetAvailableVirtualServices(int serverId)
        {
            return base.Client.GetAvailableVirtualServices(serverId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetAvailableVirtualServicesAsync(int serverId)
        {
            return await base.Client.GetAvailableVirtualServicesAsync(serverId);
        }

        public System.Data.DataSet GetVirtualServices(int serverId)
        {
            return base.Client.GetVirtualServices(serverId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetVirtualServicesAsync(int serverId)
        {
            return await base.Client.GetVirtualServicesAsync(serverId);
        }

        public int AddVirtualServices(int serverId, int[] ids)
        {
            return base.Client.AddVirtualServices(serverId, ids);
        }

        public async System.Threading.Tasks.Task<int> AddVirtualServicesAsync(int serverId, int[] ids)
        {
            return await base.Client.AddVirtualServicesAsync(serverId, ids);
        }

        public int DeleteVirtualServices(int serverId, int[] ids)
        {
            return base.Client.DeleteVirtualServices(serverId, ids);
        }

        public async System.Threading.Tasks.Task<int> DeleteVirtualServicesAsync(int serverId, int[] ids)
        {
            return await base.Client.DeleteVirtualServicesAsync(serverId, ids);
        }

        public int UpdateVirtualGroups(int serverId, SolidCP.EnterpriseServer.VirtualGroupInfo[] groups)
        {
            return base.Client.UpdateVirtualGroups(serverId, groups);
        }

        public async System.Threading.Tasks.Task<int> UpdateVirtualGroupsAsync(int serverId, SolidCP.EnterpriseServer.VirtualGroupInfo[] groups)
        {
            return await base.Client.UpdateVirtualGroupsAsync(serverId, groups);
        }

        public System.Data.DataSet GetRawServicesByServerId(int serverId)
        {
            return base.Client.GetRawServicesByServerId(serverId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawServicesByServerIdAsync(int serverId)
        {
            return await base.Client.GetRawServicesByServerIdAsync(serverId);
        }

        public SolidCP.EnterpriseServer.ServiceInfo[] /*List*/ GetServicesByServerId(int serverId)
        {
            return base.Client.GetServicesByServerId(serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServiceInfo[]> GetServicesByServerIdAsync(int serverId)
        {
            return await base.Client.GetServicesByServerIdAsync(serverId);
        }

        public SolidCP.EnterpriseServer.ServiceInfo[] /*List*/ GetServicesByServerIdGroupName(int serverId, string groupName)
        {
            return base.Client.GetServicesByServerIdGroupName(serverId, groupName);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServiceInfo[]> GetServicesByServerIdGroupNameAsync(int serverId, string groupName)
        {
            return await base.Client.GetServicesByServerIdGroupNameAsync(serverId, groupName);
        }

        public System.Data.DataSet GetRawServicesByGroupId(int groupId)
        {
            return base.Client.GetRawServicesByGroupId(groupId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawServicesByGroupIdAsync(int groupId)
        {
            return await base.Client.GetRawServicesByGroupIdAsync(groupId);
        }

        public System.Data.DataSet GetRawServicesByGroupName(string groupName)
        {
            return base.Client.GetRawServicesByGroupName(groupName);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawServicesByGroupNameAsync(string groupName)
        {
            return await base.Client.GetRawServicesByGroupNameAsync(groupName);
        }

        public SolidCP.EnterpriseServer.ServiceInfo GetServiceInfo(int serviceId)
        {
            return base.Client.GetServiceInfo(serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServiceInfo> GetServiceInfoAsync(int serviceId)
        {
            return await base.Client.GetServiceInfoAsync(serviceId);
        }

        public int AddService(SolidCP.EnterpriseServer.ServiceInfo service)
        {
            return base.Client.AddService(service);
        }

        public async System.Threading.Tasks.Task<int> AddServiceAsync(SolidCP.EnterpriseServer.ServiceInfo service)
        {
            return await base.Client.AddServiceAsync(service);
        }

        public int UpdateService(SolidCP.EnterpriseServer.ServiceInfo service)
        {
            return base.Client.UpdateService(service);
        }

        public async System.Threading.Tasks.Task<int> UpdateServiceAsync(SolidCP.EnterpriseServer.ServiceInfo service)
        {
            return await base.Client.UpdateServiceAsync(service);
        }

        public int DeleteService(int serviceId)
        {
            return base.Client.DeleteService(serviceId);
        }

        public async System.Threading.Tasks.Task<int> DeleteServiceAsync(int serviceId)
        {
            return await base.Client.DeleteServiceAsync(serviceId);
        }

        public string[] GetServiceSettings(int serviceId)
        {
            return base.Client.GetServiceSettings(serviceId);
        }

        public async System.Threading.Tasks.Task<string[]> GetServiceSettingsAsync(int serviceId)
        {
            return await base.Client.GetServiceSettingsAsync(serviceId);
        }

        public string[] GetServiceSettingsRDS(int serviceId)
        {
            return base.Client.GetServiceSettingsRDS(serviceId);
        }

        public async System.Threading.Tasks.Task<string[]> GetServiceSettingsRDSAsync(int serviceId)
        {
            return await base.Client.GetServiceSettingsRDSAsync(serviceId);
        }

        public int UpdateServiceSettings(int serviceId, string[] settings)
        {
            return base.Client.UpdateServiceSettings(serviceId, settings);
        }

        public async System.Threading.Tasks.Task<int> UpdateServiceSettingsAsync(int serviceId, string[] settings)
        {
            return await base.Client.UpdateServiceSettingsAsync(serviceId, settings);
        }

        public string[] InstallService(int serviceId)
        {
            return base.Client.InstallService(serviceId);
        }

        public async System.Threading.Tasks.Task<string[]> InstallServiceAsync(int serviceId)
        {
            return await base.Client.InstallServiceAsync(serviceId);
        }

        public SolidCP.EnterpriseServer.QuotaInfo GetProviderServiceQuota(int providerId)
        {
            return base.Client.GetProviderServiceQuota(providerId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.QuotaInfo> GetProviderServiceQuotaAsync(int providerId)
        {
            return await base.Client.GetProviderServiceQuotaAsync(providerId);
        }

        public string[] GetMailServiceSettingsByPackage(int packageID)
        {
            return base.Client.GetMailServiceSettingsByPackage(packageID);
        }

        public async System.Threading.Tasks.Task<string[]> GetMailServiceSettingsByPackageAsync(int packageID)
        {
            return await base.Client.GetMailServiceSettingsByPackageAsync(packageID);
        }

        public SolidCP.EnterpriseServer.ProviderInfo[] /*List*/ GetInstalledProviders(int groupId)
        {
            return base.Client.GetInstalledProviders(groupId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ProviderInfo[]> GetInstalledProvidersAsync(int groupId)
        {
            return await base.Client.GetInstalledProvidersAsync(groupId);
        }

        public SolidCP.EnterpriseServer.ResourceGroupInfo[] /*List*/ GetResourceGroups()
        {
            return base.Client.GetResourceGroups();
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ResourceGroupInfo[]> GetResourceGroupsAsync()
        {
            return await base.Client.GetResourceGroupsAsync();
        }

        public SolidCP.EnterpriseServer.ResourceGroupInfo GetResourceGroup(int groupId)
        {
            return base.Client.GetResourceGroup(groupId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ResourceGroupInfo> GetResourceGroupAsync(int groupId)
        {
            return await base.Client.GetResourceGroupAsync(groupId);
        }

        public SolidCP.EnterpriseServer.ProviderInfo GetProvider(int providerId)
        {
            return base.Client.GetProvider(providerId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ProviderInfo> GetProviderAsync(int providerId)
        {
            return await base.Client.GetProviderAsync(providerId);
        }

        public SolidCP.EnterpriseServer.ProviderInfo[] /*List*/ GetProviders()
        {
            return base.Client.GetProviders();
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ProviderInfo[]> GetProvidersAsync()
        {
            return await base.Client.GetProvidersAsync();
        }

        public SolidCP.EnterpriseServer.ProviderInfo[] /*List*/ GetProvidersByGroupId(int groupId)
        {
            return base.Client.GetProvidersByGroupId(groupId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ProviderInfo[]> GetProvidersByGroupIdAsync(int groupId)
        {
            return await base.Client.GetProvidersByGroupIdAsync(groupId);
        }

        public SolidCP.EnterpriseServer.ProviderInfo GetPackageServiceProvider(int packageId, string groupName)
        {
            return base.Client.GetPackageServiceProvider(packageId, groupName);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ProviderInfo> GetPackageServiceProviderAsync(int packageId, string groupName)
        {
            return await base.Client.GetPackageServiceProviderAsync(packageId, groupName);
        }

        public System.String GetMailFilterUrl(int packageId, string groupName)
        {
            return base.Client.GetMailFilterUrl(packageId, groupName);
        }

        public async System.Threading.Tasks.Task<System.String> GetMailFilterUrlAsync(int packageId, string groupName)
        {
            return await base.Client.GetMailFilterUrlAsync(packageId, groupName);
        }

        public System.String GetMailFilterUrlByHostingPlan(int PlanID, string groupName)
        {
            return base.Client.GetMailFilterUrlByHostingPlan(PlanID, groupName);
        }

        public async System.Threading.Tasks.Task<System.String> GetMailFilterUrlByHostingPlanAsync(int PlanID, string groupName)
        {
            return await base.Client.GetMailFilterUrlByHostingPlanAsync(PlanID, groupName);
        }

        public SolidCP.Providers.Common.BoolResult IsInstalled(int serverId, int providerId)
        {
            return base.Client.IsInstalled(serverId, providerId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.BoolResult> IsInstalledAsync(int serverId, int providerId)
        {
            return await base.Client.IsInstalledAsync(serverId, providerId);
        }

        public string GetServerVersion(int serverId)
        {
            return base.Client.GetServerVersion(serverId);
        }

        public async System.Threading.Tasks.Task<string> GetServerVersionAsync(int serverId)
        {
            return await base.Client.GetServerVersionAsync(serverId);
        }

        public string GetServerFilePath(int serverId)
        {
            return base.Client.GetServerFilePath(serverId);
        }

        public async System.Threading.Tasks.Task<string> GetServerFilePathAsync(int serverId)
        {
            return await base.Client.GetServerFilePathAsync(serverId);
        }

        public SolidCP.EnterpriseServer.VLANsPaged GetPrivateNetworVLANsPaged(int serverId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetPrivateNetworVLANsPaged(serverId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VLANsPaged> GetPrivateNetworVLANsPagedAsync(int serverId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetPrivateNetworVLANsPagedAsync(serverId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.ResultObjects.IntResult AddPrivateNetworkVLAN(int serverId, int vlan, string comments)
        {
            return base.Client.AddPrivateNetworkVLAN(serverId, vlan, comments);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> AddPrivateNetworkVLANAsync(int serverId, int vlan, string comments)
        {
            return await base.Client.AddPrivateNetworkVLANAsync(serverId, vlan, comments);
        }

        public SolidCP.Providers.Common.ResultObject DeletePrivateNetworkVLANs(int[] vlans)
        {
            return base.Client.DeletePrivateNetworkVLANs(vlans);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeletePrivateNetworkVLANsAsync(int[] vlans)
        {
            return await base.Client.DeletePrivateNetworkVLANsAsync(vlans);
        }

        public SolidCP.Providers.Common.ResultObject AddPrivateNetworkVLANsRange(int serverId, int startVLAN, int endVLAN, string comments)
        {
            return base.Client.AddPrivateNetworkVLANsRange(serverId, startVLAN, endVLAN, comments);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddPrivateNetworkVLANsRangeAsync(int serverId, int startVLAN, int endVLAN, string comments)
        {
            return await base.Client.AddPrivateNetworkVLANsRangeAsync(serverId, startVLAN, endVLAN, comments);
        }

        public SolidCP.EnterpriseServer.VLANInfo GetPrivateNetworVLAN(int vlanId)
        {
            return base.Client.GetPrivateNetworVLAN(vlanId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VLANInfo> GetPrivateNetworVLANAsync(int vlanId)
        {
            return await base.Client.GetPrivateNetworVLANAsync(vlanId);
        }

        public SolidCP.Providers.Common.ResultObject UpdatePrivateNetworVLAN(int vlanId, int serverId, int vlan, string comments)
        {
            return base.Client.UpdatePrivateNetworVLAN(vlanId, serverId, vlan, comments);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdatePrivateNetworVLANAsync(int vlanId, int serverId, int vlan, string comments)
        {
            return await base.Client.UpdatePrivateNetworVLANAsync(vlanId, serverId, vlan, comments);
        }

        public SolidCP.EnterpriseServer.PackageVLANsPaged GetPackagePrivateNetworkVLANs(int packageId, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetPackagePrivateNetworkVLANs(packageId, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageVLANsPaged> GetPackagePrivateNetworkVLANsAsync(int packageId, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetPackagePrivateNetworkVLANsAsync(packageId, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Common.ResultObject DeallocatePackageVLANs(int packageId, int[] vlanId)
        {
            return base.Client.DeallocatePackageVLANs(packageId, vlanId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeallocatePackageVLANsAsync(int packageId, int[] vlanId)
        {
            return await base.Client.DeallocatePackageVLANsAsync(packageId, vlanId);
        }

        public SolidCP.EnterpriseServer.VLANInfo[] /*List*/ GetUnallottedVLANs(int packageId, string groupName)
        {
            return base.Client.GetUnallottedVLANs(packageId, groupName);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VLANInfo[]> GetUnallottedVLANsAsync(int packageId, string groupName)
        {
            return await base.Client.GetUnallottedVLANsAsync(packageId, groupName);
        }

        public SolidCP.Providers.Common.ResultObject AllocatePackageVLANs(int packageId, string groupName, bool allocateRandom, int vlansNumber, int[] vlanId)
        {
            return base.Client.AllocatePackageVLANs(packageId, groupName, allocateRandom, vlansNumber, vlanId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AllocatePackageVLANsAsync(int packageId, string groupName, bool allocateRandom, int vlansNumber, int[] vlanId)
        {
            return await base.Client.AllocatePackageVLANsAsync(packageId, groupName, allocateRandom, vlansNumber, vlanId);
        }

        public SolidCP.EnterpriseServer.IPAddressInfo[] /*List*/ GetIPAddresses(SolidCP.EnterpriseServer.IPAddressPool pool, int serverId)
        {
            return base.Client.GetIPAddresses(pool, serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.IPAddressInfo[]> GetIPAddressesAsync(SolidCP.EnterpriseServer.IPAddressPool pool, int serverId)
        {
            return await base.Client.GetIPAddressesAsync(pool, serverId);
        }

        public SolidCP.EnterpriseServer.IPAddressesPaged GetIPAddressesPaged(SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetIPAddressesPaged(pool, serverId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.IPAddressesPaged> GetIPAddressesPagedAsync(SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetIPAddressesPagedAsync(pool, serverId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.EnterpriseServer.IPAddressInfo GetIPAddress(int addressId)
        {
            return base.Client.GetIPAddress(addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.IPAddressInfo> GetIPAddressAsync(int addressId)
        {
            return await base.Client.GetIPAddressAsync(addressId);
        }

        public SolidCP.Providers.ResultObjects.IntResult AddIPAddress(SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string externalIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            return base.Client.AddIPAddress(pool, serverId, externalIP, internalIP, subnetMask, defaultGateway, comments, VLAN);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> AddIPAddressAsync(SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string externalIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            return await base.Client.AddIPAddressAsync(pool, serverId, externalIP, internalIP, subnetMask, defaultGateway, comments, VLAN);
        }

        public SolidCP.Providers.Common.ResultObject AddIPAddressesRange(SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string externalIP, string endIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            return base.Client.AddIPAddressesRange(pool, serverId, externalIP, endIP, internalIP, subnetMask, defaultGateway, comments, VLAN);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddIPAddressesRangeAsync(SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string externalIP, string endIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            return await base.Client.AddIPAddressesRangeAsync(pool, serverId, externalIP, endIP, internalIP, subnetMask, defaultGateway, comments, VLAN);
        }

        public SolidCP.Providers.Common.ResultObject UpdateIPAddress(int addressId, SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string externalIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            return base.Client.UpdateIPAddress(addressId, pool, serverId, externalIP, internalIP, subnetMask, defaultGateway, comments, VLAN);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateIPAddressAsync(int addressId, SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string externalIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            return await base.Client.UpdateIPAddressAsync(addressId, pool, serverId, externalIP, internalIP, subnetMask, defaultGateway, comments, VLAN);
        }

        public SolidCP.Providers.Common.ResultObject UpdateIPAddresses(int[] addresses, SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            return base.Client.UpdateIPAddresses(addresses, pool, serverId, subnetMask, defaultGateway, comments, VLAN);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateIPAddressesAsync(int[] addresses, SolidCP.EnterpriseServer.IPAddressPool pool, int serverId, string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            return await base.Client.UpdateIPAddressesAsync(addresses, pool, serverId, subnetMask, defaultGateway, comments, VLAN);
        }

        public SolidCP.Providers.Common.ResultObject DeleteIPAddress(int addressId)
        {
            return base.Client.DeleteIPAddress(addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteIPAddressAsync(int addressId)
        {
            return await base.Client.DeleteIPAddressAsync(addressId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteIPAddresses(int[] addresses)
        {
            return base.Client.DeleteIPAddresses(addresses);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteIPAddressesAsync(int[] addresses)
        {
            return await base.Client.DeleteIPAddressesAsync(addresses);
        }

        public SolidCP.EnterpriseServer.IPAddressInfo[] /*List*/ GetUnallottedIPAddresses(int packageId, string groupName, SolidCP.EnterpriseServer.IPAddressPool pool)
        {
            return base.Client.GetUnallottedIPAddresses(packageId, groupName, pool);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.IPAddressInfo[]> GetUnallottedIPAddressesAsync(int packageId, string groupName, SolidCP.EnterpriseServer.IPAddressPool pool)
        {
            return await base.Client.GetUnallottedIPAddressesAsync(packageId, groupName, pool);
        }

        public SolidCP.EnterpriseServer.PackageIPAddressesPaged GetPackageIPAddresses(int packageId, int orgId, SolidCP.EnterpriseServer.IPAddressPool pool, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            return base.Client.GetPackageIPAddresses(packageId, orgId, pool, filterColumn, filterValue, sortColumn, startRow, maximumRows, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageIPAddressesPaged> GetPackageIPAddressesAsync(int packageId, int orgId, SolidCP.EnterpriseServer.IPAddressPool pool, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            return await base.Client.GetPackageIPAddressesAsync(packageId, orgId, pool, filterColumn, filterValue, sortColumn, startRow, maximumRows, recursive);
        }

        public int GetPackageIPAddressesCount(int packageId, int orgId, SolidCP.EnterpriseServer.IPAddressPool pool)
        {
            return base.Client.GetPackageIPAddressesCount(packageId, orgId, pool);
        }

        public async System.Threading.Tasks.Task<int> GetPackageIPAddressesCountAsync(int packageId, int orgId, SolidCP.EnterpriseServer.IPAddressPool pool)
        {
            return await base.Client.GetPackageIPAddressesCountAsync(packageId, orgId, pool);
        }

        public SolidCP.EnterpriseServer.PackageIPAddress[] /*List*/ GetPackageUnassignedIPAddresses(int packageId, int orgId, SolidCP.EnterpriseServer.IPAddressPool pool)
        {
            return base.Client.GetPackageUnassignedIPAddresses(packageId, orgId, pool);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageIPAddress[]> GetPackageUnassignedIPAddressesAsync(int packageId, int orgId, SolidCP.EnterpriseServer.IPAddressPool pool)
        {
            return await base.Client.GetPackageUnassignedIPAddressesAsync(packageId, orgId, pool);
        }

        public SolidCP.Providers.Common.ResultObject AllocatePackageIPAddresses(int packageId, int orgId, string groupName, SolidCP.EnterpriseServer.IPAddressPool pool, bool allocateRandom, int addressesNumber, int[] addressId)
        {
            return base.Client.AllocatePackageIPAddresses(packageId, orgId, groupName, pool, allocateRandom, addressesNumber, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AllocatePackageIPAddressesAsync(int packageId, int orgId, string groupName, SolidCP.EnterpriseServer.IPAddressPool pool, bool allocateRandom, int addressesNumber, int[] addressId)
        {
            return await base.Client.AllocatePackageIPAddressesAsync(packageId, orgId, groupName, pool, allocateRandom, addressesNumber, addressId);
        }

        public SolidCP.Providers.Common.ResultObject AllocateMaximumPackageIPAddresses(int packageId, string groupName, SolidCP.EnterpriseServer.IPAddressPool pool)
        {
            return base.Client.AllocateMaximumPackageIPAddresses(packageId, groupName, pool);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AllocateMaximumPackageIPAddressesAsync(int packageId, string groupName, SolidCP.EnterpriseServer.IPAddressPool pool)
        {
            return await base.Client.AllocateMaximumPackageIPAddressesAsync(packageId, groupName, pool);
        }

        public SolidCP.Providers.Common.ResultObject DeallocatePackageIPAddresses(int packageId, int[] addressId)
        {
            return base.Client.DeallocatePackageIPAddresses(packageId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeallocatePackageIPAddressesAsync(int packageId, int[] addressId)
        {
            return await base.Client.DeallocatePackageIPAddressesAsync(packageId, addressId);
        }

        public SolidCP.EnterpriseServer.ClusterInfo[] /*List*/ GetClusters()
        {
            return base.Client.GetClusters();
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ClusterInfo[]> GetClustersAsync()
        {
            return await base.Client.GetClustersAsync();
        }

        public int AddCluster(SolidCP.EnterpriseServer.ClusterInfo cluster)
        {
            return base.Client.AddCluster(cluster);
        }

        public async System.Threading.Tasks.Task<int> AddClusterAsync(SolidCP.EnterpriseServer.ClusterInfo cluster)
        {
            return await base.Client.AddClusterAsync(cluster);
        }

        public int DeleteCluster(int clusterId)
        {
            return base.Client.DeleteCluster(clusterId);
        }

        public async System.Threading.Tasks.Task<int> DeleteClusterAsync(int clusterId)
        {
            return await base.Client.DeleteClusterAsync(clusterId);
        }

        public System.Data.DataSet GetRawDnsRecordsByService(int serviceId)
        {
            return base.Client.GetRawDnsRecordsByService(serviceId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawDnsRecordsByServiceAsync(int serviceId)
        {
            return await base.Client.GetRawDnsRecordsByServiceAsync(serviceId);
        }

        public System.Data.DataSet GetRawDnsRecordsByServer(int serverId)
        {
            return base.Client.GetRawDnsRecordsByServer(serverId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawDnsRecordsByServerAsync(int serverId)
        {
            return await base.Client.GetRawDnsRecordsByServerAsync(serverId);
        }

        public System.Data.DataSet GetRawDnsRecordsByPackage(int packageId)
        {
            return base.Client.GetRawDnsRecordsByPackage(packageId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawDnsRecordsByPackageAsync(int packageId)
        {
            return await base.Client.GetRawDnsRecordsByPackageAsync(packageId);
        }

        public System.Data.DataSet GetRawDnsRecordsByGroup(int groupId)
        {
            return base.Client.GetRawDnsRecordsByGroup(groupId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawDnsRecordsByGroupAsync(int groupId)
        {
            return await base.Client.GetRawDnsRecordsByGroupAsync(groupId);
        }

        public SolidCP.EnterpriseServer.GlobalDnsRecord[] /*List*/ GetDnsRecordsByService(int serviceId)
        {
            return base.Client.GetDnsRecordsByService(serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.GlobalDnsRecord[]> GetDnsRecordsByServiceAsync(int serviceId)
        {
            return await base.Client.GetDnsRecordsByServiceAsync(serviceId);
        }

        public SolidCP.EnterpriseServer.GlobalDnsRecord[] /*List*/ GetDnsRecordsByServer(int serverId)
        {
            return base.Client.GetDnsRecordsByServer(serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.GlobalDnsRecord[]> GetDnsRecordsByServerAsync(int serverId)
        {
            return await base.Client.GetDnsRecordsByServerAsync(serverId);
        }

        public SolidCP.EnterpriseServer.GlobalDnsRecord[] /*List*/ GetDnsRecordsByPackage(int packageId)
        {
            return base.Client.GetDnsRecordsByPackage(packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.GlobalDnsRecord[]> GetDnsRecordsByPackageAsync(int packageId)
        {
            return await base.Client.GetDnsRecordsByPackageAsync(packageId);
        }

        public SolidCP.EnterpriseServer.GlobalDnsRecord[] /*List*/ GetDnsRecordsByGroup(int groupId)
        {
            return base.Client.GetDnsRecordsByGroup(groupId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.GlobalDnsRecord[]> GetDnsRecordsByGroupAsync(int groupId)
        {
            return await base.Client.GetDnsRecordsByGroupAsync(groupId);
        }

        public SolidCP.EnterpriseServer.GlobalDnsRecord GetDnsRecord(int recordId)
        {
            return base.Client.GetDnsRecord(recordId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.GlobalDnsRecord> GetDnsRecordAsync(int recordId)
        {
            return await base.Client.GetDnsRecordAsync(recordId);
        }

        public int AddDnsRecord(SolidCP.EnterpriseServer.GlobalDnsRecord record)
        {
            return base.Client.AddDnsRecord(record);
        }

        public async System.Threading.Tasks.Task<int> AddDnsRecordAsync(SolidCP.EnterpriseServer.GlobalDnsRecord record)
        {
            return await base.Client.AddDnsRecordAsync(record);
        }

        public int UpdateDnsRecord(SolidCP.EnterpriseServer.GlobalDnsRecord record)
        {
            return base.Client.UpdateDnsRecord(record);
        }

        public async System.Threading.Tasks.Task<int> UpdateDnsRecordAsync(SolidCP.EnterpriseServer.GlobalDnsRecord record)
        {
            return await base.Client.UpdateDnsRecordAsync(record);
        }

        public int DeleteDnsRecord(int recordId)
        {
            return base.Client.DeleteDnsRecord(recordId);
        }

        public async System.Threading.Tasks.Task<int> DeleteDnsRecordAsync(int recordId)
        {
            return await base.Client.DeleteDnsRecordAsync(recordId);
        }

        public SolidCP.Providers.DomainLookup.DnsRecordInfo[] /*List*/ GetDomainDnsRecords(int domainId)
        {
            return base.Client.GetDomainDnsRecords(domainId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.DomainLookup.DnsRecordInfo[]> GetDomainDnsRecordsAsync(int domainId)
        {
            return await base.Client.GetDomainDnsRecordsAsync(domainId);
        }

        public SolidCP.EnterpriseServer.DomainInfo[] /*List*/ GetDomains(int packageId)
        {
            return base.Client.GetDomains(packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DomainInfo[]> GetDomainsAsync(int packageId)
        {
            return await base.Client.GetDomainsAsync(packageId);
        }

        public SolidCP.EnterpriseServer.DomainInfo[] /*List*/ GetDomainsByDomainId(int domainId)
        {
            return base.Client.GetDomainsByDomainId(domainId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DomainInfo[]> GetDomainsByDomainIdAsync(int domainId)
        {
            return await base.Client.GetDomainsByDomainIdAsync(domainId);
        }

        public SolidCP.EnterpriseServer.DomainInfo[] /*List*/ GetMyDomains(int packageId)
        {
            return base.Client.GetMyDomains(packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DomainInfo[]> GetMyDomainsAsync(int packageId)
        {
            return await base.Client.GetMyDomainsAsync(packageId);
        }

        public SolidCP.EnterpriseServer.DomainInfo[] /*List*/ GetResellerDomains(int packageId)
        {
            return base.Client.GetResellerDomains(packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DomainInfo[]> GetResellerDomainsAsync(int packageId)
        {
            return await base.Client.GetResellerDomainsAsync(packageId);
        }

        public System.Data.DataSet GetDomainsPaged(int packageId, int serverId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetDomainsPaged(packageId, serverId, recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetDomainsPagedAsync(int packageId, int serverId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetDomainsPagedAsync(packageId, serverId, recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.EnterpriseServer.DomainInfo GetDomain(int domainId)
        {
            return base.Client.GetDomain(domainId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DomainInfo> GetDomainAsync(int domainId)
        {
            return await base.Client.GetDomainAsync(domainId);
        }

        public int AddDomain(SolidCP.EnterpriseServer.DomainInfo domain)
        {
            return base.Client.AddDomain(domain);
        }

        public async System.Threading.Tasks.Task<int> AddDomainAsync(SolidCP.EnterpriseServer.DomainInfo domain)
        {
            return await base.Client.AddDomainAsync(domain);
        }

        public int AddDomainWithProvisioning(int packageId, string domainName, SolidCP.EnterpriseServer.DomainType domainType, bool createWebSite, int pointWebSiteId, int pointMailDomainId, bool createDnsZone, bool createPreviewDomain, bool allowSubDomains, string hostName)
        {
            return base.Client.AddDomainWithProvisioning(packageId, domainName, domainType, createWebSite, pointWebSiteId, pointMailDomainId, createDnsZone, createPreviewDomain, allowSubDomains, hostName);
        }

        public async System.Threading.Tasks.Task<int> AddDomainWithProvisioningAsync(int packageId, string domainName, SolidCP.EnterpriseServer.DomainType domainType, bool createWebSite, int pointWebSiteId, int pointMailDomainId, bool createDnsZone, bool createPreviewDomain, bool allowSubDomains, string hostName)
        {
            return await base.Client.AddDomainWithProvisioningAsync(packageId, domainName, domainType, createWebSite, pointWebSiteId, pointMailDomainId, createDnsZone, createPreviewDomain, allowSubDomains, hostName);
        }

        public int UpdateDomain(SolidCP.EnterpriseServer.DomainInfo domain)
        {
            return base.Client.UpdateDomain(domain);
        }

        public async System.Threading.Tasks.Task<int> UpdateDomainAsync(SolidCP.EnterpriseServer.DomainInfo domain)
        {
            return await base.Client.UpdateDomainAsync(domain);
        }

        public int DeleteDomain(int domainId)
        {
            return base.Client.DeleteDomain(domainId);
        }

        public async System.Threading.Tasks.Task<int> DeleteDomainAsync(int domainId)
        {
            return await base.Client.DeleteDomainAsync(domainId);
        }

        public int DetachDomain(int domainId)
        {
            return base.Client.DetachDomain(domainId);
        }

        public async System.Threading.Tasks.Task<int> DetachDomainAsync(int domainId)
        {
            return await base.Client.DetachDomainAsync(domainId);
        }

        public int EnableDomainDns(int domainId)
        {
            return base.Client.EnableDomainDns(domainId);
        }

        public async System.Threading.Tasks.Task<int> EnableDomainDnsAsync(int domainId)
        {
            return await base.Client.EnableDomainDnsAsync(domainId);
        }

        public int DisableDomainDns(int domainId)
        {
            return base.Client.DisableDomainDns(domainId);
        }

        public async System.Threading.Tasks.Task<int> DisableDomainDnsAsync(int domainId)
        {
            return await base.Client.DisableDomainDnsAsync(domainId);
        }

        public int CreateDomainPreviewDomain(string hostName, int domainId)
        {
            return base.Client.CreateDomainPreviewDomain(hostName, domainId);
        }

        public async System.Threading.Tasks.Task<int> CreateDomainPreviewDomainAsync(string hostName, int domainId)
        {
            return await base.Client.CreateDomainPreviewDomainAsync(hostName, domainId);
        }

        public int DeleteDomainPreviewDomain(int domainId)
        {
            return base.Client.DeleteDomainPreviewDomain(domainId);
        }

        public async System.Threading.Tasks.Task<int> DeleteDomainPreviewDomainAsync(int domainId)
        {
            return await base.Client.DeleteDomainPreviewDomainAsync(domainId);
        }

        public SolidCP.Providers.DNS.DnsRecord[] GetDnsZoneRecords(int domainId)
        {
            return base.Client.GetDnsZoneRecords(domainId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.DNS.DnsRecord[]> GetDnsZoneRecordsAsync(int domainId)
        {
            return await base.Client.GetDnsZoneRecordsAsync(domainId);
        }

        public System.Data.DataSet GetRawDnsZoneRecords(int domainId)
        {
            return base.Client.GetRawDnsZoneRecords(domainId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawDnsZoneRecordsAsync(int domainId)
        {
            return await base.Client.GetRawDnsZoneRecordsAsync(domainId);
        }

        public int AddDnsZoneRecord(int domainId, string recordName, SolidCP.Providers.DNS.DnsRecordType recordType, string recordData, int mxPriority, int srvPriority, int srvWeight, int srvPortNumber)
        {
            return base.Client.AddDnsZoneRecord(domainId, recordName, recordType, recordData, mxPriority, srvPriority, srvWeight, srvPortNumber);
        }

        public async System.Threading.Tasks.Task<int> AddDnsZoneRecordAsync(int domainId, string recordName, SolidCP.Providers.DNS.DnsRecordType recordType, string recordData, int mxPriority, int srvPriority, int srvWeight, int srvPortNumber)
        {
            return await base.Client.AddDnsZoneRecordAsync(domainId, recordName, recordType, recordData, mxPriority, srvPriority, srvWeight, srvPortNumber);
        }

        public int UpdateDnsZoneRecord(int domainId, string originalRecordName, string originalRecordData, string recordName, SolidCP.Providers.DNS.DnsRecordType recordType, string recordData, int mxPriority, int srvPriority, int srvWeight, int srvPortNumber)
        {
            return base.Client.UpdateDnsZoneRecord(domainId, originalRecordName, originalRecordData, recordName, recordType, recordData, mxPriority, srvPriority, srvWeight, srvPortNumber);
        }

        public async System.Threading.Tasks.Task<int> UpdateDnsZoneRecordAsync(int domainId, string originalRecordName, string originalRecordData, string recordName, SolidCP.Providers.DNS.DnsRecordType recordType, string recordData, int mxPriority, int srvPriority, int srvWeight, int srvPortNumber)
        {
            return await base.Client.UpdateDnsZoneRecordAsync(domainId, originalRecordName, originalRecordData, recordName, recordType, recordData, mxPriority, srvPriority, srvWeight, srvPortNumber);
        }

        public int DeleteDnsZoneRecord(int domainId, string recordName, SolidCP.Providers.DNS.DnsRecordType recordType, string recordData)
        {
            return base.Client.DeleteDnsZoneRecord(domainId, recordName, recordType, recordData);
        }

        public async System.Threading.Tasks.Task<int> DeleteDnsZoneRecordAsync(int domainId, string recordName, SolidCP.Providers.DNS.DnsRecordType recordType, string recordData)
        {
            return await base.Client.DeleteDnsZoneRecordAsync(domainId, recordName, recordType, recordData);
        }

        public SolidCP.Providers.OS.TerminalSession[] GetTerminalServicesSessions(int serverId)
        {
            return base.Client.GetTerminalServicesSessions(serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.TerminalSession[]> GetTerminalServicesSessionsAsync(int serverId)
        {
            return await base.Client.GetTerminalServicesSessionsAsync(serverId);
        }

        public int CloseTerminalServicesSession(int serverId, int sessionId)
        {
            return base.Client.CloseTerminalServicesSession(serverId, sessionId);
        }

        public async System.Threading.Tasks.Task<int> CloseTerminalServicesSessionAsync(int serverId, int sessionId)
        {
            return await base.Client.CloseTerminalServicesSessionAsync(serverId, sessionId);
        }

        public SolidCP.Providers.OS.OSProcess[] GetOSProcesses(int serverId)
        {
            return base.Client.GetOSProcesses(serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.OSProcess[]> GetOSProcessesAsync(int serverId)
        {
            return await base.Client.GetOSProcessesAsync(serverId);
        }

        public int TerminateOSProcess(int serverId, int pid)
        {
            return base.Client.TerminateOSProcess(serverId, pid);
        }

        public async System.Threading.Tasks.Task<int> TerminateOSProcessAsync(int serverId, int pid)
        {
            return await base.Client.TerminateOSProcessAsync(serverId, pid);
        }

        public bool CheckLoadUserProfile(int serverId)
        {
            return base.Client.CheckLoadUserProfile(serverId);
        }

        public async System.Threading.Tasks.Task<bool> CheckLoadUserProfileAsync(int serverId)
        {
            return await base.Client.CheckLoadUserProfileAsync(serverId);
        }

        public void EnableLoadUserProfile(int serverId)
        {
            base.Client.EnableLoadUserProfile(serverId);
        }

        public async System.Threading.Tasks.Task EnableLoadUserProfileAsync(int serverId)
        {
            await base.Client.EnableLoadUserProfileAsync(serverId);
        }

        public void InitWPIFeeds(int serverId)
        {
            base.Client.InitWPIFeeds(serverId);
        }

        public async System.Threading.Tasks.Task InitWPIFeedsAsync(int serverId)
        {
            await base.Client.InitWPIFeedsAsync(serverId);
        }

        public SolidCP.Server.WPITab[] GetWPITabs(int serverId)
        {
            return base.Client.GetWPITabs(serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPITab[]> GetWPITabsAsync(int serverId)
        {
            return await base.Client.GetWPITabsAsync(serverId);
        }

        public SolidCP.Server.WPIKeyword[] GetWPIKeywords(int serverId)
        {
            return base.Client.GetWPIKeywords(serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIKeyword[]> GetWPIKeywordsAsync(int serverId)
        {
            return await base.Client.GetWPIKeywordsAsync(serverId);
        }

        public SolidCP.Server.WPIProduct[] GetWPIProducts(int serverId, string tabId, string keywordId)
        {
            return base.Client.GetWPIProducts(serverId, tabId, keywordId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsAsync(int serverId, string tabId, string keywordId)
        {
            return await base.Client.GetWPIProductsAsync(serverId, tabId, keywordId);
        }

        public SolidCP.Server.WPIProduct[] GetWPIProductsFiltered(int serverId, string keywordId)
        {
            return base.Client.GetWPIProductsFiltered(serverId, keywordId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsFilteredAsync(int serverId, string keywordId)
        {
            return await base.Client.GetWPIProductsFilteredAsync(serverId, keywordId);
        }

        public SolidCP.Server.WPIProduct GetWPIProductById(int serverId, string productdId)
        {
            return base.Client.GetWPIProductById(serverId, productdId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIProduct> GetWPIProductByIdAsync(int serverId, string productdId)
        {
            return await base.Client.GetWPIProductByIdAsync(serverId, productdId);
        }

        public SolidCP.Server.WPIProduct[] GetWPIProductsWithDependencies(int serverId, string[] products)
        {
            return base.Client.GetWPIProductsWithDependencies(serverId, products);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsWithDependenciesAsync(int serverId, string[] products)
        {
            return await base.Client.GetWPIProductsWithDependenciesAsync(serverId, products);
        }

        public void InstallWPIProducts(int serverId, string[] products)
        {
            base.Client.InstallWPIProducts(serverId, products);
        }

        public async System.Threading.Tasks.Task InstallWPIProductsAsync(int serverId, string[] products)
        {
            await base.Client.InstallWPIProductsAsync(serverId, products);
        }

        public void CancelInstallWPIProducts(int serviceId)
        {
            base.Client.CancelInstallWPIProducts(serviceId);
        }

        public async System.Threading.Tasks.Task CancelInstallWPIProductsAsync(int serviceId)
        {
            await base.Client.CancelInstallWPIProductsAsync(serviceId);
        }

        public string GetWPIStatus(int serverId)
        {
            return base.Client.GetWPIStatus(serverId);
        }

        public async System.Threading.Tasks.Task<string> GetWPIStatusAsync(int serverId)
        {
            return await base.Client.GetWPIStatusAsync(serverId);
        }

        public string WpiGetLogFileDirectory(int serverId)
        {
            return base.Client.WpiGetLogFileDirectory(serverId);
        }

        public async System.Threading.Tasks.Task<string> WpiGetLogFileDirectoryAsync(int serverId)
        {
            return await base.Client.WpiGetLogFileDirectoryAsync(serverId);
        }

        public SolidCP.Providers.SettingPair[] WpiGetLogsInDirectory(int serverId, string Path)
        {
            return base.Client.WpiGetLogsInDirectory(serverId, Path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SettingPair[]> WpiGetLogsInDirectoryAsync(int serverId, string Path)
        {
            return await base.Client.WpiGetLogsInDirectoryAsync(serverId, Path);
        }

        public SolidCP.Providers.OS.OSService[] GetOSServices(int serverId)
        {
            return base.Client.GetOSServices(serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.OSService[]> GetOSServicesAsync(int serverId)
        {
            return await base.Client.GetOSServicesAsync(serverId);
        }

        public int ChangeOSServiceStatus(int serverId, string id, SolidCP.Providers.OS.OSServiceStatus status)
        {
            return base.Client.ChangeOSServiceStatus(serverId, id, status);
        }

        public async System.Threading.Tasks.Task<int> ChangeOSServiceStatusAsync(int serverId, string id, SolidCP.Providers.OS.OSServiceStatus status)
        {
            return await base.Client.ChangeOSServiceStatusAsync(serverId, id, status);
        }

        public string[] GetLogNames(int serverId)
        {
            return base.Client.GetLogNames(serverId);
        }

        public async System.Threading.Tasks.Task<string[]> GetLogNamesAsync(int serverId)
        {
            return await base.Client.GetLogNamesAsync(serverId);
        }

        public SolidCP.Providers.OS.SystemLogEntry[] GetLogEntries(int serverId, string logName)
        {
            return base.Client.GetLogEntries(serverId, logName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemLogEntry[]> GetLogEntriesAsync(int serverId, string logName)
        {
            return await base.Client.GetLogEntriesAsync(serverId, logName);
        }

        public SolidCP.Providers.OS.SystemLogEntriesPaged GetLogEntriesPaged(int serverId, string logName, int startRow, int maximumRows)
        {
            return base.Client.GetLogEntriesPaged(serverId, logName, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemLogEntriesPaged> GetLogEntriesPagedAsync(int serverId, string logName, int startRow, int maximumRows)
        {
            return await base.Client.GetLogEntriesPagedAsync(serverId, logName, startRow, maximumRows);
        }

        public int ClearLog(int serverId, string logName)
        {
            return base.Client.ClearLog(serverId, logName);
        }

        public async System.Threading.Tasks.Task<int> ClearLogAsync(int serverId, string logName)
        {
            return await base.Client.ClearLogAsync(serverId, logName);
        }

        public int RebootSystem(int serverId)
        {
            return base.Client.RebootSystem(serverId);
        }

        public async System.Threading.Tasks.Task<int> RebootSystemAsync(int serverId)
        {
            return await base.Client.RebootSystemAsync(serverId);
        }

        public SolidCP.Providers.OS.Memory GetMemoryPackageId(int packageId)
        {
            return base.Client.GetMemoryPackageId(packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.Memory> GetMemoryPackageIdAsync(int packageId)
        {
            return await base.Client.GetMemoryPackageIdAsync(packageId);
        }

        public SolidCP.Providers.OS.Memory GetMemory(int serverId)
        {
            return base.Client.GetMemory(serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.Memory> GetMemoryAsync(int serverId)
        {
            return await base.Client.GetMemoryAsync(serverId);
        }
    }
}
#endif