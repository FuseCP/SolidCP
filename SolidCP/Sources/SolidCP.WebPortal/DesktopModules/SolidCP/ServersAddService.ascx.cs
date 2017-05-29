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
using SolidCP.Providers.Common;

namespace SolidCP.Portal
{
    public partial class ServersAddService : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGroup();
                BindProviders();
            }
        }

        private void BindGroup()
        {
            ResourceGroupInfo group = ES.Services.Servers.GetResourceGroup(PanelRequest.GroupID);
			litGroupName.Text = serviceName.Text = PanelFormatter.GetLocalizedResourceGroupName(group.GroupName);
        }

        private void BindProviders()
        {
            ddlProviders.DataSource = ES.Services.Servers.GetProvidersByGroupId(PanelRequest.GroupID);
            ddlProviders.DataBind();
            ddlProviders.Items.Insert(0, new ListItem(GetLocalizedString("SelectProvider.Text"), ""));
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            // validate input
            if (!Page.IsValid)
                return;

            // register service type
            int providerId = Utils.ParseInt(ddlProviders.SelectedValue, 0);

            // add a new service ...
            try
            {
                ServiceInfo service = new ServiceInfo();
                service.ServerId = PanelRequest.ServerId;
                service.ProviderId = providerId;
                service.ServiceName = serviceName.Text;
                BoolResult res = ES.Services.Servers.IsInstalled(PanelRequest.ServerId, providerId);
                if (res.IsSuccess)
                {
                    if (!res.Value)
                    {
                        ShowErrorMessage("SOFTWARE_IS_NOT_INSTALLED");
                        return;
                    }
                }
                else
                {
                    ShowErrorMessage("SERVER_ADD_SERVICE");
                }
                int serviceId = ES.Services.Servers.AddService(service);

                if (serviceId < 0)
                {
                    ShowResultMessage(serviceId);
                    return;
                }

                // ...and go to service configuration page
                Response.Redirect(EditUrl("ServerID", PanelRequest.ServerId.ToString(), "edit_service",
                    "ServiceID=" + serviceId.ToString()), true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SERVER_ADD_SERVICE", ex);
                return;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ServerID", PanelRequest.ServerId.ToString(), "edit_server"), true);
        }
    }
}
