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

﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.Virtualization;
using SolidCP.Providers.ResultObjects;

namespace SolidCP.Portal.VPSForPC
{
    public partial class VpsMoveServer : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindHyperVServices();
                BindSourceService();
            }
        }

        public void BindHyperVServices()
        {
            // bind
            HyperVServices.DataSource = ES.Services.Servers.GetRawServicesByGroupName(ResourceGroups.VPS).Tables[0].DefaultView;
            HyperVServices.DataBind();

            // add select value
            HyperVServices.Items.Insert(0, new ListItem(GetLocalizedString("SelectHyperVService.Text"), ""));
        }

        private void BindSourceService()
        {
            VirtualMachine vm = ES.Services.VPS.GetVirtualMachineItem(PanelRequest.ItemID);
            if (vm == null)
                ReturnBack();

            ListItem sourceItem = null;
            foreach (ListItem item in HyperVServices.Items)
            {
                if (item.Value == vm.ServiceId.ToString())
                {
                    sourceItem = item;
                    SourceHyperVService.Text = item.Text;
                    break;
                }
            }

            if (sourceItem != null)
                HyperVServices.Items.Remove(sourceItem);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReturnBack();
        }

        private void ReturnBack()
        {
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), ""));
        }

        protected void btnMove_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            // move item
            int destinationServiceId = Utils.ParseInt(HyperVServices.SelectedValue);
            int result = ES.Services.Packages.MovePackageItem(PanelRequest.ItemID, destinationServiceId);
            if (result < 0)
            {
                ShowResultMessage(result);
                return;
            }

            // redirect to properties screen
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "vps_general",
                "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }
    }
}
