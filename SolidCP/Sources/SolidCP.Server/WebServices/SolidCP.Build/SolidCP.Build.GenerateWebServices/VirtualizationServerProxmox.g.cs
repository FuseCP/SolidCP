#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
//using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;
using System.IO;
using SolidCP.Providers;
using SolidCP.Providers.Virtualization;
using SolidCP.Server.Utils;
using System.Collections.Generic;
using SolidCP.Server;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/server/")]
    public interface IVirtualizationServerProxmox
    {
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        VirtualMachine GetVirtualMachine(string vmId);
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
        VirtualMachine CreateVirtualMachine(VirtualMachine vm);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        VirtualMachine UpdateVirtualMachine(VirtualMachine vm);
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
        JobResult ExportVirtualMachine(string vmId, string exportPath);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string GetVirtualMachineVNC(string vmId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        VirtualMachineSnapshot GetSnapshot(string vmId, string snapshotId);
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
        JobResult DeleteSnapshotSubtree(string vmId, string snapshotId);
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
        LibraryItem[] GetDVDISOs(string vmId);
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
        int GetProcessorCoresNumber();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<CertificateInfo> GetCertificates(string remoteServer);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetReplicaServer(string remoteServer, string thumbprint, string storagePath);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UnsetReplicaServer(string remoteServer);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ReplicationServerInfo GetReplicaServer(string remoteServer);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void EnableVmReplication(string vmId, string replicaServer, VmReplication replication);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetVmReplication(string vmId, string replicaServer, VmReplication replication);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void TestReplicationServer(string vmId, string replicaServer, string localThumbprint);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void StartInitialReplication(string vmId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        VmReplication GetReplication(string vmId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DisableVmReplication(string vmId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ReplicationDetailInfo GetReplicationInfo(string vmId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void PauseReplication(string vmId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ResumeReplication(string vmId);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class VirtualizationServerProxmox : SolidCP.Server.VirtualizationServerProxmox, IVirtualizationServerProxmox
    {
        public new VirtualMachine GetVirtualMachine(string vmId)
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

        public new VirtualMachine CreateVirtualMachine(VirtualMachine vm)
        {
            return base.CreateVirtualMachine(vm);
        }

        public new VirtualMachine UpdateVirtualMachine(VirtualMachine vm)
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

        public new JobResult ExportVirtualMachine(string vmId, string exportPath)
        {
            return base.ExportVirtualMachine(vmId, exportPath);
        }

        public new string GetVirtualMachineVNC(string vmId)
        {
            return base.GetVirtualMachineVNC(vmId);
        }

        public new List<VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId)
        {
            return base.GetVirtualMachineSnapshots(vmId);
        }

        public new VirtualMachineSnapshot GetSnapshot(string vmId, string snapshotId)
        {
            return base.GetSnapshot(vmId, snapshotId);
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

        public new JobResult DeleteSnapshotSubtree(string vmId, string snapshotId)
        {
            return base.DeleteSnapshotSubtree(vmId, snapshotId);
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

        public new LibraryItem[] GetDVDISOs(string vmId)
        {
            return base.GetDVDISOs(vmId);
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

        public new int GetProcessorCoresNumber()
        {
            return base.GetProcessorCoresNumber();
        }

        public new List<CertificateInfo> GetCertificates(string remoteServer)
        {
            return base.GetCertificates(remoteServer);
        }

        public new void SetReplicaServer(string remoteServer, string thumbprint, string storagePath)
        {
            base.SetReplicaServer(remoteServer, thumbprint, storagePath);
        }

        public new void UnsetReplicaServer(string remoteServer)
        {
            base.UnsetReplicaServer(remoteServer);
        }

        public new ReplicationServerInfo GetReplicaServer(string remoteServer)
        {
            return base.GetReplicaServer(remoteServer);
        }

        public new void EnableVmReplication(string vmId, string replicaServer, VmReplication replication)
        {
            base.EnableVmReplication(vmId, replicaServer, replication);
        }

        public new void SetVmReplication(string vmId, string replicaServer, VmReplication replication)
        {
            base.SetVmReplication(vmId, replicaServer, replication);
        }

        public new void TestReplicationServer(string vmId, string replicaServer, string localThumbprint)
        {
            base.TestReplicationServer(vmId, replicaServer, localThumbprint);
        }

        public new void StartInitialReplication(string vmId)
        {
            base.StartInitialReplication(vmId);
        }

        public new VmReplication GetReplication(string vmId)
        {
            return base.GetReplication(vmId);
        }

        public new void DisableVmReplication(string vmId)
        {
            base.DisableVmReplication(vmId);
        }

        public new ReplicationDetailInfo GetReplicationInfo(string vmId)
        {
            return base.GetReplicationInfo(vmId);
        }

        public new void PauseReplication(string vmId)
        {
            base.PauseReplication(vmId);
        }

        public new void ResumeReplication(string vmId)
        {
            base.ResumeReplication(vmId);
        }
    }
}
#endif