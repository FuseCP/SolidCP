﻿// Copyright (c) 2019, SolidCP
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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.Providers.Common;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.Virtualization;

namespace SolidCP.Portal.VPS2012
{
    public partial class VpsDetailsAddDmzAddress : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindControls();
            }

            ToggleControls();
        }

        private void BindControls()
        {
            // load adapter details
            NetworkAdapterDetails nic = ES.Services.VPS2012.GetDmzNetworkAdapterDetails(PanelRequest.ItemID);

            // load package context
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            if (cntx.Quotas.ContainsKey(Quotas.VPS2012_DMZ_IP_ADDRESSES_NUMBER))
            {
                // set max number
                QuotaValueInfo privQuota = cntx.Quotas[Quotas.VPS2012_DMZ_IP_ADDRESSES_NUMBER];
                int maxDmz = privQuota.QuotaAllocatedValue;
                if (maxDmz == -1)
                    maxDmz = 10;

                maxDmz -= nic.IPAddresses.Length;

                txtDmzAddressesNumber.Text = maxDmz.ToString();
                litMaxDmzAddresses.Text = String.Format(GetLocalizedString("litMaxDmzAddresses.Text"), maxDmz);
                btnAdd.Enabled = btnAddByInject.Enabled = maxDmz > 0;
                txtGateway.Text = nic.DefaultGateway;
                txtDNS1.Text = nic.PreferredNameServer;
                txtDNS2.Text = nic.AlternateNameServer;
                txtMask.Text = nic.SubnetMask;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            AddIP(sender, e, false);
        }

        protected void btnAddByInject_Click(object sender, EventArgs e)
        {
            AddIP(sender, e, true);
        }

        protected void AddIP(object sender, EventArgs e, bool byNewMethod)
        {
            int number = Utils.ParseInt(txtDmzAddressesNumber.Text.Trim(), 0);
            string[] dmzIps = Utils.ParseDelimitedString(txtDmzAddressesList.Text, '\n', '\r', ' ', '\t');

            try
            {
                ResultObject res = null;

                if (byNewMethod)
                    res = ES.Services.VPS2012.AddVirtualMachineDmzIPAddressesByInject(PanelRequest.ItemID,
                    radioDmzRandom.Checked, number, dmzIps, chkCustomGateway.Checked, txtGateway.Text, txtDNS1.Text, txtDNS2.Text, txtMask.Text);
                else
                    res = ES.Services.VPS2012.AddVirtualMachineDmzIPAddresses(PanelRequest.ItemID,
                    radioDmzRandom.Checked, number, dmzIps, chkCustomGateway.Checked, txtGateway.Text, txtDNS1.Text, txtDNS2.Text, txtMask.Text);

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
            DmzAddressesNumberRow.Visible = radioDmzRandom.Checked;
            DmzAddressesListRow.Visible = radioDmzSelected.Checked;
            trCustomGateway.Visible = chkCustomGateway.Checked;
        }
    }
}
