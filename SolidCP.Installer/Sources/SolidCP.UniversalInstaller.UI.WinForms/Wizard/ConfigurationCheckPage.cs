// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.DirectoryServices;
using System.IO;
using System.Management;
using SolidCP.UniversalInstaller;
using Microsoft.Win32;
using SolidCP.Providers.OS;
using SolidCP.UniversalInstaller.Web;

namespace SolidCP.UniversalInstaller.WinForms
{
	public partial class ConfigurationCheckPage : BannerWizardPage
	{
		public const string AspNet40HasBeenInstalledMessage = "ASP.NET 4.0 has been installed.";

		private Thread thread;
		private List<ConfigurationCheck> checks;

		private WebUtils WebUtils => Installer.Current.WebUtils;
		private SecurityUtils SecurityUtils => Installer.Current.SecurityUtils;

		public ConfigurationCheckPage()
		{
			InitializeComponent();
			checks = new List<ConfigurationCheck>();
			this.CustomCancelHandler = true;
			if (OSInfo.IsWindows)
			{
				checks.AddRange(new ConfigurationCheck[]
				{
					new ConfigurationCheck(CheckTypes.WindowsOperatingSystem, "Operating System Requirement"),
					new ConfigurationCheck(CheckTypes.IISVersion, "IIS Requirement"),
					new ConfigurationCheck(CheckTypes.ASPNET, "ASP.NET Requirement")
				});
			} else
			{
				checks.AddRange(new ConfigurationCheck[]
				{
					new ConfigurationCheck(CheckTypes.OperatingSystem, "Operating System Requirement"),
					new ConfigurationCheck(CheckTypes.Net8Runtime, ".NET 8 Runtime Requirement"),
					new ConfigurationCheck(CheckTypes.Systemd, "Systemd Requirement")
				});
			}

		}

		public List<ConfigurationCheck> Checks
		{
			get
			{
				return checks;
			}
		}

		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();
			this.Text = "System Configuration Check";
			this.Description = "Wait while the system is checked for potential installation problems.";

			this.imgError.Visible = false;
			this.imgOk.Visible = false;
			this.lblResult.Visible = false;
		}

		protected internal override void OnBeforeDisplay(EventArgs e)
		{
			base.OnBeforeDisplay(e);

			this.AllowMoveBack = false;
			this.AllowMoveNext = false;


		}

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			thread = new Thread(new ThreadStart(this.Start));
			thread.Start();
		}

		/// <summary>
		/// Displays process progress.
		/// </summary>
		public void Start()
		{
			bool pass = true;
			try
			{

				lvCheck.Items.Clear();
				this.imgError.Visible = false;
				this.imgOk.Visible = false;
				this.lblResult.Visible = false;

				foreach (ConfigurationCheck check in Checks)
				{
					AddListViewItem(check);
				}
				this.Update();
				CheckStatuses status = CheckStatuses.Success;
				string details = string.Empty;
				//
				foreach (ListViewItem item in lvCheck.Items)
				{
					ConfigurationCheck check = (ConfigurationCheck)item.Tag;
					item.ImageIndex = 0;
					item.SubItems[2].Text = "Running";
					this.Update();

					#region Previous Prereq Verification
					switch (check.CheckType)
					{
						case CheckTypes.OperatingSystem:
							status = CheckOS(out details);
							break;
						case CheckTypes.WindowsOperatingSystem:
							status = CheckWindowsOS(out details);
							break;
						case CheckTypes.IISVersion:
							status = CheckIISVersion(out details);
							break;
						case CheckTypes.ASPNET:
							status = CheckASPNET(out details);
							break;
						case CheckTypes.SCPServer:
							status = CheckSCPServer(out details);
							break;
						case CheckTypes.SCPEnterpriseServer:
							status = CheckSCPEnterpriseServer(out details);
							break;
						case CheckTypes.SCPPortal:
							status = CheckSCPPortal(out details);
							break;
                        case CheckTypes.SCPWebDavPortal:
                            status = CheckSCPWebDavPortal(out details);
                            break;
						case CheckTypes.Net8Runtime:
							status = CheckNet8Runtime(out details);
							break;
						case CheckTypes.ApacheVersion:
							status = CheckApacheVersion(out details);
							break;
						case CheckTypes.Systemd:
							status = CheckSystemd(out details);
							break;
						default:
							status = CheckStatuses.Warning;
							break;
					}

					#endregion

					switch (status)
					{
						case CheckStatuses.Success:
							item.ImageIndex = 1;
							item.SubItems[2].Text = "Success";
							break;
						case CheckStatuses.Warning:
							item.ImageIndex = 2;
							item.SubItems[2].Text = "Warning";
							break;
						case CheckStatuses.Error:
							item.ImageIndex = 3;
							item.SubItems[2].Text = "Error";
							pass = false;
							break;
					}
					item.SubItems[3].Text = details;
					this.Update();
				}
				//
				//actionManager.PrerequisiteComplete += new EventHandler<ActionProgressEventArgs<bool>>((object sender, ActionProgressEventArgs<bool> e) =>
				//{
					//
				//});
				//
				//actionManager.VerifyDistributivePrerequisites();

				ShowResult(pass);
				if (pass)
				{
					//unattended setup
					if (Installer.Current.Settings.Installer.IsUnattended && AllowMoveNext)
						Wizard.GoNext();
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException)
					return;

				ShowError();
				return;
			}
		}



		private void ShowResult(bool success)
		{
			this.AllowMoveNext = success;
			this.imgError.Visible = !success;
			this.imgOk.Visible = success;

			this.lblResult.Text = success ? "Success" : "Error";
			this.lblResult.Visible = true;
			Update();
		}

		private void AddListViewItem(ConfigurationCheck check)
		{
			lvCheck.BeginUpdate();
			ListViewItem item = new ListViewItem(string.Empty);
			item.SubItems.AddRange(new string[] { check.Action, string.Empty, string.Empty });
			item.Tag = check;
			lvCheck.Items.Add(item);
			lvCheck.EndUpdate();
			Update();

		}

		internal static CheckStatuses CheckOS(out string details)
		{
			details = string.Empty;
			try
			{
				//check OS version
				WindowsVersion version = OSInfo.WindowsVersion;
				details = OSInfo.Description;
				if (Utils.IsWin64())
					details += " x64";
				Log.WriteInfo(string.Format("OS check: {0}", details));

				if (!(/*version == WindowsVersion.WindowsServer2008 ||*/
					version == WindowsVersion.WindowsServer2008R2 ||
					version == WindowsVersion.WindowsServer2012 ||
					version == WindowsVersion.WindowsServer2012R2 ||
					version == WindowsVersion.WindowsServer2016 ||
					version == WindowsVersion.WindowsServer2019 ||
					version == WindowsVersion.WindowsServer2022 ||
					version == WindowsVersion.WindowsServer2025 ||
					/* version == WindowsVersion.Vista || */
					version == WindowsVersion.Windows7 ||
					version == WindowsVersion.Windows8 ||
					version == WindowsVersion.Windows81 ||
					version == WindowsVersion.Windows10 ||
					version == WindowsVersion.Windows11 ||
					version == WindowsVersion.NonWindows))
				{
					details = "OS required: 2008 R2/2012/2016/2019/2022/2025 or Windows 7/8/10/11.";
					Log.WriteError(string.Format("OS check: {0}", details), null);
#if DEBUG
					return CheckStatuses.Warning;
#endif
#if !DEBUG
					return CheckStatuses.Error;
#endif
				}
				return CheckStatuses.Success;
			}
			catch (Exception ex)
			{
				if (!(ex is ThreadAbortException))
					Log.WriteError("Check error", ex);
				details = "Unexpected error";
				return CheckStatuses.Error;
			}
		}
		internal static CheckStatuses CheckWindowsOS(out string details)
		{
			details = string.Empty;
			try
			{
				//check OS version
				WindowsVersion version = OSInfo.WindowsVersion;
				details = OSInfo.Description;
				if (Utils.IsWin64())
					details += " x64";
				Log.WriteInfo(string.Format("OS check: {0}", details));

				if (!(/*version == WindowsVersion.WindowsServer2008 || */
					version == WindowsVersion.WindowsServer2008R2 ||
					version == WindowsVersion.WindowsServer2012 ||
					version == WindowsVersion.WindowsServer2012R2 ||
					version == WindowsVersion.WindowsServer2016 ||
					version == WindowsVersion.WindowsServer2019 ||
					version == WindowsVersion.WindowsServer2022 ||
					version == WindowsVersion.WindowsServer2025 ||
					/* version == WindowsVersion.Vista || */
					version == WindowsVersion.Windows7 ||
					version == WindowsVersion.Windows8 ||
					version == WindowsVersion.Windows81 ||
					version == WindowsVersion.Windows10 ||
					version == WindowsVersion.Windows11))
				{
					details = "OS required: Windows Server 2008 R2/2012/2016/2019/2022/2025 or Windows 7/8/10/11.";
					Log.WriteError(string.Format("OS check: {0}", details), null);
#if DEBUG
					return CheckStatuses.Warning;
#endif
#if !DEBUG
					return CheckStatuses.Error;
#endif
				}
				return CheckStatuses.Success;
			}
			catch (Exception ex)
			{
				if (!(ex is ThreadAbortException))
					Log.WriteError("Check error", ex);
				details = "Unexpected error";
				return CheckStatuses.Error;
			}
		}

		internal static CheckStatuses CheckIISVersion(out string details)
		{
			details = string.Empty;
			try
			{
				var version = OSInfo.Windows.WebServer.Version;
				details = string.Format("IIS {0}", version.ToString(2));
                if (version.Major == 6 &&
					Utils.IsWin64() && Utils.IIS32Enabled())
				{
					details += " (32-bit mode)";
				}

				Log.WriteInfo(string.Format("IIS check: {0}", details));
				// require IIS 7
                if (version.Major < 6)
				{
					details = "IIS 6.0 or greater required.";
					Log.WriteError(string.Format("IIS check: {0}", details), null);
					return CheckStatuses.Error;
				}

				return CheckStatuses.Success;
			}
			catch (Exception ex)
			{
				if (!(ex is ThreadAbortException))
					Log.WriteError("Check error", ex);
				details = "Unexpected error";
				return CheckStatuses.Error;
			}
		}

		internal static CheckStatuses CheckApacheVersion(out string details)
		{
			if (OSInfo.Current.WebServer.GetType().FullName.Contains("Apache"))
			{
				var version = OSInfo.Current.WebServer.Version;
				details = $"Apache {version} is installed.";
				return CheckStatuses.Success;
			} else
			{
				details = "Apache not found";
				return CheckStatuses.Error;
			}
		}
		internal static CheckStatuses CheckSystemd(out string details)
		{
			details = "Systemd is installed.";

			if (OSInfo.Current.ServiceController != null &&
				OSInfo.Current.ServiceController.IsInstalled &&
				OSInfo.Current.ServiceController is SystemdServiceController) return CheckStatuses.Success;

			details = "Systemd not found.";
			return CheckStatuses.Error;
		}
		internal static CheckStatuses CheckNet8Runtime(out string details)
		{
			details = ".NET 8 Runtime is installed.";
			CheckStatuses ret = CheckStatuses.Success;
			if (!UniversalInstaller.Installer.Current.CheckNet8RuntimeInstalled())
			{
				details = ".NET 8 Runtime not installed.";
				ret = CheckStatuses.Warning;
			}
			return ret;
		}
		internal static CheckStatuses CheckASPNET(out string details)
		{
			details = "ASP.NET 4.0 is installed.";
			CheckStatuses ret = CheckStatuses.Success;
			try
			{
				// IIS 6
                if (OSInfo.IsWindows && OSInfo.Current.WebServer.Version.Major == 6)
				{
					//
                    if (Utils.CheckAspNet40Registered() == false)
					{
						// Register ASP.NET 4.0
                        Utils.RegisterAspNet40();
						//
						ret = CheckStatuses.Warning;
						details = AspNet40HasBeenInstalledMessage;
					}
					// Enable ASP.NET 4.0 Web Server Extension if it is prohibited
                    if (Utils.GetAspNetWebExtensionStatus_Iis6() == WebExtensionStatus.Prohibited)
					{
						Utils.EnableAspNetWebExtension_Iis6();
					}
				}
				// IIS 7 on Windows 2008 and higher
				else
				{
					if (!IsWebServerRoleInstalled())
					{
						details = "Web Server (IIS) role is not installed on your server. Run Server Manager to add Web Server (IIS) role.";
						Log.WriteInfo(string.Format("ASP.NET check: {0}", details));
						return CheckStatuses.Error;
					}
                    if (!IsAspNetRoleServiceInstalled())
                    {
                        details = "ASP.NET role service is not installed on your server. Run Server Manager to add ASP.NET role service.";
                        Log.WriteInfo(string.Format("ASP.NET check: {0}", details));
                        return CheckStatuses.Error;
                    }
					// Register ASP.NET 4.0
                    if (Utils.CheckAspNet40Registered() == false)
					{
						// Register ASP.NET 4.0
                        Utils.RegisterAspNet40();
						//
						ret = CheckStatuses.Warning;
						details = AspNet40HasBeenInstalledMessage;
					}
				}
				// Log details
				Log.WriteInfo(string.Format("ASP.NET check: {0}", details));
				//
				return ret;
			}
			catch (Exception ex)
			{
				if (!(ex is ThreadAbortException))
					Log.WriteError("Check error", ex);
				details = "Unexpected error";
#if DEBUG
				return CheckStatuses.Warning;
#endif
#if !DEBUG
				return CheckStatuses.Error;
#endif
			}
		}

        internal static CheckStatuses CheckIIS32Mode(out string details)
		{
			details = string.Empty;
			CheckStatuses ret = CheckIISVersion(out details);
			if (ret == CheckStatuses.Error)
				return ret;

			try
			{
				//IIS 6
				if (OSInfo.IsWindows && OSInfo.Windows.WebServer.Version.Major == 6)
				{
					//x64
					if (Utils.IsWin64())
					{
						if (!Utils.IIS32Enabled())
						{
							Log.WriteInfo("IIS 32-bit mode disabled");
							EnableIIS32Mode();
							details = "IIS 32-bit mode has been enabled.";
							Log.WriteInfo(string.Format("IIS 32-bit mode check: {0}", details));
							return CheckStatuses.Warning;
						}
					}
				}
				return CheckStatuses.Success;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Check error", ex);
				details = "Unexpected error";
				return CheckStatuses.Error;
			}
		}

		private bool SiteBindingsExist(CommonSettings settings, out string ipadr, out string port, out string domain)
		{
			ipadr = port = domain = "";
			if (!OSInfo.IsWindows) return false;

            bool iis7 = OSInfo.Windows.WebServer.Version.Major >= 7;

			System.Net.IPAddress ip;
			var bindings = settings.Urls.Split(',',';')
				.Select(url => new Uri(url))
				.Select(uri => new
				{
					Ip = System.Net.IPAddress.TryParse(uri.Host, out ip) ? uri.Host : "0.0.0.0",
					Port = uri.Port.ToString(),
					Domain = System.Net.IPAddress.TryParse(uri.Host, out ip) ? "" : uri.Host
				});

			foreach (var binding in bindings)
			{
				if ((iis7 ?
					WebUtils.GetIIS7SiteIdByBinding(binding.Ip, binding.Port, binding.Domain) :
					WebUtils.GetSiteIdByBinding(binding.Ip, binding.Port, binding.Domain)) != null) {
					ipadr = binding.Ip;
					port = binding.Port;
					domain = binding.Domain;
					return true;
				}
			}
			return false;
		}

		private bool AccountExists(CommonSettings settings, out string domain, out string username)
		{
			username = settings.Username;
			var backslash = settings.Username.IndexOf('\\');
			if (backslash >= 0)
			{
				domain = username.Substring(0, backslash);
				username = (backslash + 1 < username.Length) ? username.Substring(backslash + 1) : "";
			} else
			{
				domain = Environment.MachineName;
			}
			return SecurityUtils.UserExists(domain, username);
		}

		private CheckStatuses CheckSCPServer(out string details)
		{
			details = "";
			string ip, port, domain;
			var settings = Installer.Current.Settings.Server;

			if (OSInfo.IsWindows)
			{
				try
				{
					if (SiteBindingsExist(settings, out ip, out port, out domain))
					{
						details = string.Format("Site with specified bindings already exists (ip: {0}, port: {1}, domain: {2})", ip, port, domain);
						Log.WriteError(string.Format("Site bindings check: {0}", details), null);
						return CheckStatuses.Error;
					}

					string username;
					if (AccountExists(settings, out domain, out username))
					{
						details = string.Format("Windows account already exists: {0}\\{1}", domain, username);
						Log.WriteError(string.Format("Account check: {0}", details), null);
						return CheckStatuses.Error;
					}

					if (!CheckDiskSpace(settings, out details))
					{
						Log.WriteError(string.Format("Disk space check: {0}", details), null);
						return CheckStatuses.Error;
					}

					return CheckStatuses.Success;
				}
				catch (Exception ex)
				{
					if (!Utils.IsThreadAbortException(ex))
						Log.WriteError("Check error", ex);
					details = "Unexpected error";
					return CheckStatuses.Error;
				}
			}
			else
			{
				return CheckStatuses.Success;
			}
		}

		private bool CheckDiskSpace(ComponentSettings settings, out string details)
		{
			details = string.Empty;

			if (string.IsNullOrEmpty(Installer.Current.ComponentTempPath))
			{
				details = "Installation folder is not specified.";
				return false;
			}

			long spaceRequired = FileUtils.CalculateFolderSize(Installer.Current.ComponentTempPath);
			
			string drive = null;
			try
			{
				drive = Path.GetPathRoot(Path.GetFullPath(settings.InstallFolder));
			}
			catch
			{
				details = "Installation folder is invalid.";
				return false;
			}

			ulong freeBytesAvailable, totalBytes, freeBytes;
			if (FileUtils.GetDiskFreeSpace(drive, out freeBytesAvailable, out totalBytes, out freeBytes))
			{
				long freeSpace = Convert.ToInt64(freeBytesAvailable);
				if (spaceRequired > freeSpace)
				{
					details = string.Format("There is not enough space on the disk ({0} required, {1} available)",
						FileUtils.SizeToMB(spaceRequired), FileUtils.SizeToMB(freeSpace));
					return false;
				}
			}
			else
			{
				details = "I/O error";
				return false;
			}
			return true;
		}

		private CheckStatuses CheckSCPEnterpriseServer(out string details)
		{
			var settings = Installer.Current.Settings.EnterpriseServer;

			string ip, port, domain, username;
			details = "";
			try
			{
				if (SiteBindingsExist(settings, out ip, out port, out domain))
				{
					details = string.Format("Site with specified bindings already exists (ip: {0}, port: {1}, domain: {2})", ip, port, domain);
					Log.WriteError(string.Format("Site bindings check: {0}", details), null);
					return CheckStatuses.Error;
				}

				if (AccountExists(settings, out domain, out username))
				{
					details = string.Format("User account already exists: {0}\\{1}", domain, username);
					Log.WriteError(string.Format("Account check: {0}", details), null);
					return CheckStatuses.Error;
				}

				if (!CheckDiskSpace(settings, out details))
				{
					Log.WriteError(string.Format("Disk space check: {0}", details), null);
					return CheckStatuses.Error;
				}

				return CheckStatuses.Success;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Check error", ex);
				details = "Unexpected error";
				return CheckStatuses.Error;
			}
		}

		private CheckStatuses CheckSCPPortal(out string details)
		{
			var settings = Installer.Current.Settings.WebPortal;

			string ip, port, domain, username;

			details = "";
			try
			{
				if (AccountExists(settings, out domain, out username))
				{
					details = string.Format("Windows account already exists: {0}\\{1}", domain, username);
					Log.WriteError(string.Format("Account check: {0}", details), null);
					return CheckStatuses.Error;
				}

				if (!CheckDiskSpace(settings, out details))
				{
					Log.WriteError(string.Format("Disk space check: {0}", details), null);
					return CheckStatuses.Error;
				}

				return CheckStatuses.Success;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Check error", ex);
				details = "Unexpected error";
				return CheckStatuses.Error;
			}
		}


        private CheckStatuses CheckSCPWebDavPortal(out string details)
        {
			if (!OSInfo.IsWindows)
			{
				details = "Portal can only be installed on Windows";
				return CheckStatuses.Error;
			}

			var settings = Installer.Current.Settings.WebDavPortal;
			string domain, username;
			details = "";
            try
            {
                if (AccountExists(settings, out domain, out username))
                {
                    details = string.Format("Windows account already exists: {0}\\{1}", domain, username);
                    Log.WriteError(string.Format("Account check: {0}", details), null);
                    return CheckStatuses.Error;
                }

                if (!CheckDiskSpace(settings, out details))
                {
                    Log.WriteError(string.Format("Disk space check: {0}", details), null);
                    return CheckStatuses.Error;
                }

                return CheckStatuses.Success;
            }
            catch (Exception ex)
            {
                if (!Utils.IsThreadAbortException(ex))
                    Log.WriteError("Check error", ex);
                details = "Unexpected error";
                return CheckStatuses.Error;
            }
        }

        private static void EnableIIS32Mode()
		{
			Log.WriteStart("Enabling IIS 32-bit mode");
			using (DirectoryEntry iisService = new DirectoryEntry("IIS://LocalHost/W3SVC/AppPools"))
			{
				Utils.SetObjectProperty(iisService, "Enable32bitAppOnWin64", true);
				iisService.CommitChanges();
			}
			Log.WriteEnd("Enabled IIS 32-bit mode");
		}

		private static void InstallASPNET()
		{
			Log.WriteStart("Starting aspnet_regiis -i");
			string util = (Utils.IsWin64() && !Utils.IIS32Enabled()) ?
				@"Microsoft.NET\Framework64\v4.0.30319\aspnet_regiis.exe" :
				@"Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe";

			string path = Path.Combine(Utils.GetWindowsDirectory(), util);
			ProcessStartInfo info = new ProcessStartInfo(path, "-i");
			info.WindowStyle = ProcessWindowStyle.Minimized;
			Process process = Process.Start(info);
			process.WaitForExit();
			Log.WriteEnd("Finished aspnet_regiis -i");
		}

		private static bool IsWebServerRoleInstalled()
		{
            RegistryKey regkey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\InetStp");
            if (regkey == null)
                return false;

            int value = (int)regkey.GetValue("MajorVersion", 0);
            return value == 7 || value == 8 || value == 10;
		}

		private static bool IsAspNetRoleServiceInstalled()
		{
            return GetIsWebFeaturesInstalled();
		}
        private static bool GetIsWebFeaturesInstalled()
        {   // See SolidCP.Installer\Sources\SolidCP.WIXInstaller\Common\Tool.cs
            bool Result = false;
            var LMKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            Result |= CheckAspNetRegValue(LMKey);
            LMKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            Result |= CheckAspNetRegValue(LMKey);
            return Result;
        }
        private static bool CheckAspNetRegValue(RegistryKey BaseKey)
        {
            var WebComponentsKey = "SOFTWARE\\Microsoft\\InetStp\\Components";
            var AspNet = "ASPNET";
            var AspNet45 = "ASPNET45";
            RegistryKey Key = BaseKey.OpenSubKey(WebComponentsKey);
            if (Key == null)
                return false;
            var Value = int.Parse(Key.GetValue(AspNet, 0).ToString());
            if (Value != 1)
                Value = int.Parse(Key.GetValue(AspNet45, 0).ToString());
            return Value == 1;
        }
	}
}
