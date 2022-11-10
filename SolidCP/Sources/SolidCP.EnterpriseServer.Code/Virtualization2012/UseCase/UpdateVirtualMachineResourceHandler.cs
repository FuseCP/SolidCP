using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers;
using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.VM;
using SolidCP.Providers.Common;
using SolidCP.Providers.Virtualization;
using SolidCP.Providers.Virtualization2012;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012.UseCase
{
    public static class UpdateVirtualMachineResourceHandler
    {
        private const string SHUTDOWN_REASON_CHANGE_CONFIG = "SolidCP - changing VPS configuration";
        private const short MINIMUM_DYNAMIC_MEMORY_BUFFER = 5;

        public static ResultObject UpdateVirtualMachineResource(int itemId, VirtualMachine vmSettings)
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
            PackageContext cntx = PackageController.GetPackageContext(vm.PackageId);

            var currentRam = vm.RamSize;
            var newRam = vmSettings.RamSize;

            // dynamic memory
            if (vm.DynamicMemory != null && vm.DynamicMemory.Enabled)
                currentRam = vm.DynamicMemory.Maximum;
            if (vmSettings.DynamicMemory != null && vmSettings.DynamicMemory.Enabled)
            {
                newRam = vmSettings.DynamicMemory.Maximum;

                if (vmSettings.RamSize > vmSettings.DynamicMemory.Maximum || vmSettings.RamSize < vmSettings.DynamicMemory.Minimum)
                    quotaResults.Add(VirtualizationErrorCodes.QUOTA_NOT_IN_DYNAMIC_RAM);
            }

            QuotaHelper.CheckNumericQuota(cntx, quotaResults, Quotas.VPS2012_CPU_NUMBER, vm.CpuCores, vmSettings.CpuCores, VirtualizationErrorCodes.QUOTA_EXCEEDED_CPU);
            QuotaHelper.CheckNumericQuota(cntx, quotaResults, Quotas.VPS2012_RAM, currentRam, newRam, VirtualizationErrorCodes.QUOTA_EXCEEDED_RAM);
            int newTotalHddSize = 0;
            int currentTotalHddSize = 0;
            for (int i = 0; i < vmSettings.HddSize.Length; i++)
            {
                newTotalHddSize += vmSettings.HddSize[i];
            }
            for (int i = 0; i < vm.HddSize.Length; i++)
            {
                currentTotalHddSize += vm.HddSize[i];
            }
            if (vmSettings.HddSize.Length > 1)
            {
                QuotaHelper.CheckNumericQuota(cntx, quotaResults, Quotas.VPS2012_HDD, currentTotalHddSize, newTotalHddSize, VirtualizationErrorCodes.QUOTA_EXCEEDED_HDDS);
                QuotaHelper.CheckNumericQuota(cntx, quotaResults, Quotas.VPS2012_ADDITIONAL_VHD_COUNT, vmSettings.HddSize.Length - 1, VirtualizationErrorCodes.QUOTA_EXCEEDED_ADDITIONAL_HDD);
            }
            else
            {
                QuotaHelper.CheckNumericQuota(cntx, quotaResults, Quotas.VPS2012_HDD, currentTotalHddSize, newTotalHddSize, VirtualizationErrorCodes.QUOTA_EXCEEDED_HDD);
            }
            QuotaHelper.CheckNumericQuota(cntx, quotaResults, Quotas.VPS2012_SNAPSHOTS_NUMBER, vmSettings.SnapshotsNumber, VirtualizationErrorCodes.QUOTA_EXCEEDED_SNAPSHOTS);

            QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_DVD_ENABLED, vmSettings.DvdDriveInstalled, VirtualizationErrorCodes.QUOTA_EXCEEDED_DVD_ENABLED);
            QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_BOOT_CD_ALLOWED, vmSettings.BootFromCD, VirtualizationErrorCodes.QUOTA_EXCEEDED_CD_ALLOWED);

            QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_START_SHUTDOWN_ALLOWED, vmSettings.StartTurnOffAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_START_SHUTDOWN_ALLOWED);
            QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_PAUSE_RESUME_ALLOWED, vmSettings.PauseResumeAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_PAUSE_RESUME_ALLOWED);
            QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_REBOOT_ALLOWED, vmSettings.RebootAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_REBOOT_ALLOWED);
            QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_RESET_ALOWED, vmSettings.ResetAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_RESET_ALOWED);
            QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_REINSTALL_ALLOWED, vmSettings.ReinstallAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_REINSTALL_ALLOWED);

            QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_EXTERNAL_NETWORK_ENABLED, vmSettings.ExternalNetworkEnabled, VirtualizationErrorCodes.QUOTA_EXCEEDED_EXTERNAL_NETWORK_ENABLED);
            QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_PRIVATE_NETWORK_ENABLED, vmSettings.PrivateNetworkEnabled, VirtualizationErrorCodes.QUOTA_EXCEEDED_PRIVATE_NETWORK_ENABLED);

            // check acceptable values
            if (vmSettings.RamSize <= 0)
                quotaResults.Add(VirtualizationErrorCodes.QUOTA_WRONG_RAM);
            foreach (var hddSize in vmSettings.HddSize)
            {
                if (hddSize <= 0)
                    quotaResults.Add(VirtualizationErrorCodes.QUOTA_WRONG_HDD);
            }
            if (vmSettings.SnapshotsNumber < 0)
                quotaResults.Add(VirtualizationErrorCodes.QUOTA_WRONG_SNAPSHOTS);

            // IOPS checks
            //TODO

            if (quotaResults.Count > 0)
            {
                res.ErrorCodes.AddRange(quotaResults);
                return res;
            }
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("VPS2012", "UPDATE_CONFIGURATION", vm.Id, vm.Name, vm.PackageId);

            try
            {
                JobResult result = null;

                // get proxy
                VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);

                VirtualMachine vps = vs.GetVirtualMachine(vm.VirtualMachineId);

                /////////////////////////////////////////////
                // update meta-item //TODO: rewrite 
                //vm = vmSettings; //heh we can't do that :(
                vm.CpuCores = vmSettings.CpuCores;
                vm.RamSize = vmSettings.RamSize;
                vm.HddSize = vmSettings.HddSize;
                vm.VirtualHardDrivePath = vmSettings.VirtualHardDrivePath;
                vm.HddMinimumIOPS = vmSettings.HddMinimumIOPS;
                vm.HddMaximumIOPS = vmSettings.HddMaximumIOPS;
                vm.SnapshotsNumber = vmSettings.SnapshotsNumber;

                vm.Version = vps.Version; //save true VM veriosn.

                vm.BootFromCD = vmSettings.BootFromCD;
                vm.NumLockEnabled = vmSettings.NumLockEnabled;
                vm.EnableSecureBoot = vmSettings.EnableSecureBoot;
                vm.DvdDriveInstalled = vmSettings.DvdDriveInstalled;

                vm.StartTurnOffAllowed = vmSettings.StartTurnOffAllowed;
                vm.PauseResumeAllowed = vmSettings.PauseResumeAllowed;
                vm.ResetAllowed = vmSettings.ResetAllowed;
                vm.RebootAllowed = vmSettings.RebootAllowed;
                vm.ReinstallAllowed = vmSettings.ReinstallAllowed;

                vm.ExternalNetworkEnabled = vmSettings.ExternalNetworkEnabled;
                vm.PrivateNetworkEnabled = vmSettings.PrivateNetworkEnabled;
                vm.defaultaccessvlan = vmSettings.defaultaccessvlan;
                vm.PrivateNetworkVlan = vmSettings.PrivateNetworkVlan;
                /////////////////////////////////////////////

                // dynamic memory
                #region dynamic memory
                if (vmSettings.DynamicMemory != null && vmSettings.DynamicMemory.Enabled)
                {
                    if (vmSettings.DynamicMemory.Buffer < MINIMUM_DYNAMIC_MEMORY_BUFFER) //minimum is 5.
                        vmSettings.DynamicMemory.Buffer = MINIMUM_DYNAMIC_MEMORY_BUFFER;
                    vm.DynamicMemory = vmSettings.DynamicMemory;
                }
                else
                    vm.DynamicMemory = null;
                #endregion

                // load service settings
                StringDictionary settings = ServerController.GetServiceSettings(vm.ServiceId);

                vm.ClusterName = (Utils.ParseBool(settings["UseFailoverCluster"], false)) ? settings["ClusterName"] : null;

                #region setup external network
                if (vm.ExternalNetworkEnabled
                    && String.IsNullOrEmpty(vm.ExternalNicMacAddress))
                {
                    // connect to network
                    vm.ExternalSwitchId = settings["ExternalNetworkId"];
                    vm.ExternalNicMacAddress = NetworkHelper.GenerateMacAddress();
                }
                #endregion

                #region setup private network
                if (vm.PrivateNetworkEnabled
                    && String.IsNullOrEmpty(vm.PrivateNicMacAddress))
                {
                    // connecto to network
                    vm.PrivateSwitchId = settings["PrivateNetworkId"];

                    if (String.IsNullOrEmpty(vm.PrivateSwitchId))
                    {
                        // create/load private virtual switch
                        vm.PrivateSwitchId = VirtualizationHelper.EnsurePrivateVirtualSwitch(vm);
                    }
                    vm.PrivateNicMacAddress = NetworkHelper.GenerateMacAddress();
                }
                #endregion

                bool isSuccessChangedWihoutReboot = false;
                if (!vmSettings.NeedReboot || vps.State != VirtualMachineState.Off)
                {
                    isSuccessChangedWihoutReboot = vs.IsTryToUpdateVirtualMachineWithoutRebootSuccess(vm);
                    TaskManager.Write(String.Format("Is update without reboot was success - {0}.", isSuccessChangedWihoutReboot));
                }

                bool wasStarted = false;
                if (!isSuccessChangedWihoutReboot)
                {
                    TaskManager.Write(String.Format("Shutting down the server for updating..."));
                    // stop VPS if required
                    // stop (shut down) virtual machine
                    #region stop VM
                    if (vps.State != VirtualMachineState.Off)
                    {
                        wasStarted = true;
                        ReturnCode code = vs.ShutDownVirtualMachine(vm.VirtualMachineId, true, SHUTDOWN_REASON_CHANGE_CONFIG);
                        if (code == ReturnCode.OK)
                        {
                            // spin until fully stopped
                            vps = vs.GetVirtualMachine(vm.VirtualMachineId);
                            short timeOut = 60 * 10; //10 min
                            while (vps.State != VirtualMachineState.Off) //TODO: rewrite
                            {
                                timeOut--;
                                System.Threading.Thread.Sleep(1000); // sleep 1 second
                                vps = vs.GetVirtualMachine(vm.VirtualMachineId);
                                if (timeOut == 0)// turnoff
                                {
                                    ResultObject turnOffResult = ChangeVirtualMachineStateHandler.ChangeVirtualMachineState(itemId, VirtualMachineRequestedState.TurnOff);
                                    if (!turnOffResult.IsSuccess)
                                    {
                                        TaskManager.CompleteResultTask(res);
                                        return turnOffResult;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // turn off
                            result = vs.ChangeVirtualMachineState(vm.VirtualMachineId, VirtualMachineRequestedState.TurnOff, vm.ClusterName);
                            if (!JobHelper.JobCompleted(vs, result.Job))
                            {
                                LogHelper.LogJobResult(res, result.Job);
                                TaskManager.CompleteResultTask(res);
                                return res;
                            }
                        }
                    } // end OFF
                    #endregion

                    // update configuration on virtualization server
                    vm = vs.UpdateVirtualMachine(vm);
                    TaskManager.Write(String.Format("The server configuration has been updated."));
                }

                // update meta item
                PackageController.UpdatePackageItem(vm);
                TaskManager.Write(String.Format("VM settings have been updated."));

                // unprovision external IP addresses
                if (!vm.ExternalNetworkEnabled)
                    ServerController.DeleteItemIPAddresses(itemId);
                //else //why should we do that??
                //    // send KVP config items
                //    SendNetworkAdapterKVP(itemId, "External");

                // unprovision private IP addresses
                if (!vm.PrivateNetworkEnabled)
                    DataProvider.DeleteItemPrivateIPAddresses(SecurityContext.User.UserId, itemId);
                //else //why should we do that??
                //    // send KVP config items
                //    SendNetworkAdapterKVP(itemId, "Private");

                // start if required
                if (wasStarted && !isSuccessChangedWihoutReboot)
                {
                    TaskManager.Write(String.Format("Starting the server..."));
                    result = vs.ChangeVirtualMachineState(vm.VirtualMachineId, VirtualMachineRequestedState.Start, vm.ClusterName);
                    if (!JobHelper.JobCompleted(vs, result.Job))
                    {
                        LogHelper.LogJobResult(res, result.Job);
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
    }
}
