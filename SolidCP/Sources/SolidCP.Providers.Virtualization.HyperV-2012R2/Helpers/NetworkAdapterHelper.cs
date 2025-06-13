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
    public class NetworkAdapterHelper
    {
        private const int defaultvlan = 0;
        private PowerShellManager _powerShell;

        public NetworkAdapterHelper(PowerShellManager powerShellManager)
        {
            _powerShell = powerShellManager;
        }

        public VirtualMachineNetworkAdapter[] Get(PSObject vmObj)
        {
            List<VirtualMachineNetworkAdapter> adapters = new List<VirtualMachineNetworkAdapter>();

            Command cmd = new Command("Get-VMNetworkAdapter");
            if (vmObj != null) cmd.Parameters.Add("VM", vmObj);

            Command cmdvlan = new Command("Get-VMNetworkAdapterVlan");
            if (vmObj != null) cmdvlan.Parameters.Add("VM", vmObj);

            Collection<PSObject> result = _powerShell.Execute(cmd, false);//False, because all remote connection information is already contained in vmObj
            Collection<PSObject> resultvlan = _powerShell.Execute(cmdvlan, false);
            int i = 0;
            if (result != null && result.Count > 0)
            {
                foreach (PSObject psAdapter in result)
                {
                    VirtualMachineNetworkAdapter adapter = new VirtualMachineNetworkAdapter();

                    adapter.Name = psAdapter.GetString("Name");
                    try
                    {
                        adapter.IPAddresses = psAdapter.GetProperty<string[]>("IPAddresses");
                    }
                    catch (Exception ex) { HostedSolution.HostedSolutionLog.LogError("VirtualMachineNetworkAdapter", ex); }
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
                    //We can't do that things! https://docs.microsoft.com/en-us/powershell/module/hyper-v/remove-vmnetworkadapter
                    //adapter.Name = String.Format("{0} VLAN: {1}", psAdapter.GetString("Name"), adapter.vlan.ToString());
                    i++;
                    adapters.Add(adapter);
                }
            }
            return adapters.ToArray();
        }

        public VirtualMachineNetworkAdapter Get(PSObject vmObj, string macAddress)
        {
            var adapters = Get(vmObj);
            return adapters.FirstOrDefault(a => a.MacAddress == macAddress);
        }

        public void Update(VirtualMachine vm, PSObject vmObj)
        {
            // External NIC
            if (!vm.ExternalNetworkEnabled && !String.IsNullOrEmpty(vm.ExternalNicMacAddress))
            {
                Delete(vmObj, vm.ExternalNicMacAddress);
                vm.ExternalNicMacAddress = null; // reset MAC
            }
            else if (vm.ExternalNetworkEnabled && !String.IsNullOrEmpty(vm.ExternalNicMacAddress)
                && Get(vmObj, vm.ExternalNicMacAddress) == null)
            {
                Add(vmObj, vm.ExternalSwitchId, vm.ExternalNicMacAddress, Constants.EXTERNAL_NETWORK_ADAPTER_NAME, vm.LegacyNetworkAdapter);
                try
                {
                    SetVLAN(vmObj, Constants.EXTERNAL_NETWORK_ADAPTER_NAME, vm.defaultaccessvlan);
                }
                catch (Exception ex)
                {
                    HostedSolution.HostedSolutionLog.LogError("NetworkAdapterHelperSetVLAN", ex);
                }
            }

            // Private NIC
            if (!vm.PrivateNetworkEnabled && !String.IsNullOrEmpty(vm.PrivateNicMacAddress))
            {
                Delete(vmObj, vm.PrivateNicMacAddress);
                vm.PrivateNicMacAddress = null; // reset MAC
            }
            else if (vm.PrivateNetworkEnabled && !String.IsNullOrEmpty(vm.PrivateNicMacAddress)
                 && Get(vmObj, vm.PrivateNicMacAddress) == null)
            {
                Add(vmObj, vm.PrivateSwitchId, vm.PrivateNicMacAddress, Constants.PRIVATE_NETWORK_ADAPTER_NAME, vm.LegacyNetworkAdapter);
                try
                {
                    SetVLAN(vmObj, Constants.PRIVATE_NETWORK_ADAPTER_NAME, vm.PrivateNetworkVlan);
                }
                catch (Exception ex)
                {
                    HostedSolution.HostedSolutionLog.LogError("NetworkAdapterHelperSetVLAN", ex);
                }
            }

            // DMZ NIC
            if (!vm.DmzNetworkEnabled && !String.IsNullOrEmpty(vm.DmzNicMacAddress))
            {
                Delete(vmObj, vm.DmzNicMacAddress);
                vm.DmzNicMacAddress = null; // reset MAC
            }
            else if (vm.DmzNetworkEnabled && !String.IsNullOrEmpty(vm.DmzNicMacAddress)
                 && Get(vmObj, vm.DmzNicMacAddress) == null)
            {
                Add(vmObj, vm.DmzSwitchId, vm.DmzNicMacAddress, Constants.DMZ_NETWORK_ADAPTER_NAME, vm.LegacyNetworkAdapter);
                try
                {
                    SetVLAN(vmObj, Constants.DMZ_NETWORK_ADAPTER_NAME, vm.DmzNetworkVlan);
                }
                catch (Exception ex)
                {
                    HostedSolution.HostedSolutionLog.LogError("NetworkAdapterHelperSetVLAN", ex);
                }
            }
        }

        public void Add(PSObject vmObj, string switchId, string macAddress, string adapterName, bool legacyAdapter)
        {
            Command cmd = new Command("Add-VMNetworkAdapter");

            cmd.Parameters.Add("VM", vmObj);
            cmd.Parameters.Add("Name", adapterName);
            cmd.Parameters.Add("SwitchName", switchId);

            if (String.IsNullOrEmpty(macAddress))
                cmd.Parameters.Add("DynamicMacAddress");
            else
                cmd.Parameters.Add("StaticMacAddress", macAddress);

            _powerShell.Execute(cmd, false, true); //False, because all remote connection information is already contained in vmObj

        }

        public void SetVLAN(PSObject vmObj, string adapterName, int vlan)
        {
            if (vlan >= 1)
            {
                Command cmd = new Command("Set-VMNetworkAdapterVlan");

                cmd.Parameters.Add("VM", vmObj);
                cmd.Parameters.Add("VMNetworkAdapterName", adapterName);
                cmd.Parameters.Add("Access");
                cmd.Parameters.Add("VlanId", vlan.ToString());

                _powerShell.Execute(cmd, false, true); //False, because all remote connection information is already contained in vmObj
            }
        }

        public void Delete(PSObject vmObj, string macAddress)
        {
            var networkAdapter = Get(vmObj, macAddress);

            if (networkAdapter == null)
                return;

            Delete(vmObj, networkAdapter);
        }

        public void Delete(PSObject vmObj, VirtualMachineNetworkAdapter networkAdapter)
        {
            Command cmd = new Command("Remove-VMNetworkAdapter");

            cmd.Parameters.Add("VM", vmObj);
            cmd.Parameters.Add("Name", networkAdapter.Name);

            _powerShell.Execute(cmd, false, true); //False, because all remote connection information is already contained in vmObj
        }
    }
}
