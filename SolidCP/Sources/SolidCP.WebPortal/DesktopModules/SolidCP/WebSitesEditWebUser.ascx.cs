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
    public partial class WebSitesEditWebUser : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGroups();

                // bind user
                BindUser();

				// set policies
				usernameControl.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.WEB_POLICY, "SecuredUserNamePolicy");
				passwordControl.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.WEB_POLICY, "SecuredUserPasswordPolicy");
            }
        }

        private void BindGroups()
        {
            dlGroups.DataSource = ES.Services.WebServers.GetSecuredGroups(PanelRequest.ItemID);
            dlGroups.DataBind();
        }

        private void BindUser()
        {
            if (String.IsNullOrEmpty(PanelRequest.Name))
                return;

            // read user
            WebUser user = ES.Services.WebServers.GetSecuredUser(PanelRequest.ItemID, PanelRequest.Name);
            if (user == null)
                ReturnBack();

            usernameControl.Text = user.Name;
            usernameControl.EditMode = true;
            passwordControl.EditMode = true;

            // groups
            foreach (string group in user.Groups)
            {
                ListItem li = dlGroups.Items.FindByValue(group);
                if (li != null) li.Selected = true;
            }
        }

        private void SaveUser()
        {
            WebUser user = new WebUser();
            user.Name = usernameControl.Text;
            user.Password = passwordControl.Password;

            List<string> groups = new List<string>();
            foreach (ListItem li in dlGroups.Items)
                if (li.Selected)
                    groups.Add(li.Value);

            user.Groups = groups.ToArray();

            try
            {
                int result = ES.Services.WebServers.UpdateSecuredUser(PanelRequest.ItemID, user);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_UPDATE_SECURED_USER", ex);
                return;
            }

            ReturnBack();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveUser();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReturnBack();
        }

        private void ReturnBack()
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "edit_item",
                "MenuID=securedfolders",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }
    }
}
