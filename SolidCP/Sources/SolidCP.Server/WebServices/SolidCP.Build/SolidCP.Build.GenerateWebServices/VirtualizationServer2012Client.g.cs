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
    [ServiceContract(ConfigurationName = "IVirtualizationServer2012", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IVirtualizationServer2012
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineResponse")]
        VirtualMachine GetVirtualMachine(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineResponse")]
        System.Threading.Tasks.Task<VirtualMachine> GetVirtualMachineAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineEx", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineExResponse")]
        VirtualMachine GetVirtualMachineEx(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineEx", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineExResponse")]
        System.Threading.Tasks.Task<VirtualMachine> GetVirtualMachineExAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachines", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachinesResponse")]
        List<VirtualMachine> GetVirtualMachines();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachines", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachinesResponse")]
        System.Threading.Tasks.Task<List<VirtualMachine>> GetVirtualMachinesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineThumbnailImageResponse")]
        byte[] GetVirtualMachineThumbnailImage(string vmId, ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineThumbnailImageResponse")]
        System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailImageAsync(string vmId, ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateVirtualMachineResponse")]
        VirtualMachine CreateVirtualMachine(VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateVirtualMachineResponse")]
        System.Threading.Tasks.Task<VirtualMachine> CreateVirtualMachineAsync(VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/UpdateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/UpdateVirtualMachineResponse")]
        VirtualMachine UpdateVirtualMachine(VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/UpdateVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/UpdateVirtualMachineResponse")]
        System.Threading.Tasks.Task<VirtualMachine> UpdateVirtualMachineAsync(VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ChangeVirtualMachineState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ChangeVirtualMachineStateResponse")]
        JobResult ChangeVirtualMachineState(string vmId, VirtualMachineRequestedState newState, string clusterName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ChangeVirtualMachineState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ChangeVirtualMachineStateResponse")]
        System.Threading.Tasks.Task<JobResult> ChangeVirtualMachineStateAsync(string vmId, VirtualMachineRequestedState newState, string clusterName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ShutDownVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ShutDownVirtualMachineResponse")]
        ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ShutDownVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ShutDownVirtualMachineResponse")]
        System.Threading.Tasks.Task<ReturnCode> ShutDownVirtualMachineAsync(string vmId, bool force, string reason);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineJobsResponse")]
        List<ConcreteJob> GetVirtualMachineJobs(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineJobsResponse")]
        System.Threading.Tasks.Task<List<ConcreteJob>> GetVirtualMachineJobsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/RenameVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/RenameVirtualMachineResponse")]
        JobResult RenameVirtualMachine(string vmId, string name, string clusterName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/RenameVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/RenameVirtualMachineResponse")]
        System.Threading.Tasks.Task<JobResult> RenameVirtualMachineAsync(string vmId, string name, string clusterName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteVirtualMachineResponse")]
        JobResult DeleteVirtualMachine(string vmId, string clusterName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteVirtualMachineResponse")]
        System.Threading.Tasks.Task<JobResult> DeleteVirtualMachineAsync(string vmId, string clusterName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteVirtualMachineExtended", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteVirtualMachineExtendedResponse")]
        JobResult DeleteVirtualMachineExtended(string vmId, string clusterName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteVirtualMachineExtended", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteVirtualMachineExtendedResponse")]
        System.Threading.Tasks.Task<JobResult> DeleteVirtualMachineExtendedAsync(string vmId, string clusterName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExportVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExportVirtualMachineResponse")]
        JobResult ExportVirtualMachine(string vmId, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExportVirtualMachine", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExportVirtualMachineResponse")]
        System.Threading.Tasks.Task<JobResult> ExportVirtualMachineAsync(string vmId, string exportPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/IsTryToUpdateVirtualMachineWithoutRebootSuccess", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/IsTryToUpdateVirtualMachineWithoutRebootSuccessResponse")]
        bool IsTryToUpdateVirtualMachineWithoutRebootSuccess(VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/IsTryToUpdateVirtualMachineWithoutRebootSuccess", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/IsTryToUpdateVirtualMachineWithoutRebootSuccessResponse")]
        System.Threading.Tasks.Task<bool> IsTryToUpdateVirtualMachineWithoutRebootSuccessAsync(VirtualMachine vm);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineSnapshots", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineSnapshotsResponse")]
        List<VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineSnapshots", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachineSnapshotsResponse")]
        System.Threading.Tasks.Task<List<VirtualMachineSnapshot>> GetVirtualMachineSnapshotsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSnapshotResponse")]
        VirtualMachineSnapshot GetSnapshot(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSnapshotResponse")]
        System.Threading.Tasks.Task<VirtualMachineSnapshot> GetSnapshotAsync(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateSnapshotResponse")]
        JobResult CreateSnapshot(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateSnapshotResponse")]
        System.Threading.Tasks.Task<JobResult> CreateSnapshotAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/RenameSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/RenameSnapshotResponse")]
        JobResult RenameSnapshot(string vmId, string snapshotId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/RenameSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/RenameSnapshotResponse")]
        System.Threading.Tasks.Task<JobResult> RenameSnapshotAsync(string vmId, string snapshotId, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ApplySnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ApplySnapshotResponse")]
        JobResult ApplySnapshot(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ApplySnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ApplySnapshotResponse")]
        System.Threading.Tasks.Task<JobResult> ApplySnapshotAsync(string vmId, string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteSnapshotResponse")]
        JobResult DeleteSnapshot(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteSnapshot", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteSnapshotResponse")]
        System.Threading.Tasks.Task<JobResult> DeleteSnapshotAsync(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteSnapshotSubtree", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteSnapshotSubtreeResponse")]
        JobResult DeleteSnapshotSubtree(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteSnapshotSubtree", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteSnapshotSubtreeResponse")]
        System.Threading.Tasks.Task<JobResult> DeleteSnapshotSubtreeAsync(string snapshotId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSnapshotThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSnapshotThumbnailImageResponse")]
        byte[] GetSnapshotThumbnailImage(string snapshotId, ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSnapshotThumbnailImage", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSnapshotThumbnailImageResponse")]
        System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailImageAsync(string snapshotId, ThumbnailSize size);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSecureBootTemplates", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSecureBootTemplatesResponse")]
        List<SecureBootTemplate> GetSecureBootTemplates(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSecureBootTemplates", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSecureBootTemplatesResponse")]
        System.Threading.Tasks.Task<List<SecureBootTemplate>> GetSecureBootTemplatesAsync(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetExternalSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetExternalSwitchesResponse")]
        List<VirtualSwitch> GetExternalSwitches(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetExternalSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetExternalSwitchesResponse")]
        System.Threading.Tasks.Task<List<VirtualSwitch>> GetExternalSwitchesAsync(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetExternalSwitchesWMI", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetExternalSwitchesWMIResponse")]
        List<VirtualSwitch> GetExternalSwitchesWMI(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetExternalSwitchesWMI", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetExternalSwitchesWMIResponse")]
        System.Threading.Tasks.Task<List<VirtualSwitch>> GetExternalSwitchesWMIAsync(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetInternalSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetInternalSwitchesResponse")]
        List<VirtualSwitch> GetInternalSwitches(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetInternalSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetInternalSwitchesResponse")]
        System.Threading.Tasks.Task<List<VirtualSwitch>> GetInternalSwitchesAsync(string computerName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSwitchesResponse")]
        List<VirtualSwitch> GetSwitches();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSwitches", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetSwitchesResponse")]
        System.Threading.Tasks.Task<List<VirtualSwitch>> GetSwitchesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/SwitchExists", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/SwitchExistsResponse")]
        bool SwitchExists(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/SwitchExists", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/SwitchExistsResponse")]
        System.Threading.Tasks.Task<bool> SwitchExistsAsync(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateSwitchResponse")]
        VirtualSwitch CreateSwitch(string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateSwitchResponse")]
        System.Threading.Tasks.Task<VirtualSwitch> CreateSwitchAsync(string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteSwitchResponse")]
        ReturnCode DeleteSwitch(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteSwitch", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DeleteSwitchResponse")]
        System.Threading.Tasks.Task<ReturnCode> DeleteSwitchAsync(string switchId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachinesNetwordAdapterSettings", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachinesNetwordAdapterSettingsResponse")]
        List<VirtualMachineNetworkAdapter> GetVirtualMachinesNetwordAdapterSettings(string vmName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachinesNetwordAdapterSettings", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualMachinesNetwordAdapterSettingsResponse")]
        System.Threading.Tasks.Task<List<VirtualMachineNetworkAdapter>> GetVirtualMachinesNetwordAdapterSettingsAsync(string vmName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/InjectIPs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/InjectIPsResponse")]
        JobResult InjectIPs(string vmId, GuestNetworkAdapterConfiguration guestNetworkAdapterConfiguration);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/InjectIPs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/InjectIPsResponse")]
        System.Threading.Tasks.Task<JobResult> InjectIPsAsync(string vmId, GuestNetworkAdapterConfiguration guestNetworkAdapterConfiguration);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetInsertedDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetInsertedDVDResponse")]
        string GetInsertedDVD(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetInsertedDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetInsertedDVDResponse")]
        System.Threading.Tasks.Task<string> GetInsertedDVDAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/InsertDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/InsertDVDResponse")]
        JobResult InsertDVD(string vmId, string isoPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/InsertDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/InsertDVDResponse")]
        System.Threading.Tasks.Task<JobResult> InsertDVDAsync(string vmId, string isoPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/EjectDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/EjectDVDResponse")]
        JobResult EjectDVD(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/EjectDVD", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/EjectDVDResponse")]
        System.Threading.Tasks.Task<JobResult> EjectDVDAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetKVPItemsResponse")]
        List<KvpExchangeDataItem> GetKVPItems(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetKVPItemsResponse")]
        System.Threading.Tasks.Task<List<KvpExchangeDataItem>> GetKVPItemsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetStandardKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetStandardKVPItemsResponse")]
        List<KvpExchangeDataItem> GetStandardKVPItems(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetStandardKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetStandardKVPItemsResponse")]
        System.Threading.Tasks.Task<List<KvpExchangeDataItem>> GetStandardKVPItemsAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/AddKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/AddKVPItemsResponse")]
        JobResult AddKVPItems(string vmId, KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/AddKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/AddKVPItemsResponse")]
        System.Threading.Tasks.Task<JobResult> AddKVPItemsAsync(string vmId, KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/RemoveKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/RemoveKVPItemsResponse")]
        JobResult RemoveKVPItems(string vmId, string[] itemNames);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/RemoveKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/RemoveKVPItemsResponse")]
        System.Threading.Tasks.Task<JobResult> RemoveKVPItemsAsync(string vmId, string[] itemNames);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ModifyKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ModifyKVPItemsResponse")]
        JobResult ModifyKVPItems(string vmId, KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ModifyKVPItems", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ModifyKVPItemsResponse")]
        System.Threading.Tasks.Task<JobResult> ModifyKVPItemsAsync(string vmId, KvpExchangeDataItem[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/IsEmptyFolders", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/IsEmptyFoldersResponse")]
        bool IsEmptyFolders(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/IsEmptyFolders", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/IsEmptyFoldersResponse")]
        System.Threading.Tasks.Task<bool> IsEmptyFoldersAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/FileExists", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/FileExistsResponse")]
        bool FileExists(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/FileExists", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/FileExistsResponse")]
        System.Threading.Tasks.Task<bool> FileExistsAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualHardDiskInfo", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualHardDiskInfoResponse")]
        VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualHardDiskInfo", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVirtualHardDiskInfoResponse")]
        System.Threading.Tasks.Task<VirtualHardDiskInfo> GetVirtualHardDiskInfoAsync(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/MountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/MountVirtualHardDiskResponse")]
        MountedDiskInfo MountVirtualHardDisk(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/MountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/MountVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<MountedDiskInfo> MountVirtualHardDiskAsync(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/UnmountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/UnmountVirtualHardDiskResponse")]
        ReturnCode UnmountVirtualHardDisk(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/UnmountVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/UnmountVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<ReturnCode> UnmountVirtualHardDiskAsync(string vhdPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExpandVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExpandVirtualHardDiskResponse")]
        JobResult ExpandVirtualHardDisk(string vhdPath, UInt64 sizeGB);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExpandVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExpandVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<JobResult> ExpandVirtualHardDiskAsync(string vhdPath, UInt64 sizeGB);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ConvertVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ConvertVirtualHardDiskResponse")]
        JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, VirtualHardDiskType diskType, uint blockSizeBytes);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ConvertVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ConvertVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<JobResult> ConvertVirtualHardDiskAsync(string sourcePath, string destinationPath, VirtualHardDiskType diskType, uint blockSizeBytes);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateVirtualHardDiskResponse")]
        JobResult CreateVirtualHardDisk(string destinationPath, VirtualHardDiskType diskType, uint blockSizeBytes, UInt64 sizeGB);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateVirtualHardDisk", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/CreateVirtualHardDiskResponse")]
        System.Threading.Tasks.Task<JobResult> CreateVirtualHardDiskAsync(string destinationPath, VirtualHardDiskType diskType, uint blockSizeBytes, UInt64 sizeGB);
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
        ConcreteJob GetJob(string jobId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetJob", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetJobResponse")]
        System.Threading.Tasks.Task<ConcreteJob> GetJobAsync(string jobId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetAllJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetAllJobsResponse")]
        List<ConcreteJob> GetAllJobs();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetAllJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetAllJobsResponse")]
        System.Threading.Tasks.Task<List<ConcreteJob>> GetAllJobsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ClearOldJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ClearOldJobsResponse")]
        void ClearOldJobs();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ClearOldJobs", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ClearOldJobsResponse")]
        System.Threading.Tasks.Task ClearOldJobsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ChangeJobState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ChangeJobStateResponse")]
        ChangeJobStateReturnCode ChangeJobState(string jobId, ConcreteJobRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ChangeJobState", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ChangeJobStateResponse")]
        System.Threading.Tasks.Task<ChangeJobStateReturnCode> ChangeJobStateAsync(string jobId, ConcreteJobRequestedState newState);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetProcessorCoresNumber", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetProcessorCoresNumberResponse")]
        int GetProcessorCoresNumber();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetProcessorCoresNumber", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetProcessorCoresNumberResponse")]
        System.Threading.Tasks.Task<int> GetProcessorCoresNumberAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVMConfigurationVersionSupportedList", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVMConfigurationVersionSupportedListResponse")]
        List<VMConfigurationVersion> GetVMConfigurationVersionSupportedList();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVMConfigurationVersionSupportedList", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetVMConfigurationVersionSupportedListResponse")]
        System.Threading.Tasks.Task<List<VMConfigurationVersion>> GetVMConfigurationVersionSupportedListAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetCertificates", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetCertificatesResponse")]
        List<CertificateInfo> GetCertificates(string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetCertificates", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetCertificatesResponse")]
        System.Threading.Tasks.Task<List<CertificateInfo>> GetCertificatesAsync(string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/SetReplicaServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/SetReplicaServerResponse")]
        void SetReplicaServer(string remoteServer, string thumbprint, string storagePath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/SetReplicaServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/SetReplicaServerResponse")]
        System.Threading.Tasks.Task SetReplicaServerAsync(string remoteServer, string thumbprint, string storagePath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/UnsetReplicaServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/UnsetReplicaServerResponse")]
        void UnsetReplicaServer(string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/UnsetReplicaServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/UnsetReplicaServerResponse")]
        System.Threading.Tasks.Task UnsetReplicaServerAsync(string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetReplicaServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetReplicaServerResponse")]
        ReplicationServerInfo GetReplicaServer(string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetReplicaServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetReplicaServerResponse")]
        System.Threading.Tasks.Task<ReplicationServerInfo> GetReplicaServerAsync(string remoteServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/EnableVmReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/EnableVmReplicationResponse")]
        void EnableVmReplication(string vmId, string replicaServer, VmReplication replication);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/EnableVmReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/EnableVmReplicationResponse")]
        System.Threading.Tasks.Task EnableVmReplicationAsync(string vmId, string replicaServer, VmReplication replication);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/SetVmReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/SetVmReplicationResponse")]
        void SetVmReplication(string vmId, string replicaServer, VmReplication replication);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/SetVmReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/SetVmReplicationResponse")]
        System.Threading.Tasks.Task SetVmReplicationAsync(string vmId, string replicaServer, VmReplication replication);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/TestReplicationServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/TestReplicationServerResponse")]
        void TestReplicationServer(string vmId, string replicaServer, string localThumbprint);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/TestReplicationServer", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/TestReplicationServerResponse")]
        System.Threading.Tasks.Task TestReplicationServerAsync(string vmId, string replicaServer, string localThumbprint);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/StartInitialReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/StartInitialReplicationResponse")]
        void StartInitialReplication(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/StartInitialReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/StartInitialReplicationResponse")]
        System.Threading.Tasks.Task StartInitialReplicationAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetReplicationResponse")]
        VmReplication GetReplication(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetReplicationResponse")]
        System.Threading.Tasks.Task<VmReplication> GetReplicationAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DisableVmReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DisableVmReplicationResponse")]
        void DisableVmReplication(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DisableVmReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/DisableVmReplicationResponse")]
        System.Threading.Tasks.Task DisableVmReplicationAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetReplicationInfo", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetReplicationInfoResponse")]
        ReplicationDetailInfo GetReplicationInfo(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetReplicationInfo", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/GetReplicationInfoResponse")]
        System.Threading.Tasks.Task<ReplicationDetailInfo> GetReplicationInfoAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/PauseReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/PauseReplicationResponse")]
        void PauseReplication(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/PauseReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/PauseReplicationResponse")]
        System.Threading.Tasks.Task PauseReplicationAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ResumeReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ResumeReplicationResponse")]
        void ResumeReplication(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ResumeReplication", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ResumeReplicationResponse")]
        System.Threading.Tasks.Task ResumeReplicationAsync(string vmId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExecuteCustomPsScript", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExecuteCustomPsScriptResponse")]
        JobResult ExecuteCustomPsScript(string script);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExecuteCustomPsScript", ReplyAction = "http://smbsaas/solidcp/server/IVirtualizationServer2012/ExecuteCustomPsScriptResponse")]
        System.Threading.Tasks.Task<JobResult> ExecuteCustomPsScriptAsync(string script);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class VirtualizationServer2012AssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IVirtualizationServer2012
    {
        public VirtualMachine GetVirtualMachine(string vmId)
        {
            return (VirtualMachine)Invoke("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachine", vmId);
        }

        public async System.Threading.Tasks.Task<VirtualMachine> GetVirtualMachineAsync(string vmId)
        {
            return await InvokeAsync<VirtualMachine>("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachine", vmId);
        }

        public VirtualMachine GetVirtualMachineEx(string vmId)
        {
            return (VirtualMachine)Invoke("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachineEx", vmId);
        }

        public async System.Threading.Tasks.Task<VirtualMachine> GetVirtualMachineExAsync(string vmId)
        {
            return await InvokeAsync<VirtualMachine>("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachineEx", vmId);
        }

        public List<VirtualMachine> GetVirtualMachines()
        {
            return (List<VirtualMachine>)Invoke("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachines");
        }

        public async System.Threading.Tasks.Task<List<VirtualMachine>> GetVirtualMachinesAsync()
        {
            return await InvokeAsync<List<VirtualMachine>>("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachines");
        }

        public byte[] GetVirtualMachineThumbnailImage(string vmId, ThumbnailSize size)
        {
            return (byte[])Invoke("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachineThumbnailImage", vmId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetVirtualMachineThumbnailImageAsync(string vmId, ThumbnailSize size)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachineThumbnailImage", vmId, size);
        }

        public VirtualMachine CreateVirtualMachine(VirtualMachine vm)
        {
            return (VirtualMachine)Invoke("SolidCP.Server.VirtualizationServer2012", "CreateVirtualMachine", vm);
        }

        public async System.Threading.Tasks.Task<VirtualMachine> CreateVirtualMachineAsync(VirtualMachine vm)
        {
            return await InvokeAsync<VirtualMachine>("SolidCP.Server.VirtualizationServer2012", "CreateVirtualMachine", vm);
        }

        public VirtualMachine UpdateVirtualMachine(VirtualMachine vm)
        {
            return (VirtualMachine)Invoke("SolidCP.Server.VirtualizationServer2012", "UpdateVirtualMachine", vm);
        }

        public async System.Threading.Tasks.Task<VirtualMachine> UpdateVirtualMachineAsync(VirtualMachine vm)
        {
            return await InvokeAsync<VirtualMachine>("SolidCP.Server.VirtualizationServer2012", "UpdateVirtualMachine", vm);
        }

        public JobResult ChangeVirtualMachineState(string vmId, VirtualMachineRequestedState newState, string clusterName)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer2012", "ChangeVirtualMachineState", vmId, newState, clusterName);
        }

        public async System.Threading.Tasks.Task<JobResult> ChangeVirtualMachineStateAsync(string vmId, VirtualMachineRequestedState newState, string clusterName)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer2012", "ChangeVirtualMachineState", vmId, newState, clusterName);
        }

        public ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason)
        {
            return (ReturnCode)Invoke("SolidCP.Server.VirtualizationServer2012", "ShutDownVirtualMachine", vmId, force, reason);
        }

        public async System.Threading.Tasks.Task<ReturnCode> ShutDownVirtualMachineAsync(string vmId, bool force, string reason)
        {
            return await InvokeAsync<ReturnCode>("SolidCP.Server.VirtualizationServer2012", "ShutDownVirtualMachine", vmId, force, reason);
        }

        public List<ConcreteJob> GetVirtualMachineJobs(string vmId)
        {
            return (List<ConcreteJob>)Invoke("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachineJobs", vmId);
        }

        public async System.Threading.Tasks.Task<List<ConcreteJob>> GetVirtualMachineJobsAsync(string vmId)
        {
            return await InvokeAsync<List<ConcreteJob>>("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachineJobs", vmId);
        }

        public JobResult RenameVirtualMachine(string vmId, string name, string clusterName)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer2012", "RenameVirtualMachine", vmId, name, clusterName);
        }

        public async System.Threading.Tasks.Task<JobResult> RenameVirtualMachineAsync(string vmId, string name, string clusterName)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer2012", "RenameVirtualMachine", vmId, name, clusterName);
        }

        public JobResult DeleteVirtualMachine(string vmId, string clusterName)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer2012", "DeleteVirtualMachine", vmId, clusterName);
        }

        public async System.Threading.Tasks.Task<JobResult> DeleteVirtualMachineAsync(string vmId, string clusterName)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer2012", "DeleteVirtualMachine", vmId, clusterName);
        }

        public JobResult DeleteVirtualMachineExtended(string vmId, string clusterName)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer2012", "DeleteVirtualMachineExtended", vmId, clusterName);
        }

        public async System.Threading.Tasks.Task<JobResult> DeleteVirtualMachineExtendedAsync(string vmId, string clusterName)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer2012", "DeleteVirtualMachineExtended", vmId, clusterName);
        }

        public JobResult ExportVirtualMachine(string vmId, string exportPath)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer2012", "ExportVirtualMachine", vmId, exportPath);
        }

        public async System.Threading.Tasks.Task<JobResult> ExportVirtualMachineAsync(string vmId, string exportPath)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer2012", "ExportVirtualMachine", vmId, exportPath);
        }

        public bool IsTryToUpdateVirtualMachineWithoutRebootSuccess(VirtualMachine vm)
        {
            return (bool)Invoke("SolidCP.Server.VirtualizationServer2012", "IsTryToUpdateVirtualMachineWithoutRebootSuccess", vm);
        }

        public async System.Threading.Tasks.Task<bool> IsTryToUpdateVirtualMachineWithoutRebootSuccessAsync(VirtualMachine vm)
        {
            return await InvokeAsync<bool>("SolidCP.Server.VirtualizationServer2012", "IsTryToUpdateVirtualMachineWithoutRebootSuccess", vm);
        }

        public List<VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId)
        {
            return (List<VirtualMachineSnapshot>)Invoke("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachineSnapshots", vmId);
        }

        public async System.Threading.Tasks.Task<List<VirtualMachineSnapshot>> GetVirtualMachineSnapshotsAsync(string vmId)
        {
            return await InvokeAsync<List<VirtualMachineSnapshot>>("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachineSnapshots", vmId);
        }

        public VirtualMachineSnapshot GetSnapshot(string snapshotId)
        {
            return (VirtualMachineSnapshot)Invoke("SolidCP.Server.VirtualizationServer2012", "GetSnapshot", snapshotId);
        }

        public async System.Threading.Tasks.Task<VirtualMachineSnapshot> GetSnapshotAsync(string snapshotId)
        {
            return await InvokeAsync<VirtualMachineSnapshot>("SolidCP.Server.VirtualizationServer2012", "GetSnapshot", snapshotId);
        }

        public JobResult CreateSnapshot(string vmId)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer2012", "CreateSnapshot", vmId);
        }

        public async System.Threading.Tasks.Task<JobResult> CreateSnapshotAsync(string vmId)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer2012", "CreateSnapshot", vmId);
        }

        public JobResult RenameSnapshot(string vmId, string snapshotId, string name)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer2012", "RenameSnapshot", vmId, snapshotId, name);
        }

        public async System.Threading.Tasks.Task<JobResult> RenameSnapshotAsync(string vmId, string snapshotId, string name)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer2012", "RenameSnapshot", vmId, snapshotId, name);
        }

        public JobResult ApplySnapshot(string vmId, string snapshotId)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer2012", "ApplySnapshot", vmId, snapshotId);
        }

        public async System.Threading.Tasks.Task<JobResult> ApplySnapshotAsync(string vmId, string snapshotId)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer2012", "ApplySnapshot", vmId, snapshotId);
        }

        public JobResult DeleteSnapshot(string snapshotId)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer2012", "DeleteSnapshot", snapshotId);
        }

        public async System.Threading.Tasks.Task<JobResult> DeleteSnapshotAsync(string snapshotId)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer2012", "DeleteSnapshot", snapshotId);
        }

        public JobResult DeleteSnapshotSubtree(string snapshotId)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer2012", "DeleteSnapshotSubtree", snapshotId);
        }

        public async System.Threading.Tasks.Task<JobResult> DeleteSnapshotSubtreeAsync(string snapshotId)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer2012", "DeleteSnapshotSubtree", snapshotId);
        }

        public byte[] GetSnapshotThumbnailImage(string snapshotId, ThumbnailSize size)
        {
            return (byte[])Invoke("SolidCP.Server.VirtualizationServer2012", "GetSnapshotThumbnailImage", snapshotId, size);
        }

        public async System.Threading.Tasks.Task<byte[]> GetSnapshotThumbnailImageAsync(string snapshotId, ThumbnailSize size)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.VirtualizationServer2012", "GetSnapshotThumbnailImage", snapshotId, size);
        }

        public List<SecureBootTemplate> GetSecureBootTemplates(string computerName)
        {
            return (List<SecureBootTemplate>)Invoke("SolidCP.Server.VirtualizationServer2012", "GetSecureBootTemplates", computerName);
        }

        public async System.Threading.Tasks.Task<List<SecureBootTemplate>> GetSecureBootTemplatesAsync(string computerName)
        {
            return await InvokeAsync<List<SecureBootTemplate>>("SolidCP.Server.VirtualizationServer2012", "GetSecureBootTemplates", computerName);
        }

        public List<VirtualSwitch> GetExternalSwitches(string computerName)
        {
            return (List<VirtualSwitch>)Invoke("SolidCP.Server.VirtualizationServer2012", "GetExternalSwitches", computerName);
        }

        public async System.Threading.Tasks.Task<List<VirtualSwitch>> GetExternalSwitchesAsync(string computerName)
        {
            return await InvokeAsync<List<VirtualSwitch>>("SolidCP.Server.VirtualizationServer2012", "GetExternalSwitches", computerName);
        }

        public List<VirtualSwitch> GetExternalSwitchesWMI(string computerName)
        {
            return (List<VirtualSwitch>)Invoke("SolidCP.Server.VirtualizationServer2012", "GetExternalSwitchesWMI", computerName);
        }

        public async System.Threading.Tasks.Task<List<VirtualSwitch>> GetExternalSwitchesWMIAsync(string computerName)
        {
            return await InvokeAsync<List<VirtualSwitch>>("SolidCP.Server.VirtualizationServer2012", "GetExternalSwitchesWMI", computerName);
        }

        public List<VirtualSwitch> GetInternalSwitches(string computerName)
        {
            return (List<VirtualSwitch>)Invoke("SolidCP.Server.VirtualizationServer2012", "GetInternalSwitches", computerName);
        }

        public async System.Threading.Tasks.Task<List<VirtualSwitch>> GetInternalSwitchesAsync(string computerName)
        {
            return await InvokeAsync<List<VirtualSwitch>>("SolidCP.Server.VirtualizationServer2012", "GetInternalSwitches", computerName);
        }

        public List<VirtualSwitch> GetSwitches()
        {
            return (List<VirtualSwitch>)Invoke("SolidCP.Server.VirtualizationServer2012", "GetSwitches");
        }

        public async System.Threading.Tasks.Task<List<VirtualSwitch>> GetSwitchesAsync()
        {
            return await InvokeAsync<List<VirtualSwitch>>("SolidCP.Server.VirtualizationServer2012", "GetSwitches");
        }

        public bool SwitchExists(string switchId)
        {
            return (bool)Invoke("SolidCP.Server.VirtualizationServer2012", "SwitchExists", switchId);
        }

        public async System.Threading.Tasks.Task<bool> SwitchExistsAsync(string switchId)
        {
            return await InvokeAsync<bool>("SolidCP.Server.VirtualizationServer2012", "SwitchExists", switchId);
        }

        public VirtualSwitch CreateSwitch(string name)
        {
            return (VirtualSwitch)Invoke("SolidCP.Server.VirtualizationServer2012", "CreateSwitch", name);
        }

        public async System.Threading.Tasks.Task<VirtualSwitch> CreateSwitchAsync(string name)
        {
            return await InvokeAsync<VirtualSwitch>("SolidCP.Server.VirtualizationServer2012", "CreateSwitch", name);
        }

        public ReturnCode DeleteSwitch(string switchId)
        {
            return (ReturnCode)Invoke("SolidCP.Server.VirtualizationServer2012", "DeleteSwitch", switchId);
        }

        public async System.Threading.Tasks.Task<ReturnCode> DeleteSwitchAsync(string switchId)
        {
            return await InvokeAsync<ReturnCode>("SolidCP.Server.VirtualizationServer2012", "DeleteSwitch", switchId);
        }

        public List<VirtualMachineNetworkAdapter> GetVirtualMachinesNetwordAdapterSettings(string vmName)
        {
            return (List<VirtualMachineNetworkAdapter>)Invoke("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachinesNetwordAdapterSettings", vmName);
        }

        public async System.Threading.Tasks.Task<List<VirtualMachineNetworkAdapter>> GetVirtualMachinesNetwordAdapterSettingsAsync(string vmName)
        {
            return await InvokeAsync<List<VirtualMachineNetworkAdapter>>("SolidCP.Server.VirtualizationServer2012", "GetVirtualMachinesNetwordAdapterSettings", vmName);
        }

        public JobResult InjectIPs(string vmId, GuestNetworkAdapterConfiguration guestNetworkAdapterConfiguration)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer2012", "InjectIPs", vmId, guestNetworkAdapterConfiguration);
        }

        public async System.Threading.Tasks.Task<JobResult> InjectIPsAsync(string vmId, GuestNetworkAdapterConfiguration guestNetworkAdapterConfiguration)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer2012", "InjectIPs", vmId, guestNetworkAdapterConfiguration);
        }

        public string GetInsertedDVD(string vmId)
        {
            return (string)Invoke("SolidCP.Server.VirtualizationServer2012", "GetInsertedDVD", vmId);
        }

        public async System.Threading.Tasks.Task<string> GetInsertedDVDAsync(string vmId)
        {
            return await InvokeAsync<string>("SolidCP.Server.VirtualizationServer2012", "GetInsertedDVD", vmId);
        }

        public JobResult InsertDVD(string vmId, string isoPath)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer2012", "InsertDVD", vmId, isoPath);
        }

        public async System.Threading.Tasks.Task<JobResult> InsertDVDAsync(string vmId, string isoPath)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer2012", "InsertDVD", vmId, isoPath);
        }

        public JobResult EjectDVD(string vmId)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer2012", "EjectDVD", vmId);
        }

        public async System.Threading.Tasks.Task<JobResult> EjectDVDAsync(string vmId)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer2012", "EjectDVD", vmId);
        }

        public List<KvpExchangeDataItem> GetKVPItems(string vmId)
        {
            return (List<KvpExchangeDataItem>)Invoke("SolidCP.Server.VirtualizationServer2012", "GetKVPItems", vmId);
        }

        public async System.Threading.Tasks.Task<List<KvpExchangeDataItem>> GetKVPItemsAsync(string vmId)
        {
            return await InvokeAsync<List<KvpExchangeDataItem>>("SolidCP.Server.VirtualizationServer2012", "GetKVPItems", vmId);
        }

        public List<KvpExchangeDataItem> GetStandardKVPItems(string vmId)
        {
            return (List<KvpExchangeDataItem>)Invoke("SolidCP.Server.VirtualizationServer2012", "GetStandardKVPItems", vmId);
        }

        public async System.Threading.Tasks.Task<List<KvpExchangeDataItem>> GetStandardKVPItemsAsync(string vmId)
        {
            return await InvokeAsync<List<KvpExchangeDataItem>>("SolidCP.Server.VirtualizationServer2012", "GetStandardKVPItems", vmId);
        }

        public JobResult AddKVPItems(string vmId, KvpExchangeDataItem[] items)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer2012", "AddKVPItems", vmId, items);
        }

        public async System.Threading.Tasks.Task<JobResult> AddKVPItemsAsync(string vmId, KvpExchangeDataItem[] items)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer2012", "AddKVPItems", vmId, items);
        }

        public JobResult RemoveKVPItems(string vmId, string[] itemNames)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer2012", "RemoveKVPItems", vmId, itemNames);
        }

        public async System.Threading.Tasks.Task<JobResult> RemoveKVPItemsAsync(string vmId, string[] itemNames)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer2012", "RemoveKVPItems", vmId, itemNames);
        }

        public JobResult ModifyKVPItems(string vmId, KvpExchangeDataItem[] items)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer2012", "ModifyKVPItems", vmId, items);
        }

        public async System.Threading.Tasks.Task<JobResult> ModifyKVPItemsAsync(string vmId, KvpExchangeDataItem[] items)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer2012", "ModifyKVPItems", vmId, items);
        }

        public bool IsEmptyFolders(string path)
        {
            return (bool)Invoke("SolidCP.Server.VirtualizationServer2012", "IsEmptyFolders", path);
        }

        public async System.Threading.Tasks.Task<bool> IsEmptyFoldersAsync(string path)
        {
            return await InvokeAsync<bool>("SolidCP.Server.VirtualizationServer2012", "IsEmptyFolders", path);
        }

        public bool FileExists(string path)
        {
            return (bool)Invoke("SolidCP.Server.VirtualizationServer2012", "FileExists", path);
        }

        public async System.Threading.Tasks.Task<bool> FileExistsAsync(string path)
        {
            return await InvokeAsync<bool>("SolidCP.Server.VirtualizationServer2012", "FileExists", path);
        }

        public VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath)
        {
            return (VirtualHardDiskInfo)Invoke("SolidCP.Server.VirtualizationServer2012", "GetVirtualHardDiskInfo", vhdPath);
        }

        public async System.Threading.Tasks.Task<VirtualHardDiskInfo> GetVirtualHardDiskInfoAsync(string vhdPath)
        {
            return await InvokeAsync<VirtualHardDiskInfo>("SolidCP.Server.VirtualizationServer2012", "GetVirtualHardDiskInfo", vhdPath);
        }

        public MountedDiskInfo MountVirtualHardDisk(string vhdPath)
        {
            return (MountedDiskInfo)Invoke("SolidCP.Server.VirtualizationServer2012", "MountVirtualHardDisk", vhdPath);
        }

        public async System.Threading.Tasks.Task<MountedDiskInfo> MountVirtualHardDiskAsync(string vhdPath)
        {
            return await InvokeAsync<MountedDiskInfo>("SolidCP.Server.VirtualizationServer2012", "MountVirtualHardDisk", vhdPath);
        }

        public ReturnCode UnmountVirtualHardDisk(string vhdPath)
        {
            return (ReturnCode)Invoke("SolidCP.Server.VirtualizationServer2012", "UnmountVirtualHardDisk", vhdPath);
        }

        public async System.Threading.Tasks.Task<ReturnCode> UnmountVirtualHardDiskAsync(string vhdPath)
        {
            return await InvokeAsync<ReturnCode>("SolidCP.Server.VirtualizationServer2012", "UnmountVirtualHardDisk", vhdPath);
        }

        public JobResult ExpandVirtualHardDisk(string vhdPath, UInt64 sizeGB)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer2012", "ExpandVirtualHardDisk", vhdPath, sizeGB);
        }

        public async System.Threading.Tasks.Task<JobResult> ExpandVirtualHardDiskAsync(string vhdPath, UInt64 sizeGB)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer2012", "ExpandVirtualHardDisk", vhdPath, sizeGB);
        }

        public JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, VirtualHardDiskType diskType, uint blockSizeBytes)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer2012", "ConvertVirtualHardDisk", sourcePath, destinationPath, diskType, blockSizeBytes);
        }

        public async System.Threading.Tasks.Task<JobResult> ConvertVirtualHardDiskAsync(string sourcePath, string destinationPath, VirtualHardDiskType diskType, uint blockSizeBytes)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer2012", "ConvertVirtualHardDisk", sourcePath, destinationPath, diskType, blockSizeBytes);
        }

        public JobResult CreateVirtualHardDisk(string destinationPath, VirtualHardDiskType diskType, uint blockSizeBytes, UInt64 sizeGB)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer2012", "CreateVirtualHardDisk", destinationPath, diskType, blockSizeBytes, sizeGB);
        }

        public async System.Threading.Tasks.Task<JobResult> CreateVirtualHardDiskAsync(string destinationPath, VirtualHardDiskType diskType, uint blockSizeBytes, UInt64 sizeGB)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer2012", "CreateVirtualHardDisk", destinationPath, diskType, blockSizeBytes, sizeGB);
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
            return (string)Invoke("SolidCP.Server.VirtualizationServer2012", "ReadRemoteFile", path);
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

        public ConcreteJob GetJob(string jobId)
        {
            return (ConcreteJob)Invoke("SolidCP.Server.VirtualizationServer2012", "GetJob", jobId);
        }

        public async System.Threading.Tasks.Task<ConcreteJob> GetJobAsync(string jobId)
        {
            return await InvokeAsync<ConcreteJob>("SolidCP.Server.VirtualizationServer2012", "GetJob", jobId);
        }

        public List<ConcreteJob> GetAllJobs()
        {
            return (List<ConcreteJob>)Invoke("SolidCP.Server.VirtualizationServer2012", "GetAllJobs");
        }

        public async System.Threading.Tasks.Task<List<ConcreteJob>> GetAllJobsAsync()
        {
            return await InvokeAsync<List<ConcreteJob>>("SolidCP.Server.VirtualizationServer2012", "GetAllJobs");
        }

        public void ClearOldJobs()
        {
            Invoke("SolidCP.Server.VirtualizationServer2012", "ClearOldJobs");
        }

        public async System.Threading.Tasks.Task ClearOldJobsAsync()
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServer2012", "ClearOldJobs");
        }

        public ChangeJobStateReturnCode ChangeJobState(string jobId, ConcreteJobRequestedState newState)
        {
            return (ChangeJobStateReturnCode)Invoke("SolidCP.Server.VirtualizationServer2012", "ChangeJobState", jobId, newState);
        }

        public async System.Threading.Tasks.Task<ChangeJobStateReturnCode> ChangeJobStateAsync(string jobId, ConcreteJobRequestedState newState)
        {
            return await InvokeAsync<ChangeJobStateReturnCode>("SolidCP.Server.VirtualizationServer2012", "ChangeJobState", jobId, newState);
        }

        public int GetProcessorCoresNumber()
        {
            return (int)Invoke("SolidCP.Server.VirtualizationServer2012", "GetProcessorCoresNumber");
        }

        public async System.Threading.Tasks.Task<int> GetProcessorCoresNumberAsync()
        {
            return await InvokeAsync<int>("SolidCP.Server.VirtualizationServer2012", "GetProcessorCoresNumber");
        }

        public List<VMConfigurationVersion> GetVMConfigurationVersionSupportedList()
        {
            return (List<VMConfigurationVersion>)Invoke("SolidCP.Server.VirtualizationServer2012", "GetVMConfigurationVersionSupportedList");
        }

        public async System.Threading.Tasks.Task<List<VMConfigurationVersion>> GetVMConfigurationVersionSupportedListAsync()
        {
            return await InvokeAsync<List<VMConfigurationVersion>>("SolidCP.Server.VirtualizationServer2012", "GetVMConfigurationVersionSupportedList");
        }

        public List<CertificateInfo> GetCertificates(string remoteServer)
        {
            return (List<CertificateInfo>)Invoke("SolidCP.Server.VirtualizationServer2012", "GetCertificates", remoteServer);
        }

        public async System.Threading.Tasks.Task<List<CertificateInfo>> GetCertificatesAsync(string remoteServer)
        {
            return await InvokeAsync<List<CertificateInfo>>("SolidCP.Server.VirtualizationServer2012", "GetCertificates", remoteServer);
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

        public ReplicationServerInfo GetReplicaServer(string remoteServer)
        {
            return (ReplicationServerInfo)Invoke("SolidCP.Server.VirtualizationServer2012", "GetReplicaServer", remoteServer);
        }

        public async System.Threading.Tasks.Task<ReplicationServerInfo> GetReplicaServerAsync(string remoteServer)
        {
            return await InvokeAsync<ReplicationServerInfo>("SolidCP.Server.VirtualizationServer2012", "GetReplicaServer", remoteServer);
        }

        public void EnableVmReplication(string vmId, string replicaServer, VmReplication replication)
        {
            Invoke("SolidCP.Server.VirtualizationServer2012", "EnableVmReplication", vmId, replicaServer, replication);
        }

        public async System.Threading.Tasks.Task EnableVmReplicationAsync(string vmId, string replicaServer, VmReplication replication)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServer2012", "EnableVmReplication", vmId, replicaServer, replication);
        }

        public void SetVmReplication(string vmId, string replicaServer, VmReplication replication)
        {
            Invoke("SolidCP.Server.VirtualizationServer2012", "SetVmReplication", vmId, replicaServer, replication);
        }

        public async System.Threading.Tasks.Task SetVmReplicationAsync(string vmId, string replicaServer, VmReplication replication)
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

        public VmReplication GetReplication(string vmId)
        {
            return (VmReplication)Invoke("SolidCP.Server.VirtualizationServer2012", "GetReplication", vmId);
        }

        public async System.Threading.Tasks.Task<VmReplication> GetReplicationAsync(string vmId)
        {
            return await InvokeAsync<VmReplication>("SolidCP.Server.VirtualizationServer2012", "GetReplication", vmId);
        }

        public void DisableVmReplication(string vmId)
        {
            Invoke("SolidCP.Server.VirtualizationServer2012", "DisableVmReplication", vmId);
        }

        public async System.Threading.Tasks.Task DisableVmReplicationAsync(string vmId)
        {
            await InvokeAsync("SolidCP.Server.VirtualizationServer2012", "DisableVmReplication", vmId);
        }

        public ReplicationDetailInfo GetReplicationInfo(string vmId)
        {
            return (ReplicationDetailInfo)Invoke("SolidCP.Server.VirtualizationServer2012", "GetReplicationInfo", vmId);
        }

        public async System.Threading.Tasks.Task<ReplicationDetailInfo> GetReplicationInfoAsync(string vmId)
        {
            return await InvokeAsync<ReplicationDetailInfo>("SolidCP.Server.VirtualizationServer2012", "GetReplicationInfo", vmId);
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

        public JobResult ExecuteCustomPsScript(string script)
        {
            return (JobResult)Invoke("SolidCP.Server.VirtualizationServer2012", "ExecuteCustomPsScript", script);
        }

        public async System.Threading.Tasks.Task<JobResult> ExecuteCustomPsScriptAsync(string script)
        {
            return await InvokeAsync<JobResult>("SolidCP.Server.VirtualizationServer2012", "ExecuteCustomPsScript", script);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class VirtualizationServer2012 : SolidCP.Web.Client.ClientBase<IVirtualizationServer2012, VirtualizationServer2012AssemblyClient>, IVirtualizationServer2012
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

        public JobResult ChangeVirtualMachineState(string vmId, VirtualMachineRequestedState newState, string clusterName)
        {
            return base.Client.ChangeVirtualMachineState(vmId, newState, clusterName);
        }

        public async System.Threading.Tasks.Task<JobResult> ChangeVirtualMachineStateAsync(string vmId, VirtualMachineRequestedState newState, string clusterName)
        {
            return await base.Client.ChangeVirtualMachineStateAsync(vmId, newState, clusterName);
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

        public JobResult RenameVirtualMachine(string vmId, string name, string clusterName)
        {
            return base.Client.RenameVirtualMachine(vmId, name, clusterName);
        }

        public async System.Threading.Tasks.Task<JobResult> RenameVirtualMachineAsync(string vmId, string name, string clusterName)
        {
            return await base.Client.RenameVirtualMachineAsync(vmId, name, clusterName);
        }

        public JobResult DeleteVirtualMachine(string vmId, string clusterName)
        {
            return base.Client.DeleteVirtualMachine(vmId, clusterName);
        }

        public async System.Threading.Tasks.Task<JobResult> DeleteVirtualMachineAsync(string vmId, string clusterName)
        {
            return await base.Client.DeleteVirtualMachineAsync(vmId, clusterName);
        }

        public JobResult DeleteVirtualMachineExtended(string vmId, string clusterName)
        {
            return base.Client.DeleteVirtualMachineExtended(vmId, clusterName);
        }

        public async System.Threading.Tasks.Task<JobResult> DeleteVirtualMachineExtendedAsync(string vmId, string clusterName)
        {
            return await base.Client.DeleteVirtualMachineExtendedAsync(vmId, clusterName);
        }

        public JobResult ExportVirtualMachine(string vmId, string exportPath)
        {
            return base.Client.ExportVirtualMachine(vmId, exportPath);
        }

        public async System.Threading.Tasks.Task<JobResult> ExportVirtualMachineAsync(string vmId, string exportPath)
        {
            return await base.Client.ExportVirtualMachineAsync(vmId, exportPath);
        }

        public bool IsTryToUpdateVirtualMachineWithoutRebootSuccess(VirtualMachine vm)
        {
            return base.Client.IsTryToUpdateVirtualMachineWithoutRebootSuccess(vm);
        }

        public async System.Threading.Tasks.Task<bool> IsTryToUpdateVirtualMachineWithoutRebootSuccessAsync(VirtualMachine vm)
        {
            return await base.Client.IsTryToUpdateVirtualMachineWithoutRebootSuccessAsync(vm);
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

        public List<SecureBootTemplate> GetSecureBootTemplates(string computerName)
        {
            return base.Client.GetSecureBootTemplates(computerName);
        }

        public async System.Threading.Tasks.Task<List<SecureBootTemplate>> GetSecureBootTemplatesAsync(string computerName)
        {
            return await base.Client.GetSecureBootTemplatesAsync(computerName);
        }

        public List<VirtualSwitch> GetExternalSwitches(string computerName)
        {
            return base.Client.GetExternalSwitches(computerName);
        }

        public async System.Threading.Tasks.Task<List<VirtualSwitch>> GetExternalSwitchesAsync(string computerName)
        {
            return await base.Client.GetExternalSwitchesAsync(computerName);
        }

        public List<VirtualSwitch> GetExternalSwitchesWMI(string computerName)
        {
            return base.Client.GetExternalSwitchesWMI(computerName);
        }

        public async System.Threading.Tasks.Task<List<VirtualSwitch>> GetExternalSwitchesWMIAsync(string computerName)
        {
            return await base.Client.GetExternalSwitchesWMIAsync(computerName);
        }

        public List<VirtualSwitch> GetInternalSwitches(string computerName)
        {
            return base.Client.GetInternalSwitches(computerName);
        }

        public async System.Threading.Tasks.Task<List<VirtualSwitch>> GetInternalSwitchesAsync(string computerName)
        {
            return await base.Client.GetInternalSwitchesAsync(computerName);
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

        public List<VirtualMachineNetworkAdapter> GetVirtualMachinesNetwordAdapterSettings(string vmName)
        {
            return base.Client.GetVirtualMachinesNetwordAdapterSettings(vmName);
        }

        public async System.Threading.Tasks.Task<List<VirtualMachineNetworkAdapter>> GetVirtualMachinesNetwordAdapterSettingsAsync(string vmName)
        {
            return await base.Client.GetVirtualMachinesNetwordAdapterSettingsAsync(vmName);
        }

        public JobResult InjectIPs(string vmId, GuestNetworkAdapterConfiguration guestNetworkAdapterConfiguration)
        {
            return base.Client.InjectIPs(vmId, guestNetworkAdapterConfiguration);
        }

        public async System.Threading.Tasks.Task<JobResult> InjectIPsAsync(string vmId, GuestNetworkAdapterConfiguration guestNetworkAdapterConfiguration)
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

        public JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, VirtualHardDiskType diskType, uint blockSizeBytes)
        {
            return base.Client.ConvertVirtualHardDisk(sourcePath, destinationPath, diskType, blockSizeBytes);
        }

        public async System.Threading.Tasks.Task<JobResult> ConvertVirtualHardDiskAsync(string sourcePath, string destinationPath, VirtualHardDiskType diskType, uint blockSizeBytes)
        {
            return await base.Client.ConvertVirtualHardDiskAsync(sourcePath, destinationPath, diskType, blockSizeBytes);
        }

        public JobResult CreateVirtualHardDisk(string destinationPath, VirtualHardDiskType diskType, uint blockSizeBytes, UInt64 sizeGB)
        {
            return base.Client.CreateVirtualHardDisk(destinationPath, diskType, blockSizeBytes, sizeGB);
        }

        public async System.Threading.Tasks.Task<JobResult> CreateVirtualHardDiskAsync(string destinationPath, VirtualHardDiskType diskType, uint blockSizeBytes, UInt64 sizeGB)
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

        public void ClearOldJobs()
        {
            base.Client.ClearOldJobs();
        }

        public async System.Threading.Tasks.Task ClearOldJobsAsync()
        {
            await base.Client.ClearOldJobsAsync();
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

        public List<VMConfigurationVersion> GetVMConfigurationVersionSupportedList()
        {
            return base.Client.GetVMConfigurationVersionSupportedList();
        }

        public async System.Threading.Tasks.Task<List<VMConfigurationVersion>> GetVMConfigurationVersionSupportedListAsync()
        {
            return await base.Client.GetVMConfigurationVersionSupportedListAsync();
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

        public JobResult ExecuteCustomPsScript(string script)
        {
            return base.Client.ExecuteCustomPsScript(script);
        }

        public async System.Threading.Tasks.Task<JobResult> ExecuteCustomPsScriptAsync(string script)
        {
            return await base.Client.ExecuteCustomPsScriptAsync(script);
        }
    }
}
#endif