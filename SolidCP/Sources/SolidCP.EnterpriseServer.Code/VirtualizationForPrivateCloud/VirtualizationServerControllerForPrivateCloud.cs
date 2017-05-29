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
using System.Xml;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.Virtualization;
using SolidCP.Providers.VirtualizationForPC;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using SolidCP.Providers;
using System.Text;
using System.Collections;
using System.Net.Mail;

namespace SolidCP.EnterpriseServer
{
    public class VirtualizationServerControllerForPrivateCloud
    {
        private const string SHUTDOWN_REASON = "SolidCP - Initiated by user";
        private const string SHUTDOWN_REASON_CHANGE_CONFIG = "SolidCP - changing VPS configuration";
        private const Int64 Size1G = 0x40000000;
        private const string MS_MAC_PREFIX = "00155D"; // IEEE prefix of MS MAC addresses

        // default server creation (if "Unlimited" was specified in the hosting plan)
        private const int DEFAULT_PASSWORD_LENGTH = 12;
        private const int DEFAULT_RAM_SIZE = 512; // megabytes
        private const int DEFAULT_HDD_SIZE = 20; // gigabytes
        private const int DEFAULT_PRIVATE_IPS_NUMBER = 1;
        private const int DEFAULT_SNAPSHOTS_NUMBER = 5;

        #region Huper-V Cloud
        public static bool CheckServerState(VMForPCSettingsName control, string connString, string connName, int serviceId)
        {
			var ws = new VirtualizationServerForPrivateCloud();
            ServiceProviderProxy.Init(ws, serviceId);
            return ws.CheckServerState(control, connString, connName);

        }
        #endregion Huper-V Cloud

        #region Virtual Machines
        public static VirtualMachineMetaItemsPaged GetVirtualMachines(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            VirtualMachineMetaItemsPaged result = new VirtualMachineMetaItemsPaged();

            // get reader
            IDataReader reader = DataProvider.GetVirtualMachinesForPCPaged(
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

        public static VirtualMachine[] GetVirtualMachinesByServiceId(int serviceId)
        {
            // get proxy
            var vps = GetVirtualizationProxy(serviceId);

            // load details
            return vps.GetVirtualMachines();
        }
        #endregion

        #region Private Network
        public static PrivateIPAddressesPaged GetPackagePrivateIPAddressesPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            PrivateIPAddressesPaged result = new PrivateIPAddressesPaged();

            // get reader
            IDataReader reader = DataProvider.GetPackagePrivateIPAddressesPaged(packageId, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);

            // number of items = first data reader
            reader.Read();
            result.Count = (int)reader[0];

            // items = second data reader
            reader.NextResult();
            result.Items = ObjectUtils.CreateListFromDataReader<PrivateIPAddress>(reader).ToArray();

            return result;
        }

        public static List<PrivateIPAddress> GetPackagePrivateIPAddresses(int packageId)
        {
            return ObjectUtils.CreateListFromDataReader<PrivateIPAddress>(
                DataProvider.GetPackagePrivateIPAddresses(packageId));
        }
        #endregion

        #region User Permissions
        public static List<VirtualMachinePermission> GetSpaceUserPermissions(int packageId)
        {
            List<VirtualMachinePermission> result = new List<VirtualMachinePermission>();
            return result;
        }

        public static int UpdateSpaceUserPermissions(int packageId, VirtualMachinePermission[] permissions)
        {
            // VDC - UPDATE_PERMISSIONS
            return 0;
        }
        #endregion

        #region Audit Log
        public static List<LogRecord> GetSpaceAuditLog(int packageId, DateTime startPeriod, DateTime endPeriod,
            int severity, string sortColumn, int startRow, int maximumRows)
        {
            List<LogRecord> result = new List<LogRecord>();
            return result;
        }

        public static List<LogRecord> GetVirtualMachineAuditLog(int itemId, DateTime startPeriod, DateTime endPeriod,
            int severity, string sortColumn, int startRow, int maximumRows)
        {
            List<LogRecord> result = new List<LogRecord>();
            return result;
        }
        #endregion

        #region VPS Create – Name & OS
        public static LibraryItem[] GetOperatingSystemTemplates(int packageId)
        {
            // load service settings
            int serviceId = GetServiceId(packageId);

            // return templates
            return GetOperatingSystemTemplatesByServiceId(serviceId);
        }

        public static LibraryItem[] GetOperatingSystemTemplatesByServiceId(int serviceId)
        {
                // get proxy
                var vs = GetVirtualizationProxy(serviceId);
                return vs.GetOSLibraryItems();
        }
        #endregion


        public static LibraryItem[] GetClusters(int serviceId)
        {
            // get Hosts
            var vs = GetVirtualizationProxy(serviceId);
            return vs.GetClusters();
        }

        public static LibraryItem[] GetHosts(int serviceId)
        {
            // get Hosts
            var vs = GetVirtualizationProxy(serviceId);
            return vs.GetHosts();
        }


        #region VPS Create - Configuration
        public static int GetMaximumCpuCoresNumber(int packageId, string templateId)
        {
            // get proxy
            var vs = GetVirtualizationProxyByPackageId(packageId);

            return vs.GetProcessorCoresNumber(templateId);
        }

        public static string GetDefaultExportPath(int itemId)
        {
            // load meta item
            VMInfo vm = GetVirtualMachineByItemId(itemId);

            if (vm == null)
                return null;

            // load settings
            StringDictionary settings = ServerController.GetServiceSettings(vm.ServiceId);
            return settings["ExportedVpsPath"];
        }
        #endregion

        //#region VPS Create
        public static ResultObject CreateVMFromVM(int packageId, VMInfo vmTemplate, string vmName)
        {
            ResultObject res = new ResultObject();

            // load service item
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, packageId, DemandPackage.IsActive))
                return res;

            #region Check if host name is already used

            try
            {
                ServiceProviderItem item = PackageController.GetPackageItemByName(packageId, vmName,
                                                                                  typeof(VMInfo));

                if (item != null)
                {
                    res.ErrorCodes.Add(VirtualizationErrorCodes.HOST_NAMER_IS_ALREADY_USED);
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.AddError(VirtualizationErrorCodes.CANNOT_CHECK_HOST_EXISTS, ex);
                return res;
            }

            #endregion

            try
            {
                CreateVMFromVMAsyncWorker worker = new CreateVMFromVMAsyncWorker();

                worker.ThreadUserId = SecurityContext.User.UserId;
                worker.vmTemplate = vmTemplate;
                worker.vmName = vmName;
                worker.packageId = packageId;

                worker.CreateAsync();
            }
            catch (Exception ex)
            {
                res.AddError(VirtualizationErrorCodes.CREATE_ERROR, ex);
                return res;
            }

            res.IsSuccess = true;
            return res;
        }


        public static void CreateVMFromVMAsunc(int packageId, VMInfo vmTemplate, string vmName)
        {
            ResultObject res = new ResultObject();

            Guid taksId = Guid.NewGuid();

            res = TaskManager.StartResultTask<ResultObject>("VPSForPC", "CREATE", taksId, vmName, packageId);

            string templateName = vmTemplate.Name;

            vmTemplate.Id = 0;
            vmTemplate.Name = vmName;
            vmTemplate.CurrentTaskId = taksId.ToString("N"); // generate creation task id
            vmTemplate.ProvisioningStatus = VirtualMachineProvisioningStatus.InProgress;

            try
            {
                vmTemplate.Id = PackageController.AddPackageItem(vmTemplate);
            }
            catch (Exception ex)
            {
                res.AddError(VirtualizationErrorCodes.CREATE_META_ITEM_ERROR, ex);
                TaskManager.CompleteResultTask(res);
                throw;
            }

            TaskManager.ItemId = vmTemplate.Id;

            try
            {
                var vs = GetVirtualizationProxy(vmTemplate.ServiceId);
                vmTemplate = vs.CreateVMFromVM(templateName, vmTemplate, taksId);
                PackageController.UpdatePackageItem(vmTemplate);

//                    CreateAsyncVMfromVM.Run(templateName, vmTemplate);
            }
            catch (Exception ex)
            {
                // delete meta item
                PackageController.DeletePackageItem(vmTemplate.Id);

                // return from method
                res.AddError(VirtualizationErrorCodes.CREATE_TASK_START_ERROR, ex);
                TaskManager.CompleteResultTask(res);
                throw;
            }

            if (!String.IsNullOrEmpty(vmTemplate.exMessage))
            {
                TaskManager.CompleteResultTask(res
                            , VirtualizationErrorCodes.CREATE_ERROR
                            , new Exception(vmTemplate.exMessage)
                            , vmTemplate.logMessage);
            }
            else
            {
                TaskManager.CompleteResultTask(res, null, null, vmTemplate.logMessage);
            }
        }

        public static IntResult CreateVirtualMachine(int packageId,
                string hostname, string domain, string osTemplateFile, string vmName, string password, string summaryLetterEmail,
                int cpuCores, int ramMB, int hddGB, int snapshots,
                bool dvdInstalled, bool bootFromCD, bool numLock,
                bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed,
                bool externalNetworkEnabled, string externalNetworkLocation, string externalNicMacAddress, string externalVirtualNetwork,
                bool privateNetworkEnabled, string privateNetworkLocation, string privateNicMacAddress, string privateVirtualNetwork, ushort privateVLanID)
        {
            // result object
            IntResult res = new IntResult();

            //Create Task 
            res = TaskManager.StartResultTask<IntResult>("VPSForPC", "CREATE", vmName, packageId);

            // meta item
            VMInfo vmInfo = null;

            try
            {
                #region Check account and space statuses
                // check account
                if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                {
                    res.ErrorCodes.Add(VirtualizationErrorCodes.CREATE_ERROR);
                    TaskManager.CompleteResultTask(res);
                    return res;
                }
                // check package
                if (!SecurityContext.CheckPackage(res, packageId, DemandPackage.IsActive))
                {
                    res.ErrorCodes.Add(VirtualizationErrorCodes.CREATE_ERROR);
                    TaskManager.CompleteResultTask(res);
                    return res;
                }
                
                #endregion

                #region Check if host name is already used

                try
                {
                    string itemName = (String.IsNullOrEmpty(hostname) && String.IsNullOrEmpty(domain) ? vmName: String.Format("{0}.{1}", hostname, domain));
                    ServiceProviderItem item = PackageController.GetPackageItemByName(packageId, itemName,
                                                                                      typeof (VMInfo));

                    if (item != null)
                    {
                        res.ErrorCodes.Add(VirtualizationErrorCodes.HOST_NAMER_IS_ALREADY_USED);
                        TaskManager.CompleteResultTask(res);
                        return res;
                    }
                }
                catch(Exception ex)
                {
                    res.AddError(VirtualizationErrorCodes.CANNOT_CHECK_HOST_EXISTS, ex);
                    TaskManager.CompleteResultTask(res);
                    return res;   
                }

                #endregion

                #region Check Quotas
                // check quotas
                List<string> quotaResults = new List<string>();
                PackageContext cntx = PackageController.GetPackageContext(packageId);


                CheckListsQuota(cntx, quotaResults, Quotas.VPSForPC_SERVERS_NUMBER, VirtualizationErrorCodes.QUOTA_EXCEEDED_SERVERS_NUMBER);

                CheckNumericQuota(cntx, quotaResults, Quotas.VPSForPC_CPU_NUMBER, cpuCores, VirtualizationErrorCodes.QUOTA_EXCEEDED_CPU);
                CheckNumericQuota(cntx, quotaResults, Quotas.VPSForPC_RAM, ramMB, VirtualizationErrorCodes.QUOTA_EXCEEDED_RAM);
                CheckNumericQuota(cntx, quotaResults, Quotas.VPSForPC_HDD, hddGB, VirtualizationErrorCodes.QUOTA_EXCEEDED_HDD);
				//CheckNumericQuota(cntx, quotaResults, Quotas.VPSForPC_SNAPSHOTS_NUMBER, snapshots, VirtualizationErrorCodes.QUOTA_EXCEEDED_SNAPSHOTS);

				//CheckBooleanQuota(cntx, quotaResults, Quotas.VPSForPC_DVD_ENABLED, dvdInstalled, VirtualizationErrorCodes.QUOTA_EXCEEDED_DVD_ENABLED);
				//CheckBooleanQuota(cntx, quotaResults, Quotas.VPSForPC_BOOT_CD_ALLOWED, bootFromCD, VirtualizationErrorCodes.QUOTA_EXCEEDED_CD_ALLOWED);

                CheckBooleanQuota(cntx, quotaResults, Quotas.VPSForPC_START_SHUTDOWN_ALLOWED, startShutdownAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_START_SHUTDOWN_ALLOWED);
                CheckBooleanQuota(cntx, quotaResults, Quotas.VPSForPC_PAUSE_RESUME_ALLOWED, pauseResumeAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_PAUSE_RESUME_ALLOWED);
                CheckBooleanQuota(cntx, quotaResults, Quotas.VPSForPC_REBOOT_ALLOWED, rebootAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_REBOOT_ALLOWED);
                CheckBooleanQuota(cntx, quotaResults, Quotas.VPSForPC_RESET_ALOWED, resetAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_RESET_ALOWED);
                CheckBooleanQuota(cntx, quotaResults, Quotas.VPSForPC_REINSTALL_ALLOWED, reinstallAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_REINSTALL_ALLOWED);

                CheckBooleanQuota(cntx, quotaResults, Quotas.VPSForPC_EXTERNAL_NETWORK_ENABLED, externalNetworkEnabled, VirtualizationErrorCodes.QUOTA_EXCEEDED_EXTERNAL_NETWORK_ENABLED);
                CheckBooleanQuota(cntx, quotaResults, Quotas.VPSForPC_PRIVATE_NETWORK_ENABLED, privateNetworkEnabled, VirtualizationErrorCodes.QUOTA_EXCEEDED_PRIVATE_NETWORK_ENABLED);

                // check management network parameters
                NetworkAdapterDetails manageNic = GetManagementNetworkDetails(packageId);
                if (!String.IsNullOrEmpty(manageNic.NetworkId))
                {
                    // network enabled - check management IPs pool
                    int manageIpsNumber = ServerController.GetUnallottedIPAddresses(
                            packageId, ResourceGroups.VPSForPC, IPAddressPool.VpsManagementNetwork).Count;

                    if (manageIpsNumber == 0)
                        quotaResults.Add(VirtualizationErrorCodes.QUOTA_EXCEEDED_MANAGEMENT_NETWORK);
                }

                // check acceptable values
                if (ramMB <= 0)
                    quotaResults.Add(VirtualizationErrorCodes.QUOTA_WRONG_RAM);
                if (hddGB <= 0)
                    quotaResults.Add(VirtualizationErrorCodes.QUOTA_WRONG_HDD);
				//if (snapshots < 0)
				//    quotaResults.Add(VirtualizationErrorCodes.QUOTA_WRONG_SNAPSHOTS);

                if (quotaResults.Count > 0)
                {
                    res.ErrorCodes.AddRange(quotaResults);
                    TaskManager.CompleteResultTask(res);
                    return res;
                }
                #endregion

                #region Context variables
                // service ID
                int serviceId = GetServiceId(packageId);

                // load service settings
                StringDictionary settings = ServerController.GetServiceSettings(serviceId);

                PackageInfo pk = PackageController.GetPackage(packageId);
                UserInfo user = UserController.GetUser(pk.UserId);
                #endregion

                #region Create meta item
                // create meta item

                vmInfo = new VMInfo();

//                vmInfo.AdminUserName = user.Username;
                vmInfo.AdminUserName = null;
                vmInfo.AdminPassword = password;
                vmInfo.DvdDriver = dvdInstalled;
                vmInfo.HostName = hostname;
                vmInfo.CPUCount = cpuCores;
                vmInfo.JoinDomain = (String.IsNullOrWhiteSpace(domain) ? null : domain);
                vmInfo.JoinDomainPassword = null;
                vmInfo.JoinDomainUserName = null;
                vmInfo.Memory = ramMB;
                vmInfo.NumLockEnabled = numLock;
                vmInfo.HddSize = hddGB;

                vmInfo.ProductKey = null;
                vmInfo.TemplateId = new Guid(osTemplateFile);
                vmInfo.VmPath = settings["RootFolder"];
                vmInfo.ComputerName = vmName;
                vmInfo.Owner = null;

                vmInfo.PackageId = packageId;
                vmInfo.ServiceId = serviceId;
                vmInfo.Name = vmName;

                vmInfo.StartTurnOffAllowed = startShutdownAllowed;
                vmInfo.PauseResumeAllowed = pauseResumeAllowed;
                vmInfo.RebootAllowed = rebootAllowed;
                vmInfo.ResetAllowed = resetAllowed;
                vmInfo.ReinstallAllowed = reinstallAllowed;

                vmInfo.BootFromCD = bootFromCD;

                //vmInfo.CurrentTaskId = Guid.NewGuid().ToString("N"); // generate creation task id
                vmInfo.ProvisioningStatus = VirtualMachineProvisioningStatus.InProgress;

                // networking
                vmInfo.ExternalNetworkEnabled = externalNetworkEnabled;
                vmInfo.ExternalNetworkLocation = externalNetworkLocation;
                vmInfo.ExternalNicMacAddress = externalNicMacAddress;
                vmInfo.ExternalVirtualNetwork = string.IsNullOrEmpty(externalVirtualNetwork) ? settings["ExternalNetworkName"] : externalVirtualNetwork;

                vmInfo.PrivateNetworkEnabled = privateNetworkEnabled;
                vmInfo.PrivateNetworkLocation = privateNetworkLocation;
                vmInfo.PrivateNicMacAddress = privateNicMacAddress;
                vmInfo.PrivateVLanID = privateVLanID;
                vmInfo.PrivateVirtualNetwork = string.IsNullOrEmpty(privateVirtualNetwork) ? settings["PrivateNetworkName"] : privateVirtualNetwork;
                
                // save meta-item
                try
                {
                    vmInfo.VmId = PackageController.AddPackageItem(vmInfo);
                    vmInfo.Id = vmInfo.VmId;

                    TaskManager.ItemId = vmInfo.VmId;
                }
                catch (Exception ex)
                {
                    res.AddError(VirtualizationErrorCodes.CREATE_META_ITEM_ERROR, ex);
                    TaskManager.CompleteResultTask(res);
                    return res;
                }
                
                #endregion

                #region Start Asynchronous task
                try
                {
//                    CreateServerAsyncWorkerForPrivateCloud worker = new CreateServerAsyncWorkerForPrivateCloud();
                    CreateVMAsyncWorker worker = new CreateVMAsyncWorker();
                    worker.vmTemplate = vmInfo;
                    worker.ThreadUserId = SecurityContext.User.UserId;
                    worker.CreateAsync();

//                    CreateServerAsyncWorkerForPrivateCloud.Run(vmInfo);

//                    worker.Run(vmInfo);
                }
                catch (Exception ex)
                {
                    // delete meta item
                    PackageController.DeletePackageItem(vmInfo.VmId);
                    // return from method
                    res.AddError(VirtualizationErrorCodes.CREATE_TASK_START_ERROR, ex);
                    TaskManager.CompleteResultTask(res);
                    return res;
                }
                #endregion
            }
            catch (Exception ex)
            {
                res.AddError(VirtualizationErrorCodes.CREATE_ERROR, ex);
                TaskManager.CompleteResultTask(res);
                return res;
            }

            res.Value = vmInfo.VmId;
            res.IsSuccess = true;
            TaskManager.CompleteTask();
            return res;
        }


        internal static void CreateVirtualMachineAsunc(VMInfo vm)
        {
             Guid taksId = Guid.NewGuid();

            ResultObject taskInfo = TaskManager.StartResultTask<ResultObject>("VPSForPC", "CREATE", taksId, vm.Id, vm.Name, vm.PackageId);

            vm.CurrentTaskId = taksId.ToString("N");

            try
            {
                var vs = GetVirtualizationProxy(vm.ServiceId);

                vm = vs.CreateVirtualMachine(vm);
                PackageController.UpdatePackageItem(vm);
            }
            catch(Exception ex)
            {
                // return from method
                taskInfo.AddError(VirtualizationErrorCodes.CREATE_TASK_START_ERROR, ex);
                TaskManager.CompleteResultTask(taskInfo);
                throw;
            }

            if (!String.IsNullOrEmpty(vm.exMessage))
            {
                TaskManager.CompleteResultTask(taskInfo
                            , VirtualizationErrorCodes.CREATE_ERROR
                            , new Exception(vm.exMessage)
                            , vm.logMessage);
            }
            else
            {
                TaskManager.CompleteResultTask(taskInfo, null, null, vm.logMessage);
            }
        }


        //internal static void CreateVirtualMachineInternal(string taskId, VMInfo vm, string TemplateId,
        //        int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses,
        //        int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses,
        //        string summaryLetterEmail)
        //{
        //    // start task
        //    TaskManager.StartTask(taskId, "VPSForPC", "CREATE", vm.Name);
        //    TaskManager.ItemId = vm.Id;
        //    TaskManager.PackageId = vm.PackageId;

        //    try
        //    {
        //        // set Error flag
        //        vm.ProvisioningStatus = VirtualMachineProvisioningStatus.Error;

        //        // load proxy
        //        VirtualizationServerForPC vs = GetVirtualizationProxy(vm.ServiceId);

        //        // load service settings
        //        StringDictionary settings = ServerController.GetServiceSettings(vm.ServiceId);

        //        #region Setup External network
        //        TaskManager.Write("VPSForPC_CREATE_SETUP_EXTERNAL_NETWORK");

        //        try
        //        {
        //            if (vm.ExternalNetworkEnabled)
        //            {
        //                // provision IP addresses
        //                ResultObject privResult = AddVirtualMachineExternalIPAddresses(vm.Id, randomExternalAddresses,
        //                    externalAddressesNumber, externalAddresses, false);

        //                // set primary IP address
        //                NetworkAdapterDetails extNic = GetExternalNetworkAdapterDetails(vm.Id);
        //                if (extNic.IPAddresses.Length > 0)
        //                    SetVirtualMachinePrimaryExternalIPAddress(vm.Id, extNic.IPAddresses[0].AddressId, false);

        //                // connect to network
        //                vm.ExternalNicMacAddress = GenerateMacAddress();
        //            }
        //            else
        //            {
        //                TaskManager.Write("VPSForPC_CREATE_SETUP_EXTERNAL_NETWORK_SKIP");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            TaskManager.WriteError(ex, "VPSForPC_CREATE_SETUP_EXTERNAL_NETWORK_ERROR");
        //            TaskManager.CompleteTask();
        //            return;
        //        }
        //        #endregion

        //        #region Setup Management network
        //        TaskManager.Write("VPS_CREATE_SETUP_MANAGEMENT_NETWORK");

        //        try
        //        {
        //            if (vm.ManagementNetworkEnabled)
        //            {
        //                // check that package contains unassigned IP
        //                // that could be re-used
        //                List<PackageIPAddress> packageIps = ServerController.GetPackageUnassignedIPAddresses(
        //                    vm.PackageId, IPAddressPool.VpsManagementNetwork);

        //                if (packageIps.Count == 0)
        //                {
        //                    // must be fresh space
        //                    // allocate package IP from the pool
        //                    List<IPAddressInfo> ips = ServerController.GetUnallottedIPAddresses(
        //                        vm.PackageId, ResourceGroups.VPSForPC, IPAddressPool.VpsManagementNetwork);

        //                    if (ips.Count > 0)
        //                    {
        //                        // assign IP to the package
        //                        ServerController.AllocatePackageIPAddresses(vm.PackageId, new int[] { ips[0].AddressId });

        //                        // re-read package IPs
        //                        packageIps = ServerController.GetPackageUnassignedIPAddresses(
        //                                        vm.PackageId, IPAddressPool.VpsManagementNetwork);
        //                    }
        //                    else
        //                    {
        //                        // nothing to allocate - pool empty
        //                        TaskManager.WriteWarning("VPSForPC_CREATE_SETUP_MANAGEMENT_NETWORK_POOL_EMPTY");
        //                    }
        //                }

        //                if (packageIps.Count > 0)
        //                {
        //                    // assign to the item
        //                    ServerController.AddItemIPAddress(vm.Id, packageIps[0].PackageAddressID);

        //                    // set primary IP address
        //                    ServerController.SetItemPrimaryIPAddress(vm.Id, packageIps[0].PackageAddressID);

        //                    // connect to network
        //                    vm.ManagementNicMacAddress = GenerateMacAddress();
        //                }
        //            }
        //            else
        //            {
        //                TaskManager.Write("VPSForPC_CREATE_SETUP_MANAGEMENT_NETWORK_SKIP");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            TaskManager.WriteError(ex, "VPSForPC_CREATE_SETUP_MANAGEMENT_NETWORK_ERROR");
        //            TaskManager.CompleteTask();
        //            return;
        //        }
        //        #endregion

        //        #region Setup Private network
        //        TaskManager.Write("VPSForPC_CREATE_SETUP_PRIVATE_NETWORK");

        //        try
        //        {
        //            if (vm.PrivateNetworkEnabled)
        //            {
        //                NetworkAdapterDetails privNic = GetPrivateNetworkDetailsInternal(vm.ServiceId);

        //                if (!privNic.IsDHCP)
        //                {
        //                    // provision IP addresses
        //                    ResultObject extResult = AddVirtualMachinePrivateIPAddresses(vm.Id, randomPrivateAddresses, privateAddressesNumber, privateAddresses, false);

        //                    // set primary IP address
        //                    privNic = GetPrivateNetworkAdapterDetails(vm.Id);
        //                    if (privNic.IPAddresses.Length > 0)
        //                        SetVirtualMachinePrimaryPrivateIPAddress(vm.Id, privNic.IPAddresses[0].AddressId, false);
        //                }

        //                vm.PrivateNicMacAddress = GenerateMacAddress();
        //            }
        //            else
        //            {
        //                TaskManager.Write("VPSForPC_CREATE_SETUP_PRIVATE_NETWORK_SKIP");
        //                TaskManager.CompleteTask();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            TaskManager.WriteError(ex, "VPSForPC_CREATE_SETUP_PRIVATE_NETWORK_ERROR");
        //            TaskManager.CompleteTask();
        //            return;
        //        }
        //        #endregion

        //        // update service item
        //        VirtualMachineProvisioningStatus status = vm.ProvisioningStatus;
        //        vm.ProvisioningStatus = VirtualMachineProvisioningStatus.InProgress;
        //        PackageController.UpdatePackageItem(vm);
        //        vm.ProvisioningStatus = status;

        //        #region Copy/convert VHD
        //        JobResult result = null;
        //        ReturnCode code = ReturnCode.OK;
        //        TaskManager.Write("VPS_CREATE_OS_TEMPLATE", TemplateId);
        //        TaskManager.Write("VPS_CREATE_CONVERT_VHD");
        //        TaskManager.Write("VPS_CREATE_CONVERT_SOURCE_VHD", vm.VmPath);
        //        try
        //        {
        //            // convert VHD
        //            VirtualHardDiskType vhdType = (VirtualHardDiskType)Enum.Parse(typeof(VirtualHardDiskType), settings["VirtualDiskType"], true);
        //            result = vs.ConvertVirtualHardDisk(vm.VmPath, String.Empty, vhdType);

        //            // check return
        //            if (result.ReturnValue != ReturnCode.JobStarted)
        //            {
        //                TaskManager.WriteError("VPS_CREATE_CONVERT_VHD_ERROR_JOB_START", result.ReturnValue.ToString());
        //                TaskManager.CompleteTask();
        //                return;
        //            }

        //            // wait for completion
        //            if (!JobCompleted(vs, result.Job))
        //            {
        //                TaskManager.WriteError("VPS_CREATE_CONVERT_VHD_ERROR_JOB_EXEC", result.Job.ErrorDescription.ToString());
        //                TaskManager.CompleteTask();
        //                return;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            TaskManager.WriteError(ex, "VPS_CREATE_CONVERT_VHD_ERROR");
        //            TaskManager.CompleteTask();
        //            return;
        //        }
        //        #endregion

        //        #region Get VHD info
        //        VirtualHardDiskInfo vhdInfo = null;
        //        try
        //        {
        //            vhdInfo = vs.GetVirtualHardDiskInfo(vm.VmPath);
        //        }
        //        catch (Exception ex)
        //        {
        //            TaskManager.WriteError(ex, "VPS_CREATE_GET_VHD_INFO");
        //            return;
        //        }

        //        if (vhdInfo == null || vhdInfo.InUse)
        //        {
        //            // master VHD is in use
        //            TaskManager.WriteError("VPS_CREATE_MASTER_VHD_IN_USE");
        //            return;
        //        }

        //        // check if it should be expanded
        //        int hddSizeGB = Convert.ToInt32(vhdInfo.MaxInternalSize / Size1G);

        //        TaskManager.Write("VPS_CREATE_EXPAND_SOURCE_VHD_SIZE", hddSizeGB.ToString());
        //        TaskManager.Write("VPS_CREATE_EXPAND_DEST_VHD_SIZE", vm.HddSize.ToString());
        //        #endregion

        //        #region Expand VHD
        //        bool expanded = false;
        //        if (vm.HddSize > hddSizeGB)
        //        {
        //            TaskManager.Write("VPS_CREATE_EXPAND_VHD");

        //            // expand VHD
        //            try
        //            {
        //                result = vs.ExpandVirtualHardDisk(String.Empty, (ulong)vm.HddSize);
        //            }
        //            catch (Exception ex)
        //            {
        //                TaskManager.WriteError(ex, "VPS_CREATE_EXPAND_VHD_ERROR");
        //                return;
        //            }

        //            // check return
        //            if (result.ReturnValue != ReturnCode.JobStarted)
        //            {
        //                // error starting Expand job
        //                TaskManager.WriteError("VPS_CREATE_EXPAND_VHD_ERROR_JOB_START", result.ReturnValue.ToString());
        //                return;
        //            }

        //            // wait for completion
        //            if (!JobCompleted(vs, result.Job))
        //            {
        //                // error executing Expand job
        //                TaskManager.WriteError("VPS_CREATE_EXPAND_VHD_ERROR_JOB_EXEC", result.Job.ErrorDescription);
        //                return;
        //            }
        //            expanded = true;
        //        }
        //        else
        //        {
        //            // skip expanding
        //            TaskManager.Write("VPS_CREATE_EXPAND_VHD_SKIP");
        //        }
        //        #endregion

        //        //#region Process VHD contents
        //        //// mount VHD
        //        //if ((expanded && !String.IsNullOrWhiteSpace())
        //        //    || (osTemplate.SysprepFiles != null && osTemplate.SysprepFiles.Length > 0))
        //        //{
        //        //    try
        //        //    {
        //        //        #region Mount VHD
        //        //        MountedDiskInfo mountedInfo = vs.MountVirtualHardDisk(vm.VirtualHardDrivePath);
        //        //        if (mountedInfo == null)
        //        //        {
        //        //            // mount returned NULL
        //        //            TaskManager.WriteError("VPS_CREATE_MOUNT_VHD_NULL");
        //        //            return;
        //        //        }
        //        //        #endregion

        //        //        #region Expand volume
        //        //        if (expanded && osTemplate.ProcessVolume != -1 && mountedInfo.DiskVolumes.Length > 0)
        //        //        {
        //        //            try
        //        //            {
        //        //                vs.ExpandDiskVolume(mountedInfo.DiskAddress, mountedInfo.DiskVolumes[osTemplate.ProcessVolume]);
        //        //            }
        //        //            catch (Exception ex)
        //        //            {
        //        //                TaskManager.WriteError(ex, "VPS_CREATE_DISKPART_ERROR");
        //        //            }
        //        //        }
        //        //        else
        //        //        {
        //        //            TaskManager.Write("VPS_CREATE_EXPAND_VHD_SKIP_NO_VOLUMES");
        //        //        }
        //        //        #endregion

        //        //        #region Sysprep
        //        //        if (mountedInfo.DiskVolumes.Length > 0
        //        //            && osTemplate.ProcessVolume != -1
        //        //            && osTemplate.SysprepFiles != null && osTemplate.SysprepFiles.Length > 0)
        //        //        {
        //        //            foreach (string remoteFile in osTemplate.SysprepFiles)
        //        //            {
        //        //                try
        //        //                {
        //        //                    TaskManager.Write("VPS_CREATE_SYSPREP_FILE", remoteFile);

        //        //                    // build remote path
        //        //                    string path = remoteFile;
        //        //                    if (!remoteFile.StartsWith("\\"))
        //        //                        path = remoteFile.Substring(remoteFile.IndexOf("\\"));

        //        //                    path = String.Format("{0}:{1}", mountedInfo.DiskVolumes[osTemplate.ProcessVolume], path);

        //        //                    // read remote file
        //        //                    string contents = vs.ReadRemoteFile(path);
        //        //                    if (contents == null)
        //        //                    {
        //        //                        TaskManager.Write("VPS_CREATE_SYSPREP_FILE_NOT_FOUND", remoteFile);
        //        //                        continue;
        //        //                    }

        //        //                    // process file contents
        //        //                    contents = EvaluateVirtualMachineTemplate(vm.Id, false, false, contents);

        //        //                    // write remote file
        //        //                    vs.WriteRemoteFile(path, contents);
        //        //                }
        //        //                catch (Exception ex)
        //        //                {
        //        //                    TaskManager.WriteError("VPS_CREATE_SYSPREP_FILE_ERROR", ex.Message);
        //        //                }
        //        //            }
        //        //        }
        //        //        #endregion

        //        //        #region Unmount VHD
        //        //        try
        //        //        {
        //        //            code = vs.UnmountVirtualHardDisk(vm.VirtualHardDrivePath);
        //        //            if (code != ReturnCode.OK)
        //        //            {
        //        //                TaskManager.WriteError("VPS_CREATE_UNMOUNT_ERROR_JOB_START", code.ToString());
        //        //                return;
        //        //            }
        //        //        }
        //        //        catch (Exception ex)
        //        //        {
        //        //            TaskManager.WriteError(ex, "VPS_CREATE_UNMOUNT_ERROR");
        //        //            return;
        //        //        }
        //        //        #endregion
        //        //    }
        //        //    catch (Exception ex)
        //        //    {
        //        //        // error mounting
        //        //        TaskManager.WriteError(ex, "VPS_CREATE_MOUNT_VHD");
        //        //        return;
        //        //    }
        //        //} // end if (expanded ...
        //        //#endregion

        //        #region Create Virtual Machine
        //        TaskManager.Write("VPS_CREATE_CPU_CORES", vm.CPUCount.ToString());
        //        TaskManager.Write("VPS_CREATE_RAM_SIZE", vm.Memory.ToString());
        //        TaskManager.Write("VPS_CREATE_CREATE_VM");
        //        // create virtual machine
        //        try
        //        {
        //            VMInfo ttt = new VMInfo();

        //            // create
        //            ttt = vs.CreateVirtualMachine(vm);
        //        }
        //        catch (Exception ex)
        //        {
        //            TaskManager.WriteError(ex, "VPS_CREATE_CREATE_VM_ERROR");
        //            TaskManager.CompleteTask();
        //            return;
        //        }

        //        // update meta item
        //        PackageController.UpdatePackageItem(vm);

        //        TaskManager.Write("VPS_CREATE_CREATED_VM");
        //        #endregion

        //        // set OK flag
        //        vm.ProvisioningStatus = VirtualMachineProvisioningStatus.OK;

        //        #region Send KVP
        //        //// configure computer name
        //        //if (osTemplate.ProvisionComputerName)
        //        //{
        //        //    TaskManager.Write("VPS_CREATE_SET_COMPUTER_NAME_KVP");
        //        //    SendComputerNameKVP(vm.Id, vm.Name);
        //        //}

        //        //// change administrator password
        //        //if (osTemplate.ProvisionAdministratorPassword)
        //        //{
        //        //    TaskManager.Write("VPS_CREATE_SET_PASSWORD_KVP");
        //        //    SendAdministratorPasswordKVP(vm.Id, CryptoUtils.Decrypt(vm.AdminPassword));
        //        //}

        //        //// configure network adapters
        //        //if(osTemplate.ProvisionNetworkAdapters)
        //        //{
        //        //    // external NIC
        //        //    TaskManager.Write("VPS_CREATE_SET_EXTERNAL_NIC_KVP");
        //        //    if (vm.ExternalNetworkEnabled)
        //        //    {
        //        //        result = SendNetworkAdapterKVP(vm.Id, "External");

        //        //        if (result.ReturnValue != ReturnCode.JobStarted)
        //        //            TaskManager.WriteWarning("VPS_CREATE_SET_EXTERNAL_NIC_KVP_ERROR", result.ReturnValue.ToString());
        //        //    }

        //        //    // management NIC
        //        //    TaskManager.Write("VPS_CREATE_SET_MANAGEMENT_NIC_KVP");
        //        //    if (vm.ManagementNetworkEnabled)
        //        //    {
        //        //        result = SendNetworkAdapterKVP(vm.Id, "Management");

        //        //        if (result.ReturnValue != ReturnCode.JobStarted)
        //        //            TaskManager.WriteWarning("VPS_CREATE_SET_MANAGEMENT_NIC_KVP_ERROR", result.ReturnValue.ToString());
        //        //    }

        //        //    // private NIC
        //        //    TaskManager.Write("VPS_CREATE_SET_PRIVATE_NIC_KVP");
        //        //    if (vm.PrivateNetworkEnabled)
        //        //    {
        //        //        result = SendNetworkAdapterKVP(vm.Id, "Private");

        //        //        if (result.ReturnValue != ReturnCode.JobStarted)
        //        //            TaskManager.WriteWarning("VPS_CREATE_SET_PRIVATE_NIC_KVP_ERROR", result.ReturnValue.ToString());
        //        //    }
        //        //}
        //        //#endregion

        //        #region Start VPS
        //        TaskManager.Write("VPS_CREATE_START_VPS");

        //        try
        //        {
        //            // start virtual machine
        //            result = vs.ChangeVirtualMachineState(vm.VmGuid.ToString(), VirtualMachineRequestedState.Start);

        //            // check return
        //            if (result.ReturnValue == ReturnCode.JobStarted)
        //            {
        //                // wait for completion
        //                if (!JobCompleted(vs, result.Job))
        //                {
        //                    TaskManager.WriteWarning("VPS_CREATE_START_VPS_ERROR_JOB_EXEC", result.Job.ErrorDescription.ToString());
        //                }
        //            }
        //            else
        //            {
        //                TaskManager.WriteWarning("VPS_CREATE_START_VPS_ERROR_JOB_START", result.ReturnValue.ToString());
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            TaskManager.WriteWarning("VPS_CREATE_START_VPS_ERROR", ex.Message);
        //        }
        //        TaskManager.Write("VPS_CREATE_STARTED_VPS");
        //        #endregion

        //        #region Send Summary letter
        //        // send summary e-mail
        //        if (!String.IsNullOrEmpty(summaryLetterEmail))
        //        {
        //            SendVirtualMachineSummaryLetter(vm.Id, summaryLetterEmail, null, true);
        //        }
        //        #endregion

        //    }
        //    catch (Exception ex)
        //    {
        //        TaskManager.WriteError(ex, VirtualizationErrorCodes.CREATE_ERROR);
        //        TaskManager.CompleteTask();
        //        return;
        //    }
        //    finally
        //    {
        //        // reset task ID
        //        vm.CurrentTaskId = null;
        //        PackageController.UpdatePackageItem(vm);

        //        if (vm.ProvisioningStatus == VirtualMachineProvisioningStatus.OK)
        //            TaskManager.Write("VPS_CREATE_SUCCESS");
        //        else if (vm.ProvisioningStatus == VirtualMachineProvisioningStatus.Error)
        //            TaskManager.Write("VPS_CREATE_ERROR_END");

        //        // complete task
        //        TaskManager.CompleteTask();
        //    }
        //}

        private static void CheckNumericQuota(PackageContext cntx, List<string> errors, string quotaName, int currentVal, int val, string messageKey)
        {
            CheckQuotaValue(cntx, errors, quotaName, currentVal, val, messageKey);
        }

        private static void CheckNumericQuota(PackageContext cntx, List<string> errors, string quotaName, int val, string messageKey)
        {
            CheckQuotaValue(cntx, errors, quotaName, 0, val, messageKey);
        }

        private static void CheckBooleanQuota(PackageContext cntx, List<string> errors, string quotaName, bool val, string messageKey)
        {
            CheckQuotaValue(cntx, errors, quotaName, 0, val ? 1 : 0, messageKey);
        }

        private static void CheckListsQuota(PackageContext cntx, List<string> errors, string quotaName, string messageKey)
        {
            CheckQuotaValue(cntx, errors, quotaName, 0, -1, messageKey);
        }

        private static void CheckQuotaValue(PackageContext cntx, List<string> errors, string quotaName, int currentVal, int val, string messageKey)
        {
            if (!cntx.Quotas.ContainsKey(quotaName))
                return;

            QuotaValueInfo quota = cntx.Quotas[quotaName];

            if(val == -1 && quota.QuotaExhausted) // check if quota already reached
            {
                errors.Add(messageKey + ":" + quota.QuotaAllocatedValue);
            }
            else if(quota.QuotaAllocatedValue == -1)
                return; // unlimited
            else if (quota.QuotaTypeId == 1 && val == 1 && quota.QuotaAllocatedValue == 0) // bool quota
                errors.Add(messageKey);
            else if (quota.QuotaTypeId == 2)
            {
                int maxValue = quota.QuotaAllocatedValue - quota.QuotaUsedValue + currentVal;
                if(val > maxValue)
                    errors.Add(messageKey + ":" + maxValue);
            }
            else if (quota.QuotaTypeId == 3 && val > quota.QuotaAllocatedValue)
            {
                int maxValue = quota.QuotaAllocatedValue;
                errors.Add(messageKey + ":" + maxValue);
            }
        }

        public static IntResult ImportVirtualMachine(int packageId,
            int serviceId, string vmId,
            string osTemplateFile, string adminPassword,
            bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed,
            string externalNicMacAddress, int[] externalAddresses,
            string managementNicMacAddress, int managementAddress)
        {
            // result object
            IntResult res = new IntResult();

            // meta item
            VirtualMachine item = null;

            try
            {
                #region Check account and space statuses
                // check account
                if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive | DemandAccount.IsAdmin))
                    return res;

                // check package
                if (!SecurityContext.CheckPackage(res, packageId, DemandPackage.IsActive))
                    return res;

                #endregion

                // load package context
                PackageContext cntx = PackageController.GetPackageContext(packageId);

                item = new VirtualMachine();
                item.ServiceId = serviceId;
                item.PackageId = packageId;
                item.VirtualMachineId = vmId;

                // load service settings
                StringDictionary settings = ServerController.GetServiceSettings(serviceId);

                // load virtual machine info from service
                var vs = GetVirtualizationProxy(serviceId);
                VirtualMachine vm = vs.GetVirtualMachineEx(vmId);

                // set VM properties
                item.Name = vm.Name;
                item.ProvisioningStatus = VirtualMachineProvisioningStatus.OK;

                item.CpuCores = vm.CpuCores;
                item.RamSize = vm.RamSize;
                item.HddSize = vm.HddSize;
                item.VirtualHardDrivePath = vm.VirtualHardDrivePath;
                item.RootFolderPath = Path.GetDirectoryName(vm.VirtualHardDrivePath);
                item.SnapshotsNumber = cntx.Quotas[Quotas.VPS_SNAPSHOTS_NUMBER].QuotaAllocatedValue;
                item.DvdDriveInstalled = vm.DvdDriveInstalled;
                item.BootFromCD = vm.BootFromCD;
                item.NumLockEnabled = vm.NumLockEnabled;
                item.StartTurnOffAllowed = startShutdownAllowed;
                item.PauseResumeAllowed = pauseResumeAllowed;
                item.RebootAllowed = rebootAllowed;
                item.ResetAllowed = resetAllowed;
                item.ReinstallAllowed = reinstallAllowed;

                // remote desktop
                if(!String.IsNullOrEmpty(adminPassword))
                {
                    item.RemoteDesktopEnabled = true;
                    item.AdministratorPassword = CryptoUtils.Encrypt(adminPassword);
                }

                // set OS template
                string templatesPath = settings["OsTemplatesPath"];
                item.OperatingSystemTemplatePath = Path.Combine(templatesPath, osTemplateFile + ".vhd");
                try
                {
                    LibraryItem[] osTemplates = GetOperatingSystemTemplatesByServiceId(serviceId);
                    foreach (LibraryItem osTemplate in osTemplates)
                    {
                        if (String.Compare(osTemplate.Path, osTemplateFile, true) == 0)
                        {
                            item.OperatingSystemTemplate = osTemplate.Name;
                            item.LegacyNetworkAdapter = osTemplate.LegacyNetworkAdapter;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    res.AddError(VirtualizationErrorCodes.GET_OS_TEMPLATES_ERROR, ex);
                    return res;
                }

                // save item
                int itemId = PackageController.AddPackageItem(item);
                item.Id = itemId;
                res.Value = itemId;

                #region Setup external network
                // setup external network
                if (!String.IsNullOrEmpty(externalNicMacAddress))
                {
                    item.ExternalNetworkEnabled = true;
                    item.ExternalNicMacAddress = externalNicMacAddress;
                    item.ExternalSwitchId = settings["ExternalNetworkId"];

                    // assign selected IP addresses to package
                    ServerController.AllocatePackageIPAddresses(packageId, externalAddresses);

                    // re-read package IPs
                    List<PackageIPAddress> packageIPs = ServerController.GetPackageUnassignedIPAddresses(
                                    packageId, IPAddressPool.VpsExternalNetwork);

                    // assign IP addresses to VM
                    for(int i = 0; i < externalAddresses.Length; i++)
                    {
                        foreach (PackageIPAddress ip in packageIPs)
                        {
                            if (ip.AddressID == externalAddresses[i])
                            {
                                // assign to the item
                                ServerController.AddItemIPAddress(itemId, ip.PackageAddressID);

                                // set primary IP address
                                if(i == 0)
                                    ServerController.SetItemPrimaryIPAddress(itemId, ip.PackageAddressID);

                                break;
                            }
                        }
                    }
                }
                #endregion

                #region Setup management network
                // setup management network
                if (!String.IsNullOrEmpty(managementNicMacAddress))
                {
                    item.ManagementNetworkEnabled = true;
                    item.ManagementNicMacAddress = managementNicMacAddress;
                    item.ManagementSwitchId = settings["ManagementNetworkId"];

                    // assign selected IP addresses to package
                    ServerController.AllocatePackageIPAddresses(packageId, new int[] { managementAddress });

                    // re-read package IPs
                    List<PackageIPAddress> packageIPs = ServerController.GetPackageUnassignedIPAddresses(
                                    packageId, IPAddressPool.VpsManagementNetwork);

                    // assign IP addresses to VM
                    foreach (PackageIPAddress ip in packageIPs)
                    {
                        if (ip.AddressID == managementAddress)
                        {
                            // assign to the item
                            ServerController.AddItemIPAddress(itemId, ip.PackageAddressID);

                            break;
                        }
                    }
                }
                #endregion

                // save item once again
                PackageController.UpdatePackageItem(item);
            }
            catch (Exception ex)
            {
                res.AddError(VirtualizationErrorCodes.IMPORT_ERROR, ex);
                return res;
            }

            res.IsSuccess = true;
            return res;
        }

        private static JobResult SendNetworkAdapterKVP(int itemId, string adapterName)
        {
            // load item
            VMInfo vm = GetVirtualMachineByItemId(itemId);
            if (vm == null)
                return null;

            // build task parameters
            Dictionary<string, string> props = new Dictionary<string, string>();
            NetworkAdapterDetails nic = null;

            if(String.Compare(adapterName, "external", true) == 0)
            {
                // external
                nic = GetExternalNetworkAdapterDetails(itemId);
            }
            else if(String.Compare(adapterName, "private", true) == 0)
            {
                // private
                nic = GetPrivateNetworkAdapterDetails(itemId);
            }
            else
            {
                // management
                nic = GetManagementNetworkAdapterDetails(itemId);
            }
            
            // network format
            if (nic != null && !String.IsNullOrEmpty(nic.MacAddress))
            {
                props["MAC"] = nic.MacAddress.Replace("-", ":");
                props["EnableDHCP"] = nic.IsDHCP.ToString();
                if (!nic.IsDHCP)
                {
                    string[] ips = new string[nic.IPAddresses.Length];
                    string[] subnetMasks = new string[nic.IPAddresses.Length];
                    for (int i = 0; i < ips.Length; i++)
                    {
                        ips[i] = nic.IPAddresses[i].IPAddress;
                        subnetMasks[i] = nic.IPAddresses[i].SubnetMask;

                        // set gateway from the first (primary) IP
                        if (i == 0)
                            props["DefaultIPGateway"] = nic.IPAddresses[i].DefaultGateway;
                    }

                    props["IPAddress"] = String.Join(";", ips);
                    props["SubnetMask"] = String.Join(";", subnetMasks);

                    // name servers
                    props["PreferredDNSServer"] = nic.PreferredNameServer;
                    if (!String.IsNullOrEmpty(nic.AlternateNameServer))
                        props["PreferredDNSServer"] += ";" + nic.AlternateNameServer;
                }
            }

            // DNS
            if (!props.ContainsKey("PreferredDNSServer")
                || String.IsNullOrEmpty(props["PreferredDNSServer"]))
            {
                props["PreferredDNSServer"] = "0.0.0.0"; // obtain automatically
            }

            // send items
            return SendKvpItems(itemId, "SetupNetworkAdapter", props);
        }

        private static string GetSymbolDelimitedMacAddress(string mac, string delimiter)
        {
            if (String.IsNullOrEmpty(mac))
                return mac;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 6; i++)
            {
                sb.Append(mac[i * 2]).Append(mac[i * 2 + 1]);
                if (i < 5) sb.Append(delimiter);
            }
            return sb.ToString();
        }

        private static JobResult SendComputerNameKVP(int itemId, string computerName)
        {
            // load item
            VMInfo vm = GetVirtualMachineByItemId(itemId);
            if (vm == null)
                return null;

            // build task parameters
            Dictionary<string, string> props = new Dictionary<string, string>();

            props["FullComputerName"] = computerName;

            // send items
            return SendKvpItems(itemId, "ChangeComputerName", props);
        }

        private static JobResult SendAdministratorPasswordKVP(int itemId, string password)
        {
            // load item
            VMInfo vm = GetVirtualMachineByItemId(itemId);
            if (vm == null)
                return null;

            // build task parameters
            Dictionary<string, string> props = new Dictionary<string, string>();

            props["Password"] = password;

            // send items
            return SendKvpItems(itemId, "ChangeAdministratorPassword", props);
        }

        private static JobResult SendKvpItems(int itemId, string taskName, Dictionary<string, string> taskProps)
        {
            string TASK_PREFIX = "SCP-";

            // load item
            VMInfo vm = GetVirtualMachineByItemId(itemId);
            if (vm == null)
                return null;

            JobResult result = null;

            // load proxy
            var vs = GetVirtualizationProxy(vm.ServiceId);

            try
            {
                // delete completed task definitions
                List<string> completedTasks = new List<string>();
                KvpExchangeDataItem[] vmKvps = vs.GetKVPItems(vm.VmGuid.ToString());
                foreach (KvpExchangeDataItem vmKvp in vmKvps)
                {
                    if (vmKvp.Name.StartsWith(TASK_PREFIX))
                        completedTasks.Add(vmKvp.Name);
                }

                // delete completed items
                vs.RemoveKVPItems(vm.VmId.ToString(), completedTasks.ToArray());
            }
            catch (Exception ex)
            {
                // log error
                TaskManager.WriteWarning(String.Format("Error deleting KVP items: {0}", ex.Message));
            }

            // build items array
            List<string> items = new List<string>();
            foreach (string propName in taskProps.Keys)
                items.Add(propName + "=" + taskProps[propName]);

            taskName = String.Format("{0}{1}-{2}", TASK_PREFIX, taskName, DateTime.Now.Ticks);
            string taskData = String.Join("|", items.ToArray());

            // create KVP item
            KvpExchangeDataItem[] kvp = new KvpExchangeDataItem[1];
            kvp[0] = new KvpExchangeDataItem();
            kvp[0].Name = taskName;
            kvp[0].Data = taskData;

            try
            {
                // try adding KVP items
                result = vs.AddKVPItems(vm.VmId.ToString(), kvp);

                if (result.Job != null && result.Job.JobState == ConcreteJobState.Exception)
                {
                    // try updating KVP items
                    return vs.ModifyKVPItems(vm.VmId.ToString(), kvp);
                }
                else
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                // log error
                TaskManager.WriteWarning(String.Format("Error setting KVP items '{0}': {1}", kvp[0].Data, ex.Message));
            }

            return null;
        }

        private static string EnsurePrivateVirtualSwitch(ServiceProviderItem item)
        {
            // try locate switch in the package
            List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(item.PackageId, typeof(VirtualSwitch));

            // exists - return ID
            if (items.Count > 0)
                return ((VirtualSwitch)items[0]).SwitchId;

            // switch name
            string name = EvaluateItemVariables("[username] - [space_name]", item);

            // log
            TaskManager.Write("VPS_CREATE_PRIVATE_VIRTUAL_SWITCH", name);

            try
            {
                // create switch
                // load proxy
                var vs = GetVirtualizationProxy(item.ServiceId);

                // create switch
                VirtualSwitch sw = vs.CreateSwitch(name);
                sw.ServiceId = item.ServiceId;
                sw.PackageId = item.PackageId;

                // save item
                PackageController.AddPackageItem(sw);

                return sw.SwitchId;
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex, "VPS_CREATE_PRIVATE_VIRTUAL_SWITCH_ERROR");
                return null;
            }
        }

        private static string EvaluateItemVariables(string str, ServiceProviderItem item)
        {
            str = Utils.ReplaceStringVariable(str, "vps_hostname", item.Name);

            return EvaluateSpaceVariables(str, item.PackageId);
        }

        private static string EvaluateSpaceVariables(string str, int packageId)
        {
            // load package
            PackageInfo package = PackageController.GetPackage(packageId);
            UserInfo user = UserController.GetUser(package.UserId);
            str = Utils.ReplaceStringVariable(str, "space_id", packageId.ToString());
            str = Utils.ReplaceStringVariable(str, "space_name", package.PackageName);
            str = Utils.ReplaceStringVariable(str, "user_id", user.UserId.ToString());
            str = Utils.ReplaceStringVariable(str, "username", user.Username);

            return str;
        }

        #region VPS – General

        public static List<ConcreteJob> GetVirtualMachineJobs(int itemId)
        {
            // load meta item
            VMInfo vm = GetVirtualMachineByItemId(itemId);

            if (vm == null)
                return null;

            // get proxy
            var vps = GetVirtualizationProxy(vm.ServiceId);

            // load jobs
            ConcreteJob[] jobs = vps.GetVirtualMachineJobs(vm.VmId.ToString());
            List<ConcreteJob> retJobs = new List<ConcreteJob>();

            foreach (ConcreteJob job in jobs)
            {
                if (job.JobState == ConcreteJobState.Running)
                {
                    retJobs.Add(job);
                }
            }

            return retJobs;
        }
        
        public static byte[] GetVirtualMachineThumbnail(int itemId, ThumbnailSize size)
        {
            // load meta item
            VMInfo vm = GetVirtualMachineByItemId(itemId);

            if (vm == null)
                return null;

            // get proxy
            var vps = GetVirtualizationProxy(vm.ServiceId);

            // return thumbnail
            return vps.GetVirtualMachineThumbnailImage(vm.Name, size);
        }

        public static VMInfo GetVirtualMachineGeneralDetails(int itemId)
        {
            // load meta item
            VMInfo machine = GetVirtualMachineByItemId(itemId);

            if (machine == null || String.IsNullOrEmpty(machine.Name.ToString()))
                return null;

            // get proxy
            var vps = GetVirtualizationProxy(machine.ServiceId);

			// load details
            VMInfo vm = vps.GetVirtualMachine(machine.Name.ToString());

            // add meta props
            if (vm != null)
            {
                vm.Id = machine.Id;
                vm.Name = machine.Name;
                vm.HostName = (vm.HostName ?? String.Empty);
                vm.ServiceId = machine.ServiceId;
            }

            return vm;
        }

        public static VirtualMachine GetVirtualMachineExtendedInfo(int serviceId, string vmId)
        {
            // get proxy
            var vps = GetVirtualizationProxy(serviceId);

            // load details
            return vps.GetVirtualMachineEx(vmId);
        }

        public static int CancelVirtualMachineJob(string jobId)
        {
            // VPS - CANCEL_JOB
            return 0;
        }

        public static ResultObject UpdateVirtualMachineHostName(int itemId, string hostname, bool updateNetBIOS)
        {
            if (String.IsNullOrEmpty(hostname))
                throw new ArgumentNullException("hostname");

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
            res = TaskManager.StartResultTask<ResultObject>("VPS", "UPDATE_HOSTNAME", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // get proxy
                var vs = GetVirtualizationProxy(vm.ServiceId);

                // update virtual machine name
                JobResult result = vs.RenameVirtualMachine(vm.VirtualMachineId, hostname);
                if (result.ReturnValue != ReturnCode.OK)
                {
                    LogReturnValueResult(res, result);
                    TaskManager.CompleteResultTask(res);
                    return res;
                }

                // update meta item
                vm.Name = hostname;
                PackageController.UpdatePackageItem(vm);
                
                // update NetBIOS name if required
                if (updateNetBIOS)
                {
                    result = SendComputerNameKVP(itemId, hostname);
                    if (result.ReturnValue != ReturnCode.JobStarted
                        && result.Job.JobState == ConcreteJobState.Completed)
                    {
                        LogReturnValueResult(res, result);
                        TaskManager.CompleteResultTask(res);
                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.CHANGE_ADMIN_PASSWORD_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public static ResultObject ChangeVirtualMachineStateExternal(int itemId, VirtualMachineRequestedState state)
        {
            ResultObject res = new ResultObject();

            // load service item
            VMInfo vm = (VMInfo)PackageController.GetPackageItem(itemId);
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

            //TODO
            //if ((state == VirtualMachineRequestedState.Start
            //    || state == VirtualMachineRequestedState.TurnOff
            //    || state == VirtualMachineRequestedState.ShutDown)
            //    && !vm.StartTurnOffAllowed)
            //    quotaResults.Add(VirtualizationErrorCodes.QUOTA_EXCEEDED_START_SHUTDOWN_ALLOWED);

            //else if ((state == VirtualMachineRequestedState.Pause
            //    || state == VirtualMachineRequestedState.Resume)
            //    && !vm.PauseResumeAllowed)
            //    quotaResults.Add(VirtualizationErrorCodes.QUOTA_EXCEEDED_PAUSE_RESUME_ALLOWED);

            //else if (state == VirtualMachineRequestedState.Reboot
            //    && !vm.RebootAllowed)
            //    quotaResults.Add(VirtualizationErrorCodes.QUOTA_EXCEEDED_REBOOT_ALLOWED);

            //else if (state == VirtualMachineRequestedState.Reset
            //    && !vm.ResetAllowed)
            //    quotaResults.Add(VirtualizationErrorCodes.QUOTA_EXCEEDED_RESET_ALOWED);


            //if ((state == VirtualMachineRequestedState.Start
            //    || state == VirtualMachineRequestedState.TurnOff
            //    || state == VirtualMachineRequestedState.ShutDown))
            //    quotaResults.Add(VirtualizationErrorCodes.QUOTA_EXCEEDED_START_SHUTDOWN_ALLOWED);

            //else if ((state == VirtualMachineRequestedState.Pause
            //    || state == VirtualMachineRequestedState.Resume))
            //    quotaResults.Add(VirtualizationErrorCodes.QUOTA_EXCEEDED_PAUSE_RESUME_ALLOWED);

            //else if (state == VirtualMachineRequestedState.Reboot)
            //    quotaResults.Add(VirtualizationErrorCodes.QUOTA_EXCEEDED_REBOOT_ALLOWED);

            //else if (state == VirtualMachineRequestedState.Reset)
            //    quotaResults.Add(VirtualizationErrorCodes.QUOTA_EXCEEDED_RESET_ALOWED);



            if (quotaResults.Count > 0)
            {
                res.ErrorCodes.AddRange(quotaResults);
                res.IsSuccess = false;
                TaskManager.CompleteResultTask();
                return res;
            }
            #endregion

            return ChangeVirtualMachineState(itemId, state);
        }

        private static ResultObject ChangeVirtualMachineState(int itemId, VirtualMachineRequestedState state)
        {
            // start task
            ResultObject res = TaskManager.StartResultTask<ResultObject>("VPSForPC", "CHANGE_STATE");
                                                
            try
            {
                // load service item
                VMInfo machine = (VMInfo)PackageController.GetPackageItem(itemId);

                if (machine == null)
                {
                    TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                    return res;
                }

                BackgroundTask topTask = TaskManager.TopTask;
                topTask.ItemId = machine.Id;
                topTask.ItemName = machine.Name;
                topTask.PackageId = machine.PackageId;

                TaskController.UpdateTask(topTask);

                TaskManager.WriteParameter("New state", state);

                // load proxy
                var vps = GetVirtualizationProxy(machine.ServiceId);

                try
                {
                    if (state == VirtualMachineRequestedState.ShutDown)
                    {
                        ReturnCode code = vps.ShutDownVirtualMachine(machine.Name, true, SHUTDOWN_REASON);
                        if (code != ReturnCode.OK)
                        {
                            res.ErrorCodes.Add(VirtualizationErrorCodes.JOB_START_ERROR + ":" + code);
                            TaskManager.CompleteResultTask(res);
                            return res;
                        }

                        // spin until fully stopped
                        VMInfo vm = vps.GetVirtualMachine(machine.Name);
                        while (vm.State != SolidCP.Providers.Virtualization.VMComputerSystemStateInfo.PowerOff)
                        {
                            System.Threading.Thread.Sleep(1000); // sleep 1 second
                            vm = vps.GetVirtualMachine(machine.Name);
                        }
                    }
                    else if (state == VirtualMachineRequestedState.Reboot)
                    {
                        // shutdown first
                        ResultObject shutdownResult = ChangeVirtualMachineState(itemId, VirtualMachineRequestedState.ShutDown);
                        if(!shutdownResult.IsSuccess)
                        {
                            TaskManager.CompleteResultTask(res);
                            return shutdownResult;
                        }

                        // start machine
                        ResultObject startResult = ChangeVirtualMachineState(itemId, VirtualMachineRequestedState.Start);
                        if (!startResult.IsSuccess)
                        {
                            TaskManager.CompleteResultTask(res);
                            return startResult;
                        }
                    }
                    else
                    {
                        JobResult result = vps.ChangeVirtualMachineState(machine.Name, state);

                        // check return
                        if (result.ReturnValue != ReturnCode.JobStarted)
                        {
                            LogReturnValueResult(res, result);
                            TaskManager.CompleteResultTask(res);
                            return res;
                        }

                        // wait for completion
                        if (!JobCompleted(vps, result.Job))
                        {
                            LogJobResult(res, result.Job);
                            TaskManager.CompleteResultTask(res);
                            return res;
                        }
                    }
                }
                catch(Exception ex)
                {
                    res.IsSuccess = false;
                    res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_CHANGE_VIRTUAL_SERVER_STATE);
                    TaskManager.WriteError(ex);
                    TaskManager.CompleteResultTask(res);
                }
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorCodes.Add(VirtualizationErrorCodes.CHANGE_VIRTUAL_MACHINE_STATE_GENERAL_ERROR);
                TaskManager.WriteError(ex);
                TaskManager.CompleteResultTask(res);
                return res;
            }

            TaskManager.CompleteTask();
            return res;
        }
        #endregion

        #region VPS - Configuration
        public static ResultObject ChangeAdministratorPassword(int itemId, string password)
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
            res = TaskManager.StartResultTask<ResultObject>("VPS", "CHANGE_ADMIN_PASSWORD", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // get proxy
                var vs = GetVirtualizationProxy(vm.ServiceId);

                // change administrator password
                JobResult result = SendAdministratorPasswordKVP(itemId, password);
                if (result.ReturnValue != ReturnCode.JobStarted
                    && result.Job.JobState == ConcreteJobState.Completed)
                {
                    LogReturnValueResult(res, result);
                    TaskManager.CompleteResultTask(res);
                    return res;
                }

                // update meta item
                vm.AdministratorPassword = CryptoUtils.Encrypt(password);
                PackageController.UpdatePackageItem(vm);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.CHANGE_ADMIN_PASSWORD_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }
        #endregion

        #region VPS – Edit Configuration
        public static ResultObject UpdateVirtualMachineConfiguration(int itemId, int cpuCores, int ramMB, int hddGB, int snapshots,
                    bool dvdInstalled, bool bootFromCD, bool numLock,
                    bool startShutdownAllowed, bool pauseResumeAllowed, bool rebootAllowed, bool resetAllowed, bool reinstallAllowed,
                    bool externalNetworkEnabled,
                    bool privateNetworkEnabled)
        {
            ResultObject res = new ResultObject();

            // load service item
            VMInfo vm = (VMInfo)PackageController.GetPackageItem(itemId);
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
            PackageContext cntx = PackageController.GetPackageContext(vm.PackageId);

            CheckNumericQuota(cntx, quotaResults, Quotas.VPS_CPU_NUMBER, cpuCores, VirtualizationErrorCodes.QUOTA_EXCEEDED_CPU);
            CheckNumericQuota(cntx, quotaResults, Quotas.VPS_RAM, vm.Memory, ramMB, VirtualizationErrorCodes.QUOTA_EXCEEDED_RAM);
            CheckNumericQuota(cntx, quotaResults, Quotas.VPS_HDD, vm.HddSize, hddGB, VirtualizationErrorCodes.QUOTA_EXCEEDED_HDD);
            CheckNumericQuota(cntx, quotaResults, Quotas.VPS_SNAPSHOTS_NUMBER, snapshots, VirtualizationErrorCodes.QUOTA_EXCEEDED_SNAPSHOTS);

            CheckBooleanQuota(cntx, quotaResults, Quotas.VPS_DVD_ENABLED, dvdInstalled, VirtualizationErrorCodes.QUOTA_EXCEEDED_DVD_ENABLED);
            CheckBooleanQuota(cntx, quotaResults, Quotas.VPS_BOOT_CD_ALLOWED, bootFromCD, VirtualizationErrorCodes.QUOTA_EXCEEDED_CD_ALLOWED);

            CheckBooleanQuota(cntx, quotaResults, Quotas.VPS_START_SHUTDOWN_ALLOWED, startShutdownAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_START_SHUTDOWN_ALLOWED);
            CheckBooleanQuota(cntx, quotaResults, Quotas.VPS_PAUSE_RESUME_ALLOWED, pauseResumeAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_PAUSE_RESUME_ALLOWED);
            CheckBooleanQuota(cntx, quotaResults, Quotas.VPS_REBOOT_ALLOWED, rebootAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_REBOOT_ALLOWED);
            CheckBooleanQuota(cntx, quotaResults, Quotas.VPS_RESET_ALOWED, resetAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_RESET_ALOWED);
            CheckBooleanQuota(cntx, quotaResults, Quotas.VPS_REINSTALL_ALLOWED, reinstallAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_REINSTALL_ALLOWED);

            CheckBooleanQuota(cntx, quotaResults, Quotas.VPS_EXTERNAL_NETWORK_ENABLED, externalNetworkEnabled, VirtualizationErrorCodes.QUOTA_EXCEEDED_EXTERNAL_NETWORK_ENABLED);
            CheckBooleanQuota(cntx, quotaResults, Quotas.VPS_PRIVATE_NETWORK_ENABLED, privateNetworkEnabled, VirtualizationErrorCodes.QUOTA_EXCEEDED_PRIVATE_NETWORK_ENABLED);

            // check acceptable values
            if (ramMB <= 0)
                quotaResults.Add(VirtualizationErrorCodes.QUOTA_WRONG_RAM);
            if (hddGB <= 0)
                quotaResults.Add(VirtualizationErrorCodes.QUOTA_WRONG_HDD);
            if (snapshots < 0)
                quotaResults.Add(VirtualizationErrorCodes.QUOTA_WRONG_SNAPSHOTS);

            if (quotaResults.Count > 0)
            {
                res.ErrorCodes.AddRange(quotaResults);
                return res;
            }
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("VPS", "UPDATE_CONFIGURATION", vm.Id, vm.Name, vm.PackageId);

            try
            {
                JobResult result = null;

                // get proxy
                var vs = GetVirtualizationProxy(vm.ServiceId);

                // stop VPS if required
                VMInfo vps = vs.GetVirtualMachine(vm.Name);

                bool wasStarted = false;

                // stop (shut down) virtual machine
                if (vps.State != VMComputerSystemStateInfo.PowerOff)
                {
                    wasStarted = true;
                    ReturnCode code = vs.ShutDownVirtualMachine(vm.Name, true, SHUTDOWN_REASON_CHANGE_CONFIG);
                    if (code == ReturnCode.OK)
                    {
                        // spin until fully stopped
                        vps = vs.GetVirtualMachine(vm.Name);
                        while (vps.State != VMComputerSystemStateInfo.PowerOff)
                        {
                            System.Threading.Thread.Sleep(1000); // sleep 1 second
                            vps = vs.GetVirtualMachine(vm.Name);
                        }
                    }
                    else
                    {
                        // turn off
                        result = vs.ChangeVirtualMachineState(vm.Name, VirtualMachineRequestedState.TurnOff);
                        if (!JobCompleted(vs, result.Job))
                        {
                            LogJobResult(res, result.Job);
                            TaskManager.CompleteResultTask(res);
                            return res;
                        }
                    }
                } // end OFF

                // update meta-item
                vm.CPUCount = cpuCores;
                vm.Memory = ramMB;
                vm.HddSize = hddGB;
                vm.SnapshotsNumber = snapshots;

                vm.BootFromCD = bootFromCD;
                vm.NumLockEnabled = numLock;
                vm.DvdDriver = dvdInstalled;

                vm.StartTurnOffAllowed = startShutdownAllowed;
                vm.PauseResumeAllowed = pauseResumeAllowed;
                vm.ResetAllowed = resetAllowed;
                vm.RebootAllowed = rebootAllowed;
                vm.ReinstallAllowed = reinstallAllowed;

                vm.ExternalNetworkEnabled = externalNetworkEnabled;
                vm.PrivateNetworkEnabled = privateNetworkEnabled;

                // load service settings
                StringDictionary settings = ServerController.GetServiceSettings(vm.ServiceId);

                #region setup external network
                if (vm.ExternalNetworkEnabled
                    && String.IsNullOrEmpty(vm.ExternalNicMacAddress))
                {
                    // connect to network
                    vm.ExternalNicMacAddress = GenerateMacAddress();
                }
                #endregion

                #region setup private network
                if (vm.PrivateNetworkEnabled
                    && String.IsNullOrEmpty(vm.PrivateNicMacAddress))
                {
                    vm.PrivateNicMacAddress = GenerateMacAddress();
                }
                #endregion

                // update configuration on virtualization server
                vm = vs.UpdateVirtualMachine(vm);

                // update meta item
                PackageController.UpdatePackageItem(vm);

                // unprovision external IP addresses
                if (!vm.ExternalNetworkEnabled)
                    ServerController.DeleteItemIPAddresses(itemId);
                else
                    // send KVP config items
                    SendNetworkAdapterKVP(itemId, "External");

                // unprovision private IP addresses
                if (!vm.PrivateNetworkEnabled)
                    DataProvider.DeleteItemPrivateIPAddresses(SecurityContext.User.UserId, itemId);
                else
                    // send KVP config items
                    SendNetworkAdapterKVP(itemId, "Private");

                // start if required
                if (wasStarted)
                {
                    result = vs.ChangeVirtualMachineState(vm.Name, VirtualMachineRequestedState.Start);
                    if (!JobCompleted(vs, result.Job))
                    {
                        LogJobResult(res, result.Job);
                        TaskManager.CompleteResultTask(res);
                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.CHANGE_VM_CONFIGURATION, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }
        #endregion

        #region DVD
        public static LibraryItem GetInsertedDvdDisk(int itemId)
        {
            // load item
            VMInfo vm = GetVirtualMachineByItemId(itemId);

            // get proxy
            var vs = GetVirtualizationProxy(vm.ServiceId);
            string isoPath = vs.GetInsertedDVD(vm.VmGuid.ToString());

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

        public static LibraryItem[] GetLibraryDisks(int itemId)
        {
            // load item
            VMInfo vm = GetVirtualMachineByItemId(itemId);

            // load service settings
            StringDictionary settings = ServerController.GetServiceSettings(vm.ServiceId);
            string path = settings["DvdLibraryPath"];

            // get proxy
            var vs = GetVirtualizationProxy(vm.ServiceId);

            return vs.GetLibraryItems(path);
        }

        public static ResultObject InsertDvdDisk(int itemId, string isoPath)
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
            res = TaskManager.StartResultTask<ResultObject>("VPS", "INSERT_DVD_DISK", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // load service settings
                StringDictionary settings = ServerController.GetServiceSettings(vm.ServiceId);
                string libPath = settings["DvdLibraryPath"];

                // combine full path
                string fullPath = Path.Combine(libPath, isoPath);

                // get proxy
                var vs = GetVirtualizationProxy(vm.ServiceId);

                // insert DVD
                JobResult result = vs.InsertDVD(vm.VirtualMachineId, fullPath);
                if (result.ReturnValue != ReturnCode.OK)
                {
                    LogReturnValueResult(res, result);
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

        public static ResultObject EjectDvdDisk(int itemId)
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
            res = TaskManager.StartResultTask<ResultObject>("VPS", "EJECT_DVD_DISK", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // get proxy
                var vs = GetVirtualizationProxy(vm.ServiceId);

                // insert DVD
                JobResult result = vs.EjectDVD(vm.VirtualMachineId);
                if (result.ReturnValue != ReturnCode.OK)
                {
                    LogReturnValueResult(res, result);
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
        public static VirtualMachineSnapshot[] GetVirtualMachineSnapshots(int itemId)
        {
            // load service item
            VMInfo vm = (VMInfo)PackageController.GetPackageItem(itemId);
            if (vm == null)
                return null;

            // get proxy
            var vs = GetVirtualizationProxy(vm.ServiceId);
            return vs.GetVirtualMachineSnapshots(vm.Name);
        }

        public static VirtualMachineSnapshot GetSnapshot(int itemId, string snaphostId)
        {
            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
                return null;

            // get proxy
            var vs = GetVirtualizationProxy(vm.ServiceId);
            return vs.GetSnapshot(snaphostId);
        }

        public static ResultObject CreateSnapshot(int itemId)
        {
            ResultObject res = new ResultObject();

            // load service item
            VMInfo vm = (VMInfo)PackageController.GetPackageItem(itemId);
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
            res = TaskManager.StartResultTask<ResultObject>("VPS", "TAKE_SNAPSHOT", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // get proxy
                var vs = GetVirtualizationProxy(vm.ServiceId);

                #region Check Quotas
                // check quotas
                List<string> quotaResults = new List<string>();
                PackageContext cntx = PackageController.GetPackageContext(vm.PackageId);

                                // check the number of created snapshots
                int createdNumber = vs.GetVirtualMachineSnapshots(vm.Name).Length;
                if (createdNumber >= vm.SnapshotsNumber)
                {
                    TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.QUOTA_EXCEEDED_SNAPSHOTS + ":" + vm.SnapshotsNumber);
                    return res;
                }
                #endregion

                // take snapshot
                JobResult result = vs.CreateSnapshot(vm.Name);
                if (result.ReturnValue != ReturnCode.JobStarted)
                {
                    LogReturnValueResult(res, result);
                    TaskManager.CompleteResultTask(res);
                    return res;
                }

                if (!JobCompleted(vs, result.Job))
                {
                    LogJobResult(res, result.Job);
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

        public static ResultObject ApplySnapshot(int itemId, string snapshotId)
        {
            ResultObject res = new ResultObject();

            // load service item
            VMInfo vm = (VMInfo)PackageController.GetPackageItem(itemId);
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
            res = TaskManager.StartResultTask<ResultObject>("VPS", "APPLY_SNAPSHOT", vm.Id, vm.Name, vm.PackageId);

            try
            {
                JobResult result = null;

                // get proxy
                var vs = GetVirtualizationProxy(vm.ServiceId);

                // check VM state
                VMInfo vps = vs.GetVirtualMachine(vm.Name);

                // stop virtual machine
                if ((vps.State != VMComputerSystemStateInfo.PowerOff) 
                 && (vps.State != VMComputerSystemStateInfo.Saved) 
                 && (vps.State != VMComputerSystemStateInfo.Stored))
                {
                    result = vs.ChangeVirtualMachineState(vm.Name, VirtualMachineRequestedState.TurnOff);
                    if (!JobCompleted(vs, result.Job))
                    {
                        LogJobResult(res, result.Job);
                        TaskManager.CompleteResultTask(res);
                        return res;
                    }
                }

                // take snapshot
                res.IsSuccess = true;
                result = vs.ApplySnapshot(vm.Name, snapshotId);
                if (result.ReturnValue != ReturnCode.OK)
                {
                    LogReturnValueResult(res, result);
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
            res.IsSuccess = true;
            return res;
        }

        public static ResultObject RenameSnapshot(int itemId, string snapshotId, string newName)
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
            res = TaskManager.StartResultTask<ResultObject>("VPS", "RENAME_SNAPSHOT", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // get proxy
                var vs = GetVirtualizationProxy(vm.ServiceId);

                // take snapshot
                JobResult result = vs.RenameSnapshot(vm.VirtualMachineId, snapshotId, newName);
                if (result.ReturnValue != ReturnCode.OK)
                {
                    LogReturnValueResult(res, result);
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

        public static ResultObject DeleteSnapshot(int itemId, string snapshotId)
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
            res = TaskManager.StartResultTask<ResultObject>("VPS", "DELETE_SNAPSHOT", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // get proxy
                var vs = GetVirtualizationProxy(vm.ServiceId);

                // take snapshot
                JobResult result = vs.DeleteSnapshot(vm.Name, snapshotId);
                if (result.ReturnValue != ReturnCode.JobStarted)
                {
                    LogReturnValueResult(res, result);
                    TaskManager.CompleteResultTask(res);
                    return res;
                }

                if (!JobCompleted(vs, result.Job))
                {
                    LogJobResult(res, result.Job);
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

        public static ResultObject DeleteSnapshotSubtree(int itemId, string snapshotId)
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
            res = TaskManager.StartResultTask<ResultObject>("VPS", "DELETE_SNAPSHOT_SUBTREE", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // get proxy
                var vs = GetVirtualizationProxy(vm.ServiceId);

                // take snapshot
                JobResult result = vs.DeleteSnapshotSubtree(snapshotId);
                if (result.ReturnValue != ReturnCode.JobStarted)
                {
                    LogReturnValueResult(res, result);
                    TaskManager.CompleteResultTask(res);
                    return res;
                }

                if (!JobCompleted(vs, result.Job))
                {
                    LogJobResult(res, result.Job);
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

        public static byte[] GetSnapshotThumbnail(int itemId, string snapshotId, ThumbnailSize size)
        {
            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
                return null;

            // get proxy
            var vs = GetVirtualizationProxy(vm.ServiceId);

            return vs.GetSnapshotThumbnailImage(snapshotId, size);
        }
        #endregion

        #region Network - External
        public static void ConfigureCreatedVMNetworkAdapters(VMInfo vmInfo)
        {
            var vs = GetVirtualizationProxy(vmInfo.ServiceId);
            vs.ConfigureCreatedVMNetworkAdapters(vmInfo);
        }


        public static NetworkAdapterDetails GetExternalNetworkDetails(int packageId)
        {
            // load service
            int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.VPSForPC);

            return GetExternalNetworkDetailsInternal(serviceId);
        }

        public static NetworkAdapterDetails GetExternalNetworkAdapterDetails(int itemId)
        {
            // load service item
            VMInfo vm = (VMInfo)PackageController.GetPackageItem(itemId);
            if (vm == null)
                return null;

            // get default NIC
            NetworkAdapterDetails nic = GetExternalNetworkDetailsInternal(vm.ServiceId);

            // update NIC
            nic.MacAddress = GetSymbolDelimitedMacAddress(vm.ExternalNicMacAddress, "-");

            // load IP addresses
            nic.IPAddresses = ObjectUtils.CreateListFromDataReader<NetworkAdapterIPAddress>(
                DataProvider.GetItemIPAddresses(SecurityContext.User.UserId, itemId, (int)IPAddressPool.VpsExternalNetwork)).ToArray();

            // update subnet CIDR
            foreach (NetworkAdapterIPAddress ip in nic.IPAddresses)
                ip.SubnetMaskCidr = GetSubnetMaskCidr(ip.SubnetMask);

            if (nic.IPAddresses.Length > 0)
            {
                // from primary address
                nic.SubnetMask = nic.IPAddresses[0].SubnetMask;
                nic.SubnetMaskCidr = GetSubnetMaskCidr(nic.SubnetMask);
                nic.DefaultGateway = nic.IPAddresses[0].DefaultGateway;
            }

            return nic;
        }

        public static NetworkAdapterDetails GetManagementNetworkDetails(int packageId)
        {
            // load service
            int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.VPSForPC);
            return GetManagementNetworkDetailsInternal(serviceId);
        }

        private static NetworkAdapterDetails GetExternalNetworkDetailsInternal(int serviceId)
        {
            // load service settings
            StringDictionary settings = ServerController.GetServiceSettings(serviceId);

            // create NIC object
            NetworkAdapterDetails nic = new NetworkAdapterDetails();
            nic.NetworkId = settings["ExternalNetworkId"];
            nic.PreferredNameServer = settings["ExternalPreferredNameServer"];
            nic.AlternateNameServer = settings["ExternalAlternateNameServer"];
            return nic;
        }

        public static NetworkAdapterDetails GetManagementNetworkAdapterDetails(int itemId)
        {
            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
                return null;

            // get default NIC
            NetworkAdapterDetails nic = GetManagementNetworkDetailsInternal(vm.ServiceId);

            // update NIC
            nic.MacAddress = GetSymbolDelimitedMacAddress(vm.ManagementNicMacAddress, "-");

            // load IP addresses
            nic.IPAddresses = ObjectUtils.CreateListFromDataReader<NetworkAdapterIPAddress>(
                DataProvider.GetItemIPAddresses(SecurityContext.User.UserId, itemId, (int)IPAddressPool.VpsManagementNetwork)).ToArray();

            // update subnet CIDR
            foreach (NetworkAdapterIPAddress ip in nic.IPAddresses)
                ip.SubnetMaskCidr = GetSubnetMaskCidr(ip.SubnetMask);

            if (nic.IPAddresses.Length > 0)
            {
                // from primary address
                nic.SubnetMask = nic.IPAddresses[0].SubnetMask;
                nic.SubnetMaskCidr = GetSubnetMaskCidr(nic.SubnetMask);
                nic.DefaultGateway = nic.IPAddresses[0].DefaultGateway;
            }

            return nic;
        }

        private static NetworkAdapterDetails GetManagementNetworkDetailsInternal(int serviceId)
        {
            // load service settings
            StringDictionary settings = ServerController.GetServiceSettings(serviceId);

            // create NIC object
            NetworkAdapterDetails nic = new NetworkAdapterDetails();
            nic.NetworkId = settings["ManagementNetworkId"];
            nic.IsDHCP = (String.Compare(settings["ManagementNicConfig"], "DHCP", true) == 0);

            if (!nic.IsDHCP)
            {
                nic.PreferredNameServer = settings["ManagementPreferredNameServer"];
                nic.AlternateNameServer = settings["ManagementAlternateNameServer"];
            }
            return nic;
        }

        public static ResultObject AddVirtualMachineExternalIPAddresses(int itemId, bool selectRandom, int addressesNumber, int[] addressIds, bool provisionKvp)
        {
            if (addressIds == null)
                throw new ArgumentNullException("addressIds");

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
            res = TaskManager.StartResultTask<ResultObject>("VPS", "ADD_EXTERNAL_IP", vm.Id, vm.Name, vm.PackageId);

            try
            {
                if (selectRandom)
                {
                    List<PackageIPAddress> ips = ServerController.GetPackageUnassignedIPAddresses(vm.PackageId, IPAddressPool.VpsExternalNetwork);
                    if (addressesNumber > ips.Count)
                    {
                        TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.NOT_ENOUGH_PACKAGE_IP_ADDRESSES);
                        return res;
                    }

                    // get next N unassigned addresses
                    addressIds = new int[addressesNumber];
                    for (int i = 0; i < addressesNumber; i++)
                        addressIds[i] = ips[i].PackageAddressID;
                }

                // add addresses
                foreach (int addressId in addressIds)
                    ServerController.AddItemIPAddress(itemId, addressId);

                // send KVP config items
                if(provisionKvp)
                    SendNetworkAdapterKVP(itemId, "External");
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.ADD_VIRTUAL_MACHINE_EXTERNAL_IP_ADDRESS_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public static ResultObject SetVirtualMachinePrimaryExternalIPAddress(int itemId, int packageAddressId, bool provisionKvp)
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
            res = TaskManager.StartResultTask<ResultObject>("VPS", "SET_PRIMARY_EXTERNAL_IP", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // call database
                ServerController.SetItemPrimaryIPAddress(itemId, packageAddressId);

                // send KVP config items
                if(provisionKvp)
                    SendNetworkAdapterKVP(itemId, "External");
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.SET_VIRTUAL_MACHINE_PRIMARY_EXTERNAL_IP_ADDRESS_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public static ResultObject DeleteVirtualMachineExternalIPAddresses(int itemId, int[] packageAddressIds, bool provisionKvp)
        {
            if (packageAddressIds == null)
                throw new ArgumentNullException("addressIds");

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
            res = TaskManager.StartResultTask<ResultObject>("VPS", "DELETE_EXTERNAL_IP", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // call database
                foreach (int packageAddressId in packageAddressIds)
                    ServerController.DeleteItemIPAddress(itemId, packageAddressId);

                // send KVP config items
                if(provisionKvp)
                    SendNetworkAdapterKVP(itemId, "External");
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.DELETE_VIRTUAL_MACHINE_EXTERNAL_IP_ADDRESS_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }
        #endregion

        #region Network – Private
        public static NetworkAdapterDetails GetPrivateNetworkDetails(int packageId)
        {
            // load service
            int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.VPSForPC);

            return GetPrivateNetworkDetailsInternal(serviceId);
        }

        public static NetworkAdapterDetails GetPrivateNetworkAdapterDetails(int itemId)
        {
            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
                return null;

            // load default internal adapter
            NetworkAdapterDetails nic = GetPrivateNetworkDetailsInternal(vm.ServiceId);

            // update NIC
            nic.MacAddress = GetSymbolDelimitedMacAddress(vm.PrivateNicMacAddress, "-");

            // load IP addresses
            nic.IPAddresses = ObjectUtils.CreateListFromDataReader<NetworkAdapterIPAddress>(
                DataProvider.GetItemPrivateIPAddresses(SecurityContext.User.UserId, itemId)).ToArray();

            foreach (NetworkAdapterIPAddress ip in nic.IPAddresses)
            {
                ip.SubnetMask = nic.SubnetMask;
                ip.SubnetMaskCidr = nic.SubnetMaskCidr;
                ip.DefaultGateway = nic.DefaultGateway;
            }

            return nic;
        }

        private static NetworkAdapterDetails GetPrivateNetworkDetailsInternal(int serviceId)
        {
            // load service settings
            StringDictionary settings = ServerController.GetServiceSettings(serviceId);

            // create NIC object
            NetworkAdapterDetails nic = new NetworkAdapterDetails();

            string networkFormat = settings["PrivateNetworkFormat"];
            if (String.IsNullOrEmpty(networkFormat))
            {
                // custom format
                nic.NetworkFormat = settings["PrivateIPAddress"];
				var v6 = IPAddress.Parse(nic.NetworkFormat).V6;
                nic.SubnetMask = GetPrivateNetworkSubnetMask(settings["PrivateSubnetMask"], v6);
            }
            else
            {
                // standard format
                string[] formatPair = settings["PrivateNetworkFormat"].Split('/');
                nic.NetworkFormat = formatPair[0];
				var v6 = IPAddress.Parse(nic.NetworkFormat).V6;
				nic.SubnetMask = GetPrivateNetworkSubnetMask(formatPair[1], v6);
            }

            nic.SubnetMaskCidr = GetSubnetMaskCidr(nic.SubnetMask);
            nic.DefaultGateway = settings["PrivateDefaultGateway"];
            nic.PreferredNameServer = settings["PrivatePreferredNameServer"];
            nic.AlternateNameServer = settings["PrivateAlternateNameServer"];

            return nic;
        }

        public static ResultObject AddVirtualMachinePrivateIPAddresses(int itemId, bool selectRandom, int addressesNumber, string[] addresses, bool provisionKvp)
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
            res = TaskManager.StartResultTask<ResultObject>("VPS", "ADD_PRIVATE_IP", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // load network adapter
                NetworkAdapterDetails nic = GetPrivateNetworkAdapterDetails(itemId);

                bool wasEmptyList = (nic.IPAddresses.Length == 0);

                // check IP addresses if they are specified
                List<string> checkResults = CheckPrivateIPAddresses(vm.PackageId, addresses);
                if (checkResults.Count > 0)
                {
                    res.ErrorCodes.AddRange(checkResults);
                    res.IsSuccess = false;
                    TaskManager.CompleteResultTask();
                    return res;
                }

                // load all existing private IP addresses
                List<PrivateIPAddress> ips = GetPackagePrivateIPAddresses(vm.PackageId);

                // sort them
                SortedList<IPAddress, string> sortedIps = GetSortedNormalizedIPAddresses(ips, nic.SubnetMask);

                if (selectRandom)
                {
                    // generate N number of IP addresses
                    addresses = new string[addressesNumber];
                    for (int i = 0; i < addressesNumber; i++)
                        addresses[i] = GenerateNextAvailablePrivateIP(sortedIps, nic.SubnetMask, nic.NetworkFormat);
                }

                PackageContext cntx = PackageController.GetPackageContext(vm.PackageId);
                QuotaValueInfo quota = cntx.Quotas[Quotas.VPS_PRIVATE_IP_ADDRESSES_NUMBER];
                if (quota.QuotaAllocatedValue != -1)
                {
                    int maxAddresses = quota.QuotaAllocatedValue - nic.IPAddresses.Length;

                    if (addresses.Length > maxAddresses)
                    {
                        TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.QUOTA_EXCEEDED_PRIVATE_ADDRESSES_NUMBER + ":" + maxAddresses);
                        return res;
                    }
                }

                // add addresses to database
                foreach (string address in addresses)
                    DataProvider.AddItemPrivateIPAddress(SecurityContext.User.UserId, itemId, address);

                // set primary IP address
                if (wasEmptyList)
                {
                    nic = GetPrivateNetworkAdapterDetails(itemId);
                    if (nic.IPAddresses.Length > 0)
                        SetVirtualMachinePrimaryPrivateIPAddress(itemId, nic.IPAddresses[0].AddressId, false);
                }

                // send KVP config items
                if(provisionKvp)
                    SendNetworkAdapterKVP(itemId, "Private");
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.ADD_VIRTUAL_MACHINE_PRIVATE_IP_ADDRESS_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        private static List<string> CheckPrivateIPAddresses(int packageId, string[] addresses)
        {
            List<string> codes = new List<string>();

            // check IP addresses if they are specified
            if (addresses != null && addresses.Length > 0)
            {
                // load network adapter
                NetworkAdapterDetails nic = GetPrivateNetworkDetails(packageId);

                foreach (string address in addresses)
                {
                    if (!CheckPrivateIPAddress(nic.SubnetMask, address))
                        codes.Add(VirtualizationErrorCodes.WRONG_PRIVATE_IP_ADDRESS_FORMAT + ":" + address);
                }
            }

            return codes;
        }

        public static ResultObject SetVirtualMachinePrimaryPrivateIPAddress(int itemId, int addressId, bool provisionKvp)
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
            res = TaskManager.StartResultTask<ResultObject>("VPS", "SET_PRIMARY_PRIVATE_IP", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // call data access layer
                DataProvider.SetItemPrivatePrimaryIPAddress(SecurityContext.User.UserId, itemId, addressId);

                // send KVP config items
                if(provisionKvp)
                    SendNetworkAdapterKVP(itemId, "Private");
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.SET_VIRTUAL_MACHINE_PRIMARY_PRIVATE_IP_ADDRESS_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public static ResultObject DeleteVirtualMachinePrivateIPAddresses(int itemId, int[] addressIds, bool provisionKvp)
        {
            if (addressIds == null)
                throw new ArgumentNullException("addressIds");

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
            res = TaskManager.StartResultTask<ResultObject>("VPS", "DELETE_PRIVATE_IP", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // call data access layer
                foreach (int addressId in addressIds)
                    DataProvider.DeleteItemPrivateIPAddress(SecurityContext.User.UserId, itemId, addressId);

                // send KVP config items
                if(provisionKvp)
                    SendNetworkAdapterKVP(itemId, "Private");
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.DELETE_VIRTUAL_MACHINE_PRIVATE_IP_ADDRESS_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        private static string GenerateNextAvailablePrivateIP(SortedList<IPAddress, string> ips, string subnetMask, string startIPAddress)
        {
            // start IP address
            var startIp = IPAddress.Parse(startIPAddress);
            var mask = IPAddress.Parse(subnetMask);

            var lastAddress = (startIp & ~mask) - 1;
            foreach (var addr in ips.Keys)
            {
                if ((addr - lastAddress) > 1)
                {
                    // it is a gap
                    break;
                }
                else
                {
                    lastAddress = addr;
                }
            }

            var genAddr = lastAddress + 1;

            // convert to IP address
            var ip = startIp & mask | genAddr;
			string genIP = ip.ToString();

            // store in cache
            ips.Add(genAddr, genIP);

            return genIP;
        }

        private static SortedList<IPAddress, string> GetSortedNormalizedIPAddresses(List<PrivateIPAddress> ips, string subnetMask)
        {
            var mask = IPAddress.Parse(subnetMask);
            SortedList<IPAddress, string> sortedIps = new SortedList<IPAddress, string>();
            foreach (PrivateIPAddress ip in ips)
            {
                var addr = ~mask & IPAddress.Parse(ip.IPAddress);
                sortedIps.Add(addr, ip.IPAddress);
            }
            return sortedIps;
        }

        private static string GetPrivateNetworkSubnetMask(string cidr, bool v6)
        {
			if (v6) return "/" + cidr;
			else return IPAddress.Parse("/" + cidr).ToV4MaskString();
        }

		private static string GetSubnetMaskCidr(string subnetMask) {
			if (String.IsNullOrEmpty(subnetMask))
				return subnetMask;
			var ip = IPAddress.Parse(subnetMask);
			if (ip.V4) {
				int cidr = 32;
				long mask = (long)ip.Address;
				while ((mask & 1) == 0 && cidr > 0) {
					mask >>= 1;
					cidr -= 1;
				}
				return cidr.ToString();
			} else {
				return ip.Cidr.ToString();
			}
		}

        private static bool CheckPrivateIPAddress(string subnetMask, string ipAddress)
        {
            var mask = IPAddress.Parse(subnetMask);
            var ip =  IPAddress.Parse(ipAddress);

            return ((mask & ip) == mask);
        }
        #endregion

        #region Virtual Machine Permissions
        public static List<VirtualMachinePermission> GetVirtualMachinePermissions(int itemId)
        {
            List<VirtualMachinePermission> result = new List<VirtualMachinePermission>();
            return result;
        }

        public static int UpdateVirtualMachineUserPermissions(int itemId, VirtualMachinePermission[] permissions)
        {
            // VPS - UPDATE_PERMISSIONS
            return 0;
        }
        #endregion

        #region Virtual Switches
        public static VirtualSwitch[] GetExternalSwitches(int serviceId, string computerName)
        {
			var vs = new VirtualizationServerForPrivateCloud();
            ServiceProviderProxy.Init(vs, serviceId);
            return vs.GetExternalSwitches(computerName);
        }
        #endregion

        #region Tools
        public static ResultObject DeleteVirtualMachine(int itemId, bool saveFiles, bool exportVps, string exportPath)
        {
            ResultObject res = new ResultObject();

            // load service item
            VMInfo vm = (VMInfo)PackageController.GetPackageItem(itemId);
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
            res = TaskManager.StartResultTask<ResultObject>("VPSForPC", "DELETE", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // get proxy
                var vs = GetVirtualizationProxy(vm.ServiceId);

                // check VM state
                VMInfo vps = vs.GetVirtualMachine(vm.Name);

                JobResult result = null;

                if (vps != null)
                {
                    #region turn off machine (if required)

                    // stop virtual machine
                    if (vps.State != VMComputerSystemStateInfo.PowerOff 
                     && vps.State != VMComputerSystemStateInfo.CreationFailed
                     && vps.State != VMComputerSystemStateInfo.Stored
                     && vps.State != VMComputerSystemStateInfo.Deleting
                     && vps.State != VMComputerSystemStateInfo.IncompleteVMConfig)
                     
                    {
                        TaskManager.Write("VPS_DELETE_TURN_OFF");

                        result = vs.ChangeVirtualMachineState(vm.Name, VirtualMachineRequestedState.TurnOff);
                        // check result
                        if (result.ReturnValue != ReturnCode.JobStarted 
                            && result.ReturnValue != ReturnCode.OK)
                        {
                            LogReturnValueResult(res, result);
                            TaskManager.CompleteResultTask(res);
                            return res;
                        }

                        // wait for completion
                        if (!JobCompleted(vs, result.Job))
                        {
                            LogJobResult(res, result.Job);
                            TaskManager.CompleteResultTask(res);
                            return res;
                        }
                    }
                    #endregion

                    #region delete machine
                    TaskManager.Write("VPSForPC_DELETE_DELETE");
                    result = vs.DeleteVirtualMachine(vm.Name);

                    // check result
                    if (result.ReturnValue != ReturnCode.JobStarted 
                        && result.ReturnValue != ReturnCode.OK)
                    {
                        LogReturnValueResult(res, result);
                        TaskManager.CompleteResultTask(res);
                        return res;
                    }

					//VMInfo vmState = vs.GetVirtualMachine(vm.Name);

					//while ((vmState != null) && vmState.State == VMComputerSystemStateInfo.Deleting)
					//{
					//    System.Threading.Thread.Sleep(5000);

					//    vmState = vs.GetVirtualMachine(vm.Name);
					//}

                    //// wait for completion
                    //if (!JobCompleted(vs, result.Job))
                    //{
                    //    LogJobResult(res, result.Job);
                    //    TaskManager.CompleteResultTask(res);
                    //    return res;
                    //}
                    #endregion
                }

                //#region delete files
                //if (!saveFiles)
                //{
                //    TaskManager.Write("VPS_DELETE_FILES", vm.VmPath);
                //    try
                //    {
                //        vs.DeleteRemoteFile(vm.VmPath);
                //    }
                //    catch (Exception ex)
                //    {
                //        res.ErrorCodes.Add(VirtualizationErrorCodes.DELETE_VM_FILES_ERROR + ": " + ex.Message);
                //    }
                //}
                //#endregion

                // delete meta item
                PackageController.DeletePackageItem(itemId);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.DELETE_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public static int ReinstallVirtualMachine(int itemId, string adminPassword, bool preserveVirtualDiskFiles,
            bool saveVirtualDisk, bool exportVps, string exportPath)
        {
            // VPS - REINSTALL
            return 0;
        }
        #endregion

        #region Help
        public static string GetVirtualMachineSummaryText(int itemId, bool emailMode, bool creation)
        {
            // load item
            VMInfo vm = GetVirtualMachineByItemId(itemId);

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

        public static ResultObject SendVirtualMachineSummaryLetter(int itemId, string to, string bcc, bool creation)
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

        public static string EvaluateVirtualMachineTemplate(int itemId, bool emailMode, bool creation, string template)
        {
            Hashtable items = new Hashtable();

            // load machine details
            VMInfo vm = GetVirtualMachineByItemId(itemId);
            if (vm == null)
                throw new Exception("VPS with the specified ID was not found.");

            // space info
            PackageInfo package = PackageController.GetPackage(vm.PackageId);
            items["space"] = package;

            // user info
            items["user"] = PackageController.GetPackageOwner(vm.PackageId);

            // VM item
            items["vm"] = vm;

            // load external NIC
            items["external_nic"] = GetExternalNetworkAdapterDetails(itemId);

            // load private NIC
            items["private_nic"] = GetPrivateNetworkAdapterDetails(itemId);

            // load private NIC
            items["management_nic"] = GetManagementNetworkAdapterDetails(itemId);

            // load service settings
            StringDictionary settings = ServerController.GetServiceSettings(vm.ServiceId);

            foreach (string key in settings.Keys)
                items[key] = settings[key];

            // service items
            items["email"] = emailMode;
            items["creation"] = creation;

            // evaluate template
            return PackageController.EvaluateTemplate(template, items);
        }
        #endregion

        #region Helper methods
        private static int GetServiceId(int packageId)
        {
            int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.VPSForPC);
            return serviceId;
        }

		private static VirtualizationServerForPrivateCloud GetVirtualizationProxyByPackageId(int packageId)
        {
            // get service
            int serviceId = GetServiceId(packageId);

            return GetVirtualizationProxy(serviceId);
        }

		private static VirtualizationServerForPrivateCloud GetVirtualizationProxy(int serviceId)
        {
			var ws = new VirtualizationServerForPrivateCloud();
            ServiceProviderProxy.Init(ws, serviceId);
            return ws;
        }

        public static VMInfo GetVirtualMachineByItemId(int itemId)
        {
            VMInfo vm = (VMInfo)PackageController.GetPackageItem(itemId);
            if (vm == null)
                return null;

            // Проверяем созданали виртуалка и если да то удаляем job
            //if (vm.CurrentJob != null)
            //{
            //    VirtualizationServerForPC vs = GetVirtualizationProxy(vm.ServiceId);
            //    VMInfo newVmInfo = vs.GetVirtualMachine(vm.Name);

            //    if (newVmInfo.State == vm.CurrentJob.TargetState)
            //    {
            //        vm.ProvisioningStatus = VirtualMachineProvisioningStatus.OK;
            //        vm.CurrentJob = null;
            //        PackageController.UpdatePackageItem(vm);
            //    }
            //}

            // check if task was aborted during provisioning
            if (!String.IsNullOrEmpty(vm.CurrentTaskId)
                && TaskManager.GetTask(vm.CurrentTaskId) == null)
            {
                // set to error
                vm.CurrentTaskId = null;
                if (vm.ProvisioningStatus != VirtualMachineProvisioningStatus.OK 
                    && vm.ProvisioningStatus != VirtualMachineProvisioningStatus.InProgress)
                {
                    vm.ProvisioningStatus = VirtualMachineProvisioningStatus.Error;
                }

                PackageController.UpdatePackageItem(vm);
            }

            vm.AdminPassword = CryptoUtils.Decrypt(vm.AdminPassword);
            return vm;
        }

        private static void LogReturnValueResult(ResultObject res, JobResult job)
        {
            res.ErrorCodes.Add(VirtualizationErrorCodes.JOB_START_ERROR + ":" + job.ReturnValue);
        }

        private static void LogJobResult(ResultObject res, ConcreteJob job)
        {
            res.ErrorCodes.Add(VirtualizationErrorCodes.JOB_FAILED_ERROR + ":" + job.ErrorDescription);
        }

		private static bool JobCompleted(VirtualizationServerForPrivateCloud vs, ConcreteJob job)
        {
            TaskManager.IndicatorMaximum = 100;
            bool jobCompleted = true;

            while (job.JobState == ConcreteJobState.Starting ||
                job.JobState == ConcreteJobState.Running)
            {
                System.Threading.Thread.Sleep(1000);
                VMInfo vm = vs.GetVirtualMachine(job.Id);
                TaskManager.IndicatorCurrent = 50;
                if (vm.State == job.TargetState)
                {
                    job.JobState = ConcreteJobState.Completed;
                }
                else if(vm.ProvisioningStatus == VirtualMachineProvisioningStatus.Error) 
                {
                    job.JobState = ConcreteJobState.Exception;
                }
            }

            if (job.JobState != ConcreteJobState.Completed)
            {
                jobCompleted = false;
            }

            TaskManager.IndicatorCurrent = 0; // reset indicator

            return jobCompleted;
        }

        private static string GenerateMacAddress()
        {
            return MS_MAC_PREFIX + Utils.GetRandomHexString(3);
        }

        #endregion

        #region Monitoring
        /// <summary>
        /// Get events log from device
        /// </summary>
        /// <param name="ItemID">ItemID describe current/selected VM</param>
        /// <returns>Collection of object described VM's events</returns>
        public static MonitoredObjectEvent[] GetDeviceEvents(int ItemID)
        {
            // Get required info about selected VM by ItemID
            MonitoredObjectEvent[] ret;

            try
            {
                ServiceProviderItem serviceProviderItem = PackageController.GetPackageItem(ItemID);
                ServiceInfo serviceInfo = ServerController.GetServiceInfo(serviceProviderItem.ServiceId);

                var vs = GetVirtualizationProxy(serviceProviderItem.ServiceId);

                ret = vs.GetDeviceEvents(serviceInfo.ServerName, serviceProviderItem.Name);
            }
            catch(Exception )
            {
                ret = null;
            }

            return ret;
        }

        /// <summary>
        /// Get alerts log for selected VM
        /// </summary>
        /// <param name="ItemID"></param>
        /// <returns></returns>
        public static MonitoredObjectAlert[] GetMonitoringAlerts(int ItemID)
        {
            MonitoredObjectAlert[] ret;
            // Get required info about selected VM by ItemID
            try
            {
                ServiceProviderItem serviceProviderItem = PackageController.GetPackageItem(ItemID);
                ServiceInfo serviceInfo = ServerController.GetServiceInfo(serviceProviderItem.ServiceId);

                var vs = GetVirtualizationProxy(serviceProviderItem.ServiceId);

                ret  = vs.GetMonitoringAlerts(serviceInfo.ServerName, serviceProviderItem.Name);
            }
            catch (Exception )
            { 
                ret = null;
            }
            return ret;
        }

        /// <summary>
        /// Get performance date for selected VM
        /// </summary>
        /// <param name="ItemID"></param>
        /// <param name="perf"></param>
        /// <param name="startPeriod"></param>
        /// <param name="endPeriod"></param>
        /// <returns></returns>
        public static PerformanceDataValue[] GetPerfomanceValue(int ItemID, PerformanceType perf, DateTime startPeriod, DateTime endPeriod)
        {
            PerformanceDataValue[] ret;
            // Get required info about selected VM by ItemID
            try
            {
                ServiceProviderItem serviceProviderItem = PackageController.GetPackageItem(ItemID);
                ServiceInfo serviceInfo = ServerController.GetServiceInfo(serviceProviderItem.ServiceId);

                var vs = GetVirtualizationProxy(serviceProviderItem.ServiceId);

                ret = vs.GetPerfomanceValue(serviceProviderItem.Name, perf, startPeriod, endPeriod);
            }
            catch (Exception )
            {
                ret = null;
            }
            return ret;
        }

        /// <summary>
        /// Get current networs by host
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public static VirtualNetworkInfo[] GetVirtualNetwork(int packageId)
        {
            int serviceId = GetServiceId(packageId);
            ServiceInfo serviceInfo = ServerController.GetServiceInfo(serviceId);

            var vs = GetVirtualizationProxy(serviceId);
            return vs.GetVirtualNetworkByHostName(serviceInfo.ServerName);
        }

        #endregion
    }
}
