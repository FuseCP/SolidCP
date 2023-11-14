using SolidCP.Providers.WebAppGallery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace SolidCP.Server.Utils
{
    public enum OSFlavor { Windows, Mac, Debian, Mint, Ubuntu, Fedora, RedHat, CentOS, SUSE, Alpine, FreeBSD, NetBSD, Other }
    public class OS
    {
        public static bool IsMono => Type.GetType("Mono.Runtime") != null;
        public static bool IsCore => !(IsNetFX || IsNetNative);
        public static bool IsNetFX => RuntimeInformation.FrameworkDescription.StartsWith(".NET Framework", StringComparison.OrdinalIgnoreCase);
        public static bool IsNetNative => RuntimeInformation.FrameworkDescription.StartsWith(".NET Native", StringComparison.OrdinalIgnoreCase);
        public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        public static bool IsMac => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        public static readonly OSPlatform FreeBSD = OSPlatform.Create("FREEBSD");
        public static readonly OSPlatform NetBSD = OSPlatform.Create("NETBSD");

        public static bool IsUnix => IsLinux || IsMac || IsFreeBSD || IsNetBSD;
        public static bool IsFreeBSD => RuntimeInformation.IsOSPlatform(FreeBSD);
        public static bool IsNetBSD => RuntimeInformation.IsOSPlatform(NetBSD);

        static OSFlavor? flavor = OSFlavor.Other;
        static Version version = new Version("0.0.0.0");

        public static OSPlatform OSPlatform => IsWindows ? OSPlatform.Windows :
            (IsMac ? OSPlatform.OSX :
            (IsLinux ? OSPlatform.Linux :
            (IsNetBSD ? NetBSD :
            (IsFreeBSD ? FreeBSD : OSPlatform.Create("UNIX")))));
        public static OSFlavor OSFlavor
        {
            get
            {
                if (flavor != null) return flavor.Value;
                version = Environment.OSVersion.Version;
                if (IsWindows) return OSFlavor.Windows;
                if (IsMac) return OSFlavor.Mac;
                if (IsFreeBSD) return OSFlavor.FreeBSD;
                if (IsNetBSD) return OSFlavor.NetBSD;
                if (IsLinux)
                {
                    string name = null;
                    const string OsReleaseFile = "/etc/os-release";
                    if (File.Exists(OsReleaseFile))
                    {
                        var osRelease = File.ReadAllText("/etc/os-release");
                        var match = Regex.Match(osRelease, "(?<=^NAME\\s*=\\s*\")[^\"]+(?=\")", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        if (match.Success) name = match.Value;
                        match = Regex.Match(osRelease, @"(?<=^VERSION\s*=\s*""[^""0-9]*?)[0-9]+\.[0-9]+(\.[0-9]+)?(\.[0-9]+)?", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        if (match.Success) Version.TryParse(match.Value, out version);
                    }
                    if (name == null)
                    {
                        var osRelease = Current.DefaultShell.ExecAsync("lsb_release -a").Output().Result;
                        var match = Regex.Match(osRelease, "(?<=^Distributor ID\\s*:\\s*)[^\\s$]+(?=\\s|$)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        if (match.Success) name = match.Value;
                        match = Regex.Match(osRelease, @"(?<=^Release\s*:[^0-9]*?)[0-9]+\.[0-9]+(\.[0-9]+)?(\.[0-9]+)?", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        if (match.Success) Version.TryParse(match.Value, out version);
                    }
                    // TODO use hostnamectl
                    OSFlavor f;
                    if (name == null) flavor = OSFlavor.Other;
                    else if (Enum.TryParse<OSFlavor>(name, out f)) flavor = f;
                }
                return flavor ?? OSFlavor.Other;
            }
        }
        public static Version OSVersion => version;
        public static string Description => RuntimeInformation.OSDescription;

        static Providers.OS.IOperatingSystem os = null;
        public static Providers.OS.IOperatingSystem Current
        {
            get
            {
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
                    }
                    else if (IsUnix)
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
