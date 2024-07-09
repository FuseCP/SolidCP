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
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.Common;

namespace SolidCP.Portal.UserControls
{
    public partial class PackageVLANs : SolidCPControlBase
    {
        private bool spaceOwner;

        private string spaceHomeControl;
        public string SpaceHomeControl
        {
            get { return spaceHomeControl; }
            set { spaceHomeControl = value; }
        }

        private string allocateVLANsControl;
        public string AllocateVLANsControl
        {
            get { return allocateVLANsControl; }
            set { allocateVLANsControl = value; }
        }

        public bool ManageAllowed
        {
            get { return ViewState["ManageAllowed"] != null ? (bool)ViewState["ManageAllowed"] : false; }
            set { ViewState["ManageAllowed"] = value; }
        }

        public bool IsDmz
        {
            get { return ViewState["IsDmz"] != null ? (bool)ViewState["IsDmz"] : false; }
            set { ViewState["IsDmz"] = value; }
        }

        private PackageVLANsPaged packageVLANs;

        protected void Page_Load(object sender, EventArgs e)
        {
            bool isUserSelected = PanelSecurity.SelectedUser.Role == SolidCP.EnterpriseServer.UserRole.User;
            bool isUserLogged = PanelSecurity.EffectiveUser.Role == SolidCP.EnterpriseServer.UserRole.User;
            spaceOwner = PanelSecurity.EffectiveUserId == PanelSecurity.SelectedUserId;

            cbIsDmz.Checked = IsDmz;

            gvVLANs.Columns[2].Visible = !isUserSelected; // space
            gvVLANs.Columns[3].Visible = !isUserSelected; // user

            // managing external network permissions
            gvVLANs.Columns[0].Visible = !isUserLogged && ManageAllowed;
            btnAllocateVLAN.Visible = !isUserLogged && !spaceOwner && ManageAllowed && !String.IsNullOrEmpty(AllocateVLANsControl);
            btnDeallocateVLANs.Visible = !isUserLogged && ManageAllowed;
        }

        public string GetSpaceHomeUrl(string spaceId)
        {
            return HostModule.EditUrl("SpaceID", spaceId, SpaceHomeControl);
        }

        protected void odsVLANsPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                messageBox.ShowErrorMessage("VLAN_GET_VLAN", e.Exception);
                e.ExceptionHandled = true;
            }
        }

        protected void btnAllocateVLAN_Click(object sender, EventArgs e)
        {
            Response.Redirect(HostModule.EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), AllocateVLANsControl));
        }

        protected void gvVLANs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PackageVLAN item = e.Row.DataItem as PackageVLAN;
            if (item != null)
            {
                // checkbox
                CheckBox chkSelect = e.Row.FindControl("chkSelect") as CheckBox;
                chkSelect.Enabled = (!spaceOwner || (PanelSecurity.PackageId != item.PackageId));
            }
        }

        protected void btnDeallocateVLANs_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> items = new List<int>();
                for (int i = 0; i < gvVLANs.Rows.Count; i++)
                {
                    GridViewRow row = gvVLANs.Rows[i];
                    CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                    if (chkSelect.Checked)
                        items.Add((int)gvVLANs.DataKeys[i].Value);
                }

                // check if at least one is selected
                if (items.Count == 0)
                {
                    messageBox.ShowWarningMessage("VLAN_EDIT_LIST_EMPTY_ERROR");
                    return;
                }

                ResultObject res = ES.Services.Servers.DeallocatePackageVLANs(PanelSecurity.PackageId, items.ToArray());
                messageBox.ShowMessage(res, "DEALLOCATE_SPACE_VLANS", "VPS");
                gvVLANs.DataBind();
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("DEALLOCATE_SPACE_VLANS", ex);
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvVLANs.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            gvVLANs.DataBind();
        }
    }
}