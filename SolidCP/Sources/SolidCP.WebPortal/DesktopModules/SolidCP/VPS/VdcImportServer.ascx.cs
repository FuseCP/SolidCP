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

namespace SolidCP.Portal.VPS
{
    public partial class VdcImportServer : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // bind hyper-V services
                BindHyperVServices();

                // bind virtual machines
                BindVirtualMachines();

                // bind OS templates
                BindOsTemplates();

                // bind IP addresses
                BindExternalAddresses();
                BindManagementAddresses();
            }

            ToggleControls();
        }

        private void ToggleControls()
        {
            AdminPasswordPanel.Visible = EnableRemoteDesktop.Checked;
            RequiredAdminPassword.Enabled = EnableRemoteDesktop.Checked;
            VirtualMachinePanel.Visible = (VirtualMachines.SelectedValue != "");
            ExternalAddressesRow.Visible = (ExternalAdapters.SelectedIndex != 0);
            ManagementAddressesRow.Visible = (ManagementAdapters.SelectedIndex != 0);
        }

        public void BindHyperVServices()
        {
            // bind
            HyperVServices.DataSource = ES.Services.Servers.GetRawServicesByGroupName(ResourceGroups.VPS).Tables[0].DefaultView;
            HyperVServices.DataBind();

            // add select value
            HyperVServices.Items.Insert(0, new ListItem(GetLocalizedString("SelectHyperVService.Text"), ""));
        }

        public void BindVirtualMachines()
        {
            // clear list
            VirtualMachines.Items.Clear();

            // bind
            int serviceId = Utils.ParseInt(HyperVServices.SelectedValue, 0);
            if (serviceId > 0)
            {
                VirtualMachines.DataSource = ES.Services.VPS.GetVirtualMachinesByServiceId(serviceId);
                VirtualMachines.DataBind();
            }

            // add select value
            VirtualMachines.Items.Insert(0, new ListItem(GetLocalizedString("SelectVirtualMachine.Text"), ""));
        }

        public void BindOsTemplates()
        {
            // clear list
            OsTemplates.Items.Clear();

            int serviceId = Utils.ParseInt(HyperVServices.SelectedValue, 0);
            if (serviceId > 0)
            {
                OsTemplates.DataSource = ES.Services.VPS.GetOperatingSystemTemplatesByServiceId(serviceId);
                OsTemplates.DataBind();
            }
            OsTemplates.Items.Insert(0, new ListItem(GetLocalizedString("SelectOsTemplate.Text"), ""));
        }

        public void BindVirtualMachineDetails()
        {
            int serviceId = Utils.ParseInt(HyperVServices.SelectedValue, 0);
            string vmId = VirtualMachines.SelectedValue;
            if (serviceId > 0 && vmId != "")
            {
                VirtualMachine vm = ES.Services.VPS.GetVirtualMachineExtendedInfo(serviceId, vmId);
                if (vm != null)
                {
                    // bind VM
                    CpuCores.Text = vm.CpuCores.ToString();
                    RamSize.Text = vm.RamSize.ToString();
                    HddSize.Text = vm.HddSize.ToString();
                    VhdPath.Text = vm.VirtualHardDrivePath;

                    // other settings
                    NumLockEnabled.Value = vm.NumLockEnabled;
                    BootFromCd.Value = vm.BootFromCD;
                    DvdInstalled.Value = vm.DvdDriveInstalled;

                    // network adapters
                    ExternalAdapters.DataSource = vm.Adapters;
                    ExternalAdapters.DataBind();
                    ExternalAdapters.Items.Insert(0, new ListItem(GetLocalizedString("SelectNetworkAdapter.Text"), ""));

                    ManagementAdapters.DataSource = vm.Adapters;
                    ManagementAdapters.DataBind();
                    ManagementAdapters.Items.Insert(0, new ListItem(GetLocalizedString("SelectNetworkAdapter.Text"), ""));
                }
            }
        }

        public void BindExternalAddresses()
        {
            BindAddresses(ExternalAddresses, IPAddressPool.VpsExternalNetwork);
        }

        public void BindManagementAddresses()
        {
            BindAddresses(ManagementAddresses, IPAddressPool.VpsManagementNetwork);
        }

        public void BindAddresses(ListBox list, IPAddressPool pool)
        {
            IPAddressInfo[] ips = ES.Services.Servers.GetUnallottedIPAddresses(PanelSecurity.PackageId, ResourceGroups.VPS, pool);
            foreach (IPAddressInfo ip in ips)
            {
                string txt = ip.ExternalIP;
                if (!String.IsNullOrEmpty(ip.DefaultGateway))
                    txt += "/" + ip.DefaultGateway;
                list.Items.Add(new ListItem(txt, ip.AddressId.ToString()));
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                // external IPs
                List<int> extIps = new List<int>();
                foreach (ListItem li in ExternalAddresses.Items)
                    if (li.Selected) extIps.Add(Utils.ParseInt(li.Value));

                // management IPs
                int manIp = 0;
                foreach (ListItem li in ManagementAddresses.Items)
                    if (li.Selected)
                    {
                        manIp = Utils.ParseInt(li.Value);
                        break;
                    }

                // create virtual machine
                IntResult res = ES.Services.VPS.ImportVirtualMachine(PanelSecurity.PackageId,
                    Utils.ParseInt(HyperVServices.SelectedValue),
                    VirtualMachines.SelectedValue,
                    OsTemplates.SelectedValue, adminPassword.Text,
                    AllowStartShutdown.Checked, AllowPause.Checked, AllowReboot.Checked, AllowReset.Checked, false,
                    ExternalAdapters.SelectedValue, extIps.ToArray(),
                    ManagementAdapters.SelectedValue, manIp);

                if (res.IsSuccess)
                {
                    Response.Redirect(EditUrl("ItemID", res.Value.ToString(), "vps_general",
                        "SpaceID=" + PanelSecurity.PackageId.ToString()));
                }
                else
                {
                    messageBox.ShowMessage(res, "VPS_ERROR_IMPORT", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_IMPORT", ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), ""));
        }

        protected void HyperVServices_SelectedIndexChanged(object sender, EventArgs e)
        {
            // bind VMs
            BindVirtualMachines();

            // bind OS templates
            BindOsTemplates();
        }

        protected void VirtualMachines_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindVirtualMachineDetails();
            ToggleControls();
        }
    }
}
