using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidCP.Providers.RemoteDesktopServices
{
    public class RdsServerDriveInfo
    {
        public string DeviceId { get; set; }
        public string VolumeName { get; set; }
        public double SizeMb { get; set; }
        public double FreeSpaceMb { get; set; }
    }
}
