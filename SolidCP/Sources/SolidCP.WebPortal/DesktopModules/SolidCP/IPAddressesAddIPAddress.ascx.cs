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
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.Common;

namespace SolidCP.Portal
{
    public partial class IPAddressesAddIPAddress : SolidCPModuleBase
    {
        private void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                // bind dropdowns
                try
                {
                    BindServers();

                    // set server if found in request
                    if (PanelRequest.ServerId != 0)
                        Utils.SelectListItem(ddlServer, PanelRequest.ServerId);

                    if (!String.IsNullOrEmpty(PanelRequest.PoolId))
                        Utils.SelectListItem(ddlPools, PanelRequest.PoolId);
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("IP_ADD_INIT_FORM", ex);
                    return;
                }

                ToggleControls();
            }
        }

        private void BindServers()
        {
            try
            {
                ddlServer.DataSource = ES.Services.Servers.GetServers();
                ddlServer.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }

            // add "select" item
            ddlServer.Items.Insert(0, new ListItem(GetLocalizedString("Text.NotAssigned"), ""));
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                bool vps = ddlPools.SelectedIndex > 1;
                int vlantag = 0;
                try
                {
                    vlantag = Convert.ToInt32(VLAN.Text);
                }
                catch
                {
                    vlantag = 0;
                }
                if (vps)
                {
                    if (vlantag > 4096 || vlantag < 0)
                    {
                        ShowErrorMessage("Error updating IP address - Invalid VLAN TAG", "VLANTAG");
                        return;
                    }

                }
                int serverId = Utils.ParseInt(ddlServer.SelectedValue, 0);
                IPAddressPool pool = (IPAddressPool)Enum.Parse(typeof(IPAddressPool), ddlPools.SelectedValue, true);
                string comments = txtComments.Text.Trim();

                // add ip address
                if (endIP.Text != "" || startIP.Text.Contains("/"))
                {
                    try
                    {
                        // add IP range
                        ResultObject res = ES.Services.Servers.AddIPAddressesRange(pool, serverId, startIP.Text, endIP.Text,
                            internalIP.Text, subnetMask.Text, defaultGateway.Text, comments, vlantag);
                        if (!res.IsSuccess)
                        {
                            // show error
                            messageBox.ShowMessage(res, "IP_ADD_IP_RANGE", "IP");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage("IP_ADD_IP_RANGE", ex);
                        return;
                    }
                }
                else
                {
                    // add single IP
                    try
                    {
                        IntResult res = ES.Services.Servers.AddIPAddress(pool, serverId, startIP.Text,
                            internalIP.Text, subnetMask.Text, defaultGateway.Text, comments, vlantag);
                        if (!res.IsSuccess)
                        {
                            messageBox.ShowMessage(res, "IP_ADD_IP", "IP");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage("IP_ADD_IP", ex);
                        return;
                    }
                }

                // Redirect back to the portal home page
                RedirectBack();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Redirect back to the portal home page
            RedirectBack();
        }

        private void RedirectBack()
        {
            var returnUrl = Request["ReturnUrl"];

            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = NavigateURL("PoolID", ddlPools.SelectedValue);
            }

            Response.Redirect(returnUrl);
        }

        protected void ddlPools_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        private void ToggleControls()
        {
            bool vps = ddlPools.SelectedIndex > 1;
            SubnetRow.Visible = vps;
            GatewayRow.Visible = vps;
        }

		public void CheckIPAddresses(object sender, ServerValidateEventArgs args) {
			startIP.Validate(sender, args);
			endIP.Validate(sender, args);
			subnetMask.Validate(sender, args);
			args.IsValid = startIP.IsV6 == endIP.IsV6 && (startIP.IsV6 == subnetMask.IsV6 || subnetMask.IsMask);
		}
    }
}
