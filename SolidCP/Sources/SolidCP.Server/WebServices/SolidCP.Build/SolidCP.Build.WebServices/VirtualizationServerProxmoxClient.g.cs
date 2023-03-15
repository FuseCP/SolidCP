#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("ServerPolicy")]
    [SolidCP.Providers.SoapHeader]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IVirtualizationServerProxmox", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IVirtualizationServerProxmox
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachine(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineEx", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineExResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineEx(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineEx", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineExResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineExAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachines", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachinesResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine[] /*List*/ GetVirtualMachines();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachines", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachinesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine[]> GetVirtualMachinesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineThumbnailImageResponse")]
        byte[] GetVirtualMachineThumbnailImage(string vmId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineThumbnailImageResponse")]
        System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailImageAsync(string vmId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/CreateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/CreateVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine CreateVirtualMachine(SolidCP.Providers.Virtualization.VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/CreateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/CreateVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> CreateVirtualMachineAsync(SolidCP.Providers.Virtualization.VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/UpdateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/UpdateVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine UpdateVirtualMachine(SolidCP.Providers.Virtualization.VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/UpdateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/UpdateVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> UpdateVirtualMachineAsync(SolidCP.Providers.Virtualization.VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ChangeVirtualMachineState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ChangeVirtualMachineStateResponse")]
        SolidCP.Providers.Virtualization.JobResult ChangeVirtualMachineState(string vmId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ChangeVirtualMachineState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ChangeVirtualMachineStateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ChangeVirtualMachineStateAsync(string vmId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ShutDownVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ShutDownVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ShutDownVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ShutDownVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> ShutDownVirtualMachineAsync(string vmId, bool force, string reason);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineJobsResponse")]
        SolidCP.Providers.Virtualization.ConcreteJob[] /*List*/ GetVirtualMachineJobs(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineJobsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob[]> GetVirtualMachineJobsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/RenameVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/RenameVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.JobResult RenameVirtualMachine(string vmId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/RenameVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/RenameVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RenameVirtualMachineAsync(string vmId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.JobResult DeleteVirtualMachine(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteVirtualMachineAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ExportVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ExportVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.JobResult ExportVirtualMachine(string vmId, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ExportVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ExportVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ExportVirtualMachineAsync(string vmId, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineVNC", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineVNCResponse")]
        string GetVirtualMachineVNC(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineVNC", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineVNCResponse")]
        System.Threading.Tasks.Task<string> GetVirtualMachineVNCAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineSnapshots", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineSnapshotsResponse")]
        SolidCP.Providers.Virtualization.VirtualMachineSnapshot[] /*List*/ GetVirtualMachineSnapshots(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineSnapshots", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineSnapshotsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]> GetVirtualMachineSnapshotsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetSnapshotResponse")]
        SolidCP.Providers.Virtualization.VirtualMachineSnapshot GetSnapshot(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot> GetSnapshotAsync(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/CreateSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/CreateSnapshotResponse")]
        SolidCP.Providers.Virtualization.JobResult CreateSnapshot(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/CreateSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/CreateSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> CreateSnapshotAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/RenameSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/RenameSnapshotResponse")]
        SolidCP.Providers.Virtualization.JobResult RenameSnapshot(string vmId, string snapshotId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/RenameSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/RenameSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RenameSnapshotAsync(string vmId, string snapshotId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ApplySnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ApplySnapshotResponse")]
        SolidCP.Providers.Virtualization.JobResult ApplySnapshot(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ApplySnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ApplySnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ApplySnapshotAsync(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteSnapshotResponse")]
        SolidCP.Providers.Virtualization.JobResult DeleteSnapshot(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteSnapshotAsync(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteSnapshotSubtree", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteSnapshotSubtreeResponse")]
        SolidCP.Providers.Virtualization.JobResult DeleteSnapshotSubtree(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteSnapshotSubtree", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteSnapshotSubtreeResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteSnapshotSubtreeAsync(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetSnapshotThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetSnapshotThumbnailImageResponse")]
        byte[] GetSnapshotThumbnailImage(string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetSnapshotThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetSnapshotThumbnailImageResponse")]
        System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailImageAsync(string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetExternalSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetExternalSwitchesResponse")]
        SolidCP.Providers.Virtualization.VirtualSwitch[] /*List*/ GetExternalSwitches(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetExternalSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetExternalSwitchesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetExternalSwitchesAsync(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetSwitchesResponse")]
        SolidCP.Providers.Virtualization.VirtualSwitch[] /*List*/ GetSwitches();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetSwitchesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetSwitchesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/SwitchExists", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/SwitchExistsResponse")]
        bool SwitchExists(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/SwitchExists", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/SwitchExistsResponse")]
        System.Threading.Tasks.Task<bool> SwitchExistsAsync(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/CreateSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/CreateSwitchResponse")]
        SolidCP.Providers.Virtualization.VirtualSwitch CreateSwitch(string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/CreateSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/CreateSwitchResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch> CreateSwitchAsync(string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteSwitchResponse")]
        SolidCP.Providers.Virtualization.ReturnCode DeleteSwitch(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteSwitchResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> DeleteSwitchAsync(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetDVDISOs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetDVDISOsResponse")]
        SolidCP.Providers.Virtualization.LibraryItem[] GetDVDISOs(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetDVDISOs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetDVDISOsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetDVDISOsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetInsertedDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetInsertedDVDResponse")]
        string GetInsertedDVD(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetInsertedDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetInsertedDVDResponse")]
        System.Threading.Tasks.Task<string> GetInsertedDVDAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/InsertDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/InsertDVDResponse")]
        SolidCP.Providers.Virtualization.JobResult InsertDVD(string vmId, string isoPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/InsertDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/InsertDVDResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> InsertDVDAsync(string vmId, string isoPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/EjectDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/EjectDVDResponse")]
        SolidCP.Providers.Virtualization.JobResult EjectDVD(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/EjectDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/EjectDVDResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> EjectDVDAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetKVPItemsResponse")]
        SolidCP.Providers.Virtualization.KvpExchangeDataItem[] /*List*/ GetKVPItems(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetKVPItemsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.KvpExchangeDataItem[]> GetKVPItemsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetStandardKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetStandardKVPItemsResponse")]
        SolidCP.Providers.Virtualization.KvpExchangeDataItem[] /*List*/ GetStandardKVPItems(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetStandardKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetStandardKVPItemsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.KvpExchangeDataItem[]> GetStandardKVPItemsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/AddKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/AddKVPItemsResponse")]
        SolidCP.Providers.Virtualization.JobResult AddKVPItems(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/AddKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/AddKVPItemsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> AddKVPItemsAsync(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/RemoveKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/RemoveKVPItemsResponse")]
        SolidCP.Providers.Virtualization.JobResult RemoveKVPItems(string vmId, string[] itemNames);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/RemoveKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/RemoveKVPItemsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RemoveKVPItemsAsync(string vmId, string[] itemNames);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ModifyKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ModifyKVPItemsResponse")]
        SolidCP.Providers.Virtualization.JobResult ModifyKVPItems(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ModifyKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ModifyKVPItemsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ModifyKVPItemsAsync(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualHardDiskInfo", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualHardDiskInfoResponse")]
        SolidCP.Providers.Virtualization.VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualHardDiskInfo", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualHardDiskInfoResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualHardDiskInfo> GetVirtualHardDiskInfoAsync(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/MountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/MountVirtualHardDiskResponse")]
        SolidCP.Providers.Virtualization.MountedDiskInfo MountVirtualHardDisk(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/MountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/MountVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.MountedDiskInfo> MountVirtualHardDiskAsync(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/UnmountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/UnmountVirtualHardDiskResponse")]
        SolidCP.Providers.Virtualization.ReturnCode UnmountVirtualHardDisk(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/UnmountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/UnmountVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> UnmountVirtualHardDiskAsync(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ExpandVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ExpandVirtualHardDiskResponse")]
        SolidCP.Providers.Virtualization.JobResult ExpandVirtualHardDisk(string vhdPath, System.UInt64 sizeGB);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ExpandVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ExpandVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ExpandVirtualHardDiskAsync(string vhdPath, System.UInt64 sizeGB);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ConvertVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ConvertVirtualHardDiskResponse")]
        SolidCP.Providers.Virtualization.JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ConvertVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ConvertVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ConvertVirtualHardDiskAsync(string sourcePath, string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteRemoteFile", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteRemoteFileResponse")]
        void DeleteRemoteFile(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteRemoteFile", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteRemoteFileResponse")]
        System.Threading.Tasks.Task DeleteRemoteFileAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ExpandDiskVolume", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ExpandDiskVolumeResponse")]
        void ExpandDiskVolume(string diskAddress, string volumeName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ExpandDiskVolume", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ExpandDiskVolumeResponse")]
        System.Threading.Tasks.Task ExpandDiskVolumeAsync(string diskAddress, string volumeName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ReadRemoteFile", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ReadRemoteFileResponse")]
        string ReadRemoteFile(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ReadRemoteFile", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ReadRemoteFileResponse")]
        System.Threading.Tasks.Task<string> ReadRemoteFileAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/WriteRemoteFile", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/WriteRemoteFileResponse")]
        void WriteRemoteFile(string path, string content);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/WriteRemoteFile", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/WriteRemoteFileResponse")]
        System.Threading.Tasks.Task WriteRemoteFileAsync(string path, string content);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetJob", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetJobResponse")]
        SolidCP.Providers.Virtualization.ConcreteJob GetJob(string jobId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetJob", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetJobResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob> GetJobAsync(string jobId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetAllJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetAllJobsResponse")]
        SolidCP.Providers.Virtualization.ConcreteJob[] /*List*/ GetAllJobs();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetAllJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetAllJobsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob[]> GetAllJobsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ChangeJobState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ChangeJobStateResponse")]
        SolidCP.Providers.Virtualization.ChangeJobStateReturnCode ChangeJobState(string jobId, SolidCP.Providers.Virtualization.ConcreteJobRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ChangeJobState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ChangeJobStateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ChangeJobStateReturnCode> ChangeJobStateAsync(string jobId, SolidCP.Providers.Virtualization.ConcreteJobRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetProcessorCoresNumber", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetProcessorCoresNumberResponse")]
        int GetProcessorCoresNumber();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetProcessorCoresNumber", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetProcessorCoresNumberResponse")]
        System.Threading.Tasks.Task<int> GetProcessorCoresNumberAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetCertificates", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetCertificatesResponse")]
        SolidCP.Providers.Virtualization.CertificateInfo[] /*List*/ GetCertificates(string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetCertificates", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetCertificatesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.CertificateInfo[]> GetCertificatesAsync(string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/SetReplicaServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/SetReplicaServerResponse")]
        void SetReplicaServer(string remoteServer, string thumbprint, string storagePath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/SetReplicaServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/SetReplicaServerResponse")]
        System.Threading.Tasks.Task SetReplicaServerAsync(string remoteServer, string thumbprint, string storagePath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/UnsetReplicaServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/UnsetReplicaServerResponse")]
        void UnsetReplicaServer(string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/UnsetReplicaServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/UnsetReplicaServerResponse")]
        System.Threading.Tasks.Task UnsetReplicaServerAsync(string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetReplicaServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetReplicaServerResponse")]
        SolidCP.Providers.Virtualization.ReplicationServerInfo GetReplicaServer(string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetReplicaServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetReplicaServerResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReplicationServerInfo> GetReplicaServerAsync(string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/EnableVmReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/EnableVmReplicationResponse")]
        void EnableVmReplication(string vmId, string replicaServer, SolidCP.Providers.Virtualization.VmReplication replication);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/EnableVmReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/EnableVmReplicationResponse")]
        System.Threading.Tasks.Task EnableVmReplicationAsync(string vmId, string replicaServer, SolidCP.Providers.Virtualization.VmReplication replication);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/SetVmReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/SetVmReplicationResponse")]
        void SetVmReplication(string vmId, string replicaServer, SolidCP.Providers.Virtualization.VmReplication replication);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/SetVmReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/SetVmReplicationResponse")]
        System.Threading.Tasks.Task SetVmReplicationAsync(string vmId, string replicaServer, SolidCP.Providers.Virtualization.VmReplication replication);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/TestReplicationServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/TestReplicationServerResponse")]
        void TestReplicationServer(string vmId, string replicaServer, string localThumbprint);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/TestReplicationServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/TestReplicationServerResponse")]
        System.Threading.Tasks.Task TestReplicationServerAsync(string vmId, string replicaServer, string localThumbprint);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/StartInitialReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/StartInitialReplicationResponse")]
        void StartInitialReplication(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/StartInitialReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/StartInitialReplicationResponse")]
        System.Threading.Tasks.Task StartInitialReplicationAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetReplicationResponse")]
        SolidCP.Providers.Virtualization.VmReplication GetReplication(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetReplicationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VmReplication> GetReplicationAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DisableVmReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DisableVmReplicationResponse")]
        void DisableVmReplication(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DisableVmReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DisableVmReplicationResponse")]
        System.Threading.Tasks.Task DisableVmReplicationAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetReplicationInfo", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetReplicationInfoResponse")]
        SolidCP.Providers.Virtualization.ReplicationDetailInfo GetReplicationInfo(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetReplicationInfo", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetReplicationInfoResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReplicationDetailInfo> GetReplicationInfoAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/PauseReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/PauseReplicationResponse")]
        void PauseReplication(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/PauseReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/PauseReplicationResponse")]
        System.Threading.Tasks.Task PauseReplicationAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ResumeReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ResumeReplicationResponse")]
        void ResumeReplication(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ResumeReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ResumeReplicationResponse")]
        System.Threading.Tasks.Task ResumeReplicationAsync(string vmId);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class VirtualizationServerProxmoxAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IVirtualizationServerProxmox
    {
        public SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachine(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachine", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachine", vmId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineEx(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachineEx", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineExAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachineEx", vmId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine[] /*List*/ GetVirtualMachines()
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine[], SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachines");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine[]> GetVirtualMachinesAsync()
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine[], SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachines");
        }

        public byte[] GetVirtualMachineThumbnailImage(string vmId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return Invoke<byte[]>("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachineThumbnailImage", vmId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailImageAsync(string vmId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachineThumbnailImage", vmId, size);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine CreateVirtualMachine(SolidCP.Providers.Virtualization.VirtualMachine vm)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServerProxmox", "CreateVirtualMachine", vm);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> CreateVirtualMachineAsync(SolidCP.Providers.Virtualization.VirtualMachine vm)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServerProxmox", "CreateVirtualMachine", vm);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine UpdateVirtualMachine(SolidCP.Providers.Virtualization.VirtualMachine vm)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServerProxmox", "UpdateVirtualMachine", vm);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> UpdateVirtualMachineAsync(SolidCP.Providers.Virtualization.VirtualMachine vm)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServerProxmox", "UpdateVirtualMachine", vm);
        }

        public SolidCP.Providers.Virtualization.JobResult ChangeVirtualMachineState(string vmId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState newState)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "ChangeVirtualMachineState", vmId, newState);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ChangeVirtualMachineStateAsync(string vmId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState newState)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "ChangeVirtualMachineState", vmId, newState);
        }

        public SolidCP.Providers.Virtualization.ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason)
        {
            return Invoke<SolidCP.Providers.Virtualization.ReturnCode>("SolidCP.Server.VirtualizationServerProxmox", "ShutDownVirtualMachine", vmId, force, reason);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> ShutDownVirtualMachineAsync(string vmId, bool force, string reason)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ReturnCode>("SolidCP.Server.VirtualizationServerProxmox", "ShutDownVirtualMachine", vmId, force, reason);
        }

        public SolidCP.Providers.Virtualization.ConcreteJob[] /*List*/ GetVirtualMachineJobs(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.ConcreteJob[], SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachineJobs", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob[]> GetVirtualMachineJobsAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ConcreteJob[], SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachineJobs", vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult RenameVirtualMachine(string vmId, string name)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "RenameVirtualMachine", vmId, name);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RenameVirtualMachineAsync(string vmId, string name)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "RenameVirtualMachine", vmId, name);
        }

        public SolidCP.Providers.Virtualization.JobResult DeleteVirtualMachine(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "DeleteVirtualMachine", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteVirtualMachineAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "DeleteVirtualMachine", vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult ExportVirtualMachine(string vmId, string exportPath)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "ExportVirtualMachine", vmId, exportPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ExportVirtualMachineAsync(string vmId, string exportPath)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "ExportVirtualMachine", vmId, exportPath);
        }

        public string GetVirtualMachineVNC(string vmId)
        {
            return Invoke<string>("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachineVNC", vmId);
        }

        public async System.Threading.Tasks.Task<string> GetVirtualMachineVNCAsync(string vmId)
        {
            return await InvokeAsync<string>("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachineVNC", vmId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachineSnapshot[] /*List*/ GetVirtualMachineSnapshots(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[], SolidCP.Providers.Virtualization.VirtualMachineSnapshot>("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachineSnapshots", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]> GetVirtualMachineSnapshotsAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[], SolidCP.Providers.Virtualization.VirtualMachineSnapshot>("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachineSnapshots", vmId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachineSnapshot GetSnapshot(string vmId, string snapshotId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachineSnapshot>("SolidCP.Server.VirtualizationServerProxmox", "GetSnapshot", vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot> GetSnapshotAsync(string vmId, string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachineSnapshot>("SolidCP.Server.VirtualizationServerProxmox", "GetSnapshot", vmId, snapshotId);
        }

        public SolidCP.Providers.Virtualization.JobResult CreateSnapshot(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "CreateSnapshot", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> CreateSnapshotAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "CreateSnapshot", vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult RenameSnapshot(string vmId, string snapshotId, string name)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "RenameSnapshot", vmId, snapshotId, name);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RenameSnapshotAsync(string vmId, string snapshotId, string name)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "RenameSnapshot", vmId, snapshotId, name);
        }

        public SolidCP.Providers.Virtualization.JobResult ApplySnapshot(string vmId, string snapshotId)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "ApplySnapshot", vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ApplySnapshotAsync(string vmId, string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "ApplySnapshot", vmId, snapshotId);
        }

        public SolidCP.Providers.Virtualization.JobResult DeleteSnapshot(string vmId, string snapshotId)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "DeleteSnapshot", vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteSnapshotAsync(string vmId, string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "DeleteSnapshot", vmId, snapshotId);
        }

        public SolidCP.Providers.Virtualization.JobResult DeleteSnapshotSubtree(string vmId, string snapshotId)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "DeleteSnapshotSubtree", vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteSnapshotSubtreeAsync(string vmId, string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "DeleteSnapshotSubtree", vmId, snapshotId);
        }

        public byte[] GetSnapshotThumbnailImage(string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return Invoke<byte[]>("SolidCP.Server.VirtualizationServerProxmox", "GetSnapshotThumbnailImage", snapshotId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailImageAsync(string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.VirtualizationServerProxmox", "GetSnapshotThumbnailImage", snapshotId, size);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch[] /*List*/ GetExternalSwitches(string computerName)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualSwitch[], SolidCP.Providers.Virtualization.VirtualSwitch>("SolidCP.Server.VirtualizationServerProxmox", "GetExternalSwitches", computerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetExternalSwitchesAsync(string computerName)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualSwitch[], SolidCP.Providers.Virtualization.VirtualSwitch>("SolidCP.Server.VirtualizationServerProxmox", "GetExternalSwitches", computerName);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch[] /*List*/ GetSwitches()
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualSwitch[], SolidCP.Providers.Virtualization.VirtualSwitch>("SolidCP.Server.VirtualizationServerProxmox", "GetSwitches");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetSwitchesAsync()
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualSwitch[], SolidCP.Providers.Virtualization.VirtualSwitch>("SolidCP.Server.VirtualizationServerProxmox", "GetSwitches");
        }

        public bool SwitchExists(string switchId)
        {
            return Invoke<bool>("SolidCP.Server.VirtualizationServerProxmox", "SwitchExists", switchId);
        }

        public async System.Threading.Tasks.Task<bool> SwitchExistsAsync(string switchId)
        {
            return await InvokeAsync<bool>("SolidCP.Server.VirtualizationServerProxmox", "SwitchExists", switchId);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch CreateSwitch(string name)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualSwitch>("SolidCP.Server.VirtualizationServerProxmox", "CreateSwitch", name);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch> CreateSwitchAsync(string name)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualSwitch>("SolidCP.Server.VirtualizationServerProxmox", "CreateSwitch", name);
        }

        public SolidCP.Providers.Virtualization.ReturnCode DeleteSwitch(string switchId)
        {
            return Invoke<SolidCP.Providers.Virtualization.ReturnCode>("SolidCP.Server.VirtualizationServerProxmox", "DeleteSwitch", switchId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> DeleteSwitchAsync(string switchId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ReturnCode>("SolidCP.Server.VirtualizationServerProxmox", "DeleteSwitch", switchId);
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetDVDISOs(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.Server.VirtualizationServerProxmox", "GetDVDISOs", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetDVDISOsAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.Server.VirtualizationServerProxmox", "GetDVDISOs", vmId);
        }

        public string GetInsertedDVD(string vmId)
        {
            return Invoke<string>("SolidCP.Server.VirtualizationServerProxmox", "GetInsertedDVD", vmId);
        }

        public async System.Threading.Tasks.Task<string> GetInsertedDVDAsync(string vmId)
        {
            return await InvokeAsync<string>("SolidCP.Server.VirtualizationServerProxmox", "GetInsertedDVD", vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult InsertDVD(string vmId, string isoPath)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "InsertDVD", vmId, isoPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> InsertDVDAsync(string vmId, string isoPath)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "InsertDVD", vmId, isoPath);
        }

        public SolidCP.Providers.Virtualization.JobResult EjectDVD(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "EjectDVD", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> EjectDVDAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "EjectDVD", vmId);
        }

        public SolidCP.Providers.Virtualization.KvpExchangeDataItem[] /*List*/ GetKVPItems(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.KvpExchangeDataItem[], SolidCP.Providers.Virtualization.KvpExchangeDataItem>("SolidCP.Server.VirtualizationServerProxmox", "GetKVPItems", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.KvpExchangeDataItem[]> GetKVPItemsAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.KvpExchangeDataItem[], SolidCP.Providers.Virtualization.KvpExchangeDataItem>("SolidCP.Server.VirtualizationServerProxmox", "GetKVPItems", vmId);
        }

        public SolidCP.Providers.Virtualization.KvpExchangeDataItem[] /*List*/ GetStandardKVPItems(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.KvpExchangeDataItem[], SolidCP.Providers.Virtualization.KvpExchangeDataItem>("SolidCP.Server.VirtualizationServerProxmox", "GetStandardKVPItems", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.KvpExchangeDataItem[]> GetStandardKVPItemsAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.KvpExchangeDataItem[], SolidCP.Providers.Virtualization.KvpExchangeDataItem>("SolidCP.Server.VirtualizationServerProxmox", "GetStandardKVPItems", vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult AddKVPItems(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "AddKVPItems", vmId, items);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> AddKVPItemsAsync(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "AddKVPItems", vmId, items);
        }

        public SolidCP.Providers.Virtualization.JobResult RemoveKVPItems(string vmId, string[] itemNames)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "RemoveKVPItems", vmId, itemNames);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RemoveKVPItemsAsync(string vmId, string[] itemNames)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "RemoveKVPItems", vmId, itemNames);
        }

        public SolidCP.Providers.Virtualization.JobResult ModifyKVPItems(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "ModifyKVPItems", vmId, items);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ModifyKVPItemsAsync(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "ModifyKVPItems", vmId, items);
        }

        public SolidCP.Providers.Virtualization.VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualHardDiskInfo>("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualHardDiskInfo", vhdPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualHardDiskInfo> GetVirtualHardDiskInfoAsync(string vhdPath)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualHardDiskInfo>("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualHardDiskInfo", vhdPath);
        }

        public SolidCP.Providers.Virtualization.MountedDiskInfo MountVirtualHardDisk(string vhdPath)
        {
            return Invoke<SolidCP.Providers.Virtualization.MountedDiskInfo>("SolidCP.Server.VirtualizationServerProxmox", "MountVirtualHardDisk", vhdPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.MountedDiskInfo> MountVirtualHardDiskAsync(string vhdPath)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.MountedDiskInfo>("SolidCP.Server.VirtualizationServerProxmox", "MountVirtualHardDisk", vhdPath);
        }

        public SolidCP.Providers.Virtualization.ReturnCode UnmountVirtualHardDisk(string vhdPath)
        {
            return Invoke<SolidCP.Providers.Virtualization.ReturnCode>("SolidCP.Server.VirtualizationServerProxmox", "UnmountVirtualHardDisk", vhdPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> UnmountVirtualHardDiskAsync(string vhdPath)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ReturnCode>("SolidCP.Server.VirtualizationServerProxmox", "UnmountVirtualHardDisk", vhdPath);
        }

        public SolidCP.Providers.Virtualization.JobResult ExpandVirtualHardDisk(string vhdPath, System.UInt64 sizeGB)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "ExpandVirtualHardDisk", vhdPath, sizeGB);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ExpandVirtualHardDiskAsync(string vhdPath, System.UInt64 sizeGB)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "ExpandVirtualHardDisk", vhdPath, sizeGB);
        }

        public SolidCP.Providers.Virtualization.JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "ConvertVirtualHardDisk", sourcePath, destinationPath, diskType);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ConvertVirtualHardDiskAsync(string sourcePath, string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerProxmox", "ConvertVirtualHardDisk", sourcePath, destinationPath, diskType);
        }

        public void DeleteRemoteFile(string path)
        {
            Invoke("SolidCP.Server.VirtualizationServerProxmox", "DeleteRemoteFile", path);
        }

        public async System.Threading.Tasks.Task DeleteRemoteFileAsync(string path)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServerProxmox", "DeleteRemoteFile", path);
        }

        public void ExpandDiskVolume(string diskAddress, string volumeName)
        {
            Invoke("SolidCP.Server.VirtualizationServerProxmox", "ExpandDiskVolume", diskAddress, volumeName);
        }

        public async System.Threading.Tasks.Task ExpandDiskVolumeAsync(string diskAddress, string volumeName)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServerProxmox", "ExpandDiskVolume", diskAddress, volumeName);
        }

        public string ReadRemoteFile(string path)
        {
            return Invoke<string>("SolidCP.Server.VirtualizationServerProxmox", "ReadRemoteFile", path);
        }

        public async System.Threading.Tasks.Task<string> ReadRemoteFileAsync(string path)
        {
            return await InvokeAsync<string>("SolidCP.Server.VirtualizationServerProxmox", "ReadRemoteFile", path);
        }

        public void WriteRemoteFile(string path, string content)
        {
            Invoke("SolidCP.Server.VirtualizationServerProxmox", "WriteRemoteFile", path, content);
        }

        public async System.Threading.Tasks.Task WriteRemoteFileAsync(string path, string content)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServerProxmox", "WriteRemoteFile", path, content);
        }

        public SolidCP.Providers.Virtualization.ConcreteJob GetJob(string jobId)
        {
            return Invoke<SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.Server.VirtualizationServerProxmox", "GetJob", jobId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob> GetJobAsync(string jobId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.Server.VirtualizationServerProxmox", "GetJob", jobId);
        }

        public SolidCP.Providers.Virtualization.ConcreteJob[] /*List*/ GetAllJobs()
        {
            return Invoke<SolidCP.Providers.Virtualization.ConcreteJob[], SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.Server.VirtualizationServerProxmox", "GetAllJobs");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob[]> GetAllJobsAsync()
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ConcreteJob[], SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.Server.VirtualizationServerProxmox", "GetAllJobs");
        }

        public SolidCP.Providers.Virtualization.ChangeJobStateReturnCode ChangeJobState(string jobId, SolidCP.Providers.Virtualization.ConcreteJobRequestedState newState)
        {
            return Invoke<SolidCP.Providers.Virtualization.ChangeJobStateReturnCode>("SolidCP.Server.VirtualizationServerProxmox", "ChangeJobState", jobId, newState);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ChangeJobStateReturnCode> ChangeJobStateAsync(string jobId, SolidCP.Providers.Virtualization.ConcreteJobRequestedState newState)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ChangeJobStateReturnCode>("SolidCP.Server.VirtualizationServerProxmox", "ChangeJobState", jobId, newState);
        }

        public int GetProcessorCoresNumber()
        {
            return Invoke<int>("SolidCP.Server.VirtualizationServerProxmox", "GetProcessorCoresNumber");
        }

        public async System.Threading.Tasks.Task<int> GetProcessorCoresNumberAsync()
        {
            return await InvokeAsync<int>("SolidCP.Server.VirtualizationServerProxmox", "GetProcessorCoresNumber");
        }

        public SolidCP.Providers.Virtualization.CertificateInfo[] /*List*/ GetCertificates(string remoteServer)
        {
            return Invoke<SolidCP.Providers.Virtualization.CertificateInfo[], SolidCP.Providers.Virtualization.CertificateInfo>("SolidCP.Server.VirtualizationServerProxmox", "GetCertificates", remoteServer);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.CertificateInfo[]> GetCertificatesAsync(string remoteServer)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.CertificateInfo[], SolidCP.Providers.Virtualization.CertificateInfo>("SolidCP.Server.VirtualizationServerProxmox", "GetCertificates", remoteServer);
        }

        public void SetReplicaServer(string remoteServer, string thumbprint, string storagePath)
        {
            Invoke("SolidCP.Server.VirtualizationServerProxmox", "SetReplicaServer", remoteServer, thumbprint, storagePath);
        }

        public async System.Threading.Tasks.Task SetReplicaServerAsync(string remoteServer, string thumbprint, string storagePath)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServerProxmox", "SetReplicaServer", remoteServer, thumbprint, storagePath);
        }

        public void UnsetReplicaServer(string remoteServer)
        {
            Invoke("SolidCP.Server.VirtualizationServerProxmox", "UnsetReplicaServer", remoteServer);
        }

        public async System.Threading.Tasks.Task UnsetReplicaServerAsync(string remoteServer)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServerProxmox", "UnsetReplicaServer", remoteServer);
        }

        public SolidCP.Providers.Virtualization.ReplicationServerInfo GetReplicaServer(string remoteServer)
        {
            return Invoke<SolidCP.Providers.Virtualization.ReplicationServerInfo>("SolidCP.Server.VirtualizationServerProxmox", "GetReplicaServer", remoteServer);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReplicationServerInfo> GetReplicaServerAsync(string remoteServer)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ReplicationServerInfo>("SolidCP.Server.VirtualizationServerProxmox", "GetReplicaServer", remoteServer);
        }

        public void EnableVmReplication(string vmId, string replicaServer, SolidCP.Providers.Virtualization.VmReplication replication)
        {
            Invoke("SolidCP.Server.VirtualizationServerProxmox", "EnableVmReplication", vmId, replicaServer, replication);
        }

        public async System.Threading.Tasks.Task EnableVmReplicationAsync(string vmId, string replicaServer, SolidCP.Providers.Virtualization.VmReplication replication)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServerProxmox", "EnableVmReplication", vmId, replicaServer, replication);
        }

        public void SetVmReplication(string vmId, string replicaServer, SolidCP.Providers.Virtualization.VmReplication replication)
        {
            Invoke("SolidCP.Server.VirtualizationServerProxmox", "SetVmReplication", vmId, replicaServer, replication);
        }

        public async System.Threading.Tasks.Task SetVmReplicationAsync(string vmId, string replicaServer, SolidCP.Providers.Virtualization.VmReplication replication)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServerProxmox", "SetVmReplication", vmId, replicaServer, replication);
        }

        public void TestReplicationServer(string vmId, string replicaServer, string localThumbprint)
        {
            Invoke("SolidCP.Server.VirtualizationServerProxmox", "TestReplicationServer", vmId, replicaServer, localThumbprint);
        }

        public async System.Threading.Tasks.Task TestReplicationServerAsync(string vmId, string replicaServer, string localThumbprint)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServerProxmox", "TestReplicationServer", vmId, replicaServer, localThumbprint);
        }

        public void StartInitialReplication(string vmId)
        {
            Invoke("SolidCP.Server.VirtualizationServerProxmox", "StartInitialReplication", vmId);
        }

        public async System.Threading.Tasks.Task StartInitialReplicationAsync(string vmId)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServerProxmox", "StartInitialReplication", vmId);
        }

        public SolidCP.Providers.Virtualization.VmReplication GetReplication(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VmReplication>("SolidCP.Server.VirtualizationServerProxmox", "GetReplication", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VmReplication> GetReplicationAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VmReplication>("SolidCP.Server.VirtualizationServerProxmox", "GetReplication", vmId);
        }

        public void DisableVmReplication(string vmId)
        {
            Invoke("SolidCP.Server.VirtualizationServerProxmox", "DisableVmReplication", vmId);
        }

        public async System.Threading.Tasks.Task DisableVmReplicationAsync(string vmId)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServerProxmox", "DisableVmReplication", vmId);
        }

        public SolidCP.Providers.Virtualization.ReplicationDetailInfo GetReplicationInfo(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.ReplicationDetailInfo>("SolidCP.Server.VirtualizationServerProxmox", "GetReplicationInfo", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReplicationDetailInfo> GetReplicationInfoAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ReplicationDetailInfo>("SolidCP.Server.VirtualizationServerProxmox", "GetReplicationInfo", vmId);
        }

        public void PauseReplication(string vmId)
        {
            Invoke("SolidCP.Server.VirtualizationServerProxmox", "PauseReplication", vmId);
        }

        public async System.Threading.Tasks.Task PauseReplicationAsync(string vmId)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServerProxmox", "PauseReplication", vmId);
        }

        public void ResumeReplication(string vmId)
        {
            Invoke("SolidCP.Server.VirtualizationServerProxmox", "ResumeReplication", vmId);
        }

        public async System.Threading.Tasks.Task ResumeReplicationAsync(string vmId)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServerProxmox", "ResumeReplication", vmId);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class VirtualizationServerProxmox : SolidCP.Web.Client.ClientBase<IVirtualizationServerProxmox, VirtualizationServerProxmoxAssemblyClient>, IVirtualizationServerProxmox
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

        public SolidCP.Providers.Virtualization.JobResult ChangeVirtualMachineState(string vmId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState newState)
        {
            return base.Client.ChangeVirtualMachineState(vmId, newState);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ChangeVirtualMachineStateAsync(string vmId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState newState)
        {
            return await base.Client.ChangeVirtualMachineStateAsync(vmId, newState);
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

        public SolidCP.Providers.Virtualization.JobResult RenameVirtualMachine(string vmId, string name)
        {
            return base.Client.RenameVirtualMachine(vmId, name);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RenameVirtualMachineAsync(string vmId, string name)
        {
            return await base.Client.RenameVirtualMachineAsync(vmId, name);
        }

        public SolidCP.Providers.Virtualization.JobResult DeleteVirtualMachine(string vmId)
        {
            return base.Client.DeleteVirtualMachine(vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteVirtualMachineAsync(string vmId)
        {
            return await base.Client.DeleteVirtualMachineAsync(vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult ExportVirtualMachine(string vmId, string exportPath)
        {
            return base.Client.ExportVirtualMachine(vmId, exportPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ExportVirtualMachineAsync(string vmId, string exportPath)
        {
            return await base.Client.ExportVirtualMachineAsync(vmId, exportPath);
        }

        public string GetVirtualMachineVNC(string vmId)
        {
            return base.Client.GetVirtualMachineVNC(vmId);
        }

        public async System.Threading.Tasks.Task<string> GetVirtualMachineVNCAsync(string vmId)
        {
            return await base.Client.GetVirtualMachineVNCAsync(vmId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachineSnapshot[] /*List*/ GetVirtualMachineSnapshots(string vmId)
        {
            return base.Client.GetVirtualMachineSnapshots(vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]> GetVirtualMachineSnapshotsAsync(string vmId)
        {
            return await base.Client.GetVirtualMachineSnapshotsAsync(vmId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachineSnapshot GetSnapshot(string vmId, string snapshotId)
        {
            return base.Client.GetSnapshot(vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot> GetSnapshotAsync(string vmId, string snapshotId)
        {
            return await base.Client.GetSnapshotAsync(vmId, snapshotId);
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

        public SolidCP.Providers.Virtualization.JobResult DeleteSnapshot(string vmId, string snapshotId)
        {
            return base.Client.DeleteSnapshot(vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteSnapshotAsync(string vmId, string snapshotId)
        {
            return await base.Client.DeleteSnapshotAsync(vmId, snapshotId);
        }

        public SolidCP.Providers.Virtualization.JobResult DeleteSnapshotSubtree(string vmId, string snapshotId)
        {
            return base.Client.DeleteSnapshotSubtree(vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteSnapshotSubtreeAsync(string vmId, string snapshotId)
        {
            return await base.Client.DeleteSnapshotSubtreeAsync(vmId, snapshotId);
        }

        public byte[] GetSnapshotThumbnailImage(string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return base.Client.GetSnapshotThumbnailImage(snapshotId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailImageAsync(string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return await base.Client.GetSnapshotThumbnailImageAsync(snapshotId, size);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch[] /*List*/ GetExternalSwitches(string computerName)
        {
            return base.Client.GetExternalSwitches(computerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetExternalSwitchesAsync(string computerName)
        {
            return await base.Client.GetExternalSwitchesAsync(computerName);
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

        public SolidCP.Providers.Virtualization.LibraryItem[] GetDVDISOs(string vmId)
        {
            return base.Client.GetDVDISOs(vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetDVDISOsAsync(string vmId)
        {
            return await base.Client.GetDVDISOsAsync(vmId);
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

        public SolidCP.Providers.Virtualization.JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType)
        {
            return base.Client.ConvertVirtualHardDisk(sourcePath, destinationPath, diskType);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ConvertVirtualHardDiskAsync(string sourcePath, string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType)
        {
            return await base.Client.ConvertVirtualHardDiskAsync(sourcePath, destinationPath, diskType);
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
    }
}
#endif