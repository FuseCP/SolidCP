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
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.Common;

namespace SolidCP.Portal
{
    public partial class PhoneNumbersAddPhoneNumber : SolidCPModuleBase
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
                ServerInfo[] allServers = ES.Services.Servers.GetServers();
                List<ServerInfo> servers = new List<ServerInfo>();
                foreach(ServerInfo server in allServers)
                    {
                        ServiceInfo[] service = ES.Services.Servers.GetServicesByServerIdGroupName(server.ServerId, ResourceGroups.Lync);

                        if (service.Length > 0) servers.Add(server);
                    }
                foreach (ServerInfo server in allServers)
                {
                    ServiceInfo[] service = ES.Services.Servers.GetServicesByServerIdGroupName(server.ServerId, ResourceGroups.SfB);

                    if (service.Length > 0) servers.Add(server);
                }

                ddlServer.DataSource = servers;
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
                int serverId = Utils.ParseInt(ddlServer.SelectedValue, 0);
                IPAddressPool pool = IPAddressPool.PhoneNumbers;
                string comments = txtComments.Text.Trim();

                string start; 
                string end;

                    start = startPhone.Text; 
                    end = endPhone.Text;

                // add ip address
                if (end != "" || start.Contains("/"))
                {
                    string errorKey = "IP_ADD_PHONE_RANGE";

                    try
                    {
                        // add IP range
                        ResultObject res = ES.Services.Servers.AddIPAddressesRange(pool, serverId, start, end,
                            "", "", "", comments, 0);
                        if (!res.IsSuccess)
                        {
                            // show error
                            messageBox.ShowMessage(res, errorKey, "IP");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage(errorKey, ex);
                        return;
                    }
                }
                else
                {
                    string errorKey = "IP_ADD_PHONE";

                    // add single IP
                    try
                    {
                        IntResult res = ES.Services.Servers.AddIPAddress(pool, serverId, start,
                            "", "", "", comments, 0);
                        if (!res.IsSuccess)
                        {
                            messageBox.ShowMessage(res, errorKey, "IP");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage(errorKey, ex);
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
            Response.Redirect(NavigateURL("PoolID", "PhoneNumbers"));
        }

        protected void ddlPools_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        private void ToggleControls()
        {
            requireStartPhoneValidator.Enabled = true;
        }

    }
}
