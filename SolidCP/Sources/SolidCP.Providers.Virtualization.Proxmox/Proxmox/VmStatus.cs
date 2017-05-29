using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.Virtualization.Proxmox
{
    public enum VmStatus
    {
        running,
        stopped,
        unknown
    }

    public static class VmStatusExtension
    {
        public static VmStatus AsVmStatus(this string vmStatus)
        {
            if (vmStatus == Enum.GetName(typeof(VmStatus), VmStatus.running))
            {
                return VmStatus.running;
            }
            if (vmStatus == Enum.GetName(typeof(VmStatus), VmStatus.stopped))
            {
                return VmStatus.stopped;
            }
            return VmStatus.unknown;
        }
    }
}
