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
    public partial class SearchBox : SolidCPControlBase
    {
        public string AjaxData { get; set; }

        public string FilterColumn
        {
            get
            {
                return ddlFilterColumn.SelectedValue;
            }
            set
            {
                ddlFilterColumn.SelectedIndex = -1;
                ListItem li = ddlFilterColumn.Items.FindByValue(value);
                if (li != null)
                    li.Selected = true;
            }
        }

        public string FilterValue
        {
            get
            {
                string val = tbSearchText.Text.Trim();
                string valText = tbSearch.Text.Trim();
                if (valText.Length == 0)
                    val = valText;
                if (val.Length == 0)
                    val = tbSearch.Text.Trim();
                val = val.Replace("%", "");
                return "%" + val + "%";
            }
            set
            {
                if (value != null)
                {
                    value = value.Replace("%", "");
                    tbSearch.Text = value;
                    tbSearchText.Text = value;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void AddCriteria(string columnName, string columnTitle)
        {
            ddlFilterColumn.Items.Add(new ListItem(columnTitle, columnName));
        }

        public string GetCriterias()
        {
            string res = null;
            foreach (ListItem itm in ddlFilterColumn.Items)
            {
                if (res != null)
                    res += ", ";
                res = res + "'" + itm.Value + "'";
            }
            return res;
        }

        public override void Focus()
        {
            base.Focus();
            tbSearch.Focus();
        }

        protected void cmdSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (tbObjectId.Text.Length > 0)
            {
                Response.Redirect(PortalUtils.GetUserHomePageUrl(Int32.Parse(tbObjectId.Text)));
            }
            else
            {
                String strText = tbSearchText.Text;
                if (strText.Length > 0)
                {
                    Response.Redirect(NavigatePageURL(PortalUtils.GetUserCustomersPageId(),
                        PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString(),
                        "FilterColumn=" + ddlFilterColumn.SelectedValue,
                        "FilterValue=" + Server.UrlEncode(FilterValue)));
                }
                else
                {
                    Response.Redirect(PortalUtils.NavigatePageURL(PortalUtils.GetObjectSearchPageId(),
                        PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString(),
                        "Query=" + Server.UrlEncode(tbSearch.Text),"FullType=Users"));
                }
            }
        }
    }
}
