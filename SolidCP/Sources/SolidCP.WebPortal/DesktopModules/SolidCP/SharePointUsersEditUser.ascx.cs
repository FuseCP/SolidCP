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

using SolidCP.EnterpriseServer;
using SolidCP.Providers.OS;

namespace SolidCP.Portal
{
    public partial class SharePointUsersEditUser : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnDelete.Visible = (PanelRequest.ItemID > 0);

            if (!IsPostBack)
            {
                BindItem();
            }
        }

        private void BindGroups(int packageId)
        {
            try
            {
                SystemGroup[] groups = ES.Services.SharePointServers.GetSharePointGroups(packageId, false);
                dlGroups.DataSource = groups;
                dlGroups.DataBind();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SHAREPOINT_GET_GROUP", ex);
            }
        }

        private void BindItem()
        {
            if (PanelRequest.ItemID == 0)
            {
                usernameControl.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.SHAREPOINT_POLICY, "UserNamePolicy");
                passwordControl.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.SHAREPOINT_POLICY, "UserPasswordPolicy");
                BindGroups(PanelSecurity.PackageId);
                return;
            }

            // load item
            SystemUser item = null;
            try
            {
                item = ES.Services.SharePointServers.GetSharePointUser(PanelRequest.ItemID);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SHAREPOINT_GET_USER", ex);
                return;
            }

            if (item == null)
                RedirectToBrowsePage();

            BindGroups(item.PackageId);
            usernameControl.SetPackagePolicy(item.PackageId, UserSettings.SHAREPOINT_POLICY, "UserNamePolicy");
            passwordControl.SetPackagePolicy(item.PackageId, UserSettings.SHAREPOINT_POLICY, "UserPasswordPolicy");
            usernameControl.Text = item.Name;
            usernameControl.EditMode = true;
            passwordControl.EditMode = true;

            foreach (string group in item.MemberOf)
            {
                ListItem li = dlGroups.Items.FindByValue(group);
                if (li != null)
                    li.Selected = true;
            }
        }

        private void SaveItem()
        {
            if (!Page.IsValid)
                return;

            // get form data
            SystemUser item = new SystemUser();
            item.Id = PanelRequest.ItemID;
            item.PackageId = PanelSecurity.PackageId;
            item.Name = usernameControl.Text;
            item.Password = passwordControl.Password;

            List<string> memberOf = new List<string>();
            foreach (ListItem li in dlGroups.Items)
            {
                if (li.Selected)
                    memberOf.Add(li.Value);
            }
            item.MemberOf = memberOf.ToArray();

            if (PanelRequest.ItemID == 0)
            {
                // new item
                try
                {
                    int result = ES.Services.SharePointServers.AddSharePointUser(item);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SHAREPOINT_ADD_USER", ex);
                    return;
                }
            }
            else
            {
                // existing item
                try
                {
                    int result = ES.Services.SharePointServers.UpdateSharePointUser(item);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SHAREPOINT_UPDATE_USER", ex);
                    return;
                }
            }

            // return
            RedirectSpaceHomePage();
        }

        private void DeleteItem()
        {
            // delete
            try
            {
                int result = ES.Services.SharePointServers.DeleteSharePointUser(PanelRequest.ItemID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SHAREPOINT_DELETE_USER", ex);
                return;
            }

            // return
            RedirectSpaceHomePage();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveItem();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectSpaceHomePage();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }
    }
}
