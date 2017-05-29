using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SolidCP.Providers.Virtualization
{
    public static class NetworkAdapterHelper
    {
        private const int defaultvlan = 0;

        public static VirtualMachineNetworkAdapter[] Get(PowerShellManager powerShell, string vmName)
        {
            List<VirtualMachineNetworkAdapter> adapters = new List<VirtualMachineNetworkAdapter>();

            Command cmd = new Command("Get-VMNetworkAdapter");
            if (!string.IsNullOrEmpty(vmName)) cmd.Parameters.Add("VMName", vmName);

            Command cmdvlan = new Command("Get-VMNetworkAdapterVlan");
            if (!string.IsNullOrEmpty(vmName)) cmdvlan.Parameters.Add("VMName", vmName);

            Collection<PSObject> result = powerShell.Execute(cmd, true);
            Collection<PSObject> resultvlan = powerShell.Execute(cmdvlan, true);
            int i = 0;
            if (result != null && result.Count > 0)
            {
                foreach (PSObject psAdapter in result)
                {
                    VirtualMachineNetworkAdapter adapter = new VirtualMachineNetworkAdapter();

                    //adapter.Name = psAdapter.GetString("Name");
                    adapter.MacAddress = psAdapter.GetString("MacAddress");
                    adapter.SwitchName = psAdapter.GetString("SwitchName");

                    try
                    {
                        adapter.vlan = resultvlan[i].GetInt("AccessVlanId");
                    }
                    catch
                    {
                        adapter.vlan = defaultvlan;
                    }
                    if (adapter.vlan == 0)
                        adapter.vlan = defaultvlan;
                    adapter.Name = String.Format("{0} VLAN: {1}", psAdapter.GetString("Name"), adapter.vlan.ToString());
                    i++;
                    adapters.Add(adapter);
                }
            }
            return adapters.ToArray();
        }

        public static VirtualMachineNetworkAdapter Get(PowerShellManager powerShell, string vmName, string macAddress)
        {
            var adapters = Get(powerShell, vmName);
            return adapters.FirstOrDefault(a => a.MacAddress == macAddress);
        }

        public static void Update(PowerShellManager powerShell, VirtualMachine vm)
        {
            // External NIC
            if (!vm.ExternalNetworkEnabled && !String.IsNullOrEmpty(vm.ExternalNicMacAddress))
            {
                Delete(powerShell, vm.Name, vm.ExternalNicMacAddress);
                vm.ExternalNicMacAddress = null; // reset MAC
            }
            else if (vm.ExternalNetworkEnabled && !String.IsNullOrEmpty(vm.ExternalNicMacAddress)
                && Get(powerShell, vm.Name, vm.ExternalNicMacAddress) == null)
            {
                Add(powerShell, vm.Name, vm.ExternalSwitchId, vm.ExternalNicMacAddress, Constants.EXTERNAL_NETWORK_ADAPTER_NAME, vm.LegacyNetworkAdapter);
                try
                {
                    SetVLAN(powerShell, vm.Name, vm.defaultaccessvlan);
                }
                catch (Exception ex)
                {
                    HostedSolution.HostedSolutionLog.LogError("NetworkAdapterHelperSetVLAN", ex);
                }
            }

            // Private NIC
            if (!vm.PrivateNetworkEnabled && !String.IsNullOrEmpty(vm.PrivateNicMacAddress))
            {
                Delete(powerShell, vm.Name, vm.PrivateNicMacAddress);
                vm.PrivateNicMacAddress = null; // reset MAC
            }
            else if (vm.PrivateNetworkEnabled && !String.IsNullOrEmpty(vm.PrivateNicMacAddress)
                 && Get(powerShell, vm.Name, vm.PrivateNicMacAddress) == null)
            {
                Add(powerShell, vm.Name, vm.PrivateSwitchId, vm.PrivateNicMacAddress, Constants.PRIVATE_NETWORK_ADAPTER_NAME, vm.LegacyNetworkAdapter);
            }
        }

        public static void Add(PowerShellManager powerShell, string vmName, string switchId, string macAddress, string adapterName, bool legacyAdapter)
        {
            Command cmd = new Command("Add-VMNetworkAdapter");

            cmd.Parameters.Add("VMName", vmName);
            cmd.Parameters.Add("Name", adapterName);
            cmd.Parameters.Add("SwitchName", switchId);

            if (String.IsNullOrEmpty(macAddress))
                cmd.Parameters.Add("DynamicMacAddress");
            else
                cmd.Parameters.Add("StaticMacAddress", macAddress);

            powerShell.Execute(cmd, true);

        }

        public static void SetVLAN(PowerShellManager powerShell, string vmName, int vlan)
        {
            if (vlan >= 1)
            {
                Command cmd = new Command("Set-VMNetworkAdapterVlan");

                cmd.Parameters.Add("VMName", vmName);
                cmd.Parameters.Add("Access");
                cmd.Parameters.Add("VlanId", vlan.ToString());

                powerShell.Execute(cmd, true);
            }
        }

        public static void Delete(PowerShellManager powerShell, string vmName, string macAddress)
        {
            var networkAdapter = Get(powerShell, vmName, macAddress);

            if (networkAdapter == null)
                return;

            Delete(powerShell, vmName, networkAdapter);
        }

        public static void Delete(PowerShellManager powerShell, string vmName, VirtualMachineNetworkAdapter networkAdapter)
        {
            Command cmd = new Command("Remove-VMNetworkAdapter");

            cmd.Parameters.Add("VMName", vmName);
            cmd.Parameters.Add("Name", networkAdapter.Name);

            powerShell.Execute(cmd, true);
        }
    }
}
