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
using System.Web.Security;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;

namespace SolidCP.Portal.ExchangeServer
{
    public partial class ExchangeCreateMailbox : SolidCPModuleBase
    {
        private bool IsNewUser
        {
            get
            {
                return NewUserDiv.Visible;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            if (!IsPostBack)
            {
                BindPasswordSettings();

                string instructions = ES.Services.ExchangeServer.GetMailboxSetupInstructions(PanelRequest.ItemID, PanelRequest.AccountID, false, false, false, " ");
                if (!string.IsNullOrEmpty(instructions))
                {
                    chkSendInstructions.Checked = chkSendInstructions.Visible = sendInstructionEmail.Visible = true;
                    PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
                    if (package != null)
                    {
                        UserInfo user = ES.Services.Users.GetUserById(package.UserId);
                        if (user != null)
                            sendInstructionEmail.Text = user.Email;
                    }
                }
                else
                {
                    chkSendInstructions.Checked = chkSendInstructions.Visible = sendInstructionEmail.Visible = false;
                }



                SolidCP.Providers.HostedSolution.ExchangeMailboxPlan[] plans = ES.Services.ExchangeServer.GetExchangeMailboxPlans(PanelRequest.ItemID, false);

                if (plans.Length == 0)
                    btnCreate.Enabled = false;

                bool allowResourceMailbox = false;

                if (cntx.Quotas.ContainsKey(Quotas.EXCHANGE2007_ISCONSUMER))
                {
                    if (cntx.Quotas[Quotas.EXCHANGE2007_ISCONSUMER].QuotaAllocatedValue != 1)
                    {
                        locSubscriberNumber.Visible = txtSubscriberNumber.Visible = valRequireSubscriberNumber.Enabled = false;
                        allowResourceMailbox = true;
                    }
                }

                if (cntx.Quotas.ContainsKey(Quotas.EXCHANGE2013_RESOURCEMAILBOXES))
                {
                    if (cntx.Quotas[Quotas.EXCHANGE2013_RESOURCEMAILBOXES].QuotaAllocatedValue != 0)
                        allowResourceMailbox = true;
                }


                if (allowResourceMailbox)
                {
                    rbMailboxType.Items.Add(new System.Web.UI.WebControls.ListItem(GetLocalizedString("RoomMailbox.Text"), "5"));
                    rbMailboxType.Items.Add(new System.Web.UI.WebControls.ListItem(GetLocalizedString("EquipmentMailbox.Text"), "6"));
                }

                if (cntx.Quotas.ContainsKey(Quotas.EXCHANGE2013_SHAREDMAILBOXES))
                {
                    if (cntx.Quotas[Quotas.EXCHANGE2013_SHAREDMAILBOXES].QuotaAllocatedValue != 0)
                        rbMailboxType.Items.Add(new System.Web.UI.WebControls.ListItem(GetLocalizedString("SharedMailbox.Text"), "10"));
                }


                divRetentionPolicy.Visible = Utils.CheckQouta(Quotas.EXCHANGE2013_ALLOWRETENTIONPOLICY, cntx);
            }

            divArchiving.Visible = false;

            int planId = -1;
            int.TryParse(mailboxPlanSelector.MailboxPlanId, out planId);
            ExchangeMailboxPlan plan = ES.Services.ExchangeServer.GetExchangeMailboxPlan(PanelRequest.ItemID, planId);
            if (plan!=null)
                divArchiving.Visible = plan.EnableArchiving;

        }

        private void BindPasswordSettings()
        {
            var grainedPasswordSettigns = ES.Services.Organizations.GetOrganizationPasswordSettings(PanelRequest.ItemID);

            if (grainedPasswordSettigns != null)
            {
                password.SetUserPolicy(grainedPasswordSettigns);
            }
            else
            {
                messageBox.ShowErrorMessage("UNABLETOLOADPASSWORDSETTINGS");
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            CreateMailbox();
        }

        private void CreateMailbox()
        {
            if (!Page.IsValid)
                return;

            try
            {
                string name = IsNewUser ? email.AccountName : userSelector.GetPrimaryEmailAddress().Split('@')[0];
                string displayName = IsNewUser ? txtDisplayName.Text.Trim() : userSelector.GetDisplayName();
                string accountName = IsNewUser ? string.Empty : userSelector.GetAccount();

                bool enableArchive = chkEnableArchiving.Checked;

                ExchangeAccountType type = IsNewUser
                                               ? (ExchangeAccountType)Utils.ParseInt(rbMailboxType.SelectedValue, 1)
                                               : ExchangeAccountType.Mailbox;

                string domain = IsNewUser ? email.DomainName : userSelector.GetPrimaryEmailAddress().Split('@')[1];

                int accountId = IsNewUser ? 0 : userSelector.GetAccountId();

                string subscriberNumber = IsNewUser ? txtSubscriberNumber.Text.Trim() : userSelector.GetSubscriberNumber();

                var passwordString = password.Password;

                if (sendToControl.IsRequestSend && IsNewUser)
                {
                    passwordString = Membership.GeneratePassword(16, 3);
                }

                accountId = ES.Services.ExchangeServer.CreateMailbox(PanelRequest.ItemID, accountId, type,
                                    accountName,
                                    displayName,
                                    name,
                                    domain,
                                    passwordString,
                                    chkSendInstructions.Checked,
                                    sendInstructionEmail.Text,
                                    Convert.ToInt32(mailboxPlanSelector.MailboxPlanId),
                                    Convert.ToInt32(archivingMailboxPlanSelector.MailboxPlanId),
                                    subscriberNumber, enableArchive);


                if (accountId < 0)
                {
                    messageBox.ShowResultMessage(accountId);
                    return;
                }
                else
                {
                    if ((!string.IsNullOrEmpty(txtFirstName.Text)) | (!string.IsNullOrEmpty(txtLastName.Text)) | (!string.IsNullOrEmpty(txtInitials.Text)))
                    {
                        SetUserAttributes(accountId);
                    }
                }

                if (sendToControl.SendEmail && IsNewUser)
                {
                    ES.Services.Organizations.SendUserPasswordRequestEmail(PanelRequest.ItemID, accountId, "User creation", sendToControl.Email, true);
                }
                else if (sendToControl.SendMobile && IsNewUser)
                {
                    ES.Services.Organizations.SendUserPasswordRequestSms(PanelRequest.ItemID, accountId, "User creation", sendToControl.Mobile);
                }

                Response.Redirect(EditUrl("AccountID", accountId.ToString(), "mailbox_settings",
                    "SpaceID=" + PanelSecurity.PackageId.ToString(),
                    "ItemID=" + PanelRequest.ItemID.ToString()));
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_CREATE_MAILBOX", ex);
            }
        }

        private void SetUserAttributes(int accountId)
        {
            OrganizationUser user = ES.Services.Organizations.GetUserGeneralSettings(PanelRequest.ItemID, accountId);

            ES.Services.Organizations.SetUserGeneralSettings(
                    PanelRequest.ItemID, accountId,
                    txtDisplayName.Text,
                    null,
                    false,
                    user.Disabled,
                    user.Locked,

                    txtFirstName.Text,
                    txtInitials.Text,
                    txtLastName.Text,

                    null,
                    null,
                    null,
                    null,
                    null,

                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    user.ExternalEmail,
                    txtSubscriberNumber.Text,
                    0,
                    false,
                    chkUserMustChangePassword.Checked);
        }



        protected void rbtnUserExistingUser_CheckedChanged(object sender, EventArgs e)
        {
            ExistingUserDiv.Visible = true;
            NewUserDiv.Visible = false;
        }

        protected void rbtnCreateNewMailbox_CheckedChanged(object sender, EventArgs e)
        {
            NewUserDiv.Visible = true;
            ExistingUserDiv.Visible = false;

        }

        protected void mailboxPlanSelector_Change(object sender, EventArgs e)
        {
        }

    }
}
