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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SolidCP.EnterpriseServer;

namespace SolidCP.Portal
{
    public partial class DomainsEditDomain : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDomain();

                if (GetLocalizedString("buttonPanel.OnSaveClientClick") != null)
                    buttonPanel.OnSaveClientClick = GetLocalizedString("buttonPanel.OnSaveClientClick");
            }
        }

        private void BindDomain()
        {
            try
            {
                // load domain
                DomainInfo domain = ES.Services.Servers.GetDomain(PanelRequest.DomainID);
                if (domain == null)
                    RedirectToBrowsePage();


                // load package context
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(domain.PackageId);

                DomainName.Text = domain.DomainName;

                bool webEnabled = cntx.Groups.ContainsKey(ResourceGroups.Web);
                bool mailEnabled = cntx.Groups.ContainsKey(ResourceGroups.Mail);
                bool dnsEnabled = cntx.Groups.ContainsKey(ResourceGroups.Dns);

                // web site
                if (webEnabled && domain.WebSiteId > 0)
                {
                    SolidCP.Visible = true;
                    WebSiteAliasPanel.Visible = true;

                    WebSiteName.Text = domain.WebSiteName;
                    WebSiteParkedPanel.Visible = (String.Compare(domain.WebSiteName, domain.DomainName, true) == 0);
                    WebSitePointedPanel.Visible = !WebSiteParkedPanel.Visible;

                    BrowseWebSite.NavigateUrl = "http://" + domain.DomainName;
                }

                // mail domain
                if (mailEnabled && domain.MailDomainId > 0)
                {
                    MailDomainPanel.Visible = true;
                    MailDomainAliasPanel.Visible = true;

                    MailDomainName.Text = domain.MailDomainName;
                    MailEnabledPanel.Visible = (String.Compare(domain.MailDomainName, domain.DomainName, true) == 0);
                    PointMailDomainPanel.Visible = !MailEnabledPanel.Visible;
                }

                // DNS
                if (dnsEnabled)
                {
                    DnsPanel.Visible = true;
                    DnsEnabledPanel.Visible = (domain.ZoneItemId > 0);
                    DnsDisabledPanel.Visible = !DnsEnabledPanel.Visible;

                    // dns editor
                    EditDnsRecords.Visible = (cntx.Quotas.ContainsKey(Quotas.DNS_EDITOR)
                        && cntx.Quotas[Quotas.DNS_EDITOR].QuotaAllocatedValue != 0)
                        || PanelSecurity.LoggedUser.Role == UserRole.Administrator;
                }

                // instant alias
                PackageSettings settings = ES.Services.Packages.GetPackageSettings(PanelSecurity.PackageId, PackageSettings.INSTANT_ALIAS);

                bool instantAliasAllowed = !String.IsNullOrEmpty(domain.InstantAliasName);
                bool instantAliasExists = (domain.InstantAliasId > 0) && (settings != null && !String.IsNullOrEmpty(settings["InstantAlias"]));
                if (instantAliasAllowed
                    && !domain.IsDomainPointer && !domain.IsInstantAlias)
                {
                    InstantAliasPanel.Visible = true;
                    InstantAliasEnabled.Visible = instantAliasExists;
                    InstantAliasDisabled.Visible = !instantAliasExists;

                    // load instant alias
                    DomainInfo instantAlias = ES.Services.Servers.GetDomain(domain.InstantAliasId);
                    WebSiteAliasPanel.Visible = false;
                    if (instantAlias != null)
                    {
                        DomainInfo[] Domains = ES.Services.Servers.GetDomainsByDomainId(domain.InstantAliasId);
                        foreach (DomainInfo d in Domains)
                        {
                            if (d.WebSiteId > 0)
                            {
                                WebSiteAliasPanel.Visible = true;
                            }
                        }

                        MailDomainAliasPanel.Visible = (instantAlias.MailDomainId > 0);
                    }

                    // instant alias
                    InstantAliasName.Text = domain.InstantAliasName;

                    // web site alias
                    WebSiteAlias.Text = WebSiteAlias.NavigateUrl = "http://" + domain.InstantAliasName;

                    // mail domain alias
                    MailDomainAlias.Text = "@" + domain.InstantAliasName;
                }

                // resellers
                AllowSubDomains.Checked = domain.HostingAllowed;
                if (PanelSecurity.EffectiveUser.Role != UserRole.User
                    && !(domain.IsDomainPointer || domain.IsSubDomain || domain.IsInstantAlias))
                {
                    ResellersPanel.Visible = true;
                }

                if (!(domain.IsDomainPointer || domain.IsSubDomain || domain.IsInstantAlias))
                {
                    UserInfo user = UsersHelper.GetUser(PanelSecurity.EffectiveUserId);

                    if (user != null)
                    {
                        if (user.Role == UserRole.User)
                        {
                            btnDelete.Enabled = !Utils.CheckQouta(Quotas.OS_NOTALLOWTENANTCREATEDOMAINS, cntx);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage("DOMAIN_GET_DOMAIN", ex);
                return;
            }
        }

        private void SaveDomain()
        {
            // load original domain
            DomainInfo domain = ES.Services.Servers.GetDomain(PanelRequest.DomainID);
            if (domain == null)
                RedirectToBrowsePage();

            // change domain
            domain.HostingAllowed = AllowSubDomains.Checked;

            // save
            try
            {
                int result = ES.Services.Servers.UpdateDomain(domain);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
                ShowSuccessMessage("DOMAIN_UPDATE_DOMAIN");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("DOMAIN_UPDATE_DOMAIN", ex);
                return;
            }
        }

        private void DeleteDomain()
        {
            // save
            try
            {
                int result = ES.Services.Servers.DeleteDomain(PanelRequest.DomainID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
                 //Delete Domain to Mail Cleaner
                 Knom.Helpers.Net.APIMailCleanerHelper.DomainRemove(DomainName.Text);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("DOMAIN_DELETE_DOMAIN", ex);
                return;
            }

            // return
            RedirectSpaceHomePage();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveDomain();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            SaveDomain();

            // return
            RedirectSpaceHomePage();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteDomain();
        }

        protected void EditDnsRecords_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("DomainID", PanelRequest.DomainID.ToString(), "zone_records",
                "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }

        protected void DisableDns_Click(object sender, EventArgs e)
        {
            try
            {
                // disable DNS
                int result = ES.Services.Servers.DisableDomainDns(PanelRequest.DomainID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                // re-bind
                BindDomain();

                // show message
                ShowSuccessMessage("DOMAIN_DISABLE_DNS");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("DOMAIN_DISABLE_DNS", ex);
                return;
            }
        }

        protected void EnableDns_Click(object sender, EventArgs e)
        {
            try
            {
                // enable DNS
                int result = ES.Services.Servers.EnableDomainDns(PanelRequest.DomainID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                // re-bind
                BindDomain();

                // show message
                ShowSuccessMessage("DOMAIN_ENABLE_DNS");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("DOMAIN_ENABLE_DNS", ex);
                return;
            }
        }

        protected void DeleteInstantAlias_Click(object sender, EventArgs e)
        {
            try
            {
                // delete instant alias
                int result = ES.Services.Servers.DeleteDomainInstantAlias(PanelRequest.DomainID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                // re-bind
                BindDomain();

                // show message
                ShowSuccessMessage("DOMAIN_DELETE_INSTANT_ALIAS");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("DOMAIN_DELETE_INSTANT_ALIAS", ex);
                return;
            }
        }

        protected void CreateInstantAlias_Click(object sender, EventArgs e)
        {
            try
            {
                // create instant alias
                int result = ES.Services.Servers.CreateDomainInstantAlias("", PanelRequest.DomainID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                // re-bind
                BindDomain();

                // show message
                ShowSuccessMessage("DOMAIN_CREATE_INSTANT_ALIAS");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("DOMAIN_CREATE_INSTANT_ALIAS", ex);
                return;
            }
        }
    }
}
