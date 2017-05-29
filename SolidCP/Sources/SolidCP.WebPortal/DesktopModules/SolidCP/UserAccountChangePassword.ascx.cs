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

using SolidCP.EnterpriseServer;

namespace SolidCP.Portal
{
    public partial class UserAccountChangePassword : SolidCPModuleBase
    {
        const string changePasswordWarningKey = "LoggedUserEditDetails.ChangePasswordWarning";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUser();
            }
        }

        private void BindUser()
        {
            try
            {
                UserInfo user = UsersHelper.GetUser(PanelSecurity.SelectedUserId);
                if (user != null)
                {
                    // account info
                    lblUsername.Text = user.Username;

                    // password policy
                    userPassword.SetUserPolicy(PanelSecurity.SelectedUserId, UserSettings.SolidCP_POLICY, "PasswordPolicy");
                    userPassword.ValidationGroup = "NewPassword";
                }
                else
                {
                    // can't be found
                    RedirectBack();
                }

                if (PanelSecurity.LoggedUserId == PanelSecurity.SelectedUserId)
                {
                    trChangePasswordWarning.Visible = true;
                    string changePasswordWarningText = GetSharedLocalizedString(changePasswordWarningKey);
                    if (!String.IsNullOrEmpty(changePasswordWarningText))
                        lblChangePasswordWarning.Text = changePasswordWarningText;
                }

                if (PanelRequest.GetBool("onetimepassword"))
                {
                    ShowWarningMessage("USER_SHOULD_CHANGE_ONETIMEPASSWORD");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("USER_GET_USER", ex);
                return;
            }
        }

        protected void cmdChangePassword_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                //int result = UsersHelper.ChangeUserPassword(PortalId, PanelRequest.UserID, userPassword.Password);
                int result = PortalUtils.ChangeUserPassword(PanelRequest.UserID, userPassword.Password);

                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                ShowSuccessMessage("USER_CHANGE_PASSWORD");
                if (PanelSecurity.SelectedUserId == PanelSecurity.LoggedUserId)
                {
                    const int redirectTimeout = Utils.CHANGE_PASSWORD_REDIRECT_TIMEOUT;
                    PasswordPanel.Visible = false;
                    string loginClientUrl = Page.ResolveClientUrl(PortalUtils.LoginRedirectUrl);
                    ShowSuccessMessage(Utils.ModuleName, "LOGGED_USER_CHANGE_PASSWORD", loginClientUrl, (redirectTimeout / 1000).ToString());
                    FormsAuthentication.SignOut();
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "RedirectToLogin", String.Format("setTimeout(\"window.location='{0}'\",{1});", loginClientUrl, redirectTimeout), true); 
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("USER_CHANGE_PASSWORD", ex);
                return;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectBack();
        }

        private void RedirectBack()
        {
            Response.Redirect(NavigateURL(PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString()));
        }
    }
}
