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
using System.Diagnostics;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using SolidCP.Installer.Common;
using SolidCP.Installer.Core;
using SolidCP.Installer.Configuration;

namespace SolidCP.Installer.Controls
{
	/// <summary>
	/// Settings control
	/// </summary>
	internal partial class SettingsControl : ResultViewControl
	{
		/// <summary>
		/// Initializes a new instance of the SettingsControl class.
		/// </summary>
		public SettingsControl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Shows control
		/// </summary>
		/// <param name="context"></param>
		public override void ShowControl(AppContext context)
		{
			base.ShowControl(context);
			if (!IsInitialized)
			{
				AppContext = context;
				LoadSettings();
				IsInitialized = true;
			}
		}

		/// <summary>
		/// Loads application settings
		/// </summary>
		private void LoadSettings()
		{
			InstallerSection appConfig = AppConfigManager.AppConfiguration;
			chkAutoUpdate.Checked = appConfig.GetBooleanSetting(ConfigKeys.Web_AutoCheck);
			chkUseHTTPProxy.Checked = appConfig.GetBooleanSetting(ConfigKeys.Web_Proxy_UseProxy);
			txtAddress.Text = appConfig.GetStringSetting(ConfigKeys.Web_Proxy_Address);
			txtUserName.Text = appConfig.GetStringSetting(ConfigKeys.Web_Proxy_UserName);
			txtPassword.Text = appConfig.GetStringSetting(ConfigKeys.Web_Proxy_Password);
		}

		private void OnUseHTTPProxyCheckedChanged(object sender, EventArgs e)
		{
			txtAddress.Enabled = chkUseHTTPProxy.Checked;
			txtUserName.Enabled = chkUseHTTPProxy.Checked;
			txtPassword.Enabled = chkUseHTTPProxy.Checked;
		}

		/// <summary>
		/// Save application configuration
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnUpdateClick(object sender, EventArgs e)
		{
			KeyValueConfigurationCollection settings = AppConfigManager.AppConfiguration.Settings;
			settings[ConfigKeys.Web_AutoCheck].Value = chkAutoUpdate.Checked.ToString();
			settings[ConfigKeys.Web_Proxy_UseProxy].Value = chkUseHTTPProxy.Checked.ToString();
			settings[ConfigKeys.Web_Proxy_Address].Value = txtAddress.Text;
			settings[ConfigKeys.Web_Proxy_UserName].Value = txtUserName.Text;
			settings[ConfigKeys.Web_Proxy_Password].Value = txtPassword.Text;
			//
			AppConfigManager.SaveConfiguration(true);
		}

		/// <summary>
		/// Checks for updates
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnCheckClick(object sender, EventArgs e)
		{
			//start check in the separated thread
			AppContext.AppForm.StartAsyncProgress("Connecting...", true);
			ThreadStart threadDelegate = new ThreadStart(StartCheck);
			Thread newThread = new Thread(threadDelegate);
			newThread.Start();
		}

		/// <summary>
		/// Starts check
		/// </summary>
		private void StartCheck()
		{
			bool startUpdate = CheckForUpdate();
			if (startUpdate)
			{
				AppContext.AppForm.Close();
			}
		}

		/// <summary>
		/// Checks for update
		/// </summary>
		/// <returns></returns>
		private bool CheckForUpdate()
		{
			string fileName;
			bool updateAvailable = false;
			try
			{
				updateAvailable = AppContext.AppForm.CheckForUpdate(out fileName);
				AppContext.AppForm.FinishProgress();
			}
			catch (Exception ex)
			{
				Log.WriteError("Service error", ex);
				AppContext.AppForm.FinishProgress();
				AppContext.AppForm.ShowServerError();
				return false;
			}
			
			string appName = AppContext.AppForm.Text;
			if (updateAvailable)
			{
				string message = string.Format("This version of {0} is out of date.\nWould you like to download the latest version?", appName);
				if (MessageBox.Show(AppContext.AppForm, message, appName, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
				{
					return AppContext.AppForm.StartUpdateProcess(fileName);
				}
			}
			else
			{
				string message = string.Format("This version of {0} is up to date.", appName);
				Log.WriteInfo(message);
				AppContext.AppForm.ShowInfo(message);
			}
			return false;
		}

		private void OnViewLogClick(object sender, EventArgs e)
		{
			Log.ShowLogFile();
		}
	}
}
