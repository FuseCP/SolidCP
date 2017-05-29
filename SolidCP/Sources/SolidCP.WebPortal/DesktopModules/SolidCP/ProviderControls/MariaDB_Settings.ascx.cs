using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.DynamicData;
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal.ProviderControls {
    public partial class MariaDB_Settings : SolidCPControlBase, IHostingServiceProviderSettings {
        protected void Page_Load(object sender, EventArgs e) {
            if(!IsPostBack) {
                RenderFtuNote();
            }
        }


        private void RenderFtuNote() {
            string ftuNote = GetLocalizedString("FirsttimeUserNote");
            //
            ServerInfo serverInfo = ES.Services.Servers.GetServerById(PanelRequest.ServerId);
            //
            lblFirsttimeUserNote.InnerHtml = String.Format(ftuNote, serverInfo.ServerName);
        }

        public void BindSettings(StringDictionary settings) {
            txtInternalAddress.Text = settings["InternalAddress"];
            txtExternalAddress.Text = settings["ExternalAddress"];
            txtBinFolder.Text = settings["InstallFolder"];
            chkOldPassword.Checked = Utils.ParseBool(settings["OldPassword"], false);
            txtUserName.Text = settings["RootLogin"];
            ViewState["PWD"] = settings["RootPassword"];
            rowPassword.Visible = ((string)ViewState["PWD"]) != "";

            txtBrowseUrl.Text = settings["BrowseURL"];
            Utils.SelectListItem(ddlBrowseMethod, settings["BrowseMethod"]);
            txtBrowseParameters.Text = settings["BrowseParameters"];
        }

        public void SaveSettings(StringDictionary settings) {
            settings["InternalAddress"] = txtInternalAddress.Text.Trim();
            settings["ExternalAddress"] = txtExternalAddress.Text.Trim();
            settings["InstallFolder"] = txtBinFolder.Text.Trim();
            settings["OldPassword"] = chkOldPassword.Checked.ToString();
            settings["RootLogin"] = txtUserName.Text.Trim();
            settings["RootPassword"] = (txtPassword.Text.Length > 0) ? txtPassword.Text : (string)ViewState["PWD"];
            settings["BrowseURL"] = txtBrowseUrl.Text.Trim();
            settings["BrowseMethod"] = ddlBrowseMethod.SelectedValue;
            settings["BrowseParameters"] = txtBrowseParameters.Text;
        }
    }
}
