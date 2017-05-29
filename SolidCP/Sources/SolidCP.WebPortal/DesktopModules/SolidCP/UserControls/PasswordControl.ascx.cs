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
using System.Configuration.Internal;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SolidCP.EnterpriseServer;
using SolidCP.Providers.HostedSolution;

namespace SolidCP.Portal
{
    public partial class PasswordControl : SolidCPControlBase
    {
        public const string EMPTY_PASSWORD = "";
        public const int MIN_PASSWORD_LENGTH = 1;

        public bool ValidationEnabled
        {
            get { return (ViewState["ValidationEnabled"] != null) ? (bool)ViewState["ValidationEnabled"] : true; }
            set { ViewState["ValidationEnabled"] = value; ToggleControls(); }
        }

        public bool AllowGeneratePassword
        {
            get { return lnkGenerate.Visible; }
            set { lnkGenerate.Visible = value;  }
        }

        public string GetRandomPasswordUrl()
        {
            return Page.ClientScript.GetWebResourceUrl(
                typeof(PasswordControl), "SolidCP.Portal.Scripts.RandomPassword.js");
        }

        public string ValidationGroup
        {
            get
            {
                return valRequirePassword.ValidationGroup;
            }
            set
            {
                valRequirePassword.ValidationGroup = value;
                valRequireConfirmPassword.ValidationGroup = value;
                valRequireEqualPassword.ValidationGroup = value;
                valCorrectLength.ValidationGroup = value;
                valRequireNumbers.ValidationGroup = value;
                valRequireUppercase.ValidationGroup = value;
                valRequireSymbols.ValidationGroup = value;
            }
        }

        public bool EditMode
        {
            get { return (ViewState["EditMode"] != null) ? (bool)ViewState["EditMode"] : false; }
            set { ViewState["EditMode"] = value; ToggleControls(); }
        }

        public string Password
        {
            get { return (txtPassword.Text == EMPTY_PASSWORD) ? "" : txtPassword.Text; }
            set { txtPassword.Text = value; }
        }

        public bool CheckPasswordLength
        {
            get { return (ViewState["CheckPasswordLength"] != null) ? (bool)ViewState["CheckPasswordLength"] : true; }
            set { ViewState["CheckPasswordLength"] = value; ToggleControls(); }
        }

        public int MinimumLength
        {
            get { return (ViewState["MinimumLength"] != null) ? (int)ViewState["MinimumLength"] : 0; }
            set { ViewState["MinimumLength"] = value; }
        }

        public int MaximumLength
        {
            get { return (ViewState["MaximumLength"] != null) ? (int)ViewState["MaximumLength"] : 0; }
            set
            {
                {
                    txtPassword.MaxLength = value;
                    txtConfirmPassword.MaxLength = value;
                    ViewState["MaximumLength"] = value;
                }
            }
        }

        public int MinimumNumbers
        {
            get { return (ViewState["MinimumNumbers"] != null) ? (int)ViewState["MinimumNumbers"] : 0; }
            set { ViewState["MinimumNumbers"] = value; }
        }

        public int MinimumUppercase
        {
            get { return (ViewState["MinimumUppercase"] != null) ? (int)ViewState["MinimumUppercase"] : 0; }
            set { ViewState["MinimumUppercase"] = value; }
        }

        public int MinimumSymbols
        {
            get { return (ViewState["MinimumSymbols"] != null) ? (int)ViewState["MinimumSymbols"] : 0; }
            set { ViewState["MinimumSymbols"] = value; }
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
                //UserSettings settings = UsersHelper.GetCachedUserSettings(userId, settingsName);
                //EP 2009/09/15: Removed caching for user policy as it was confusing users
                UserSettings settings = ES.Services.Users.GetUserSettings(userId, settingsName);             
                
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

        public void SetUserPolicy(OrganizationPasswordSettings settings)
        {
            int minimumUppercase;
            int minimumNumbers;
            int minimumSymbols;


            if (settings.PasswordComplexityEnabled)
            {
                minimumUppercase = settings.UppercaseLettersCount;
                minimumNumbers = settings.NumbersCount;
                minimumSymbols = settings.SymbolsCount;
            }
            else
            {
                minimumUppercase = 0;
                minimumNumbers = 0;
                minimumSymbols = 0;
            }

            PolicyValue = string.Join(";", true, settings.MinimumLength, settings.MaximumLength, minimumUppercase, minimumNumbers, minimumSymbols, true);

            ToggleControls();
        }

        public void SetUserPolicy(bool enabled, int minLength, int maxLength, int minimumUppercase, int minimumNumbers, int minimumSymbols, bool notEqualToUsername)
        {
            PolicyValue = string.Join(";", enabled, minLength, maxLength, minimumUppercase, minimumNumbers, minimumSymbols, notEqualToUsername);

            ToggleControls();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            txtPassword.Attributes["value"] = txtPassword.Text;
            txtConfirmPassword.Attributes["value"] = txtConfirmPassword.Text;
            
            ToggleControls();
        }

        
        protected override void OnPreRender(EventArgs e)
        {
            RenderValidationJavaScript();
        }

        private void RenderValidationJavaScript()
        {
            if (!Page.ClientScript.IsClientScriptIncludeRegistered("scpValidationFunctions"))
            {
              Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "scpValidationFunctions", @"
                
                function scpValidatePasswordNumbers(source, args)
                {
                    if(args.Value == source.getAttribute('dpsw')) return true;
                    args.IsValid = scpValidatePattern(/(\d)/g, args.Value,
                        parseInt(source.getAttribute('minimumNumber')));
                }

                function scpValidatePasswordUppercase(source, args)
                {
                    if(args.Value == source.getAttribute('dpsw')) return true;
                    args.IsValid = scpValidatePattern(/([A-Z])/g, args.Value,
                        parseInt(source.getAttribute('minimumNumber')));
                }

                function scpValidatePasswordSymbols(source, args)
                {
                    if(args.Value == source.getAttribute('dpsw')) return true;
                    args.IsValid = scpValidatePattern(/([\W_])/g, args.Value,
                        parseInt(source.getAttribute('minimumNumber')));
                }

                function scpValidatePasswordLength(source, args)
                {
                    if(args.Value == source.getAttribute('dpsw')) return true;
                    args.IsValid = (args.Value.length >= parseInt(source.getAttribute('minimumLength')));
                }
                
                function scpValidatePattern(re, val, minMatches)
                {
                    var matches = val.match(re);
                    return ((matches != null) && matches.length >= minMatches);
                }
                ", true);


            }

            if (!Page.ClientScript.IsClientScriptIncludeRegistered("jqueryui-tooltip"))
            {
                  Page.ClientScript.RegisterClientScriptInclude(this.GetType(),"jqueryui-tooltip", ResolveUrl("~/JavaScript/jquery.poshytip.min.js"));
            }
        }

        private void ToggleControls()
        {
            valCorrectLength.Attributes["dpsw"] = EMPTY_PASSWORD;
            valRequireNumbers.Attributes["dpsw"] = EMPTY_PASSWORD;
            valRequireUppercase.Attributes["dpsw"] = EMPTY_PASSWORD;
            valRequireSymbols.Attributes["dpsw"] = EMPTY_PASSWORD;

            // set empty password
            if (txtPassword.Text == "" && EditMode)
            {
                txtPassword.Attributes["value"] = EMPTY_PASSWORD;
                txtConfirmPassword.Attributes["value"] = EMPTY_PASSWORD;
            }

            // enable/disable require validators
            valRequirePassword.Enabled = ValidationEnabled;
            valRequireConfirmPassword.Enabled = ValidationEnabled;

            // require default length
            MinimumLength = Math.Max(MIN_PASSWORD_LENGTH, MinimumLength);

            // parse and enforce policy
            if (PolicyValue != null)
            {
                bool enabled = false;
                int minLength = -1;
                int maxLength = -1;
                bool notEqualToUsername = false;

                try
                {
                    // parse settings
                    string[] parts = PolicyValue.Split(';');
                    enabled = Utils.ParseBool(parts[0], false);
                    minLength = Math.Max(Utils.ParseInt(parts[1], 0), MinimumLength);
                    maxLength = Math.Max(Utils.ParseInt(parts[2], 0), MaximumLength);
                    MinimumUppercase = Math.Max(Utils.ParseInt(parts[3], 0), MinimumUppercase);
                    MinimumNumbers = Math.Max(Utils.ParseInt(parts[4], 0), MinimumNumbers);
                    MinimumSymbols = Math.Max(Utils.ParseInt(parts[5], 0), MinimumSymbols);
                    notEqualToUsername = Utils.ParseBool(parts[6], false);
                }
                catch { /* skip */ }

                // apply policy
                if (enabled)
                {
                    // min length
                    if (minLength > 0)
                    {
                        MinimumLength = minLength;
                        valCorrectLength.Enabled = true;
                        valCorrectLength.Attributes["minimumLength"] = MinimumLength.ToString();
                        valCorrectLength.ErrorMessage = String.Format(GetLocalizedString("CorrectLength.Text"), MinimumLength);
                    }

                    // max length
                    if (maxLength > 0)
                    {
                        MaximumLength = maxLength;
                        txtPassword.MaxLength = maxLength;
                        txtConfirmPassword.MaxLength = maxLength;
                    }

                    // numbers
                    if (MinimumNumbers > 0)
                    {
                        valRequireNumbers.Enabled = true;
                        valRequireNumbers.Attributes["minimumNumber"] = MinimumNumbers.ToString();
                        valRequireNumbers.ErrorMessage = String.Format(
                            GetLocalizedString("RequireNumbers.Text"), MinimumNumbers);
                    }

                    // UPPERCASE
                    if (MinimumUppercase > 0)
                    {
                        valRequireUppercase.Enabled = true;
                        valRequireUppercase.Attributes["minimumNumber"] = MinimumUppercase.ToString();
                        valRequireUppercase.ErrorMessage = String.Format(
                            GetLocalizedString("RequireUppercase.Text"), MinimumUppercase);
                    }

                    // symbols
                    if (MinimumSymbols > 0)
                    {
                        valRequireSymbols.Enabled = true;
                        valRequireSymbols.Attributes["minimumNumber"] = MinimumSymbols.ToString();
                        valRequireSymbols.ErrorMessage = String.Format(
                            GetLocalizedString("RequireSymbols.Text"), MinimumSymbols);
                    }

                } // if(enabled)
            } // if (PolicyValue != null)

            // set min password generator
            lnkGenerate.NavigateUrl = String.Format("javascript:GeneratePassword('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
              MaximumLength, MinimumUppercase, MinimumNumbers, MinimumSymbols, txtPassword.ClientID, txtConfirmPassword.ClientID);
            
        }

        protected void valCorrectLength_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ((args.Value == EMPTY_PASSWORD) || (args.Value.Length >= MinimumLength));
        }

        protected void valRequireNumbers_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ((args.Value == EMPTY_PASSWORD) || ValidatePattern("(\\d)", args.Value, MinimumNumbers));
        }

        protected void valRequireUppercase_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ((args.Value == EMPTY_PASSWORD) || ValidatePattern("([A-Z])", args.Value, MinimumUppercase));
        }

        protected void valRequireSymbols_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ((args.Value == EMPTY_PASSWORD) || ValidatePattern("([\\W_])", args.Value, MinimumSymbols));
        }

        private bool ValidatePattern(string regexp, string val, int minimumNumber)
        {
            return (Regex.Matches(val, regexp).Count >= minimumNumber);
        }
    }
}
