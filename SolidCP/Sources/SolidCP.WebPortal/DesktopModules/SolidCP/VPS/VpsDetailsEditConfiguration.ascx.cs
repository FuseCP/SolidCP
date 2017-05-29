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
using SolidCP.Providers.Common;
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal.VPS
{
    public partial class VpsDetailsEditConfiguration : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindConfiguration();
            }
        }

        private void BindConfiguration()
        {
            VirtualMachine vm = null;

            try
            {
                // load machine
                vm = ES.Services.VPS.GetVirtualMachineItem(PanelRequest.ItemID);

                if (vm == null)
                {
                    messageBox.ShowErrorMessage("VPS_LOAD_VM_META_ITEM");
                    return;
                }

                // bind CPU cores
                int maxCores = ES.Services.VPS.GetMaximumCpuCoresNumber(vm.PackageId);
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
                if (cntx.Quotas.ContainsKey(Quotas.VPS_CPU_NUMBER))
                {
                    QuotaValueInfo cpuQuota = cntx.Quotas[Quotas.VPS_CPU_NUMBER];

                    if (cpuQuota.QuotaAllocatedValue != -1
                        && maxCores > cpuQuota.QuotaAllocatedValue)
                        maxCores = cpuQuota.QuotaAllocatedValue;
                }

                for (int i = 1; i < maxCores + 1; i++)
                    ddlCpu.Items.Add(i.ToString());

                // bind item
                ddlCpu.SelectedValue = vm.CpuCores.ToString();
                txtRam.Text = vm.RamSize.ToString();
                txtHdd.Text = vm.HddSize.ToString();
                txtSnapshots.Text = vm.SnapshotsNumber.ToString();

                chkDvdInstalled.Checked = vm.DvdDriveInstalled;
                chkBootFromCd.Checked = vm.BootFromCD;
                chkNumLock.Checked = vm.NumLockEnabled;

                chkStartShutdown.Checked = vm.StartTurnOffAllowed;
                chkPauseResume.Checked = vm.PauseResumeAllowed;
                chkReset.Checked = vm.ResetAllowed;
                chkReboot.Checked = vm.RebootAllowed;
                chkReinstall.Checked = vm.ReinstallAllowed;

                chkExternalNetworkEnabled.Checked = vm.ExternalNetworkEnabled;
                chkPrivateNetworkEnabled.Checked = vm.PrivateNetworkEnabled;

                // other quotas
                BindCheckboxOption(chkDvdInstalled, Quotas.VPS_DVD_ENABLED);
                chkBootFromCd.Enabled = PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.VPS_BOOT_CD_ALLOWED);

                BindCheckboxOption(chkStartShutdown, Quotas.VPS_START_SHUTDOWN_ALLOWED);
                BindCheckboxOption(chkPauseResume, Quotas.VPS_PAUSE_RESUME_ALLOWED);
                BindCheckboxOption(chkReset, Quotas.VPS_RESET_ALOWED);
                BindCheckboxOption(chkReboot, Quotas.VPS_REBOOT_ALLOWED);
                BindCheckboxOption(chkReinstall, Quotas.VPS_REINSTALL_ALLOWED);

                BindCheckboxOption(chkExternalNetworkEnabled, Quotas.VPS_EXTERNAL_NETWORK_ENABLED);
                BindCheckboxOption(chkPrivateNetworkEnabled, Quotas.VPS_PRIVATE_NETWORK_ENABLED);
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_LOAD_VM_META_ITEM", ex);
            }
        }

        private void BindCheckboxOption(CheckBox chk, string quotaName)
        {
            chk.Enabled = PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, quotaName);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectBack("cancel");
        }

        private void RedirectBack(string action)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "vps_config",
                "SpaceID=" + PanelSecurity.PackageId.ToString(),
                "action=" + action));
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // check rights
                bool manageAllowed = VirtualMachinesHelper.IsVirtualMachineManagementAllowed(PanelSecurity.PackageId);
                if (!manageAllowed)
                {
                    return;
                }

                ResultObject res = ES.Services.VPS.UpdateVirtualMachineConfiguration(PanelRequest.ItemID,
                    Utils.ParseInt(ddlCpu.SelectedValue),
                    Utils.ParseInt(txtRam.Text.Trim()),
                    Utils.ParseInt(txtHdd.Text.Trim()),
                    Utils.ParseInt(txtSnapshots.Text.Trim()),
                    chkDvdInstalled.Checked,
                    chkBootFromCd.Checked,
                    chkNumLock.Checked,
                    chkStartShutdown.Checked,
                    chkPauseResume.Checked,
                    chkReboot.Checked,
                    chkReset.Checked,
                    chkReinstall.Checked,
                    chkExternalNetworkEnabled.Checked,
                    chkPrivateNetworkEnabled.Checked);

                if (res.IsSuccess)
                {
                    // redirect back
                    RedirectBack("changed");
                }
                else
                {
                    // show error
                    messageBox.ShowMessage(res, "VPS_CHANGE_VM_CONFIGURATION", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_CHANGE_VM_CONFIGURATION", ex);
            }
        }
    }
}
