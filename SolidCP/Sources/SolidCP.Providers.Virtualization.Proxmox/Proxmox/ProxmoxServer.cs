using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.Virtualization.Proxmox
{
    public class ProxmoxServer
    {
        public string Hostname { get; set; }
        public string Ip { get; set; }
        public string Port { get; set; }
    }
}
