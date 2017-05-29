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
using System.Web;
using SolidCP.EnterpriseServer;
using System.Collections.Generic;
using SolidCP.Portal.UserControls;
//using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Net;
using System.IO;

namespace SolidCP.Portal
{
	public partial class DomainsAddDomain : SolidCPModuleBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{ 
				// bind controls
				BindControls();

                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

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

                DomainType type = GetDomainType(Request["DomainType"]);

                if ((PanelSecurity.LoggedUser.Role == UserRole.User) & (type != DomainType.SubDomain))
                {
                    if (cntx.Groups.ContainsKey(ResourceGroups.Dns))
                    {
                        if (!PackagesHelper.CheckGroupQuotaEnabled(PanelSecurity.PackageId, ResourceGroups.Dns, Quotas.DNS_EDITOR))
                            this.DisableControls = true;
                    }
                }
			}
			catch (Exception ex)
			{
				ShowErrorMessage("DOMAIN_GET_DOMAIN", ex);
			}
		}

		private void BindControls()
		{
			// get domain type
			DomainType type = GetDomainType(Request["DomainType"]);

			// enable domain/sub-domain fields
			if (type == DomainType.Domain || type == DomainType.DomainPointer)
			{
				// domains
			    DomainName.IsSubDomain = false;
			}
			else
			{
				// sub-domains
                DomainName.IsSubDomain = true;

				// fill sub-domains
				if (!IsPostBack)
				{
					if (type == DomainType.SubDomain)
						BindUserDomains();
					else
						BindResellerDomains();
				}
			}
			// load package context
			PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

			if ((type == DomainType.DomainPointer || (type == DomainType.Domain)) && !IsPostBack)
			{
                // bind web sites
                WebSitesList.DataSource = ES.Services.WebServers.GetWebSites(PanelSecurity.PackageId, false);
                WebSitesList.DataBind();
			}

            if ((type == DomainType.DomainPointer || (type == DomainType.Domain)) && !IsPostBack)
            {
                // bind mail domains
                MailDomainsList.DataSource = ES.Services.MailServers.GetMailDomains(PanelSecurity.PackageId, false);
                MailDomainsList.DataBind();
            }


			// create web site option
			CreateSolidCP.Visible = (type == DomainType.Domain || type == DomainType.SubDomain)
				&& cntx.Groups.ContainsKey(ResourceGroups.Web);

            if (PointWebSite.Checked)
            {
                CreateWebSite.Checked = false;
                CreateWebSite.Enabled = false;
            }
            else
            {
                CreateWebSite.Enabled = true;
                CreateWebSite.Checked &= CreateSolidCP.Visible;
            }

            // point Web site
            PointSolidCP.Visible = (type == DomainType.DomainPointer || (type == DomainType.Domain))
                && cntx.Groups.ContainsKey(ResourceGroups.Web) && WebSitesList.Items.Count > 0;
            WebSitesList.Enabled = PointWebSite.Checked;

			// point mail domain
            PointMailDomainPanel.Visible = (type == DomainType.DomainPointer || (type == DomainType.Domain))
				&& cntx.Groups.ContainsKey(ResourceGroups.Mail) && MailDomainsList.Items.Count > 0;
			MailDomainsList.Enabled = PointMailDomain.Checked;

			// DNS option
			EnableDnsPanel.Visible = cntx.Groups.ContainsKey(ResourceGroups.Dns);
			EnableDns.Checked &= EnableDnsPanel.Visible;

			// instant alias
			// check if instant alias was setup
			bool instantAliasAllowed = false;
			PackageSettings settings = ES.Services.Packages.GetPackageSettings(PanelSecurity.PackageId, PackageSettings.INSTANT_ALIAS);
			instantAliasAllowed = (settings != null && !String.IsNullOrEmpty(settings["InstantAlias"]));

			InstantAliasPanel.Visible = instantAliasAllowed && (type != DomainType.DomainPointer) /*&& EnableDnsPanel.Visible*/;
			CreateInstantAlias.Checked &= InstantAliasPanel.Visible;

			// allow sub-domains
			AllowSubDomainsPanel.Visible = (type == DomainType.Domain) && PanelSecurity.EffectiveUser.Role != UserRole.User;

		    if (IsPostBack)
		    {
		        CheckForCorrectIdnDomainUsage(DomainName.Text);
		    }
		}

		private DomainType GetDomainType(string typeName)
		{
			DomainType type = DomainType.Domain;

			if (!String.IsNullOrEmpty(typeName))
				type = (DomainType)Enum.Parse(typeof(DomainType), typeName, true);

			return type;
		}

		private void BindUserDomains()
		{
			DomainInfo[] allDomains = ES.Services.Servers.GetMyDomains(PanelSecurity.PackageId);

			// filter domains
			List<DomainInfo> domains = new List<DomainInfo>();
			foreach (DomainInfo domain in allDomains)
				if (!domain.IsDomainPointer && !domain.IsSubDomain && !domain.IsInstantAlias)
					domains.Add(domain);

            DomainName.DataSource = domains;
			DomainName.DataBind();
		}

		private void BindResellerDomains()
		{
			DomainName.DataSource = ES.Services.Servers.GetResellerDomains(PanelSecurity.PackageId);
			DomainName.DataBind();
		}
   
        private void AddDomain()
		{
			if (!Page.IsValid)
				return;

			// get domain type
			DomainType type = GetDomainType(Request["DomainType"]);

			// get domain name
		    var domainName = DomainName.Text;

			int pointWebSiteId = 0;
			int pointMailDomainId = 0;

			// load package context
			PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

			if (type == DomainType.DomainPointer || (type == DomainType.Domain))
			{

                if (PointWebSite.Checked && WebSitesList.Items.Count > 0)
                    pointWebSiteId = Utils.ParseInt(WebSitesList.SelectedValue, 0);
			}

            if (type == DomainType.DomainPointer || (type == DomainType.Domain))
            {
                if (PointMailDomain.Checked && MailDomainsList.Items.Count > 0)
                    pointMailDomainId = Utils.ParseInt(MailDomainsList.SelectedValue, 0);
            }


			// add domain
			int domainId = 0;
			try
			{
				domainId = ES.Services.Servers.AddDomainWithProvisioning(PanelSecurity.PackageId,
					domainName.ToLower(), type, CreateWebSite.Checked, pointWebSiteId, pointMailDomainId,
                    EnableDns.Checked, CreateInstantAlias.Checked, AllowSubDomains.Checked, (PointWebSite.Checked && WebSitesList.Items.Count > 0) ? string.Empty : txtHostName.Text.ToLower());

				if (domainId < 0)
				{
					ShowResultMessage(domainId);
					return;
				}
                //Add Domain to Mail Cleaner
                if (type == DomainType.Domain && PointMailDomain.Checked) //Only For domain -- Ignore Subdomains 
                     Knom.Helpers.Net.APIMailCleanerHelper.DomainAdd(domainName);
            }
			catch (Exception ex)
			{
				ShowErrorMessage("DOMAIN_ADD_DOMAIN", ex);
				return;
			}

			// put created domain to the cookie
			HttpCookie domainCookie = new HttpCookie("CreatedDomainId", domainId.ToString());
			Response.Cookies.Add(domainCookie);

			// return
			RedirectBack();
		}

		private void RedirectBack()
		{
			RedirectSpaceHomePage();
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			// return
			RedirectBack();
		}
		protected void btnAdd_Click(object sender, EventArgs e)
		{
		    if (CheckForCorrectIdnDomainUsage(DomainName.Text))
		    {
		        AddDomain();
		    }
		}

	    private bool CheckForCorrectIdnDomainUsage(string domainName)
	    {
	        // If the choosen domain is a idn domain, don't allow to create mail
	        if (Utils.IsIdnDomain(domainName) && PointMailDomain.Checked)
	        {
	            ShowErrorMessage("IDNDOMAIN_NO_MAIL");
	            return false;
	        }

	        return true;
	    }
	}
}
