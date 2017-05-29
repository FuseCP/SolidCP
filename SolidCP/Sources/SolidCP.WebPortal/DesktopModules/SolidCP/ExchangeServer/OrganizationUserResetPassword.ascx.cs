using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.Providers.HostedSolution;
using SCP = SolidCP.EnterpriseServer;

namespace SolidCP.Portal.ExchangeServer
{
    public partial class OrganizationUserResetPassword : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSettings();
            }
        }

        private void BindSettings()
        {
            OrganizationUser user = ES.Services.Organizations.GetUserGeneralSettings(PanelRequest.ItemID,
                PanelRequest.AccountID);

            litDisplayName.Text = PortalAntiXSS.Encode(user.DisplayName);

            txtEmailAddress.Text = user.PrimaryEmailAddress;

            txtMobile.Text = user.MobilePhone;

            var settings = ES.Services.System.GetSystemSettingsActive(SCP.SystemSettings.TWILIO_SETTINGS, false);

            bool twilioEnabled = settings != null 
                && !string.IsNullOrEmpty(settings.GetValueOrDefault(SCP.SystemSettings.TWILIO_ACCOUNTSID_KEY, string.Empty))
                && !string.IsNullOrEmpty(settings.GetValueOrDefault(SCP.SystemSettings.TWILIO_AUTHTOKEN_KEY, string.Empty))
                && !string.IsNullOrEmpty(settings.GetValueOrDefault(SCP.SystemSettings.TWILIO_PHONEFROM_KEY, string.Empty));

            rbtnMobile.Visible = twilioEnabled;
        }

        protected void btnResetPassoword_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            if (rbtnEmail.Checked)
            {
                ES.Services.Organizations.SendResetUserPasswordEmail(PanelRequest.ItemID, PanelRequest.AccountID,
                    txtReason.Text, txtEmailAddress.Text, true);
            }
            else
            {
                var result = ES.Services.Organizations.SendResetUserPasswordLinkSms(PanelRequest.ItemID,
                    PanelRequest.AccountID, txtReason.Text, txtMobile.Text);

                if (!result.IsSuccess)
                {
                    ShowErrorMessage("SEND_USER_PASSWORD_RESET_SMS");

                    return;
                }

                if (chkSaveAsMobile.Checked)
                {
                    OrganizationUser user = ES.Services.Organizations.GetUserGeneralSettings(PanelRequest.ItemID,
                        PanelRequest.AccountID);

                    ES.Services.Organizations.SetUserGeneralSettings(
                        PanelRequest.ItemID, PanelRequest.AccountID,
                        user.DisplayName,
                        string.Empty,
                        false,
                        user.Disabled,
                        user.Locked,

                        user.FirstName,
                        user.Initials,
                        user.LastName,

                        user.Address,
                        user.City,
                        user.State,
                        user.Zip,
                        user.Country,

                        user.JobTitle,
                        user.Company,
                        user.Department,
                        user.Office,
                        user.Manager == null ? null : user.Manager.AccountName,

                        user.BusinessPhone,
                        user.Fax,
                        user.HomePhone,
                        txtMobile.Text,
                        user.Pager,
                        user.WebPage,
                        user.Notes,
                        user.ExternalEmail,
                        user.SubscriberNumber,
                        user.LevelId,
                        user.IsVIP,
                        user.UserMustChangePassword);
                }
            }

            Response.Redirect(PortalUtils.EditUrl("ItemID", PanelRequest.ItemID.ToString(),
                (PanelRequest.Context == "Mailbox") ? "mailboxes" : "users",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        protected void SendToGroupCheckedChanged(object sender, EventArgs e)
        {
            EmailRow.Visible = rbtnEmail.Checked;
            MobileRow.Visible = !rbtnEmail.Checked;
        }
    }
}