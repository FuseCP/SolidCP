using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.Virtualization.Proxmox
{
    public class ListProxmoxSnapshots
    {
        public List<snapshotfields> data { get; set; }
    }
    public class snapshotfields
    {
        public string name { get; set; }
        public string description { get; set; }
        public string parent { get; set; }
        public int snaptime { get; set; }
        public int vmstate { get; set; }
        public string digest { get; set; }
    }
}
