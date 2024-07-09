// Copyright (c) 2019, SolidCP
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

using System;
using System.Data;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.Virtualization;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using SolidCP.Providers;
using System.Text;
using System.Collections;
using System.Net.Mail;
using System.Diagnostics;
using SolidCP.EnterpriseServer.Code.Virtualization2012;
using SolidCP.Server.Client;
//using SolidCP.Providers.Virtualization2012;
using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers;
using System.Threading;
using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.VM;
using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.PS;
using SolidCP.EnterpriseServer.Code.Virtualization2012.UseCase;

namespace SolidCP.EnterpriseServer
{
    public class VirtualizationServerController2012: ControllerBase
    {
        //private const string MS_MAC_PREFIX = "00155D"; // IEEE prefix of MS MAC addresses

        // default server creation (if "Unlimited" was specified in the hosting plan)
        private const int DEFAULT_PASSWORD_LENGTH = 12;
        private const int DEFAULT_RAM_SIZE = 512; // megabytes
        private const int DEFAULT_HDD_SIZE = 20; // gigabytes
        private const int DEFAULT_PRIVATE_IPS_NUMBER = 1;
        private const int DEFAULT_SNAPSHOTS_NUMBER = 5;
        private const int DEFAULT_VLAN = 0;
        private const int DEFAULT_MINIMUM_IOPS = 0;
        private const int DEFAULT_MAXIMUM_IOPS = 0;

        public VirtualizationServerController2012(ControllerBase provider) : base(provider) { }

        #region Virtual Machines
        public VirtualMachineMetaItemsPaged GetVirtualMachines(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            VirtualMachineMetaItemsPaged result = new VirtualMachineMetaItemsPaged();

            // get reader
            IDataReader reader = Database.GetVirtualMachinesPaged2012(
                    SecurityContext.User.UserId,
                    packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows, recursive);

            // number of items = first data reader
            reader.Read();
            result.Count = (int)reader[0];

            // items = second data reader
            reader.NextResult();
            result.Items = ObjectUtils.CreateListFromDataReader<VirtualMachineMetaItem>(reader).ToArray();

            return result;
        }

        public VirtualMachine[] GetVirtualMachinesByServiceId(int serviceId)
        {
            // get proxy
            VirtualizationServer2012 vps = VirtualizationHelper.GetVirtualizationProxy(serviceId);

            // load details
            return vps.GetVirtualMachines();
        }
        #endregion

        #region Private Network
        public PrivateIPAddressesPaged GetPackagePrivateIPAddressesPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            PrivateIPAddressesPaged result = new PrivateIPAddressesPaged();

            // get reader
            IDataReader reader = Database.GetPackagePrivateIPAddressesPaged(packageId, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);

            // number of items = first data reader
            reader.Read();
            result.Count = (int)reader[0];

            // items = second data reader
            reader.NextResult();
            result.Items = ObjectUtils.CreateListFromDataReader<PrivateIPAddress>(reader).ToArray();

            return result;
        }

        public List<PrivateIPAddress> GetPackagePrivateIPAddresses(int packageId)
        {
            return IpAddressPrivateHelper.GetPackagePrivateIPAddresses(packageId);
        }
        #endregion

        #region DMZ Network
        public static DmzIPAddressesPaged GetPackageDmzIPAddressesPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            DmzIPAddressesPaged result = new DmzIPAddressesPaged();

            // get reader
            IDataReader reader = DataProvider.GetPackageDmzIPAddressesPaged(packageId, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);

            // number of items = first data reader
            reader.Read();
            result.Count = (int)reader[0];

            // items = second data reader
            reader.NextResult();
            result.Items = ObjectUtils.CreateListFromDataReader<DmzIPAddress>(reader).ToArray();

            return result;
        }

        public static List<DmzIPAddress> GetPackageDmzIPAddresses(int packageId)
        {
            return IpAddressPrivateHelper.GetPackageDmzIPAddresses(packageId);
        }
        #endregion

        #region User Permissions
        public List<VirtualMachinePermission> GetSpaceUserPermissions(int packageId)
        {
            List<VirtualMachinePermission> result = new List<VirtualMachinePermission>();
            return result;
        }

        public int UpdateSpaceUserPermissions(int packageId, VirtualMachinePermission[] permissions)
        {
            // VDC - UPDATE_PERMISSIONS
            return 0;
        }
        #endregion

        #region Audit Log
        public List<LogRecord> GetSpaceAuditLog(int packageId, DateTime startPeriod, DateTime endPeriod,
            int severity, string sortColumn, int startRow, int maximumRows)
        {
            List<LogRecord> result = new List<LogRecord>();
            return result;
        }

        public List<LogRecord> GetVirtualMachineAuditLog(int itemId, DateTime startPeriod, DateTime endPeriod,
            int severity, string sortColumn, int startRow, int maximumRows)
        {
            List<LogRecord> result = new List<LogRecord>();
            return result;
        }
        #endregion

        #region VPS Create – Name & OS
        public LibraryItem[] GetOperatingSystemTemplates(int packageId)
        {
            return VirtualizationHelper.GetOperatingSystemTemplates(packageId);
        }

        public LibraryItem[] GetOperatingSystemTemplatesByServiceId(int serviceId)
        {
            return VirtualizationHelper.GetOperatingSystemTemplatesByServiceId(serviceId);
        }
        #endregion

        #region VPS Create - Configuration
        public int GetMaximumCpuCoresNumber(int packageId)
        {
            return VirtualizationHelper.GetMaximumCpuCoresNumber(packageId);
        }

        public string GetDefaultExportPath(int itemId)
        {
            return VirtualizationHelper.GetDefaultExportPath(itemId);
        }
        #endregion

        #region VPS Create
        public IntResult CreateDefaultVirtualMachine(int packageId,
            string hostname, string osTemplate, string password, string summaryLetterEmail)
        {
            #region VPS Create Default (is this useful?)
            if (String.IsNullOrEmpty(osTemplate))
                throw new ArgumentNullException("osTemplate");

            IntResult res = new IntResult();

            VirtualMachine vmSettings = new VirtualMachine();

            // load package info
            PackageInfo package = PackageController.GetPackage(packageId);
            if (package == null)
            {
                res.ErrorCodes.Add("VPS_CREATE_PACKAGE_NOT_FOUND");
                return res;
            }

            // generate host name if not specified
            if (String.IsNullOrEmpty(hostname))
            {
                // load hostname pattern
                PackageSettings spaceSettings = PackageController.GetPackageSettings(packageId, PackageSettings.VIRTUAL_PRIVATE_SERVERS_2012);
                string hostnamePattern = spaceSettings["HostnamePattern"];
                if (String.IsNullOrEmpty(hostnamePattern))
                {
                    res.ErrorCodes.Add("VPS_CREATE_EMPTY_HOSTNAME_PATTERN");
                    return res;
                }

                hostname = VirtualizationUtils.EvaluateSpaceVariables(hostnamePattern, packageId);
            }

            // generate password if not specified
            if (String.IsNullOrEmpty(password))
            {
                int passwordLength = DEFAULT_PASSWORD_LENGTH; // default length

                // load password policy
                UserSettings userSettings = UserController.GetUserSettings(package.UserId, UserSettings.VPS_POLICY);
                string passwordPolicy = userSettings["AdministratorPasswordPolicy"];

                if (!String.IsNullOrEmpty(passwordPolicy))
                {
                    // get second parameter - max length
                    passwordLength = Utils.ParseInt(passwordPolicy.Split(';')[1].Trim(), passwordLength);
                }

                // generate password
                password = Utils.GetRandomString(passwordLength);
            }

            // load quotas
            PackageContext cntx = PackageController.GetPackageContext(packageId);
            if (cntx.Groups.ContainsKey(ResourceGroups.VPS2012))
            {
                res.ErrorCodes.Add("VPS_CREATE_VPS_GROUP_DISABLED");
                return res;
            }

            // CPU cores
            int cpuCores = cntx.Quotas[Quotas.VPS2012_CPU_NUMBER].QuotaAllocatedValue;
            if (cpuCores == -1) // unlimited is not possible
                cpuCores = GetMaximumCpuCoresNumber(packageId);

            // RAM
            int ramMB = cntx.Quotas[Quotas.VPS2012_RAM].QuotaAllocatedValue;
            if (ramMB == -1) // unlimited is not possible
                ramMB = DEFAULT_RAM_SIZE;

            // HDD
            int hddGB = cntx.Quotas[Quotas.VPS2012_HDD].QuotaAllocatedValue;
            if (hddGB == -1) // unlimited is not possible
                hddGB = DEFAULT_HDD_SIZE;

            // IOPS
            // TODO IOPS checks
            vmSettings.HddMinimumIOPS = DEFAULT_MINIMUM_IOPS;
            vmSettings.HddMaximumIOPS = DEFAULT_MAXIMUM_IOPS;

            // snapshots
            int snapshots = cntx.Quotas[Quotas.VPS2012_SNAPSHOTS_NUMBER].QuotaAllocatedValue;
            if (snapshots == -1) // unlimited is not possible
                snapshots = DEFAULT_SNAPSHOTS_NUMBER;

            bool dvdInstalled = !cntx.Quotas[Quotas.VPS2012_DVD_ENABLED].QuotaExhausted;
            bool bootFromCD = !cntx.Quotas[Quotas.VPS2012_BOOT_CD_ENABLED].QuotaExhausted;
            bool numLock = true;

            bool startShutdownAllowed = !cntx.Quotas[Quotas.VPS2012_START_SHUTDOWN_ALLOWED].QuotaExhausted;
            bool pauseResumeAllowed = !cntx.Quotas[Quotas.VPS2012_PAUSE_RESUME_ALLOWED].QuotaExhausted;
            bool rebootAllowed = !cntx.Quotas[Quotas.VPS2012_REBOOT_ALLOWED].QuotaExhausted;
            bool resetAllowed = !cntx.Quotas[Quotas.VPS2012_RESET_ALOWED].QuotaExhausted;
            bool reinstallAllowed = !cntx.Quotas[Quotas.VPS2012_REINSTALL_ALLOWED].QuotaExhausted;

            bool externalNetworkEnabled = !cntx.Quotas[Quotas.VPS2012_EXTERNAL_NETWORK_ENABLED].QuotaExhausted;
            int externalAddressesNumber = cntx.Quotas[Quotas.VPS2012_EXTERNAL_IP_ADDRESSES_NUMBER].QuotaAllocatedValue;
            bool randomExternalAddresses = true;
            int[] externalAddresses = new int[0]; // empty array
            if (externalNetworkEnabled)
            {
                int maxExternalAddresses = ServerController.GetPackageUnassignedIPAddresses(packageId, IPAddressPool.VpsExternalNetwork).Count;
                if (externalAddressesNumber == -1
                    || externalAddressesNumber > maxExternalAddresses)
                    externalAddressesNumber = maxExternalAddresses;
            }

            bool privateNetworkEnabled = !cntx.Quotas[Quotas.VPS2012_PRIVATE_NETWORK_ENABLED].QuotaExhausted;
            int privateAddressesNumber = cntx.Quotas[Quotas.VPS2012_PRIVATE_IP_ADDRESSES_NUMBER].QuotaAllocatedValue;
            bool randomPrivateAddresses = true;
            string[] privateAddresses = new string[0]; // empty array
            if (privateAddressesNumber == -1) // unlimited is not possible
            {
                privateAddressesNumber = DEFAULT_PRIVATE_IPS_NUMBER;
            }

            bool dmzNetworkEnabled = false;
            int dmzAddressesNumber = 0;
            bool randomDmzAddresses = false;
            string[] dmzAddresses = new string[0]; // empty array

            // create server and return result
            return CreateVirtualMachine(packageId, hostname, osTemplate, password, summaryLetterEmail,
                cpuCores, ramMB, hddGB, snapshots,
                dvdInstalled, bootFromCD, numLock,
                startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed,
                externalNetworkEnabled, externalAddressesNumber, randomExternalAddresses, externalAddresses,
                privateNetworkEnabled, privateAddressesNumber, randomPrivateAddresses, privateAddresses,
                dmzNetworkEnabled, dmzAddressesNumber, randomDmzAddresses, dmzAddresses, vmSettings);
            #endregion
        }
        public IntResult CreateNewVirtualMachine(VirtualMachine VMSettings, string osTemplateFile, string password, string summaryLetterEmail,
            int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses,
            int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses,
            int dmzAddressesNumber, bool randomDmzAddresses, string[] dmzAddresses)
        {
            return CreateNewVirtualMachineInternal(VMSettings, osTemplateFile, password, summaryLetterEmail,
            externalAddressesNumber, randomExternalAddresses, externalAddresses,
            privateAddressesNumber, randomPrivateAddresses, privateAddresses,
            dmzAddressesNumber, randomDmzAddresses, dmzAddresses,
            true);
        }

        private IntResult CreateNewVirtualMachineInternal(VirtualMachine VMSettings, string osTemplateFile, string password, string summaryLetterEmail,
            int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses,
            int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses,
            int dmzAddressesNumber, bool randomDmzAddresses, string[] dmzAddresses,
            bool createMetaItem)
        {
            return CreateVirtualMachineHandler.CreateNewVirtualMachineInternal(VMSettings, osTemplateFile, password, summaryLetterEmail,
            externalAddressesNumber, randomExternalAddresses, externalAddresses,
            privateAddressesNumber, randomPrivateAddresses, privateAddresses,
            dmzAddressesNumber, randomDmzAddresses, dmzAddresses,
            createMetaItem);
        }

        //[Obsolete("CreateVirtualMachine is deprecated, please use CreateNewVirtualMachine instead.")]
        public IntResult CreateVirtualMachine(int packageId,
                string hostname, string osTemplateFile, string password, string summaryLetterEmail,
                int cpuCores, int ramMB, int hddGB, int snapshots,
                bool dvdInstalled, bool bootFromCD, bool numLock,
                bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed,
                bool externalNetworkEnabled, int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses,
                bool privateNetworkEnabled, int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses,
                bool dmzNetworkEnabled, int dmzAddressesNumber, bool randomDmzAddresses, string[] dmzAddresses, VirtualMachine otherSettings)
        {
            otherSettings.PackageId = packageId;
            otherSettings.Name = hostname;
            otherSettings.CpuCores = cpuCores;
            otherSettings.RamSize = ramMB;
            otherSettings.HddSize = new[] { hddGB };
            //otherSettings.HddMinimumIOPS = hddMinimumIOPS;
            //otherSettings.HddMaximumIOPS = hddMaximumIOPS;
            otherSettings.SnapshotsNumber = snapshots;
            otherSettings.DvdDriveInstalled = dvdInstalled;
            otherSettings.BootFromCD = bootFromCD;
            otherSettings.NumLockEnabled = numLock;
            otherSettings.StartTurnOffAllowed = startShutdownAllowed;
            otherSettings.PauseResumeAllowed = pauseResumeAllowed;
            otherSettings.RebootAllowed = rebootAllowed;
            otherSettings.ResetAllowed = resetAllowed;
            otherSettings.ReinstallAllowed = reinstallAllowed;
            otherSettings.ExternalNetworkEnabled = externalNetworkEnabled;
            otherSettings.PrivateNetworkEnabled = privateNetworkEnabled;
            otherSettings.DmzNetworkEnabled = dmzNetworkEnabled;

            return CreateNewVirtualMachine(otherSettings, osTemplateFile, password, summaryLetterEmail,
                externalAddressesNumber, randomExternalAddresses, externalAddresses,
                privateAddressesNumber, randomPrivateAddresses, privateAddresses,
                dmzAddressesNumber, randomDmzAddresses, dmzAddresses);
        }

        public IntResult ImportVirtualMachine(int packageId,
            int serviceId, string vmId,
            string osTemplateFile, string adminPassword,
            bool IsBootFromCd, bool IsDvdInstalled,
            bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed,
            string externalNicMacAddress, int[] externalAddresses,
            string managementNicMacAddress, int managementAddress,
            int maxSnapshots,
            bool ignoreChecks)
        {
            return ImportVirtualMachineHandler.ImportVirtualMachine(packageId, serviceId, vmId, osTemplateFile, adminPassword, IsBootFromCd, IsDvdInstalled,
                startShutdownAllowed, pauseResumeAllowed, rebootAllowed, resetAllowed, reinstallAllowed, externalNicMacAddress, externalAddresses, managementNicMacAddress,
                managementAddress, maxSnapshots, ignoreChecks);
        }
        #endregion

        #region VPS – General

        public List<ConcreteJob> GetVirtualMachineJobs(int itemId)
        {
            return VirtualMachineHelper.GetVirtualMachineJobs(itemId);
        }

        public byte[] GetVirtualMachineThumbnail(int itemId, ThumbnailSize size)
        {
            return VirtualMachineHelper.GetVirtualMachineThumbnail(itemId, size);
        }

        public int DiscoverVirtualMachine(int itemId)
        {
            return VirtualizationHelper.DiscoverVirtualMachine(itemId);
        }

        public VirtualMachine GetVirtualMachineGeneralDetails(int itemId)
        {
            return VirtualMachineHelper.GetVirtualMachineGeneralDetails(itemId);
        }

        public VirtualMachine GetVirtualMachineExtendedInfo(int serviceId, string vmId)
        {
            return VirtualMachineHelper.GetVirtualMachineExtendedInfo(serviceId, vmId);
            //// get proxy
            //VirtualizationServer2012 vps = GetVirtualizationProxy(serviceId);

            //// load details
            //return vps.GetVirtualMachineEx(vmId);
        }

        public int CancelVirtualMachineJob(string jobId)
        {
            // VPS - CANCEL_JOB
            return 0;
        }

        public ResultObject UpdateVirtualMachineHostName(int itemId, string hostname, bool updateNetBIOS)
        {
            return UpdateVirtualMachineHostNameHandler.UpdateVirtualMachineHostName(itemId, hostname, updateNetBIOS);
        }

        public ResultObject ChangeVirtualMachineStateExternal(int itemId, VirtualMachineRequestedState state)
        {
            ResultObject res = new ResultObject();

            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                return res;
            }

            #region Check account and space statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, vm.PackageId, DemandPackage.IsActive))
                return res;
            #endregion

            #region Check Quotas
            // check quotas
            List<string> quotaResults = new List<string>();

            if ((state == VirtualMachineRequestedState.Start
                || state == VirtualMachineRequestedState.TurnOff
                || state == VirtualMachineRequestedState.ShutDown)
                && !vm.StartTurnOffAllowed)
                quotaResults.Add(VirtualizationErrorCodes.QUOTA_EXCEEDED_START_SHUTDOWN_ALLOWED);

            else if ((state == VirtualMachineRequestedState.Pause
                || state == VirtualMachineRequestedState.Resume)
                && !vm.PauseResumeAllowed)
                quotaResults.Add(VirtualizationErrorCodes.QUOTA_EXCEEDED_PAUSE_RESUME_ALLOWED);

            else if (state == VirtualMachineRequestedState.Reboot
                && !vm.RebootAllowed)
                quotaResults.Add(VirtualizationErrorCodes.QUOTA_EXCEEDED_REBOOT_ALLOWED);

            else if (state == VirtualMachineRequestedState.Reset
                && !vm.ResetAllowed)
                quotaResults.Add(VirtualizationErrorCodes.QUOTA_EXCEEDED_RESET_ALOWED);

            if (quotaResults.Count > 0)
            {
                res.ErrorCodes.AddRange(quotaResults);
                res.IsSuccess = false;
                TaskManager.CompleteResultTask();
                return res;
            }
            #endregion

            return ChangeVirtualMachineStateHandler.ChangeVirtualMachineState(itemId, state);
        }

        #endregion

        #region VPS - Configuration
        public VirtualMachineNetworkAdapter[] GetVirtualMachinesNetwordAdapterSettings(int itemId)
        {
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
                return null;

            VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);
            return vs.GetVirtualMachinesNetwordAdapterSettings(vm.Name);
        }

        public ResultObject ChangeAdministratorPassword(int itemId, string password)
        {
            return ChangeVirtualMachineAdministratorPasswordHandler.ChangeAdministratorPassword(itemId, password);
        }

        public ResultObject ChangeAdministratorPasswordAndCleanResult(int itemId, string password)
        {
            return ChangeVirtualMachineAdministratorPasswordHandler.ChangeAdministratorPasswordAndCleanResult(itemId, password);
        }
        #endregion

        #region VPS – Edit Configuration
        public ResultObject UpdateVirtualMachineResource(int itemId, VirtualMachine vmSettings)
        {
            return UpdateVirtualMachineResourceHandler.UpdateVirtualMachineResource(itemId, vmSettings);
        }

        //[Obsolete("UpdateVirtualMachineConfiguration is deprecated, please use UpdateVirtualMachineResource instead.")]
        public ResultObject UpdateVirtualMachineConfiguration(
            int itemId, int cpuCores, int ramMB, int[] hddGB, int snapshots,
            bool dvdInstalled, bool bootFromCD, bool numLock, bool startShutdownAllowed, bool pauseResumeAllowed,
            bool rebootAllowed, bool resetAllowed, bool reinstallAllowed, bool externalNetworkEnabled, bool privateNetworkEnabled, VirtualMachine otherSettings)
        {
            otherSettings.CpuCores = cpuCores;
            otherSettings.RamSize = ramMB;
            otherSettings.HddSize = hddGB;
            otherSettings.SnapshotsNumber = snapshots;

            otherSettings.BootFromCD = bootFromCD;
            otherSettings.NumLockEnabled = numLock;
            otherSettings.DvdDriveInstalled = dvdInstalled;

            otherSettings.StartTurnOffAllowed = startShutdownAllowed;
            otherSettings.PauseResumeAllowed = pauseResumeAllowed;
            otherSettings.ResetAllowed = resetAllowed;
            otherSettings.RebootAllowed = rebootAllowed;
            otherSettings.ReinstallAllowed = reinstallAllowed;

            otherSettings.ExternalNetworkEnabled = externalNetworkEnabled;
            otherSettings.PrivateNetworkEnabled = privateNetworkEnabled;
            return UpdateVirtualMachineResource(itemId, otherSettings);
        }
        #endregion

        #region VNC
        public string GetVirtualMachineGuacamoleURL(int itemId)
        {
            VirtualMachine vm = VirtualMachineHelper.GetVirtualMachineByItemId(itemId);
            string vncurl = GuacaHelper.GetUrl(vm);

            return vncurl;
        }
        #endregion

        #region DVD
        public LibraryItem GetInsertedDvdDisk(int itemId)
        {
            // load item
            VirtualMachine vm = VirtualMachineHelper.GetVirtualMachineByItemId(itemId);

            // get proxy
            VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);
            string isoPath = vs.GetInsertedDVD(vm.VirtualMachineId);

            if (String.IsNullOrEmpty(isoPath))
                return null;

            // load library items
            LibraryItem[] disks = GetLibraryDisks(itemId);

            // find required disk
            isoPath = Path.GetFileName(isoPath);
            foreach (LibraryItem disk in disks)
            {
                if (String.Compare(isoPath, disk.Path, true) == 0)
                    return disk;
            }
            return null;
        }

        public LibraryItem[] GetLibraryDisks(int itemId)
        {
            // load item
            VirtualMachine vm = VirtualMachineHelper.GetVirtualMachineByItemId(itemId);

            // load service settings
            StringDictionary settings = ServerController.GetServiceSettings(vm.ServiceId);
            string xml = settings["DvdLibrary"];

            var config = new ConfigFile(xml);

            return config.LibraryItems;
        }

        public ResultObject InsertDvdDisk(int itemId, string isoPath)
        {
            ResultObject res = new ResultObject();

            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                return res;
            }

            #region Check account and space statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, vm.PackageId, DemandPackage.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("VPS2012", "INSERT_DVD_DISK", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // load service settings
                StringDictionary settings = ServerController.GetServiceSettings(vm.ServiceId);
                string libPath = settings["DvdLibraryPath"];

                // combine full path
                string fullPath = Path.Combine(libPath, isoPath);

                // get proxy
                VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);

                // insert DVD
                JobResult result = vs.InsertDVD(vm.VirtualMachineId, fullPath);
                if (result.ReturnValue != ReturnCode.OK)
                {
                    LogHelper.LogReturnValueResult(res, result);
                    TaskManager.CompleteResultTask(res);
                    return res;
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.INSERT_DVD_DISK_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public ResultObject EjectDvdDisk(int itemId)
        {
            ResultObject res = new ResultObject();

            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                return res;
            }

            #region Check account and space statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, vm.PackageId, DemandPackage.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("VPS2012", "EJECT_DVD_DISK", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // get proxy
                VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);

                // insert DVD
                JobResult result = vs.EjectDVD(vm.VirtualMachineId);
                if (result.ReturnValue != ReturnCode.OK)
                {
                    LogHelper.LogReturnValueResult(res, result);
                    TaskManager.CompleteResultTask(res);
                    return res;
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.EJECT_DVD_DISK_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }
        #endregion

        #region Snaphosts
        public VirtualMachineSnapshot[] GetVirtualMachineSnapshots(int itemId)
        {
            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
                return null;

            // get proxy
            VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);
            return vs.GetVirtualMachineSnapshots(vm.VirtualMachineId);
        }

        public VirtualMachineSnapshot GetSnapshot(int itemId, string snaphostId)
        {
            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
                return null;

            // get proxy
            VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);
            return vs.GetSnapshot(snaphostId);
        }

        public ResultObject CreateSnapshot(int itemId)
        {
            ResultObject res = new ResultObject();

            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                return res;
            }

            #region Check account and space statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, vm.PackageId, DemandPackage.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("VPS2012", "TAKE_SNAPSHOT", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // get proxy
                VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);

                #region Check Quotas
                // check quotas
                List<string> quotaResults = new List<string>();
                PackageContext cntx = PackageController.GetPackageContext(vm.PackageId);

                // check the number of created snapshots
                int createdNumber = vs.GetVirtualMachineSnapshots(vm.VirtualMachineId).Length;
                if (createdNumber >= vm.SnapshotsNumber)
                {
                    TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.QUOTA_EXCEEDED_SNAPSHOTS + ":" + vm.SnapshotsNumber);
                    return res;
                }
                #endregion

                // take snapshot
                JobResult result = vs.CreateSnapshot(vm.VirtualMachineId);
                if (result.ReturnValue != ReturnCode.JobStarted)
                {
                    LogHelper.LogReturnValueResult(res, result);
                    TaskManager.CompleteResultTask(res);
                    return res;
                }

                if (!JobHelper.JobCompleted(vs, result.Job))
                {
                    LogHelper.LogJobResult(res, result.Job);
                    TaskManager.CompleteResultTask(res);
                    return res;
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.TAKE_SNAPSHOT_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public ResultObject ApplySnapshot(int itemId, string snapshotId)
        {
            ResultObject res = new ResultObject();

            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                return res;
            }

            #region Check account and space statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, vm.PackageId, DemandPackage.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("VPS2012", "APPLY_SNAPSHOT", vm.Id, vm.Name, vm.PackageId);

            try
            {
                JobResult result = null;

                // get proxy
                VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);

                // check VM state
                VirtualMachine vps = vs.GetVirtualMachine(vm.VirtualMachineId);

                // stop virtual machine
                if (vps.State != VirtualMachineState.Off)
                {
                    result = vs.ChangeVirtualMachineState(vm.VirtualMachineId, VirtualMachineRequestedState.TurnOff, vm.ClusterName);
                    if (!JobHelper.JobCompleted(vs, result.Job))
                    {
                        LogHelper.LogJobResult(res, result.Job);
                        TaskManager.CompleteResultTask(res);
                        return res;
                    }
                }

                // take snapshot
                result = vs.ApplySnapshot(vm.VirtualMachineId, snapshotId);
                if (result.ReturnValue != ReturnCode.OK)
                {
                    LogHelper.LogReturnValueResult(res, result);
                    TaskManager.CompleteResultTask(res);
                    return res;
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.APPLY_SNAPSHOT_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public ResultObject RenameSnapshot(int itemId, string snapshotId, string newName)
        {
            ResultObject res = new ResultObject();

            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                return res;
            }

            #region Check account and space statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, vm.PackageId, DemandPackage.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("VPS2012", "RENAME_SNAPSHOT", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // get proxy
                VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);

                // take snapshot
                JobResult result = vs.RenameSnapshot(vm.VirtualMachineId, snapshotId, newName);
                if (result.ReturnValue != ReturnCode.OK)
                {
                    LogHelper.LogReturnValueResult(res, result);
                    TaskManager.CompleteResultTask(res);
                    return res;
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.RENAME_SNAPSHOT_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public ResultObject DeleteSnapshot(int itemId, string snapshotId)
        {
            ResultObject res = new ResultObject();

            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                return res;
            }

            #region Check account and space statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, vm.PackageId, DemandPackage.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("VPS2012", "DELETE_SNAPSHOT", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // get proxy
                VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);

                // take snapshot
                JobResult result = vs.DeleteSnapshot(snapshotId);
                if (result.ReturnValue != ReturnCode.JobStarted)
                {
                    LogHelper.LogReturnValueResult(res, result);
                    TaskManager.CompleteResultTask(res);
                    return res;
                }

                if (!JobHelper.JobCompleted(vs, result.Job))
                {
                    LogHelper.LogJobResult(res, result.Job);
                    TaskManager.CompleteResultTask(res);
                    return res;
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.DELETE_SNAPSHOT_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public ResultObject DeleteSnapshotSubtree(int itemId, string snapshotId)
        {
            ResultObject res = new ResultObject();

            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                return res;
            }

            #region Check account and space statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, vm.PackageId, DemandPackage.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("VPS2012", "DELETE_SNAPSHOT_SUBTREE", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // get proxy
                VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);

                // take snapshot
                JobResult result = vs.DeleteSnapshotSubtree(snapshotId);
                if (result.ReturnValue != ReturnCode.JobStarted)
                {
                    LogHelper.LogReturnValueResult(res, result);
                    TaskManager.CompleteResultTask(res);
                    return res;
                }

                if (!JobHelper.JobCompleted(vs, result.Job))
                {
                    LogHelper.LogJobResult(res, result.Job);
                    TaskManager.CompleteResultTask(res);
                    return res;
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.DELETE_SNAPSHOT_SUBTREE_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public byte[] GetSnapshotThumbnail(int itemId, string snapshotId, ThumbnailSize size)
        {
            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
                return null;

            // get proxy
            VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);

            return vs.GetSnapshotThumbnailImage(snapshotId, size);
        }
        #endregion

        #region Network - External
        public AvailableVLANList GetAvailableVLANs(int PackageId)
        {
            throw new NotImplementedException();
            /*
            AvailableVLANList vlanlist = new AvailableVLANList();
            List<int> vlans = new List<int>();
            try
            {

                List<PackageIPAddress> packageips = ServerController.GetPackageUnassignedIPAddresses(PackageId, IPAddressPool.VpsExternalNetwork);
                foreach (PackageIPAddress ip in packageips)
                {
                    vlans.Add(ip.VLAN);
                }

                // return vlan list without dupes
                vlanlist.vlans = vlans.Distinct().ToList();
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex, "VPS_GET_VLAN_ERROR");
            }
            return vlanlist;
            */
        }

        public int GetExternalNetworkVLAN(int itemId)
        {
            return NetworkVLANHelper.GetExternalNetworkVLAN(itemId);
        }

        public NetworkAdapterDetails GetExternalNetworkDetails(int packageId)
        {
            return NetworkAdapterDetailsHelper.GetExternalNetworkDetails(packageId);
        }

        public NetworkAdapterDetails GetExternalNetworkAdapterDetails(int itemId)
        {
            return NetworkAdapterDetailsHelper.GetExternalNetworkAdapterDetails(itemId);
        }

        public ResultObject RestoreVirtualMachineExternalIPAddressesByInjection(int itemId)
        {
            return IpAddressExternalHelper.RestoreVirtualMachineExternalIPAddressesByInjection(itemId);
        }

        public ResultObject AddVirtualMachineExternalIPAddressesByInjection(int itemId, bool selectRandom, int addressesNumber, int[] addressIds)
        {
            return IpAddressExternalHelper.AddVirtualMachineExternalIPAddressesByInjection(itemId, selectRandom, addressesNumber, addressIds);
        }

        public ResultObject AddVirtualMachineExternalIPAddresses(int itemId, bool selectRandom, int addressesNumber, int[] addressIds, bool provisionKvp)
        {
            return IpAddressExternalHelper.AddVirtualMachineExternalIPAddresses(itemId, selectRandom, addressesNumber, addressIds, provisionKvp);
        }

        public ResultObject SetVirtualMachinePrimaryExternalIPAddress(int itemId, int packageAddressId, bool provisionKvp)
        {
            return IpAddressExternalHelper.SetVirtualMachinePrimaryExternalIPAddress(itemId, packageAddressId, provisionKvp);
        }

        public ResultObject DeleteVirtualMachineExternalIPAddressesByInjection(int itemId, int[] packageAddressIds)
        {
            return IpAddressExternalHelper.DeleteVirtualMachineExternalIPAddressesByInjection(itemId, packageAddressIds);
        }

        public ResultObject DeleteVirtualMachineExternalIPAddresses(int itemId, int[] packageAddressIds, bool provisionKvp)
        {
            return IpAddressExternalHelper.DeleteVirtualMachineExternalIPAddresses(itemId, packageAddressIds, provisionKvp);
        }
        #endregion

        #region Network – Private
        public NetworkAdapterDetails GetPrivateNetworkDetails(int packageId)
        {
            return NetworkAdapterDetailsHelper.GetPrivateNetworkDetails(packageId);
        }

        public NetworkAdapterDetails GetPrivateNetworkAdapterDetails(int itemId)
        {
            return NetworkAdapterDetailsHelper.GetPrivateNetworkAdapterDetails(itemId);
        }

        public ResultObject RestoreVirtualMachinePrivateIPAddressesByInjection(int itemId)
        {
            return IpAddressPrivateHelper.RestoreVirtualMachinePrivateIPAddressesByInjection(itemId);
        }

        public ResultObject AddVirtualMachinePrivateIPAddressesByInject(int itemId, bool selectRandom, int addressesNumber, string[] addresses, bool customGatewayAndDns, string gateway, string dns1, string dns2, string subnetMask)
        {
            return IpAddressPrivateHelper.AddVirtualMachinePrivateIPAddressesByInject(itemId, selectRandom, addressesNumber, addresses, customGatewayAndDns, gateway, dns1, dns2, subnetMask);
        }

        public ResultObject AddVirtualMachinePrivateIPAddresses(int itemId, bool selectRandom, int addressesNumber, string[] addresses, bool provisionKvp, bool customGatewayAndDns, string gateway, string dns1, string dns2, string subnetMask)
        {
            return IpAddressPrivateHelper.AddVirtualMachinePrivateIPAddresses(itemId, selectRandom, addressesNumber, addresses, provisionKvp, customGatewayAndDns, gateway, dns1, dns2, subnetMask);
        }

        public ResultObject SetVirtualMachinePrimaryPrivateIPAddress(int itemId, int addressId, bool provisionKvp)
        {
            return IpAddressPrivateHelper.SetVirtualMachinePrimaryPrivateIPAddress(itemId, addressId, provisionKvp);
        }

        public ResultObject DeleteVirtualMachinePrivateIPAddressesByInject(int itemId, int[] addressIds)
        {
            return IpAddressPrivateHelper.DeleteVirtualMachinePrivateIPAddressesByInject(itemId, addressIds);
        }

        public ResultObject DeleteVirtualMachinePrivateIPAddresses(int itemId, int[] addressIds, bool provisionKvp)
        {
            return IpAddressPrivateHelper.DeleteVirtualMachinePrivateIPAddresses(itemId, addressIds, provisionKvp);
        }
        #endregion

        #region Network – DMZ
        public static NetworkAdapterDetails GetDmzNetworkDetails(int packageId)
        {
            return NetworkAdapterDetailsHelper.GetDmzNetworkDetails(packageId);
        }

        public static NetworkAdapterDetails GetDmzNetworkAdapterDetails(int itemId)
        {
            return NetworkAdapterDetailsHelper.GetDmzNetworkAdapterDetails(itemId);
        }

        public static ResultObject AddVirtualMachineDmzIPAddresses(int itemId, bool selectRandom, int addressesNumber, string[] addresses, bool provisionKvp, bool customGatewayAndDns, string gateway, string dns1, string dns2, string subnetMask)
        {
            return IpAddressPrivateHelper.AddVirtualMachineDmzIPAddresses(itemId, selectRandom, addressesNumber, addresses, provisionKvp, customGatewayAndDns, gateway, dns1, dns2, subnetMask);
        }

        public static ResultObject AddVirtualMachineDmzIPAddressesByInject(int itemId, bool selectRandom, int addressesNumber, string[] addresses, bool customGatewayAndDns, string gateway, string dns1, string dns2, string subnetMask)
        {
            return IpAddressPrivateHelper.AddVirtualMachineDmzIPAddressesByInject(itemId, selectRandom, addressesNumber, addresses, customGatewayAndDns, gateway, dns1, dns2, subnetMask);
        }

        public static ResultObject DeleteVirtualMachineDmzIPAddressesByInject(int itemId, int[] addressIds)
        {
            return IpAddressPrivateHelper.DeleteVirtualMachineDmzIPAddressesByInject(itemId, addressIds);
        }

        public static ResultObject DeleteVirtualMachineDmzIPAddresses(int itemId, int[] addressIds, bool provisionKvp)
        {
            return IpAddressPrivateHelper.DeleteVirtualMachineDmzIPAddresses(itemId, addressIds, provisionKvp);
        }

        public static ResultObject RestoreVirtualMachineDmzIPAddressesByInjection(int itemId)
        {
            return IpAddressPrivateHelper.RestoreVirtualMachineDmzIPAddressesByInjection(itemId);
        }

        public static ResultObject SetVirtualMachinePrimaryDmzIPAddress(int itemId, int addressId, bool provisionKvp)
        {
            return IpAddressPrivateHelper.SetVirtualMachinePrimaryDmzIPAddress(itemId, addressId, provisionKvp);
        }
        #endregion

        #region Virtual Machine Permissions
        public List<VirtualMachinePermission> GetVirtualMachinePermissions(int itemId)
        {
            List<VirtualMachinePermission> result = new List<VirtualMachinePermission>();
            return result;
        }

        public int UpdateVirtualMachineUserPermissions(int itemId, VirtualMachinePermission[] permissions)
        {
            // VPS - UPDATE_PERMISSIONS
            return 0;
        }
        #endregion

        #region Secure Boot Templates
        public SecureBootTemplate[] GetSecureBootTemplates(int serviceId, string computerName)
        {
            VirtualizationServer2012 vs = new VirtualizationServer2012();
            ServiceProviderProxy.Init(vs, serviceId);
            return vs.GetSecureBootTemplates(computerName);
        }
        #endregion

        #region Virtual Machine Configuration versions 
        public VMConfigurationVersion[] GetVMConfigurationVersionSupportedList(int serviceId)
        {
            VirtualizationServer2012 vs = new VirtualizationServer2012();
            ServiceProviderProxy.Init(vs, serviceId);
            return vs.GetVMConfigurationVersionSupportedList();
        }
        #endregion

        #region Virtual Switches
        public VirtualSwitch[] GetExternalSwitches(int serviceId, string computerName)
        {
            VirtualizationServer2012 vs = new VirtualizationServer2012();
            ServiceProviderProxy.Init(vs, serviceId);
            return vs.GetExternalSwitches(computerName);
        }

        public VirtualSwitch[] GetExternalSwitchesWMI(int serviceId, string computerName)
        {
            VirtualizationServer2012 vs = new VirtualizationServer2012();
            ServiceProviderProxy.Init(vs, serviceId);
            return vs.GetExternalSwitchesWMI(computerName);
        }

        public VirtualSwitch[] GetInternalSwitches(int serviceId, string computerName)
        {
            VirtualizationServer2012 vs = new VirtualizationServer2012();
            ServiceProviderProxy.Init(vs, serviceId);
            return vs.GetInternalSwitches(computerName);
        }
        #endregion

        #region Tools
        public ResultObject DeleteVirtualMachine(int itemId, bool saveFiles, bool exportVps, string exportPath) //TODO: Is possible to rework method (Duplicated in server)?
        {
            return DeleteVirtualMachineHandler.DeleteVirtualMachine(itemId, saveFiles, exportVps, exportPath);
        }
        public ResultObject DeleteVirtualMachineAsynchronous(int itemId, bool saveFiles, bool exportVps, string exportPath)
        {
            return DeleteVirtualMachineHandler.DeleteVirtualMachineAsynchronous(itemId, saveFiles, exportVps, exportPath);
        }

        public IntResult ReinstallVirtualMachine(int itemId, VirtualMachine VMSettings, string adminPassword, string[] privIps, string[] dmzIps,
            bool saveVirtualDisk, bool exportVps, string exportPath)
        {
            return ReinstallVirtualMachineHandler.ReinstallVirtualMachine(itemId, VMSettings, adminPassword, privIps, dmzIps, saveVirtualDisk, exportVps, exportPath);
        }

        //TODO: Add another reinstall method.
        //public int ReinstallVirtualMachine(int itemId, string adminPassword, bool preserveVirtualDiskFiles,
        //    bool saveVirtualDisk, bool exportVps, string exportPath)
        //{

        //    return 0;
        //}
        #endregion

        #region Help
        public string GetVirtualMachineSummaryText(int itemId, bool emailMode, bool creation)
        {
            // load item
            VirtualMachine vm = VirtualMachineHelper.GetVirtualMachineByItemId(itemId);

            // load user info
            UserInfo user = PackageController.GetPackageOwner(vm.PackageId);

            // get letter settings
            UserSettings settings = UserController.GetUserSettings(user.UserId, UserSettings.VPS_SUMMARY_LETTER);

            string settingName = user.HtmlMail ? "HtmlBody" : "TextBody";
            string body = settings[settingName];
            if (String.IsNullOrEmpty(body))
                return null;

            string result = EvaluateVirtualMachineTemplate(itemId, emailMode, creation, body);
            return user.HtmlMail ? result : result.Replace("\n", "<br/>");
        }

        public ResultObject SendVirtualMachineSummaryLetter(int itemId, string to, string bcc, bool creation)
        {
            ResultObject res = new ResultObject();

            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                return res;
            }

            #region Check account and space statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, vm.PackageId, DemandPackage.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("VPS", "SEND_SUMMARY_LETTER", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // load user info
                UserInfo user = PackageController.GetPackageOwner(vm.PackageId);

                // get letter settings
                UserSettings settings = UserController.GetUserSettings(user.UserId, UserSettings.VPS_SUMMARY_LETTER);

                string from = settings["From"];
                if (bcc == null)
                    bcc = settings["CC"];
                string subject = settings["Subject"];
                string body = user.HtmlMail ? settings["HtmlBody"] : settings["TextBody"];
                bool isHtml = user.HtmlMail;

                MailPriority priority = MailPriority.Normal;
                if (!String.IsNullOrEmpty(settings["Priority"]))
                    priority = (MailPriority)Enum.Parse(typeof(MailPriority), settings["Priority"], true);

                if (String.IsNullOrEmpty(body))
                {
                    TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.SUMMARY_TEMPLATE_IS_EMPTY);
                    return res;
                }

                // load user info
                if (to == null)
                    to = user.Email;

                subject = EvaluateVirtualMachineTemplate(itemId, true, creation, subject);
                body = EvaluateVirtualMachineTemplate(itemId, true, creation, body);

                // send message
                int result = MailHelper.SendMessage(from, to, bcc, subject, body, priority, isHtml);

                if (result != 0)
                {
                    TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.SEND_SUMMARY_LETTER_CODE + ":" + result);
                    TaskManager.WriteWarning("VPS_SEND_SUMMARY_LETTER_ERROR_CODE", result.ToString());
                    return res;
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.SEND_SUMMARY_LETTER, ex);
                TaskManager.WriteWarning("VPS_SEND_SUMMARY_LETTER_ERROR", ex.Message);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public string EvaluateVirtualMachineTemplate(int itemId, bool emailMode, bool creation, string template)
        {
            return VirtualMachineHelper.EvaluateVirtualMachineTemplate(itemId, emailMode, creation, template);
        }

        public VirtualMachine GetVirtualMachineByItemId(int itemId)
        {
            return VirtualMachineHelper.GetVirtualMachineByItemId(itemId);
        }

        public string GenerateMacAddress()
        {
            return NetworkHelper.GenerateMacAddress(); //MS_MAC_PREFIX + Utils.GetRandomHexString(3);
        }

        #endregion


        #region Replication

        #region IsReplicaServer Part

        public CertificateInfo[] GetCertificates(int serviceId, string remoteServer)
        {
            VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(serviceId);
            return vs.GetCertificates(remoteServer);
        }

        public ResultObject SetReplicaServer(int serviceId, string remoteServer, string thumbprint, string storagePath)
        {
            ResultObject result = new ResultObject();
            try
            {
                if (string.IsNullOrEmpty(storagePath))
                    throw new Exception("Please enter replication path");

                VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(serviceId);
                vs.SetReplicaServer(remoteServer, thumbprint, storagePath);
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.AddError(VirtualizationErrorCodes.SET_REPLICA_SERVER_ERROR, ex);
            }
            return result;
        }

        public ResultObject UnsetReplicaServer(int serviceId, string remoteServer)
        {
            ResultObject result = new ResultObject();
            try
            {
                VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(serviceId);
                vs.UnsetReplicaServer(remoteServer);
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.AddError(VirtualizationErrorCodes.UNSET_REPLICA_SERVER_ERROR, ex);
            }
            return result;
        }

        public ReplicationServerInfo GetReplicaServer(int serviceId, string remoteServer)
        {
            VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(serviceId);
            return vs.GetReplicaServer(remoteServer);
        }

        #endregion

        public VmReplication GetReplication(int itemId)
        {
            VirtualMachine vm = VirtualMachineHelper.GetVirtualMachineByItemId(itemId);
            VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);
            return vs.GetReplication(vm.VirtualMachineId);
        }

        public ReplicationDetailInfo GetReplicationInfo(int itemId)
        {
            VirtualMachine vm = VirtualMachineHelper.GetVirtualMachineByItemId(itemId);
            VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);
            return vs.GetReplicationInfo(vm.VirtualMachineId);
        }

        public ResultObject SetVmReplication(int itemId, VmReplication replication)
        {
            TaskManager.StartTask("VPS2012", "SetVmReplication");

            ResultObject result = new ResultObject();


            try
            {
                VirtualMachine vm = VirtualMachineHelper.GetVirtualMachineByItemId(itemId);

                // Check Quotas
                ReplicationHelper.CheckReplicationQuota(vm.PackageId, ref result);
                if (result.ErrorCodes.Count > 0)
                    return result;

                VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);

                // Get replica server
                var replicaServerInfo = ReplicationHelper.GetReplicaInfoForService(vm.ServiceId, ref result);
                if (result.ErrorCodes.Count > 0) return result;

                // We should use enable replication or set replication?
                var vmReplica = vs.GetReplication(vm.VirtualMachineId);
                if (vmReplica == null) // need enable
                {
                    vs.EnableVmReplication(vm.VirtualMachineId, replicaServerInfo.ComputerName, replication);
                    vs.StartInitialReplication(vm.VirtualMachineId);
                }
                else // need set
                {
                    vs.SetVmReplication(vm.VirtualMachineId, replicaServerInfo.ComputerName, replication);
                }
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
            TaskManager.WriteWarning("Organization with itemId '{0}' not found", itemId.ToString());
            return result;
        }

        public ResultObject DisableVmReplication(int itemId)
        {
            ResultObject result = new ResultObject();
            try
            {
                VirtualMachine vm = VirtualMachineHelper.GetVirtualMachineByItemId(itemId);

                // Check Quotas
                ReplicationHelper.CheckReplicationQuota(vm.PackageId, ref result);
                if (result.ErrorCodes.Count > 0) return result;

                VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);
                vs.DisableVmReplication(vm.VirtualMachineId);

                ReplicationHelper.CleanUpReplicaServer(vm);

                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.AddError(VirtualizationErrorCodes.DISABLE_REPLICATION_ERROR, ex);
            }
            return result;
        }

        public ResultObject PauseReplication(int itemId)
        {
            ResultObject result = new ResultObject();
            try
            {
                VirtualMachine vm = VirtualMachineHelper.GetVirtualMachineByItemId(itemId);

                // Check Quotas
                ReplicationHelper.CheckReplicationQuota(vm.PackageId, ref result);
                if (result.ErrorCodes.Count > 0) return result;

                VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);
                vs.PauseReplication(vm.VirtualMachineId);

                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.AddError(VirtualizationErrorCodes.PAUSE_REPLICATION_ERROR, ex);
            }
            return result;
        }

        public ResultObject ResumeReplication(int itemId)
        {
            ResultObject result = new ResultObject();
            try
            {
                VirtualMachine vm = VirtualMachineHelper.GetVirtualMachineByItemId(itemId);

                // Check Quotas
                ReplicationHelper.CheckReplicationQuota(vm.PackageId, ref result);
                if (result.ErrorCodes.Count > 0) return result;

                VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);
                vs.ResumeReplication(vm.VirtualMachineId);

                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.AddError(VirtualizationErrorCodes.RESUME_REPLICATION_ERROR, ex);
            }
            return result;
        }

        #endregion

        #region PsScripts
        //public enum PsScriptPoint
        //{
        //    disabled,
        //    after_creation,
        //    before_deletion,
        //    before_renaming,
        //    after_renaming,
        //    external_network_configuration,
        //    private_network_configuration,
        //    management_network_configuration
        //}

        //public void CheckCustomPsScript(PsScriptPoint point, VirtualMachine vm)
        //{
        //    PowerShellScript.CheckCustomPsScript(point, vm);
        //}
        #endregion

    }
}
