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
using SolidCP.Providers.Virtualization;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.Common;

namespace SolidCP.Portal.VPSForPC
{
    public partial class VpsDetailsNetwork : SolidCPModuleBase
    {
        VirtualMachine vm = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindVirtualMachine();
                BindExternalAddresses();
                BindPrivateAddresses();
                ToggleButtons();
            }
        }

        private void BindVirtualMachine()
        {
            vm = ES.Services.VPS.GetVirtualMachineItem(PanelRequest.ItemID);

            // external network
            if (!vm.ExternalNetworkEnabled)
            {
                secExternalNetwork.Visible = false;
                ExternalNetworkPanel.Visible = false;
            }

            // private network
            if (!vm.PrivateNetworkEnabled)
            {
                secPrivateNetwork.Visible = false;
                PrivateNetworkPanel.Visible = false;
            }
        }

        private void BindExternalAddresses()
        {
            // load details
            NetworkAdapterDetails nic = ES.Services.VPS.GetExternalNetworkAdapterDetails(PanelRequest.ItemID);

            // bind details
            foreach (NetworkAdapterIPAddress ip in nic.IPAddresses)
            {
                if (ip.IsPrimary)
                {
                    litExtAddress.Text = ip.IPAddress;
                    litExtSubnet.Text = ip.SubnetMask;
                    litExtGateway.Text = ip.DefaultGateway;
                    break;
                }
            }
            lblTotalExternal.Text = nic.IPAddresses.Length.ToString();

            // bind IP addresses
            gvExternalAddresses.DataSource = nic.IPAddresses;
            gvExternalAddresses.DataBind();
        }

        private void BindPrivateAddresses()
        {
            // load details
            NetworkAdapterDetails nic = ES.Services.VPS.GetPrivateNetworkAdapterDetails(PanelRequest.ItemID);

            // bind details
            foreach (NetworkAdapterIPAddress ip in nic.IPAddresses)
            {
                if (ip.IsPrimary)
                {
                    litPrivAddress.Text = ip.IPAddress;
                    break;
                }
            }
            litPrivSubnet.Text = nic.SubnetMask;
            litPrivFormat.Text = nic.NetworkFormat;
            lblTotalPrivate.Text = nic.IPAddresses.Length.ToString();

            // bind IP addresses
            gvPrivateAddresses.DataSource = nic.IPAddresses;
            gvPrivateAddresses.DataBind();

            if (nic.IsDHCP)
            {
                PrivateAddressesPanel.Visible = false;
                litPrivAddress.Text = GetLocalizedString("Automatic.Text");
            }
        }

        private void ToggleButtons()
        {
            bool manageAllowed = VirtualMachinesForPCHelper.IsVirtualMachineManagementAllowed(PanelSecurity.PackageId);

            btnAddExternalAddress.Visible = manageAllowed;
            btnSetPrimaryExternal.Visible = manageAllowed;
            btnDeleteExternal.Visible = manageAllowed;
            gvExternalAddresses.Columns[0].Visible = manageAllowed;

            btnAddPrivateAddress.Visible = manageAllowed;
            btnSetPrimaryPrivate.Visible = manageAllowed;
            btnDeletePrivate.Visible = manageAllowed;
            gvPrivateAddresses.Columns[0].Visible = manageAllowed;
        }

        protected void btnAddExternalAddress_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "vps_add_external_ip",
                "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }

        protected void btnAddPrivateAddress_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "vps_add_private_ip",
                "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }

        protected void btnSetPrimaryPrivate_Click(object sender, EventArgs e)
        {
            int[] addressIds = GetSelectedItems(gvPrivateAddresses);
            
            // check if at least one is selected
            if (addressIds.Length == 0)
            {
                messageBox.ShowWarningMessage("IP_ADDRESS_NOT_SELECTED");
                return;
            }

            try
            {
                ResultObject res = ES.Services.VPS.SetVirtualMachinePrimaryPrivateIPAddress(PanelRequest.ItemID, addressIds[0]);

                if (res.IsSuccess)
                {
                    BindPrivateAddresses();
                    return;
                }
                else
                {
                    messageBox.ShowMessage(res, "VPS_ERROR_SETTING_PRIMARY_IP", "VPS");
                    return;
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_SETTING_PRIMARY_IP", ex);
            }
        }

        protected void btnDeletePrivate_Click(object sender, EventArgs e)
        {
            int[] addressIds = GetSelectedItems(gvPrivateAddresses);

            // check if at least one is selected
            if (addressIds.Length == 0)
            {
                messageBox.ShowWarningMessage("IP_ADDRESS_NOT_SELECTED");
                return;
            }

            try
            {
                ResultObject res = ES.Services.VPS.DeleteVirtualMachinePrivateIPAddresses(PanelRequest.ItemID, addressIds);

                if (res.IsSuccess)
                {
                    BindPrivateAddresses();
                    return;
                }
                else
                {
                    messageBox.ShowMessage(res, "VPS_ERROR_DELETING_IP_ADDRESS", "VPS");
                    return;
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_DELETING_IP_ADDRESS", ex);
            }
        }

        protected void btnSetPrimaryExternal_Click(object sender, EventArgs e)
        {
            int[] addressIds = GetSelectedItems(gvExternalAddresses);

            // check if at least one is selected
            if (addressIds.Length == 0)
            {
                messageBox.ShowWarningMessage("IP_ADDRESS_NOT_SELECTED");
                return;
            }

            try
            {
                ResultObject res = ES.Services.VPS.SetVirtualMachinePrimaryExternalIPAddress(PanelRequest.ItemID, addressIds[0]);

                if (res.IsSuccess)
                {
                    BindExternalAddresses();
                    return;
                }
                else
                {
                    messageBox.ShowMessage(res, "VPS_ERROR_SETTING_PRIMARY_IP", "VPS");
                    return;
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_SETTING_PRIMARY_IP", ex);
            }
        }

        protected void btnDeleteExternal_Click(object sender, EventArgs e)
        {
            int[] addressIds = GetSelectedItems(gvExternalAddresses);

            // check if at least one is selected
            if (addressIds.Length == 0)
            {
                messageBox.ShowWarningMessage("IP_ADDRESS_NOT_SELECTED");
                return;
            }

            try
            {
                ResultObject res = ES.Services.VPS.DeleteVirtualMachineExternalIPAddresses(PanelRequest.ItemID, addressIds);

                if (res.IsSuccess)
                {
                    BindExternalAddresses();
                    return;
                }
                else
                {
                    messageBox.ShowMessage(res, "VPS_ERROR_DELETING_IP_ADDRESS", "VPS");
                    return;
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_DELETING_IP_ADDRESS", ex);
            }
        }

        private int[] GetSelectedItems(GridView gv)
        {
            List<int> items = new List<int>();

            for (int i = 0; i < gv.Rows.Count; i++)
            {
                GridViewRow row = gv.Rows[i];
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect.Checked)
                    items.Add((int)gv.DataKeys[i].Value);
            }

            return items.ToArray();
        }
    }
}
