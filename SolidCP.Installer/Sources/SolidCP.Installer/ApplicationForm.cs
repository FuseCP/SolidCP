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
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Configuration;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

using SolidCP.Installer.Common;
using SolidCP.Installer.Controls;
using SolidCP.Installer.Services;
using SolidCP.Installer.Configuration;
using System.Xml;
using System.Runtime.Remoting.Lifetime;
using SolidCP.Installer.Core;

namespace SolidCP.Installer
{
	/// <summary>
	/// Main application form
	/// </summary>
	internal partial class ApplicationForm : Form
	{
		private ProgressManager progressManager;
		private ScopeNode activeScopeNode;
		private static ApplicationForm instance;
		delegate void VoidCallback();

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the ApplicationForm class.
		/// </summary>
		internal ApplicationForm()
		{
			InitializeComponent();
			if (DesignMode)
			{
				return;
			}
		}
		#endregion

		#region Scope tree

		/// <summary>
		/// Adds predefined nodes
		/// </summary>
		private void AddDefaultNodes()
		{
			scopeTree.Nodes.Clear();
			ScopeNode componentsNode = AddScopeNode(null, "Components", Properties.Resources.Folder32, Properties.Resources.Folder16, new ComponentsControl(), NodeType.Components, null);
			AddScopeNode(null, "Application Settings", Properties.Resources.Tool32, Properties.Resources.Tool16, new SettingsControl(), NodeType.Settings, null);
			componentsNode.Expand();
			ExpandScopeNode(componentsNode);
			scopeTree.SelectedNode = componentsNode;

		}

		/// <summary>
		/// Adds scope node to the scope tree
		/// </summary>
		internal ScopeNode AddScopeNode(ScopeNode parent, string text, Icon largeIcon, Icon smallIcon, ResultViewControl resultView, NodeType nodeType, object tag)
		{
			string smallIconName = smallIcon.GetHashCode().ToString();
			if (!this.smallImages.Images.ContainsKey(smallIconName))
			{
				this.smallImages.Images.Add(smallIconName, smallIcon);
			}
			ScopeNode node = new ScopeNode();
			node.Text = text;
			node.ResultView = resultView;
			node.SmallIcon = smallIcon;
			node.LargeIcon = largeIcon;
			node.ImageKey = smallIconName;
			node.SelectedImageKey = smallIconName;
			node.NodeType = nodeType;
			node.Populated = false;
			node.Tag = tag;
			if (parent == null)
			{
				scopeTree.Nodes.Add(node);
			}
			else
			{
				parent.Nodes.Add(node);
			}
			//add fake node to show +
			node.Nodes.Add(" ");
			return node;
		}


		/// <summary>
		/// Adds component node to the scope tree
		/// </summary>
		internal ScopeNode AddComponentNode(ScopeNode parent, string text, object tag)
		{
			return AddScopeNode(parent, text, Properties.Resources.Service32, Properties.Resources.Service16, new ComponentControl(), NodeType.Component, tag);
		}

		/// <summary>
		/// Actions on select node in the scope tree
		/// </summary>
		private void OnScopeTreeAfterSelect(object sender, TreeViewEventArgs e)
		{
			ScopeNode node = e.Node as ScopeNode;
			//node.ContextMenuStrip = scopeItemContextMenu;
			ExpandScopeNode(node);
			PopulateResultView(node);
			activeScopeNode = node;
		}

		/// <summary>
		/// Expands scope node in the scope tree
		/// </summary>
		private void ExpandScopeNode(ScopeNode node)
		{
			if (node != null)
			{
				if (!node.Populated)
				{
					node.Nodes.Clear();
					StartProgress("Loading...");
					string text = node.Text;
					node.Text += " expanding...";
					scopeTree.Update();
					scopeTree.BeginUpdate();
					switch (node.NodeType)
					{
						/*case NodeType.Servers:
							LoadServers(node);
							break;
						case NodeType.Server:
							LoadServerComponents(node);
							break;*/
						case NodeType.Components:
							LoadInstalledComponents(node);
							break;
					}
					node.Text = text;
					node.Populated = true;
					node.Expand();
					scopeTree.EndUpdate();
					FinishProgress();
				}
			}
		}

		/// <summary>
		/// Action on expanding scope tree node
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnScopeTreeBeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			ScopeNode node = e.Node as ScopeNode;
			ExpandScopeNode(node);
		}
		#endregion

		#region Result view

		/// <summary>
		/// Displays result view control
		/// </summary>
		/// <param name="parentNode"></param>
		private void PopulateResultView(ScopeNode parentNode)
		{
			this.SuspendLayout();
			pnlResultView.Controls.Clear();
			if (parentNode.LargeIcon != null)
			{
				pictureBox.Image = parentNode.LargeIcon.ToBitmap();
			}
			else
			{
				pictureBox.Image = Properties.Resources.Folder32.ToBitmap();
			}
			lblResultViewTitle.Text = parentNode.Text;
			lblResultViewPath.Text = parentNode.FullPath;
			ResultViewControl control = parentNode.ResultView;
			if (control != null)
			{
				pnlResultView.Controls.Add(control);
				control.Dock = DockStyle.Fill;
				try
				{
					AppContext context = new AppContext();
					context.AppForm = this;
					context.ScopeNode = parentNode;
					control.ShowControl(context);
				}
				catch (Exception ex)
				{
					Log.WriteError("Console error", ex);
					ShowError(ex);
				}
			}
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		#region Console

		/// <summary>
		/// Application form instance
		/// </summary>
		internal static ApplicationForm Instance
		{
			get { return instance; }
		}

		/// <summary>
		/// Current version
		/// </summary>
		public string Version
		{
			get
			{
				return this.GetType().Assembly.GetName().Version.ToString();
			}
		}

		/// <summary>
		/// Reloads application
		/// </summary>
		internal void ReloadApplication()
		{
			//thread safe call
			if (this.InvokeRequired)
			{
				VoidCallback callback = new VoidCallback(ReloadApplication);
				this.Invoke(callback);
			}
			else
			{
				AppConfigManager.LoadConfiguration();
				Update();
				//LoadConfiguration();
				ScopeNode componentsNode = scopeTree.Nodes[0] as ScopeNode;
				componentsNode.Nodes.Clear();
				componentsNode.Populated = false;
				OnScopeTreeAfterSelect(scopeTree, new TreeViewEventArgs(componentsNode));
			}
		}

		/// <summary>
		/// Initializes application
		/// </summary>
		internal void InitializeApplication()
		{
			CheckForIllegalCrossThreadCalls = false;
			LifetimeServices.LeaseTime = TimeSpan.Zero;

			this.splitContainer.Panel2MinSize = 380;
			this.splitContainer.Panel1MinSize = 150;
			this.progressManager = new ProgressManager(this, this.statusBarLabel);
			instance = this;

			AddDefaultNodes();

		}

		/// <summary>
		/// Disables application content
		/// </summary>
		internal void DisableContent()
		{
			scopeTree.Enabled = false;
			pnlResultView.Enabled = false;
		}

		/// <summary>
		/// Enables application content
		/// </summary>
		internal void EnableContent()
		{
			scopeTree.Enabled = true;
			pnlResultView.Enabled = true;
		}

		/// <summary>
		/// Shows error message
		/// </summary>
		/// <param name="message">Message</param>
		internal void ShowError(string message)
		{
			MessageBox.Show(this, message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		/// <summary>
		/// Shows default web error message
		/// </summary>
		internal void ShowServerError()
		{
			ShowError("An error occurred while connecting to the remote server. Please check your internet connection.");
		}

		/// <summary>
		/// Shows default error message
		/// </summary>
		internal void ShowError()
		{
			ShowError("An unexpected error has occurred. We apologize for this inconvenience.\n" +
				"Please contact Technical Support at support@solidcp.com.\n\n" +
				"Make sure you include a copy of the Installer.log file from the\n" +
				"SolidCP Installer home directory.");
		}

		/// <summary>
		/// Shows security error message
		/// </summary>
		internal void ShowSecurityError()
		{
			ShowError(Global.Messages.NotEnoughPermissionsError);
		}

		internal void ShowError(Exception ex)
		{
			if (Utils.IsSecurityException(ex))
				ShowSecurityError();
			else
				ShowError();
		}
			

		/// <summary>
		/// Shows info message
		/// </summary>
		/// <param name="message"></param>
		internal void ShowInfo(string message)
		{
			MessageBox.Show(this, message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		/// <summary>
		/// Shows warning message
		/// </summary>
		/// <param name="message"></param>
		internal void ShowWarning(string message)
		{
			MessageBox.Show(this, message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		/// <summary>
		/// Loads installed components from configuration file
		/// </summary>
		private void LoadInstalledComponents(ScopeNode node)
		{
			Log.WriteStart("Loading installed components");
			node.Nodes.Clear();

			foreach (ComponentConfigElement componentConfig in AppConfigManager.AppConfiguration.Components)
			{
				string instance = string.Empty;
				if (componentConfig.Settings["Instance"] != null &&
					!string.IsNullOrEmpty(componentConfig.Settings["Instance"].Value))
				{
					instance = "(" + componentConfig.Settings["Instance"].Value + ")";
				}
				string title = string.Format("{0} {1} {2} {3}",
					componentConfig.Settings["ApplicationName"].Value,
					componentConfig.Settings["ComponentName"].Value,
					componentConfig.Settings["Release"].Value,
					instance);

				AddComponentNode(node, title, componentConfig);
			}
			node.Populated = true;
			Log.WriteEnd(string.Format("{0} installed component(s) loaded", AppConfigManager.AppConfiguration.Components.Count));
		}

		/// <summary>
		/// Returns installer web service
		/// </summary>
		internal InstallerService WebService
		{
			get
			{
				return ServiceProviderProxy.GetInstallerWebService();
			}
		}

		/// <summary>
		/// Checks for the application update
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns>true if update is available for download; otherwise false</returns>
		internal bool CheckForUpdate(out string fileName)
		{
			bool ret = false;
			fileName = string.Empty;
			Log.WriteStart("Checking for a new version");
			//
			var webService = ServiceProviderProxy.GetInstallerWebService();
			DataSet ds = webService.GetLatestComponentUpdate("cfg core");
			//
			Log.WriteEnd("Checked for a new version");
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				DataRow row = ds.Tables[0].Rows[0];
				Version currentVersion = GetType().Assembly.GetName().Version;
				Version newVersion = null;
				try
				{
					newVersion = new Version(row["Version"].ToString());
				}
				catch (FormatException e)
				{
					Log.WriteError("Version error", e);
					return false;
				}
				if (newVersion > currentVersion)
				{
					ret = true;
					fileName = row["UpgradeFilePath"].ToString();
					Log.WriteInfo(string.Format("Version {0} is available for download", newVersion));
				}
			}
			return ret;
		}

		/// <summary>
		/// Runs application updater
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns>true if updater started successfully</returns>
		internal bool StartUpdateProcess(string fileName)
		{
			Log.WriteStart("Starting updater");
			string tmpFile = Path.ChangeExtension(Path.GetTempFileName(), ".exe");
			using (Stream writeStream = File.Create(tmpFile))
			{
				using (Stream readStream = typeof(Program).Assembly.GetManifestResourceStream("SolidCP.Installer.Updater.exe"))
				{
					byte[] buffer = new byte[(int)readStream.Length];
					readStream.Read(buffer, 0, buffer.Length);
					writeStream.Write(buffer, 0, buffer.Length);
				}
			}
			string targetFile = GetType().Module.FullyQualifiedName;
			//
			var webService = ServiceProviderProxy.GetInstallerWebService();
			string url = webService.Url;
			//
			string proxyServer = string.Empty;
			string user = string.Empty;
			string password = string.Empty;

			// check if we need to add a proxy to access Internet
			bool useProxy = AppConfigManager.AppConfiguration.GetBooleanSetting(ConfigKeys.Web_Proxy_UseProxy);
			if (useProxy)
			{
				proxyServer = AppConfigManager.AppConfiguration.Settings[ConfigKeys.Web_Proxy_Address].Value;
				user = AppConfigManager.AppConfiguration.Settings[ConfigKeys.Web_Proxy_UserName].Value;
				password = AppConfigManager.AppConfiguration.Settings[ConfigKeys.Web_Proxy_Password].Value;
			}

			ProcessStartInfo info = new ProcessStartInfo();
			info.FileName = tmpFile;

			//prepare command line args
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("\\url:\"{0}\" ", url);
			sb.AppendFormat("\\target:\"{0}\" ", targetFile);
			sb.AppendFormat("\\file:\"{0}\" ", fileName);
			sb.AppendFormat("\\proxy:\"{0}\" ", proxyServer);
			sb.AppendFormat("\\user:\"{0}\" ", user);
			sb.AppendFormat("\\password:\"{0}\" ", password);
			info.Arguments = sb.ToString();
			Process process = Process.Start(info);
			if (process.Handle != IntPtr.Zero)
			{
				User32.SetForegroundWindow(process.Handle);
			}
			Log.WriteEnd("Updater started");
			return (process.Handle != IntPtr.Zero);
		}
		#endregion

		#region Progress indication

		/// <summary>
		/// Starts progress indication
		/// </summary>
		/// <param name="title">Title</param>
		internal void StartProgress(string title)
		{
			StartProgress(title, false);
		}

		/// <summary>
		/// Starts progress indication
		/// </summary>
		/// <param name="title">Title</param>
		/// <param name="disableContent">Disable content</param>
		internal void StartProgress(string title, bool disableContent)
		{
			if (disableContent)
			{
				DisableContent();
			}
			progressManager.StartProgress(title);
		}

		/// <summary>
		/// Starts async progress indication
		/// </summary>
		/// <param name="title">Title</param>
		/// <param name="disableContent">Disable content</param>
		internal void StartAsyncProgress(string title, bool disableContent)
		{
			if (disableContent)
			{
				DisableContent();
			}
			topLogoControl.ShowProgress();
			progressManager.StartProgress(title);
		}

		/// <summary>
		/// Finishes progress indication
		/// </summary>
		internal void FinishProgress()
		{
			topLogoControl.HideProgress();
			progressManager.FinishProgress();
			EnableContent();
		}

		#endregion

		private void OnApplicationFormShown(object sender, EventArgs e)
		{
			StartUnattendedSetup();
		}

		private void StartUnattendedSetup()
		{
			XmlDocument doc = Global.SetupXmlDocument;
			if (doc != null)
			{
				XmlNode root = doc.SelectSingleNode("setup");
				if (root == null)
				{
					Log.WriteError("Incorrect setup xml file");
					Close();
				}
				Log.WriteStart("Starting unattended setup");
				ProceedUnattendedSetup();
			}
		}

		public void ProceedUnattendedSetup()
		{
			XmlDocument doc = Global.SetupXmlDocument;
			XmlNode root = doc.SelectSingleNode("setup");
			if (root.ChildNodes.Count == 0)
			{
				Log.WriteEnd("Unuttended setup finished");
				Close();
				return;
			}
			XmlNode node = root.ChildNodes[0];
			switch (node.Name.ToLower())
			{
				case "install":
					ParseInstallNode(node);
					break;
			}
		}

		private void ParseInstallNode(XmlNode installNode)
		{
			XmlNodeList components = installNode.SelectNodes("component");
			if (components.Count == 0)
			{
				//remove parent install node
				installNode.ParentNode.RemoveChild(installNode);
				ProceedUnattendedSetup();
			}
			else
			{
				//remove current node and start installation
				XmlElement componentNode = (XmlElement)components[0];
				string componentCode = componentNode.GetAttribute("code");
				string componentVersion = componentNode.GetAttribute("version");
				string xml = componentNode.InnerXml;
				installNode.RemoveChild(componentNode);

				if (!string.IsNullOrEmpty(componentCode))
				{
					ScopeNode componentsNode = scopeTree.Nodes[0] as ScopeNode;
					scopeTree.SelectedNode = componentsNode;
					ComponentsControl ctrl = componentsNode.ResultView as ComponentsControl;
					ctrl.InstallComponent(componentCode, componentVersion, xml);
				}
			}
		}

		public override object InitializeLifetimeService()
		{
			ILease lease = (ILease)base.InitializeLifetimeService();

			//Set lease properties
			lease.InitialLeaseTime = TimeSpan.Zero;
			return lease;
		}

	}
}
	
	
