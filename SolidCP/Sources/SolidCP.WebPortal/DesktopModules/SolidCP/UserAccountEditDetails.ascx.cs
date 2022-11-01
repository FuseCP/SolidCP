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
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal
{
    public partial class UserAccountEditDetails : SolidCPModuleBase
    {
        private const string UserStatusConst = "UserStatus";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUser();

                if (PortalUtils.GetHideDemoCheckbox()) rowDemo.Visible = false;
            }

            if (PanelSecurity.LoggedUser.Role == UserRole.User)
            {
                txtSubscriberNumber.ReadOnly = true;
            }


        }

        private void BindUser()
        {
            try
            {
                UserInfo user = UsersHelper.GetUser(PanelSecurity.SelectedUserId);
                if (user != null)
                {
                    // bind roles
                    BindRoles(PanelSecurity.EffectiveUserId);

                    bool editAdminAccount = (user.UserId == PanelSecurity.EffectiveUserId);

                    if(!editAdminAccount)
                        role.Items.Remove("Administrator");

                    // select role
                    Utils.SelectListItem(role, user.Role.ToString());

                    // select loginStatus
                    loginStatus.SelectedIndex = user.LoginStatusId;

                    rowRole.Visible = !editAdminAccount;

                    // select status
                    chkDemo.Checked = user.IsDemo;
                    rowDemo.Visible = !editAdminAccount;

                    // account info
                    txtFirstName.Text = PortalAntiXSS.DecodeOld(user.FirstName);
                    txtLastName.Text = PortalAntiXSS.DecodeOld(user.LastName);
                    txtSubscriberNumber.Text = PortalAntiXSS.DecodeOld(user.SubscriberNumber);
                    txtEmail.Text = user.Email;
                    txtSecondaryEmail.Text = user.SecondaryEmail;
                    ddlMailFormat.SelectedIndex = user.HtmlMail ? 1 : 0;
                    lblUsername.Text = user.Username;
                    cbxMfaEnabled.Checked = user.MfaMode > 0 ? true: false;

                    // contact info
                    contact.CompanyName = user.CompanyName;
                    contact.Address = user.Address;
                    contact.City = user.City;
                    contact.Country = user.Country;
                    contact.State = user.State;
                    contact.Zip = user.Zip;
                    contact.PrimaryPhone = user.PrimaryPhone;
                    contact.SecondaryPhone = user.SecondaryPhone;
                    contact.Fax = user.Fax;
                    contact.MessengerId = user.InstantMessenger;

                    ViewState[UserStatusConst] = user.Status;
                }
                else
                {
                    // can't be found
                    RedirectAccountHomePage();
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage("USER_GET_USER", ex);
                return;
            }
        }

        private void SaveUser()
        {
            if (Page.IsValid)
            {
                // gather data from form
                UserInfo user = new UserInfo();
                user.UserId = PanelSecurity.SelectedUserId;
                user.Role = (UserRole)Enum.Parse(typeof(UserRole), role.SelectedValue);
                user.IsDemo = chkDemo.Checked;
                user.Status = ViewState[UserStatusConst] != null ? (UserStatus) ViewState[UserStatusConst]: UserStatus.Active;

                user.LoginStatusId = loginStatus.SelectedIndex;
                
                // account info
                user.FirstName = txtFirstName.Text;
                user.LastName = txtLastName.Text;
                user.SubscriberNumber = txtSubscriberNumber.Text;
                user.Email = txtEmail.Text;
                user.SecondaryEmail = txtSecondaryEmail.Text;
                user.HtmlMail = ddlMailFormat.SelectedIndex == 1;

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

                // update existing user
                try
                {
                    int result = PortalUtils.UpdateUserAccount(/*TaskID, */user);

                    //int result = ES.Services.Users.UpdateUserTaskAsynchronously(TaskID, user);
                    AsyncTaskID = TaskID;

                    if (result.Equals(-102))
                    {
                        if (user.RoleId.Equals(3))
                        {
                            ShowResultMessage(result);
                            return;
                        }
                    }
                    else
                    {
                        if (result < 0)
                        {
                            ShowResultMessage(result);
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("USER_UPDATE_USER", ex);
                    return;
                }

                // return back to the list
                RedirectAccountHomePage();
            }
        }

        private void BindRoles(int userId)
        {
            // load selected user
            UserInfo user = UsersHelper.GetUser(userId);

            if (user != null)
            {
                if (user.Role == UserRole.Reseller || user.Role == UserRole.User)
                    role.Items.Remove("Administrator");
                if ((user.Role == UserRole.User) |(PanelSecurity.LoggedUser.Role == UserRole.ResellerCSR) |
                    (PanelSecurity.LoggedUser.Role == UserRole.ResellerHelpdesk))
                    role.Items.Remove("Reseller");
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveUser();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectAccountHomePage();
        }

        protected void cbxMfaEnabled_CheckedChanged(object sender, EventArgs e)
        {
            UserInfo user = ES.Services.Users.GetUserById(PanelSecurity.SelectedUserId);
            PortalUtils.UpdateUserMfa(user.Username, cbxMfaEnabled.Checked);
        }
    }
}
