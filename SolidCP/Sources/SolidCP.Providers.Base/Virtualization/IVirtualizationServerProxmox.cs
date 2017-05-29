// Copyright (c) 2017, Centron GmbH
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2014, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.Providers.Virtualization
{
    public interface IVirtualizationServerProxmox
    {
        // Virtual Machines
        VirtualMachine GetVirtualMachine(string vmId);
        VirtualMachine GetVirtualMachineEx(string vmId);
        List<VirtualMachine> GetVirtualMachines();
        byte[] GetVirtualMachineThumbnailImage(string vmId, ThumbnailSize size);
        VirtualMachine CreateVirtualMachine(VirtualMachine vm);
        VirtualMachine UpdateVirtualMachine(VirtualMachine vm);
        JobResult ChangeVirtualMachineState(string vmId, VirtualMachineRequestedState newState);
        ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason);
        List<ConcreteJob> GetVirtualMachineJobs(string vmId);
        JobResult RenameVirtualMachine(string vmId, string name);
        JobResult ExportVirtualMachine(string vmId, string exportPath);
        JobResult DeleteVirtualMachine(string vmId);
        string GetVirtualMachineVNC(string vmId);

        // Snapshots
        List<VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId);
        VirtualMachineSnapshot GetSnapshot(string vmId, string snapshotId);
        JobResult CreateSnapshot(string vmId);
        JobResult RenameSnapshot(string vmId, string snapshotId, string name);
        JobResult ApplySnapshot(string vmId, string snapshotId);
        JobResult DeleteSnapshot(string vmId, string snapshotId);
        JobResult DeleteSnapshotSubtree(string vmId, string snapshotId);
        byte[] GetSnapshotThumbnailImage(string snapshotId, ThumbnailSize size);

        // Virtual Switches
        List<VirtualSwitch> GetExternalSwitches(string computerName);
        List<VirtualSwitch> GetSwitches();
        bool SwitchExists(string switchId);
        VirtualSwitch CreateSwitch(string name);
        ReturnCode DeleteSwitch(string switchId);

        // DVD operations
        LibraryItem[] GetDVDISOs(string vmId);
        string GetInsertedDVD(string vmId);
        JobResult InsertDVD(string vmId, string isoPath);
        JobResult EjectDVD(string vmId);

        // KVP items
        List<KvpExchangeDataItem> GetKVPItems(string vmId);
        List<KvpExchangeDataItem> GetStandardKVPItems(string vmId);
        JobResult AddKVPItems(string vmId, KvpExchangeDataItem[] items);
        JobResult RemoveKVPItems(string vmId, string[] itemNames);
        JobResult ModifyKVPItems(string vmId, KvpExchangeDataItem[] items);

        // Storage
        VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath);
        MountedDiskInfo MountVirtualHardDisk(string vhdPath);
        ReturnCode UnmountVirtualHardDisk(string vhdPath);
        JobResult ExpandVirtualHardDisk(string vhdPath, UInt64 sizeGB);
        JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, VirtualHardDiskType diskType);
        void ExpandDiskVolume(string diskAddress, string volumeName);
        void DeleteRemoteFile(string path);
        string ReadRemoteFile(string path);
        void WriteRemoteFile(string path, string content);

        // Jobs
        ConcreteJob GetJob(string jobId);
        List<ConcreteJob> GetAllJobs();
        ChangeJobStateReturnCode ChangeJobState(string jobId, ConcreteJobRequestedState newState);

        // Configuration
        int GetProcessorCoresNumber();

        // Replication 
        List<CertificateInfo> GetCertificates(string remoteServer);
        void SetReplicaServer(string remoteServer, string thumbprint, string storagePath);
        void UnsetReplicaServer(string remoteServer);
        ReplicationServerInfo GetReplicaServer(string remoteServer);
        void EnableVmReplication(string vmId, string replicaServer, VmReplication replication);
        void SetVmReplication(string vmId, string replicaServer, VmReplication replication);
        void TestReplicationServer(string vmId, string replicaServer, string localThumbprint);
        void StartInitialReplication(string vmId);
        VmReplication GetReplication(string vmId);
        void DisableVmReplication(string vmId);
        ReplicationDetailInfo GetReplicationInfo(string vmId);
        void PauseReplication(string vmId);
        void ResumeReplication(string vmId);
    }
}
