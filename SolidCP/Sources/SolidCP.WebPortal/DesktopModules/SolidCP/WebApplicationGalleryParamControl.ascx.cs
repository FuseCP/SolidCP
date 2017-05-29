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
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.Providers.WebAppGallery;

namespace SolidCP.Portal
{
    public partial class WebApplicationGalleryParamControl : SolidCPModuleBase
    {
        #region Constants
        public const string DatabaseIdentifierRegexp = "^[a-zA-Z_]+[a-zA-Z0-9_]{0,63}$";
        #endregion

        #region Private Properties
        DeploymentParameterWellKnownTag wellKnownTags
        {
            get { return ViewState["WellKnownTags"] != null ? (DeploymentParameterWellKnownTag)ViewState["WellKnownTags"] : DeploymentParameterWellKnownTag.None; }
            set { ViewState["WellKnownTags"] = value; }
        }

        DeploymentParameterValidationKind validationKind
        {
            get { return ViewState["ValidationKind"] != null ? (DeploymentParameterValidationKind)ViewState["ValidationKind"] : DeploymentParameterValidationKind.None; }
            set { ViewState["ValidationKind"] = value; }
        }

        string validationString
        {
            get { return ViewState["ValidationString"] != null ? (string)ViewState["ValidationString"] : null; }
            set { ViewState["ValidationString"] = value; }
        }
        #endregion

        #region Public Properties
        public string Name
        {
            get { return ViewState["Name"] != null ? (string)ViewState["Name"] : null; }
            private set { ViewState["Name"] = value; }
        }

        public string FriendlyName
        {
            get { return friendlyName.Text; }
            private set { friendlyName.Text = value; }
        }

        public string Description
        {
            get { return description.Text; }
            private set { description.Text = value; }
        }

        public string DefaultValue
        {
            get { return ViewState["DefaultValue"] != null ? (string)ViewState["DefaultValue"] : null; }
            private set { ViewState["DefaultValue"] = value; }
        }

        public string ValuePrefix
        {
            get { return ViewState["ValuePrefix"] != null ? (string)ViewState["ValuePrefix"] : null; }
            private set { ViewState["ValuePrefix"] = value; }
        }

        public string ValueSuffix
        {
            get { return ViewState["ValueSuffix"] != null ? (string)ViewState["ValueSuffix"] : null; }
            private set { ViewState["ValueSuffix"] = value; }
        }

        public DeploymentParameterWellKnownTag WellKnownTags
        {
            get { return this.wellKnownTags; }
            set { wellKnownTags = value; BindControls(); }
        }

        public DeploymentParameterValidationKind ValidationKind
        {
            get { return this.validationKind; }
            set { validationKind = value; BindControls(); }
        }

        public string ValidationString
        {
            get { return this.validationString; }
            set { validationString = value; BindControls(); }
        }

        public string Value
        {
            get { return null; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindParameter(DeploymentParameter param)
        {
            // store parameter details
            this.Name = param.Name;
            this.FriendlyName = param.FriendlyName;
            this.Description = param.Description;
            this.DefaultValue = param.DefaultValue;
            this.wellKnownTags = param.WellKnownTags;
            this.validationKind = param.ValidationKind;
            this.validationString = param.ValidationString;
            this.ValuePrefix = param.ValuePrefix;
            this.ValueSuffix = param.ValueSuffix;

            // toggle controls
            BindControls();
        }

        public void SetTextValuePrefixSuffix(string prefix, string suffix)
        {
            if (!string.IsNullOrEmpty(prefix))
            {
                valPrefix.Text = prefix;
            }
            if (!string.IsNullOrEmpty(suffix))
            {
                valSuffix.Text = suffix;
            }
        }

        public DeploymentParameter GetParameter()
        {
            DeploymentParameter parameter = new DeploymentParameter();
            parameter.Name = this.Name;
            parameter.FriendlyName = this.FriendlyName;
            parameter.Description = this.Description;
            parameter.Value = GetParameterValue();
            parameter.DefaultValue = this.DefaultValue;
            parameter.WellKnownTags = this.WellKnownTags;
            parameter.ValidationKind = this.ValidationKind;
            parameter.ValidationString = this.ValidationString;
            return parameter;
        }

        private string GetParameterValue()
        {
            if (PasswordControl.Visible)
                return password.Text;
            else if (TextControl.Visible)
                return valPrefix.Text + textValue.Text.Trim() + valSuffix.Text;
            else if (BooleanControl.Visible)
                return boolValue.Checked.ToString();
            else if (EnumControl.Visible)
                return enumValue.SelectedValue;
            else
                return null;
        }

        private void BindControls()
        {
            try
            {

                // hide database server parameters
                DeploymentParameterWellKnownTag hiddenTags =
                    DeploymentParameterWellKnownTag.IisApp |
                    DeploymentParameterWellKnownTag.Hidden |
                    DeploymentParameterWellKnownTag.DBServer |
                    DeploymentParameterWellKnownTag.DBAdminUserName |
                    DeploymentParameterWellKnownTag.DBAdminPassword;

                if ((WellKnownTags & hiddenTags) > 0)
                {
                    this.Visible = false;
                    return;
                }

                // disable all editor controls
                BooleanControl.Visible = false;
                EnumControl.Visible = false;
                PasswordControl.Visible = false;
                TextControl.Visible = false;

                // enable specific control
                if ((ValidationKind & DeploymentParameterValidationKind.Boolean) == DeploymentParameterValidationKind.Boolean)
                {
                    // Boolean value
                    BooleanControl.Visible = true;
                    bool val = false;
                    Boolean.TryParse(DefaultValue, out val);
                    boolValue.Checked = val;
                }
                else if ((ValidationKind & DeploymentParameterValidationKind.Enumeration) == DeploymentParameterValidationKind.Enumeration)
                {
                    // Enumeration value
                    EnumControl.Visible = true;

                    // fill dropdown
                    enumValue.Items.Clear();
                    string[] items = (ValidationString ?? "").Trim().Split(',');
                    foreach (string item in items)
                        enumValue.Items.Add(item.Trim());

                    // select default value
                    enumValue.SelectedValue = DefaultValue;
                }
                else if ((WellKnownTags & DeploymentParameterWellKnownTag.Password) == DeploymentParameterWellKnownTag.Password)
                {
                    // Password value
                    PasswordControl.Visible = true;
                    confirmPasswordControls.Visible = ((WellKnownTags & DeploymentParameterWellKnownTag.New) == DeploymentParameterWellKnownTag.New);
                }
                else
                {
                    // Text value
                    TextControl.Visible = true;
                    textValue.Text = DefaultValue == null ? "" : DefaultValue;
                    valPrefix.Text = ValuePrefix == null ? "" : ValuePrefix;
                    valSuffix.Text = ValueSuffix == null ? "" : ValueSuffix;

                    if (
                        (ValuePrefix != null) && (ValueSuffix != null)
                        &&
                        ((WellKnownTags & DeploymentParameterWellKnownTag.MySql) == DeploymentParameterWellKnownTag.MySql)
                        &&
                        ((WellKnownTags & DeploymentParameterWellKnownTag.DBUserName) == DeploymentParameterWellKnownTag.DBUserName)
                        )
                    {
                        MysqlUsernameLengthValidator.Enabled = true;
                    }
                }

                // enforce validation for database parameters if they are allowed empty by app pack developers
                bool isDatabaseParameter = (WellKnownTags & (
                    DeploymentParameterWellKnownTag.DBName |
                    DeploymentParameterWellKnownTag.DBUserName |
                    DeploymentParameterWellKnownTag.DBUserPassword)) > 0;

                // enforce validation for database name and username
                if ((WellKnownTags & (DeploymentParameterWellKnownTag.DBName | DeploymentParameterWellKnownTag.DBUserName)) > 0
                        && String.IsNullOrEmpty(ValidationString))
                {
                    validationKind |= DeploymentParameterValidationKind.RegularExpression;
                    validationString = DatabaseIdentifierRegexp;
                }

                // validation common for all editors
                requireTextValue.Enabled = requirePassword.Enabled = requireConfirmPassword.Enabled = requireEnumValue.Enabled =
                    ((ValidationKind & DeploymentParameterValidationKind.AllowEmpty) != DeploymentParameterValidationKind.AllowEmpty) || isDatabaseParameter;

                requireTextValue.Text = requirePassword.Text = requireEnumValue.Text =
                    String.Format(GetLocalizedString("RequiredValidator.Text"), FriendlyName);

                regexpTextValue.Enabled = regexpPassword.Enabled = regexpEnumValue.Enabled =
                    (ValidationKind & DeploymentParameterValidationKind.RegularExpression) == DeploymentParameterValidationKind.RegularExpression;

                regexpTextValue.ValidationExpression = regexpPassword.ValidationExpression = regexpEnumValue.ValidationExpression =
                    ValidationString ?? "";

                regexpTextValue.Text = regexpPassword.Text = regexpEnumValue.Text =
                    String.Format(GetLocalizedString("RegexpValidator.Text"), FriendlyName, ValidationString);


            }
            catch { } // just skip

        }

        protected void mysqlUsernameLen_OnServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            
            // get entered database name with prefixes / suffixes
            string value = GetParameterValue();

            // check length
            if (!string.IsNullOrEmpty(value) && value.Length <= 16)
                return;

            // validation failed
            args.IsValid = false;
        }
        protected void mariadbUsernameLen_OnServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;

            // get entered database name with prefixes / suffixes
            string value = GetParameterValue();

            // check length
            if (!string.IsNullOrEmpty(value) && value.Length <= 16)
                return;

            // validation failed
            args.IsValid = false;
        }
    }
}
