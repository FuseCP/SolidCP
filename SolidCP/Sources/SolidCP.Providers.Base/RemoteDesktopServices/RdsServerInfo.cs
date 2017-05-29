using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidCP.Providers.RemoteDesktopServices
{
    public class RdsServerInfo
    {
        public string Status { get; set; }
        public int NumberOfCores { get; set; }
        public int MaxClockSpeed { get; set; }
        public int LoadPercentage { get; set; }
        public double MemoryAllocatedMb { get; set; }
        public double FreeMemoryMb { get; set; }
        public RdsServerDriveInfo[] Drives { get; set; }
    }
}
