using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.Common
{
    public class SystemResourceUsageInfo
    {
        public short ProcessorTimeUsagePercent { get; set; } = -1; //Non Hyper-V Processor Information(_Total)\% Processor Time (useless for Hyper-V hosts)
        public short LogicalProcessorUsagePercent { get; set; } = -1; //Hyper-V Hypervisor Logical Processor(_Total)\% Total Run Time
        public SystemMemoryInfo SystemMemoryInfo { get; set; }

        //We cannot collect "Hyper-V Hypervisor Virtual Processor(*)\% Total Run Time" for all VMs, as it causes excessive load on the server, with memory leacking
    }
}
