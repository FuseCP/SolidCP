using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal
{
    public partial class SettingsRdsSetupLetter : SolidCPControlBase, IUserSettingsEditorControl
    {
        public void BindSettings(UserSettings settings)
        {
            txtFrom.Text = settings["From"];
            txtCC.Text = settings["CC"];
            txtSubject.Text = settings["Subject"];
            Utils.SelectListItem(ddlPriority, settings["Priority"]);
            txtHtmlBody.Text = settings["HtmlBody"];
            txtTextBody.Text = settings["TextBody"];
        }

        public void SaveSettings(UserSettings settings)
        {
            settings["From"] = txtFrom.Text;
            settings["CC"] = txtCC.Text;
            settings["Subject"] = txtSubject.Text;
            settings["Priority"] = ddlPriority.SelectedValue;
            settings["HtmlBody"] = txtHtmlBody.Text;
            settings["TextBody"] = txtTextBody.Text;
        }
    }
}