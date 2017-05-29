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

namespace SolidCP.Portal
{
    public partial class UserCreateUserAccount : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // bind roles
                BindForm();
                BindRoles(PanelSecurity.SelectedUser);
            }
        }

        private void BindForm()
        {
            UserSettings settings = ES.Services.Users.GetUserSettings(PanelSecurity.LoggedUserId,
                UserSettings.ACCOUNT_SUMMARY_LETTER);
            bool accountSummaryEmailEnabled = !String.IsNullOrEmpty(settings["EnableLetter"]) &&
                                              Utils.ParseBool(settings["EnableLetter"], false);
            this.chkAccountLetter.Enabled = accountSummaryEmailEnabled;
            this.pnlDisabledSummaryLetterHint.Visible = !accountSummaryEmailEnabled;
            if (PortalUtils.GetHideDemoCheckbox())
                this.lblDemoAccount.Visible = this.chkDemo.Checked = this.chkDemo.Visible = false;

            //reseller.UserId = PanelSecurity.SelectedUserId;
            userPassword.SetUserPolicy(PanelSecurity.SelectedUserId, UserSettings.SolidCP_POLICY, "PasswordPolicy");
        }

        private void BindRoles(UserInfo user)
        {
            if (user.Role == UserRole.User)
                role.Items.Remove("Reseller");

            if ((PanelSecurity.LoggedUser.Role == UserRole.ResellerCSR) |
                (PanelSecurity.LoggedUser.Role == UserRole.ResellerHelpdesk))
                role.Items.Remove("Reseller");
        }

        private void SaveUser()
        {
            if (!Page.IsValid)
                return;

            // gather data from form
            UserInfo user = new UserInfo();
            user.UserId = 0;
            user.Role = (UserRole) Enum.Parse(typeof (UserRole), role.SelectedValue);
            user.StatusId = Int32.Parse(status.SelectedValue);
            user.OwnerId = PanelSecurity.SelectedUserId;
            user.IsDemo = chkDemo.Checked;
            user.IsPeer = false;

            // account info
            user.FirstName = txtFirstName.Text;
            user.LastName = txtLastName.Text;
            user.SubscriberNumber = txtSubscriberNumber.Text;
            user.Email = txtEmail.Text;
            user.SecondaryEmail = txtSecondaryEmail.Text;
            user.HtmlMail = ddlMailFormat.SelectedIndex == 1;
            user.Username = txtUsername.Text.Trim();
//            user.Password = userPassword.Password;

            // contact info
            user.CompanyName = contact.CompanyName;
            user.Address = contact.Address;
            user.City = contact.City;
            user.Country = contact.Country;
            user.State = contact.State;
            user.Zip = contact.Zip;
            user.PrimaryPhone = contact.PrimaryPhone;
            user.SecondaryPhone = contact.SecondaryPhone;
            user.Fax = contact.Fax;
            user.InstantMessenger = contact.MessengerId;


            // add a new user
            List<string> log = new List<string>();
            try
            {
                //int userId = UsersHelper.AddUser(log, PortalId, user);
                int userId = PortalUtils.AddUserAccount(log, user, chkAccountLetter.Checked, userPassword.Password);

                if (userId == BusinessErrorCodes.ERROR_INVALID_USER_NAME)
                {
                    ShowResultMessage(BusinessErrorCodes.ERROR_INVALID_USER_NAME);
                    return;
                }

                if (userId < 0)
                {
                    ShowResultMessage(userId);
                    return;
                }

                // show log records if any
                if (log.Count > 0)
                {
                    blLog.Items.Clear();
                    foreach (string error in log)
                        blLog.Items.Add(error);

                    return;
                }

                // go to user home
                Response.Redirect(PortalUtils.GetUserHomePageUrl(userId));
            }
            catch (Exception ex)
            {
                ShowErrorMessage("USER_ADD_USER", ex);
                return;
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            SaveUser();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(NavigateURL(PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString()));
        }
    }
}
