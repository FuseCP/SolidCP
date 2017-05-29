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
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.Common;
using SolidCP.Portal.SkinControls;

namespace SolidCP.Portal.ExchangeServer
{
    public partial class ExchangeAddMailboxPlan : SolidCPModuleBase
    {
        private bool RetentionPolicy
        {
            get
            {
                bool res = false;
                bool.TryParse(hfArchivingPlan.Value, out res);
                return res;
            }
            set
            {
                hfArchivingPlan.Value = value.ToString();
            }
        }

        private Control FindControlRecursive(Control rootControl, string controlID)
        {
            if (rootControl.ID == controlID) return rootControl;

            foreach (Control controlToSearch in rootControl.Controls)
            {
                Control controlToReturn =
                    FindControlRecursive(controlToSearch, controlID);
                if (controlToReturn != null) return controlToReturn;
            }
            return null;
        }

        private string MainValidationGroup
        {
            get { return RetentionPolicy ? "CreateRetentionPolicy" : "CreateMailboxPlan"; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

                if (PanelRequest.GetInt("MailboxPlanId") != 0)
                {
                    Providers.HostedSolution.ExchangeMailboxPlan plan = ES.Services.ExchangeServer.GetExchangeMailboxPlan(PanelRequest.ItemID, PanelRequest.GetInt("MailboxPlanId"));
                    txtMailboxPlan.Text = plan.MailboxPlan;
                    RetentionPolicy = plan.Archiving;

                    if (RetentionPolicy)
                    {
                        List<ExchangeMailboxPlanRetentionPolicyTag> tags = new List<ExchangeMailboxPlanRetentionPolicyTag>();
                        tags.AddRange(ES.Services.ExchangeServer.GetExchangeMailboxPlanRetentionPolicyTags(plan.MailboxPlanId));

                        ViewState["Tags"] = tags;
                        gvPolicy.DataSource = tags;
                        gvPolicy.DataBind();
                    }
                    else
                    {
                        mailboxSize.QuotaValue = plan.MailboxSizeMB;
                        maxRecipients.QuotaValue = plan.MaxRecipients;
                        maxSendMessageSizeKB.QuotaValue = plan.MaxSendMessageSizeKB;
                        maxReceiveMessageSizeKB.QuotaValue = plan.MaxReceiveMessageSizeKB;
                        chkPOP3.Checked = plan.EnablePOP;
                        chkIMAP.Checked = plan.EnableIMAP;
                        chkOWA.Checked = plan.EnableOWA;
                        chkMAPI.Checked = plan.EnableMAPI;
                        chkActiveSync.Checked = plan.EnableActiveSync;
                        sizeIssueWarning.ValueKB = plan.IssueWarningPct;
                        sizeProhibitSend.ValueKB = plan.ProhibitSendPct;
                        sizeProhibitSendReceive.ValueKB = plan.ProhibitSendReceivePct;
                        daysKeepDeletedItems.ValueDays = plan.KeepDeletedItemsDays;
                        chkHideFromAddressBook.Checked = plan.HideFromAddressBook;
                        chkEnableLitigationHold.Checked = plan.AllowLitigationHold;
                        recoverableItemsSpace.QuotaValue = plan.RecoverableItemsSpace;
                        recoverableItemsWarning.ValueKB = plan.RecoverableItemsWarningPct;
                        txtLitigationHoldMsg.Text = plan.LitigationHoldMsg;
                        txtLitigationHoldUrl.Text = plan.LitigationHoldUrl;

                        chkEnableArchiving.Checked = plan.EnableArchiving;
                        archiveQuota.QuotaValue = plan.ArchiveSizeMB;
                        archiveWarningQuota.ValueKB = plan.ArchiveWarningPct;
                        chkEnableForceArchiveDeletion.Checked = plan.EnableForceArchiveDeletion;
                    }

                    locTitle.Text = plan.MailboxPlan;

                    this.DisableControls = true;

                }
                else
                {

                    if (cntx != null)
                    {
                        foreach (QuotaValueInfo quota in cntx.QuotasArray)
                        {
                            switch (quota.QuotaId)
                            {
                                case 77:
                                    break;
                                case 365:
                                    if (quota.QuotaAllocatedValue != -1)
                                    {
                                        maxRecipients.QuotaValue = quota.QuotaAllocatedValue;
                                    }
                                    break;
                                case 366:
                                    if (quota.QuotaAllocatedValue != -1)
                                    {
                                        maxSendMessageSizeKB.QuotaValue = quota.QuotaAllocatedValue;
                                    }
                                    break;
                                case 367:
                                    if (quota.QuotaAllocatedValue != -1)
                                    {
                                        maxReceiveMessageSizeKB.QuotaValue = quota.QuotaAllocatedValue;
                                    }
                                    break;
                                case 83:
                                    chkPOP3.Checked = Convert.ToBoolean(quota.QuotaAllocatedValue);
                                    chkPOP3.Enabled = Convert.ToBoolean(quota.QuotaAllocatedValue);
                                    break;
                                case 84:
                                    chkIMAP.Checked = Convert.ToBoolean(quota.QuotaAllocatedValue);
                                    chkIMAP.Enabled = Convert.ToBoolean(quota.QuotaAllocatedValue);
                                    break;
                                case 85:
                                    chkOWA.Checked = Convert.ToBoolean(quota.QuotaAllocatedValue);
                                    chkOWA.Enabled = Convert.ToBoolean(quota.QuotaAllocatedValue);
                                    break;
                                case 86:
                                    chkMAPI.Checked = Convert.ToBoolean(quota.QuotaAllocatedValue);
                                    chkMAPI.Enabled = Convert.ToBoolean(quota.QuotaAllocatedValue);
                                    break;
                                case 87:
                                    chkActiveSync.Checked = Convert.ToBoolean(quota.QuotaAllocatedValue);
                                    chkActiveSync.Enabled = Convert.ToBoolean(quota.QuotaAllocatedValue);
                                    break;
                                case 364:
                                    daysKeepDeletedItems.ValueDays = quota.QuotaAllocatedValue;
                                    daysKeepDeletedItems.RequireValidatorEnabled = true;
                                    break;
                                case 420:
                                    chkEnableLitigationHold.Checked = Convert.ToBoolean(quota.QuotaAllocatedValue);
                                    chkEnableLitigationHold.Enabled = Convert.ToBoolean(quota.QuotaAllocatedValue);
                                    break;

                            }

                            sizeIssueWarning.ValueKB = 95;
                            sizeProhibitSend.ValueKB = 100;
                            sizeProhibitSendReceive.ValueKB = 100;
                            recoverableItemsWarning.ValueKB = 95;

                            RetentionPolicy = PanelRequest.GetBool("archiving", false);

                            if (!RetentionPolicy)
                            {
                                chkEnableArchiving.Checked = true;
                                archiveQuota.QuotaValue = cntx.Quotas[Quotas.EXCHANGE2013_ARCHIVINGSTORAGE].QuotaAllocatedValue;
                                archiveWarningQuota.ValueKB = 95;
                            }
                        }
                    }
                    else
                        this.DisableControls = true;
                }

                if (RetentionPolicy)
                    UpdateTags();

                locTitle.Text = RetentionPolicy ? GetLocalizedString("locTitleArchiving.Text") : GetLocalizedString("locTitle.Text");
                secMailboxPlan.Text = RetentionPolicy ? GetLocalizedString("secMailboxPlanArchiving.Text") : GetLocalizedString("secMailboxPlan.Text");
                UserSpaceBreadcrumb bc = FindControlRecursive(Page, "breadcrumb") as UserSpaceBreadcrumb;
                if (bc != null)
                {
                    Label lbOrgCurPage = bc.FindControl("lbOrgCurPage") as Label;
                    if (lbOrgCurPage != null)
                        lbOrgCurPage.Text = locTitle.Text;
                }


                secMailboxFeatures.Visible = !RetentionPolicy;
                secMailboxGeneral.Visible = !RetentionPolicy;
                secStorageQuotas.Visible = !RetentionPolicy;
                secDeleteRetention.Visible = !RetentionPolicy;
                secLitigationHold.Visible = !RetentionPolicy;

                secArchiving.Visible = !RetentionPolicy;

                secRetentionPolicyTags.Visible = RetentionPolicy;

                valRequireMailboxPlan.ValidationGroup = MainValidationGroup;
                btnAdd.ValidationGroup = MainValidationGroup;

            }

        }

        protected void UpdateTags()
        {
            if (RetentionPolicy)
            {
                ddTags.Items.Clear();

                Providers.HostedSolution.ExchangeRetentionPolicyTag[] allTags = ES.Services.ExchangeServer.GetExchangeRetentionPolicyTags(PanelRequest.ItemID);

                List<ExchangeMailboxPlanRetentionPolicyTag> selectedTags = ViewState["Tags"] as List<ExchangeMailboxPlanRetentionPolicyTag>;

                foreach (Providers.HostedSolution.ExchangeRetentionPolicyTag tag in allTags)
                {
                    if (selectedTags!=null)
                    {
                        if (selectedTags.Find(x => x.TagID == tag.TagID) != null)
                            continue;
                    }

                    ddTags.Items.Add(new System.Web.UI.WebControls.ListItem(tag.TagName, tag.TagID.ToString()));
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Page.Validate(MainValidationGroup);

            if (!Page.IsValid)
                return;

            AddMailboxPlan();
        }

        private void AddMailboxPlan()
        {
            try
            {
                Providers.HostedSolution.ExchangeMailboxPlan plan = new Providers.HostedSolution.ExchangeMailboxPlan();
                plan.MailboxPlan = txtMailboxPlan.Text;
                plan.Archiving = RetentionPolicy;

                if (RetentionPolicy)
                {

                }
                else
                {
                    plan.MailboxSizeMB = mailboxSize.QuotaValue;

                    plan.IsDefault = false;
                    plan.MaxRecipients = maxRecipients.QuotaValue;
                    plan.MaxSendMessageSizeKB = maxSendMessageSizeKB.QuotaValue;
                    plan.MaxReceiveMessageSizeKB = maxReceiveMessageSizeKB.QuotaValue;
                    plan.EnablePOP = chkPOP3.Checked;
                    plan.EnableIMAP = chkIMAP.Checked;
                    plan.EnableOWA = chkOWA.Checked;
                    plan.EnableMAPI = chkMAPI.Checked;
                    plan.EnableActiveSync = chkActiveSync.Checked;
                    plan.IssueWarningPct = sizeIssueWarning.ValueKB;
                    if ((plan.IssueWarningPct == 0)) plan.IssueWarningPct = 100;
                    plan.ProhibitSendPct = sizeProhibitSend.ValueKB;
                    if ((plan.ProhibitSendPct == 0)) plan.ProhibitSendPct = 100;
                    plan.ProhibitSendReceivePct = sizeProhibitSendReceive.ValueKB;
                    if ((plan.ProhibitSendReceivePct == 0)) plan.ProhibitSendReceivePct = 100;
                    plan.KeepDeletedItemsDays = daysKeepDeletedItems.ValueDays;
                    plan.HideFromAddressBook = chkHideFromAddressBook.Checked;
                    plan.AllowLitigationHold = chkEnableLitigationHold.Checked;
                    plan.RecoverableItemsSpace = recoverableItemsSpace.QuotaValue;
                    plan.RecoverableItemsWarningPct = recoverableItemsWarning.ValueKB;
                    if ((plan.RecoverableItemsWarningPct == 0)) plan.RecoverableItemsWarningPct = 100;
                    plan.LitigationHoldMsg = txtLitigationHoldMsg.Text.Trim();
                    plan.LitigationHoldUrl = txtLitigationHoldUrl.Text.Trim();

                    plan.EnableArchiving = chkEnableArchiving.Checked;
                    plan.ArchiveSizeMB = archiveQuota.QuotaValue;
                    plan.ArchiveWarningPct = archiveWarningQuota.ValueKB;
                    if ((plan.ArchiveWarningPct == 0))
                    {
                        plan.ArchiveWarningPct = 100;
                    }
                    plan.EnableForceArchiveDeletion = chkEnableForceArchiveDeletion.Checked;
                }

                int planId = ES.Services.ExchangeServer.AddExchangeMailboxPlan(PanelRequest.ItemID,
                                                                                plan);


                if (planId < 0)
                {
                    messageBox.ShowResultMessage(planId);
                    return;
                }

                List<ExchangeMailboxPlanRetentionPolicyTag> tags = ViewState["Tags"] as List<ExchangeMailboxPlanRetentionPolicyTag>;

                if (tags!=null)
                {
                    foreach(ExchangeMailboxPlanRetentionPolicyTag tag in tags)
                    {
                        tag.MailboxPlanId = planId;
                        IntResult result = ES.Services.ExchangeServer.AddExchangeMailboxPlanRetentionPolicyTag(PanelRequest.ItemID, tag);
                        if (!result.IsSuccess)
                        {
                            messageBox.ShowMessage(result, "EXCHANGE_ADD_MAILBOXPLAN", null);
                            return;
                        }
                    }
                }


                Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), RetentionPolicy ? "retentionpolicy" : "mailboxplans",
                    "SpaceID=" + PanelSecurity.PackageId));
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_ADD_MAILBOXPLAN", ex);
            }
        }

        protected void bntAddTag_Click(object sender, EventArgs e)
        {
            int addTagId;
            if (!int.TryParse(ddTags.SelectedValue, out addTagId))
                return;

            Providers.HostedSolution.ExchangeRetentionPolicyTag tag = ES.Services.ExchangeServer.GetExchangeRetentionPolicyTag(PanelRequest.ItemID, addTagId);
            if (tag == null) return;

            List<ExchangeMailboxPlanRetentionPolicyTag> res = ViewState["Tags"] as List<ExchangeMailboxPlanRetentionPolicyTag>;
            if (res==null) res = new List<ExchangeMailboxPlanRetentionPolicyTag>();

            ExchangeMailboxPlanRetentionPolicyTag add = new ExchangeMailboxPlanRetentionPolicyTag();
            add.MailboxPlanId = PanelRequest.GetInt("MailboxPlanId");
            add.TagID = tag.TagID;
            add.TagName = tag.TagName;

            res.Add(add);

            ViewState["Tags"] = res;

            gvPolicy.DataSource = res;
            gvPolicy.DataBind();

            UpdateTags();
        }

        protected void gvPolicy_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            switch (e.CommandName)
            {
                case "DeleteItem":
                    try
                    {
                        int tagId;
                        if (!int.TryParse(e.CommandArgument.ToString(), out tagId))
                            return;

                        List<ExchangeMailboxPlanRetentionPolicyTag> tags = ViewState["Tags"] as List<ExchangeMailboxPlanRetentionPolicyTag>;
                        if (tags == null) return;

                        int i = tags.FindIndex(x => x.TagID == tagId);
                        if (i >= 0) tags.RemoveAt(i);

                        ViewState["Tags"] = tags;
                        gvPolicy.DataSource = tags;
                        gvPolicy.DataBind();
                        UpdateTags();
                    }
                    catch (Exception)
                    {
                    }

                    break;

            }
        }

    }
}
