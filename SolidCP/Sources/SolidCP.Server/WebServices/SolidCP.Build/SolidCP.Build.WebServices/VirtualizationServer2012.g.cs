#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers;
using SolidCP.Providers.Virtualization;
using SolidCP.Server.Utils;
using System.Collections.Generic;
using SolidCP.Server;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/server/")]
    public interface IVirtualizationServer2012
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
        JobResult ChangeVirtualMachineState(string vmId, VirtualMachineRequestedState newState, string clusterName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<ConcreteJob> GetVirtualMachineJobs(string vmId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        JobResult RenameVirtualMachine(string vmId, string name, string clusterName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        JobResult DeleteVirtualMachine(string vmId, string clusterName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        JobResult DeleteVirtualMachineExtended(string vmId, string clusterName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        JobResult ExportVirtualMachine(string vmId, string exportPath);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool IsTryToUpdateVirtualMachineWithoutRebootSuccess(VirtualMachine vm);
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
        JobResult DeleteSnapshot(string snapshotId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        JobResult DeleteSnapshotSubtree(string snapshotId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        byte[] GetSnapshotThumbnailImage(string snapshotId, ThumbnailSize size);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<SecureBootTemplate> GetSecureBootTemplates(string computerName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<VirtualSwitch> GetExternalSwitches(string computerName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<VirtualSwitch> GetExternalSwitchesWMI(string computerName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<VirtualSwitch> GetInternalSwitches(string computerName);
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
        List<VirtualMachineNetworkAdapter> GetVirtualMachinesNetwordAdapterSettings(string vmName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        JobResult InjectIPs(string vmId, GuestNetworkAdapterConfiguration guestNetworkAdapterConfiguration);
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
        bool IsEmptyFolders(string path);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool FileExists(string path);
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
        JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, VirtualHardDiskType diskType, uint blockSizeBytes);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        JobResult CreateVirtualHardDisk(string destinationPath, VirtualHardDiskType diskType, uint blockSizeBytes, UInt64 sizeGB);
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
        void ClearOldJobs();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ChangeJobStateReturnCode ChangeJobState(string jobId, ConcreteJobRequestedState newState);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        int GetProcessorCoresNumber();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<VMConfigurationVersion> GetVMConfigurationVersionSupportedList();
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
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        JobResult ExecuteCustomPsScript(string script);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class VirtualizationServer2012 : SolidCP.Server.VirtualizationServer2012, IVirtualizationServer2012
    {
    }
}
#endif