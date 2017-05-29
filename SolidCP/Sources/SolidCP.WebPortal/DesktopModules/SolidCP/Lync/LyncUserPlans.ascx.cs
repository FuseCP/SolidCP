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
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;

namespace SolidCP.Portal.Lync
{
    public partial class LyncUserPlans : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPlans();

                txtStatus.Visible = false;

                if (PanelSecurity.LoggedUser.Role == UserRole.User)
                {
                    PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
                    if (cntx.Quotas.ContainsKey(Quotas.LYNC_ENABLEDPLANSEDITING))
                    {
                        if (cntx.Quotas[Quotas.LYNC_ENABLEDPLANSEDITING].QuotaAllocatedValue != 1)
                        {
                            gvPlans.Columns[2].Visible = false;
                            btnAddPlan.Enabled = btnAddPlan.Visible = false;
                        }
                    }
                }


            }

        }

        public string GetPlanDisplayUrl(string LyncUserPlanId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "add_lyncuserplan",
                    "LyncUserPlanId=" + LyncUserPlanId,
                    "ItemID=" + PanelRequest.ItemID);
        }


        private void BindPlans()
        {
            LyncUserPlan[] list = ES.Services.Lync.GetLyncUserPlans(PanelRequest.ItemID);

            gvPlans.DataSource = list;
            gvPlans.DataBind();

            //check if organization has only one default domain
            if (gvPlans.Rows.Count == 1)
            {
                btnSetDefaultPlan.Enabled = false;
            }

            btnSave.Enabled = (gvPlans.Rows.Count >= 1);
        }

        public string IsChecked(bool val)
        {
            return val ? "checked" : "";
        }

        protected void btnAddPlan_Click(object sender, EventArgs e)
        {
            btnSetDefaultPlan.Enabled = true;
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "add_lyncuserplan",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        protected void gvPlan_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                int planId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                try
                {
                    LyncUserPlan plan = ES.Services.Lync.GetLyncUserPlan(PanelRequest.ItemID, planId);

                    if (plan.LyncUserPlanType > 0)
                    {
                        ShowErrorMessage("EXCHANGE_UNABLE_USE_SYSTEMPLAN");
                        BindPlans();
                        return;
                    }


                    int result = ES.Services.Lync.DeleteLyncUserPlan(PanelRequest.ItemID, planId);

                    if (result < 0)
                    {
                        messageBox.ShowResultMessage(result);
                        return;
                    }
                    else
                        ShowSuccessMessage("REQUEST_COMPLETED_SUCCESFULLY");


                }
                catch (Exception)
                {
                    messageBox.ShowErrorMessage("LYNC_DELETE_PLAN");
                }

                BindPlans();
            }
        }

        protected void btnSetDefaultPlan_Click(object sender, EventArgs e)
        {
            // get domain
            int planId = Utils.ParseInt(Request.Form["DefaultPlan"], 0);

            try
            {
                /*
                LyncUserPlan plan = ES.Services.Lync.GetLyncUserPlan(PanelRequest.ItemID, planId);

                if (plan.LyncUserPlanType > 0)
                {
                    ShowErrorMessage("EXCHANGE_UNABLE_USE_SYSTEMPLAN");
                    BindPlans();
                    return;
                }
                */
                ES.Services.Lync.SetOrganizationDefaultLyncUserPlan(PanelRequest.ItemID, planId);

                ShowSuccessMessage("REQUEST_COMPLETED_SUCCESFULLY");

                // rebind domains
                BindPlans();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("LYNC_SET_DEFAULT_PLAN", ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            txtStatus.Visible = true;

            try
            {
                LyncUser[] Accounts = ES.Services.Lync.GetLyncUsersByPlanId(PanelRequest.ItemID, Convert.ToInt32(lyncUserPlanSelectorSource.planId));

                foreach (LyncUser a in Accounts)
                {
                    txtStatus.Text = "Completed";
                    LyncUserResult result = ES.Services.Lync.SetUserLyncPlan(PanelRequest.ItemID, a.AccountID, Convert.ToInt32(lyncUserPlanSelectorTarget.planId));
                    if (result.IsSuccess)
                    {
                        BindPlans();
                        txtStatus.Text = "Error: " + a.DisplayName;
                        ShowErrorMessage("LYNC_FAILED_TO_STAMP");
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage("LYNC_FAILED_TO_STAMP", ex);
            }
        }



        public string GetPlanType(int planType)
        {
            string imgName = string.Empty;

            LyncUserPlanType type = (LyncUserPlanType)planType;
            switch (type)
            {
                case LyncUserPlanType.Reseller:
                    imgName = "company24.png";
                    break;
                case LyncUserPlanType.Administrator:
                    imgName = "company24.png";
                    break;
                default:
                    imgName = "admin_16.png";
                    break;
            }

            return GetThemedImage("Exchange/" + imgName);
        }


    }
}
