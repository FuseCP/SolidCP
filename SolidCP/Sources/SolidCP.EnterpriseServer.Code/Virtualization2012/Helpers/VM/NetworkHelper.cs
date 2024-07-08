using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.PS;
using SolidCP.Providers.Virtualization;
//using SolidCP.Providers.Virtualization2012;
using SolidCP.Server.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.VM
{
    public class NetworkHelper: ControllerBase
    {
        private const string MS_MAC_PREFIX = "00155D"; // IEEE prefix of MS MAC addresses

        public NetworkHelper(ControllerBase provider) : base(provider) { }

        public JobResult InjectIPadresses(int itemId, string adapterName)
        {
            // load item
            VirtualMachine vm = VirtualMachineHelper.GetVirtualMachineByItemId(itemId);
            if (vm == null)
                return null;

            // get proxy
            VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);

            GuestNetworkAdapterConfiguration guestNetworkAdapterConfiguration = new GuestNetworkAdapterConfiguration();
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
                guestNetworkAdapterConfiguration.MAC = nic.MacAddress.Replace(" ", "").Replace(":", "").Replace("-", "");
                guestNetworkAdapterConfiguration.DHCPEnabled = nic.IsDHCP;
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
                                guestNetworkAdapterConfiguration.DefaultGateways = new string[] { vm.CustomPrivateGateway };
                            }
                            else
                            {
                                guestNetworkAdapterConfiguration.DefaultGateways = new string[] { nic.IPAddresses[i].DefaultGateway };
                            }
                        }
                    }

                    guestNetworkAdapterConfiguration.IPAddresses = ips;
                    guestNetworkAdapterConfiguration.Subnets = subnetMasks;

                    // name servers
                    if (!String.IsNullOrEmpty(nic.AlternateNameServer))
                        guestNetworkAdapterConfiguration.DNSServers = new string[] { nic.PreferredNameServer, nic.AlternateNameServer };
                    else
                        guestNetworkAdapterConfiguration.DNSServers = new string[] { nic.PreferredNameServer };
                }
            }

            // DNS
            if (guestNetworkAdapterConfiguration.DNSServers.Length == 0)
            {
                guestNetworkAdapterConfiguration.DNSServers = new string[] { "0.0.0.0" }; // obtain automatically
            }

            return vs.InjectIPs(vm.VirtualMachineId, guestNetworkAdapterConfiguration);
        }

        public string GetSymbolDelimitedMacAddress(string mac, string delimiter)
        {
            if (String.IsNullOrEmpty(mac))
                return mac;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 6; i++)
            {
                sb.Append(mac[i * 2]).Append(mac[i * 2 + 1]);
                if (i < 5) sb.Append(delimiter);
            }
            return sb.ToString();
        }

        public string GetSubnetMaskCidr(string subnetMask)
        {
            if (String.IsNullOrEmpty(subnetMask))
                return subnetMask;
            var ip = IPAddress.Parse(subnetMask);
            if (ip.V4)
            {
                int cidr = 32;
                var mask = ip.Address;
                while ((mask & 1) == 0 && cidr > 0)
                {
                    mask >>= 1;
                    cidr -= 1;
                }
                return cidr.ToString();
            }
            else
            {
                return ip.Cidr.ToString();
            }
        }

        public string GetPrivateNetworkSubnetMask(string cidr, bool v6)
        {
            if (v6)
            {
                return "/" + cidr;
            }
            else
            {
                return IPAddress.Parse("/" + cidr).ToV4MaskString();
            }
        }
        public string GenerateMacAddress()
        {
            return MS_MAC_PREFIX + Utils.GetRandomHexString(3);
        }
    }
}
