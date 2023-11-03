#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesVirtualizationServerProxmox", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesVirtualizationServerProxmox
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachines", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachinesResponse")]
        SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged GetVirtualMachines(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachines", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachinesResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged> GetVirtualMachinesAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachinesByServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachinesByServiceIdResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine[] GetVirtualMachinesByServiceId(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachinesByServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachinesByServiceIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine[]> GetVirtualMachinesByServiceIdAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineItem", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineItemResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineItem(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineItem", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineItemResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineItemAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/EvaluateVirtualMachineTemplate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/EvaluateVirtualMachineTemplateResponse")]
        string EvaluateVirtualMachineTemplate(int itemId, string template);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/EvaluateVirtualMachineTemplate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/EvaluateVirtualMachineTemplateResponse")]
        System.Threading.Tasks.Task<string> EvaluateVirtualMachineTemplateAsync(int itemId, string template);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetExternalNetworkVLAN", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetExternalNetworkVLANResponse")]
        int GetExternalNetworkVLAN(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetExternalNetworkVLAN", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetExternalNetworkVLANResponse")]
        System.Threading.Tasks.Task<int> GetExternalNetworkVLANAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetExternalNetworkDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetExternalNetworkDetailsResponse")]
        SolidCP.EnterpriseServer.NetworkAdapterDetails GetExternalNetworkDetails(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetExternalNetworkDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetExternalNetworkDetailsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetExternalNetworkDetailsAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetPackagePrivateIPAddressesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetPackagePrivateIPAddressesPagedResponse")]
        SolidCP.EnterpriseServer.PrivateIPAddressesPaged GetPackagePrivateIPAddressesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetPackagePrivateIPAddressesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetPackagePrivateIPAddressesPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PrivateIPAddressesPaged> GetPackagePrivateIPAddressesPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetPackagePrivateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetPackagePrivateIPAddressesResponse")]
        SolidCP.EnterpriseServer.PrivateIPAddress[] /*List*/ GetPackagePrivateIPAddresses(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetPackagePrivateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetPackagePrivateIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PrivateIPAddress[]> GetPackagePrivateIPAddressesAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetPrivateNetworkDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetPrivateNetworkDetailsResponse")]
        SolidCP.EnterpriseServer.NetworkAdapterDetails GetPrivateNetworkDetails(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetPrivateNetworkDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetPrivateNetworkDetailsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetPrivateNetworkDetailsAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetSpaceUserPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetSpaceUserPermissionsResponse")]
        SolidCP.EnterpriseServer.VirtualMachinePermission[] /*List*/ GetSpaceUserPermissions(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetSpaceUserPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetSpaceUserPermissionsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachinePermission[]> GetSpaceUserPermissionsAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/UpdateSpaceUserPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/UpdateSpaceUserPermissionsResponse")]
        int UpdateSpaceUserPermissions(int packageId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/UpdateSpaceUserPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/UpdateSpaceUserPermissionsResponse")]
        System.Threading.Tasks.Task<int> UpdateSpaceUserPermissionsAsync(int packageId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetSpaceAuditLog", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetSpaceAuditLogResponse")]
        SolidCP.EnterpriseServer.LogRecord[] /*List*/ GetSpaceAuditLog(int packageId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetSpaceAuditLog", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetSpaceAuditLogResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.LogRecord[]> GetSpaceAuditLogAsync(int packageId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineAuditLog", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineAuditLogResponse")]
        SolidCP.EnterpriseServer.LogRecord[] /*List*/ GetVirtualMachineAuditLog(int itemId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineAuditLog", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineAuditLogResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.LogRecord[]> GetVirtualMachineAuditLogAsync(int itemId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetOperatingSystemTemplates", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetOperatingSystemTemplatesResponse")]
        SolidCP.Providers.Virtualization.LibraryItem[] GetOperatingSystemTemplates(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetOperatingSystemTemplates", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetOperatingSystemTemplatesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetOperatingSystemTemplatesAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetOperatingSystemTemplatesByServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetOperatingSystemTemplatesByServiceIdResponse")]
        SolidCP.Providers.Virtualization.LibraryItem[] GetOperatingSystemTemplatesByServiceId(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetOperatingSystemTemplatesByServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetOperatingSystemTemplatesByServiceIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetOperatingSystemTemplatesByServiceIdAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetMaximumCpuCoresNumber", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetMaximumCpuCoresNumberResponse")]
        int GetMaximumCpuCoresNumber(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetMaximumCpuCoresNumber", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetMaximumCpuCoresNumberResponse")]
        System.Threading.Tasks.Task<int> GetMaximumCpuCoresNumberAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetDefaultExportPath", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetDefaultExportPathResponse")]
        string GetDefaultExportPath(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetDefaultExportPath", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetDefaultExportPathResponse")]
        System.Threading.Tasks.Task<string> GetDefaultExportPathAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/CreateDefaultVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/CreateDefaultVirtualMachineResponse")]
        SolidCP.Providers.ResultObjects.IntResult CreateDefaultVirtualMachine(int packageId, string hostname, string osTemplate, string password, string summaryLetterEmail);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/CreateDefaultVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/CreateDefaultVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> CreateDefaultVirtualMachineAsync(int packageId, string hostname, string osTemplate, string password, string summaryLetterEmail);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/CreateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/CreateVirtualMachineResponse")]
        SolidCP.Providers.ResultObjects.IntResult CreateVirtualMachine(int packageId, string hostname, string osTemplateFile, string password, string summaryLetterEmail, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses, bool privateNetworkEnabled, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses, SolidCP.Providers.Virtualization.VirtualMachine otherSettings);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/CreateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/CreateVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> CreateVirtualMachineAsync(int packageId, string hostname, string osTemplateFile, string password, string summaryLetterEmail, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses, bool privateNetworkEnabled, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses, SolidCP.Providers.Virtualization.VirtualMachine otherSettings);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/ImportVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/ImportVirtualMachineResponse")]
        SolidCP.Providers.ResultObjects.IntResult ImportVirtualMachine(int packageId, int serviceId, string vmId, string osTemplateFile, string adminPassword, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, string externalNicMacAddress, int[] externalAddresses, string managementNicMacAddress, int managementAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/ImportVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/ImportVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> ImportVirtualMachineAsync(int packageId, int serviceId, string vmId, string osTemplateFile, string adminPassword, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, string externalNicMacAddress, int[] externalAddresses, string managementNicMacAddress, int managementAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineThumbnail", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineThumbnailResponse")]
        byte[] GetVirtualMachineThumbnail(int itemId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineThumbnail", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineThumbnailResponse")]
        System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailAsync(int itemId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineGeneralDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineGeneralDetailsResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineGeneralDetails(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineGeneralDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineGeneralDetailsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineGeneralDetailsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineExtendedInfo", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineExtendedInfoResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineExtendedInfo(int serviceId, string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineExtendedInfo", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineExtendedInfoResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineExtendedInfoAsync(int serviceId, string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/CancelVirtualMachineJob", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/CancelVirtualMachineJobResponse")]
        int CancelVirtualMachineJob(string jobId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/CancelVirtualMachineJob", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/CancelVirtualMachineJobResponse")]
        System.Threading.Tasks.Task<int> CancelVirtualMachineJobAsync(string jobId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/UpdateVirtualMachineHostName", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/UpdateVirtualMachineHostNameResponse")]
        SolidCP.Providers.Common.ResultObject UpdateVirtualMachineHostName(int itemId, string hostname, bool updateNetBIOS);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/UpdateVirtualMachineHostName", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/UpdateVirtualMachineHostNameResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateVirtualMachineHostNameAsync(int itemId, string hostname, bool updateNetBIOS);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/ChangeVirtualMachineState", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/ChangeVirtualMachineStateResponse")]
        SolidCP.Providers.Common.ResultObject ChangeVirtualMachineState(int itemId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState state);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/ChangeVirtualMachineState", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/ChangeVirtualMachineStateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeVirtualMachineStateAsync(int itemId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState state);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineJobs", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineJobsResponse")]
        SolidCP.Providers.Virtualization.ConcreteJob[] /*List*/ GetVirtualMachineJobs(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineJobs", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineJobsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob[]> GetVirtualMachineJobsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineVNCURL", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineVNCURLResponse")]
        string GetVirtualMachineVNCURL(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineVNCURL", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineVNCURLResponse")]
        System.Threading.Tasks.Task<string> GetVirtualMachineVNCURLAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/ChangeAdministratorPassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/ChangeAdministratorPasswordResponse")]
        SolidCP.Providers.Common.ResultObject ChangeAdministratorPassword(int itemId, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/ChangeAdministratorPassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/ChangeAdministratorPasswordResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeAdministratorPasswordAsync(int itemId, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/UpdateVirtualMachineConfiguration", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/UpdateVirtualMachineConfigurationResponse")]
        SolidCP.Providers.Common.ResultObject UpdateVirtualMachineConfiguration(int itemId, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, bool privateNetworkEnabled, SolidCP.Providers.Virtualization.VirtualMachine otherSettings);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/UpdateVirtualMachineConfiguration", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/UpdateVirtualMachineConfigurationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateVirtualMachineConfigurationAsync(int itemId, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, bool privateNetworkEnabled, SolidCP.Providers.Virtualization.VirtualMachine otherSettings);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetInsertedDvdDisk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetInsertedDvdDiskResponse")]
        SolidCP.Providers.Virtualization.LibraryItem GetInsertedDvdDisk(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetInsertedDvdDisk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetInsertedDvdDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem> GetInsertedDvdDiskAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetLibraryDisks", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetLibraryDisksResponse")]
        SolidCP.Providers.Virtualization.LibraryItem[] GetLibraryDisks(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetLibraryDisks", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetLibraryDisksResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetLibraryDisksAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/InsertDvdDisk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/InsertDvdDiskResponse")]
        SolidCP.Providers.Common.ResultObject InsertDvdDisk(int itemId, string isoPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/InsertDvdDisk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/InsertDvdDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> InsertDvdDiskAsync(int itemId, string isoPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/EjectDvdDisk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/EjectDvdDiskResponse")]
        SolidCP.Providers.Common.ResultObject EjectDvdDisk(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/EjectDvdDisk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/EjectDvdDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> EjectDvdDiskAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineSnapshots", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineSnapshotsResponse")]
        SolidCP.Providers.Virtualization.VirtualMachineSnapshot[] GetVirtualMachineSnapshots(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineSnapshots", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineSnapshotsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]> GetVirtualMachineSnapshotsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetSnapshotResponse")]
        SolidCP.Providers.Virtualization.VirtualMachineSnapshot GetSnapshot(int itemId, string snaphostId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot> GetSnapshotAsync(int itemId, string snaphostId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/CreateSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/CreateSnapshotResponse")]
        SolidCP.Providers.Common.ResultObject CreateSnapshot(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/CreateSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/CreateSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateSnapshotAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/ApplySnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/ApplySnapshotResponse")]
        SolidCP.Providers.Common.ResultObject ApplySnapshot(int itemId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/ApplySnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/ApplySnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ApplySnapshotAsync(int itemId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/RenameSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/RenameSnapshotResponse")]
        SolidCP.Providers.Common.ResultObject RenameSnapshot(int itemId, string snapshotId, string newName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/RenameSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/RenameSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RenameSnapshotAsync(int itemId, string snapshotId, string newName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/DeleteSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/DeleteSnapshotResponse")]
        SolidCP.Providers.Common.ResultObject DeleteSnapshot(int itemId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/DeleteSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/DeleteSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteSnapshotAsync(int itemId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/DeleteSnapshotSubtree", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/DeleteSnapshotSubtreeResponse")]
        SolidCP.Providers.Common.ResultObject DeleteSnapshotSubtree(int itemId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/DeleteSnapshotSubtree", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/DeleteSnapshotSubtreeResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteSnapshotSubtreeAsync(int itemId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetSnapshotThumbnail", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetSnapshotThumbnailResponse")]
        byte[] GetSnapshotThumbnail(int itemId, string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetSnapshotThumbnail", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetSnapshotThumbnailResponse")]
        System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailAsync(int itemId, string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetExternalNetworkAdapterDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetExternalNetworkAdapterDetailsResponse")]
        SolidCP.EnterpriseServer.NetworkAdapterDetails GetExternalNetworkAdapterDetails(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetExternalNetworkAdapterDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetExternalNetworkAdapterDetailsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetExternalNetworkAdapterDetailsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/AddVirtualMachineExternalIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/AddVirtualMachineExternalIPAddressesResponse")]
        SolidCP.Providers.Common.ResultObject AddVirtualMachineExternalIPAddresses(int itemId, bool selectRandom, int addressesNumber, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/AddVirtualMachineExternalIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/AddVirtualMachineExternalIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachineExternalIPAddressesAsync(int itemId, bool selectRandom, int addressesNumber, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/SetVirtualMachinePrimaryExternalIPAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/SetVirtualMachinePrimaryExternalIPAddressResponse")]
        SolidCP.Providers.Common.ResultObject SetVirtualMachinePrimaryExternalIPAddress(int itemId, int addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/SetVirtualMachinePrimaryExternalIPAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/SetVirtualMachinePrimaryExternalIPAddressResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetVirtualMachinePrimaryExternalIPAddressAsync(int itemId, int addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/DeleteVirtualMachineExternalIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/DeleteVirtualMachineExternalIPAddressesResponse")]
        SolidCP.Providers.Common.ResultObject DeleteVirtualMachineExternalIPAddresses(int itemId, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/DeleteVirtualMachineExternalIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/DeleteVirtualMachineExternalIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineExternalIPAddressesAsync(int itemId, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetPrivateNetworkAdapterDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetPrivateNetworkAdapterDetailsResponse")]
        SolidCP.EnterpriseServer.NetworkAdapterDetails GetPrivateNetworkAdapterDetails(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetPrivateNetworkAdapterDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetPrivateNetworkAdapterDetailsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetPrivateNetworkAdapterDetailsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/AddVirtualMachinePrivateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/AddVirtualMachinePrivateIPAddressesResponse")]
        SolidCP.Providers.Common.ResultObject AddVirtualMachinePrivateIPAddresses(int itemId, bool selectRandom, int addressesNumber, string[] addresses);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/AddVirtualMachinePrivateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/AddVirtualMachinePrivateIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachinePrivateIPAddressesAsync(int itemId, bool selectRandom, int addressesNumber, string[] addresses);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/SetVirtualMachinePrimaryPrivateIPAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/SetVirtualMachinePrimaryPrivateIPAddressResponse")]
        SolidCP.Providers.Common.ResultObject SetVirtualMachinePrimaryPrivateIPAddress(int itemId, int addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/SetVirtualMachinePrimaryPrivateIPAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/SetVirtualMachinePrimaryPrivateIPAddressResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetVirtualMachinePrimaryPrivateIPAddressAsync(int itemId, int addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/DeleteVirtualMachinePrivateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/DeleteVirtualMachinePrivateIPAddressesResponse")]
        SolidCP.Providers.Common.ResultObject DeleteVirtualMachinePrivateIPAddresses(int itemId, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/DeleteVirtualMachinePrivateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/DeleteVirtualMachinePrivateIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachinePrivateIPAddressesAsync(int itemId, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachinePermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachinePermissionsResponse")]
        SolidCP.EnterpriseServer.VirtualMachinePermission[] /*List*/ GetVirtualMachinePermissions(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachinePermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachinePermissionsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachinePermission[]> GetVirtualMachinePermissionsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/UpdateVirtualMachineUserPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/UpdateVirtualMachineUserPermissionsResponse")]
        int UpdateVirtualMachineUserPermissions(int itemId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/UpdateVirtualMachineUserPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/UpdateVirtualMachineUserPermissionsResponse")]
        System.Threading.Tasks.Task<int> UpdateVirtualMachineUserPermissionsAsync(int itemId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetExternalSwitches", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetExternalSwitchesResponse")]
        SolidCP.Providers.Virtualization.VirtualSwitch[] GetExternalSwitches(int serviceId, string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetExternalSwitches", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetExternalSwitchesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetExternalSwitchesAsync(int serviceId, string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/DeleteVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/DeleteVirtualMachineResponse")]
        SolidCP.Providers.Common.ResultObject DeleteVirtualMachine(int itemId, bool saveFiles, bool exportVps, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/DeleteVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/DeleteVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineAsync(int itemId, bool saveFiles, bool exportVps, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/ReinstallVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/ReinstallVirtualMachineResponse")]
        int ReinstallVirtualMachine(int itemId, string adminPassword, bool preserveVirtualDiskFiles, bool saveVirtualDisk, bool exportVps, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/ReinstallVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/ReinstallVirtualMachineResponse")]
        System.Threading.Tasks.Task<int> ReinstallVirtualMachineAsync(int itemId, string adminPassword, bool preserveVirtualDiskFiles, bool saveVirtualDisk, bool exportVps, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineSummaryText", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineSummaryTextResponse")]
        string GetVirtualMachineSummaryText(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineSummaryText", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetVirtualMachineSummaryTextResponse")]
        System.Threading.Tasks.Task<string> GetVirtualMachineSummaryTextAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/SendVirtualMachineSummaryLetter", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/SendVirtualMachineSummaryLetterResponse")]
        SolidCP.Providers.Common.ResultObject SendVirtualMachineSummaryLetter(int itemId, string to, string bcc);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/SendVirtualMachineSummaryLetter", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/SendVirtualMachineSummaryLetterResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SendVirtualMachineSummaryLetterAsync(int itemId, string to, string bcc);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetCertificates", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetCertificatesResponse")]
        SolidCP.Providers.Virtualization.CertificateInfo[] GetCertificates(int serviceId, string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetCertificates", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetCertificatesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.CertificateInfo[]> GetCertificatesAsync(int serviceId, string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/SetReplicaServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/SetReplicaServerResponse")]
        SolidCP.Providers.Common.ResultObject SetReplicaServer(int serviceId, string remoteServer, string thumbprint, string storagePath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/SetReplicaServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/SetReplicaServerResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetReplicaServerAsync(int serviceId, string remoteServer, string thumbprint, string storagePath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/UnsetReplicaServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/UnsetReplicaServerResponse")]
        SolidCP.Providers.Common.ResultObject UnsetReplicaServer(int serviceId, string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/UnsetReplicaServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/UnsetReplicaServerResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UnsetReplicaServerAsync(int serviceId, string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetReplicaServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetReplicaServerResponse")]
        SolidCP.Providers.Virtualization.ReplicationServerInfo GetReplicaServer(int serviceId, string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetReplicaServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetReplicaServerResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReplicationServerInfo> GetReplicaServerAsync(int serviceId, string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetReplication", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetReplicationResponse")]
        SolidCP.Providers.Virtualization.VmReplication GetReplication(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetReplication", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetReplicationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VmReplication> GetReplicationAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetReplicationInfo", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetReplicationInfoResponse")]
        SolidCP.Providers.Virtualization.ReplicationDetailInfo GetReplicationInfo(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetReplicationInfo", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/GetReplicationInfoResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReplicationDetailInfo> GetReplicationInfoAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/SetVmReplication", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/SetVmReplicationResponse")]
        SolidCP.Providers.Common.ResultObject SetVmReplication(int itemId, SolidCP.Providers.Virtualization.VmReplication replication);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/SetVmReplication", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/SetVmReplicationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetVmReplicationAsync(int itemId, SolidCP.Providers.Virtualization.VmReplication replication);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/DisableVmReplication", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/DisableVmReplicationResponse")]
        SolidCP.Providers.Common.ResultObject DisableVmReplication(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/DisableVmReplication", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/DisableVmReplicationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DisableVmReplicationAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/PauseReplication", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/PauseReplicationResponse")]
        SolidCP.Providers.Common.ResultObject PauseReplication(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/PauseReplication", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/PauseReplicationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> PauseReplicationAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/ResumeReplication", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/ResumeReplicationResponse")]
        SolidCP.Providers.Common.ResultObject ResumeReplication(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/ResumeReplication", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServerProxmox/ResumeReplicationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ResumeReplicationAsync(int itemId);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esVirtualizationServerProxmoxAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesVirtualizationServerProxmox
    {
        public SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged GetVirtualMachines(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            return Invoke<SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetVirtualMachines", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged> GetVirtualMachinesAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetVirtualMachines", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows, recursive);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine[] GetVirtualMachinesByServiceId(int serviceId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine[]>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetVirtualMachinesByServiceId", serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine[]> GetVirtualMachinesByServiceIdAsync(int serviceId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine[]>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetVirtualMachinesByServiceId", serviceId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineItem(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetVirtualMachineItem", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineItemAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetVirtualMachineItem", itemId);
        }

        public string EvaluateVirtualMachineTemplate(int itemId, string template)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "EvaluateVirtualMachineTemplate", itemId, template);
        }

        public async System.Threading.Tasks.Task<string> EvaluateVirtualMachineTemplateAsync(int itemId, string template)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "EvaluateVirtualMachineTemplate", itemId, template);
        }

        public int GetExternalNetworkVLAN(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetExternalNetworkVLAN", itemId);
        }

        public async System.Threading.Tasks.Task<int> GetExternalNetworkVLANAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetExternalNetworkVLAN", itemId);
        }

        public SolidCP.EnterpriseServer.NetworkAdapterDetails GetExternalNetworkDetails(int packageId)
        {
            return Invoke<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetExternalNetworkDetails", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetExternalNetworkDetailsAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetExternalNetworkDetails", packageId);
        }

        public SolidCP.EnterpriseServer.PrivateIPAddressesPaged GetPackagePrivateIPAddressesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.EnterpriseServer.PrivateIPAddressesPaged>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetPackagePrivateIPAddressesPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PrivateIPAddressesPaged> GetPackagePrivateIPAddressesPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PrivateIPAddressesPaged>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetPackagePrivateIPAddressesPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.EnterpriseServer.PrivateIPAddress[] /*List*/ GetPackagePrivateIPAddresses(int packageId)
        {
            return Invoke<SolidCP.EnterpriseServer.PrivateIPAddress[], SolidCP.EnterpriseServer.PrivateIPAddress>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetPackagePrivateIPAddresses", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PrivateIPAddress[]> GetPackagePrivateIPAddressesAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PrivateIPAddress[], SolidCP.EnterpriseServer.PrivateIPAddress>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetPackagePrivateIPAddresses", packageId);
        }

        public SolidCP.EnterpriseServer.NetworkAdapterDetails GetPrivateNetworkDetails(int packageId)
        {
            return Invoke<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetPrivateNetworkDetails", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetPrivateNetworkDetailsAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetPrivateNetworkDetails", packageId);
        }

        public SolidCP.EnterpriseServer.VirtualMachinePermission[] /*List*/ GetSpaceUserPermissions(int packageId)
        {
            return Invoke<SolidCP.EnterpriseServer.VirtualMachinePermission[], SolidCP.EnterpriseServer.VirtualMachinePermission>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetSpaceUserPermissions", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachinePermission[]> GetSpaceUserPermissionsAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.VirtualMachinePermission[], SolidCP.EnterpriseServer.VirtualMachinePermission>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetSpaceUserPermissions", packageId);
        }

        public int UpdateSpaceUserPermissions(int packageId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "UpdateSpaceUserPermissions", packageId, permissions);
        }

        public async System.Threading.Tasks.Task<int> UpdateSpaceUserPermissionsAsync(int packageId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "UpdateSpaceUserPermissions", packageId, permissions);
        }

        public SolidCP.EnterpriseServer.LogRecord[] /*List*/ GetSpaceAuditLog(int packageId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.EnterpriseServer.LogRecord[], SolidCP.EnterpriseServer.LogRecord>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetSpaceAuditLog", packageId, startPeriod, endPeriod, severity, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.LogRecord[]> GetSpaceAuditLogAsync(int packageId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.LogRecord[], SolidCP.EnterpriseServer.LogRecord>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetSpaceAuditLog", packageId, startPeriod, endPeriod, severity, sortColumn, startRow, maximumRows);
        }

        public SolidCP.EnterpriseServer.LogRecord[] /*List*/ GetVirtualMachineAuditLog(int itemId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.EnterpriseServer.LogRecord[], SolidCP.EnterpriseServer.LogRecord>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetVirtualMachineAuditLog", itemId, startPeriod, endPeriod, severity, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.LogRecord[]> GetVirtualMachineAuditLogAsync(int itemId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.LogRecord[], SolidCP.EnterpriseServer.LogRecord>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetVirtualMachineAuditLog", itemId, startPeriod, endPeriod, severity, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetOperatingSystemTemplates(int packageId)
        {
            return Invoke<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetOperatingSystemTemplates", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetOperatingSystemTemplatesAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetOperatingSystemTemplates", packageId);
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetOperatingSystemTemplatesByServiceId(int serviceId)
        {
            return Invoke<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetOperatingSystemTemplatesByServiceId", serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetOperatingSystemTemplatesByServiceIdAsync(int serviceId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetOperatingSystemTemplatesByServiceId", serviceId);
        }

        public int GetMaximumCpuCoresNumber(int packageId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetMaximumCpuCoresNumber", packageId);
        }

        public async System.Threading.Tasks.Task<int> GetMaximumCpuCoresNumberAsync(int packageId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetMaximumCpuCoresNumber", packageId);
        }

        public string GetDefaultExportPath(int itemId)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetDefaultExportPath", itemId);
        }

        public async System.Threading.Tasks.Task<string> GetDefaultExportPathAsync(int itemId)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetDefaultExportPath", itemId);
        }

        public SolidCP.Providers.ResultObjects.IntResult CreateDefaultVirtualMachine(int packageId, string hostname, string osTemplate, string password, string summaryLetterEmail)
        {
            return Invoke<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "CreateDefaultVirtualMachine", packageId, hostname, osTemplate, password, summaryLetterEmail);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> CreateDefaultVirtualMachineAsync(int packageId, string hostname, string osTemplate, string password, string summaryLetterEmail)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "CreateDefaultVirtualMachine", packageId, hostname, osTemplate, password, summaryLetterEmail);
        }

        public SolidCP.Providers.ResultObjects.IntResult CreateVirtualMachine(int packageId, string hostname, string osTemplateFile, string password, string summaryLetterEmail, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses, bool privateNetworkEnabled, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses, SolidCP.Providers.Virtualization.VirtualMachine otherSettings)
        {
            return Invoke<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "CreateVirtualMachine", packageId, hostname, osTemplateFile, password, summaryLetterEmail, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, externalAddressesNumber, randomExternalAddresses, externalAddresses, privateNetworkEnabled, privateAddressesNumber, randomPrivateAddresses, privateAddresses, otherSettings);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> CreateVirtualMachineAsync(int packageId, string hostname, string osTemplateFile, string password, string summaryLetterEmail, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses, bool privateNetworkEnabled, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses, SolidCP.Providers.Virtualization.VirtualMachine otherSettings)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "CreateVirtualMachine", packageId, hostname, osTemplateFile, password, summaryLetterEmail, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, externalAddressesNumber, randomExternalAddresses, externalAddresses, privateNetworkEnabled, privateAddressesNumber, randomPrivateAddresses, privateAddresses, otherSettings);
        }

        public SolidCP.Providers.ResultObjects.IntResult ImportVirtualMachine(int packageId, int serviceId, string vmId, string osTemplateFile, string adminPassword, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, string externalNicMacAddress, int[] externalAddresses, string managementNicMacAddress, int managementAddress)
        {
            return Invoke<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "ImportVirtualMachine", packageId, serviceId, vmId, osTemplateFile, adminPassword, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNicMacAddress, externalAddresses, managementNicMacAddress, managementAddress);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> ImportVirtualMachineAsync(int packageId, int serviceId, string vmId, string osTemplateFile, string adminPassword, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, string externalNicMacAddress, int[] externalAddresses, string managementNicMacAddress, int managementAddress)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "ImportVirtualMachine", packageId, serviceId, vmId, osTemplateFile, adminPassword, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNicMacAddress, externalAddresses, managementNicMacAddress, managementAddress);
        }

        public byte[] GetVirtualMachineThumbnail(int itemId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return Invoke<byte[]>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetVirtualMachineThumbnail", itemId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailAsync(int itemId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return await InvokeAsync<byte[]>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetVirtualMachineThumbnail", itemId, size);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineGeneralDetails(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetVirtualMachineGeneralDetails", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineGeneralDetailsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetVirtualMachineGeneralDetails", itemId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineExtendedInfo(int serviceId, string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetVirtualMachineExtendedInfo", serviceId, vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineExtendedInfoAsync(int serviceId, string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetVirtualMachineExtendedInfo", serviceId, vmId);
        }

        public int CancelVirtualMachineJob(string jobId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "CancelVirtualMachineJob", jobId);
        }

        public async System.Threading.Tasks.Task<int> CancelVirtualMachineJobAsync(string jobId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "CancelVirtualMachineJob", jobId);
        }

        public SolidCP.Providers.Common.ResultObject UpdateVirtualMachineHostName(int itemId, string hostname, bool updateNetBIOS)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "UpdateVirtualMachineHostName", itemId, hostname, updateNetBIOS);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateVirtualMachineHostNameAsync(int itemId, string hostname, bool updateNetBIOS)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "UpdateVirtualMachineHostName", itemId, hostname, updateNetBIOS);
        }

        public SolidCP.Providers.Common.ResultObject ChangeVirtualMachineState(int itemId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState state)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "ChangeVirtualMachineState", itemId, state);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeVirtualMachineStateAsync(int itemId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState state)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "ChangeVirtualMachineState", itemId, state);
        }

        public SolidCP.Providers.Virtualization.ConcreteJob[] /*List*/ GetVirtualMachineJobs(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.ConcreteJob[], SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetVirtualMachineJobs", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob[]> GetVirtualMachineJobsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ConcreteJob[], SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetVirtualMachineJobs", itemId);
        }

        public string GetVirtualMachineVNCURL(int itemId)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetVirtualMachineVNCURL", itemId);
        }

        public async System.Threading.Tasks.Task<string> GetVirtualMachineVNCURLAsync(int itemId)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetVirtualMachineVNCURL", itemId);
        }

        public SolidCP.Providers.Common.ResultObject ChangeAdministratorPassword(int itemId, string password)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "ChangeAdministratorPassword", itemId, password);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeAdministratorPasswordAsync(int itemId, string password)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "ChangeAdministratorPassword", itemId, password);
        }

        public SolidCP.Providers.Common.ResultObject UpdateVirtualMachineConfiguration(int itemId, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, bool privateNetworkEnabled, SolidCP.Providers.Virtualization.VirtualMachine otherSettings)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "UpdateVirtualMachineConfiguration", itemId, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, privateNetworkEnabled, otherSettings);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateVirtualMachineConfigurationAsync(int itemId, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, bool privateNetworkEnabled, SolidCP.Providers.Virtualization.VirtualMachine otherSettings)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "UpdateVirtualMachineConfiguration", itemId, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, privateNetworkEnabled, otherSettings);
        }

        public SolidCP.Providers.Virtualization.LibraryItem GetInsertedDvdDisk(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.LibraryItem>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetInsertedDvdDisk", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem> GetInsertedDvdDiskAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.LibraryItem>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetInsertedDvdDisk", itemId);
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetLibraryDisks(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetLibraryDisks", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetLibraryDisksAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetLibraryDisks", itemId);
        }

        public SolidCP.Providers.Common.ResultObject InsertDvdDisk(int itemId, string isoPath)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "InsertDvdDisk", itemId, isoPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> InsertDvdDiskAsync(int itemId, string isoPath)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "InsertDvdDisk", itemId, isoPath);
        }

        public SolidCP.Providers.Common.ResultObject EjectDvdDisk(int itemId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "EjectDvdDisk", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> EjectDvdDiskAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "EjectDvdDisk", itemId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachineSnapshot[] GetVirtualMachineSnapshots(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetVirtualMachineSnapshots", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]> GetVirtualMachineSnapshotsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetVirtualMachineSnapshots", itemId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachineSnapshot GetSnapshot(int itemId, string snaphostId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachineSnapshot>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetSnapshot", itemId, snaphostId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot> GetSnapshotAsync(int itemId, string snaphostId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachineSnapshot>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetSnapshot", itemId, snaphostId);
        }

        public SolidCP.Providers.Common.ResultObject CreateSnapshot(int itemId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "CreateSnapshot", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateSnapshotAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "CreateSnapshot", itemId);
        }

        public SolidCP.Providers.Common.ResultObject ApplySnapshot(int itemId, string snapshotId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "ApplySnapshot", itemId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ApplySnapshotAsync(int itemId, string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "ApplySnapshot", itemId, snapshotId);
        }

        public SolidCP.Providers.Common.ResultObject RenameSnapshot(int itemId, string snapshotId, string newName)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "RenameSnapshot", itemId, snapshotId, newName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RenameSnapshotAsync(int itemId, string snapshotId, string newName)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "RenameSnapshot", itemId, snapshotId, newName);
        }

        public SolidCP.Providers.Common.ResultObject DeleteSnapshot(int itemId, string snapshotId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "DeleteSnapshot", itemId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteSnapshotAsync(int itemId, string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "DeleteSnapshot", itemId, snapshotId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteSnapshotSubtree(int itemId, string snapshotId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "DeleteSnapshotSubtree", itemId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteSnapshotSubtreeAsync(int itemId, string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "DeleteSnapshotSubtree", itemId, snapshotId);
        }

        public byte[] GetSnapshotThumbnail(int itemId, string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return Invoke<byte[]>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetSnapshotThumbnail", itemId, snapshotId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailAsync(int itemId, string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return await InvokeAsync<byte[]>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetSnapshotThumbnail", itemId, snapshotId, size);
        }

        public SolidCP.EnterpriseServer.NetworkAdapterDetails GetExternalNetworkAdapterDetails(int itemId)
        {
            return Invoke<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetExternalNetworkAdapterDetails", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetExternalNetworkAdapterDetailsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetExternalNetworkAdapterDetails", itemId);
        }

        public SolidCP.Providers.Common.ResultObject AddVirtualMachineExternalIPAddresses(int itemId, bool selectRandom, int addressesNumber, int[] addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "AddVirtualMachineExternalIPAddresses", itemId, selectRandom, addressesNumber, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachineExternalIPAddressesAsync(int itemId, bool selectRandom, int addressesNumber, int[] addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "AddVirtualMachineExternalIPAddresses", itemId, selectRandom, addressesNumber, addressId);
        }

        public SolidCP.Providers.Common.ResultObject SetVirtualMachinePrimaryExternalIPAddress(int itemId, int addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "SetVirtualMachinePrimaryExternalIPAddress", itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetVirtualMachinePrimaryExternalIPAddressAsync(int itemId, int addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "SetVirtualMachinePrimaryExternalIPAddress", itemId, addressId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteVirtualMachineExternalIPAddresses(int itemId, int[] addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "DeleteVirtualMachineExternalIPAddresses", itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineExternalIPAddressesAsync(int itemId, int[] addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "DeleteVirtualMachineExternalIPAddresses", itemId, addressId);
        }

        public SolidCP.EnterpriseServer.NetworkAdapterDetails GetPrivateNetworkAdapterDetails(int itemId)
        {
            return Invoke<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetPrivateNetworkAdapterDetails", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetPrivateNetworkAdapterDetailsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetPrivateNetworkAdapterDetails", itemId);
        }

        public SolidCP.Providers.Common.ResultObject AddVirtualMachinePrivateIPAddresses(int itemId, bool selectRandom, int addressesNumber, string[] addresses)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "AddVirtualMachinePrivateIPAddresses", itemId, selectRandom, addressesNumber, addresses);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachinePrivateIPAddressesAsync(int itemId, bool selectRandom, int addressesNumber, string[] addresses)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "AddVirtualMachinePrivateIPAddresses", itemId, selectRandom, addressesNumber, addresses);
        }

        public SolidCP.Providers.Common.ResultObject SetVirtualMachinePrimaryPrivateIPAddress(int itemId, int addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "SetVirtualMachinePrimaryPrivateIPAddress", itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetVirtualMachinePrimaryPrivateIPAddressAsync(int itemId, int addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "SetVirtualMachinePrimaryPrivateIPAddress", itemId, addressId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteVirtualMachinePrivateIPAddresses(int itemId, int[] addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "DeleteVirtualMachinePrivateIPAddresses", itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachinePrivateIPAddressesAsync(int itemId, int[] addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "DeleteVirtualMachinePrivateIPAddresses", itemId, addressId);
        }

        public SolidCP.EnterpriseServer.VirtualMachinePermission[] /*List*/ GetVirtualMachinePermissions(int itemId)
        {
            return Invoke<SolidCP.EnterpriseServer.VirtualMachinePermission[], SolidCP.EnterpriseServer.VirtualMachinePermission>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetVirtualMachinePermissions", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachinePermission[]> GetVirtualMachinePermissionsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.VirtualMachinePermission[], SolidCP.EnterpriseServer.VirtualMachinePermission>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetVirtualMachinePermissions", itemId);
        }

        public int UpdateVirtualMachineUserPermissions(int itemId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "UpdateVirtualMachineUserPermissions", itemId, permissions);
        }

        public async System.Threading.Tasks.Task<int> UpdateVirtualMachineUserPermissionsAsync(int itemId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "UpdateVirtualMachineUserPermissions", itemId, permissions);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch[] GetExternalSwitches(int serviceId, string computerName)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualSwitch[]>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetExternalSwitches", serviceId, computerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetExternalSwitchesAsync(int serviceId, string computerName)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualSwitch[]>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetExternalSwitches", serviceId, computerName);
        }

        public SolidCP.Providers.Common.ResultObject DeleteVirtualMachine(int itemId, bool saveFiles, bool exportVps, string exportPath)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "DeleteVirtualMachine", itemId, saveFiles, exportVps, exportPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineAsync(int itemId, bool saveFiles, bool exportVps, string exportPath)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "DeleteVirtualMachine", itemId, saveFiles, exportVps, exportPath);
        }

        public int ReinstallVirtualMachine(int itemId, string adminPassword, bool preserveVirtualDiskFiles, bool saveVirtualDisk, bool exportVps, string exportPath)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "ReinstallVirtualMachine", itemId, adminPassword, preserveVirtualDiskFiles, saveVirtualDisk, exportVps, exportPath);
        }

        public async System.Threading.Tasks.Task<int> ReinstallVirtualMachineAsync(int itemId, string adminPassword, bool preserveVirtualDiskFiles, bool saveVirtualDisk, bool exportVps, string exportPath)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "ReinstallVirtualMachine", itemId, adminPassword, preserveVirtualDiskFiles, saveVirtualDisk, exportVps, exportPath);
        }

        public string GetVirtualMachineSummaryText(int itemId)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetVirtualMachineSummaryText", itemId);
        }

        public async System.Threading.Tasks.Task<string> GetVirtualMachineSummaryTextAsync(int itemId)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetVirtualMachineSummaryText", itemId);
        }

        public SolidCP.Providers.Common.ResultObject SendVirtualMachineSummaryLetter(int itemId, string to, string bcc)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "SendVirtualMachineSummaryLetter", itemId, to, bcc);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SendVirtualMachineSummaryLetterAsync(int itemId, string to, string bcc)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "SendVirtualMachineSummaryLetter", itemId, to, bcc);
        }

        public SolidCP.Providers.Virtualization.CertificateInfo[] GetCertificates(int serviceId, string remoteServer)
        {
            return Invoke<SolidCP.Providers.Virtualization.CertificateInfo[]>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetCertificates", serviceId, remoteServer);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.CertificateInfo[]> GetCertificatesAsync(int serviceId, string remoteServer)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.CertificateInfo[]>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetCertificates", serviceId, remoteServer);
        }

        public SolidCP.Providers.Common.ResultObject SetReplicaServer(int serviceId, string remoteServer, string thumbprint, string storagePath)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "SetReplicaServer", serviceId, remoteServer, thumbprint, storagePath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetReplicaServerAsync(int serviceId, string remoteServer, string thumbprint, string storagePath)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "SetReplicaServer", serviceId, remoteServer, thumbprint, storagePath);
        }

        public SolidCP.Providers.Common.ResultObject UnsetReplicaServer(int serviceId, string remoteServer)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "UnsetReplicaServer", serviceId, remoteServer);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UnsetReplicaServerAsync(int serviceId, string remoteServer)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "UnsetReplicaServer", serviceId, remoteServer);
        }

        public SolidCP.Providers.Virtualization.ReplicationServerInfo GetReplicaServer(int serviceId, string remoteServer)
        {
            return Invoke<SolidCP.Providers.Virtualization.ReplicationServerInfo>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetReplicaServer", serviceId, remoteServer);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReplicationServerInfo> GetReplicaServerAsync(int serviceId, string remoteServer)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ReplicationServerInfo>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetReplicaServer", serviceId, remoteServer);
        }

        public SolidCP.Providers.Virtualization.VmReplication GetReplication(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VmReplication>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetReplication", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VmReplication> GetReplicationAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VmReplication>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetReplication", itemId);
        }

        public SolidCP.Providers.Virtualization.ReplicationDetailInfo GetReplicationInfo(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.ReplicationDetailInfo>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetReplicationInfo", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReplicationDetailInfo> GetReplicationInfoAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ReplicationDetailInfo>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "GetReplicationInfo", itemId);
        }

        public SolidCP.Providers.Common.ResultObject SetVmReplication(int itemId, SolidCP.Providers.Virtualization.VmReplication replication)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "SetVmReplication", itemId, replication);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetVmReplicationAsync(int itemId, SolidCP.Providers.Virtualization.VmReplication replication)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "SetVmReplication", itemId, replication);
        }

        public SolidCP.Providers.Common.ResultObject DisableVmReplication(int itemId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "DisableVmReplication", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DisableVmReplicationAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "DisableVmReplication", itemId);
        }

        public SolidCP.Providers.Common.ResultObject PauseReplication(int itemId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "PauseReplication", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> PauseReplicationAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "PauseReplication", itemId);
        }

        public SolidCP.Providers.Common.ResultObject ResumeReplication(int itemId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "ResumeReplication", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ResumeReplicationAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServerProxmox", "ResumeReplication", itemId);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esVirtualizationServerProxmox : SolidCP.Web.Client.ClientBase<IesVirtualizationServerProxmox, esVirtualizationServerProxmoxAssemblyClient>, IesVirtualizationServerProxmox
    {
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

        public SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineItem(int itemId)
        {
            return base.Client.GetVirtualMachineItem(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineItemAsync(int itemId)
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

        public int GetExternalNetworkVLAN(int itemId)
        {
            return base.Client.GetExternalNetworkVLAN(itemId);
        }

        public async System.Threading.Tasks.Task<int> GetExternalNetworkVLANAsync(int itemId)
        {
            return await base.Client.GetExternalNetworkVLANAsync(itemId);
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

        public SolidCP.Providers.Virtualization.LibraryItem[] GetOperatingSystemTemplates(int packageId)
        {
            return base.Client.GetOperatingSystemTemplates(packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetOperatingSystemTemplatesAsync(int packageId)
        {
            return await base.Client.GetOperatingSystemTemplatesAsync(packageId);
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetOperatingSystemTemplatesByServiceId(int serviceId)
        {
            return base.Client.GetOperatingSystemTemplatesByServiceId(serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetOperatingSystemTemplatesByServiceIdAsync(int serviceId)
        {
            return await base.Client.GetOperatingSystemTemplatesByServiceIdAsync(serviceId);
        }

        public int GetMaximumCpuCoresNumber(int packageId)
        {
            return base.Client.GetMaximumCpuCoresNumber(packageId);
        }

        public async System.Threading.Tasks.Task<int> GetMaximumCpuCoresNumberAsync(int packageId)
        {
            return await base.Client.GetMaximumCpuCoresNumberAsync(packageId);
        }

        public string GetDefaultExportPath(int itemId)
        {
            return base.Client.GetDefaultExportPath(itemId);
        }

        public async System.Threading.Tasks.Task<string> GetDefaultExportPathAsync(int itemId)
        {
            return await base.Client.GetDefaultExportPathAsync(itemId);
        }

        public SolidCP.Providers.ResultObjects.IntResult CreateDefaultVirtualMachine(int packageId, string hostname, string osTemplate, string password, string summaryLetterEmail)
        {
            return base.Client.CreateDefaultVirtualMachine(packageId, hostname, osTemplate, password, summaryLetterEmail);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> CreateDefaultVirtualMachineAsync(int packageId, string hostname, string osTemplate, string password, string summaryLetterEmail)
        {
            return await base.Client.CreateDefaultVirtualMachineAsync(packageId, hostname, osTemplate, password, summaryLetterEmail);
        }

        public SolidCP.Providers.ResultObjects.IntResult CreateVirtualMachine(int packageId, string hostname, string osTemplateFile, string password, string summaryLetterEmail, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses, bool privateNetworkEnabled, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses, SolidCP.Providers.Virtualization.VirtualMachine otherSettings)
        {
            return base.Client.CreateVirtualMachine(packageId, hostname, osTemplateFile, password, summaryLetterEmail, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, externalAddressesNumber, randomExternalAddresses, externalAddresses, privateNetworkEnabled, privateAddressesNumber, randomPrivateAddresses, privateAddresses, otherSettings);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> CreateVirtualMachineAsync(int packageId, string hostname, string osTemplateFile, string password, string summaryLetterEmail, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses, bool privateNetworkEnabled, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses, SolidCP.Providers.Virtualization.VirtualMachine otherSettings)
        {
            return await base.Client.CreateVirtualMachineAsync(packageId, hostname, osTemplateFile, password, summaryLetterEmail, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, externalAddressesNumber, randomExternalAddresses, externalAddresses, privateNetworkEnabled, privateAddressesNumber, randomPrivateAddresses, privateAddresses, otherSettings);
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

        public SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineGeneralDetails(int itemId)
        {
            return base.Client.GetVirtualMachineGeneralDetails(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineGeneralDetailsAsync(int itemId)
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

        public string GetVirtualMachineVNCURL(int itemId)
        {
            return base.Client.GetVirtualMachineVNCURL(itemId);
        }

        public async System.Threading.Tasks.Task<string> GetVirtualMachineVNCURLAsync(int itemId)
        {
            return await base.Client.GetVirtualMachineVNCURLAsync(itemId);
        }

        public SolidCP.Providers.Common.ResultObject ChangeAdministratorPassword(int itemId, string password)
        {
            return base.Client.ChangeAdministratorPassword(itemId, password);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeAdministratorPasswordAsync(int itemId, string password)
        {
            return await base.Client.ChangeAdministratorPasswordAsync(itemId, password);
        }

        public SolidCP.Providers.Common.ResultObject UpdateVirtualMachineConfiguration(int itemId, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, bool privateNetworkEnabled, SolidCP.Providers.Virtualization.VirtualMachine otherSettings)
        {
            return base.Client.UpdateVirtualMachineConfiguration(itemId, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, privateNetworkEnabled, otherSettings);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateVirtualMachineConfigurationAsync(int itemId, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, bool privateNetworkEnabled, SolidCP.Providers.Virtualization.VirtualMachine otherSettings)
        {
            return await base.Client.UpdateVirtualMachineConfigurationAsync(itemId, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, privateNetworkEnabled, otherSettings);
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

        public SolidCP.Providers.Virtualization.CertificateInfo[] GetCertificates(int serviceId, string remoteServer)
        {
            return base.Client.GetCertificates(serviceId, remoteServer);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.CertificateInfo[]> GetCertificatesAsync(int serviceId, string remoteServer)
        {
            return await base.Client.GetCertificatesAsync(serviceId, remoteServer);
        }

        public SolidCP.Providers.Common.ResultObject SetReplicaServer(int serviceId, string remoteServer, string thumbprint, string storagePath)
        {
            return base.Client.SetReplicaServer(serviceId, remoteServer, thumbprint, storagePath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetReplicaServerAsync(int serviceId, string remoteServer, string thumbprint, string storagePath)
        {
            return await base.Client.SetReplicaServerAsync(serviceId, remoteServer, thumbprint, storagePath);
        }

        public SolidCP.Providers.Common.ResultObject UnsetReplicaServer(int serviceId, string remoteServer)
        {
            return base.Client.UnsetReplicaServer(serviceId, remoteServer);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UnsetReplicaServerAsync(int serviceId, string remoteServer)
        {
            return await base.Client.UnsetReplicaServerAsync(serviceId, remoteServer);
        }

        public SolidCP.Providers.Virtualization.ReplicationServerInfo GetReplicaServer(int serviceId, string remoteServer)
        {
            return base.Client.GetReplicaServer(serviceId, remoteServer);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReplicationServerInfo> GetReplicaServerAsync(int serviceId, string remoteServer)
        {
            return await base.Client.GetReplicaServerAsync(serviceId, remoteServer);
        }

        public SolidCP.Providers.Virtualization.VmReplication GetReplication(int itemId)
        {
            return base.Client.GetReplication(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VmReplication> GetReplicationAsync(int itemId)
        {
            return await base.Client.GetReplicationAsync(itemId);
        }

        public SolidCP.Providers.Virtualization.ReplicationDetailInfo GetReplicationInfo(int itemId)
        {
            return base.Client.GetReplicationInfo(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReplicationDetailInfo> GetReplicationInfoAsync(int itemId)
        {
            return await base.Client.GetReplicationInfoAsync(itemId);
        }

        public SolidCP.Providers.Common.ResultObject SetVmReplication(int itemId, SolidCP.Providers.Virtualization.VmReplication replication)
        {
            return base.Client.SetVmReplication(itemId, replication);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetVmReplicationAsync(int itemId, SolidCP.Providers.Virtualization.VmReplication replication)
        {
            return await base.Client.SetVmReplicationAsync(itemId, replication);
        }

        public SolidCP.Providers.Common.ResultObject DisableVmReplication(int itemId)
        {
            return base.Client.DisableVmReplication(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DisableVmReplicationAsync(int itemId)
        {
            return await base.Client.DisableVmReplicationAsync(itemId);
        }

        public SolidCP.Providers.Common.ResultObject PauseReplication(int itemId)
        {
            return base.Client.PauseReplication(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> PauseReplicationAsync(int itemId)
        {
            return await base.Client.PauseReplicationAsync(itemId);
        }

        public SolidCP.Providers.Common.ResultObject ResumeReplication(int itemId)
        {
            return base.Client.ResumeReplication(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ResumeReplicationAsync(int itemId)
        {
            return await base.Client.ResumeReplicationAsync(itemId);
        }
    }
}
#endif