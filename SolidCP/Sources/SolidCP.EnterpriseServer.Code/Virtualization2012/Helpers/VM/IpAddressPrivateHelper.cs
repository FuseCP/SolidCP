using SolidCP.Providers.Common;
using SolidCP.Providers.Virtualization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidCP.EnterpriseServer.Data;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.VM
{
    public class IpAddressPrivateHelper: ControllerBase
    {
        public IpAddressPrivateHelper(ControllerBase provider) : base(provider) { }

        public List<PrivateIPAddress> GetPackagePrivateIPAddresses(int packageId)
        {
            return ObjectUtils.CreateListFromDataReader<PrivateIPAddress>(
                Database.GetPackagePrivateIPAddresses(packageId));
        }

        public List<DmzIPAddress> GetPackageDmzIPAddresses(int packageId)
        {
            return ObjectUtils.CreateListFromDataReader<DmzIPAddress>(
                Database.GetPackageDmzIPAddresses(packageId));
        }

        public ResultObject RestoreVirtualMachinePrivateIPAddressesByInjection(int itemId)
        {
            ResultObject res = new ResultObject();

            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                return res;
            }

            #region Check account and space statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, vm.PackageId, DemandPackage.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("VPS2012", "RESTORE_PRIVATE_IP", vm.Id, vm.Name, vm.PackageId);
            try
            {
                // send KVP config items
                NetworkHelper.InjectIPadresses(itemId, "Private");
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.RESTORE_VIRTUAL_MACHINE_PRIVATE_IP_ADDRESS_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public ResultObject RestoreVirtualMachineDmzIPAddressesByInjection(int itemId)
        {
            ResultObject res = new ResultObject();

            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                return res;
            }

            #region Check account and space statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, vm.PackageId, DemandPackage.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("VPS2012", "RESTORE_DMZ_IP", vm.Id, vm.Name, vm.PackageId);
            try
            {
                // send KVP config items
                NetworkHelper.InjectIPadresses(itemId, "DMZ");
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.RESTORE_VIRTUAL_MACHINE_DMZ_IP_ADDRESS_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public ResultObject AddVirtualMachinePrivateIPAddressesByInject(int itemId, bool selectRandom, int addressesNumber, string[] addresses, bool customGatewayAndDns, string gateway, string dns1, string dns2, string subnetMask)
        {
            int provisionKvpType = 2;
            return AddVirtualMachinePrivateIPAddresses(itemId, selectRandom, addressesNumber, addresses, provisionKvpType, customGatewayAndDns, gateway, dns1, dns2, subnetMask);
        }

        public ResultObject AddVirtualMachinePrivateIPAddresses(int itemId, bool selectRandom, int addressesNumber, string[] addresses, bool provisionKvp, bool customGatewayAndDns, string gateway, string dns1, string dns2, string subnetMask)
        {
            int provisionKvpType = 0;
            if (provisionKvp)
                provisionKvpType = 1;

            return AddVirtualMachinePrivateIPAddresses(itemId, selectRandom, addressesNumber, addresses, provisionKvpType, customGatewayAndDns, gateway, dns1, dns2, subnetMask);
        }

        public ResultObject AddVirtualMachinePrivateIPAddresses(int itemId, bool selectRandom, int addressesNumber, string[] addresses, int provisionKvpType, bool customGatewayAndDns, string gateway, string dns1, string dns2, string subnetMask)
        {
            // trace info
            Trace.TraceInformation("Entering AddVirtualMachinePrivateIPAddresses()");
            Trace.TraceInformation("Item ID: {0}", itemId);
            Trace.TraceInformation("SelectRandom: {0}", selectRandom);
            Trace.TraceInformation("AddressesNumber: {0}", addressesNumber);

            if (addresses != null)
            {
                foreach (var address in addresses)
                    Trace.TraceInformation("addresses[n]: {0}", address);
            }

            ResultObject res = new ResultObject();

            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                return res;
            }

            #region Check account and space statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, vm.PackageId, DemandPackage.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("VPS2012", "ADD_PRIVATE_IP", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // load network adapter
                NetworkAdapterDetails nic = NetworkAdapterDetailsHelper.GetPrivateNetworkAdapterDetails(itemId);

                bool wasEmptyList = (nic.IPAddresses.Length == 0);

                if (wasEmptyList)
                    Trace.TraceInformation("NIC IP addresses list is empty");

                // check IP addresses if they are specified
                List<string> checkResults = CheckPrivateIPAddresses(vm.PackageId, addresses);
                if (checkResults.Count > 0)
                {
                    res.ErrorCodes.AddRange(checkResults);
                    res.IsSuccess = false;
                    TaskManager.CompleteResultTask();
                    return res;
                }

                // load all existing private IP addresses
                List<PrivateIPAddress> ips = GetPackagePrivateIPAddresses(vm.PackageId);

                // sort them
                SortedList<IPAddress, string> sortedIps = GetSortedNormalizedPrivateIPAddresses(ips, nic.SubnetMask);

                if (selectRandom)
                {
                    // generate N number of IP addresses
                    addresses = new string[addressesNumber];
                    for (int i = 0; i < addressesNumber; i++)
                        addresses[i] = GenerateNextAvailableIP(sortedIps, nic.SubnetMask, nic.NetworkFormat);
                }

                PackageContext cntx = PackageController.GetPackageContext(vm.PackageId);
                QuotaValueInfo quota = cntx.Quotas[Quotas.VPS2012_PRIVATE_IP_ADDRESSES_NUMBER];
                if (quota.QuotaAllocatedValue != -1)
                {
                    int maxAddresses = quota.QuotaAllocatedValue - nic.IPAddresses.Length;

                    if (addresses.Length > maxAddresses)
                    {
                        TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.QUOTA_EXCEEDED_PRIVATE_ADDRESSES_NUMBER + ":" + maxAddresses);
                        return res;
                    }
                }

                // add addresses to database
                foreach (string address in addresses)
                    Database.AddItemPrivateIPAddress(SecurityContext.User.UserId, itemId, address);

                // set primary IP address
                if (wasEmptyList)
                {
                    nic = NetworkAdapterDetailsHelper.GetPrivateNetworkAdapterDetails(itemId);
                    if (nic.IPAddresses.Length > 0)
                        SetVirtualMachinePrimaryPrivateIPAddress(itemId, nic.IPAddresses[0].AddressId, false);
                }

                if (customGatewayAndDns) // set custom Gateway and DNS
                {
                    vm.CustomPrivateGateway = gateway;
                    vm.CustomPrivateDNS1 = dns1;
                    vm.CustomPrivateDNS2 = dns2;
                    vm.CustomPrivateMask = subnetMask;
                    PackageController.UpdatePackageItem(vm);
                }

                // send KVP config items
                switch (provisionKvpType)
                {
                    case 1:
                        {
                            TaskManager.Write(String.Format("Used KVP"));
                            KvpExchangeHelper.SendNetworkAdapterKVP(itemId, "Private");
                            break;
                        }
                    case 2:
                        {
                            TaskManager.Write(String.Format("Used Inject IP"));
                            NetworkHelper.InjectIPadresses(itemId, "Private");
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

                //if(provisionKvp)
                //    SendNetworkAdapterKVP(itemId, "Private");
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.ADD_VIRTUAL_MACHINE_PRIVATE_IP_ADDRESS_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public ResultObject AddVirtualMachineDmzIPAddressesByInject(int itemId, bool selectRandom, int addressesNumber, string[] addresses, bool customGatewayAndDns, string gateway, string dns1, string dns2, string subnetMask)
        {
            int provisionKvpType = 2;
            return AddVirtualMachineDmzIPAddresses(itemId, selectRandom, addressesNumber, addresses, provisionKvpType, customGatewayAndDns, gateway, dns1, dns2, subnetMask);
        }

        public ResultObject AddVirtualMachineDmzIPAddresses(int itemId, bool selectRandom, int addressesNumber, string[] addresses, bool provisionKvp, bool customGatewayAndDns, string gateway, string dns1, string dns2, string subnetMask)
        {
            int provisionKvpType = 0;
            if (provisionKvp)
                provisionKvpType = 1;

            return AddVirtualMachineDmzIPAddresses(itemId, selectRandom, addressesNumber, addresses, provisionKvpType, customGatewayAndDns, gateway, dns1, dns2, subnetMask);
        }

        public ResultObject AddVirtualMachineDmzIPAddresses(int itemId, bool selectRandom, int addressesNumber, string[] addresses, int provisionKvpType, bool customGatewayAndDns, string gateway, string dns1, string dns2, string subnetMask)
        {
            // trace info
            Trace.TraceInformation("Entering AddVirtualMachineDmzIPAddresses()");
            Trace.TraceInformation("Item ID: {0}", itemId);
            Trace.TraceInformation("SelectRandom: {0}", selectRandom);
            Trace.TraceInformation("AddressesNumber: {0}", addressesNumber);

            if (addresses != null)
            {
                foreach (var address in addresses)
                    Trace.TraceInformation("addresses[n]: {0}", address);
            }

            ResultObject res = new ResultObject();

            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                return res;
            }

            #region Check account and space statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, vm.PackageId, DemandPackage.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("VPS2012", "ADD_DMZ_IP", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // load network adapter
                NetworkAdapterDetails nic = NetworkAdapterDetailsHelper.GetDmzNetworkAdapterDetails(itemId);

                bool wasEmptyList = (nic.IPAddresses.Length == 0);

                if (wasEmptyList)
                    Trace.TraceInformation("NIC IP addresses list is empty");

                // check IP addresses if they are specified
                List<string> checkResults = CheckDmzIPAddresses(vm.PackageId, addresses);
                if (checkResults.Count > 0)
                {
                    res.ErrorCodes.AddRange(checkResults);
                    res.IsSuccess = false;
                    TaskManager.CompleteResultTask();
                    return res;
                }

                // load all existing dmz IP addresses
                List<DmzIPAddress> ips = GetPackageDmzIPAddresses(vm.PackageId);

                // sort them
                SortedList<IPAddress, string> sortedIps = GetSortedNormalizedDmzIPAddresses(ips, nic.SubnetMask);

                if (selectRandom)
                {
                    // generate N number of IP addresses
                    addresses = new string[addressesNumber];
                    for (int i = 0; i < addressesNumber; i++)
                        addresses[i] = GenerateNextAvailableIP(sortedIps, nic.SubnetMask, nic.NetworkFormat);
                }

                PackageContext cntx = PackageController.GetPackageContext(vm.PackageId);
                QuotaValueInfo quota = cntx.Quotas[Quotas.VPS2012_DMZ_IP_ADDRESSES_NUMBER];
                if (quota.QuotaAllocatedValue != -1)
                {
                    int maxAddresses = quota.QuotaAllocatedValue - nic.IPAddresses.Length;

                    if (addresses.Length > maxAddresses)
                    {
                        TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.QUOTA_EXCEEDED_DMZ_ADDRESSES_NUMBER + ":" + maxAddresses);
                        return res;
                    }
                }

                // add addresses to database
                foreach (string address in addresses)
                    Database.AddItemDmzIPAddress(SecurityContext.User.UserId, itemId, address);

                // set primary IP address
                if (wasEmptyList)
                {
                    nic = NetworkAdapterDetailsHelper.GetDmzNetworkAdapterDetails(itemId);
                    if (nic.IPAddresses.Length > 0)
                        SetVirtualMachinePrimaryDmzIPAddress(itemId, nic.IPAddresses[0].AddressId, false);
                }

                if (customGatewayAndDns) // set custom Gateway and DNS
                {
                    vm.CustomDmzGateway = gateway;
                    vm.CustomDmzDNS1 = dns1;
                    vm.CustomDmzDNS2 = dns2;
                    vm.CustomDmzMask = subnetMask;
                    PackageController.UpdatePackageItem(vm);
                }

                // send KVP config items
                switch (provisionKvpType)
                {
                    case 1:
                        {
                            TaskManager.Write(String.Format("Used KVP"));
                            KvpExchangeHelper.SendNetworkAdapterKVP(itemId, "DMZ");
                            break;
                        }
                    case 2:
                        {
                            TaskManager.Write(String.Format("Used Inject IP"));
                            NetworkHelper.InjectIPadresses(itemId, "DMZ");
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.ADD_VIRTUAL_MACHINE_DMZ_IP_ADDRESS_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        private string GenerateNextAvailableIP(SortedList<IPAddress, string> ips, string subnetMask, string startIPAddress)
        {
            if (ips.Count > 0)
            {
                startIPAddress = ips.Values[0];
            }
            Trace.TraceInformation("Entering GenerateNextAvailablePrivateIP()");
            Trace.TraceInformation("Param - number of sorted IPs in the list: {0}", ips.Count);
            Trace.TraceInformation("Param - startIPAddress: {0}", startIPAddress);
            Trace.TraceInformation("Param - subnetMask: {0}", subnetMask);

            // start IP address
            var ip = IPAddress.Parse(startIPAddress) - 1;

            Trace.TraceInformation("Start looking for next available IP");
            foreach (var addr in ips.Keys)
            {
                if ((addr - ip) > 1)
                {
                    // it is a gap
                    break;
                }
                else
                {
                    ip = addr;
                }
            }

            // final IP found
            ip = ip + 1;

            string genIP = ip.ToString();
            Trace.TraceInformation("Generated IP: {0}", genIP);

            // store in cache
            Trace.TraceInformation("Adding to sorted list");
            ips.Add(ip, genIP);

            Trace.TraceInformation("Leaving GenerateNextAvailablePrivateIP()");
            return genIP;
        }

        private SortedList<IPAddress, string> GetSortedNormalizedPrivateIPAddresses(List<PrivateIPAddress> ips, string subnetMask)
        {
            Trace.TraceInformation("Entering GetSortedNormalizedIPAddresses()");
            Trace.TraceInformation("Param - subnetMask: {0}", subnetMask);

            var mask = IPAddress.Parse(subnetMask);
            SortedList<IPAddress, string> sortedIps = new SortedList<IPAddress, string>();
            foreach (PrivateIPAddress ip in ips)
            {
                var addr = IPAddress.Parse(ip.IPAddress);
                sortedIps.Add(addr, ip.IPAddress);

                Trace.TraceInformation("Added {0} to sorted IPs list with key: {1} ", ip.IPAddress, addr.ToString());
            }
            Trace.TraceInformation("Leaving GetSortedNormalizedIPAddresses()");
            return sortedIps;
        }

        private SortedList<IPAddress, string> GetSortedNormalizedDmzIPAddresses(List<DmzIPAddress> ips, string subnetMask)
        {
            Trace.TraceInformation("Entering GetSortedNormalizedIPAddresses()");
            Trace.TraceInformation("Param - subnetMask: {0}", subnetMask);

            var mask = IPAddress.Parse(subnetMask);
            SortedList<IPAddress, string> sortedIps = new SortedList<IPAddress, string>();
            foreach (DmzIPAddress ip in ips)
            {
                var addr = IPAddress.Parse(ip.IPAddress);
                sortedIps.Add(addr, ip.IPAddress);

                Trace.TraceInformation("Added {0} to sorted IPs list with key: {1} ", ip.IPAddress, addr.ToString());
            }
            Trace.TraceInformation("Leaving GetSortedNormalizedIPAddresses()");
            return sortedIps;
        }

        public List<string> CheckPrivateIPAddresses(int packageId, string[] addresses)
        {
            List<string> codes = new List<string>();

            // check IP addresses if they are specified
            if (addresses != null && addresses.Length > 0)
            {
                // load network adapter
                NetworkAdapterDetails nic = NetworkAdapterDetailsHelper.GetPrivateNetworkDetails(packageId);

                foreach (string address in addresses)
                {
                    if (!CheckPrivateIPAddress(nic.SubnetMask, address))
                        codes.Add(VirtualizationErrorCodes.WRONG_PRIVATE_IP_ADDRESS_FORMAT + ":" + address);
                }
            }

            return codes;
        }

        private bool CheckPrivateIPAddress(string subnetMask, string ipAddress)
        {
            var mask = IPAddress.Parse(subnetMask);
            var ip = IPAddress.Parse(ipAddress);

            //return ((mask & ip) == mask);
            return true;
        }

        public List<string> CheckDmzIPAddresses(int packageId, string[] addresses)
        {
            List<string> codes = new List<string>();

            // check IP addresses if they are specified
            if (addresses != null && addresses.Length > 0)
            {
                // load network adapter
                NetworkAdapterDetails nic = NetworkAdapterDetailsHelper.GetDmzNetworkDetails(packageId);

                foreach (string address in addresses)
                {
                    if (!CheckDmzIPAddress(nic.SubnetMask, address))
                        codes.Add(VirtualizationErrorCodes.WRONG_DMZ_IP_ADDRESS_FORMAT + ":" + address);
                }
            }

            return codes;
        }

        private bool CheckDmzIPAddress(string subnetMask, string ipAddress)
        {
            var mask = IPAddress.Parse(subnetMask);
            var ip = IPAddress.Parse(ipAddress);

            //return ((mask & ip) == mask);
            return true;
        }

        public ResultObject SetVirtualMachinePrimaryPrivateIPAddress(int itemId, int addressId, bool provisionKvp)
        {
            ResultObject res = new ResultObject();

            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                return res;
            }

            #region Check account and space statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, vm.PackageId, DemandPackage.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("VPS2012", "SET_PRIMARY_PRIVATE_IP", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // call data access layer
                Database.SetItemPrivatePrimaryIPAddress(SecurityContext.User.UserId, itemId, addressId);

                // send KVP config items
                if (provisionKvp)
                    KvpExchangeHelper.SendNetworkAdapterKVP(itemId, "Private");
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.SET_VIRTUAL_MACHINE_PRIMARY_PRIVATE_IP_ADDRESS_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public ResultObject SetVirtualMachinePrimaryDmzIPAddress(int itemId, int addressId, bool provisionKvp)
        {
            ResultObject res = new ResultObject();

            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                return res;
            }

            #region Check account and space statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, vm.PackageId, DemandPackage.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("VPS2012", "SET_PRIMARY_DMZ_IP", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // call data access layer
                Database.SetItemDmzPrimaryIPAddress(SecurityContext.User.UserId, itemId, addressId);

                // send KVP config items
                if (provisionKvp)
                    KvpExchangeHelper.SendNetworkAdapterKVP(itemId, "DMZ");
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.SET_VIRTUAL_MACHINE_PRIMARY_DMZ_IP_ADDRESS_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public ResultObject DeleteVirtualMachinePrivateIPAddressesByInject(int itemId, int[] addressIds)
        {
            int provisionKvpType = 2;
            return DeleteVirtualMachinePrivateIPAddresses(itemId, addressIds, provisionKvpType);
        }

        public ResultObject DeleteVirtualMachinePrivateIPAddresses(int itemId, int[] addressIds, bool provisionKvp)
        {
            int provisionKvpType = 0;
            if (provisionKvp)
                provisionKvpType = 1;
            return DeleteVirtualMachinePrivateIPAddresses(itemId, addressIds, provisionKvpType);
        }

        public ResultObject DeleteVirtualMachinePrivateIPAddresses(int itemId, int[] addressIds, int provisionKvpType)
        {
            if (addressIds == null)
                throw new ArgumentNullException("addressIds");

            ResultObject res = new ResultObject();

            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                return res;
            }

            #region Check account and space statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, vm.PackageId, DemandPackage.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("VPS2012", "DELETE_PRIVATE_IP", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // call data access layer
                foreach (int addressId in addressIds)
                    Database.DeleteItemPrivateIPAddress(SecurityContext.User.UserId, itemId, addressId);

                // send KVP config items
                switch (provisionKvpType)
                {
                    case 1:
                        {
                            TaskManager.Write(String.Format("Used KVP"));
                            KvpExchangeHelper.SendNetworkAdapterKVP(itemId, "Private");
                            break;
                        }
                    case 2:
                        {
                            TaskManager.Write(String.Format("Used Inject IP"));
                            NetworkHelper.InjectIPadresses(itemId, "Private");
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                //if(provisionKvp)
                //    SendNetworkAdapterKVP(itemId, "Private");
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.DELETE_VIRTUAL_MACHINE_PRIVATE_IP_ADDRESS_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public ResultObject DeleteVirtualMachineDmzIPAddressesByInject(int itemId, int[] addressIds)
        {
            int provisionKvpType = 2;
            return DeleteVirtualMachineDmzIPAddresses(itemId, addressIds, provisionKvpType);
        }

        public ResultObject DeleteVirtualMachineDmzIPAddresses(int itemId, int[] addressIds, bool provisionKvp)
        {
            int provisionKvpType = 0;
            if (provisionKvp)
                provisionKvpType = 1;
            return DeleteVirtualMachineDmzIPAddresses(itemId, addressIds, provisionKvpType);
        }

        public ResultObject DeleteVirtualMachineDmzIPAddresses(int itemId, int[] addressIds, int provisionKvpType)
        {
            if (addressIds == null)
                throw new ArgumentNullException("addressIds");

            ResultObject res = new ResultObject();

            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                return res;
            }

            #region Check account and space statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, vm.PackageId, DemandPackage.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("VPS2012", "DELETE_DMZ_IP", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // call data access layer
                foreach (int addressId in addressIds)
                    Database.DeleteItemDmzIPAddress(SecurityContext.User.UserId, itemId, addressId);

                // send KVP config items
                switch (provisionKvpType)
                {
                    case 1:
                        {
                            TaskManager.Write(String.Format("Used KVP"));
                            KvpExchangeHelper.SendNetworkAdapterKVP(itemId, "DMZ");
                            break;
                        }
                    case 2:
                        {
                            TaskManager.Write(String.Format("Used Inject IP"));
                            NetworkHelper.InjectIPadresses(itemId, "DMZ");
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.DELETE_VIRTUAL_MACHINE_DMZ_IP_ADDRESS_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

    }
}
