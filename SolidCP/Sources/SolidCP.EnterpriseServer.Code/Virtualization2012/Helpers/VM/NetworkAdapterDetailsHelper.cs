﻿using SolidCP.Providers.Virtualization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.VM
{
    public static class NetworkAdapterDetailsHelper
    {

        public static NetworkAdapterDetails GetExternalNetworkDetails(int packageId)
        {
            // load service
            int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.VPS2012);

            return GetExternalNetworkDetailsInternal(serviceId);
        }

        public static NetworkAdapterDetails GetExternalNetworkAdapterDetails(int itemId)
        {
            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
                return null;

            // get default NIC
            NetworkAdapterDetails nic = GetExternalNetworkDetailsInternal(vm.ServiceId);

            // update NIC
            nic.MacAddress = NetworkHelper.GetSymbolDelimitedMacAddress(vm.ExternalNicMacAddress, "-");
            nic.VLAN = vm.defaultaccessvlan;

            // load IP addresses
            nic.IPAddresses = ObjectUtils.CreateListFromDataReader<NetworkAdapterIPAddress>(
                DataProvider.GetItemIPAddresses(SecurityContext.User.UserId, itemId, (int)IPAddressPool.VpsExternalNetwork)).ToArray();

            // update subnet CIDR
            foreach (NetworkAdapterIPAddress ip in nic.IPAddresses)
                ip.SubnetMaskCidr = NetworkHelper.GetSubnetMaskCidr(ip.SubnetMask);

            if (nic.IPAddresses.Length > 0)
            {
                // from primary address
                nic.SubnetMask = nic.IPAddresses[0].SubnetMask;
                nic.SubnetMaskCidr = NetworkHelper.GetSubnetMaskCidr(nic.SubnetMask);
                nic.DefaultGateway = nic.IPAddresses[0].DefaultGateway;
            }

            return nic;
        }

        public static NetworkAdapterDetails GetManagementNetworkDetails(int packageId)
        {
            // load service
            int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.VPS2012);
            return GetManagementNetworkDetailsInternal(serviceId);
        }

        private static NetworkAdapterDetails GetExternalNetworkDetailsInternal(int serviceId)
        {
            // load service settings
            StringDictionary settings = ServerController.GetServiceSettings(serviceId);

            // create NIC object
            NetworkAdapterDetails nic = new NetworkAdapterDetails();
            nic.NetworkId = settings["ExternalNetworkId"];
            nic.PreferredNameServer = settings["ExternalPreferredNameServer"];
            nic.AlternateNameServer = settings["ExternalAlternateNameServer"];
            return nic;
        }

        public static NetworkAdapterDetails GetManagementNetworkAdapterDetails(int itemId)
        {
            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
                return null;

            // get default NIC
            NetworkAdapterDetails nic = GetManagementNetworkDetailsInternal(vm.ServiceId);

            // update NIC
            nic.MacAddress = NetworkHelper.GetSymbolDelimitedMacAddress(vm.ManagementNicMacAddress, "-");

            // load IP addresses
            nic.IPAddresses = ObjectUtils.CreateListFromDataReader<NetworkAdapterIPAddress>(
                DataProvider.GetItemIPAddresses(SecurityContext.User.UserId, itemId, (int)IPAddressPool.VpsManagementNetwork)).ToArray();

            // update subnet CIDR
            foreach (NetworkAdapterIPAddress ip in nic.IPAddresses)
                ip.SubnetMaskCidr = NetworkHelper.GetSubnetMaskCidr(ip.SubnetMask);

            if (nic.IPAddresses.Length > 0)
            {
                // from primary address
                nic.SubnetMask = nic.IPAddresses[0].SubnetMask;
                nic.SubnetMaskCidr = NetworkHelper.GetSubnetMaskCidr(nic.SubnetMask);
                nic.DefaultGateway = nic.IPAddresses[0].DefaultGateway;
            }

            return nic;
        }

        private static NetworkAdapterDetails GetManagementNetworkDetailsInternal(int serviceId)
        {
            // load service settings
            StringDictionary settings = ServerController.GetServiceSettings(serviceId);

            // create NIC object
            NetworkAdapterDetails nic = new NetworkAdapterDetails();
            nic.NetworkId = settings["ManagementNetworkId"];
            nic.IsDHCP = (String.Compare(settings["ManagementNicConfig"], "DHCP", true) == 0);

            if (!nic.IsDHCP)
            {
                nic.PreferredNameServer = settings["ManagementPreferredNameServer"];
                nic.AlternateNameServer = settings["ManagementAlternateNameServer"];
            }
            return nic;
        }

        #region PrivateNetworkDetails
        public static NetworkAdapterDetails GetPrivateNetworkDetails(int packageId)
        {
            // load service
            int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.VPS2012);

            return GetPrivateNetworkDetailsInternal(serviceId);
        }

        public static NetworkAdapterDetails GetPrivateNetworkAdapterDetails(int itemId)
        {
            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
                return null;

            // load default internal adapter
            NetworkAdapterDetails nic = GetPrivateNetworkDetailsInternal(vm.ServiceId);

            // update NIC
            nic.MacAddress = NetworkHelper.GetSymbolDelimitedMacAddress(vm.PrivateNicMacAddress, "-");
            nic.VLAN = vm.PrivateNetworkVlan;
            if (!String.IsNullOrEmpty(vm.CustomPrivateGateway)) nic.DefaultGateway = vm.CustomPrivateGateway;
            if (!String.IsNullOrEmpty(vm.CustomPrivateDNS1)) nic.PreferredNameServer = vm.CustomPrivateDNS1;
            if (!String.IsNullOrEmpty(vm.CustomPrivateDNS2)) nic.AlternateNameServer = vm.CustomPrivateDNS2;
            if (!String.IsNullOrEmpty(vm.CustomPrivateMask))
            {
                nic.SubnetMask = vm.CustomPrivateMask;
                nic.SubnetMaskCidr = NetworkHelper.GetSubnetMaskCidr(nic.SubnetMask);
            }

            // load IP addresses
            nic.IPAddresses = ObjectUtils.CreateListFromDataReader<NetworkAdapterIPAddress>(
            DataProvider.GetItemPrivateIPAddresses(SecurityContext.User.UserId, itemId)).ToArray();

            foreach (NetworkAdapterIPAddress ip in nic.IPAddresses)
            {
                ip.SubnetMask = nic.SubnetMask;
                ip.SubnetMaskCidr = nic.SubnetMaskCidr;
                ip.DefaultGateway = nic.DefaultGateway;
            }

            return nic;
        }

        public static NetworkAdapterDetails GetPrivateNetworkDetailsInternal(int serviceId)
        {
            // load service settings
            StringDictionary settings = ServerController.GetServiceSettings(serviceId);

            // create NIC object
            NetworkAdapterDetails nic = new NetworkAdapterDetails();

            string networkFormat = settings["PrivateNetworkFormat"];
            if (String.IsNullOrEmpty(networkFormat))
            {
                // custom format
                nic.NetworkFormat = settings["PrivateIPAddress"];
                var v6 = IPAddress.Parse(nic.NetworkFormat).V6;
                nic.SubnetMask = NetworkHelper.GetPrivateNetworkSubnetMask(settings["PrivateSubnetMask"], v6);
            }
            else
            {
                // standard format
                string[] formatPair = settings["PrivateNetworkFormat"].Split('/');
                nic.NetworkFormat = formatPair[0];
                var v6 = IPAddress.Parse(nic.NetworkFormat).V6;
                nic.SubnetMask = NetworkHelper.GetPrivateNetworkSubnetMask(formatPair[1], v6);
            }

            nic.SubnetMaskCidr = NetworkHelper.GetSubnetMaskCidr(nic.SubnetMask);
            nic.DefaultGateway = settings["PrivateDefaultGateway"];
            nic.PreferredNameServer = settings["PrivatePreferredNameServer"];
            nic.AlternateNameServer = settings["PrivateAlternateNameServer"];

            return nic;
        }
        #endregion
    }
}
