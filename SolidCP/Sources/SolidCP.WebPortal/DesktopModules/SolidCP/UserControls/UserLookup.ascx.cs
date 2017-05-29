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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

namespace SolidCP.Portal
{
    public partial class UserLookup : SolidCPControlBase
    {
        public bool AllowEmptySelection
        {
            get { return (ViewState["AllowEmptySelection"] != null) ? (bool)ViewState["AllowEmptySelection"] : true; }
            set { ViewState["AllowEmptySelection"] = value; }
        }

        public int SelectedUserId
        {
            get { return (ViewState["SelectedUserId"] != null) ? (int)ViewState["SelectedUserId"] : 0; }
            set { ViewState["SelectedUserId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                searchBox.AddCriteria("Username", GetLocalizedString("SearchField.Username"));
                searchBox.AddCriteria("FullName", GetLocalizedString("SearchField.Name"));
                searchBox.AddCriteria("Email", GetLocalizedString("SearchField.Email"));

                // reset user
                user.UserId = SelectedUserId;

                // toggle controls
                ToggleSearchPanel(false);

                // remove menu items
                if (!AllowEmptySelection)
                    RemoveMenuItem("switch_empty");
            }
            searchBox.AjaxData = this.GetSearchBoxAjaxData();
        }

        private void RemoveMenuItem(string name)
        {
            MenuItem item = null;
            foreach (MenuItem mnuItem in mnuActions.Items[0].ChildItems)
            {
                if (mnuItem.Value == name)
                {
                    item = mnuItem;
                    break;
                }
            }

            if (item != null)
                mnuActions.Items[0].ChildItems.Remove(item);
        }

        private void ToggleSearchPanel(bool selectMode)
        {
            SelectPanel.Visible = selectMode;
            gvUsers.DataSourceID = selectMode ? "odsUsersPaged" : "";
        }

        protected void mnuPackages_MenuItemClick(object sender, MenuEventArgs e)
        {
            if (e.Item.Value == "switch_logged")
            {
                ToggleSearchPanel(false);
                SelectedUserId = PanelSecurity.EffectiveUserId;
                user.UserId = SelectedUserId;
            }
            else if (e.Item.Value == "switch_workfor")
            {
                ToggleSearchPanel(false);
                SelectedUserId = PanelSecurity.SelectedUserId;
                user.UserId = SelectedUserId;
            }
            else if (e.Item.Value == "switch_empty")
            {
                ToggleSearchPanel(false);
                SelectedUserId = 0;
                user.UserId = SelectedUserId;
            }
            else if (e.Item.Value == "select")
            {
                ToggleSearchPanel(true);
            }
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "select")
            {
                SelectedUserId = Utils.ParseInt(e.CommandArgument.ToString(), PanelSecurity.EffectiveUserId);
                user.UserId = SelectedUserId;
                ToggleSearchPanel(false);
            }
        }

        protected void odsUsersPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ToggleSearchPanel(false);
        }

        public string GetSearchBoxAjaxData()
        {
            StringBuilder res = new StringBuilder();
            res.Append("PagedStored: 'Users'");
            res.Append(", UserID: " + PanelSecurity.EffectiveUserId.ToString());
	        res.Append(", StatusID: 0");
            res.Append(", RoleID: 0");
            res.Append(", Recursive: true");
            return res.ToString();
        }
    }
}
