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
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CPCC;

using SolidCP.Providers.Statistics;

namespace SolidCP.Portal.ProviderControls
{
    public partial class SmarterStats_EditSite : SolidCPControlBase, IStatsEditInstallationControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && ViewState["binded"] == null)
            {
                // users
                List<StatsUser> users = new List<StatsUser>();
                AddNewUser(users, true, true);

                users[0].Username = "admin";

                // bind users
                gvUsers.DataSource = users;
                gvUsers.DataBind();

                // set site ID
                txtSiteId.Text = GetLocalizedString("Text.Pending");
                litSiteStatus.Text = GetLocalizedString("Text.Unknown");
            }
        }

        public void BindItem(StatsSite item)
        {
            LocalizeGridView(gvUsers);

            if (item == null)
                return;

            txtSiteId.Text = item.SiteId;
            litSiteStatus.Text = item.Status;

            // users
            List<StatsUser> users = new List<StatsUser>();
            users.AddRange(item.Users);

            if (users.Count == 0)
                AddNewUser(users, true, true);

            // bind users
            gvUsers.DataSource = users;
            gvUsers.DataBind();

            ViewState["binded"] = true;
        }

        public void SaveItem(StatsSite item)
        {
            // users
            item.Users = CollectFormData(false).ToArray();
        }

        public List<StatsUser> CollectFormData(bool includeEmpty)
        {
            List<StatsUser> users = new List<StatsUser>();
            foreach (GridViewRow row in gvUsers.Rows)
            {
                CheckBox chkOwner = (CheckBox)row.FindControl("chkOwner");
                CheckBox chkAdmin = (CheckBox)row.FindControl("chkAdmin");
                TextBox txtUsername = (TextBox)row.FindControl("txtUsername");
                TextBox txtPassword = (TextBox)row.FindControl("txtPassword");
                TextBox txtFirstName = (TextBox)row.FindControl("txtFirstName");
                TextBox txtLastName = (TextBox)row.FindControl("txtLastName");

                // create a new HttpError object and add it to the collection
                StatsUser user = new StatsUser();
                user.IsOwner = chkOwner.Checked;
                user.IsAdmin = chkAdmin.Checked;
                user.Username = txtUsername.Text.Trim();
                user.Password = txtPassword.Text.Trim();
                user.FirstName = txtFirstName.Text.Trim();
                user.LastName = txtLastName.Text.Trim();

                if (includeEmpty || user.Username != "")
                    users.Add(user);
            }

            return users;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            // collect form data
            List<StatsUser> users = CollectFormData(true);

            // add new user
            AddNewUser(users, false, false);

            // bind users
            gvUsers.DataSource = users;
            gvUsers.DataBind();
        }

        public void AddNewUser(List<StatsUser> users, bool isAdmin, bool isOwner)
        {
            StatsUser user = new StatsUser();
            user.IsAdmin = isAdmin;
            user.IsOwner = isOwner;
            user.Username = "";
            user.Password = "";
            user.FirstName = "";
            user.LastName = "";
            users.Add(user);
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "delete_item")
            {
                List<StatsUser> users = CollectFormData(true);

                // remove error
                users.RemoveAt(Utils.ParseInt((string)e.CommandArgument, 0));

                // bind users
                gvUsers.DataSource = users;
                gvUsers.DataBind();
            }
        }

        protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            StyleButton cmdDelete = (StyleButton)e.Row.FindControl("cmdDelete");
            CheckBox chkAdmin = (CheckBox)e.Row.FindControl("chkAdmin");

            if (cmdDelete != null)
                cmdDelete.CommandArgument = e.Row.RowIndex.ToString();

            StatsUser user = (StatsUser)e.Row.DataItem;
            if (user != null && user.IsOwner)
            {
                cmdDelete.Visible = false;
                chkAdmin.Enabled = false;
            }
        }
    }
}
