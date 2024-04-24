using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.VM;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.Virtualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012.Tasks
{
    public class ReinstallVirtualMachineTask: ControllerBase
    {
        public ReinstallVirtualMachineTask(ControllerBase provider) : base(provider) { }

        internal void ReinstallVirtualMachineNewTask(int itemId, VirtualMachine VMSettings, LibraryItem OsTemplate, string adminPassword, string[] privIps,
            bool saveVirtualDisk, bool exportVps, string exportPath)
        {
            string taskId = VMSettings.CurrentTaskId;
            // start task
            int maximumExecutionSeconds = 60 * 60 * 2; //2 hours for this task. Anyway the Powershell cmd vhd convert has max 1 hour limit.
            TaskManager.StartTask(taskId, "VPS2012", "REINSTALL", VMSettings.Name, VMSettings.Id, VMSettings.PackageId, maximumExecutionSeconds);

            ReinstallVirtualMachineInternal(taskId, itemId, VMSettings, OsTemplate, adminPassword, privIps, saveVirtualDisk, exportVps, exportPath);

            TaskManager.CompleteTask();
        }

        private void ReinstallVirtualMachineInternal(string taskId, int itemId, VirtualMachine VMSettings, LibraryItem OsTemplate, string adminPassword, string[] privIps,
            bool saveVirtualDisk, bool exportVps, string exportPath)
        {
            //string taskId = VMSettings.CurrentTaskId;
            //// start task
            //int maximumExecutionSeconds = 60 * 30; //30 min for this task.
            //TaskManager.StartTask(taskId, "VPS2012", "REINSTALL", VMSettings.Name, VMSettings.Id, VMSettings.PackageId, maximumExecutionSeconds);

            IntResult result = new IntResult();
            string osTemplateFile = VMSettings.OperatingSystemTemplate;
            TaskManager.Write(String.Format("VPS Operating System Template {0}", osTemplateFile));

            #region Setup IPs
            List<int> extIps = new List<int>();
            byte externalAddressesNumber = 0;
            if (VMSettings.ExternalNetworkEnabled)
            {
                List<int> ipAddressesID = new List<int>();
                externalAddressesNumber = 1;
                NetworkAdapterDetails nic = NetworkAdapterDetailsHelper.GetExternalNetworkAdapterDetails(itemId);
                if (nic.IPAddresses != null && nic.IPAddresses.GetLength(0) > 0)
                {
                    foreach (NetworkAdapterIPAddress ip in nic.IPAddresses)
                    {
                        ipAddressesID.Add(ip.AddressId);
                    }
                }
                extIps = ipAddressesID;
                //TODO: not needed at the moment, thanks a bug for this :)
                //List<PackageIPAddress> uips = ServerController.GetItemIPAddresses(itemId, IPAddressPool.VpsExternalNetwork);
                //foreach (PackageIPAddress uip in uips)
                //    foreach (int ip in ipAddressesID)
                //        if (ip == uip.AddressID)
                //        {
                //            TaskManager.Write(String.Format("PackageAddressID {0}", uip.AddressID));                                                        
                //            extIps.Add(uip.AddressID); //PIP.PackageAddressID AS AddressID (install_db.sql line 22790), really? It looks like a bug... 
                //                                       //but ok, just for furture if someone fix it, here too need change to uip.PackageAddressID
                //            break;
                //        }
            }

            byte privateAddressesNumber = 0;
            if (VMSettings.PrivateNetworkEnabled && (privIps != null && privIps.Length > 0))
                privateAddressesNumber = 1;
            List<int> ipLanAddressesID = new List<int>();
            NetworkAdapterDetails nicLan = NetworkAdapterDetailsHelper.GetPrivateNetworkAdapterDetails(itemId);
            if (nicLan.IPAddresses != null && nicLan.IPAddresses.GetLength(0) > 0)
            {
                int i = 0;
                foreach (NetworkAdapterIPAddress ip in nicLan.IPAddresses)
                {
                    ipLanAddressesID.Add(ip.AddressId);
                    privIps[i] = ip.IPAddress;
                    i++;
                }
            }
            #endregion
            
            bool keepMetaItem = true;
            //ResultObject res = DeleteVirtualMachineAsynchronous(itemId, saveVirtualDisk, exportVps, exportPath, keepMetaItem);
            DeleteVirtualMachineTask.DeleteVirtualMachineContinueTask(taskId, itemId, VMSettings, saveVirtualDisk, exportVps, exportPath, keepMetaItem);

            BackgroundTask currentTask = TaskManager.GetTask(taskId);
            if (TaskManager.HasErrors(currentTask)) {
                return;
            }

            //if (res.IsSuccess)
            if (true)
            {
                int timeOut = 240;
                while (((VirtualMachine)PackageController.GetPackageItem(itemId)).ProvisioningStatus != VirtualMachineProvisioningStatus.Deleted && timeOut > 0)
                {
                    TaskManager.IndicatorCurrent += 10;
                    System.Threading.Thread.Sleep(1000);
                    timeOut--;
                }
                if (timeOut > 0)
                {

                    if (keepMetaItem) //Clean assigned IPs from VM item
                    {
                        IpAddressExternalHelper.DeleteVirtualMachineExternalIPAddresses(itemId, extIps.ToArray(), false);
                        IpAddressPrivateHelper.DeleteVirtualMachinePrivateIPAddresses(itemId, ipLanAddressesID.ToArray(), false);
                    }
                    TaskManager.Write(String.Format("The old VPS was deleted."));
                    System.Threading.Thread.Sleep(1000); //give a little time to delete, just for sure.       

                    

                    VMSettings.AdministratorPassword = CryptoUtils.Encrypt(adminPassword);
                    //result = CreateNewVirtualMachineInternal(VMSettings, osTemplateFile, adminPassword, null,
                    //    externalAddressesNumber, false, extIps.ToArray(), privateAddressesNumber, false, privIps, false);
                    TaskManager.Write(String.Format("Begin to create a new VPS"));
                    CreateVirtualMachineTask.CreateVirtualMachineContinueTask(taskId, VMSettings, OsTemplate, externalAddressesNumber, false, extIps.ToArray(), privateAddressesNumber, false, privIps, null);
                }
            }
            //TaskManager.CompleteTask();
        }
    }
}
