#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("ServerPolicy")]
    [SolidCP.Providers.SoapHeader]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IVirtualizationServerForPrivateCloud", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IVirtualizationServerForPrivateCloud
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.VMInfo GetVirtualMachine(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMInfo> GetVirtualMachineAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineEx", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineExResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineEx(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineEx", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineExResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineExAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachines", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachinesResponse")]
        SolidCP.Providers.Virtualization.VirtualMachine[] /*List*/ GetVirtualMachines();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachines", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachinesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine[]> GetVirtualMachinesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineThumbnailImageResponse")]
        byte[] GetVirtualMachineThumbnailImage(string vmId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineThumbnailImageResponse")]
        System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailImageAsync(string vmId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.VMInfo CreateVirtualMachine(SolidCP.Providers.Virtualization.VMInfo vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMInfo> CreateVirtualMachineAsync(SolidCP.Providers.Virtualization.VMInfo vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateVMFromVM", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateVMFromVMResponse")]
        SolidCP.Providers.Virtualization.VMInfo CreateVMFromVM(string sourceName, SolidCP.Providers.Virtualization.VMInfo vmTemplate, System.Guid taskGuid);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateVMFromVM", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateVMFromVMResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMInfo> CreateVMFromVMAsync(string sourceName, SolidCP.Providers.Virtualization.VMInfo vmTemplate, System.Guid taskGuid);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/UpdateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/UpdateVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.VMInfo UpdateVirtualMachine(SolidCP.Providers.Virtualization.VMInfo vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/UpdateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/UpdateVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMInfo> UpdateVirtualMachineAsync(SolidCP.Providers.Virtualization.VMInfo vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ChangeVirtualMachineState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ChangeVirtualMachineStateResponse")]
        SolidCP.Providers.Virtualization.JobResult ChangeVirtualMachineState(string vmId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ChangeVirtualMachineState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ChangeVirtualMachineStateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ChangeVirtualMachineStateAsync(string vmId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ShutDownVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ShutDownVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ShutDownVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ShutDownVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> ShutDownVirtualMachineAsync(string vmId, bool force, string reason);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineJobsResponse")]
        SolidCP.Providers.Virtualization.ConcreteJob[] /*List*/ GetVirtualMachineJobs(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineJobsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob[]> GetVirtualMachineJobsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/RenameVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/RenameVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.JobResult RenameVirtualMachine(string vmId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/RenameVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/RenameVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RenameVirtualMachineAsync(string vmId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteVirtualMachineResponse")]
        SolidCP.Providers.Virtualization.JobResult DeleteVirtualMachine(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteVirtualMachineResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteVirtualMachineAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineSnapshots", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineSnapshotsResponse")]
        SolidCP.Providers.Virtualization.VirtualMachineSnapshot[] /*List*/ GetVirtualMachineSnapshots(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineSnapshots", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineSnapshotsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]> GetVirtualMachineSnapshotsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetSnapshotResponse")]
        SolidCP.Providers.Virtualization.VirtualMachineSnapshot GetSnapshot(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot> GetSnapshotAsync(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateSnapshotResponse")]
        SolidCP.Providers.Virtualization.JobResult CreateSnapshot(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> CreateSnapshotAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/RenameSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/RenameSnapshotResponse")]
        SolidCP.Providers.Virtualization.JobResult RenameSnapshot(string vmId, string snapshotId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/RenameSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/RenameSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RenameSnapshotAsync(string vmId, string snapshotId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ApplySnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ApplySnapshotResponse")]
        SolidCP.Providers.Virtualization.JobResult ApplySnapshot(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ApplySnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ApplySnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ApplySnapshotAsync(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteSnapshotResponse")]
        SolidCP.Providers.Virtualization.JobResult DeleteSnapshot(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteSnapshotResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteSnapshotAsync(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteSnapshotSubtree", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteSnapshotSubtreeResponse")]
        SolidCP.Providers.Virtualization.JobResult DeleteSnapshotSubtree(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteSnapshotSubtree", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteSnapshotSubtreeResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteSnapshotSubtreeAsync(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetSnapshotThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetSnapshotThumbnailImageResponse")]
        byte[] GetSnapshotThumbnailImage(string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetSnapshotThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetSnapshotThumbnailImageResponse")]
        System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailImageAsync(string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetExternalSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetExternalSwitchesResponse")]
        SolidCP.Providers.Virtualization.VirtualSwitch[] /*List*/ GetExternalSwitches(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetExternalSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetExternalSwitchesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetExternalSwitchesAsync(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetSwitchesResponse")]
        SolidCP.Providers.Virtualization.VirtualSwitch[] /*List*/ GetSwitches();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetSwitchesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetSwitchesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/SwitchExists", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/SwitchExistsResponse")]
        bool SwitchExists(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/SwitchExists", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/SwitchExistsResponse")]
        System.Threading.Tasks.Task<bool> SwitchExistsAsync(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateSwitchResponse")]
        SolidCP.Providers.Virtualization.VirtualSwitch CreateSwitch(string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateSwitchResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch> CreateSwitchAsync(string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteSwitchResponse")]
        SolidCP.Providers.Virtualization.ReturnCode DeleteSwitch(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteSwitchResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> DeleteSwitchAsync(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetInsertedDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetInsertedDVDResponse")]
        string GetInsertedDVD(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetInsertedDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetInsertedDVDResponse")]
        System.Threading.Tasks.Task<string> GetInsertedDVDAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/InsertDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/InsertDVDResponse")]
        SolidCP.Providers.Virtualization.JobResult InsertDVD(string vmId, string isoPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/InsertDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/InsertDVDResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> InsertDVDAsync(string vmId, string isoPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/EjectDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/EjectDVDResponse")]
        SolidCP.Providers.Virtualization.JobResult EjectDVD(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/EjectDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/EjectDVDResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> EjectDVDAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetLibraryItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetLibraryItemsResponse")]
        SolidCP.Providers.Virtualization.LibraryItem[] GetLibraryItems(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetLibraryItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetLibraryItemsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetLibraryItemsAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetOSLibraryItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetOSLibraryItemsResponse")]
        SolidCP.Providers.Virtualization.LibraryItem[] GetOSLibraryItems();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetOSLibraryItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetOSLibraryItemsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetOSLibraryItemsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetHosts", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetHostsResponse")]
        SolidCP.Providers.Virtualization.LibraryItem[] GetHosts();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetHosts", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetHostsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetHostsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetClusters", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetClustersResponse")]
        SolidCP.Providers.Virtualization.LibraryItem[] GetClusters();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetClusters", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetClustersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetClustersAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetKVPItemsResponse")]
        SolidCP.Providers.Virtualization.KvpExchangeDataItem[] /*List*/ GetKVPItems(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetKVPItemsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.KvpExchangeDataItem[]> GetKVPItemsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetStandardKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetStandardKVPItemsResponse")]
        SolidCP.Providers.Virtualization.KvpExchangeDataItem[] /*List*/ GetStandardKVPItems(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetStandardKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetStandardKVPItemsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.KvpExchangeDataItem[]> GetStandardKVPItemsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/AddKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/AddKVPItemsResponse")]
        SolidCP.Providers.Virtualization.JobResult AddKVPItems(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/AddKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/AddKVPItemsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> AddKVPItemsAsync(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/RemoveKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/RemoveKVPItemsResponse")]
        SolidCP.Providers.Virtualization.JobResult RemoveKVPItems(string vmId, string[] itemNames);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/RemoveKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/RemoveKVPItemsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RemoveKVPItemsAsync(string vmId, string[] itemNames);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ModifyKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ModifyKVPItemsResponse")]
        SolidCP.Providers.Virtualization.JobResult ModifyKVPItems(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ModifyKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ModifyKVPItemsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ModifyKVPItemsAsync(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualHardDiskInfo", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualHardDiskInfoResponse")]
        SolidCP.Providers.Virtualization.VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualHardDiskInfo", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualHardDiskInfoResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualHardDiskInfo> GetVirtualHardDiskInfoAsync(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/MountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/MountVirtualHardDiskResponse")]
        SolidCP.Providers.Virtualization.MountedDiskInfo MountVirtualHardDisk(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/MountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/MountVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.MountedDiskInfo> MountVirtualHardDiskAsync(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/UnmountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/UnmountVirtualHardDiskResponse")]
        SolidCP.Providers.Virtualization.ReturnCode UnmountVirtualHardDisk(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/UnmountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/UnmountVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> UnmountVirtualHardDiskAsync(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ExpandVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ExpandVirtualHardDiskResponse")]
        SolidCP.Providers.Virtualization.JobResult ExpandVirtualHardDisk(string vhdPath, System.UInt64 sizeGB);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ExpandVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ExpandVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ExpandVirtualHardDiskAsync(string vhdPath, System.UInt64 sizeGB);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ConvertVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ConvertVirtualHardDiskResponse")]
        SolidCP.Providers.Virtualization.JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ConvertVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ConvertVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ConvertVirtualHardDiskAsync(string sourcePath, string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteRemoteFile", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteRemoteFileResponse")]
        void DeleteRemoteFile(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteRemoteFile", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteRemoteFileResponse")]
        System.Threading.Tasks.Task DeleteRemoteFileAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ExpandDiskVolume", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ExpandDiskVolumeResponse")]
        void ExpandDiskVolume(string diskAddress, string volumeName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ExpandDiskVolume", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ExpandDiskVolumeResponse")]
        System.Threading.Tasks.Task ExpandDiskVolumeAsync(string diskAddress, string volumeName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ReadRemoteFile", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ReadRemoteFileResponse")]
        string ReadRemoteFile(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ReadRemoteFile", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ReadRemoteFileResponse")]
        System.Threading.Tasks.Task<string> ReadRemoteFileAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/WriteRemoteFile", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/WriteRemoteFileResponse")]
        void WriteRemoteFile(string path, string content);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/WriteRemoteFile", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/WriteRemoteFileResponse")]
        System.Threading.Tasks.Task WriteRemoteFileAsync(string path, string content);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetJob", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetJobResponse")]
        SolidCP.Providers.Virtualization.ConcreteJob GetJob(string jobId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetJob", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetJobResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob> GetJobAsync(string jobId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetAllJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetAllJobsResponse")]
        SolidCP.Providers.Virtualization.ConcreteJob[] /*List*/ GetAllJobs();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetAllJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetAllJobsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob[]> GetAllJobsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ChangeJobState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ChangeJobStateResponse")]
        SolidCP.Providers.Virtualization.ChangeJobStateReturnCode ChangeJobState(string jobId, SolidCP.Providers.Virtualization.ConcreteJobRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ChangeJobState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ChangeJobStateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ChangeJobStateReturnCode> ChangeJobStateAsync(string jobId, SolidCP.Providers.Virtualization.ConcreteJobRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetProcessorCoresNumber", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetProcessorCoresNumberResponse")]
        int GetProcessorCoresNumber(string templateId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetProcessorCoresNumber", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetProcessorCoresNumberResponse")]
        System.Threading.Tasks.Task<int> GetProcessorCoresNumberAsync(string templateId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CheckServerState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CheckServerStateResponse")]
        bool CheckServerState(SolidCP.Providers.Virtualization.VMForPCSettingsName control, string connString, string connName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CheckServerState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CheckServerStateResponse")]
        System.Threading.Tasks.Task<bool> CheckServerStateAsync(SolidCP.Providers.Virtualization.VMForPCSettingsName control, string connString, string connName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetDeviceEvents", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetDeviceEventsResponse")]
        SolidCP.Providers.Virtualization.MonitoredObjectEvent[] /*List*/ GetDeviceEvents(string serviceName, string displayName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetDeviceEvents", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetDeviceEventsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.MonitoredObjectEvent[]> GetDeviceEventsAsync(string serviceName, string displayName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetMonitoringAlerts", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetMonitoringAlertsResponse")]
        SolidCP.Providers.Virtualization.MonitoredObjectAlert[] /*List*/ GetMonitoringAlerts(string serviceName, string virtualMachineName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetMonitoringAlerts", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetMonitoringAlertsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.MonitoredObjectAlert[]> GetMonitoringAlertsAsync(string serviceName, string virtualMachineName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetPerfomanceValue", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetPerfomanceValueResponse")]
        SolidCP.Providers.Virtualization.PerformanceDataValue[] /*List*/ GetPerfomanceValue(string VmName, SolidCP.Providers.Virtualization.PerformanceType perf, System.DateTime startPeriod, System.DateTime endPeriod);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetPerfomanceValue", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetPerfomanceValueResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.PerformanceDataValue[]> GetPerfomanceValueAsync(string VmName, SolidCP.Providers.Virtualization.PerformanceType perf, System.DateTime startPeriod, System.DateTime endPeriod);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ConfigureCreatedVMNetworkAdapters", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ConfigureCreatedVMNetworkAdaptersResponse")]
        void ConfigureCreatedVMNetworkAdapters(SolidCP.Providers.Virtualization.VMInfo vmInfo);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ConfigureCreatedVMNetworkAdapters", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ConfigureCreatedVMNetworkAdaptersResponse")]
        System.Threading.Tasks.Task ConfigureCreatedVMNetworkAdaptersAsync(SolidCP.Providers.Virtualization.VMInfo vmInfo);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/MoveVM", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/MoveVMResponse")]
        SolidCP.Providers.Virtualization.VMInfo MoveVM(SolidCP.Providers.Virtualization.VMInfo vmForMove);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/MoveVM", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/MoveVMResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMInfo> MoveVMAsync(SolidCP.Providers.Virtualization.VMInfo vmForMove);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualNetworkByHostName", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualNetworkByHostNameResponse")]
        SolidCP.Providers.Virtualization.VirtualNetworkInfo[] GetVirtualNetworkByHostName(string hostName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualNetworkByHostName", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualNetworkByHostNameResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualNetworkInfo[]> GetVirtualNetworkByHostNameAsync(string hostName);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class VirtualizationServerForPrivateCloudAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IVirtualizationServerForPrivateCloud
    {
        public SolidCP.Providers.Virtualization.VMInfo GetVirtualMachine(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VMInfo>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualMachine", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMInfo> GetVirtualMachineAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VMInfo>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualMachine", vmId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine GetVirtualMachineEx(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualMachineEx", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine> GetVirtualMachineExAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualMachineEx", vmId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachine[] /*List*/ GetVirtualMachines()
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachine[], SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualMachines");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachine[]> GetVirtualMachinesAsync()
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachine[], SolidCP.Providers.Virtualization.VirtualMachine>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualMachines");
        }

        public byte[] GetVirtualMachineThumbnailImage(string vmId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return Invoke<byte[]>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualMachineThumbnailImage", vmId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailImageAsync(string vmId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualMachineThumbnailImage", vmId, size);
        }

        public SolidCP.Providers.Virtualization.VMInfo CreateVirtualMachine(SolidCP.Providers.Virtualization.VMInfo vm)
        {
            return Invoke<SolidCP.Providers.Virtualization.VMInfo>("SolidCP.Server.VirtualizationServerForPrivateCloud", "CreateVirtualMachine", vm);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMInfo> CreateVirtualMachineAsync(SolidCP.Providers.Virtualization.VMInfo vm)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VMInfo>("SolidCP.Server.VirtualizationServerForPrivateCloud", "CreateVirtualMachine", vm);
        }

        public SolidCP.Providers.Virtualization.VMInfo CreateVMFromVM(string sourceName, SolidCP.Providers.Virtualization.VMInfo vmTemplate, System.Guid taskGuid)
        {
            return Invoke<SolidCP.Providers.Virtualization.VMInfo>("SolidCP.Server.VirtualizationServerForPrivateCloud", "CreateVMFromVM", sourceName, vmTemplate, taskGuid);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMInfo> CreateVMFromVMAsync(string sourceName, SolidCP.Providers.Virtualization.VMInfo vmTemplate, System.Guid taskGuid)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VMInfo>("SolidCP.Server.VirtualizationServerForPrivateCloud", "CreateVMFromVM", sourceName, vmTemplate, taskGuid);
        }

        public SolidCP.Providers.Virtualization.VMInfo UpdateVirtualMachine(SolidCP.Providers.Virtualization.VMInfo vm)
        {
            return Invoke<SolidCP.Providers.Virtualization.VMInfo>("SolidCP.Server.VirtualizationServerForPrivateCloud", "UpdateVirtualMachine", vm);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMInfo> UpdateVirtualMachineAsync(SolidCP.Providers.Virtualization.VMInfo vm)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VMInfo>("SolidCP.Server.VirtualizationServerForPrivateCloud", "UpdateVirtualMachine", vm);
        }

        public SolidCP.Providers.Virtualization.JobResult ChangeVirtualMachineState(string vmId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState newState)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "ChangeVirtualMachineState", vmId, newState);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ChangeVirtualMachineStateAsync(string vmId, SolidCP.Providers.Virtualization.VirtualMachineRequestedState newState)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "ChangeVirtualMachineState", vmId, newState);
        }

        public SolidCP.Providers.Virtualization.ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason)
        {
            return Invoke<SolidCP.Providers.Virtualization.ReturnCode>("SolidCP.Server.VirtualizationServerForPrivateCloud", "ShutDownVirtualMachine", vmId, force, reason);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> ShutDownVirtualMachineAsync(string vmId, bool force, string reason)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ReturnCode>("SolidCP.Server.VirtualizationServerForPrivateCloud", "ShutDownVirtualMachine", vmId, force, reason);
        }

        public SolidCP.Providers.Virtualization.ConcreteJob[] /*List*/ GetVirtualMachineJobs(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.ConcreteJob[], SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualMachineJobs", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob[]> GetVirtualMachineJobsAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ConcreteJob[], SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualMachineJobs", vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult RenameVirtualMachine(string vmId, string name)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "RenameVirtualMachine", vmId, name);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RenameVirtualMachineAsync(string vmId, string name)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "RenameVirtualMachine", vmId, name);
        }

        public SolidCP.Providers.Virtualization.JobResult DeleteVirtualMachine(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "DeleteVirtualMachine", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteVirtualMachineAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "DeleteVirtualMachine", vmId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachineSnapshot[] /*List*/ GetVirtualMachineSnapshots(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[], SolidCP.Providers.Virtualization.VirtualMachineSnapshot>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualMachineSnapshots", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[]> GetVirtualMachineSnapshotsAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachineSnapshot[], SolidCP.Providers.Virtualization.VirtualMachineSnapshot>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualMachineSnapshots", vmId);
        }

        public SolidCP.Providers.Virtualization.VirtualMachineSnapshot GetSnapshot(string snapshotId)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualMachineSnapshot>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetSnapshot", snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualMachineSnapshot> GetSnapshotAsync(string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualMachineSnapshot>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetSnapshot", snapshotId);
        }

        public SolidCP.Providers.Virtualization.JobResult CreateSnapshot(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "CreateSnapshot", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> CreateSnapshotAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "CreateSnapshot", vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult RenameSnapshot(string vmId, string snapshotId, string name)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "RenameSnapshot", vmId, snapshotId, name);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RenameSnapshotAsync(string vmId, string snapshotId, string name)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "RenameSnapshot", vmId, snapshotId, name);
        }

        public SolidCP.Providers.Virtualization.JobResult ApplySnapshot(string vmId, string snapshotId)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "ApplySnapshot", vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ApplySnapshotAsync(string vmId, string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "ApplySnapshot", vmId, snapshotId);
        }

        public SolidCP.Providers.Virtualization.JobResult DeleteSnapshot(string vmId, string snapshotId)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "DeleteSnapshot", vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteSnapshotAsync(string vmId, string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "DeleteSnapshot", vmId, snapshotId);
        }

        public SolidCP.Providers.Virtualization.JobResult DeleteSnapshotSubtree(string snapshotId)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "DeleteSnapshotSubtree", snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteSnapshotSubtreeAsync(string snapshotId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "DeleteSnapshotSubtree", snapshotId);
        }

        public byte[] GetSnapshotThumbnailImage(string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return Invoke<byte[]>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetSnapshotThumbnailImage", snapshotId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailImageAsync(string snapshotId, SolidCP.Providers.Virtualization.ThumbnailSize size)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetSnapshotThumbnailImage", snapshotId, size);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch[] /*List*/ GetExternalSwitches(string computerName)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualSwitch[], SolidCP.Providers.Virtualization.VirtualSwitch>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetExternalSwitches", computerName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetExternalSwitchesAsync(string computerName)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualSwitch[], SolidCP.Providers.Virtualization.VirtualSwitch>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetExternalSwitches", computerName);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch[] /*List*/ GetSwitches()
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualSwitch[], SolidCP.Providers.Virtualization.VirtualSwitch>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetSwitches");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch[]> GetSwitchesAsync()
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualSwitch[], SolidCP.Providers.Virtualization.VirtualSwitch>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetSwitches");
        }

        public bool SwitchExists(string switchId)
        {
            return Invoke<bool>("SolidCP.Server.VirtualizationServerForPrivateCloud", "SwitchExists", switchId);
        }

        public async System.Threading.Tasks.Task<bool> SwitchExistsAsync(string switchId)
        {
            return await InvokeAsync<bool>("SolidCP.Server.VirtualizationServerForPrivateCloud", "SwitchExists", switchId);
        }

        public SolidCP.Providers.Virtualization.VirtualSwitch CreateSwitch(string name)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualSwitch>("SolidCP.Server.VirtualizationServerForPrivateCloud", "CreateSwitch", name);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualSwitch> CreateSwitchAsync(string name)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualSwitch>("SolidCP.Server.VirtualizationServerForPrivateCloud", "CreateSwitch", name);
        }

        public SolidCP.Providers.Virtualization.ReturnCode DeleteSwitch(string switchId)
        {
            return Invoke<SolidCP.Providers.Virtualization.ReturnCode>("SolidCP.Server.VirtualizationServerForPrivateCloud", "DeleteSwitch", switchId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> DeleteSwitchAsync(string switchId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ReturnCode>("SolidCP.Server.VirtualizationServerForPrivateCloud", "DeleteSwitch", switchId);
        }

        public string GetInsertedDVD(string vmId)
        {
            return Invoke<string>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetInsertedDVD", vmId);
        }

        public async System.Threading.Tasks.Task<string> GetInsertedDVDAsync(string vmId)
        {
            return await InvokeAsync<string>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetInsertedDVD", vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult InsertDVD(string vmId, string isoPath)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "InsertDVD", vmId, isoPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> InsertDVDAsync(string vmId, string isoPath)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "InsertDVD", vmId, isoPath);
        }

        public SolidCP.Providers.Virtualization.JobResult EjectDVD(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "EjectDVD", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> EjectDVDAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "EjectDVD", vmId);
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetLibraryItems(string path)
        {
            return Invoke<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetLibraryItems", path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetLibraryItemsAsync(string path)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetLibraryItems", path);
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetOSLibraryItems()
        {
            return Invoke<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetOSLibraryItems");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetOSLibraryItemsAsync()
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetOSLibraryItems");
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetHosts()
        {
            return Invoke<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetHosts");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetHostsAsync()
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetHosts");
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetClusters()
        {
            return Invoke<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetClusters");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetClustersAsync()
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.LibraryItem[]>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetClusters");
        }

        public SolidCP.Providers.Virtualization.KvpExchangeDataItem[] /*List*/ GetKVPItems(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.KvpExchangeDataItem[], SolidCP.Providers.Virtualization.KvpExchangeDataItem>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetKVPItems", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.KvpExchangeDataItem[]> GetKVPItemsAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.KvpExchangeDataItem[], SolidCP.Providers.Virtualization.KvpExchangeDataItem>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetKVPItems", vmId);
        }

        public SolidCP.Providers.Virtualization.KvpExchangeDataItem[] /*List*/ GetStandardKVPItems(string vmId)
        {
            return Invoke<SolidCP.Providers.Virtualization.KvpExchangeDataItem[], SolidCP.Providers.Virtualization.KvpExchangeDataItem>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetStandardKVPItems", vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.KvpExchangeDataItem[]> GetStandardKVPItemsAsync(string vmId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.KvpExchangeDataItem[], SolidCP.Providers.Virtualization.KvpExchangeDataItem>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetStandardKVPItems", vmId);
        }

        public SolidCP.Providers.Virtualization.JobResult AddKVPItems(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "AddKVPItems", vmId, items);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> AddKVPItemsAsync(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "AddKVPItems", vmId, items);
        }

        public SolidCP.Providers.Virtualization.JobResult RemoveKVPItems(string vmId, string[] itemNames)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "RemoveKVPItems", vmId, itemNames);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> RemoveKVPItemsAsync(string vmId, string[] itemNames)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "RemoveKVPItems", vmId, itemNames);
        }

        public SolidCP.Providers.Virtualization.JobResult ModifyKVPItems(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "ModifyKVPItems", vmId, items);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ModifyKVPItemsAsync(string vmId, SolidCP.Providers.Virtualization.KvpExchangeDataItem[] items)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "ModifyKVPItems", vmId, items);
        }

        public SolidCP.Providers.Virtualization.VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualHardDiskInfo>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualHardDiskInfo", vhdPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualHardDiskInfo> GetVirtualHardDiskInfoAsync(string vhdPath)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualHardDiskInfo>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualHardDiskInfo", vhdPath);
        }

        public SolidCP.Providers.Virtualization.MountedDiskInfo MountVirtualHardDisk(string vhdPath)
        {
            return Invoke<SolidCP.Providers.Virtualization.MountedDiskInfo>("SolidCP.Server.VirtualizationServerForPrivateCloud", "MountVirtualHardDisk", vhdPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.MountedDiskInfo> MountVirtualHardDiskAsync(string vhdPath)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.MountedDiskInfo>("SolidCP.Server.VirtualizationServerForPrivateCloud", "MountVirtualHardDisk", vhdPath);
        }

        public SolidCP.Providers.Virtualization.ReturnCode UnmountVirtualHardDisk(string vhdPath)
        {
            return Invoke<SolidCP.Providers.Virtualization.ReturnCode>("SolidCP.Server.VirtualizationServerForPrivateCloud", "UnmountVirtualHardDisk", vhdPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ReturnCode> UnmountVirtualHardDiskAsync(string vhdPath)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ReturnCode>("SolidCP.Server.VirtualizationServerForPrivateCloud", "UnmountVirtualHardDisk", vhdPath);
        }

        public SolidCP.Providers.Virtualization.JobResult ExpandVirtualHardDisk(string vhdPath, System.UInt64 sizeGB)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "ExpandVirtualHardDisk", vhdPath, sizeGB);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ExpandVirtualHardDiskAsync(string vhdPath, System.UInt64 sizeGB)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "ExpandVirtualHardDisk", vhdPath, sizeGB);
        }

        public SolidCP.Providers.Virtualization.JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType)
        {
            return Invoke<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "ConvertVirtualHardDisk", sourcePath, destinationPath, diskType);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> ConvertVirtualHardDiskAsync(string sourcePath, string destinationPath, SolidCP.Providers.Virtualization.VirtualHardDiskType diskType)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "ConvertVirtualHardDisk", sourcePath, destinationPath, diskType);
        }

        public void DeleteRemoteFile(string path)
        {
            Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "DeleteRemoteFile", path);
        }

        public async System.Threading.Tasks.Task DeleteRemoteFileAsync(string path)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServerForPrivateCloud", "DeleteRemoteFile", path);
        }

        public void ExpandDiskVolume(string diskAddress, string volumeName)
        {
            Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "ExpandDiskVolume", diskAddress, volumeName);
        }

        public async System.Threading.Tasks.Task ExpandDiskVolumeAsync(string diskAddress, string volumeName)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServerForPrivateCloud", "ExpandDiskVolume", diskAddress, volumeName);
        }

        public string ReadRemoteFile(string path)
        {
            return Invoke<string>("SolidCP.Server.VirtualizationServerForPrivateCloud", "ReadRemoteFile", path);
        }

        public async System.Threading.Tasks.Task<string> ReadRemoteFileAsync(string path)
        {
            return await InvokeAsync<string>("SolidCP.Server.VirtualizationServerForPrivateCloud", "ReadRemoteFile", path);
        }

        public void WriteRemoteFile(string path, string content)
        {
            Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "WriteRemoteFile", path, content);
        }

        public async System.Threading.Tasks.Task WriteRemoteFileAsync(string path, string content)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServerForPrivateCloud", "WriteRemoteFile", path, content);
        }

        public SolidCP.Providers.Virtualization.ConcreteJob GetJob(string jobId)
        {
            return Invoke<SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetJob", jobId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob> GetJobAsync(string jobId)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetJob", jobId);
        }

        public SolidCP.Providers.Virtualization.ConcreteJob[] /*List*/ GetAllJobs()
        {
            return Invoke<SolidCP.Providers.Virtualization.ConcreteJob[], SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetAllJobs");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ConcreteJob[]> GetAllJobsAsync()
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ConcreteJob[], SolidCP.Providers.Virtualization.ConcreteJob>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetAllJobs");
        }

        public SolidCP.Providers.Virtualization.ChangeJobStateReturnCode ChangeJobState(string jobId, SolidCP.Providers.Virtualization.ConcreteJobRequestedState newState)
        {
            return Invoke<SolidCP.Providers.Virtualization.ChangeJobStateReturnCode>("SolidCP.Server.VirtualizationServerForPrivateCloud", "ChangeJobState", jobId, newState);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.ChangeJobStateReturnCode> ChangeJobStateAsync(string jobId, SolidCP.Providers.Virtualization.ConcreteJobRequestedState newState)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.ChangeJobStateReturnCode>("SolidCP.Server.VirtualizationServerForPrivateCloud", "ChangeJobState", jobId, newState);
        }

        public int GetProcessorCoresNumber(string templateId)
        {
            return Invoke<int>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetProcessorCoresNumber", templateId);
        }

        public async System.Threading.Tasks.Task<int> GetProcessorCoresNumberAsync(string templateId)
        {
            return await InvokeAsync<int>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetProcessorCoresNumber", templateId);
        }

        public bool CheckServerState(SolidCP.Providers.Virtualization.VMForPCSettingsName control, string connString, string connName)
        {
            return Invoke<bool>("SolidCP.Server.VirtualizationServerForPrivateCloud", "CheckServerState", control, connString, connName);
        }

        public async System.Threading.Tasks.Task<bool> CheckServerStateAsync(SolidCP.Providers.Virtualization.VMForPCSettingsName control, string connString, string connName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.VirtualizationServerForPrivateCloud", "CheckServerState", control, connString, connName);
        }

        public SolidCP.Providers.Virtualization.MonitoredObjectEvent[] /*List*/ GetDeviceEvents(string serviceName, string displayName)
        {
            return Invoke<SolidCP.Providers.Virtualization.MonitoredObjectEvent[], SolidCP.Providers.Virtualization.MonitoredObjectEvent>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetDeviceEvents", serviceName, displayName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.MonitoredObjectEvent[]> GetDeviceEventsAsync(string serviceName, string displayName)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.MonitoredObjectEvent[], SolidCP.Providers.Virtualization.MonitoredObjectEvent>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetDeviceEvents", serviceName, displayName);
        }

        public SolidCP.Providers.Virtualization.MonitoredObjectAlert[] /*List*/ GetMonitoringAlerts(string serviceName, string virtualMachineName)
        {
            return Invoke<SolidCP.Providers.Virtualization.MonitoredObjectAlert[], SolidCP.Providers.Virtualization.MonitoredObjectAlert>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetMonitoringAlerts", serviceName, virtualMachineName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.MonitoredObjectAlert[]> GetMonitoringAlertsAsync(string serviceName, string virtualMachineName)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.MonitoredObjectAlert[], SolidCP.Providers.Virtualization.MonitoredObjectAlert>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetMonitoringAlerts", serviceName, virtualMachineName);
        }

        public SolidCP.Providers.Virtualization.PerformanceDataValue[] /*List*/ GetPerfomanceValue(string VmName, SolidCP.Providers.Virtualization.PerformanceType perf, System.DateTime startPeriod, System.DateTime endPeriod)
        {
            return Invoke<SolidCP.Providers.Virtualization.PerformanceDataValue[], SolidCP.Providers.Virtualization.PerformanceDataValue>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetPerfomanceValue", VmName, perf, startPeriod, endPeriod);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.PerformanceDataValue[]> GetPerfomanceValueAsync(string VmName, SolidCP.Providers.Virtualization.PerformanceType perf, System.DateTime startPeriod, System.DateTime endPeriod)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.PerformanceDataValue[], SolidCP.Providers.Virtualization.PerformanceDataValue>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetPerfomanceValue", VmName, perf, startPeriod, endPeriod);
        }

        public void ConfigureCreatedVMNetworkAdapters(SolidCP.Providers.Virtualization.VMInfo vmInfo)
        {
            Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "ConfigureCreatedVMNetworkAdapters", vmInfo);
        }

        public async System.Threading.Tasks.Task ConfigureCreatedVMNetworkAdaptersAsync(SolidCP.Providers.Virtualization.VMInfo vmInfo)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServerForPrivateCloud", "ConfigureCreatedVMNetworkAdapters", vmInfo);
        }

        public SolidCP.Providers.Virtualization.VMInfo MoveVM(SolidCP.Providers.Virtualization.VMInfo vmForMove)
        {
            return Invoke<SolidCP.Providers.Virtualization.VMInfo>("SolidCP.Server.VirtualizationServerForPrivateCloud", "MoveVM", vmForMove);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMInfo> MoveVMAsync(SolidCP.Providers.Virtualization.VMInfo vmForMove)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VMInfo>("SolidCP.Server.VirtualizationServerForPrivateCloud", "MoveVM", vmForMove);
        }

        public SolidCP.Providers.Virtualization.VirtualNetworkInfo[] GetVirtualNetworkByHostName(string hostName)
        {
            return Invoke<SolidCP.Providers.Virtualization.VirtualNetworkInfo[]>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualNetworkByHostName", hostName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualNetworkInfo[]> GetVirtualNetworkByHostNameAsync(string hostName)
        {
            return await InvokeAsync<SolidCP.Providers.Virtualization.VirtualNetworkInfo[]>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualNetworkByHostName", hostName);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class VirtualizationServerForPrivateCloud : SolidCP.Web.Client.ClientBase<IVirtualizationServerForPrivateCloud, VirtualizationServerForPrivateCloudAssemblyClient>, IVirtualizationServerForPrivateCloud
    {
        public SolidCP.Providers.Virtualization.VMInfo GetVirtualMachine(string vmId)
        {
            return base.Client.GetVirtualMachine(vmId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMInfo> GetVirtualMachineAsync(string vmId)
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

        public SolidCP.Providers.Virtualization.VMInfo CreateVirtualMachine(SolidCP.Providers.Virtualization.VMInfo vm)
        {
            return base.Client.CreateVirtualMachine(vm);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMInfo> CreateVirtualMachineAsync(SolidCP.Providers.Virtualization.VMInfo vm)
        {
            return await base.Client.CreateVirtualMachineAsync(vm);
        }

        public SolidCP.Providers.Virtualization.VMInfo CreateVMFromVM(string sourceName, SolidCP.Providers.Virtualization.VMInfo vmTemplate, System.Guid taskGuid)
        {
            return base.Client.CreateVMFromVM(sourceName, vmTemplate, taskGuid);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMInfo> CreateVMFromVMAsync(string sourceName, SolidCP.Providers.Virtualization.VMInfo vmTemplate, System.Guid taskGuid)
        {
            return await base.Client.CreateVMFromVMAsync(sourceName, vmTemplate, taskGuid);
        }

        public SolidCP.Providers.Virtualization.VMInfo UpdateVirtualMachine(SolidCP.Providers.Virtualization.VMInfo vm)
        {
            return base.Client.UpdateVirtualMachine(vm);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMInfo> UpdateVirtualMachineAsync(SolidCP.Providers.Virtualization.VMInfo vm)
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

        public SolidCP.Providers.Virtualization.JobResult DeleteSnapshot(string vmId, string snapshotId)
        {
            return base.Client.DeleteSnapshot(vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.JobResult> DeleteSnapshotAsync(string vmId, string snapshotId)
        {
            return await base.Client.DeleteSnapshotAsync(vmId, snapshotId);
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

        public SolidCP.Providers.Virtualization.LibraryItem[] GetOSLibraryItems()
        {
            return base.Client.GetOSLibraryItems();
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetOSLibraryItemsAsync()
        {
            return await base.Client.GetOSLibraryItemsAsync();
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetHosts()
        {
            return base.Client.GetHosts();
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetHostsAsync()
        {
            return await base.Client.GetHostsAsync();
        }

        public SolidCP.Providers.Virtualization.LibraryItem[] GetClusters()
        {
            return base.Client.GetClusters();
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.LibraryItem[]> GetClustersAsync()
        {
            return await base.Client.GetClustersAsync();
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

        public int GetProcessorCoresNumber(string templateId)
        {
            return base.Client.GetProcessorCoresNumber(templateId);
        }

        public async System.Threading.Tasks.Task<int> GetProcessorCoresNumberAsync(string templateId)
        {
            return await base.Client.GetProcessorCoresNumberAsync(templateId);
        }

        public bool CheckServerState(SolidCP.Providers.Virtualization.VMForPCSettingsName control, string connString, string connName)
        {
            return base.Client.CheckServerState(control, connString, connName);
        }

        public async System.Threading.Tasks.Task<bool> CheckServerStateAsync(SolidCP.Providers.Virtualization.VMForPCSettingsName control, string connString, string connName)
        {
            return await base.Client.CheckServerStateAsync(control, connString, connName);
        }

        public SolidCP.Providers.Virtualization.MonitoredObjectEvent[] /*List*/ GetDeviceEvents(string serviceName, string displayName)
        {
            return base.Client.GetDeviceEvents(serviceName, displayName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.MonitoredObjectEvent[]> GetDeviceEventsAsync(string serviceName, string displayName)
        {
            return await base.Client.GetDeviceEventsAsync(serviceName, displayName);
        }

        public SolidCP.Providers.Virtualization.MonitoredObjectAlert[] /*List*/ GetMonitoringAlerts(string serviceName, string virtualMachineName)
        {
            return base.Client.GetMonitoringAlerts(serviceName, virtualMachineName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.MonitoredObjectAlert[]> GetMonitoringAlertsAsync(string serviceName, string virtualMachineName)
        {
            return await base.Client.GetMonitoringAlertsAsync(serviceName, virtualMachineName);
        }

        public SolidCP.Providers.Virtualization.PerformanceDataValue[] /*List*/ GetPerfomanceValue(string VmName, SolidCP.Providers.Virtualization.PerformanceType perf, System.DateTime startPeriod, System.DateTime endPeriod)
        {
            return base.Client.GetPerfomanceValue(VmName, perf, startPeriod, endPeriod);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.PerformanceDataValue[]> GetPerfomanceValueAsync(string VmName, SolidCP.Providers.Virtualization.PerformanceType perf, System.DateTime startPeriod, System.DateTime endPeriod)
        {
            return await base.Client.GetPerfomanceValueAsync(VmName, perf, startPeriod, endPeriod);
        }

        public void ConfigureCreatedVMNetworkAdapters(SolidCP.Providers.Virtualization.VMInfo vmInfo)
        {
            base.Client.ConfigureCreatedVMNetworkAdapters(vmInfo);
        }

        public async System.Threading.Tasks.Task ConfigureCreatedVMNetworkAdaptersAsync(SolidCP.Providers.Virtualization.VMInfo vmInfo)
        {
            await base.Client.ConfigureCreatedVMNetworkAdaptersAsync(vmInfo);
        }

        public SolidCP.Providers.Virtualization.VMInfo MoveVM(SolidCP.Providers.Virtualization.VMInfo vmForMove)
        {
            return base.Client.MoveVM(vmForMove);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VMInfo> MoveVMAsync(SolidCP.Providers.Virtualization.VMInfo vmForMove)
        {
            return await base.Client.MoveVMAsync(vmForMove);
        }

        public SolidCP.Providers.Virtualization.VirtualNetworkInfo[] GetVirtualNetworkByHostName(string hostName)
        {
            return base.Client.GetVirtualNetworkByHostName(hostName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Virtualization.VirtualNetworkInfo[]> GetVirtualNetworkByHostNameAsync(string hostName)
        {
            return await base.Client.GetVirtualNetworkByHostNameAsync(hostName);
        }
    }
}
#endif