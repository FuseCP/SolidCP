using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.Portal.Code.UserControls;

namespace SolidCP.Portal.ExchangeServer.UserControls
{
    public partial class OrganizationSettingsTabs : SolidCPControlBase
    {
        public string SelectedTab { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindTabs();
        }

        private void BindTabs()
        {
            List<Tab> tabsList = new List<Tab>();
            tabsList.Add(CreateTab("organization_settings_general_settings", "Tab.GeneralSettigns"));
            tabsList.Add(CreateTab("organization_settings_password_settings", "Tab.PasswordSettings"));

            

            int idx = 0;

            foreach (Tab tab in tabsList)
            {
                if (String.Compare(tab.Id, SelectedTab, true) == 0)
                {
                    break;
                }

                idx++;
            }

            osTabs.SelectedIndex = idx;
            osTabs.DataSource = tabsList;
            osTabs.DataBind();
        }

        private Tab CreateTab(string id, string text)
        {
            return new Tab(id, GetLocalizedString(text),
                HostModule.EditUrl("ItemID", PanelRequest.ItemID.ToString(), id,
                "SpaceID=" + PanelSecurity.PackageId
                ));
        }
    }
}