using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers;
using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.PS;
using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.VM;
using SolidCP.Providers.Common;
using SolidCP.Providers.Virtualization;
//using SolidCP.Providers.Virtualization2012;
using SolidCP.Server.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012.Tasks
{
    internal class CreateVirtualMachineTask
    {
        internal static void CreateVirtualMachineNewTask(string taskId, VirtualMachine vm, LibraryItem osTemplate,
                int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses,
                int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses,
                string summaryLetterEmail)
        {
            // start task
            //TaskManager.StartTask(taskId, "VPS2012", "CREATE", vm.Name, vm.Id, vm.PackageId);
            int maximumExecutionSeconds = 60 * 60 * 2; //2 hours for this task. Anyway the Powershell cmd vhd convert has max 1 hour limit.
            TaskManager.StartTask(taskId, "VPS2012", "CREATE", vm.Name, vm.Id, vm.PackageId, maximumExecutionSeconds);

            CreateVirtualMachineInternal(taskId, vm, osTemplate, externalAddressesNumber, randomExternalAddresses, externalAddresses,
                privateAddressesNumber, randomPrivateAddresses, privateAddresses, summaryLetterEmail);

            // complete task
            TaskManager.CompleteTask();
        }

        internal static void CreateVirtualMachineContinueTask(string taskId, VirtualMachine vm, LibraryItem osTemplate,
                int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses,
                int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses,
                string summaryLetterEmail)
        {
            if (taskId != vm.CurrentTaskId) {
                throw new ArgumentException("The task is not the same as the virtual machine :" + taskId + " and VM task" + vm.CurrentTaskId);
            }

            if (TaskManager.GetTask(vm.CurrentTaskId) == null) {
                throw new NullReferenceException("There is not the Task with ID: " + taskId);
            }

            CreateVirtualMachineInternal(taskId, vm, osTemplate, externalAddressesNumber, randomExternalAddresses, externalAddresses,
                privateAddressesNumber, randomPrivateAddresses, privateAddresses, summaryLetterEmail);

        }


        #region Create
        private static void CreateVirtualMachineInternal(string taskId, VirtualMachine vm, LibraryItem osTemplate,
                int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses,
                int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses,
                string summaryLetterEmail)
        {
            // start task
            //TaskManager.StartTask(taskId, "VPS2012", "CREATE", vm.Name, vm.Id, vm.PackageId);
            //int maximumExecutionSeconds = 60 * 60 * 2; //2 hours for this task. Anyway the Powershell cmd vhd convert has max 1 hour limit.
            //TaskManager.StartTask(taskId, "VPS2012", "CREATE", vm.Name, vm.Id, vm.PackageId, maximumExecutionSeconds);

            bool isDiskConverted = false;
            try
            {
                // set Error flag
                vm.ProvisioningStatus = VirtualMachineProvisioningStatus.Error;

                // load proxy
                VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);

                // load service settings
                StringDictionary settings = ServerController.GetServiceSettings(vm.ServiceId);

                #region Setup External network
                TaskManager.Write("VPS_CREATE_SETUP_EXTERNAL_NETWORK");
                TaskManager.IndicatorCurrent = 0; // progress bar

                try
                {
                    if (vm.ExternalNetworkEnabled)
                    {
                        // provision IP addresses
                        ResultObject privResult = IpAddressExternalHelper.AddVirtualMachineInternalIPAddresses(vm.Id, randomExternalAddresses,
                            externalAddressesNumber, externalAddresses, 0, vm.defaultaccessvlan);

                        // set primary IP address
                        NetworkAdapterDetails extNic = NetworkAdapterDetailsHelper.GetExternalNetworkAdapterDetails(vm.Id);
                        if (extNic.IPAddresses.Length > 0)
                            IpAddressExternalHelper.SetVirtualMachinePrimaryExternalIPAddress(vm.Id, extNic.IPAddresses[0].AddressId, false);

                        // connect to network
                        vm.ExternalSwitchId = settings["ExternalNetworkId"];

                        bool generateMAC = true;
                        if (!string.IsNullOrEmpty(vm.ExternalNicMacAddress))
                        {
                            generateMAC = false;
                            vm.ExternalNicMacAddress = vm.ExternalNicMacAddress.Replace(" ", "").Replace(":", "").Replace("-", "");
                            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^[a-fA-F0-9]{12}$");
                            if (!regex.IsMatch(vm.ExternalNicMacAddress))
                                generateMAC = true;
                        }
                        if (generateMAC)
                            vm.ExternalNicMacAddress = NetworkHelper.GenerateMacAddress();
                    }
                    else
                    {
                        TaskManager.Write("VPS_CREATE_SETUP_EXTERNAL_NETWORK_SKIP");
                    }
                }
                catch (Exception ex)
                {
                    TaskManager.WriteError(ex, "VPS_CREATE_SETUP_EXTERNAL_NETWORK_ERROR");
                    return;
                }
                #endregion

                #region Setup Management network
                TaskManager.Write("VPS_CREATE_SETUP_MANAGEMENT_NETWORK");
                TaskManager.IndicatorCurrent = 0; //keep it on 0, because previous tasks extremely fast.

                try
                {
                    if (vm.ManagementNetworkEnabled)
                    {
                        // check that package contains unassigned IP
                        // that could be re-used
                        List<PackageIPAddress> packageIps = ServerController.GetPackageUnassignedIPAddresses(
                            vm.PackageId, IPAddressPool.VpsManagementNetwork);

                        if (packageIps.Count == 0)
                        {
                            // must be fresh space
                            // allocate package IP from the pool
                            List<IPAddressInfo> ips = ServerController.GetUnallottedIPAddresses(
                                vm.PackageId, ResourceGroups.VPS2012, IPAddressPool.VpsManagementNetwork);

                            if (ips.Count > 0)
                            {
                                // assign IP to the package
                                ServerController.AllocatePackageIPAddresses(vm.PackageId, new int[] { ips[0].AddressId });

                                // re-read package IPs
                                packageIps = ServerController.GetPackageUnassignedIPAddresses(
                                                vm.PackageId, IPAddressPool.VpsManagementNetwork);
                            }
                            else
                            {
                                // nothing to allocate - pool empty
                                TaskManager.WriteWarning("VPS_CREATE_SETUP_MANAGEMENT_NETWORK_POOL_EMPTY");
                            }
                        }

                        if (packageIps.Count > 0)
                        {
                            // assign to the item
                            ServerController.AddItemIPAddress(vm.Id, packageIps[0].PackageAddressID);

                            // set primary IP address
                            ServerController.SetItemPrimaryIPAddress(vm.Id, packageIps[0].PackageAddressID);

                            // connect to network
                            vm.ManagementSwitchId = settings["ManagementNetworkId"];
                            vm.ManagementNicMacAddress = NetworkHelper.GenerateMacAddress();
                        }
                    }
                    else
                    {
                        TaskManager.Write("VPS_CREATE_SETUP_MANAGEMENT_NETWORK_SKIP");
                    }
                }
                catch (Exception ex)
                {
                    TaskManager.WriteError(ex, "VPS_CREATE_SETUP_MANAGEMENT_NETWORK_ERROR");
                    return;
                }
                #endregion

                #region Setup Private network
                TaskManager.Write("VPS_CREATE_SETUP_PRIVATE_NETWORK");
                TaskManager.IndicatorCurrent = 0; //keep it on 0, because previous tasks extremely fast.

                try
                {
                    if (vm.PrivateNetworkEnabled)
                    {
                        NetworkAdapterDetails privNic = NetworkAdapterDetailsHelper.GetPrivateNetworkDetailsInternal(vm.ServiceId);

                        if (!privNic.IsDHCP)
                        {
                            // provision IP addresses
                            ResultObject extResult = IpAddressPrivateHelper.AddVirtualMachinePrivateIPAddresses(vm.Id, randomPrivateAddresses, privateAddressesNumber, privateAddresses, false, false, null, null, null, null);

                            // set primary IP address
                            privNic = NetworkAdapterDetailsHelper.GetPrivateNetworkAdapterDetails(vm.Id);
                            if (privNic.IPAddresses.Length > 0)
                                IpAddressPrivateHelper.SetVirtualMachinePrimaryPrivateIPAddress(vm.Id, privNic.IPAddresses[0].AddressId, false);
                        }

                        // connecto to network
                        vm.PrivateSwitchId = settings["PrivateNetworkId"];

                        if (string.IsNullOrEmpty(vm.PrivateSwitchId))
                        {
                            // create/load private virtual switch
                            vm.PrivateSwitchId = VirtualizationHelper.EnsurePrivateVirtualSwitch(vm);
                            if (vm.PrivateSwitchId == null)
                                return; // exit on error
                        }
                        vm.PrivateNicMacAddress = NetworkHelper.GenerateMacAddress();
                    }
                    else
                    {
                        TaskManager.Write("VPS_CREATE_SETUP_PRIVATE_NETWORK_SKIP");
                    }
                }
                catch (Exception ex)
                {
                    TaskManager.WriteError(ex, "VPS_CREATE_SETUP_PRIVATE_NETWORK_ERROR");
                    return;
                }
                #endregion

                // update service item
                VirtualMachineProvisioningStatus status = vm.ProvisioningStatus;
                vm.ProvisioningStatus = VirtualMachineProvisioningStatus.InProgress;
                vm.CreationTime = DateTime.Now.ToString();
                PackageController.UpdatePackageItem(vm);
                vm.ProvisioningStatus = status;

                #region Copy/convert VHD
                JobResult result = null;
                ReturnCode code = ReturnCode.OK;

                TaskManager.Write("VPS_CREATE_OS_GENERATION", osTemplate.Generation.ToString());
                if (osTemplate.Generation > 1)
                    TaskManager.Write("VPS_CREATE_OS_SECUREBOOT", osTemplate.EnableSecureBoot ? "Enabled" : "Disabled");
                TaskManager.Write("VPS_CREATE_OS_TEMPLATE", osTemplate.Name);
                TaskManager.Write("VPS_CREATE_CONVERT_VHD");
                if (osTemplate.VhdBlockSizeBytes > 0)
                    TaskManager.Write("VPS_CREATE_CONVERT_SET_VHD_BLOCKSIZE", osTemplate.VhdBlockSizeBytes.ToString());
                TaskManager.Write("VPS_CREATE_CONVERT_SOURCE_VHD", vm.OperatingSystemTemplatePath);
                TaskManager.Write("VPS_CREATE_CONVERT_DEST_VHD", vm.VirtualHardDrivePath);
                TaskManager.IndicatorCurrent = 0; //keep it on 0, because previous tasks extremely fast.
                try
                {
                    // convert VHD
                    VirtualHardDiskType vhdType = (VirtualHardDiskType)Enum.Parse(typeof(VirtualHardDiskType), settings["VirtualDiskType"], true);

                    for (int i = 0; i < vm.VirtualHardDrivePath.Length; i++)
                    {
                        if (i == 0)
                        {
                            result = vs.ConvertVirtualHardDisk(vm.OperatingSystemTemplatePath, vm.VirtualHardDrivePath[i], vhdType, osTemplate.VhdBlockSizeBytes);
                        }
                        else
                        {
                            result = vs.CreateVirtualHardDisk(vm.VirtualHardDrivePath[i], vhdType, osTemplate.VhdBlockSizeBytes, (ulong)vm.HddSize[i]);
                        }

                        // check return
                        if (result.ReturnValue != ReturnCode.JobStarted)
                        {
                            TaskManager.WriteError("VPS_CREATE_CONVERT_VHD_ERROR_JOB_START", result.ReturnValue.ToString());
                            return;
                        }

                        // wait for completion
                        if (!JobHelper.TryJobCompleted(vs, result.Job, false)) //there we are updating TaskManager.IndicatorCurrent
                        {
                            TaskManager.WriteError("VPS_CREATE_CONVERT_VHD_ERROR_JOB_EXEC", result.Job.ErrorDescription.ToString());
                            return;
                        }
                    }

                    isDiskConverted = true; //We are sure that the disc was copied.
                }
                catch (Exception ex)
                {
                    TaskManager.WriteError(ex, "VPS_CREATE_CONVERT_VHD_ERROR");
                    return;
                }
                #endregion
                // reset the progress bar to 70, from 100 after VHD conversion, just to keep the progress bar nice
                // in most case we anyway don't get 100, only if there are no empty blocks in the VHD template at the end of the VHD and Windows cannot skip them when converting (that usually happens if templates wasn't defragmentated).
                TaskManager.IndicatorCurrent = 70;

                #region Get VHD info
                VirtualHardDiskInfo vhdInfo = null;
                try
                {
                    vhdInfo = vs.GetVirtualHardDiskInfo(vm.VirtualHardDrivePath[0]);
                }
                catch (Exception ex)
                {
                    TaskManager.WriteError(ex, "VPS_CREATE_GET_VHD_INFO");
                    return;
                }

                if (vhdInfo == null || vhdInfo.InUse)
                {
                    // master VHD is in use
                    TaskManager.WriteError("VPS_CREATE_MASTER_VHD_IN_USE");
                    return;
                }

                // check if it should be expanded
                int hddSizeGB = Convert.ToInt32(vhdInfo.MaxInternalSize / VirtualizationUtils.Size1G);

                TaskManager.Write("VPS_CREATE_EXPAND_SOURCE_VHD_SIZE", hddSizeGB.ToString());
                TaskManager.Write("VPS_CREATE_EXPAND_DEST_VHD_SIZE", vm.HddSize[0].ToString());
                #endregion

                #region Expand VHD
                bool expanded = false;
                if (vm.HddSize[0] > hddSizeGB)
                {
                    TaskManager.Write("VPS_CREATE_EXPAND_VHD");
                    TaskManager.IndicatorCurrent = 71;

                    // expand VHD
                    try
                    {
                        result = vs.ExpandVirtualHardDisk(vm.VirtualHardDrivePath[0], (ulong)vm.HddSize[0]);
                    }
                    catch (Exception ex)
                    {
                        TaskManager.WriteError(ex, "VPS_CREATE_EXPAND_VHD_ERROR");
                        return;
                    }

                    // check return
                    if (result.ReturnValue != ReturnCode.JobStarted)
                    {
                        // error starting Expand job
                        TaskManager.WriteError("VPS_CREATE_EXPAND_VHD_ERROR_JOB_START", result.ReturnValue.ToString());
                        return;
                    }

                    // wait for completion
                    if (!JobHelper.JobCompleted(vs, result.Job, false))
                    {
                        // error executing Expand job
                        TaskManager.WriteError("VPS_CREATE_EXPAND_VHD_ERROR_JOB_EXEC", result.Job.ErrorDescription);
                        return;
                    }
                    expanded = true;
                }
                else
                {
                    // skip expanding
                    TaskManager.Write("VPS_CREATE_EXPAND_VHD_SKIP");
                }
                #endregion

                #region Process VHD contents
                // mount VHD
                if (expanded && osTemplate.ProcessVolume != -1 || osTemplate.SysprepFiles != null && osTemplate.SysprepFiles.Length > 0)
                {
                    try
                    {
                        #region Mount VHD
                        byte attemps = 3;
                        MountedDiskInfo mountedInfo = null;

                        while (attemps > 0)
                        {
                            try
                            {
                                //TODO: Is possible to lose vm.VirtualHardDrivePath ? Add Check?
                                mountedInfo = vs.MountVirtualHardDisk(vm.VirtualHardDrivePath[0]);
                                attemps = 0;
                            }
                            catch (Exception ex)
                            {
                                attemps--;
                                if (attemps == 0)
                                    throw ex;

                                Thread.Sleep(5000); //wait and try again.                                
                            }
                        }

                        if (mountedInfo == null)
                        {
                            // mount returned NULL
                            TaskManager.WriteError("VPS_CREATE_MOUNT_VHD_NULL");
                            return;
                        }
                        #endregion
                        TaskManager.IndicatorCurrent = 75;

                        #region Expand volume
                        if (expanded && osTemplate.ProcessVolume != -1 && mountedInfo.DiskVolumes.Length > 0)
                        {
                            try
                            {
                                vs.ExpandDiskVolume(mountedInfo.DiskAddress, mountedInfo.DiskVolumes[osTemplate.ProcessVolume]);
                            }
                            catch (Exception ex)
                            {
                                TaskManager.WriteError(ex, "VPS_CREATE_DISKPART_ERROR");
                            }
                        }
                        else
                        {
                            TaskManager.Write("VPS_CREATE_EXPAND_VHD_SKIP_NO_VOLUMES");
                        }
                        #endregion
                        TaskManager.IndicatorCurrent = 80;

                        #region Sysprep
                        if (mountedInfo.DiskVolumes.Length > 0
                            && osTemplate.ProcessVolume != -1
                            && osTemplate.SysprepFiles != null && osTemplate.SysprepFiles.Length > 0)
                        {
                            foreach (string remoteFile in osTemplate.SysprepFiles)
                            {
                                try
                                {
                                    TaskManager.Write("VPS_CREATE_SYSPREP_FILE", remoteFile);

                                    // build remote path
                                    string path = remoteFile;
                                    if (!remoteFile.StartsWith("\\"))
                                        path = remoteFile.Substring(remoteFile.IndexOf("\\"));

                                    path = string.Format("{0}:{1}", mountedInfo.DiskVolumes[osTemplate.ProcessVolume], path);

                                    // read remote file
                                    string contents = vs.ReadRemoteFile(path);
                                    if (contents == null)
                                    {
                                        TaskManager.Write("VPS_CREATE_SYSPREP_FILE_NOT_FOUND", remoteFile);
                                        continue;
                                    }

                                    // process file contents
                                    contents = VirtualMachineHelper.EvaluateVirtualMachineTemplate(vm.Id, false, false, contents);

                                    // write remote file
                                    vs.WriteRemoteFile(path, contents);

                                    TaskManager.Write("OS Time Zone: {0}", osTemplate.TimeZoneId);
                                }
                                catch (Exception ex)
                                {
                                    TaskManager.WriteError("VPS_CREATE_SYSPREP_FILE_ERROR", ex.Message);
                                }
                            }
                        }
                        #endregion
                        TaskManager.IndicatorCurrent = 85;

                        #region Unmount VHD
                        try
                        {
                            code = vs.UnmountVirtualHardDisk(vm.VirtualHardDrivePath[0]);
                            if (code != ReturnCode.OK)
                            {
                                TaskManager.WriteError("VPS_CREATE_UNMOUNT_ERROR_JOB_START", code.ToString());
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            TaskManager.WriteError(ex, "VPS_CREATE_UNMOUNT_ERROR");
                            return;
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        // error mounting
                        TaskManager.WriteError(ex, "VPS_CREATE_MOUNT_VHD");
                        return;
                    }
                } // end if (expanded ...
                #endregion
                TaskManager.IndicatorCurrent = 90;

                vm.ClusterName = Utils.ParseBool(settings["UseFailoverCluster"], false) ? settings["ClusterName"] : null;

                #region Create Virtual Machine
                TaskManager.Write("VPS_CREATE_CPU_CORES", vm.CpuCores.ToString());
                TaskManager.Write("VPS_CREATE_RAM_SIZE", vm.RamSize.ToString());
                TaskManager.Write("VPS_CREATE_VHD_MIN_IOPS", vm.HddMinimumIOPS.ToString());
                TaskManager.Write("VPS_CREATE_VHD_MAX_IOPS", vm.HddMaximumIOPS.ToString());
                TaskManager.Write("VPS_CREATE_CREATE_VM");
                TaskManager.IndicatorCurrent = 95;
                // create virtual machine
                bool isCreatedVMConfigSuccess = false;
                for (byte attempt = 1; attempt <= 3; attempt++)
                {
                    try
                    {
                        // create
                        var tempVm = vs.CreateVirtualMachine(vm); //sometimes it just not works and returns nothing
                        if (!string.IsNullOrEmpty(tempVm.VirtualMachineId))
                        {
                            vm = tempVm;
                            isCreatedVMConfigSuccess = true;
                            break;
                        }
                        TaskManager.Write("VPS_CREATE_CREATE_VM_ATTEMPT", attempt.ToString());
                        Thread.Sleep(2000 * attempt);

                    }
                    catch (Exception ex)
                    {
                        TaskManager.WriteError(ex, "VPS_CREATE_CREATE_VM_ERROR");
                        return;
                    }
                }
                if (!isCreatedVMConfigSuccess)
                {
                    TaskManager.WriteError("VPS_CREATE_CREATE_VM_ERROR");
                    return;
                }

                // update meta item
                PackageController.UpdatePackageItem(vm);

                TaskManager.Write("VPS_CREATE_CREATED_VM");
                #endregion

                // set OK flag
                vm.ProvisioningStatus = VirtualMachineProvisioningStatus.OK;

                #region Send KVP
                // configure computer name
                if (osTemplate.ProvisionComputerName)
                {
                    TaskManager.Write("VPS_CREATE_SET_COMPUTER_NAME_KVP");
                    KvpExchangeHelper.SendComputerNameKVP(vm.Id, vm.Name);
                }

                // change administrator password
                if (osTemplate.ProvisionAdministratorPassword)
                {
                    TaskManager.Write("VPS_CREATE_SET_PASSWORD_KVP");
                    KvpExchangeHelper.SendAdministratorPasswordKVP(vm.Id, CryptoUtils.Decrypt(vm.AdministratorPassword), false); //TODO check mb need true
                }

                // configure network adapters
                if (osTemplate.ProvisionNetworkAdapters)
                {
                    // external NIC
                    TaskManager.Write("VPS_CREATE_SET_EXTERNAL_NIC_KVP");
                    if (vm.ExternalNetworkEnabled)
                    {
                        result = KvpExchangeHelper.SendNetworkAdapterKVP(vm.Id, "External");

                        if (result.ReturnValue != ReturnCode.JobStarted)
                            TaskManager.WriteWarning("VPS_CREATE_SET_EXTERNAL_NIC_KVP_ERROR", result.ReturnValue.ToString());
                    }

                    // management NIC
                    TaskManager.Write("VPS_CREATE_SET_MANAGEMENT_NIC_KVP");
                    if (vm.ManagementNetworkEnabled)
                    {
                        result = KvpExchangeHelper.SendNetworkAdapterKVP(vm.Id, "Management");

                        if (result.ReturnValue != ReturnCode.JobStarted)
                            TaskManager.WriteWarning("VPS_CREATE_SET_MANAGEMENT_NIC_KVP_ERROR", result.ReturnValue.ToString());
                    }

                    // private NIC
                    TaskManager.Write("VPS_CREATE_SET_PRIVATE_NIC_KVP");
                    if (vm.PrivateNetworkEnabled)
                    {
                        result = KvpExchangeHelper.SendNetworkAdapterKVP(vm.Id, "Private");

                        if (result.ReturnValue != ReturnCode.JobStarted)
                            TaskManager.WriteWarning("VPS_CREATE_SET_PRIVATE_NIC_KVP_ERROR", result.ReturnValue.ToString());
                    }
                }
                #endregion

                PowerShellScript.CheckCustomPsScript(PsScriptPoint.after_creation, vm);

                #region Start VPS
                TaskManager.Write("VPS_CREATE_START_VPS");
                TaskManager.IndicatorCurrent = 98;

                try
                {
                    // start virtual machine
                    result = vs.ChangeVirtualMachineState(vm.VirtualMachineId, VirtualMachineRequestedState.Start, vm.ClusterName);

                    // check return
                    if (result.ReturnValue == ReturnCode.JobStarted)
                    {
                        // wait for completion
                        if (!JobHelper.JobCompleted(vs, result.Job, false))
                        {
                            TaskManager.WriteWarning("VPS_CREATE_START_VPS_ERROR_JOB_EXEC", result.Job.ErrorDescription.ToString());
                        }
                    }
                    else
                    {
                        TaskManager.WriteWarning("VPS_CREATE_START_VPS_ERROR_JOB_START", result.ReturnValue.ToString());
                    }
                }
                catch (Exception ex)
                {
                    TaskManager.WriteWarning("VPS_CREATE_START_VPS_ERROR", ex.Message);
                }
                TaskManager.Write("VPS_CREATE_STARTED_VPS");
                #endregion

                #region Send Summary letter
                // send summary e-mail
                if (!string.IsNullOrEmpty(summaryLetterEmail))
                {
                    VirtualizationServerController2012.SendVirtualMachineSummaryLetter(vm.Id, summaryLetterEmail, null, true);
                }
                #endregion
                TaskManager.IndicatorCurrent = 99; //CompleteTask make it to 100
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex, VirtualizationErrorCodes.CREATE_ERROR);
                return;
            }
            finally
            {
                // reset task ID
                vm.CurrentTaskId = null;
                PackageController.UpdatePackageItem(vm);

                if (vm.ProvisioningStatus == VirtualMachineProvisioningStatus.OK)
                    TaskManager.Write("VPS_CREATE_SUCCESS");
                else if (vm.ProvisioningStatus == VirtualMachineProvisioningStatus.Error)
                {
                    TaskManager.Write("VPS_CREATE_ERROR_END");
                    if (isDiskConverted)
                    {
                        //TODO: Add deletion of the broken file. (2019)
                        //// get proxy
                        //VirtualizationServer2012 vs = GetVirtualizationProxy(vm.ServiceId);
                        //if (vs.IsEmptyFolders(vm.RootFolderPath))
                        //{
                        //    vs.DeleteRemoteFile(vm.RootFolderPath);
                        //}
                    }
                }

                //// complete task
                //TaskManager.CompleteTask();
            }
        }
        #endregion
    }
}
