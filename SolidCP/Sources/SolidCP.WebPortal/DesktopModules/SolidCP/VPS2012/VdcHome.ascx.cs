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
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.Virtualization;

namespace SolidCP.Portal.VPS2012
{
    public partial class VdcHome : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                searchBox.AddCriteria("ItemName", GetLocalizedString("SearchField.ItemName"));
                searchBox.AddCriteria("Username", GetLocalizedString("SearchField.Username"));
                searchBox.AddCriteria("ExternalIP", GetLocalizedString("SearchField.ExternalIP"));
                searchBox.AddCriteria("IPAddress", GetLocalizedString("SearchField.IPAddress"));
            }
            searchBox.AjaxData = this.GetSearchBoxAjaxData();

            // toggle columns
            bool isUserSelected = PanelSecurity.SelectedUser.Role == SolidCP.EnterpriseServer.UserRole.User;
            gvServers.Columns[3].Visible = !isUserSelected;
            gvServers.Columns[4].Visible = !isUserSelected;

            // replication
            gvServers.Columns[5].Visible = false;
            btnReplicaStates.Visible = PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.VPS2012_REPLICATION_ENABLED);

            // check package quotas
            bool manageAllowed = VirtualMachines2012Helper.IsVirtualMachineManagementAllowed(PanelSecurity.PackageId);

            btnCreate.Visible = manageAllowed;
            btnImport.Visible = (PanelSecurity.EffectiveUser.Role == UserRole.Administrator);
            gvServers.Columns[6].Visible = manageAllowed; // delete column

            // admin operations column
            gvServers.Columns[7].Visible = (PanelSecurity.EffectiveUser.Role == UserRole.Administrator);

        }

        public string GetServerEditUrl(string itemID)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "vps_general",
                    "ItemID=" + itemID);
        }

        public string GetSpaceHomeUrl(string spaceId)
        {
            return EditUrl("SpaceID", spaceId, "");
        }

        public string GetUserHomeUrl(int userId)
        {
            return PortalUtils.GetUserHomePageUrl(userId);
        }

        private VirtualMachine[] _machines;
        public string GetReplicationStatus(int itemID)
        {
            if (!gvServers.Columns[5].Visible)
                return "";

            if (_machines == null)
            {
                var packageVm = ES.Services.VPS2012.GetVirtualMachineItem(itemID);
                _machines = ES.Services.VPS2012.GetVirtualMachinesByServiceId(packageVm.ServiceId);
            }

            var vmItem = ES.Services.VPS2012.GetVirtualMachineItem(itemID);
            if (vmItem == null) return "";

            var vm = _machines.FirstOrDefault(v => v.VirtualMachineId == vmItem.VirtualMachineId);
            return vm != null ? vm.ReplicationState.ToString() : "";
        }
        
        protected void odsServersPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                messageBox.ShowErrorMessage("EXCHANGE_GET_MAILBOXES", e.Exception);
                e.ExceptionHandled = true;
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "vdc_create_server"));
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "vdc_import_server"));
        }

        protected void btnReplicaStates_Click(object sender, EventArgs e)
        {
            gvServers.Columns[5].Visible = PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.VPS2012_REPLICATION_ENABLED);
        }

        protected void gvServers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                // get server ID
                int itemId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                // go to delete page
                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "vps_tools_delete",
                    "ItemID=" + itemId));
            }
            else if (e.CommandName == "Detach")
            {
                // remove item from meta base
                int itemId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                int result = ES.Services.Packages.DetachPackageItem(itemId);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                // refresh the list
                gvServers.DataBind();
            }
            else if (e.CommandName == "Move")
            {
                // get server ID
                int itemId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                // go to delete page
                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "vps_tools_move",
                    "ItemID=" + itemId));
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvServers.PageSize = Convert.ToInt16(ddlPageSize.SelectedValue);
            gvServers.DataBind();
        }

        public string GetSearchBoxAjaxData()
        {
            StringBuilder res = new StringBuilder();
            res.Append("PagedStored: 'VirtualMachines'");
            res.Append(", RedirectUrl: '" + GetServerEditUrl("{0}").Substring(2) + "'");
            res.Append(", PackageID: " + (String.IsNullOrEmpty(Request["SpaceID"]) ? "0" : Request["SpaceID"]));
            res.Append(", Recursive: true");
            res.Append(", VPSTypeID: 'VPS2012'");
            return res.ToString();
        }
    }
}
