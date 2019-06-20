using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.Portal.Code.UserControls;
using SCP = SolidCP.EnterpriseServer;

namespace SolidCP.Portal.ExchangeServer.UserControls
{
    public partial class EnterpriseStorageEditFolderTabs : SolidCPControlBase
    {
        public string SelectedTab { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindTabs();
        }

        private void BindTabs()
        {
            List<Tab> tabsList = new List<Tab>();
            tabsList.Add(CreateTab("enterprisestorage_folder_settings", "Tab.GeneralSettings"));
            tabsList.Add(CreateTab("enterprisestorage_folder_settings_folder_permissions", "Tab.FolderPermissions"));

            SCP.SystemSettings settings = ES.Services.System.GetSystemSettings(SCP.SystemSettings.WEBDAV_PORTAL_SETTINGS);
            bool isOwaEnabled = false;
            if (settings != null)
            {
                isOwaEnabled = Utils.ParseBool(settings[SCP.SystemSettings.WEBDAV_OWA_ENABLED_KEY], false);
            }
            if (isOwaEnabled) tabsList.Add(CreateTab("enterprisestorage_folder_settings_owa_editing", "Tab.OwaEditPermissions"));

            int idx = 0;

            foreach (Tab tab in tabsList)
            {
                if (String.Compare(tab.Id, SelectedTab, true) == 0)
                {
                    break;
                }

                idx++;
            }

            esTabs.SelectedIndex = idx;
            esTabs.DataSource = tabsList;
            esTabs.DataBind();
        }

        private Tab CreateTab(string id, string text)
        {
            return new Tab(id,GetLocalizedString(text),
                HostModule.EditUrl("AccountID", PanelRequest.AccountID.ToString(), id,
                "SpaceID=" + PanelSecurity.PackageId.ToString(),
                "ItemID=" + PanelRequest.ItemID.ToString(), "FolderID=" + PanelRequest.FolderID));
        }
    }
}