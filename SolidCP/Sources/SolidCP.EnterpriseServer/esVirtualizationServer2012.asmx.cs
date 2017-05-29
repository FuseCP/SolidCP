// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
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

﻿using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;
using Microsoft.Web.Services3;
using SolidCP.Providers;
using SolidCP.Providers.Common;
using SolidCP.Providers.Virtualization;
using SolidCP.Providers.ResultObjects; 
using SolidCP.Providers.Virtualization;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esVirtualizationServer
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class esVirtualizationServer2012 : System.Web.Services.WebService
    {
        #region Virtual Machines
        [WebMethod]
        public VirtualMachineMetaItemsPaged GetVirtualMachines(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            return VirtualizationServerController2012.GetVirtualMachines(packageId,
                filterColumn, filterValue, sortColumn, startRow, maximumRows, recursive);
        }

        [WebMethod]
        public VirtualMachine[] GetVirtualMachinesByServiceId(int serviceId)
        {
            return VirtualizationServerController2012.GetVirtualMachinesByServiceId(serviceId);
        }

        [WebMethod]
        public VirtualMachine GetVirtualMachineItem(int itemId)
        {
            return VirtualizationServerController2012.GetVirtualMachineByItemId(itemId);
        }

        [WebMethod]
        public string EvaluateVirtualMachineTemplate(int itemId, string template)
        {
            if (SecurityContext.CheckAccount(DemandAccount.IsActive | DemandAccount.IsAdmin | DemandAccount.NotDemo) != 0)
                throw new Exception("This method could be called by serveradmin only.");

            return VirtualizationServerController2012.EvaluateVirtualMachineTemplate(itemId, false, false, template);
        }
        #endregion

        #region External Network
        [WebMethod]
        public NetworkAdapterDetails GetExternalNetworkDetails(int packageId)
        {
            return VirtualizationServerController2012.GetExternalNetworkDetails(packageId);
        }
        #endregion

        #region Private Network
        [WebMethod]
        public PrivateIPAddressesPaged GetPackagePrivateIPAddressesPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return VirtualizationServerController2012.GetPackagePrivateIPAddressesPaged(packageId,
                filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<PrivateIPAddress> GetPackagePrivateIPAddresses(int packageId)
        {
            return VirtualizationServerController2012.GetPackagePrivateIPAddresses(packageId);
        }

        [WebMethod]
        public NetworkAdapterDetails GetPrivateNetworkDetails(int packageId)
        {
            return VirtualizationServerController2012.GetPrivateNetworkDetails(packageId);
        }
        #endregion

        #region User Permissions
        [WebMethod]
        public List<VirtualMachinePermission> GetSpaceUserPermissions(int packageId)
        {
            return VirtualizationServerController2012.GetSpaceUserPermissions(packageId);
        }

        [WebMethod]
        public int UpdateSpaceUserPermissions(int packageId, VirtualMachinePermission[] permissions)
        {
            return VirtualizationServerController2012.UpdateSpaceUserPermissions(packageId, permissions);
        }
        #endregion

        #region Audit Log
        [WebMethod]
        public List<LogRecord> GetSpaceAuditLog(int packageId, DateTime startPeriod, DateTime endPeriod,
            int severity, string sortColumn, int startRow, int maximumRows)
        {
            return VirtualizationServerController2012.GetSpaceAuditLog(packageId, startPeriod, endPeriod,
                severity, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<LogRecord> GetVirtualMachineAuditLog(int itemId, DateTime startPeriod, DateTime endPeriod,
            int severity, string sortColumn, int startRow, int maximumRows)
        {
            return VirtualizationServerController2012.GetVirtualMachineAuditLog(itemId, startPeriod, endPeriod,
                severity, sortColumn, startRow, maximumRows);
        }
        #endregion

        #region VPS Create – Name & OS
        [WebMethod]
        public LibraryItem[] GetOperatingSystemTemplates(int packageId)
        {
            return VirtualizationServerController2012.GetOperatingSystemTemplates(packageId);
        }

        [WebMethod]
        public LibraryItem[] GetOperatingSystemTemplatesByServiceId(int serviceId)
        {
            return VirtualizationServerController2012.GetOperatingSystemTemplatesByServiceId(serviceId);
        }
        #endregion

        #region VNC
        [WebMethod]
        public string GetVirtualMachineGuacamoleURL(int itemId)
        {
            return VirtualizationServerController2012.GetVirtualMachineGuacamoleURL(itemId);
        }
        #endregion

        #region VPS Create - Configuration
        [WebMethod]
        public int GetMaximumCpuCoresNumber(int packageId)
        {
            return VirtualizationServerController2012.GetMaximumCpuCoresNumber(packageId);
        }

        [WebMethod]
        public string GetDefaultExportPath(int itemId)
        {
            return VirtualizationServerController2012.GetDefaultExportPath(itemId);
        }
        #endregion

        #region VPS Create
        [WebMethod]
        public IntResult CreateDefaultVirtualMachine(int packageId,
            string hostname, string osTemplate, string password, string summaryLetterEmail)
        {
            return VirtualizationServerController2012.CreateDefaultVirtualMachine(packageId, hostname, osTemplate, password, summaryLetterEmail);
        }

        [WebMethod]
        public IntResult CreateVirtualMachine(int packageId,
                string hostname, string osTemplateFile, string password, string summaryLetterEmail,
                int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock,
                bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed,
                bool externalNetworkEnabled, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses,
                bool privateNetworkEnabled, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses, VirtualMachine otherSettings)
        {
            return VirtualizationServerController2012.CreateVirtualMachine(packageId,
                hostname, osTemplateFile, password, summaryLetterEmail,
                cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock,
                startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed,
                externalNetworkEnabled, externalAddressesNumber, randomExternalAddresses, externalAddresses,
                privateNetworkEnabled, privateAddressesNumber, randomPrivateAddresses, privateAddresses,  otherSettings);
        }
        #endregion

        #region VPS - Import
        [WebMethod]
        public IntResult ImportVirtualMachine(int packageId,
            int serviceId, string vmId,
            string osTemplateFile, string adminPassword,
            bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed,
            string externalNicMacAddress, int[] externalAddresses,
            string managementNicMacAddress, int managementAddress)
        {
            return VirtualizationServerController2012.ImportVirtualMachine(packageId,
                serviceId, vmId,
                osTemplateFile, adminPassword,
                startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed,
                externalNicMacAddress, externalAddresses,
                managementNicMacAddress, managementAddress);
        }
        #endregion

        #region VPS – General
        [WebMethod]
        public byte[] GetVirtualMachineThumbnail(int itemId, ThumbnailSize size)
        {
            return VirtualizationServerController2012.GetVirtualMachineThumbnail(itemId, size);
        }

        [WebMethod]
        public VirtualMachine GetVirtualMachineGeneralDetails(int itemId)
        {
            return VirtualizationServerController2012.GetVirtualMachineGeneralDetails(itemId);
        }

        [WebMethod]
        public VirtualMachine GetVirtualMachineExtendedInfo(int serviceId, string vmId)
        {
            return VirtualizationServerController2012.GetVirtualMachineExtendedInfo(serviceId, vmId);
        }

        [WebMethod]
        public int CancelVirtualMachineJob(string jobId)
        {
            return VirtualizationServerController2012.CancelVirtualMachineJob(jobId);
        }

        [WebMethod]
        public ResultObject UpdateVirtualMachineHostName(int itemId, string hostname, bool updateNetBIOS)
        {
            return VirtualizationServerController2012.UpdateVirtualMachineHostName(itemId, hostname, updateNetBIOS);
        }

        [WebMethod]
        public ResultObject ChangeVirtualMachineState(int itemId, VirtualMachineRequestedState state)
        {
               return VirtualizationServerController2012.ChangeVirtualMachineStateExternal(itemId, state);    
        }


        [WebMethod]
        public List<ConcreteJob> GetVirtualMachineJobs(int itemId)
        {
            return VirtualizationServerController2012.GetVirtualMachineJobs(itemId);

        }
        #endregion

        #region VPS - Configuration
        [WebMethod]
        public ResultObject ChangeAdministratorPassword(int itemId, string password)
        {
            return VirtualizationServerController2012.ChangeAdministratorPassword(itemId, password);
        }
        #endregion

        #region VPS – Edit Configuration
        [WebMethod]
        public ResultObject UpdateVirtualMachineConfiguration(int itemId, int cpuCores, int ramMB, int hddGB, int snapshots,
                    bool dvdInstalled, bool bootFromCD, bool numLock,
                    bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed,
                    bool externalNetworkEnabled,
                    bool privateNetworkEnabled, VirtualMachine otherSettings)
        {
            return VirtualizationServerController2012.UpdateVirtualMachineConfiguration(
                    itemId, cpuCores, ramMB, hddGB, snapshots,
                    dvdInstalled, bootFromCD, numLock,
                    startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed,
                    externalNetworkEnabled, privateNetworkEnabled,
                    otherSettings);
        }
        #endregion

        #region DVD
        [WebMethod]
        public LibraryItem GetInsertedDvdDisk(int itemId)
        {
            return VirtualizationServerController2012.GetInsertedDvdDisk(itemId);
        }

        [WebMethod]
        public LibraryItem[] GetLibraryDisks(int itemId)
        {
            return VirtualizationServerController2012.GetLibraryDisks(itemId);
        }

        [WebMethod]
        public ResultObject InsertDvdDisk(int itemId, string isoPath)
        {
            return VirtualizationServerController2012.InsertDvdDisk(itemId, isoPath);
        }

        [WebMethod]
        public ResultObject EjectDvdDisk(int itemId)
        {
            return VirtualizationServerController2012.EjectDvdDisk(itemId);
        }
        #endregion

        #region Snaphosts
        [WebMethod]
        public VirtualMachineSnapshot[] GetVirtualMachineSnapshots(int itemId)
        {
            return VirtualizationServerController2012.GetVirtualMachineSnapshots(itemId);
        }

        [WebMethod]
        public VirtualMachineSnapshot GetSnapshot(int itemId, string snaphostId)
        {
            return VirtualizationServerController2012.GetSnapshot(itemId, snaphostId);
        }

        [WebMethod]
        public ResultObject CreateSnapshot(int itemId)
        {
            return VirtualizationServerController2012.CreateSnapshot(itemId);
        }

        [WebMethod]
        public ResultObject ApplySnapshot(int itemId, string snapshotId)
        {
            return VirtualizationServerController2012.ApplySnapshot(itemId, snapshotId);
        }

        [WebMethod]
        public ResultObject RenameSnapshot(int itemId, string snapshotId, string newName)
        {
            return VirtualizationServerController2012.RenameSnapshot(itemId, snapshotId, newName);
        }

        [WebMethod]
        public ResultObject DeleteSnapshot(int itemId, string snapshotId)
        {
            return VirtualizationServerController2012.DeleteSnapshot(itemId, snapshotId);
        }

        [WebMethod]
        public ResultObject DeleteSnapshotSubtree(int itemId, string snapshotId)
        {
            return VirtualizationServerController2012.DeleteSnapshotSubtree(itemId, snapshotId);
        }

        [WebMethod]
        public byte[] GetSnapshotThumbnail(int itemId, string snapshotId, ThumbnailSize size)
        {
            return VirtualizationServerController2012.GetSnapshotThumbnail(itemId, snapshotId, size);
        }
        #endregion

        #region VPS - External Network
        [WebMethod]
        public int GetExternalNetworkVLAN(int itemId)
        {
            return VirtualizationServerController2012.GetExternalNetworkVLAN(itemId);
        }

    [WebMethod]
        public NetworkAdapterDetails GetExternalNetworkAdapterDetails(int itemId)
        {
            return VirtualizationServerController2012.GetExternalNetworkAdapterDetails(itemId);
        }

        [WebMethod]
        public ResultObject AddVirtualMachineExternalIPAddresses(int itemId, bool selectRandom,
            int addressesNumber, int[] addressId)
        {
            return VirtualizationServerController2012.AddVirtualMachineExternalIPAddresses(itemId, selectRandom,
                addressesNumber, addressId, true);
        }

        [WebMethod]
        public ResultObject SetVirtualMachinePrimaryExternalIPAddress(int itemId, int addressId)
        {
            return VirtualizationServerController2012.SetVirtualMachinePrimaryExternalIPAddress(itemId, addressId, true);
        }

        [WebMethod]
        public ResultObject DeleteVirtualMachineExternalIPAddresses(int itemId, int[] addressId)
        {
            return VirtualizationServerController2012.DeleteVirtualMachineExternalIPAddresses(itemId, addressId, true);
        }
        #endregion

        #region VPS – Private Network
        [WebMethod]
        public NetworkAdapterDetails GetPrivateNetworkAdapterDetails(int itemId)
        {
            return VirtualizationServerController2012.GetPrivateNetworkAdapterDetails(itemId);
        }

        [WebMethod]
        public ResultObject AddVirtualMachinePrivateIPAddresses(int itemId, bool selectRandom,
            int addressesNumber, string[] addresses)
        {
            return VirtualizationServerController2012.AddVirtualMachinePrivateIPAddresses(itemId, selectRandom,
                addressesNumber, addresses, true);
        }

        [WebMethod]
        public ResultObject SetVirtualMachinePrimaryPrivateIPAddress(int itemId, int addressId)
        {
            return VirtualizationServerController2012.SetVirtualMachinePrimaryPrivateIPAddress(itemId, addressId, true);
        }

        [WebMethod]
        public ResultObject DeleteVirtualMachinePrivateIPAddresses(int itemId, int[] addressId)
        {
            return VirtualizationServerController2012.DeleteVirtualMachinePrivateIPAddresses(itemId, addressId, true);
        }
        #endregion

        #region Virtual Machine Permissions
        [WebMethod]
        public List<VirtualMachinePermission> GetVirtualMachinePermissions(int itemId)
        {
            return VirtualizationServerController2012.GetVirtualMachinePermissions(itemId);
        }

        [WebMethod]
        public int UpdateVirtualMachineUserPermissions(int itemId, VirtualMachinePermission[] permissions)
        {
            return VirtualizationServerController2012.UpdateVirtualMachineUserPermissions(itemId, permissions);
        }
        #endregion

        #region Virtual Switches
        [WebMethod]
        public VirtualSwitch[] GetExternalSwitches(int serviceId, string computerName)
        {
            return VirtualizationServerController2012.GetExternalSwitches(serviceId, computerName);
        }
        #endregion

        #region Tools
        [WebMethod]
        public ResultObject DeleteVirtualMachine(int itemId, bool saveFiles, bool exportVps, string exportPath)
        {
            return VirtualizationServerController2012.DeleteVirtualMachine(itemId, saveFiles, exportVps, exportPath);
        }

        [WebMethod]
        public int ReinstallVirtualMachine(int itemId, string adminPassword, bool preserveVirtualDiskFiles,
            bool saveVirtualDisk, bool exportVps, string exportPath)
        {
            return VirtualizationServerController2012.ReinstallVirtualMachine(itemId, adminPassword, preserveVirtualDiskFiles,
                saveVirtualDisk, exportVps, exportPath);
        }
        #endregion

        #region Help
        [WebMethod]
        public string GetVirtualMachineSummaryText(int itemId)
        {
            return VirtualizationServerController2012.GetVirtualMachineSummaryText(itemId, false, false);
        }

        [WebMethod]
        public ResultObject SendVirtualMachineSummaryLetter(int itemId, string to, string bcc)
        {
            return VirtualizationServerController2012.SendVirtualMachineSummaryLetter(itemId, to, bcc, false);
        }
        #endregion

        #region Replication

        [WebMethod]
        public CertificateInfo[] GetCertificates(int serviceId, string remoteServer)
        {
            return VirtualizationServerController2012.GetCertificates(serviceId, remoteServer);
        }

        [WebMethod]
        public ResultObject SetReplicaServer(int serviceId, string remoteServer, string thumbprint, string storagePath)
        {
            return VirtualizationServerController2012.SetReplicaServer(serviceId, remoteServer, thumbprint, storagePath);
        }

        [WebMethod]
        public ResultObject UnsetReplicaServer(int serviceId, string remoteServer)
        {
            return VirtualizationServerController2012.UnsetReplicaServer(serviceId, remoteServer);
        }

        [WebMethod]
        public ReplicationServerInfo GetReplicaServer(int serviceId, string remoteServer)
        {
            return VirtualizationServerController2012.GetReplicaServer(serviceId, remoteServer);
        }

        [WebMethod]
        public VmReplication GetReplication(int itemId)
        {
            return VirtualizationServerController2012.GetReplication(itemId);
        }

        [WebMethod]
        public ReplicationDetailInfo GetReplicationInfo(int itemId)
        {
            return VirtualizationServerController2012.GetReplicationInfo(itemId);
        }

        [WebMethod]
        public ResultObject SetVmReplication(int itemId, VmReplication replication)
        {
            return VirtualizationServerController2012.SetVmReplication(itemId, replication);
        }

        [WebMethod]
        public ResultObject DisableVmReplication(int itemId)
        {
            return VirtualizationServerController2012.DisableVmReplication(itemId);
        }

        [WebMethod]
        public ResultObject PauseReplication(int itemId)
        {
            return VirtualizationServerController2012.PauseReplication(itemId);
        }

        [WebMethod]
        public ResultObject ResumeReplication(int itemId)
        {
            return VirtualizationServerController2012.ResumeReplication(itemId);
        }


        #endregion
  }
}
