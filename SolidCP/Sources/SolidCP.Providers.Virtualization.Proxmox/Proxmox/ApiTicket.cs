using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.Virtualization.Proxmox
{
    public class ApiTicket
    {
        public string CSRFPreventionToken { get; set; }
        public string ticket { get; set; }
        public string username { get; set; }
    }
}
