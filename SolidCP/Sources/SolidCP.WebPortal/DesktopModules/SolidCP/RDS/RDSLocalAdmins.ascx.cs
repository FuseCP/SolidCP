using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.RemoteDesktopServices;

namespace SolidCP.Portal.RDS
{
    public partial class RdsLocalAdmins : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            users.OnRefreshClicked -= OnRefreshClicked;
            users.OnRefreshClicked += OnRefreshClicked;
            users.Module = Module;

            if (!IsPostBack)
            {                
                var collectionLocalAdmins = ES.Services.RDS.GetRdsCollectionLocalAdmins(PanelRequest.CollectionID);
                var collection = ES.Services.RDS.GetRdsCollection(PanelRequest.CollectionID);

                litCollectionName.Text = collection.DisplayName;

                foreach(var user in collectionLocalAdmins)
                {
                    user.IsVIP = false;
                }

                users.SetUsers(collectionLocalAdmins);
            }
        }

        private void OnRefreshClicked(object sender, EventArgs e)
        {
            ((ModalPopupExtender)asyncTasks.FindControl("ModalPopupProperties")).Hide();            
        }

        private bool SaveLocalAdmins()
        {
            try
            {
                ES.Services.RDS.SaveRdsCollectionLocalAdmins(users.GetUsers(), PanelRequest.CollectionID);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("RDSLOCALADMINS_NOT_ADDED", ex);
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

            SaveLocalAdmins();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            if (SaveLocalAdmins())
            {
                Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "rds_collections", "SpaceID=" + PanelSecurity.PackageId));
            }
        }
    }
}