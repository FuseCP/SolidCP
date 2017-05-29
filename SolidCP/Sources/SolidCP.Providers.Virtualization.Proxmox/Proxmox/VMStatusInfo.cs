using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.Virtualization.Proxmox
{
    public class VMStatusInfo
    {
        public string data { get; set; }
        public string status { get; set; }
        public long maxdisk { get; set; }
        public string ballooninfo { get; set; }
        public int cpus { get; set; }
        public long uptime { get; set; }
        public long mem { get; set; }
        public string name { get; set; }
        public float cpu { get; set; }
        public long total_mem { get; set; }
        public long free_mem { get; set; }
        public long maxmem { get; set; }
        public string qmpstatus { get; set; }
    }
}
