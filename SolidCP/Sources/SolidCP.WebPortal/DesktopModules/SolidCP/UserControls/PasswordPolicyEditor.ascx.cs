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
using System.Text;
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
    public partial class PasswordPolicyEditor : SolidCPControlBase
    {
        public bool ShowLockoutSettings { get; set; }

        public string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(chkEnabled.Checked.ToString()).Append(";");
                sb.Append(txtMinimumLength.Text).Append(";");
                sb.Append(txtMaximumLength.Text).Append(";");
                sb.Append(txtMinimumUppercase.Text).Append(";");
                sb.Append(txtMinimumNumbers.Text).Append(";");
                sb.Append(txtMinimumSymbols.Text).Append(";");
                sb.Append(chkNotEqualUsername.Checked.ToString()).Append(";");
                sb.Append(txtLockedOut.Text).Append(";");

                sb.Append(txtEnforcePasswordHistory.Text).Append(";");
                sb.Append(txtAccountLockoutDuration.Text).Append(";");
                sb.Append(txtResetAccountLockout.Text).Append(";");
                sb.Append(chkLockOutSettigns.Checked.ToString()).Append(";");
                sb.Append(chkPasswordComplexity.Checked.ToString()).Append(";");

                sb.Append(txtMaxPasswordAge.Text).Append(";");

                return sb.ToString();
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    // bind default settings
                    chkEnabled.Checked = false;
                    txtMinimumLength.Text = "3";
                    txtMaximumLength.Text = "10";
                    txtMinimumUppercase.Text = "0";
                    txtMinimumNumbers.Text = "0";
                    txtMinimumSymbols.Text = "0";
                    txtLockedOut.Text = "3";
                    chkPasswordComplexity.Checked = true;
                    txtMaxPasswordAge.Text = "42";
                }
                else
                {
                    try
                    {
                        // parse settings
                        string[] parts = value.Split(';');
                        chkEnabled.Checked = Utils.ParseBool(parts[0], false);
                        txtMinimumLength.Text = parts[1];
                        txtMaximumLength.Text = parts[2];
                        txtMinimumUppercase.Text = parts[3];
                        txtMinimumNumbers.Text = parts[4];
                        txtMinimumSymbols.Text = parts[5];
                        chkNotEqualUsername.Checked = Utils.ParseBool(parts[6], false);
                        txtLockedOut.Text = parts[7];

                        txtEnforcePasswordHistory.Text = GetValueSafe(parts, 8, "0");
                        txtAccountLockoutDuration.Text = GetValueSafe(parts, 9, "0");
                        txtResetAccountLockout.Text = GetValueSafe(parts, 10, "0");
                        chkLockOutSettigns.Checked = GetValueSafe(parts, 11, false) && ShowLockoutSettings;
                        chkPasswordComplexity.Checked = GetValueSafe(parts, 12, true);

                        txtMaxPasswordAge.Text = GetValueSafe(parts, 13, "42");
                    }
                    catch
                    {
                        /* skip */
                    }
                }
                ToggleControls();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void ToggleControls()
        {
            PolicyTable.Visible = chkEnabled.Checked;

            ToggleLockOutSettignsControls();
            TogglePasswordCompelxitySettignsControls();

            RowChkLockOutSettigns.Visible = ShowLockoutSettings;
        }

        protected void chkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        private void ToggleLockOutSettignsControls()
        {
            RowAccountLockoutDuration.Visible = chkLockOutSettigns.Checked;
            RowLockedOut.Visible = chkLockOutSettigns.Checked;
            RowResetAccountLockout.Visible = chkLockOutSettigns.Checked;
        }

        private void TogglePasswordCompelxitySettignsControls()
        {
            RowMinimumUppercase.Visible = chkPasswordComplexity.Checked;
            RowMinimumNumbers.Visible = chkPasswordComplexity.Checked;
            RowMinimumSymbols.Visible = chkPasswordComplexity.Checked;
        }

        protected void chkLockOutSettigns_CheckedChanged(object sender, EventArgs e)
        {
            ToggleLockOutSettignsControls();
        }

        protected void chkPasswordComplexity_CheckedChanged(object sender, EventArgs e)
        {
            TogglePasswordCompelxitySettignsControls();
        }

        public static T GetValueSafe<T>(string[] array, int index, T defaultValue)
        {
            try
            {
                if (array.Length > index)
                {
                    if (string.IsNullOrEmpty(array[index]))
                    {
                        return defaultValue;
                    }

                    return (T)Convert.ChangeType(array[index], typeof(T));
                }
            }
            catch{}

            return defaultValue;
        }

    }
}
