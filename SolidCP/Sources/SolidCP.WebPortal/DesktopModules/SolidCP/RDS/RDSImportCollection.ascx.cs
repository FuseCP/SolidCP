using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolidCP.Portal.RDS
{
    public partial class RDSImportCollection : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                ES.Services.RDS.ImportCollection(PanelRequest.ItemID, txtCollectionName.Text);
                Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "rds_collections", "SpaceID=" + PanelSecurity.PackageId));
            }
            catch (Exception ex)
            {
                ShowErrorMessage("RDSCOLLECTION_NOT_IMPORTED", ex);
            }            
        }
    }
}