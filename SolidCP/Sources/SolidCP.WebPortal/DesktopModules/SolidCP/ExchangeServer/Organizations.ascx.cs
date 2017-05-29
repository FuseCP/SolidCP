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
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.HostedSolution;

namespace SolidCP.Portal.ExchangeServer
{
    public partial class Organizations : SolidCPModuleBase
    {
        private int CurrentDefaultOrgId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            // set display preferences
            gvOrgs.PageSize = UsersHelper.GetDisplayItemsPerPage();

            // visibility
            chkRecursive.Visible = (PanelSecurity.SelectedUser.Role != UserRole.User);
            gvOrgs.Columns[2].Visible = gvOrgs.Columns[3].Visible = (PanelSecurity.SelectedUser.Role != UserRole.User) && chkRecursive.Checked;

            btnSetDefaultOrganization.Enabled = !(gvOrgs.Rows.Count < 2);

            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
            if (cntx.Quotas.ContainsKey(Quotas.ORGANIZATIONS))
            {
                btnCreate.Enabled = (!(cntx.Quotas[Quotas.ORGANIZATIONS].QuotaAllocatedValue <= gvOrgs.Rows.Count) || (cntx.Quotas[Quotas.ORGANIZATIONS].QuotaAllocatedValue == -1));
            }

            /*
            if (PanelSecurity.LoggedUser.Role == UserRole.User)
            {
                gvOrgs.Columns[2].Visible = gvOrgs.Columns[3].Visible = gvOrgs.Columns[5].Visible = false;
                btnCreate.Enabled = false;
                btnSetDefaultOrganization.Enabled = false;
            }
             */

            if (!Page.IsPostBack)
            {
                RedirectToRequiredOrg();
            }
        }

        private List<string> GetPossibleUrlRefferers()
        {
            List<string> urlReferrers = new List<string>();
            var queryBuilder = new StringBuilder();

            queryBuilder.AppendFormat("?pid=Home&UserID={0}", PanelSecurity.SelectedUserId);

            urlReferrers.Add(queryBuilder.ToString());
            urlReferrers.Add("?pid=Home");
            urlReferrers.Add("?");
            urlReferrers.Add(string.Empty);

            queryBuilder.Clear();

            return urlReferrers;
        }

        private void RedirectToRequiredOrg()
        {
            if (Request.UrlReferrer != null && gvOrgs.Rows.Count > 0)
            {
                List<string> referrers = GetPossibleUrlRefferers();

                if (PanelSecurity.SelectedUser.Role == UserRole.User)
                {
                    if (Request.UrlReferrer.Query.Equals(referrers[0]))
                    {
                        RedirectToOrgHomePage();
                    }
                }

                if (PanelSecurity.LoggedUser.Role == UserRole.User)
                {
                    if (referrers.Contains(Request.UrlReferrer.Query))
                    {
                        RedirectToOrgHomePage();
                    }
                }
            }
        }

        private void RedirectToOrgHomePage()
        {
            if (CurrentDefaultOrgId > 0) Response.Redirect(GetOrganizationEditUrl(CurrentDefaultOrgId.ToString()));

            Response.Redirect(((HyperLink)gvOrgs.Rows[0].Cells[1].Controls[1]).NavigateUrl);
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "create_organization"));
        }

        protected void odsOrgsPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                messageBox.ShowErrorMessage("GET_ORGS", e.Exception);
                e.ExceptionHandled = true;
            }
        }

        public string GetOrganizationEditUrl(string itemId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "organization_home",
                    "ItemID=" + itemId);
        }

        public string GetUserHomePageUrl(int userId)
        {
            return PortalUtils.GetUserHomePageUrl(userId);
        }

        public string GetSpaceHomePageUrl(int spaceId)
        {
            return NavigateURL(PortalUtils.SPACE_ID_PARAM, spaceId.ToString());
        }

        protected void gvOrgs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                // delete organization
                int itemId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                try
                {
                    int result = ES.Services.Organizations.DeleteOrganization(itemId);
                    if (result < 0)
                    {
                        messageBox.ShowResultMessage(result);
                        return;
                    }

                    // rebind grid
                    gvOrgs.DataBind();

                    orgsQuota.BindQuota();

                    PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
                    if (cntx.Quotas.ContainsKey(Quotas.ORGANIZATIONS))
                    {
                        btnCreate.Enabled = !(cntx.Quotas[Quotas.ORGANIZATIONS].QuotaAllocatedValue <= gvOrgs.Rows.Count);
                    }

                }
                catch (Exception ex)
                {
                    messageBox.ShowErrorMessage("DELETE_ORG", ex);
                }
            }
        }

        protected void btnSetDefaultOrganization_Click(object sender, EventArgs e)
        {
            // get org
            int newDefaultOrgId = Utils.ParseInt(Request.Form["DefaultOrganization"], CurrentDefaultOrgId);

            try
            {
                ES.Services.Organizations.SetDefaultOrganization(newDefaultOrgId, CurrentDefaultOrgId);

                ShowSuccessMessage("REQUEST_COMPLETED_SUCCESFULLY");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("ORGANIZATION_SET_DEFAULT_ORG", ex);
            }
        }

        public string IsChecked(string val, string itemId)
        {
            if (!string.IsNullOrEmpty(val) && val.ToLowerInvariant() == "true")
            {
                CurrentDefaultOrgId = Utils.ParseInt(itemId, 0);
                return "checked";
            }

            return "";
        }
    }
}
