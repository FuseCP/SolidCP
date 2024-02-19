using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.PS;
using SolidCP.Providers.Virtualization;
using SolidCP.Providers.Virtualization2012;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.VM
{
    public static class KvpExchangeHelper
    {

        public static JobResult SendComputerNameKVP(int itemId, string computerName)
        {
            // load item
            VirtualMachine vm = VirtualMachineHelper.GetVirtualMachineByItemId(itemId);
            if (vm == null)
                return null;

            // build task parameters
            Dictionary<string, string> props = new Dictionary<string, string>();

            #region Check Hostname
            //Check Hostname
            string hostname, domain;
            int dotIdx = computerName.IndexOf(".");
            if (dotIdx > -1)
            {
                hostname = computerName.Substring(0, dotIdx);
                domain = computerName.Substring(dotIdx);
            }
            else
            {
                hostname = computerName;
                domain = "";
            }

            if (hostname.Length > 15) //MAX hostname size is 15!
            {
                computerName = hostname.Substring(0, hostname.Length - (hostname.Length - 15)) + domain;
            }
            #endregion

            props["FullComputerName"] = computerName;

            // send items
            return SendKvpItems(itemId, "ChangeComputerName", props);
        }

        public static JobResult SendAdministratorPasswordKVP(int itemId, string password, bool cleanResult)
        {
            // load item
            VirtualMachine vm = VirtualMachineHelper.GetVirtualMachineByItemId(itemId);
            if (vm == null)
                return null;

            // build task parameters
            Dictionary<string, string> props = new Dictionary<string, string>();

            props["Password"] = password;

            // send items
            if (cleanResult)
                return SendKvpItemsAndCleanResult(itemId, "ChangeAdministratorPassword", props);
            else
                return SendKvpItems(itemId, "ChangeAdministratorPassword", props);
        }

        public static JobResult SendNetworkAdapterKVP(int itemId, string adapterName)
        {
            // load item
            VirtualMachine vm = VirtualMachineHelper.GetVirtualMachineByItemId(itemId);
            if (vm == null)
                return null;

            // build task parameters
            Dictionary<string, string> props = new Dictionary<string, string>();
            NetworkAdapterDetails nic = null;

            if (String.Compare(adapterName, "external", true) == 0)
            {
                // external
                nic = NetworkAdapterDetailsHelper.GetExternalNetworkAdapterDetails(itemId);
                PowerShellScript.CheckCustomPsScript(PsScriptPoint.external_network_configuration, vm);
            }
            else if (String.Compare(adapterName, "private", true) == 0)
            {
                // private
                nic = NetworkAdapterDetailsHelper.GetPrivateNetworkAdapterDetails(itemId);
                if (!String.IsNullOrEmpty(vm.CustomPrivateGateway)) nic.DefaultGateway = vm.CustomPrivateGateway;
                if (!String.IsNullOrEmpty(vm.CustomPrivateDNS1)) nic.PreferredNameServer = vm.CustomPrivateDNS1;
                if (!String.IsNullOrEmpty(vm.CustomPrivateDNS2)) nic.AlternateNameServer = vm.CustomPrivateDNS2;
                if (!String.IsNullOrEmpty(vm.CustomPrivateMask)) nic.SubnetMask = vm.CustomPrivateMask;
                PowerShellScript.CheckCustomPsScript(PsScriptPoint.private_network_configuration, vm);
            }
            else
            {
                // management
                nic = NetworkAdapterDetailsHelper.GetManagementNetworkAdapterDetails(itemId);
                PowerShellScript.CheckCustomPsScript(PsScriptPoint.management_network_configuration, vm);
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
                        {
                            if (String.Compare(adapterName, "private", true) == 0 && !String.IsNullOrEmpty(vm.CustomPrivateGateway))
                            {
                                props["DefaultIPGateway"] = vm.CustomPrivateGateway;
                            }
                            else
                            {
                                props["DefaultIPGateway"] = nic.IPAddresses[i].DefaultGateway;
                            }
                        }
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

        public static JobResult SendKvpItems(int itemId, string taskName, Dictionary<string, string> taskProps)
        {
            return SendKvpItemsInternal(itemId, taskName, taskProps, false);
        }

        public static JobResult SendKvpItemsAndCleanResult(int itemId, string taskName, Dictionary<string, string> taskProps)
        {
            return SendKvpItemsInternal(itemId, taskName, taskProps, true);
        }

        public static JobResult SendKvpItemsInternal(int itemId, string taskName, Dictionary<string, string> taskProps, bool cleanResult)
        {
            string TASK_PREFIX = "SCP-";
            string TASK_PREFIX_OLD = "WSP-"; //backward compatibility for the WSPanel and the MSPControl version 0000 < 3000 <= ????

            // load item
            VirtualMachine vm = VirtualMachineHelper.GetVirtualMachineByItemId(itemId);
            if (vm == null)
                return null;

            JobResult result = null;

            // load proxy
            VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);

            try
            {
                // delete completed task definitions
                List<string> completedTasks = new List<string>();
                KvpExchangeDataItem[] vmKvps = vs.GetKVPItems(vm.VirtualMachineId);
                foreach (KvpExchangeDataItem vmKvp in vmKvps)
                {
                    if (vmKvp.Name.StartsWith(TASK_PREFIX))
                    {
                        completedTasks.Add(vmKvp.Name);
                        TryToDelUnusedTask(ref vm, ref vs, vmKvp.Name.ToString(), TASK_PREFIX_OLD, TASK_PREFIX);
                    }
                    else if (vmKvp.Name.StartsWith(TASK_PREFIX_OLD))
                    {
                        completedTasks.Add(vmKvp.Name);
                        TryToDelUnusedTask(ref vm, ref vs, vmKvp.Name.ToString(), TASK_PREFIX, TASK_PREFIX_OLD);
                    }
                }

                // delete completed items
                vs.RemoveKVPItems(vm.VirtualMachineId, completedTasks.ToArray());
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

            //taskName = String.Format("{0}{1}-{2}", TASK_PREFIX, taskName, DateTime.Now.Ticks);
            //string taskData = String.Join("|", items.ToArray());
            string[] taskNameArr = new string[2];
            long dataNowTick = DateTime.Now.Ticks;
            taskNameArr[0] = String.Format("{0}{1}-{2}", TASK_PREFIX, taskName, dataNowTick);
            taskNameArr[1] = String.Format("{0}{1}-{2}", TASK_PREFIX_OLD, taskName, dataNowTick);
            string taskData = String.Join("|", items.ToArray());

            // create KVP item
            KvpExchangeDataItem[] kvp = new KvpExchangeDataItem[taskNameArr.Length];
            for (int i = 0; i < kvp.Length; i++)
            {
                kvp[i] = new KvpExchangeDataItem();
                kvp[i].Name = taskNameArr[i];
                kvp[i].Data = taskData;
            }
            string taskDataLog = taskData;
            if ("ChangeAdministratorPassword".Equals(taskName))
            {
                taskDataLog = "Password=******";
            }

            try
            {
                // try adding KVP items
                result = vs.AddKVPItems(vm.VirtualMachineId, kvp);
                TaskManager.Write(String.Format("Trying to add the Task"));
                if (result.Job != null && result.Job.JobState == ConcreteJobState.Exception)
                {
                    // try updating KVP items
                    TaskManager.Write(String.Format("Trying to update the task in the VPS"));
                    return vs.ModifyKVPItems(vm.VirtualMachineId, kvp);
                }
                else
                {
                    TaskManager.Write(String.Format("The task has been sent to the VPS"));
                    if (cleanResult)
                    {
                        Thread t = new Thread(() => CleanLastKVPResult(ref vm, ref vs, TASK_PREFIX, TASK_PREFIX_OLD, taskNameArr));
                        t.Start();
                        //CleanLastKVPResult(ref vm, ref vs, TASK_PREFIX, TASK_PREFIX_OLD, taskNameArr);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                // log error
                TaskManager.WriteWarning(String.Format("Error setting KVP items '{0}': {1}", taskDataLog, ex.Message));
            }

            return null;
        }

        private static void CleanLastKVPResult(ref VirtualMachine vm, ref VirtualizationServer2012 vs, string TASK_PREFIX, string TASK_PREFIX_OLD, string[] taskNameArr)
        {
            try
            {
                ushort waitSec = 60;
                for (ushort i = 0; i < waitSec; i++)
                {
                    KvpExchangeDataItem[] vmKvps = vs.GetKVPItems(vm.VirtualMachineId);
                    System.Threading.Thread.Sleep(1000);
                    foreach (KvpExchangeDataItem vmKvp in vmKvps)
                    {
                        if (vmKvp.Name.Equals(taskNameArr[0]))
                        {
                            TryToDelUnusedTask(ref vm, ref vs, vmKvp.Name.ToString(), TASK_PREFIX_OLD, TASK_PREFIX);
                            vs.RemoveKVPItems(vm.VirtualMachineId, new string[] { taskNameArr[0] });
                            i = waitSec;
                        }
                        else if (vmKvp.Name.Equals(taskNameArr[1]))
                        {
                            TryToDelUnusedTask(ref vm, ref vs, vmKvp.Name.ToString(), TASK_PREFIX, TASK_PREFIX_OLD);
                            vs.RemoveKVPItems(vm.VirtualMachineId, new string[] { taskNameArr[1] });
                            i = waitSec;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // log error
                TaskManager.WriteWarning(String.Format("Error clean last KVP item: {0}", ex.Message));
            }
        }

        private static void TryToDelUnusedTask(ref VirtualMachine vm, ref VirtualizationServer2012 vs, string taskName, string PREFIX, string REPLACE_PREFIX)
        {
            if (taskName.Substring(PREFIX.Length).Equals("CurrentTask")) //Ignore CurrentTask
            {
                return;
            }

            try
            {
                taskName = taskName.Replace(REPLACE_PREFIX, PREFIX);
                vs.RemoveKVPItems(vm.VirtualMachineId, new string[] { taskName });
            }
            catch (Exception ex)
            {
                // log error
                TaskManager.WriteWarning(String.Format("Error deleting  Unused KVP items: {0}", ex.Message));
            }
        }
    }
}
