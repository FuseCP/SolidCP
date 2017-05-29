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
    public class esVirtualizationServer : System.Web.Services.WebService
    {
        #region Virtual Machines
        [WebMethod]
        public VirtualMachineMetaItemsPaged GetVirtualMachines(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            return VirtualizationServerController.GetVirtualMachines(packageId,
                filterColumn, filterValue, sortColumn, startRow, maximumRows, recursive);
        }

        [WebMethod]
        public VirtualMachine[] GetVirtualMachinesByServiceId(int serviceId)
        {
            return VirtualizationServerController.GetVirtualMachinesByServiceId(serviceId);
        }

        [WebMethod]
        public VirtualMachine GetVirtualMachineItem(int itemId)
        {
            return VirtualizationServerController.GetVirtualMachineByItemId(itemId);
        }

        [WebMethod]
        public string EvaluateVirtualMachineTemplate(int itemId, string template)
        {
            if (SecurityContext.CheckAccount(DemandAccount.IsActive | DemandAccount.IsAdmin | DemandAccount.NotDemo) != 0)
                throw new Exception("This method could be called by serveradmin only.");

            return VirtualizationServerController.EvaluateVirtualMachineTemplate(itemId, false, false, template);
        }
        #endregion

        #region External Network
        [WebMethod]
        public NetworkAdapterDetails GetExternalNetworkDetails(int packageId)
        {
            return VirtualizationServerController.GetExternalNetworkDetails(packageId);
        }
        #endregion

        #region Private Network
        [WebMethod]
        public PrivateIPAddressesPaged GetPackagePrivateIPAddressesPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return VirtualizationServerController.GetPackagePrivateIPAddressesPaged(packageId,
                filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<PrivateIPAddress> GetPackagePrivateIPAddresses(int packageId)
        {
            return VirtualizationServerController.GetPackagePrivateIPAddresses(packageId);
        }

        [WebMethod]
        public NetworkAdapterDetails GetPrivateNetworkDetails(int packageId)
        {
            return VirtualizationServerController.GetPrivateNetworkDetails(packageId);
        }
        #endregion

        #region User Permissions
        [WebMethod]
        public List<VirtualMachinePermission> GetSpaceUserPermissions(int packageId)
        {
            return VirtualizationServerController.GetSpaceUserPermissions(packageId);
        }

        [WebMethod]
        public int UpdateSpaceUserPermissions(int packageId, VirtualMachinePermission[] permissions)
        {
            return VirtualizationServerController.UpdateSpaceUserPermissions(packageId, permissions);
        }
        #endregion

        #region Audit Log
        [WebMethod]
        public List<LogRecord> GetSpaceAuditLog(int packageId, DateTime startPeriod, DateTime endPeriod,
            int severity, string sortColumn, int startRow, int maximumRows)
        {
            return VirtualizationServerController.GetSpaceAuditLog(packageId, startPeriod, endPeriod,
                severity, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<LogRecord> GetVirtualMachineAuditLog(int itemId, DateTime startPeriod, DateTime endPeriod,
            int severity, string sortColumn, int startRow, int maximumRows)
        {
            return VirtualizationServerController.GetVirtualMachineAuditLog(itemId, startPeriod, endPeriod,
                severity, sortColumn, startRow, maximumRows);
        }
        #endregion

        #region VPS Create – Name & OS
        [WebMethod]
        public LibraryItem[] GetOperatingSystemTemplates(int packageId)
        {
            return VirtualizationServerController.GetOperatingSystemTemplates(packageId);
        }

        [WebMethod]
        public LibraryItem[] GetOperatingSystemTemplatesByServiceId(int serviceId)
        {
            return VirtualizationServerController.GetOperatingSystemTemplatesByServiceId(serviceId);
        }
        #endregion

        #region VPS Create - Configuration
        [WebMethod]
        public int GetMaximumCpuCoresNumber(int packageId)
        {
            return VirtualizationServerController.GetMaximumCpuCoresNumber(packageId);
        }

        [WebMethod]
        public string GetDefaultExportPath(int itemId)
        {
            return VirtualizationServerController.GetDefaultExportPath(itemId);
        }
        #endregion

        #region VPS Create
        [WebMethod]
        public IntResult CreateDefaultVirtualMachine(int packageId,
            string hostname, string osTemplate, string password, string summaryLetterEmail)
        {
            return VirtualizationServerController.CreateDefaultVirtualMachine(packageId, hostname, osTemplate, password, summaryLetterEmail);
        }

        [WebMethod]
        public IntResult CreateVirtualMachine(int packageId,
                string hostname, string osTemplateFile, string password, string summaryLetterEmail,
                int generation, int cpuCores, int ramMB, int hddGB, int snapshots, bool dvdInstalled, bool bootFromCD, bool numLock,
                bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed,
                bool externalNetworkEnabled, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses,
                bool privateNetworkEnabled, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses)
        {
            return VirtualizationServerController.CreateVirtualMachine(packageId,
                hostname, osTemplateFile, password, summaryLetterEmail,
                generation, cpuCores, ramMB, hddGB, snapshots, dvdInstalled, bootFromCD, numLock,
                startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed,
                externalNetworkEnabled, externalAddressesNumber, randomExternalAddresses, externalAddresses,
                privateNetworkEnabled, privateAddressesNumber, randomPrivateAddresses, privateAddresses);
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
            return VirtualizationServerController.ImportVirtualMachine(packageId,
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
            return VirtualizationServerController.GetVirtualMachineThumbnail(itemId, size);
        }

        [WebMethod]
        public VirtualMachine GetVirtualMachineGeneralDetails(int itemId)
        {
            return VirtualizationServerController.GetVirtualMachineGeneralDetails(itemId);
        }

        [WebMethod]
        public VirtualMachine GetVirtualMachineExtendedInfo(int serviceId, string vmId)
        {
            return VirtualizationServerController.GetVirtualMachineExtendedInfo(serviceId, vmId);
        }

        [WebMethod]
        public int CancelVirtualMachineJob(string jobId)
        {
            return VirtualizationServerController.CancelVirtualMachineJob(jobId);
        }

        [WebMethod]
        public ResultObject UpdateVirtualMachineHostName(int itemId, string hostname, bool updateNetBIOS)
        {
            return VirtualizationServerController.UpdateVirtualMachineHostName(itemId, hostname, updateNetBIOS);
        }

        [WebMethod]
        public ResultObject ChangeVirtualMachineState(int itemId, VirtualMachineRequestedState state)
        {
               return VirtualizationServerController.ChangeVirtualMachineStateExternal(itemId, state);    
        }


        [WebMethod]
        public List<ConcreteJob> GetVirtualMachineJobs(int itemId)
        {
            return VirtualizationServerController.GetVirtualMachineJobs(itemId);

        }
        #endregion

        #region VPS - Configuration
        [WebMethod]
        public ResultObject ChangeAdministratorPassword(int itemId, string password)
        {
            return VirtualizationServerController.ChangeAdministratorPassword(itemId, password);
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
            return VirtualizationServerController.UpdateVirtualMachineConfiguration(
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
            return VirtualizationServerController.GetInsertedDvdDisk(itemId);
        }

        [WebMethod]
        public LibraryItem[] GetLibraryDisks(int itemId)
        {
            return VirtualizationServerController.GetLibraryDisks(itemId);
        }

        [WebMethod]
        public ResultObject InsertDvdDisk(int itemId, string isoPath)
        {
            return VirtualizationServerController.InsertDvdDisk(itemId, isoPath);
        }

        [WebMethod]
        public ResultObject EjectDvdDisk(int itemId)
        {
            return VirtualizationServerController.EjectDvdDisk(itemId);
        }
        #endregion

        #region Snaphosts
        [WebMethod]
        public VirtualMachineSnapshot[] GetVirtualMachineSnapshots(int itemId)
        {
            return VirtualizationServerController.GetVirtualMachineSnapshots(itemId);
        }

        [WebMethod]
        public VirtualMachineSnapshot GetSnapshot(int itemId, string snaphostId)
        {
            return VirtualizationServerController.GetSnapshot(itemId, snaphostId);
        }

        [WebMethod]
        public ResultObject CreateSnapshot(int itemId)
        {
            return VirtualizationServerController.CreateSnapshot(itemId);
        }

        [WebMethod]
        public ResultObject ApplySnapshot(int itemId, string snapshotId)
        {
            return VirtualizationServerController.ApplySnapshot(itemId, snapshotId);
        }

        [WebMethod]
        public ResultObject RenameSnapshot(int itemId, string snapshotId, string newName)
        {
            return VirtualizationServerController.RenameSnapshot(itemId, snapshotId, newName);
        }

        [WebMethod]
        public ResultObject DeleteSnapshot(int itemId, string snapshotId)
        {
            return VirtualizationServerController.DeleteSnapshot(itemId, snapshotId);
        }

        [WebMethod]
        public ResultObject DeleteSnapshotSubtree(int itemId, string snapshotId)
        {
            return VirtualizationServerController.DeleteSnapshotSubtree(itemId, snapshotId);
        }

        [WebMethod]
        public byte[] GetSnapshotThumbnail(int itemId, string snapshotId, ThumbnailSize size)
        {
            return VirtualizationServerController.GetSnapshotThumbnail(itemId, snapshotId, size);
        }
        #endregion

        #region VPS - External Network
        [WebMethod]
        public NetworkAdapterDetails GetExternalNetworkAdapterDetails(int itemId)
        {
            return VirtualizationServerController.GetExternalNetworkAdapterDetails(itemId);
        }

        [WebMethod]
        public ResultObject AddVirtualMachineExternalIPAddresses(int itemId, bool selectRandom,
            int addressesNumber, int[] addressId)
        {
            return VirtualizationServerController.AddVirtualMachineExternalIPAddresses(itemId, selectRandom,
                addressesNumber, addressId, true);
        }

        [WebMethod]
        public ResultObject SetVirtualMachinePrimaryExternalIPAddress(int itemId, int addressId)
        {
            return VirtualizationServerController.SetVirtualMachinePrimaryExternalIPAddress(itemId, addressId, true);
        }

        [WebMethod]
        public ResultObject DeleteVirtualMachineExternalIPAddresses(int itemId, int[] addressId)
        {
            return VirtualizationServerController.DeleteVirtualMachineExternalIPAddresses(itemId, addressId, true);
        }
        #endregion

        #region VPS – Private Network
        [WebMethod]
        public NetworkAdapterDetails GetPrivateNetworkAdapterDetails(int itemId)
        {
            return VirtualizationServerController.GetPrivateNetworkAdapterDetails(itemId);
        }

        [WebMethod]
        public ResultObject AddVirtualMachinePrivateIPAddresses(int itemId, bool selectRandom,
            int addressesNumber, string[] addresses)
        {
            return VirtualizationServerController.AddVirtualMachinePrivateIPAddresses(itemId, selectRandom,
                addressesNumber, addresses, true);
        }

        [WebMethod]
        public ResultObject SetVirtualMachinePrimaryPrivateIPAddress(int itemId, int addressId)
        {
            return VirtualizationServerController.SetVirtualMachinePrimaryPrivateIPAddress(itemId, addressId, true);
        }

        [WebMethod]
        public ResultObject DeleteVirtualMachinePrivateIPAddresses(int itemId, int[] addressId)
        {
            return VirtualizationServerController.DeleteVirtualMachinePrivateIPAddresses(itemId, addressId, true);
        }
        #endregion

        #region Virtual Machine Permissions
        [WebMethod]
        public List<VirtualMachinePermission> GetVirtualMachinePermissions(int itemId)
        {
            return VirtualizationServerController.GetVirtualMachinePermissions(itemId);
        }

        [WebMethod]
        public int UpdateVirtualMachineUserPermissions(int itemId, VirtualMachinePermission[] permissions)
        {
            return VirtualizationServerController.UpdateVirtualMachineUserPermissions(itemId, permissions);
        }
        #endregion

        #region Virtual Switches
        [WebMethod]
        public VirtualSwitch[] GetExternalSwitches(int serviceId, string computerName)
        {
            return VirtualizationServerController.GetExternalSwitches(serviceId, computerName);
        }
        #endregion

        #region Tools
        [WebMethod]
        public ResultObject DeleteVirtualMachine(int itemId, bool saveFiles, bool exportVps, string exportPath)
        {
            return VirtualizationServerController.DeleteVirtualMachine(itemId, saveFiles, exportVps, exportPath);
        }

        [WebMethod]
        public int ReinstallVirtualMachine(int itemId, string adminPassword, bool preserveVirtualDiskFiles,
            bool saveVirtualDisk, bool exportVps, string exportPath)
        {
            return VirtualizationServerController.ReinstallVirtualMachine(itemId, adminPassword, preserveVirtualDiskFiles,
                saveVirtualDisk, exportVps, exportPath);
        }
        #endregion

        #region Help
        [WebMethod]
        public string GetVirtualMachineSummaryText(int itemId)
        {
            return VirtualizationServerController.GetVirtualMachineSummaryText(itemId, false, false);
        }

        [WebMethod]
        public ResultObject SendVirtualMachineSummaryLetter(int itemId, string to, string bcc)
        {
            return VirtualizationServerController.SendVirtualMachineSummaryLetter(itemId, to, bcc, false);
        }
        #endregion
    }
}
