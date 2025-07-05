using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers;
using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.VM;
using SolidCP.Providers;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.Virtualization;
//using SolidCP.Providers.Virtualization2012;
using SolidCP.Server.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012.UseCase
{
    public class CreateVirtualMachineHandler: ControllerBase
    {
        public CreateVirtualMachineHandler(ControllerBase provider) : base(provider) { }

        public IntResult CreateNewVirtualMachineInternal(VirtualMachine VMSettings, string osTemplateFile, string password, string summaryLetterEmail,
            int externalAddressesNumber, bool randomExternalAddresses, int[] externalAddresses,
            int privateAddressesNumber, bool randomPrivateAddresses, string[] privateAddresses,
            int dmzAddressesNumber, bool randomDmzAddresses, string[] dmzAddresses,
            bool createMetaItem)
        {
            // result object
            IntResult res = new IntResult();

            // meta item
            VirtualMachine vm = null;

            int packageId = VMSettings.PackageId;
            try
            {
                #region Check account and space statuses
                // check account
                if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                    return res;

                // check package
                if (!SecurityContext.CheckPackage(res, packageId, DemandPackage.IsActive))
                    return res;

                #endregion                

                //TODO: move to separate method
                #region Check Quotas
                // check quotas
                List<string> quotaResults = new List<string>();
                PackageContext cntx = PackageController.GetPackageContext(packageId);

                if (createMetaItem) //don't need check Quota of some parameters if we reinstall server.
                {
                    // dynamic memory
                    var newRam = VMSettings.RamSize;
                    if (VMSettings.DynamicMemory != null && VMSettings.DynamicMemory.Enabled)
                    {
                        newRam = VMSettings.DynamicMemory.Maximum;

                        if (VMSettings.RamSize > VMSettings.DynamicMemory.Maximum || VMSettings.RamSize < VMSettings.DynamicMemory.Minimum)
                            quotaResults.Add(VirtualizationErrorCodes.QUOTA_NOT_IN_DYNAMIC_RAM);
                    }

                    QuotaHelper.CheckListsQuota(cntx, quotaResults, Quotas.VPS2012_SERVERS_NUMBER, VirtualizationErrorCodes.QUOTA_EXCEEDED_SERVERS_NUMBER);

                    QuotaHelper.CheckNumericQuota(cntx, quotaResults, Quotas.VPS2012_CPU_NUMBER, VMSettings.CpuCores, VirtualizationErrorCodes.QUOTA_EXCEEDED_CPU);
                    QuotaHelper.CheckNumericQuota(cntx, quotaResults, Quotas.VPS2012_RAM, newRam, VirtualizationErrorCodes.QUOTA_EXCEEDED_RAM);
                    int totalHddSize = 0;
                    for (int i = 0; i < VMSettings.HddSize.Length; i++)
                    {
                        totalHddSize += VMSettings.HddSize[i];
                    }
                    if (VMSettings.HddSize.Length > 1)
                    {
                        QuotaHelper.CheckNumericQuota(cntx, quotaResults, Quotas.VPS2012_HDD, totalHddSize, VirtualizationErrorCodes.QUOTA_EXCEEDED_HDDS);
                        QuotaHelper.CheckNumericQuota(cntx, quotaResults, Quotas.VPS2012_ADDITIONAL_VHD_COUNT, VMSettings.HddSize.Length - 1, VirtualizationErrorCodes.QUOTA_EXCEEDED_ADDITIONAL_HDD);
                    }
                    else
                    {
                        QuotaHelper.CheckNumericQuota(cntx, quotaResults, Quotas.VPS2012_HDD, totalHddSize, VirtualizationErrorCodes.QUOTA_EXCEEDED_HDD);
                    }
                    QuotaHelper.CheckNumericQuota(cntx, quotaResults, Quotas.VPS2012_SNAPSHOTS_NUMBER, VMSettings.SnapshotsNumber, VirtualizationErrorCodes.QUOTA_EXCEEDED_SNAPSHOTS);
                }
                QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_DVD_ENABLED, VMSettings.DvdDriveInstalled, VirtualizationErrorCodes.QUOTA_EXCEEDED_DVD_ENABLED);
                QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_BOOT_CD_ALLOWED, VMSettings.BootFromCD, VirtualizationErrorCodes.QUOTA_EXCEEDED_CD_ALLOWED);

                QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_START_SHUTDOWN_ALLOWED, VMSettings.StartTurnOffAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_START_SHUTDOWN_ALLOWED);
                QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_PAUSE_RESUME_ALLOWED, VMSettings.PauseResumeAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_PAUSE_RESUME_ALLOWED);
                QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_REBOOT_ALLOWED, VMSettings.RebootAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_REBOOT_ALLOWED);
                QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_RESET_ALOWED, VMSettings.ResetAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_RESET_ALOWED);
                QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_REINSTALL_ALLOWED, VMSettings.ReinstallAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_REINSTALL_ALLOWED);

                QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_EXTERNAL_NETWORK_ENABLED, VMSettings.ExternalNetworkEnabled, VirtualizationErrorCodes.QUOTA_EXCEEDED_EXTERNAL_NETWORK_ENABLED);
                QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_PRIVATE_NETWORK_ENABLED, VMSettings.PrivateNetworkEnabled, VirtualizationErrorCodes.QUOTA_EXCEEDED_PRIVATE_NETWORK_ENABLED);
                QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_DMZ_NETWORK_ENABLED, VMSettings.DmzNetworkEnabled, VirtualizationErrorCodes.QUOTA_EXCEEDED_DMZ_NETWORK_ENABLED);


                // check external addresses number
                if (!randomExternalAddresses && externalAddresses != null)
                    externalAddressesNumber = externalAddresses.Length;

                int maxAddresses = ServerController.GetPackageUnassignedIPAddresses(packageId, IPAddressPool.VpsExternalNetwork).Count; //Get num IPs if they exist
                if (maxAddresses == 0) //get quota for Unallotted IPs network     
                {
                    int max = cntx.Quotas[Quotas.VPS2012_EXTERNAL_IP_ADDRESSES_NUMBER].QuotaAllocatedValue != -1 ?
                        cntx.Quotas[Quotas.VPS2012_EXTERNAL_IP_ADDRESSES_NUMBER].QuotaAllocatedValue : int.MaxValue;
                    maxAddresses = max - cntx.Quotas[Quotas.VPS2012_EXTERNAL_IP_ADDRESSES_NUMBER].QuotaUsedValue;
                }


                if (VMSettings.ExternalNetworkEnabled && externalAddressesNumber > maxAddresses)
                    quotaResults.Add(VirtualizationErrorCodes.QUOTA_EXCEEDED_EXTERNAL_ADDRESSES_NUMBER + ":" + maxAddresses.ToString());

                // check private addresses number
                if (!randomPrivateAddresses && privateAddresses != null)
                    privateAddressesNumber = privateAddresses.Length;
                QuotaHelper.CheckNumericQuota(cntx, quotaResults, Quotas.VPS2012_PRIVATE_IP_ADDRESSES_NUMBER, privateAddressesNumber, VirtualizationErrorCodes.QUOTA_EXCEEDED_PRIVATE_ADDRESSES_NUMBER);

                // check dmz addresses number
                if (!randomDmzAddresses && dmzAddresses != null)
                    dmzAddressesNumber = dmzAddresses.Length;
                QuotaHelper.CheckNumericQuota(cntx, quotaResults, Quotas.VPS2012_DMZ_IP_ADDRESSES_NUMBER, dmzAddressesNumber, VirtualizationErrorCodes.QUOTA_EXCEEDED_DMZ_ADDRESSES_NUMBER);

                // check management network parameters
                NetworkAdapterDetails manageNic = NetworkAdapterDetailsHelper.GetManagementNetworkDetails(packageId);
                if (!String.IsNullOrEmpty(manageNic.NetworkId))
                {
                    // network enabled - check management IPs pool
                    int manageIpsNumber = ServerController.GetUnallottedIPAddresses(
                            packageId, ResourceGroups.VPS2012, IPAddressPool.VpsManagementNetwork).Count;

                    if (manageIpsNumber == 0)
                        quotaResults.Add(VirtualizationErrorCodes.QUOTA_EXCEEDED_MANAGEMENT_NETWORK);
                }

                // check acceptable values
                if (VMSettings.RamSize < 32)
                    quotaResults.Add(VirtualizationErrorCodes.QUOTA_WRONG_RAM_HV);

                if (VMSettings.HddSize.Length == 0) //if we pass the empty array of HddSize it broke everything.
                    quotaResults.Add(VirtualizationErrorCodes.QUOTA_WRONG_HDD);

                foreach (var hddSize in VMSettings.HddSize)
                {
                    if (hddSize <= 0)
                        quotaResults.Add(VirtualizationErrorCodes.QUOTA_WRONG_HDD);
                }
                if (VMSettings.SnapshotsNumber < 0)
                    quotaResults.Add(VirtualizationErrorCodes.QUOTA_WRONG_SNAPSHOTS);

                if (quotaResults.Count > 0)
                {
                    res.ErrorCodes.AddRange(quotaResults);
                    return res;
                }
                #endregion

                #region Check input parameters
                // check private network IP addresses if they are specified
                List<string> checkResults = IpAddressPrivateHelper.CheckPrivateIPAddresses(packageId, privateAddresses);
                if (checkResults.Count > 0)
                {
                    res.ErrorCodes.AddRange(checkResults);
                    return res;
                }

                // check dmz network IP addresses if they are specified
                checkResults = IpAddressPrivateHelper.CheckDmzIPAddresses(packageId, dmzAddresses);
                if (checkResults.Count > 0)
                {
                    res.ErrorCodes.AddRange(checkResults);
                    return res;
                }
                #endregion

                #region Setup external Unallotted IPs network
                // setup external Unallotted IPs network
                if (VMSettings.ExternalNetworkEnabled)
                {
                    int maxItems = 100000000;
                    PackageIPAddress[] ips = ServerController.GetPackageIPAddresses(packageId, 0,
                                IPAddressPool.VpsExternalNetwork, "", "", "", 0, maxItems, true).Items;
                    if (ips.Length == 0) //if the Customer does not have IP - addresses
                    {
                        // assign selected IP addresses to package
                        ServerController.AllocatePackageIPAddresses(packageId, externalAddresses);

                        // re-read package IPs
                        List<PackageIPAddress> packageIPs = ServerController.GetPackageUnassignedIPAddresses(
                                        packageId, IPAddressPool.VpsExternalNetwork);
                        // get new externalAddresses IDs (Yep, very strange WSP/SolidCP logic)
                        for (int i = 0; i < externalAddresses.Length; i++)
                        {
                            externalAddresses[i] = packageIPs[i].PackageAddressID;
                        }
                    }
                }
                #endregion

                #region Context variables
                // service ID
                int serviceId = VirtualizationHelper.GetServiceId(packageId);

                if (!createMetaItem && serviceId != VMSettings.ServiceId)// VPS reinstall --> VM was moved
                {
                    PackageController.MovePackageItem(VMSettings.Id, serviceId, true);
                }

                // load service settings
                StringDictionary settings = ServerController.GetServiceSettings(serviceId);
                #endregion

                #region Maintenance Mode Check
                if (VirtualizationHelper.IsMaintenanceMode(settings))
                {
                    res.ErrorCodes.Add(VirtualizationErrorCodes.MAINTENANCE_MODE_IS_ENABLE);
                    return res;
                }
                #endregion

                #region Check host name

                if (string.IsNullOrEmpty(VMSettings.Name))
                {
                    string hostnamePattern = settings["HostnamePattern"];
                    if (hostnamePattern.IndexOf("[") == -1) //If we do not find a pattern, replace the string with the default value
                    {
                        hostnamePattern = "[netbiosname].localhost.local";
                    }
                    VMSettings.Name = VirtualizationUtils.EvaluateSpaceVariables(hostnamePattern, packageId);
                }

                try //TODO: Change this check. It works only in one Package. Just use => packageId = 1?
                {
                    ServiceProviderItem item = PackageController.GetPackageItemByName(packageId, VMSettings.Name,
                                                                                      typeof(VirtualMachine));
                    if (item != null)
                    {
                        if (item.Id != VMSettings.Id)
                        {
                            res.ErrorCodes.Add(VirtualizationErrorCodes.HOST_NAMER_IS_ALREADY_USED);
                            return res;
                        }
                    }
                }
                catch (Exception ex)
                {
                    res.AddError(VirtualizationErrorCodes.CANNOT_CHECK_HOST_EXISTS, ex);
                    return res;
                }
                #endregion

                #region Create meta item
                // create meta item
                vm = VMSettings; //new VirtualMachine();

                //vm.Name = VMSettings.Name;
                vm.AdministratorPassword = CryptoUtils.Encrypt(password);
                vm.PackageId = packageId;
                vm.VirtualMachineId = null; // from service
                vm.ServiceId = serviceId;
                vm.Version = VirtualizationUtils.GetHyperVConfigurationVersionFromSettings(settings); //string.IsNullOrEmpty(settings["HyperVConfigurationVersion"]) ? "0.0" : settings["HyperVConfigurationVersion"];

                vm.CurrentTaskId = Guid.NewGuid().ToString("N"); // generate creation task id
                vm.ProvisioningStatus = VirtualMachineProvisioningStatus.InProgress;


                // dynamic memory
                if (VMSettings.DynamicMemory != null && VMSettings.DynamicMemory.Enabled)
                    vm.DynamicMemory = VMSettings.DynamicMemory;
                else
                    vm.DynamicMemory = null;

                // networking
                //vm.ExternalNetworkEnabled = externalNetworkEnabled;
                //vm.PrivateNetworkEnabled = privateNetworkEnabled;
                vm.ManagementNetworkEnabled = !String.IsNullOrEmpty(manageNic.NetworkId);

                #region load OS templates
                // load OS templates
                LibraryItem osTemplate = null;

                try
                {
                    bool isTemplateExist = false;
                    LibraryItem[] osTemplates = VirtualizationHelper.GetOperatingSystemTemplates(vm.PackageId);
                    foreach (LibraryItem item in osTemplates)
                    {
                        if (String.Compare(item.Path, osTemplateFile, true) == 0)
                        {
                            osTemplate = item;
                            isTemplateExist = true;

                            // check minimal disk size
                            if (osTemplate.DiskSize > 0 && vm.HddSize[0] < osTemplate.DiskSize)
                            {
                                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.QUOTA_TEMPLATE_DISK_MINIMAL_SIZE + ":" + osTemplate.DiskSize);
                                return res;
                            }
                            if (osTemplate.Generation < 1)
                                throw new Exception("The generation of VM was not configured in the template");
                            vm.Generation = osTemplate.Generation;
                            vm.SecureBootTemplate = osTemplate.SecureBootTemplate;
                            vm.EnableSecureBoot = osTemplate.Generation == 1 ? false : osTemplate.EnableSecureBoot;
                            vm.OperatingSystemTemplate = osTemplate.Name;
                            vm.LegacyNetworkAdapter = osTemplate.LegacyNetworkAdapter;
                            vm.RemoteDesktopEnabled = osTemplate.RemoteDesktop;
                            break;
                        }
                    }
                    if (!isTemplateExist)
                    { //give a description of the error for Third party services if they use SOAP
                        throw new Exception("The template " + osTemplateFile + " was not found in the HyperV Service Template Library.");
                    }
                }
                catch (Exception ex)
                {
                    res.AddError(VirtualizationErrorCodes.GET_OS_TEMPLATES_ERROR, ex);
                    return res;
                }
                #endregion

                #region setup VM paths
                // setup VM paths
                vm.RootFolderPath = VirtualizationUtils.GetCorrectVmRootFolderPath(settings, vm);

                string templatesPath = settings["OsTemplatesPath"];
                var correctVhdPath = VirtualizationUtils.GetCorrectTemplateFilePath(templatesPath, osTemplateFile);
                vm.OperatingSystemTemplatePath = correctVhdPath;
                vm.VirtualHardDrivePath = VirtualizationUtils.GetCorrectVmVirtualHardDrivePaths(VMSettings.HddSize.Length, vm.Name, vm.RootFolderPath, Path.GetExtension(correctVhdPath));

                #endregion

                // check RAM limit
                ulong ramReserve = string.IsNullOrEmpty(settings["RamReserve"]) ? 0 : UInt64.Parse(settings["RamReserve"]);
                if (ramReserve > 0 && createMetaItem) //0 - no RAM reserve. if createMetaItem = false - reinstallation, disable check.
                {
                    try
                    {
                        Server.Memory memory = VirtualizationServerController2012.GetMemoryPackageId(packageId);

                        long freePhysicalMemoryMB = (long)(memory.FreePhysicalMemoryKB / 1024);
                        long futureFreeMemoryMB = freePhysicalMemoryMB - (long)vm.RamSize; //futureFreeMemoryMB can be negative
                        bool isEnoughRAM = (futureFreeMemoryMB >= (long)ramReserve);

                        if (!isEnoughRAM)
                        {
                            throw new Exception("Not enough Memory on the Node! Reserved: " + ramReserve.ToString() + " Available: " + freePhysicalMemoryMB.ToString());
                        }

                    }
                    catch (Exception ex)
                    {
                        res.AddError(VirtualizationErrorCodes.RAM_VM_RAM_RESERVE_ERROR, ex);
                        return res;
                    }
                }

                // check hdd file
                try
                {
                    VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);
                    foreach (var virtualHardDrivePath in vm.VirtualHardDrivePath)
                    {
                        if (vs.FileExists(virtualHardDrivePath))
                            throw new Exception(virtualHardDrivePath + " is already present in the system");
                    }
                }
                catch (Exception ex)
                {
                    res.AddError(VirtualizationErrorCodes.HDD_VM_FILE_EXIST_ERROR, ex);
                    return res;
                }

                // save/update meta-item
                try
                {
                    if (createMetaItem)
                        vm.Id = PackageController.AddPackageItem(vm);
                    else
                        PackageController.UpdatePackageItem(vm);
                }
                catch (Exception ex)
                {
                    res.AddError(VirtualizationErrorCodes.CREATE_META_ITEM_ERROR, ex);
                    return res;
                }

                #endregion

                #region Start Asynchronous task
                try
                {
                    // asynchronous process starts here
                    CreateServerAsyncWorker2012 worker = new CreateServerAsyncWorker2012();

                    worker.TaskId = vm.CurrentTaskId; // async task ID
                    worker.ThreadUserId = SecurityContext.User.UserId;
                    worker.Item = vm;
                    worker.OsTemplate = osTemplate;

                    worker.ExternalAddressesNumber = externalAddressesNumber;
                    worker.RandomExternalAddresses = randomExternalAddresses;
                    worker.ExternalAddresses = externalAddresses;

                    worker.PrivateAddressesNumber = privateAddressesNumber;
                    worker.RandomPrivateAddresses = randomPrivateAddresses;
                    worker.PrivateAddresses = privateAddresses;

                    worker.DmzAddressesNumber = dmzAddressesNumber;
                    worker.RandomDmzAddresses = randomDmzAddresses;
                    worker.DmzAddresses = dmzAddresses;

                    worker.SummaryLetterEmail = summaryLetterEmail;

                    worker.CreateAsync();
                }
                catch (Exception ex)
                {
                    // delete meta item
                    PackageController.DeletePackageItem(vm.Id);

                    // return from method
                    res.AddError(VirtualizationErrorCodes.CREATE_TASK_START_ERROR, ex);
                    return res;
                }
                #endregion
            }
            catch (Exception ex)
            {
                if (!createMetaItem)
                {
                    vm.CurrentTaskId = null;
                    vm.ProvisioningStatus = VirtualMachineProvisioningStatus.Error;
                    PackageController.UpdatePackageItem(vm); //to access the audit log.
                }
                res.AddError(VirtualizationErrorCodes.CREATE_ERROR, ex);
                return res;
            }
            res.Value = vm.Id;
            res.IsSuccess = true;
            return res;
        }
    }
}
