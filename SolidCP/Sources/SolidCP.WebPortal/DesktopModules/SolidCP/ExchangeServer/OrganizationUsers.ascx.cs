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
using System.Linq;
using System.Web.UI.WebControls;
using SolidCP.Providers.HostedSolution;
using SolidCP.EnterpriseServer;
using SolidCP.EnterpriseServer.Base.HostedSolution;
using System.Web.UI;

namespace SolidCP.Portal.HostedSolution
{
    public partial class OrganizationUsers : SolidCPModuleBase
    {
        private ServiceLevel[] ServiceLevels;
        private PackageContext cntx;

        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterClientScriptInclude("jquery", ResolveUrl("~/JavaScript/jquery-1.4.4.min.js"));

            cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            BindServiceLevels();

            if (cntx.Quotas.ContainsKey(Quotas.EXCHANGE2007_ISCONSUMER))
            {
                if (cntx.Quotas[Quotas.EXCHANGE2007_ISCONSUMER].QuotaAllocatedValue != 1)
                {
                    gvUsers.Columns[6].Visible = false;
                }
            }
            gvUsers.Columns[4].Visible = cntx.Groups.ContainsKey(ResourceGroups.ServiceLevels);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            BindStats();
        }

        private void BindServiceLevels()
        {
            ServiceLevels = ES.Services.Organizations.GetSupportServiceLevels();
        }

        private void BindStats()
        {
            // quota values
            OrganizationStatistics stats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);
            usersQuota.QuotaUsedValue = stats.CreatedUsers;
            usersQuota.QuotaValue = stats.AllocatedUsers;
            if (stats.AllocatedUsers != -1) usersQuota.QuotaAvailable = stats.AllocatedUsers - stats.CreatedUsers;

            if(cntx != null && cntx.Groups.ContainsKey(ResourceGroups.ServiceLevels)) BindServiceLevelsStats();
        }

        private void BindServiceLevelsStats()
        {
            ServiceLevels = ES.Services.Organizations.GetSupportServiceLevels();
            OrganizationStatistics stats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);

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

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "create_user",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        public string GetUserEditUrl(string accountId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_user",
                    "AccountID=" + accountId,
                    "ItemID=" + PanelRequest.ItemID,
                    "Context=User");
        }

        protected void odsAccountsPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                messageBox.ShowErrorMessage("ORGANZATION_GET_USERS", e.Exception);
                e.ExceptionHandled = true;
            }
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                int rowIndex = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                var accountId = Utils.ParseInt(gvUsers.DataKeys[rowIndex][0], 0);

                var accountType = (ExchangeAccountType)gvUsers.DataKeys[rowIndex][1];

                    chkEnableForceArchiveMailbox.Visible = false;

                hdAccountId.Value = accountId.ToString();

                DeleteUserModal.Show();
            }

            if (e.CommandName == "OpenMailProperties")
            {
                int accountId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "mailbox_settings",
                    "AccountID=" + accountId,
                    "ItemID=" + PanelRequest.ItemID));
            }

            if (e.CommandName == "OpenBlackBerryProperties")
            {
                int accountId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_blackberry_user",
                    "AccountID=" + accountId,
                    "ItemID=" + PanelRequest.ItemID));
            }

            if (e.CommandName == "OpenCRMProperties")
            {
                int accountId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "mailbox_settings",
                    "AccountID=" + accountId,
                    "ItemID=" + PanelRequest.ItemID));
            }

            if (e.CommandName == "OpenUCProperties")
            {
                string[] Tmp = e.CommandArgument.ToString().Split('|');

                int accountId = Utils.ParseInt(Tmp[0], 0);
                if (Tmp[1] == "True")
                    Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_ocs_user",
                        "AccountID=" + accountId,
                        "ItemID=" + PanelRequest.ItemID));
                if (Tmp[2] == "True")
                        Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_lync_user",
                            "AccountID=" + accountId,
                            "ItemID=" + PanelRequest.ItemID));
                else
                    if (Tmp[3] == "True")
                    Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_sfb_user",
                        "AccountID=" + accountId,
                        "ItemID=" + PanelRequest.ItemID));
            }
        }

        public string GetAccountImage(int accountTypeId, bool vip)
        {
            string imgName = string.Empty;

            ExchangeAccountType accountType = (ExchangeAccountType)accountTypeId;
            switch (accountType)
            {
                case ExchangeAccountType.Room:
                    imgName = "room_16.gif";
                    break;
                case ExchangeAccountType.Equipment:
                    imgName = "equipment_16.gif";
                    break;
                default:
                    imgName = "admin_16.png";
                    break;
            }
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


        public string GetMailImage(int accountTypeId)
        {
            string imgName = "exchange24.png";

            ExchangeAccountType accountType = (ExchangeAccountType)accountTypeId;

            if (accountType == ExchangeAccountType.User)
                imgName = "blank16.gif";

            return GetThemedImage("Exchange/" + imgName);
        }

        public string GetOCSImage(bool IsOCSUser, bool IsLyncUser, bool IsSfBUser)
        {
            string imgName = "blank16.gif";

            if (IsLyncUser)
                imgName = "lync16.png";
            if (IsSfBUser)
                imgName = "SfB16.png";
            else
                if ((IsOCSUser))
                    imgName = "ocs16.png";

            return GetThemedImage("Exchange/" + imgName);
        }

        public string GetBlackBerryImage(bool IsBlackBerryUser)
        {
            string imgName = "blank16.gif";

            if (IsBlackBerryUser)
                imgName = "blackberry16.png";

            return GetThemedImage("Exchange/" + imgName);
        }

        public string GetCRMImage(Guid CrmUserId)
        {
            string imgName = "blank16.gif";

            if (CrmUserId != Guid.Empty)
                imgName = "crm_16.png";

            return GetThemedImage("Exchange/" + imgName);
        }


        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)   
        {   
            gvUsers.PageSize = Convert.ToInt16(ddlPageSize.SelectedValue);   
                 
            // rebind grid   
            gvUsers.DataBind();   
       
            // bind stats   
            BindStats();   
        }


        public bool EnableMailImageButton(int accountTypeId)
        {
            bool imgName = true;

            ExchangeAccountType accountType = (ExchangeAccountType)accountTypeId;

            if (accountType == ExchangeAccountType.User)
                imgName = false;

            return imgName;
        }

        public bool EnableOCSImageButton(bool IsOCSUser, bool IsLyncUser, bool IsSfBUser)
        {
            bool imgName = false;

            if (IsLyncUser)
                imgName = true;
            if (IsSfBUser)
                imgName = true;
            else
                if ((IsOCSUser))
                    imgName = true;

            return imgName;
        }

        public bool EnableBlackBerryImageButton(bool IsBlackBerryUser)
        {
            bool imgName = false;

            if (IsBlackBerryUser)
                imgName = true;

            return imgName;
        }


        public string GetOCSArgument(int accountID, bool IsOCS, bool IsLync, bool IsSfB)
        {
            return accountID.ToString() + "|" + IsOCS.ToString() + "|" + IsLync.ToString() + "|" + IsSfB.ToString();
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteUserModal.Hide();

            // delete user
            try
            {
                int result = 0;
                    result = ES.Services.Organizations.DeleteUser(PanelRequest.ItemID, int.Parse(hdAccountId.Value));


                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

                // rebind grid
                gvUsers.DataBind();

                // bind stats
                BindStats();
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("ORGANIZATIONS_DELETE_USERS", ex);
            }
        }

    }
}
