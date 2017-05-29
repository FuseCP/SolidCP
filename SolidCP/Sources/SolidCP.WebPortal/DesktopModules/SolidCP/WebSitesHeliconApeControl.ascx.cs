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
using SolidCP.Providers.ResultObjects;

namespace SolidCP.Portal
{
    public partial class WebSitesHeliconApeControl : SolidCPControlBase
    {

        private bool IsSecuredFoldersInstalled
        {
            get { return ViewState["IsSecuredFoldersInstalled"] != null ? (bool)ViewState["IsSecuredFoldersInstalled"] : false; }
            set { ViewState["IsSecuredFoldersInstalled"] = value; }
        }

        private HeliconApeStatus HeliconApeStatus
        {
            get
            {
                if (null == ViewState["HeliconApeStatus"])
                {
                    HeliconApeStatus nullstatus = new HeliconApeStatus();
                    return nullstatus;
                }
                else
                {
                    return (HeliconApeStatus)ViewState["HeliconApeStatus"];    
                }
                
                
            }
            set { ViewState["HeliconApeStatus"] = value; }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterClientScriptInclude("jquery", ResolveUrl("~/JavaScript/jquery-1.4.4.min.js"));

            if (HeliconApeStatus.IsInstalled)
            {
                if (!IsPostBack)
                {
                    WebSite site = null;
                    try
                    {
                        site = ES.Services.WebServers.GetWebSite(PanelRequest.ItemID);
                    }
                    catch (Exception ex)
                    {
                        HostModule.ShowErrorMessage("WEB_GET_SITE", ex);
                        return;
                    }

                    if (site == null)
                        RedirectToBrowsePage();

                    BindHeliconApe(site);
                }
            }
        }

        public void BindHeliconApe(WebSite site)
        {
            // save initial state
            this.IsSecuredFoldersInstalled = site.SecuredFoldersInstalled;
            this.HeliconApeStatus = site.HeliconApeStatus;

           
            // Render a warning message about the automatic site's settings change
            if (site.IIs7)
            {
                if (!HeliconApeStatus.IsEnabled)
                {
                    // Ensure the message is displayed only when neccessary
                    if (site.EnableWindowsAuthentication || !site.AspNetInstalled.EndsWith("I") || site.SecuredFoldersInstalled)
                    {
                        // TODO: show warning, do not force to enable integrated pool
                        string warningStr = GetLocalizedString("EnableFoldersIIs7Warning.Text");
                        // Render a warning only if specified
                        if (!String.IsNullOrEmpty(warningStr))
                            btnToggleHeliconApe.OnClientClick = String.Format("return confirm('{0}')", warningStr);
                    }
                   

                }

            }
            // toggle
            ToggleControls();
        }

        private void ToggleControls()
        {
            if (HeliconApeStatus.IsInstalled)
            {
                bool IsHeliconApeEnabled = HeliconApeStatus.IsEnabled;

                // toggle button
                if (IsHeliconApeEnabled)
                {
                    btnToggleHeliconApe.Text = GetLocalizedString("DisableHeliconApe.Text");
                }
                else
                {
                    btnToggleHeliconApe.Text = GetLocalizedString("EnableHeliconApe.Text");
                }
                
             

                // toggle panels
                HeliconApeFoldersPanel.Visible = IsHeliconApeEnabled;
                panelHeliconApeIsNotEnabledMessage.Visible = !IsHeliconApeEnabled;

                // bind items
                if (IsHeliconApeEnabled)
                {
                    BindFolders();
                    BindUsers();
                    BindGroups();
                }
            }
            else
            {
                // Display the module not installed message for informational purposes.
                panelHeliconApeIsNotInstalledMessage.Visible = true;
                //
                btnToggleHeliconApe.Visible = false;
                HeliconApeFoldersPanel.Visible = false;
            }
        }

        private void BindFolders()
        {
            gvHeliconApeFolders.DataSource = ES.Services.WebServers.GetHeliconApeFolders(PanelRequest.ItemID);
            gvHeliconApeFolders.DataBind();
        }

        private void BindUsers()
        {
            gvHeliconApeUsers.DataSource = ES.Services.WebServers.GetHeliconApeUsers(PanelRequest.ItemID);
            gvHeliconApeUsers.DataBind();
        }

        private void BindGroups()
        {
            gvHeliconApeGroups.DataSource = ES.Services.WebServers.GetHeliconApeGroups(PanelRequest.ItemID);
            gvHeliconApeGroups.DataBind();
        }


        protected void btnToggleHeliconApe_Click(object sender, EventArgs e)
        {
            try
            {
                int result = 0;
                if (HeliconApeStatus.IsEnabled)
                {
                    // uninstall folders
                    result = ES.Services.WebServers.DisableHeliconApe(PanelRequest.ItemID);
                }
                else
                {
                    // install folders
                    result = ES.Services.WebServers.EnableHeliconApe(PanelRequest.ItemID);
                }

                if (result < 0)
                {
                    HostModule.ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                HostModule.ShowErrorMessage("WEB_INSTALL_HTACCESS", ex);
                return;
            }

            // change state
            HeliconApeStatus status = HeliconApeStatus;
            status.IsEnabled = !status.IsEnabled;

            HeliconApeStatus = status;

            // bind items
            ToggleControls();
        }

        public string GetEditUrl(string ctrlKey, string name)
        {
            name = Server.UrlEncode(name);
            return HostModule.EditUrl("ItemID", PanelRequest.ItemID.ToString(), ctrlKey,
                "Name=" + name);
        }

        protected void btnAddHeliconApeFolder_Click(object sender, EventArgs e)
        {
            RedirectToEditControl("edit_htaccessfolder", "");
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
                int result = ES.Services.WebServers.DeleteHeliconApeFolder(PanelRequest.ItemID, name);
                if (result < 0)
                {
                    HostModule.ShowResultMessage(result);
                    return false;
                }
            }
            catch (Exception ex)
            {
                HostModule.ShowErrorMessage("WEB_DELETE_HELICON_APE_FOLDER", ex);
                return false;
            }
            return true;
        }

        protected void gvHeliconApeFolders_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // delete folder
            string folderName = (string)gvHeliconApeFolders.DataKeys[e.RowIndex][1];

            if (DeleteFolder(folderName))
            {
                // reb-bind folders
                BindFolders();
            }

            // cancel command
            e.Cancel = true;
        }

        protected void btnAddHeliconApeUser_Click(object sender, EventArgs e)
        {
            RedirectToEditControl("edit_htaccessuser", "");
        }

        protected void btnAddHeliconApeGroup_Click(object sender, EventArgs e)
        {
            RedirectToEditControl("edit_htaccessgroup", "");
        }

        private bool DeleteUser(string name)
        {
            try
            {
                int result = ES.Services.WebServers.DeleteHeliconApeUser(PanelRequest.ItemID, name);
                if (result < 0)
                {
                    HostModule.ShowResultMessage(result);
                    return false;
                }
            }
            catch (Exception ex)
            {
                HostModule.ShowErrorMessage("WEB_DELETE_HeliconApe_USER", ex);
                return false;
            }
            return true;
        }

        protected void gvHeliconApeUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // delete user
            string name = (string)gvHeliconApeUsers.DataKeys[e.RowIndex][0];

            if (DeleteUser(name))
            {
                // reb-bind users
                BindUsers();
            }

            // cancel command
            e.Cancel = true;
        }

        private bool DeleteGroup(string name)
        {
            try
            {
                int result = ES.Services.WebServers.DeleteHeliconApeGroup(PanelRequest.ItemID, name);
                if (result < 0)
                {
                    HostModule.ShowResultMessage(result);
                    return false;
                }
            }
            catch (Exception ex)
            {
                HostModule.ShowErrorMessage("WEB_DELETE_HeliconApe_GROUP", ex);
                return false;
            }
            return true;
        }

        protected void gvHeliconApeGroups_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // delete group
            string name = (string)gvHeliconApeGroups.DataKeys[e.RowIndex][0];

            if (DeleteGroup(name))
            {
                // reb-bind groups
                BindGroups();
            }

            // cancel command
            e.Cancel = true;
        }

        protected string GetHtaccessPathOnSite(string path)
        {
            path = path.Replace('\\', '/');
            if (!path.EndsWith("/"))
            {
                path += "/";
            }

            path += ".htaccess";

            return path;
        }
    }
}
