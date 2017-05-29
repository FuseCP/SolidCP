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
using System.Globalization;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.HostedSolution;

namespace SolidCP.Portal.ProviderControls
{
    public partial class CRM2011_Settings : SolidCPControlBase, IHostingServiceProviderSettings
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

     

        public void BindSettings(System.Collections.Specialized.StringDictionary settings)
        {
            txtReportingService.Text = settings[Constants.ReportingServer];
            txtSqlServer.Text = settings[Constants.SqlServer];
            txtDomainName.Text = settings[Constants.IFDWebApplicationRootDomain];
            txtPort.Text = settings[Constants.Port];

            txtAppRootDomain.Text = settings[Constants.AppRootDomain];
            txtOrganizationWebService.Text = settings[Constants.OrganizationWebService];
            txtDiscoveryWebService.Text = settings[Constants.DiscoveryWebService];
            txtDeploymentWebService.Text = settings[Constants.DeploymentWebService];

            txtPassword.Text = settings[Constants.Password];
            ViewState["PWD"] = settings[Constants.Password];
            txtUserName.Text = settings[Constants.UserName];

            int selectedAddressid = FindAddressByText(settings[Constants.CRMWebsiteIP]);
            ddlCrmIpAddress.AddressId = (selectedAddressid > 0) ? selectedAddressid : 0; 
            
            Utils.SelectListItem(ddlSchema, settings[Constants.UrlSchema]);

            // Collation
            StringArrayResultObject res = ES.Services.CRM.GetCollationByServiceId(PanelRequest.ServiceId);
            if (res.IsSuccess)
            {
                ddlCollation.DataSource = res.Value;
                ddlCollation.DataBind();
                Utils.SelectListItem(ddlCollation, "Latin1_General_CI_AI"); // default
            }
            Utils.SelectListItem(ddlCollation, settings[Constants.Collation]);

            // Currency
            ddlCurrency.Items.Clear();
            CurrencyArrayResultObject cres = ES.Services.CRM.GetCurrencyByServiceId(PanelRequest.ServiceId);
            if (cres.IsSuccess)
            {
                foreach (Currency currency in cres.Value)
                {
                    ListItem item = new ListItem(string.Format("{0} ({1})",
                                                               currency.RegionName, currency.CurrencyName),
                                                 string.Join("|",
                                                             new string[]
                                                                 {
                                                                     currency.CurrencyCode, currency.CurrencyName,
                                                                     currency.CurrencySymbol, currency.RegionName
                                                                 }));

                    ddlCurrency.Items.Add(item);
                }
                Utils.SelectListItem(ddlCurrency, "USD|US Dollar|$|United States"); // default
            }
            Utils.SelectListItem(ddlCurrency, settings[Constants.Currency]);

            // Base Language
            ddlBaseLanguage.Items.Clear();
            int[] langPacksId = ES.Services.CRM.GetInstalledLanguagePacksByServiceId(PanelRequest.ServiceId);
            if (langPacksId != null)
            {
                foreach (int langId in langPacksId)
                {
                    CultureInfo ci = CultureInfo.GetCultureInfo(langId);
                    ListItem item = new ListItem(ci.EnglishName, langId.ToString());
                    ddlBaseLanguage.Items.Add(item);
                }
                Utils.SelectListItem(ddlBaseLanguage, "1033"); // default
            }
            Utils.SelectListItem(ddlBaseLanguage, settings[Constants.BaseLanguage]);
        }

        public void SaveSettings(System.Collections.Specialized.StringDictionary settings)
        {
            settings[Constants.ReportingServer] = txtReportingService.Text;
            settings[Constants.SqlServer] = txtSqlServer.Text;
            settings[Constants.IFDWebApplicationRootDomain] = txtDomainName.Text;
            settings[Constants.Port] = txtPort.Text;

            settings[Constants.AppRootDomain] = txtAppRootDomain.Text;
            settings[Constants.OrganizationWebService] = txtOrganizationWebService.Text;
            settings[Constants.DiscoveryWebService] = txtDiscoveryWebService.Text;
            settings[Constants.DeploymentWebService] = txtDeploymentWebService.Text;

            settings[Constants.Password] = (txtPassword.Text.Length > 0) ? txtPassword.Text : (string)ViewState["PWD"];
            settings[Constants.UserName] = txtUserName.Text;

            if (ddlCrmIpAddress.AddressId > 0)
			{
				IPAddressInfo address = ES.Services.Servers.GetIPAddress(ddlCrmIpAddress.AddressId);
                if (String.IsNullOrEmpty(address.ExternalIP))
				{
                    settings[Constants.CRMWebsiteIP] = address.InternalIP;
				}
				else
				{
                    settings[Constants.CRMWebsiteIP] = address.ExternalIP;
				}
			}
			else
			{
                settings[Constants.CRMWebsiteIP] = String.Empty;
			}
             
            settings[Constants.UrlSchema] = ddlSchema.SelectedValue;

            settings[Constants.Collation] = ddlCollation.SelectedValue;
            settings[Constants.Currency] = ddlCurrency.SelectedValue;
            settings[Constants.BaseLanguage] = ddlBaseLanguage.SelectedValue;

        }

        private static int FindAddressByText(string address)
        {
            foreach (IPAddressInfo addressInfo in ES.Services.Servers.GetIPAddresses(IPAddressPool.General, PanelRequest.ServerId))
            {
                if (addressInfo.InternalIP == address || addressInfo.ExternalIP == address)
                {
                    return addressInfo.AddressId;
                }
            }
            return 0;
        }

     
    }
}
