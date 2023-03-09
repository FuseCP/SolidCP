#if Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;
using System.IO;
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
    [ServiceContract(ConfigurationName = "IVirtualizationServerProxmox", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IVirtualizationServerProxmox
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineResponse")]
        VirtualMachine GetVirtualMachine(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineResponse")]
        System.Threading.Tasks.Task<VirtualMachine> GetVirtualMachineAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineEx", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineExResponse")]
        VirtualMachine GetVirtualMachineEx(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineEx", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineExResponse")]
        System.Threading.Tasks.Task<VirtualMachine> GetVirtualMachineExAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachines", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachinesResponse")]
        List<VirtualMachine> GetVirtualMachines();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachines", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachinesResponse")]
        System.Threading.Tasks.Task<List<VirtualMachine>> GetVirtualMachinesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineThumbnailImageResponse")]
        byte[] GetVirtualMachineThumbnailImage(string vmId, ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineThumbnailImageResponse")]
        System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailImageAsync(string vmId, ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/CreateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/CreateVirtualMachineResponse")]
        VirtualMachine CreateVirtualMachine(VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/CreateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/CreateVirtualMachineResponse")]
        System.Threading.Tasks.Task<VirtualMachine> CreateVirtualMachineAsync(VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/UpdateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/UpdateVirtualMachineResponse")]
        VirtualMachine UpdateVirtualMachine(VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/UpdateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/UpdateVirtualMachineResponse")]
        System.Threading.Tasks.Task<VirtualMachine> UpdateVirtualMachineAsync(VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ChangeVirtualMachineState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ChangeVirtualMachineStateResponse")]
        JobResult ChangeVirtualMachineState(string vmId, VirtualMachineRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ChangeVirtualMachineState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ChangeVirtualMachineStateResponse")]
        System.Threading.Tasks.Task<JobResult> ChangeVirtualMachineStateAsync(string vmId, VirtualMachineRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ShutDownVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ShutDownVirtualMachineResponse")]
        ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ShutDownVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ShutDownVirtualMachineResponse")]
        System.Threading.Tasks.Task<ReturnCode> ShutDownVirtualMachineAsync(string vmId, bool force, string reason);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineJobsResponse")]
        List<ConcreteJob> GetVirtualMachineJobs(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineJobsResponse")]
        System.Threading.Tasks.Task<List<ConcreteJob>> GetVirtualMachineJobsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/RenameVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/RenameVirtualMachineResponse")]
        JobResult RenameVirtualMachine(string vmId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/RenameVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/RenameVirtualMachineResponse")]
        System.Threading.Tasks.Task<JobResult> RenameVirtualMachineAsync(string vmId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteVirtualMachineResponse")]
        JobResult DeleteVirtualMachine(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteVirtualMachineResponse")]
        System.Threading.Tasks.Task<JobResult> DeleteVirtualMachineAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ExportVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ExportVirtualMachineResponse")]
        JobResult ExportVirtualMachine(string vmId, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ExportVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ExportVirtualMachineResponse")]
        System.Threading.Tasks.Task<JobResult> ExportVirtualMachineAsync(string vmId, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineVNC", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineVNCResponse")]
        string GetVirtualMachineVNC(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineVNC", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineVNCResponse")]
        System.Threading.Tasks.Task<string> GetVirtualMachineVNCAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineSnapshots", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineSnapshotsResponse")]
        List<VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineSnapshots", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualMachineSnapshotsResponse")]
        System.Threading.Tasks.Task<List<VirtualMachineSnapshot>> GetVirtualMachineSnapshotsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetSnapshotResponse")]
        VirtualMachineSnapshot GetSnapshot(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetSnapshotResponse")]
        System.Threading.Tasks.Task<VirtualMachineSnapshot> GetSnapshotAsync(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/CreateSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/CreateSnapshotResponse")]
        JobResult CreateSnapshot(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/CreateSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/CreateSnapshotResponse")]
        System.Threading.Tasks.Task<JobResult> CreateSnapshotAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/RenameSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/RenameSnapshotResponse")]
        JobResult RenameSnapshot(string vmId, string snapshotId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/RenameSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/RenameSnapshotResponse")]
        System.Threading.Tasks.Task<JobResult> RenameSnapshotAsync(string vmId, string snapshotId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ApplySnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ApplySnapshotResponse")]
        JobResult ApplySnapshot(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ApplySnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ApplySnapshotResponse")]
        System.Threading.Tasks.Task<JobResult> ApplySnapshotAsync(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteSnapshotResponse")]
        JobResult DeleteSnapshot(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteSnapshotResponse")]
        System.Threading.Tasks.Task<JobResult> DeleteSnapshotAsync(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteSnapshotSubtree", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteSnapshotSubtreeResponse")]
        JobResult DeleteSnapshotSubtree(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteSnapshotSubtree", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteSnapshotSubtreeResponse")]
        System.Threading.Tasks.Task<JobResult> DeleteSnapshotSubtreeAsync(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetSnapshotThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetSnapshotThumbnailImageResponse")]
        byte[] GetSnapshotThumbnailImage(string snapshotId, ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetSnapshotThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetSnapshotThumbnailImageResponse")]
        System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailImageAsync(string snapshotId, ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetExternalSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetExternalSwitchesResponse")]
        List<VirtualSwitch> GetExternalSwitches(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetExternalSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetExternalSwitchesResponse")]
        System.Threading.Tasks.Task<List<VirtualSwitch>> GetExternalSwitchesAsync(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetSwitchesResponse")]
        List<VirtualSwitch> GetSwitches();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetSwitchesResponse")]
        System.Threading.Tasks.Task<List<VirtualSwitch>> GetSwitchesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/SwitchExists", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/SwitchExistsResponse")]
        bool SwitchExists(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/SwitchExists", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/SwitchExistsResponse")]
        System.Threading.Tasks.Task<bool> SwitchExistsAsync(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/CreateSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/CreateSwitchResponse")]
        VirtualSwitch CreateSwitch(string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/CreateSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/CreateSwitchResponse")]
        System.Threading.Tasks.Task<VirtualSwitch> CreateSwitchAsync(string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteSwitchResponse")]
        ReturnCode DeleteSwitch(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DeleteSwitchResponse")]
        System.Threading.Tasks.Task<ReturnCode> DeleteSwitchAsync(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetDVDISOs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetDVDISOsResponse")]
        LibraryItem[] GetDVDISOs(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetDVDISOs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetDVDISOsResponse")]
        System.Threading.Tasks.Task<LibraryItem[]> GetDVDISOsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetInsertedDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetInsertedDVDResponse")]
        string GetInsertedDVD(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetInsertedDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetInsertedDVDResponse")]
        System.Threading.Tasks.Task<string> GetInsertedDVDAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/InsertDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/InsertDVDResponse")]
        JobResult InsertDVD(string vmId, string isoPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/InsertDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/InsertDVDResponse")]
        System.Threading.Tasks.Task<JobResult> InsertDVDAsync(string vmId, string isoPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/EjectDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/EjectDVDResponse")]
        JobResult EjectDVD(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/EjectDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/EjectDVDResponse")]
        System.Threading.Tasks.Task<JobResult> EjectDVDAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetKVPItemsResponse")]
        List<KvpExchangeDataItem> GetKVPItems(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetKVPItemsResponse")]
        System.Threading.Tasks.Task<List<KvpExchangeDataItem>> GetKVPItemsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetStandardKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetStandardKVPItemsResponse")]
        List<KvpExchangeDataItem> GetStandardKVPItems(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetStandardKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetStandardKVPItemsResponse")]
        System.Threading.Tasks.Task<List<KvpExchangeDataItem>> GetStandardKVPItemsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/AddKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/AddKVPItemsResponse")]
        JobResult AddKVPItems(string vmId, KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/AddKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/AddKVPItemsResponse")]
        System.Threading.Tasks.Task<JobResult> AddKVPItemsAsync(string vmId, KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/RemoveKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/RemoveKVPItemsResponse")]
        JobResult RemoveKVPItems(string vmId, string[] itemNames);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/RemoveKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/RemoveKVPItemsResponse")]
        System.Threading.Tasks.Task<JobResult> RemoveKVPItemsAsync(string vmId, string[] itemNames);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ModifyKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ModifyKVPItemsResponse")]
        JobResult ModifyKVPItems(string vmId, KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ModifyKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ModifyKVPItemsResponse")]
        System.Threading.Tasks.Task<JobResult> ModifyKVPItemsAsync(string vmId, KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualHardDiskInfo", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualHardDiskInfoResponse")]
        VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualHardDiskInfo", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetVirtualHardDiskInfoResponse")]
        System.Threading.Tasks.Task<VirtualHardDiskInfo> GetVirtualHardDiskInfoAsync(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/MountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/MountVirtualHardDiskResponse")]
        MountedDiskInfo MountVirtualHardDisk(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/MountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/MountVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<MountedDiskInfo> MountVirtualHardDiskAsync(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/UnmountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/UnmountVirtualHardDiskResponse")]
        ReturnCode UnmountVirtualHardDisk(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/UnmountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/UnmountVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<ReturnCode> UnmountVirtualHardDiskAsync(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ExpandVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ExpandVirtualHardDiskResponse")]
        JobResult ExpandVirtualHardDisk(string vhdPath, UInt64 sizeGB);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ExpandVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ExpandVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<JobResult> ExpandVirtualHardDiskAsync(string vhdPath, UInt64 sizeGB);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ConvertVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ConvertVirtualHardDiskResponse")]
        JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, VirtualHardDiskType diskType);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ConvertVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ConvertVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<JobResult> ConvertVirtualHardDiskAsync(string sourcePath, string destinationPath, VirtualHardDiskType diskType);
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
        ConcreteJob GetJob(string jobId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetJob", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetJobResponse")]
        System.Threading.Tasks.Task<ConcreteJob> GetJobAsync(string jobId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetAllJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetAllJobsResponse")]
        List<ConcreteJob> GetAllJobs();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetAllJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetAllJobsResponse")]
        System.Threading.Tasks.Task<List<ConcreteJob>> GetAllJobsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ChangeJobState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ChangeJobStateResponse")]
        ChangeJobStateReturnCode ChangeJobState(string jobId, ConcreteJobRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ChangeJobState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/ChangeJobStateResponse")]
        System.Threading.Tasks.Task<ChangeJobStateReturnCode> ChangeJobStateAsync(string jobId, ConcreteJobRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetProcessorCoresNumber", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetProcessorCoresNumberResponse")]
        int GetProcessorCoresNumber();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetProcessorCoresNumber", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetProcessorCoresNumberResponse")]
        System.Threading.Tasks.Task<int> GetProcessorCoresNumberAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetCertificates", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetCertificatesResponse")]
        List<CertificateInfo> GetCertificates(string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetCertificates", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetCertificatesResponse")]
        System.Threading.Tasks.Task<List<CertificateInfo>> GetCertificatesAsync(string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/SetReplicaServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/SetReplicaServerResponse")]
        void SetReplicaServer(string remoteServer, string thumbprint, string storagePath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/SetReplicaServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/SetReplicaServerResponse")]
        System.Threading.Tasks.Task SetReplicaServerAsync(string remoteServer, string thumbprint, string storagePath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/UnsetReplicaServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/UnsetReplicaServerResponse")]
        void UnsetReplicaServer(string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/UnsetReplicaServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/UnsetReplicaServerResponse")]
        System.Threading.Tasks.Task UnsetReplicaServerAsync(string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetReplicaServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetReplicaServerResponse")]
        ReplicationServerInfo GetReplicaServer(string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetReplicaServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetReplicaServerResponse")]
        System.Threading.Tasks.Task<ReplicationServerInfo> GetReplicaServerAsync(string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/EnableVmReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/EnableVmReplicationResponse")]
        void EnableVmReplication(string vmId, string replicaServer, VmReplication replication);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/EnableVmReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/EnableVmReplicationResponse")]
        System.Threading.Tasks.Task EnableVmReplicationAsync(string vmId, string replicaServer, VmReplication replication);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/SetVmReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/SetVmReplicationResponse")]
        void SetVmReplication(string vmId, string replicaServer, VmReplication replication);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/SetVmReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/SetVmReplicationResponse")]
        System.Threading.Tasks.Task SetVmReplicationAsync(string vmId, string replicaServer, VmReplication replication);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/TestReplicationServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/TestReplicationServerResponse")]
        void TestReplicationServer(string vmId, string replicaServer, string localThumbprint);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/TestReplicationServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/TestReplicationServerResponse")]
        System.Threading.Tasks.Task TestReplicationServerAsync(string vmId, string replicaServer, string localThumbprint);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/StartInitialReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/StartInitialReplicationResponse")]
        void StartInitialReplication(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/StartInitialReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/StartInitialReplicationResponse")]
        System.Threading.Tasks.Task StartInitialReplicationAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetReplicationResponse")]
        VmReplication GetReplication(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetReplicationResponse")]
        System.Threading.Tasks.Task<VmReplication> GetReplicationAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DisableVmReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DisableVmReplicationResponse")]
        void DisableVmReplication(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DisableVmReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/DisableVmReplicationResponse")]
        System.Threading.Tasks.Task DisableVmReplicationAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetReplicationInfo", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetReplicationInfoResponse")]
        ReplicationDetailInfo GetReplicationInfo(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetReplicationInfo", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServerProxmox/GetReplicationInfoResponse")]
        System.Threading.Tasks.Task<ReplicationDetailInfo> GetReplicationInfoAsync(string vmId);
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
        public VirtualMachine GetVirtualMachine(string vmId)
        {
            return (VirtualMachine)Invoke("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachine", vmId);
        }

        public async System.Threading.Tasks.Task<VirtualMachine> GetVirtualMachineAsync(string vmId)
        {
            return await InvokeAsync<VirtualMachine>("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachine", vmId);
        }

        public VirtualMachine GetVirtualMachineEx(string vmId)
        {
            return (VirtualMachine)Invoke("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachineEx", vmId);
        }

        public async System.Threading.Tasks.Task<VirtualMachine> GetVirtualMachineExAsync(string vmId)
        {
            return await InvokeAsync<VirtualMachine>("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachineEx", vmId);
        }

        public List<VirtualMachine> GetVirtualMachines()
        {
            return (List<VirtualMachine>)Invoke("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachines");
        }

        public async System.Threading.Tasks.Task<List<VirtualMachine>> GetVirtualMachinesAsync()
        {
            return await InvokeAsync<List<VirtualMachine>>("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachines");
        }

        public byte[] GetVirtualMachineThumbnailImage(string vmId, ThumbnailSize size)
        {
            return (byte[])Invoke("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachineThumbnailImage", vmId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailImageAsync(string vmId, ThumbnailSize size)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachineThumbnailImage", vmId, size);
        }

        public VirtualMachine CreateVirtualMachine(VirtualMachine vm)
        {
            return (VirtualMachine)Invoke("SolidCP.Server.VirtualizationServerProxmox", "CreateVirtualMachine", vm);
        }

        public async System.Threading.Tasks.Task<VirtualMachine> CreateVirtualMachineAsync(VirtualMachine vm)
        {
            return await InvokeAsync<VirtualMachine>("SolidCP.Server.VirtualizationServerProxmox", "CreateVirtualMachine", vm);
        }

        public VirtualMachine UpdateVirtualMachine(VirtualMachine vm)
        {
            return (VirtualMachine)Invoke("SolidCP.Server.VirtualizationServerProxmox", "UpdateVirtualMachine", vm);
        }

        public async System.Threading.Tasks.Task<VirtualMachine> UpdateVirtualMachineAsync(VirtualMachine vm)
        {
            return await InvokeAsync<VirtualMachine>("SolidCP.Server.VirtualizationServerProxmox", "UpdateVirtualMachine", vm);
        }

        public JobResult ChangeVirtualMachineState(string vmId, VirtualMachineRequestedState newState)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerProxmox", "ChangeVirtualMachineState", vmId, newState);
        }

        public async System.Threading.Tasks.Task<JobResult> ChangeVirtualMachineStateAsync(string vmId, VirtualMachineRequestedState newState)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerProxmox", "ChangeVirtualMachineState", vmId, newState);
        }

        public ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason)
        {
            return (ReturnCode)Invoke("SolidCP.Server.VirtualizationServerProxmox", "ShutDownVirtualMachine", vmId, force, reason);
        }

        public async System.Threading.Tasks.Task<ReturnCode> ShutDownVirtualMachineAsync(string vmId, bool force, string reason)
        {
            return await InvokeAsync<ReturnCode>("SolidCP.Server.VirtualizationServerProxmox", "ShutDownVirtualMachine", vmId, force, reason);
        }

        public List<ConcreteJob> GetVirtualMachineJobs(string vmId)
        {
            return (List<ConcreteJob>)Invoke("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachineJobs", vmId);
        }

        public async System.Threading.Tasks.Task<List<ConcreteJob>> GetVirtualMachineJobsAsync(string vmId)
        {
            return await InvokeAsync<List<ConcreteJob>>("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachineJobs", vmId);
        }

        public JobResult RenameVirtualMachine(string vmId, string name)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerProxmox", "RenameVirtualMachine", vmId, name);
        }

        public async System.Threading.Tasks.Task<JobResult> RenameVirtualMachineAsync(string vmId, string name)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerProxmox", "RenameVirtualMachine", vmId, name);
        }

        public JobResult DeleteVirtualMachine(string vmId)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerProxmox", "DeleteVirtualMachine", vmId);
        }

        public async System.Threading.Tasks.Task<JobResult> DeleteVirtualMachineAsync(string vmId)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerProxmox", "DeleteVirtualMachine", vmId);
        }

        public JobResult ExportVirtualMachine(string vmId, string exportPath)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerProxmox", "ExportVirtualMachine", vmId, exportPath);
        }

        public async System.Threading.Tasks.Task<JobResult> ExportVirtualMachineAsync(string vmId, string exportPath)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerProxmox", "ExportVirtualMachine", vmId, exportPath);
        }

        public string GetVirtualMachineVNC(string vmId)
        {
            return (string)Invoke("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachineVNC", vmId);
        }

        public async System.Threading.Tasks.Task<string> GetVirtualMachineVNCAsync(string vmId)
        {
            return await InvokeAsync<string>("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachineVNC", vmId);
        }

        public List<VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId)
        {
            return (List<VirtualMachineSnapshot>)Invoke("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachineSnapshots", vmId);
        }

        public async System.Threading.Tasks.Task<List<VirtualMachineSnapshot>> GetVirtualMachineSnapshotsAsync(string vmId)
        {
            return await InvokeAsync<List<VirtualMachineSnapshot>>("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualMachineSnapshots", vmId);
        }

        public VirtualMachineSnapshot GetSnapshot(string vmId, string snapshotId)
        {
            return (VirtualMachineSnapshot)Invoke("SolidCP.Server.VirtualizationServerProxmox", "GetSnapshot", vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<VirtualMachineSnapshot> GetSnapshotAsync(string vmId, string snapshotId)
        {
            return await InvokeAsync<VirtualMachineSnapshot>("SolidCP.Server.VirtualizationServerProxmox", "GetSnapshot", vmId, snapshotId);
        }

        public JobResult CreateSnapshot(string vmId)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerProxmox", "CreateSnapshot", vmId);
        }

        public async System.Threading.Tasks.Task<JobResult> CreateSnapshotAsync(string vmId)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerProxmox", "CreateSnapshot", vmId);
        }

        public JobResult RenameSnapshot(string vmId, string snapshotId, string name)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerProxmox", "RenameSnapshot", vmId, snapshotId, name);
        }

        public async System.Threading.Tasks.Task<JobResult> RenameSnapshotAsync(string vmId, string snapshotId, string name)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerProxmox", "RenameSnapshot", vmId, snapshotId, name);
        }

        public JobResult ApplySnapshot(string vmId, string snapshotId)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerProxmox", "ApplySnapshot", vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<JobResult> ApplySnapshotAsync(string vmId, string snapshotId)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerProxmox", "ApplySnapshot", vmId, snapshotId);
        }

        public JobResult DeleteSnapshot(string vmId, string snapshotId)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerProxmox", "DeleteSnapshot", vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<JobResult> DeleteSnapshotAsync(string vmId, string snapshotId)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerProxmox", "DeleteSnapshot", vmId, snapshotId);
        }

        public JobResult DeleteSnapshotSubtree(string vmId, string snapshotId)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerProxmox", "DeleteSnapshotSubtree", vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<JobResult> DeleteSnapshotSubtreeAsync(string vmId, string snapshotId)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerProxmox", "DeleteSnapshotSubtree", vmId, snapshotId);
        }

        public byte[] GetSnapshotThumbnailImage(string snapshotId, ThumbnailSize size)
        {
            return (byte[])Invoke("SolidCP.Server.VirtualizationServerProxmox", "GetSnapshotThumbnailImage", snapshotId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailImageAsync(string snapshotId, ThumbnailSize size)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.VirtualizationServerProxmox", "GetSnapshotThumbnailImage", snapshotId, size);
        }

        public List<VirtualSwitch> GetExternalSwitches(string computerName)
        {
            return (List<VirtualSwitch>)Invoke("SolidCP.Server.VirtualizationServerProxmox", "GetExternalSwitches", computerName);
        }

        public async System.Threading.Tasks.Task<List<VirtualSwitch>> GetExternalSwitchesAsync(string computerName)
        {
            return await InvokeAsync<List<VirtualSwitch>>("SolidCP.Server.VirtualizationServerProxmox", "GetExternalSwitches", computerName);
        }

        public List<VirtualSwitch> GetSwitches()
        {
            return (List<VirtualSwitch>)Invoke("SolidCP.Server.VirtualizationServerProxmox", "GetSwitches");
        }

        public async System.Threading.Tasks.Task<List<VirtualSwitch>> GetSwitchesAsync()
        {
            return await InvokeAsync<List<VirtualSwitch>>("SolidCP.Server.VirtualizationServerProxmox", "GetSwitches");
        }

        public bool SwitchExists(string switchId)
        {
            return (bool)Invoke("SolidCP.Server.VirtualizationServerProxmox", "SwitchExists", switchId);
        }

        public async System.Threading.Tasks.Task<bool> SwitchExistsAsync(string switchId)
        {
            return await InvokeAsync<bool>("SolidCP.Server.VirtualizationServerProxmox", "SwitchExists", switchId);
        }

        public VirtualSwitch CreateSwitch(string name)
        {
            return (VirtualSwitch)Invoke("SolidCP.Server.VirtualizationServerProxmox", "CreateSwitch", name);
        }

        public async System.Threading.Tasks.Task<VirtualSwitch> CreateSwitchAsync(string name)
        {
            return await InvokeAsync<VirtualSwitch>("SolidCP.Server.VirtualizationServerProxmox", "CreateSwitch", name);
        }

        public ReturnCode DeleteSwitch(string switchId)
        {
            return (ReturnCode)Invoke("SolidCP.Server.VirtualizationServerProxmox", "DeleteSwitch", switchId);
        }

        public async System.Threading.Tasks.Task<ReturnCode> DeleteSwitchAsync(string switchId)
        {
            return await InvokeAsync<ReturnCode>("SolidCP.Server.VirtualizationServerProxmox", "DeleteSwitch", switchId);
        }

        public LibraryItem[] GetDVDISOs(string vmId)
        {
            return (LibraryItem[])Invoke("SolidCP.Server.VirtualizationServerProxmox", "GetDVDISOs", vmId);
        }

        public async System.Threading.Tasks.Task<LibraryItem[]> GetDVDISOsAsync(string vmId)
        {
            return await InvokeAsync<LibraryItem[]>("SolidCP.Server.VirtualizationServerProxmox", "GetDVDISOs", vmId);
        }

        public string GetInsertedDVD(string vmId)
        {
            return (string)Invoke("SolidCP.Server.VirtualizationServerProxmox", "GetInsertedDVD", vmId);
        }

        public async System.Threading.Tasks.Task<string> GetInsertedDVDAsync(string vmId)
        {
            return await InvokeAsync<string>("SolidCP.Server.VirtualizationServerProxmox", "GetInsertedDVD", vmId);
        }

        public JobResult InsertDVD(string vmId, string isoPath)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerProxmox", "InsertDVD", vmId, isoPath);
        }

        public async System.Threading.Tasks.Task<JobResult> InsertDVDAsync(string vmId, string isoPath)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerProxmox", "InsertDVD", vmId, isoPath);
        }

        public JobResult EjectDVD(string vmId)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerProxmox", "EjectDVD", vmId);
        }

        public async System.Threading.Tasks.Task<JobResult> EjectDVDAsync(string vmId)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerProxmox", "EjectDVD", vmId);
        }

        public List<KvpExchangeDataItem> GetKVPItems(string vmId)
        {
            return (List<KvpExchangeDataItem>)Invoke("SolidCP.Server.VirtualizationServerProxmox", "GetKVPItems", vmId);
        }

        public async System.Threading.Tasks.Task<List<KvpExchangeDataItem>> GetKVPItemsAsync(string vmId)
        {
            return await InvokeAsync<List<KvpExchangeDataItem>>("SolidCP.Server.VirtualizationServerProxmox", "GetKVPItems", vmId);
        }

        public List<KvpExchangeDataItem> GetStandardKVPItems(string vmId)
        {
            return (List<KvpExchangeDataItem>)Invoke("SolidCP.Server.VirtualizationServerProxmox", "GetStandardKVPItems", vmId);
        }

        public async System.Threading.Tasks.Task<List<KvpExchangeDataItem>> GetStandardKVPItemsAsync(string vmId)
        {
            return await InvokeAsync<List<KvpExchangeDataItem>>("SolidCP.Server.VirtualizationServerProxmox", "GetStandardKVPItems", vmId);
        }

        public JobResult AddKVPItems(string vmId, KvpExchangeDataItem[] items)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerProxmox", "AddKVPItems", vmId, items);
        }

        public async System.Threading.Tasks.Task<JobResult> AddKVPItemsAsync(string vmId, KvpExchangeDataItem[] items)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerProxmox", "AddKVPItems", vmId, items);
        }

        public JobResult RemoveKVPItems(string vmId, string[] itemNames)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerProxmox", "RemoveKVPItems", vmId, itemNames);
        }

        public async System.Threading.Tasks.Task<JobResult> RemoveKVPItemsAsync(string vmId, string[] itemNames)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerProxmox", "RemoveKVPItems", vmId, itemNames);
        }

        public JobResult ModifyKVPItems(string vmId, KvpExchangeDataItem[] items)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerProxmox", "ModifyKVPItems", vmId, items);
        }

        public async System.Threading.Tasks.Task<JobResult> ModifyKVPItemsAsync(string vmId, KvpExchangeDataItem[] items)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerProxmox", "ModifyKVPItems", vmId, items);
        }

        public VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath)
        {
            return (VirtualHardDiskInfo)Invoke("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualHardDiskInfo", vhdPath);
        }

        public async System.Threading.Tasks.Task<VirtualHardDiskInfo> GetVirtualHardDiskInfoAsync(string vhdPath)
        {
            return await InvokeAsync<VirtualHardDiskInfo>("SolidCP.Server.VirtualizationServerProxmox", "GetVirtualHardDiskInfo", vhdPath);
        }

        public MountedDiskInfo MountVirtualHardDisk(string vhdPath)
        {
            return (MountedDiskInfo)Invoke("SolidCP.Server.VirtualizationServerProxmox", "MountVirtualHardDisk", vhdPath);
        }

        public async System.Threading.Tasks.Task<MountedDiskInfo> MountVirtualHardDiskAsync(string vhdPath)
        {
            return await InvokeAsync<MountedDiskInfo>("SolidCP.Server.VirtualizationServerProxmox", "MountVirtualHardDisk", vhdPath);
        }

        public ReturnCode UnmountVirtualHardDisk(string vhdPath)
        {
            return (ReturnCode)Invoke("SolidCP.Server.VirtualizationServerProxmox", "UnmountVirtualHardDisk", vhdPath);
        }

        public async System.Threading.Tasks.Task<ReturnCode> UnmountVirtualHardDiskAsync(string vhdPath)
        {
            return await InvokeAsync<ReturnCode>("SolidCP.Server.VirtualizationServerProxmox", "UnmountVirtualHardDisk", vhdPath);
        }

        public JobResult ExpandVirtualHardDisk(string vhdPath, UInt64 sizeGB)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerProxmox", "ExpandVirtualHardDisk", vhdPath, sizeGB);
        }

        public async System.Threading.Tasks.Task<JobResult> ExpandVirtualHardDiskAsync(string vhdPath, UInt64 sizeGB)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerProxmox", "ExpandVirtualHardDisk", vhdPath, sizeGB);
        }

        public JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, VirtualHardDiskType diskType)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServerProxmox", "ConvertVirtualHardDisk", sourcePath, destinationPath, diskType);
        }

        public async System.Threading.Tasks.Task<JobResult> ConvertVirtualHardDiskAsync(string sourcePath, string destinationPath, VirtualHardDiskType diskType)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServerProxmox", "ConvertVirtualHardDisk", sourcePath, destinationPath, diskType);
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
            return (string)Invoke("SolidCP.Server.VirtualizationServerProxmox", "ReadRemoteFile", path);
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

        public ConcreteJob GetJob(string jobId)
        {
            return (ConcreteJob)Invoke("SolidCP.Server.VirtualizationServerProxmox", "GetJob", jobId);
        }

        public async System.Threading.Tasks.Task<ConcreteJob> GetJobAsync(string jobId)
        {
            return await InvokeAsync<ConcreteJob>("SolidCP.Server.VirtualizationServerProxmox", "GetJob", jobId);
        }

        public List<ConcreteJob> GetAllJobs()
        {
            return (List<ConcreteJob>)Invoke("SolidCP.Server.VirtualizationServerProxmox", "GetAllJobs");
        }

        public async System.Threading.Tasks.Task<List<ConcreteJob>> GetAllJobsAsync()
        {
            return await InvokeAsync<List<ConcreteJob>>("SolidCP.Server.VirtualizationServerProxmox", "GetAllJobs");
        }

        public ChangeJobStateReturnCode ChangeJobState(string jobId, ConcreteJobRequestedState newState)
        {
            return (ChangeJobStateReturnCode)Invoke("SolidCP.Server.VirtualizationServerProxmox", "ChangeJobState", jobId, newState);
        }

        public async System.Threading.Tasks.Task<ChangeJobStateReturnCode> ChangeJobStateAsync(string jobId, ConcreteJobRequestedState newState)
        {
            return await InvokeAsync<ChangeJobStateReturnCode>("SolidCP.Server.VirtualizationServerProxmox", "ChangeJobState", jobId, newState);
        }

        public int GetProcessorCoresNumber()
        {
            return (int)Invoke("SolidCP.Server.VirtualizationServerProxmox", "GetProcessorCoresNumber");
        }

        public async System.Threading.Tasks.Task<int> GetProcessorCoresNumberAsync()
        {
            return await InvokeAsync<int>("SolidCP.Server.VirtualizationServerProxmox", "GetProcessorCoresNumber");
        }

        public List<CertificateInfo> GetCertificates(string remoteServer)
        {
            return (List<CertificateInfo>)Invoke("SolidCP.Server.VirtualizationServerProxmox", "GetCertificates", remoteServer);
        }

        public async System.Threading.Tasks.Task<List<CertificateInfo>> GetCertificatesAsync(string remoteServer)
        {
            return await InvokeAsync<List<CertificateInfo>>("SolidCP.Server.VirtualizationServerProxmox", "GetCertificates", remoteServer);
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

        public ReplicationServerInfo GetReplicaServer(string remoteServer)
        {
            return (ReplicationServerInfo)Invoke("SolidCP.Server.VirtualizationServerProxmox", "GetReplicaServer", remoteServer);
        }

        public async System.Threading.Tasks.Task<ReplicationServerInfo> GetReplicaServerAsync(string remoteServer)
        {
            return await InvokeAsync<ReplicationServerInfo>("SolidCP.Server.VirtualizationServerProxmox", "GetReplicaServer", remoteServer);
        }

        public void EnableVmReplication(string vmId, string replicaServer, VmReplication replication)
        {
            Invoke("SolidCP.Server.VirtualizationServerProxmox", "EnableVmReplication", vmId, replicaServer, replication);
        }

        public async System.Threading.Tasks.Task EnableVmReplicationAsync(string vmId, string replicaServer, VmReplication replication)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServerProxmox", "EnableVmReplication", vmId, replicaServer, replication);
        }

        public void SetVmReplication(string vmId, string replicaServer, VmReplication replication)
        {
            Invoke("SolidCP.Server.VirtualizationServerProxmox", "SetVmReplication", vmId, replicaServer, replication);
        }

        public async System.Threading.Tasks.Task SetVmReplicationAsync(string vmId, string replicaServer, VmReplication replication)
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

        public VmReplication GetReplication(string vmId)
        {
            return (VmReplication)Invoke("SolidCP.Server.VirtualizationServerProxmox", "GetReplication", vmId);
        }

        public async System.Threading.Tasks.Task<VmReplication> GetReplicationAsync(string vmId)
        {
            return await InvokeAsync<VmReplication>("SolidCP.Server.VirtualizationServerProxmox", "GetReplication", vmId);
        }

        public void DisableVmReplication(string vmId)
        {
            Invoke("SolidCP.Server.VirtualizationServerProxmox", "DisableVmReplication", vmId);
        }

        public async System.Threading.Tasks.Task DisableVmReplicationAsync(string vmId)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServerProxmox", "DisableVmReplication", vmId);
        }

        public ReplicationDetailInfo GetReplicationInfo(string vmId)
        {
            return (ReplicationDetailInfo)Invoke("SolidCP.Server.VirtualizationServerProxmox", "GetReplicationInfo", vmId);
        }

        public async System.Threading.Tasks.Task<ReplicationDetailInfo> GetReplicationInfoAsync(string vmId)
        {
            return await InvokeAsync<ReplicationDetailInfo>("SolidCP.Server.VirtualizationServerProxmox", "GetReplicationInfo", vmId);
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
        public VirtualMachine GetVirtualMachine(string vmId)
        {
            return base.Client.GetVirtualMachine(vmId);
        }

        public async System.Threading.Tasks.Task<VirtualMachine> GetVirtualMachineAsync(string vmId)
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

        public VirtualMachine CreateVirtualMachine(VirtualMachine vm)
        {
            return base.Client.CreateVirtualMachine(vm);
        }

        public async System.Threading.Tasks.Task<VirtualMachine> CreateVirtualMachineAsync(VirtualMachine vm)
        {
            return await base.Client.CreateVirtualMachineAsync(vm);
        }

        public VirtualMachine UpdateVirtualMachine(VirtualMachine vm)
        {
            return base.Client.UpdateVirtualMachine(vm);
        }

        public async System.Threading.Tasks.Task<VirtualMachine> UpdateVirtualMachineAsync(VirtualMachine vm)
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

        public JobResult ExportVirtualMachine(string vmId, string exportPath)
        {
            return base.Client.ExportVirtualMachine(vmId, exportPath);
        }

        public async System.Threading.Tasks.Task<JobResult> ExportVirtualMachineAsync(string vmId, string exportPath)
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

        public List<VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId)
        {
            return base.Client.GetVirtualMachineSnapshots(vmId);
        }

        public async System.Threading.Tasks.Task<List<VirtualMachineSnapshot>> GetVirtualMachineSnapshotsAsync(string vmId)
        {
            return await base.Client.GetVirtualMachineSnapshotsAsync(vmId);
        }

        public VirtualMachineSnapshot GetSnapshot(string vmId, string snapshotId)
        {
            return base.Client.GetSnapshot(vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<VirtualMachineSnapshot> GetSnapshotAsync(string vmId, string snapshotId)
        {
            return await base.Client.GetSnapshotAsync(vmId, snapshotId);
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

        public JobResult DeleteSnapshotSubtree(string vmId, string snapshotId)
        {
            return base.Client.DeleteSnapshotSubtree(vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<JobResult> DeleteSnapshotSubtreeAsync(string vmId, string snapshotId)
        {
            return await base.Client.DeleteSnapshotSubtreeAsync(vmId, snapshotId);
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

        public LibraryItem[] GetDVDISOs(string vmId)
        {
            return base.Client.GetDVDISOs(vmId);
        }

        public async System.Threading.Tasks.Task<LibraryItem[]> GetDVDISOsAsync(string vmId)
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

        public int GetProcessorCoresNumber()
        {
            return base.Client.GetProcessorCoresNumber();
        }

        public async System.Threading.Tasks.Task<int> GetProcessorCoresNumberAsync()
        {
            return await base.Client.GetProcessorCoresNumberAsync();
        }

        public List<CertificateInfo> GetCertificates(string remoteServer)
        {
            return base.Client.GetCertificates(remoteServer);
        }

        public async System.Threading.Tasks.Task<List<CertificateInfo>> GetCertificatesAsync(string remoteServer)
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

        public ReplicationServerInfo GetReplicaServer(string remoteServer)
        {
            return base.Client.GetReplicaServer(remoteServer);
        }

        public async System.Threading.Tasks.Task<ReplicationServerInfo> GetReplicaServerAsync(string remoteServer)
        {
            return await base.Client.GetReplicaServerAsync(remoteServer);
        }

        public void EnableVmReplication(string vmId, string replicaServer, VmReplication replication)
        {
            base.Client.EnableVmReplication(vmId, replicaServer, replication);
        }

        public async System.Threading.Tasks.Task EnableVmReplicationAsync(string vmId, string replicaServer, VmReplication replication)
        {
            await base.Client.EnableVmReplicationAsync(vmId, replicaServer, replication);
        }

        public void SetVmReplication(string vmId, string replicaServer, VmReplication replication)
        {
            base.Client.SetVmReplication(vmId, replicaServer, replication);
        }

        public async System.Threading.Tasks.Task SetVmReplicationAsync(string vmId, string replicaServer, VmReplication replication)
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

        public VmReplication GetReplication(string vmId)
        {
            return base.Client.GetReplication(vmId);
        }

        public async System.Threading.Tasks.Task<VmReplication> GetReplicationAsync(string vmId)
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

        public ReplicationDetailInfo GetReplicationInfo(string vmId)
        {
            return base.Client.GetReplicationInfo(vmId);
        }

        public async System.Threading.Tasks.Task<ReplicationDetailInfo> GetReplicationInfoAsync(string vmId)
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