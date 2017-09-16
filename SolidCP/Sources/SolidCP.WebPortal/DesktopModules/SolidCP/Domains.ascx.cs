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
using System.Linq;

using SolidCP.EnterpriseServer;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.Portal
{
    public partial class Domains : SolidCPModuleBase
    {
        public Dictionary<int, string> dnsRecords;

        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterClientScriptInclude("jquery", ResolveUrl("~/JavaScript/jquery-1.4.4.min.js"));

            dnsRecords = new Dictionary<int, string>();

            gvDomains.PageSize = UsersHelper.GetDisplayItemsPerPage();

            // SE Check
            bool SEEnable = false;

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["SpamExpertsEnable"]))
            {
                SEEnable = true;
            }

            // visibility
            chkRecursive.Visible = (PanelSecurity.SelectedUser.Role != UserRole.User);
            gvDomains.Columns[5].Visible = gvDomains.Columns[6].Visible = (PanelSecurity.SelectedUser.Role != UserRole.User) && chkRecursive.Checked;
			gvDomains.Columns[7].Visible = (PanelSecurity.SelectedUser.Role == UserRole.Administrator);
            gvDomains.Columns[8].Visible = SEEnable;
            gvDomains.Columns[9].Visible = (PanelSecurity.EffectiveUser.Role == UserRole.Administrator);

            if (!IsPostBack)
            {
                // toggle controls
                btnAddDomain.Enabled = PackagesHelper.CheckGroupQuotaEnabled(PanelSecurity.PackageId, ResourceGroups.Os, Quotas.OS_DOMAINS)
                    || PackagesHelper.CheckGroupQuotaEnabled(PanelSecurity.PackageId, ResourceGroups.Os, Quotas.OS_SUBDOMAINS)
                    || PackagesHelper.CheckGroupQuotaEnabled(PanelSecurity.PackageId, ResourceGroups.Os, Quotas.OS_DOMAINPOINTERS);

                searchBox.AddCriteria("DomainName", GetLocalizedString("SearchField.DomainName"));
                if ((PanelSecurity.SelectedUser.Role != UserRole.User) && chkRecursive.Checked)
                {
                    searchBox.AddCriteria("Username", GetLocalizedString("SearchField.Username"));
                    searchBox.AddCriteria("FullName", GetLocalizedString("SearchField.FullName"));
                    searchBox.AddCriteria("Email", GetLocalizedString("SearchField.Email"));
                }
            }
            searchBox.AjaxData = GetSearchBoxAjaxData();
        }

        public string GetItemEditUrl(object packageId, object itemId)
        {
            return EditUrl("DomainID", itemId.ToString(), "edit_item",
                PortalUtils.SPACE_ID_PARAM + "=" + packageId.ToString());
        }

        public string GetUserHomePageUrl(int userId)
        {
            return PortalUtils.GetUserHomePageUrl(userId);
        }

        public string GetSpaceHomePageUrl(int spaceId)
        {
            return NavigateURL(PortalUtils.SPACE_ID_PARAM, spaceId.ToString());
        }

        public string GetItemsPageUrl(string parameterName, string parameterValue)
        {
            return NavigateURL(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(),
                parameterName + "=" + parameterValue);
        }

        public string GetDomainTypeName(bool isSubDomain, bool isPreviewDomain, bool isDomainPointer)
        {
            if(isDomainPointer)
                return GetLocalizedString("DomainType.DomainPointer");
            else if (isSubDomain)
                return GetLocalizedString("DomainType.SubDomain");
            else
                return GetLocalizedString("DomainType.Domain");
        }

        public string GetDomainExpirationDate(object expirationDateObject, object LastUpdateDateObject)
        {
            var expirationDate = expirationDateObject as DateTime?;
            var lastUpdateDate = LastUpdateDateObject as DateTime?;
           
            if (expirationDate != null && expirationDate < DateTime.Now)
            {
                return GetLocalizedString("DomainExpirationDate.Expired");
            }
            else if(expirationDate != null)
            {
                return expirationDate.Value.ToShortDateString();
            }
            else if (lastUpdateDate == null)
            {
                return GetLocalizedString("DomainExpirationDate.NotChecked");
            }
            else
            {
                return GetLocalizedString("DomainExpirationDate.NotExist");
            }
        }

        public bool ShowDomainDnsInfo(object expirationDateObject, object LastUpdateDateObject, bool isTopLevelDomain)
        {
            var expirationDate = expirationDateObject as DateTime?;
            var lastUpdateDate = LastUpdateDateObject as DateTime?;

            if (!isTopLevelDomain)
            {
                return false;
            }
            else if (expirationDate != null && expirationDate < DateTime.Now)
            {
                return false;
            }
            else if(expirationDate != null)
            {
                return true;
            }
            else if (lastUpdateDate == null)
            {
                return false;
            }
            else
            {
                return false;
            }
        }

        public string GetDomainDnsRecords(int domainId)
        {
            if(dnsRecords.ContainsKey(domainId))
            {
                return dnsRecords[domainId];
            }

            var records = ES.Services.Servers.GetDomainDnsRecords(domainId);

            if (!records.Any())
            {
                dnsRecords.Add(domainId, string.Empty);

                return string.Empty;
            }

            var header = GetLocalizedString("DomainLookup.TooltipHeader");

            var tooltipLines = new List<string>();

            tooltipLines.Add(header);
            tooltipLines.Add(" ");
            tooltipLines.AddRange( records.Select(x=>string.Format("{0}: {1}", x.RecordType, x.Value)));

            dnsRecords.Add(domainId, string.Join("\r\n", tooltipLines));

            return dnsRecords[domainId];
        }

        public string GetDomainTooltip(int domainId, string registrar)
        {
            var dnsString = GetDomainDnsRecords(domainId);

            var tooltipLines = new List<string>();

            if (!string.IsNullOrEmpty(registrar))
            {
                var header = GetLocalizedString("DomainLookup.TooltipHeader.Registrar");
                tooltipLines.Add(header + " " + registrar);
                tooltipLines.Add("\r\n");
            }

            return string.Join("\r\n", tooltipLines) + dnsString;
        }

        protected void odsDomainsPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ProcessException(e.Exception);
                //this.DisableControls = true;
                e.ExceptionHandled = true;
            }
        }

        protected void btnAddDomain_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(), "add_domain"));
        }

		protected void gvDomains_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (e.CommandName == "Detach")
			{
				
                // remove item from meta base
				int domainId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

				int result = ES.Services.Servers.DetachDomain(domainId);
				if (result < 0)
				{
					 
                    ShowResultMessage(result);
				//	return;
				}

				// refresh the list
				gvDomains.DataBind();
			}
		}

        public string GetSearchBoxAjaxData()
        {
            StringBuilder res = new StringBuilder();
            res.Append("PagedStored: 'Domains'");
            res.Append(", RedirectUrl: '" + GetItemEditUrl(Request["SpaceID"] ?? "-1", "{0}").Substring(2) + "'");
            res.Append(", PackageID: " + (String.IsNullOrEmpty(Request["SpaceID"]) ? "-1" : Request["SpaceID"]));
            res.Append(", ServerID: " + (String.IsNullOrEmpty(Request["ServerID"]) ? "0" : Request["ServerID"]));
            res.Append(", Recursive: ($('#" + chkRecursive.ClientID + "').val() == 'on')");
            return res.ToString();
        }

        // RB ADDED for SpamExperts
        #region
        protected void btnSE_Click(object sender, EventArgs e)
        {
            string SEUrl = ConfigurationManager.AppSettings["SpamExpertsUrl"];
            string SEUser = ConfigurationManager.AppSettings["SpamExpertsUser"];
            string SEPassword = ConfigurationManager.AppSettings["SpamExpertsPassword"];

            Button btn = sender as Button;
            string domain = btn.CommandArgument;


            string result = string.Empty;
            try
            {
                System.Net.WebClient Client = new System.Net.WebClient();
                Client.Credentials = new System.Net.NetworkCredential(SEUser, SEPassword);
                string uri = SEUrl + "api/authticket/create/username/" + domain;
                using (System.IO.Stream strm = Client.OpenRead(uri))
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(strm);
                    result = sr.ReadToEnd();
                }
            }
            catch
            {
                result = string.Empty;
            }

            if (!string.IsNullOrEmpty(result))
            {
                string script = "window.open('" + SEUrl + "?authticket=" + result + "', 'SpamExperts');";
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "SpamExperts", script, true);
            }
        }
        #endregion

    }
}
