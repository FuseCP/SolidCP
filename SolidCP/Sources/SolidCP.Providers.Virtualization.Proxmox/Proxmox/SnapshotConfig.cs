using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.Virtualization.Proxmox
{
    public class SnapshotConfig
    {
        public string name { get; set; }
        public string description { get; set; }
        public string parent { get; set; }
        public int snaptime { get; set; }
        public string vmstate { get; set; }
        public string digest { get; set; }
    }
}
