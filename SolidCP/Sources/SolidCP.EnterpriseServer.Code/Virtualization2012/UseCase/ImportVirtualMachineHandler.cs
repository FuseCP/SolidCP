using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers;
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
    public class ImportVirtualMachineHandler: ControllerBase
    {
        public ImportVirtualMachineHandler(ControllerBase provider) : base(provider) { }

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
                VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(serviceId);
                VirtualMachine vm = vs.GetVirtualMachineEx(vmId);

                // set VM properties
                item.Name = vm.Name;
                item.ProvisioningStatus = VirtualMachineProvisioningStatus.OK;

                item.Generation = vm.Generation;
                item.SecureBootTemplate = vm.SecureBootTemplate;
                item.EnableSecureBoot = vm.EnableSecureBoot;
                item.CpuCores = vm.CpuCores;
                item.RamSize = vm.RamSize;
                //Hyper-V usually loses CreatedDate and set it to 01/01/1601
                item.CreationTime = vm.CreatedDate < DateTime.Now.AddYears(-10) ? DateTime.Now.ToString() : vm.CreatedDate.ToString();
                item.DynamicMemory = vm.DynamicMemory;
                item.HddSize = vm.HddSize;
                item.HddMinimumIOPS = vm.HddMinimumIOPS;
                item.HddMaximumIOPS = vm.HddMaximumIOPS;
                item.VirtualHardDrivePath = vm.VirtualHardDrivePath;
                item.RootFolderPath = Path.GetDirectoryName(vm.VirtualHardDrivePath[0]);
                string msHddHyperVFolderName = "Virtual Hard Disks";
                if (item.RootFolderPath.EndsWith(msHddHyperVFolderName)) //We have to know root folder of VM, not of hdd.
                    item.RootFolderPath = item.RootFolderPath.Substring(0, item.RootFolderPath.Length - msHddHyperVFolderName.Length);
                item.SnapshotsNumber = maxSnapshots; //cntx.Quotas[Quotas.VPS2012_SNAPSHOTS_NUMBER].QuotaAllocatedValue -> this return -1 if unlimited;
                if (item.SnapshotsNumber == -1) item.SnapshotsNumber = 50;
                item.DvdDriveInstalled = IsDvdInstalled; //vm.DvdDriveInstalled;
                item.BootFromCD = IsBootFromCd; //vm.BootFromCD;
                item.NumLockEnabled = vm.NumLockEnabled;
                item.StartTurnOffAllowed = startShutdownAllowed;
                item.PauseResumeAllowed = pauseResumeAllowed;
                item.RebootAllowed = rebootAllowed;
                item.ResetAllowed = resetAllowed;
                item.ReinstallAllowed = reinstallAllowed;

                // remote desktop
                if (!String.IsNullOrEmpty(adminPassword))
                {
                    item.RemoteDesktopEnabled = true;
                    item.AdministratorPassword = CryptoUtils.Encrypt(adminPassword);
                }
                if (!String.IsNullOrEmpty(settings["GuacamoleConnectScript"]) && !String.IsNullOrEmpty(settings["GuacamoleHyperVIP"]))
                {
                    item.RemoteDesktopEnabled = true;
                }

                // set OS template
                string templatesPath = settings["OsTemplatesPath"];
                var correctVhdPath = VirtualizationUtils.GetCorrectTemplateFilePath(templatesPath, osTemplateFile);
                item.OperatingSystemTemplatePath = correctVhdPath;
                try
                {
                    LibraryItem[] osTemplates = VirtualizationHelper.GetOperatingSystemTemplatesByServiceId(serviceId);
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

                //TODO: move to separate method
                #region Check Quotas
                if (!ignoreChecks)
                {
                    // check quotas
                    List<string> quotaResults = new List<string>();
                    //PackageContext cntx = PackageController.GetPackageContext(packageId);

                    // dynamic memory
                    var newRam = item.RamSize;
                    if (item.DynamicMemory != null && item.DynamicMemory.Enabled)
                    {
                        newRam = item.DynamicMemory.Maximum;

                        if (item.RamSize > item.DynamicMemory.Maximum || item.RamSize < item.DynamicMemory.Minimum)
                            quotaResults.Add(VirtualizationErrorCodes.QUOTA_NOT_IN_DYNAMIC_RAM);
                    }

                    QuotaHelper.CheckListsQuota(cntx, quotaResults, Quotas.VPS2012_SERVERS_NUMBER, VirtualizationErrorCodes.QUOTA_EXCEEDED_SERVERS_NUMBER);

                    QuotaHelper.CheckNumericQuota(cntx, quotaResults, Quotas.VPS2012_CPU_NUMBER, item.CpuCores, VirtualizationErrorCodes.QUOTA_EXCEEDED_CPU);
                    QuotaHelper.CheckNumericQuota(cntx, quotaResults, Quotas.VPS2012_RAM, newRam, VirtualizationErrorCodes.QUOTA_EXCEEDED_RAM);
                    int totalHddSize = 0;
                    for (int i = 0; i < item.HddSize.Length; i++)
                    {
                        totalHddSize += item.HddSize[i];
                    }
                    if (item.HddSize.Length > 1)
                    {
                        QuotaHelper.CheckNumericQuota(cntx, quotaResults, Quotas.VPS2012_HDD, totalHddSize, VirtualizationErrorCodes.QUOTA_EXCEEDED_HDDS);
                        QuotaHelper.CheckNumericQuota(cntx, quotaResults, Quotas.VPS2012_ADDITIONAL_VHD_COUNT, item.HddSize.Length - 1, VirtualizationErrorCodes.QUOTA_EXCEEDED_ADDITIONAL_HDD);
                    }
                    else
                    {
                        QuotaHelper.CheckNumericQuota(cntx, quotaResults, Quotas.VPS2012_HDD, totalHddSize, VirtualizationErrorCodes.QUOTA_EXCEEDED_HDD);
                    }
                    QuotaHelper.CheckNumericQuota(cntx, quotaResults, Quotas.VPS2012_SNAPSHOTS_NUMBER, item.SnapshotsNumber, VirtualizationErrorCodes.QUOTA_EXCEEDED_SNAPSHOTS);

                    QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_DVD_ENABLED, item.DvdDriveInstalled, VirtualizationErrorCodes.QUOTA_EXCEEDED_DVD_ENABLED);
                    QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_BOOT_CD_ALLOWED, item.BootFromCD, VirtualizationErrorCodes.QUOTA_EXCEEDED_CD_ALLOWED);

                    QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_START_SHUTDOWN_ALLOWED, item.StartTurnOffAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_START_SHUTDOWN_ALLOWED);
                    QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_PAUSE_RESUME_ALLOWED, item.PauseResumeAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_PAUSE_RESUME_ALLOWED);
                    QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_REBOOT_ALLOWED, item.RebootAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_REBOOT_ALLOWED);
                    QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_RESET_ALOWED, item.ResetAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_RESET_ALOWED);
                    QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_REINSTALL_ALLOWED, item.ReinstallAllowed, VirtualizationErrorCodes.QUOTA_EXCEEDED_REINSTALL_ALLOWED);

                    QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_EXTERNAL_NETWORK_ENABLED, item.ExternalNetworkEnabled, VirtualizationErrorCodes.QUOTA_EXCEEDED_EXTERNAL_NETWORK_ENABLED);
                    QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_PRIVATE_NETWORK_ENABLED, item.PrivateNetworkEnabled, VirtualizationErrorCodes.QUOTA_EXCEEDED_PRIVATE_NETWORK_ENABLED);

                    // check acceptable values
                    if (item.RamSize < 32)
                        quotaResults.Add(VirtualizationErrorCodes.QUOTA_WRONG_RAM_HV);
                    foreach (var hddSize in item.HddSize)
                    {
                        if (hddSize <= 0)
                            quotaResults.Add(VirtualizationErrorCodes.QUOTA_WRONG_HDD);
                    }
                    if (item.SnapshotsNumber < 0)
                        quotaResults.Add(VirtualizationErrorCodes.QUOTA_WRONG_SNAPSHOTS);

                    if (quotaResults.Count > 0)
                    {
                        res.ErrorCodes.AddRange(quotaResults);
                        return res;
                    }
                }
                #endregion

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
                    for (int i = 0; i < externalAddresses.Length; i++)
                    {
                        foreach (PackageIPAddress ip in packageIPs)
                        {
                            if (ip.AddressID == externalAddresses[i])
                            {
                                // assign to the item
                                ServerController.AddItemIPAddress(itemId, ip.PackageAddressID);

                                // set primary IP address
                                if (i == 0)
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
    }
}
