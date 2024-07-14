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
using System.Configuration.Install;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SolidCP.Providers.OS;
using SolidCP.UniversalInstaller.Core;
using SolidCP.EnterpriseServer.Data;

namespace SolidCP.Setup
{
	public partial class UninstallPage : BannerWizardPage
	{
		private Thread thread;
		private List<InstallAction> actions;

		public UninstallPage()
		{
			InitializeComponent();
			actions = new List<InstallAction>();
		}

		public List<InstallAction> Actions
		{
			get
			{
				return actions;
			}
		}

		protected override void InitializePageInternal()
		{
			string name = Wizard.SetupVariables.ComponentFullName;
			this.Text = string.Format("Uninstalling {0}", name);
			this.Description = string.Format("Please wait while {0} is being uninstalled.", name);
			this.AllowMoveBack = false;
			this.AllowMoveNext = false;
			this.AllowCancel = false;
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
			this.progressBar.Value = 0;

			string component = Wizard.SetupVariables.ComponentFullName;
			string componentId = Wizard.SetupVariables.ComponentId;
			Version iisVersion = Wizard.SetupVariables.IISVersion;
            bool iis7 = (iisVersion.Major >= 7);

			try
			{
				this.lblProcess.Text = "Creating uninstall script...";
				this.Update();

				//default actions
				List<InstallAction> actions = GetUninstallActions(componentId);

				//add external actions
				foreach (InstallAction extAction in Actions)
				{
					actions.Add(extAction);
				}

				//process actions
				for (int i = 0, progress = 1; i < actions.Count; i++, progress++)
				{
					InstallAction action = actions[i];
					this.lblProcess.Text = action.Description;
					this.progressBar.Value = progress * 100 / actions.Count;
					this.Update();

					try
					{

						switch (action.ActionType)
						{
							case ActionTypes.DeleteRegistryKey:
								if (OSInfo.IsWindows) DeleteRegistryKey(action.Key, action.Empty);
								break;
							case ActionTypes.DeleteDirectory:
								DeleteDirectory(action.Path);
								break;
							case ActionTypes.DeleteDatabase:
								DeleteDatabase(
									action.ConnectionString,
									action.Name);
								break;
							case ActionTypes.DeleteDatabaseUser:
								DeleteDatabaseUser(
									action.ConnectionString,
									action.UserName);
								break;
							case ActionTypes.DeleteDatabaseLogin:
								DeleteDatabaseLogin(
									action.ConnectionString,
									action.UserName);
								break;
							case ActionTypes.DeleteWebSite:
								if (!OSInfo.IsWindows)
								{
									DeleteUnixWebSite(action.Port);
								} else if (iis7)
									DeleteIIS7WebSite(action.SiteId);
								else
									DeleteWebSite(action.SiteId);
								break;
							case ActionTypes.DeleteVirtualDirectory:
								DeleteVirtualDirectory(
									action.SiteId,
									action.Name);
								break;
							case ActionTypes.DeleteUserMembership:
								DeleteUserMembership(action.Domain, action.Name, action.Membership);
								break;
							case ActionTypes.DeleteUserAccount:
								DeleteUserAccount(action.Domain, action.Name);
								break;
							case ActionTypes.DeleteApplicationPool:
								if (OSInfo.IsWindows)
								{
									if (iis7)
										DeleteIIS7ApplicationPool(action.Name);
									else
										DeleteApplicationPool(action.Name);
								}
								break;
							case ActionTypes.UpdateConfig:
								UpdateSystemConfiguration(action.Key);
								break;
							case ActionTypes.DeleteShortcuts:
								DeleteShortcuts(action.Name);
								break;
							case ActionTypes.UnregisterWindowsService:
								UnregisterWindowsService(action.Path, action.Name);
								break;
						}
					}
					catch (Exception ex)
					{
						if (!Utils.IsThreadAbortException(ex))
							Log.WriteError("Uninstall error", ex);
					}
				}
				this.progressBar.Value = 100;

			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				ShowError();
				this.Wizard.Close();
			}

			this.lblProcess.Text = "Completed. Click Next to continue.";
			this.AllowMoveNext = true;
			this.AllowCancel = true;
		}

		private void UnregisterWindowsService(string path, string serviceName)
		{
			if (!OSInfo.IsWindows) return;

			try
			{
				Log.WriteStart(string.Format("Removing \"{0}\" Windows service", serviceName));
				Log.WriteStart(string.Format("Stopping \"{0}\" Windows service", serviceName));
				try
				{
					Utils.StopService(serviceName);
					Log.WriteEnd("Stopped Windows service");
				}
				catch (Exception ex)
				{
					if (!Utils.IsThreadAbortException(ex))
						Log.WriteError("Windows service stop error", ex);
				}

                try
                {
                    ManagedInstallerClass.InstallHelper(new[] {"/u", path});                    
                }
                catch(Exception)
                {
                    Log.WriteError(string.Format("Unable to remove \"{0}\" Windows service.", serviceName), null);
                    InstallLog.AppendLine(string.Format("- Failed to remove \"{0}\" Windows service", serviceName));
                    throw;
                }

                Log.WriteEnd("Removed Windows service");
                InstallLog.AppendLine(string.Format("- Removed \"{0}\" Windows service", serviceName));
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Windows service error", ex);
				InstallLog.AppendLine(string.Format("- Failed to remove \"{0}\" Windows service", serviceName));
				throw;
			}
		}

		private void DeleteShortcuts(string fileName)
		{
			if (!OSInfo.IsWindows) return;

			try
			{
				Log.WriteStart("Deleting menu shortcut");
				string programs = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
				string path = Path.Combine(programs, "SolidCP Software");
				path = Path.Combine(path, fileName);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				Log.WriteEnd("Deleted menu shortcut");

				Log.WriteStart("Deleting desktop shortcut");
				string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
				path = Path.Combine(desktop, fileName);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				Log.WriteEnd("Deleted desktop shortcut");
				InstallLog.AppendLine("- Deleted application shortcuts");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Delete shortcut error", ex);
				InstallLog.AppendLine("- Failed to delete application shortcuts");
				throw;
			}
		}

		internal List<InstallAction> GetUninstallActions(string componentId)
		{
			var list = new List<InstallAction>();
			InstallAction action = null;

			if (OSInfo.IsWindows)
			{

				//windows service
				string serviceName = AppConfig.GetComponentSettingStringValue(componentId, "ServiceName");
				string serviceFile = AppConfig.GetComponentSettingStringValue(componentId, "ServiceFile");
				string installFolder = AppConfig.GetComponentSettingStringValue(componentId, "InstallFolder");
				if (!string.IsNullOrEmpty(serviceName) && !string.IsNullOrEmpty(serviceFile))
				{
					action = new InstallAction(ActionTypes.UnregisterWindowsService);
					action.Path = Path.Combine(installFolder, serviceFile);
					action.Name = serviceName;
					action.Description = "Removing Windows service...";
					action.Log = string.Format("- Remove {0} Windows service", serviceName);
					list.Add(action);
				}

				if (System.ServiceProcess.ServiceController.GetServices().Any(s => s.DisplayName.Equals(Global.Parameters.SchedulerServiceName, StringComparison.CurrentCultureIgnoreCase)))
				{
					action = new InstallAction(ActionTypes.UnregisterWindowsService) { Path = Path.Combine(installFolder, "bin", Global.Parameters.SchedulerServiceFileName), Name = Global.Parameters.SchedulerServiceName, Description = "Removing Windows service..." };
					action.Log = string.Format("- Remove {0} Windows service", action.Name);
					list.Add(action);
				}

				//database
				bool deleteDatabase = AppConfig.GetComponentSettingBooleanValue(componentId, "NewDatabase");
				if (deleteDatabase)
				{
					string connectionString = AppConfig.GetComponentSettingStringValue(componentId, "InstallConnectionString");
					string database = AppConfig.GetComponentSettingStringValue(componentId, "Database");
					action = new InstallAction(ActionTypes.DeleteDatabase);
					action.ConnectionString = connectionString;
					action.Name = database;
					action.Description = "Deleting database...";
					action.Log = string.Format("- Delete {0} database", database);
					list.Add(action);
				}
				//database user
				bool deleteDatabaseUser = AppConfig.GetComponentSettingBooleanValue(componentId, "NewDatabaseUser");
				if (deleteDatabaseUser)
				{
					string connectionString = AppConfig.GetComponentSettingStringValue(componentId, "InstallConnectionString");
					string username = AppConfig.GetComponentSettingStringValue(componentId, "DatabaseUser");
					action = new InstallAction(ActionTypes.DeleteDatabaseUser);
					action.ConnectionString = connectionString;
					action.UserName = username;
					action.Description = "Deleting database user...";
					action.Log = string.Format("- Delete {0} database user", username);
					list.Add(action);
				}
				//database login (from standalone setup)
				string loginName = AppConfig.GetComponentSettingStringValue(componentId, "DatabaseLogin");
				if (!string.IsNullOrEmpty(loginName))
				{
					string connectionString = AppConfig.GetComponentSettingStringValue(componentId, "InstallConnectionString");
					action = new InstallAction(ActionTypes.DeleteDatabaseLogin);
					action.ConnectionString = connectionString;
					action.UserName = loginName;
					action.Description = "Deleting database login...";
					action.Log = string.Format("- Delete {0} database login", loginName);
					list.Add(action);
				}
			}

			if (OSInfo.IsWindows)
			{
				//virtual directory
				bool deleteVirtualDirectory = AppConfig.GetComponentSettingBooleanValue(componentId, "NewVirtualDirectory");
				if (deleteVirtualDirectory)
				{
					string virtualDirectory = AppConfig.GetComponentSettingStringValue(componentId, "VirtualDirectory");
					string virtualDirectorySiteId = AppConfig.GetComponentSettingStringValue(componentId, "WebSiteId");
					action = new InstallAction(ActionTypes.DeleteVirtualDirectory);
					action.SiteId = virtualDirectorySiteId;
					action.Name = virtualDirectory;
					action.Description = "Deleting virtual directory...";
					action.Log = string.Format("- Delete {0} virtual directory...", virtualDirectory);
					list.Add(action);
				}
			}

			//web site
			bool deleteWebSite = AppConfig.GetComponentSettingBooleanValue(componentId, "NewWebSite");
			if (deleteWebSite)
			{
				string siteId = AppConfig.GetComponentSettingStringValue(componentId, "WebSiteId");
				string port = AppConfig.GetComponentSettingStringValue(componentId, "WebSitePort");

				action = new InstallAction(ActionTypes.DeleteWebSite);
				action.Port = port;
				action.SiteId = siteId;
				action.Description = "Deleting web site...";
				action.Log = OSInfo.IsWindows ? $"- Delete {siteId} web site." : $"- Delete {siteId} web site.{Environment.NewLine}- Remove firewall rule for port {port}.";
				list.Add(action);
			}

			if (OSInfo.IsWindows)
			{
				//application pool
				bool deleteAppPool = AppConfig.GetComponentSettingBooleanValue(componentId, "NewApplicationPool");
				if (deleteAppPool)
				{
					string appPoolName = AppConfig.GetComponentSettingStringValue(componentId, "ApplicationPool");
					if (string.IsNullOrEmpty(appPoolName))
						appPoolName = WebUtils.SolidCP_ADMIN_POOL;
					action = new InstallAction(ActionTypes.DeleteApplicationPool);
					action.Name = appPoolName;
					action.Description = "Deleting application pool...";
					action.Log = string.Format("- Delete {0} application pool", appPoolName);
					list.Add(action);
				}
			}

			if (OSInfo.IsWindows)
			{
				//user account
				bool deleteUserAccount = AppConfig.GetComponentSettingBooleanValue(componentId, "NewUserAccount");
				if (deleteUserAccount)
				{
					string username = AppConfig.GetComponentSettingStringValue(componentId, "UserAccount");
					string domain = AppConfig.GetComponentSettingStringValue(componentId, "Domain");
					//membership
					if (Wizard.SetupVariables.UserMembership != null && Wizard.SetupVariables.UserMembership.Length > 0)
					{
						action = new InstallAction(ActionTypes.DeleteUserMembership);
						action.Name = username;
						action.Domain = domain;
						action.Membership = Wizard.SetupVariables.UserMembership;
						action.Description = "Removing user account membership...";
						action.Log = string.Format("- Remove {0} user account membership", username);
						list.Add(action);
					}

					action = new InstallAction(ActionTypes.DeleteUserAccount);
					action.Name = username;
					action.Domain = domain;
					action.Description = "Deleting user account...";
					action.Log = string.Format("- Delete {0} user account", username);
					list.Add(action);
				}
			}

			//directory
			string path = AppConfig.GetComponentSettingStringValue(componentId, "InstallFolder");
			if (!string.IsNullOrEmpty(path))
			{
				action = new InstallAction(ActionTypes.DeleteDirectory);
				action.Path = path;
				action.Description = "Deleting application folder...";
				action.Log = string.Format("- Delete {0} folder", path);
				list.Add(action);
			}

			//config
			action = new InstallAction(ActionTypes.UpdateConfig);
			action.Key = componentId;
			action.Description = "Updating configuration settings...";
			action.Log = "- Update configuration settings";
			list.Add(action);
			return list;
		}

		private void DeleteDirectory(string path)
		{
			try
			{
				Log.WriteStart("Deleting folder");
				Log.WriteInfo(string.Format("Deleting \"{0}\" folder", path));
				if (FileUtils.DirectoryExists(path))
				{
					FileUtils.DeleteDirectory(path);
					Log.WriteEnd("Deleted folder");
				}
				InstallLog.AppendLine(string.Format("- Deleted \"{0}\" folder", path));
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("I/O error", ex);
				InstallLog.AppendLine(string.Format("- Failed to delete \"{0}\" folder", path));
				//throw;
			}
		}

		private void DeleteRegistryKey(string subkey, bool deleteEmptyOnly)
		{
			if (!OSInfo.IsWindows) return;
			try
			{
				Log.WriteStart("Deleting registry key");
				if (RegistryUtils.RegistryKeyExist(subkey))
				{
					if (deleteEmptyOnly && RegistryUtils.GetSubKeyCount(subkey) != 0)
					{
						Log.WriteEnd(string.Format("Registry key \"{0}\" is not empty", subkey));
						return;
					}
					Log.WriteInfo(string.Format("Deleting registry key \"{0}\"", subkey));
					RegistryUtils.DeleteRegistryKey(subkey);
					Log.WriteEnd("Deleted registry key");
					InstallLog.AppendLine(string.Format("- Deleted registry key \"{0}\"", subkey));
				}
				else
				{
					Log.WriteEnd(string.Format("Registry key \"{0}\" not found", subkey));
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Registry key delete error", ex);
				InstallLog.AppendLine(string.Format("- Failed to delete registry key \"{0}\"", subkey));
				throw;
			}
		}

		private void UpdateSystemConfiguration(string componentId)
		{
			try
			{
				Log.WriteStart("Updating system configuration");
				string componentName = AppConfig.GetComponentSettingStringValue(componentId, "ComponentName");
				Log.WriteInfo(string.Format("Deleting \"{0}\" component settings ", componentName));
				XmlUtils.RemoveXmlNode(AppConfig.GetComponentConfig(componentId));
				Log.WriteInfo("Saving system configuration");
				AppConfig.SaveConfiguration();
				Log.WriteEnd("Updated system configuration");
				InstallLog.AppendLine("- Updated system configuration");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Config error", ex);
				InstallLog.AppendLine("- Failed to update system configuration");
				throw;
			}
		}

		private void DeleteDatabase(string connectionString, string database)
		{
			try
			{
				Log.WriteStart("Deleting SQL server database");
				Log.WriteInfo(string.Format("Deleting \"{0}\" SQL server database", database));
				if (DatabaseUtils.DatabaseExists(connectionString, database))
				{
					DatabaseUtils.DeleteDatabase(connectionString, database);
					Log.WriteEnd("Deleted database");
					InstallLog.AppendLine(string.Format("- Deleted \"{0}\" SQL server database ", database));
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Database delete error", ex);
				InstallLog.AppendLine(string.Format("- Failed to delete \"{0}\" SQL server database ", database));
				throw;
			}
		}

		private void DeleteDatabaseUser(string connectionString, string username)
		{
			try
			{
				Log.WriteStart("Deleting SQL server user");
				Log.WriteInfo(string.Format("Deleting \"{0}\" SQL server user", username));
				if (DatabaseUtils.UserExists(connectionString, username))
				{
					DatabaseUtils.DeleteUser(connectionString, username);
					Log.WriteEnd("Deleted SQL server user");
					InstallLog.AppendLine(string.Format("- Deleted \"{0}\" SQL server user ", username));
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Database user delete error", ex);
				InstallLog.AppendLine(string.Format("- Failed to delete \"{0}\" SQL server user ", username));
				throw;
			}
		}

		private void DeleteDatabaseLogin(string connectionString, string loginName)
		{
			try
			{
				Log.WriteStart("Deleting SQL server login");
				Log.WriteInfo(string.Format("Deleting \"{0}\" SQL server login", loginName));
				if (DatabaseUtils.LoginExists(connectionString, loginName))
				{
					DatabaseUtils.DeleteLogin(connectionString, loginName);
					Log.WriteEnd("Deleted SQL server login");
					InstallLog.AppendLine(string.Format("- Deleted \"{0}\" SQL server login ", loginName));
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Database login delete error", ex);
				InstallLog.AppendLine(string.Format("- Failed to delete \"{0}\" SQL server login ", loginName));
				throw;
			}
		}


		private void DeleteUserMembership(string domain, string username, string[] membership)
		{
			if (!OSInfo.IsWindows) return;

			try
			{
				Log.WriteStart("Removing user membership");
				if (SecurityUtils.UserExists(domain, username))
				{
					Log.WriteInfo(string.Format("Removing user \"{0}\" membership", username));
					SecurityUtils.RemoveUserFromGroups(domain, username, membership);
					Log.WriteEnd("Removed user membership");
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("User membership delete error", ex);
				throw;
			}
		}

		private void DeleteUserAccount(string domain, string username)
		{
			if (!OSInfo.IsWindows) return;
			try
			{
				Log.WriteStart("Deleting user account");
				Log.WriteInfo(string.Format("Deleting \"{0}\" user account", username));
				if (SecurityUtils.UserExists(domain, username))
				{
					SecurityUtils.DeleteUser(domain, username);
					Log.WriteEnd("Deleted user account");
					InstallLog.AppendLine(string.Format("- Deleted \"{0}\" user account ", username));
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("User account delete error", ex);
				InstallLog.AppendLine(string.Format("- Failed to delete \"{0}\" user account ", username));
				throw;
			}
		}

		private void DeleteApplicationPool(string name)
		{
			try
			{
				Log.WriteStart("Deleting application pool");
				Log.WriteInfo(string.Format("Deleting \"{0}\" application pool", name));
				if (WebUtils.ApplicationPoolExists(name))
				{
					int count = WebUtils.GetApplicationPoolSitesCount(name);
					if (count > 0)
					{
						Log.WriteEnd("Application pool is not empty");
					}
					else
					{
						WebUtils.DeleteApplicationPool(name);
						Log.WriteEnd("Deleted  application pool");
						InstallLog.AppendLine(string.Format("- Deleted \"{0}\" application pool ", name));
					}
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Application pool delete error", ex);
				InstallLog.AppendLine(string.Format("- Failed to delete \"{0}\" application pool ", name));
				throw;
			}
		}

		private void DeleteIIS7ApplicationPool(string name)
		{
			try
			{
				Log.WriteStart("Deleting application pool");
				Log.WriteInfo(string.Format("Deleting \"{0}\" application pool", name));
				if (WebUtils.IIS7ApplicationPoolExists(name))
				{
					int count = WebUtils.GetIIS7ApplicationPoolSitesCount(name);
					if (count > 0)
					{
						Log.WriteEnd("Application pool is not empty");
					}
					else
					{
						WebUtils.DeleteIIS7ApplicationPool(name);
						Log.WriteEnd("Deleted  application pool");
						InstallLog.AppendLine(string.Format("- Deleted \"{0}\" application pool ", name));
					}
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Application pool delete error", ex);
				InstallLog.AppendLine(string.Format("- Failed to delete \"{0}\" application pool ", name));
				throw;
			}
		}

		private void DeleteWebSite(string siteId)
		{
			try
			{
				Log.WriteStart("Deleting web site");
				Log.WriteInfo(string.Format("Deleting \"{0}\" web site", siteId));
				if (WebUtils.SiteIdExists(siteId))
				{
					WebUtils.DeleteSite(siteId);
					Log.WriteEnd("Deleted web site");
					InstallLog.AppendLine(string.Format("- Deleted \"{0}\" web site ", siteId));
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Web site delete error", ex);
				InstallLog.AppendLine(string.Format("- Failed to delete \"{0}\" web site ", siteId));
				throw;
			}
		}

		private void DeleteIIS7WebSite(string siteId)
		{
			try
			{
				Log.WriteStart("Deleting web site");
				Log.WriteInfo(string.Format("Deleting \"{0}\" web site", siteId));
				if (WebUtils.IIS7SiteExists(siteId))
				{
					WebUtils.DeleteIIS7Site(siteId);
					Log.WriteEnd("Deleted web site");
					InstallLog.AppendLine(string.Format("- Deleted \"{0}\" web site ", siteId));
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Web site delete error", ex);
				InstallLog.AppendLine(string.Format("- Failed to delete \"{0}\" web site ", siteId));

				throw;
			}
		}
		private void DeleteUnixWebSite(string port)
		{
			Log.WriteStart("Deleting web site");
			var serviceId = (UniversalInstaller.Installer.Current as UniversalInstaller.UnixInstaller)?.UnixServiceId ?? "solidcp-server";

			try
			{
				Log.WriteInfo($"Deleting \"{serviceId}\" system service");
				var installer = UniversalInstaller.Installer.Current;
				installer.RemoveServer();
				Log.WriteEnd("Deleted web site");
				InstallLog.AppendLine($"- Deleted \"{serviceId}\" system service");
				InstallLog.AppendLine($"- Removed firewall rules for port {port}");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Web site delete error", ex);
				InstallLog.AppendLine($"- Failed to delete \"{serviceId}\" system service");

				throw;

			}
		}

		private void DeleteVirtualDirectory(string siteId, string name)
		{
			if (!OSInfo.IsWindows) return;

			try
			{
				Log.WriteStart("Deleting virtual directory");
				Log.WriteInfo(string.Format("Deleting virtual directory \"{0}\" for the site \"{1}\"", name, siteId));
				if (WebUtils.VirtualDirectoryExists(siteId, name))
				{
					WebUtils.DeleteVirtualDirectory(siteId, name);
					Log.WriteEnd("Deleted  virtual directory");
					InstallLog.AppendLine(string.Format("- Deleted \"{0}\" virtual directory ", name));
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Virtual directory delete error", ex);
				InstallLog.AppendLine(string.Format("- Failed to delete \"{0}\" virtual directory ", name));
				throw;
			}
		}
	}
}
