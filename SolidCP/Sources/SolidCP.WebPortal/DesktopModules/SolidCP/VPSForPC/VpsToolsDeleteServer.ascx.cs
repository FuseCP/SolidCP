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

namespace SolidCP.Portal.VPSForPC
{
    public partial class VpsToolsDeleteServer : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindFormDetails();
            }

            ToogleControls();
        }

        private void ToogleControls()
        {
            //txtExportPath.Visible = chkExport.Checked;
            //ExportPathValidator.Enabled = chkExport.Checked;
        }

        private void BindFormDetails()
        {
            // load VM item
            VMInfo vm = VirtualMachinesForPCHelper.GetCachedVirtualMachineForPC(PanelRequest.ItemID);
            if (!String.IsNullOrEmpty(vm.CurrentTaskId))
            {
                messageBox.ShowWarningMessage("VPS_PROVISIONING_PROCESS");
                btnDelete.Enabled = false;
                return;
            }

            AdminOptionsPanel.Visible = (PanelSecurity.EffectiveUser.Role == SolidCP.EnterpriseServer.UserRole.Administrator);
            
            //// load export settings
            //if (PanelSecurity.EffectiveUser.Role == SolidCP.EnterpriseServer.UserRole.Administrator)
            //{
            //    txtExportPath.Text = ES.Services.VPSPC.GetDefaultExportPath(PanelRequest.ItemID);
            //}
            //else
            //{
            //    AdminOptionsPanel.Visible = false;
            //}
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), ""));
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!chkConfirmDelete.Checked)
            {
                messageBox.ShowWarningMessage("VPS_DELETE_CONFIRM");
                return;
            }

            // delete machine
            try
            {
                ResultObject res = ES.Services.VPSPC.DeleteVirtualMachine(PanelRequest.ItemID,
                    chkSaveFiles.Checked, false, String.Empty);

                if (res.IsSuccess)
                {
                    // return to the list
                    Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), ""));
                    return;
                }
                else
                {
                    // show error
                    messageBox.ShowMessage(res, "VPS_ERROR_DELETE", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_DELETE", ex);
            }
        }
    }
}
