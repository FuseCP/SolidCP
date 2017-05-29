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
    public partial class EditItemComments : SolidCPControlBase
    {
        public int ItemId
        {
            get
            {
                // get item id from view state
                int itemId = (ViewState["ItemId"] != null) ? (int)ViewState["ItemId"] : -1;
                if (itemId == -1)
                {
                    // lookup in the request
                    if (RequestItemId != null)
                        itemId = Utils.ParseInt(Request[RequestItemId], -1);
                }

                return itemId;
            }
            set { ViewState["ItemId"] = value; }
        }

        public string ItemTypeId
        {
            get { return (ViewState["ItemTypeId"] != null) ? (string)ViewState["ItemTypeId"] : ""; }
            set { ViewState["ItemTypeId"] = value; }
        }

        public string RequestItemId
        {
            get { return (ViewState["RequestItemId"] != null) ? (string)ViewState["RequestItemId"] : null; }
            set { ViewState["RequestItemId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindComments();

                btnAdd.Enabled = (ItemId > 0);
                AddCommentPanel.Visible = (ItemId > 0);
            }
        }

        private void BindComments()
        {
            try
            {
                gvComments.DataSource = ES.Services.Comments.GetComments(PanelSecurity.EffectiveUserId, ItemTypeId, ItemId);
                gvComments.DataBind();
            }
            catch (Exception ex)
            {
                HostModule.ShowErrorMessage("COMMENT_GET", ex);
                return;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtComments.Text.Trim() == "")
                return;

            try
            {
                int result = ES.Services.Comments.AddComment(ItemTypeId, ItemId, txtComments.Text, 2);
                if (result < 0)
                {
                    HostModule.ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                HostModule.ShowErrorMessage("COMMENT_ADD", ex);
                return;
            }

            // clear fields
            txtComments.Text = "";

            // rebind list
            BindComments();
        }

        public string WrapComment(string text)
        {
            return (text != null) ? Server.HtmlEncode(text).Replace("\n", "<br/>") : text;
        }

        protected void gvComments_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int commentId = (int)gvComments.DataKeys[e.RowIndex][0];

            // delete comment
            try
            {
                int result = ES.Services.Comments.DeleteComment(commentId);
                if (result < 0)
                {
                    HostModule.ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                HostModule.ShowErrorMessage("COMMENT_DELETE", ex);
                return;
            }

            // rebind list
            BindComments();
        }
    }
}
