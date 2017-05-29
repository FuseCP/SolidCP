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
using SolidCP.Providers.Common;
using SolidCP.Providers.Virtualization;
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal.VPSForPC
{
    public partial class VpsDetailsAddExternalAddress : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ToggleControls();

            if (!IsPostBack)
            {
                BindExternalIPAddresses();
            }
        }

        private void BindExternalIPAddresses()
        {
            PackageIPAddress[] ips = ES.Services.Servers.GetPackageUnassignedIPAddresses(PanelSecurity.PackageId, 0, IPAddressPool.VpsExternalNetwork);
            foreach (PackageIPAddress ip in ips)
            {
                string txt = ip.ExternalIP;
                if (!String.IsNullOrEmpty(ip.DefaultGateway))
                    txt += "/" + ip.DefaultGateway;
                listExternalAddresses.Items.Add(new ListItem(txt, ip.PackageAddressID.ToString()));
            }

            // toggle controls
            int maxAddresses = listExternalAddresses.Items.Count;
            litMaxExternalAddresses.Text = String.Format(GetLocalizedString("litMaxExternalAddresses.Text"), maxAddresses);

            bool empty = maxAddresses == 0;
            EmptyExternalAddressesMessage.Visible = empty;
            ExternalAddressesTable.Visible = !empty;
            btnAdd.Enabled = !empty;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectBack();
        }

        private void RedirectBack()
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "vps_network",
                "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }

        private void ToggleControls()
        {
            // external network
            ExternalAddressesNumberRow.Visible = radioExternalRandom.Checked;
            ExternalAddressesListRow.Visible = radioExternalSelected.Checked;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            int number = Utils.ParseInt(txtExternalAddressesNumber.Text.Trim(), 0);
            List<int> addressIds = new List<int>();
            foreach (ListItem li in listExternalAddresses.Items)
                if (li.Selected)
                    addressIds.Add(Utils.ParseInt(li.Value, 0));

            try
            {
                ResultObject res = ES.Services.VPS.AddVirtualMachineExternalIPAddresses(PanelRequest.ItemID,
                    radioExternalRandom.Checked, number, addressIds.ToArray());

                if (res.IsSuccess)
                {
                    RedirectBack();
                }
                else
                {
                    messageBox.ShowMessage(res, "VPS_ERROR_ADDING_IP_ADDRESS", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_ADDING_IP_ADDRESS", ex);
            }
        }
    }
}
