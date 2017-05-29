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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SolidCP.Providers.Web;

namespace SolidCP.Portal
{
    public partial class WebSitesSecuredFoldersControl : SolidCPControlBase
    {
        private bool IsSecuredFoldersInstalled
        {
            get { return ViewState["IsSecuredFoldersInstalled"] != null ? (bool)ViewState["IsSecuredFoldersInstalled"] : false; }
            set { ViewState["IsSecuredFoldersInstalled"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

		public void BindSecuredFolders(WebSite site)
        {
            // save initial state
            IsSecuredFoldersInstalled = site.SecuredFoldersInstalled;
			// Render a warning message about the automatic site's settings change
			if (!IsSecuredFoldersInstalled && site.IIs7)
			{
				// Ensure the message is displayed only when neccessary
				if (site.EnableWindowsAuthentication || !site.AspNetInstalled.EndsWith("I"))
				{
					string warningStr = GetLocalizedString("EnableFoldersIIs7Warning.Text");
					// Render a warning only if specified
					if (!String.IsNullOrEmpty(warningStr))
						btnToggleSecuredFolders.OnClientClick = String.Format("return confirm('{0}')", warningStr);
				}
			}
            // toggle
            ToggleControls();
        }

        private void ToggleControls()
        {
            // toggle button
            btnToggleSecuredFolders.Text = GetLocalizedString(
                IsSecuredFoldersInstalled ? "DisableFolders.Text" : "EnableFolders.Text");

            // show hide panel
            SecuredFoldersPanel.Visible = IsSecuredFoldersInstalled;

            // bind items
            if (IsSecuredFoldersInstalled)
            {
                BindFolders();
                BindUsers();
                BindGroups();
            }
        }

        private void BindFolders()
        {
            gvFolders.DataSource = ES.Services.WebServers.GetSecuredFolders(PanelRequest.ItemID);
            gvFolders.DataBind();
        }

        private void BindUsers()
        {
            gvUsers.DataSource = ES.Services.WebServers.GetSecuredUsers(PanelRequest.ItemID);
            gvUsers.DataBind();
        }

        private void BindGroups()
        {
            gvGroups.DataSource = ES.Services.WebServers.GetSecuredGroups(PanelRequest.ItemID);
            gvGroups.DataBind();
        }

        protected void btnToggleSecuredFolders_Click(object sender, EventArgs e)
        {
            try
            {
                int result = 0;
                if (IsSecuredFoldersInstalled)
                {
                    // uninstall folders
                    result = ES.Services.WebServers.UninstallSecuredFolders(PanelRequest.ItemID);
                }
                else
                {
                    // install folders
                    result = ES.Services.WebServers.InstallSecuredFolders(PanelRequest.ItemID);
                }

                if (result < 0)
                {
                    HostModule.ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                HostModule.ShowErrorMessage("WEB_INSTALL_FOLDERS", ex);
                return;
            }

            // change state
            IsSecuredFoldersInstalled = !IsSecuredFoldersInstalled;

            // bind items
            ToggleControls();
        }

        public string GetEditUrl(string ctrlKey, string name)
        {
            name = Server.UrlEncode(name);
            return HostModule.EditUrl("ItemID", PanelRequest.ItemID.ToString(), ctrlKey,
                "Name=" + name);
        }

        protected void btnAddFolder_Click(object sender, EventArgs e)
        {
            RedirectToEditControl("edit_webfolder", "");
        }

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            RedirectToEditControl("edit_webuser", "");
        }

        protected void btnAddGroup_Click(object sender, EventArgs e)
        {
            RedirectToEditControl("edit_webgroup", "");
        }

        public void RedirectToEditControl(string ctrlKey, string name)
        {
            Response.Redirect(GetEditControlUrl(ctrlKey, name));
        }

        public string GetEditControlUrl(string ctrlKey, string name)
        {
            return HostModule.EditUrl("ItemID", PanelRequest.ItemID.ToString(), ctrlKey,
                "Name=" + name,
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString());
        }

        private bool DeleteFolder(string name)
        {
            try
            {
                int result = ES.Services.WebServers.DeleteSecuredFolder(PanelRequest.ItemID, name);
                if (result < 0)
                {
                    HostModule.ShowResultMessage(result);
                    return false;
                }
            }
            catch (Exception ex)
            {
                HostModule.ShowErrorMessage("WEB_DELETE_SECURED_FOLDER", ex);
                return false;
            }
            return true;
        }

        private bool DeleteUser(string name)
        {
            try
            {
                int result = ES.Services.WebServers.DeleteSecuredUser(PanelRequest.ItemID, name);
                if (result < 0)
                {
                    HostModule.ShowResultMessage(result);
                    return false;
                }
            }
            catch (Exception ex)
            {
                HostModule.ShowErrorMessage("WEB_DELETE_SECURED_USER", ex);
                return false;
            }
            return true;
        }

        private bool DeleteGroup(string name)
        {
            try
            {
                int result = ES.Services.WebServers.DeleteSecuredGroup(PanelRequest.ItemID, name);
                if (result < 0)
                {
                    HostModule.ShowResultMessage(result);
                    return false;
                }
            }
            catch (Exception ex)
            {
                HostModule.ShowErrorMessage("WEB_DELETE_SECURED_GROUP", ex);
                return false;
            }
            return true;
        }

        protected void gvFolders_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // delete folder
            string folderName = (string)gvFolders.DataKeys[e.RowIndex][0];

            if (DeleteFolder(folderName))
            {
                // reb-bind folders
                BindFolders();
            }

            // cancel command
            e.Cancel = true;
        }

        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // delete user
            string name = (string)gvUsers.DataKeys[e.RowIndex][0];

            if (DeleteUser(name))
            {
                // reb-bind users
                BindUsers();
            }

            // cancel command
            e.Cancel = true;
        }

        protected void gvGroups_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // delete group
            string name = (string)gvGroups.DataKeys[e.RowIndex][0];

            if (DeleteGroup(name))
            {
                // reb-bind groups
                BindGroups();
            }

            // cancel command
            e.Cancel = true;
        }
    }
}
