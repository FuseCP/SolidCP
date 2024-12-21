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
using System.Threading;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SolidCP.UniversalInstaller.Controls
{
	/// <summary>
	/// Component control
	/// </summary>
	internal partial class ComponentControl : ResultViewControl
	{
		delegate void ReloadApplicationCallback();

		/// <summary>
		/// Initializes a new instance of the ComponentControl class.
		/// </summary>
		public ComponentControl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Shows control
		/// </summary>
		/// <param name="context"></param>
		public override void ShowControl(SCPAppContext context)
		{
			base.ShowControl(context);
			if (!IsInitialized)
			{
				ComponentInfo element = context.ScopeNode.Tag as ComponentInfo;
				if (element != null)
				{
					txtApplication.Text = element.ApplicationName;
					txtComponent.Text = element.ComponentName;
					txtVersion.Text = element.Version.ToString();
					lblDescription.Text = element.ComponentDescription;

					string installer = element.FullFilePath;// element.Installer;
					string path = element.InstallerPath;
					string type = element.InstallerType;
					if (string.IsNullOrEmpty(installer) ||
						string.IsNullOrEmpty(path) || 
						string.IsNullOrEmpty(type))
					{
						btnRemove.Enabled = false;
						btnSettings.Enabled = false;
					}
				}
				IsInitialized = true;
			}
		}

		/// <summary>
		/// Action on Remove button click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnRemoveClick(object sender, EventArgs e)
		{
			UninstallComponent();
		}

		/// <summary>
		/// Action on Settings button click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnSettingsClick(object sender, EventArgs e)
		{
			SetupComponent();
		}

		/// <summary>
		/// Action on Check For Update button click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnCheckUpdateClick(object sender, EventArgs e)
		{
			//start check in the separate thread
			AppContext.AppForm.StartAsyncProgress("Connecting...", true);
			ThreadStart threadDelegate = new ThreadStart(CheckForUpdate);
			Thread newThread = new Thread(threadDelegate);
			newThread.Start();
		}

		/// <summary>
		/// Checks for component update
		/// </summary>
		private void CheckForUpdate()
		{
			Log.WriteStart("Checking for component update");
			ComponentInfo element = AppContext.ScopeNode.Tag as ComponentInfo;

			string componentName = element.ComponentName;
			string componentCode = element.ComponentCode;
			string release = element.Version.ToString();		
	
			// call web service
			ComponentUpdateInfo info;
			try
			{
				Log.WriteInfo(string.Format("Checking {0} {1}", componentName, release));
				//
				var webService = Installer.Current.InstallerWebService;
				info = webService.GetComponentUpdate(componentCode, release);
				//
				Log.WriteEnd("Component update checked");
				AppContext.AppForm.FinishProgress();
			}
			catch (Exception ex)
			{
				Log.WriteError("Service error", ex);
				AppContext.AppForm.FinishProgress();
				AppContext.AppForm.ShowServerError();
				return;
			}

			string appName = AppContext.AppForm.Text;
			if (info != null)
			{
				string newVersion = info.Version.ToString();
				Log.WriteInfo(string.Format("Version {0} is available for download", newVersion)); 

				string message = string.Format("{0} {1} is available now.\nWould you like to install new version?", componentName, newVersion);
				if (MessageBox.Show(AppContext.AppForm, message, appName, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
				{
					string fileToDownload = info.UpgradeFilePath;
					string installerPath = info.InstallerPath;
					string installerType = info.InstallerType;
					UpdateComponent(fileToDownload, installerPath, installerType, newVersion);
				}
			}
			else
			{
				string message = string.Format("Current version of {0} is up to date.", componentName);
				Log.WriteInfo(message);
				AppContext.AppForm.ShowInfo(message);
			}
		}

		delegate void UpdateComponentCallback(string fileName, string path, string type, string version);

		/// <summary>
		/// Runs component update
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="path"></param>
		/// <param name="type"></param>
		private void UpdateComponent(string fileName, string path, string type, string version)
		{
			// InvokeRequired required compares the thread ID of the
			// calling thread to the thread ID of the creating thread.
			// If these threads are different, it returns true.
			if (this.InvokeRequired)
			{
				UpdateComponentCallback callBack = new UpdateComponentCallback(UpdateComponent);
				Invoke(callBack, new object[] { fileName, path, type, version });
			}
			else
			{
				Log.WriteStart("Updating component");

				ComponentInfo element = AppContext.ScopeNode.Tag as ComponentInfo;
				string componentId = element.ComponentCode;
				string componentName = element.ComponentName;

				try
				{
					Log.WriteInfo(string.Format("Updating {0}", componentName));
					//download installer
					Loader form = new Loader(fileName, (e) => AppContext.AppForm.ShowError(e));
					DialogResult result = form.ShowDialog(this);
					if (result == DialogResult.OK)
					{
						//run installer
						string tmpFolder = FileUtils.GetTempDirectory();
						string installerPath = Path.Combine(tmpFolder, path);
						Update();
						string method = "Update";
						Log.WriteStart(string.Format("Running installer {0}.{1} from {2}", type, method, path));
						Hashtable args = new Hashtable();
						args["ComponentId"] = componentId;
						args["ShellVersion"] = AppContext.AppForm.Version;
						args["BaseDirectory"] = FileUtils.GetCurrentDirectory();
						args["UpdateVersion"] = version;
						args["Installer"] = Path.GetFileName(fileName);
						args["InstallerType"] = type;
						args["InstallerPath"] = path;
						args["InstallerFolder"] = tmpFolder;
						args["IISVersion"] = Global.IISVersion;
						args["ParentForm"] = FindForm();

						var res = (bool)AssemblyLoader.Execute(installerPath, type, method, new object[] { args });
						Log.WriteInfo(string.Format("Installer returned {0}", result));
						Log.WriteEnd("Installer finished");
						Update();
						if (res)
						{
							ReloadApplication();
						}
						FileUtils.DeleteTempDirectory();
					}
					Log.WriteEnd("Update completed");
				}
				catch (Exception ex)
				{
					Log.WriteError("Installer error", ex);
					AppContext.AppForm.ShowError(ex);
				}
			}
		}

		/// <summary>
		/// Uninstalls component
		/// </summary>
		private void UninstallComponent()
		{
			Log.WriteStart("Uninstalling component");
			
			ComponentInfo element = AppContext.ScopeNode.Tag as ComponentInfo;
			string installer = element.FullFilePath; //.GetStringSetting(Global.Parameters.Installer);
			string path = element.InstallerPath;
			string type = element.InstallerType;
			string componentId = element.ComponentCode;
			string componentCode = element.ComponentCode;
			string componentName = element.ComponentName;
			string release = element.Version.ToString();

			try
			{
				Log.WriteInfo(string.Format("Uninstalling {0}", componentName));
				//download installer
				Loader form = new Loader(installer, componentCode, release, (e) => AppContext.AppForm.ShowError(e));
				DialogResult result = form.ShowDialog(this);
				if (result == DialogResult.OK)
				{
					//run installer
					string tmpFolder = FileUtils.GetTempDirectory();
					path = Path.Combine(tmpFolder, path);
					Update();
					string method = "Uninstall";
					//
					Log.WriteStart(string.Format("Running installer {0}.{1} from {2}", type, method, path));
					//
					var args = new Hashtable
					{
						{ Global.Parameters.ComponentId, componentId },
						{ Global.Parameters.ComponentCode, componentCode },
						{ Global.Parameters.ShellVersion, AppContext.AppForm.Version },
						{ Global.Parameters.BaseDirectory, FileUtils.GetCurrentDirectory() },
						{ Global.Parameters.IISVersion, Global.IISVersion },
						{ Global.Parameters.ParentForm,  FindForm() },
					};
					//
					var res = (bool)AssemblyLoader.Execute(path, type, method, new object[] { args });
					//
					Log.WriteInfo(string.Format("Installer returned {0}", result));
					Log.WriteEnd("Installer finished");
					Update();
					ReloadApplication();
					FileUtils.DeleteTempDirectory();
					
				}
				Log.WriteEnd("Uninstall completed");
			}
			catch (Exception ex)
			{
				Log.WriteError("Installer error", ex);
				AppContext.AppForm.ShowError(ex);
			}
		}

		/// <summary>
		/// Setup component
		/// </summary>
		private void SetupComponent()
		{
			Log.WriteStart("Starting component setup");

			var element = AppContext.ScopeNode.Tag as ComponentInfo;

			string installer = element.FullFilePath; //.GetStringSetting("Installer");
			string path = element.InstallerPath;
			string type = element.InstallerType;
			string componentId = element.ComponentCode; //.ID;
			string ccode = element.ComponentCode;
			string componentName = element.ComponentName;
			string cversion = element.Version.ToString();

			try
			{
				Log.WriteInfo(string.Format("Setup {0} {1}", componentName, cversion));
                //download installer
				Loader form = new Loader(installer, ccode, cversion, (e) => AppContext.AppForm.ShowError(e));
				DialogResult result = form.ShowDialog(this);
				if (result == DialogResult.OK)
				{
					string tmpFolder = Path.Combine(AppContext.CurrentPath, "Tmp");
					path = Path.Combine(tmpFolder, path);
					Update();
					string method = "Setup";
					Log.WriteStart(string.Format("Running installer {0}.{1} from {2}", type, method, path));
					Hashtable args = new Hashtable();
					args["ComponentId"] = componentId;
					args["ShellVersion"] = AppContext.AppForm.Version;
					args["BaseDirectory"] = FileUtils.GetCurrentDirectory();
                    args["IISVersion"] = Global.IISVersion;
					args["ParentForm"] = FindForm();
					args[Global.Parameters.ShellMode] = Global.VisualInstallerShell;
					//
					var res = (bool)AssemblyLoader.Execute(path, type, method, new object[] { args });
					//
					Log.WriteInfo(string.Format("Installer returned {0}", result));
					Log.WriteEnd("Installer finished");

					if (res)
					{
						ReloadApplication();
					}
					FileUtils.DeleteTempDirectory();
				}
				Log.WriteEnd("Component setup completed");
			}
			catch (Exception ex)
			{
				Log.WriteError("Installer error", ex);
				this.AppContext.AppForm.ShowError(ex);
			}
		}

		/// <summary>
		/// Thread safe application reload
		/// </summary>
		private void ReloadApplication()
		{
			// InvokeRequired required compares the thread ID of the
			// calling thread to the thread ID of the creating thread.
			// If these threads are different, it returns true.
			if (this.InvokeRequired)
			{
				ReloadApplicationCallback callback = new ReloadApplicationCallback(ReloadApplication);
				Invoke(callback, null);
			}
			else
			{
				AppContext.AppForm.ReloadApplication();
			}
		}
	}
}
