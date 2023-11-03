#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesVirtualizationServer", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesVirtualizationServer
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachines", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachinesResponse")]
        SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged GetVirtualMachines(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachines", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachinesResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged> GetVirtualMachinesAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachinesByServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachinesByServiceIdResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine[] GetVirtualMachinesByServiceId(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachinesByServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachinesByServiceIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine[]> GetVirtualMachinesByServiceIdAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineItem", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineItemResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineItem(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineItem", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineItemResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineItemAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/EvaluateVirtualMachineTemplate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/EvaluateVirtualMachineTemplateResponse")]
        string EvaluateVirtualMachineTemplate(int itemId, string template);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/EvaluateVirtualMachineTemplate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/EvaluateVirtualMachineTemplateResponse")]
        System.Threading.Tasks.Task<string> EvaluateVirtualMachineTemplateAsync(int itemId, string template);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetExternalNetworkDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetExternalNetworkDetailsResponse")]
        SolidCP.EnterpriseServer.NetworkAdapterDetails GetExternalNetworkDetails(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetExternalNetworkDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetExternalNetworkDetailsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetExternalNetworkDetailsAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetPackagePrivateIPAddressesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetPackagePrivateIPAddressesPagedResponse")]
        SolidCP.EnterpriseServer.PrivateIPAddressesPaged GetPackagePrivateIPAddressesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetPackagePrivateIPAddressesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetPackagePrivateIPAddressesPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PrivateIPAddressesPaged> GetPackagePrivateIPAddressesPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetPackagePrivateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetPackagePrivateIPAddressesResponse")]
        SolidCP.EnterpriseServer.PrivateIPAddress[] /*List*/ GetPackagePrivateIPAddresses(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetPackagePrivateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetPackagePrivateIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PrivateIPAddress[]> GetPackagePrivateIPAddressesAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetPrivateNetworkDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetPrivateNetworkDetailsResponse")]
        SolidCP.EnterpriseServer.NetworkAdapterDetails GetPrivateNetworkDetails(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetPrivateNetworkDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetPrivateNetworkDetailsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetPrivateNetworkDetailsAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetSpaceUserPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetSpaceUserPermissionsResponse")]
        SolidCP.EnterpriseServer.VirtualMachinePermission[] /*List*/ GetSpaceUserPermissions(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetSpaceUserPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetSpaceUserPermissionsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachinePermission[]> GetSpaceUserPermissionsAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/UpdateSpaceUserPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/UpdateSpaceUserPermissionsResponse")]
        int UpdateSpaceUserPermissions(int packageId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/UpdateSpaceUserPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/UpdateSpaceUserPermissionsResponse")]
        System.Threading.Tasks.Task<int> UpdateSpaceUserPermissionsAsync(int packageId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetSpaceAuditLog", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetSpaceAuditLogResponse")]
        SolidCP.EnterpriseServer.LogRecord[] /*List*/ GetSpaceAuditLog(int packageId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetSpaceAuditLog", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetSpaceAuditLogResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.LogRecord[]> GetSpaceAuditLogAsync(int packageId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineAuditLog", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineAuditLogResponse")]
        SolidCP.EnterpriseServer.LogRecord[] /*List*/ GetVirtualMachineAuditLog(int itemId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineAuditLog", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineAuditLogResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.LogRecord[]> GetVirtualMachineAuditLogAsync(int itemId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetOperatingSystemTemplates", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetOperatingSystemTemplatesResponse")]
        SolidCP.Providers.Virtualization.LibraryItem[] GetOperatingSystemTemplates(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetOperatingSystemTemplates", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetOperatingSystemTemplatesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetOperatingSystemTemplatesAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetOperatingSystemTemplatesByServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetOperatingSystemTemplatesByServiceIdResponse")]
        SolidCP.Providers.Virtualization.LibraryItem[] GetOperatingSystemTemplatesByServiceId(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetOperatingSystemTemplatesByServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetOperatingSystemTemplatesByServiceIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetOperatingSystemTemplatesByServiceIdAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetMaximumCpuCoresNumber", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetMaximumCpuCoresNumberResponse")]
        int GetMaximumCpuCoresNumber(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetMaximumCpuCoresNumber", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetMaximumCpuCoresNumberResponse")]
        System.Threading.Tasks.Task<int> GetMaximumCpuCoresNumberAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetDefaultExportPath", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetDefaultExportPathResponse")]
        string GetDefaultExportPath(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetDefaultExportPath", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetDefaultExportPathResponse")]
        System.Threading.Tasks.Task<string> GetDefaultExportPathAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/CreateDefaultVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/CreateDefaultVirtualMachineResponse")]
        SolidCP.Providers.ResultObjects.IntResult CreateDefaultVirtualMachine(int packageId, string hostname, string osTemplate, string password, string summaryLetterEmail);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/CreateDefaultVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/CreateDefaultVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> CreateDefaultVirtualMachineAsync(int packageId, string hostname, string osTemplate, string password, string summaryLetterEmail);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/CreateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/CreateVirtualMachineResponse")]
        SolidCP.Providers.ResultObjects.IntResult CreateVirtualMachine(int packageId, string hostname, string osTemplateFile, string password, string summaryLetterEmail, int generation, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses, bool privateNetworkEnabled, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/CreateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/CreateVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> CreateVirtualMachineAsync(int packageId, string hostname, string osTemplateFile, string password, string summaryLetterEmail, int generation, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses, bool privateNetworkEnabled, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/ImportVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/ImportVirtualMachineResponse")]
        SolidCP.Providers.ResultObjects.IntResult ImportVirtualMachine(int packageId, int serviceId, string vmId, string osTemplateFile, string adminPassword, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, string externalNicMacAddress, int[] externalAddresses, string managementNicMacAddress, int managementAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/ImportVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/ImportVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> ImportVirtualMachineAsync(int packageId, int serviceId, string vmId, string osTemplateFile, string adminPassword, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, string externalNicMacAddress, int[] externalAddresses, string managementNicMacAddress, int managementAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineThumbnail", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineThumbnailResponse")]
        byte[] GetVirtualMachineThumbnail(int itemId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineThumbnail", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineThumbnailResponse")]
        System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailAsync(int itemId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineGeneralDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineGeneralDetailsResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineGeneralDetails(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineGeneralDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineGeneralDetailsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineGeneralDetailsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineExtendedInfo", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineExtendedInfoResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineExtendedInfo(int serviceId, string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineExtendedInfo", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineExtendedInfoResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineExtendedInfoAsync(int serviceId, string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/CancelVirtualMachineJob", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/CancelVirtualMachineJobResponse")]
        int CancelVirtualMachineJob(string jobId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/CancelVirtualMachineJob", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/CancelVirtualMachineJobResponse")]
        System.Threading.Tasks.Task<int> CancelVirtualMachineJobAsync(string jobId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/UpdateVirtualMachineHostName", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/UpdateVirtualMachineHostNameResponse")]
        SolidCP.Providers.Common.ResultObject UpdateVirtualMachineHostName(int itemId, string hostname, bool updateNetBIOS);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/UpdateVirtualMachineHostName", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/UpdateVirtualMachineHostNameResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateVirtualMachineHostNameAsync(int itemId, string hostname, bool updateNetBIOS);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/ChangeVirtualMachineState", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/ChangeVirtualMachineStateResponse")]
        SolidCP.Providers.Common.ResultObject ChangeVirtualMachineState(int itemId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState state);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/ChangeVirtualMachineState", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/ChangeVirtualMachineStateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeVirtualMachineStateAsync(int itemId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState state);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineJobs", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineJobsResponse")]
        SolidCP.Providers.Virtualization.ConcreteJob[] /*List*/ GetVirtualMachineJobs(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineJobs", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineJobsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob[]> GetVirtualMachineJobsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/ChangeAdministratorPassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/ChangeAdministratorPasswordResponse")]
        SolidCP.Providers.Common.ResultObject ChangeAdministratorPassword(int itemId, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/ChangeAdministratorPassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/ChangeAdministratorPasswordResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeAdministratorPasswordAsync(int itemId, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/UpdateVirtualMachineConfiguration", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/UpdateVirtualMachineConfigurationResponse")]
        SolidCP.Providers.Common.ResultObject UpdateVirtualMachineConfiguration(int itemId, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, bool privateNetworkEnabled);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/UpdateVirtualMachineConfiguration", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/UpdateVirtualMachineConfigurationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateVirtualMachineConfigurationAsync(int itemId, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, bool privateNetworkEnabled);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetInsertedDvdDisk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetInsertedDvdDiskResponse")]
        SolidCP.Providers.Virtualization.LibraryItem GetInsertedDvdDisk(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetInsertedDvdDisk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetInsertedDvdDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem> GetInsertedDvdDiskAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetLibraryDisks", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetLibraryDisksResponse")]
        SolidCP.Providers.Virtualization.LibraryItem[] GetLibraryDisks(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetLibraryDisks", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetLibraryDisksResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetLibraryDisksAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/InsertDvdDisk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/InsertDvdDiskResponse")]
        SolidCP.Providers.Common.ResultObject InsertDvdDisk(int itemId, string isoPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/InsertDvdDisk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/InsertDvdDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> InsertDvdDiskAsync(int itemId, string isoPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/EjectDvdDisk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/EjectDvdDiskResponse")]
        SolidCP.Providers.Common.ResultObject EjectDvdDisk(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/EjectDvdDisk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/EjectDvdDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> EjectDvdDiskAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineSnapshots", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineSnapshotsResponse")]
        SolidCP.Providers.Virtualization.VirtualMachineSnapshot[] GetVirtualMachineSnapshots(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineSnapshots", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineSnapshotsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]> GetVirtualMachineSnapshotsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetSnapshotResponse")]
        SolidCP.Providers.Virtualization.VirtualMachineSnapshot GetSnapshot(int itemId, string snaphostId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot> GetSnapshotAsync(int itemId, string snaphostId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/CreateSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/CreateSnapshotResponse")]
        SolidCP.Providers.Common.ResultObject CreateSnapshot(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/CreateSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/CreateSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateSnapshotAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/ApplySnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/ApplySnapshotResponse")]
        SolidCP.Providers.Common.ResultObject ApplySnapshot(int itemId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/ApplySnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/ApplySnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ApplySnapshotAsync(int itemId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/RenameSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/RenameSnapshotResponse")]
        SolidCP.Providers.Common.ResultObject RenameSnapshot(int itemId, string snapshotId, string newName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/RenameSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/RenameSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RenameSnapshotAsync(int itemId, string snapshotId, string newName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/DeleteSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/DeleteSnapshotResponse")]
        SolidCP.Providers.Common.ResultObject DeleteSnapshot(int itemId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/DeleteSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/DeleteSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteSnapshotAsync(int itemId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/DeleteSnapshotSubtree", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/DeleteSnapshotSubtreeResponse")]
        SolidCP.Providers.Common.ResultObject DeleteSnapshotSubtree(int itemId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/DeleteSnapshotSubtree", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/DeleteSnapshotSubtreeResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteSnapshotSubtreeAsync(int itemId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetSnapshotThumbnail", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetSnapshotThumbnailResponse")]
        byte[] GetSnapshotThumbnail(int itemId, string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetSnapshotThumbnail", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetSnapshotThumbnailResponse")]
        System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailAsync(int itemId, string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetExternalNetworkAdapterDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetExternalNetworkAdapterDetailsResponse")]
        SolidCP.EnterpriseServer.NetworkAdapterDetails GetExternalNetworkAdapterDetails(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetExternalNetworkAdapterDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetExternalNetworkAdapterDetailsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetExternalNetworkAdapterDetailsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/AddVirtualMachineExternalIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/AddVirtualMachineExternalIPAddressesResponse")]
        SolidCP.Providers.Common.ResultObject AddVirtualMachineExternalIPAddresses(int itemId, bool selectRandom, int addressesNumber, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/AddVirtualMachineExternalIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/AddVirtualMachineExternalIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachineExternalIPAddressesAsync(int itemId, bool selectRandom, int addressesNumber, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/SetVirtualMachinePrimaryExternalIPAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/SetVirtualMachinePrimaryExternalIPAddressResponse")]
        SolidCP.Providers.Common.ResultObject SetVirtualMachinePrimaryExternalIPAddress(int itemId, int addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/SetVirtualMachinePrimaryExternalIPAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/SetVirtualMachinePrimaryExternalIPAddressResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetVirtualMachinePrimaryExternalIPAddressAsync(int itemId, int addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/DeleteVirtualMachineExternalIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/DeleteVirtualMachineExternalIPAddressesResponse")]
        SolidCP.Providers.Common.ResultObject DeleteVirtualMachineExternalIPAddresses(int itemId, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/DeleteVirtualMachineExternalIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/DeleteVirtualMachineExternalIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineExternalIPAddressesAsync(int itemId, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetPrivateNetworkAdapterDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetPrivateNetworkAdapterDetailsResponse")]
        SolidCP.EnterpriseServer.NetworkAdapterDetails GetPrivateNetworkAdapterDetails(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetPrivateNetworkAdapterDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetPrivateNetworkAdapterDetailsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetPrivateNetworkAdapterDetailsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/AddVirtualMachinePrivateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/AddVirtualMachinePrivateIPAddressesResponse")]
        SolidCP.Providers.Common.ResultObject AddVirtualMachinePrivateIPAddresses(int itemId, bool selectRandom, int addressesNumber, string[] addresses);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/AddVirtualMachinePrivateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/AddVirtualMachinePrivateIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachinePrivateIPAddressesAsync(int itemId, bool selectRandom, int addressesNumber, string[] addresses);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/SetVirtualMachinePrimaryPrivateIPAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/SetVirtualMachinePrimaryPrivateIPAddressResponse")]
        SolidCP.Providers.Common.ResultObject SetVirtualMachinePrimaryPrivateIPAddress(int itemId, int addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/SetVirtualMachinePrimaryPrivateIPAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/SetVirtualMachinePrimaryPrivateIPAddressResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetVirtualMachinePrimaryPrivateIPAddressAsync(int itemId, int addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/DeleteVirtualMachinePrivateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/DeleteVirtualMachinePrivateIPAddressesResponse")]
        SolidCP.Providers.Common.ResultObject DeleteVirtualMachinePrivateIPAddresses(int itemId, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/DeleteVirtualMachinePrivateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/DeleteVirtualMachinePrivateIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachinePrivateIPAddressesAsync(int itemId, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachinePermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachinePermissionsResponse")]
        SolidCP.EnterpriseServer.VirtualMachinePermission[] /*List*/ GetVirtualMachinePermissions(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachinePermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachinePermissionsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachinePermission[]> GetVirtualMachinePermissionsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/UpdateVirtualMachineUserPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/UpdateVirtualMachineUserPermissionsResponse")]
        int UpdateVirtualMachineUserPermissions(int itemId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/UpdateVirtualMachineUserPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/UpdateVirtualMachineUserPermissionsResponse")]
        System.Threading.Tasks.Task<int> UpdateVirtualMachineUserPermissionsAsync(int itemId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetExternalSwitches", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetExternalSwitchesResponse")]
        SolidCP.Providers.Virtualization.VirtualSwitch[] GetExternalSwitches(int serviceId, string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetExternalSwitches", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetExternalSwitchesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetExternalSwitchesAsync(int serviceId, string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/DeleteVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/DeleteVirtualMachineResponse")]
        SolidCP.Providers.Common.ResultObject DeleteVirtualMachine(int itemId, bool saveFiles, bool exportVps, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/DeleteVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/DeleteVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineAsync(int itemId, bool saveFiles, bool exportVps, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/ReinstallVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/ReinstallVirtualMachineResponse")]
        int ReinstallVirtualMachine(int itemId, string adminPassword, bool preserveVirtualDiskFiles, bool saveVirtualDisk, bool exportVps, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/ReinstallVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/ReinstallVirtualMachineResponse")]
        System.Threading.Tasks.Task<int> ReinstallVirtualMachineAsync(int itemId, string adminPassword, bool preserveVirtualDiskFiles, bool saveVirtualDisk, bool exportVps, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineSummaryText", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineSummaryTextResponse")]
        string GetVirtualMachineSummaryText(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineSummaryText", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/GetVirtualMachineSummaryTextResponse")]
        System.Threading.Tasks.Task<string> GetVirtualMachineSummaryTextAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/SendVirtualMachineSummaryLetter", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/SendVirtualMachineSummaryLetterResponse")]
        SolidCP.Providers.Common.ResultObject SendVirtualMachineSummaryLetter(int itemId, string to, string bcc);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/SendVirtualMachineSummaryLetter", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer/SendVirtualMachineSummaryLetterResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SendVirtualMachineSummaryLetterAsync(int itemId, string to, string bcc);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esVirtualizationServerAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesVirtualizationServer
    {
        public SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged GetVirtualMachines(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            return Invoke<SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetVirtualMachines", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged> GetVirtualMachinesAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetVirtualMachines", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows, recursive);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine[] GetVirtualMachinesByServiceId(int serviceId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine[]>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetVirtualMachinesByServiceId", serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine[]> GetVirtualMachinesByServiceIdAsync(int serviceId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine[]>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetVirtualMachinesByServiceId", serviceId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineItem(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetVirtualMachineItem", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineItemAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetVirtualMachineItem", itemId);
        }

        public string EvaluateVirtualMachineTemplate(int itemId, string template)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esVirtualizationServer", "EvaluateVirtualMachineTemplate", itemId, template);
        }

        public async System.Threading.Tasks.Task<string> EvaluateVirtualMachineTemplateAsync(int itemId, string template)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esVirtualizationServer", "EvaluateVirtualMachineTemplate", itemId, template);
        }

        public SolidCP.EnterpriseServer.NetworkAdapterDetails GetExternalNetworkDetails(int packageId)
        {
            return Invoke<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetExternalNetworkDetails", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetExternalNetworkDetailsAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetExternalNetworkDetails", packageId);
        }

        public SolidCP.EnterpriseServer.PrivateIPAddressesPaged GetPackagePrivateIPAddressesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.EnterpriseServer.PrivateIPAddressesPaged>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetPackagePrivateIPAddressesPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PrivateIPAddressesPaged> GetPackagePrivateIPAddressesPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PrivateIPAddressesPaged>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetPackagePrivateIPAddressesPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.EnterpriseServer.PrivateIPAddress[] /*List*/ GetPackagePrivateIPAddresses(int packageId)
        {
            return Invoke<SolidCP.EnterpriseServer.PrivateIPAddress[], SolidCP.EnterpriseServer.PrivateIPAddress>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetPackagePrivateIPAddresses", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PrivateIPAddress[]> GetPackagePrivateIPAddressesAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PrivateIPAddress[], SolidCP.EnterpriseServer.PrivateIPAddress>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetPackagePrivateIPAddresses", packageId);
        }

        public SolidCP.EnterpriseServer.NetworkAdapterDetails GetPrivateNetworkDetails(int packageId)
        {
            return Invoke<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetPrivateNetworkDetails", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetPrivateNetworkDetailsAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetPrivateNetworkDetails", packageId);
        }

        public SolidCP.EnterpriseServer.VirtualMachinePermission[] /*List*/ GetSpaceUserPermissions(int packageId)
        {
            return Invoke<SolidCP.EnterpriseServer.VirtualMachinePermission[], SolidCP.EnterpriseServer.VirtualMachinePermission>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetSpaceUserPermissions", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachinePermission[]> GetSpaceUserPermissionsAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.VirtualMachinePermission[], SolidCP.EnterpriseServer.VirtualMachinePermission>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetSpaceUserPermissions", packageId);
        }

        public int UpdateSpaceUserPermissions(int packageId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esVirtualizationServer", "UpdateSpaceUserPermissions", packageId, permissions);
        }

        public async System.Threading.Tasks.Task<int> UpdateSpaceUserPermissionsAsync(int packageId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esVirtualizationServer", "UpdateSpaceUserPermissions", packageId, permissions);
        }

        public SolidCP.EnterpriseServer.LogRecord[] /*List*/ GetSpaceAuditLog(int packageId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.EnterpriseServer.LogRecord[], SolidCP.EnterpriseServer.LogRecord>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetSpaceAuditLog", packageId, startPeriod, endPeriod, severity, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.LogRecord[]> GetSpaceAuditLogAsync(int packageId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.LogRecord[], SolidCP.EnterpriseServer.LogRecord>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetSpaceAuditLog", packageId, startPeriod, endPeriod, severity, sortColumn, startRow, maximumRows);
        }

        public SolidCP.EnterpriseServer.LogRecord[] /*List*/ GetVirtualMachineAuditLog(int itemId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.EnterpriseServer.LogRecord[], SolidCP.EnterpriseServer.LogRecord>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetVirtualMachineAuditLog", itemId, startPeriod, endPeriod, severity, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.LogRecord[]> GetVirtualMachineAuditLogAsync(int itemId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.LogRecord[], SolidCP.EnterpriseServer.LogRecord>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetVirtualMachineAuditLog", itemId, startPeriod, endPeriod, severity, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetOperatingSystemTemplates(int packageId)
        {
            return Invoke<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetOperatingSystemTemplates", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetOperatingSystemTemplatesAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetOperatingSystemTemplates", packageId);
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetOperatingSystemTemplatesByServiceId(int serviceId)
        {
            return Invoke<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetOperatingSystemTemplatesByServiceId", serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetOperatingSystemTemplatesByServiceIdAsync(int serviceId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetOperatingSystemTemplatesByServiceId", serviceId);
        }

        public int GetMaximumCpuCoresNumber(int packageId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetMaximumCpuCoresNumber", packageId);
        }

        public async System.Threading.Tasks.Task<int> GetMaximumCpuCoresNumberAsync(int packageId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetMaximumCpuCoresNumber", packageId);
        }

        public string GetDefaultExportPath(int itemId)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetDefaultExportPath", itemId);
        }

        public async System.Threading.Tasks.Task<string> GetDefaultExportPathAsync(int itemId)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetDefaultExportPath", itemId);
        }

        public SolidCP.Providers.ResultObjects.IntResult CreateDefaultVirtualMachine(int packageId, string hostname, string osTemplate, string password, string summaryLetterEmail)
        {
            return Invoke<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServer", "CreateDefaultVirtualMachine", packageId, hostname, osTemplate, password, summaryLetterEmail);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> CreateDefaultVirtualMachineAsync(int packageId, string hostname, string osTemplate, string password, string summaryLetterEmail)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServer", "CreateDefaultVirtualMachine", packageId, hostname, osTemplate, password, summaryLetterEmail);
        }

        public SolidCP.Providers.ResultObjects.IntResult CreateVirtualMachine(int packageId, string hostname, string osTemplateFile, string password, string summaryLetterEmail, int generation, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses, bool privateNetworkEnabled, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses)
        {
            return Invoke<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServer", "CreateVirtualMachine", packageId, hostname, osTemplateFile, password, summaryLetterEmail, generation, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, externalAddressesNumber, randomExternalAddresses, externalAddresses, privateNetworkEnabled, privateAddressesNumber, randomPrivateAddresses, privateAddresses);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> CreateVirtualMachineAsync(int packageId, string hostname, string osTemplateFile, string password, string summaryLetterEmail, int generation, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses, bool privateNetworkEnabled, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServer", "CreateVirtualMachine", packageId, hostname, osTemplateFile, password, summaryLetterEmail, generation, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, externalAddressesNumber, randomExternalAddresses, externalAddresses, privateNetworkEnabled, privateAddressesNumber, randomPrivateAddresses, privateAddresses);
        }

        public SolidCP.Providers.ResultObjects.IntResult ImportVirtualMachine(int packageId, int serviceId, string vmId, string osTemplateFile, string adminPassword, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, string externalNicMacAddress, int[] externalAddresses, string managementNicMacAddress, int managementAddress)
        {
            return Invoke<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServer", "ImportVirtualMachine", packageId, serviceId, vmId, osTemplateFile, adminPassword, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNicMacAddress, externalAddresses, managementNicMacAddress, managementAddress);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> ImportVirtualMachineAsync(int packageId, int serviceId, string vmId, string osTemplateFile, string adminPassword, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, string externalNicMacAddress, int[] externalAddresses, string managementNicMacAddress, int managementAddress)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServer", "ImportVirtualMachine", packageId, serviceId, vmId, osTemplateFile, adminPassword, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNicMacAddress, externalAddresses, managementNicMacAddress, managementAddress);
        }

        public byte[] GetVirtualMachineThumbnail(int itemId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return Invoke<byte[]>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetVirtualMachineThumbnail", itemId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailAsync(int itemId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return await InvokeAsync<byte[]>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetVirtualMachineThumbnail", itemId, size);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineGeneralDetails(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetVirtualMachineGeneralDetails", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineGeneralDetailsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetVirtualMachineGeneralDetails", itemId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineExtendedInfo(int serviceId, string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetVirtualMachineExtendedInfo", serviceId, vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineExtendedInfoAsync(int serviceId, string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetVirtualMachineExtendedInfo", serviceId, vmId);
        }

        public int CancelVirtualMachineJob(string jobId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esVirtualizationServer", "CancelVirtualMachineJob", jobId);
        }

        public async System.Threading.Tasks.Task<int> CancelVirtualMachineJobAsync(string jobId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esVirtualizationServer", "CancelVirtualMachineJob", jobId);
        }

        public SolidCP.Providers.Common.ResultObject UpdateVirtualMachineHostName(int itemId, string hostname, bool updateNetBIOS)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "UpdateVirtualMachineHostName", itemId, hostname, updateNetBIOS);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateVirtualMachineHostNameAsync(int itemId, string hostname, bool updateNetBIOS)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "UpdateVirtualMachineHostName", itemId, hostname, updateNetBIOS);
        }

        public SolidCP.Providers.Common.ResultObject ChangeVirtualMachineState(int itemId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState state)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "ChangeVirtualMachineState", itemId, state);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeVirtualMachineStateAsync(int itemId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState state)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "ChangeVirtualMachineState", itemId, state);
        }

        public SolidCP.Providers.Virtualization.ConcreteJob[] /*List*/ GetVirtualMachineJobs(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.ConcreteJob[], SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetVirtualMachineJobs", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob[]> GetVirtualMachineJobsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ConcreteJob[], SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetVirtualMachineJobs", itemId);
        }

        public SolidCP.Providers.Common.ResultObject ChangeAdministratorPassword(int itemId, string password)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "ChangeAdministratorPassword", itemId, password);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeAdministratorPasswordAsync(int itemId, string password)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "ChangeAdministratorPassword", itemId, password);
        }

        public SolidCP.Providers.Common.ResultObject UpdateVirtualMachineConfiguration(int itemId, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, bool privateNetworkEnabled)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "UpdateVirtualMachineConfiguration", itemId, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, privateNetworkEnabled);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateVirtualMachineConfigurationAsync(int itemId, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, bool privateNetworkEnabled)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "UpdateVirtualMachineConfiguration", itemId, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, privateNetworkEnabled);
        }

        public SolidCP.Providers.Virtualization.LibraryItem GetInsertedDvdDisk(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.LibraryItem>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetInsertedDvdDisk", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem> GetInsertedDvdDiskAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.LibraryItem>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetInsertedDvdDisk", itemId);
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetLibraryDisks(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetLibraryDisks", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetLibraryDisksAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetLibraryDisks", itemId);
        }

        public SolidCP.Providers.Common.ResultObject InsertDvdDisk(int itemId, string isoPath)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "InsertDvdDisk", itemId, isoPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> InsertDvdDiskAsync(int itemId, string isoPath)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "InsertDvdDisk", itemId, isoPath);
        }

        public SolidCP.Providers.Common.ResultObject EjectDvdDisk(int itemId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "EjectDvdDisk", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> EjectDvdDiskAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "EjectDvdDisk", itemId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachineSnapshot[] GetVirtualMachineSnapshots(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetVirtualMachineSnapshots", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]> GetVirtualMachineSnapshotsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetVirtualMachineSnapshots", itemId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachineSnapshot GetSnapshot(int itemId, string snaphostId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachineSnapshot>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetSnapshot", itemId, snaphostId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot> GetSnapshotAsync(int itemId, string snaphostId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachineSnapshot>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetSnapshot", itemId, snaphostId);
        }

        public SolidCP.Providers.Common.ResultObject CreateSnapshot(int itemId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "CreateSnapshot", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateSnapshotAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "CreateSnapshot", itemId);
        }

        public SolidCP.Providers.Common.ResultObject ApplySnapshot(int itemId, string snapshotId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "ApplySnapshot", itemId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ApplySnapshotAsync(int itemId, string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "ApplySnapshot", itemId, snapshotId);
        }

        public SolidCP.Providers.Common.ResultObject RenameSnapshot(int itemId, string snapshotId, string newName)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "RenameSnapshot", itemId, snapshotId, newName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RenameSnapshotAsync(int itemId, string snapshotId, string newName)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "RenameSnapshot", itemId, snapshotId, newName);
        }

        public SolidCP.Providers.Common.ResultObject DeleteSnapshot(int itemId, string snapshotId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "DeleteSnapshot", itemId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteSnapshotAsync(int itemId, string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "DeleteSnapshot", itemId, snapshotId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteSnapshotSubtree(int itemId, string snapshotId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "DeleteSnapshotSubtree", itemId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteSnapshotSubtreeAsync(int itemId, string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "DeleteSnapshotSubtree", itemId, snapshotId);
        }

        public byte[] GetSnapshotThumbnail(int itemId, string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return Invoke<byte[]>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetSnapshotThumbnail", itemId, snapshotId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailAsync(int itemId, string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return await InvokeAsync<byte[]>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetSnapshotThumbnail", itemId, snapshotId, size);
        }

        public SolidCP.EnterpriseServer.NetworkAdapterDetails GetExternalNetworkAdapterDetails(int itemId)
        {
            return Invoke<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetExternalNetworkAdapterDetails", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetExternalNetworkAdapterDetailsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetExternalNetworkAdapterDetails", itemId);
        }

        public SolidCP.Providers.Common.ResultObject AddVirtualMachineExternalIPAddresses(int itemId, bool selectRandom, int addressesNumber, int[] addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "AddVirtualMachineExternalIPAddresses", itemId, selectRandom, addressesNumber, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachineExternalIPAddressesAsync(int itemId, bool selectRandom, int addressesNumber, int[] addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "AddVirtualMachineExternalIPAddresses", itemId, selectRandom, addressesNumber, addressId);
        }

        public SolidCP.Providers.Common.ResultObject SetVirtualMachinePrimaryExternalIPAddress(int itemId, int addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "SetVirtualMachinePrimaryExternalIPAddress", itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetVirtualMachinePrimaryExternalIPAddressAsync(int itemId, int addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "SetVirtualMachinePrimaryExternalIPAddress", itemId, addressId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteVirtualMachineExternalIPAddresses(int itemId, int[] addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "DeleteVirtualMachineExternalIPAddresses", itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineExternalIPAddressesAsync(int itemId, int[] addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "DeleteVirtualMachineExternalIPAddresses", itemId, addressId);
        }

        public SolidCP.EnterpriseServer.NetworkAdapterDetails GetPrivateNetworkAdapterDetails(int itemId)
        {
            return Invoke<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetPrivateNetworkAdapterDetails", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetPrivateNetworkAdapterDetailsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetPrivateNetworkAdapterDetails", itemId);
        }

        public SolidCP.Providers.Common.ResultObject AddVirtualMachinePrivateIPAddresses(int itemId, bool selectRandom, int addressesNumber, string[] addresses)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "AddVirtualMachinePrivateIPAddresses", itemId, selectRandom, addressesNumber, addresses);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachinePrivateIPAddressesAsync(int itemId, bool selectRandom, int addressesNumber, string[] addresses)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "AddVirtualMachinePrivateIPAddresses", itemId, selectRandom, addressesNumber, addresses);
        }

        public SolidCP.Providers.Common.ResultObject SetVirtualMachinePrimaryPrivateIPAddress(int itemId, int addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "SetVirtualMachinePrimaryPrivateIPAddress", itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetVirtualMachinePrimaryPrivateIPAddressAsync(int itemId, int addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "SetVirtualMachinePrimaryPrivateIPAddress", itemId, addressId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteVirtualMachinePrivateIPAddresses(int itemId, int[] addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "DeleteVirtualMachinePrivateIPAddresses", itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachinePrivateIPAddressesAsync(int itemId, int[] addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "DeleteVirtualMachinePrivateIPAddresses", itemId, addressId);
        }

        public SolidCP.EnterpriseServer.VirtualMachinePermission[] /*List*/ GetVirtualMachinePermissions(int itemId)
        {
            return Invoke<SolidCP.EnterpriseServer.VirtualMachinePermission[], SolidCP.EnterpriseServer.VirtualMachinePermission>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetVirtualMachinePermissions", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachinePermission[]> GetVirtualMachinePermissionsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.VirtualMachinePermission[], SolidCP.EnterpriseServer.VirtualMachinePermission>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetVirtualMachinePermissions", itemId);
        }

        public int UpdateVirtualMachineUserPermissions(int itemId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esVirtualizationServer", "UpdateVirtualMachineUserPermissions", itemId, permissions);
        }

        public async System.Threading.Tasks.Task<int> UpdateVirtualMachineUserPermissionsAsync(int itemId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esVirtualizationServer", "UpdateVirtualMachineUserPermissions", itemId, permissions);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch[] GetExternalSwitches(int serviceId, string computerName)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualSwitch[]>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetExternalSwitches", serviceId, computerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetExternalSwitchesAsync(int serviceId, string computerName)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualSwitch[]>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetExternalSwitches", serviceId, computerName);
        }

        public SolidCP.Providers.Common.ResultObject DeleteVirtualMachine(int itemId, bool saveFiles, bool exportVps, string exportPath)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "DeleteVirtualMachine", itemId, saveFiles, exportVps, exportPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineAsync(int itemId, bool saveFiles, bool exportVps, string exportPath)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "DeleteVirtualMachine", itemId, saveFiles, exportVps, exportPath);
        }

        public int ReinstallVirtualMachine(int itemId, string adminPassword, bool preserveVirtualDiskFiles, bool saveVirtualDisk, bool exportVps, string exportPath)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esVirtualizationServer", "ReinstallVirtualMachine", itemId, adminPassword, preserveVirtualDiskFiles, saveVirtualDisk, exportVps, exportPath);
        }

        public async System.Threading.Tasks.Task<int> ReinstallVirtualMachineAsync(int itemId, string adminPassword, bool preserveVirtualDiskFiles, bool saveVirtualDisk, bool exportVps, string exportPath)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esVirtualizationServer", "ReinstallVirtualMachine", itemId, adminPassword, preserveVirtualDiskFiles, saveVirtualDisk, exportVps, exportPath);
        }

        public string GetVirtualMachineSummaryText(int itemId)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetVirtualMachineSummaryText", itemId);
        }

        public async System.Threading.Tasks.Task<string> GetVirtualMachineSummaryTextAsync(int itemId)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esVirtualizationServer", "GetVirtualMachineSummaryText", itemId);
        }

        public SolidCP.Providers.Common.ResultObject SendVirtualMachineSummaryLetter(int itemId, string to, string bcc)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "SendVirtualMachineSummaryLetter", itemId, to, bcc);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SendVirtualMachineSummaryLetterAsync(int itemId, string to, string bcc)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer", "SendVirtualMachineSummaryLetter", itemId, to, bcc);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esVirtualizationServer : SolidCP.Web.Client.ClientBase<IesVirtualizationServer, esVirtualizationServerAssemblyClient>, IesVirtualizationServer
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

        public SolidCP.Providers.ResultObjects.IntResult CreateVirtualMachine(int packageId, string hostname, string osTemplateFile, string password, string summaryLetterEmail, int generation, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses, bool privateNetworkEnabled, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses)
        {
            return base.Client.CreateVirtualMachine(packageId, hostname, osTemplateFile, password, summaryLetterEmail, generation, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, externalAddressesNumber, randomExternalAddresses, externalAddresses, privateNetworkEnabled, privateAddressesNumber, randomPrivateAddresses, privateAddresses);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> CreateVirtualMachineAsync(int packageId, string hostname, string osTemplateFile, string password, string summaryLetterEmail, int generation, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses, bool privateNetworkEnabled, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses)
        {
            return await base.Client.CreateVirtualMachineAsync(packageId, hostname, osTemplateFile, password, summaryLetterEmail, generation, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, externalAddressesNumber, randomExternalAddresses, externalAddresses, privateNetworkEnabled, privateAddressesNumber, randomPrivateAddresses, privateAddresses);
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
    }
}
#endif