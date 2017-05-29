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
    public partial class VpsDetailsConfiguration : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request["action"] == "changed")
                messageBox.ShowSuccessMessage("VPS_CHANGE_VM_CONFIGURATION");

            if (!IsPostBack)
            {
                // config
                BindConfiguration();

                // bind password policy
                password.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.VPS_POLICY, "AdministratorPasswordPolicy");
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

                // bind item
                litOperatingSystem.Text = vm.OperatingSystemTemplate;

                litCpu.Text = String.Format(GetLocalizedString("CpuCores.Text"), vm.CpuCores);
                litRam.Text = String.Format(GetLocalizedString("Ram.Text"), vm.RamSize);
                litHdd.Text = String.Format(GetLocalizedString("Hdd.Text"), vm.HddSize);
                litSnapshots.Text = vm.SnapshotsNumber.ToString();

                optionDvdInstalled.Value = vm.DvdDriveInstalled;
                optionBootFromCD.Value = vm.BootFromCD;
                optionNumLock.Value = vm.NumLockEnabled;

                optionStartShutdown.Value = vm.StartTurnOffAllowed;
                optionPauseResume.Value = vm.PauseResumeAllowed;
                optionReset.Value = vm.ResetAllowed;
                optionReboot.Value = vm.RebootAllowed;
                optionReinstall.Value = vm.ReinstallAllowed;

                optionExternalNetwork.Value = vm.ExternalNetworkEnabled;
                optionPrivateNetwork.Value = vm.PrivateNetworkEnabled;

                // toggle buttons
                bool manageAllowed = VirtualMachinesHelper.IsVirtualMachineManagementAllowed(PanelSecurity.PackageId);
                btnEdit.Visible = manageAllowed;
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_LOAD_VM_META_ITEM", ex);
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                ResultObject res = ES.Services.VPS.ChangeAdministratorPassword(PanelRequest.ItemID, password.Password);

                if (res.IsSuccess)
                {
                    // show success message
                    messageBox.ShowSuccessMessage("VPS_CHANGE_ADMIN_PASSWORD");
                }
                else
                {
                    // show error
                    messageBox.ShowMessage(res, "VPS_CHANGE_ADMIN_PASSWORD", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_CHANGE_ADMIN_PASSWORD", ex);
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "vps_edit_config",
                "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }
    }
}
