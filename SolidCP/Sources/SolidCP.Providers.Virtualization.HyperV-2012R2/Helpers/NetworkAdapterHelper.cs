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

        public VirtualMachineNetworkAdapter[] Get(VirtualMachineData vmData)
        {
            List<VirtualMachineNetworkAdapter> adapters = new List<VirtualMachineNetworkAdapter>();

            Command cmd = new Command("Get-VMNetworkAdapter");
            Command cmdvlan = new Command("Get-VMNetworkAdapterVlan");

            Collection<PSObject> result = _powerShell.ExecuteOnVm(cmd, vmData);
            Collection<PSObject> resultvlan = _powerShell.ExecuteOnVm(cmdvlan, vmData);
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

        public VirtualMachineNetworkAdapter Get(VirtualMachineData vmData, string macAddress)
        {
            var adapters = Get(vmData);
            return adapters.FirstOrDefault(a => a.MacAddress == macAddress);
        }

        public void Update(VirtualMachineData realVmData, VirtualMachine vmSettings)
        {
            // External NIC
            if (!vmSettings.ExternalNetworkEnabled && !String.IsNullOrEmpty(vmSettings.ExternalNicMacAddress))
            {
                Delete(realVmData, vmSettings.ExternalNicMacAddress);
                vmSettings.ExternalNicMacAddress = null; // reset MAC
            }
            else if (vmSettings.ExternalNetworkEnabled && !String.IsNullOrEmpty(vmSettings.ExternalNicMacAddress)
                && Get(realVmData, vmSettings.ExternalNicMacAddress) == null)
            {
                Add(realVmData, vmSettings.ExternalSwitchId, vmSettings.ExternalNicMacAddress, Constants.EXTERNAL_NETWORK_ADAPTER_NAME, vmSettings.LegacyNetworkAdapter);
                try
                {
                    SetVLAN(realVmData, Constants.EXTERNAL_NETWORK_ADAPTER_NAME, vmSettings.defaultaccessvlan);
                }
                catch (Exception ex)
                {
                    HostedSolution.HostedSolutionLog.LogError("NetworkAdapterHelperSetVLAN", ex);
                }
            }

            // Private NIC
            if (!vmSettings.PrivateNetworkEnabled && !String.IsNullOrEmpty(vmSettings.PrivateNicMacAddress))
            {
                Delete(realVmData, vmSettings.PrivateNicMacAddress);
                vmSettings.PrivateNicMacAddress = null; // reset MAC
            }
            else if (vmSettings.PrivateNetworkEnabled && !String.IsNullOrEmpty(vmSettings.PrivateNicMacAddress)
                 && Get(realVmData, vmSettings.PrivateNicMacAddress) == null)
            {
                Add(realVmData, vmSettings.PrivateSwitchId, vmSettings.PrivateNicMacAddress, Constants.PRIVATE_NETWORK_ADAPTER_NAME, vmSettings.LegacyNetworkAdapter);
                try
                {
                    SetVLAN(realVmData, Constants.PRIVATE_NETWORK_ADAPTER_NAME, vmSettings.PrivateNetworkVlan);
                }
                catch (Exception ex)
                {
                    HostedSolution.HostedSolutionLog.LogError("NetworkAdapterHelperSetVLAN", ex);
                }
            }

            // DMZ NIC
            if (!vmSettings.DmzNetworkEnabled && !String.IsNullOrEmpty(vmSettings.DmzNicMacAddress))
            {
                Delete(realVmData, vmSettings.DmzNicMacAddress);
                vmSettings.DmzNicMacAddress = null; // reset MAC
            }
            else if (vmSettings.DmzNetworkEnabled && !String.IsNullOrEmpty(vmSettings.DmzNicMacAddress)
                 && Get(realVmData, vmSettings.DmzNicMacAddress) == null)
            {
                Add(realVmData, vmSettings.DmzSwitchId, vmSettings.DmzNicMacAddress, Constants.DMZ_NETWORK_ADAPTER_NAME, vmSettings.LegacyNetworkAdapter);
                try
                {
                    SetVLAN(realVmData, Constants.DMZ_NETWORK_ADAPTER_NAME, vmSettings.DmzNetworkVlan);
                }
                catch (Exception ex)
                {
                    HostedSolution.HostedSolutionLog.LogError("NetworkAdapterHelperSetVLAN", ex);
                }
            }
        }

        public void Add(VirtualMachineData vmData, string switchId, string macAddress, string adapterName, bool legacyAdapter)
        {
            Command cmd = new Command("Add-VMNetworkAdapter");

            cmd.Parameters.Add("Name", adapterName);
            cmd.Parameters.Add("SwitchName", switchId);

            if (String.IsNullOrEmpty(macAddress))
                cmd.Parameters.Add("DynamicMacAddress");
            else
                cmd.Parameters.Add("StaticMacAddress", macAddress);

            _powerShell.ExecuteOnVm(cmd, vmData, true);

        }

        public void SetVLAN(VirtualMachineData vmData, string adapterName, int vlan)
        {
            if (vlan >= 1)
            {
                Command cmd = new Command("Set-VMNetworkAdapterVlan");

                cmd.Parameters.Add("VMNetworkAdapterName", adapterName);
                cmd.Parameters.Add("Access");
                cmd.Parameters.Add("VlanId", vlan.ToString());

                _powerShell.ExecuteOnVm(cmd, vmData, true);
            }
        }

        public void Delete(VirtualMachineData vmData, string macAddress)
        {
            var networkAdapter = Get(vmData, macAddress);

            if (networkAdapter == null)
                return;

            Delete(vmData, networkAdapter);
        }

        public void Delete(VirtualMachineData vmData, VirtualMachineNetworkAdapter networkAdapter)
        {
            Command cmd = new Command("Remove-VMNetworkAdapter");

            cmd.Parameters.Add("Name", networkAdapter.Name);

            _powerShell.ExecuteOnVm(cmd, vmData, true);
        }
    }
}
