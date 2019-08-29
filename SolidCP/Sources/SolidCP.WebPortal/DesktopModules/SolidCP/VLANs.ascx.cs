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
using System.Web.UI.WebControls;
using System.Collections.Generic;
using SolidCP.Providers.Common;
using System.Text;
using SolidCP.Portal.Code.Helpers;

namespace SolidCP.Portal
{
    public partial class VLANs : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // set display preferences
            if (!IsPostBack)
            {
                // page size
                gvVLANs.PageSize = UsersHelper.GetDisplayItemsPerPage();
                ddlItemsPerPage.SelectedValue = gvVLANs.PageSize.ToString();

                gvVLANs.PageIndex = PageIndex;
            }
            else
            {
                gvVLANs.PageSize = Utils.ParseInt(ddlItemsPerPage.SelectedValue, 10);
            }
        }

        protected void odsVLANs_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ProcessException(e.Exception);
                this.DisableControls = true;
                e.ExceptionHandled = true;
            }
        }

        public string GetSpaceHomeUrl(int spaceId)
        {
            return PortalUtils.GetSpaceHomePageUrl(spaceId);
        }

        public string GetReturnUrl()
        {
            var returnUrl = Request.Url.AddParameter("Page", gvVLANs.PageIndex.ToString());
            return Uri.EscapeDataString("~" + returnUrl.PathAndQuery);
        }

        public int PageIndex
        {
            get
            {
                return PanelRequest.GetInt("Page", 0);
            }
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ServerID", PanelRequest.ServerId.ToString(), "add_vlan", "ReturnUrl=" + GetReturnUrl()), true);
        }

        protected void ddlItemsPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvVLANs.PageSize = Utils.ParseInt(ddlItemsPerPage.SelectedValue, 10);
            gvVLANs.DataBind();
        }

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            int[] vlans = GetSelectedItems(gvVLANs);
            if (vlans.Length == 0)
            {
                ShowWarningMessage("VLAN_DELETE_LIST_EMPTY_ERROR");
                return;
            }

            try
            {
                // delete selected VLANs
                ResultObject res = ES.Services.Servers.DeletePrivateNetworkVLANs(vlans);

                if (!res.IsSuccess)
                {
                    messageBox.ShowMessage(res, "VLAN_DELETE_RANGE_VLAN", "VLAN");
                    return;
                }

                // refresh grid
                gvVLANs.DataBind();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("VLAN_DELETE_RANGE_VLAN", ex);
                return;
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