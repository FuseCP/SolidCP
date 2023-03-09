#if Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;
using SolidCP.Providers;
using SolidCP.Providers.Virtualization;
using SolidCP.Server.Utils;
using System.Collections.Generic;
using SolidCP.Server;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IVirtualizationServerForPrivateCloud", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IVirtualizationServerForPrivateCloud
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineResponse")]
        VMInfo GetVirtualMachine(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineResponse")]
        System.Threading.Tasks.Task<VMInfo> GetVirtualMachineAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineEx", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineExResponse")]
        VirtualMachine GetVirtualMachineEx(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineEx", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineExResponse")]
        System.Threading.Tasks.Task<VirtualMachine> GetVirtualMachineExAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachines", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachinesResponse")]
        List<VirtualMachine> GetVirtualMachines();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachines", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachinesResponse")]
        System.Threading.Tasks.Task<List<VirtualMachine>> GetVirtualMachinesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineThumbnailImageResponse")]
        byte[] GetVirtualMachineThumbnailImage(string vmId, ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineThumbnailImageResponse")]
        System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailImageAsync(string vmId, ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateVirtualMachineResponse")]
        VMInfo CreateVirtualMachine(VMInfo vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateVirtualMachineResponse")]
        System.Threading.Tasks.Task<VMInfo> CreateVirtualMachineAsync(VMInfo vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateVMFromVM", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateVMFromVMResponse")]
        VMInfo CreateVMFromVM(string sourceName, VMInfo vmTemplate, Guid taskGuid);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateVMFromVM", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateVMFromVMResponse")]
        System.Threading.Tasks.Task<VMInfo> CreateVMFromVMAsync(string sourceName, VMInfo vmTemplate, Guid taskGuid);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/UpdateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/UpdateVirtualMachineResponse")]
        VMInfo UpdateVirtualMachine(VMInfo vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/UpdateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/UpdateVirtualMachineResponse")]
        System.Threading.Tasks.Task<VMInfo> UpdateVirtualMachineAsync(VMInfo vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ChangeVirtualMachineState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ChangeVirtualMachineStateResponse")]
        JobResult ChangeVirtualMachineState(string vmId, VirtualMachineRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ChangeVirtualMachineState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ChangeVirtualMachineStateResponse")]
        System.Threading.Tasks.Task<JobResult> ChangeVirtualMachineStateAsync(string vmId, VirtualMachineRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ShutDownVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ShutDownVirtualMachineResponse")]
        ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ShutDownVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ShutDownVirtualMachineResponse")]
        System.Threading.Tasks.Task<ReturnCode> ShutDownVirtualMachineAsync(string vmId, bool force, string reason);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineJobsResponse")]
        List<ConcreteJob> GetVirtualMachineJobs(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineJobsResponse")]
        System.Threading.Tasks.Task<List<ConcreteJob>> GetVirtualMachineJobsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/RenameVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/RenameVirtualMachineResponse")]
        JobResult RenameVirtualMachine(string vmId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/RenameVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/RenameVirtualMachineResponse")]
        System.Threading.Tasks.Task<JobResult> RenameVirtualMachineAsync(string vmId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteVirtualMachineResponse")]
        JobResult DeleteVirtualMachine(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteVirtualMachineResponse")]
        System.Threading.Tasks.Task<JobResult> DeleteVirtualMachineAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineSnapshots", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineSnapshotsResponse")]
        List<VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineSnapshots", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualMachineSnapshotsResponse")]
        System.Threading.Tasks.Task<List<VirtualMachineSnapshot>> GetVirtualMachineSnapshotsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetSnapshotResponse")]
        VirtualMachineSnapshot GetSnapshot(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetSnapshotResponse")]
        System.Threading.Tasks.Task<VirtualMachineSnapshot> GetSnapshotAsync(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateSnapshotResponse")]
        JobResult CreateSnapshot(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateSnapshotResponse")]
        System.Threading.Tasks.Task<JobResult> CreateSnapshotAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/RenameSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/RenameSnapshotResponse")]
        JobResult RenameSnapshot(string vmId, string snapshotId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/RenameSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/RenameSnapshotResponse")]
        System.Threading.Tasks.Task<JobResult> RenameSnapshotAsync(string vmId, string snapshotId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ApplySnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ApplySnapshotResponse")]
        JobResult ApplySnapshot(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ApplySnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ApplySnapshotResponse")]
        System.Threading.Tasks.Task<JobResult> ApplySnapshotAsync(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteSnapshotResponse")]
        JobResult DeleteSnapshot(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteSnapshotResponse")]
        System.Threading.Tasks.Task<JobResult> DeleteSnapshotAsync(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteSnapshotSubtree", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteSnapshotSubtreeResponse")]
        JobResult DeleteSnapshotSubtree(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteSnapshotSubtree", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteSnapshotSubtreeResponse")]
        System.Threading.Tasks.Task<JobResult> DeleteSnapshotSubtreeAsync(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetSnapshotThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetSnapshotThumbnailImageResponse")]
        byte[] GetSnapshotThumbnailImage(string snapshotId, ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetSnapshotThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetSnapshotThumbnailImageResponse")]
        System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailImageAsync(string snapshotId, ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetExternalSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetExternalSwitchesResponse")]
        List<VirtualSwitch> GetExternalSwitches(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetExternalSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetExternalSwitchesResponse")]
        System.Threading.Tasks.Task<List<VirtualSwitch>> GetExternalSwitchesAsync(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetSwitchesResponse")]
        List<VirtualSwitch> GetSwitches();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetSwitchesResponse")]
        System.Threading.Tasks.Task<List<VirtualSwitch>> GetSwitchesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/SwitchExists", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/SwitchExistsResponse")]
        bool SwitchExists(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/SwitchExists", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/SwitchExistsResponse")]
        System.Threading.Tasks.Task<bool> SwitchExistsAsync(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateSwitchResponse")]
        VirtualSwitch CreateSwitch(string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CreateSwitchResponse")]
        System.Threading.Tasks.Task<VirtualSwitch> CreateSwitchAsync(string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteSwitchResponse")]
        ReturnCode DeleteSwitch(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/DeleteSwitchResponse")]
        System.Threading.Tasks.Task<ReturnCode> DeleteSwitchAsync(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetInsertedDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetInsertedDVDResponse")]
        string GetInsertedDVD(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetInsertedDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetInsertedDVDResponse")]
        System.Threading.Tasks.Task<string> GetInsertedDVDAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/InsertDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/InsertDVDResponse")]
        JobResult InsertDVD(string vmId, string isoPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/InsertDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/InsertDVDResponse")]
        System.Threading.Tasks.Task<JobResult> InsertDVDAsync(string vmId, string isoPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/EjectDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/EjectDVDResponse")]
        JobResult EjectDVD(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/EjectDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/EjectDVDResponse")]
        System.Threading.Tasks.Task<JobResult> EjectDVDAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetLibraryItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetLibraryItemsResponse")]
        LibraryItem[] GetLibraryItems(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetLibraryItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetLibraryItemsResponse")]
        System.Threading.Tasks.Task<LibraryItem[]> GetLibraryItemsAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetOSLibraryItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetOSLibraryItemsResponse")]
        LibraryItem[] GetOSLibraryItems();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetOSLibraryItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetOSLibraryItemsResponse")]
        System.Threading.Tasks.Task<LibraryItem[]> GetOSLibraryItemsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetHosts", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetHostsResponse")]
        LibraryItem[] GetHosts();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetHosts", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetHostsResponse")]
        System.Threading.Tasks.Task<LibraryItem[]> GetHostsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetClusters", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetClustersResponse")]
        LibraryItem[] GetClusters();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetClusters", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetClustersResponse")]
        System.Threading.Tasks.Task<LibraryItem[]> GetClustersAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetKVPItemsResponse")]
        List<KvpExchangeDataItem> GetKVPItems(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetKVPItemsResponse")]
        System.Threading.Tasks.Task<List<KvpExchangeDataItem>> GetKVPItemsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetStandardKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetStandardKVPItemsResponse")]
        List<KvpExchangeDataItem> GetStandardKVPItems(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetStandardKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetStandardKVPItemsResponse")]
        System.Threading.Tasks.Task<List<KvpExchangeDataItem>> GetStandardKVPItemsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/AddKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/AddKVPItemsResponse")]
        JobResult AddKVPItems(string vmId, KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/AddKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/AddKVPItemsResponse")]
        System.Threading.Tasks.Task<JobResult> AddKVPItemsAsync(string vmId, KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/RemoveKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/RemoveKVPItemsResponse")]
        JobResult RemoveKVPItems(string vmId, string[] itemNames);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/RemoveKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/RemoveKVPItemsResponse")]
        System.Threading.Tasks.Task<JobResult> RemoveKVPItemsAsync(string vmId, string[] itemNames);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ModifyKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ModifyKVPItemsResponse")]
        JobResult ModifyKVPItems(string vmId, KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ModifyKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ModifyKVPItemsResponse")]
        System.Threading.Tasks.Task<JobResult> ModifyKVPItemsAsync(string vmId, KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualHardDiskInfo", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualHardDiskInfoResponse")]
        VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualHardDiskInfo", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualHardDiskInfoResponse")]
        System.Threading.Tasks.Task<VirtualHardDiskInfo> GetVirtualHardDiskInfoAsync(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/MountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/MountVirtualHardDiskResponse")]
        MountedDiskInfo MountVirtualHardDisk(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/MountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/MountVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<MountedDiskInfo> MountVirtualHardDiskAsync(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/UnmountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/UnmountVirtualHardDiskResponse")]
        ReturnCode UnmountVirtualHardDisk(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/UnmountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/UnmountVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<ReturnCode> UnmountVirtualHardDiskAsync(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ExpandVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ExpandVirtualHardDiskResponse")]
        JobResult ExpandVirtualHardDisk(string vhdPath, UInt64 sizeGB);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ExpandVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ExpandVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<JobResult> ExpandVirtualHardDiskAsync(string vhdPath, UInt64 sizeGB);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ConvertVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ConvertVirtualHardDiskResponse")]
        JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, VirtualHardDiskType diskType);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ConvertVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ConvertVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<JobResult> ConvertVirtualHardDiskAsync(string sourcePath, string destinationPath, VirtualHardDiskType diskType);
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
        ConcreteJob GetJob(string jobId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetJob", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetJobResponse")]
        System.Threading.Tasks.Task<ConcreteJob> GetJobAsync(string jobId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetAllJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetAllJobsResponse")]
        List<ConcreteJob> GetAllJobs();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetAllJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetAllJobsResponse")]
        System.Threading.Tasks.Task<List<ConcreteJob>> GetAllJobsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ChangeJobState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ChangeJobStateResponse")]
        ChangeJobStateReturnCode ChangeJobState(string jobId, ConcreteJobRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ChangeJobState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ChangeJobStateResponse")]
        System.Threading.Tasks.Task<ChangeJobStateReturnCode> ChangeJobStateAsync(string jobId, ConcreteJobRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetProcessorCoresNumber", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetProcessorCoresNumberResponse")]
        int GetProcessorCoresNumber(string templateId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetProcessorCoresNumber", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetProcessorCoresNumberResponse")]
        System.Threading.Tasks.Task<int> GetProcessorCoresNumberAsync(string templateId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CheckServerState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CheckServerStateResponse")]
        bool CheckServerState(VMForPCSettingsName control, string connString, string connName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CheckServerState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/CheckServerStateResponse")]
        System.Threading.Tasks.Task<bool> CheckServerStateAsync(VMForPCSettingsName control, string connString, string connName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetDeviceEvents", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetDeviceEventsResponse")]
        List<MonitoredObjectEvent> GetDeviceEvents(string serviceName, string displayName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetDeviceEvents", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetDeviceEventsResponse")]
        System.Threading.Tasks.Task<List<MonitoredObjectEvent>> GetDeviceEventsAsync(string serviceName, string displayName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetMonitoringAlerts", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetMonitoringAlertsResponse")]
        List<MonitoredObjectAlert> GetMonitoringAlerts(string serviceName, string virtualMachineName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetMonitoringAlerts", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetMonitoringAlertsResponse")]
        System.Threading.Tasks.Task<List<MonitoredObjectAlert>> GetMonitoringAlertsAsync(string serviceName, string virtualMachineName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetPerfomanceValue", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetPerfomanceValueResponse")]
        List<PerformanceDataValue> GetPerfomanceValue(string VmName, PerformanceType perf, DateTime startPeriod, DateTime endPeriod);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetPerfomanceValue", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetPerfomanceValueResponse")]
        System.Threading.Tasks.Task<List<PerformanceDataValue>> GetPerfomanceValueAsync(string VmName, PerformanceType perf, DateTime startPeriod, DateTime endPeriod);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ConfigureCreatedVMNetworkAdapters", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ConfigureCreatedVMNetworkAdaptersResponse")]
        void ConfigureCreatedVMNetworkAdapters(VMInfo vmInfo);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ConfigureCreatedVMNetworkAdapters", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/ConfigureCreatedVMNetworkAdaptersResponse")]
        System.Threading.Tasks.Task ConfigureCreatedVMNetworkAdaptersAsync(VMInfo vmInfo);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/MoveVM", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/MoveVMResponse")]
        VMInfo MoveVM(VMInfo vmForMove);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/MoveVM", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/MoveVMResponse")]
        System.Threading.Tasks.Task<VMInfo> MoveVMAsync(VMInfo vmForMove);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualNetworkByHostName", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualNetworkByHostNameResponse")]
        VirtualNetworkInfo[] GetVirtualNetworkByHostName(string hostName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualNetworkByHostName", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerForPrivateCloud/GetVirtualNetworkByHostNameResponse")]
        System.Threading.Tasks.Task<VirtualNetworkInfo[]> GetVirtualNetworkByHostNameAsync(string hostName);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class VirtualizationServerForPrivateCloudAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IVirtualizationServerForPrivateCloud
    {
        public VMInfo GetVirtualMachine(string vmId)
        {
            return (VMInfo)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualMachine", vmId);
        }

        public async System.Threading.Tasks.Task<VMInfo> GetVirtualMachineAsync(string vmId)
        {
            return await InvokeAsync<VMInfo>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualMachine", vmId);
        }

        public VirtualMachine GetVirtualMachineEx(string vmId)
        {
            return (VirtualMachine)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualMachineEx", vmId);
        }

        public async System.Threading.Tasks.Task<VirtualMachine> GetVirtualMachineExAsync(string vmId)
        {
            return await InvokeAsync<VirtualMachine>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualMachineEx", vmId);
        }

        public List<VirtualMachine> GetVirtualMachines()
        {
            return (List<VirtualMachine>)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualMachines");
        }

        public async System.Threading.Tasks.Task<List<VirtualMachine>> GetVirtualMachinesAsync()
        {
            return await InvokeAsync<List<VirtualMachine>>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualMachines");
        }

        public byte[] GetVirtualMachineThumbnailImage(string vmId, ThumbnailSize size)
        {
            return (byte[])Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualMachineThumbnailImage", vmId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailImageAsync(string vmId, ThumbnailSize size)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualMachineThumbnailImage", vmId, size);
        }

        public VMInfo CreateVirtualMachine(VMInfo vm)
        {
            return (VMInfo)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "CreateVirtualMachine", vm);
        }

        public async System.Threading.Tasks.Task<VMInfo> CreateVirtualMachineAsync(VMInfo vm)
        {
            return await InvokeAsync<VMInfo>("SolidCP.Server.VirtualizationServerForPrivateCloud", "CreateVirtualMachine", vm);
        }

        public VMInfo CreateVMFromVM(string sourceName, VMInfo vmTemplate, Guid taskGuid)
        {
            return (VMInfo)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "CreateVMFromVM", sourceName, vmTemplate, taskGuid);
        }

        public async System.Threading.Tasks.Task<VMInfo> CreateVMFromVMAsync(string sourceName, VMInfo vmTemplate, Guid taskGuid)
        {
            return await InvokeAsync<VMInfo>("SolidCP.Server.VirtualizationServerForPrivateCloud", "CreateVMFromVM", sourceName, vmTemplate, taskGuid);
        }

        public VMInfo UpdateVirtualMachine(VMInfo vm)
        {
            return (VMInfo)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "UpdateVirtualMachine", vm);
        }

        public async System.Threading.Tasks.Task<VMInfo> UpdateVirtualMachineAsync(VMInfo vm)
        {
            return await InvokeAsync<VMInfo>("SolidCP.Server.VirtualizationServerForPrivateCloud", "UpdateVirtualMachine", vm);
        }

        public JobResult ChangeVirtualMachineState(string vmId, VirtualMachineRequestedState newState)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "ChangeVirtualMachineState", vmId, newState);
        }

        public async System.Threading.Tasks.Task<JobResult> ChangeVirtualMachineStateAsync(string vmId, VirtualMachineRequestedState newState)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "ChangeVirtualMachineState", vmId, newState);
        }

        public ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason)
        {
            return (ReturnCode)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "ShutDownVirtualMachine", vmId, force, reason);
        }

        public async System.Threading.Tasks.Task<ReturnCode> ShutDownVirtualMachineAsync(string vmId, bool force, string reason)
        {
            return await InvokeAsync<ReturnCode>("SolidCP.Server.VirtualizationServerForPrivateCloud", "ShutDownVirtualMachine", vmId, force, reason);
        }

        public List<ConcreteJob> GetVirtualMachineJobs(string vmId)
        {
            return (List<ConcreteJob>)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualMachineJobs", vmId);
        }

        public async System.Threading.Tasks.Task<List<ConcreteJob>> GetVirtualMachineJobsAsync(string vmId)
        {
            return await InvokeAsync<List<ConcreteJob>>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualMachineJobs", vmId);
        }

        public JobResult RenameVirtualMachine(string vmId, string name)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "RenameVirtualMachine", vmId, name);
        }

        public async System.Threading.Tasks.Task<JobResult> RenameVirtualMachineAsync(string vmId, string name)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "RenameVirtualMachine", vmId, name);
        }

        public JobResult DeleteVirtualMachine(string vmId)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "DeleteVirtualMachine", vmId);
        }

        public async System.Threading.Tasks.Task<JobResult> DeleteVirtualMachineAsync(string vmId)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "DeleteVirtualMachine", vmId);
        }

        public List<VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId)
        {
            return (List<VirtualMachineSnapshot>)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualMachineSnapshots", vmId);
        }

        public async System.Threading.Tasks.Task<List<VirtualMachineSnapshot>> GetVirtualMachineSnapshotsAsync(string vmId)
        {
            return await InvokeAsync<List<VirtualMachineSnapshot>>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualMachineSnapshots", vmId);
        }

        public VirtualMachineSnapshot GetSnapshot(string snapshotId)
        {
            return (VirtualMachineSnapshot)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetSnapshot", snapshotId);
        }

        public async System.Threading.Tasks.Task<VirtualMachineSnapshot> GetSnapshotAsync(string snapshotId)
        {
            return await InvokeAsync<VirtualMachineSnapshot>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetSnapshot", snapshotId);
        }

        public JobResult CreateSnapshot(string vmId)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "CreateSnapshot", vmId);
        }

        public async System.Threading.Tasks.Task<JobResult> CreateSnapshotAsync(string vmId)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "CreateSnapshot", vmId);
        }

        public JobResult RenameSnapshot(string vmId, string snapshotId, string name)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "RenameSnapshot", vmId, snapshotId, name);
        }

        public async System.Threading.Tasks.Task<JobResult> RenameSnapshotAsync(string vmId, string snapshotId, string name)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "RenameSnapshot", vmId, snapshotId, name);
        }

        public JobResult ApplySnapshot(string vmId, string snapshotId)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "ApplySnapshot", vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<JobResult> ApplySnapshotAsync(string vmId, string snapshotId)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "ApplySnapshot", vmId, snapshotId);
        }

        public JobResult DeleteSnapshot(string vmId, string snapshotId)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "DeleteSnapshot", vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<JobResult> DeleteSnapshotAsync(string vmId, string snapshotId)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "DeleteSnapshot", vmId, snapshotId);
        }

        public JobResult DeleteSnapshotSubtree(string snapshotId)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "DeleteSnapshotSubtree", snapshotId);
        }

        public async System.Threading.Tasks.Task<JobResult> DeleteSnapshotSubtreeAsync(string snapshotId)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "DeleteSnapshotSubtree", snapshotId);
        }

        public byte[] GetSnapshotThumbnailImage(string snapshotId, ThumbnailSize size)
        {
            return (byte[])Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetSnapshotThumbnailImage", snapshotId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailImageAsync(string snapshotId, ThumbnailSize size)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetSnapshotThumbnailImage", snapshotId, size);
        }

        public List<VirtualSwitch> GetExternalSwitches(string computerName)
        {
            return (List<VirtualSwitch>)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetExternalSwitches", computerName);
        }

        public async System.Threading.Tasks.Task<List<VirtualSwitch>> GetExternalSwitchesAsync(string computerName)
        {
            return await InvokeAsync<List<VirtualSwitch>>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetExternalSwitches", computerName);
        }

        public List<VirtualSwitch> GetSwitches()
        {
            return (List<VirtualSwitch>)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetSwitches");
        }

        public async System.Threading.Tasks.Task<List<VirtualSwitch>> GetSwitchesAsync()
        {
            return await InvokeAsync<List<VirtualSwitch>>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetSwitches");
        }

        public bool SwitchExists(string switchId)
        {
            return (bool)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "SwitchExists", switchId);
        }

        public async System.Threading.Tasks.Task<bool> SwitchExistsAsync(string switchId)
        {
            return await InvokeAsync<bool>("SolidCP.Server.VirtualizationServerForPrivateCloud", "SwitchExists", switchId);
        }

        public VirtualSwitch CreateSwitch(string name)
        {
            return (VirtualSwitch)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "CreateSwitch", name);
        }

        public async System.Threading.Tasks.Task<VirtualSwitch> CreateSwitchAsync(string name)
        {
            return await InvokeAsync<VirtualSwitch>("SolidCP.Server.VirtualizationServerForPrivateCloud", "CreateSwitch", name);
        }

        public ReturnCode DeleteSwitch(string switchId)
        {
            return (ReturnCode)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "DeleteSwitch", switchId);
        }

        public async System.Threading.Tasks.Task<ReturnCode> DeleteSwitchAsync(string switchId)
        {
            return await InvokeAsync<ReturnCode>("SolidCP.Server.VirtualizationServerForPrivateCloud", "DeleteSwitch", switchId);
        }

        public string GetInsertedDVD(string vmId)
        {
            return (string)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetInsertedDVD", vmId);
        }

        public async System.Threading.Tasks.Task<string> GetInsertedDVDAsync(string vmId)
        {
            return await InvokeAsync<string>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetInsertedDVD", vmId);
        }

        public JobResult InsertDVD(string vmId, string isoPath)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "InsertDVD", vmId, isoPath);
        }

        public async System.Threading.Tasks.Task<JobResult> InsertDVDAsync(string vmId, string isoPath)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "InsertDVD", vmId, isoPath);
        }

        public JobResult EjectDVD(string vmId)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "EjectDVD", vmId);
        }

        public async System.Threading.Tasks.Task<JobResult> EjectDVDAsync(string vmId)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "EjectDVD", vmId);
        }

        public LibraryItem[] GetLibraryItems(string path)
        {
            return (LibraryItem[])Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetLibraryItems", path);
        }

        public async System.Threading.Tasks.Task<LibraryItem[]> GetLibraryItemsAsync(string path)
        {
            return await InvokeAsync<LibraryItem[]>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetLibraryItems", path);
        }

        public LibraryItem[] GetOSLibraryItems()
        {
            return (LibraryItem[])Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetOSLibraryItems");
        }

        public async System.Threading.Tasks.Task<LibraryItem[]> GetOSLibraryItemsAsync()
        {
            return await InvokeAsync<LibraryItem[]>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetOSLibraryItems");
        }

        public LibraryItem[] GetHosts()
        {
            return (LibraryItem[])Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetHosts");
        }

        public async System.Threading.Tasks.Task<LibraryItem[]> GetHostsAsync()
        {
            return await InvokeAsync<LibraryItem[]>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetHosts");
        }

        public LibraryItem[] GetClusters()
        {
            return (LibraryItem[])Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetClusters");
        }

        public async System.Threading.Tasks.Task<LibraryItem[]> GetClustersAsync()
        {
            return await InvokeAsync<LibraryItem[]>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetClusters");
        }

        public List<KvpExchangeDataItem> GetKVPItems(string vmId)
        {
            return (List<KvpExchangeDataItem>)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetKVPItems", vmId);
        }

        public async System.Threading.Tasks.Task<List<KvpExchangeDataItem>> GetKVPItemsAsync(string vmId)
        {
            return await InvokeAsync<List<KvpExchangeDataItem>>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetKVPItems", vmId);
        }

        public List<KvpExchangeDataItem> GetStandardKVPItems(string vmId)
        {
            return (List<KvpExchangeDataItem>)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetStandardKVPItems", vmId);
        }

        public async System.Threading.Tasks.Task<List<KvpExchangeDataItem>> GetStandardKVPItemsAsync(string vmId)
        {
            return await InvokeAsync<List<KvpExchangeDataItem>>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetStandardKVPItems", vmId);
        }

        public JobResult AddKVPItems(string vmId, KvpExchangeDataItem[] items)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "AddKVPItems", vmId, items);
        }

        public async System.Threading.Tasks.Task<JobResult> AddKVPItemsAsync(string vmId, KvpExchangeDataItem[] items)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "AddKVPItems", vmId, items);
        }

        public JobResult RemoveKVPItems(string vmId, string[] itemNames)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "RemoveKVPItems", vmId, itemNames);
        }

        public async System.Threading.Tasks.Task<JobResult> RemoveKVPItemsAsync(string vmId, string[] itemNames)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "RemoveKVPItems", vmId, itemNames);
        }

        public JobResult ModifyKVPItems(string vmId, KvpExchangeDataItem[] items)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "ModifyKVPItems", vmId, items);
        }

        public async System.Threading.Tasks.Task<JobResult> ModifyKVPItemsAsync(string vmId, KvpExchangeDataItem[] items)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "ModifyKVPItems", vmId, items);
        }

        public VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath)
        {
            return (VirtualHardDiskInfo)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualHardDiskInfo", vhdPath);
        }

        public async System.Threading.Tasks.Task<VirtualHardDiskInfo> GetVirtualHardDiskInfoAsync(string vhdPath)
        {
            return await InvokeAsync<VirtualHardDiskInfo>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualHardDiskInfo", vhdPath);
        }

        public MountedDiskInfo MountVirtualHardDisk(string vhdPath)
        {
            return (MountedDiskInfo)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "MountVirtualHardDisk", vhdPath);
        }

        public async System.Threading.Tasks.Task<MountedDiskInfo> MountVirtualHardDiskAsync(string vhdPath)
        {
            return await InvokeAsync<MountedDiskInfo>("SolidCP.Server.VirtualizationServerForPrivateCloud", "MountVirtualHardDisk", vhdPath);
        }

        public ReturnCode UnmountVirtualHardDisk(string vhdPath)
        {
            return (ReturnCode)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "UnmountVirtualHardDisk", vhdPath);
        }

        public async System.Threading.Tasks.Task<ReturnCode> UnmountVirtualHardDiskAsync(string vhdPath)
        {
            return await InvokeAsync<ReturnCode>("SolidCP.Server.VirtualizationServerForPrivateCloud", "UnmountVirtualHardDisk", vhdPath);
        }

        public JobResult ExpandVirtualHardDisk(string vhdPath, UInt64 sizeGB)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "ExpandVirtualHardDisk", vhdPath, sizeGB);
        }

        public async System.Threading.Tasks.Task<JobResult> ExpandVirtualHardDiskAsync(string vhdPath, UInt64 sizeGB)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "ExpandVirtualHardDisk", vhdPath, sizeGB);
        }

        public JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, VirtualHardDiskType diskType)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "ConvertVirtualHardDisk", sourcePath, destinationPath, diskType);
        }

        public async System.Threading.Tasks.Task<JobResult> ConvertVirtualHardDiskAsync(string sourcePath, string destinationPath, VirtualHardDiskType diskType)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerForPrivateCloud", "ConvertVirtualHardDisk", sourcePath, destinationPath, diskType);
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
            return (string)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "ReadRemoteFile", path);
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

        public ConcreteJob GetJob(string jobId)
        {
            return (ConcreteJob)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetJob", jobId);
        }

        public async System.Threading.Tasks.Task<ConcreteJob> GetJobAsync(string jobId)
        {
            return await InvokeAsync<ConcreteJob>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetJob", jobId);
        }

        public List<ConcreteJob> GetAllJobs()
        {
            return (List<ConcreteJob>)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetAllJobs");
        }

        public async System.Threading.Tasks.Task<List<ConcreteJob>> GetAllJobsAsync()
        {
            return await InvokeAsync<List<ConcreteJob>>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetAllJobs");
        }

        public ChangeJobStateReturnCode ChangeJobState(string jobId, ConcreteJobRequestedState newState)
        {
            return (ChangeJobStateReturnCode)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "ChangeJobState", jobId, newState);
        }

        public async System.Threading.Tasks.Task<ChangeJobStateReturnCode> ChangeJobStateAsync(string jobId, ConcreteJobRequestedState newState)
        {
            return await InvokeAsync<ChangeJobStateReturnCode>("SolidCP.Server.VirtualizationServerForPrivateCloud", "ChangeJobState", jobId, newState);
        }

        public int GetProcessorCoresNumber(string templateId)
        {
            return (int)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetProcessorCoresNumber", templateId);
        }

        public async System.Threading.Tasks.Task<int> GetProcessorCoresNumberAsync(string templateId)
        {
            return await InvokeAsync<int>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetProcessorCoresNumber", templateId);
        }

        public bool CheckServerState(VMForPCSettingsName control, string connString, string connName)
        {
            return (bool)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "CheckServerState", control, connString, connName);
        }

        public async System.Threading.Tasks.Task<bool> CheckServerStateAsync(VMForPCSettingsName control, string connString, string connName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.VirtualizationServerForPrivateCloud", "CheckServerState", control, connString, connName);
        }

        public List<MonitoredObjectEvent> GetDeviceEvents(string serviceName, string displayName)
        {
            return (List<MonitoredObjectEvent>)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetDeviceEvents", serviceName, displayName);
        }

        public async System.Threading.Tasks.Task<List<MonitoredObjectEvent>> GetDeviceEventsAsync(string serviceName, string displayName)
        {
            return await InvokeAsync<List<MonitoredObjectEvent>>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetDeviceEvents", serviceName, displayName);
        }

        public List<MonitoredObjectAlert> GetMonitoringAlerts(string serviceName, string virtualMachineName)
        {
            return (List<MonitoredObjectAlert>)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetMonitoringAlerts", serviceName, virtualMachineName);
        }

        public async System.Threading.Tasks.Task<List<MonitoredObjectAlert>> GetMonitoringAlertsAsync(string serviceName, string virtualMachineName)
        {
            return await InvokeAsync<List<MonitoredObjectAlert>>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetMonitoringAlerts", serviceName, virtualMachineName);
        }

        public List<PerformanceDataValue> GetPerfomanceValue(string VmName, PerformanceType perf, DateTime startPeriod, DateTime endPeriod)
        {
            return (List<PerformanceDataValue>)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetPerfomanceValue", VmName, perf, startPeriod, endPeriod);
        }

        public async System.Threading.Tasks.Task<List<PerformanceDataValue>> GetPerfomanceValueAsync(string VmName, PerformanceType perf, DateTime startPeriod, DateTime endPeriod)
        {
            return await InvokeAsync<List<PerformanceDataValue>>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetPerfomanceValue", VmName, perf, startPeriod, endPeriod);
        }

        public void ConfigureCreatedVMNetworkAdapters(VMInfo vmInfo)
        {
            Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "ConfigureCreatedVMNetworkAdapters", vmInfo);
        }

        public async System.Threading.Tasks.Task ConfigureCreatedVMNetworkAdaptersAsync(VMInfo vmInfo)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServerForPrivateCloud", "ConfigureCreatedVMNetworkAdapters", vmInfo);
        }

        public VMInfo MoveVM(VMInfo vmForMove)
        {
            return (VMInfo)Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "MoveVM", vmForMove);
        }

        public async System.Threading.Tasks.Task<VMInfo> MoveVMAsync(VMInfo vmForMove)
        {
            return await InvokeAsync<VMInfo>("SolidCP.Server.VirtualizationServerForPrivateCloud", "MoveVM", vmForMove);
        }

        public VirtualNetworkInfo[] GetVirtualNetworkByHostName(string hostName)
        {
            return (VirtualNetworkInfo[])Invoke("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualNetworkByHostName", hostName);
        }

        public async System.Threading.Tasks.Task<VirtualNetworkInfo[]> GetVirtualNetworkByHostNameAsync(string hostName)
        {
            return await InvokeAsync<VirtualNetworkInfo[]>("SolidCP.Server.VirtualizationServerForPrivateCloud", "GetVirtualNetworkByHostName", hostName);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class VirtualizationServerForPrivateCloud : SolidCP.Web.Client.ClientBase<IVirtualizationServerForPrivateCloud, VirtualizationServerForPrivateCloudAssemblyClient>, IVirtualizationServerForPrivateCloud
    {
        public VMInfo GetVirtualMachine(string vmId)
        {
            return base.Client.GetVirtualMachine(vmId);
        }

        public async System.Threading.Tasks.Task<VMInfo> GetVirtualMachineAsync(string vmId)
        {
            return await base.Client.GetVirtualMachineAsync(vmId);
        }

        public VirtualMachine GetVirtualMachineEx(string vmId)
        {
            return base.Client.GetVirtualMachineEx(vmId);
        }

        public async System.Threading.Tasks.Task<VirtualMachine> GetVirtualMachineExAsync(string vmId)
        {
            return await base.Client.GetVirtualMachineExAsync(vmId);
        }

        public List<VirtualMachine> GetVirtualMachines()
        {
            return base.Client.GetVirtualMachines();
        }

        public async System.Threading.Tasks.Task<List<VirtualMachine>> GetVirtualMachinesAsync()
        {
            return await base.Client.GetVirtualMachinesAsync();
        }

        public byte[] GetVirtualMachineThumbnailImage(string vmId, ThumbnailSize size)
        {
            return base.Client.GetVirtualMachineThumbnailImage(vmId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailImageAsync(string vmId, ThumbnailSize size)
        {
            return await base.Client.GetVirtualMachineThumbnailImageAsync(vmId, size);
        }

        public VMInfo CreateVirtualMachine(VMInfo vm)
        {
            return base.Client.CreateVirtualMachine(vm);
        }

        public async System.Threading.Tasks.Task<VMInfo> CreateVirtualMachineAsync(VMInfo vm)
        {
            return await base.Client.CreateVirtualMachineAsync(vm);
        }

        public VMInfo CreateVMFromVM(string sourceName, VMInfo vmTemplate, Guid taskGuid)
        {
            return base.Client.CreateVMFromVM(sourceName, vmTemplate, taskGuid);
        }

        public async System.Threading.Tasks.Task<VMInfo> CreateVMFromVMAsync(string sourceName, VMInfo vmTemplate, Guid taskGuid)
        {
            return await base.Client.CreateVMFromVMAsync(sourceName, vmTemplate, taskGuid);
        }

        public VMInfo UpdateVirtualMachine(VMInfo vm)
        {
            return base.Client.UpdateVirtualMachine(vm);
        }

        public async System.Threading.Tasks.Task<VMInfo> UpdateVirtualMachineAsync(VMInfo vm)
        {
            return await base.Client.UpdateVirtualMachineAsync(vm);
        }

        public JobResult ChangeVirtualMachineState(string vmId, VirtualMachineRequestedState newState)
        {
            return base.Client.ChangeVirtualMachineState(vmId, newState);
        }

        public async System.Threading.Tasks.Task<JobResult> ChangeVirtualMachineStateAsync(string vmId, VirtualMachineRequestedState newState)
        {
            return await base.Client.ChangeVirtualMachineStateAsync(vmId, newState);
        }

        public ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason)
        {
            return base.Client.ShutDownVirtualMachine(vmId, force, reason);
        }

        public async System.Threading.Tasks.Task<ReturnCode> ShutDownVirtualMachineAsync(string vmId, bool force, string reason)
        {
            return await base.Client.ShutDownVirtualMachineAsync(vmId, force, reason);
        }

        public List<ConcreteJob> GetVirtualMachineJobs(string vmId)
        {
            return base.Client.GetVirtualMachineJobs(vmId);
        }

        public async System.Threading.Tasks.Task<List<ConcreteJob>> GetVirtualMachineJobsAsync(string vmId)
        {
            return await base.Client.GetVirtualMachineJobsAsync(vmId);
        }

        public JobResult RenameVirtualMachine(string vmId, string name)
        {
            return base.Client.RenameVirtualMachine(vmId, name);
        }

        public async System.Threading.Tasks.Task<JobResult> RenameVirtualMachineAsync(string vmId, string name)
        {
            return await base.Client.RenameVirtualMachineAsync(vmId, name);
        }

        public JobResult DeleteVirtualMachine(string vmId)
        {
            return base.Client.DeleteVirtualMachine(vmId);
        }

        public async System.Threading.Tasks.Task<JobResult> DeleteVirtualMachineAsync(string vmId)
        {
            return await base.Client.DeleteVirtualMachineAsync(vmId);
        }

        public List<VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId)
        {
            return base.Client.GetVirtualMachineSnapshots(vmId);
        }

        public async System.Threading.Tasks.Task<List<VirtualMachineSnapshot>> GetVirtualMachineSnapshotsAsync(string vmId)
        {
            return await base.Client.GetVirtualMachineSnapshotsAsync(vmId);
        }

        public VirtualMachineSnapshot GetSnapshot(string snapshotId)
        {
            return base.Client.GetSnapshot(snapshotId);
        }

        public async System.Threading.Tasks.Task<VirtualMachineSnapshot> GetSnapshotAsync(string snapshotId)
        {
            return await base.Client.GetSnapshotAsync(snapshotId);
        }

        public JobResult CreateSnapshot(string vmId)
        {
            return base.Client.CreateSnapshot(vmId);
        }

        public async System.Threading.Tasks.Task<JobResult> CreateSnapshotAsync(string vmId)
        {
            return await base.Client.CreateSnapshotAsync(vmId);
        }

        public JobResult RenameSnapshot(string vmId, string snapshotId, string name)
        {
            return base.Client.RenameSnapshot(vmId, snapshotId, name);
        }

        public async System.Threading.Tasks.Task<JobResult> RenameSnapshotAsync(string vmId, string snapshotId, string name)
        {
            return await base.Client.RenameSnapshotAsync(vmId, snapshotId, name);
        }

        public JobResult ApplySnapshot(string vmId, string snapshotId)
        {
            return base.Client.ApplySnapshot(vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<JobResult> ApplySnapshotAsync(string vmId, string snapshotId)
        {
            return await base.Client.ApplySnapshotAsync(vmId, snapshotId);
        }

        public JobResult DeleteSnapshot(string vmId, string snapshotId)
        {
            return base.Client.DeleteSnapshot(vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<JobResult> DeleteSnapshotAsync(string vmId, string snapshotId)
        {
            return await base.Client.DeleteSnapshotAsync(vmId, snapshotId);
        }

        public JobResult DeleteSnapshotSubtree(string snapshotId)
        {
            return base.Client.DeleteSnapshotSubtree(snapshotId);
        }

        public async System.Threading.Tasks.Task<JobResult> DeleteSnapshotSubtreeAsync(string snapshotId)
        {
            return await base.Client.DeleteSnapshotSubtreeAsync(snapshotId);
        }

        public byte[] GetSnapshotThumbnailImage(string snapshotId, ThumbnailSize size)
        {
            return base.Client.GetSnapshotThumbnailImage(snapshotId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailImageAsync(string snapshotId, ThumbnailSize size)
        {
            return await base.Client.GetSnapshotThumbnailImageAsync(snapshotId, size);
        }

        public List<VirtualSwitch> GetExternalSwitches(string computerName)
        {
            return base.Client.GetExternalSwitches(computerName);
        }

        public async System.Threading.Tasks.Task<List<VirtualSwitch>> GetExternalSwitchesAsync(string computerName)
        {
            return await base.Client.GetExternalSwitchesAsync(computerName);
        }

        public List<VirtualSwitch> GetSwitches()
        {
            return base.Client.GetSwitches();
        }

        public async System.Threading.Tasks.Task<List<VirtualSwitch>> GetSwitchesAsync()
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

        public VirtualSwitch CreateSwitch(string name)
        {
            return base.Client.CreateSwitch(name);
        }

        public async System.Threading.Tasks.Task<VirtualSwitch> CreateSwitchAsync(string name)
        {
            return await base.Client.CreateSwitchAsync(name);
        }

        public ReturnCode DeleteSwitch(string switchId)
        {
            return base.Client.DeleteSwitch(switchId);
        }

        public async System.Threading.Tasks.Task<ReturnCode> DeleteSwitchAsync(string switchId)
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

        public JobResult InsertDVD(string vmId, string isoPath)
        {
            return base.Client.InsertDVD(vmId, isoPath);
        }

        public async System.Threading.Tasks.Task<JobResult> InsertDVDAsync(string vmId, string isoPath)
        {
            return await base.Client.InsertDVDAsync(vmId, isoPath);
        }

        public JobResult EjectDVD(string vmId)
        {
            return base.Client.EjectDVD(vmId);
        }

        public async System.Threading.Tasks.Task<JobResult> EjectDVDAsync(string vmId)
        {
            return await base.Client.EjectDVDAsync(vmId);
        }

        public LibraryItem[] GetLibraryItems(string path)
        {
            return base.Client.GetLibraryItems(path);
        }

        public async System.Threading.Tasks.Task<LibraryItem[]> GetLibraryItemsAsync(string path)
        {
            return await base.Client.GetLibraryItemsAsync(path);
        }

        public LibraryItem[] GetOSLibraryItems()
        {
            return base.Client.GetOSLibraryItems();
        }

        public async System.Threading.Tasks.Task<LibraryItem[]> GetOSLibraryItemsAsync()
        {
            return await base.Client.GetOSLibraryItemsAsync();
        }

        public LibraryItem[] GetHosts()
        {
            return base.Client.GetHosts();
        }

        public async System.Threading.Tasks.Task<LibraryItem[]> GetHostsAsync()
        {
            return await base.Client.GetHostsAsync();
        }

        public LibraryItem[] GetClusters()
        {
            return base.Client.GetClusters();
        }

        public async System.Threading.Tasks.Task<LibraryItem[]> GetClustersAsync()
        {
            return await base.Client.GetClustersAsync();
        }

        public List<KvpExchangeDataItem> GetKVPItems(string vmId)
        {
            return base.Client.GetKVPItems(vmId);
        }

        public async System.Threading.Tasks.Task<List<KvpExchangeDataItem>> GetKVPItemsAsync(string vmId)
        {
            return await base.Client.GetKVPItemsAsync(vmId);
        }

        public List<KvpExchangeDataItem> GetStandardKVPItems(string vmId)
        {
            return base.Client.GetStandardKVPItems(vmId);
        }

        public async System.Threading.Tasks.Task<List<KvpExchangeDataItem>> GetStandardKVPItemsAsync(string vmId)
        {
            return await base.Client.GetStandardKVPItemsAsync(vmId);
        }

        public JobResult AddKVPItems(string vmId, KvpExchangeDataItem[] items)
        {
            return base.Client.AddKVPItems(vmId, items);
        }

        public async System.Threading.Tasks.Task<JobResult> AddKVPItemsAsync(string vmId, KvpExchangeDataItem[] items)
        {
            return await base.Client.AddKVPItemsAsync(vmId, items);
        }

        public JobResult RemoveKVPItems(string vmId, string[] itemNames)
        {
            return base.Client.RemoveKVPItems(vmId, itemNames);
        }

        public async System.Threading.Tasks.Task<JobResult> RemoveKVPItemsAsync(string vmId, string[] itemNames)
        {
            return await base.Client.RemoveKVPItemsAsync(vmId, itemNames);
        }

        public JobResult ModifyKVPItems(string vmId, KvpExchangeDataItem[] items)
        {
            return base.Client.ModifyKVPItems(vmId, items);
        }

        public async System.Threading.Tasks.Task<JobResult> ModifyKVPItemsAsync(string vmId, KvpExchangeDataItem[] items)
        {
            return await base.Client.ModifyKVPItemsAsync(vmId, items);
        }

        public VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath)
        {
            return base.Client.GetVirtualHardDiskInfo(vhdPath);
        }

        public async System.Threading.Tasks.Task<VirtualHardDiskInfo> GetVirtualHardDiskInfoAsync(string vhdPath)
        {
            return await base.Client.GetVirtualHardDiskInfoAsync(vhdPath);
        }

        public MountedDiskInfo MountVirtualHardDisk(string vhdPath)
        {
            return base.Client.MountVirtualHardDisk(vhdPath);
        }

        public async System.Threading.Tasks.Task<MountedDiskInfo> MountVirtualHardDiskAsync(string vhdPath)
        {
            return await base.Client.MountVirtualHardDiskAsync(vhdPath);
        }

        public ReturnCode UnmountVirtualHardDisk(string vhdPath)
        {
            return base.Client.UnmountVirtualHardDisk(vhdPath);
        }

        public async System.Threading.Tasks.Task<ReturnCode> UnmountVirtualHardDiskAsync(string vhdPath)
        {
            return await base.Client.UnmountVirtualHardDiskAsync(vhdPath);
        }

        public JobResult ExpandVirtualHardDisk(string vhdPath, UInt64 sizeGB)
        {
            return base.Client.ExpandVirtualHardDisk(vhdPath, sizeGB);
        }

        public async System.Threading.Tasks.Task<JobResult> ExpandVirtualHardDiskAsync(string vhdPath, UInt64 sizeGB)
        {
            return await base.Client.ExpandVirtualHardDiskAsync(vhdPath, sizeGB);
        }

        public JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, VirtualHardDiskType diskType)
        {
            return base.Client.ConvertVirtualHardDisk(sourcePath, destinationPath, diskType);
        }

        public async System.Threading.Tasks.Task<JobResult> ConvertVirtualHardDiskAsync(string sourcePath, string destinationPath, VirtualHardDiskType diskType)
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

        public ConcreteJob GetJob(string jobId)
        {
            return base.Client.GetJob(jobId);
        }

        public async System.Threading.Tasks.Task<ConcreteJob> GetJobAsync(string jobId)
        {
            return await base.Client.GetJobAsync(jobId);
        }

        public List<ConcreteJob> GetAllJobs()
        {
            return base.Client.GetAllJobs();
        }

        public async System.Threading.Tasks.Task<List<ConcreteJob>> GetAllJobsAsync()
        {
            return await base.Client.GetAllJobsAsync();
        }

        public ChangeJobStateReturnCode ChangeJobState(string jobId, ConcreteJobRequestedState newState)
        {
            return base.Client.ChangeJobState(jobId, newState);
        }

        public async System.Threading.Tasks.Task<ChangeJobStateReturnCode> ChangeJobStateAsync(string jobId, ConcreteJobRequestedState newState)
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

        public bool CheckServerState(VMForPCSettingsName control, string connString, string connName)
        {
            return base.Client.CheckServerState(control, connString, connName);
        }

        public async System.Threading.Tasks.Task<bool> CheckServerStateAsync(VMForPCSettingsName control, string connString, string connName)
        {
            return await base.Client.CheckServerStateAsync(control, connString, connName);
        }

        public List<MonitoredObjectEvent> GetDeviceEvents(string serviceName, string displayName)
        {
            return base.Client.GetDeviceEvents(serviceName, displayName);
        }

        public async System.Threading.Tasks.Task<List<MonitoredObjectEvent>> GetDeviceEventsAsync(string serviceName, string displayName)
        {
            return await base.Client.GetDeviceEventsAsync(serviceName, displayName);
        }

        public List<MonitoredObjectAlert> GetMonitoringAlerts(string serviceName, string virtualMachineName)
        {
            return base.Client.GetMonitoringAlerts(serviceName, virtualMachineName);
        }

        public async System.Threading.Tasks.Task<List<MonitoredObjectAlert>> GetMonitoringAlertsAsync(string serviceName, string virtualMachineName)
        {
            return await base.Client.GetMonitoringAlertsAsync(serviceName, virtualMachineName);
        }

        public List<PerformanceDataValue> GetPerfomanceValue(string VmName, PerformanceType perf, DateTime startPeriod, DateTime endPeriod)
        {
            return base.Client.GetPerfomanceValue(VmName, perf, startPeriod, endPeriod);
        }

        public async System.Threading.Tasks.Task<List<PerformanceDataValue>> GetPerfomanceValueAsync(string VmName, PerformanceType perf, DateTime startPeriod, DateTime endPeriod)
        {
            return await base.Client.GetPerfomanceValueAsync(VmName, perf, startPeriod, endPeriod);
        }

        public void ConfigureCreatedVMNetworkAdapters(VMInfo vmInfo)
        {
            base.Client.ConfigureCreatedVMNetworkAdapters(vmInfo);
        }

        public async System.Threading.Tasks.Task ConfigureCreatedVMNetworkAdaptersAsync(VMInfo vmInfo)
        {
            await base.Client.ConfigureCreatedVMNetworkAdaptersAsync(vmInfo);
        }

        public VMInfo MoveVM(VMInfo vmForMove)
        {
            return base.Client.MoveVM(vmForMove);
        }

        public async System.Threading.Tasks.Task<VMInfo> MoveVMAsync(VMInfo vmForMove)
        {
            return await base.Client.MoveVMAsync(vmForMove);
        }

        public VirtualNetworkInfo[] GetVirtualNetworkByHostName(string hostName)
        {
            return base.Client.GetVirtualNetworkByHostName(hostName);
        }

        public async System.Threading.Tasks.Task<VirtualNetworkInfo[]> GetVirtualNetworkByHostNameAsync(string hostName)
        {
            return await base.Client.GetVirtualNetworkByHostNameAsync(hostName);
        }
    }
}
#endif