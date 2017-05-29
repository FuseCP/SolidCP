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

namespace SolidCP.Portal
{
    public partial class ServersEditEventViewer : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // set display preferences
                gvEntries.PageSize = UsersHelper.GetDisplayItemsPerPage();

                btnClearLog.Enabled = false;
                ddlLogNames.Attributes.Add("onchange", "ShowProgressDialog('Loading...');");
                BindLogNames();
            }
        }

        private void BindLogNames()
        {
            ddlLogNames.DataSource = ES.Services.Servers.GetLogNames(PanelRequest.ServerId);
            ddlLogNames.DataBind();

            // add empty
            ddlLogNames.Items.Insert(0, new ListItem(GetLocalizedString("SelectLog.Text"), ""));
        }

        protected void odsLogEntries_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                messageBox.Visible = true;
                messageBox.ShowErrorMessage("SERVERSEDITEVENTVIEWER_SELECTED", e.Exception);
                btnClearLog.Enabled = false;
                e.ExceptionHandled = true;
            }
            else
            {
                messageBox.Visible = false;
            }
        }

        protected void btnClearLog_Click(object sender, EventArgs e)
        {
            int result = ES.Services.Servers.ClearLog(PanelRequest.ServerId, ddlLogNames.SelectedValue);
            if (result < 0)
            {
                ShowResultMessage(result);
                return;
            }

            // rebind items
            gvEntries.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ServerID", PanelRequest.ServerId.ToString(), "edit_server"));
        }

        protected void gvEntries_DataBound(object sender, EventArgs e)
        {
            if (gvEntries.Rows.Count > 0)
            {
                btnClearLog.Enabled = true;
            }
            else
            {
                btnClearLog.Enabled = false;
            }
        }

        protected void LogNameSelected(object sender, EventArgs e)
        {
            if (gvEntries.PageIndex > 0)
            {
                gvEntries.PageIndex = 0;
                gvEntries.DataBind();
            }
        }
    }
}
