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

namespace SolidCP.Portal
{
    public partial class QuotaEditor : SolidCPControlBase
    {
        public int QuotaId
        {
            get { return (int)ViewState["QuotaId"]; }
            set { ViewState["QuotaId"] = value; }
        }

        public int QuotaTypeId
        {
            get { return (int)ViewState["QuotaTypeId"]; }
            set
            {
                ViewState["QuotaTypeId"] = value;

                // toggle controls
                txtQuotaValue.Visible = (value > 1);
                chkQuotaUnlimited.Visible = (value > 1);
                chkQuotaEnabled.Visible = (value == 1);
            }
        }

        public string UnlimitedText
        {
            get { return chkQuotaUnlimited.Text; }
            set { chkQuotaUnlimited.Text = value; }

        }

        public int QuotaValue
        {
            get
            {
                if (QuotaTypeId == 1)
                    // bool quota
                    return chkQuotaEnabled.Checked ? 1 : 0;
                else
                {
                    if (ParentQuotaValue == -1)
                    {
                        if ((QuotaMinValue > 0) | (QuotaMaxValue > 0))
                        {
                            int quotaValue = 0;
                            // numeric quota
                            if (chkQuotaUnlimited.Checked)
                                quotaValue = -1;
                            else
                            {

                                if (QuotaMinValue > 0)
                                    quotaValue = Math.Max(Utils.ParseInt(txtQuotaValue.Text, 0), QuotaMinValue);
                                else
                                    quotaValue = Utils.ParseInt(txtQuotaValue.Text, 0);

                                if (QuotaMaxValue > 0)
                                {
                                    if (Utils.ParseInt(txtQuotaValue.Text, 0) > QuotaMaxValue)
                                        quotaValue = QuotaMaxValue;
                                }
                            }
                            return quotaValue;
                        }
                        else
                            return chkQuotaUnlimited.Checked ? -1 : Utils.ParseInt(txtQuotaValue.Text, 0);
                        
                    }
                    else
                    {

                        if ((QuotaMinValue > 0) | (QuotaMaxValue > 0))
                        {

                            int quotaValue = 0;
                            // numeric quota
                            if (chkQuotaUnlimited.Checked)
                                quotaValue = ParentQuotaValue;
                            else
                            {
                                quotaValue = Utils.ParseInt(txtQuotaValue.Text, 0);


                                if (QuotaMinValue > 0)
                                    quotaValue = Math.Max(quotaValue, QuotaMinValue);

                                if (QuotaMaxValue > 0)
                                {
                                    if (quotaValue > QuotaMaxValue)
                                        quotaValue = QuotaMaxValue;
                                }

                                quotaValue = Math.Min(quotaValue, ParentQuotaValue);
                            }
                            return quotaValue;
                        }
                        else
                        {
                            return
                                chkQuotaUnlimited.Checked
                                    ? ParentQuotaValue
                                    : Math.Min(Utils.ParseInt(txtQuotaValue.Text, 0), ParentQuotaValue);
                        }


                        
                    }
                }
            }
            set
            {
                if (QuotaMinValue > 0)
                    txtQuotaValue.Text = Math.Max(value, QuotaMinValue).ToString();
                else
                    txtQuotaValue.Text = value.ToString();

                chkQuotaEnabled.Checked = (value > 0);
                chkQuotaUnlimited.Checked = (value == -1);
            }
        }

        public int QuotaMinValue
        {
            get { return ViewState["QuotaMinValue"] != null ? (int)ViewState["QuotaMinValue"] : 0; }
            set
            {
                ViewState["QuotaMinValue"] = value;

                if (QuotaMinValue > 0)
                {
                    if (QuotaValue < QuotaMinValue) QuotaValue = QuotaMinValue;
                }
            }

        }

        public int QuotaMaxValue
        {
            get { return ViewState["QuotaMaxValue"] != null ? (int)ViewState["QuotaMaxValue"] : 0; }
            set { ViewState["QuotaMaxValue"] = value; }
        }

        public int ParentQuotaValue
        {
            set
            {
                ViewState["ParentQuotaValue"] = value;
                if (value == 0)
                {
                    txtQuotaValue.Enabled = false;
                    chkQuotaEnabled.Enabled = false;
                    chkQuotaUnlimited.Visible = false;
                    chkQuotaEnabled.Checked = false;
                }

                if (value != -1)
                    chkQuotaUnlimited.Visible = false;
            }
            get
            {
                return ViewState["ParentQuotaValue"] != null ? (int)ViewState["ParentQuotaValue"] : 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WriteScriptBlock();
        }

        protected override void OnPreRender(EventArgs e)
        {
            // set textbox attributes
            txtQuotaValue.Style["display"] = (txtQuotaValue.Text == "-1") ? "none" : "inline";

            chkQuotaUnlimited.Attributes["onclick"] = String.Format("ToggleQuota('{0}', '{1}', {2});",
                txtQuotaValue.ClientID, chkQuotaUnlimited.ClientID, QuotaMinValue);

            // call base handler
            base.OnPreRender(e);
        }

        private void WriteScriptBlock()
        {
            string scriptKey = "QuataScript";
            if (!Page.ClientScript.IsClientScriptBlockRegistered(scriptKey))
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), scriptKey, @"<script language='javascript' type='text/javascript'>
                        function ToggleQuota(txtId, chkId, minValue)
                        {   
                            var unlimited = document.getElementById(chkId).checked;
                            document.getElementById(txtId).style.display = unlimited ? 'none' : 'inline';
                            document.getElementById(txtId).value = unlimited ? '-1' : '0';
                            if (minValue > 0) 
                            {
                                if (document.getElementById(txtId).value < minValue) document.getElementById(txtId).value = minValue;
                            }
                        }
                        </script>");
            }

        }


    }
}
