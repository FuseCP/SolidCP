using SolidCP.Providers.WebAppGallery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace SolidCP.Providers.OS
{

    public enum OSPlatform { Unknown, Windows, Mac, Linux, Unix, Other };
	public enum OSFlavor { Unknown, Windows, Mac, Debian, Mint, Ubuntu, Fedora, RedHat, CentOS, SUSE, Alpine, FreeBSD, NetBSD, Other }

	public class OSInfo
	{
		public static bool IsMono => Type.GetType("Mono.Runtime") != null;
		public static bool IsCore => !(IsNetFX || IsNetNative);
		public static bool IsNetFX => RuntimeInformation.FrameworkDescription.StartsWith(".NET Framework", StringComparison.OrdinalIgnoreCase);
		public static bool IsNetNative => RuntimeInformation.FrameworkDescription.StartsWith(".NET Native", StringComparison.OrdinalIgnoreCase);
		public static bool IsWindows => RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
		public static bool IsLinux => RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);
		public static bool IsMac => RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX);

		public static readonly System.Runtime.InteropServices.OSPlatform FreeBSD = System.Runtime.InteropServices.OSPlatform.Create("FREEBSD");
		public static readonly System.Runtime.InteropServices.OSPlatform NetBSD = System.Runtime.InteropServices.OSPlatform.Create("NETBSD");

		public static bool IsUnix => IsLinux || IsMac || IsFreeBSD || IsNetBSD;
		public static bool IsFreeBSD => RuntimeInformation.IsOSPlatform(FreeBSD);
		public static bool IsNetBSD => RuntimeInformation.IsOSPlatform(NetBSD);

		static OSFlavor flavor = OSFlavor.Unknown;
		static Version version = new Version("0.0.0.0");

		public static OSPlatform OSPlatform => IsWindows ? OSPlatform.Windows :
			 (IsMac ? OSPlatform.Mac :
			 (IsLinux ? OSPlatform.Linux :
			 (IsNetBSD || IsFreeBSD ? OSPlatform.Unix : OSPlatform.Other)));

		public static OSFlavor OSFlavor
		{
			get
			{
				if (flavor != OSFlavor.Unknown) return flavor;
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
						var osRelease = Current.DefaultShell.Exec("lsb_release -a").Output().Result;
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
				return flavor == OSFlavor.Unknown ?  OSFlavor.Other : flavor;
			}
		}
		public static Version OSVersion
		{
			get
			{
				var flavor = OSFlavor;
				return version;
			}
		}
		public static WindowsVersion WindowsVersion => IsWindows ? WindowsOSInfo.GetVersion() : WindowsVersion.NonWindows;
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
						var version = WindowsOSInfo.GetVersion();
						switch (version)
						{
							case WindowsVersion.WindowsServer2022:
							case WindowsVersion.Windows11:
								os = Activator.CreateInstance(Type.GetType("SolidCP.Providers.OS.Windows2022, SolidCP.Providers.OS.Windows2022")) as Providers.OS.IOperatingSystem;
								break;
							case WindowsVersion.WindowsServer2019:
								os = Activator.CreateInstance(Type.GetType("SolidCP.Providers.OS.Windows2019, SolidCP.Providers.OS.Windows2019")) as Providers.OS.IOperatingSystem;
								break;
							case WindowsVersion.Windows10:
							case WindowsVersion.WindowsServer2016:
								os = Activator.CreateInstance(Type.GetType("SolidCP.Providers.OS.Windows2016, SolidCP.Providers.OS.Windows2016")) as Providers.OS.IOperatingSystem;
								break;
							case WindowsVersion.WindowsServer2012:
							case WindowsVersion.Windows8:
							case WindowsVersion.WindowsServer2012R2:
							case WindowsVersion.Windows81:
								os = Activator.CreateInstance(Type.GetType("SolidCP.Providers.OS.Windows2012, SolidCP.Providers.OS.Windows2012")) as Providers.OS.IOperatingSystem;
								break;
							case WindowsVersion.WindowsServer2008:
							case WindowsVersion.WindowsServer2008R2:
							case WindowsVersion.Vista:
							case WindowsVersion.Windows7:
								os = Activator.CreateInstance(Type.GetType("SolidCP.Providers.OS.Windows2008m SolidCP.Providers.OS.Windows2008")) as Providers.OS.IOperatingSystem;
								break;

							case WindowsVersion.WindowsServer2003:
							case WindowsVersion.WindowsXP:
							case WindowsVersion.WindowsNT4:
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
		public static Shell Shell => Current.DefaultShell;
		public static Installer Installer => Current.DefaultInstaller;
	}
}
