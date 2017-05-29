using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.Virtualization.Proxmox
{
    public class VMConfig
    {
        public long baloon { get; set; }
        public string bootdisk { get; set; }
        public long cores { get; set; }
        public string cpu { get; set; }
        public string digest { get; set; }
        public string ide2 { get; set; }
        public long memory { get; set; }
        public string name { get; set; }
        public string net0 { get; set; }
        public string ostype { get; set; }
        public string parent { get; set; }
        public string smbios1 { get; set; }
        public string virtio0 { get; set; }
        public string scsi0 { get; set; }
    }
}
