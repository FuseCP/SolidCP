using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.HostedSolution;

namespace SolidCP.Portal.ExchangeServer
{
    public partial class OrganizationSettingsPasswordSettings : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Organization org = ES.Services.Organizations.GetOrganization(PanelRequest.ItemID);
                litOrganizationName.Text = org.OrganizationId;

                BindSettings();
            }

        }

        private void BindSettings()
        {
            var settings = ES.Services.Organizations.GetOrganizationPasswordSettings(PanelRequest.ItemID);

            if (settings == null)
            {
                var defaultSettings = ES.Services.Users.GetUserSettings(PanelSecurity.LoggedUserId, UserSettings.EXCHANGE_POLICY);

                BindDefaultSettings(defaultSettings[UserSettings.HOSTED_ORGANIZATION_PASSWORD_POLICY]);
            }
            else
            {
                BindSettings(settings);
            }

            ToggleLockoutControls(chkLockOutSettigns.Checked);
            ToggleComplexityControls(chkPasswordComplexity.Checked);
        }

        private void BindDefaultSettings(string defaultSettings)
        {
            // parse settings
            string[] parts = defaultSettings.Split(';');
            txtMinimumLength.Text = parts[1];
            txtMaximumLength.Text = parts[2];
            txtMinimumUppercase.Text = parts[3];
            txtMinimumNumbers.Text = parts[4];
            txtMinimumSymbols.Text = parts[5];
            chkNotEqualUsername.Checked = Utils.ParseBool(parts[6], false);
            txtLockedOut.Text = parts[7];

            txtEnforcePasswordHistory.Text = PasswordPolicyEditor.GetValueSafe(parts, 8, "0");
            txtAccountLockoutDuration.Text = PasswordPolicyEditor.GetValueSafe(parts, 9, "0");
            txtResetAccountLockout.Text = PasswordPolicyEditor.GetValueSafe(parts, 10, "0");
            chkLockOutSettigns.Checked = PasswordPolicyEditor.GetValueSafe(parts, 11, false);
            chkPasswordComplexity.Checked = PasswordPolicyEditor.GetValueSafe(parts, 12, true);

            txtMaxPasswordAge.Text = PasswordPolicyEditor.GetValueSafe(parts, 13, "42");
        }

        private void BindSettings(OrganizationPasswordSettings settings)
        {
            txtMinimumLength.Text = settings.MinimumLength.ToString();
            txtMaximumLength.Text = settings.MaximumLength.ToString();
            txtMinimumUppercase.Text = settings.UppercaseLettersCount.ToString();
            txtMinimumNumbers.Text = settings.NumbersCount.ToString();
            txtMinimumSymbols.Text = settings.SymbolsCount.ToString();
            txtLockedOut.Text = settings.AccountLockoutThreshold.ToString();

            txtEnforcePasswordHistory.Text = settings.EnforcePasswordHistory.ToString();
            txtAccountLockoutDuration.Text = settings.AccountLockoutDuration.ToString();
            txtResetAccountLockout.Text = settings.ResetAccountLockoutCounterAfter.ToString();
            chkLockOutSettigns.Checked = settings.LockoutSettingsEnabled;
            chkPasswordComplexity.Checked = settings.PasswordComplexityEnabled;

            txtMaxPasswordAge.Text = settings.MaxPasswordAge.ToString();
        }

        private OrganizationPasswordSettings GetSettings()
        {
            var settings = new OrganizationPasswordSettings();

            settings.MinimumLength = Utils.ParseInt(txtMinimumLength.Text, 3);
            settings.MaximumLength = Utils.ParseInt(txtMaximumLength.Text, 7);
            settings.UppercaseLettersCount = Utils.ParseInt(txtMinimumUppercase.Text, 3);
            settings.NumbersCount = Utils.ParseInt(txtMinimumNumbers.Text, 3);
            settings.SymbolsCount = Utils.ParseInt(txtMinimumSymbols.Text, 3);
            settings.AccountLockoutThreshold = Utils.ParseInt(txtLockedOut.Text, 3);
            settings.EnforcePasswordHistory = Utils.ParseInt(txtEnforcePasswordHistory.Text, 3);
            settings.AccountLockoutDuration = Utils.ParseInt(txtAccountLockoutDuration.Text, 3);
            settings.ResetAccountLockoutCounterAfter = Utils.ParseInt(txtResetAccountLockout.Text, 3);

            settings.LockoutSettingsEnabled = chkLockOutSettigns.Checked;
            settings.PasswordComplexityEnabled =chkPasswordComplexity.Checked;

            settings.MaxPasswordAge = Utils.ParseInt(txtMaxPasswordAge.Text, 42);

            return settings;
        }


        private bool SavePasswordSettings()
        {
            try
            {
                ES.Services.Organizations.UpdateOrganizationPasswordSettings(PanelRequest.ItemID, GetSettings());
            }
            catch (Exception ex)
            {
                ShowErrorMessage("ORANIZATIONSETTINGS_NOT_UPDATED", ex);
                return false;
            }

            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            SavePasswordSettings();
        }

        private void ToggleLockoutControls(bool visible)
        {
            RowAccountLockoutDuration.Visible = visible;
            RowLockedOut.Visible = visible;
            RowResetAccountLockout.Visible = visible;
        }

        protected void chkLockOutSettigns_CheckedChanged(object sender, EventArgs e)
        {
            ToggleLockoutControls(chkLockOutSettigns.Checked);
        }

        private void ToggleComplexityControls(bool visible)
        {
            RowMinimumUppercase.Visible = visible;
            RowMinimumNumbers.Visible = visible;
            RowMinimumSymbols.Visible = visible;
        }

        protected void chkPasswordComplexity_CheckedChanged(object sender, EventArgs e)
        {
            ToggleComplexityControls(chkPasswordComplexity.Checked);
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            if (SavePasswordSettings())
            {
                Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "organization_home", "SpaceID=" + PanelSecurity.PackageId));
            }
        }
    }
}