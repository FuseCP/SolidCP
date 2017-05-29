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
    public partial class WebSitesAddSite : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // set controls
                domainsSelectDomainControl.PackageId = PanelSecurity.PackageId;

                // bind IP Addresses
                BindIPAddresses();

                BindIgnoreZoneTemplate();

                // toggle
                ToggleControls();
            }
        }

        private void ToggleControls()
        {
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            rowDedicatedIP.Visible = rbDedicatedIP.Checked;
            
            if (Utils.CheckQouta(Quotas.WEB_ENABLEHOSTNAMESUPPORT, cntx))
            {
                txtHostName.Visible = chkIgnoreGlobalDNSRecords.Visible = lblIgnoreGlobalDNSRecords.Visible = lblTheDotInTheMiddle.Visible = true;
                UserSettings settings = ES.Services.Users.GetUserSettings(PanelSecurity.LoggedUserId, UserSettings.WEB_POLICY);
                txtHostName.Text = String.IsNullOrEmpty(settings["HostName"]) ? "" : settings["HostName"];
                chkIgnoreGlobalDNSRecords.Checked = false;
            }
            else
            {
                txtHostName.Visible = chkIgnoreGlobalDNSRecords.Visible = lblIgnoreGlobalDNSRecords.Visible = lblTheDotInTheMiddle.Visible = false;
                chkIgnoreGlobalDNSRecords.Checked = true;
                txtHostName.Text = "";
                domainsSelectDomainControl.HideWebSites = true;
            }

        }

        private void BindIgnoreZoneTemplate()
        {
            chkIgnoreGlobalDNSRecords.Checked = false;
        }

        private void BindIPAddresses()
        {
            ddlIpAddresses.Items.Add(new ListItem("<Select IP>", ""));

            PackageIPAddress[] ips = ES.Services.Servers.GetPackageUnassignedIPAddresses(PanelSecurity.PackageId, 0, IPAddressPool.WebSites);
            foreach (PackageIPAddress ip in ips)
            {
                string fullIP = ip.ExternalIP;
                if (ip.InternalIP != null &&
                    ip.InternalIP != "" &&
                    ip.InternalIP != ip.ExternalIP)
                    fullIP += " (" + ip.InternalIP + ")";

                ddlIpAddresses.Items.Add(new ListItem(fullIP, ip.PackageAddressID.ToString()));
            }

            rowSiteIP.Visible = (ddlIpAddresses.Items.Count > 1);
            rbDedicatedIP.Enabled = (ddlIpAddresses.Items.Count > 1);
        }

        private void AddWebSite()
        {
            int siteItemId = 0;

            try
            {
                int packageAddressId = rbDedicatedIP.Checked ? Utils.ParseInt(ddlIpAddresses.SelectedValue, 0) : 0;

                siteItemId = ES.Services.WebServers.AddWebSite(PanelSecurity.PackageId, txtHostName.Text.ToLower(), domainsSelectDomainControl.DomainId,
                    packageAddressId, !chkIgnoreGlobalDNSRecords.Checked);

                if (siteItemId < 0)
                {
                    ShowResultMessage(siteItemId);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_ADD_SITE", ex);
                return;
            }

            // go to edit site
            Response.Redirect(EditUrl("ItemID", siteItemId.ToString(), "edit_item",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectSpaceHomePage();
        }
        protected void rbIP_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            AddWebSite();
        }
    }
}
