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
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal.CRM
{
    public partial class CRMOrganizationDetails : SolidCPModuleBase
    {

        private StringDictionary ConvertArrayToDictionary(string[] settings)
        {
            StringDictionary r = new StringDictionary();
            foreach (string setting in settings)
            {
                int idx = setting.IndexOf('=');
                r.Add(setting.Substring(0, idx), setting.Substring(idx + 1));
            }
            return r;
        }

        private StringDictionary serviceSettings = null;

        private StringDictionary ServiceSettings
        {
            get
            {
                if (serviceSettings != null)
                    return serviceSettings;

                PackageInfo pi = PackagesHelper.GetCachedPackage(PanelSecurity.PackageId);
                ServiceInfo[] si = ES.Services.Servers.GetServicesByServerIdGroupName(pi.ServerId, ResourceGroups.HostedCRM2013);
                if (si.Length == 0) si = ES.Services.Servers.GetServicesByServerIdGroupName(pi.ServerId, ResourceGroups.HostedCRM);

                if (si.Length > 0)
                {
                    int serviceId = si[0].ServiceId;
                    string[] settings = ES.Services.Servers.GetServiceSettings(serviceId);
                    serviceSettings = ConvertArrayToDictionary(settings);
                }
                else
                    serviceSettings = new StringDictionary();

                return serviceSettings;
            }
        }


        private StringArrayResultObject BindCollations()
        {            
            StringArrayResultObject res = ES.Services.CRM.GetCollation(PanelSecurity.PackageId);
            if (res.IsSuccess)
            {
                ddlCollation.DataSource = res.Value;
                ddlCollation.DataBind();
                Utils.SelectListItem(ddlCollation, "Latin1_General_CI_AI"); // default
                Utils.SelectListItem(ddlCollation, ServiceSettings[Constants.Collation]);
            }

            return res;
        }

        private CurrencyArrayResultObject BindCurrency()
        {            
            ddlCurrency.Items.Clear();
            CurrencyArrayResultObject res = ES.Services.CRM.GetCurrency(PanelSecurity.PackageId);
            if (res.IsSuccess)
            {
                foreach (Currency currency in res.Value)
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
                Utils.SelectListItem(ddlCurrency, ServiceSettings[Constants.Currency]);
            }

            return res;
           
        }

        private void BindBaseLanguage()
        {
            ddlBaseLanguage.Items.Clear();
            int[] langPacksId = ES.Services.CRM.GetInstalledLanguagePacks(PanelSecurity.PackageId);
            if (langPacksId != null)
            {
                foreach (int langId in langPacksId)
                {
                    CultureInfo ci = CultureInfo.GetCultureInfo(langId);

                    ListItem item = new ListItem(ci.EnglishName, langId.ToString());
                    ddlBaseLanguage.Items.Add(item);
                }

                Utils.SelectListItem(ddlBaseLanguage, "1033"); // default
                Utils.SelectListItem(ddlBaseLanguage, ServiceSettings[Constants.BaseLanguage]);
            }

        }
        
       
        
        private void ShowCrmOrganizationDetails(string admin, Organization org)
        {
            btnCreate.Visible = false;
            ddlCollation.Enabled = false;
            ddlCurrency.Enabled = false;
            ddlBaseLanguage.Enabled = false;
            administrator.Visible = false;
            lblAdmin.Visible = true;
            lblAdmin.Text = admin;
            btnDelete.Visible = true;
            hlOrganizationPage.Visible = true;
            hlOrganizationPage.NavigateUrl = org.CrmUrl;
            hlOrganizationPage.Text = org.CrmUrl;                                           
        }

        private void ShowOrganizationDetails()
        {
            btnCreate.Visible = true;
            ddlCollation.Enabled = true;
            ddlCurrency.Enabled = true;
            ddlBaseLanguage.Enabled = true;
            administrator.Visible = true;
            lblAdmin.Visible = false;
            
            btnDelete.Visible = false;
            hlOrganizationPage.Visible = false;
        }
        
        
        private void BindOrganizationDetails()
        {
            try
            {
                CurrencyArrayResultObject res = BindCurrency();
                if (!res.IsSuccess)
                {
                    messageBox.ShowMessage(res, "CRM_BIND_ORGANIZATION_DETAILS", "HostedCRM");    
                    return;
                }
                
                
                StringArrayResultObject stringRes = BindCollations();
                if (!stringRes.IsSuccess)
                {
                    messageBox.ShowMessage(res, "CRM_BIND_ORGANIZATION_DETAILS", "HostedCRM");    
                    return;                    
                }

                BindBaseLanguage();

                Organization org = ES.Services.Organizations.GetOrganization(PanelRequest.ItemID);
                lblCrmOrgId.Text = org.OrganizationId;
                lblCrmOrgName.Text = org.Name;
                
                if (!string.IsNullOrEmpty(org.CrmCurrency)) //CRM organization
                {
                    OrganizationUser admin =
                        ES.Services.Organizations.GetUserGeneralSettings(org.Id, org.CrmAdministratorId);
                
                    Utils.SelectListItem(ddlCurrency, org.CrmCurrency);
                    Utils.SelectListItem(ddlCollation, org.CrmCollation);
                    Utils.SelectListItem(ddlBaseLanguage, org.CrmLanguadgeCode);

                    ShowCrmOrganizationDetails(admin.DisplayName, org);
                }
            }
            catch(Exception ex)
            {
                messageBox.ShowErrorMessage("CRM_BIND_ORGANIZATION_DETAILS", ex);
            }
        }        

  
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindOrganizationDetails();
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                Organization org = ES.Services.Organizations.GetOrganization(PanelRequest.ItemID);

                string[] cuurrencyData = ddlCurrency.SelectedValue.Split('|');

                if (string.IsNullOrEmpty(administrator.GetAccount()))
                {
                    messageBox.ShowWarningMessage("CRM_ADMIN_IS_NOT_SPECIFIED");
                    return;
                }

                int baseLanguage = 0;
                int.TryParse(ddlBaseLanguage.SelectedValue, out baseLanguage);

                EnterpriseServer.esCRM CRM = ES.Services.CRM;

                CRM.Timeout = 7200000; //# Set longer timeout

                OrganizationResult res = CRM.CreateOrganization(org.Id, cuurrencyData[0], cuurrencyData[1], cuurrencyData[2], cuurrencyData[3],
                                                   administrator.GetAccountId(), ddlCollation.SelectedValue, baseLanguage);

                messageBox.ShowMessage(res, "CreateCrmOrganization", "HostedCRM");
                if (res.IsSuccess)
                    ShowCrmOrganizationDetails(administrator.GetAccount(), res.Value);
                
                
            }
            catch(Exception ex)
            {
                messageBox.ShowErrorMessage("CRM_CREATE_ORGANIZATION", ex);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Organization org = ES.Services.Organizations.GetOrganization(PanelRequest.ItemID);

                ResultObject res = ES.Services.CRM.DeleteCRMOrganization(org.Id);

                if (res.IsSuccess)
                    ShowOrganizationDetails();
                
                messageBox.ShowMessage(res, "DeleteCrmOrganization", "HostedCRM");
                

            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("CRM_CREATE_ORGANIZATION", ex);
            }
        }                      
    }
}
