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
using SolidCP.EnterpriseServer;
using SolidCP.Portal.UserControls;
using SolidCP.Providers.HostedSolution;

namespace SolidCP.Portal
{
    public partial class UserCreateSpace : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    chkIntegratedOUProvisioning.Visible = false;
                    ftpAccountName.SetUserPolicy(PanelSecurity.SelectedUserId, UserSettings.FTP_POLICY, "UserNamePolicy");
                    BindHostingPlans(PanelSecurity.SelectedUserId);
                    BindHostingPlan();



                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
                this.DisableControls = true;
                //ShowErrorMessage("USERWIZARD_INIT_FORM", ex);
                return;
            }
        }

        private void BindHostingPlans(int userId)
        {
            ddlPlans.DataSource = ES.Services.Packages.GetUserAvailableHostingPlans(userId);
            ddlPlans.DataBind();

            ddlPlans.Items.Insert(0, new ListItem(GetLocalizedString("SelectHostingPlan.Text"), ""));
        }

        private void BindHostingPlan()
        {
            // plan resources
            int planId = Utils.ParseInt(ddlPlans.SelectedValue, 0);

            chkCreateResources.Visible = (planId > 0);
            bool createResources = chkCreateResources.Checked;
            ResourcesPanel.Visible = createResources & chkCreateResources.Visible;
            if (!createResources)
                return;

            if ((PanelSecurity.LoggedUser.Role == UserRole.ResellerCSR) |
                (PanelSecurity.LoggedUser.Role == UserRole.ResellerHelpdesk))
                this.chkCreateResources.Enabled = this.chkIntegratedOUProvisioning.Enabled = false;


            bool systemEnabled = false;
            bool webEnabled = false;
            bool ftpEnabled = false;
            bool mailEnabled = false;
            bool integratedOUEnabled = false;

            // load hosting context
            if (planId > 0)
            {
                HostingPlanContext cntx = PackagesHelper.GetCachedHostingPlanContext(planId);
                if (cntx != null)
                {
                    systemEnabled = cntx.Groups.ContainsKey(ResourceGroups.Os);
                    webEnabled = cntx.Groups.ContainsKey(ResourceGroups.Web);

                    if (Utils.CheckQouta(Quotas.WEB_ENABLEHOSTNAMESUPPORT, cntx))
                    {
                        lblHostName.Visible = txtHostName.Visible = true;
                        UserSettings settings = ES.Services.Users.GetUserSettings(PanelSecurity.LoggedUserId, UserSettings.WEB_POLICY);
                        txtHostName.Text = String.IsNullOrEmpty(settings["HostName"]) ? "" : settings["HostName"];

                    }
                    else
                    {
                        lblHostName.Visible = txtHostName.Visible = false;
                        txtHostName.Text = "";
                    }

                    ftpEnabled = cntx.Groups.ContainsKey(ResourceGroups.Ftp);
                    mailEnabled = cntx.Groups.ContainsKey(ResourceGroups.Mail);

                    if (Utils.CheckQouta(Quotas.ORGANIZATION_DOMAINS, cntx))
                    {
                        integratedOUEnabled = true;
                    }
                }
            }

            // toggle group controls
            fsSystem.Visible = systemEnabled;

            fsWeb.Visible = webEnabled;
            chkCreateWebSite.Checked &= webEnabled;
            

            fsFtp.Visible = ftpEnabled;
            chkCreateFtpAccount.Checked &= ftpEnabled;

            fsMail.Visible = mailEnabled;
            chkCreateMailAccount.Checked &= mailEnabled;

            ftpAccountName.Visible = (rbFtpAccountName.SelectedIndex == 1);

            chkIntegratedOUProvisioning.Checked = chkIntegratedOUProvisioning.Visible = (chkCreateResources.Visible && integratedOUEnabled);
        }

        private void CreateHostingSpace()
        {
            if (!Page.IsValid)
                return;

            string spaceName = ddlPlans.SelectedItem.Text;

            string ftpAccount = (rbFtpAccountName.SelectedIndex == 0) ? null : ftpAccountName.Text;

            string domainName = txtDomainName.Text.Trim();
            
            PackageResult result = null;
            try
            {
                result = ES.Services.Packages.AddPackageWithResources(PanelSecurity.SelectedUserId,
                    Utils.ParseInt(ddlPlans.SelectedValue, 0),
                    spaceName,
                    Utils.ParseInt(ddlStatus.SelectedValue, 0),
                    chkPackageLetter.Checked,
                    chkCreateResources.Checked, domainName, false, chkCreateWebSite.Checked,
                    chkCreateFtpAccount.Checked, ftpAccount, chkCreateMailAccount.Checked, txtHostName.Text);

                if (result.Result < 0)
                {
                    ShowResultMessage(result.Result);
                    lblMessage.Text = PortalAntiXSS.Encode(GetExceedingQuotasMessage(result.ExceedingQuotas));
                    return;

                }
                else
                {
                    if ((chkIntegratedOUProvisioning.Checked) & !string.IsNullOrEmpty(domainName))
                    {
                        UserInfo user = UsersHelper.GetUser(PanelSecurity.SelectedUserId);

                        if (user != null)
                        {
                            if (user.Role != UserRole.Reseller)
                            {
                                UserSettings settings = ES.Services.Users.GetUserSettings(user.UserId, UserSettings.EXCHANGE_POLICY);
                                string orgId = domainName.ToLower();

                                if (settings != null && settings["OrgIdPolicy"] != null)
                                {
                                    orgId = GetOrgId(settings["OrgIdPolicy"], domainName, result.Result);
                                }

                                ES.Services.Organizations.CreateOrganization(result.Result, orgId, domainName.ToLower(), domainName.ToLower());

                                if (result.Result < 0)
                                {
                                    ShowErrorMessage("USERWIZARD_CREATE_ACCOUNT");
                                    return;
                                }

                                //Add Mail Cleaner
                                Knom.Helpers.Net.APIMailCleanerHelper.DomainAdd(domainName, Utils.ParseInt(ddlPlans.SelectedValue, 0));
                            }
                        }
                    }
                    if ((chkCreateMailAccount.Checked) & !string.IsNullOrEmpty(domainName))
                    {
                        //Add Mail Cleaner
                        Knom.Helpers.Net.APIMailCleanerHelper.DomainAdd(domainName, Utils.ParseInt(ddlPlans.SelectedValue, 0));
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("USERWIZARD_CREATE_ACCOUNT", ex);
                return;
            }

            // go to space home
            Response.Redirect(PortalUtils.GetSpaceHomePageUrl(result.Result));
        }

        private string GetOrgId(string orgIdPolicy, string domainName, int packageId)
        {
            string[] values = orgIdPolicy.Split(';');

            if (values.Length > 1 && Convert.ToBoolean(values[0]))
            {
                try
                {
                    int maxLength = Convert.ToInt32(values[1]);
                    
                    if (domainName.Length > maxLength)
                    {
                        domainName = domainName.Substring(0, maxLength);
                        string orgId = domainName;
                        int counter = 0;

                        while (ES.Services.Organizations.CheckOrgIdExists(orgId))
                        {
                            counter++;
                            orgId = maxLength > 3 ? string.Format("{0}{1}", orgId.Substring(0, orgId.Length - 3), counter.ToString("d3")) : counter.ToString("d3");
                        }

                        return orgId;
                    }
                }
                catch (Exception)
                {
                }
            }

            return domainName;
        }

        protected void ddlPlans_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindHostingPlan();
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if (CheckForCorrectIdnDomainUsage())
            {
                CreateHostingSpace();
            }
        }

        protected void rbFtpAccountName_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindHostingPlan();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(NavigateURL(PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString()));
        }

        protected void chkCreateResources_CheckedChanged(object sender, EventArgs e)
        {
            BindHostingPlan();
        }

        private bool CheckForCorrectIdnDomainUsage()
        {
            if (Utils.IsIdnDomain(txtDomainName.Text))
            {
                if (chkIntegratedOUProvisioning.Checked)
                {
                    ShowErrorMessage("IDNDOMAIN_NO_ORGANIZATION");
                    return false;
                }

                if (chkCreateMailAccount.Checked)
                {
                    ShowErrorMessage("IDNDOMAIN_NO_MAIL");
                    return false;
                }
            }

            return true;
        }

        protected void txtDomainName_OnTextChanged(object sender, DomainControl.DomainNameEventArgs e)
        {
            CheckForCorrectIdnDomainUsage();
        }
    }
}
