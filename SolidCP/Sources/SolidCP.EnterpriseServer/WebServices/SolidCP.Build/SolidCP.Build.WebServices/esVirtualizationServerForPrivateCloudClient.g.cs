#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesVirtualizationServerForPrivateCloud", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesVirtualizationServerForPrivateCloud
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/CheckServerState", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/CheckServerStateResponse")]
        bool CheckServerState(SolidCP.Providers.Virtualization.VMForPCSettingsName control, string connString, string connName, int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/CheckServerState", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/CheckServerStateResponse")]
        System.Threading.Tasks.Task<bool> CheckServerStateAsync(SolidCP.Providers.Virtualization.VMForPCSettingsName control, string connString, string connName, int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachines", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachinesResponse")]
        SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged GetVirtualMachines(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachines", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachinesResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged> GetVirtualMachinesAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachinesByServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachinesByServiceIdResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine[] GetVirtualMachinesByServiceId(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachinesByServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachinesByServiceIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine[]> GetVirtualMachinesByServiceIdAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineItem", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineItemResponse")]
        SolidCP.Providers.Virtualization.VMInfo GetVirtualMachineItem(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineItem", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineItemResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMInfo> GetVirtualMachineItemAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/EvaluateVirtualMachineTemplate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/EvaluateVirtualMachineTemplateResponse")]
        string EvaluateVirtualMachineTemplate(int itemId, string template);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/EvaluateVirtualMachineTemplate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/EvaluateVirtualMachineTemplateResponse")]
        System.Threading.Tasks.Task<string> EvaluateVirtualMachineTemplateAsync(int itemId, string template);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetExternalNetworkDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetExternalNetworkDetailsResponse")]
        SolidCP.EnterpriseServer.NetworkAdapterDetails GetExternalNetworkDetails(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetExternalNetworkDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetExternalNetworkDetailsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetExternalNetworkDetailsAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetPackagePrivateIPAddressesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetPackagePrivateIPAddressesPagedResponse")]
        SolidCP.EnterpriseServer.PrivateIPAddressesPaged GetPackagePrivateIPAddressesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetPackagePrivateIPAddressesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetPackagePrivateIPAddressesPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PrivateIPAddressesPaged> GetPackagePrivateIPAddressesPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetPackagePrivateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetPackagePrivateIPAddressesResponse")]
        SolidCP.EnterpriseServer.PrivateIPAddress[] /*List*/ GetPackagePrivateIPAddresses(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetPackagePrivateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetPackagePrivateIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PrivateIPAddress[]> GetPackagePrivateIPAddressesAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetPrivateNetworkDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetPrivateNetworkDetailsResponse")]
        SolidCP.EnterpriseServer.NetworkAdapterDetails GetPrivateNetworkDetails(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetPrivateNetworkDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetPrivateNetworkDetailsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetPrivateNetworkDetailsAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetSpaceUserPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetSpaceUserPermissionsResponse")]
        SolidCP.EnterpriseServer.VirtualMachinePermission[] /*List*/ GetSpaceUserPermissions(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetSpaceUserPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetSpaceUserPermissionsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachinePermission[]> GetSpaceUserPermissionsAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/UpdateSpaceUserPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/UpdateSpaceUserPermissionsResponse")]
        int UpdateSpaceUserPermissions(int packageId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/UpdateSpaceUserPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/UpdateSpaceUserPermissionsResponse")]
        System.Threading.Tasks.Task<int> UpdateSpaceUserPermissionsAsync(int packageId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetSpaceAuditLog", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetSpaceAuditLogResponse")]
        SolidCP.EnterpriseServer.LogRecord[] /*List*/ GetSpaceAuditLog(int packageId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetSpaceAuditLog", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetSpaceAuditLogResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.LogRecord[]> GetSpaceAuditLogAsync(int packageId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineAuditLog", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineAuditLogResponse")]
        SolidCP.EnterpriseServer.LogRecord[] /*List*/ GetVirtualMachineAuditLog(int itemId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineAuditLog", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineAuditLogResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.LogRecord[]> GetVirtualMachineAuditLogAsync(int itemId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetOperatingSystemTemplatesPC", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetOperatingSystemTemplatesPCResponse")]
        SolidCP.Providers.Virtualization.LibraryItem[] GetOperatingSystemTemplatesPC(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetOperatingSystemTemplatesPC", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetOperatingSystemTemplatesPCResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetOperatingSystemTemplatesPCAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetHosts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetHostsResponse")]
        SolidCP.Providers.Virtualization.LibraryItem[] GetHosts(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetHosts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetHostsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetHostsAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetClusters", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetClustersResponse")]
        SolidCP.Providers.Virtualization.LibraryItem[] GetClusters(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetClusters", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetClustersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetClustersAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetOperatingSystemTemplatesByServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetOperatingSystemTemplatesByServiceIdResponse")]
        SolidCP.Providers.Virtualization.LibraryItem[] GetOperatingSystemTemplatesByServiceId(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetOperatingSystemTemplatesByServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetOperatingSystemTemplatesByServiceIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetOperatingSystemTemplatesByServiceIdAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetMaximumCpuCoresNumber", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetMaximumCpuCoresNumberResponse")]
        int GetMaximumCpuCoresNumber(int packageId, string templateId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetMaximumCpuCoresNumber", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetMaximumCpuCoresNumberResponse")]
        System.Threading.Tasks.Task<int> GetMaximumCpuCoresNumberAsync(int packageId, string templateId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetDefaultExportPath", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetDefaultExportPathResponse")]
        string GetDefaultExportPath(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetDefaultExportPath", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetDefaultExportPathResponse")]
        System.Threading.Tasks.Task<string> GetDefaultExportPathAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/CreateVMFromVM", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/CreateVMFromVMResponse")]
        SolidCP.Providers.Common.ResultObject CreateVMFromVM(int packageId, SolidCP.Providers.Virtualization.VMInfo vmTemplate, string vmName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/CreateVMFromVM", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/CreateVMFromVMResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateVMFromVMAsync(int packageId, SolidCP.Providers.Virtualization.VMInfo vmTemplate, string vmName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/CreateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/CreateVirtualMachineResponse")]
        SolidCP.Providers.ResultObjects.IntResult CreateVirtualMachine(int packageId, string hostname, string domain, string osTemplateFile, string vmName, string password, string summaryLetterEmail, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, string externalNetworkLocation, string externalNicMacAddress, string externalVirtualNetwork, bool privateNetworkEnabled, string privateNetworkLocation, string privateNicMacAddress, string privateVirtualNetwork, ushort privateVLanID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/CreateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/CreateVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> CreateVirtualMachineAsync(int packageId, string hostname, string domain, string osTemplateFile, string vmName, string password, string summaryLetterEmail, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, string externalNetworkLocation, string externalNicMacAddress, string externalVirtualNetwork, bool privateNetworkEnabled, string privateNetworkLocation, string privateNicMacAddress, string privateVirtualNetwork, ushort privateVLanID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/ImportVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/ImportVirtualMachineResponse")]
        SolidCP.Providers.ResultObjects.IntResult ImportVirtualMachine(int packageId, int serviceId, string vmId, string osTemplateFile, string adminPassword, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, string externalNicMacAddress, int[] externalAddresses, string managementNicMacAddress, int managementAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/ImportVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/ImportVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> ImportVirtualMachineAsync(int packageId, int serviceId, string vmId, string osTemplateFile, string adminPassword, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, string externalNicMacAddress, int[] externalAddresses, string managementNicMacAddress, int managementAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineThumbnail", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineThumbnailResponse")]
        byte[] GetVirtualMachineThumbnail(int itemId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineThumbnail", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineThumbnailResponse")]
        System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailAsync(int itemId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineGeneralDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineGeneralDetailsResponse")]
        SolidCP.Providers.Virtualization.VMInfo GetVirtualMachineGeneralDetails(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineGeneralDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineGeneralDetailsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMInfo> GetVirtualMachineGeneralDetailsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineExtendedInfo", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineExtendedInfoResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineExtendedInfo(int serviceId, string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineExtendedInfo", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineExtendedInfoResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineExtendedInfoAsync(int serviceId, string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/CancelVirtualMachineJob", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/CancelVirtualMachineJobResponse")]
        int CancelVirtualMachineJob(string jobId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/CancelVirtualMachineJob", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/CancelVirtualMachineJobResponse")]
        System.Threading.Tasks.Task<int> CancelVirtualMachineJobAsync(string jobId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/UpdateVirtualMachineHostName", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/UpdateVirtualMachineHostNameResponse")]
        SolidCP.Providers.Common.ResultObject UpdateVirtualMachineHostName(int itemId, string hostname, bool updateNetBIOS);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/UpdateVirtualMachineHostName", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/UpdateVirtualMachineHostNameResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateVirtualMachineHostNameAsync(int itemId, string hostname, bool updateNetBIOS);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/ChangeVirtualMachineState", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/ChangeVirtualMachineStateResponse")]
        SolidCP.Providers.Common.ResultObject ChangeVirtualMachineState(int itemId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState state);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/ChangeVirtualMachineState", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/ChangeVirtualMachineStateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeVirtualMachineStateAsync(int itemId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState state);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineJobs", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineJobsResponse")]
        SolidCP.Providers.Virtualization.ConcreteJob[] /*List*/ GetVirtualMachineJobs(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineJobs", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineJobsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob[]> GetVirtualMachineJobsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/ChangeAdministratorPassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/ChangeAdministratorPasswordResponse")]
        SolidCP.Providers.Common.ResultObject ChangeAdministratorPassword(int itemId, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/ChangeAdministratorPassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/ChangeAdministratorPasswordResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeAdministratorPasswordAsync(int itemId, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/UpdateVirtualMachineConfiguration", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/UpdateVirtualMachineConfigurationResponse")]
        SolidCP.Providers.Common.ResultObject UpdateVirtualMachineConfiguration(int itemId, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, bool privateNetworkEnabled);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/UpdateVirtualMachineConfiguration", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/UpdateVirtualMachineConfigurationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateVirtualMachineConfigurationAsync(int itemId, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, bool privateNetworkEnabled);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetInsertedDvdDisk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetInsertedDvdDiskResponse")]
        SolidCP.Providers.Virtualization.LibraryItem GetInsertedDvdDisk(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetInsertedDvdDisk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetInsertedDvdDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem> GetInsertedDvdDiskAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetLibraryDisks", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetLibraryDisksResponse")]
        SolidCP.Providers.Virtualization.LibraryItem[] GetLibraryDisks(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetLibraryDisks", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetLibraryDisksResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetLibraryDisksAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/InsertDvdDisk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/InsertDvdDiskResponse")]
        SolidCP.Providers.Common.ResultObject InsertDvdDisk(int itemId, string isoPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/InsertDvdDisk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/InsertDvdDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> InsertDvdDiskAsync(int itemId, string isoPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/EjectDvdDisk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/EjectDvdDiskResponse")]
        SolidCP.Providers.Common.ResultObject EjectDvdDisk(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/EjectDvdDisk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/EjectDvdDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> EjectDvdDiskAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineSnapshots", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineSnapshotsResponse")]
        SolidCP.Providers.Virtualization.VirtualMachineSnapshot[] GetVirtualMachineSnapshots(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineSnapshots", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineSnapshotsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]> GetVirtualMachineSnapshotsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetSnapshotResponse")]
        SolidCP.Providers.Virtualization.VirtualMachineSnapshot GetSnapshot(int itemId, string snaphostId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot> GetSnapshotAsync(int itemId, string snaphostId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/CreateSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/CreateSnapshotResponse")]
        SolidCP.Providers.Common.ResultObject CreateSnapshot(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/CreateSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/CreateSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateSnapshotAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/ApplySnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/ApplySnapshotResponse")]
        SolidCP.Providers.Common.ResultObject ApplySnapshot(int itemId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/ApplySnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/ApplySnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ApplySnapshotAsync(int itemId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/RenameSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/RenameSnapshotResponse")]
        SolidCP.Providers.Common.ResultObject RenameSnapshot(int itemId, string snapshotId, string newName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/RenameSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/RenameSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RenameSnapshotAsync(int itemId, string snapshotId, string newName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/DeleteSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/DeleteSnapshotResponse")]
        SolidCP.Providers.Common.ResultObject DeleteSnapshot(int itemId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/DeleteSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/DeleteSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteSnapshotAsync(int itemId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/DeleteSnapshotSubtree", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/DeleteSnapshotSubtreeResponse")]
        SolidCP.Providers.Common.ResultObject DeleteSnapshotSubtree(int itemId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/DeleteSnapshotSubtree", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/DeleteSnapshotSubtreeResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteSnapshotSubtreeAsync(int itemId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetSnapshotThumbnail", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetSnapshotThumbnailResponse")]
        byte[] GetSnapshotThumbnail(int itemId, string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetSnapshotThumbnail", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetSnapshotThumbnailResponse")]
        System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailAsync(int itemId, string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/ConfigureCreatedVMNetworkAdapters", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/ConfigureCreatedVMNetworkAdaptersResponse")]
        void ConfigureCreatedVMNetworkAdapters(SolidCP.Providers.Virtualization.VMInfo vmInfo);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/ConfigureCreatedVMNetworkAdapters", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/ConfigureCreatedVMNetworkAdaptersResponse")]
        System.Threading.Tasks.Task ConfigureCreatedVMNetworkAdaptersAsync(SolidCP.Providers.Virtualization.VMInfo vmInfo);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetExternalNetworkAdapterDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetExternalNetworkAdapterDetailsResponse")]
        SolidCP.EnterpriseServer.NetworkAdapterDetails GetExternalNetworkAdapterDetails(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetExternalNetworkAdapterDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetExternalNetworkAdapterDetailsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetExternalNetworkAdapterDetailsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/AddVirtualMachineExternalIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/AddVirtualMachineExternalIPAddressesResponse")]
        SolidCP.Providers.Common.ResultObject AddVirtualMachineExternalIPAddresses(int itemId, bool selectRandom, int addressesNumber, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/AddVirtualMachineExternalIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/AddVirtualMachineExternalIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachineExternalIPAddressesAsync(int itemId, bool selectRandom, int addressesNumber, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/SetVirtualMachinePrimaryExternalIPAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/SetVirtualMachinePrimaryExternalIPAddressResponse")]
        SolidCP.Providers.Common.ResultObject SetVirtualMachinePrimaryExternalIPAddress(int itemId, int addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/SetVirtualMachinePrimaryExternalIPAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/SetVirtualMachinePrimaryExternalIPAddressResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetVirtualMachinePrimaryExternalIPAddressAsync(int itemId, int addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/DeleteVirtualMachineExternalIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/DeleteVirtualMachineExternalIPAddressesResponse")]
        SolidCP.Providers.Common.ResultObject DeleteVirtualMachineExternalIPAddresses(int itemId, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/DeleteVirtualMachineExternalIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/DeleteVirtualMachineExternalIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineExternalIPAddressesAsync(int itemId, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetPrivateNetworkAdapterDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetPrivateNetworkAdapterDetailsResponse")]
        SolidCP.EnterpriseServer.NetworkAdapterDetails GetPrivateNetworkAdapterDetails(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetPrivateNetworkAdapterDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetPrivateNetworkAdapterDetailsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetPrivateNetworkAdapterDetailsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/AddVirtualMachinePrivateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/AddVirtualMachinePrivateIPAddressesResponse")]
        SolidCP.Providers.Common.ResultObject AddVirtualMachinePrivateIPAddresses(int itemId, bool selectRandom, int addressesNumber, string[] addresses);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/AddVirtualMachinePrivateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/AddVirtualMachinePrivateIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachinePrivateIPAddressesAsync(int itemId, bool selectRandom, int addressesNumber, string[] addresses);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/SetVirtualMachinePrimaryPrivateIPAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/SetVirtualMachinePrimaryPrivateIPAddressResponse")]
        SolidCP.Providers.Common.ResultObject SetVirtualMachinePrimaryPrivateIPAddress(int itemId, int addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/SetVirtualMachinePrimaryPrivateIPAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/SetVirtualMachinePrimaryPrivateIPAddressResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetVirtualMachinePrimaryPrivateIPAddressAsync(int itemId, int addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/DeleteVirtualMachinePrivateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/DeleteVirtualMachinePrivateIPAddressesResponse")]
        SolidCP.Providers.Common.ResultObject DeleteVirtualMachinePrivateIPAddresses(int itemId, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/DeleteVirtualMachinePrivateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/DeleteVirtualMachinePrivateIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachinePrivateIPAddressesAsync(int itemId, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachinePermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachinePermissionsResponse")]
        SolidCP.EnterpriseServer.VirtualMachinePermission[] /*List*/ GetVirtualMachinePermissions(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachinePermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachinePermissionsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachinePermission[]> GetVirtualMachinePermissionsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/UpdateVirtualMachineUserPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/UpdateVirtualMachineUserPermissionsResponse")]
        int UpdateVirtualMachineUserPermissions(int itemId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/UpdateVirtualMachineUserPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/UpdateVirtualMachineUserPermissionsResponse")]
        System.Threading.Tasks.Task<int> UpdateVirtualMachineUserPermissionsAsync(int itemId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetExternalSwitches", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetExternalSwitchesResponse")]
        SolidCP.Providers.Virtualization.VirtualSwitch[] GetExternalSwitches(int serviceId, string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetExternalSwitches", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetExternalSwitchesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetExternalSwitchesAsync(int serviceId, string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/DeleteVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/DeleteVirtualMachineResponse")]
        SolidCP.Providers.Common.ResultObject DeleteVirtualMachine(int itemId, bool saveFiles, bool exportVps, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/DeleteVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/DeleteVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineAsync(int itemId, bool saveFiles, bool exportVps, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/ReinstallVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/ReinstallVirtualMachineResponse")]
        int ReinstallVirtualMachine(int itemId, string adminPassword, bool preserveVirtualDiskFiles, bool saveVirtualDisk, bool exportVps, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/ReinstallVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/ReinstallVirtualMachineResponse")]
        System.Threading.Tasks.Task<int> ReinstallVirtualMachineAsync(int itemId, string adminPassword, bool preserveVirtualDiskFiles, bool saveVirtualDisk, bool exportVps, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineSummaryText", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineSummaryTextResponse")]
        string GetVirtualMachineSummaryText(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineSummaryText", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualMachineSummaryTextResponse")]
        System.Threading.Tasks.Task<string> GetVirtualMachineSummaryTextAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/SendVirtualMachineSummaryLetter", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/SendVirtualMachineSummaryLetterResponse")]
        SolidCP.Providers.Common.ResultObject SendVirtualMachineSummaryLetter(int itemId, string to, string bcc);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/SendVirtualMachineSummaryLetter", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/SendVirtualMachineSummaryLetterResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SendVirtualMachineSummaryLetterAsync(int itemId, string to, string bcc);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetDeviceEvents", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetDeviceEventsResponse")]
        SolidCP.Providers.Virtualization.MonitoredObjectEvent[] GetDeviceEvents(int ItemID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetDeviceEvents", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetDeviceEventsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.MonitoredObjectEvent[]> GetDeviceEventsAsync(int ItemID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetMonitoringAlerts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetMonitoringAlertsResponse")]
        SolidCP.Providers.Virtualization.MonitoredObjectAlert[] GetMonitoringAlerts(int ItemID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetMonitoringAlerts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetMonitoringAlertsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.MonitoredObjectAlert[]> GetMonitoringAlertsAsync(int ItemID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetPerfomanceValue", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetPerfomanceValueResponse")]
        SolidCP.Providers.Virtualization.PerformanceDataValue[] GetPerfomanceValue(int ItemID, SolidCP.Providers.Virtualization.PerformanceType perf, System.DateTime startPeriod, System.DateTime endPeriod);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetPerfomanceValue", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetPerfomanceValueResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.PerformanceDataValue[]> GetPerfomanceValueAsync(int ItemID, SolidCP.Providers.Virtualization.PerformanceType perf, System.DateTime startPeriod, System.DateTime endPeriod);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualNetwork", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualNetworkResponse")]
        SolidCP.Providers.Virtualization.VirtualNetworkInfo[] GetVirtualNetwork(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualNetwork", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerForPrivateCloud/GetVirtualNetworkResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualNetworkInfo[]> GetVirtualNetworkAsync(int packageId);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esVirtualizationServerForPrivateCloudAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesVirtualizationServerForPrivateCloud
    {
        public bool CheckServerState(SolidCP.Providers.Virtualization.VMForPCSettingsName control, string connString, string connName, int serviceId)
        {
            return Invoke<bool>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "CheckServerState", control, connString, connName, serviceId);
        }

        public async System.Threading.Tasks.Task<bool> CheckServerStateAsync(SolidCP.Providers.Virtualization.VMForPCSettingsName control, string connString, string connName, int serviceId)
        {
            return await InvokeAsync<bool>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "CheckServerState", control, connString, connName, serviceId);
        }

        public SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged GetVirtualMachines(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            return Invoke<SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetVirtualMachines", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged> GetVirtualMachinesAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetVirtualMachines", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows, recursive);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine[] GetVirtualMachinesByServiceId(int serviceId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetVirtualMachinesByServiceId", serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine[]> GetVirtualMachinesByServiceIdAsync(int serviceId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetVirtualMachinesByServiceId", serviceId);
        }

        public SolidCP.Providers.Virtualization.VMInfo GetVirtualMachineItem(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VMInfo>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetVirtualMachineItem", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMInfo> GetVirtualMachineItemAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VMInfo>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetVirtualMachineItem", itemId);
        }

        public string EvaluateVirtualMachineTemplate(int itemId, string template)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "EvaluateVirtualMachineTemplate", itemId, template);
        }

        public async System.Threading.Tasks.Task<string> EvaluateVirtualMachineTemplateAsync(int itemId, string template)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "EvaluateVirtualMachineTemplate", itemId, template);
        }

        public SolidCP.EnterpriseServer.NetworkAdapterDetails GetExternalNetworkDetails(int packageId)
        {
            return Invoke<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetExternalNetworkDetails", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetExternalNetworkDetailsAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetExternalNetworkDetails", packageId);
        }

        public SolidCP.EnterpriseServer.PrivateIPAddressesPaged GetPackagePrivateIPAddressesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.EnterpriseServer.PrivateIPAddressesPaged>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetPackagePrivateIPAddressesPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PrivateIPAddressesPaged> GetPackagePrivateIPAddressesPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PrivateIPAddressesPaged>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetPackagePrivateIPAddressesPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.EnterpriseServer.PrivateIPAddress[] /*List*/ GetPackagePrivateIPAddresses(int packageId)
        {
            return Invoke<SolidCP.EnterpriseServer.PrivateIPAddress[], SolidCP.EnterpriseServer.PrivateIPAddress>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetPackagePrivateIPAddresses", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PrivateIPAddress[]> GetPackagePrivateIPAddressesAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PrivateIPAddress[], SolidCP.EnterpriseServer.PrivateIPAddress>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetPackagePrivateIPAddresses", packageId);
        }

        public SolidCP.EnterpriseServer.NetworkAdapterDetails GetPrivateNetworkDetails(int packageId)
        {
            return Invoke<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetPrivateNetworkDetails", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetPrivateNetworkDetailsAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetPrivateNetworkDetails", packageId);
        }

        public SolidCP.EnterpriseServer.VirtualMachinePermission[] /*List*/ GetSpaceUserPermissions(int packageId)
        {
            return Invoke<SolidCP.EnterpriseServer.VirtualMachinePermission[], SolidCP.EnterpriseServer.VirtualMachinePermission>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetSpaceUserPermissions", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachinePermission[]> GetSpaceUserPermissionsAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.VirtualMachinePermission[], SolidCP.EnterpriseServer.VirtualMachinePermission>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetSpaceUserPermissions", packageId);
        }

        public int UpdateSpaceUserPermissions(int packageId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "UpdateSpaceUserPermissions", packageId, permissions);
        }

        public async System.Threading.Tasks.Task<int> UpdateSpaceUserPermissionsAsync(int packageId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "UpdateSpaceUserPermissions", packageId, permissions);
        }

        public SolidCP.EnterpriseServer.LogRecord[] /*List*/ GetSpaceAuditLog(int packageId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.EnterpriseServer.LogRecord[], SolidCP.EnterpriseServer.LogRecord>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetSpaceAuditLog", packageId, startPeriod, endPeriod, severity, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.LogRecord[]> GetSpaceAuditLogAsync(int packageId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.LogRecord[], SolidCP.EnterpriseServer.LogRecord>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetSpaceAuditLog", packageId, startPeriod, endPeriod, severity, sortColumn, startRow, maximumRows);
        }

        public SolidCP.EnterpriseServer.LogRecord[] /*List*/ GetVirtualMachineAuditLog(int itemId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.EnterpriseServer.LogRecord[], SolidCP.EnterpriseServer.LogRecord>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetVirtualMachineAuditLog", itemId, startPeriod, endPeriod, severity, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.LogRecord[]> GetVirtualMachineAuditLogAsync(int itemId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.LogRecord[], SolidCP.EnterpriseServer.LogRecord>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetVirtualMachineAuditLog", itemId, startPeriod, endPeriod, severity, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetOperatingSystemTemplatesPC(int packageId)
        {
            return Invoke<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetOperatingSystemTemplatesPC", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetOperatingSystemTemplatesPCAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetOperatingSystemTemplatesPC", packageId);
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetHosts(int serviceId)
        {
            return Invoke<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetHosts", serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetHostsAsync(int serviceId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetHosts", serviceId);
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetClusters(int serviceId)
        {
            return Invoke<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetClusters", serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetClustersAsync(int serviceId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetClusters", serviceId);
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetOperatingSystemTemplatesByServiceId(int serviceId)
        {
            return Invoke<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetOperatingSystemTemplatesByServiceId", serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetOperatingSystemTemplatesByServiceIdAsync(int serviceId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetOperatingSystemTemplatesByServiceId", serviceId);
        }

        public int GetMaximumCpuCoresNumber(int packageId, string templateId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetMaximumCpuCoresNumber", packageId, templateId);
        }

        public async System.Threading.Tasks.Task<int> GetMaximumCpuCoresNumberAsync(int packageId, string templateId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetMaximumCpuCoresNumber", packageId, templateId);
        }

        public string GetDefaultExportPath(int itemId)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetDefaultExportPath", itemId);
        }

        public async System.Threading.Tasks.Task<string> GetDefaultExportPathAsync(int itemId)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetDefaultExportPath", itemId);
        }

        public SolidCP.Providers.Common.ResultObject CreateVMFromVM(int packageId, SolidCP.Providers.Virtualization.VMInfo vmTemplate, string vmName)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "CreateVMFromVM", packageId, vmTemplate, vmName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateVMFromVMAsync(int packageId, SolidCP.Providers.Virtualization.VMInfo vmTemplate, string vmName)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "CreateVMFromVM", packageId, vmTemplate, vmName);
        }

        public SolidCP.Providers.ResultObjects.IntResult CreateVirtualMachine(int packageId, string hostname, string domain, string osTemplateFile, string vmName, string password, string summaryLetterEmail, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, string externalNetworkLocation, string externalNicMacAddress, string externalVirtualNetwork, bool privateNetworkEnabled, string privateNetworkLocation, string privateNicMacAddress, string privateVirtualNetwork, ushort privateVLanID)
        {
            return Invoke<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "CreateVirtualMachine", packageId, hostname, domain, osTemplateFile, vmName, password, summaryLetterEmail, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, externalNetworkLocation, externalNicMacAddress, externalVirtualNetwork, privateNetworkEnabled, privateNetworkLocation, privateNicMacAddress, privateVirtualNetwork, privateVLanID);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> CreateVirtualMachineAsync(int packageId, string hostname, string domain, string osTemplateFile, string vmName, string password, string summaryLetterEmail, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, string externalNetworkLocation, string externalNicMacAddress, string externalVirtualNetwork, bool privateNetworkEnabled, string privateNetworkLocation, string privateNicMacAddress, string privateVirtualNetwork, ushort privateVLanID)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "CreateVirtualMachine", packageId, hostname, domain, osTemplateFile, vmName, password, summaryLetterEmail, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, externalNetworkLocation, externalNicMacAddress, externalVirtualNetwork, privateNetworkEnabled, privateNetworkLocation, privateNicMacAddress, privateVirtualNetwork, privateVLanID);
        }

        public SolidCP.Providers.ResultObjects.IntResult ImportVirtualMachine(int packageId, int serviceId, string vmId, string osTemplateFile, string adminPassword, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, string externalNicMacAddress, int[] externalAddresses, string managementNicMacAddress, int managementAddress)
        {
            return Invoke<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "ImportVirtualMachine", packageId, serviceId, vmId, osTemplateFile, adminPassword, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNicMacAddress, externalAddresses, managementNicMacAddress, managementAddress);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> ImportVirtualMachineAsync(int packageId, int serviceId, string vmId, string osTemplateFile, string adminPassword, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, string externalNicMacAddress, int[] externalAddresses, string managementNicMacAddress, int managementAddress)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "ImportVirtualMachine", packageId, serviceId, vmId, osTemplateFile, adminPassword, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNicMacAddress, externalAddresses, managementNicMacAddress, managementAddress);
        }

        public byte[] GetVirtualMachineThumbnail(int itemId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return Invoke<byte[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetVirtualMachineThumbnail", itemId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailAsync(int itemId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return await InvokeAsync<byte[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetVirtualMachineThumbnail", itemId, size);
        }

        public SolidCP.Providers.Virtualization.VMInfo GetVirtualMachineGeneralDetails(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VMInfo>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetVirtualMachineGeneralDetails", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMInfo> GetVirtualMachineGeneralDetailsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VMInfo>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetVirtualMachineGeneralDetails", itemId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineExtendedInfo(int serviceId, string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetVirtualMachineExtendedInfo", serviceId, vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineExtendedInfoAsync(int serviceId, string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetVirtualMachineExtendedInfo", serviceId, vmId);
        }

        public int CancelVirtualMachineJob(string jobId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "CancelVirtualMachineJob", jobId);
        }

        public async System.Threading.Tasks.Task<int> CancelVirtualMachineJobAsync(string jobId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "CancelVirtualMachineJob", jobId);
        }

        public SolidCP.Providers.Common.ResultObject UpdateVirtualMachineHostName(int itemId, string hostname, bool updateNetBIOS)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "UpdateVirtualMachineHostName", itemId, hostname, updateNetBIOS);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateVirtualMachineHostNameAsync(int itemId, string hostname, bool updateNetBIOS)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "UpdateVirtualMachineHostName", itemId, hostname, updateNetBIOS);
        }

        public SolidCP.Providers.Common.ResultObject ChangeVirtualMachineState(int itemId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState state)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "ChangeVirtualMachineState", itemId, state);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeVirtualMachineStateAsync(int itemId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState state)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "ChangeVirtualMachineState", itemId, state);
        }

        public SolidCP.Providers.Virtualization.ConcreteJob[] /*List*/ GetVirtualMachineJobs(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.ConcreteJob[], SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetVirtualMachineJobs", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob[]> GetVirtualMachineJobsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ConcreteJob[], SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetVirtualMachineJobs", itemId);
        }

        public SolidCP.Providers.Common.ResultObject ChangeAdministratorPassword(int itemId, string password)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "ChangeAdministratorPassword", itemId, password);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeAdministratorPasswordAsync(int itemId, string password)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "ChangeAdministratorPassword", itemId, password);
        }

        public SolidCP.Providers.Common.ResultObject UpdateVirtualMachineConfiguration(int itemId, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, bool privateNetworkEnabled)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "UpdateVirtualMachineConfiguration", itemId, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, privateNetworkEnabled);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateVirtualMachineConfigurationAsync(int itemId, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, bool privateNetworkEnabled)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "UpdateVirtualMachineConfiguration", itemId, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, privateNetworkEnabled);
        }

        public SolidCP.Providers.Virtualization.LibraryItem GetInsertedDvdDisk(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.LibraryItem>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetInsertedDvdDisk", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem> GetInsertedDvdDiskAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.LibraryItem>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetInsertedDvdDisk", itemId);
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetLibraryDisks(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetLibraryDisks", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetLibraryDisksAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetLibraryDisks", itemId);
        }

        public SolidCP.Providers.Common.ResultObject InsertDvdDisk(int itemId, string isoPath)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "InsertDvdDisk", itemId, isoPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> InsertDvdDiskAsync(int itemId, string isoPath)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "InsertDvdDisk", itemId, isoPath);
        }

        public SolidCP.Providers.Common.ResultObject EjectDvdDisk(int itemId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "EjectDvdDisk", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> EjectDvdDiskAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "EjectDvdDisk", itemId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachineSnapshot[] GetVirtualMachineSnapshots(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetVirtualMachineSnapshots", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]> GetVirtualMachineSnapshotsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetVirtualMachineSnapshots", itemId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachineSnapshot GetSnapshot(int itemId, string snaphostId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachineSnapshot>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetSnapshot", itemId, snaphostId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot> GetSnapshotAsync(int itemId, string snaphostId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachineSnapshot>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetSnapshot", itemId, snaphostId);
        }

        public SolidCP.Providers.Common.ResultObject CreateSnapshot(int itemId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "CreateSnapshot", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateSnapshotAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "CreateSnapshot", itemId);
        }

        public SolidCP.Providers.Common.ResultObject ApplySnapshot(int itemId, string snapshotId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "ApplySnapshot", itemId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ApplySnapshotAsync(int itemId, string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "ApplySnapshot", itemId, snapshotId);
        }

        public SolidCP.Providers.Common.ResultObject RenameSnapshot(int itemId, string snapshotId, string newName)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "RenameSnapshot", itemId, snapshotId, newName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RenameSnapshotAsync(int itemId, string snapshotId, string newName)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "RenameSnapshot", itemId, snapshotId, newName);
        }

        public SolidCP.Providers.Common.ResultObject DeleteSnapshot(int itemId, string snapshotId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "DeleteSnapshot", itemId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteSnapshotAsync(int itemId, string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "DeleteSnapshot", itemId, snapshotId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteSnapshotSubtree(int itemId, string snapshotId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "DeleteSnapshotSubtree", itemId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteSnapshotSubtreeAsync(int itemId, string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "DeleteSnapshotSubtree", itemId, snapshotId);
        }

        public byte[] GetSnapshotThumbnail(int itemId, string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return Invoke<byte[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetSnapshotThumbnail", itemId, snapshotId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailAsync(int itemId, string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return await InvokeAsync<byte[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetSnapshotThumbnail", itemId, snapshotId, size);
        }

        public void ConfigureCreatedVMNetworkAdapters(SolidCP.Providers.Virtualization.VMInfo vmInfo)
        {
            Invoke("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "ConfigureCreatedVMNetworkAdapters", vmInfo);
        }

        public async System.Threading.Tasks.Task ConfigureCreatedVMNetworkAdaptersAsync(SolidCP.Providers.Virtualization.VMInfo vmInfo)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "ConfigureCreatedVMNetworkAdapters", vmInfo);
        }

        public SolidCP.EnterpriseServer.NetworkAdapterDetails GetExternalNetworkAdapterDetails(int itemId)
        {
            return Invoke<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetExternalNetworkAdapterDetails", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetExternalNetworkAdapterDetailsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetExternalNetworkAdapterDetails", itemId);
        }

        public SolidCP.Providers.Common.ResultObject AddVirtualMachineExternalIPAddresses(int itemId, bool selectRandom, int addressesNumber, int[] addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "AddVirtualMachineExternalIPAddresses", itemId, selectRandom, addressesNumber, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachineExternalIPAddressesAsync(int itemId, bool selectRandom, int addressesNumber, int[] addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "AddVirtualMachineExternalIPAddresses", itemId, selectRandom, addressesNumber, addressId);
        }

        public SolidCP.Providers.Common.ResultObject SetVirtualMachinePrimaryExternalIPAddress(int itemId, int addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "SetVirtualMachinePrimaryExternalIPAddress", itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetVirtualMachinePrimaryExternalIPAddressAsync(int itemId, int addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "SetVirtualMachinePrimaryExternalIPAddress", itemId, addressId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteVirtualMachineExternalIPAddresses(int itemId, int[] addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "DeleteVirtualMachineExternalIPAddresses", itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineExternalIPAddressesAsync(int itemId, int[] addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "DeleteVirtualMachineExternalIPAddresses", itemId, addressId);
        }

        public SolidCP.EnterpriseServer.NetworkAdapterDetails GetPrivateNetworkAdapterDetails(int itemId)
        {
            return Invoke<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetPrivateNetworkAdapterDetails", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetPrivateNetworkAdapterDetailsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetPrivateNetworkAdapterDetails", itemId);
        }

        public SolidCP.Providers.Common.ResultObject AddVirtualMachinePrivateIPAddresses(int itemId, bool selectRandom, int addressesNumber, string[] addresses)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "AddVirtualMachinePrivateIPAddresses", itemId, selectRandom, addressesNumber, addresses);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachinePrivateIPAddressesAsync(int itemId, bool selectRandom, int addressesNumber, string[] addresses)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "AddVirtualMachinePrivateIPAddresses", itemId, selectRandom, addressesNumber, addresses);
        }

        public SolidCP.Providers.Common.ResultObject SetVirtualMachinePrimaryPrivateIPAddress(int itemId, int addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "SetVirtualMachinePrimaryPrivateIPAddress", itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetVirtualMachinePrimaryPrivateIPAddressAsync(int itemId, int addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "SetVirtualMachinePrimaryPrivateIPAddress", itemId, addressId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteVirtualMachinePrivateIPAddresses(int itemId, int[] addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "DeleteVirtualMachinePrivateIPAddresses", itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachinePrivateIPAddressesAsync(int itemId, int[] addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "DeleteVirtualMachinePrivateIPAddresses", itemId, addressId);
        }

        public SolidCP.EnterpriseServer.VirtualMachinePermission[] /*List*/ GetVirtualMachinePermissions(int itemId)
        {
            return Invoke<SolidCP.EnterpriseServer.VirtualMachinePermission[], SolidCP.EnterpriseServer.VirtualMachinePermission>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetVirtualMachinePermissions", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachinePermission[]> GetVirtualMachinePermissionsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.VirtualMachinePermission[], SolidCP.EnterpriseServer.VirtualMachinePermission>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetVirtualMachinePermissions", itemId);
        }

        public int UpdateVirtualMachineUserPermissions(int itemId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "UpdateVirtualMachineUserPermissions", itemId, permissions);
        }

        public async System.Threading.Tasks.Task<int> UpdateVirtualMachineUserPermissionsAsync(int itemId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "UpdateVirtualMachineUserPermissions", itemId, permissions);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch[] GetExternalSwitches(int serviceId, string computerName)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualSwitch[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetExternalSwitches", serviceId, computerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetExternalSwitchesAsync(int serviceId, string computerName)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualSwitch[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetExternalSwitches", serviceId, computerName);
        }

        public SolidCP.Providers.Common.ResultObject DeleteVirtualMachine(int itemId, bool saveFiles, bool exportVps, string exportPath)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "DeleteVirtualMachine", itemId, saveFiles, exportVps, exportPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineAsync(int itemId, bool saveFiles, bool exportVps, string exportPath)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "DeleteVirtualMachine", itemId, saveFiles, exportVps, exportPath);
        }

        public int ReinstallVirtualMachine(int itemId, string adminPassword, bool preserveVirtualDiskFiles, bool saveVirtualDisk, bool exportVps, string exportPath)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "ReinstallVirtualMachine", itemId, adminPassword, preserveVirtualDiskFiles, saveVirtualDisk, exportVps, exportPath);
        }

        public async System.Threading.Tasks.Task<int> ReinstallVirtualMachineAsync(int itemId, string adminPassword, bool preserveVirtualDiskFiles, bool saveVirtualDisk, bool exportVps, string exportPath)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "ReinstallVirtualMachine", itemId, adminPassword, preserveVirtualDiskFiles, saveVirtualDisk, exportVps, exportPath);
        }

        public string GetVirtualMachineSummaryText(int itemId)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetVirtualMachineSummaryText", itemId);
        }

        public async System.Threading.Tasks.Task<string> GetVirtualMachineSummaryTextAsync(int itemId)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetVirtualMachineSummaryText", itemId);
        }

        public SolidCP.Providers.Common.ResultObject SendVirtualMachineSummaryLetter(int itemId, string to, string bcc)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "SendVirtualMachineSummaryLetter", itemId, to, bcc);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SendVirtualMachineSummaryLetterAsync(int itemId, string to, string bcc)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "SendVirtualMachineSummaryLetter", itemId, to, bcc);
        }

        public SolidCP.Providers.Virtualization.MonitoredObjectEvent[] GetDeviceEvents(int ItemID)
        {
            return Invoke<SolidCP.Providers.Virtualization.MonitoredObjectEvent[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetDeviceEvents", ItemID);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.MonitoredObjectEvent[]> GetDeviceEventsAsync(int ItemID)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.MonitoredObjectEvent[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetDeviceEvents", ItemID);
        }

        public SolidCP.Providers.Virtualization.MonitoredObjectAlert[] GetMonitoringAlerts(int ItemID)
        {
            return Invoke<SolidCP.Providers.Virtualization.MonitoredObjectAlert[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetMonitoringAlerts", ItemID);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.MonitoredObjectAlert[]> GetMonitoringAlertsAsync(int ItemID)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.MonitoredObjectAlert[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetMonitoringAlerts", ItemID);
        }

        public SolidCP.Providers.Virtualization.PerformanceDataValue[] GetPerfomanceValue(int ItemID, SolidCP.Providers.Virtualization.PerformanceType perf, System.DateTime startPeriod, System.DateTime endPeriod)
        {
            return Invoke<SolidCP.Providers.Virtualization.PerformanceDataValue[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetPerfomanceValue", ItemID, perf, startPeriod, endPeriod);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.PerformanceDataValue[]> GetPerfomanceValueAsync(int ItemID, SolidCP.Providers.Virtualization.PerformanceType perf, System.DateTime startPeriod, System.DateTime endPeriod)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.PerformanceDataValue[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetPerfomanceValue", ItemID, perf, startPeriod, endPeriod);
        }

        public SolidCP.Providers.Virtualization.VirtualNetworkInfo[] GetVirtualNetwork(int packageId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualNetworkInfo[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetVirtualNetwork", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualNetworkInfo[]> GetVirtualNetworkAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualNetworkInfo[]>("SolidCP.EnterpriseServer.esVirtualizationServerForPrivateCloud", "GetVirtualNetwork", packageId);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esVirtualizationServerForPrivateCloud : SolidCP.Web.Client.ClientBase<IesVirtualizationServerForPrivateCloud, esVirtualizationServerForPrivateCloudAssemblyClient>, IesVirtualizationServerForPrivateCloud
    {
        public bool CheckServerState(SolidCP.Providers.Virtualization.VMForPCSettingsName control, string connString, string connName, int serviceId)
        {
            return base.Client.CheckServerState(control, connString, connName, serviceId);
        }

        public async System.Threading.Tasks.Task<bool> CheckServerStateAsync(SolidCP.Providers.Virtualization.VMForPCSettingsName control, string connString, string connName, int serviceId)
        {
            return await base.Client.CheckServerStateAsync(control, connString, connName, serviceId);
        }

        public SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged GetVirtualMachines(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            return base.Client.GetVirtualMachines(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged> GetVirtualMachinesAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            return await base.Client.GetVirtualMachinesAsync(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows, recursive);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine[] GetVirtualMachinesByServiceId(int serviceId)
        {
            return base.Client.GetVirtualMachinesByServiceId(serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine[]> GetVirtualMachinesByServiceIdAsync(int serviceId)
        {
            return await base.Client.GetVirtualMachinesByServiceIdAsync(serviceId);
        }

        public SolidCP.Providers.Virtualization.VMInfo GetVirtualMachineItem(int itemId)
        {
            return base.Client.GetVirtualMachineItem(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMInfo> GetVirtualMachineItemAsync(int itemId)
        {
            return await base.Client.GetVirtualMachineItemAsync(itemId);
        }

        public string EvaluateVirtualMachineTemplate(int itemId, string template)
        {
            return base.Client.EvaluateVirtualMachineTemplate(itemId, template);
        }

        public async System.Threading.Tasks.Task<string> EvaluateVirtualMachineTemplateAsync(int itemId, string template)
        {
            return await base.Client.EvaluateVirtualMachineTemplateAsync(itemId, template);
        }

        public SolidCP.EnterpriseServer.NetworkAdapterDetails GetExternalNetworkDetails(int packageId)
        {
            return base.Client.GetExternalNetworkDetails(packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetExternalNetworkDetailsAsync(int packageId)
        {
            return await base.Client.GetExternalNetworkDetailsAsync(packageId);
        }

        public SolidCP.EnterpriseServer.PrivateIPAddressesPaged GetPackagePrivateIPAddressesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetPackagePrivateIPAddressesPaged(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PrivateIPAddressesPaged> GetPackagePrivateIPAddressesPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetPackagePrivateIPAddressesPagedAsync(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.EnterpriseServer.PrivateIPAddress[] /*List*/ GetPackagePrivateIPAddresses(int packageId)
        {
            return base.Client.GetPackagePrivateIPAddresses(packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PrivateIPAddress[]> GetPackagePrivateIPAddressesAsync(int packageId)
        {
            return await base.Client.GetPackagePrivateIPAddressesAsync(packageId);
        }

        public SolidCP.EnterpriseServer.NetworkAdapterDetails GetPrivateNetworkDetails(int packageId)
        {
            return base.Client.GetPrivateNetworkDetails(packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetPrivateNetworkDetailsAsync(int packageId)
        {
            return await base.Client.GetPrivateNetworkDetailsAsync(packageId);
        }

        public SolidCP.EnterpriseServer.VirtualMachinePermission[] /*List*/ GetSpaceUserPermissions(int packageId)
        {
            return base.Client.GetSpaceUserPermissions(packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachinePermission[]> GetSpaceUserPermissionsAsync(int packageId)
        {
            return await base.Client.GetSpaceUserPermissionsAsync(packageId);
        }

        public int UpdateSpaceUserPermissions(int packageId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions)
        {
            return base.Client.UpdateSpaceUserPermissions(packageId, permissions);
        }

        public async System.Threading.Tasks.Task<int> UpdateSpaceUserPermissionsAsync(int packageId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions)
        {
            return await base.Client.UpdateSpaceUserPermissionsAsync(packageId, permissions);
        }

        public SolidCP.EnterpriseServer.LogRecord[] /*List*/ GetSpaceAuditLog(int packageId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetSpaceAuditLog(packageId, startPeriod, endPeriod, severity, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.LogRecord[]> GetSpaceAuditLogAsync(int packageId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetSpaceAuditLogAsync(packageId, startPeriod, endPeriod, severity, sortColumn, startRow, maximumRows);
        }

        public SolidCP.EnterpriseServer.LogRecord[] /*List*/ GetVirtualMachineAuditLog(int itemId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetVirtualMachineAuditLog(itemId, startPeriod, endPeriod, severity, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.LogRecord[]> GetVirtualMachineAuditLogAsync(int itemId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetVirtualMachineAuditLogAsync(itemId, startPeriod, endPeriod, severity, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetOperatingSystemTemplatesPC(int packageId)
        {
            return base.Client.GetOperatingSystemTemplatesPC(packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetOperatingSystemTemplatesPCAsync(int packageId)
        {
            return await base.Client.GetOperatingSystemTemplatesPCAsync(packageId);
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetHosts(int serviceId)
        {
            return base.Client.GetHosts(serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetHostsAsync(int serviceId)
        {
            return await base.Client.GetHostsAsync(serviceId);
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetClusters(int serviceId)
        {
            return base.Client.GetClusters(serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetClustersAsync(int serviceId)
        {
            return await base.Client.GetClustersAsync(serviceId);
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetOperatingSystemTemplatesByServiceId(int serviceId)
        {
            return base.Client.GetOperatingSystemTemplatesByServiceId(serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetOperatingSystemTemplatesByServiceIdAsync(int serviceId)
        {
            return await base.Client.GetOperatingSystemTemplatesByServiceIdAsync(serviceId);
        }

        public int GetMaximumCpuCoresNumber(int packageId, string templateId)
        {
            return base.Client.GetMaximumCpuCoresNumber(packageId, templateId);
        }

        public async System.Threading.Tasks.Task<int> GetMaximumCpuCoresNumberAsync(int packageId, string templateId)
        {
            return await base.Client.GetMaximumCpuCoresNumberAsync(packageId, templateId);
        }

        public string GetDefaultExportPath(int itemId)
        {
            return base.Client.GetDefaultExportPath(itemId);
        }

        public async System.Threading.Tasks.Task<string> GetDefaultExportPathAsync(int itemId)
        {
            return await base.Client.GetDefaultExportPathAsync(itemId);
        }

        public SolidCP.Providers.Common.ResultObject CreateVMFromVM(int packageId, SolidCP.Providers.Virtualization.VMInfo vmTemplate, string vmName)
        {
            return base.Client.CreateVMFromVM(packageId, vmTemplate, vmName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateVMFromVMAsync(int packageId, SolidCP.Providers.Virtualization.VMInfo vmTemplate, string vmName)
        {
            return await base.Client.CreateVMFromVMAsync(packageId, vmTemplate, vmName);
        }

        public SolidCP.Providers.ResultObjects.IntResult CreateVirtualMachine(int packageId, string hostname, string domain, string osTemplateFile, string vmName, string password, string summaryLetterEmail, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, string externalNetworkLocation, string externalNicMacAddress, string externalVirtualNetwork, bool privateNetworkEnabled, string privateNetworkLocation, string privateNicMacAddress, string privateVirtualNetwork, ushort privateVLanID)
        {
            return base.Client.CreateVirtualMachine(packageId, hostname, domain, osTemplateFile, vmName, password, summaryLetterEmail, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, externalNetworkLocation, externalNicMacAddress, externalVirtualNetwork, privateNetworkEnabled, privateNetworkLocation, privateNicMacAddress, privateVirtualNetwork, privateVLanID);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> CreateVirtualMachineAsync(int packageId, string hostname, string domain, string osTemplateFile, string vmName, string password, string summaryLetterEmail, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, string externalNetworkLocation, string externalNicMacAddress, string externalVirtualNetwork, bool privateNetworkEnabled, string privateNetworkLocation, string privateNicMacAddress, string privateVirtualNetwork, ushort privateVLanID)
        {
            return await base.Client.CreateVirtualMachineAsync(packageId, hostname, domain, osTemplateFile, vmName, password, summaryLetterEmail, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, externalNetworkLocation, externalNicMacAddress, externalVirtualNetwork, privateNetworkEnabled, privateNetworkLocation, privateNicMacAddress, privateVirtualNetwork, privateVLanID);
        }

        public SolidCP.Providers.ResultObjects.IntResult ImportVirtualMachine(int packageId, int serviceId, string vmId, string osTemplateFile, string adminPassword, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, string externalNicMacAddress, int[] externalAddresses, string managementNicMacAddress, int managementAddress)
        {
            return base.Client.ImportVirtualMachine(packageId, serviceId, vmId, osTemplateFile, adminPassword, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNicMacAddress, externalAddresses, managementNicMacAddress, managementAddress);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> ImportVirtualMachineAsync(int packageId, int serviceId, string vmId, string osTemplateFile, string adminPassword, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, string externalNicMacAddress, int[] externalAddresses, string managementNicMacAddress, int managementAddress)
        {
            return await base.Client.ImportVirtualMachineAsync(packageId, serviceId, vmId, osTemplateFile, adminPassword, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNicMacAddress, externalAddresses, managementNicMacAddress, managementAddress);
        }

        public byte[] GetVirtualMachineThumbnail(int itemId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return base.Client.GetVirtualMachineThumbnail(itemId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailAsync(int itemId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return await base.Client.GetVirtualMachineThumbnailAsync(itemId, size);
        }

        public SolidCP.Providers.Virtualization.VMInfo GetVirtualMachineGeneralDetails(int itemId)
        {
            return base.Client.GetVirtualMachineGeneralDetails(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMInfo> GetVirtualMachineGeneralDetailsAsync(int itemId)
        {
            return await base.Client.GetVirtualMachineGeneralDetailsAsync(itemId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineExtendedInfo(int serviceId, string vmId)
        {
            return base.Client.GetVirtualMachineExtendedInfo(serviceId, vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineExtendedInfoAsync(int serviceId, string vmId)
        {
            return await base.Client.GetVirtualMachineExtendedInfoAsync(serviceId, vmId);
        }

        public int CancelVirtualMachineJob(string jobId)
        {
            return base.Client.CancelVirtualMachineJob(jobId);
        }

        public async System.Threading.Tasks.Task<int> CancelVirtualMachineJobAsync(string jobId)
        {
            return await base.Client.CancelVirtualMachineJobAsync(jobId);
        }

        public SolidCP.Providers.Common.ResultObject UpdateVirtualMachineHostName(int itemId, string hostname, bool updateNetBIOS)
        {
            return base.Client.UpdateVirtualMachineHostName(itemId, hostname, updateNetBIOS);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateVirtualMachineHostNameAsync(int itemId, string hostname, bool updateNetBIOS)
        {
            return await base.Client.UpdateVirtualMachineHostNameAsync(itemId, hostname, updateNetBIOS);
        }

        public SolidCP.Providers.Common.ResultObject ChangeVirtualMachineState(int itemId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState state)
        {
            return base.Client.ChangeVirtualMachineState(itemId, state);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeVirtualMachineStateAsync(int itemId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState state)
        {
            return await base.Client.ChangeVirtualMachineStateAsync(itemId, state);
        }

        public SolidCP.Providers.Virtualization.ConcreteJob[] /*List*/ GetVirtualMachineJobs(int itemId)
        {
            return base.Client.GetVirtualMachineJobs(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob[]> GetVirtualMachineJobsAsync(int itemId)
        {
            return await base.Client.GetVirtualMachineJobsAsync(itemId);
        }

        public SolidCP.Providers.Common.ResultObject ChangeAdministratorPassword(int itemId, string password)
        {
            return base.Client.ChangeAdministratorPassword(itemId, password);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeAdministratorPasswordAsync(int itemId, string password)
        {
            return await base.Client.ChangeAdministratorPasswordAsync(itemId, password);
        }

        public SolidCP.Providers.Common.ResultObject UpdateVirtualMachineConfiguration(int itemId, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, bool privateNetworkEnabled)
        {
            return base.Client.UpdateVirtualMachineConfiguration(itemId, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, privateNetworkEnabled);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateVirtualMachineConfigurationAsync(int itemId, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, bool privateNetworkEnabled)
        {
            return await base.Client.UpdateVirtualMachineConfigurationAsync(itemId, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, privateNetworkEnabled);
        }

        public SolidCP.Providers.Virtualization.LibraryItem GetInsertedDvdDisk(int itemId)
        {
            return base.Client.GetInsertedDvdDisk(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem> GetInsertedDvdDiskAsync(int itemId)
        {
            return await base.Client.GetInsertedDvdDiskAsync(itemId);
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetLibraryDisks(int itemId)
        {
            return base.Client.GetLibraryDisks(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetLibraryDisksAsync(int itemId)
        {
            return await base.Client.GetLibraryDisksAsync(itemId);
        }

        public SolidCP.Providers.Common.ResultObject InsertDvdDisk(int itemId, string isoPath)
        {
            return base.Client.InsertDvdDisk(itemId, isoPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> InsertDvdDiskAsync(int itemId, string isoPath)
        {
            return await base.Client.InsertDvdDiskAsync(itemId, isoPath);
        }

        public SolidCP.Providers.Common.ResultObject EjectDvdDisk(int itemId)
        {
            return base.Client.EjectDvdDisk(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> EjectDvdDiskAsync(int itemId)
        {
            return await base.Client.EjectDvdDiskAsync(itemId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachineSnapshot[] GetVirtualMachineSnapshots(int itemId)
        {
            return base.Client.GetVirtualMachineSnapshots(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]> GetVirtualMachineSnapshotsAsync(int itemId)
        {
            return await base.Client.GetVirtualMachineSnapshotsAsync(itemId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachineSnapshot GetSnapshot(int itemId, string snaphostId)
        {
            return base.Client.GetSnapshot(itemId, snaphostId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot> GetSnapshotAsync(int itemId, string snaphostId)
        {
            return await base.Client.GetSnapshotAsync(itemId, snaphostId);
        }

        public SolidCP.Providers.Common.ResultObject CreateSnapshot(int itemId)
        {
            return base.Client.CreateSnapshot(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateSnapshotAsync(int itemId)
        {
            return await base.Client.CreateSnapshotAsync(itemId);
        }

        public SolidCP.Providers.Common.ResultObject ApplySnapshot(int itemId, string snapshotId)
        {
            return base.Client.ApplySnapshot(itemId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ApplySnapshotAsync(int itemId, string snapshotId)
        {
            return await base.Client.ApplySnapshotAsync(itemId, snapshotId);
        }

        public SolidCP.Providers.Common.ResultObject RenameSnapshot(int itemId, string snapshotId, string newName)
        {
            return base.Client.RenameSnapshot(itemId, snapshotId, newName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RenameSnapshotAsync(int itemId, string snapshotId, string newName)
        {
            return await base.Client.RenameSnapshotAsync(itemId, snapshotId, newName);
        }

        public SolidCP.Providers.Common.ResultObject DeleteSnapshot(int itemId, string snapshotId)
        {
            return base.Client.DeleteSnapshot(itemId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteSnapshotAsync(int itemId, string snapshotId)
        {
            return await base.Client.DeleteSnapshotAsync(itemId, snapshotId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteSnapshotSubtree(int itemId, string snapshotId)
        {
            return base.Client.DeleteSnapshotSubtree(itemId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteSnapshotSubtreeAsync(int itemId, string snapshotId)
        {
            return await base.Client.DeleteSnapshotSubtreeAsync(itemId, snapshotId);
        }

        public byte[] GetSnapshotThumbnail(int itemId, string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return base.Client.GetSnapshotThumbnail(itemId, snapshotId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailAsync(int itemId, string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return await base.Client.GetSnapshotThumbnailAsync(itemId, snapshotId, size);
        }

        public void ConfigureCreatedVMNetworkAdapters(SolidCP.Providers.Virtualization.VMInfo vmInfo)
        {
            base.Client.ConfigureCreatedVMNetworkAdapters(vmInfo);
        }

        public async System.Threading.Tasks.Task ConfigureCreatedVMNetworkAdaptersAsync(SolidCP.Providers.Virtualization.VMInfo vmInfo)
        {
            await base.Client.ConfigureCreatedVMNetworkAdaptersAsync(vmInfo);
        }

        public SolidCP.EnterpriseServer.NetworkAdapterDetails GetExternalNetworkAdapterDetails(int itemId)
        {
            return base.Client.GetExternalNetworkAdapterDetails(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetExternalNetworkAdapterDetailsAsync(int itemId)
        {
            return await base.Client.GetExternalNetworkAdapterDetailsAsync(itemId);
        }

        public SolidCP.Providers.Common.ResultObject AddVirtualMachineExternalIPAddresses(int itemId, bool selectRandom, int addressesNumber, int[] addressId)
        {
            return base.Client.AddVirtualMachineExternalIPAddresses(itemId, selectRandom, addressesNumber, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachineExternalIPAddressesAsync(int itemId, bool selectRandom, int addressesNumber, int[] addressId)
        {
            return await base.Client.AddVirtualMachineExternalIPAddressesAsync(itemId, selectRandom, addressesNumber, addressId);
        }

        public SolidCP.Providers.Common.ResultObject SetVirtualMachinePrimaryExternalIPAddress(int itemId, int addressId)
        {
            return base.Client.SetVirtualMachinePrimaryExternalIPAddress(itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetVirtualMachinePrimaryExternalIPAddressAsync(int itemId, int addressId)
        {
            return await base.Client.SetVirtualMachinePrimaryExternalIPAddressAsync(itemId, addressId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteVirtualMachineExternalIPAddresses(int itemId, int[] addressId)
        {
            return base.Client.DeleteVirtualMachineExternalIPAddresses(itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineExternalIPAddressesAsync(int itemId, int[] addressId)
        {
            return await base.Client.DeleteVirtualMachineExternalIPAddressesAsync(itemId, addressId);
        }

        public SolidCP.EnterpriseServer.NetworkAdapterDetails GetPrivateNetworkAdapterDetails(int itemId)
        {
            return base.Client.GetPrivateNetworkAdapterDetails(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetPrivateNetworkAdapterDetailsAsync(int itemId)
        {
            return await base.Client.GetPrivateNetworkAdapterDetailsAsync(itemId);
        }

        public SolidCP.Providers.Common.ResultObject AddVirtualMachinePrivateIPAddresses(int itemId, bool selectRandom, int addressesNumber, string[] addresses)
        {
            return base.Client.AddVirtualMachinePrivateIPAddresses(itemId, selectRandom, addressesNumber, addresses);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachinePrivateIPAddressesAsync(int itemId, bool selectRandom, int addressesNumber, string[] addresses)
        {
            return await base.Client.AddVirtualMachinePrivateIPAddressesAsync(itemId, selectRandom, addressesNumber, addresses);
        }

        public SolidCP.Providers.Common.ResultObject SetVirtualMachinePrimaryPrivateIPAddress(int itemId, int addressId)
        {
            return base.Client.SetVirtualMachinePrimaryPrivateIPAddress(itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetVirtualMachinePrimaryPrivateIPAddressAsync(int itemId, int addressId)
        {
            return await base.Client.SetVirtualMachinePrimaryPrivateIPAddressAsync(itemId, addressId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteVirtualMachinePrivateIPAddresses(int itemId, int[] addressId)
        {
            return base.Client.DeleteVirtualMachinePrivateIPAddresses(itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachinePrivateIPAddressesAsync(int itemId, int[] addressId)
        {
            return await base.Client.DeleteVirtualMachinePrivateIPAddressesAsync(itemId, addressId);
        }

        public SolidCP.EnterpriseServer.VirtualMachinePermission[] /*List*/ GetVirtualMachinePermissions(int itemId)
        {
            return base.Client.GetVirtualMachinePermissions(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachinePermission[]> GetVirtualMachinePermissionsAsync(int itemId)
        {
            return await base.Client.GetVirtualMachinePermissionsAsync(itemId);
        }

        public int UpdateVirtualMachineUserPermissions(int itemId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions)
        {
            return base.Client.UpdateVirtualMachineUserPermissions(itemId, permissions);
        }

        public async System.Threading.Tasks.Task<int> UpdateVirtualMachineUserPermissionsAsync(int itemId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions)
        {
            return await base.Client.UpdateVirtualMachineUserPermissionsAsync(itemId, permissions);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch[] GetExternalSwitches(int serviceId, string computerName)
        {
            return base.Client.GetExternalSwitches(serviceId, computerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetExternalSwitchesAsync(int serviceId, string computerName)
        {
            return await base.Client.GetExternalSwitchesAsync(serviceId, computerName);
        }

        public SolidCP.Providers.Common.ResultObject DeleteVirtualMachine(int itemId, bool saveFiles, bool exportVps, string exportPath)
        {
            return base.Client.DeleteVirtualMachine(itemId, saveFiles, exportVps, exportPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineAsync(int itemId, bool saveFiles, bool exportVps, string exportPath)
        {
            return await base.Client.DeleteVirtualMachineAsync(itemId, saveFiles, exportVps, exportPath);
        }

        public int ReinstallVirtualMachine(int itemId, string adminPassword, bool preserveVirtualDiskFiles, bool saveVirtualDisk, bool exportVps, string exportPath)
        {
            return base.Client.ReinstallVirtualMachine(itemId, adminPassword, preserveVirtualDiskFiles, saveVirtualDisk, exportVps, exportPath);
        }

        public async System.Threading.Tasks.Task<int> ReinstallVirtualMachineAsync(int itemId, string adminPassword, bool preserveVirtualDiskFiles, bool saveVirtualDisk, bool exportVps, string exportPath)
        {
            return await base.Client.ReinstallVirtualMachineAsync(itemId, adminPassword, preserveVirtualDiskFiles, saveVirtualDisk, exportVps, exportPath);
        }

        public string GetVirtualMachineSummaryText(int itemId)
        {
            return base.Client.GetVirtualMachineSummaryText(itemId);
        }

        public async System.Threading.Tasks.Task<string> GetVirtualMachineSummaryTextAsync(int itemId)
        {
            return await base.Client.GetVirtualMachineSummaryTextAsync(itemId);
        }

        public SolidCP.Providers.Common.ResultObject SendVirtualMachineSummaryLetter(int itemId, string to, string bcc)
        {
            return base.Client.SendVirtualMachineSummaryLetter(itemId, to, bcc);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SendVirtualMachineSummaryLetterAsync(int itemId, string to, string bcc)
        {
            return await base.Client.SendVirtualMachineSummaryLetterAsync(itemId, to, bcc);
        }

        public SolidCP.Providers.Virtualization.MonitoredObjectEvent[] GetDeviceEvents(int ItemID)
        {
            return base.Client.GetDeviceEvents(ItemID);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.MonitoredObjectEvent[]> GetDeviceEventsAsync(int ItemID)
        {
            return await base.Client.GetDeviceEventsAsync(ItemID);
        }

        public SolidCP.Providers.Virtualization.MonitoredObjectAlert[] GetMonitoringAlerts(int ItemID)
        {
            return base.Client.GetMonitoringAlerts(ItemID);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.MonitoredObjectAlert[]> GetMonitoringAlertsAsync(int ItemID)
        {
            return await base.Client.GetMonitoringAlertsAsync(ItemID);
        }

        public SolidCP.Providers.Virtualization.PerformanceDataValue[] GetPerfomanceValue(int ItemID, SolidCP.Providers.Virtualization.PerformanceType perf, System.DateTime startPeriod, System.DateTime endPeriod)
        {
            return base.Client.GetPerfomanceValue(ItemID, perf, startPeriod, endPeriod);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.PerformanceDataValue[]> GetPerfomanceValueAsync(int ItemID, SolidCP.Providers.Virtualization.PerformanceType perf, System.DateTime startPeriod, System.DateTime endPeriod)
        {
            return await base.Client.GetPerfomanceValueAsync(ItemID, perf, startPeriod, endPeriod);
        }

        public SolidCP.Providers.Virtualization.VirtualNetworkInfo[] GetVirtualNetwork(int packageId)
        {
            return base.Client.GetVirtualNetwork(packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualNetworkInfo[]> GetVirtualNetworkAsync(int packageId)
        {
            return await base.Client.GetVirtualNetworkAsync(packageId);
        }
    }
}
#endif