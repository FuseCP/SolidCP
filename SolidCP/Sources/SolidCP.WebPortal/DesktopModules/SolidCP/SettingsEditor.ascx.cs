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

using SolidCP.EnterpriseServer;

namespace SolidCP.Portal
{
    public partial class SettingsEditor : SolidCPModuleBase
    {
        private string SettingsName
        {
            get { return Request["SettingsName"]; }
        }

        IUserSettingsEditorControl ctlSettings;

        protected void Page_Load(object sender, EventArgs e)
        {
            // load settings control
            LoadSettingsControl();

            // entry point
            try
            {
                if (!IsPostBack)
                {
                    // save referrer URL
                    if(Request.UrlReferrer != null)
                        ViewState["ReturnURL"] = Request.UrlReferrer.ToString();

                    // bind settings
                    BindSettings();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("USER_SETTINGS_GET", ex);
                return;
            }
        }

        private void BindSettings()
        {
            // load user settings
            UserSettings settings = ES.Services.Users.GetUserSettings(PanelSecurity.SelectedUserId, SettingsName);

            ddlOverride.SelectedIndex = (settings.UserId == PanelSecurity.SelectedUserId) ? 1 : 0;
            ToggleControls();

            // bind settings
            ctlSettings.BindSettings(settings);
        }

        protected void ddlOverride_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOverride.SelectedIndex == 0) // use host settings
            {
                // delete current settings
                UserSettings settings = new UserSettings();
                settings.UserId = PanelSecurity.SelectedUserId;
                settings.SettingsName = SettingsName;
                ES.Services.Users.UpdateUserSettings(settings);

                // rebind settings
                BindSettings();
            }
            else if (!SettingsName.Equals("ServiceLevels"))
            {
                ToggleControls();
            }
        }

        private void ToggleControls()
        {
			ddlOverride.Enabled = (PanelSecurity.SelectedUser.Role != UserRole.Administrator);

            // check if we should enable controls
            bool enabled = (ddlOverride.SelectedIndex == 1);

            // enable/disable controls
            EnableControlRecursively((Control)ctlSettings, enabled);
        }

        private void EnableControlRecursively(Control ctrl, bool enabled)
        {
            WebControl wc = ctrl as WebControl;
            if (wc != null && !(wc is Label))
                wc.Enabled = enabled;

            // process children
            foreach (Control childCtrl in ctrl.Controls)
                EnableControlRecursively(childCtrl, enabled);
        }

        private void SaveSettings()
        {
            try
            {
                UserSettings settings = new UserSettings();
                settings.UserId = PanelSecurity.SelectedUserId;
                settings.SettingsName = SettingsName;

                

                // set properties
                if (ddlOverride.SelectedIndex == 1)
                {
                    // gather settings from the control
                    // if overriden
                    ctlSettings.SaveSettings(settings);

                    // check settings
                    foreach (Control control in settingsPlace.Controls)
                    {
                        string[] parts;
                        if (control is SettingsSolidCPPolicy)
                        {
                            if (!PasswordPolicyValidation(settings["PasswordPolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }
                        }
                        if (control is SettingsWebPolicy)
                        {
                            if (!UserNamePolicyValidation(settings["AnonymousAccountPolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }

                            if (!UserNamePolicyValidation(settings["VirtDirNamePolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }

                            if (!UserNamePolicyValidation(settings["FrontPageAccountPolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }

                            if (!PasswordPolicyValidation(settings["FrontPagePasswordPolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }

                            if (!UserNamePolicyValidation(settings["SecuredUserNamePolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }

                            if (!PasswordPolicyValidation(settings["SecuredUserPasswordPolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }

                            if (!UserNamePolicyValidation(settings["SecuredGroupNamePolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }
                        }
                        if (control is SettingsFtpPolicy)
                        {
                            if (!UserNamePolicyValidation(settings["UserNamePolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }

                            if (!PasswordPolicyValidation(settings["UserPasswordPolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }
                        }

                        if (control is SettingsMailPolicy)
                        {
                            if (!UserNamePolicyValidation(settings["AccountNamePolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }

                            if (!PasswordPolicyValidation(settings["AccountPasswordPolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }
                        }

                        if (control is SettingsMsSqlPolicy)
                        {
                            if (!UserNamePolicyValidation(settings["DatabaseNamePolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }

                            if (!UserNamePolicyValidation(settings["UserNamePolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }

                            if (!PasswordPolicyValidation(settings["UserPasswordPolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }
                        }

                        if (control is SettingsMySqlPolicy)
                        {
                            if (!UserNamePolicyValidation(settings["DatabaseNamePolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }

                            if (!UserNamePolicyValidation(settings["UserNamePolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }
                            parts = settings["UserNamePolicy"].Split(';');

                            if (Utils.ParseInt(parts[3]) > 40)
                            {
                                ShowWarningMessage("MySQL_USERNAME_MAX_LENGTH");
                                return;
                            }

                            if (!PasswordPolicyValidation(settings["UserPasswordPolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }
                        }

                        if (control is SettingsMariaDBPolicy)
                        {
                            if (!UserNamePolicyValidation(settings["DatabaseNamePolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }

                            if (!UserNamePolicyValidation(settings["UserNamePolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }
                            parts = settings["UserNamePolicy"].Split(';');

                            if (Utils.ParseInt(parts[3]) > 40)
                            {
                                ShowWarningMessage("MariaDB_USERNAME_MAX_LENGTH");
                                return;
                            }

                            if (!PasswordPolicyValidation(settings["UserPasswordPolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }
                        }

                        if (control is SettingsSharePointPolicy)
                        {
                            if (!UserNamePolicyValidation(settings["GroupNamePolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }

                            if (!UserNamePolicyValidation(settings["UserNamePolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }

                            if (!PasswordPolicyValidation(settings["UserPasswordPolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }
                        }

                        if (control is SettingsOperatingSystemPolicy)
                        {
                            if (!UserNamePolicyValidation(settings["DsnNamePolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }
                        }

                        if (control is SettingsExchangePolicy)
                        {
                            if (!PasswordPolicyValidation(settings["MailboxPasswordPolicy"]))
                            {
                                ShowWarningMessage("WRONG_POLICIES_VALUES");
                                return;
                            }
                        }
                    }
                } // end if overriden

                int result = ES.Services.Users.UpdateUserSettings(settings);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                RedirectBack();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("USER_SETTINGS_UPDATE", ex);
                return;
            }
        }

        private static bool PasswordPolicyValidation(string passwordPolicy)
        {
            string[] parts = passwordPolicy.Split(';');
            return (Utils.ParseInt(parts[1]) < Utils.ParseInt(parts[2])
                            && Utils.ParseInt(parts[3]) < Utils.ParseInt(parts[2])
                            && Utils.ParseInt(parts[4]) < Utils.ParseInt(parts[2])
                            && Utils.ParseInt(parts[5]) < Utils.ParseInt(parts[2])
                            && (Utils.ParseInt(parts[3]) + Utils.ParseInt(parts[4]) + Utils.ParseInt(parts[5])) <= Utils.ParseInt(parts[2]));
        }

        private static bool UserNamePolicyValidation(string usernamePolicy)
        {
            string[] parts = usernamePolicy.Split(';');
            return (Utils.ParseInt(parts[2]) < Utils.ParseInt(parts[3]));
        }

        private void LoadSettingsControl()
        {
            string controlName = Request["SettingsControl"];
            if (!String.IsNullOrEmpty(controlName))
            {
                string currPath = this.AppRelativeVirtualPath;
                currPath = currPath.Substring(0, currPath.LastIndexOf("/"));
                string ctrlPath = currPath + "/" + controlName + ".ascx";

                Control ctrl = Page.LoadControl(ctrlPath);
                ctlSettings = (IUserSettingsEditorControl)ctrl;
                settingsPlace.Controls.Add(ctrl);
            }
            else
            {
                RedirectBack();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectBack();
        }

        private void RedirectBack()
        {
            if (ViewState["ReturnURL"] != null)
                Response.Redirect((string)ViewState["ReturnURL"]);
            else
                RedirectToBrowsePage();
        }
    }
}
