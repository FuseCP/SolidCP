// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2014, Outercurve Foundation.
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
using SolidCP.EnterpriseServer.Base.HostedSolution;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;

namespace SolidCP.Portal.HostedSolution
{
    public partial class DeletedUserGeneralSettings : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindServiceLevels();

                BindSettings();

                MailboxTabsId.Visible = (PanelRequest.Context == "Mailbox");
                UserTabsId.Visible = (PanelRequest.Context == "User");
            }
        }

        private void BindSettings()
        {
            try
            {
                                // get settings
                OrganizationUser user = ES.Services.Organizations.GetUserGeneralSettings(PanelRequest.ItemID,
                    PanelRequest.AccountID);

                litDisplayName.Text = PortalAntiXSS.Encode(user.DisplayName);

                lblUserDomainName.Text = user.DomainUserName;

                // bind form
                lblDisplayName.Text = user.DisplayName;

                chkDisable.Checked = user.Disabled;

                lblFirstName.Text = user.FirstName;
                lblInitials.Text = user.Initials;
                lblLastName.Text = user.LastName;



                lblJobTitle.Text = user.JobTitle;
                lblCompany.Text = user.Company;
                lblDepartment.Text = user.Department;
                lblOffice.Text = user.Office;

                if (user.Manager != null)
                {
                    lblManager.Text = user.Manager.DisplayName;
                }

                lblBusinessPhone.Text = user.BusinessPhone;
                lblFax.Text = user.Fax;
                lblHomePhone.Text = user.HomePhone;
                lblMobilePhone.Text = user.MobilePhone;
                lblPager.Text = user.Pager;
                lblWebPage.Text = user.WebPage;

                lblAddress.Text = user.Address;
                lblCity.Text = user.City;
                lblState.Text = user.State;
                lblZip.Text = user.Zip;
                lblCountry.Text = user.Country;

                lblNotes.Text = user.Notes;
                lblExternalEmailAddress.Text = user.ExternalEmail;

                lblExternalEmailAddress.Enabled = user.AccountType == ExchangeAccountType.User;
                lblUserDomainName.Text = user.DomainUserName;

                lblSubscriberNumber.Text = user.SubscriberNumber;
                lblUserPrincipalName.Text = user.UserPrincipalName;

                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
                if (cntx.Quotas.ContainsKey(Quotas.EXCHANGE2007_ISCONSUMER))
                {
                    if (cntx.Quotas[Quotas.EXCHANGE2007_ISCONSUMER].QuotaAllocatedValue != 1)
                    {
                        locSubscriberNumber.Visible = false;
                        lblSubscriberNumber.Visible = false;
                    }
                }

                if (user.LevelId > 0 && secServiceLevels.Visible)
                {
                    secServiceLevels.IsCollapsed = false;

                    ServiceLevel serviceLevel = ES.Services.Organizations.GetSupportServiceLevel(user.LevelId);

                    litServiceLevel.Visible = true;
                    litServiceLevel.Text = serviceLevel.LevelName;
                    litServiceLevel.ToolTip = serviceLevel.LevelDescription;

                    lblServiceLevel.Text = serviceLevel.LevelName;
                }

                chkVIP.Checked = user.IsVIP && secServiceLevels.Visible;
                imgVipUser.Visible = user.IsVIP && secServiceLevels.Visible;

                if (cntx.Quotas.ContainsKey(Quotas.ORGANIZATION_ALLOWCHANGEUPN))
                {
                    if (cntx.Quotas[Quotas.ORGANIZATION_ALLOWCHANGEUPN].QuotaAllocatedValue != 1)
                    {
                        chkInherit.Visible = false;
                    }
                    else
                    {
                        chkInherit.Visible = true;
                    }
                }

                if (user.Locked)
                    chkLocked.Enabled = true;
                else
                    chkLocked.Enabled = false;

                chkLocked.Checked = user.Locked;
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("ORGANIZATION_GET_USER_SETTINGS", ex);
            }
        }

        private void BindServiceLevels()
        {
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            if (cntx.Groups.ContainsKey(ResourceGroups.ServiceLevels))
            {
                secServiceLevels.Visible = true;
            }
            else
            {
                secServiceLevels.Visible = false;
            }
        }

        private bool CheckServiceLevelQuota(QuotaValueInfo quota)
        {

            if (quota.QuotaAllocatedValue != -1)
            {
                return quota.QuotaAllocatedValue > quota.QuotaUsedValue;
            }

            return true;
        }
    }
}