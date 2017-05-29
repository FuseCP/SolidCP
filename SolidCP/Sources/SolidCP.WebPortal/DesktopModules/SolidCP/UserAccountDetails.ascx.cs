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
    public partial class UserAccountDetails : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindAccount();
        }

        private void BindAccount()
        {
            // load user
            UserInfo user = UsersHelper.GetUser(PanelSecurity.SelectedUserId);
            if (user != null)
            {
                imgAdmin.Visible = (user.Role == UserRole.Administrator);
                imgReseller.Visible = (user.Role == UserRole.Reseller);
                imgUser.Visible = (user.Role == UserRole.User);

                // bind account details
                litUsername.Text = user.Username;
                litFullName.Text = Utils.EllipsisString(PortalAntiXSS.Encode(user.FirstName) + " " + PortalAntiXSS.Encode(user.LastName), 25);
                litSubscriberNumber.Text = PortalAntiXSS.Encode(user.SubscriberNumber);
                litRole.Text = PanelFormatter.GetUserRoleName(user.RoleId);
                litCreated.Text = user.Created.ToString();
                litUpdated.Text = user.Changed.ToString();
				lnkEmail.Text = Utils.EllipsisString(user.Email, 25);
                lnkEmail.NavigateUrl = "mailto:" + user.Email;

                // load owner account
                //UserInfo owner = UsersHelper.GetUser(user.OwnerId);
                //if(owner != null)
                //{
                //    litReseller.Text = owner.Username;
                //}


                // bind account status
                UserStatus status = user.Status;
                litStatus.Text = PanelFormatter.GetAccountStatusName((int)status);

                cmdActive.Visible = (status != UserStatus.Active);
                cmdSuspend.Visible = (status == UserStatus.Active);
                cmdCancel.Visible = (status != UserStatus.Cancelled);

                StatusBlock.Visible = (PanelSecurity.SelectedUserId != PanelSecurity.EffectiveUserId);



                // links
                lnkSummaryLetter.NavigateUrl = EditUrl("UserID", PanelSecurity.SelectedUserId.ToString(), "summary_letter");
                lnkSummaryLetter.Visible = (PanelSecurity.SelectedUser.Role != UserRole.Administrator);

                lnkEditAccountDetails.NavigateUrl = EditUrl("UserID", PanelSecurity.SelectedUserId.ToString(), "edit_details");

                lnkChangePassword.NavigateUrl = EditUrl("UserID", PanelSecurity.SelectedUserId.ToString(), "change_password");
                lnkChangePassword.Visible = !((PanelSecurity.SelectedUserId == PanelSecurity.EffectiveUserId) && PanelSecurity.LoggedUser.IsPeer);

                lnkDelete.NavigateUrl = EditUrl("UserID", PanelSecurity.SelectedUserId.ToString(), "delete");

                if (!((PanelSecurity.LoggedUser.Role == UserRole.Reseller) | (PanelSecurity.LoggedUser.Role == UserRole.Administrator))) 
                    lnkDelete.Visible = false;
                else 
                    lnkDelete.Visible = (PanelSecurity.SelectedUserId != PanelSecurity.EffectiveUserId);
            }
        }

        protected void statusButton_Click(object sender, ImageClickEventArgs e)
        {
            string sStatus = ((ImageButton)sender).CommandName;
            UserStatus status = (UserStatus)Enum.Parse(typeof(UserStatus), sStatus, true);

            // chanhe user status
            try
            {
                int result = PortalUtils.ChangeUserStatus(PanelSecurity.SelectedUserId, status);

                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("USER_CHANGE_STATUS", ex);
                return;
            }

            // re-bind user
            BindAccount();
        }
    }
}
