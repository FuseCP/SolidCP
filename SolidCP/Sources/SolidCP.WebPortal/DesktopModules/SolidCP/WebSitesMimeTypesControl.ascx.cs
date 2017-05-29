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

using SolidCP.Providers.Web;

namespace SolidCP.Portal
{
    public partial class WebSitesMimeTypesControl : SolidCPControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindWebItem(WebAppVirtualDirectory item)
        {
            gvMimeTypes.DataSource = item.MimeMaps;
            gvMimeTypes.DataBind();
        }

        public void SaveWebItem(WebAppVirtualDirectory item)
        {
            item.MimeMaps = CollectFormData(false).ToArray();
        }

        public List<MimeMap> CollectFormData(bool includeEmpty)
        {
            List<MimeMap> types = new List<MimeMap>();
            foreach (GridViewRow row in gvMimeTypes.Rows)
            {
                TextBox txtExtension = (TextBox)row.FindControl("txtExtension");
                TextBox txtMimeType = (TextBox)row.FindControl("txtMimeType");

                // create a new HttpError object and add it to the collection
                MimeMap type = new MimeMap();
                type.Extension = txtExtension.Text.Trim();
                type.MimeType = txtMimeType.Text.Trim();

                if (includeEmpty || type.Extension != "")
                    types.Add(type);
            }

            return types;
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            List<MimeMap> types = CollectFormData(true);

            // add empty error
            MimeMap type = new MimeMap();
            type.Extension = "";
            type.MimeType = "";
            types.Add(type);

            gvMimeTypes.DataSource = types;
            gvMimeTypes.DataBind();
        }
        protected void gvMimeTypes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "delete_item")
            {
                List<MimeMap> types = CollectFormData(true);

                // remove error
                types.RemoveAt(Utils.ParseInt((string)e.CommandArgument, 0));

                gvMimeTypes.DataSource = types;
                gvMimeTypes.DataBind();
            }
        }
        protected void gvMimeTypes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            StyleButton cmdDelete = (StyleButton)e.Row.FindControl("cmdDeleteMime");
            if (cmdDelete != null)
                cmdDelete.CommandArgument = e.Row.RowIndex.ToString();
        }
    }
}
