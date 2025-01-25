using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net.Http;
using Newtonsoft.Json.Bson;
using SolidCP.Providers.Web;
using SolidCP.Providers;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Security.Policy;
using System.Security.Principal;
using Microsoft.Win32;

namespace SolidCP.UniversalInstaller;

public class WindowsInstaller : Installer
{
	const bool Net8RuntimeNeededOnWindows = true;

	public override string InstallExeRootPath
	{
		get => base.InstallExeRootPath ??
			(base.InstallExeRootPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), SolidCP));
		set => base.InstallExeRootPath = value;
	}
	public override string InstallWebRootPath { get => base.InstallWebRootPath ?? InstallExeRootPath; set => base.InstallWebRootPath = value; }
	public override string WebsiteLogsPath => InstallExeRootPath ?? "";
	WinGet WinGet => (WinGet)((IWindowsOperatingSystem)OSInfo.Current).WinGet;
	public override string Net8Filter(string file)
	{
		file = SetupFilter(file);
		return (file != null && (!file.StartsWith("bin/") || file.StartsWith("bin/netstandard/")) &&
			!Regex.IsMatch(file, "(?:^|/)(?<!(?:^|/)bin_dotnet/)web.config", RegexOptions.IgnoreCase) &&
			!file.EndsWith(".aspx") && !file.EndsWith(".asax") && !file.EndsWith(".asmx")) ? file : null;
	}

	public override void InstallNet8Runtime()
	{
		if (Net8RuntimeNeededOnWindows)
		{
			if (CheckNet8RuntimeInstalled()) return;

			var ver = OSInfo.WindowsVersion;
			if (!(OSInfo.IsWindowsServer && ver >= WindowsVersion.WindowsServer2012 ||
				!OSInfo.IsWindowsServer && ver >= WindowsVersion.Windows10))
				throw new PlatformNotSupportedException("NET 8 is not supported on this OS.");

			WinGet.Install("Microsoft.DotNet.AspNetCore.8;Microsoft.DotNet.Runtime.8");

			InstallLog("Installed .NET 8 Runtime.");

			ResetHasDotnet();
		}
	}

	public override void RemoveNet8AspRuntime()
	{
		WinGet.Remove("Microsoft.DotNet.AspNetCore.8");

		ResetHasDotnet();
	}
	public override void RemoveNet8NetRuntime()
	{
		WinGet.Remove("Microsoft.DotNet.Runtime.8");

		ResetHasDotnet();
	}

	private static List<string> GetInstalledNetFX1To45VersionFromRegistry()
	{
		var list = new List<string>();
		// Opens the registry key for the .NET Framework entry.
		using (RegistryKey ndpKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
		{
			foreach (string versionKeyName in ndpKey.GetSubKeyNames())
			{
				// Skip .NET Framework 4.5 version information.
				if (versionKeyName == "v4")
				{
					continue;
				}

				if (versionKeyName.StartsWith("v"))
				{

					RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
					// Get the .NET Framework version value.
					string name = (string)versionKey.GetValue("Version", "");
					// Get the service pack (SP) number.
					string sp = versionKey.GetValue("SP", "").ToString();

					// Get the installation flag, or an empty string if there is none.
					string install = versionKey.GetValue("Install", "").ToString();
					if (string.IsNullOrEmpty(install)) // No install info; it must be in a child subkey.
						list.Add(name);
					else
					{
						if (!(string.IsNullOrEmpty(sp)) && install == "1")
						{
							list.Add(name);
						}
					}
					if (!string.IsNullOrEmpty(name))
					{
						continue;
					}
					foreach (string subKeyName in versionKey.GetSubKeyNames())
					{
						RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
						name = (string)subKey.GetValue("Version", "");
						if (!string.IsNullOrEmpty(name))
							sp = subKey.GetValue("SP", "").ToString();

						install = subKey.GetValue("Install", "").ToString();
						if (string.IsNullOrEmpty(install)) //No install info; it must be later.
							list.Add(name);
						else
						{
							if (!(string.IsNullOrEmpty(sp)) && install == "1")
							{
								list.Add(name);
							}
							else if (install == "1")
							{
								list.Add(name);
							}
						}
					}
				}
			}
		}
		return list;
	}
	private static List<string> GetInstalledNetFX45PlusFromRegistry()
	{
		var list = new List<string>();

		const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

		using (RegistryKey ndpKey = Registry.LocalMachine.OpenSubKey(subkey))
		{
			if (ndpKey == null)
				return list;
			//First check if there's an specific version indicated
			if (ndpKey.GetValue("Version") != null)
			{
				list.Add(ndpKey.GetValue("Version").ToString());
			}
			else
			{
				if (ndpKey != null && ndpKey.GetValue("Release") != null)
				{
					list.Add(CheckFor45PlusVersion((int)ndpKey.GetValue("Release")));
				}
			}
			return list;
		}

		// Checking the version using >= enables forward compatibility.
		string CheckFor45PlusVersion(int releaseKey)
		{
			if (releaseKey >= 533320)
				return "4.8.1";
			if (releaseKey >= 528040)
				return "4.8";
			if (releaseKey >= 461808)
				return "4.7.2";
			if (releaseKey >= 461308)
				return "4.7.1";
			if (releaseKey >= 460798)
				return "4.7";
			if (releaseKey >= 394802)
				return "4.6.2";
			if (releaseKey >= 394254)
				return "4.6.1";
			if (releaseKey >= 393295)
				return "4.6";
			if (releaseKey >= 379893)
				return "4.5.2";
			if (releaseKey >= 378675)
				return "4.5.1";
			if (releaseKey >= 378389)
				return "4.5";
			// This code should never execute. A non-null release key should mean
			// that 4.5 or later is installed.
			return "";
		}
	}

	public virtual bool IsNet48Installed
	{
		get
		{
			if (OSInfo.IsWindows)
			{
				var versions = GetInstalledNetFX45PlusFromRegistry()
					.Select(ver =>
					{
						Version version = default;
						System.Version.TryParse(ver, out version);
						return version;
					});

				return versions.Any(ver => ver >= new System.Version(4, 8));
			}
			else return false;
		}
	}
	public void InstallNet48()
	{
		if (!IsNet48Installed)
		{
			Log.WriteLine("Installing NET Framework 4.8");

			try
			{
				var file = DownloadFile("https://dotnet.microsoft.com/en-us/download/dotnet-framework/thank-you/net48-web-installer");
				if (file != null)
				{
					Shell.Exec($"\"{file}\" /q");
				}

				InstallLog("Installed .NET Framework 4.8.");
			}
			catch (Exception ex) { }
		}
	}

	public override void InstallServerPrerequisites()
	{
		InstallNet8Runtime();
		InstallNet48();
	}

	public override void InstallServerWebsite()
	{
		var websitePath = Path.Combine(InstallWebRootPath, ServerFolder);
		InstallWebsite($"{SolidCP}Server", websitePath,
			Settings.Server.Urls ?? "",
			Settings.Server.Username ?? $"{SolidCP}Server",
			Settings.Server.Password ?? "");

		InstallLog("Installed Server website, listening on the url(s):" +
			$"{string.Join(NewLine, Settings.Server.Urls.Split(',', ';')
				.Select(url => "  " + url))}");
	}

	public override bool IsRunningAsAdmin
		=> new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

	public override void ShowLogFile()
	{
		try
		{
			var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Log.File);
			Shell.Standard.Exec($"notepad.exe \"{path}\"");
		}
		catch { }
	}

	public override bool CheckOSSupported() => OSInfo.WindowsVersion >= WindowsVersion.Windows7;

	public override bool CheckIISVersionSupported() => CheckOSSupported();

	public override bool CheckSystemdSupported() => false;

	public override bool CheckNetVersionSupported() => OSInfo.IsNet48 || OSInfo.IsCore && int.Parse(Regex.Match(OSInfo.FrameworkDescription, "[0-9]+").Value) >= 8;

	public override void RestartAsAdmin()
	{
		if (RunAsAdmin)
		{
			//var currentp = Process.GetCurrentProcess();
			ProcessStartInfo procInfo = new ProcessStartInfo();
			procInfo.UseShellExecute = true;
			var assemblyFile = Assembly.GetEntryAssembly().Location;
			if (OSInfo.IsMono) procInfo.FileName = "mono";
			else if (assemblyFile.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)) procInfo.FileName = assemblyFile;
			else if (OSInfo.IsCore) procInfo.FileName = "dotnet";
			procInfo.WorkingDirectory = Environment.CurrentDirectory;
			procInfo.Arguments = string.Join(" ", Environment.GetCommandLineArgs()
				.Select(arg => arg.Contains(' ') ? $"\"{arg}\"" : arg));
			procInfo.Verb = "runas";
			try
			{
				var p = Process.Start(procInfo);
				p.WaitForExit();
				Environment.Exit(p.ExitCode);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.Message);
				Console.Read();
				Environment.Exit(-1);
			}
		}
	}

	public override string GetUninstallLog(ComponentSettings settings)
	{
		switch (settings.ComponentCode)
		{
			case Global.Server.ComponentCode:
				return
@"- Remove SolidCP Server website
- Delete SolidCP Server folder.
- Remove firewall rule.";
			case Global.EntServer.ComponentCode:
				return
@"- Remove SolidCP EnterpriseServer website.
- Delete SolidCP EnterpriseServer folder.
- Remove SolidCP Database.
- Remove firewall rule.";
			case Global.WebPortal.ComponentCode:
				return
@"- Remove SolidCP WebPortal website.
- Delete SolidCP WebPortal folder.
- Remove firewall rule.";
			case Global.WebDavPortal.ComponentCode:
				return
@"- Remove SolidCP EnterpriseServer website.
- Delete SolidCP EnterpriseServer folder.
- Remove firewall rule.";
			case Global.StandaloneServer.ComponentCode:
				return
@"- Remove SolidCP WebPortal website.
- Delete SolidCP WebPortal, EnterpriseServer & Server folder.
- Remove firewall rule.";
			default: throw new NotSupportedException();
		}
	}
}