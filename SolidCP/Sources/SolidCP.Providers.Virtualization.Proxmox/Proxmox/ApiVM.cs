using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.Virtualization.Proxmox
{
    public class ApiVM: IEquatable<ApiVM>
    {
        public string Node { get; set; }
        public string Id { get; set; }

        public bool Equals(ApiVM other) => Node == other.Node && Id == other.Id;

        public override int GetHashCode() => Node.GetHashCode() ^ Id.GetHashCode();
    }
}
