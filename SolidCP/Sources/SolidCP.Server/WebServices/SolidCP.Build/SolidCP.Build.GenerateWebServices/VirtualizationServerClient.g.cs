#if Client
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("ServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IVirtualizationServer", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IVirtualizationServer
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachine(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineEx", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineExResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineEx(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineEx", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineExResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineExAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachines", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachinesResponse")]
        System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachines();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachines", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachinesResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualMachine>> GetVirtualMachinesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineThumbnailImageResponse")]
        byte[] GetVirtualMachineThumbnailImage(string vmId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineThumbnailImageResponse")]
        System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailImageAsync(string vmId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/CreateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/CreateVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine CreateVirtualMachine(SolidCP.Providers.Virtualization.VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/CreateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/CreateVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> CreateVirtualMachineAsync(SolidCP.Providers.Virtualization.VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/UpdateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/UpdateVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine UpdateVirtualMachine(SolidCP.Providers.Virtualization.VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/UpdateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/UpdateVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> UpdateVirtualMachineAsync(SolidCP.Providers.Virtualization.VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ChangeVirtualMachineState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ChangeVirtualMachineStateResponse")]
        SolidCP.Providers.Virtualization.JobResult ChangeVirtualMachineState(string vmId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ChangeVirtualMachineState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ChangeVirtualMachineStateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ChangeVirtualMachineStateAsync(string vmId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ShutDownVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ShutDownVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ShutDownVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ShutDownVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> ShutDownVirtualMachineAsync(string vmId, bool force, string reason);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineJobsResponse")]
        System.Collections.Generic.List<SolidCP.Providers.Virtualization.ConcreteJob> GetVirtualMachineJobs(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineJobsResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Providers.Virtualization.ConcreteJob>> GetVirtualMachineJobsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/RenameVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/RenameVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.JobResult RenameVirtualMachine(string vmId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/RenameVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/RenameVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RenameVirtualMachineAsync(string vmId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.JobResult DeleteVirtualMachine(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteVirtualMachineAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ExportVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ExportVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.JobResult ExportVirtualMachine(string vmId, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ExportVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ExportVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ExportVirtualMachineAsync(string vmId, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineSnapshots", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineSnapshotsResponse")]
        System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineSnapshots", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineSnapshotsResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualMachineSnapshot>> GetVirtualMachineSnapshotsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetSnapshotResponse")]
        SolidCP.Providers.Virtualization.VirtualMachineSnapshot GetSnapshot(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot> GetSnapshotAsync(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/CreateSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/CreateSnapshotResponse")]
        SolidCP.Providers.Virtualization.JobResult CreateSnapshot(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/CreateSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/CreateSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> CreateSnapshotAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/RenameSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/RenameSnapshotResponse")]
        SolidCP.Providers.Virtualization.JobResult RenameSnapshot(string vmId, string snapshotId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/RenameSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/RenameSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RenameSnapshotAsync(string vmId, string snapshotId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ApplySnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ApplySnapshotResponse")]
        SolidCP.Providers.Virtualization.JobResult ApplySnapshot(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ApplySnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ApplySnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ApplySnapshotAsync(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteSnapshotResponse")]
        SolidCP.Providers.Virtualization.JobResult DeleteSnapshot(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteSnapshotAsync(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteSnapshotSubtree", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteSnapshotSubtreeResponse")]
        SolidCP.Providers.Virtualization.JobResult DeleteSnapshotSubtree(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteSnapshotSubtree", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteSnapshotSubtreeResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteSnapshotSubtreeAsync(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetSnapshotThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetSnapshotThumbnailImageResponse")]
        byte[] GetSnapshotThumbnailImage(string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetSnapshotThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetSnapshotThumbnailImageResponse")]
        System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailImageAsync(string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetExternalSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetExternalSwitchesResponse")]
        System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualSwitch> GetExternalSwitches(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetExternalSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetExternalSwitchesResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualSwitch>> GetExternalSwitchesAsync(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetSwitchesResponse")]
        System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualSwitch> GetSwitches();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetSwitchesResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualSwitch>> GetSwitchesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/SwitchExists", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/SwitchExistsResponse")]
        bool SwitchExists(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/SwitchExists", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/SwitchExistsResponse")]
        System.Threading.Tasks.Task<bool> SwitchExistsAsync(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/CreateSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/CreateSwitchResponse")]
        SolidCP.Providers.Virtualization.VirtualSwitch CreateSwitch(string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/CreateSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/CreateSwitchResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch> CreateSwitchAsync(string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteSwitchResponse")]
        SolidCP.Providers.Virtualization.ReturnCode DeleteSwitch(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteSwitchResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> DeleteSwitchAsync(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetInsertedDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetInsertedDVDResponse")]
        string GetInsertedDVD(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetInsertedDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetInsertedDVDResponse")]
        System.Threading.Tasks.Task<string> GetInsertedDVDAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/InsertDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/InsertDVDResponse")]
        SolidCP.Providers.Virtualization.JobResult InsertDVD(string vmId, string isoPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/InsertDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/InsertDVDResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> InsertDVDAsync(string vmId, string isoPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/EjectDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/EjectDVDResponse")]
        SolidCP.Providers.Virtualization.JobResult EjectDVD(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/EjectDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/EjectDVDResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> EjectDVDAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetLibraryItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetLibraryItemsResponse")]
        SolidCP.Providers.Virtualization.LibraryItem[] GetLibraryItems(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetLibraryItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetLibraryItemsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetLibraryItemsAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetKVPItemsResponse")]
        System.Collections.Generic.List<SolidCP.Providers.Virtualization.KvpExchangeDataItem> GetKVPItems(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetKVPItemsResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Providers.Virtualization.KvpExchangeDataItem>> GetKVPItemsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetStandardKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetStandardKVPItemsResponse")]
        System.Collections.Generic.List<SolidCP.Providers.Virtualization.KvpExchangeDataItem> GetStandardKVPItems(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetStandardKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetStandardKVPItemsResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Providers.Virtualization.KvpExchangeDataItem>> GetStandardKVPItemsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/AddKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/AddKVPItemsResponse")]
        SolidCP.Providers.Virtualization.JobResult AddKVPItems(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/AddKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/AddKVPItemsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> AddKVPItemsAsync(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/RemoveKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/RemoveKVPItemsResponse")]
        SolidCP.Providers.Virtualization.JobResult RemoveKVPItems(string vmId, string[] itemNames);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/RemoveKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/RemoveKVPItemsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RemoveKVPItemsAsync(string vmId, string[] itemNames);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ModifyKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ModifyKVPItemsResponse")]
        SolidCP.Providers.Virtualization.JobResult ModifyKVPItems(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ModifyKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ModifyKVPItemsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ModifyKVPItemsAsync(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualHardDiskInfo", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualHardDiskInfoResponse")]
        SolidCP.Providers.Virtualization.VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualHardDiskInfo", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualHardDiskInfoResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualHardDiskInfo> GetVirtualHardDiskInfoAsync(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/MountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/MountVirtualHardDiskResponse")]
        SolidCP.Providers.Virtualization.MountedDiskInfo MountVirtualHardDisk(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/MountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/MountVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.MountedDiskInfo> MountVirtualHardDiskAsync(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/UnmountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/UnmountVirtualHardDiskResponse")]
        SolidCP.Providers.Virtualization.ReturnCode UnmountVirtualHardDisk(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/UnmountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/UnmountVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> UnmountVirtualHardDiskAsync(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ExpandVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ExpandVirtualHardDiskResponse")]
        SolidCP.Providers.Virtualization.JobResult ExpandVirtualHardDisk(string vhdPath, System.UInt64 sizeGB);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ExpandVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ExpandVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ExpandVirtualHardDiskAsync(string vhdPath, System.UInt64 sizeGB);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ConvertVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ConvertVirtualHardDiskResponse")]
        SolidCP.Providers.Virtualization.JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ConvertVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ConvertVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ConvertVirtualHardDiskAsync(string sourcePath, string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteRemoteFile", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteRemoteFileResponse")]
        void DeleteRemoteFile(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteRemoteFile", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteRemoteFileResponse")]
        System.Threading.Tasks.Task DeleteRemoteFileAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ExpandDiskVolume", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ExpandDiskVolumeResponse")]
        void ExpandDiskVolume(string diskAddress, string volumeName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ExpandDiskVolume", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ExpandDiskVolumeResponse")]
        System.Threading.Tasks.Task ExpandDiskVolumeAsync(string diskAddress, string volumeName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ReadRemoteFile", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ReadRemoteFileResponse")]
        string ReadRemoteFile(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ReadRemoteFile", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ReadRemoteFileResponse")]
        System.Threading.Tasks.Task<string> ReadRemoteFileAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/WriteRemoteFile", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/WriteRemoteFileResponse")]
        void WriteRemoteFile(string path, string content);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/WriteRemoteFile", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/WriteRemoteFileResponse")]
        System.Threading.Tasks.Task WriteRemoteFileAsync(string path, string content);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetJob", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetJobResponse")]
        SolidCP.Providers.Virtualization.ConcreteJob GetJob(string jobId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetJob", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetJobResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob> GetJobAsync(string jobId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetAllJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetAllJobsResponse")]
        System.Collections.Generic.List<SolidCP.Providers.Virtualization.ConcreteJob> GetAllJobs();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetAllJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetAllJobsResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Providers.Virtualization.ConcreteJob>> GetAllJobsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ChangeJobState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ChangeJobStateResponse")]
        SolidCP.Providers.Virtualization.ChangeJobStateReturnCode ChangeJobState(string jobId, SolidCP.Providers.Virtualization.ConcreteJobRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ChangeJobState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ChangeJobStateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ChangeJobStateReturnCode> ChangeJobStateAsync(string jobId, SolidCP.Providers.Virtualization.ConcreteJobRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetProcessorCoresNumber", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetProcessorCoresNumberResponse")]
        int GetProcessorCoresNumber();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetProcessorCoresNumber", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetProcessorCoresNumberResponse")]
        System.Threading.Tasks.Task<int> GetProcessorCoresNumberAsync();
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class VirtualizationServerAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IVirtualizationServer
    {
        public SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachine(string vmId)
        {
            return (SolidCP.Providers.Virtualization.VirtualMachine)Invoke("SolidCP.Server.VirtualizationServer", "GetVirtualMachine", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServer", "GetVirtualMachine", vmId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineEx(string vmId)
        {
            return (SolidCP.Providers.Virtualization.VirtualMachine)Invoke("SolidCP.Server.VirtualizationServer", "GetVirtualMachineEx", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineExAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServer", "GetVirtualMachineEx", vmId);
        }

        public System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachines()
        {
            return (System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualMachine>)Invoke("SolidCP.Server.VirtualizationServer", "GetVirtualMachines");
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualMachine>> GetVirtualMachinesAsync()
        {
            return await InvokeAsync<System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualMachine>>("SolidCP.Server.VirtualizationServer", "GetVirtualMachines");
        }

        public byte[] GetVirtualMachineThumbnailImage(string vmId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return (byte[])Invoke("SolidCP.Server.VirtualizationServer", "GetVirtualMachineThumbnailImage", vmId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailImageAsync(string vmId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.VirtualizationServer", "GetVirtualMachineThumbnailImage", vmId, size);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine CreateVirtualMachine(SolidCP.Providers.Virtualization.VirtualMachine vm)
        {
            return (SolidCP.Providers.Virtualization.VirtualMachine)Invoke("SolidCP.Server.VirtualizationServer", "CreateVirtualMachine", vm);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> CreateVirtualMachineAsync(SolidCP.Providers.Virtualization.VirtualMachine vm)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServer", "CreateVirtualMachine", vm);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine UpdateVirtualMachine(SolidCP.Providers.Virtualization.VirtualMachine vm)
        {
            return (SolidCP.Providers.Virtualization.VirtualMachine)Invoke("SolidCP.Server.VirtualizationServer", "UpdateVirtualMachine", vm);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> UpdateVirtualMachineAsync(SolidCP.Providers.Virtualization.VirtualMachine vm)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServer", "UpdateVirtualMachine", vm);
        }

        public SolidCP.Providers.Virtualization.JobResult ChangeVirtualMachineState(string vmId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState newState)
        {
            return (SolidCP.Providers.Virtualization.JobResult)Invoke("SolidCP.Server.VirtualizationServer", "ChangeVirtualMachineState", vmId, newState);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ChangeVirtualMachineStateAsync(string vmId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState newState)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer", "ChangeVirtualMachineState", vmId, newState);
        }

        public SolidCP.Providers.Virtualization.ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason)
        {
            return (SolidCP.Providers.Virtualization.ReturnCode)Invoke("SolidCP.Server.VirtualizationServer", "ShutDownVirtualMachine", vmId, force, reason);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> ShutDownVirtualMachineAsync(string vmId, bool force, string reason)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ReturnCode>("SolidCP.Server.VirtualizationServer", "ShutDownVirtualMachine", vmId, force, reason);
        }

        public System.Collections.Generic.List<SolidCP.Providers.Virtualization.ConcreteJob> GetVirtualMachineJobs(string vmId)
        {
            return (System.Collections.Generic.List<SolidCP.Providers.Virtualization.ConcreteJob>)Invoke("SolidCP.Server.VirtualizationServer", "GetVirtualMachineJobs", vmId);
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Providers.Virtualization.ConcreteJob>> GetVirtualMachineJobsAsync(string vmId)
        {
            return await InvokeAsync<System.Collections.Generic.List<SolidCP.Providers.Virtualization.ConcreteJob>>("SolidCP.Server.VirtualizationServer", "GetVirtualMachineJobs", vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult RenameVirtualMachine(string vmId, string name)
        {
            return (SolidCP.Providers.Virtualization.JobResult)Invoke("SolidCP.Server.VirtualizationServer", "RenameVirtualMachine", vmId, name);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RenameVirtualMachineAsync(string vmId, string name)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer", "RenameVirtualMachine", vmId, name);
        }

        public SolidCP.Providers.Virtualization.JobResult DeleteVirtualMachine(string vmId)
        {
            return (SolidCP.Providers.Virtualization.JobResult)Invoke("SolidCP.Server.VirtualizationServer", "DeleteVirtualMachine", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteVirtualMachineAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer", "DeleteVirtualMachine", vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult ExportVirtualMachine(string vmId, string exportPath)
        {
            return (SolidCP.Providers.Virtualization.JobResult)Invoke("SolidCP.Server.VirtualizationServer", "ExportVirtualMachine", vmId, exportPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ExportVirtualMachineAsync(string vmId, string exportPath)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer", "ExportVirtualMachine", vmId, exportPath);
        }

        public System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId)
        {
            return (System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualMachineSnapshot>)Invoke("SolidCP.Server.VirtualizationServer", "GetVirtualMachineSnapshots", vmId);
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualMachineSnapshot>> GetVirtualMachineSnapshotsAsync(string vmId)
        {
            return await InvokeAsync<System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualMachineSnapshot>>("SolidCP.Server.VirtualizationServer", "GetVirtualMachineSnapshots", vmId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachineSnapshot GetSnapshot(string snapshotId)
        {
            return (SolidCP.Providers.Virtualization.VirtualMachineSnapshot)Invoke("SolidCP.Server.VirtualizationServer", "GetSnapshot", snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot> GetSnapshotAsync(string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachineSnapshot>("SolidCP.Server.VirtualizationServer", "GetSnapshot", snapshotId);
        }

        public SolidCP.Providers.Virtualization.JobResult CreateSnapshot(string vmId)
        {
            return (SolidCP.Providers.Virtualization.JobResult)Invoke("SolidCP.Server.VirtualizationServer", "CreateSnapshot", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> CreateSnapshotAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer", "CreateSnapshot", vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult RenameSnapshot(string vmId, string snapshotId, string name)
        {
            return (SolidCP.Providers.Virtualization.JobResult)Invoke("SolidCP.Server.VirtualizationServer", "RenameSnapshot", vmId, snapshotId, name);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RenameSnapshotAsync(string vmId, string snapshotId, string name)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer", "RenameSnapshot", vmId, snapshotId, name);
        }

        public SolidCP.Providers.Virtualization.JobResult ApplySnapshot(string vmId, string snapshotId)
        {
            return (SolidCP.Providers.Virtualization.JobResult)Invoke("SolidCP.Server.VirtualizationServer", "ApplySnapshot", vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ApplySnapshotAsync(string vmId, string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer", "ApplySnapshot", vmId, snapshotId);
        }

        public SolidCP.Providers.Virtualization.JobResult DeleteSnapshot(string snapshotId)
        {
            return (SolidCP.Providers.Virtualization.JobResult)Invoke("SolidCP.Server.VirtualizationServer", "DeleteSnapshot", snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteSnapshotAsync(string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer", "DeleteSnapshot", snapshotId);
        }

        public SolidCP.Providers.Virtualization.JobResult DeleteSnapshotSubtree(string snapshotId)
        {
            return (SolidCP.Providers.Virtualization.JobResult)Invoke("SolidCP.Server.VirtualizationServer", "DeleteSnapshotSubtree", snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteSnapshotSubtreeAsync(string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer", "DeleteSnapshotSubtree", snapshotId);
        }

        public byte[] GetSnapshotThumbnailImage(string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return (byte[])Invoke("SolidCP.Server.VirtualizationServer", "GetSnapshotThumbnailImage", snapshotId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailImageAsync(string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.VirtualizationServer", "GetSnapshotThumbnailImage", snapshotId, size);
        }

        public System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualSwitch> GetExternalSwitches(string computerName)
        {
            return (System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualSwitch>)Invoke("SolidCP.Server.VirtualizationServer", "GetExternalSwitches", computerName);
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualSwitch>> GetExternalSwitchesAsync(string computerName)
        {
            return await InvokeAsync<System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualSwitch>>("SolidCP.Server.VirtualizationServer", "GetExternalSwitches", computerName);
        }

        public System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualSwitch> GetSwitches()
        {
            return (System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualSwitch>)Invoke("SolidCP.Server.VirtualizationServer", "GetSwitches");
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualSwitch>> GetSwitchesAsync()
        {
            return await InvokeAsync<System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualSwitch>>("SolidCP.Server.VirtualizationServer", "GetSwitches");
        }

        public bool SwitchExists(string switchId)
        {
            return (bool)Invoke("SolidCP.Server.VirtualizationServer", "SwitchExists", switchId);
        }

        public async System.Threading.Tasks.Task<bool> SwitchExistsAsync(string switchId)
        {
            return await InvokeAsync<bool>("SolidCP.Server.VirtualizationServer", "SwitchExists", switchId);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch CreateSwitch(string name)
        {
            return (SolidCP.Providers.Virtualization.VirtualSwitch)Invoke("SolidCP.Server.VirtualizationServer", "CreateSwitch", name);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch> CreateSwitchAsync(string name)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualSwitch>("SolidCP.Server.VirtualizationServer", "CreateSwitch", name);
        }

        public SolidCP.Providers.Virtualization.ReturnCode DeleteSwitch(string switchId)
        {
            return (SolidCP.Providers.Virtualization.ReturnCode)Invoke("SolidCP.Server.VirtualizationServer", "DeleteSwitch", switchId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> DeleteSwitchAsync(string switchId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ReturnCode>("SolidCP.Server.VirtualizationServer", "DeleteSwitch", switchId);
        }

        public string GetInsertedDVD(string vmId)
        {
            return (string)Invoke("SolidCP.Server.VirtualizationServer", "GetInsertedDVD", vmId);
        }

        public async System.Threading.Tasks.Task<string> GetInsertedDVDAsync(string vmId)
        {
            return await InvokeAsync<string>("SolidCP.Server.VirtualizationServer", "GetInsertedDVD", vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult InsertDVD(string vmId, string isoPath)
        {
            return (SolidCP.Providers.Virtualization.JobResult)Invoke("SolidCP.Server.VirtualizationServer", "InsertDVD", vmId, isoPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> InsertDVDAsync(string vmId, string isoPath)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer", "InsertDVD", vmId, isoPath);
        }

        public SolidCP.Providers.Virtualization.JobResult EjectDVD(string vmId)
        {
            return (SolidCP.Providers.Virtualization.JobResult)Invoke("SolidCP.Server.VirtualizationServer", "EjectDVD", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> EjectDVDAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer", "EjectDVD", vmId);
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetLibraryItems(string path)
        {
            return (SolidCP.Providers.Virtualization.LibraryItem[])Invoke("SolidCP.Server.VirtualizationServer", "GetLibraryItems", path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetLibraryItemsAsync(string path)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.Server.VirtualizationServer", "GetLibraryItems", path);
        }

        public System.Collections.Generic.List<SolidCP.Providers.Virtualization.KvpExchangeDataItem> GetKVPItems(string vmId)
        {
            return (System.Collections.Generic.List<SolidCP.Providers.Virtualization.KvpExchangeDataItem>)Invoke("SolidCP.Server.VirtualizationServer", "GetKVPItems", vmId);
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Providers.Virtualization.KvpExchangeDataItem>> GetKVPItemsAsync(string vmId)
        {
            return await InvokeAsync<System.Collections.Generic.List<SolidCP.Providers.Virtualization.KvpExchangeDataItem>>("SolidCP.Server.VirtualizationServer", "GetKVPItems", vmId);
        }

        public System.Collections.Generic.List<SolidCP.Providers.Virtualization.KvpExchangeDataItem> GetStandardKVPItems(string vmId)
        {
            return (System.Collections.Generic.List<SolidCP.Providers.Virtualization.KvpExchangeDataItem>)Invoke("SolidCP.Server.VirtualizationServer", "GetStandardKVPItems", vmId);
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Providers.Virtualization.KvpExchangeDataItem>> GetStandardKVPItemsAsync(string vmId)
        {
            return await InvokeAsync<System.Collections.Generic.List<SolidCP.Providers.Virtualization.KvpExchangeDataItem>>("SolidCP.Server.VirtualizationServer", "GetStandardKVPItems", vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult AddKVPItems(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items)
        {
            return (SolidCP.Providers.Virtualization.JobResult)Invoke("SolidCP.Server.VirtualizationServer", "AddKVPItems", vmId, items);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> AddKVPItemsAsync(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer", "AddKVPItems", vmId, items);
        }

        public SolidCP.Providers.Virtualization.JobResult RemoveKVPItems(string vmId, string[] itemNames)
        {
            return (SolidCP.Providers.Virtualization.JobResult)Invoke("SolidCP.Server.VirtualizationServer", "RemoveKVPItems", vmId, itemNames);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RemoveKVPItemsAsync(string vmId, string[] itemNames)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer", "RemoveKVPItems", vmId, itemNames);
        }

        public SolidCP.Providers.Virtualization.JobResult ModifyKVPItems(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items)
        {
            return (SolidCP.Providers.Virtualization.JobResult)Invoke("SolidCP.Server.VirtualizationServer", "ModifyKVPItems", vmId, items);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ModifyKVPItemsAsync(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer", "ModifyKVPItems", vmId, items);
        }

        public SolidCP.Providers.Virtualization.VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath)
        {
            return (SolidCP.Providers.Virtualization.VirtualHardDiskInfo)Invoke("SolidCP.Server.VirtualizationServer", "GetVirtualHardDiskInfo", vhdPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualHardDiskInfo> GetVirtualHardDiskInfoAsync(string vhdPath)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualHardDiskInfo>("SolidCP.Server.VirtualizationServer", "GetVirtualHardDiskInfo", vhdPath);
        }

        public SolidCP.Providers.Virtualization.MountedDiskInfo MountVirtualHardDisk(string vhdPath)
        {
            return (SolidCP.Providers.Virtualization.MountedDiskInfo)Invoke("SolidCP.Server.VirtualizationServer", "MountVirtualHardDisk", vhdPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.MountedDiskInfo> MountVirtualHardDiskAsync(string vhdPath)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.MountedDiskInfo>("SolidCP.Server.VirtualizationServer", "MountVirtualHardDisk", vhdPath);
        }

        public SolidCP.Providers.Virtualization.ReturnCode UnmountVirtualHardDisk(string vhdPath)
        {
            return (SolidCP.Providers.Virtualization.ReturnCode)Invoke("SolidCP.Server.VirtualizationServer", "UnmountVirtualHardDisk", vhdPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> UnmountVirtualHardDiskAsync(string vhdPath)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ReturnCode>("SolidCP.Server.VirtualizationServer", "UnmountVirtualHardDisk", vhdPath);
        }

        public SolidCP.Providers.Virtualization.JobResult ExpandVirtualHardDisk(string vhdPath, System.UInt64 sizeGB)
        {
            return (SolidCP.Providers.Virtualization.JobResult)Invoke("SolidCP.Server.VirtualizationServer", "ExpandVirtualHardDisk", vhdPath, sizeGB);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ExpandVirtualHardDiskAsync(string vhdPath, System.UInt64 sizeGB)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer", "ExpandVirtualHardDisk", vhdPath, sizeGB);
        }

        public SolidCP.Providers.Virtualization.JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType)
        {
            return (SolidCP.Providers.Virtualization.JobResult)Invoke("SolidCP.Server.VirtualizationServer", "ConvertVirtualHardDisk", sourcePath, destinationPath, diskType);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ConvertVirtualHardDiskAsync(string sourcePath, string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServer", "ConvertVirtualHardDisk", sourcePath, destinationPath, diskType);
        }

        public void DeleteRemoteFile(string path)
        {
            Invoke("SolidCP.Server.VirtualizationServer", "DeleteRemoteFile", path);
        }

        public async System.Threading.Tasks.Task DeleteRemoteFileAsync(string path)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServer", "DeleteRemoteFile", path);
        }

        public void ExpandDiskVolume(string diskAddress, string volumeName)
        {
            Invoke("SolidCP.Server.VirtualizationServer", "ExpandDiskVolume", diskAddress, volumeName);
        }

        public async System.Threading.Tasks.Task ExpandDiskVolumeAsync(string diskAddress, string volumeName)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServer", "ExpandDiskVolume", diskAddress, volumeName);
        }

        public string ReadRemoteFile(string path)
        {
            return (string)Invoke("SolidCP.Server.VirtualizationServer", "ReadRemoteFile", path);
        }

        public async System.Threading.Tasks.Task<string> ReadRemoteFileAsync(string path)
        {
            return await InvokeAsync<string>("SolidCP.Server.VirtualizationServer", "ReadRemoteFile", path);
        }

        public void WriteRemoteFile(string path, string content)
        {
            Invoke("SolidCP.Server.VirtualizationServer", "WriteRemoteFile", path, content);
        }

        public async System.Threading.Tasks.Task WriteRemoteFileAsync(string path, string content)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServer", "WriteRemoteFile", path, content);
        }

        public SolidCP.Providers.Virtualization.ConcreteJob GetJob(string jobId)
        {
            return (SolidCP.Providers.Virtualization.ConcreteJob)Invoke("SolidCP.Server.VirtualizationServer", "GetJob", jobId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob> GetJobAsync(string jobId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.Server.VirtualizationServer", "GetJob", jobId);
        }

        public System.Collections.Generic.List<SolidCP.Providers.Virtualization.ConcreteJob> GetAllJobs()
        {
            return (System.Collections.Generic.List<SolidCP.Providers.Virtualization.ConcreteJob>)Invoke("SolidCP.Server.VirtualizationServer", "GetAllJobs");
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Providers.Virtualization.ConcreteJob>> GetAllJobsAsync()
        {
            return await InvokeAsync<System.Collections.Generic.List<SolidCP.Providers.Virtualization.ConcreteJob>>("SolidCP.Server.VirtualizationServer", "GetAllJobs");
        }

        public SolidCP.Providers.Virtualization.ChangeJobStateReturnCode ChangeJobState(string jobId, SolidCP.Providers.Virtualization.ConcreteJobRequestedState newState)
        {
            return (SolidCP.Providers.Virtualization.ChangeJobStateReturnCode)Invoke("SolidCP.Server.VirtualizationServer", "ChangeJobState", jobId, newState);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ChangeJobStateReturnCode> ChangeJobStateAsync(string jobId, SolidCP.Providers.Virtualization.ConcreteJobRequestedState newState)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ChangeJobStateReturnCode>("SolidCP.Server.VirtualizationServer", "ChangeJobState", jobId, newState);
        }

        public int GetProcessorCoresNumber()
        {
            return (int)Invoke("SolidCP.Server.VirtualizationServer", "GetProcessorCoresNumber");
        }

        public async System.Threading.Tasks.Task<int> GetProcessorCoresNumberAsync()
        {
            return await InvokeAsync<int>("SolidCP.Server.VirtualizationServer", "GetProcessorCoresNumber");
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class VirtualizationServer : SolidCP.Web.Client.ClientBase<IVirtualizationServer, VirtualizationServerAssemblyClient>, IVirtualizationServer
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

        public System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachines()
        {
            return base.Client.GetVirtualMachines();
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualMachine>> GetVirtualMachinesAsync()
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

        public System.Collections.Generic.List<SolidCP.Providers.Virtualization.ConcreteJob> GetVirtualMachineJobs(string vmId)
        {
            return base.Client.GetVirtualMachineJobs(vmId);
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Providers.Virtualization.ConcreteJob>> GetVirtualMachineJobsAsync(string vmId)
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

        public System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId)
        {
            return base.Client.GetVirtualMachineSnapshots(vmId);
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualMachineSnapshot>> GetVirtualMachineSnapshotsAsync(string vmId)
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

        public System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualSwitch> GetExternalSwitches(string computerName)
        {
            return base.Client.GetExternalSwitches(computerName);
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualSwitch>> GetExternalSwitchesAsync(string computerName)
        {
            return await base.Client.GetExternalSwitchesAsync(computerName);
        }

        public System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualSwitch> GetSwitches()
        {
            return base.Client.GetSwitches();
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Providers.Virtualization.VirtualSwitch>> GetSwitchesAsync()
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

        public SolidCP.Providers.Virtualization.LibraryItem[] GetLibraryItems(string path)
        {
            return base.Client.GetLibraryItems(path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetLibraryItemsAsync(string path)
        {
            return await base.Client.GetLibraryItemsAsync(path);
        }

        public System.Collections.Generic.List<SolidCP.Providers.Virtualization.KvpExchangeDataItem> GetKVPItems(string vmId)
        {
            return base.Client.GetKVPItems(vmId);
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Providers.Virtualization.KvpExchangeDataItem>> GetKVPItemsAsync(string vmId)
        {
            return await base.Client.GetKVPItemsAsync(vmId);
        }

        public System.Collections.Generic.List<SolidCP.Providers.Virtualization.KvpExchangeDataItem> GetStandardKVPItems(string vmId)
        {
            return base.Client.GetStandardKVPItems(vmId);
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Providers.Virtualization.KvpExchangeDataItem>> GetStandardKVPItemsAsync(string vmId)
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

        public System.Collections.Generic.List<SolidCP.Providers.Virtualization.ConcreteJob> GetAllJobs()
        {
            return base.Client.GetAllJobs();
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<SolidCP.Providers.Virtualization.ConcreteJob>> GetAllJobsAsync()
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
    }
}
#endif