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
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal
{
    public partial class WebSitesEditHeliconApeGroup : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUsers();

                // bind group
                BindGroup();

				// set policies
				usernameControl.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.WEB_POLICY, "SecuredGroupNamePolicy");

            }
        }

        private void BindUsers()
        {
            dlUsers.DataSource = ES.Services.WebServers.GetHeliconApeUsers(PanelRequest.ItemID);
            dlUsers.DataBind();
        }

        private void BindGroup()
        {
            if (String.IsNullOrEmpty(PanelRequest.Name))
                return;

            // read group
            WebGroup group = ES.Services.WebServers.GetHeliconApeGroup(PanelRequest.ItemID, PanelRequest.Name);
            if (group == null)
                ReturnBack();

            usernameControl.Text = group.Name;
            usernameControl.EditMode = true;

            // users
            foreach (string user in group.Users)
            {
                ListItem li = dlUsers.Items.FindByValue(user);
                if (li != null) li.Selected = true;
            }
        }

        private void SaveGroup()
        {
            WebGroup group = new WebGroup();
            group.Name = usernameControl.Text;

            List<string> users = new List<string>();
            foreach (ListItem li in dlUsers.Items)
                if (li.Selected)
                    users.Add(li.Value);

            group.Users = users.ToArray();

            try
            {
                int result = ES.Services.WebServers.UpdateHeliconApeGroup(PanelRequest.ItemID, group);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_UPDATE_HeliconApe_GROUP", ex);
                return;
            }

            ReturnBack();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveGroup();
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
