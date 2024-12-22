using Microsoft.Win32;
using SolidCP.Providers.OS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SolidCP.UniversalInstaller
{
	public class Utils
	{
		public const string AspNet40RegistrationToolx64 = @"Microsoft.NET\Framework64\v4.0.30319\aspnet_regiis.exe";
		public const string AspNet40RegistrationToolx86 = @"Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe";

		#region Windows Firewall
		public static bool IsWindowsFirewallEnabled()
		{
			int ret = RegistryUtils.GetRegistryKeyInt32Value(@"SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile", "EnableFirewall");
			return (ret == 1);
		}
		public static bool IsWindowsFirewallExceptionsAllowed()
		{
			int ret = RegistryUtils.GetRegistryKeyInt32Value(@"SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile", "DoNotAllowExceptions");
			return (ret != 1);
		}
		public static void OpenWindowsFirewallPort(string name, string port)
		{
			string path = Path.Combine(Environment.SystemDirectory, "netsh.exe");
			string arguments = string.Format("firewall set portopening tcp {0} \"{1}\" enable", port, name);
			RunProcess(path, arguments);
		}
		public static void OpenWindowsFirewallPortAdv(string RuleName, string Port)
		{
			string tool = Path.Combine(Environment.SystemDirectory, "netsh.exe");
			string args = string.Format("advfirewall firewall add rule name=\"{0}\" dir=in action=allow protocol=tcp localport={1}", RuleName, Port);
			RunProcess(tool, args);
		}
		#endregion

		#region Processes & Services
		public static int RunProcess(string path, string arguments)
		{
			Process process = null;
			try
			{
				ProcessStartInfo info = new ProcessStartInfo(path, arguments);
				info.WindowStyle = ProcessWindowStyle.Hidden;
				process = Process.Start(info);
				process.WaitForExit();
				return process.ExitCode;
			}
			finally
			{
				if (process != null)
				{
					process.Close();
				}
			}
		}

		public static void StartService(string serviceName)
		{
			OSInfo.Current.ServiceController.Start(serviceName);
		}

		public static void StopService(string serviceName)
		{
			OSInfo.Current.ServiceController.Stop(serviceName);
		}
		#endregion


		#region I/O
		public static string GetSystemDrive()
		{
			return Path.GetPathRoot(Environment.SystemDirectory);
		}
		#endregion

		public static bool IsWebDeployInstalled()
		{
			// TO-DO: Implement Web Deploy detection (x64/x86)
			var isInstalled = false;
			//
			try
			{
				var msdeployRegKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\IIS Extensions\MSDeploy\2");
				//
				var keyValue = msdeployRegKey.GetValue("Install");
				// We have found the required key in the registry hive
				if (keyValue != null && keyValue.Equals(1))
				{
					isInstalled = true;
				}
			}
			catch (Exception ex)
			{
				Log.WriteError("Could not retrieve Web Deploy key from the registry", ex);
			}
			//
			return isInstalled;
		}

		public static bool IsWin64()
		{
			return (IntPtr.Size == 8);
		}

		public static void ShowConsoleErrorMessage(string format, params object[] args)
		{
			Console.WriteLine(String.Format(format, args));
		}

		public static void SetObjectProperty(DirectoryEntry oDE, string name, object value)
		{
			if (value != null)
			{
				if (oDE.Properties.Contains(name))
				{
					oDE.Properties[name][0] = value;
				}
				else
				{
					oDE.Properties[name].Add(value);
				}
			}
		}

		public static object GetObjectProperty(DirectoryEntry entry, string name)
		{
			if (entry.Properties.Contains(name))
				return entry.Properties[name][0];
			else
				return null;
		}

		public static void OpenFirewallPort(string name, string port, Version iisVersion)
		{
			bool iis7 = (iisVersion.Major >= 7);
			if (iis7)
			{
				if (Utils.IsWindowsFirewallEnabled() && Utils.IsWindowsFirewallExceptionsAllowed())
				{
					Log.WriteStart(String.Format("Opening port {0} in windows firewall", port));
					Utils.OpenWindowsFirewallPortAdv(name, port);
					Log.WriteEnd("Opened port in windows firewall");
					Installer.Current.InstallLog(String.Format("Opened port {0} in Windows Firewall", port));
				}
			}
			else
			{
				if (Utils.IsWindowsFirewallEnabled() &&
					Utils.IsWindowsFirewallExceptionsAllowed())
				{
					//SetProgressText("Opening port in windows firewall...");

					Log.WriteStart(String.Format("Opening port {0} in windows firewall", port));

					Utils.OpenWindowsFirewallPort(name, port);

					//update log
					Log.WriteEnd("Opened port in windows firewall");
					Installer.Current.InstallLog(String.Format("Opened port {0} in Windows Firewall", port));
				}
			}
		}

		public static void SaveResource(string resName, string fileName)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var resources = assembly.GetManifestResourceNames();
			var resourceName = resources.FirstOrDefault(name => name.EndsWith(resName));
			if (resourceName != null)
			{
				using (var resStream = assembly.GetManifestResourceStream(resourceName))
				using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
				{
					resStream.CopyTo(file);
				}
			}
		}

		static Dictionary<string, System.Net.IPAddress[]> ResolvedHosts = new Dictionary<string, System.Net.IPAddress[]>();

		public static bool IsLocalAddress(string adr)
		{
			var isHostIP = Regex.IsMatch(adr, @"^[0.9]{1,3}(?:\.[0-9]{1,3}){3}$", RegexOptions.Singleline) || Regex.IsMatch(adr, @"^[0-9a-fA-F:]+$", RegexOptions.Singleline);
			return isHostIP && Regex.IsMatch(adr, @"(^127\.)|(^192\.168\.)|(^10\.)|(^172\.1[6-9]\.)|(^172\.2[0-9]\.)|(^172\.3[0-1]\.)|(^::1$)|(^[fF][cCdD])", RegexOptions.Singleline);
		}

		public static bool IsHostLocal(string host)
		{
			var isHostIP = Regex.IsMatch(host, @"^[0.9]{1,3}(?:\.[0-9]{1,3}){3}$", RegexOptions.Singleline) || Regex.IsMatch(host, @"^[0-9a-fA-F:]+$", RegexOptions.Singleline);
			if (host == "localhost" || host == "127.0.0.1" || host == "::1" ||
				isHostIP && IsLocalAddress(host)) return true;

			if (!isHostIP)
			{
				IPAddress[] ips;
				lock (ResolvedHosts)
				{
					if (!ResolvedHosts.TryGetValue(host, out ips))
					{
						try
						{
							ips = Dns.GetHostEntry(host).AddressList;
							ResolvedHosts.Add(host, ips);
						}
						catch
						{
							return false;
						}
					}
				}
				return ips
					.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork || ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
					.All(ip => IsLocalAddress(ip.ToString()));
			}
			return false;
		}

		public static bool IsGlobalDomain(string domain) => domain != "localhost" && !string.IsNullOrEmpty(domain) && domain.Contains('.');
		public static bool IsHttps(string ip, string domain) => !IsLocalAddress(ip) && IsGlobalDomain(domain) && !IsHostLocal(domain);
		public static bool IsHttpsAndNotWindows(string ip, string domain) => !OSInfo.IsWindows && IsHttps(ip, domain);

		public static string[] GetApplicationUrls(string ip, string domain, string port, string virtualDir)
		{
			List<string> urls = new List<string>();

			// IP address, [port] and [virtualDir]
			string url = ip;
			if (String.IsNullOrEmpty(domain))
			{
				if (!String.IsNullOrEmpty(port) && port != "80")
					url += ":" + port;
				if (!String.IsNullOrEmpty(virtualDir))
					url += "/" + virtualDir;
				urls.Add(url);
			}

			// domain, [port] and [virtualDir]
			if (!String.IsNullOrEmpty(domain))
			{
				url = domain;
				if (!String.IsNullOrEmpty(port) && port != "80")
					url += ":" + port;
				if (!String.IsNullOrEmpty(virtualDir))
					url += "/" + virtualDir;
				urls.Add(url);
			}

			return urls.ToArray();
		}

		public static string GetDistributiveLocationInfo(string ccode, string cversion)
		{
			var service = Installer.Current.InstallerWebService;
			
			var info = service.GetReleaseFileInfo(ccode, cversion);
			
			if (info == null)
			{
				Log.WriteInfo("Component code: {0}; Component version: {1};", ccode, cversion);
				
				throw new ServiceComponentNotFoundException("Seems that the Service has no idea about the component requested.");
			}
			
			return info.FullFilePath;
		}
	}
}
