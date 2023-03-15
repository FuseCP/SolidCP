#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("ServerPolicy")]
    [SolidCP.Providers.SoapHeader]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IVirtualizationServer2012", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IVirtualizationServer2012
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachine(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineEx", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineExResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineEx(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineEx", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineExResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineExAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachines", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachinesResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine[] /*List*/ GetVirtualMachines();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachines", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachinesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine[]> GetVirtualMachinesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineThumbnailImageResponse")]
        byte[] GetVirtualMachineThumbnailImage(string vmId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineThumbnailImageResponse")]
        System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailImageAsync(string vmId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine CreateVirtualMachine(SolidCP.Providers.Virtualization.VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> CreateVirtualMachineAsync(SolidCP.Providers.Virtualization.VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/UpdateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/UpdateVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine UpdateVirtualMachine(SolidCP.Providers.Virtualization.VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/UpdateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/UpdateVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> UpdateVirtualMachineAsync(SolidCP.Providers.Virtualization.VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ChangeVirtualMachineState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ChangeVirtualMachineStateResponse")]
        SolidCP.Providers.Virtualization.JobResult ChangeVirtualMachineState(string vmId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState newState, string clusterName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ChangeVirtualMachineState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ChangeVirtualMachineStateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ChangeVirtualMachineStateAsync(string vmId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState newState, string clusterName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ShutDownVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ShutDownVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ShutDownVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ShutDownVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> ShutDownVirtualMachineAsync(string vmId, bool force, string reason);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineJobsResponse")]
        SolidCP.Providers.Virtualization.ConcreteJob[] /*List*/ GetVirtualMachineJobs(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineJobsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob[]> GetVirtualMachineJobsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/RenameVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/RenameVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.JobResult RenameVirtualMachine(string vmId, string name, string clusterName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/RenameVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/RenameVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RenameVirtualMachineAsync(string vmId, string name, string clusterName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.JobResult DeleteVirtualMachine(string vmId, string clusterName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteVirtualMachineAsync(string vmId, string clusterName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteVirtualMachineExtended", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteVirtualMachineExtendedResponse")]
        SolidCP.Providers.Virtualization.JobResult DeleteVirtualMachineExtended(string vmId, string clusterName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteVirtualMachineExtended", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteVirtualMachineExtendedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteVirtualMachineExtendedAsync(string vmId, string clusterName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExportVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExportVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.JobResult ExportVirtualMachine(string vmId, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExportVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExportVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ExportVirtualMachineAsync(string vmId, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/IsTryToUpdateVirtualMachineWithoutRebootSuccess", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/IsTryToUpdateVirtualMachineWithoutRebootSuccessResponse")]
        bool IsTryToUpdateVirtualMachineWithoutRebootSuccess(SolidCP.Providers.Virtualization.VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/IsTryToUpdateVirtualMachineWithoutRebootSuccess", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/IsTryToUpdateVirtualMachineWithoutRebootSuccessResponse")]
        System.Threading.Tasks.Task<bool> IsTryToUpdateVirtualMachineWithoutRebootSuccessAsync(SolidCP.Providers.Virtualization.VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineSnapshots", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineSnapshotsResponse")]
        SolidCP.Providers.Virtualization.VirtualMachineSnapshot[] /*List*/ GetVirtualMachineSnapshots(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineSnapshots", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineSnapshotsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]> GetVirtualMachineSnapshotsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSnapshotResponse")]
        SolidCP.Providers.Virtualization.VirtualMachineSnapshot GetSnapshot(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot> GetSnapshotAsync(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateSnapshotResponse")]
        SolidCP.Providers.Virtualization.JobResult CreateSnapshot(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> CreateSnapshotAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/RenameSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/RenameSnapshotResponse")]
        SolidCP.Providers.Virtualization.JobResult RenameSnapshot(string vmId, string snapshotId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/RenameSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/RenameSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RenameSnapshotAsync(string vmId, string snapshotId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ApplySnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ApplySnapshotResponse")]
        SolidCP.Providers.Virtualization.JobResult ApplySnapshot(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ApplySnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ApplySnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ApplySnapshotAsync(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteSnapshotResponse")]
        SolidCP.Providers.Virtualization.JobResult DeleteSnapshot(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteSnapshotAsync(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteSnapshotSubtree", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteSnapshotSubtreeResponse")]
        SolidCP.Providers.Virtualization.JobResult DeleteSnapshotSubtree(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteSnapshotSubtree", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteSnapshotSubtreeResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteSnapshotSubtreeAsync(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSnapshotThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSnapshotThumbnailImageResponse")]
        byte[] GetSnapshotThumbnailImage(string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSnapshotThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSnapshotThumbnailImageResponse")]
        System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailImageAsync(string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSecureBootTemplates", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSecureBootTemplatesResponse")]
        SolidCP.Providers.Virtualization.SecureBootTemplate[] /*List*/ GetSecureBootTemplates(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSecureBootTemplates", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSecureBootTemplatesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.SecureBootTemplate[]> GetSecureBootTemplatesAsync(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetExternalSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetExternalSwitchesResponse")]
        SolidCP.Providers.Virtualization.VirtualSwitch[] /*List*/ GetExternalSwitches(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetExternalSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetExternalSwitchesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetExternalSwitchesAsync(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetExternalSwitchesWMI", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetExternalSwitchesWMIResponse")]
        SolidCP.Providers.Virtualization.VirtualSwitch[] /*List*/ GetExternalSwitchesWMI(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetExternalSwitchesWMI", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetExternalSwitchesWMIResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetExternalSwitchesWMIAsync(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetInternalSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetInternalSwitchesResponse")]
        SolidCP.Providers.Virtualization.VirtualSwitch[] /*List*/ GetInternalSwitches(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetInternalSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetInternalSwitchesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetInternalSwitchesAsync(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSwitchesResponse")]
        SolidCP.Providers.Virtualization.VirtualSwitch[] /*List*/ GetSwitches();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSwitchesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetSwitchesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/SwitchExists", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/SwitchExistsResponse")]
        bool SwitchExists(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/SwitchExists", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/SwitchExistsResponse")]
        System.Threading.Tasks.Task<bool> SwitchExistsAsync(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateSwitchResponse")]
        SolidCP.Providers.Virtualization.VirtualSwitch CreateSwitch(string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateSwitchResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch> CreateSwitchAsync(string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteSwitchResponse")]
        SolidCP.Providers.Virtualization.ReturnCode DeleteSwitch(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteSwitchResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> DeleteSwitchAsync(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachinesNetwordAdapterSettings", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachinesNetwordAdapterSettingsResponse")]
        SolidCP.Providers.Virtualization.VirtualMachineNetworkAdapter[] /*List*/ GetVirtualMachinesNetwordAdapterSettings(string vmName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachinesNetwordAdapterSettings", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachinesNetwordAdapterSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineNetworkAdapter[]> GetVirtualMachinesNetwordAdapterSettingsAsync(string vmName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/InjectIPs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/InjectIPsResponse")]
        SolidCP.Providers.Virtualization.JobResult InjectIPs(string vmId, SolidCP.Providers.Virtualization.GuestNetworkAdapterConfiguration guestNetworkAdapterConfiguration);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/InjectIPs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/InjectIPsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> InjectIPsAsync(string vmId, SolidCP.Providers.Virtualization.GuestNetworkAdapterConfiguration guestNetworkAdapterConfiguration);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetInsertedDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetInsertedDVDResponse")]
        string GetInsertedDVD(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetInsertedDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetInsertedDVDResponse")]
        System.Threading.Tasks.Task<string> GetInsertedDVDAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/InsertDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/InsertDVDResponse")]
        SolidCP.Providers.Virtualization.JobResult InsertDVD(string vmId, string isoPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/InsertDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/InsertDVDResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> InsertDVDAsync(string vmId, string isoPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/EjectDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/EjectDVDResponse")]
        SolidCP.Providers.Virtualization.JobResult EjectDVD(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/EjectDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/EjectDVDResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> EjectDVDAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetKVPItemsResponse")]
        SolidCP.Providers.Virtualization.KvpExchangeDataItem[] /*List*/ GetKVPItems(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetKVPItemsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.KvpExchangeDataItem[]> GetKVPItemsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetStandardKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetStandardKVPItemsResponse")]
        SolidCP.Providers.Virtualization.KvpExchangeDataItem[] /*List*/ GetStandardKVPItems(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetStandardKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetStandardKVPItemsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.KvpExchangeDataItem[]> GetStandardKVPItemsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/AddKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/AddKVPItemsResponse")]
        SolidCP.Providers.Virtualization.JobResult AddKVPItems(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/AddKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/AddKVPItemsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> AddKVPItemsAsync(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/RemoveKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/RemoveKVPItemsResponse")]
        SolidCP.Providers.Virtualization.JobResult RemoveKVPItems(string vmId, string[] itemNames);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/RemoveKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/RemoveKVPItemsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RemoveKVPItemsAsync(string vmId, string[] itemNames);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ModifyKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ModifyKVPItemsResponse")]
        SolidCP.Providers.Virtualization.JobResult ModifyKVPItems(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ModifyKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ModifyKVPItemsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ModifyKVPItemsAsync(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/IsEmptyFolders", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/IsEmptyFoldersResponse")]
        bool IsEmptyFolders(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/IsEmptyFolders", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/IsEmptyFoldersResponse")]
        System.Threading.Tasks.Task<bool> IsEmptyFoldersAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/FileExists", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/FileExistsResponse")]
        bool FileExists(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/FileExists", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/FileExistsResponse")]
        System.Threading.Tasks.Task<bool> FileExistsAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualHardDiskInfo", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualHardDiskInfoResponse")]
        SolidCP.Providers.Virtualization.VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualHardDiskInfo", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualHardDiskInfoResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualHardDiskInfo> GetVirtualHardDiskInfoAsync(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/MountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/MountVirtualHardDiskResponse")]
        SolidCP.Providers.Virtualization.MountedDiskInfo MountVirtualHardDisk(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/MountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/MountVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.MountedDiskInfo> MountVirtualHardDiskAsync(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/UnmountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/UnmountVirtualHardDiskResponse")]
        SolidCP.Providers.Virtualization.ReturnCode UnmountVirtualHardDisk(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/UnmountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/UnmountVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> UnmountVirtualHardDiskAsync(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExpandVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExpandVirtualHardDiskResponse")]
        SolidCP.Providers.Virtualization.JobResult ExpandVirtualHardDisk(string vhdPath, System.UInt64 sizeGB);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExpandVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExpandVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ExpandVirtualHardDiskAsync(string vhdPath, System.UInt64 sizeGB);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ConvertVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ConvertVirtualHardDiskResponse")]
        SolidCP.Providers.Virtualization.JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType, uint blockSizeBytes);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ConvertVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ConvertVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ConvertVirtualHardDiskAsync(string sourcePath, string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType, uint blockSizeBytes);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateVirtualHardDiskResponse")]
        SolidCP.Providers.Virtualization.JobResult CreateVirtualHardDisk(string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType, uint blockSizeBytes, System.UInt64 sizeGB);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> CreateVirtualHardDiskAsync(string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType, uint blockSizeBytes, System.UInt64 sizeGB);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteRemoteFile", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteRemoteFileResponse")]
        void DeleteRemoteFile(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteRemoteFile", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteRemoteFileResponse")]
        System.Threading.Tasks.Task DeleteRemoteFileAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExpandDiskVolume", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExpandDiskVolumeResponse")]
        void ExpandDiskVolume(string diskAddress, string volumeName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExpandDiskVolume", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExpandDiskVolumeResponse")]
        System.Threading.Tasks.Task ExpandDiskVolumeAsync(string diskAddress, string volumeName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ReadRemoteFile", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ReadRemoteFileResponse")]
        string ReadRemoteFile(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ReadRemoteFile", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ReadRemoteFileResponse")]
        System.Threading.Tasks.Task<string> ReadRemoteFileAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/WriteRemoteFile", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/WriteRemoteFileResponse")]
        void WriteRemoteFile(string path, string content);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/WriteRemoteFile", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/WriteRemoteFileResponse")]
        System.Threading.Tasks.Task WriteRemoteFileAsync(string path, string content);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetJob", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetJobResponse")]
        SolidCP.Providers.Virtualization.ConcreteJob GetJob(string jobId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetJob", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetJobResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob> GetJobAsync(string jobId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetAllJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetAllJobsResponse")]
        SolidCP.Providers.Virtualization.ConcreteJob[] /*List*/ GetAllJobs();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetAllJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetAllJobsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob[]> GetAllJobsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ClearOldJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ClearOldJobsResponse")]
        void ClearOldJobs();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ClearOldJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ClearOldJobsResponse")]
        System.Threading.Tasks.Task ClearOldJobsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ChangeJobState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ChangeJobStateResponse")]
        SolidCP.Providers.Virtualization.ChangeJobStateReturnCode ChangeJobState(string jobId, SolidCP.Providers.Virtualization.ConcreteJobRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ChangeJobState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ChangeJobStateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ChangeJobStateReturnCode> ChangeJobStateAsync(string jobId, SolidCP.Providers.Virtualization.ConcreteJobRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetProcessorCoresNumber", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetProcessorCoresNumberResponse")]
        int GetProcessorCoresNumber();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetProcessorCoresNumber", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetProcessorCoresNumberResponse")]
        System.Threading.Tasks.Task<int> GetProcessorCoresNumberAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVMConfigurationVersionSupportedList", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVMConfigurationVersionSupportedListResponse")]
        SolidCP.Providers.Virtualization.VMConfigurationVersion[] /*List*/ GetVMConfigurationVersionSupportedList();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVMConfigurationVersionSupportedList", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVMConfigurationVersionSupportedListResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMConfigurationVersion[]> GetVMConfigurationVersionSupportedListAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetCertificates", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetCertificatesResponse")]
        SolidCP.Providers.Virtualization.CertificateInfo[] /*List*/ GetCertificates(string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetCertificates", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetCertificatesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.CertificateInfo[]> GetCertificatesAsync(string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/SetReplicaServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/SetReplicaServerResponse")]
        void SetReplicaServer(string remoteServer, string thumbprint, string storagePath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/SetReplicaServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/SetReplicaServerResponse")]
        System.Threading.Tasks.Task SetReplicaServerAsync(string remoteServer, string thumbprint, string storagePath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/UnsetReplicaServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/UnsetReplicaServerResponse")]
        void UnsetReplicaServer(string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/UnsetReplicaServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/UnsetReplicaServerResponse")]
        System.Threading.Tasks.Task UnsetReplicaServerAsync(string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetReplicaServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetReplicaServerResponse")]
        SolidCP.Providers.Virtualization.ReplicationServerInfo GetReplicaServer(string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetReplicaServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetReplicaServerResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReplicationServerInfo> GetReplicaServerAsync(string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/EnableVmReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/EnableVmReplicationResponse")]
        void EnableVmReplication(string vmId, string replicaServer, SolidCP.Providers.Virtualization.VmReplication replication);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/EnableVmReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/EnableVmReplicationResponse")]
        System.Threading.Tasks.Task EnableVmReplicationAsync(string vmId, string replicaServer, SolidCP.Providers.Virtualization.VmReplication replication);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/SetVmReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/SetVmReplicationResponse")]
        void SetVmReplication(string vmId, string replicaServer, SolidCP.Providers.Virtualization.VmReplication replication);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/SetVmReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/SetVmReplicationResponse")]
        System.Threading.Tasks.Task SetVmReplicationAsync(string vmId, string replicaServer, SolidCP.Providers.Virtualization.VmReplication replication);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/TestReplicationServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/TestReplicationServerResponse")]
        void TestReplicationServer(string vmId, string replicaServer, string localThumbprint);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/TestReplicationServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/TestReplicationServerResponse")]
        System.Threading.Tasks.Task TestReplicationServerAsync(string vmId, string replicaServer, string localThumbprint);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/StartInitialReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/StartInitialReplicationResponse")]
        void StartInitialReplication(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/StartInitialReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/StartInitialReplicationResponse")]
        System.Threading.Tasks.Task StartInitialReplicationAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetReplicationResponse")]
        SolidCP.Providers.Virtualization.VmReplication GetReplication(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetReplicationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VmReplication> GetReplicationAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DisableVmReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DisableVmReplicationResponse")]
        void DisableVmReplication(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DisableVmReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DisableVmReplicationResponse")]
        System.Threading.Tasks.Task DisableVmReplicationAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetReplicationInfo", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetReplicationInfoResponse")]
        SolidCP.Providers.Virtualization.ReplicationDetailInfo GetReplicationInfo(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetReplicationInfo", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetReplicationInfoResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReplicationDetailInfo> GetReplicationInfoAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/PauseReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/PauseReplicationResponse")]
        void PauseReplication(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/PauseReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/PauseReplicationResponse")]
        System.Threading.Tasks.Task PauseReplicationAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ResumeReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ResumeReplicationResponse")]
        void ResumeReplication(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ResumeReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ResumeReplicationResponse")]
        System.Threading.Tasks.Task ResumeReplicationAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExecuteCustomPsScript", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExecuteCustomPsScriptResponse")]
        SolidCP.Providers.Virtualization.JobResult ExecuteCustomPsScript(string script);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExecuteCustomPsScript", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExecuteCustomPsScriptResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ExecuteCustomPsScriptAsync(string script);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class VirtualizationServer2012AssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IVirtualizationServer2012
    {
        public SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachine(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachine", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachine", vmId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineEx(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachineEx", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineExAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachineEx", vmId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine[] /*List*/ GetVirtualMachines()
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine[], SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachines");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine[]> GetVirtualMachinesAsync()
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine[], SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachines");
        }

        public byte[] GetVirtualMachineThumbnailImage(string vmId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return Invoke<byte[]>("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachineThumbnailImage", vmId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailImageAsync(string vmId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachineThumbnailImage", vmId, size);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine CreateVirtualMachine(SolidCP.Providers.Virtualization.VirtualMachine vm)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServer2012", "CreateVirtualMachine", vm);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> CreateVirtualMachineAsync(SolidCP.Providers.Virtualization.VirtualMachine vm)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServer2012", "CreateVirtualMachine", vm);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine UpdateVirtualMachine(SolidCP.Providers.Virtualization.VirtualMachine vm)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServer2012", "UpdateVirtualMachine", vm);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> UpdateVirtualMachineAsync(SolidCP.Providers.Virtualization.VirtualMachine vm)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServer2012", "UpdateVirtualMachine", vm);
        }

        public SolidCP.Providers.Virtualization.JobResult ChangeVirtualMachineState(string vmId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState newState, string clusterName)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "ChangeVirtualMachineState", vmId, newState, clusterName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ChangeVirtualMachineStateAsync(string vmId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState newState, string clusterName)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "ChangeVirtualMachineState", vmId, newState, clusterName);
        }

        public SolidCP.Providers.Virtualization.ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason)
        {
            return Invoke<SolidCP.Providers.Virtualization.ReturnCode>("SolidCP.Server.VirtualizationServer2012", "ShutDownVirtualMachine", vmId, force, reason);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> ShutDownVirtualMachineAsync(string vmId, bool force, string reason)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ReturnCode>("SolidCP.Server.VirtualizationServer2012", "ShutDownVirtualMachine", vmId, force, reason);
        }

        public SolidCP.Providers.Virtualization.ConcreteJob[] /*List*/ GetVirtualMachineJobs(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.ConcreteJob[], SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachineJobs", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob[]> GetVirtualMachineJobsAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ConcreteJob[], SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachineJobs", vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult RenameVirtualMachine(string vmId, string name, string clusterName)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "RenameVirtualMachine", vmId, name, clusterName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RenameVirtualMachineAsync(string vmId, string name, string clusterName)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "RenameVirtualMachine", vmId, name, clusterName);
        }

        public SolidCP.Providers.Virtualization.JobResult DeleteVirtualMachine(string vmId, string clusterName)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "DeleteVirtualMachine", vmId, clusterName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteVirtualMachineAsync(string vmId, string clusterName)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "DeleteVirtualMachine", vmId, clusterName);
        }

        public SolidCP.Providers.Virtualization.JobResult DeleteVirtualMachineExtended(string vmId, string clusterName)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "DeleteVirtualMachineExtended", vmId, clusterName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteVirtualMachineExtendedAsync(string vmId, string clusterName)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "DeleteVirtualMachineExtended", vmId, clusterName);
        }

        public SolidCP.Providers.Virtualization.JobResult ExportVirtualMachine(string vmId, string exportPath)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "ExportVirtualMachine", vmId, exportPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ExportVirtualMachineAsync(string vmId, string exportPath)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "ExportVirtualMachine", vmId, exportPath);
        }

        public bool IsTryToUpdateVirtualMachineWithoutRebootSuccess(SolidCP.Providers.Virtualization.VirtualMachine vm)
        {
            return Invoke<bool>("SolidCP.Server.VirtualizationServer2012", "IsTryToUpdateVirtualMachineWithoutRebootSuccess", vm);
        }

        public async System.Threading.Tasks.Task<bool> IsTryToUpdateVirtualMachineWithoutRebootSuccessAsync(SolidCP.Providers.Virtualization.VirtualMachine vm)
        {
            return await InvokeAsync<bool>("SolidCP.Server.VirtualizationServer2012", "IsTryToUpdateVirtualMachineWithoutRebootSuccess", vm);
        }

        public SolidCP.Providers.Virtualization.VirtualMachineSnapshot[] /*List*/ GetVirtualMachineSnapshots(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[], SolidCP.Providers.Virtualization.VirtualMachineSnapshot>("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachineSnapshots", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]> GetVirtualMachineSnapshotsAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[], SolidCP.Providers.Virtualization.VirtualMachineSnapshot>("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachineSnapshots", vmId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachineSnapshot GetSnapshot(string snapshotId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachineSnapshot>("SolidCP.Server.VirtualizationServer2012", "GetSnapshot", snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot> GetSnapshotAsync(string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachineSnapshot>("SolidCP.Server.VirtualizationServer2012", "GetSnapshot", snapshotId);
        }

        public SolidCP.Providers.Virtualization.JobResult CreateSnapshot(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "CreateSnapshot", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> CreateSnapshotAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "CreateSnapshot", vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult RenameSnapshot(string vmId, string snapshotId, string name)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "RenameSnapshot", vmId, snapshotId, name);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RenameSnapshotAsync(string vmId, string snapshotId, string name)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "RenameSnapshot", vmId, snapshotId, name);
        }

        public SolidCP.Providers.Virtualization.JobResult ApplySnapshot(string vmId, string snapshotId)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "ApplySnapshot", vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ApplySnapshotAsync(string vmId, string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "ApplySnapshot", vmId, snapshotId);
        }

        public SolidCP.Providers.Virtualization.JobResult DeleteSnapshot(string snapshotId)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "DeleteSnapshot", snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteSnapshotAsync(string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "DeleteSnapshot", snapshotId);
        }

        public SolidCP.Providers.Virtualization.JobResult DeleteSnapshotSubtree(string snapshotId)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "DeleteSnapshotSubtree", snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteSnapshotSubtreeAsync(string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "DeleteSnapshotSubtree", snapshotId);
        }

        public byte[] GetSnapshotThumbnailImage(string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return Invoke<byte[]>("SolidCP.Server.VirtualizationServer2012", "GetSnapshotThumbnailImage", snapshotId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailImageAsync(string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.VirtualizationServer2012", "GetSnapshotThumbnailImage", snapshotId, size);
        }

        public SolidCP.Providers.Virtualization.SecureBootTemplate[] /*List*/ GetSecureBootTemplates(string computerName)
        {
            return Invoke<SolidCP.Providers.Virtualization.SecureBootTemplate[], SolidCP.Providers.Virtualization.SecureBootTemplate>("SolidCP.Server.VirtualizationServer2012", "GetSecureBootTemplates", computerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.SecureBootTemplate[]> GetSecureBootTemplatesAsync(string computerName)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.SecureBootTemplate[], SolidCP.Providers.Virtualization.SecureBootTemplate>("SolidCP.Server.VirtualizationServer2012", "GetSecureBootTemplates", computerName);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch[] /*List*/ GetExternalSwitches(string computerName)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualSwitch[], SolidCP.Providers.Virtualization.VirtualSwitch>("SolidCP.Server.VirtualizationServer2012", "GetExternalSwitches", computerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetExternalSwitchesAsync(string computerName)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualSwitch[], SolidCP.Providers.Virtualization.VirtualSwitch>("SolidCP.Server.VirtualizationServer2012", "GetExternalSwitches", computerName);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch[] /*List*/ GetExternalSwitchesWMI(string computerName)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualSwitch[], SolidCP.Providers.Virtualization.VirtualSwitch>("SolidCP.Server.VirtualizationServer2012", "GetExternalSwitchesWMI", computerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetExternalSwitchesWMIAsync(string computerName)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualSwitch[], SolidCP.Providers.Virtualization.VirtualSwitch>("SolidCP.Server.VirtualizationServer2012", "GetExternalSwitchesWMI", computerName);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch[] /*List*/ GetInternalSwitches(string computerName)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualSwitch[], SolidCP.Providers.Virtualization.VirtualSwitch>("SolidCP.Server.VirtualizationServer2012", "GetInternalSwitches", computerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetInternalSwitchesAsync(string computerName)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualSwitch[], SolidCP.Providers.Virtualization.VirtualSwitch>("SolidCP.Server.VirtualizationServer2012", "GetInternalSwitches", computerName);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch[] /*List*/ GetSwitches()
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualSwitch[], SolidCP.Providers.Virtualization.VirtualSwitch>("SolidCP.Server.VirtualizationServer2012", "GetSwitches");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetSwitchesAsync()
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualSwitch[], SolidCP.Providers.Virtualization.VirtualSwitch>("SolidCP.Server.VirtualizationServer2012", "GetSwitches");
        }

        public bool SwitchExists(string switchId)
        {
            return Invoke<bool>("SolidCP.Server.VirtualizationServer2012", "SwitchExists", switchId);
        }

        public async System.Threading.Tasks.Task<bool> SwitchExistsAsync(string switchId)
        {
            return await InvokeAsync<bool>("SolidCP.Server.VirtualizationServer2012", "SwitchExists", switchId);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch CreateSwitch(string name)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualSwitch>("SolidCP.Server.VirtualizationServer2012", "CreateSwitch", name);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch> CreateSwitchAsync(string name)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualSwitch>("SolidCP.Server.VirtualizationServer2012", "CreateSwitch", name);
        }

        public SolidCP.Providers.Virtualization.ReturnCode DeleteSwitch(string switchId)
        {
            return Invoke<SolidCP.Providers.Virtualization.ReturnCode>("SolidCP.Server.VirtualizationServer2012", "DeleteSwitch", switchId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> DeleteSwitchAsync(string switchId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ReturnCode>("SolidCP.Server.VirtualizationServer2012", "DeleteSwitch", switchId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachineNetworkAdapter[] /*List*/ GetVirtualMachinesNetwordAdapterSettings(string vmName)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachineNetworkAdapter[], SolidCP.Providers.Virtualization.VirtualMachineNetworkAdapter>("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachinesNetwordAdapterSettings", vmName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineNetworkAdapter[]> GetVirtualMachinesNetwordAdapterSettingsAsync(string vmName)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachineNetworkAdapter[], SolidCP.Providers.Virtualization.VirtualMachineNetworkAdapter>("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachinesNetwordAdapterSettings", vmName);
        }

        public SolidCP.Providers.Virtualization.JobResult InjectIPs(string vmId, SolidCP.Providers.Virtualization.GuestNetworkAdapterConfiguration guestNetworkAdapterConfiguration)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "InjectIPs", vmId, guestNetworkAdapterConfiguration);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> InjectIPsAsync(string vmId, SolidCP.Providers.Virtualization.GuestNetworkAdapterConfiguration guestNetworkAdapterConfiguration)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "InjectIPs", vmId, guestNetworkAdapterConfiguration);
        }

        public string GetInsertedDVD(string vmId)
        {
            return Invoke<string>("SolidCP.Server.VirtualizationServer2012", "GetInsertedDVD", vmId);
        }

        public async System.Threading.Tasks.Task<string> GetInsertedDVDAsync(string vmId)
        {
            return await InvokeAsync<string>("SolidCP.Server.VirtualizationServer2012", "GetInsertedDVD", vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult InsertDVD(string vmId, string isoPath)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "InsertDVD", vmId, isoPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> InsertDVDAsync(string vmId, string isoPath)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "InsertDVD", vmId, isoPath);
        }

        public SolidCP.Providers.Virtualization.JobResult EjectDVD(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "EjectDVD", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> EjectDVDAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "EjectDVD", vmId);
        }

        public SolidCP.Providers.Virtualization.KvpExchangeDataItem[] /*List*/ GetKVPItems(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.KvpExchangeDataItem[], SolidCP.Providers.Virtualization.KvpExchangeDataItem>("SolidCP.Server.VirtualizationServer2012", "GetKVPItems", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.KvpExchangeDataItem[]> GetKVPItemsAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.KvpExchangeDataItem[], SolidCP.Providers.Virtualization.KvpExchangeDataItem>("SolidCP.Server.VirtualizationServer2012", "GetKVPItems", vmId);
        }

        public SolidCP.Providers.Virtualization.KvpExchangeDataItem[] /*List*/ GetStandardKVPItems(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.KvpExchangeDataItem[], SolidCP.Providers.Virtualization.KvpExchangeDataItem>("SolidCP.Server.VirtualizationServer2012", "GetStandardKVPItems", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.KvpExchangeDataItem[]> GetStandardKVPItemsAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.KvpExchangeDataItem[], SolidCP.Providers.Virtualization.KvpExchangeDataItem>("SolidCP.Server.VirtualizationServer2012", "GetStandardKVPItems", vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult AddKVPItems(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "AddKVPItems", vmId, items);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> AddKVPItemsAsync(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "AddKVPItems", vmId, items);
        }

        public SolidCP.Providers.Virtualization.JobResult RemoveKVPItems(string vmId, string[] itemNames)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "RemoveKVPItems", vmId, itemNames);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RemoveKVPItemsAsync(string vmId, string[] itemNames)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "RemoveKVPItems", vmId, itemNames);
        }

        public SolidCP.Providers.Virtualization.JobResult ModifyKVPItems(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "ModifyKVPItems", vmId, items);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ModifyKVPItemsAsync(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "ModifyKVPItems", vmId, items);
        }

        public bool IsEmptyFolders(string path)
        {
            return Invoke<bool>("SolidCP.Server.VirtualizationServer2012", "IsEmptyFolders", path);
        }

        public async System.Threading.Tasks.Task<bool> IsEmptyFoldersAsync(string path)
        {
            return await InvokeAsync<bool>("SolidCP.Server.VirtualizationServer2012", "IsEmptyFolders", path);
        }

        public bool FileExists(string path)
        {
            return Invoke<bool>("SolidCP.Server.VirtualizationServer2012", "FileExists", path);
        }

        public async System.Threading.Tasks.Task<bool> FileExistsAsync(string path)
        {
            return await InvokeAsync<bool>("SolidCP.Server.VirtualizationServer2012", "FileExists", path);
        }

        public SolidCP.Providers.Virtualization.VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualHardDiskInfo>("SolidCP.Server.VirtualizationServer2012", "GetVirtualHardDiskInfo", vhdPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualHardDiskInfo> GetVirtualHardDiskInfoAsync(string vhdPath)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualHardDiskInfo>("SolidCP.Server.VirtualizationServer2012", "GetVirtualHardDiskInfo", vhdPath);
        }

        public SolidCP.Providers.Virtualization.MountedDiskInfo MountVirtualHardDisk(string vhdPath)
        {
            return Invoke<SolidCP.Providers.Virtualization.MountedDiskInfo>("SolidCP.Server.VirtualizationServer2012", "MountVirtualHardDisk", vhdPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.MountedDiskInfo> MountVirtualHardDiskAsync(string vhdPath)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.MountedDiskInfo>("SolidCP.Server.VirtualizationServer2012", "MountVirtualHardDisk", vhdPath);
        }

        public SolidCP.Providers.Virtualization.ReturnCode UnmountVirtualHardDisk(string vhdPath)
        {
            return Invoke<SolidCP.Providers.Virtualization.ReturnCode>("SolidCP.Server.VirtualizationServer2012", "UnmountVirtualHardDisk", vhdPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> UnmountVirtualHardDiskAsync(string vhdPath)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ReturnCode>("SolidCP.Server.VirtualizationServer2012", "UnmountVirtualHardDisk", vhdPath);
        }

        public SolidCP.Providers.Virtualization.JobResult ExpandVirtualHardDisk(string vhdPath, System.UInt64 sizeGB)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "ExpandVirtualHardDisk", vhdPath, sizeGB);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ExpandVirtualHardDiskAsync(string vhdPath, System.UInt64 sizeGB)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "ExpandVirtualHardDisk", vhdPath, sizeGB);
        }

        public SolidCP.Providers.Virtualization.JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType, uint blockSizeBytes)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "ConvertVirtualHardDisk", sourcePath, destinationPath, diskType, blockSizeBytes);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ConvertVirtualHardDiskAsync(string sourcePath, string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType, uint blockSizeBytes)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "ConvertVirtualHardDisk", sourcePath, destinationPath, diskType, blockSizeBytes);
        }

        public SolidCP.Providers.Virtualization.JobResult CreateVirtualHardDisk(string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType, uint blockSizeBytes, System.UInt64 sizeGB)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "CreateVirtualHardDisk", destinationPath, diskType, blockSizeBytes, sizeGB);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> CreateVirtualHardDiskAsync(string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType, uint blockSizeBytes, System.UInt64 sizeGB)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "CreateVirtualHardDisk", destinationPath, diskType, blockSizeBytes, sizeGB);
        }

        public void DeleteRemoteFile(string path)
        {
            Invoke("SolidCP.Server.VirtualizationServer2012", "DeleteRemoteFile", path);
        }

        public async System.Threading.Tasks.Task DeleteRemoteFileAsync(string path)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServer2012", "DeleteRemoteFile", path);
        }

        public void ExpandDiskVolume(string diskAddress, string volumeName)
        {
            Invoke("SolidCP.Server.VirtualizationServer2012", "ExpandDiskVolume", diskAddress, volumeName);
        }

        public async System.Threading.Tasks.Task ExpandDiskVolumeAsync(string diskAddress, string volumeName)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServer2012", "ExpandDiskVolume", diskAddress, volumeName);
        }

        public string ReadRemoteFile(string path)
        {
            return Invoke<string>("SolidCP.Server.VirtualizationServer2012", "ReadRemoteFile", path);
        }

        public async System.Threading.Tasks.Task<string> ReadRemoteFileAsync(string path)
        {
            return await InvokeAsync<string>("SolidCP.Server.VirtualizationServer2012", "ReadRemoteFile", path);
        }

        public void WriteRemoteFile(string path, string content)
        {
            Invoke("SolidCP.Server.VirtualizationServer2012", "WriteRemoteFile", path, content);
        }

        public async System.Threading.Tasks.Task WriteRemoteFileAsync(string path, string content)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServer2012", "WriteRemoteFile", path, content);
        }

        public SolidCP.Providers.Virtualization.ConcreteJob GetJob(string jobId)
        {
            return Invoke<SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.Server.VirtualizationServer2012", "GetJob", jobId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob> GetJobAsync(string jobId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.Server.VirtualizationServer2012", "GetJob", jobId);
        }

        public SolidCP.Providers.Virtualization.ConcreteJob[] /*List*/ GetAllJobs()
        {
            return Invoke<SolidCP.Providers.Virtualization.ConcreteJob[], SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.Server.VirtualizationServer2012", "GetAllJobs");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob[]> GetAllJobsAsync()
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ConcreteJob[], SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.Server.VirtualizationServer2012", "GetAllJobs");
        }

        public void ClearOldJobs()
        {
            Invoke("SolidCP.Server.VirtualizationServer2012", "ClearOldJobs");
        }

        public async System.Threading.Tasks.Task ClearOldJobsAsync()
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServer2012", "ClearOldJobs");
        }

        public SolidCP.Providers.Virtualization.ChangeJobStateReturnCode ChangeJobState(string jobId, SolidCP.Providers.Virtualization.ConcreteJobRequestedState newState)
        {
            return Invoke<SolidCP.Providers.Virtualization.ChangeJobStateReturnCode>("SolidCP.Server.VirtualizationServer2012", "ChangeJobState", jobId, newState);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ChangeJobStateReturnCode> ChangeJobStateAsync(string jobId, SolidCP.Providers.Virtualization.ConcreteJobRequestedState newState)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ChangeJobStateReturnCode>("SolidCP.Server.VirtualizationServer2012", "ChangeJobState", jobId, newState);
        }

        public int GetProcessorCoresNumber()
        {
            return Invoke<int>("SolidCP.Server.VirtualizationServer2012", "GetProcessorCoresNumber");
        }

        public async System.Threading.Tasks.Task<int> GetProcessorCoresNumberAsync()
        {
            return await InvokeAsync<int>("SolidCP.Server.VirtualizationServer2012", "GetProcessorCoresNumber");
        }

        public SolidCP.Providers.Virtualization.VMConfigurationVersion[] /*List*/ GetVMConfigurationVersionSupportedList()
        {
            return Invoke<SolidCP.Providers.Virtualization.VMConfigurationVersion[], SolidCP.Providers.Virtualization.VMConfigurationVersion>("SolidCP.Server.VirtualizationServer2012", "GetVMConfigurationVersionSupportedList");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMConfigurationVersion[]> GetVMConfigurationVersionSupportedListAsync()
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VMConfigurationVersion[], SolidCP.Providers.Virtualization.VMConfigurationVersion>("SolidCP.Server.VirtualizationServer2012", "GetVMConfigurationVersionSupportedList");
        }

        public SolidCP.Providers.Virtualization.CertificateInfo[] /*List*/ GetCertificates(string remoteServer)
        {
            return Invoke<SolidCP.Providers.Virtualization.CertificateInfo[], SolidCP.Providers.Virtualization.CertificateInfo>("SolidCP.Server.VirtualizationServer2012", "GetCertificates", remoteServer);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.CertificateInfo[]> GetCertificatesAsync(string remoteServer)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.CertificateInfo[], SolidCP.Providers.Virtualization.CertificateInfo>("SolidCP.Server.VirtualizationServer2012", "GetCertificates", remoteServer);
        }

        public void SetReplicaServer(string remoteServer, string thumbprint, string storagePath)
        {
            Invoke("SolidCP.Server.VirtualizationServer2012", "SetReplicaServer", remoteServer, thumbprint, storagePath);
        }

        public async System.Threading.Tasks.Task SetReplicaServerAsync(string remoteServer, string thumbprint, string storagePath)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServer2012", "SetReplicaServer", remoteServer, thumbprint, storagePath);
        }

        public void UnsetReplicaServer(string remoteServer)
        {
            Invoke("SolidCP.Server.VirtualizationServer2012", "UnsetReplicaServer", remoteServer);
        }

        public async System.Threading.Tasks.Task UnsetReplicaServerAsync(string remoteServer)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServer2012", "UnsetReplicaServer", remoteServer);
        }

        public SolidCP.Providers.Virtualization.ReplicationServerInfo GetReplicaServer(string remoteServer)
        {
            return Invoke<SolidCP.Providers.Virtualization.ReplicationServerInfo>("SolidCP.Server.VirtualizationServer2012", "GetReplicaServer", remoteServer);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReplicationServerInfo> GetReplicaServerAsync(string remoteServer)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ReplicationServerInfo>("SolidCP.Server.VirtualizationServer2012", "GetReplicaServer", remoteServer);
        }

        public void EnableVmReplication(string vmId, string replicaServer, SolidCP.Providers.Virtualization.VmReplication replication)
        {
            Invoke("SolidCP.Server.VirtualizationServer2012", "EnableVmReplication", vmId, replicaServer, replication);
        }

        public async System.Threading.Tasks.Task EnableVmReplicationAsync(string vmId, string replicaServer, SolidCP.Providers.Virtualization.VmReplication replication)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServer2012", "EnableVmReplication", vmId, replicaServer, replication);
        }

        public void SetVmReplication(string vmId, string replicaServer, SolidCP.Providers.Virtualization.VmReplication replication)
        {
            Invoke("SolidCP.Server.VirtualizationServer2012", "SetVmReplication", vmId, replicaServer, replication);
        }

        public async System.Threading.Tasks.Task SetVmReplicationAsync(string vmId, string replicaServer, SolidCP.Providers.Virtualization.VmReplication replication)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServer2012", "SetVmReplication", vmId, replicaServer, replication);
        }

        public void TestReplicationServer(string vmId, string replicaServer, string localThumbprint)
        {
            Invoke("SolidCP.Server.VirtualizationServer2012", "TestReplicationServer", vmId, replicaServer, localThumbprint);
        }

        public async System.Threading.Tasks.Task TestReplicationServerAsync(string vmId, string replicaServer, string localThumbprint)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServer2012", "TestReplicationServer", vmId, replicaServer, localThumbprint);
        }

        public void StartInitialReplication(string vmId)
        {
            Invoke("SolidCP.Server.VirtualizationServer2012", "StartInitialReplication", vmId);
        }

        public async System.Threading.Tasks.Task StartInitialReplicationAsync(string vmId)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServer2012", "StartInitialReplication", vmId);
        }

        public SolidCP.Providers.Virtualization.VmReplication GetReplication(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VmReplication>("SolidCP.Server.VirtualizationServer2012", "GetReplication", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VmReplication> GetReplicationAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VmReplication>("SolidCP.Server.VirtualizationServer2012", "GetReplication", vmId);
        }

        public void DisableVmReplication(string vmId)
        {
            Invoke("SolidCP.Server.VirtualizationServer2012", "DisableVmReplication", vmId);
        }

        public async System.Threading.Tasks.Task DisableVmReplicationAsync(string vmId)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServer2012", "DisableVmReplication", vmId);
        }

        public SolidCP.Providers.Virtualization.ReplicationDetailInfo GetReplicationInfo(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.ReplicationDetailInfo>("SolidCP.Server.VirtualizationServer2012", "GetReplicationInfo", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReplicationDetailInfo> GetReplicationInfoAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ReplicationDetailInfo>("SolidCP.Server.VirtualizationServer2012", "GetReplicationInfo", vmId);
        }

        public void PauseReplication(string vmId)
        {
            Invoke("SolidCP.Server.VirtualizationServer2012", "PauseReplication", vmId);
        }

        public async System.Threading.Tasks.Task PauseReplicationAsync(string vmId)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServer2012", "PauseReplication", vmId);
        }

        public void ResumeReplication(string vmId)
        {
            Invoke("SolidCP.Server.VirtualizationServer2012", "ResumeReplication", vmId);
        }

        public async System.Threading.Tasks.Task ResumeReplicationAsync(string vmId)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServer2012", "ResumeReplication", vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult ExecuteCustomPsScript(string script)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "ExecuteCustomPsScript", script);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ExecuteCustomPsScriptAsync(string script)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer2012", "ExecuteCustomPsScript", script);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class VirtualizationServer2012 : SolidCP.Web.Client.ClientBase<IVirtualizationServer2012, VirtualizationServer2012AssemblyClient>, IVirtualizationServer2012
    {
        public SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachine(string vmId)
        {
            return base.Client.GetVirtualMachine(vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineAsync(string vmId)
        {
            return await base.Client.GetVirtualMachineAsync(vmId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineEx(string vmId)
        {
            return base.Client.GetVirtualMachineEx(vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineExAsync(string vmId)
        {
            return await base.Client.GetVirtualMachineExAsync(vmId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine[] /*List*/ GetVirtualMachines()
        {
            return base.Client.GetVirtualMachines();
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine[]> GetVirtualMachinesAsync()
        {
            return await base.Client.GetVirtualMachinesAsync();
        }

        public byte[] GetVirtualMachineThumbnailImage(string vmId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return base.Client.GetVirtualMachineThumbnailImage(vmId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailImageAsync(string vmId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return await base.Client.GetVirtualMachineThumbnailImageAsync(vmId, size);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine CreateVirtualMachine(SolidCP.Providers.Virtualization.VirtualMachine vm)
        {
            return base.Client.CreateVirtualMachine(vm);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> CreateVirtualMachineAsync(SolidCP.Providers.Virtualization.VirtualMachine vm)
        {
            return await base.Client.CreateVirtualMachineAsync(vm);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine UpdateVirtualMachine(SolidCP.Providers.Virtualization.VirtualMachine vm)
        {
            return base.Client.UpdateVirtualMachine(vm);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> UpdateVirtualMachineAsync(SolidCP.Providers.Virtualization.VirtualMachine vm)
        {
            return await base.Client.UpdateVirtualMachineAsync(vm);
        }

        public SolidCP.Providers.Virtualization.JobResult ChangeVirtualMachineState(string vmId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState newState, string clusterName)
        {
            return base.Client.ChangeVirtualMachineState(vmId, newState, clusterName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ChangeVirtualMachineStateAsync(string vmId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState newState, string clusterName)
        {
            return await base.Client.ChangeVirtualMachineStateAsync(vmId, newState, clusterName);
        }

        public SolidCP.Providers.Virtualization.ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason)
        {
            return base.Client.ShutDownVirtualMachine(vmId, force, reason);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> ShutDownVirtualMachineAsync(string vmId, bool force, string reason)
        {
            return await base.Client.ShutDownVirtualMachineAsync(vmId, force, reason);
        }

        public SolidCP.Providers.Virtualization.ConcreteJob[] /*List*/ GetVirtualMachineJobs(string vmId)
        {
            return base.Client.GetVirtualMachineJobs(vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob[]> GetVirtualMachineJobsAsync(string vmId)
        {
            return await base.Client.GetVirtualMachineJobsAsync(vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult RenameVirtualMachine(string vmId, string name, string clusterName)
        {
            return base.Client.RenameVirtualMachine(vmId, name, clusterName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RenameVirtualMachineAsync(string vmId, string name, string clusterName)
        {
            return await base.Client.RenameVirtualMachineAsync(vmId, name, clusterName);
        }

        public SolidCP.Providers.Virtualization.JobResult DeleteVirtualMachine(string vmId, string clusterName)
        {
            return base.Client.DeleteVirtualMachine(vmId, clusterName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteVirtualMachineAsync(string vmId, string clusterName)
        {
            return await base.Client.DeleteVirtualMachineAsync(vmId, clusterName);
        }

        public SolidCP.Providers.Virtualization.JobResult DeleteVirtualMachineExtended(string vmId, string clusterName)
        {
            return base.Client.DeleteVirtualMachineExtended(vmId, clusterName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteVirtualMachineExtendedAsync(string vmId, string clusterName)
        {
            return await base.Client.DeleteVirtualMachineExtendedAsync(vmId, clusterName);
        }

        public SolidCP.Providers.Virtualization.JobResult ExportVirtualMachine(string vmId, string exportPath)
        {
            return base.Client.ExportVirtualMachine(vmId, exportPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ExportVirtualMachineAsync(string vmId, string exportPath)
        {
            return await base.Client.ExportVirtualMachineAsync(vmId, exportPath);
        }

        public bool IsTryToUpdateVirtualMachineWithoutRebootSuccess(SolidCP.Providers.Virtualization.VirtualMachine vm)
        {
            return base.Client.IsTryToUpdateVirtualMachineWithoutRebootSuccess(vm);
        }

        public async System.Threading.Tasks.Task<bool> IsTryToUpdateVirtualMachineWithoutRebootSuccessAsync(SolidCP.Providers.Virtualization.VirtualMachine vm)
        {
            return await base.Client.IsTryToUpdateVirtualMachineWithoutRebootSuccessAsync(vm);
        }

        public SolidCP.Providers.Virtualization.VirtualMachineSnapshot[] /*List*/ GetVirtualMachineSnapshots(string vmId)
        {
            return base.Client.GetVirtualMachineSnapshots(vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]> GetVirtualMachineSnapshotsAsync(string vmId)
        {
            return await base.Client.GetVirtualMachineSnapshotsAsync(vmId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachineSnapshot GetSnapshot(string snapshotId)
        {
            return base.Client.GetSnapshot(snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot> GetSnapshotAsync(string snapshotId)
        {
            return await base.Client.GetSnapshotAsync(snapshotId);
        }

        public SolidCP.Providers.Virtualization.JobResult CreateSnapshot(string vmId)
        {
            return base.Client.CreateSnapshot(vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> CreateSnapshotAsync(string vmId)
        {
            return await base.Client.CreateSnapshotAsync(vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult RenameSnapshot(string vmId, string snapshotId, string name)
        {
            return base.Client.RenameSnapshot(vmId, snapshotId, name);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RenameSnapshotAsync(string vmId, string snapshotId, string name)
        {
            return await base.Client.RenameSnapshotAsync(vmId, snapshotId, name);
        }

        public SolidCP.Providers.Virtualization.JobResult ApplySnapshot(string vmId, string snapshotId)
        {
            return base.Client.ApplySnapshot(vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ApplySnapshotAsync(string vmId, string snapshotId)
        {
            return await base.Client.ApplySnapshotAsync(vmId, snapshotId);
        }

        public SolidCP.Providers.Virtualization.JobResult DeleteSnapshot(string snapshotId)
        {
            return base.Client.DeleteSnapshot(snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteSnapshotAsync(string snapshotId)
        {
            return await base.Client.DeleteSnapshotAsync(snapshotId);
        }

        public SolidCP.Providers.Virtualization.JobResult DeleteSnapshotSubtree(string snapshotId)
        {
            return base.Client.DeleteSnapshotSubtree(snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteSnapshotSubtreeAsync(string snapshotId)
        {
            return await base.Client.DeleteSnapshotSubtreeAsync(snapshotId);
        }

        public byte[] GetSnapshotThumbnailImage(string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return base.Client.GetSnapshotThumbnailImage(snapshotId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailImageAsync(string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return await base.Client.GetSnapshotThumbnailImageAsync(snapshotId, size);
        }

        public SolidCP.Providers.Virtualization.SecureBootTemplate[] /*List*/ GetSecureBootTemplates(string computerName)
        {
            return base.Client.GetSecureBootTemplates(computerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.SecureBootTemplate[]> GetSecureBootTemplatesAsync(string computerName)
        {
            return await base.Client.GetSecureBootTemplatesAsync(computerName);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch[] /*List*/ GetExternalSwitches(string computerName)
        {
            return base.Client.GetExternalSwitches(computerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetExternalSwitchesAsync(string computerName)
        {
            return await base.Client.GetExternalSwitchesAsync(computerName);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch[] /*List*/ GetExternalSwitchesWMI(string computerName)
        {
            return base.Client.GetExternalSwitchesWMI(computerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetExternalSwitchesWMIAsync(string computerName)
        {
            return await base.Client.GetExternalSwitchesWMIAsync(computerName);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch[] /*List*/ GetInternalSwitches(string computerName)
        {
            return base.Client.GetInternalSwitches(computerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetInternalSwitchesAsync(string computerName)
        {
            return await base.Client.GetInternalSwitchesAsync(computerName);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch[] /*List*/ GetSwitches()
        {
            return base.Client.GetSwitches();
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetSwitchesAsync()
        {
            return await base.Client.GetSwitchesAsync();
        }

        public bool SwitchExists(string switchId)
        {
            return base.Client.SwitchExists(switchId);
        }

        public async System.Threading.Tasks.Task<bool> SwitchExistsAsync(string switchId)
        {
            return await base.Client.SwitchExistsAsync(switchId);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch CreateSwitch(string name)
        {
            return base.Client.CreateSwitch(name);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch> CreateSwitchAsync(string name)
        {
            return await base.Client.CreateSwitchAsync(name);
        }

        public SolidCP.Providers.Virtualization.ReturnCode DeleteSwitch(string switchId)
        {
            return base.Client.DeleteSwitch(switchId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> DeleteSwitchAsync(string switchId)
        {
            return await base.Client.DeleteSwitchAsync(switchId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachineNetworkAdapter[] /*List*/ GetVirtualMachinesNetwordAdapterSettings(string vmName)
        {
            return base.Client.GetVirtualMachinesNetwordAdapterSettings(vmName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineNetworkAdapter[]> GetVirtualMachinesNetwordAdapterSettingsAsync(string vmName)
        {
            return await base.Client.GetVirtualMachinesNetwordAdapterSettingsAsync(vmName);
        }

        public SolidCP.Providers.Virtualization.JobResult InjectIPs(string vmId, SolidCP.Providers.Virtualization.GuestNetworkAdapterConfiguration guestNetworkAdapterConfiguration)
        {
            return base.Client.InjectIPs(vmId, guestNetworkAdapterConfiguration);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> InjectIPsAsync(string vmId, SolidCP.Providers.Virtualization.GuestNetworkAdapterConfiguration guestNetworkAdapterConfiguration)
        {
            return await base.Client.InjectIPsAsync(vmId, guestNetworkAdapterConfiguration);
        }

        public string GetInsertedDVD(string vmId)
        {
            return base.Client.GetInsertedDVD(vmId);
        }

        public async System.Threading.Tasks.Task<string> GetInsertedDVDAsync(string vmId)
        {
            return await base.Client.GetInsertedDVDAsync(vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult InsertDVD(string vmId, string isoPath)
        {
            return base.Client.InsertDVD(vmId, isoPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> InsertDVDAsync(string vmId, string isoPath)
        {
            return await base.Client.InsertDVDAsync(vmId, isoPath);
        }

        public SolidCP.Providers.Virtualization.JobResult EjectDVD(string vmId)
        {
            return base.Client.EjectDVD(vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> EjectDVDAsync(string vmId)
        {
            return await base.Client.EjectDVDAsync(vmId);
        }

        public SolidCP.Providers.Virtualization.KvpExchangeDataItem[] /*List*/ GetKVPItems(string vmId)
        {
            return base.Client.GetKVPItems(vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.KvpExchangeDataItem[]> GetKVPItemsAsync(string vmId)
        {
            return await base.Client.GetKVPItemsAsync(vmId);
        }

        public SolidCP.Providers.Virtualization.KvpExchangeDataItem[] /*List*/ GetStandardKVPItems(string vmId)
        {
            return base.Client.GetStandardKVPItems(vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.KvpExchangeDataItem[]> GetStandardKVPItemsAsync(string vmId)
        {
            return await base.Client.GetStandardKVPItemsAsync(vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult AddKVPItems(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items)
        {
            return base.Client.AddKVPItems(vmId, items);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> AddKVPItemsAsync(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items)
        {
            return await base.Client.AddKVPItemsAsync(vmId, items);
        }

        public SolidCP.Providers.Virtualization.JobResult RemoveKVPItems(string vmId, string[] itemNames)
        {
            return base.Client.RemoveKVPItems(vmId, itemNames);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RemoveKVPItemsAsync(string vmId, string[] itemNames)
        {
            return await base.Client.RemoveKVPItemsAsync(vmId, itemNames);
        }

        public SolidCP.Providers.Virtualization.JobResult ModifyKVPItems(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items)
        {
            return base.Client.ModifyKVPItems(vmId, items);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ModifyKVPItemsAsync(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items)
        {
            return await base.Client.ModifyKVPItemsAsync(vmId, items);
        }

        public bool IsEmptyFolders(string path)
        {
            return base.Client.IsEmptyFolders(path);
        }

        public async System.Threading.Tasks.Task<bool> IsEmptyFoldersAsync(string path)
        {
            return await base.Client.IsEmptyFoldersAsync(path);
        }

        public bool FileExists(string path)
        {
            return base.Client.FileExists(path);
        }

        public async System.Threading.Tasks.Task<bool> FileExistsAsync(string path)
        {
            return await base.Client.FileExistsAsync(path);
        }

        public SolidCP.Providers.Virtualization.VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath)
        {
            return base.Client.GetVirtualHardDiskInfo(vhdPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualHardDiskInfo> GetVirtualHardDiskInfoAsync(string vhdPath)
        {
            return await base.Client.GetVirtualHardDiskInfoAsync(vhdPath);
        }

        public SolidCP.Providers.Virtualization.MountedDiskInfo MountVirtualHardDisk(string vhdPath)
        {
            return base.Client.MountVirtualHardDisk(vhdPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.MountedDiskInfo> MountVirtualHardDiskAsync(string vhdPath)
        {
            return await base.Client.MountVirtualHardDiskAsync(vhdPath);
        }

        public SolidCP.Providers.Virtualization.ReturnCode UnmountVirtualHardDisk(string vhdPath)
        {
            return base.Client.UnmountVirtualHardDisk(vhdPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> UnmountVirtualHardDiskAsync(string vhdPath)
        {
            return await base.Client.UnmountVirtualHardDiskAsync(vhdPath);
        }

        public SolidCP.Providers.Virtualization.JobResult ExpandVirtualHardDisk(string vhdPath, System.UInt64 sizeGB)
        {
            return base.Client.ExpandVirtualHardDisk(vhdPath, sizeGB);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ExpandVirtualHardDiskAsync(string vhdPath, System.UInt64 sizeGB)
        {
            return await base.Client.ExpandVirtualHardDiskAsync(vhdPath, sizeGB);
        }

        public SolidCP.Providers.Virtualization.JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType, uint blockSizeBytes)
        {
            return base.Client.ConvertVirtualHardDisk(sourcePath, destinationPath, diskType, blockSizeBytes);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ConvertVirtualHardDiskAsync(string sourcePath, string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType, uint blockSizeBytes)
        {
            return await base.Client.ConvertVirtualHardDiskAsync(sourcePath, destinationPath, diskType, blockSizeBytes);
        }

        public SolidCP.Providers.Virtualization.JobResult CreateVirtualHardDisk(string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType, uint blockSizeBytes, System.UInt64 sizeGB)
        {
            return base.Client.CreateVirtualHardDisk(destinationPath, diskType, blockSizeBytes, sizeGB);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> CreateVirtualHardDiskAsync(string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType, uint blockSizeBytes, System.UInt64 sizeGB)
        {
            return await base.Client.CreateVirtualHardDiskAsync(destinationPath, diskType, blockSizeBytes, sizeGB);
        }

        public void DeleteRemoteFile(string path)
        {
            base.Client.DeleteRemoteFile(path);
        }

        public async System.Threading.Tasks.Task DeleteRemoteFileAsync(string path)
        {
            await base.Client.DeleteRemoteFileAsync(path);
        }

        public void ExpandDiskVolume(string diskAddress, string volumeName)
        {
            base.Client.ExpandDiskVolume(diskAddress, volumeName);
        }

        public async System.Threading.Tasks.Task ExpandDiskVolumeAsync(string diskAddress, string volumeName)
        {
            await base.Client.ExpandDiskVolumeAsync(diskAddress, volumeName);
        }

        public string ReadRemoteFile(string path)
        {
            return base.Client.ReadRemoteFile(path);
        }

        public async System.Threading.Tasks.Task<string> ReadRemoteFileAsync(string path)
        {
            return await base.Client.ReadRemoteFileAsync(path);
        }

        public void WriteRemoteFile(string path, string content)
        {
            base.Client.WriteRemoteFile(path, content);
        }

        public async System.Threading.Tasks.Task WriteRemoteFileAsync(string path, string content)
        {
            await base.Client.WriteRemoteFileAsync(path, content);
        }

        public SolidCP.Providers.Virtualization.ConcreteJob GetJob(string jobId)
        {
            return base.Client.GetJob(jobId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob> GetJobAsync(string jobId)
        {
            return await base.Client.GetJobAsync(jobId);
        }

        public SolidCP.Providers.Virtualization.ConcreteJob[] /*List*/ GetAllJobs()
        {
            return base.Client.GetAllJobs();
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob[]> GetAllJobsAsync()
        {
            return await base.Client.GetAllJobsAsync();
        }

        public void ClearOldJobs()
        {
            base.Client.ClearOldJobs();
        }

        public async System.Threading.Tasks.Task ClearOldJobsAsync()
        {
            await base.Client.ClearOldJobsAsync();
        }

        public SolidCP.Providers.Virtualization.ChangeJobStateReturnCode ChangeJobState(string jobId, SolidCP.Providers.Virtualization.ConcreteJobRequestedState newState)
        {
            return base.Client.ChangeJobState(jobId, newState);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ChangeJobStateReturnCode> ChangeJobStateAsync(string jobId, SolidCP.Providers.Virtualization.ConcreteJobRequestedState newState)
        {
            return await base.Client.ChangeJobStateAsync(jobId, newState);
        }

        public int GetProcessorCoresNumber()
        {
            return base.Client.GetProcessorCoresNumber();
        }

        public async System.Threading.Tasks.Task<int> GetProcessorCoresNumberAsync()
        {
            return await base.Client.GetProcessorCoresNumberAsync();
        }

        public SolidCP.Providers.Virtualization.VMConfigurationVersion[] /*List*/ GetVMConfigurationVersionSupportedList()
        {
            return base.Client.GetVMConfigurationVersionSupportedList();
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMConfigurationVersion[]> GetVMConfigurationVersionSupportedListAsync()
        {
            return await base.Client.GetVMConfigurationVersionSupportedListAsync();
        }

        public SolidCP.Providers.Virtualization.CertificateInfo[] /*List*/ GetCertificates(string remoteServer)
        {
            return base.Client.GetCertificates(remoteServer);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.CertificateInfo[]> GetCertificatesAsync(string remoteServer)
        {
            return await base.Client.GetCertificatesAsync(remoteServer);
        }

        public void SetReplicaServer(string remoteServer, string thumbprint, string storagePath)
        {
            base.Client.SetReplicaServer(remoteServer, thumbprint, storagePath);
        }

        public async System.Threading.Tasks.Task SetReplicaServerAsync(string remoteServer, string thumbprint, string storagePath)
        {
            await base.Client.SetReplicaServerAsync(remoteServer, thumbprint, storagePath);
        }

        public void UnsetReplicaServer(string remoteServer)
        {
            base.Client.UnsetReplicaServer(remoteServer);
        }

        public async System.Threading.Tasks.Task UnsetReplicaServerAsync(string remoteServer)
        {
            await base.Client.UnsetReplicaServerAsync(remoteServer);
        }

        public SolidCP.Providers.Virtualization.ReplicationServerInfo GetReplicaServer(string remoteServer)
        {
            return base.Client.GetReplicaServer(remoteServer);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReplicationServerInfo> GetReplicaServerAsync(string remoteServer)
        {
            return await base.Client.GetReplicaServerAsync(remoteServer);
        }

        public void EnableVmReplication(string vmId, string replicaServer, SolidCP.Providers.Virtualization.VmReplication replication)
        {
            base.Client.EnableVmReplication(vmId, replicaServer, replication);
        }

        public async System.Threading.Tasks.Task EnableVmReplicationAsync(string vmId, string replicaServer, SolidCP.Providers.Virtualization.VmReplication replication)
        {
            await base.Client.EnableVmReplicationAsync(vmId, replicaServer, replication);
        }

        public void SetVmReplication(string vmId, string replicaServer, SolidCP.Providers.Virtualization.VmReplication replication)
        {
            base.Client.SetVmReplication(vmId, replicaServer, replication);
        }

        public async System.Threading.Tasks.Task SetVmReplicationAsync(string vmId, string replicaServer, SolidCP.Providers.Virtualization.VmReplication replication)
        {
            await base.Client.SetVmReplicationAsync(vmId, replicaServer, replication);
        }

        public void TestReplicationServer(string vmId, string replicaServer, string localThumbprint)
        {
            base.Client.TestReplicationServer(vmId, replicaServer, localThumbprint);
        }

        public async System.Threading.Tasks.Task TestReplicationServerAsync(string vmId, string replicaServer, string localThumbprint)
        {
            await base.Client.TestReplicationServerAsync(vmId, replicaServer, localThumbprint);
        }

        public void StartInitialReplication(string vmId)
        {
            base.Client.StartInitialReplication(vmId);
        }

        public async System.Threading.Tasks.Task StartInitialReplicationAsync(string vmId)
        {
            await base.Client.StartInitialReplicationAsync(vmId);
        }

        public SolidCP.Providers.Virtualization.VmReplication GetReplication(string vmId)
        {
            return base.Client.GetReplication(vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VmReplication> GetReplicationAsync(string vmId)
        {
            return await base.Client.GetReplicationAsync(vmId);
        }

        public void DisableVmReplication(string vmId)
        {
            base.Client.DisableVmReplication(vmId);
        }

        public async System.Threading.Tasks.Task DisableVmReplicationAsync(string vmId)
        {
            await base.Client.DisableVmReplicationAsync(vmId);
        }

        public SolidCP.Providers.Virtualization.ReplicationDetailInfo GetReplicationInfo(string vmId)
        {
            return base.Client.GetReplicationInfo(vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReplicationDetailInfo> GetReplicationInfoAsync(string vmId)
        {
            return await base.Client.GetReplicationInfoAsync(vmId);
        }

        public void PauseReplication(string vmId)
        {
            base.Client.PauseReplication(vmId);
        }

        public async System.Threading.Tasks.Task PauseReplicationAsync(string vmId)
        {
            await base.Client.PauseReplicationAsync(vmId);
        }

        public void ResumeReplication(string vmId)
        {
            base.Client.ResumeReplication(vmId);
        }

        public async System.Threading.Tasks.Task ResumeReplicationAsync(string vmId)
        {
            await base.Client.ResumeReplicationAsync(vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult ExecuteCustomPsScript(string script)
        {
            return base.Client.ExecuteCustomPsScript(script);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ExecuteCustomPsScriptAsync(string script)
        {
            return await base.Client.ExecuteCustomPsScriptAsync(script);
        }
    }
}
#endif