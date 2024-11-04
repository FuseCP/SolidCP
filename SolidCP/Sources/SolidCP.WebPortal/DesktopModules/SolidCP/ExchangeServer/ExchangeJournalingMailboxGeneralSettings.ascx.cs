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
using SolidCP.Providers.HostedSolution;
using SolidCP.EnterpriseServer;
using System.Web.UI.WebControls;

namespace SolidCP.Portal.ExchangeServer
{
    public partial class ExchangeJournalingMailboxGeneralSettings : SolidCPModuleBase
    {

        private PackageContext cntx = null;
        private PackageContext Cntx
        {
            get 
            {
                if (cntx == null)
                    cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

                return cntx;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindJournalingRecipients();

                BindSettings();

                UserInfo user = UsersHelper.GetUser(PanelSecurity.EffectiveUserId);

                if (user != null)
                {
                    
                    if ((user.Role == UserRole.User) & (Utils.CheckQouta(Quotas.EXCHANGE2007_ISCONSUMER, Cntx)))
                    {
                        chkHideAddressBook.Visible = false;
                        chkDisable.Visible = false;
                    }

                }

                if (GetLocalizedString("buttonPanel.OnSaveClientClick") != null)
                    buttonPanel.OnSaveClientClick = GetLocalizedString("buttonPanel.OnSaveClientClick");
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
            if (!String.IsNullOrEmpty(domain))
            {
                string mail = org.OrganizationId + "@" + domain;
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
            accounts = ES.Services.ExchangeServer.GetAccounts(PanelRequest.ItemID, ExchangeAccountType.SharedMailbox);
            foreach (ExchangeAccount account in accounts)
            {
                ddlRecipient.Items.Add(new ListItem(account.DisplayName + " - " + account.PrimaryEmailAddress, account.PrimaryEmailAddress));
            }
        }

        private void BindSettings()
        {
            try
            {
                // get settings
                ExchangeMailbox mailbox = ES.Services.ExchangeServer.GetMailboxGeneralSettings(PanelRequest.ItemID, PanelRequest.AccountID);

                //get statistics
                ExchangeMailboxStatistics stats = ES.Services.ExchangeServer.GetMailboxStatistics(PanelRequest.ItemID, PanelRequest.AccountID);

                // title
                litDisplayName.Text = mailbox.DisplayName;

                // bind form
                chkHideAddressBook.Checked = mailbox.HideFromAddressBook;
                chkDisable.Checked = mailbox.Disabled;

                lblExchangeGuid.Text = string.IsNullOrEmpty(mailbox.ExchangeGuid) ? "<>" : mailbox.ExchangeGuid ;

                // get account meta
                ExchangeAccount account = ES.Services.ExchangeServer.GetAccount(PanelRequest.ItemID, PanelRequest.AccountID);

                // get mailbox plan
                ExchangeMailboxPlan plan = ES.Services.ExchangeServer.GetExchangeMailboxPlan(PanelRequest.ItemID, account.MailboxPlanId);

                ExchangeJournalRule rule = ES.Services.ExchangeServer.GetJournalRule(PanelRequest.ItemID, account.PrimaryEmailAddress);

                cbEnabled.Checked = rule.Enabled;
                ddlScope.SelectedValue = rule.Scope;
                ddlRecipient.SelectedValue = rule.Recipient;

                if (account.MailboxPlanId == 0)
                {
                    mailboxPlanSelector.AddNone = true;
                    mailboxPlanSelector.MailboxPlanId = "-1";
                }
                else
                {
                    mailboxPlanSelector.MailboxPlanId = account.MailboxPlanId.ToString();
                }

                mailboxSize.QuotaUsedValue = Convert.ToInt32(stats.TotalSize / 1024 / 1024);
                mailboxSize.QuotaValue = (stats.MaxSize == -1) ? -1 : (int)Math.Round((double)(stats.MaxSize / 1024 / 1024));

                if (account.LevelId > 0 && Cntx.Groups.ContainsKey(ResourceGroups.ServiceLevels))
                {
                    EnterpriseServer.Base.HostedSolution.ServiceLevel serviceLevel = ES.Services.Organizations.GetSupportServiceLevel(account.LevelId);

                    litServiceLevel.Visible = true;
                    litServiceLevel.Text = serviceLevel.LevelName;
                    litServiceLevel.ToolTip = serviceLevel.LevelDescription;

                }
                imgVipUser.Visible = account.IsVIP && Cntx.Groups.ContainsKey(ResourceGroups.ServiceLevels);
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_GET_JOURNALING_MAILBOX_SETTINGS", ex);
            }
        }

        private bool SaveSettings()
        {
            if (!Page.IsValid)
                return false;

            try
            {
                if (mailboxPlanSelector.MailboxPlanId == "-1")
                {
                    messageBox.ShowErrorMessage("EXCHANGE_SPECIFY_PLAN");
                    return false;
                }

                int result = ES.Services.ExchangeServer.SetMailboxGeneralSettings(
                    PanelRequest.ItemID, PanelRequest.AccountID,
                    chkHideAddressBook.Checked,
                    chkDisable.Checked);

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return false;
                }
                else
                {
                    int planId = Convert.ToInt32(mailboxPlanSelector.MailboxPlanId);

                    result = ES.Services.ExchangeServer.SetExchangeMailboxPlan(PanelRequest.ItemID, PanelRequest.AccountID, planId, -1, false);

                    if (result < 0)
                    {
                        messageBox.ShowResultMessage(result);
                        return false;
                    }
                    else
                    {
                        ExchangeAccount account = ES.Services.ExchangeServer.GetAccount(PanelRequest.ItemID, PanelRequest.AccountID);
                        ExchangeJournalRule rule = ES.Services.ExchangeServer.GetJournalRule(PanelRequest.ItemID, account.PrimaryEmailAddress);
                        rule.Enabled = cbEnabled.Checked;
                        rule.Scope = ddlScope.SelectedValue;
                        rule.Recipient = ddlRecipient.SelectedValue;
                        result = ES.Services.ExchangeServer.SetJournalRule(PanelRequest.ItemID, rule);
                        if (result < 0)
                        {
                            messageBox.ShowResultMessage(result);
                            return false;
                        }
                    }
                }

                messageBox.ShowSuccessMessage("EXCHANGE_UPDATE_JOURNALING_MAILBOX_SETTINGS");
                BindSettings();
                return true;
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_UPDATE_JOURNALING_MAILBOX_SETTINGS", ex);
                return false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (SaveSettings())
            {
                Response.Redirect(PortalUtils.EditUrl("ItemID", PanelRequest.ItemID.ToString(),
                "journaling_mailboxes",
                "SpaceID=" + PanelSecurity.PackageId));
            }
        }

        public void mailboxPlanSelector_Changed(object sender, EventArgs e)
        {
        }

    }
}
