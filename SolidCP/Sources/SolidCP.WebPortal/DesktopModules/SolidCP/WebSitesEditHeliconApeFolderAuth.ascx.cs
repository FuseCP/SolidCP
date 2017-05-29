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
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SolidCP.Providers.Web;

namespace SolidCP.Portal
{
    public partial class WebSitesEditHeliconApeFolderAuth : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUsers();
                BindGroups();

                // bind folder
                BindFolder();
            }
        }

        private void BindFolder()
        {
            // read web site
            WebSite site = ES.Services.WebServers.GetWebSite(PanelRequest.ItemID);
            if (site == null)
                RedirectToBrowsePage();

            folderPath.RootFolder = site.ContentPath;
            folderPath.PackageId = site.PackageId;

            if (String.IsNullOrEmpty(PanelRequest.Name))
                return;

            // read folder
            HtaccessFolder folder = ES.Services.WebServers.GetHeliconApeFolder(PanelRequest.ItemID, PanelRequest.Name);
            if(folder == null)
                ReturnBack();

            txtTitle.Text = folder.AuthName;
            folderPath.SelectedFile = folder.Path;
            folderPath.Enabled = false;

            // AuthType
            rblAuthType.Items.Clear();
            foreach (string authType in HtaccessFolder.AUTH_TYPES)
            {
                rblAuthType.Items.Add(new ListItem(authType, authType));
            }

            for (int i = 0; i < rblAuthType.Items.Count; i++)
            {
                ListItem item = rblAuthType.Items[i];
                if (item.Value == folder.AuthType)
                {
                    rblAuthType.SelectedIndex = i;
                    break;
                }
            }

            // users
            foreach (string user in folder.Users)
            {
                ListItem li = dlUsers.Items.FindByValue(user);
                if (li != null) li.Selected = true;
            }
            if (folder.ValidUser)
            {
                ListItem li = dlUsers.Items.FindByText(HtaccessFolder.VALID_USER);
                if (li != null) li.Selected = true;
            }

            // groups
            foreach (string group in folder.Groups)
            {
                ListItem li = dlGroups.Items.FindByValue(group);
                if (li != null) li.Selected = true;
            }
        }

        private void BindUsers()
        {
            List<WebUser> webUsers = new List<WebUser>(ES.Services.WebServers.GetHeliconApeUsers(PanelRequest.ItemID));
            webUsers.Add(new WebUser{Name = HtaccessFolder.VALID_USER});
            dlUsers.DataSource = webUsers;
            dlUsers.DataBind();
        }

        private void BindGroups()
        {
            dlGroups.DataSource = ES.Services.WebServers.GetHeliconApeGroups(PanelRequest.ItemID);
            dlGroups.DataBind();
        }

        private void SaveFolder()
        {
            HtaccessFolder folder;
            WebSite site = ES.Services.WebServers.GetWebSite(PanelRequest.ItemID);
            if (null != site && !String.IsNullOrEmpty(PanelRequest.Name))
            {
                folder = ES.Services.WebServers.GetHeliconApeFolder(PanelRequest.ItemID, PanelRequest.Name);
            }
            else
            {
                folder = new HtaccessFolder();
            }

            folder.AuthName = txtTitle.Text.Trim();
            folder.AuthType = rblAuthType.SelectedItem.Value;
            
            // readonly
            // folder.Path = folderPath.SelectedFile;

            List<string> users = new List<string>();
            foreach (ListItem li in dlUsers.Items)
                if (li.Selected)
                    users.Add(li.Value);

            List<string> groups = new List<string>();
            foreach (ListItem li in dlGroups.Items)
                if (li.Selected)
                    groups.Add(li.Value);

            folder.Users = users;//.ToArray();
            folder.Groups = groups;//.ToArray();

            folder.DoAuthUpdate = true;

            try
            {
                int result = ES.Services.WebServers.UpdateHeliconApeFolder(PanelRequest.ItemID, folder);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_UPDATE_HeliconApe_FOLDER", ex);
                return;
            }

            ReturnBack();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveFolder();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReturnBack();
        }

        private void ReturnBack()
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "edit_item",
                "MenuID=htaccessfolders",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }
    }
}
