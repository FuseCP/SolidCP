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
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.Common;

namespace SolidCP.Portal
{
    public partial class VLANsAddVLANs : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
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
                    ShowErrorMessage("VLAN_ADD_INIT_FORM", ex);
                    return;
                }
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
                int serverId = Utils.ParseInt(ddlServer.SelectedValue, 0);
                string comments = txtComments.Text.Trim();

                // add vlan
                if (endVLAN.Text != "")
                {
                    try
                    {
                        // add vlan range
                        ResultObject res = ES.Services.Servers.AddPrivateNetworkVLANsRange(serverId, Int32.Parse(startVLAN.Text), Int32.Parse(endVLAN.Text), comments);
                        if (!res.IsSuccess)
                        {
                            // show error
                            messageBox.ShowMessage(res, "VLAN_ADD_VLAN_RANGE", "VLAN");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage("VLAN_ADD_VLAN_RANGE", ex);
                        return;
                    }
                }
                else
                {
                    // add single vlan
                    try
                    {
                        IntResult res = ES.Services.Servers.AddPrivateNetworkVLAN(serverId, Int32.Parse(startVLAN.Text), comments);
                        if (!res.IsSuccess)
                        {
                            messageBox.ShowMessage(res, "VLAN_ADD_VLAN", "VLAN");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage("VLAN_ADD_VLAN", ex);
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
                returnUrl = NavigateURL("ServerID", ddlServer.SelectedValue);
            }

            Response.Redirect(returnUrl);
        }

        public void CheckVLANs(object sender, ServerValidateEventArgs args)
        {
            startVLAN.Validate(sender, args);
            endVLAN.Validate(sender, args);
        }
    }
}