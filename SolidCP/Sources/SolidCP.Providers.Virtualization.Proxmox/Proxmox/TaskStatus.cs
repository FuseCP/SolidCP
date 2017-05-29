using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.Virtualization.Proxmox
{
    public class ProxmoxTaskStatus
    {
        public string exitstatus { get; set; }
        public string status { get; set; }
        public string upid { get; set; }
        public string node { get; set; }
        public long pid { get; set; }
        public long starttime { get; set; }
        public string user { get; set; }
        public string type { get; set; }
        public string id { get; set; }
        public long pstart { get; set; }
    }
}
