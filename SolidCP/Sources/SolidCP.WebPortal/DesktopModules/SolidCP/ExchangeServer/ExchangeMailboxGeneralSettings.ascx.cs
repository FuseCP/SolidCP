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

namespace SolidCP.Portal.ExchangeServer
{
    public partial class ExchangeMailboxGeneralSettings : SolidCPModuleBase
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

                if (Utils.CheckQouta(Quotas.EXCHANGE2007_DISCLAIMERSALLOWED, Cntx))
                {
                    ddDisclaimer.Items.Add(new System.Web.UI.WebControls.ListItem("None", "-1"));
                    ExchangeDisclaimer[] disclaimers = ES.Services.ExchangeServer.GetExchangeDisclaimers(PanelRequest.ItemID);
                    foreach (ExchangeDisclaimer disclaimer in disclaimers)
                        ddDisclaimer.Items.Add(new System.Web.UI.WebControls.ListItem(disclaimer.DisclaimerName, disclaimer.ExchangeDisclaimerId.ToString()));
                }
                else
                {
                    locDisclaimer.Visible = false;
                    ddDisclaimer.Visible = false;
                }

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

                secRetentionPolicy.Visible = Utils.CheckQouta(Quotas.EXCHANGE2013_ALLOWRETENTIONPOLICY, Cntx);

                if (GetLocalizedString("buttonPanel.OnSaveClientClick") != null)
                    buttonPanel.OnSaveClientClick = GetLocalizedString("buttonPanel.OnSaveClientClick");
            }

            int planId = -1;
            int.TryParse(mailboxPlanSelector.MailboxPlanId, out planId);
            ExchangeMailboxPlan plan = ES.Services.ExchangeServer.GetExchangeMailboxPlan(PanelRequest.ItemID, planId);

            if (plan != null)
            {
                secArchiving.Visible = plan.EnableArchiving;
                secLitigationHoldSettings.Visible = plan.AllowLitigationHold && Utils.CheckQouta(Quotas.EXCHANGE2007_ALLOWLITIGATIONHOLD, Cntx);
            }
            else
            {
                secArchiving.Visible = false;
                secLitigationHoldSettings.Visible = false;
            }
        }

        private void BindSettings()
        {
            try
            {
                // get settings
                ExchangeMailbox mailbox = ES.Services.ExchangeServer.GetMailboxGeneralSettings(PanelRequest.ItemID,
                    PanelRequest.AccountID);

                //get statistics
                ExchangeMailboxStatistics stats = ES.Services.ExchangeServer.GetMailboxStatistics(PanelRequest.ItemID,
                    PanelRequest.AccountID);

                // mailbox plan

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

                chkPmmAllowed.Checked = (account.MailboxManagerActions & MailboxManagerActions.GeneralSettings) > 0;

                if (account.MailboxPlanId == 0)
                {
                    mailboxPlanSelector.AddNone = true;
                    mailboxPlanSelector.MailboxPlanId = "-1";
                }
                else
                {
                    mailboxPlanSelector.MailboxPlanId = account.MailboxPlanId.ToString();
                }

                if (account.ArchivingMailboxPlanId<1)
                {
                    mailboxRetentionPolicySelector.MailboxPlanId = "-1";
                }
                else
                {
                    mailboxRetentionPolicySelector.MailboxPlanId = account.ArchivingMailboxPlanId.ToString();
                }

                mailboxSize.QuotaUsedValue = Convert.ToInt32(stats.TotalSize / 1024 / 1024);
                mailboxSize.QuotaValue = (stats.MaxSize == -1) ? -1 : (int)Math.Round((double)(stats.MaxSize / 1024 / 1024));

                secCalendarSettings.Visible = ((account.AccountType == ExchangeAccountType.Equipment) | (account.AccountType == ExchangeAccountType.Room));

                secLitigationHoldSettings.Visible = mailbox.EnableLitigationHold;

                litigationHoldSpace.QuotaUsedValue = Convert.ToInt32(stats.LitigationHoldTotalSize / 1024 / 1024);
                litigationHoldSpace.QuotaValue = (stats.LitigationHoldMaxSize == -1) ? -1 : (int)Math.Round((double)(stats.LitigationHoldMaxSize / 1024 / 1024));

                if (Utils.CheckQouta(Quotas.EXCHANGE2007_DISCLAIMERSALLOWED, Cntx))
                {
                    int disclaimerId = ES.Services.ExchangeServer.GetExchangeAccountDisclaimerId(PanelRequest.ItemID, PanelRequest.AccountID);
                    ddDisclaimer.SelectedValue = disclaimerId.ToString();
                }

                int ArchivingMaxSize = -1;
                if (plan != null) ArchivingMaxSize = plan.ArchiveSizeMB;
                chkEnableArchiving.Checked = account.EnableArchiving;
                archivingQuotaViewer.QuotaUsedValue = Convert.ToInt32(stats.ArchivingTotalSize / 1024 / 1024);
                archivingQuotaViewer.QuotaValue = ArchivingMaxSize;
                rowArchiving.Visible = chkEnableArchiving.Checked;

                if (account.LevelId > 0 && Cntx.Groups.ContainsKey(ResourceGroups.ServiceLevels))
                {
                    SolidCP.EnterpriseServer.Base.HostedSolution.ServiceLevel serviceLevel = ES.Services.Organizations.GetSupportServiceLevel(account.LevelId);

                    litServiceLevel.Visible = true;
                    litServiceLevel.Text = serviceLevel.LevelName;
                    litServiceLevel.ToolTip = serviceLevel.LevelDescription;

                }
                imgVipUser.Visible = account.IsVIP && Cntx.Groups.ContainsKey(ResourceGroups.ServiceLevels);

                if (account.AccountType == ExchangeAccountType.SharedMailbox)
                    litDisplayName.Text += GetSharedLocalizedString("SharedMailbox.Text");

                if (account.AccountType == ExchangeAccountType.Room)
                    litDisplayName.Text += GetSharedLocalizedString("RoomMailbox.Text");

                if (account.AccountType == ExchangeAccountType.Equipment)
                    litDisplayName.Text += GetSharedLocalizedString("EquipmentMailbox.Text");

            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_GET_MAILBOX_SETTINGS", ex);
            }
        }

        private void SaveSettings()
        {
            if (!Page.IsValid)
                return;

            try
            {
                if (mailboxPlanSelector.MailboxPlanId == "-1")
                {
                    messageBox.ShowErrorMessage("EXCHANGE_SPECIFY_PLAN");
                    return;
                }

                int result = ES.Services.ExchangeServer.SetMailboxGeneralSettings(
                    PanelRequest.ItemID, PanelRequest.AccountID,
                    chkHideAddressBook.Checked,
                    chkDisable.Checked);

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }
                else
                {
                    int planId = Convert.ToInt32(mailboxPlanSelector.MailboxPlanId);
                    int policyId = -1;
                    int.TryParse(mailboxRetentionPolicySelector.MailboxPlanId, out policyId);

                    bool EnableArchiving = chkEnableArchiving.Checked;

                    result = ES.Services.ExchangeServer.SetExchangeMailboxPlan(PanelRequest.ItemID, PanelRequest.AccountID, planId,
                        policyId, EnableArchiving);

                    if (result < 0)
                    {
                        messageBox.ShowResultMessage(result);
                        return;
                    }
                }

                if (Utils.CheckQouta(Quotas.EXCHANGE2007_DISCLAIMERSALLOWED, Cntx))
                {
                    int disclaimerId;
                    if (int.TryParse(ddDisclaimer.SelectedValue, out disclaimerId))
                        ES.Services.ExchangeServer.SetExchangeAccountDisclaimerId(PanelRequest.ItemID, PanelRequest.AccountID, disclaimerId);
                }

                messageBox.ShowSuccessMessage("EXCHANGE_UPDATE_MAILBOX_SETTINGS");
                BindSettings();
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_UPDATE_MAILBOX_SETTINGS", ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            SaveSettings();

            Response.Redirect(PortalUtils.EditUrl("ItemID", PanelRequest.ItemID.ToString(),
                "mailboxes",
                "SpaceID=" + PanelSecurity.PackageId));
        }


        protected void chkPmmAllowed_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int result = ES.Services.ExchangeServer.SetMailboxManagerSettings(PanelRequest.ItemID, PanelRequest.AccountID,
                chkPmmAllowed.Checked, MailboxManagerActions.GeneralSettings);

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

                messageBox.ShowSuccessMessage("EXCHANGE_UPDATE_MAILMANAGER");
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_UPDATE_MAILMANAGER", ex);
            }
        }

        private int ConvertMbxSizeToIntMB(string inputValue)
        {
            int result = 0;

            if ((inputValue == null) || (inputValue == ""))
                return 0;

            if (inputValue.Contains("TB"))
            {
                result = Convert.ToInt32(inputValue.Replace(" TB", ""));
                result = result * 1024 * 1024;
            }
            else
                if (inputValue.Contains("GB"))
                {
                    result = Convert.ToInt32(inputValue.Replace(" GB", ""));
                    result = result * 1024;
                }
                else
                    if (inputValue.Contains("MB"))
                    {
                        result = Convert.ToInt32(inputValue.Replace(" MB", ""));
                    }

            return result;
        }

        public void mailboxPlanSelector_Changed(object sender, EventArgs e)
        {
        }

    }
}
