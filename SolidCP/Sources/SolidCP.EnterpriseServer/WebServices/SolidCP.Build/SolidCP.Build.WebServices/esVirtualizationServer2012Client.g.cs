#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesVirtualizationServer2012", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesVirtualizationServer2012
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachines", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachinesResponse")]
        SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged GetVirtualMachines(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachines", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachinesResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged> GetVirtualMachinesAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachinesByServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachinesByServiceIdResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine[] GetVirtualMachinesByServiceId(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachinesByServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachinesByServiceIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine[]> GetVirtualMachinesByServiceIdAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineItem", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineItemResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineItem(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineItem", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineItemResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineItemAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/EvaluateVirtualMachineTemplate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/EvaluateVirtualMachineTemplateResponse")]
        string EvaluateVirtualMachineTemplate(int itemId, string template);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/EvaluateVirtualMachineTemplate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/EvaluateVirtualMachineTemplateResponse")]
        System.Threading.Tasks.Task<string> EvaluateVirtualMachineTemplateAsync(int itemId, string template);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetExternalNetworkDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetExternalNetworkDetailsResponse")]
        SolidCP.EnterpriseServer.NetworkAdapterDetails GetExternalNetworkDetails(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetExternalNetworkDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetExternalNetworkDetailsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetExternalNetworkDetailsAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetPackagePrivateIPAddressesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetPackagePrivateIPAddressesPagedResponse")]
        SolidCP.EnterpriseServer.PrivateIPAddressesPaged GetPackagePrivateIPAddressesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetPackagePrivateIPAddressesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetPackagePrivateIPAddressesPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PrivateIPAddressesPaged> GetPackagePrivateIPAddressesPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetPackagePrivateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetPackagePrivateIPAddressesResponse")]
        SolidCP.EnterpriseServer.PrivateIPAddress[] /*List*/ GetPackagePrivateIPAddresses(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetPackagePrivateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetPackagePrivateIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PrivateIPAddress[]> GetPackagePrivateIPAddressesAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetPrivateNetworkDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetPrivateNetworkDetailsResponse")]
        SolidCP.EnterpriseServer.NetworkAdapterDetails GetPrivateNetworkDetails(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetPrivateNetworkDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetPrivateNetworkDetailsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetPrivateNetworkDetailsAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetSpaceUserPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetSpaceUserPermissionsResponse")]
        SolidCP.EnterpriseServer.VirtualMachinePermission[] /*List*/ GetSpaceUserPermissions(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetSpaceUserPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetSpaceUserPermissionsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachinePermission[]> GetSpaceUserPermissionsAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/UpdateSpaceUserPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/UpdateSpaceUserPermissionsResponse")]
        int UpdateSpaceUserPermissions(int packageId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/UpdateSpaceUserPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/UpdateSpaceUserPermissionsResponse")]
        System.Threading.Tasks.Task<int> UpdateSpaceUserPermissionsAsync(int packageId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetSpaceAuditLog", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetSpaceAuditLogResponse")]
        SolidCP.EnterpriseServer.LogRecord[] /*List*/ GetSpaceAuditLog(int packageId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetSpaceAuditLog", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetSpaceAuditLogResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.LogRecord[]> GetSpaceAuditLogAsync(int packageId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineAuditLog", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineAuditLogResponse")]
        SolidCP.EnterpriseServer.LogRecord[] /*List*/ GetVirtualMachineAuditLog(int itemId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineAuditLog", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineAuditLogResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.LogRecord[]> GetVirtualMachineAuditLogAsync(int itemId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetOperatingSystemTemplates", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetOperatingSystemTemplatesResponse")]
        SolidCP.Providers.Virtualization.LibraryItem[] GetOperatingSystemTemplates(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetOperatingSystemTemplates", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetOperatingSystemTemplatesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetOperatingSystemTemplatesAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetOperatingSystemTemplatesByServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetOperatingSystemTemplatesByServiceIdResponse")]
        SolidCP.Providers.Virtualization.LibraryItem[] GetOperatingSystemTemplatesByServiceId(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetOperatingSystemTemplatesByServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetOperatingSystemTemplatesByServiceIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetOperatingSystemTemplatesByServiceIdAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineGuacamoleURL", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineGuacamoleURLResponse")]
        string GetVirtualMachineGuacamoleURL(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineGuacamoleURL", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineGuacamoleURLResponse")]
        System.Threading.Tasks.Task<string> GetVirtualMachineGuacamoleURLAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetMaximumCpuCoresNumber", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetMaximumCpuCoresNumberResponse")]
        int GetMaximumCpuCoresNumber(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetMaximumCpuCoresNumber", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetMaximumCpuCoresNumberResponse")]
        System.Threading.Tasks.Task<int> GetMaximumCpuCoresNumberAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetDefaultExportPath", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetDefaultExportPathResponse")]
        string GetDefaultExportPath(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetDefaultExportPath", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetDefaultExportPathResponse")]
        System.Threading.Tasks.Task<string> GetDefaultExportPathAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/CreateDefaultVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/CreateDefaultVirtualMachineResponse")]
        SolidCP.Providers.ResultObjects.IntResult CreateDefaultVirtualMachine(int packageId, string hostname, string osTemplate, string password, string summaryLetterEmail);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/CreateDefaultVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/CreateDefaultVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> CreateDefaultVirtualMachineAsync(int packageId, string hostname, string osTemplate, string password, string summaryLetterEmail);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/CreateNewVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/CreateNewVirtualMachineResponse")]
        SolidCP.Providers.ResultObjects.IntResult CreateNewVirtualMachine(SolidCP.Providers.Virtualization.VirtualMachine VMSettings, string osTemplateFile, string password, string summaryLetterEmail, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/CreateNewVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/CreateNewVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> CreateNewVirtualMachineAsync(SolidCP.Providers.Virtualization.VirtualMachine VMSettings, string osTemplateFile, string password, string summaryLetterEmail, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/CreateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/CreateVirtualMachineResponse")]
        SolidCP.Providers.ResultObjects.IntResult CreateVirtualMachine(int packageId, string hostname, string osTemplateFile, string password, string summaryLetterEmail, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses, bool privateNetworkEnabled, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses, SolidCP.Providers.Virtualization.VirtualMachine otherSettings);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/CreateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/CreateVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> CreateVirtualMachineAsync(int packageId, string hostname, string osTemplateFile, string password, string summaryLetterEmail, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses, bool privateNetworkEnabled, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses, SolidCP.Providers.Virtualization.VirtualMachine otherSettings);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ImportVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ImportVirtualMachineResponse")]
        SolidCP.Providers.ResultObjects.IntResult ImportVirtualMachine(int packageId, int serviceId, string vmId, string osTemplateFile, string adminPassword, bool IsBootFromCd, bool IsDvdInstalled, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, string externalNicMacAddress, int[] externalAddresses, string managementNicMacAddress, int managementAddress, int maxSnapshots, bool ignoreChecks);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ImportVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ImportVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> ImportVirtualMachineAsync(int packageId, int serviceId, string vmId, string osTemplateFile, string adminPassword, bool IsBootFromCd, bool IsDvdInstalled, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, string externalNicMacAddress, int[] externalAddresses, string managementNicMacAddress, int managementAddress, int maxSnapshots, bool ignoreChecks);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineThumbnail", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineThumbnailResponse")]
        byte[] GetVirtualMachineThumbnail(int itemId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineThumbnail", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineThumbnailResponse")]
        System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailAsync(int itemId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineGeneralDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineGeneralDetailsResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineGeneralDetails(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineGeneralDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineGeneralDetailsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineGeneralDetailsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DiscoverVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DiscoverVirtualMachineResponse")]
        int DiscoverVirtualMachine(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DiscoverVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DiscoverVirtualMachineResponse")]
        System.Threading.Tasks.Task<int> DiscoverVirtualMachineAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineExtendedInfo", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineExtendedInfoResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineExtendedInfo(int serviceId, string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineExtendedInfo", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineExtendedInfoResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineExtendedInfoAsync(int serviceId, string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/CancelVirtualMachineJob", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/CancelVirtualMachineJobResponse")]
        int CancelVirtualMachineJob(string jobId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/CancelVirtualMachineJob", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/CancelVirtualMachineJobResponse")]
        System.Threading.Tasks.Task<int> CancelVirtualMachineJobAsync(string jobId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/UpdateVirtualMachineHostName", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/UpdateVirtualMachineHostNameResponse")]
        SolidCP.Providers.Common.ResultObject UpdateVirtualMachineHostName(int itemId, string hostname, bool updateNetBIOS);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/UpdateVirtualMachineHostName", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/UpdateVirtualMachineHostNameResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateVirtualMachineHostNameAsync(int itemId, string hostname, bool updateNetBIOS);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ChangeVirtualMachineState", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ChangeVirtualMachineStateResponse")]
        SolidCP.Providers.Common.ResultObject ChangeVirtualMachineState(int itemId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState state);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ChangeVirtualMachineState", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ChangeVirtualMachineStateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeVirtualMachineStateAsync(int itemId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState state);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineJobs", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineJobsResponse")]
        SolidCP.Providers.Virtualization.ConcreteJob[] /*List*/ GetVirtualMachineJobs(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineJobs", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineJobsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob[]> GetVirtualMachineJobsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachinesNetwordAdapterSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachinesNetwordAdapterSettingsResponse")]
        SolidCP.Providers.Virtualization.VirtualMachineNetworkAdapter[] GetVirtualMachinesNetwordAdapterSettings(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachinesNetwordAdapterSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachinesNetwordAdapterSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineNetworkAdapter[]> GetVirtualMachinesNetwordAdapterSettingsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ChangeAdministratorPassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ChangeAdministratorPasswordResponse")]
        SolidCP.Providers.Common.ResultObject ChangeAdministratorPassword(int itemId, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ChangeAdministratorPassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ChangeAdministratorPasswordResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeAdministratorPasswordAsync(int itemId, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ChangeAdministratorPasswordAndCleanResult", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ChangeAdministratorPasswordAndCleanResultResponse")]
        SolidCP.Providers.Common.ResultObject ChangeAdministratorPasswordAndCleanResult(int itemId, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ChangeAdministratorPasswordAndCleanResult", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ChangeAdministratorPasswordAndCleanResultResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeAdministratorPasswordAndCleanResultAsync(int itemId, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/UpdateVirtualMachineResource", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/UpdateVirtualMachineResourceResponse")]
        SolidCP.Providers.Common.ResultObject UpdateVirtualMachineResource(int itemId, SolidCP.Providers.Virtualization.VirtualMachine vmSettings);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/UpdateVirtualMachineResource", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/UpdateVirtualMachineResourceResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateVirtualMachineResourceAsync(int itemId, SolidCP.Providers.Virtualization.VirtualMachine vmSettings);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/UpdateVirtualMachineConfiguration", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/UpdateVirtualMachineConfigurationResponse")]
        SolidCP.Providers.Common.ResultObject UpdateVirtualMachineConfiguration(int itemId, int cpuCores, int ramMB, int[] hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, bool privateNetworkEnabled, SolidCP.Providers.Virtualization.VirtualMachine otherSettings);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/UpdateVirtualMachineConfiguration", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/UpdateVirtualMachineConfigurationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateVirtualMachineConfigurationAsync(int itemId, int cpuCores, int ramMB, int[] hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, bool privateNetworkEnabled, SolidCP.Providers.Virtualization.VirtualMachine otherSettings);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetInsertedDvdDisk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetInsertedDvdDiskResponse")]
        SolidCP.Providers.Virtualization.LibraryItem GetInsertedDvdDisk(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetInsertedDvdDisk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetInsertedDvdDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem> GetInsertedDvdDiskAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetLibraryDisks", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetLibraryDisksResponse")]
        SolidCP.Providers.Virtualization.LibraryItem[] GetLibraryDisks(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetLibraryDisks", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetLibraryDisksResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetLibraryDisksAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/InsertDvdDisk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/InsertDvdDiskResponse")]
        SolidCP.Providers.Common.ResultObject InsertDvdDisk(int itemId, string isoPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/InsertDvdDisk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/InsertDvdDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> InsertDvdDiskAsync(int itemId, string isoPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/EjectDvdDisk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/EjectDvdDiskResponse")]
        SolidCP.Providers.Common.ResultObject EjectDvdDisk(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/EjectDvdDisk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/EjectDvdDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> EjectDvdDiskAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineSnapshots", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineSnapshotsResponse")]
        SolidCP.Providers.Virtualization.VirtualMachineSnapshot[] GetVirtualMachineSnapshots(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineSnapshots", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineSnapshotsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]> GetVirtualMachineSnapshotsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetSnapshotResponse")]
        SolidCP.Providers.Virtualization.VirtualMachineSnapshot GetSnapshot(int itemId, string snaphostId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot> GetSnapshotAsync(int itemId, string snaphostId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/CreateSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/CreateSnapshotResponse")]
        SolidCP.Providers.Common.ResultObject CreateSnapshot(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/CreateSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/CreateSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateSnapshotAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ApplySnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ApplySnapshotResponse")]
        SolidCP.Providers.Common.ResultObject ApplySnapshot(int itemId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ApplySnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ApplySnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ApplySnapshotAsync(int itemId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/RenameSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/RenameSnapshotResponse")]
        SolidCP.Providers.Common.ResultObject RenameSnapshot(int itemId, string snapshotId, string newName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/RenameSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/RenameSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RenameSnapshotAsync(int itemId, string snapshotId, string newName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteSnapshotResponse")]
        SolidCP.Providers.Common.ResultObject DeleteSnapshot(int itemId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteSnapshot", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteSnapshotAsync(int itemId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteSnapshotSubtree", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteSnapshotSubtreeResponse")]
        SolidCP.Providers.Common.ResultObject DeleteSnapshotSubtree(int itemId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteSnapshotSubtree", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteSnapshotSubtreeResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteSnapshotSubtreeAsync(int itemId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetSnapshotThumbnail", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetSnapshotThumbnailResponse")]
        byte[] GetSnapshotThumbnail(int itemId, string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetSnapshotThumbnail", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetSnapshotThumbnailResponse")]
        System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailAsync(int itemId, string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GenerateMacAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GenerateMacAddressResponse")]
        string GenerateMacAddress();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GenerateMacAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GenerateMacAddressResponse")]
        System.Threading.Tasks.Task<string> GenerateMacAddressAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetExternalNetworkVLAN", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetExternalNetworkVLANResponse")]
        int GetExternalNetworkVLAN(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetExternalNetworkVLAN", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetExternalNetworkVLANResponse")]
        System.Threading.Tasks.Task<int> GetExternalNetworkVLANAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetExternalNetworkAdapterDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetExternalNetworkAdapterDetailsResponse")]
        SolidCP.EnterpriseServer.NetworkAdapterDetails GetExternalNetworkAdapterDetails(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetExternalNetworkAdapterDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetExternalNetworkAdapterDetailsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetExternalNetworkAdapterDetailsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/AddVirtualMachineExternalIPAddressesByInjection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/AddVirtualMachineExternalIPAddressesByInjectionResponse")]
        SolidCP.Providers.Common.ResultObject AddVirtualMachineExternalIPAddressesByInjection(int itemId, bool selectRandom, int addressesNumber, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/AddVirtualMachineExternalIPAddressesByInjection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/AddVirtualMachineExternalIPAddressesByInjectionResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachineExternalIPAddressesByInjectionAsync(int itemId, bool selectRandom, int addressesNumber, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteVirtualMachineExternalIPAddressesByInjection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteVirtualMachineExternalIPAddressesByInjectionResponse")]
        SolidCP.Providers.Common.ResultObject DeleteVirtualMachineExternalIPAddressesByInjection(int itemId, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteVirtualMachineExternalIPAddressesByInjection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteVirtualMachineExternalIPAddressesByInjectionResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineExternalIPAddressesByInjectionAsync(int itemId, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/RestoreVirtualMachineExternalIPAddressesByInjection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/RestoreVirtualMachineExternalIPAddressesByInjectionResponse")]
        SolidCP.Providers.Common.ResultObject RestoreVirtualMachineExternalIPAddressesByInjection(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/RestoreVirtualMachineExternalIPAddressesByInjection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/RestoreVirtualMachineExternalIPAddressesByInjectionResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RestoreVirtualMachineExternalIPAddressesByInjectionAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/AddVirtualMachineExternalIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/AddVirtualMachineExternalIPAddressesResponse")]
        SolidCP.Providers.Common.ResultObject AddVirtualMachineExternalIPAddresses(int itemId, bool selectRandom, int addressesNumber, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/AddVirtualMachineExternalIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/AddVirtualMachineExternalIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachineExternalIPAddressesAsync(int itemId, bool selectRandom, int addressesNumber, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/SetVirtualMachinePrimaryExternalIPAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/SetVirtualMachinePrimaryExternalIPAddressResponse")]
        SolidCP.Providers.Common.ResultObject SetVirtualMachinePrimaryExternalIPAddress(int itemId, int addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/SetVirtualMachinePrimaryExternalIPAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/SetVirtualMachinePrimaryExternalIPAddressResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetVirtualMachinePrimaryExternalIPAddressAsync(int itemId, int addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteVirtualMachineExternalIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteVirtualMachineExternalIPAddressesResponse")]
        SolidCP.Providers.Common.ResultObject DeleteVirtualMachineExternalIPAddresses(int itemId, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteVirtualMachineExternalIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteVirtualMachineExternalIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineExternalIPAddressesAsync(int itemId, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetPrivateNetworkAdapterDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetPrivateNetworkAdapterDetailsResponse")]
        SolidCP.EnterpriseServer.NetworkAdapterDetails GetPrivateNetworkAdapterDetails(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetPrivateNetworkAdapterDetails", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetPrivateNetworkAdapterDetailsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetPrivateNetworkAdapterDetailsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/RestoreVirtualMachinePrivateIPAddressesByInjection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/RestoreVirtualMachinePrivateIPAddressesByInjectionResponse")]
        SolidCP.Providers.Common.ResultObject RestoreVirtualMachinePrivateIPAddressesByInjection(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/RestoreVirtualMachinePrivateIPAddressesByInjection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/RestoreVirtualMachinePrivateIPAddressesByInjectionResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RestoreVirtualMachinePrivateIPAddressesByInjectionAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/AddVirtualMachinePrivateIPAddressesByInject", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/AddVirtualMachinePrivateIPAddressesByInjectResponse")]
        SolidCP.Providers.Common.ResultObject AddVirtualMachinePrivateIPAddressesByInject(int itemId, bool selectRandom, int addressesNumber, string[] addresses, bool customGatewayAndDns, string gateway, string dns1, string dns2, string subnetMask);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/AddVirtualMachinePrivateIPAddressesByInject", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/AddVirtualMachinePrivateIPAddressesByInjectResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachinePrivateIPAddressesByInjectAsync(int itemId, bool selectRandom, int addressesNumber, string[] addresses, bool customGatewayAndDns, string gateway, string dns1, string dns2, string subnetMask);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/AddVirtualMachinePrivateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/AddVirtualMachinePrivateIPAddressesResponse")]
        SolidCP.Providers.Common.ResultObject AddVirtualMachinePrivateIPAddresses(int itemId, bool selectRandom, int addressesNumber, string[] addresses, bool customGatewayAndDns, string gateway, string dns1, string dns2, string subnetMask);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/AddVirtualMachinePrivateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/AddVirtualMachinePrivateIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachinePrivateIPAddressesAsync(int itemId, bool selectRandom, int addressesNumber, string[] addresses, bool customGatewayAndDns, string gateway, string dns1, string dns2, string subnetMask);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/SetVirtualMachinePrimaryPrivateIPAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/SetVirtualMachinePrimaryPrivateIPAddressResponse")]
        SolidCP.Providers.Common.ResultObject SetVirtualMachinePrimaryPrivateIPAddress(int itemId, int addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/SetVirtualMachinePrimaryPrivateIPAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/SetVirtualMachinePrimaryPrivateIPAddressResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetVirtualMachinePrimaryPrivateIPAddressAsync(int itemId, int addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteVirtualMachinePrivateIPAddressesByInject", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteVirtualMachinePrivateIPAddressesByInjectResponse")]
        SolidCP.Providers.Common.ResultObject DeleteVirtualMachinePrivateIPAddressesByInject(int itemId, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteVirtualMachinePrivateIPAddressesByInject", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteVirtualMachinePrivateIPAddressesByInjectResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachinePrivateIPAddressesByInjectAsync(int itemId, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteVirtualMachinePrivateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteVirtualMachinePrivateIPAddressesResponse")]
        SolidCP.Providers.Common.ResultObject DeleteVirtualMachinePrivateIPAddresses(int itemId, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteVirtualMachinePrivateIPAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteVirtualMachinePrivateIPAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachinePrivateIPAddressesAsync(int itemId, int[] addressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachinePermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachinePermissionsResponse")]
        SolidCP.EnterpriseServer.VirtualMachinePermission[] /*List*/ GetVirtualMachinePermissions(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachinePermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachinePermissionsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachinePermission[]> GetVirtualMachinePermissionsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/UpdateVirtualMachineUserPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/UpdateVirtualMachineUserPermissionsResponse")]
        int UpdateVirtualMachineUserPermissions(int itemId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/UpdateVirtualMachineUserPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/UpdateVirtualMachineUserPermissionsResponse")]
        System.Threading.Tasks.Task<int> UpdateVirtualMachineUserPermissionsAsync(int itemId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetSecureBootTemplates", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetSecureBootTemplatesResponse")]
        SolidCP.Providers.Virtualization.SecureBootTemplate[] GetSecureBootTemplates(int serviceId, string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetSecureBootTemplates", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetSecureBootTemplatesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.SecureBootTemplate[]> GetSecureBootTemplatesAsync(int serviceId, string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVMConfigurationVersionSupportedList", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVMConfigurationVersionSupportedListResponse")]
        SolidCP.Providers.Virtualization.VMConfigurationVersion[] GetVMConfigurationVersionSupportedList(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVMConfigurationVersionSupportedList", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVMConfigurationVersionSupportedListResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMConfigurationVersion[]> GetVMConfigurationVersionSupportedListAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetExternalSwitches", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetExternalSwitchesResponse")]
        SolidCP.Providers.Virtualization.VirtualSwitch[] GetExternalSwitches(int serviceId, string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetExternalSwitches", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetExternalSwitchesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetExternalSwitchesAsync(int serviceId, string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetExternalSwitchesWMI", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetExternalSwitchesWMIResponse")]
        SolidCP.Providers.Virtualization.VirtualSwitch[] GetExternalSwitchesWMI(int serviceId, string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetExternalSwitchesWMI", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetExternalSwitchesWMIResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetExternalSwitchesWMIAsync(int serviceId, string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetInternalSwitches", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetInternalSwitchesResponse")]
        SolidCP.Providers.Virtualization.VirtualSwitch[] GetInternalSwitches(int serviceId, string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetInternalSwitches", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetInternalSwitchesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetInternalSwitchesAsync(int serviceId, string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteVirtualMachineResponse")]
        SolidCP.Providers.Common.ResultObject DeleteVirtualMachine(int itemId, bool saveFiles, bool exportVps, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineAsync(int itemId, bool saveFiles, bool exportVps, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteVirtualMachineAsynchronous", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteVirtualMachineAsynchronousResponse")]
        SolidCP.Providers.Common.ResultObject DeleteVirtualMachineAsynchronous(int itemId, bool saveFiles, bool exportVps, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteVirtualMachineAsynchronous", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DeleteVirtualMachineAsynchronousResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineAsynchronousAsync(int itemId, bool saveFiles, bool exportVps, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ReinstallVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ReinstallVirtualMachineResponse")]
        SolidCP.Providers.ResultObjects.IntResult ReinstallVirtualMachine(int itemId, SolidCP.Providers.Virtualization.VirtualMachine VMSettings, string adminPassword, string[] privIps, bool saveVirtualDisk, bool exportVps, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ReinstallVirtualMachine", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ReinstallVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> ReinstallVirtualMachineAsync(int itemId, SolidCP.Providers.Virtualization.VirtualMachine VMSettings, string adminPassword, string[] privIps, bool saveVirtualDisk, bool exportVps, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineSummaryText", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineSummaryTextResponse")]
        string GetVirtualMachineSummaryText(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineSummaryText", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetVirtualMachineSummaryTextResponse")]
        System.Threading.Tasks.Task<string> GetVirtualMachineSummaryTextAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/SendVirtualMachineSummaryLetter", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/SendVirtualMachineSummaryLetterResponse")]
        SolidCP.Providers.Common.ResultObject SendVirtualMachineSummaryLetter(int itemId, string to, string bcc);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/SendVirtualMachineSummaryLetter", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/SendVirtualMachineSummaryLetterResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SendVirtualMachineSummaryLetterAsync(int itemId, string to, string bcc);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetCertificates", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetCertificatesResponse")]
        SolidCP.Providers.Virtualization.CertificateInfo[] GetCertificates(int serviceId, string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetCertificates", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetCertificatesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.CertificateInfo[]> GetCertificatesAsync(int serviceId, string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/SetReplicaServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/SetReplicaServerResponse")]
        SolidCP.Providers.Common.ResultObject SetReplicaServer(int serviceId, string remoteServer, string thumbprint, string storagePath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/SetReplicaServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/SetReplicaServerResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetReplicaServerAsync(int serviceId, string remoteServer, string thumbprint, string storagePath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/UnsetReplicaServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/UnsetReplicaServerResponse")]
        SolidCP.Providers.Common.ResultObject UnsetReplicaServer(int serviceId, string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/UnsetReplicaServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/UnsetReplicaServerResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UnsetReplicaServerAsync(int serviceId, string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetReplicaServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetReplicaServerResponse")]
        SolidCP.Providers.Virtualization.ReplicationServerInfo GetReplicaServer(int serviceId, string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetReplicaServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetReplicaServerResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReplicationServerInfo> GetReplicaServerAsync(int serviceId, string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetReplication", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetReplicationResponse")]
        SolidCP.Providers.Virtualization.VmReplication GetReplication(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetReplication", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetReplicationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VmReplication> GetReplicationAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetReplicationInfo", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetReplicationInfoResponse")]
        SolidCP.Providers.Virtualization.ReplicationDetailInfo GetReplicationInfo(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetReplicationInfo", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/GetReplicationInfoResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReplicationDetailInfo> GetReplicationInfoAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/SetVmReplication", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/SetVmReplicationResponse")]
        SolidCP.Providers.Common.ResultObject SetVmReplication(int itemId, SolidCP.Providers.Virtualization.VmReplication replication);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/SetVmReplication", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/SetVmReplicationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetVmReplicationAsync(int itemId, SolidCP.Providers.Virtualization.VmReplication replication);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DisableVmReplication", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DisableVmReplicationResponse")]
        SolidCP.Providers.Common.ResultObject DisableVmReplication(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DisableVmReplication", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/DisableVmReplicationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DisableVmReplicationAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/PauseReplication", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/PauseReplicationResponse")]
        SolidCP.Providers.Common.ResultObject PauseReplication(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/PauseReplication", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/PauseReplicationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> PauseReplicationAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ResumeReplication", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ResumeReplicationResponse")]
        SolidCP.Providers.Common.ResultObject ResumeReplication(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ResumeReplication", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesVirtualizationServer2012/ResumeReplicationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ResumeReplicationAsync(int itemId);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esVirtualizationServer2012AssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesVirtualizationServer2012
    {
        public SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged GetVirtualMachines(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            return Invoke<SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachines", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged> GetVirtualMachinesAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.VirtualMachineMetaItemsPaged>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachines", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows, recursive);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine[] GetVirtualMachinesByServiceId(int serviceId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachinesByServiceId", serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine[]> GetVirtualMachinesByServiceIdAsync(int serviceId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachinesByServiceId", serviceId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineItem(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachineItem", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineItemAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachineItem", itemId);
        }

        public string EvaluateVirtualMachineTemplate(int itemId, string template)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "EvaluateVirtualMachineTemplate", itemId, template);
        }

        public async System.Threading.Tasks.Task<string> EvaluateVirtualMachineTemplateAsync(int itemId, string template)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "EvaluateVirtualMachineTemplate", itemId, template);
        }

        public SolidCP.EnterpriseServer.NetworkAdapterDetails GetExternalNetworkDetails(int packageId)
        {
            return Invoke<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetExternalNetworkDetails", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetExternalNetworkDetailsAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetExternalNetworkDetails", packageId);
        }

        public SolidCP.EnterpriseServer.PrivateIPAddressesPaged GetPackagePrivateIPAddressesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.EnterpriseServer.PrivateIPAddressesPaged>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetPackagePrivateIPAddressesPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PrivateIPAddressesPaged> GetPackagePrivateIPAddressesPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PrivateIPAddressesPaged>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetPackagePrivateIPAddressesPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.EnterpriseServer.PrivateIPAddress[] /*List*/ GetPackagePrivateIPAddresses(int packageId)
        {
            return Invoke<SolidCP.EnterpriseServer.PrivateIPAddress[], SolidCP.EnterpriseServer.PrivateIPAddress>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetPackagePrivateIPAddresses", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PrivateIPAddress[]> GetPackagePrivateIPAddressesAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PrivateIPAddress[], SolidCP.EnterpriseServer.PrivateIPAddress>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetPackagePrivateIPAddresses", packageId);
        }

        public SolidCP.EnterpriseServer.NetworkAdapterDetails GetPrivateNetworkDetails(int packageId)
        {
            return Invoke<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetPrivateNetworkDetails", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetPrivateNetworkDetailsAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetPrivateNetworkDetails", packageId);
        }

        public SolidCP.EnterpriseServer.VirtualMachinePermission[] /*List*/ GetSpaceUserPermissions(int packageId)
        {
            return Invoke<SolidCP.EnterpriseServer.VirtualMachinePermission[], SolidCP.EnterpriseServer.VirtualMachinePermission>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetSpaceUserPermissions", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachinePermission[]> GetSpaceUserPermissionsAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.VirtualMachinePermission[], SolidCP.EnterpriseServer.VirtualMachinePermission>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetSpaceUserPermissions", packageId);
        }

        public int UpdateSpaceUserPermissions(int packageId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "UpdateSpaceUserPermissions", packageId, permissions);
        }

        public async System.Threading.Tasks.Task<int> UpdateSpaceUserPermissionsAsync(int packageId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "UpdateSpaceUserPermissions", packageId, permissions);
        }

        public SolidCP.EnterpriseServer.LogRecord[] /*List*/ GetSpaceAuditLog(int packageId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.EnterpriseServer.LogRecord[], SolidCP.EnterpriseServer.LogRecord>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetSpaceAuditLog", packageId, startPeriod, endPeriod, severity, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.LogRecord[]> GetSpaceAuditLogAsync(int packageId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.LogRecord[], SolidCP.EnterpriseServer.LogRecord>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetSpaceAuditLog", packageId, startPeriod, endPeriod, severity, sortColumn, startRow, maximumRows);
        }

        public SolidCP.EnterpriseServer.LogRecord[] /*List*/ GetVirtualMachineAuditLog(int itemId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.EnterpriseServer.LogRecord[], SolidCP.EnterpriseServer.LogRecord>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachineAuditLog", itemId, startPeriod, endPeriod, severity, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.LogRecord[]> GetVirtualMachineAuditLogAsync(int itemId, System.DateTime startPeriod, System.DateTime endPeriod, int severity, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.LogRecord[], SolidCP.EnterpriseServer.LogRecord>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachineAuditLog", itemId, startPeriod, endPeriod, severity, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetOperatingSystemTemplates(int packageId)
        {
            return Invoke<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetOperatingSystemTemplates", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetOperatingSystemTemplatesAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetOperatingSystemTemplates", packageId);
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetOperatingSystemTemplatesByServiceId(int serviceId)
        {
            return Invoke<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetOperatingSystemTemplatesByServiceId", serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetOperatingSystemTemplatesByServiceIdAsync(int serviceId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetOperatingSystemTemplatesByServiceId", serviceId);
        }

        public string GetVirtualMachineGuacamoleURL(int itemId)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachineGuacamoleURL", itemId);
        }

        public async System.Threading.Tasks.Task<string> GetVirtualMachineGuacamoleURLAsync(int itemId)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachineGuacamoleURL", itemId);
        }

        public int GetMaximumCpuCoresNumber(int packageId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetMaximumCpuCoresNumber", packageId);
        }

        public async System.Threading.Tasks.Task<int> GetMaximumCpuCoresNumberAsync(int packageId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetMaximumCpuCoresNumber", packageId);
        }

        public string GetDefaultExportPath(int itemId)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetDefaultExportPath", itemId);
        }

        public async System.Threading.Tasks.Task<string> GetDefaultExportPathAsync(int itemId)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetDefaultExportPath", itemId);
        }

        public SolidCP.Providers.ResultObjects.IntResult CreateDefaultVirtualMachine(int packageId, string hostname, string osTemplate, string password, string summaryLetterEmail)
        {
            return Invoke<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "CreateDefaultVirtualMachine", packageId, hostname, osTemplate, password, summaryLetterEmail);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> CreateDefaultVirtualMachineAsync(int packageId, string hostname, string osTemplate, string password, string summaryLetterEmail)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "CreateDefaultVirtualMachine", packageId, hostname, osTemplate, password, summaryLetterEmail);
        }

        public SolidCP.Providers.ResultObjects.IntResult CreateNewVirtualMachine(SolidCP.Providers.Virtualization.VirtualMachine VMSettings, string osTemplateFile, string password, string summaryLetterEmail, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses)
        {
            return Invoke<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "CreateNewVirtualMachine", VMSettings, osTemplateFile, password, summaryLetterEmail, externalAddressesNumber, randomExternalAddresses, externalAddresses, privateAddressesNumber, randomPrivateAddresses, privateAddresses);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> CreateNewVirtualMachineAsync(SolidCP.Providers.Virtualization.VirtualMachine VMSettings, string osTemplateFile, string password, string summaryLetterEmail, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "CreateNewVirtualMachine", VMSettings, osTemplateFile, password, summaryLetterEmail, externalAddressesNumber, randomExternalAddresses, externalAddresses, privateAddressesNumber, randomPrivateAddresses, privateAddresses);
        }

        public SolidCP.Providers.ResultObjects.IntResult CreateVirtualMachine(int packageId, string hostname, string osTemplateFile, string password, string summaryLetterEmail, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses, bool privateNetworkEnabled, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses, SolidCP.Providers.Virtualization.VirtualMachine otherSettings)
        {
            return Invoke<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "CreateVirtualMachine", packageId, hostname, osTemplateFile, password, summaryLetterEmail, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, externalAddressesNumber, randomExternalAddresses, externalAddresses, privateNetworkEnabled, privateAddressesNumber, randomPrivateAddresses, privateAddresses, otherSettings);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> CreateVirtualMachineAsync(int packageId, string hostname, string osTemplateFile, string password, string summaryLetterEmail, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses, bool privateNetworkEnabled, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses, SolidCP.Providers.Virtualization.VirtualMachine otherSettings)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "CreateVirtualMachine", packageId, hostname, osTemplateFile, password, summaryLetterEmail, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, externalAddressesNumber, randomExternalAddresses, externalAddresses, privateNetworkEnabled, privateAddressesNumber, randomPrivateAddresses, privateAddresses, otherSettings);
        }

        public SolidCP.Providers.ResultObjects.IntResult ImportVirtualMachine(int packageId, int serviceId, string vmId, string osTemplateFile, string adminPassword, bool IsBootFromCd, bool IsDvdInstalled, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, string externalNicMacAddress, int[] externalAddresses, string managementNicMacAddress, int managementAddress, int maxSnapshots, bool ignoreChecks)
        {
            return Invoke<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "ImportVirtualMachine", packageId, serviceId, vmId, osTemplateFile, adminPassword, IsBootFromCd, IsDvdInstalled, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNicMacAddress, externalAddresses, managementNicMacAddress, managementAddress, maxSnapshots, ignoreChecks);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> ImportVirtualMachineAsync(int packageId, int serviceId, string vmId, string osTemplateFile, string adminPassword, bool IsBootFromCd, bool IsDvdInstalled, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, string externalNicMacAddress, int[] externalAddresses, string managementNicMacAddress, int managementAddress, int maxSnapshots, bool ignoreChecks)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "ImportVirtualMachine", packageId, serviceId, vmId, osTemplateFile, adminPassword, IsBootFromCd, IsDvdInstalled, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNicMacAddress, externalAddresses, managementNicMacAddress, managementAddress, maxSnapshots, ignoreChecks);
        }

        public byte[] GetVirtualMachineThumbnail(int itemId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return Invoke<byte[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachineThumbnail", itemId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailAsync(int itemId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return await InvokeAsync<byte[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachineThumbnail", itemId, size);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineGeneralDetails(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachineGeneralDetails", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineGeneralDetailsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachineGeneralDetails", itemId);
        }

        public int DiscoverVirtualMachine(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "DiscoverVirtualMachine", itemId);
        }

        public async System.Threading.Tasks.Task<int> DiscoverVirtualMachineAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "DiscoverVirtualMachine", itemId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineExtendedInfo(int serviceId, string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachineExtendedInfo", serviceId, vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineExtendedInfoAsync(int serviceId, string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachineExtendedInfo", serviceId, vmId);
        }

        public int CancelVirtualMachineJob(string jobId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "CancelVirtualMachineJob", jobId);
        }

        public async System.Threading.Tasks.Task<int> CancelVirtualMachineJobAsync(string jobId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "CancelVirtualMachineJob", jobId);
        }

        public SolidCP.Providers.Common.ResultObject UpdateVirtualMachineHostName(int itemId, string hostname, bool updateNetBIOS)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "UpdateVirtualMachineHostName", itemId, hostname, updateNetBIOS);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateVirtualMachineHostNameAsync(int itemId, string hostname, bool updateNetBIOS)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "UpdateVirtualMachineHostName", itemId, hostname, updateNetBIOS);
        }

        public SolidCP.Providers.Common.ResultObject ChangeVirtualMachineState(int itemId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState state)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "ChangeVirtualMachineState", itemId, state);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeVirtualMachineStateAsync(int itemId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState state)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "ChangeVirtualMachineState", itemId, state);
        }

        public SolidCP.Providers.Virtualization.ConcreteJob[] /*List*/ GetVirtualMachineJobs(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.ConcreteJob[], SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachineJobs", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob[]> GetVirtualMachineJobsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ConcreteJob[], SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachineJobs", itemId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachineNetworkAdapter[] GetVirtualMachinesNetwordAdapterSettings(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachineNetworkAdapter[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachinesNetwordAdapterSettings", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineNetworkAdapter[]> GetVirtualMachinesNetwordAdapterSettingsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachineNetworkAdapter[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachinesNetwordAdapterSettings", itemId);
        }

        public SolidCP.Providers.Common.ResultObject ChangeAdministratorPassword(int itemId, string password)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "ChangeAdministratorPassword", itemId, password);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeAdministratorPasswordAsync(int itemId, string password)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "ChangeAdministratorPassword", itemId, password);
        }

        public SolidCP.Providers.Common.ResultObject ChangeAdministratorPasswordAndCleanResult(int itemId, string password)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "ChangeAdministratorPasswordAndCleanResult", itemId, password);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeAdministratorPasswordAndCleanResultAsync(int itemId, string password)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "ChangeAdministratorPasswordAndCleanResult", itemId, password);
        }

        public SolidCP.Providers.Common.ResultObject UpdateVirtualMachineResource(int itemId, SolidCP.Providers.Virtualization.VirtualMachine vmSettings)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "UpdateVirtualMachineResource", itemId, vmSettings);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateVirtualMachineResourceAsync(int itemId, SolidCP.Providers.Virtualization.VirtualMachine vmSettings)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "UpdateVirtualMachineResource", itemId, vmSettings);
        }

        public SolidCP.Providers.Common.ResultObject UpdateVirtualMachineConfiguration(int itemId, int cpuCores, int ramMB, int[] hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, bool privateNetworkEnabled, SolidCP.Providers.Virtualization.VirtualMachine otherSettings)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "UpdateVirtualMachineConfiguration", itemId, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, privateNetworkEnabled, otherSettings);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateVirtualMachineConfigurationAsync(int itemId, int cpuCores, int ramMB, int[] hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, bool privateNetworkEnabled, SolidCP.Providers.Virtualization.VirtualMachine otherSettings)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "UpdateVirtualMachineConfiguration", itemId, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, privateNetworkEnabled, otherSettings);
        }

        public SolidCP.Providers.Virtualization.LibraryItem GetInsertedDvdDisk(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.LibraryItem>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetInsertedDvdDisk", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem> GetInsertedDvdDiskAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.LibraryItem>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetInsertedDvdDisk", itemId);
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetLibraryDisks(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetLibraryDisks", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetLibraryDisksAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetLibraryDisks", itemId);
        }

        public SolidCP.Providers.Common.ResultObject InsertDvdDisk(int itemId, string isoPath)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "InsertDvdDisk", itemId, isoPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> InsertDvdDiskAsync(int itemId, string isoPath)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "InsertDvdDisk", itemId, isoPath);
        }

        public SolidCP.Providers.Common.ResultObject EjectDvdDisk(int itemId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "EjectDvdDisk", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> EjectDvdDiskAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "EjectDvdDisk", itemId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachineSnapshot[] GetVirtualMachineSnapshots(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachineSnapshots", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]> GetVirtualMachineSnapshotsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachineSnapshots", itemId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachineSnapshot GetSnapshot(int itemId, string snaphostId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachineSnapshot>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetSnapshot", itemId, snaphostId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot> GetSnapshotAsync(int itemId, string snaphostId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachineSnapshot>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetSnapshot", itemId, snaphostId);
        }

        public SolidCP.Providers.Common.ResultObject CreateSnapshot(int itemId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "CreateSnapshot", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateSnapshotAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "CreateSnapshot", itemId);
        }

        public SolidCP.Providers.Common.ResultObject ApplySnapshot(int itemId, string snapshotId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "ApplySnapshot", itemId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ApplySnapshotAsync(int itemId, string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "ApplySnapshot", itemId, snapshotId);
        }

        public SolidCP.Providers.Common.ResultObject RenameSnapshot(int itemId, string snapshotId, string newName)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "RenameSnapshot", itemId, snapshotId, newName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RenameSnapshotAsync(int itemId, string snapshotId, string newName)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "RenameSnapshot", itemId, snapshotId, newName);
        }

        public SolidCP.Providers.Common.ResultObject DeleteSnapshot(int itemId, string snapshotId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "DeleteSnapshot", itemId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteSnapshotAsync(int itemId, string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "DeleteSnapshot", itemId, snapshotId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteSnapshotSubtree(int itemId, string snapshotId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "DeleteSnapshotSubtree", itemId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteSnapshotSubtreeAsync(int itemId, string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "DeleteSnapshotSubtree", itemId, snapshotId);
        }

        public byte[] GetSnapshotThumbnail(int itemId, string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return Invoke<byte[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetSnapshotThumbnail", itemId, snapshotId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailAsync(int itemId, string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return await InvokeAsync<byte[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetSnapshotThumbnail", itemId, snapshotId, size);
        }

        public string GenerateMacAddress()
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GenerateMacAddress");
        }

        public async System.Threading.Tasks.Task<string> GenerateMacAddressAsync()
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GenerateMacAddress");
        }

        public int GetExternalNetworkVLAN(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetExternalNetworkVLAN", itemId);
        }

        public async System.Threading.Tasks.Task<int> GetExternalNetworkVLANAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetExternalNetworkVLAN", itemId);
        }

        public SolidCP.EnterpriseServer.NetworkAdapterDetails GetExternalNetworkAdapterDetails(int itemId)
        {
            return Invoke<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetExternalNetworkAdapterDetails", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetExternalNetworkAdapterDetailsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetExternalNetworkAdapterDetails", itemId);
        }

        public SolidCP.Providers.Common.ResultObject AddVirtualMachineExternalIPAddressesByInjection(int itemId, bool selectRandom, int addressesNumber, int[] addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "AddVirtualMachineExternalIPAddressesByInjection", itemId, selectRandom, addressesNumber, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachineExternalIPAddressesByInjectionAsync(int itemId, bool selectRandom, int addressesNumber, int[] addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "AddVirtualMachineExternalIPAddressesByInjection", itemId, selectRandom, addressesNumber, addressId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteVirtualMachineExternalIPAddressesByInjection(int itemId, int[] addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "DeleteVirtualMachineExternalIPAddressesByInjection", itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineExternalIPAddressesByInjectionAsync(int itemId, int[] addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "DeleteVirtualMachineExternalIPAddressesByInjection", itemId, addressId);
        }

        public SolidCP.Providers.Common.ResultObject RestoreVirtualMachineExternalIPAddressesByInjection(int itemId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "RestoreVirtualMachineExternalIPAddressesByInjection", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RestoreVirtualMachineExternalIPAddressesByInjectionAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "RestoreVirtualMachineExternalIPAddressesByInjection", itemId);
        }

        public SolidCP.Providers.Common.ResultObject AddVirtualMachineExternalIPAddresses(int itemId, bool selectRandom, int addressesNumber, int[] addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "AddVirtualMachineExternalIPAddresses", itemId, selectRandom, addressesNumber, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachineExternalIPAddressesAsync(int itemId, bool selectRandom, int addressesNumber, int[] addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "AddVirtualMachineExternalIPAddresses", itemId, selectRandom, addressesNumber, addressId);
        }

        public SolidCP.Providers.Common.ResultObject SetVirtualMachinePrimaryExternalIPAddress(int itemId, int addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "SetVirtualMachinePrimaryExternalIPAddress", itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetVirtualMachinePrimaryExternalIPAddressAsync(int itemId, int addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "SetVirtualMachinePrimaryExternalIPAddress", itemId, addressId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteVirtualMachineExternalIPAddresses(int itemId, int[] addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "DeleteVirtualMachineExternalIPAddresses", itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineExternalIPAddressesAsync(int itemId, int[] addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "DeleteVirtualMachineExternalIPAddresses", itemId, addressId);
        }

        public SolidCP.EnterpriseServer.NetworkAdapterDetails GetPrivateNetworkAdapterDetails(int itemId)
        {
            return Invoke<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetPrivateNetworkAdapterDetails", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetPrivateNetworkAdapterDetailsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.NetworkAdapterDetails>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetPrivateNetworkAdapterDetails", itemId);
        }

        public SolidCP.Providers.Common.ResultObject RestoreVirtualMachinePrivateIPAddressesByInjection(int itemId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "RestoreVirtualMachinePrivateIPAddressesByInjection", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RestoreVirtualMachinePrivateIPAddressesByInjectionAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "RestoreVirtualMachinePrivateIPAddressesByInjection", itemId);
        }

        public SolidCP.Providers.Common.ResultObject AddVirtualMachinePrivateIPAddressesByInject(int itemId, bool selectRandom, int addressesNumber, string[] addresses, bool customGatewayAndDns, string gateway, string dns1, string dns2, string subnetMask)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "AddVirtualMachinePrivateIPAddressesByInject", itemId, selectRandom, addressesNumber, addresses, customGatewayAndDns, gateway, dns1, dns2, subnetMask);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachinePrivateIPAddressesByInjectAsync(int itemId, bool selectRandom, int addressesNumber, string[] addresses, bool customGatewayAndDns, string gateway, string dns1, string dns2, string subnetMask)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "AddVirtualMachinePrivateIPAddressesByInject", itemId, selectRandom, addressesNumber, addresses, customGatewayAndDns, gateway, dns1, dns2, subnetMask);
        }

        public SolidCP.Providers.Common.ResultObject AddVirtualMachinePrivateIPAddresses(int itemId, bool selectRandom, int addressesNumber, string[] addresses, bool customGatewayAndDns, string gateway, string dns1, string dns2, string subnetMask)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "AddVirtualMachinePrivateIPAddresses", itemId, selectRandom, addressesNumber, addresses, customGatewayAndDns, gateway, dns1, dns2, subnetMask);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachinePrivateIPAddressesAsync(int itemId, bool selectRandom, int addressesNumber, string[] addresses, bool customGatewayAndDns, string gateway, string dns1, string dns2, string subnetMask)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "AddVirtualMachinePrivateIPAddresses", itemId, selectRandom, addressesNumber, addresses, customGatewayAndDns, gateway, dns1, dns2, subnetMask);
        }

        public SolidCP.Providers.Common.ResultObject SetVirtualMachinePrimaryPrivateIPAddress(int itemId, int addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "SetVirtualMachinePrimaryPrivateIPAddress", itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetVirtualMachinePrimaryPrivateIPAddressAsync(int itemId, int addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "SetVirtualMachinePrimaryPrivateIPAddress", itemId, addressId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteVirtualMachinePrivateIPAddressesByInject(int itemId, int[] addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "DeleteVirtualMachinePrivateIPAddressesByInject", itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachinePrivateIPAddressesByInjectAsync(int itemId, int[] addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "DeleteVirtualMachinePrivateIPAddressesByInject", itemId, addressId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteVirtualMachinePrivateIPAddresses(int itemId, int[] addressId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "DeleteVirtualMachinePrivateIPAddresses", itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachinePrivateIPAddressesAsync(int itemId, int[] addressId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "DeleteVirtualMachinePrivateIPAddresses", itemId, addressId);
        }

        public SolidCP.EnterpriseServer.VirtualMachinePermission[] /*List*/ GetVirtualMachinePermissions(int itemId)
        {
            return Invoke<SolidCP.EnterpriseServer.VirtualMachinePermission[], SolidCP.EnterpriseServer.VirtualMachinePermission>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachinePermissions", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.VirtualMachinePermission[]> GetVirtualMachinePermissionsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.VirtualMachinePermission[], SolidCP.EnterpriseServer.VirtualMachinePermission>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachinePermissions", itemId);
        }

        public int UpdateVirtualMachineUserPermissions(int itemId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "UpdateVirtualMachineUserPermissions", itemId, permissions);
        }

        public async System.Threading.Tasks.Task<int> UpdateVirtualMachineUserPermissionsAsync(int itemId, SolidCP.EnterpriseServer.VirtualMachinePermission[] permissions)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "UpdateVirtualMachineUserPermissions", itemId, permissions);
        }

        public SolidCP.Providers.Virtualization.SecureBootTemplate[] GetSecureBootTemplates(int serviceId, string computerName)
        {
            return Invoke<SolidCP.Providers.Virtualization.SecureBootTemplate[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetSecureBootTemplates", serviceId, computerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.SecureBootTemplate[]> GetSecureBootTemplatesAsync(int serviceId, string computerName)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.SecureBootTemplate[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetSecureBootTemplates", serviceId, computerName);
        }

        public SolidCP.Providers.Virtualization.VMConfigurationVersion[] GetVMConfigurationVersionSupportedList(int serviceId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VMConfigurationVersion[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVMConfigurationVersionSupportedList", serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMConfigurationVersion[]> GetVMConfigurationVersionSupportedListAsync(int serviceId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VMConfigurationVersion[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVMConfigurationVersionSupportedList", serviceId);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch[] GetExternalSwitches(int serviceId, string computerName)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualSwitch[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetExternalSwitches", serviceId, computerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetExternalSwitchesAsync(int serviceId, string computerName)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualSwitch[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetExternalSwitches", serviceId, computerName);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch[] GetExternalSwitchesWMI(int serviceId, string computerName)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualSwitch[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetExternalSwitchesWMI", serviceId, computerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetExternalSwitchesWMIAsync(int serviceId, string computerName)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualSwitch[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetExternalSwitchesWMI", serviceId, computerName);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch[] GetInternalSwitches(int serviceId, string computerName)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualSwitch[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetInternalSwitches", serviceId, computerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetInternalSwitchesAsync(int serviceId, string computerName)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualSwitch[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetInternalSwitches", serviceId, computerName);
        }

        public SolidCP.Providers.Common.ResultObject DeleteVirtualMachine(int itemId, bool saveFiles, bool exportVps, string exportPath)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "DeleteVirtualMachine", itemId, saveFiles, exportVps, exportPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineAsync(int itemId, bool saveFiles, bool exportVps, string exportPath)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "DeleteVirtualMachine", itemId, saveFiles, exportVps, exportPath);
        }

        public SolidCP.Providers.Common.ResultObject DeleteVirtualMachineAsynchronous(int itemId, bool saveFiles, bool exportVps, string exportPath)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "DeleteVirtualMachineAsynchronous", itemId, saveFiles, exportVps, exportPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineAsynchronousAsync(int itemId, bool saveFiles, bool exportVps, string exportPath)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "DeleteVirtualMachineAsynchronous", itemId, saveFiles, exportVps, exportPath);
        }

        public SolidCP.Providers.ResultObjects.IntResult ReinstallVirtualMachine(int itemId, SolidCP.Providers.Virtualization.VirtualMachine VMSettings, string adminPassword, string[] privIps, bool saveVirtualDisk, bool exportVps, string exportPath)
        {
            return Invoke<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "ReinstallVirtualMachine", itemId, VMSettings, adminPassword, privIps, saveVirtualDisk, exportVps, exportPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> ReinstallVirtualMachineAsync(int itemId, SolidCP.Providers.Virtualization.VirtualMachine VMSettings, string adminPassword, string[] privIps, bool saveVirtualDisk, bool exportVps, string exportPath)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "ReinstallVirtualMachine", itemId, VMSettings, adminPassword, privIps, saveVirtualDisk, exportVps, exportPath);
        }

        public string GetVirtualMachineSummaryText(int itemId)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachineSummaryText", itemId);
        }

        public async System.Threading.Tasks.Task<string> GetVirtualMachineSummaryTextAsync(int itemId)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetVirtualMachineSummaryText", itemId);
        }

        public SolidCP.Providers.Common.ResultObject SendVirtualMachineSummaryLetter(int itemId, string to, string bcc)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "SendVirtualMachineSummaryLetter", itemId, to, bcc);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SendVirtualMachineSummaryLetterAsync(int itemId, string to, string bcc)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "SendVirtualMachineSummaryLetter", itemId, to, bcc);
        }

        public SolidCP.Providers.Virtualization.CertificateInfo[] GetCertificates(int serviceId, string remoteServer)
        {
            return Invoke<SolidCP.Providers.Virtualization.CertificateInfo[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetCertificates", serviceId, remoteServer);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.CertificateInfo[]> GetCertificatesAsync(int serviceId, string remoteServer)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.CertificateInfo[]>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetCertificates", serviceId, remoteServer);
        }

        public SolidCP.Providers.Common.ResultObject SetReplicaServer(int serviceId, string remoteServer, string thumbprint, string storagePath)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "SetReplicaServer", serviceId, remoteServer, thumbprint, storagePath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetReplicaServerAsync(int serviceId, string remoteServer, string thumbprint, string storagePath)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "SetReplicaServer", serviceId, remoteServer, thumbprint, storagePath);
        }

        public SolidCP.Providers.Common.ResultObject UnsetReplicaServer(int serviceId, string remoteServer)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "UnsetReplicaServer", serviceId, remoteServer);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UnsetReplicaServerAsync(int serviceId, string remoteServer)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "UnsetReplicaServer", serviceId, remoteServer);
        }

        public SolidCP.Providers.Virtualization.ReplicationServerInfo GetReplicaServer(int serviceId, string remoteServer)
        {
            return Invoke<SolidCP.Providers.Virtualization.ReplicationServerInfo>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetReplicaServer", serviceId, remoteServer);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReplicationServerInfo> GetReplicaServerAsync(int serviceId, string remoteServer)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ReplicationServerInfo>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetReplicaServer", serviceId, remoteServer);
        }

        public SolidCP.Providers.Virtualization.VmReplication GetReplication(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VmReplication>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetReplication", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VmReplication> GetReplicationAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VmReplication>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetReplication", itemId);
        }

        public SolidCP.Providers.Virtualization.ReplicationDetailInfo GetReplicationInfo(int itemId)
        {
            return Invoke<SolidCP.Providers.Virtualization.ReplicationDetailInfo>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetReplicationInfo", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReplicationDetailInfo> GetReplicationInfoAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ReplicationDetailInfo>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "GetReplicationInfo", itemId);
        }

        public SolidCP.Providers.Common.ResultObject SetVmReplication(int itemId, SolidCP.Providers.Virtualization.VmReplication replication)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "SetVmReplication", itemId, replication);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetVmReplicationAsync(int itemId, SolidCP.Providers.Virtualization.VmReplication replication)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "SetVmReplication", itemId, replication);
        }

        public SolidCP.Providers.Common.ResultObject DisableVmReplication(int itemId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "DisableVmReplication", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DisableVmReplicationAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "DisableVmReplication", itemId);
        }

        public SolidCP.Providers.Common.ResultObject PauseReplication(int itemId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "PauseReplication", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> PauseReplicationAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "PauseReplication", itemId);
        }

        public SolidCP.Providers.Common.ResultObject ResumeReplication(int itemId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "ResumeReplication", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ResumeReplicationAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esVirtualizationServer2012", "ResumeReplication", itemId);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esVirtualizationServer2012 : SolidCP.Web.Client.ClientBase<IesVirtualizationServer2012, esVirtualizationServer2012AssemblyClient>, IesVirtualizationServer2012
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

        public string GetVirtualMachineGuacamoleURL(int itemId)
        {
            return base.Client.GetVirtualMachineGuacamoleURL(itemId);
        }

        public async System.Threading.Tasks.Task<string> GetVirtualMachineGuacamoleURLAsync(int itemId)
        {
            return await base.Client.GetVirtualMachineGuacamoleURLAsync(itemId);
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

        public SolidCP.Providers.ResultObjects.IntResult CreateNewVirtualMachine(SolidCP.Providers.Virtualization.VirtualMachine VMSettings, string osTemplateFile, string password, string summaryLetterEmail, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses)
        {
            return base.Client.CreateNewVirtualMachine(VMSettings, osTemplateFile, password, summaryLetterEmail, externalAddressesNumber, randomExternalAddresses, externalAddresses, privateAddressesNumber, randomPrivateAddresses, privateAddresses);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> CreateNewVirtualMachineAsync(SolidCP.Providers.Virtualization.VirtualMachine VMSettings, string osTemplateFile, string password, string summaryLetterEmail, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses)
        {
            return await base.Client.CreateNewVirtualMachineAsync(VMSettings, osTemplateFile, password, summaryLetterEmail, externalAddressesNumber, randomExternalAddresses, externalAddresses, privateAddressesNumber, randomPrivateAddresses, privateAddresses);
        }

        public SolidCP.Providers.ResultObjects.IntResult CreateVirtualMachine(int packageId, string hostname, string osTemplateFile, string password, string summaryLetterEmail, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses, bool privateNetworkEnabled, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses, SolidCP.Providers.Virtualization.VirtualMachine otherSettings)
        {
            return base.Client.CreateVirtualMachine(packageId, hostname, osTemplateFile, password, summaryLetterEmail, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, externalAddressesNumber, randomExternalAddresses, externalAddresses, privateNetworkEnabled, privateAddressesNumber, randomPrivateAddresses, privateAddresses, otherSettings);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> CreateVirtualMachineAsync(int packageId, string hostname, string osTemplateFile, string password, string summaryLetterEmail, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses, bool privateNetworkEnabled, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses, SolidCP.Providers.Virtualization.VirtualMachine otherSettings)
        {
            return await base.Client.CreateVirtualMachineAsync(packageId, hostname, osTemplateFile, password, summaryLetterEmail, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, externalAddressesNumber, randomExternalAddresses, externalAddresses, privateNetworkEnabled, privateAddressesNumber, randomPrivateAddresses, privateAddresses, otherSettings);
        }

        public SolidCP.Providers.ResultObjects.IntResult ImportVirtualMachine(int packageId, int serviceId, string vmId, string osTemplateFile, string adminPassword, bool IsBootFromCd, bool IsDvdInstalled, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, string externalNicMacAddress, int[] externalAddresses, string managementNicMacAddress, int managementAddress, int maxSnapshots, bool ignoreChecks)
        {
            return base.Client.ImportVirtualMachine(packageId, serviceId, vmId, osTemplateFile, adminPassword, IsBootFromCd, IsDvdInstalled, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNicMacAddress, externalAddresses, managementNicMacAddress, managementAddress, maxSnapshots, ignoreChecks);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> ImportVirtualMachineAsync(int packageId, int serviceId, string vmId, string osTemplateFile, string adminPassword, bool IsBootFromCd, bool IsDvdInstalled, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, string externalNicMacAddress, int[] externalAddresses, string managementNicMacAddress, int managementAddress, int maxSnapshots, bool ignoreChecks)
        {
            return await base.Client.ImportVirtualMachineAsync(packageId, serviceId, vmId, osTemplateFile, adminPassword, IsBootFromCd, IsDvdInstalled, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNicMacAddress, externalAddresses, managementNicMacAddress, managementAddress, maxSnapshots, ignoreChecks);
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

        public int DiscoverVirtualMachine(int itemId)
        {
            return base.Client.DiscoverVirtualMachine(itemId);
        }

        public async System.Threading.Tasks.Task<int> DiscoverVirtualMachineAsync(int itemId)
        {
            return await base.Client.DiscoverVirtualMachineAsync(itemId);
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

        public SolidCP.Providers.Virtualization.VirtualMachineNetworkAdapter[] GetVirtualMachinesNetwordAdapterSettings(int itemId)
        {
            return base.Client.GetVirtualMachinesNetwordAdapterSettings(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineNetworkAdapter[]> GetVirtualMachinesNetwordAdapterSettingsAsync(int itemId)
        {
            return await base.Client.GetVirtualMachinesNetwordAdapterSettingsAsync(itemId);
        }

        public SolidCP.Providers.Common.ResultObject ChangeAdministratorPassword(int itemId, string password)
        {
            return base.Client.ChangeAdministratorPassword(itemId, password);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeAdministratorPasswordAsync(int itemId, string password)
        {
            return await base.Client.ChangeAdministratorPasswordAsync(itemId, password);
        }

        public SolidCP.Providers.Common.ResultObject ChangeAdministratorPasswordAndCleanResult(int itemId, string password)
        {
            return base.Client.ChangeAdministratorPasswordAndCleanResult(itemId, password);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeAdministratorPasswordAndCleanResultAsync(int itemId, string password)
        {
            return await base.Client.ChangeAdministratorPasswordAndCleanResultAsync(itemId, password);
        }

        public SolidCP.Providers.Common.ResultObject UpdateVirtualMachineResource(int itemId, SolidCP.Providers.Virtualization.VirtualMachine vmSettings)
        {
            return base.Client.UpdateVirtualMachineResource(itemId, vmSettings);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateVirtualMachineResourceAsync(int itemId, SolidCP.Providers.Virtualization.VirtualMachine vmSettings)
        {
            return await base.Client.UpdateVirtualMachineResourceAsync(itemId, vmSettings);
        }

        public SolidCP.Providers.Common.ResultObject UpdateVirtualMachineConfiguration(int itemId, int cpuCores, int ramMB, int[] hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, bool privateNetworkEnabled, SolidCP.Providers.Virtualization.VirtualMachine otherSettings)
        {
            return base.Client.UpdateVirtualMachineConfiguration(itemId, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock, startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNetworkEnabled, privateNetworkEnabled, otherSettings);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateVirtualMachineConfigurationAsync(int itemId, int cpuCores, int ramMB, int[] hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, bool privateNetworkEnabled, SolidCP.Providers.Virtualization.VirtualMachine otherSettings)
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

        public string GenerateMacAddress()
        {
            return base.Client.GenerateMacAddress();
        }

        public async System.Threading.Tasks.Task<string> GenerateMacAddressAsync()
        {
            return await base.Client.GenerateMacAddressAsync();
        }

        public int GetExternalNetworkVLAN(int itemId)
        {
            return base.Client.GetExternalNetworkVLAN(itemId);
        }

        public async System.Threading.Tasks.Task<int> GetExternalNetworkVLANAsync(int itemId)
        {
            return await base.Client.GetExternalNetworkVLANAsync(itemId);
        }

        public SolidCP.EnterpriseServer.NetworkAdapterDetails GetExternalNetworkAdapterDetails(int itemId)
        {
            return base.Client.GetExternalNetworkAdapterDetails(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.NetworkAdapterDetails> GetExternalNetworkAdapterDetailsAsync(int itemId)
        {
            return await base.Client.GetExternalNetworkAdapterDetailsAsync(itemId);
        }

        public SolidCP.Providers.Common.ResultObject AddVirtualMachineExternalIPAddressesByInjection(int itemId, bool selectRandom, int addressesNumber, int[] addressId)
        {
            return base.Client.AddVirtualMachineExternalIPAddressesByInjection(itemId, selectRandom, addressesNumber, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachineExternalIPAddressesByInjectionAsync(int itemId, bool selectRandom, int addressesNumber, int[] addressId)
        {
            return await base.Client.AddVirtualMachineExternalIPAddressesByInjectionAsync(itemId, selectRandom, addressesNumber, addressId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteVirtualMachineExternalIPAddressesByInjection(int itemId, int[] addressId)
        {
            return base.Client.DeleteVirtualMachineExternalIPAddressesByInjection(itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineExternalIPAddressesByInjectionAsync(int itemId, int[] addressId)
        {
            return await base.Client.DeleteVirtualMachineExternalIPAddressesByInjectionAsync(itemId, addressId);
        }

        public SolidCP.Providers.Common.ResultObject RestoreVirtualMachineExternalIPAddressesByInjection(int itemId)
        {
            return base.Client.RestoreVirtualMachineExternalIPAddressesByInjection(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RestoreVirtualMachineExternalIPAddressesByInjectionAsync(int itemId)
        {
            return await base.Client.RestoreVirtualMachineExternalIPAddressesByInjectionAsync(itemId);
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

        public SolidCP.Providers.Common.ResultObject RestoreVirtualMachinePrivateIPAddressesByInjection(int itemId)
        {
            return base.Client.RestoreVirtualMachinePrivateIPAddressesByInjection(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RestoreVirtualMachinePrivateIPAddressesByInjectionAsync(int itemId)
        {
            return await base.Client.RestoreVirtualMachinePrivateIPAddressesByInjectionAsync(itemId);
        }

        public SolidCP.Providers.Common.ResultObject AddVirtualMachinePrivateIPAddressesByInject(int itemId, bool selectRandom, int addressesNumber, string[] addresses, bool customGatewayAndDns, string gateway, string dns1, string dns2, string subnetMask)
        {
            return base.Client.AddVirtualMachinePrivateIPAddressesByInject(itemId, selectRandom, addressesNumber, addresses, customGatewayAndDns, gateway, dns1, dns2, subnetMask);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachinePrivateIPAddressesByInjectAsync(int itemId, bool selectRandom, int addressesNumber, string[] addresses, bool customGatewayAndDns, string gateway, string dns1, string dns2, string subnetMask)
        {
            return await base.Client.AddVirtualMachinePrivateIPAddressesByInjectAsync(itemId, selectRandom, addressesNumber, addresses, customGatewayAndDns, gateway, dns1, dns2, subnetMask);
        }

        public SolidCP.Providers.Common.ResultObject AddVirtualMachinePrivateIPAddresses(int itemId, bool selectRandom, int addressesNumber, string[] addresses, bool customGatewayAndDns, string gateway, string dns1, string dns2, string subnetMask)
        {
            return base.Client.AddVirtualMachinePrivateIPAddresses(itemId, selectRandom, addressesNumber, addresses, customGatewayAndDns, gateway, dns1, dns2, subnetMask);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddVirtualMachinePrivateIPAddressesAsync(int itemId, bool selectRandom, int addressesNumber, string[] addresses, bool customGatewayAndDns, string gateway, string dns1, string dns2, string subnetMask)
        {
            return await base.Client.AddVirtualMachinePrivateIPAddressesAsync(itemId, selectRandom, addressesNumber, addresses, customGatewayAndDns, gateway, dns1, dns2, subnetMask);
        }

        public SolidCP.Providers.Common.ResultObject SetVirtualMachinePrimaryPrivateIPAddress(int itemId, int addressId)
        {
            return base.Client.SetVirtualMachinePrimaryPrivateIPAddress(itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetVirtualMachinePrimaryPrivateIPAddressAsync(int itemId, int addressId)
        {
            return await base.Client.SetVirtualMachinePrimaryPrivateIPAddressAsync(itemId, addressId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteVirtualMachinePrivateIPAddressesByInject(int itemId, int[] addressId)
        {
            return base.Client.DeleteVirtualMachinePrivateIPAddressesByInject(itemId, addressId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachinePrivateIPAddressesByInjectAsync(int itemId, int[] addressId)
        {
            return await base.Client.DeleteVirtualMachinePrivateIPAddressesByInjectAsync(itemId, addressId);
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

        public SolidCP.Providers.Virtualization.SecureBootTemplate[] GetSecureBootTemplates(int serviceId, string computerName)
        {
            return base.Client.GetSecureBootTemplates(serviceId, computerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.SecureBootTemplate[]> GetSecureBootTemplatesAsync(int serviceId, string computerName)
        {
            return await base.Client.GetSecureBootTemplatesAsync(serviceId, computerName);
        }

        public SolidCP.Providers.Virtualization.VMConfigurationVersion[] GetVMConfigurationVersionSupportedList(int serviceId)
        {
            return base.Client.GetVMConfigurationVersionSupportedList(serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMConfigurationVersion[]> GetVMConfigurationVersionSupportedListAsync(int serviceId)
        {
            return await base.Client.GetVMConfigurationVersionSupportedListAsync(serviceId);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch[] GetExternalSwitches(int serviceId, string computerName)
        {
            return base.Client.GetExternalSwitches(serviceId, computerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetExternalSwitchesAsync(int serviceId, string computerName)
        {
            return await base.Client.GetExternalSwitchesAsync(serviceId, computerName);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch[] GetExternalSwitchesWMI(int serviceId, string computerName)
        {
            return base.Client.GetExternalSwitchesWMI(serviceId, computerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetExternalSwitchesWMIAsync(int serviceId, string computerName)
        {
            return await base.Client.GetExternalSwitchesWMIAsync(serviceId, computerName);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch[] GetInternalSwitches(int serviceId, string computerName)
        {
            return base.Client.GetInternalSwitches(serviceId, computerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetInternalSwitchesAsync(int serviceId, string computerName)
        {
            return await base.Client.GetInternalSwitchesAsync(serviceId, computerName);
        }

        public SolidCP.Providers.Common.ResultObject DeleteVirtualMachine(int itemId, bool saveFiles, bool exportVps, string exportPath)
        {
            return base.Client.DeleteVirtualMachine(itemId, saveFiles, exportVps, exportPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineAsync(int itemId, bool saveFiles, bool exportVps, string exportPath)
        {
            return await base.Client.DeleteVirtualMachineAsync(itemId, saveFiles, exportVps, exportPath);
        }

        public SolidCP.Providers.Common.ResultObject DeleteVirtualMachineAsynchronous(int itemId, bool saveFiles, bool exportVps, string exportPath)
        {
            return base.Client.DeleteVirtualMachineAsynchronous(itemId, saveFiles, exportVps, exportPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteVirtualMachineAsynchronousAsync(int itemId, bool saveFiles, bool exportVps, string exportPath)
        {
            return await base.Client.DeleteVirtualMachineAsynchronousAsync(itemId, saveFiles, exportVps, exportPath);
        }

        public SolidCP.Providers.ResultObjects.IntResult ReinstallVirtualMachine(int itemId, SolidCP.Providers.Virtualization.VirtualMachine VMSettings, string adminPassword, string[] privIps, bool saveVirtualDisk, bool exportVps, string exportPath)
        {
            return base.Client.ReinstallVirtualMachine(itemId, VMSettings, adminPassword, privIps, saveVirtualDisk, exportVps, exportPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> ReinstallVirtualMachineAsync(int itemId, SolidCP.Providers.Virtualization.VirtualMachine VMSettings, string adminPassword, string[] privIps, bool saveVirtualDisk, bool exportVps, string exportPath)
        {
            return await base.Client.ReinstallVirtualMachineAsync(itemId, VMSettings, adminPassword, privIps, saveVirtualDisk, exportVps, exportPath);
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