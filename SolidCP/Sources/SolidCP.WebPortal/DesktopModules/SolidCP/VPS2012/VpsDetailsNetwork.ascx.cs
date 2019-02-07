// Copyright (c) 2019, SolidCP
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
using System.Text;
using System.Data;

namespace SolidCP.Portal.VPS2012
{
    public partial class VpsDetailsNetwork : SolidCPModuleBase
    {
        VirtualMachine vm = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRealAssignedAddresses();
                BindVirtualMachine();
                BindExternalAddresses();
                BindPrivateAddresses();
                ToggleButtons();
            }
        }

        private void BindVirtualMachine()
        {
            vm = ES.Services.VPS2012.GetVirtualMachineItem(PanelRequest.ItemID);

            // external network
            if (!vm.ExternalNetworkEnabled)
            {
                secExternalNetwork.Visible = false;
                ExternalNetworkPanel.Visible = false;
                btnRestoreExternalAddress.Visible = false;
            }

            // private network
            if (!vm.PrivateNetworkEnabled)
            {
                secPrivateNetwork.Visible = false;
                PrivateNetworkPanel.Visible = false;
                btnRestorePrivateAddress.Visible = false;
            }
        }

        private void BindRealAssignedAddresses()
        {
            VirtualMachine itemVM = VirtualMachines2012Helper.GetCachedVirtualMachine(PanelRequest.ItemID);
            VirtualMachine _vm = null;
            try
            {
                _vm = ES.Services.VPS2012.GetVirtualMachineExtendedInfo(itemVM.ServiceId, itemVM.VirtualMachineId);
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_GET_VM_DETAILS", ex);
            }

            try
            {
                repVMNetwork.DataSource = _vm.Adapters;//new VirtualMachineNetworkAdapter[vm.Adapters.Length];
                repVMNetwork.DataBind();
                BindGridViewOfVmIPs(_vm.Adapters);
                CheckIfPossibleToDoIpInjection(_vm.Adapters);
            }
            catch (Exception ex) //TODO: replace by messageBox ????
            {
                VMNetworkError.Text = "Error - " + ex;
                VMNetworkError.Visible = true;
            }                
        }

        private void CheckIfPossibleToDoIpInjection(VirtualMachineNetworkAdapter[] Adapters)
        {
            btnDeletePrivateByInject.Visible = 
                btnDeleteExternalByInject.Visible = 
                btnRestoreExternalAddress.Visible = 
                btnRestorePrivateAddress.Visible = false;
            foreach (VirtualMachineNetworkAdapter adapter in Adapters)
            {
                if (adapter.IPAddresses != null && adapter.IPAddresses.Length > 0) //if we can get IP information at least from 1 adapter it means that VM support IP Injection.
                {
                    btnDeleteExternalByInject.Visible = btnRestoreExternalAddress.Visible = btnRestorePrivateAddress.Visible = true;
                    break;
                }
            }
        }

        private void BindGridViewOfVmIPs(VirtualMachineNetworkAdapter[] Adapters)
        {
            int i = 0;
            foreach (RepeaterItem item in repVMNetwork.Items)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("N", typeof(int));
                dt.Columns.Add("IP", typeof(string));
                for (int j = 0; j < Adapters[i].IPAddresses.Length; j++)
                {
                    DataRow NewRow = dt.NewRow();
                    NewRow["N"] = j + 1;
                    NewRow["IP"] = Adapters[i].IPAddresses[j];
                    dt.Rows.Add(NewRow);
                }
                (item.FindControl("gvVMNetwork") as GridView).DataSource = dt;
                (item.FindControl("gvVMNetwork") as GridView).DataBind();
                i++;
            }
        }

        private void BindExternalAddresses()
        {
            // load details
            NetworkAdapterDetails nic = ES.Services.VPS2012.GetExternalNetworkAdapterDetails(PanelRequest.ItemID);

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
            NetworkAdapterDetails nic = ES.Services.VPS2012.GetPrivateNetworkAdapterDetails(PanelRequest.ItemID);

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
            bool manageAllowed = VirtualMachines2012Helper.IsVirtualMachineManagementAllowed(PanelSecurity.PackageId);

            btnAddExternalAddress.Visible = manageAllowed;
            btnSetPrimaryExternal.Visible = manageAllowed;
            btnDeleteExternal.Visible = manageAllowed;
            gvExternalAddresses.Columns[0].Visible = manageAllowed;

            btnAddPrivateAddress.Visible = manageAllowed;
            btnSetPrimaryPrivate.Visible = manageAllowed;
            btnDeletePrivate.Visible = manageAllowed;
            gvPrivateAddresses.Columns[0].Visible = manageAllowed;
        }

        protected void btnRestoreExternalAddress_Click(object sender, EventArgs e)
        {
            try
            {
                ResultObject res = ES.Services.VPS2012.RestoreVirtualMachineExternalIPAddressesByInjection(PanelRequest.ItemID);
                if (res.IsSuccess)
                {
                    BindRealAssignedAddresses();
                    BindVirtualMachine();
                    return;
                }
                else
                {
                    messageBox.ShowMessage(res, "VPS_ERROR_RESTORE_EXTERNAL_IP", "VPS");
                    return;
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_RESTORE_EXTERNAL_IP", ex);
            }
        }

        protected void btnRestorePrivateByInject_Click(object sender, EventArgs e)
        {
            try
            {
                ResultObject res = ES.Services.VPS2012.RestoreVirtualMachinePrivateIPAddressesByInjection(PanelRequest.ItemID);
                if (res.IsSuccess)
                {
                    BindRealAssignedAddresses();
                    BindVirtualMachine();
                    return;
                }
                else
                {
                    messageBox.ShowMessage(res, "VPS_ERROR_RESTORE_PRIVATE_IP", "VPS");
                    return;
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_RESTORE_PRIVATE_IP", ex);
            }
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
                ResultObject res = ES.Services.VPS2012.SetVirtualMachinePrimaryPrivateIPAddress(PanelRequest.ItemID, addressIds[0]);

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
        protected void btnDeletePrivateByInject_Click(object sender, EventArgs e)
        {
            DeletePrivate(sender, e, true);
        }

        protected void btnDeletePrivate_Click(object sender, EventArgs e)
        {
            DeletePrivate(sender, e, false);
        }

        protected void DeletePrivate(object sender, EventArgs e, bool byNewMethod)
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
                ResultObject res = null;
                if (byNewMethod)
                    res = ES.Services.VPS2012.DeleteVirtualMachinePrivateIPAddressesByInject(PanelRequest.ItemID, addressIds);
                else
                    res = ES.Services.VPS2012.DeleteVirtualMachinePrivateIPAddresses(PanelRequest.ItemID, addressIds);

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
                ResultObject res = ES.Services.VPS2012.SetVirtualMachinePrimaryExternalIPAddress(PanelRequest.ItemID, addressIds[0]);

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
            DeleteExternal(sender, e, false);
        }
        protected void btnDeleteExternalByInject_Click(object sender, EventArgs e)
        {
            DeleteExternal(sender, e, true);
        }

        protected void DeleteExternal(object sender, EventArgs e, bool byNewMethod)
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
                ResultObject res = null; ES.Services.VPS2012.DeleteVirtualMachineExternalIPAddresses(PanelRequest.ItemID, addressIds);
                if(byNewMethod)
                    res = ES.Services.VPS2012.DeleteVirtualMachineExternalIPAddressesByInjection(PanelRequest.ItemID, addressIds);
                else
                    res = ES.Services.VPS2012.DeleteVirtualMachineExternalIPAddresses(PanelRequest.ItemID, addressIds);


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
