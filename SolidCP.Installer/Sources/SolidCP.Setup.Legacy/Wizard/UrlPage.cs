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
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Drawing;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using SolidCP.UniversalInstaller;

namespace SolidCP.Setup
{
	public partial class UrlPage : BannerWizardPage
	{
		public UrlPage()
		{
			InitializeComponent();
		}

		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();
			this.Text = "Enterprise Server URL";
			this.Description = "Enter the Enterprise Server URL";

			if (Wizard.SetupVariables.SetupAction == SetupActions.Setup)
			{
				LoadUrl();
			}
			this.txtURL.Text = Wizard.SetupVariables.EnterpriseServerURL;
			this.txtPath.Text = Wizard.SetupVariables.EnterpriseServerPath;
			this.chkBoxEmbed.Checked = Wizard.SetupVariables.EmbedEnterpriseServer;
			this.chkExpose.Checked = Wizard.SetupVariables.ExposeEnterpriseServerWebservices;

			chkBoxEmbed_CheckedChanged(this, EventArgs.Empty);

			var installerPath = Wizard.SetupVariables.InstallerFolder;
			var webClientsPath = Path.GetFullPath(Path.Combine(installerPath, "bin", "SolidCP.Web.Clients.dll"));
			var canEmbed = File.Exists(webClientsPath);
			this.chkBoxEmbed.Enabled = canEmbed;

			this.AllowMoveBack = true;
			this.AllowMoveNext = true;
			this.AllowCancel = true;
		}

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			//unattended setup
			if (!string.IsNullOrEmpty(Wizard.SetupVariables.SetupXml) && AllowMoveNext)
				Wizard.GoNext();
		}

		private void LoadUrl()
		{
			try
			{
				Wizard.SetupVariables.EnterpriseServerURL = string.Empty;

				string installFolder = Wizard.SetupVariables.InstallationFolder;
				string path = Path.Combine(installFolder, @"App_Data\SiteSettings.config");

				if (!File.Exists(path))
				{
					Log.WriteInfo(string.Format("File {0} not found", path));
					return;
				}

				XmlDocument doc = new XmlDocument();
				doc.Load(path);

				XmlElement urlNode = doc.SelectSingleNode("SiteSettings/EnterpriseServer") as XmlElement;
				if (urlNode == null)
				{
					Log.WriteInfo("EnterpriseServer setting not found");
					return;
				}

				Wizard.SetupVariables.EnterpriseServerURL = urlNode.InnerText;
				Wizard.SetupVariables.EmbedEnterpriseServer = urlNode.InnerText.StartsWith("assembly://");
				if (Wizard.SetupVariables.EmbedEnterpriseServer && string.IsNullOrEmpty(Wizard.SetupVariables.EnterpriseServerPath))
				{
					Wizard.SetupVariables.EnterpriseServerPath = $"..\\{Global.EntServer.ComponentName}";
				}
			}
			catch(Exception ex)
			{
				Log.WriteError("Site settings error", ex);
			}			
		}

		protected internal override void OnBeforeMoveNext(CancelEventArgs e)
		{
			try
			{
				if (!CheckFields())
				{
					e.Cancel = true;
					return;
				}
				Wizard.SetupVariables.EnterpriseServerURL = txtURL.Text;
				Wizard.SetupVariables.EnterpriseServerPath = txtPath.Text;
				Wizard.SetupVariables.EmbedEnterpriseServer = chkBoxEmbed.Checked;
				Wizard.SetupVariables.ExposeEnterpriseServerWebservices = chkExpose.Checked;
			}
			catch
			{
				this.AllowMoveNext = false;
				ShowError("Unable to set enterprise server URL.");
				return;
			}
			base.OnBeforeMoveNext(e);
		}

		private bool CheckFields()
		{
			string url = txtURL.Text;

			if (url.Trim().Length == 0)
			{
				return true;
			}

			Regex r = new Regex(@"(http|https|assembly)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
			Match m = r.Match(url);
			if (!m.Success)
			{
				ShowWarning("Please enter valid URL");
				return false;
			}

			if (chkBoxEmbed.Checked)
			{
				if (string.IsNullOrEmpty(txtPath.Text))
				{
					ShowWarning("Please enter valid Path");
					return false;
				}
				else
				{
					var entservpath = AbsolutePath(txtPath.Text);

					if (!Directory.Exists(entservpath))
					{
						ShowWarning("The specified path does not exist");
						return false;
					}
					else if (!File.Exists(Path.Combine(entservpath, "web.config")) ||
						!File.Exists(Path.Combine(entservpath, "bin", "SolidCP.EnterpriseServer.dll")))
					{
						ShowWarning("There is no Enterprise Server installation in the specified path");
						return false;
					}
				}
            }

			return true;
		}

		private string DefaultEntServerPath => $"..\\{Global.EntServer.ComponentName}";
		private string AbsolutePath(string relativePath) => Path.IsPathRooted(relativePath) ? relativePath :
			Path.GetFullPath(Path.Combine(Wizard.SetupVariables.InstallationFolder, relativePath));
        private string RelativePath(string absolutePath) => GetRelativePath(Wizard.SetupVariables.InstallationFolder, absolutePath);

        /// <summary>
        /// Creates a relative path from one file or folder to another.
        /// </summary>
        /// <param name="fromPath">Contains the directory that defines the start of the relative path.</param>
        /// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
        /// <returns>The relative path from the start directory to the end path.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fromPath"/> or <paramref name="toPath"/> is <c>null</c>.</exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static string GetRelativePath(string fromPath, string toPath)
        {
            if (string.IsNullOrEmpty(fromPath))
            {
                throw new ArgumentNullException("fromPath");
            }

            if (string.IsNullOrEmpty(toPath))
            {
                throw new ArgumentNullException("toPath");
            }

            Uri fromUri = new Uri(AppendDirectorySeparatorChar(fromPath));
            Uri toUri = new Uri(AppendDirectorySeparatorChar(toPath));

            if (fromUri.Scheme != toUri.Scheme)
            {
                return toPath;
            }

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (string.Equals(toUri.Scheme, Uri.UriSchemeFile, StringComparison.OrdinalIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }

        private static string AppendDirectorySeparatorChar(string path)
        {
            // Append a slash only if the path is a directory and does not have a slash.
            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                return path + Path.DirectorySeparatorChar;
            }

            return path;
        }

        private void chkBoxEmbed_CheckedChanged(object sender, EventArgs e)
		{
			lblEnterpriseServerPath.Enabled = chkExpose.Enabled = txtPath.Enabled = chkBoxEmbed.Checked;
			lblURL.Enabled = txtURL.Enabled = !chkBoxEmbed.Checked;
			
			if (chkBoxEmbed.Checked) {
				txtURL.Text = "assembly://SolidCP.EnterpriseServer";
				if (string.IsNullOrEmpty(txtPath.Text))
				{
					txtPath.Text = DefaultEntServerPath;
				}
			}
		}

        private void chooseFolderButton_Click(object sender, EventArgs e)
        {
			folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
			folderBrowserDialog.SelectedPath = AbsolutePath(string.IsNullOrEmpty(txtPath.Text) ? DefaultEntServerPath : txtPath.Text);
			folderBrowserDialog.ShowDialog();

			txtPath.Text = RelativePath(folderBrowserDialog.SelectedPath);
        }
    }
}
