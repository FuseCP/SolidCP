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
    public partial class PeersEditPeer : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUser();

                if (PortalUtils.GetHideDemoCheckbox()) chkDemo.Visible = lblDemoAccount.Visible= false;
            }
        }

        private void BindUser()
        {
            ShowControls(PanelRequest.PeerID != 0);

            if (PanelRequest.PeerID == 0)
            {
                userPassword.SetUserPolicy(PanelSecurity.SelectedUserId, UserSettings.SolidCP_POLICY, "PasswordPolicy");
                userPassword.ValidationGroup = "";

                if (PanelSecurity.SelectedUser.RoleId == (int)UserRole.Administrator)
                {
                    role.Items.Add("CSR");
                    role.Items.Add("Helpdesk");
                    role.Items.Add("Administrator");
                }
                else
                    if (PanelSecurity.SelectedUser.RoleId == (int)UserRole.Reseller)
                    {
                        role.Items.Add("CSR");
                        role.Items.Add("Helpdesk");
                        role.Items.Add("Reseller");
                    }
                    else
                        rowRole.Visible = false;

                return; // it's a new user
            }

            if (PanelSecurity.LoggedUser.IsPeer && PanelSecurity.LoggedUserId == PanelRequest.PeerID)
                btnDelete.Visible = false; // peer can't delete his own account
            
            UserInfo user = UsersHelper.GetUser(PanelRequest.PeerID);
            if (user != null)
            {
                if ((PanelSecurity.SelectedUser.RoleId == (int)UserRole.Administrator)|
                    (PanelSecurity.SelectedUser.RoleId == (int)UserRole.PlatformCSR)|
                    (PanelSecurity.SelectedUser.RoleId == (int)UserRole.PlatformHelpdesk))
                {
                    role.Items.Add("CSR");
                    role.Items.Add("Helpdesk");
                    role.Items.Add("Administrator");
                }
                else
                    if ((PanelSecurity.SelectedUser.RoleId == (int)UserRole.Reseller)|
                        (PanelSecurity.SelectedUser.RoleId == (int)UserRole.ResellerCSR)|
                        (PanelSecurity.SelectedUser.RoleId == (int)UserRole.ResellerHelpdesk))
                    {
                        role.Items.Add("CSR");
                        role.Items.Add("Helpdesk");
                        role.Items.Add("Reseller");
                    }
                    else
                        rowRole.Visible = false;

                userPassword.SetUserPolicy(PanelSecurity.SelectedUserId, UserSettings.SolidCP_POLICY, "PasswordPolicy");
                userPassword.ValidationGroup = "NewPassword";

                // account info
                txtFirstName.Text = PortalAntiXSS.DecodeOld(user.FirstName);
                txtLastName.Text = PortalAntiXSS.DecodeOld(user.LastName);
                txtEmail.Text = user.Email;
                txtSecondaryEmail.Text = user.SecondaryEmail;
                ddlMailFormat.SelectedIndex = user.HtmlMail ? 1 : 0;
                txtUsername.Text = user.Username;
                lblUsername.Text = user.Username;
                chkDemo.Checked = user.IsDemo;
                cbxMfaEnabled.Checked = user.MfaMode > 0 ? true : false;
                cbxMfaEnabled.Enabled = ES.Services.Users.CanUserChangeMfa(PanelRequest.PeerID);
                lblMfaEnabled.Visible = cbxMfaEnabled.Checked;


                if (user.RoleId == (int)UserRole.ResellerCSR) role.SelectedIndex = 0;
                if (user.RoleId == (int)UserRole.PlatformCSR) role.SelectedIndex = 0;
                if (user.RoleId == (int)UserRole.PlatformHelpdesk) role.SelectedIndex = 1;
                if (user.RoleId == (int)UserRole.ResellerHelpdesk) role.SelectedIndex = 1;
                if (user.RoleId == (int)UserRole.Reseller) role.SelectedIndex = 2;
                if (user.RoleId == (int)UserRole.Administrator) role.SelectedIndex = 2;

                // select loginStatus
                loginStatus.SelectedIndex = user.LoginStatusId;

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
            }
            else
            {
                // can't be found
                RedirectBack();
            }
        }

        private void SaveUser()
        {
            // get owner
            UserInfo owner = PanelSecurity.SelectedUser;

            if (Page.IsValid)
            {
                // gather data from form
                UserInfo user = new UserInfo();
                user.UserId = PanelRequest.PeerID;

                if (PanelSecurity.SelectedUser.RoleId == (int)UserRole.Administrator)
                {
                    if (role.SelectedIndex == 0)
                        user.RoleId = (int)UserRole.PlatformCSR;
                    if (role.SelectedIndex == 1)
                        user.RoleId = (int)UserRole.PlatformHelpdesk;
                    if (role.SelectedIndex == 2)
                        user.RoleId = (int)UserRole.Administrator;
                }
                else
                    if (PanelSecurity.SelectedUser.RoleId == (int)UserRole.Reseller)
                    {
                        if (role.SelectedIndex == 0)
                            user.RoleId = (int)UserRole.ResellerCSR;
                        if (role.SelectedIndex == 1)
                            user.RoleId = (int)UserRole.ResellerHelpdesk;
                        if (role.SelectedIndex == 2)
                            user.RoleId = (int)UserRole.Reseller;
                    }
                    else
                        user.RoleId = owner.RoleId;

                user.StatusId = owner.StatusId;

                user.OwnerId = owner.UserId;
                user.IsDemo = owner.IsDemo;
                user.IsPeer = true;

                // account info
                user.FirstName = txtFirstName.Text;
                user.LastName = txtLastName.Text;
                user.Email = txtEmail.Text;
                user.SecondaryEmail = txtSecondaryEmail.Text;
                user.HtmlMail = ddlMailFormat.SelectedIndex == 1;
                user.Username = txtUsername.Text;
//                user.Password = userPassword.Password;
                user.IsDemo = chkDemo.Checked;
                
                user.LoginStatusId = loginStatus.SelectedIndex;

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

                if (PanelRequest.PeerID == 0)
                {
                    // add a new peer
                    List<string> log = new List<string>();

                    try
                    {
                        //int userId = UsersHelper.AddUser(log, PortalId, user);
                        int userId = PortalUtils.AddUserAccount(log, user, false, userPassword.Password);

                        if (userId < 0)
                        {
                            ShowResultMessage(userId);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage("PEER_ADD_PEER", ex);
                        return;
                    }

                    // show lof records if any
                    if (log.Count > 0)
                    {
                        blLog.Items.Clear();
                        foreach (string error in log)
                            blLog.Items.Add(error);

                        return;
                    }
                }
                else
                {
                    // update existing user
                    try
                    {
                        //int result = UsersHelper.UpdateUser(PortalId, user);
                        int result = PortalUtils.UpdateUserAccount(user);

                        if (result < 0)
                        {
                            ShowResultMessage(result);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage("PEER_UPDATE_PEER", ex);
                        return;
                    }
                }

                // return back to the list
                RedirectBack();
            }
        }

        private void DeleteUser()
        {
            try
            {
                //int result = UsersHelper.DeleteUser(PortalId, PanelRequest.UserID);
                int result = PortalUtils.DeleteUserAccount(PanelRequest.PeerID);

                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("PEER_DELETE_PEER", ex);
                return;
            }

            // return
            RedirectBack();
        }

        private void ChangeUserPassword()
        {
            if (!Page.IsValid)
                return;

            try
            {
                //int result = UsersHelper.ChangeUserPassword(PortalId, PanelRequest.UserID, userPassword.Password);
                int result = PortalUtils.ChangeUserPassword(PanelRequest.PeerID, userPassword.Password);

                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                ShowSuccessMessage("PEER_CHANGE_PASSWORD");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("PEER_CHANGE_PASSWORD", ex);
                return;
            }
        }


        private void ShowControls(bool editMode)
        {
            rowChangePassword.Visible = editMode;
            rowUsername.Visible = !editMode;
            rowUsernameReadonly.Visible = editMode;
            btnDelete.Visible = editMode;
            btnUpdate.Text = editMode ? GetLocalizedString("Text.Update") : GetLocalizedString("Text.Add");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveUser();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectBack();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteUser();
        }

        protected void cmdChangePassword_Click(object sender, EventArgs e)
        {
            // change password
            ChangeUserPassword();
        }

        private void RedirectBack()
        {
            Response.Redirect(NavigateURL(PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString()));
        }

        protected void cbxMfaEnabled_CheckedChanged(object sender, EventArgs e)
        {
            UserInfo user = ES.Services.Users.GetUserById(PanelRequest.PeerID);
            bool result = PortalUtils.UpdateUserMfa(user.Username, cbxMfaEnabled.Checked);
            lblMfaEnabled.Visible = result;
            cbxMfaEnabled.Checked = result;
        }
    }
}
