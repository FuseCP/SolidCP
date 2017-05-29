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
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal.ProviderControls
{
    public partial class MSFTP70_Settings : SolidCPControlBase, IHostingServiceProviderSettings
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindSettings(StringDictionary settings)
        {
			int selectedAddressid = this.FindAddressByText(settings["SharedIP"]);
			ipAddress.AddressId = (selectedAddressid > 0) ? selectedAddressid : 0;
            BindSiteId(settings);
            txtAdFtpRoot.Text = settings["AdFtpRoot"];
            txtFtpGroupName.Text = settings["FtpGroupName"];
			chkBuildUncFilesPath.Checked = Utils.ParseBool(settings["BuildUncFilesPath"], false);
            ActiveDirectoryIntegration.BindSettings(settings);
        }

        public void SaveSettings(StringDictionary settings)
        {
			if (ipAddress.AddressId > 0)
			{
				IPAddressInfo address = ES.Services.Servers.GetIPAddress(ipAddress.AddressId);
				if (String.IsNullOrEmpty(address.InternalIP))
				{
					settings["SharedIP"] = address.ExternalIP;
				}
				else
				{
					settings["SharedIP"] = address.InternalIP;
				}
			}
			else
			{
				settings["SharedIP"] = String.Empty;
			}
        	settings["SiteId"] = ddlSite.SelectedValue;
            if (!string.IsNullOrWhiteSpace(txtAdFtpRoot.Text))
            {
                settings["AdFtpRoot"] = txtAdFtpRoot.Text.Trim();
            }
            settings["FtpGroupName"] = txtFtpGroupName.Text.Trim();
			settings["BuildUncFilesPath"] = chkBuildUncFilesPath.Checked.ToString();
            ActiveDirectoryIntegration.SaveSettings(settings);
        }

		private int FindAddressByText(string address)
		{
		    if (string.IsNullOrEmpty(address))
		    {
		        return 0;
		    }

            foreach (IPAddressInfo addressInfo in ES.Services.Servers.GetIPAddresses(IPAddressPool.General, PanelRequest.ServerId))
			{
				if (addressInfo.InternalIP == address || addressInfo.ExternalIP == address)
				{
					return addressInfo.AddressId;
				}
			}
			return 0;
		}

        private void BindSiteId(StringDictionary settings)
        {
            var sites = ES.Services.FtpServers.GetFtpSites(PanelRequest.ServiceId);

            foreach (var site in sites)
            {
                var item = new ListItem(site.Name + " (User Isolation Mode: " + site["UserIsolationMode"] + ")", site.Name);

                if (item.Value == settings["SiteId"])
                {
                    item.Selected = true;
                }

                ddlSite.Items.Add(item);
            }

            if (ddlSite.Items.Count == 0)
            {
                ddlSite.Items.Add(new ListItem("Default FTP Site (not yet created)", "Default FTP Site"));
            }
            else
            {
                ddlSite_SelectedIndexChanged(this, null);
            }
        }

        protected void ddlSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            var isActiveDirectoryUserIsolated = ddlSite.SelectedItem.Text.Contains("ActiveDirectory");
            FtpRootRow.Visible = isActiveDirectoryUserIsolated;
            txtAdFtpRootReqValidator.Enabled= isActiveDirectoryUserIsolated;
        }
    }
}
