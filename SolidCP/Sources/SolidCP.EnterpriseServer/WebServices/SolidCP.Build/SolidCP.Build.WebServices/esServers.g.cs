#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.EnterpriseServer.Base.Common;
using SolidCP.Providers.Common;
using SolidCP.Providers.DNS;
using SolidCP.Server;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers;
using SolidCP.Providers.OS;
using SolidCP.Providers.DomainLookup;
using SolidCP.EnterpriseServer;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif

namespace SolidCP.EnterpriseServer.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesServers
    {
        [WebMethod]
        [OperationContract]
        List<ServerInfo> GetAllServers();
        [WebMethod]
        [OperationContract]
        DataSet GetRawAllServers();
        [WebMethod]
        [OperationContract]
        List<ServerInfo> GetServers();
        [WebMethod]
        [OperationContract]
        DataSet GetRawServers();
        [WebMethod]
        [OperationContract]
        ServerInfo GetServerShortDetails(int serverId);
        [WebMethod]
        [OperationContract]
        ServerInfo GetServerById(int serverId);
        [WebMethod]
        [OperationContract]
        ServerInfo GetServerByName(string serverName);
        [WebMethod]
        [OperationContract]
        int CheckServerAvailable(string serverUrl, string password);
        [WebMethod]
        [OperationContract]
        int AddServer(ServerInfo server, bool autoDiscovery);
        [WebMethod]
        [OperationContract]
        int UpdateServer(ServerInfo server);
        [WebMethod]
        [OperationContract]
        int UpdateServerConnectionPassword(int serverId, string password);
        [WebMethod]
        [OperationContract]
        int UpdateServerADPassword(int serverId, string adPassword);
        [WebMethod]
        [OperationContract]
        int DeleteServer(int serverId);
        [WebMethod]
        [OperationContract]
        List<string> AutoUpdateServer(List<List<int>> servers, string downloadLink, string fileName);
        [WebMethod]
        [OperationContract]
        DataSet GetVirtualServers();
        [WebMethod]
        [OperationContract]
        DataSet GetAvailableVirtualServices(int serverId);
        [WebMethod]
        [OperationContract]
        DataSet GetVirtualServices(int serverId);
        [WebMethod]
        [OperationContract]
        int AddVirtualServices(int serverId, int[] ids);
        [WebMethod]
        [OperationContract]
        int DeleteVirtualServices(int serverId, int[] ids);
        [WebMethod]
        [OperationContract]
        int UpdateVirtualGroups(int serverId, VirtualGroupInfo[] groups);
        [WebMethod]
        [OperationContract]
        DataSet GetRawServicesByServerId(int serverId);
        [WebMethod]
        [OperationContract]
        List<ServiceInfo> GetServicesByServerId(int serverId);
        [WebMethod]
        [OperationContract]
        List<ServiceInfo> GetServicesByServerIdGroupName(int serverId, string groupName);
        [WebMethod]
        [OperationContract]
        DataSet GetRawServicesByGroupId(int groupId);
        [WebMethod]
        [OperationContract]
        DataSet GetRawServicesByGroupName(string groupName);
        [WebMethod]
        [OperationContract]
        ServiceInfo GetServiceInfo(int serviceId);
        [WebMethod]
        [OperationContract]
        int AddService(ServiceInfo service);
        [WebMethod]
        [OperationContract]
        int UpdateService(ServiceInfo service);
        [WebMethod]
        [OperationContract]
        int DeleteService(int serviceId);
        [WebMethod]
        [OperationContract]
        string[] GetServiceSettings(int serviceId);
        [WebMethod]
        [OperationContract]
        string[] GetServiceSettingsRDS(int serviceId);
        [WebMethod]
        [OperationContract]
        int UpdateServiceSettings(int serviceId, string[] settings);
        [WebMethod]
        [OperationContract]
        string[] InstallService(int serviceId);
        [WebMethod]
        [OperationContract]
        QuotaInfo GetProviderServiceQuota(int providerId);
        [WebMethod]
        [OperationContract]
        string[] GetMailServiceSettingsByPackage(int packageID);
        [WebMethod]
        [OperationContract]
        List<ProviderInfo> GetInstalledProviders(int groupId);
        [WebMethod]
        [OperationContract]
        List<ResourceGroupInfo> GetResourceGroups();
        [WebMethod]
        [OperationContract]
        ResourceGroupInfo GetResourceGroup(int groupId);
        [WebMethod]
        [OperationContract]
        ProviderInfo GetProvider(int providerId);
        [WebMethod]
        [OperationContract]
        List<ProviderInfo> GetProviders();
        [WebMethod]
        [OperationContract]
        List<ProviderInfo> GetProvidersByGroupId(int groupId);
        [WebMethod]
        [OperationContract]
        ProviderInfo GetPackageServiceProvider(int packageId, string groupName);
        [WebMethod]
        [OperationContract]
        String GetMailFilterUrl(int packageId, string groupName);
        [WebMethod]
        [OperationContract]
        String GetMailFilterUrlByHostingPlan(int PlanID, string groupName);
        [WebMethod]
        [OperationContract]
        BoolResult IsInstalled(int serverId, int providerId);
        [WebMethod]
        [OperationContract]
        string GetServerVersion(int serverId);
        [WebMethod]
        [OperationContract]
        string GetServerFilePath(int serverId);
        [WebMethod]
        [OperationContract]
        VLANsPaged GetPrivateNetworVLANsPaged(int serverId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        IntResult AddPrivateNetworkVLAN(int serverId, int vlan, string comments);
        [WebMethod]
        [OperationContract]
        ResultObject DeletePrivateNetworkVLANs(int[] vlans);
        [WebMethod]
        [OperationContract]
        ResultObject AddPrivateNetworkVLANsRange(int serverId, int startVLAN, int endVLAN, string comments);
        [WebMethod]
        [OperationContract]
        VLANInfo GetPrivateNetworVLAN(int vlanId);
        [WebMethod]
        [OperationContract]
        ResultObject UpdatePrivateNetworVLAN(int vlanId, int serverId, int vlan, string comments);
        [WebMethod]
        [OperationContract]
        PackageVLANsPaged GetPackagePrivateNetworkVLANs(int packageId, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        ResultObject DeallocatePackageVLANs(int packageId, int[] vlanId);
        [WebMethod]
        [OperationContract]
        List<VLANInfo> GetUnallottedVLANs(int packageId, string groupName);
        [WebMethod]
        [OperationContract]
        ResultObject AllocatePackageVLANs(int packageId, string groupName, bool allocateRandom, int vlansNumber, int[] vlanId);
        [WebMethod]
        [OperationContract]
        List<IPAddressInfo> GetIPAddresses(IPAddressPool pool, int serverId);
        [WebMethod]
        [OperationContract]
        IPAddressesPaged GetIPAddressesPaged(IPAddressPool pool, int serverId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        IPAddressInfo GetIPAddress(int addressId);
        [WebMethod]
        [OperationContract]
        IntResult AddIPAddress(IPAddressPool pool, int serverId, string externalIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN);
        [WebMethod]
        [OperationContract]
        ResultObject AddIPAddressesRange(IPAddressPool pool, int serverId, string externalIP, string endIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN);
        [WebMethod]
        [OperationContract]
        ResultObject UpdateIPAddress(int addressId, IPAddressPool pool, int serverId, string externalIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN);
        [WebMethod]
        [OperationContract]
        ResultObject UpdateIPAddresses(int[] addresses, IPAddressPool pool, int serverId, string subnetMask, string defaultGateway, string comments, int VLAN);
        [WebMethod]
        [OperationContract]
        ResultObject DeleteIPAddress(int addressId);
        [WebMethod]
        [OperationContract]
        ResultObject DeleteIPAddresses(int[] addresses);
        [WebMethod]
        [OperationContract]
        List<IPAddressInfo> GetUnallottedIPAddresses(int packageId, string groupName, IPAddressPool pool);
        [WebMethod]
        [OperationContract]
        PackageIPAddressesPaged GetPackageIPAddresses(int packageId, int orgId, IPAddressPool pool, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive);
        [WebMethod]
        [OperationContract]
        int GetPackageIPAddressesCount(int packageId, int orgId, IPAddressPool pool);
        [WebMethod]
        [OperationContract]
        List<PackageIPAddress> GetPackageUnassignedIPAddresses(int packageId, int orgId, IPAddressPool pool);
        [WebMethod]
        [OperationContract]
        ResultObject AllocatePackageIPAddresses(int packageId, int orgId, string groupName, IPAddressPool pool, bool allocateRandom, int addressesNumber, int[] addressId);
        [WebMethod]
        [OperationContract]
        ResultObject AllocateMaximumPackageIPAddresses(int packageId, string groupName, IPAddressPool pool);
        [WebMethod]
        [OperationContract]
        ResultObject DeallocatePackageIPAddresses(int packageId, int[] addressId);
        [WebMethod]
        [OperationContract]
        List<ClusterInfo> GetClusters();
        [WebMethod]
        [OperationContract]
        int AddCluster(ClusterInfo cluster);
        [WebMethod]
        [OperationContract]
        int DeleteCluster(int clusterId);
        [WebMethod]
        [OperationContract]
        DataSet GetRawDnsRecordsByService(int serviceId);
        [WebMethod]
        [OperationContract]
        DataSet GetRawDnsRecordsByServer(int serverId);
        [WebMethod]
        [OperationContract]
        DataSet GetRawDnsRecordsByPackage(int packageId);
        [WebMethod]
        [OperationContract]
        DataSet GetRawDnsRecordsByGroup(int groupId);
        [WebMethod]
        [OperationContract]
        List<GlobalDnsRecord> GetDnsRecordsByService(int serviceId);
        [WebMethod]
        [OperationContract]
        List<GlobalDnsRecord> GetDnsRecordsByServer(int serverId);
        [WebMethod]
        [OperationContract]
        List<GlobalDnsRecord> GetDnsRecordsByPackage(int packageId);
        [WebMethod]
        [OperationContract]
        List<GlobalDnsRecord> GetDnsRecordsByGroup(int groupId);
        [WebMethod]
        [OperationContract]
        GlobalDnsRecord GetDnsRecord(int recordId);
        [WebMethod]
        [OperationContract]
        int AddDnsRecord(GlobalDnsRecord record);
        [WebMethod]
        [OperationContract]
        int UpdateDnsRecord(GlobalDnsRecord record);
        [WebMethod]
        [OperationContract]
        int DeleteDnsRecord(int recordId);
        [WebMethod]
        [OperationContract]
        List<DnsRecordInfo> GetDomainDnsRecords(int domainId);
        [WebMethod]
        [OperationContract]
        List<DomainInfo> GetDomains(int packageId);
        [WebMethod]
        [OperationContract]
        List<DomainInfo> GetDomainsByDomainId(int domainId);
        [WebMethod]
        [OperationContract]
        List<DomainInfo> GetMyDomains(int packageId);
        [WebMethod]
        [OperationContract]
        List<DomainInfo> GetResellerDomains(int packageId);
        [WebMethod]
        [OperationContract]
        DataSet GetDomainsPaged(int packageId, int serverId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        DomainInfo GetDomain(int domainId);
        [WebMethod]
        [OperationContract]
        int AddDomain(DomainInfo domain);
        [WebMethod]
        [OperationContract]
        int AddDomainWithProvisioning(int packageId, string domainName, DomainType domainType, bool createWebSite, int pointWebSiteId, int pointMailDomainId, bool createDnsZone, bool createPreviewDomain, bool allowSubDomains, string hostName);
        [WebMethod]
        [OperationContract]
        int UpdateDomain(DomainInfo domain);
        [WebMethod]
        [OperationContract]
        int DeleteDomain(int domainId);
        [WebMethod]
        [OperationContract]
        int DetachDomain(int domainId);
        [WebMethod]
        [OperationContract]
        int EnableDomainDns(int domainId);
        [WebMethod]
        [OperationContract]
        int DisableDomainDns(int domainId);
        [WebMethod]
        [OperationContract]
        int CreateDomainPreviewDomain(string hostName, int domainId);
        [WebMethod]
        [OperationContract]
        int DeleteDomainPreviewDomain(int domainId);
        [WebMethod]
        [OperationContract]
        DnsRecord[] GetDnsZoneRecords(int domainId);
        [WebMethod]
        [OperationContract]
        DataSet GetRawDnsZoneRecords(int domainId);
        [WebMethod]
        [OperationContract]
        int AddDnsZoneRecord(int domainId, string recordName, DnsRecordType recordType, string recordData, int mxPriority, int srvPriority, int srvWeight, int srvPortNumber);
        [WebMethod]
        [OperationContract]
        int UpdateDnsZoneRecord(int domainId, string originalRecordName, string originalRecordData, string recordName, DnsRecordType recordType, string recordData, int mxPriority, int srvPriority, int srvWeight, int srvPortNumber);
        [WebMethod]
        [OperationContract]
        int DeleteDnsZoneRecord(int domainId, string recordName, DnsRecordType recordType, string recordData);
        [WebMethod]
        [OperationContract]
        TerminalSession[] GetTerminalServicesSessions(int serverId);
        [WebMethod]
        [OperationContract]
        int CloseTerminalServicesSession(int serverId, int sessionId);
        [WebMethod]
        [OperationContract]
        OSProcess[] GetOSProcesses(int serverId);
        [WebMethod]
        [OperationContract]
        int TerminateOSProcess(int serverId, int pid);
        [WebMethod]
        [OperationContract]
        bool CheckLoadUserProfile(int serverId);
        [WebMethod]
        [OperationContract]
        void EnableLoadUserProfile(int serverId);
        [WebMethod]
        [OperationContract]
        void InitWPIFeeds(int serverId);
        [WebMethod]
        [OperationContract]
        WPITab[] GetWPITabs(int serverId);
        [WebMethod]
        [OperationContract]
        WPIKeyword[] GetWPIKeywords(int serverId);
        [WebMethod]
        [OperationContract]
        WPIProduct[] GetWPIProducts(int serverId, string tabId, string keywordId);
        [WebMethod]
        [OperationContract]
        WPIProduct[] GetWPIProductsFiltered(int serverId, string keywordId);
        [WebMethod]
        [OperationContract]
        WPIProduct GetWPIProductById(int serverId, string productdId);
        [WebMethod]
        [OperationContract]
        WPIProduct[] GetWPIProductsWithDependencies(int serverId, string[] products);
        [WebMethod]
        [OperationContract]
        void InstallWPIProducts(int serverId, string[] products);
        [WebMethod]
        [OperationContract]
        void CancelInstallWPIProducts(int serviceId);
        [WebMethod]
        [OperationContract]
        string GetWPIStatus(int serverId);
        [WebMethod]
        [OperationContract]
        string WpiGetLogFileDirectory(int serverId);
        [WebMethod]
        [OperationContract]
        SettingPair[] WpiGetLogsInDirectory(int serverId, string Path);
        [WebMethod]
        [OperationContract]
        OSService[] GetOSServices(int serverId);
        [WebMethod]
        [OperationContract]
        int ChangeOSServiceStatus(int serverId, string id, OSServiceStatus status);
        [WebMethod]
        [OperationContract]
        string[] GetLogNames(int serverId);
        [WebMethod]
        [OperationContract]
        SystemLogEntry[] GetLogEntries(int serverId, string logName);
        [WebMethod]
        [OperationContract]
        SystemLogEntriesPaged GetLogEntriesPaged(int serverId, string logName, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        int ClearLog(int serverId, string logName);
        [WebMethod]
        [OperationContract]
        int RebootSystem(int serverId);
        [WebMethod]
        [OperationContract]
        Memory GetMemoryPackageId(int packageId);
        [WebMethod]
        [OperationContract]
        Memory GetMemory(int serverId);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esServers : SolidCP.EnterpriseServer.esServers, IesServers
    {
    }
}
#endif