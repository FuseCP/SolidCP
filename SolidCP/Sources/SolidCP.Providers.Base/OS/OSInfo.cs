using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace SolidCP.Providers.OS
{

	public enum OSPlatform { Unknown = 0, Windows, Mac, Linux, Unix, Other };
	public enum OSFlavor { Unknown = 0, Min = 0, Windows, Mac, Debian, Mint, Kali, Ubuntu, Fedora, RedHat, Oracle, CentOS, SUSE, Alpine, Arch, FreeBSD, NetBSD, Other, Max = Other }

	public class OSInfo
	{
		public static bool IsMono => Type.GetType("Mono.Runtime") != null;
		public static bool IsCore => !(IsNetFX || IsNetNative);
		public static bool IsNetFX => RuntimeInformation.FrameworkDescription.StartsWith(".NET Framework", StringComparison.OrdinalIgnoreCase);
		public static bool IsNetNative => RuntimeInformation.FrameworkDescription.StartsWith(".NET Native", StringComparison.OrdinalIgnoreCase);
		public static bool IsWindows => RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
		public static bool IsWindowsServer => WindowsVersion.ToString().Contains("WindowsServer");
		public static bool IsLinux => RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);
		public static bool IsMac => RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX);
		public static bool IsArm => Architecture == Architecture.Arm64 || Architecture == Architecture.Arm;
		public static bool IsIntel => Architecture == Architecture.X64 || Architecture == Architecture.X86;
		public static bool IsNet48 => !IsMono && IsWindows && Regex.IsMatch(RuntimeInformation.FrameworkDescription, @"^\.NET Framework 4\.[8-9]");
		public static bool Is64 => Environment.Is64BitOperatingSystem;
		public static bool Is32 => !Is64;
		public static string FrameworkDescription => RuntimeInformation.FrameworkDescription;

		public static readonly System.Runtime.InteropServices.OSPlatform FreeBSD = System.Runtime.InteropServices.OSPlatform.Create("FREEBSD");
		public static readonly System.Runtime.InteropServices.OSPlatform NetBSD = System.Runtime.InteropServices.OSPlatform.Create("NETBSD");

		public static bool IsUnix => IsLinux || IsMac || IsFreeBSD || IsNetBSD;
		public static bool IsFreeBSD => RuntimeInformation.IsOSPlatform(FreeBSD);
		public static bool IsNetBSD => RuntimeInformation.IsOSPlatform(NetBSD);

		static OSFlavor flavor = OSFlavor.Unknown;
		static Version version = new Version("0.0.0.0");

		public static string NetVersion
		{
			get
			{
				if (IsMono || IsCore) return Regex.Match(FrameworkDescription, @"[0-9]+(\.[0-9]+)?").Value;

				return WindowsOSInfo.NetFXVersion;
			}
		}

		public static string NetDescription
		{
			get
			{
				if (IsMono || IsCore) return FrameworkDescription;
				return $".NET Framework {NetVersion}";
			}
		}
		public static OSPlatform OSPlatform => IsWindows ? OSPlatform.Windows :
			 (IsMac ? OSPlatform.Mac :
			 (IsLinux ? OSPlatform.Linux :
			 (IsNetBSD || IsFreeBSD ? OSPlatform.Unix : OSPlatform.Other)));

		public static Architecture Architecture => RuntimeInformation.ProcessArchitecture;
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
						var osRelease = File.ReadAllText(OsReleaseFile);
						var match = Regex.Match(osRelease, "(?<=^NAME\\s*=\\s*\")[^\"]+(?=\")", RegexOptions.IgnoreCase | RegexOptions.Multiline);
						if (match.Success) name = match.Value;
						match = Regex.Match(osRelease, @"(?<=^VERSION_ID\s*=\s*""[^""0-9]*?)[0-9]+(\.[0-9]+)?(\.[0-9]+)?(\.[0-9]+)?", RegexOptions.IgnoreCase | RegexOptions.Multiline);
						Version osReleaseVersion;
						int intVersion;
						if (match.Success)
						{
							if (Version.TryParse(match.Value, out osReleaseVersion)) version = osReleaseVersion;
							else if (int.TryParse(match.Value, out intVersion)) {
								version = new Version(intVersion, 0);
							}
						}
					}
					if (name == null)
					{
						var osRelease = Shell.Standard.Exec("lsb_release -a").Output().Result;
						var match = Regex.Match(osRelease, "(?<=^Distributor ID\\s*:\\s*)[^\\s$]+(?=\\s|$)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
						if (match.Success) name = match.Value;
						match = Regex.Match(osRelease, @"(?<=^Release\s*:[^0-9]*?)[0-9]+\.[0-9]+(\.[0-9]+)?(\.[0-9]+)?", RegexOptions.IgnoreCase | RegexOptions.Multiline);
						if (match.Success) Version.TryParse(match.Value, out version);
					}
					// TODO use hostnamectl
					OSFlavor f;
					if (name == null) flavor = OSFlavor.Other;
					else if (Enum.TryParse<OSFlavor>(name, out f)) flavor = f;
					else
					{
						for (var os = OSFlavor.Min; os <= OSFlavor.Max; os++)
						{
							if (Regex.IsMatch(name, $"(?<=^|\\s){Regex.Escape(Enum.GetName(typeof(OSFlavor), os))}(?=\\s|$)", RegexOptions.IgnoreCase))
							{
								flavor = os;
								break;
							}
						}
					}
				}
				return flavor == OSFlavor.Unknown ? OSFlavor.Other : flavor;
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

		public static string SolidCPVersion
		{
			get
			{
				var entryAssemblies = new string[] { "SolidCP.Server", "SolidCP.EnterpriseServer", "SolidCP.WebPortal", "SolidCP.WebDavPortal" };
				var entryAssembly = AppDomain.CurrentDomain.GetAssemblies()
					.FirstOrDefault(a => entryAssemblies.Any(name => a.GetName().Name == name)) ?? Assembly.GetEntryAssembly();
				var fileVersion = entryAssembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
				if (fileVersion == null) return "";
				else return fileVersion.Version;
			}
		}
		public static WindowsVersion WindowsVersion => IsWindows ? WindowsOSInfo.GetVersion() : WindowsVersion.NonWindows;
		public static string Description
		{
			get
			{
				if (IsLinux)
				{
					string name = null;
					const string OsReleaseFile = "/etc/os-release";
					if (File.Exists(OsReleaseFile))
					{
						var osRelease = File.ReadAllText(OsReleaseFile);
						var match = Regex.Match(osRelease, "(?<=^PRETTY_NAME\\s*=\\s*\")[^\"]+(?=\")", RegexOptions.IgnoreCase | RegexOptions.Multiline);
						if (match.Success) name = match.Value;
					}
					if (name == null)
					{
						var osRelease = Shell.Standard.Exec("lsb_release -a").Output().Result;
						var match = Regex.Match(osRelease, "(?<=^Description\\s*:\\s*)[^\\s$]+(?=\\s|$)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
						if (match.Success) name = match.Value;
					}
					if (name == null)
					{
						name = Shell.Standard.Exec("uname").Output().Result;
					}
					return name;

				}
				else
				{
					return RuntimeInformation.OSDescription;
				}
			}
		}

		public static OS.IUnixOperatingSystem Unix => (IUnixOperatingSystem)Current;
		public static IWindowsOperatingSystem Windows => (IWindowsOperatingSystem)Current;

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
							case WindowsVersion.Windows10:
							case WindowsVersion.WindowsServer2019:
								os = Activator.CreateInstance(Type.GetType("SolidCP.Providers.OS.Windows2019, SolidCP.Providers.OS.Windows2019")) as Providers.OS.IOperatingSystem;
								break;
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
	}
}
