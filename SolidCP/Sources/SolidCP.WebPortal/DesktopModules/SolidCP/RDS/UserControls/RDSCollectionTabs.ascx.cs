using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.Portal.Code.UserControls;

namespace SolidCP.Portal.RDS.UserControls
{
    public partial class RdsServerTabs : SolidCPControlBase
    {
        public string SelectedTab { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindTabs();
        }

        private void BindTabs()
        {
            List<Tab> tabsList = new List<Tab>();            
            tabsList.Add(CreateTab("rds_edit_collection", "Tab.RdsServers"));
            tabsList.Add(CreateTab("rds_edit_collection_settings", "Tab.Settings"));
            tabsList.Add(CreateTab("rds_collection_user_experience", "Tab.UserExperience"));
            tabsList.Add(CreateTab("rds_collection_edit_apps", "Tab.RdsApplications"));
            tabsList.Add(CreateTab("rds_collection_edit_users", "Tab.RdsUsers"));
            tabsList.Add(CreateTab("rds_collection_user_sessions", "Tab.UserSessions"));
            tabsList.Add(CreateTab("rds_collection_local_admins", "Tab.LocalAdmins"));                                
            
            int idx = 0;

            foreach (Tab tab in tabsList)
            {
                if (String.Compare(tab.Id, SelectedTab, true) == 0)
                {
                    break;
                }

                idx++;
            }

            rdsTabs.SelectedIndex = idx;
            rdsTabs.DataSource = tabsList;
            rdsTabs.DataBind();
        }

        private Tab CreateTab(string id, string text)
        {
            return new Tab(id, GetLocalizedString(text),
                HostModule.EditUrl("AccountID", PanelRequest.AccountID.ToString(), id,
                "SpaceID=" + PanelSecurity.PackageId.ToString(),
                "ItemID=" + PanelRequest.ItemID.ToString(), "CollectionID=" + PanelRequest.CollectionID));
        }
    }
}