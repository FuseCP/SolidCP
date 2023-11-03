using SolidCP.Providers.WebAppGallery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Server.Utils
{
    public class OS
    {
        public bool IsMono;
        public bool IsDotNet;
        public bool IsNetFX;
        public bool IsWindows;
        public bool IsLinux;
        public bool IsMac;
        public bool IsUnix;

        public SolidCP.Providers.OS.IOperatingSystem Current;
        public Shell Shell => Current.Shell;
        public Installer Installer => Current.Installer;
    }
}
