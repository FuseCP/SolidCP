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
    public partial class ParameterEditor : System.Web.UI.UserControl
    {
        public string DataType
        {
            get { return (string)ViewState["DataType"]; }
            set { ViewState["DataType"] = value; }
        }

        public string Value
        {
            get { return GetValue(); }
            set
            {
                ViewState["Value"] = value;
                SetValue();
            }
        }

        public string DefaultValue
        {
            get { return (string)ViewState["DefaultValue"]; }
            set { ViewState["DefaultValue"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                SetValue();
        }

        private void SetValue()
        {
            string val = (string)ViewState["Value"];
            if (String.Compare(DataType, "string", true) == 0)
            {
                txtValue.Text = (val != null) ? val : DefaultValue;
                txtValue.Visible = true;
            }
            else if (String.Compare(DataType, "multistring", true) == 0)
            {
                txtMultiValue.Text = (val != null) ? val : DefaultValue;
                txtMultiValue.Visible = true;
            }
            else if (String.Compare(DataType, "list", true) == 0)
            {
                try
                {
                    ddlValue.Items.Clear();
                    string[] vals = DefaultValue.Split(';');
                    foreach (string v in vals)
                    {
                        string itemValue = v;
                        string itemText = v;

                        int eqIdx = v.IndexOf("=");
                        if (eqIdx != -1)
                        {
                            itemValue = v.Substring(0, eqIdx);
                            itemText = v.Substring(eqIdx + 1);
                        }

                        ddlValue.Items.Add(new ListItem(itemText, itemValue));
                    }
                }
                catch { /* skip */ }

                Utils.SelectListItem(ddlValue, val);
                ddlValue.Visible = true;
            }
        }


        private string GetValue()
        {
            if (String.Compare(DataType, "string", true) == 0)
            {
                return txtValue.Text.Trim();
            }
            else if (String.Compare(DataType, "multistring", true) == 0)
            {
                return txtMultiValue.Text.Trim();
            }
            else
            {
                return ddlValue.SelectedValue;
            }
        }
    }
}
