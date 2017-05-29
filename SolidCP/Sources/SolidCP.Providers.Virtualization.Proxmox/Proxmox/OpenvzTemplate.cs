using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.Virtualization.Proxmox
{
    public class OpenvzTemplate
    {
        public string ostemplate { get; set; }
        public string vmid { get; set; }
        public string hostname { get; set; }
        public string storage { get; set; }
        public string password { get; set; }
        public string memory { get; set; }
        public string swap { get; set; }
        public string disk { get; set; }
        public string cpus { get; set; }
        public string ip_address { get; set; }
        public string pool { get; set; }
    }
}
