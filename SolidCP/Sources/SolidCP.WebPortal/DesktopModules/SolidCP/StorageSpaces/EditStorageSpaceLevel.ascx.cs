using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.Providers.StorageSpaces;

namespace SolidCP.Portal.StorageSpaces
{
    public partial class EditStorageSpaceLevel : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           // servers.Module = Module;

            if (!Page.IsPostBack)
            {
                var level = ES.Services.StorageSpaces.GetStorageSpaceLevelById(PanelRequest.SsLevelId);

                if (level != null)
                {
                    txtName.Text = level.Name;
                    txtDescription.Text = level.Description;
                }

                var groups = ES.Services.StorageSpaces.GetLevelResourceGroups(PanelRequest.SsLevelId);

                resourceGroups.SetResourceGroups(groups);
            }
        }

        private bool SaveSsLevel(out int levelId,bool exit = false)
        {
            StorageSpaceLevel level = ES.Services.StorageSpaces.GetStorageSpaceLevelById(PanelRequest.SsLevelId) 
                                      ?? new StorageSpaceLevel();
            var groups = resourceGroups.GetResourceGroups();

            level.Id = PanelRequest.SsLevelId;
            level.Name = txtName.Text;
            level.Description = txtDescription.Text;

            var result = ES.Services.StorageSpaces.SaveStorageSpaceLevel(level, groups);

            levelId = result.Value;

            messageBox.ShowMessage(result, "STORAGE_SPACE_LEVEL_SAVE", null);

            if (!exit)
            {
                resourceGroups.SetResourceGroups(groups);
            }


            return result.IsSuccess;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            int levelId;
            if (SaveSsLevel(out levelId) && PanelRequest.SsLevelId <= 0)
            {
                EditSsLevel(levelId);
            }
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            int levelId;
            if (SaveSsLevel(out levelId))
            {
                Response.Redirect(EditUrl(null));
            }
        }

        private void EditSsLevel(int levelId)
        {
            Response.Redirect(EditUrl("SsLevelId", levelId.ToString(), "edit_storage_space_level"));
        }
    }
}