using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.Virtualization.Proxmox
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Realm { get; set; }
    }
}
