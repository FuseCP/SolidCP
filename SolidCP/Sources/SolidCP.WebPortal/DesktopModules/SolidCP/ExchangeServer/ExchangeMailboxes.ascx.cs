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
using System.Linq;
using System.Web.UI.WebControls;
using SolidCP.Providers.HostedSolution;
using SolidCP.EnterpriseServer;
using SolidCP.EnterpriseServer.Base.HostedSolution;
using System.Collections.Generic;
using System.Web.UI;

namespace SolidCP.Portal.ExchangeServer
{
    public partial class ExchangeMailboxes : SolidCPModuleBase
    {
        private bool ArchivingBoxes
        {
            get
            {
                return PanelRequest.Ctl.ToLower().Contains("archiving");
            }
        }

        private PackageContext cntx;

        private ServiceLevel[] ServiceLevels;

        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterClientScriptInclude("jquery", ResolveUrl("~/JavaScript/jquery-1.4.4.min.js"));

            locTitle.Text = ArchivingBoxes ? GetLocalizedString("locTitleArchiving.Text") : GetLocalizedString("locTitle.Text");

            btnCreateMailbox.Visible = !ArchivingBoxes;

            cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            if (!IsPostBack)
            {
                chkMailboxes.Checked = true;
                chkResourceMailboxes.Checked = true;
                chkSharedMailboxes.Checked = true;

                BindStats();
            }

            BindServiceLevels();

            if (cntx.Quotas.ContainsKey(Quotas.EXCHANGE2007_ISCONSUMER))
            {
                if (cntx.Quotas[Quotas.EXCHANGE2007_ISCONSUMER].QuotaAllocatedValue != 1)
                {
                    gvMailboxes.Columns[6].Visible = false;
                }
            }

            gvMailboxes.Columns[4].Visible = cntx.Groups.ContainsKey(ResourceGroups.ServiceLevels);
        }

        private void BindServiceLevels()
        {
            ServiceLevels = ES.Services.Organizations.GetSupportServiceLevels();
        }

        private void BindStats()
        {
            // quota values
            OrganizationStatistics stats = ES.Services.ExchangeServer.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);
            mailboxesQuota.QuotaUsedValue = stats.CreatedMailboxes;
            mailboxesQuota.QuotaValue = stats.AllocatedMailboxes;
            if (stats.AllocatedMailboxes != -1) mailboxesQuota.QuotaAvailable = stats.AllocatedMailboxes - stats.CreatedMailboxes;

            if (cntx != null && cntx.Groups.ContainsKey(ResourceGroups.ServiceLevels)) BindServiceLevelsStats(stats);
        }

        private void BindServiceLevelsStats(OrganizationStatistics stats)
        {
            List<ServiceLevelQuotaValueInfo> serviceLevelQuotas = new List<ServiceLevelQuotaValueInfo>();
            foreach (var quota in stats.ServiceLevels)
            {
                serviceLevelQuotas.Add(new ServiceLevelQuotaValueInfo
                {
                    QuotaName = quota.QuotaName,
                    QuotaDescription = quota.QuotaDescription + " in this Organization:",
                    QuotaTypeId = quota.QuotaTypeId,
                    QuotaValue = quota.QuotaAllocatedValue,
                    QuotaUsedValue = quota.QuotaUsedValue,
                    QuotaAvailable = quota.QuotaAllocatedValue - quota.QuotaUsedValue
                });
            }
            dlServiceLevelQuotas.DataSource = serviceLevelQuotas;
            dlServiceLevelQuotas.DataBind();
        }

        protected void btnCreateMailbox_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "create_mailbox",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        public string GetMailboxEditUrl(string accountId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_user",
                    "AccountID=" + accountId,
                    "ItemID=" + PanelRequest.ItemID,
                    "Context=Mailbox");
        }

        protected void odsAccountsPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                messageBox.ShowErrorMessage("EXCHANGE_GET_MAILBOXES", e.Exception);
                e.ExceptionHandled = true;
            }
        }

        public string GetAccountImage(int accountTypeId, bool vip)
        {
            ExchangeAccountType accountType = (ExchangeAccountType)accountTypeId;
            string imgName = "mailbox_16.gif";

            if (accountType == ExchangeAccountType.Contact)
                imgName = "contact_16.gif";
            else if (accountType == ExchangeAccountType.DistributionList)
                imgName = "dlist_16.gif";
            else if (accountType == ExchangeAccountType.Room)
                imgName = "room_16.gif";
            else if (accountType == ExchangeAccountType.Equipment)
                imgName = "equipment_16.gif";
            else if (accountType == ExchangeAccountType.SharedMailbox)
                imgName = "shared_16.gif";

            if (vip && cntx.Groups.ContainsKey(ResourceGroups.ServiceLevels)) imgName = "vip_user_16.png";

            return GetThemedImage("Exchange/" + imgName);
        }

        public string GetStateImage(bool locked, bool disabled)
        {
            string imgName = "enabled.png";

            if (locked)
                imgName = "locked.png";
            else
                if (disabled)
                    imgName = "disabled.png";

            return GetThemedImage("Exchange/" + imgName);
        }

        protected void gvMailboxes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                // delete mailbox
                int accountId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                try
                {
                    int result = ES.Services.ExchangeServer.DisableMailbox(PanelRequest.ItemID, accountId);
                    if (result < 0)
                    {
                        messageBox.ShowResultMessage(result);
                        return;
                    }

                    // rebind grid
                    gvMailboxes.DataBind();

                    // bind stats
                    BindStats();
                }
                catch (Exception ex)
                {
                    messageBox.ShowErrorMessage("EXCHANGE_DELETE_MAILBOX", ex);
                }
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)   
        {   
            gvMailboxes.PageSize = Convert.ToInt16(ddlPageSize.SelectedValue);   
       
            // rebind grid   
            gvMailboxes.DataBind();   
       
            // bind stats   
            BindStats();   
       
        }


        public string GetOrganizationUserEditUrl(string accountId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_user",
                    "AccountID=" + accountId,
                    "ItemID=" + PanelRequest.ItemID,
                    "Context=User");
        }

        protected void odsAccountsPaged_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["archiving"] = ArchivingBoxes;
        }

        public ServiceLevel GetServiceLevel(int levelId)
        {
            ServiceLevel serviceLevel = ServiceLevels.Where(x => x.LevelId == levelId).DefaultIfEmpty(new ServiceLevel { LevelName = "", LevelDescription = "" }).FirstOrDefault();

            bool enable = !string.IsNullOrEmpty(serviceLevel.LevelName);

            enable = enable ? cntx.Quotas.ContainsKey(Quotas.SERVICE_LEVELS + serviceLevel.LevelName) : false;
            enable = enable ? cntx.Quotas[Quotas.SERVICE_LEVELS + serviceLevel.LevelName].QuotaAllocatedValue != 0 : false;

            if (!enable)
            {
                serviceLevel.LevelName = "";
                serviceLevel.LevelDescription = "";
            }

            return serviceLevel;
        }

        protected void chkMailboxes_CheckedChanged(object sender, EventArgs e)
        {
            List<string> accountTypes = new List<string>();

            if ((!chkMailboxes.Checked)&&(!chkSharedMailboxes.Checked)&&(!chkResourceMailboxes.Checked))
                chkMailboxes.Checked = true;

            if (chkMailboxes.Checked)
                accountTypes.Add("1");

            if (chkSharedMailboxes.Checked)
                accountTypes.Add("10");

            if (chkResourceMailboxes.Checked)
                accountTypes.AddRange(new string[] {"5","6"});

            odsAccountsPaged.SelectParameters["accountTypes"].DefaultValue = string.Join(",", accountTypes);
        }
    }
}
