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
                rbPlanQuotas.Visible = rbPackageQuotas.Visible = false; //TODO: at this moment not work, look at line 255 - if (rbPackageQuotas.Checked).
                
                if (!IsPostBack)
                {
                    rbPlanQuotas.Checked = true;
                    chkRedirectToCreateVPS.Checked = chkRedirectToCreateVPS.Visible = false;
                    chkIntegratedOUProvisioning.Visible = false;
                    ftpAccountName.SetUserPolicy(PanelSecurity.SelectedUserId, UserSettings.FTP_POLICY, "UserNamePolicy");
                    BindParentSettings(PanelSecurity.SelectedUserId);
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

        private void BindParentSettings(int userId) //Obvious options, which by default must be disabled/enabled
        {
            UserInfo[] users = ES.Services.Users.GetUserParents(userId);
            UserSettings parentSettings = ES.Services.Users.GetUserSettings(users.LastOrDefault().UserId, UserSettings.PACKAGE_SUMMARY_LETTER);
            chkPackageLetter.Checked = Utils.ParseBool(parentSettings["EnableLetter"], false); //TODO: Hide the option at all (chkPackageLetter.Enabled)?
            //something else?
        }

        private void BindHostingPlans(int userId)
        {
            ddlPlans.DataSource = ES.Services.Packages.GetUserAvailableHostingPlans(userId);
            ddlPlans.DataBind();

            ddlPlans.Items.Insert(0, new ListItem(GetLocalizedString("SelectHostingPlan.Text"), ""));

            if (ES.Services.Packages.GetUserAvailableHostingAddons(userId).Length == 0)
            {
                btnAddAddon.Visible 
                    = rbPlanQuotas.Visible 
                    = rbPackageQuotas.Visible 
                    = repHostingAddons.Visible 
                    = tblRowAddons1.Visible
                    = tblRowAddons2.Visible
                    = false;
            }
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
                    chkRedirectToCreateVPS.Checked = chkRedirectToCreateVPS.Visible = cntx.Groups.ContainsKey(ResourceGroups.VPS2012);
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
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("USERWIZARD_CREATE_ACCOUNT", ex);
                return;
            }

            // Save addons
            try
            {
                int spaceId = result.Result;
                foreach (RepeaterItem item in repHostingAddons.Items)
                {
                    PackageAddonInfo addon = new PackageAddonInfo();
                    addon.PackageAddonId = 0; //PanelRequest.PackageAddonID;
                    addon.PackageId = spaceId; //PanelSecurity.PackageId;
                    addon.Comments = "";
                    addon.PlanId = Utils.ParseInt(GetDropDownListSelectedValue(item, "ddlPlan"), 0);
                    addon.StatusId = Utils.ParseInt(ddlStatus.SelectedValue, 0);
                    addon.PurchaseDate = DateTime.Now;
                    addon.Quantity = Utils.ParseInt(GetTextBoxText(item, "txtQuantity"), 1);
                    PackageResult addonResult = ES.Services.Packages.AddPackageAddon(addon);
                }

                if (rbPackageQuotas.Checked)
                {
                    //TODO: add logic to recalculate quota
                    //If checked rbPackageQuotas take all addons quota, sum it and replace to main hosting quota
                    //You can look the idea from SpaceEditDetails, but in SpaceEditDetails it is manually, need automatic here
                    //At this moment here is a lot of work, maybe later.
                }

                //PackageContext cntx = PackagesHelper.GetCachedPackageContext(spaceId);
                //string resourceGroup = "VPS2012";
                //if (cntx != null && cntx.Groups.ContainsKey(resourceGroup))
                //{
                //    string pageId = "SpaceVPS2012";
                //    string pageUrl = PortalUtils.NavigatePageURL(
                //pageId, PortalUtils.SPACE_ID_PARAM, spaceId.ToString(), null);
                //    Response.Redirect(pageUrl);
                //}               
                    
            }
            catch
            {
                //If something happens here, just ignore it. Addons not so important that a Hosting Space
            }

            if (chkRedirectToCreateVPS.Checked)
            {
                string pageId = "SpaceVPS2012";
                string pageUrl = PortalUtils.NavigatePageURL(
                            pageId, PortalUtils.SPACE_ID_PARAM, result.Result.ToString(), null);
                Response.Redirect(pageUrl);
            }
            else
            {
                // go to space home
                Response.Redirect(PortalUtils.GetSpaceHomePageUrl(result.Result));
            }            
        }

        // Hosting Addons Remove
        protected void btnRemoveAddAddon_OnCommand(object sender, CommandEventArgs e)
        {
            //rbPlanQuotas.Checked = true;
            var addons = GetAddons();
            addons.RemoveAt(Convert.ToInt32(e.CommandArgument));
            RebindAddons(addons);
        }

        // Hosting Addons add
        protected void btnAddAddon_Click(object sender, EventArgs e)
        {
            //rbPlanQuotas.Checked = true;
            var addons = GetAddons();
            addons.Add(new PackageAddonInfo());
            RebindAddons(addons);
        }
        
        private List<PackageAddonInfo> GetAddons()
        {
            var result = new List<PackageAddonInfo>();
            
            foreach (RepeaterItem item in repHostingAddons.Items)
            {                
                var addon = new PackageAddonInfo();                              
                addon.PackageAddonId = GetDropDownListSelectedValue(item, "ddlPlan");
                addon.Quantity = Utils.ParseInt(GetTextBoxText(item, "txtQuantity"), 1);
                result.Add(addon);
            }

            return result;
        }

        private HostingPlanInfo[] GetHostingPlanAddonsInfo()
        {
            UserInfo user = UsersHelper.GetUser(PanelSecurity.SelectedUserId);
            HostingPlanInfo[] hpi = ES.Services.Packages.GetUserAvailableHostingAddons(user.UserId);

            for (int i = 0; i < hpi.Length; i++)
            {
                hpi[i].PlanDescription = PortalAntiXSS.DecodeOld(hpi[i].PlanDescription);
                hpi[i].PlanName = PortalAntiXSS.DecodeOld(hpi[i].PlanName);
            }

            return hpi;
        }

        private void RebindAddons(List<PackageAddonInfo> addons)
        {
            repHostingAddons.DataSource = addons;
            repHostingAddons.DataBind();
            int i = 0;
            HostingPlanInfo[] hpi = GetHostingPlanAddonsInfo();
            foreach (RepeaterItem item in repHostingAddons.Items)
            {
                SetDropDownListSelectedIndex(item, "ddlPlan", hpi);
                (item.FindControl("ddlPlan") as DropDownList).SelectedValue = addons[i].PackageAddonId.ToString();
                //(item.FindControl("txtQuantity") as TextBox).Text = addons[i].Quantity.ToString();
                i++;
            }
        }
        private void SetDropDownListSelectedIndex(RepeaterItem item, string name, object[] source)
        {
            (item.FindControl(name) as DropDownList).DataSource = source;
            (item.FindControl(name) as DropDownList).DataBind();
            (item.FindControl(name) as DropDownList).Items.Insert(0, new ListItem(GetLocalizedString("SelectHostingPlan.Text"), "-1"));
        }
        private int GetDropDownListSelectedValue(RepeaterItem item, string name)
        {
            return Convert.ToInt32((item.FindControl(name) as DropDownList).SelectedValue);
        }
        private string GetTextBoxText(RepeaterItem item, string name)
        {
            return (item.FindControl(name) as TextBox).Text;
        }
        private void UnCkeckRadioButton(RepeaterItem item, string name)
        {
            (item.FindControl(name) as RadioButton).Checked = false;
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
