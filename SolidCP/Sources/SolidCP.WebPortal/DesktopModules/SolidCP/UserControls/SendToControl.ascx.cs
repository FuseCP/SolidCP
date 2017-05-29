using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SolidCP.Portal.UserControls
{

    public partial class SendToControl : SolidCPControlBase
    {

        public string ValidationGroup
        {
            get { return valEmailAddress.ValidationGroup; }
            set
            {
                valEmailAddress.ValidationGroup = value; 
                regexEmailValid.ValidationGroup = value;
                valMobile.ValidationGroup = value; 
                regexMobileValid.ValidationGroup = value;
            }
        }

        public bool IsRequestSend
        {
            get { return chkSendPasswordResetEmail.Checked; }
        }

        public bool SendEmail
        {
            get { return chkSendPasswordResetEmail.Checked && rbtnEmail.Checked; }
        }

        public bool SendMobile
        {
            get { return chkSendPasswordResetEmail.Checked && rbtnMobile.Checked; }
        }

        public string Email
        {
            get { return txtEmailAddress.Text; }
        }

        public string Mobile
        {
            get { return txtMobile.Text; }
        }

        public string ControlToHide { get; set; }

        protected void SendToGroupCheckedChanged(object sender, EventArgs e)
        {
            EmailDiv.Visible = rbtnEmail.Checked;
            MobileDiv.Visible = !rbtnEmail.Checked;
        }

        protected void chkSendPasswordResetEmail_StateChanged(object sender, EventArgs e)
        {
            SendToBody.Visible = chkSendPasswordResetEmail.Checked;

            if (!string.IsNullOrEmpty(ControlToHide))
            {
                var control = Parent.FindControl(ControlToHide);

                if (control != null)
                {
                    control.Visible = !chkSendPasswordResetEmail.Checked;
                }
            }
        }

        // Fix AutoPostBack issue due to Bootstrap Javascript
        protected void RadioButton_FixAutoPostBack_OnPreRender(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            HtmlGenericControl label = (HtmlGenericControl)radioButton.Parent;

            // Set onclick handler of the parent label to be the same as the radio button.
            //   This fixes issues caused by bootstrap javascript which adds labels.
            label.Attributes.Add("onclick",
                "javascript:setTimeout('__doPostBack(\\'" +
                radioButton.UniqueID + "\\',\\'\\')', 0)");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            var settings = ES.Services.Organizations.GetWebDavSystemSettings();
            var isSendPasswordEnabled = settings != null && Utils.ParseBool(settings[EnterpriseServer.SystemSettings.WEBDAV_PASSWORD_RESET_ENABLED_KEY], false);
            var isTwilioEnabled = ES.Services.System.CheckIsTwilioEnabled();

            SendPasswordResetEmailDiv.Visible = isSendPasswordEnabled;
            SendPasswordResetDisabledDiv.Visible = !isSendPasswordEnabled;
            rbtnMobile.Visible = isTwilioEnabled;

            if (!Page.IsPostBack)
            {
                if (!isTwilioEnabled)
                {
                    rbtnMobile.Checked = false;
                    rbtnEmail.Checked = true;
                    SendToGroupCheckedChanged(null, null);
                }
                else
                {
                    rbtnMobile.Checked = true;
                    rbtnEmail.Checked = false;
                    SendToGroupCheckedChanged(null, null);
                }

                chkSendPasswordResetEmail_StateChanged(null, null);
            }
        }
    }
}