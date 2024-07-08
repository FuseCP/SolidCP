using SolidCP.Providers.Common;
using SolidCP.Providers.Virtualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.VM
{
    public class IpAddressExternalHelper: ControllerBase
    {
        public IpAddressExternalHelper(ControllerBase provider) : base(provider) { }

        public ResultObject RestoreVirtualMachineExternalIPAddressesByInjection(int itemId)
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
            res = TaskManager.StartResultTask<ResultObject>("VPS2012", "RESTORE_EXTERNAL_IP", vm.Id, vm.Name, vm.PackageId);
            try
            {
                // send KVP config items
                NetworkHelper.InjectIPadresses(itemId, "External");
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.RESTORE_VIRTUAL_MACHINE_EXTERNAL_IP_ADDRESS_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public ResultObject AddVirtualMachineExternalIPAddressesByInjection(int itemId, bool selectRandom, int addressesNumber, int[] addressIds)
        {
            int provisionKvpType = 2;
            return AddVirtualMachineInternalIPAddresses(itemId, selectRandom, addressesNumber, addressIds, provisionKvpType, -1);
        }

        public ResultObject AddVirtualMachineExternalIPAddresses(int itemId, bool selectRandom, int addressesNumber, int[] addressIds, bool provisionKvp)
        {
            int provisionKvpType = 0;
            if (provisionKvp)
                provisionKvpType = 1;
            return AddVirtualMachineInternalIPAddresses(itemId, selectRandom, addressesNumber, addressIds, provisionKvpType, -1);
        }

        public ResultObject AddVirtualMachineInternalIPAddresses(int itemId, bool selectRandom, int addressesNumber, int[] addressIds, int provisionKvpType, int vlan)
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
            res = TaskManager.StartResultTask<ResultObject>("VPS2012", "ADD_EXTERNAL_IP", vm.Id, vm.Name, vm.PackageId);

            // Get VLAN of 1st Network Interface
            if (vlan == -1)
                vlan = NetworkVLANHelper.GetExternalNetworkVLAN(itemId);

            try
            {
                if (selectRandom)
                {
                    List<PackageIPAddress> packageips = ServerController.GetPackageUnassignedIPAddresses(vm.PackageId, IPAddressPool.VpsExternalNetwork);
                    List<PackageIPAddress> ips = new List<PackageIPAddress>();
                    foreach (PackageIPAddress ip in packageips)
                    {
                        if (ip.VLAN == vlan)
                        {
                            ips.Add(ip);
                        }
                    }
                    if (addressesNumber > ips.Count)
                    {
                        TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.NOT_ENOUGH_PACKAGE_IP_ADDRESSES);
                        return res;
                    }

                    // get next N unassigned addresses
                    addressIds = new int[addressesNumber];
                    for (int i = 0; i < addressesNumber; i++)
                        addressIds[i] = ips[i].PackageAddressID;
                }

                // add addresses
                foreach (int addressId in addressIds)
                    ServerController.AddItemIPAddress(itemId, addressId);

                // send KVP config items
                switch (provisionKvpType)
                {
                    case 1:
                        {
                            TaskManager.Write(String.Format("Used KVP"));
                            KvpExchangeHelper.SendNetworkAdapterKVP(itemId, "External");
                            break;
                        }
                    case 2:
                        {
                            TaskManager.Write(String.Format("Used Inject IP"));
                            NetworkHelper.InjectIPadresses(itemId, "External");
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                //if (provisionKvp)
                //    SendNetworkAdapterKVP(itemId, "External");
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.ADD_VIRTUAL_MACHINE_EXTERNAL_IP_ADDRESS_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public ResultObject SetVirtualMachinePrimaryExternalIPAddress(int itemId, int packageAddressId, bool provisionKvp)
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
            res = TaskManager.StartResultTask<ResultObject>("VPS2012", "SET_PRIMARY_EXTERNAL_IP", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // call database
                ServerController.SetItemPrimaryIPAddress(itemId, packageAddressId);

                // send KVP config items
                if (provisionKvp)
                    KvpExchangeHelper.SendNetworkAdapterKVP(itemId, "External");
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.SET_VIRTUAL_MACHINE_PRIMARY_EXTERNAL_IP_ADDRESS_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }


        public ResultObject DeleteVirtualMachineExternalIPAddressesByInjection(int itemId, int[] packageAddressIds)
        {
            int provisionKvpType = 2;
            return DeleteVirtualMachineExternalIPAddresses(itemId, packageAddressIds, provisionKvpType);
        }

        public ResultObject DeleteVirtualMachineExternalIPAddresses(int itemId, int[] packageAddressIds, bool provisionKvp)
        {
            int provisionKvpType = 0;
            if (provisionKvp)
                provisionKvpType = 1;
            return DeleteVirtualMachineExternalIPAddresses(itemId, packageAddressIds, provisionKvpType);
        }

        public ResultObject DeleteVirtualMachineExternalIPAddresses(int itemId, int[] packageAddressIds, int provisionKvpType)
        {
            if (packageAddressIds == null)
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
            res = TaskManager.StartResultTask<ResultObject>("VPS2012", "DELETE_EXTERNAL_IP", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // call database
                foreach (int packageAddressId in packageAddressIds)
                    ServerController.DeleteItemIPAddress(itemId, packageAddressId);

                TaskManager.Write(String.Format("Removed IPs"));
                // send KVP config items
                switch (provisionKvpType)
                {
                    case 1:
                        {
                            TaskManager.Write(String.Format("Used KVP"));
                            KvpExchangeHelper.SendNetworkAdapterKVP(itemId, "External");
                            break;
                        }
                    case 2:
                        {
                            TaskManager.Write(String.Format("Used Inject IP"));
                            NetworkHelper.InjectIPadresses(itemId, "External");
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                //if(provisionKvp)
                //    SendNetworkAdapterKVP(itemId, "External");
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.DELETE_VIRTUAL_MACHINE_EXTERNAL_IP_ADDRESS_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

    }
}
