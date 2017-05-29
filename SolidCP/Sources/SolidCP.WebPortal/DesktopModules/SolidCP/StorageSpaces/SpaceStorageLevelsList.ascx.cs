using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.Providers.Common;
using SolidCP.Providers.StorageSpaces;

namespace SolidCP.Portal.StorageSpaces
{
    public partial class SpaceStorageLevelsList : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvSsLevels.PageSize = Convert.ToInt16(ddlPageSize.SelectedValue);
                gvSsLevels.Sort("Name", System.Web.UI.WebControls.SortDirection.Ascending);
            }
        }

        protected void odsRDSServersPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ProcessException(e.Exception.InnerException);
                this.DisableControls = true;
                e.ExceptionHandled = true;
            }
        }

        

        protected void gvSsLevels_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                int id;
                bool hasValue = int.TryParse(e.CommandArgument.ToString(), out id);

                ResultObject result = new ResultObject();
                result.IsSuccess = false;

                if (hasValue)
                {
                    result = ES.Services.StorageSpaces.RemoveStorageSpaceLevel(id);
                }

                messageBox.ShowMessage(result, "STORAGE_SPACES_LEVEL_REMOVE", null);

                if (!result.IsSuccess)
                {
                    return;
                }

                gvSsLevels.DataBind();
            }
            else if (e.CommandName == "EditSsLevel")
            {
                EditSsLevel(e.CommandArgument.ToString());
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvSsLevels.PageSize = Convert.ToInt16(ddlPageSize.SelectedValue);

            gvSsLevels.DataBind();
        }

        protected void btnAddSsLevel_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("SsLevelId", (-1).ToString(), "add_storage_space_level"));
        }

        private void EditSsLevel(string levelId)
        {
            Response.Redirect(EditUrl("SsLevelId", levelId, "edit_storage_space_level"));
        }

        protected bool CheckLevelIsInUse(int levelId)
        {
            return ES.Services.StorageSpaces.GetStorageSpacesByLevelId(levelId).Any();
        }
    }
}