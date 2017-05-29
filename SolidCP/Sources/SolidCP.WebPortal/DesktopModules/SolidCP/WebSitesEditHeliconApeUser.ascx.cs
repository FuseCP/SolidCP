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
    public partial class WebSitesEditHeliconApeUser : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGroups();

                BindAuthControls();

                // bind user
                BindUser();

				// set policies
				usernameControl.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.WEB_POLICY, "SecuredUserNamePolicy");
				passwordControl.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.WEB_POLICY, "SecuredUserPasswordPolicy");
            }
        }

        private void BindAuthControls()
        {
            // AuthType
            rblAuthType.Items.Clear();
            foreach (string authType in HtaccessFolder.AUTH_TYPES)
            {
                rblAuthType.Items.Add(new ListItem(authType, authType));
            }
            rblAuthType.SelectedIndex = 0;

            // Encoding types
            rblEncType.Items.Clear();
            foreach (string encType in HtaccessUser.ENCODING_TYPES)
            {
                rblEncType.Items.Add(new ListItem(encType, encType));
            }
            rblEncType.SelectedIndex = 0;
        }

        private void BindGroups()
        {
            dlGroups.DataSource = ES.Services.WebServers.GetHeliconApeGroups(PanelRequest.ItemID);
            dlGroups.DataBind();
        }

        private void BindUser()
        {
            if (String.IsNullOrEmpty(PanelRequest.Name))
                return;

            // read user
            HtaccessUser user = ES.Services.WebServers.GetHeliconApeUser(PanelRequest.ItemID, PanelRequest.Name);
            if (user == null)
                ReturnBack();

            usernameControl.Text = user.Name;
            usernameControl.EditMode = true;
            passwordControl.EditMode = true;

            // AuthType
            for (int i = 0; i < rblAuthType.Items.Count; i++)
            {
                ListItem item = rblAuthType.Items[i];
                if (item.Value == user.AuthType)
                {
                    rblAuthType.SelectedIndex = i;
                    break;
                }
            }

            // Encoding types
            for (int i = 0; i < rblEncType.Items.Count; i++)
            {
                ListItem item = rblEncType.Items[i];
                if (item.Value == user.EncType)
                {
                    rblEncType.SelectedIndex = i;
                    break;
                }
            }

            // groups
            foreach (string group in user.Groups)
            {
                ListItem li = dlGroups.Items.FindByValue(group);
                if (li != null) li.Selected = true;
            }
        }

        private void SaveUser()
        {
            HtaccessUser user = new HtaccessUser();
            user.Name = usernameControl.Text;
            user.Password = passwordControl.Password;
            user.AuthType = rblAuthType.SelectedItem.Value;
            user.EncType  = rblEncType.SelectedItem.Value;
            user.Realm = tbDigestRealm.Text;

            List<string> groups = new List<string>();
            foreach (ListItem li in dlGroups.Items)
                if (li.Selected)
                    groups.Add(li.Value);

            user.Groups = groups.ToArray();

            try
            {
                int result = ES.Services.WebServers.UpdateHeliconApeUser(PanelRequest.ItemID, user);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_UPDATE_HeliconApe_USER", ex);
                return;
            }

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveUser();
            ReturnBack();
        }

        protected void btnSaveAndAddAnother_Click(object sender, EventArgs e)
        {
            SaveUser();
            ClearControls();
        }

        private void ClearControls()
        {
            usernameControl.Text = string.Empty;
            usernameControl.EditMode = false;
            
            passwordControl.Password = string.Empty;
            passwordControl.EditMode = false;
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
