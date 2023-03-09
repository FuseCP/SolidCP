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
    [ServiceContract(ConfigurationName = "IVirtualizationServer", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IVirtualizationServer
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineResponse")]
        VirtualMachine GetVirtualMachine(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineResponse")]
        System.Threading.Tasks.Task<VirtualMachine> GetVirtualMachineAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineEx", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineExResponse")]
        VirtualMachine GetVirtualMachineEx(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineEx", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineExResponse")]
        System.Threading.Tasks.Task<VirtualMachine> GetVirtualMachineExAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachines", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachinesResponse")]
        List<VirtualMachine> GetVirtualMachines();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachines", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachinesResponse")]
        System.Threading.Tasks.Task<List<VirtualMachine>> GetVirtualMachinesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineThumbnailImageResponse")]
        byte[] GetVirtualMachineThumbnailImage(string vmId, ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineThumbnailImageResponse")]
        System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailImageAsync(string vmId, ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/CreateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/CreateVirtualMachineResponse")]
        VirtualMachine CreateVirtualMachine(VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/CreateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/CreateVirtualMachineResponse")]
        System.Threading.Tasks.Task<VirtualMachine> CreateVirtualMachineAsync(VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/UpdateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/UpdateVirtualMachineResponse")]
        VirtualMachine UpdateVirtualMachine(VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/UpdateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/UpdateVirtualMachineResponse")]
        System.Threading.Tasks.Task<VirtualMachine> UpdateVirtualMachineAsync(VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ChangeVirtualMachineState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ChangeVirtualMachineStateResponse")]
        JobResult ChangeVirtualMachineState(string vmId, VirtualMachineRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ChangeVirtualMachineState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ChangeVirtualMachineStateResponse")]
        System.Threading.Tasks.Task<JobResult> ChangeVirtualMachineStateAsync(string vmId, VirtualMachineRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ShutDownVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ShutDownVirtualMachineResponse")]
        ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ShutDownVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ShutDownVirtualMachineResponse")]
        System.Threading.Tasks.Task<ReturnCode> ShutDownVirtualMachineAsync(string vmId, bool force, string reason);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineJobsResponse")]
        List<ConcreteJob> GetVirtualMachineJobs(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineJobsResponse")]
        System.Threading.Tasks.Task<List<ConcreteJob>> GetVirtualMachineJobsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/RenameVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/RenameVirtualMachineResponse")]
        JobResult RenameVirtualMachine(string vmId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/RenameVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/RenameVirtualMachineResponse")]
        System.Threading.Tasks.Task<JobResult> RenameVirtualMachineAsync(string vmId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteVirtualMachineResponse")]
        JobResult DeleteVirtualMachine(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteVirtualMachineResponse")]
        System.Threading.Tasks.Task<JobResult> DeleteVirtualMachineAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ExportVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ExportVirtualMachineResponse")]
        JobResult ExportVirtualMachine(string vmId, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ExportVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ExportVirtualMachineResponse")]
        System.Threading.Tasks.Task<JobResult> ExportVirtualMachineAsync(string vmId, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineSnapshots", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineSnapshotsResponse")]
        List<VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineSnapshots", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualMachineSnapshotsResponse")]
        System.Threading.Tasks.Task<List<VirtualMachineSnapshot>> GetVirtualMachineSnapshotsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetSnapshotResponse")]
        VirtualMachineSnapshot GetSnapshot(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetSnapshotResponse")]
        System.Threading.Tasks.Task<VirtualMachineSnapshot> GetSnapshotAsync(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/CreateSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/CreateSnapshotResponse")]
        JobResult CreateSnapshot(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/CreateSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/CreateSnapshotResponse")]
        System.Threading.Tasks.Task<JobResult> CreateSnapshotAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/RenameSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/RenameSnapshotResponse")]
        JobResult RenameSnapshot(string vmId, string snapshotId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/RenameSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/RenameSnapshotResponse")]
        System.Threading.Tasks.Task<JobResult> RenameSnapshotAsync(string vmId, string snapshotId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ApplySnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ApplySnapshotResponse")]
        JobResult ApplySnapshot(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ApplySnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ApplySnapshotResponse")]
        System.Threading.Tasks.Task<JobResult> ApplySnapshotAsync(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteSnapshotResponse")]
        JobResult DeleteSnapshot(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteSnapshotResponse")]
        System.Threading.Tasks.Task<JobResult> DeleteSnapshotAsync(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteSnapshotSubtree", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteSnapshotSubtreeResponse")]
        JobResult DeleteSnapshotSubtree(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteSnapshotSubtree", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteSnapshotSubtreeResponse")]
        System.Threading.Tasks.Task<JobResult> DeleteSnapshotSubtreeAsync(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetSnapshotThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetSnapshotThumbnailImageResponse")]
        byte[] GetSnapshotThumbnailImage(string snapshotId, ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetSnapshotThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetSnapshotThumbnailImageResponse")]
        System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailImageAsync(string snapshotId, ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetExternalSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetExternalSwitchesResponse")]
        List<VirtualSwitch> GetExternalSwitches(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetExternalSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetExternalSwitchesResponse")]
        System.Threading.Tasks.Task<List<VirtualSwitch>> GetExternalSwitchesAsync(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetSwitchesResponse")]
        List<VirtualSwitch> GetSwitches();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetSwitchesResponse")]
        System.Threading.Tasks.Task<List<VirtualSwitch>> GetSwitchesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/SwitchExists", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/SwitchExistsResponse")]
        bool SwitchExists(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/SwitchExists", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/SwitchExistsResponse")]
        System.Threading.Tasks.Task<bool> SwitchExistsAsync(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/CreateSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/CreateSwitchResponse")]
        VirtualSwitch CreateSwitch(string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/CreateSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/CreateSwitchResponse")]
        System.Threading.Tasks.Task<VirtualSwitch> CreateSwitchAsync(string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteSwitchResponse")]
        ReturnCode DeleteSwitch(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/DeleteSwitchResponse")]
        System.Threading.Tasks.Task<ReturnCode> DeleteSwitchAsync(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetInsertedDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetInsertedDVDResponse")]
        string GetInsertedDVD(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetInsertedDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetInsertedDVDResponse")]
        System.Threading.Tasks.Task<string> GetInsertedDVDAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/InsertDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/InsertDVDResponse")]
        JobResult InsertDVD(string vmId, string isoPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/InsertDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/InsertDVDResponse")]
        System.Threading.Tasks.Task<JobResult> InsertDVDAsync(string vmId, string isoPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/EjectDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/EjectDVDResponse")]
        JobResult EjectDVD(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/EjectDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/EjectDVDResponse")]
        System.Threading.Tasks.Task<JobResult> EjectDVDAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetLibraryItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetLibraryItemsResponse")]
        LibraryItem[] GetLibraryItems(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetLibraryItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetLibraryItemsResponse")]
        System.Threading.Tasks.Task<LibraryItem[]> GetLibraryItemsAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetKVPItemsResponse")]
        List<KvpExchangeDataItem> GetKVPItems(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetKVPItemsResponse")]
        System.Threading.Tasks.Task<List<KvpExchangeDataItem>> GetKVPItemsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetStandardKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetStandardKVPItemsResponse")]
        List<KvpExchangeDataItem> GetStandardKVPItems(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetStandardKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetStandardKVPItemsResponse")]
        System.Threading.Tasks.Task<List<KvpExchangeDataItem>> GetStandardKVPItemsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/AddKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/AddKVPItemsResponse")]
        JobResult AddKVPItems(string vmId, KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/AddKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/AddKVPItemsResponse")]
        System.Threading.Tasks.Task<JobResult> AddKVPItemsAsync(string vmId, KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/RemoveKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/RemoveKVPItemsResponse")]
        JobResult RemoveKVPItems(string vmId, string[] itemNames);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/RemoveKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/RemoveKVPItemsResponse")]
        System.Threading.Tasks.Task<JobResult> RemoveKVPItemsAsync(string vmId, string[] itemNames);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ModifyKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ModifyKVPItemsResponse")]
        JobResult ModifyKVPItems(string vmId, KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ModifyKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ModifyKVPItemsResponse")]
        System.Threading.Tasks.Task<JobResult> ModifyKVPItemsAsync(string vmId, KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualHardDiskInfo", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualHardDiskInfoResponse")]
        VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualHardDiskInfo", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetVirtualHardDiskInfoResponse")]
        System.Threading.Tasks.Task<VirtualHardDiskInfo> GetVirtualHardDiskInfoAsync(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/MountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/MountVirtualHardDiskResponse")]
        MountedDiskInfo MountVirtualHardDisk(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/MountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/MountVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<MountedDiskInfo> MountVirtualHardDiskAsync(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/UnmountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/UnmountVirtualHardDiskResponse")]
        ReturnCode UnmountVirtualHardDisk(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/UnmountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/UnmountVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<ReturnCode> UnmountVirtualHardDiskAsync(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ExpandVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ExpandVirtualHardDiskResponse")]
        JobResult ExpandVirtualHardDisk(string vhdPath, UInt64 sizeGB);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ExpandVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ExpandVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<JobResult> ExpandVirtualHardDiskAsync(string vhdPath, UInt64 sizeGB);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ConvertVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ConvertVirtualHardDiskResponse")]
        JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, VirtualHardDiskType diskType);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ConvertVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ConvertVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<JobResult> ConvertVirtualHardDiskAsync(string sourcePath, string destinationPath, VirtualHardDiskType diskType);
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
        ConcreteJob GetJob(string jobId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetJob", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetJobResponse")]
        System.Threading.Tasks.Task<ConcreteJob> GetJobAsync(string jobId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetAllJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetAllJobsResponse")]
        List<ConcreteJob> GetAllJobs();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetAllJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetAllJobsResponse")]
        System.Threading.Tasks.Task<List<ConcreteJob>> GetAllJobsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ChangeJobState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ChangeJobStateResponse")]
        ChangeJobStateReturnCode ChangeJobState(string jobId, ConcreteJobRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/ChangeJobState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/ChangeJobStateResponse")]
        System.Threading.Tasks.Task<ChangeJobStateReturnCode> ChangeJobStateAsync(string jobId, ConcreteJobRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetProcessorCoresNumber", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetProcessorCoresNumberResponse")]
        int GetProcessorCoresNumber();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer/GetProcessorCoresNumber", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer/GetProcessorCoresNumberResponse")]
        System.Threading.Tasks.Task<int> GetProcessorCoresNumberAsync();
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class VirtualizationServerAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IVirtualizationServer
    {
        public VirtualMachine GetVirtualMachine(string vmId)
        {
            return (VirtualMachine)Invoke("SolidCP.Server.VirtualizationServer", "GetVirtualMachine", vmId);
        }

        public async System.Threading.Tasks.Task<VirtualMachine> GetVirtualMachineAsync(string vmId)
        {
            return await InvokeAsync<VirtualMachine>("SolidCP.Server.VirtualizationServer", "GetVirtualMachine", vmId);
        }

        public VirtualMachine GetVirtualMachineEx(string vmId)
        {
            return (VirtualMachine)Invoke("SolidCP.Server.VirtualizationServer", "GetVirtualMachineEx", vmId);
        }

        public async System.Threading.Tasks.Task<VirtualMachine> GetVirtualMachineExAsync(string vmId)
        {
            return await InvokeAsync<VirtualMachine>("SolidCP.Server.VirtualizationServer", "GetVirtualMachineEx", vmId);
        }

        public List<VirtualMachine> GetVirtualMachines()
        {
            return (List<VirtualMachine>)Invoke("SolidCP.Server.VirtualizationServer", "GetVirtualMachines");
        }

        public async System.Threading.Tasks.Task<List<VirtualMachine>> GetVirtualMachinesAsync()
        {
            return await InvokeAsync<List<VirtualMachine>>("SolidCP.Server.VirtualizationServer", "GetVirtualMachines");
        }

        public byte[] GetVirtualMachineThumbnailImage(string vmId, ThumbnailSize size)
        {
            return (byte[])Invoke("SolidCP.Server.VirtualizationServer", "GetVirtualMachineThumbnailImage", vmId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailImageAsync(string vmId, ThumbnailSize size)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.VirtualizationServer", "GetVirtualMachineThumbnailImage", vmId, size);
        }

        public VirtualMachine CreateVirtualMachine(VirtualMachine vm)
        {
            return (VirtualMachine)Invoke("SolidCP.Server.VirtualizationServer", "CreateVirtualMachine", vm);
        }

        public async System.Threading.Tasks.Task<VirtualMachine> CreateVirtualMachineAsync(VirtualMachine vm)
        {
            return await InvokeAsync<VirtualMachine>("SolidCP.Server.VirtualizationServer", "CreateVirtualMachine", vm);
        }

        public VirtualMachine UpdateVirtualMachine(VirtualMachine vm)
        {
            return (VirtualMachine)Invoke("SolidCP.Server.VirtualizationServer", "UpdateVirtualMachine", vm);
        }

        public async System.Threading.Tasks.Task<VirtualMachine> UpdateVirtualMachineAsync(VirtualMachine vm)
        {
            return await InvokeAsync<VirtualMachine>("SolidCP.Server.VirtualizationServer", "UpdateVirtualMachine", vm);
        }

        public JobResult ChangeVirtualMachineState(string vmId, VirtualMachineRequestedState newState)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer", "ChangeVirtualMachineState", vmId, newState);
        }

        public async System.Threading.Tasks.Task<JobResult> ChangeVirtualMachineStateAsync(string vmId, VirtualMachineRequestedState newState)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer", "ChangeVirtualMachineState", vmId, newState);
        }

        public ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason)
        {
            return (ReturnCode)Invoke("SolidCP.Server.VirtualizationServer", "ShutDownVirtualMachine", vmId, force, reason);
        }

        public async System.Threading.Tasks.Task<ReturnCode> ShutDownVirtualMachineAsync(string vmId, bool force, string reason)
        {
            return await InvokeAsync<ReturnCode>("SolidCP.Server.VirtualizationServer", "ShutDownVirtualMachine", vmId, force, reason);
        }

        public List<ConcreteJob> GetVirtualMachineJobs(string vmId)
        {
            return (List<ConcreteJob>)Invoke("SolidCP.Server.VirtualizationServer", "GetVirtualMachineJobs", vmId);
        }

        public async System.Threading.Tasks.Task<List<ConcreteJob>> GetVirtualMachineJobsAsync(string vmId)
        {
            return await InvokeAsync<List<ConcreteJob>>("SolidCP.Server.VirtualizationServer", "GetVirtualMachineJobs", vmId);
        }

        public JobResult RenameVirtualMachine(string vmId, string name)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer", "RenameVirtualMachine", vmId, name);
        }

        public async System.Threading.Tasks.Task<JobResult> RenameVirtualMachineAsync(string vmId, string name)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer", "RenameVirtualMachine", vmId, name);
        }

        public JobResult DeleteVirtualMachine(string vmId)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer", "DeleteVirtualMachine", vmId);
        }

        public async System.Threading.Tasks.Task<JobResult> DeleteVirtualMachineAsync(string vmId)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer", "DeleteVirtualMachine", vmId);
        }

        public JobResult ExportVirtualMachine(string vmId, string exportPath)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer", "ExportVirtualMachine", vmId, exportPath);
        }

        public async System.Threading.Tasks.Task<JobResult> ExportVirtualMachineAsync(string vmId, string exportPath)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer", "ExportVirtualMachine", vmId, exportPath);
        }

        public List<VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId)
        {
            return (List<VirtualMachineSnapshot>)Invoke("SolidCP.Server.VirtualizationServer", "GetVirtualMachineSnapshots", vmId);
        }

        public async System.Threading.Tasks.Task<List<VirtualMachineSnapshot>> GetVirtualMachineSnapshotsAsync(string vmId)
        {
            return await InvokeAsync<List<VirtualMachineSnapshot>>("SolidCP.Server.VirtualizationServer", "GetVirtualMachineSnapshots", vmId);
        }

        public VirtualMachineSnapshot GetSnapshot(string snapshotId)
        {
            return (VirtualMachineSnapshot)Invoke("SolidCP.Server.VirtualizationServer", "GetSnapshot", snapshotId);
        }

        public async System.Threading.Tasks.Task<VirtualMachineSnapshot> GetSnapshotAsync(string snapshotId)
        {
            return await InvokeAsync<VirtualMachineSnapshot>("SolidCP.Server.VirtualizationServer", "GetSnapshot", snapshotId);
        }

        public JobResult CreateSnapshot(string vmId)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer", "CreateSnapshot", vmId);
        }

        public async System.Threading.Tasks.Task<JobResult> CreateSnapshotAsync(string vmId)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer", "CreateSnapshot", vmId);
        }

        public JobResult RenameSnapshot(string vmId, string snapshotId, string name)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer", "RenameSnapshot", vmId, snapshotId, name);
        }

        public async System.Threading.Tasks.Task<JobResult> RenameSnapshotAsync(string vmId, string snapshotId, string name)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer", "RenameSnapshot", vmId, snapshotId, name);
        }

        public JobResult ApplySnapshot(string vmId, string snapshotId)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer", "ApplySnapshot", vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<JobResult> ApplySnapshotAsync(string vmId, string snapshotId)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer", "ApplySnapshot", vmId, snapshotId);
        }

        public JobResult DeleteSnapshot(string snapshotId)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer", "DeleteSnapshot", snapshotId);
        }

        public async System.Threading.Tasks.Task<JobResult> DeleteSnapshotAsync(string snapshotId)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer", "DeleteSnapshot", snapshotId);
        }

        public JobResult DeleteSnapshotSubtree(string snapshotId)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer", "DeleteSnapshotSubtree", snapshotId);
        }

        public async System.Threading.Tasks.Task<JobResult> DeleteSnapshotSubtreeAsync(string snapshotId)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer", "DeleteSnapshotSubtree", snapshotId);
        }

        public byte[] GetSnapshotThumbnailImage(string snapshotId, ThumbnailSize size)
        {
            return (byte[])Invoke("SolidCP.Server.VirtualizationServer", "GetSnapshotThumbnailImage", snapshotId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailImageAsync(string snapshotId, ThumbnailSize size)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.VirtualizationServer", "GetSnapshotThumbnailImage", snapshotId, size);
        }

        public List<VirtualSwitch> GetExternalSwitches(string computerName)
        {
            return (List<VirtualSwitch>)Invoke("SolidCP.Server.VirtualizationServer", "GetExternalSwitches", computerName);
        }

        public async System.Threading.Tasks.Task<List<VirtualSwitch>> GetExternalSwitchesAsync(string computerName)
        {
            return await InvokeAsync<List<VirtualSwitch>>("SolidCP.Server.VirtualizationServer", "GetExternalSwitches", computerName);
        }

        public List<VirtualSwitch> GetSwitches()
        {
            return (List<VirtualSwitch>)Invoke("SolidCP.Server.VirtualizationServer", "GetSwitches");
        }

        public async System.Threading.Tasks.Task<List<VirtualSwitch>> GetSwitchesAsync()
        {
            return await InvokeAsync<List<VirtualSwitch>>("SolidCP.Server.VirtualizationServer", "GetSwitches");
        }

        public bool SwitchExists(string switchId)
        {
            return (bool)Invoke("SolidCP.Server.VirtualizationServer", "SwitchExists", switchId);
        }

        public async System.Threading.Tasks.Task<bool> SwitchExistsAsync(string switchId)
        {
            return await InvokeAsync<bool>("SolidCP.Server.VirtualizationServer", "SwitchExists", switchId);
        }

        public VirtualSwitch CreateSwitch(string name)
        {
            return (VirtualSwitch)Invoke("SolidCP.Server.VirtualizationServer", "CreateSwitch", name);
        }

        public async System.Threading.Tasks.Task<VirtualSwitch> CreateSwitchAsync(string name)
        {
            return await InvokeAsync<VirtualSwitch>("SolidCP.Server.VirtualizationServer", "CreateSwitch", name);
        }

        public ReturnCode DeleteSwitch(string switchId)
        {
            return (ReturnCode)Invoke("SolidCP.Server.VirtualizationServer", "DeleteSwitch", switchId);
        }

        public async System.Threading.Tasks.Task<ReturnCode> DeleteSwitchAsync(string switchId)
        {
            return await InvokeAsync<ReturnCode>("SolidCP.Server.VirtualizationServer", "DeleteSwitch", switchId);
        }

        public string GetInsertedDVD(string vmId)
        {
            return (string)Invoke("SolidCP.Server.VirtualizationServer", "GetInsertedDVD", vmId);
        }

        public async System.Threading.Tasks.Task<string> GetInsertedDVDAsync(string vmId)
        {
            return await InvokeAsync<string>("SolidCP.Server.VirtualizationServer", "GetInsertedDVD", vmId);
        }

        public JobResult InsertDVD(string vmId, string isoPath)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer", "InsertDVD", vmId, isoPath);
        }

        public async System.Threading.Tasks.Task<JobResult> InsertDVDAsync(string vmId, string isoPath)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer", "InsertDVD", vmId, isoPath);
        }

        public JobResult EjectDVD(string vmId)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer", "EjectDVD", vmId);
        }

        public async System.Threading.Tasks.Task<JobResult> EjectDVDAsync(string vmId)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer", "EjectDVD", vmId);
        }

        public LibraryItem[] GetLibraryItems(string path)
        {
            return (LibraryItem[])Invoke("SolidCP.Server.VirtualizationServer", "GetLibraryItems", path);
        }

        public async System.Threading.Tasks.Task<LibraryItem[]> GetLibraryItemsAsync(string path)
        {
            return await InvokeAsync<LibraryItem[]>("SolidCP.Server.VirtualizationServer", "GetLibraryItems", path);
        }

        public List<KvpExchangeDataItem> GetKVPItems(string vmId)
        {
            return (List<KvpExchangeDataItem>)Invoke("SolidCP.Server.VirtualizationServer", "GetKVPItems", vmId);
        }

        public async System.Threading.Tasks.Task<List<KvpExchangeDataItem>> GetKVPItemsAsync(string vmId)
        {
            return await InvokeAsync<List<KvpExchangeDataItem>>("SolidCP.Server.VirtualizationServer", "GetKVPItems", vmId);
        }

        public List<KvpExchangeDataItem> GetStandardKVPItems(string vmId)
        {
            return (List<KvpExchangeDataItem>)Invoke("SolidCP.Server.VirtualizationServer", "GetStandardKVPItems", vmId);
        }

        public async System.Threading.Tasks.Task<List<KvpExchangeDataItem>> GetStandardKVPItemsAsync(string vmId)
        {
            return await InvokeAsync<List<KvpExchangeDataItem>>("SolidCP.Server.VirtualizationServer", "GetStandardKVPItems", vmId);
        }

        public JobResult AddKVPItems(string vmId, KvpExchangeDataItem[] items)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer", "AddKVPItems", vmId, items);
        }

        public async System.Threading.Tasks.Task<JobResult> AddKVPItemsAsync(string vmId, KvpExchangeDataItem[] items)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer", "AddKVPItems", vmId, items);
        }

        public JobResult RemoveKVPItems(string vmId, string[] itemNames)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer", "RemoveKVPItems", vmId, itemNames);
        }

        public async System.Threading.Tasks.Task<JobResult> RemoveKVPItemsAsync(string vmId, string[] itemNames)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer", "RemoveKVPItems", vmId, itemNames);
        }

        public JobResult ModifyKVPItems(string vmId, KvpExchangeDataItem[] items)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer", "ModifyKVPItems", vmId, items);
        }

        public async System.Threading.Tasks.Task<JobResult> ModifyKVPItemsAsync(string vmId, KvpExchangeDataItem[] items)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer", "ModifyKVPItems", vmId, items);
        }

        public VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath)
        {
            return (VirtualHardDiskInfo)Invoke("SolidCP.Server.VirtualizationServer", "GetVirtualHardDiskInfo", vhdPath);
        }

        public async System.Threading.Tasks.Task<VirtualHardDiskInfo> GetVirtualHardDiskInfoAsync(string vhdPath)
        {
            return await InvokeAsync<VirtualHardDiskInfo>("SolidCP.Server.VirtualizationServer", "GetVirtualHardDiskInfo", vhdPath);
        }

        public MountedDiskInfo MountVirtualHardDisk(string vhdPath)
        {
            return (MountedDiskInfo)Invoke("SolidCP.Server.VirtualizationServer", "MountVirtualHardDisk", vhdPath);
        }

        public async System.Threading.Tasks.Task<MountedDiskInfo> MountVirtualHardDiskAsync(string vhdPath)
        {
            return await InvokeAsync<MountedDiskInfo>("SolidCP.Server.VirtualizationServer", "MountVirtualHardDisk", vhdPath);
        }

        public ReturnCode UnmountVirtualHardDisk(string vhdPath)
        {
            return (ReturnCode)Invoke("SolidCP.Server.VirtualizationServer", "UnmountVirtualHardDisk", vhdPath);
        }

        public async System.Threading.Tasks.Task<ReturnCode> UnmountVirtualHardDiskAsync(string vhdPath)
        {
            return await InvokeAsync<ReturnCode>("SolidCP.Server.VirtualizationServer", "UnmountVirtualHardDisk", vhdPath);
        }

        public JobResult ExpandVirtualHardDisk(string vhdPath, UInt64 sizeGB)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer", "ExpandVirtualHardDisk", vhdPath, sizeGB);
        }

        public async System.Threading.Tasks.Task<JobResult> ExpandVirtualHardDiskAsync(string vhdPath, UInt64 sizeGB)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer", "ExpandVirtualHardDisk", vhdPath, sizeGB);
        }

        public JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, VirtualHardDiskType diskType)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer", "ConvertVirtualHardDisk", sourcePath, destinationPath, diskType);
        }

        public async System.Threading.Tasks.Task<JobResult> ConvertVirtualHardDiskAsync(string sourcePath, string destinationPath, VirtualHardDiskType diskType)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer", "ConvertVirtualHardDisk", sourcePath, destinationPath, diskType);
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

        public ConcreteJob GetJob(string jobId)
        {
            return (ConcreteJob)Invoke("SolidCP.Server.VirtualizationServer", "GetJob", jobId);
        }

        public async System.Threading.Tasks.Task<ConcreteJob> GetJobAsync(string jobId)
        {
            return await InvokeAsync<ConcreteJob>("SolidCP.Server.VirtualizationServer", "GetJob", jobId);
        }

        public List<ConcreteJob> GetAllJobs()
        {
            return (List<ConcreteJob>)Invoke("SolidCP.Server.VirtualizationServer", "GetAllJobs");
        }

        public async System.Threading.Tasks.Task<List<ConcreteJob>> GetAllJobsAsync()
        {
            return await InvokeAsync<List<ConcreteJob>>("SolidCP.Server.VirtualizationServer", "GetAllJobs");
        }

        public ChangeJobStateReturnCode ChangeJobState(string jobId, ConcreteJobRequestedState newState)
        {
            return (ChangeJobStateReturnCode)Invoke("SolidCP.Server.VirtualizationServer", "ChangeJobState", jobId, newState);
        }

        public async System.Threading.Tasks.Task<ChangeJobStateReturnCode> ChangeJobStateAsync(string jobId, ConcreteJobRequestedState newState)
        {
            return await InvokeAsync<ChangeJobStateReturnCode>("SolidCP.Server.VirtualizationServer", "ChangeJobState", jobId, newState);
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

        public JobResult DeleteSnapshot(string snapshotId)
        {
            return base.Client.DeleteSnapshot(snapshotId);
        }

        public async System.Threading.Tasks.Task<JobResult> DeleteSnapshotAsync(string snapshotId)
        {
            return await base.Client.DeleteSnapshotAsync(snapshotId);
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
    }
}
#endif