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
using System.Windows.Forms;
using System.Threading;
using System.DirectoryServices;
using SolidCP.Setup.Web;
using System.IO;
using System.Management;
using SolidCP.Setup.Actions;
using Microsoft.Win32;

namespace SolidCP.Setup
{
	public partial class ConfigurationCheckPage : BannerWizardPage
	{
		public const string AspNet40HasBeenInstalledMessage = "ASP.NET 4.0 has been installed.";

		private Thread thread;
		private List<ConfigurationCheck> checks;

		public ConfigurationCheckPage()
		{
			InitializeComponent();
			checks = new List<ConfigurationCheck>();
			this.CustomCancelHandler = true;
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
						case CheckTypes.OperationSystem:
							status = CheckOS(check.SetupVariables, out details);
							break;
						case CheckTypes.IISVersion:
							status = CheckIISVersion(check.SetupVariables, out details);
							break;
						case CheckTypes.ASPNET:
							status = CheckASPNET(check.SetupVariables, out details);
							break;
						case CheckTypes.SCPServer:
							status = CheckSCPServer(check.SetupVariables, out details);
							break;
						case CheckTypes.SCPEnterpriseServer:
							status = CheckSCPEnterpriseServer(check.SetupVariables, out details);
							break;
						case CheckTypes.SCPPortal:
							status = CheckSCPPortal(check.SetupVariables, out details);
							break;
                        case CheckTypes.SCPWebDavPortal:
                            status = CheckSCPWebDavPortal(check.SetupVariables, out details);
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
					if (!string.IsNullOrEmpty(Wizard.SetupVariables.SetupXml) && AllowMoveNext)
						Wizard.GoNext();
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
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

		internal static CheckStatuses CheckOS(SetupVariables setupVariables, out string details)
		{
			details = string.Empty;
			try
			{
				//check OS version
				OS.WindowsVersion version = OS.GetVersion();
				details = OS.GetName(version);
				if (Utils.IsWin64())
					details += " x64";
				Log.WriteInfo(string.Format("OS check: {0}", details));

				if (!(version == OS.WindowsVersion.WindowsServer2003 ||
                    version == OS.WindowsVersion.WindowsServer2008 ||
                    version == OS.WindowsVersion.WindowsServer2008R2 ||
                    version == OS.WindowsVersion.WindowsServer2012 ||
                    version == OS.WindowsVersion.WindowsServer2012R2 ||
                    version == OS.WindowsVersion.WindowsServer2016 ||
                    version == OS.WindowsVersion.WindowsVista ||
                    version == OS.WindowsVersion.Windows7 ||
                    version == OS.WindowsVersion.Windows8 ||
                    version == OS.WindowsVersion.Windows10))
				{
					details = "OS required: Windows Server 2008/2008 R2/2012 or Windows Vista/7/8.";
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
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Check error", ex);
				details = "Unexpected error";
				return CheckStatuses.Error;
			}
		}
        internal static CheckStatuses CheckIISVersion(SetupVariables setupVariables, out string details)
		{
			details = string.Empty;
			try
			{
                details = string.Format("IIS {0}", setupVariables.IISVersion.ToString(2));
                if (setupVariables.IISVersion.Major == 6 &&
					Utils.IsWin64() && Utils.IIS32Enabled())
				{
					details += " (32-bit mode)";
				}

				Log.WriteInfo(string.Format("IIS check: {0}", details));
                if (setupVariables.IISVersion.Major < 6)
				{
					details = "IIS 6.0 or greater required.";
					Log.WriteError(string.Format("IIS check: {0}", details), null);
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

		internal static CheckStatuses CheckASPNET(SetupVariables setupVariables, out string details)
		{
			details = "ASP.NET 4.0 is installed.";
			CheckStatuses ret = CheckStatuses.Success;
			try
			{
				// IIS 6
                if (setupVariables.IISVersion.Major == 6)
				{
					//
                    if (Utils.CheckAspNet40Registered(setupVariables) == false)
					{
						// Register ASP.NET 4.0
                        Utils.RegisterAspNet40(setupVariables);
						//
						ret = CheckStatuses.Warning;
						details = AspNet40HasBeenInstalledMessage;
					}
					// Enable ASP.NET 4.0 Web Server Extension if it is prohibited
                    if (Utils.GetAspNetWebExtensionStatus_Iis6(setupVariables) == WebExtensionStatus.Prohibited)
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
                    if (Utils.CheckAspNet40Registered(setupVariables) == false)
					{
						// Register ASP.NET 4.0
                        Utils.RegisterAspNet40(setupVariables);
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
				if (!Utils.IsThreadAbortException(ex))
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

        internal static CheckStatuses CheckIIS32Mode(SetupVariables setupVariables, out string details)
		{
			details = string.Empty;
			CheckStatuses ret = CheckIISVersion(setupVariables, out details);
			if (ret == CheckStatuses.Error)
				return ret;

			try
			{
				//IIS 6
				if (setupVariables.IISVersion.Major == 6)
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

		private bool SiteBindingsExist(SetupVariables setupVariables)
		{
            bool iis7 = (setupVariables.IISVersion.Major >= 7);
			string ip = setupVariables.WebSiteIP;
			string port = setupVariables.WebSitePort;
			string domain = setupVariables.WebSiteDomain;

			string siteId = iis7 ?
				WebUtils.GetIIS7SiteIdByBinding(ip, port, domain) :
				WebUtils.GetSiteIdByBinding(ip, port, domain);
			return (siteId != null);
		}

		private bool AccountExists(SetupVariables setupVariables)
		{
			string domain = setupVariables.UserDomain;
			string username = setupVariables.UserAccount;
			return SecurityUtils.UserExists(domain, username);
		}

		private CheckStatuses CheckSCPServer(SetupVariables setupVariables, out string details)
		{
			details = "";
			try
			{
				if (SiteBindingsExist(setupVariables))
				{
					details = string.Format("Site with specified bindings already exists (ip: {0}, port: {1}, domain: {2})",
							setupVariables.WebSiteIP, setupVariables.WebSitePort, setupVariables.WebSiteDomain);
					Log.WriteError(string.Format("Site bindings check: {0}", details), null);
					return CheckStatuses.Error;
				}

				if (AccountExists(setupVariables))
				{
					details = string.Format("Windows account already exists: {0}\\{1}",
							   setupVariables.UserDomain, setupVariables.UserAccount);
					Log.WriteError(string.Format("Account check: {0}", details), null);
					return CheckStatuses.Error;
				}

				if (!CheckDiskSpace(setupVariables, out details))
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

		private bool CheckDiskSpace(SetupVariables setupVariables, out string details)
		{
			details = string.Empty;

			long spaceRequired = FileUtils.CalculateFolderSize(setupVariables.InstallerFolder);

			if (string.IsNullOrEmpty(setupVariables.InstallationFolder))
			{
				details = "Installation folder is not specified.";
				return false;
			}
			string drive = null;
			try
			{
				drive = Path.GetPathRoot(Path.GetFullPath(setupVariables.InstallationFolder));
			}
			catch
			{
				details = "Installation folder is invalid.";
				return false;
			}

			ulong freeBytesAvailable, totalBytes, freeBytes;
			if (FileUtils.GetDiskFreeSpaceEx(drive, out freeBytesAvailable, out totalBytes, out freeBytes))
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

		private CheckStatuses CheckSCPEnterpriseServer(SetupVariables setupVariables, out string details)
		{
			details = "";
			try
			{
				if (SiteBindingsExist(setupVariables))
				{
					details = string.Format("Site with specified bindings already exists (ip: {0}, port: {1}, domain: {2})",
							setupVariables.WebSiteIP, setupVariables.WebSitePort, setupVariables.WebSiteDomain);
					Log.WriteError(string.Format("Site bindings check: {0}", details), null);
					return CheckStatuses.Error;
				}

				if (AccountExists(setupVariables))
				{
					details = string.Format("Windows account already exists: {0}\\{1}",
							   setupVariables.UserDomain, setupVariables.UserAccount);
					Log.WriteError(string.Format("Account check: {0}", details), null);
					return CheckStatuses.Error;
				}

				if (!CheckDiskSpace(setupVariables, out details))
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

		private CheckStatuses CheckSCPPortal(SetupVariables setupVariables, out string details)
		{
			details = "";
			try
			{
				if (AccountExists(setupVariables))
				{
					details = string.Format("Windows account already exists: {0}\\{1}",
							   setupVariables.UserDomain, setupVariables.UserAccount);
					Log.WriteError(string.Format("Account check: {0}", details), null);
					return CheckStatuses.Error;
				}

				if (!CheckDiskSpace(setupVariables, out details))
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


        private CheckStatuses CheckSCPWebDavPortal(SetupVariables setupVariables, out string details)
        {
            details = "";
            try
            {
                if (AccountExists(setupVariables))
                {
                    details = string.Format("Windows account already exists: {0}\\{1}",
                               setupVariables.UserDomain, setupVariables.UserAccount);
                    Log.WriteError(string.Format("Account check: {0}", details), null);
                    return CheckStatuses.Error;
                }

                if (!CheckDiskSpace(setupVariables, out details))
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

			string path = Path.Combine(OS.GetWindowsDirectory(), util);
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
