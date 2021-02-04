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
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;

namespace SolidCP.Portal.ExchangeServer
{
    public partial class ExchangeCreateJournalingMailbox : SolidCPModuleBase
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
            if (!IsPostBack)
            {
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

                BindPasswordSettings();

                ExchangeMailboxPlan[] plans = ES.Services.ExchangeServer.GetExchangeMailboxPlans(PanelRequest.ItemID, false);

                if (plans.Length == 0)
                    btnCreate.Enabled = false;

                if (cntx.Quotas.ContainsKey(Quotas.EXCHANGE2007_ISCONSUMER))
                {
                    if (cntx.Quotas[Quotas.EXCHANGE2007_ISCONSUMER].QuotaAllocatedValue != 1)
                    {
                        locSubscriberNumber.Visible = txtSubscriberNumber.Visible = valRequireSubscriberNumber.Enabled = false;
                    }
                }

                BindJournalingRecipients();
            }
        }

        private void BindJournalingRecipients()
        {
            Organization org = ES.Services.Organizations.GetOrganization(PanelRequest.ItemID);

            string path = org.SecurityGroup;
            string[] parts = path.Substring(path.ToUpper().IndexOf("DC=")).Split(',');
            string domain = "";
            for (int i = 0; i < parts.Length; i++)
            {
                domain += parts[i].Substring(3) + (i < parts.Length - 1 ? "." : "");
            }

            ddlRecipient.Items.Clear();
            if (!String.IsNullOrEmpty(domain)) {
                string mail = org.Name + "@" + domain;
                ddlRecipient.Items.Add(new ListItem(GetLocalizedString("OrganizationGroup"), mail, true));
            }

            ExchangeAccount[] accounts = ES.Services.ExchangeServer.GetAccounts(PanelRequest.ItemID, ExchangeAccountType.DistributionList);
            foreach (ExchangeAccount account in accounts)
            {
                ddlRecipient.Items.Add(new ListItem(account.DisplayName + " - " + account.PrimaryEmailAddress, account.PrimaryEmailAddress));
            }
            accounts = ES.Services.ExchangeServer.GetAccounts(PanelRequest.ItemID, ExchangeAccountType.Mailbox);
            foreach (ExchangeAccount account in accounts)
            {
                ddlRecipient.Items.Add(new ListItem(account.DisplayName + " - " + account.PrimaryEmailAddress, account.PrimaryEmailAddress));
            }
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
                string primryEmail = IsNewUser ? email.Email : userSelector.GetPrimaryEmailAddress();
                string displayName = IsNewUser ? txtDisplayName.Text.Trim() : userSelector.GetDisplayName();
                string accountName = IsNewUser ? string.Empty : userSelector.GetAccount();

                string domain = IsNewUser ? email.DomainName : userSelector.GetPrimaryEmailAddress().Split('@')[1];

                int accountId = IsNewUser ? 0 : userSelector.GetAccountId();

                string subscriberNumber = IsNewUser ? txtSubscriberNumber.Text.Trim() : userSelector.GetSubscriberNumber();

                var passwordString = password.Password;

                if (sendToControl.IsRequestSend && IsNewUser)
                {
                    passwordString = Membership.GeneratePassword(16, 3);
                }

                accountId = ES.Services.ExchangeServer.CreateMailbox(PanelRequest.ItemID, accountId, ExchangeAccountType.JournalingMailbox, accountName,
                    displayName, name, domain, passwordString, false, "", Convert.ToInt32(mailboxPlanSelector.MailboxPlanId), 0, subscriberNumber, false);


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

                    ES.Services.ExchangeServer.CreateJournalRule(PanelRequest.ItemID, primryEmail, ddlScope.SelectedValue, ddlRecipient.SelectedValue, cbEnabled.Checked);
                }

                if (sendToControl.SendEmail && IsNewUser)
                {
                    ES.Services.Organizations.SendUserPasswordRequestEmail(PanelRequest.ItemID, accountId, "User creation", sendToControl.Email, true);
                }
                else if (sendToControl.SendMobile && IsNewUser)
                {
                    ES.Services.Organizations.SendUserPasswordRequestSms(PanelRequest.ItemID, accountId, "User creation", sendToControl.Mobile);
                }

                Response.Redirect(EditUrl("AccountID", accountId.ToString(), "journaling_mailbox_settings",
                    "SpaceID=" + PanelSecurity.PackageId.ToString(),
                    "ItemID=" + PanelRequest.ItemID.ToString(),
                    "Context=JournalingMailbox"));
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_CREATE_JOURNALING_MAILBOX", ex);
            }
        }

        private void SetUserAttributes(int accountId)
        {
            OrganizationUser user = ES.Services.Organizations.GetUserGeneralSettings(PanelRequest.ItemID, accountId);

            ES.Services.Organizations.SetUserGeneralSettings(PanelRequest.ItemID, accountId, txtDisplayName.Text, null, false, user.Disabled, user.Locked,
                txtFirstName.Text, txtInitials.Text, txtLastName.Text, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
                null, null, null, user.ExternalEmail, txtSubscriberNumber.Text, 0, false, false);
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
