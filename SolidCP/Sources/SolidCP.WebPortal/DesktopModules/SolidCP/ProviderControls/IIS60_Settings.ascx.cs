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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SolidCP.EnterpriseServer;
using System.Text;

namespace SolidCP.Portal.ProviderControls
{
    public partial class IIS60_Settings : SolidCPControlBase, IHostingServiceProviderSettings
    {
		private string FilteredAppIds;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

		public void WebAppGalleryList_DataBound(object sender, EventArgs e)
		{
			SetAppsCalatolgFilter(FilteredAppIds);
		}

		public void ResetButton_Click(object sender, EventArgs e)
		{
			WebAppGalleryList.ClearSelection();
			//
			FilterDialogButton.Text = GetLocalizedString("FilterDialogButton.Text");
		}

		protected void SetAppsCalatolgFilter(string appsIds)
		{
			if (String.IsNullOrEmpty(appsIds))
				return;
			//
			string[] filteredApps = appsIds.Split(new string[] { "," },
				StringSplitOptions.RemoveEmptyEntries);
			//
			foreach (ListItem li in WebAppGalleryList.Items)
			{
				li.Selected = Array.Exists<string>(filteredApps,
					x => x.Equals(li.Value.Trim(),
						StringComparison.InvariantCultureIgnoreCase));
			}
			//
			FilterDialogButton.Text = GetLocalizedString("FilterDialogButton.AlternateText");
		}

		protected string GetAppsCatalogFilter()
		{
			var builder = new StringBuilder();
			var formatStr = "{0}";
			//
			foreach (ListItem li in WebAppGalleryList.Items)
			{
				if (li.Selected)
				{
					builder.AppendFormat(formatStr, li.Value.Trim());
					//
					formatStr = ",{0}";
				}
			}
			//
			return builder.ToString();
		}

        public void BindSettings(StringDictionary settings)
        {
            ipAddress.AddressId = (settings["SharedIP"] != null) ? Utils.ParseInt(settings["SharedIP"], 0) : 0;
            ipAddress.SelectValueText = GetLocalizedString("ipAddress.SelectValueText");
            txtPublicSharedIP.Text = settings["PublicSharedIP"];

            txtWebGroupName.Text = settings["WebGroupName"];
            chkAssignIPAutomatically.Checked = Utils.ParseBool(settings["AutoAssignDedicatedIP"], true);

            txtAspNet11Pool.Text = settings["AspNet11Pool"];
            txtAspNet20Pool.Text = settings["AspNet20Pool"];
			txtAspNet40Pool.Text = settings["AspNet40Pool"];
            txtAspPath.Text = settings["AspPath"];
            txtAspNet11Path.Text = settings["AspNet11Path"];
            txtAspNet20Path.Text = settings["AspNet20Path"];
			txtAspNet40Path.Text = settings["AspNet40Path"];
            txtPhp4Path.Text = settings["Php4Path"];
            txtPhp5Path.Text = settings["Php5Path"];
            txtPerlPath.Text = settings["PerlPath"];
            txtPythonPath.Text = settings["PythonPath"];
            txtColdFusionPath.Text = settings["ColdFusionPath"];
            txtScriptsDirectory.Text = settings["CFScriptsDirectory"];
            txtFlashRemotingDir.Text = settings["CFFlashRemotingDirectory"];

            txtPasswordFilterPath.Text = settings["SecuredFoldersFilterPath"];
			txtProtectedAccessFile.Text = settings["ProtectedAccessFile"];
			txtProtectedUsersFile.Text = settings["ProtectedUsersFile"];
			txtProtectedGroupsFile.Text = settings["ProtectedGroupsFile"];
			txtProtectedFoldersFile.Text = settings["ProtectedFoldersFile"];

            sharedSslSites.Value = settings[PackageSettings.SHARED_SSL_SITES];
            ActiveDirectoryIntegration.BindSettings(settings);
			
            //
            wpiEditFeedsList.Value = settings["FeedUrls"];
			FilteredAppIds = settings["GalleryAppsFilter"];
            radioFilterAppsList.SelectedIndex = Utils.ParseInt(settings["GalleryAppsFilterMode"], 0);
            chkGalleryAppsAlwaysIgnoreDependencies.Checked = Utils.ParseBool(settings["GalleryAppsAlwaysIgnoreDependencies"], true);
        }

        public void SaveSettings(StringDictionary settings)
        {
            settings["SharedIP"] = ipAddress.AddressId.ToString();
            settings["PublicSharedIP"] = txtPublicSharedIP.Text.Trim();
            settings["WebGroupName"] = txtWebGroupName.Text.Trim();
            settings["AutoAssignDedicatedIP"] = chkAssignIPAutomatically.Checked.ToString();

            settings["AspPath"] = txtAspPath.Text.Trim();
            settings["AspNet11Pool"] = txtAspNet11Pool.Text.Trim();
            settings["AspNet20Pool"] = txtAspNet20Pool.Text.Trim();
			settings["AspNet40Pool"] = txtAspNet40Pool.Text.Trim();
            settings["AspNet11Path"] = txtAspNet11Path.Text.Trim();
            settings["AspNet20Path"] = txtAspNet20Path.Text.Trim();
			settings["AspNet40Path"] = txtAspNet40Path.Text.Trim();
            settings["Php4Path"] = txtPhp4Path.Text.Trim();
            settings["Php5Path"] = txtPhp5Path.Text.Trim();
            settings["PerlPath"] = txtPerlPath.Text.Trim();
            settings["PythonPath"] = txtPythonPath.Text.Trim();
            settings["ColdFusionPath"] = txtColdFusionPath.Text.Trim();
            settings["CFScriptsDirectory"] = txtScriptsDirectory.Text.Trim();
            settings["CFFlashRemotingDirectory"] = txtFlashRemotingDir.Text.Trim();
			settings[PackageSettings.SHARED_SSL_SITES] = sharedSslSites.Value;

            settings["SecuredFoldersFilterPath"] = txtPasswordFilterPath.Text.Trim();
			settings["ProtectedAccessFile"] = txtProtectedAccessFile.Text.Trim();
			settings["ProtectedUsersFile"] = txtProtectedUsersFile.Text.Trim();
			settings["ProtectedGroupsFile"] = txtProtectedGroupsFile.Text.Trim();
			settings["ProtectedFoldersFile"] = txtProtectedFoldersFile.Text.Trim();
            
            ActiveDirectoryIntegration.SaveSettings(settings);
			//
            settings["FeedUrls"] = wpiEditFeedsList.Value;
			settings["GalleryAppsFilter"] = GetAppsCatalogFilter();
            settings["GalleryAppsFilterMode"] = radioFilterAppsList.SelectedIndex.ToString();
            settings["GalleryAppsAlwaysIgnoreDependencies"] = chkGalleryAppsAlwaysIgnoreDependencies.Checked.ToString();
        }
    }
}
