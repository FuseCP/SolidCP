using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.OS
{
    public class SystemMemoryInfo
    {
        public ulong FreePhysicalKB { get; set; }
        public ulong FreeVirtualKB { get; set; }
        public ulong TotalVirtualSizeKB { get; set; }
        public ulong TotalVisibleSizeKB { get; set; }
    }
}
