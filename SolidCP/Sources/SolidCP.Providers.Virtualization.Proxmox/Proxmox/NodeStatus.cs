using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.Virtualization.Proxmox
{
    public class NodeStatus
    {
        public string data { get; set; }
        public int cpu { get; set; }
        public cpuinfo cpuinfo { get; set; }
    }
    public class cpuinfo
    {
        public int cpus { get; set; }
        public string model { get; set; }
        public string mhz { get; set; }
    }

}
