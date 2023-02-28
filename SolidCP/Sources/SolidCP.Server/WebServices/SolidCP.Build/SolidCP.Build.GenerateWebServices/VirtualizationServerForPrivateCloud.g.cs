#if !Client
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

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [ServiceContract]
    public interface IVirtualizationServerForPrivateCloud
    {
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        VMInfo GetVirtualMachine(string vmId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        VirtualMachine GetVirtualMachineEx(string vmId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<VirtualMachine> GetVirtualMachines();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        byte[] GetVirtualMachineThumbnailImage(string vmId, ThumbnailSize size);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        VMInfo CreateVirtualMachine(VMInfo vm);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        VMInfo CreateVMFromVM(string sourceName, VMInfo vmTemplate, Guid taskGuid);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        VMInfo UpdateVirtualMachine(VMInfo vm);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        JobResult ChangeVirtualMachineState(string vmId, VirtualMachineRequestedState newState);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<ConcreteJob> GetVirtualMachineJobs(string vmId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        JobResult RenameVirtualMachine(string vmId, string name);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        JobResult DeleteVirtualMachine(string vmId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        VirtualMachineSnapshot GetSnapshot(string snapshotId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        JobResult CreateSnapshot(string vmId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        JobResult RenameSnapshot(string vmId, string snapshotId, string name);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        JobResult ApplySnapshot(string vmId, string snapshotId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        JobResult DeleteSnapshot(string vmId, string snapshotId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        JobResult DeleteSnapshotSubtree(string snapshotId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        byte[] GetSnapshotThumbnailImage(string snapshotId, ThumbnailSize size);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<VirtualSwitch> GetExternalSwitches(string computerName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<VirtualSwitch> GetSwitches();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool SwitchExists(string switchId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        VirtualSwitch CreateSwitch(string name);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ReturnCode DeleteSwitch(string switchId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string GetInsertedDVD(string vmId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        JobResult InsertDVD(string vmId, string isoPath);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        JobResult EjectDVD(string vmId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        LibraryItem[] GetLibraryItems(string path);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        LibraryItem[] GetOSLibraryItems();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        LibraryItem[] GetHosts();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        LibraryItem[] GetClusters();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<KvpExchangeDataItem> GetKVPItems(string vmId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<KvpExchangeDataItem> GetStandardKVPItems(string vmId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        JobResult AddKVPItems(string vmId, KvpExchangeDataItem[] items);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        JobResult RemoveKVPItems(string vmId, string[] itemNames);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        JobResult ModifyKVPItems(string vmId, KvpExchangeDataItem[] items);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        MountedDiskInfo MountVirtualHardDisk(string vhdPath);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ReturnCode UnmountVirtualHardDisk(string vhdPath);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        JobResult ExpandVirtualHardDisk(string vhdPath, UInt64 sizeGB);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, VirtualHardDiskType diskType);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteRemoteFile(string path);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ExpandDiskVolume(string diskAddress, string volumeName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string ReadRemoteFile(string path);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void WriteRemoteFile(string path, string content);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ConcreteJob GetJob(string jobId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<ConcreteJob> GetAllJobs();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ChangeJobStateReturnCode ChangeJobState(string jobId, ConcreteJobRequestedState newState);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        int GetProcessorCoresNumber(string templateId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool CheckServerState(VMForPCSettingsName control, string connString, string connName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<MonitoredObjectEvent> GetDeviceEvents(string serviceName, string displayName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<MonitoredObjectAlert> GetMonitoringAlerts(string serviceName, string virtualMachineName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<PerformanceDataValue> GetPerfomanceValue(string VmName, PerformanceType perf, DateTime startPeriod, DateTime endPeriod);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ConfigureCreatedVMNetworkAdapters(VMInfo vmInfo);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        VMInfo MoveVM(VMInfo vmForMove);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        VirtualNetworkInfo[] GetVirtualNetworkByHostName(string hostName);
    }

    // wcf service
    public class VirtualizationServerForPrivateCloudService : VirtualizationServerForPrivateCloud, IVirtualizationServerForPrivateCloud
    {
        public new VMInfo GetVirtualMachine(string vmId)
        {
            return base.GetVirtualMachine(vmId);
        }

        public new VirtualMachine GetVirtualMachineEx(string vmId)
        {
            return base.GetVirtualMachineEx(vmId);
        }

        public new List<VirtualMachine> GetVirtualMachines()
        {
            return base.GetVirtualMachines();
        }

        public new byte[] GetVirtualMachineThumbnailImage(string vmId, ThumbnailSize size)
        {
            return base.GetVirtualMachineThumbnailImage(vmId, size);
        }

        public new VMInfo CreateVirtualMachine(VMInfo vm)
        {
            return base.CreateVirtualMachine(vm);
        }

        public new VMInfo CreateVMFromVM(string sourceName, VMInfo vmTemplate, Guid taskGuid)
        {
            return base.CreateVMFromVM(sourceName, vmTemplate, taskGuid);
        }

        public new VMInfo UpdateVirtualMachine(VMInfo vm)
        {
            return base.UpdateVirtualMachine(vm);
        }

        public new JobResult ChangeVirtualMachineState(string vmId, VirtualMachineRequestedState newState)
        {
            return base.ChangeVirtualMachineState(vmId, newState);
        }

        public new ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason)
        {
            return base.ShutDownVirtualMachine(vmId, force, reason);
        }

        public new List<ConcreteJob> GetVirtualMachineJobs(string vmId)
        {
            return base.GetVirtualMachineJobs(vmId);
        }

        public new JobResult RenameVirtualMachine(string vmId, string name)
        {
            return base.RenameVirtualMachine(vmId, name);
        }

        public new JobResult DeleteVirtualMachine(string vmId)
        {
            return base.DeleteVirtualMachine(vmId);
        }

        public new List<VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId)
        {
            return base.GetVirtualMachineSnapshots(vmId);
        }

        public new VirtualMachineSnapshot GetSnapshot(string snapshotId)
        {
            return base.GetSnapshot(snapshotId);
        }

        public new JobResult CreateSnapshot(string vmId)
        {
            return base.CreateSnapshot(vmId);
        }

        public new JobResult RenameSnapshot(string vmId, string snapshotId, string name)
        {
            return base.RenameSnapshot(vmId, snapshotId, name);
        }

        public new JobResult ApplySnapshot(string vmId, string snapshotId)
        {
            return base.ApplySnapshot(vmId, snapshotId);
        }

        public new JobResult DeleteSnapshot(string vmId, string snapshotId)
        {
            return base.DeleteSnapshot(vmId, snapshotId);
        }

        public new JobResult DeleteSnapshotSubtree(string snapshotId)
        {
            return base.DeleteSnapshotSubtree(snapshotId);
        }

        public new byte[] GetSnapshotThumbnailImage(string snapshotId, ThumbnailSize size)
        {
            return base.GetSnapshotThumbnailImage(snapshotId, size);
        }

        public new List<VirtualSwitch> GetExternalSwitches(string computerName)
        {
            return base.GetExternalSwitches(computerName);
        }

        public new List<VirtualSwitch> GetSwitches()
        {
            return base.GetSwitches();
        }

        public new bool SwitchExists(string switchId)
        {
            return base.SwitchExists(switchId);
        }

        public new VirtualSwitch CreateSwitch(string name)
        {
            return base.CreateSwitch(name);
        }

        public new ReturnCode DeleteSwitch(string switchId)
        {
            return base.DeleteSwitch(switchId);
        }

        public new string GetInsertedDVD(string vmId)
        {
            return base.GetInsertedDVD(vmId);
        }

        public new JobResult InsertDVD(string vmId, string isoPath)
        {
            return base.InsertDVD(vmId, isoPath);
        }

        public new JobResult EjectDVD(string vmId)
        {
            return base.EjectDVD(vmId);
        }

        public new LibraryItem[] GetLibraryItems(string path)
        {
            return base.GetLibraryItems(path);
        }

        public new LibraryItem[] GetOSLibraryItems()
        {
            return base.GetOSLibraryItems();
        }

        public new LibraryItem[] GetHosts()
        {
            return base.GetHosts();
        }

        public new LibraryItem[] GetClusters()
        {
            return base.GetClusters();
        }

        public new List<KvpExchangeDataItem> GetKVPItems(string vmId)
        {
            return base.GetKVPItems(vmId);
        }

        public new List<KvpExchangeDataItem> GetStandardKVPItems(string vmId)
        {
            return base.GetStandardKVPItems(vmId);
        }

        public new JobResult AddKVPItems(string vmId, KvpExchangeDataItem[] items)
        {
            return base.AddKVPItems(vmId, items);
        }

        public new JobResult RemoveKVPItems(string vmId, string[] itemNames)
        {
            return base.RemoveKVPItems(vmId, itemNames);
        }

        public new JobResult ModifyKVPItems(string vmId, KvpExchangeDataItem[] items)
        {
            return base.ModifyKVPItems(vmId, items);
        }

        public new VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath)
        {
            return base.GetVirtualHardDiskInfo(vhdPath);
        }

        public new MountedDiskInfo MountVirtualHardDisk(string vhdPath)
        {
            return base.MountVirtualHardDisk(vhdPath);
        }

        public new ReturnCode UnmountVirtualHardDisk(string vhdPath)
        {
            return base.UnmountVirtualHardDisk(vhdPath);
        }

        public new JobResult ExpandVirtualHardDisk(string vhdPath, UInt64 sizeGB)
        {
            return base.ExpandVirtualHardDisk(vhdPath, sizeGB);
        }

        public new JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, VirtualHardDiskType diskType)
        {
            return base.ConvertVirtualHardDisk(sourcePath, destinationPath, diskType);
        }

        public new void DeleteRemoteFile(string path)
        {
            base.DeleteRemoteFile(path);
        }

        public new void ExpandDiskVolume(string diskAddress, string volumeName)
        {
            base.ExpandDiskVolume(diskAddress, volumeName);
        }

        public new string ReadRemoteFile(string path)
        {
            return base.ReadRemoteFile(path);
        }

        public new void WriteRemoteFile(string path, string content)
        {
            base.WriteRemoteFile(path, content);
        }

        public new ConcreteJob GetJob(string jobId)
        {
            return base.GetJob(jobId);
        }

        public new List<ConcreteJob> GetAllJobs()
        {
            return base.GetAllJobs();
        }

        public new ChangeJobStateReturnCode ChangeJobState(string jobId, ConcreteJobRequestedState newState)
        {
            return base.ChangeJobState(jobId, newState);
        }

        public new int GetProcessorCoresNumber(string templateId)
        {
            return base.GetProcessorCoresNumber(templateId);
        }

        public new bool CheckServerState(VMForPCSettingsName control, string connString, string connName)
        {
            return base.CheckServerState(control, connString, connName);
        }

        public new List<MonitoredObjectEvent> GetDeviceEvents(string serviceName, string displayName)
        {
            return base.GetDeviceEvents(serviceName, displayName);
        }

        public new List<MonitoredObjectAlert> GetMonitoringAlerts(string serviceName, string virtualMachineName)
        {
            return base.GetMonitoringAlerts(serviceName, virtualMachineName);
        }

        public new List<PerformanceDataValue> GetPerfomanceValue(string VmName, PerformanceType perf, DateTime startPeriod, DateTime endPeriod)
        {
            return base.GetPerfomanceValue(VmName, perf, startPeriod, endPeriod);
        }

        public new void ConfigureCreatedVMNetworkAdapters(VMInfo vmInfo)
        {
            base.ConfigureCreatedVMNetworkAdapters(vmInfo);
        }

        public new VMInfo MoveVM(VMInfo vmForMove)
        {
            return base.MoveVM(vmForMove);
        }

        public new VirtualNetworkInfo[] GetVirtualNetworkByHostName(string hostName)
        {
            return base.GetVirtualNetworkByHostName(hostName);
        }
    }
}
#endif