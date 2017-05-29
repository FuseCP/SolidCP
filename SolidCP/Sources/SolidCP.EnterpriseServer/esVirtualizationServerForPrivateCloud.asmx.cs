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

using SolidCP.Providers.Virtualization;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esVirtualizationServerPC
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class esVirtualizationServerForPrivateCloud : System.Web.Services.WebService
    {
        #region Huper-V Cloud
        [WebMethod]
        public bool CheckServerState(VMForPCSettingsName control, string connString, string connName, int serviceId)
        {
            return VirtualizationServerControllerForPrivateCloud.CheckServerState(control, connString, connName, serviceId);
        }

        #endregion Huper-V Cloud

        #region Virtual Machines
        [WebMethod]
        public VirtualMachineMetaItemsPaged GetVirtualMachines(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            return VirtualizationServerControllerForPrivateCloud.GetVirtualMachines(packageId,
                filterColumn, filterValue, sortColumn, startRow, maximumRows, recursive);
        }

        [WebMethod]
        public VirtualMachine[] GetVirtualMachinesByServiceId(int serviceId)
        {
            return VirtualizationServerControllerForPrivateCloud.GetVirtualMachinesByServiceId(serviceId);
        }

        [WebMethod]
        public VMInfo GetVirtualMachineItem(int itemId)
        {
            return VirtualizationServerControllerForPrivateCloud.GetVirtualMachineByItemId(itemId);
        }

        [WebMethod]
        public string EvaluateVirtualMachineTemplate(int itemId, string template)
        {
            if (SecurityContext.CheckAccount(DemandAccount.IsActive | DemandAccount.IsAdmin | DemandAccount.NotDemo) != 0)
                throw new Exception("This method could be called by serveradmin only.");

            return VirtualizationServerControllerForPrivateCloud.EvaluateVirtualMachineTemplate(itemId, false, false, template);
        }
        #endregion

        #region External Network
        [WebMethod]
        public NetworkAdapterDetails GetExternalNetworkDetails(int packageId)
        {
            return VirtualizationServerControllerForPrivateCloud.GetExternalNetworkDetails(packageId);
        }
        #endregion

        #region Private Network
        [WebMethod]
        public PrivateIPAddressesPaged GetPackagePrivateIPAddressesPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return VirtualizationServerControllerForPrivateCloud.GetPackagePrivateIPAddressesPaged(packageId,
                filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<PrivateIPAddress> GetPackagePrivateIPAddresses(int packageId)
        {
            return VirtualizationServerControllerForPrivateCloud.GetPackagePrivateIPAddresses(packageId);
        }

        [WebMethod]
        public NetworkAdapterDetails GetPrivateNetworkDetails(int packageId)
        {
            return VirtualizationServerControllerForPrivateCloud.GetPrivateNetworkDetails(packageId);
        }
        #endregion

        #region User Permissions
        [WebMethod]
        public List<VirtualMachinePermission> GetSpaceUserPermissions(int packageId)
        {
            return VirtualizationServerControllerForPrivateCloud.GetSpaceUserPermissions(packageId);
        }

        [WebMethod]
        public int UpdateSpaceUserPermissions(int packageId, VirtualMachinePermission[] permissions)
        {
            return VirtualizationServerControllerForPrivateCloud.UpdateSpaceUserPermissions(packageId, permissions);
        }
        #endregion

        #region Audit Log
        [WebMethod]
        public List<LogRecord> GetSpaceAuditLog(int packageId, DateTime startPeriod, DateTime endPeriod,
            int severity, string sortColumn, int startRow, int maximumRows)
        {
            return VirtualizationServerControllerForPrivateCloud.GetSpaceAuditLog(packageId, startPeriod, endPeriod,
                severity, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<LogRecord> GetVirtualMachineAuditLog(int itemId, DateTime startPeriod, DateTime endPeriod,
            int severity, string sortColumn, int startRow, int maximumRows)
        {
            return VirtualizationServerControllerForPrivateCloud.GetVirtualMachineAuditLog(itemId, startPeriod, endPeriod,
                severity, sortColumn, startRow, maximumRows);
        }
        #endregion

        #region VPS Create – Name & OS
        [WebMethod]
        public LibraryItem[] GetOperatingSystemTemplatesPC(int packageId)
        {
            return VirtualizationServerControllerForPrivateCloud.GetOperatingSystemTemplates(packageId);
        }

        [WebMethod]
        public LibraryItem[] GetHosts(int serviceId)
        {
            return VirtualizationServerControllerForPrivateCloud.GetHosts(serviceId);
        }

        [WebMethod]
        public LibraryItem[] GetClusters(int serviceId)
        {
            return VirtualizationServerControllerForPrivateCloud.GetClusters(serviceId);
        }


        [WebMethod]
        public LibraryItem[] GetOperatingSystemTemplatesByServiceId(int serviceId)
        {
            return VirtualizationServerControllerForPrivateCloud.GetOperatingSystemTemplatesByServiceId(serviceId);
        }
        #endregion

        #region VPS Create - Configuration
        [WebMethod]
        public int GetMaximumCpuCoresNumber(int packageId, string templateId)
        {
            return VirtualizationServerControllerForPrivateCloud.GetMaximumCpuCoresNumber(packageId, templateId);
        }

        [WebMethod]
        public string GetDefaultExportPath(int itemId)
        {
            return VirtualizationServerControllerForPrivateCloud.GetDefaultExportPath(itemId);
        }
        #endregion

        #region VPS Create
        [WebMethod]
        public ResultObject CreateVMFromVM(int packageId, VMInfo vmTemplate, string vmName)
        {
            return VirtualizationServerControllerForPrivateCloud.CreateVMFromVM(packageId, vmTemplate, vmName);
        }

        [WebMethod]
        public IntResult CreateVirtualMachine(int packageId,
                string hostname, string domain, string osTemplateFile, string vmName, string password, string summaryLetterEmail,
                int cpuCores, int ramMB, int hddGB, int snapshots,
                bool dvdInstalled, bool bootFromCD, bool numLock,
                bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed,
                bool externalNetworkEnabled, string externalNetworkLocation, string externalNicMacAddress, string externalVirtualNetwork,
                bool privateNetworkEnabled, string privateNetworkLocation, string privateNicMacAddress, string privateVirtualNetwork, ushort privateVLanID)
        {
            IntResult ret = VirtualizationServerControllerForPrivateCloud.CreateVirtualMachine(packageId,
                hostname, domain, osTemplateFile, vmName, password, summaryLetterEmail,
                cpuCores, ramMB, hddGB, snapshots,
                dvdInstalled, bootFromCD, numLock,
                startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed,
                externalNetworkEnabled, externalNetworkLocation, externalNicMacAddress, externalVirtualNetwork,
                privateNetworkEnabled, privateNetworkLocation, privateNicMacAddress, privateVirtualNetwork, privateVLanID);

            return ret;
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
            return VirtualizationServerControllerForPrivateCloud.ImportVirtualMachine(packageId,
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
            return VirtualizationServerControllerForPrivateCloud.GetVirtualMachineThumbnail(itemId, size);
        }

        [WebMethod]
        public VMInfo GetVirtualMachineGeneralDetails(int itemId)
        {
            return VirtualizationServerControllerForPrivateCloud.GetVirtualMachineGeneralDetails(itemId);
        }

        [WebMethod]
        public VirtualMachine GetVirtualMachineExtendedInfo(int serviceId, string vmId)
        {
            return VirtualizationServerControllerForPrivateCloud.GetVirtualMachineExtendedInfo(serviceId, vmId);
        }

        [WebMethod]
        public int CancelVirtualMachineJob(string jobId)
        {
            return VirtualizationServerControllerForPrivateCloud.CancelVirtualMachineJob(jobId);
        }

        [WebMethod]
        public ResultObject UpdateVirtualMachineHostName(int itemId, string hostname, bool updateNetBIOS)
        {
            return VirtualizationServerControllerForPrivateCloud.UpdateVirtualMachineHostName(itemId, hostname, updateNetBIOS);
        }

        [WebMethod]
        public ResultObject ChangeVirtualMachineState(int itemId, VirtualMachineRequestedState state)
        {
               return VirtualizationServerControllerForPrivateCloud.ChangeVirtualMachineStateExternal(itemId, state);    
        }


        [WebMethod]
        public List<ConcreteJob> GetVirtualMachineJobs(int itemId)
        {
            return VirtualizationServerControllerForPrivateCloud.GetVirtualMachineJobs(itemId);

        }
        #endregion

        #region VPS - Configuration
        [WebMethod]
        public ResultObject ChangeAdministratorPassword(int itemId, string password)
        {
            return VirtualizationServerControllerForPrivateCloud.ChangeAdministratorPassword(itemId, password);
        }
        #endregion

        #region VPS – Edit Configuration
        [WebMethod]
        public ResultObject UpdateVirtualMachineConfiguration(int itemId, int cpuCores, int ramMB, int hddGB, int snapshots,
                    bool dvdInstalled, bool bootFromCD, bool numLock,
                    bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed,
                    bool externalNetworkEnabled,
                    bool privateNetworkEnabled)
        {
            return VirtualizationServerControllerForPrivateCloud.UpdateVirtualMachineConfiguration(
                    itemId, cpuCores, ramMB, hddGB, snapshots,
                    dvdInstalled, bootFromCD, numLock,
                    startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed,
                    externalNetworkEnabled,
                    privateNetworkEnabled);
        }
        #endregion

        #region DVD
        [WebMethod]
        public LibraryItem GetInsertedDvdDisk(int itemId)
        {
            return VirtualizationServerControllerForPrivateCloud.GetInsertedDvdDisk(itemId);
        }

        [WebMethod]
        public LibraryItem[] GetLibraryDisks(int itemId)
        {
            return VirtualizationServerControllerForPrivateCloud.GetLibraryDisks(itemId);
        }

        [WebMethod]
        public ResultObject InsertDvdDisk(int itemId, string isoPath)
        {
            return VirtualizationServerControllerForPrivateCloud.InsertDvdDisk(itemId, isoPath);
        }

        [WebMethod]
        public ResultObject EjectDvdDisk(int itemId)
        {
            return VirtualizationServerControllerForPrivateCloud.EjectDvdDisk(itemId);
        }
        #endregion

        #region Snaphosts
        [WebMethod]
        public VirtualMachineSnapshot[] GetVirtualMachineSnapshots(int itemId)
        {
            return VirtualizationServerControllerForPrivateCloud.GetVirtualMachineSnapshots(itemId);
        }

        [WebMethod]
        public VirtualMachineSnapshot GetSnapshot(int itemId, string snaphostId)
        {
            return VirtualizationServerControllerForPrivateCloud.GetSnapshot(itemId, snaphostId);
        }

        [WebMethod]
        public ResultObject CreateSnapshot(int itemId)
        {
            return VirtualizationServerControllerForPrivateCloud.CreateSnapshot(itemId);
        }

        [WebMethod]
        public ResultObject ApplySnapshot(int itemId, string snapshotId)
        {
            return VirtualizationServerControllerForPrivateCloud.ApplySnapshot(itemId, snapshotId);
        }

        [WebMethod]
        public ResultObject RenameSnapshot(int itemId, string snapshotId, string newName)
        {
            return VirtualizationServerControllerForPrivateCloud.RenameSnapshot(itemId, snapshotId, newName);
        }

        [WebMethod]
        public ResultObject DeleteSnapshot(int itemId, string snapshotId)
        {
            return VirtualizationServerControllerForPrivateCloud.DeleteSnapshot(itemId, snapshotId);
        }

        [WebMethod]
        public ResultObject DeleteSnapshotSubtree(int itemId, string snapshotId)
        {
            return VirtualizationServerControllerForPrivateCloud.DeleteSnapshotSubtree(itemId, snapshotId);
        }

        [WebMethod]
        public byte[] GetSnapshotThumbnail(int itemId, string snapshotId, ThumbnailSize size)
        {
            return VirtualizationServerControllerForPrivateCloud.GetSnapshotThumbnail(itemId, snapshotId, size);
        }
        #endregion

        #region VM - Network
        [WebMethod]
        public void ConfigureCreatedVMNetworkAdapters(VMInfo vmInfo)
        {
            VirtualizationServerControllerForPrivateCloud.ConfigureCreatedVMNetworkAdapters(vmInfo);
        }
        #endregion VM - Network

        #region VPS - External Network
        [WebMethod]
        public NetworkAdapterDetails GetExternalNetworkAdapterDetails(int itemId)
        {
            return VirtualizationServerControllerForPrivateCloud.GetExternalNetworkAdapterDetails(itemId);
        }

        [WebMethod]
        public ResultObject AddVirtualMachineExternalIPAddresses(int itemId, bool selectRandom,
            int addressesNumber, int[] addressId)
        {
            return VirtualizationServerControllerForPrivateCloud.AddVirtualMachineExternalIPAddresses(itemId, selectRandom,
                addressesNumber, addressId, true);
        }

        [WebMethod]
        public ResultObject SetVirtualMachinePrimaryExternalIPAddress(int itemId, int addressId)
        {
            return VirtualizationServerControllerForPrivateCloud.SetVirtualMachinePrimaryExternalIPAddress(itemId, addressId, true);
        }

        [WebMethod]
        public ResultObject DeleteVirtualMachineExternalIPAddresses(int itemId, int[] addressId)
        {
            return VirtualizationServerControllerForPrivateCloud.DeleteVirtualMachineExternalIPAddresses(itemId, addressId, true);
        }
        #endregion

        #region VPS – Private Network
        [WebMethod]
        public NetworkAdapterDetails GetPrivateNetworkAdapterDetails(int itemId)
        {
            return VirtualizationServerControllerForPrivateCloud.GetPrivateNetworkAdapterDetails(itemId);
        }

        [WebMethod]
        public ResultObject AddVirtualMachinePrivateIPAddresses(int itemId, bool selectRandom,
            int addressesNumber, string[] addresses)
        {
            return VirtualizationServerControllerForPrivateCloud.AddVirtualMachinePrivateIPAddresses(itemId, selectRandom,
                addressesNumber, addresses, true);
        }

        [WebMethod]
        public ResultObject SetVirtualMachinePrimaryPrivateIPAddress(int itemId, int addressId)
        {
            return VirtualizationServerControllerForPrivateCloud.SetVirtualMachinePrimaryPrivateIPAddress(itemId, addressId, true);
        }

        [WebMethod]
        public ResultObject DeleteVirtualMachinePrivateIPAddresses(int itemId, int[] addressId)
        {
            return VirtualizationServerControllerForPrivateCloud.DeleteVirtualMachinePrivateIPAddresses(itemId, addressId, true);
        }
        #endregion

        #region Virtual Machine Permissions
        [WebMethod]
        public List<VirtualMachinePermission> GetVirtualMachinePermissions(int itemId)
        {
            return VirtualizationServerControllerForPrivateCloud.GetVirtualMachinePermissions(itemId);
        }

        [WebMethod]
        public int UpdateVirtualMachineUserPermissions(int itemId, VirtualMachinePermission[] permissions)
        {
            return VirtualizationServerControllerForPrivateCloud.UpdateVirtualMachineUserPermissions(itemId, permissions);
        }
        #endregion

        #region Virtual Switches
        [WebMethod]
        public VirtualSwitch[] GetExternalSwitches(int serviceId, string computerName)
        {
            return VirtualizationServerControllerForPrivateCloud.GetExternalSwitches(serviceId, computerName);
        }
        #endregion

        #region Tools
        [WebMethod]
        public ResultObject DeleteVirtualMachine(int itemId, bool saveFiles, bool exportVps, string exportPath)
        {
            return VirtualizationServerControllerForPrivateCloud.DeleteVirtualMachine(itemId, saveFiles, exportVps, exportPath);
        }

        [WebMethod]
        public int ReinstallVirtualMachine(int itemId, string adminPassword, bool preserveVirtualDiskFiles,
            bool saveVirtualDisk, bool exportVps, string exportPath)
        {
            return VirtualizationServerControllerForPrivateCloud.ReinstallVirtualMachine(itemId, adminPassword, preserveVirtualDiskFiles,
                saveVirtualDisk, exportVps, exportPath);
        }
        #endregion

        #region Help
        [WebMethod]
        public string GetVirtualMachineSummaryText(int itemId)
        {
            return VirtualizationServerControllerForPrivateCloud.GetVirtualMachineSummaryText(itemId, false, false);
        }

        [WebMethod]
        public ResultObject SendVirtualMachineSummaryLetter(int itemId, string to, string bcc)
        {
            return VirtualizationServerControllerForPrivateCloud.SendVirtualMachineSummaryLetter(itemId, to, bcc, false);
        }
        #endregion

        #region Monitoring
        [WebMethod]
        public MonitoredObjectEvent[] GetDeviceEvents(int ItemID)
        {
            return VirtualizationServerControllerForPrivateCloud.GetDeviceEvents(ItemID);
        }

        [WebMethod]
        public MonitoredObjectAlert[] GetMonitoringAlerts(int ItemID)
        {
            return VirtualizationServerControllerForPrivateCloud.GetMonitoringAlerts(ItemID);
        }

        [WebMethod]
        public PerformanceDataValue[] GetPerfomanceValue(int ItemID, PerformanceType perf, DateTime startPeriod, DateTime endPeriod)
        {
            return VirtualizationServerControllerForPrivateCloud.GetPerfomanceValue(ItemID, perf, startPeriod, endPeriod);
        }

        #endregion

        #region Network
        [WebMethod]
        public VirtualNetworkInfo[] GetVirtualNetwork(int packageId)
        {
            return VirtualizationServerControllerForPrivateCloud.GetVirtualNetwork(packageId);
        }
        #endregion

    }
}
