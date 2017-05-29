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

namespace SolidCP.Portal.ExchangeServer.UserControls
{
    public partial class SizeBox : System.Web.UI.UserControl
    {
        int emptyValue = -1;

        public int EmptyValue
        {
            get { return emptyValue; }
            set { emptyValue = value; }
        }

        public string ValidationGroup
        {
            get { return valRequireCorrectNumber.ValidationGroup; }
            set { valRequireCorrectNumber.ValidationGroup = valRequireNumber.ValidationGroup = value; }
        }

        public bool Enabled
        {
            get { return txtValue.Enabled; }
            set
            {
                txtValue.Enabled = value;
                valRequireCorrectNumber.Enabled = value;
            }
        }

        public bool RequireValidatorEnabled
        {
            get { return valRequireNumber.Enabled; }
            set { valRequireNumber.Enabled = value; }
        }

        public int ValueKB
        {
            get
            {
                string val = txtValue.Text.Trim();
                return val == "" ? emptyValue : Utils.ParseInt(val, 0);
            }
            set
            {
                txtValue.Text = value == emptyValue ? "" : value.ToString();
            }
        }

        public bool DisplayUnitsKB
        {
            get { return locKB.Visible; }
            set {
                locKB.Visible = value;
            }
        }

        public bool DisplayUnitsMB
        {
            get { return locMB.Visible; }
            set {
                locMB.Visible = value;
            }
        }

        public bool DisplayUnitsPct
        {
            get { return locPct.Visible; }
            set { 
                
                locPct.Visible = value;
                }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (locPct.Visible)
            {
                valRequireCorrectNumber.ValidationExpression = @"(^100)$|^([0-9]{1,2})$";
            }
            else
                valRequireCorrectNumber.ValidationExpression = @"[0-9]{0,15}";

        }
    }
}
