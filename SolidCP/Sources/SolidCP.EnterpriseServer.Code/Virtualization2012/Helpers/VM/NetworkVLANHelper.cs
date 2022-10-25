using SolidCP.Providers.Virtualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.VM
{
    public static class NetworkVLANHelper
    {
        private const int DEFAULT_VLAN = 0;

        public static int GetExternalNetworkVLAN(int itemId)
        {
            int adaptervlan = DEFAULT_VLAN;
            VirtualMachine vm = null;
            try
            {
                VirtualMachine vmgeneral = VirtualMachineHelper.GetVirtualMachineGeneralDetails(itemId);
                vm = VirtualMachineHelper.GetVirtualMachineExtendedInfo(vmgeneral.ServiceId, vmgeneral.VirtualMachineId);
                vm.ExternalNicMacAddress = vmgeneral.ExternalNicMacAddress;
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex, "VPS_GET_VM_DETAILS");
            }
            if (vm != null)
            {
                bool firstadapter = true;
                foreach (VirtualMachineNetworkAdapter adapter in vm.Adapters)
                {
                    if (firstadapter)
                    {
                        firstadapter = false;
                        adaptervlan = adapter.vlan;
                    }
                    // Overwrite First Adapter by Mac Match
                    if (adapter.MacAddress == vm.ExternalNicMacAddress)
                    {
                        adaptervlan = adapter.vlan;
                    }
                }
            }
            return adaptervlan;
        }
    }
}
