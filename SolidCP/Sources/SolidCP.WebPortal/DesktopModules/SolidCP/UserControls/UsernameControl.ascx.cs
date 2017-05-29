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
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal
{
    public partial class UsernameControl : SolidCPControlBase
    {
        public Unit Width
        {
            get { return txtName.Width; }
            set { txtName.Width = value; }
        }

        public string ValidationGroup
        {
            get
            {
                return valRequireUsername.ValidationGroup;
            }
            set
            {
                valRequireUsername.ValidationGroup = value;
                valCorrectUsername.ValidationGroup = value;
                valCorrectMinLength.ValidationGroup = value;
            }
        }

        public bool EditMode
        {
            get { return (ViewState["EditMode"] != null) ? (bool)ViewState["EditMode"] : false; }
            set { ViewState["EditMode"] = value; ToggleControls(); }
        }

        public bool RequiredField
        {
            get { return (ViewState["RequiredField"] != null) ? (bool)ViewState["RequiredField"] : true; }
            set { ViewState["RequiredField"] = value; ToggleControls(); }
        }

        public string Text
        {
            get { return EditMode ? txtName.Text.Trim() : litPrefix.Text + txtName.Text.Trim() + litSuffix.Text; }
            set { txtName.Text = value; lblName.Text = PortalAntiXSS.Encode(value); }
        }

        private UserInfo PolicyUser
        {
            get { return (ViewState["PolicyUser"] != null) ? (UserInfo)ViewState["PolicyUser"] : null; }
            set { ViewState["PolicyUser"] = value; }
        }

        private string PolicyValue
        {
            get { return (ViewState["PolicyValue"] != null) ? (string)ViewState["PolicyValue"] : null; }
            set { ViewState["PolicyValue"] = value; }
        }

        public void SetPackagePolicy(int packageId, string settingsName, string key)
        {
            // load package
            PackageInfo package = PackagesHelper.GetCachedPackage(packageId);
            if (package != null)
            {
                // init by user
                SetUserPolicy(package.UserId, settingsName, key);
            }
        }

        public void SetUserPolicy(int userId, string settingsName, string key)
        {
            // load user profile
            UserInfo user = UsersHelper.GetCachedUser(userId);

            if (user != null)
            {
                PolicyUser = user;

                // load settings
                UserSettings settings = UsersHelper.GetCachedUserSettings(userId, settingsName);
                if (settings != null)
                {
                    string policyValue = settings[key];
                    if (policyValue != null)
                        PolicyValue = policyValue;
                }
            }

            // toggle controls
            ToggleControls();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ToggleControls();
        }

        private void ToggleControls()
        {
            // hide/show controls
            litPrefix.Visible = ((!EditMode) && !String.IsNullOrEmpty(litPrefix.Text));
            txtName.Visible = !EditMode;
            lblName.Visible = EditMode;
            litSuffix.Visible = ((!EditMode) && !String.IsNullOrEmpty(litSuffix.Text));
            valRequireUsername.Enabled = RequiredField && !EditMode;
            valCorrectUsername.Enabled = !EditMode;
            valCorrectMinLength.Enabled = !EditMode;

            if (EditMode)
                return;

            // require validator
            valRequireUsername.ErrorMessage = GetLocalizedString("CantBeBlank.Text");

            // disable min length validator
            valCorrectMinLength.Enabled = false;

            // username validator
            string defAllowedRegexp = PanelGlobals.UsernameDefaultAllowedRegExp;
            string defAllowedText = "a-z&nbsp;&nbsp;A-Z&nbsp;&nbsp;0-9&nbsp;&nbsp;.&nbsp;&nbsp;_";

            // parse and enforce policy
            if (PolicyValue != null)
            {
                bool enabled = false;
                string allowedSymbols = null;
                int minLength = -1;
                int maxLength = -1;
                string prefix = null;
                string suffix = null;

                try
                {
                    // parse settings
                    string[] parts = PolicyValue.Split(';');
                    enabled = Utils.ParseBool(parts[0], false);
                    allowedSymbols = parts[1];
                    minLength = Utils.ParseInt(parts[2], -1);
                    maxLength = Utils.ParseInt(parts[3], -1);
                    prefix = parts[4];
                    suffix = parts[5];
                }
                catch { /* skip */ }

                // apply policy
                if (enabled)
                {
                    // prefix
                    if (!String.IsNullOrEmpty(prefix))
                    {
                        // substitute vars
                        prefix = Utils.ReplaceStringVariable(prefix, "user_id", PolicyUser.UserId.ToString());
                        prefix = Utils.ReplaceStringVariable(prefix, "user_name", PolicyUser.Username);

                        // display
                        litPrefix.Text = prefix;

                        // adjust max length
                        maxLength -= prefix.Length;
                    }

                    // suffix
                    if (!String.IsNullOrEmpty(suffix))
                    {
                        // substitute vars
                        suffix = Utils.ReplaceStringVariable(suffix, "user_id", PolicyUser.UserId.ToString());
                        suffix = Utils.ReplaceStringVariable(suffix, "user_name", PolicyUser.Username);

                        // display
                        litSuffix.Text = suffix;

                        // adjust max length
                        maxLength -= suffix.Length;
                    }

                    // min length
                    if (minLength > 0)
                    {
                        valCorrectMinLength.Enabled = true;
                        valCorrectMinLength.ValidationExpression = "^.{" + minLength.ToString() + ",}$";
                        valCorrectMinLength.ErrorMessage = String.Format(
                            GetLocalizedString("MinLength.Text"), minLength);
                    }

                    // max length
                    if (maxLength > 0)
                        txtName.MaxLength = maxLength;

                    // process allowed symbols
                    if (!String.IsNullOrEmpty(allowedSymbols))
                    {
                        StringBuilder sb = new StringBuilder(defAllowedRegexp);
                        for (int i = 0; i < allowedSymbols.Length; i++)
                        {
							// Escape characters only if required
							if (PanelGlobals.MetaCharacters2Escape.IndexOf(allowedSymbols[i]) > -1)
								sb.Append(@"\").Append(allowedSymbols[i]);
							else
								sb.Append(allowedSymbols[i]);
                            //
                            defAllowedText += "&nbsp;&nbsp;" + allowedSymbols[i];
                        }
                        defAllowedRegexp = sb.ToString();
                    }

                } // if(enabled)
            } // if (PolicyValue != null)

            valCorrectUsername.ValidationExpression = @"^[" + defAllowedRegexp + @"]*$";
            valCorrectUsername.ErrorMessage = String.Format(GetLocalizedString("AllowedSymbols.Text"),
                defAllowedText);

        } // ToggleControls()
    }
}
