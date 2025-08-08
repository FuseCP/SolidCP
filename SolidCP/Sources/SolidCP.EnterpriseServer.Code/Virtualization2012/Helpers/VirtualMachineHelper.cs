using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.PS;
using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.VM;
using SolidCP.Providers.Common;
using SolidCP.Providers.Virtualization;
//using SolidCP.Providers.Virtualization2012;
using SolidCP.Server.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers
{
    public class VirtualMachineHelper: ControllerBase
    {
        public VirtualMachineHelper(ControllerBase provider) : base(provider) { }

        public VirtualMachine GetVirtualMachineByItemId(int itemId)
        {
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
                return null;

            //Wait next Task.
            int attempts = 10;
            while (!String.IsNullOrEmpty(vm.CurrentTaskId)
                && TaskManager.GetTask(vm.CurrentTaskId) == null
                && attempts > 0)
            {
                Thread.Sleep(5000);

                attempts--;
                vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
                if (vm == null)
                    return null;
            }

            // host name
            int dotIdx = vm.Name.IndexOf(".");
            if (dotIdx > -1) {
                vm.Hostname = vm.Name.Substring(0, dotIdx);
                vm.Domain = vm.Name.Substring(dotIdx + 1);
            } else {
                vm.Hostname = vm.Name;
                vm.Domain = "";
            }

            if (vm.Hostname.Length > 15) //MAX hostname size is 15!
            {
                vm.Hostname = vm.Hostname.Substring(0, vm.Hostname.Length - (vm.Hostname.Length - 15));
            }

            // check if task was aborted during provisioning
            if (!String.IsNullOrEmpty(vm.CurrentTaskId)
                && TaskManager.GetTask(vm.CurrentTaskId) == null)
            {
                // set to error
                vm.CurrentTaskId = null;
                vm.ProvisioningStatus = VirtualMachineProvisioningStatus.Error;
                PackageController.UpdatePackageItem(vm);
            }

            vm.AdministratorPassword = CryptoUtils.Decrypt(vm.AdministratorPassword);
            return vm;
        }

        public VirtualMachine GetVirtualMachineGeneralDetails(int itemId)
        {
            // load meta item
            VirtualMachine machine = GetVirtualMachineByItemId(itemId);

            if (machine == null || String.IsNullOrEmpty(machine.VirtualMachineId))
                return null;

            // get proxy
            VirtualizationServer2012 vps = VirtualizationHelper.GetVirtualizationProxy(machine.ServiceId);

            // load details
            VirtualMachine vm = vps.GetVirtualMachine(machine.VirtualMachineId);

            // check if VM RootFolderPath and VirtualHardDrivePath is correct
            CheckVirtualMachinePath(machine, vm, vps);

            // add meta props
            vm.Id = machine.Id;
            vm.Name = machine.Name;
            vm.RamSize = machine.RamSize;
            vm.ServiceId = machine.ServiceId;
            vm.CreationTime = machine.CreationTime;
            vm.ExternalNicMacAddress = machine.ExternalNicMacAddress;

            return vm;
        }

        private static void CheckVirtualMachinePath(VirtualMachine vmItem, VirtualMachine realVm, VirtualizationServer2012 vps)
        {
            bool update = false;
            if (!String.IsNullOrEmpty(realVm.RootFolderPath) && !realVm.RootFolderPath.Equals(vmItem.RootFolderPath))
            {
                vmItem.RootFolderPath = realVm.RootFolderPath;
                update = true;
            }
            if (realVm.VirtualHardDrivePath != null && realVm.VirtualHardDrivePath.Length > 0)
            {
                if (!realVm.VirtualHardDrivePath.SequenceEqual(vmItem.VirtualHardDrivePath))
                {
                    // we also need to update the HddSize array to match the paths
                    VirtualMachine extVm = vps.GetVirtualMachineEx(vmItem.VirtualMachineId);
                    if (extVm.VirtualHardDrivePath != null && extVm.VirtualHardDrivePath.Length > 0 
                        && extVm.HddSize != null && extVm.VirtualHardDrivePath.Length == extVm.HddSize.Length)
                    {
                        vmItem.VirtualHardDrivePath = extVm.VirtualHardDrivePath;
                        vmItem.HddSize = extVm.HddSize;
                        update = true;
                    }
                }
            }
            if (update) PackageController.UpdatePackageItem(vmItem);
        }

        public static VirtualMachine GetVirtualMachineExtendedInfo(int serviceId, string vmId)
        {
            // get proxy
            VirtualizationServer2012 vps = VirtualizationHelper.GetVirtualizationProxy(serviceId);

            // load details
            return vps.GetVirtualMachineEx(vmId);
        }

        public List<ConcreteJob> GetVirtualMachineJobs(int itemId)
        {
            // load meta item
            VirtualMachine vm = VirtualMachineHelper.GetVirtualMachineByItemId(itemId);

            if (vm == null)
                return null;

            // get proxy
            VirtualizationServer2012 vps = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);

            // load jobs
            ConcreteJob[] jobs = vps.GetVirtualMachineJobs(vm.VirtualMachineId);
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

        public byte[] GetVirtualMachineThumbnail(int itemId, ThumbnailSize size)
        {
            // load meta item
            VirtualMachine vm = GetVirtualMachineByItemId(itemId);

            if (vm == null)
                return null;

            // get proxy
            VirtualizationServer2012 vps = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);

            // return thumbnail
            return vps.GetVirtualMachineThumbnailImage(vm.VirtualMachineId, size);
        }

        public string EvaluateVirtualMachineTemplate(int itemId, bool emailMode, bool creation, string template)
        {
            Hashtable items = new Hashtable();

            // load machine details
            VirtualMachine vm = GetVirtualMachineByItemId(itemId);
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
            items["external_nic"] = NetworkAdapterDetailsHelper.GetExternalNetworkAdapterDetails(itemId);

            // load private NIC
            items["private_nic"] = NetworkAdapterDetailsHelper.GetPrivateNetworkAdapterDetails(itemId);

            // load private NIC
            items["management_nic"] = NetworkAdapterDetailsHelper.GetManagementNetworkAdapterDetails(itemId);

            // load template item
            LibraryItem osTemplate = null;
            try
            {
                LibraryItem[] osTemplates = VirtualizationHelper.GetOperatingSystemTemplates(vm.PackageId);
                foreach (LibraryItem item in osTemplates)
                    if (string.Compare(item.Path, Path.GetFileName(vm.OperatingSystemTemplatePath), true) == 0)
                    {
                        osTemplate = item;
                        break;
                    }
            }
            catch { }
            items["os_template"] = osTemplate;

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

    }
}
