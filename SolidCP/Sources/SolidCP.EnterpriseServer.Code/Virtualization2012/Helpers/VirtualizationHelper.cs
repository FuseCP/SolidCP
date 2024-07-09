using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers;
using SolidCP.Providers;
using SolidCP.Providers.Virtualization;
//using SolidCP.Providers.Virtualization2012;
using SolidCP.Server.Client;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012
{
    public class VirtualizationHelper: ControllerBase
    {
        private const string MAINTENANCE_MODE_EMABLED = "enabled";

        public VirtualizationHelper(ControllerBase provider) : base(provider) { }

        public VirtualizationServer2012 GetVirtualizationProxy(int serviceId)
        {
            VirtualizationServer2012 ws = new VirtualizationServer2012();
            ServiceProviderProxy.Init(ws, serviceId);
            return ws;
        }

        #region MaintenanceMode
        public bool IsMaintenanceMode(int itemId)
        {
            return IsMaintenanceMode(itemId, null);
        }
        public bool IsMaintenanceMode(StringDictionary settings)
        {
            return IsMaintenanceMode(-1, settings);
        }
        public bool IsMaintenanceMode(int itemId, StringDictionary settings)
        {
            if (settings == null && itemId != -1)
            {
                // service ID
                int serviceId = GetServiceId(PackageController.GetPackageItem(itemId).PackageId);

                // load service settings
                settings = ServerController.GetServiceSettings(serviceId);
            }

            bool isMaintenanceMode = settings["MaintenanceMode"] == MAINTENANCE_MODE_EMABLED;

            // Administrator ignore that rule
            return UserController.GetUserInternally(SecurityContext.User.UserId).Role != UserRole.Administrator && isMaintenanceMode;
        }
        #endregion

        public int GetServiceId(int packageId)
        {
            int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.VPS2012);
            return serviceId;
        }

        public VirtualizationServer2012 GetVirtualizationProxyByPackageId(int packageId)
        {
            // get service
            int serviceId = GetServiceId(packageId);

            return GetVirtualizationProxy(serviceId);
        }

        #region VPS OS
        public LibraryItem[] GetOperatingSystemTemplates(int packageId)
        {
            // load service settings
            int serviceId = GetServiceId(packageId);

            // return templates
            return GetOperatingSystemTemplatesByServiceId(serviceId);
        }

        public LibraryItem[] GetOperatingSystemTemplatesByServiceId(int serviceId)
        {
            // load service settings
            StringDictionary settings = ServerController.GetServiceSettings(serviceId);
            string xml = settings["OsTemplates"];

            var config = new ConfigFile(xml);

            return config.LibraryItems;
        }
        #endregion

        #region VPS Configuration
        public int GetMaximumCpuCoresNumber(int packageId)
        {
            // get proxy
            VirtualizationServer2012 vs = GetVirtualizationProxyByPackageId(packageId);

            return vs.GetProcessorCoresNumber();
        }

        public string GetDefaultExportPath(int itemId)
        {
            // load meta item
            VirtualMachine vm = VirtualMachineHelper.GetVirtualMachineByItemId(itemId);

            if (vm == null)
                return null;

            // load settings
            StringDictionary settings = ServerController.GetServiceSettings(vm.ServiceId);
            return settings["ExportedVpsPath"];
        }
        #endregion

        public int DiscoverVirtualMachine(int itemId)
        {
            try
            {
                VirtualMachine vm = VirtualMachineHelper.GetVirtualMachineByItemId(itemId);
                if (vm == null || String.IsNullOrEmpty(vm.VirtualMachineId)) return -1;
                int oldServiceId = vm.ServiceId;

                // check virtual server services first
                PackageInfo package = PackageController.GetPackage(vm.PackageId);
                if (package != null)
                {
                    ServerInfo server = ServerController.GetServerById(package.ServerId, true);
                    if (server != null && server.VirtualServer)
                    {
                        DataSet dsServices = ServerController.GetVirtualServices(package.ServerId, true);
                        DataView dvGroups = dsServices.Tables[0].DefaultView;
                        int vpsGroupId = -1;
                        foreach (DataRowView dr in dvGroups)
                        {
                            if (ResourceGroups.VPS2012.Equals((string)dr["GroupName"]))
                            {
                                vpsGroupId = (int)dr["GroupID"];
                                break;
                            }
                        }
                        if (vpsGroupId != -1)
                        {
                            DataView dvVirtualServices = dsServices.Tables[1].DefaultView;
                            foreach (DataRowView dr in dvVirtualServices)
                            {
                                if ((int)dr["GroupID"] != vpsGroupId) continue;
                                int serviceId = (int)dr["ServiceID"];
                                if (serviceId != oldServiceId && CheckVmService(serviceId, itemId, vm.VirtualMachineId)) return serviceId;
                            }
                        }
                    }
                }

                // check all VPS2012 services
                DataView dvServices = ServerController.GetRawServicesByGroupName(ResourceGroups.VPS2012, true).Tables[0].DefaultView;

                foreach (DataRowView dr in dvServices)
                {
                    int serviceId = (int)dr["ServiceID"];
                    if (serviceId != oldServiceId && CheckVmService(serviceId, itemId, vm.VirtualMachineId)) return serviceId;
                }
            }
            catch (Exception) { }

            return -1;
        }

        private bool CheckVmService(int serviceId, int itemId, string virtualMachineId)
        {
            try
            {
                VirtualizationServer2012 vps = GetVirtualizationProxy(serviceId);
                VirtualMachine newVm = vps.GetVirtualMachine(virtualMachineId);
                if (newVm != null && newVm.State != VirtualMachineState.Unknown)
                {
                    PackageController.MovePackageItem(itemId, serviceId, true);
                    return true;
                }
            }
            catch (Exception) { }
            return false;
        }

        public string EnsurePrivateVirtualSwitch(ServiceProviderItem item)
        {
            // try locate switch in the package
            List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(item.PackageId, typeof(VirtualSwitch));

            // exists - return ID
            if (items.Count > 0)
                return ((VirtualSwitch)items[0]).SwitchId;

            // switch name
            string name = VirtualizationUtils.EvaluateItemVariables("[username] - [space_name]", item);

            // log
            TaskManager.Write("VPS_CREATE_PRIVATE_VIRTUAL_SWITCH", name);

            try
            {
                // create switch
                // load proxy
                VirtualizationServer2012 vs = GetVirtualizationProxy(item.ServiceId);

                // create switch
                VirtualSwitch sw = vs.CreateSwitch(name);
                sw.ServiceId = item.ServiceId;
                sw.PackageId = item.PackageId;

                // save item
                PackageController.AddPackageItem(sw);

                return sw.SwitchId;
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex, "VPS_CREATE_PRIVATE_VIRTUAL_SWITCH_ERROR");
                return null;
            }
        }

        public static string EnsureDmzVirtualSwitch(ServiceProviderItem item)
        {
            // try locate switch in the package
            List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(item.PackageId, typeof(VirtualSwitch));

            // exists - return ID
            if (items.Count > 0)
                return ((VirtualSwitch)items[0]).SwitchId;

            // switch name
            string name = VirtualizationUtils.EvaluateItemVariables("[username] - [space_name]", item);

            // log
            TaskManager.Write("VPS_CREATE_DMZ_VIRTUAL_SWITCH", name);

            try
            {
                // create switch
                // load proxy
                VirtualizationServer2012 vs = GetVirtualizationProxy(item.ServiceId);

                // create switch
                VirtualSwitch sw = vs.CreateSwitch(name);
                sw.ServiceId = item.ServiceId;
                sw.PackageId = item.PackageId;

                // save item
                PackageController.AddPackageItem(sw);

                return sw.SwitchId;
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex, "VPS_CREATE_DMZ_VIRTUAL_SWITCH_ERROR");
                return null;
            }
        }
    }
}
