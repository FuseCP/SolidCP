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
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SolidCP.EnterpriseServer;
using SolidCP.Providers.Common;

namespace SolidCP.Portal
{
    public partial class PhoneNumbersEditPhoneNumber : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    // bind dropdowns
                    BindServers();

                    // bind IP
                    BindPhone();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("IP_GET_IP", ex);
                    return;
                }
            }
        }

        private void BindPhone()
        {
            int addressId = PanelRequest.AddressID;

            // check if multiple editing
            if (!String.IsNullOrEmpty(PanelRequest.Addresses))
            {
                string[] ids = PanelRequest.Addresses.Split(',');
                addressId = Utils.ParseInt(ids[0], 0);
            }

            // bind first address
            IPAddressInfo addr = ES.Services.Servers.GetIPAddress(addressId);

            if (addr != null)
            {
                Utils.SelectListItem(ddlServer, addr.ServerId);

                Phone.Text = addr.ExternalIP;
                txtComments.Text = addr.Comments;

                ToggleControls();
            }
            else
            {
                // exit
                RedirectBack();
            }
        }

        private void BindServers()
        {
            ServerInfo[] allServers = ES.Services.Servers.GetServers();
            List<ServerInfo> servers = new List<ServerInfo>();
            foreach (ServerInfo server in allServers)
            {
                ServiceInfo[] service = ES.Services.Servers.GetServicesByServerIdGroupName(server.ServerId, ResourceGroups.Lync);
                if (service.Length > 0) servers.Add(server);
            }

            ddlServer.DataSource = servers;

            ddlServer.DataBind();
            ddlServer.Items.Insert(0, new ListItem(GetLocalizedString("Text.NotAssigned"), ""));
        }

        private void RedirectBack()
        {
            Response.Redirect(NavigateURL("PoolID", "PhoneNumbers"));
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    int serverId = Utils.ParseInt(ddlServer.SelectedValue, 0);
                    IPAddressPool pool = IPAddressPool.PhoneNumbers;

                    ResultObject res = null;

                    // update single IP address
                    if (!String.IsNullOrEmpty(PanelRequest.Addresses))
                    {
                        // update multiple IPs
                        string[] ids = PanelRequest.Addresses.Split(',');
                        int[] addresses = new int[ids.Length];
                        for (int i = 0; i < ids.Length; i++)
                            addresses[i] = Utils.ParseInt(ids[i], 0);

                        res = ES.Services.Servers.UpdateIPAddresses(addresses,
                            pool, serverId, "", "", txtComments.Text.Trim(), 0);
                    }
                    else
                    {
                        string address = Phone.Text;

                        // update single IP
                        res = ES.Services.Servers.UpdateIPAddress(PanelRequest.AddressID,
                            pool, serverId, address, "", "", "", txtComments.Text.Trim(), 0);
                    }

                    if (!res.IsSuccess)
                    {
                        messageBox.ShowMessage(res, "IP_UPDATE_IP", "IP");
                        return;
                    }

                    //	 Redirect back to the portal home page
                    RedirectBack();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("IP_UPDATE_IP", ex);
                    return;
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Redirect back to the portal home page
            RedirectBack();
        }

        protected void ddlPools_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        private void ToggleControls()
        {
            bool multipleEdit = !String.IsNullOrEmpty(PanelRequest.Addresses);
            PhoneNumbersRow.Visible = !multipleEdit;
            requireStartPhoneValidator.Enabled = !multipleEdit;
        }
    }
}
