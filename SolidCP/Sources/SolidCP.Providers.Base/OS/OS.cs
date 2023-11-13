using SolidCP.Providers.WebAppGallery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SolidCP.Server.Utils
{
    public class OS
    {
        public static bool IsMono => Type.GetType("Mono.Runtime") != null;
        public static bool IsCore => !(IsNetFX || IsNetNative);
        public static bool IsNetFX => RuntimeInformation.FrameworkDescription.StartsWith(".NET Framework", StringComparison.OrdinalIgnoreCase);
		public static bool IsNetNative => RuntimeInformation.FrameworkDescription.StartsWith(".NET Native", StringComparison.OrdinalIgnoreCase);
		public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        public static bool IsMac => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
		public static bool IsUnix => IsLinux || IsMac ||
			RuntimeInformation.IsOSPlatform(OSPlatform.Create("FREEBSD")) ||
			RuntimeInformation.IsOSPlatform(OSPlatform.Create("NETBSD"));
        public static string Description => RuntimeInformation.OSDescription;

        static Providers.OS.IOperatingSystem os = null;
		public static Providers.OS.IOperatingSystem Current {
            get {
                if (os == null)
                {
                    if (IsWindows)
                    {
                        var version = WindowsOS.GetVersion();
                        switch (version)
                        {
                            case WindowsOS.WindowsVersion.WindowsServer2022:
                            case WindowsOS.WindowsVersion.Windows11:
                                os = Activator.CreateInstance(Type.GetType("SolidCP.Providers.OS.Windows2022, SolidCP.Providers.OS.Windows2022")) as Providers.OS.IOperatingSystem;
                                break;
							case WindowsOS.WindowsVersion.WindowsServer2019:
								os = Activator.CreateInstance(Type.GetType("SolidCP.Providers.OS.Windows2019, SolidCP.Providers.OS.Windows2019")) as Providers.OS.IOperatingSystem;
                                break;
							case WindowsOS.WindowsVersion.Windows10:
							case WindowsOS.WindowsVersion.WindowsServer2016:
								os = Activator.CreateInstance(Type.GetType("SolidCP.Providers.OS.Windows2016, SolidCP.Providers.OS.Windows2016")) as Providers.OS.IOperatingSystem;
								break;
							case WindowsOS.WindowsVersion.WindowsServer2012:
	                        case WindowsOS.WindowsVersion.Windows8:
                            case WindowsOS.WindowsVersion.WindowsServer2012R2:
	                        case WindowsOS.WindowsVersion.Windows81:
								os = Activator.CreateInstance(Type.GetType("SolidCP.Providers.OS.Windows2012, SolidCP.Providers.OS.Windows2012")) as Providers.OS.IOperatingSystem;
                                break;
                            case WindowsOS.WindowsVersion.WindowsServer2008:
                            case WindowsOS.WindowsVersion.WindowsServer2008R2:
                            case WindowsOS.WindowsVersion.Vista:
                            case WindowsOS.WindowsVersion.Windows7:
								os = Activator.CreateInstance(Type.GetType("SolidCP.Providers.OS.Windows2008m SolidCP.Providers.OS.Windows2008")) as Providers.OS.IOperatingSystem;
                                break;

                            case WindowsOS.WindowsVersion.WindowsServer2003:
                            case WindowsOS.WindowsVersion.WindowsXP:
                            case WindowsOS.WindowsVersion.WindowsNT4:
								os = Activator.CreateInstance(Type.GetType("SolidCP.Providers.OS.Windows2003")) as Providers.OS.IOperatingSystem;
								break;
						}
					} else if (IsUnix)
                    {
						os = Activator.CreateInstance(Type.GetType("SolidCP.Providers.OS.Unix, SolidCP.Providers.OS.Unix")) as Providers.OS.IOperatingSystem;
					}
				}
                return os;
            }
        }
        public static Providers.Shell Shell => Current.DefaultShell;
        public static Providers.Installer Installer => Current.DefaultInstaller;
    }
}
