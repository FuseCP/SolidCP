using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.OS;

namespace SolidCP.Portal.ExchangeServer
{
    public partial class EnterpriseStorageFolderSettingsOwaEditing : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!ES.Services.EnterpriseStorage.CheckUsersDomainExists(PanelRequest.ItemID))
                {
                    Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "enterprisestorage_folders",
                        "ItemID=" + PanelRequest.ItemID));
                }

                BindSettings();
            }
        }

        private void BindSettings()
        {
            try
            {
                // get settings
                Organization org = ES.Services.Organizations.GetOrganization(PanelRequest.ItemID);

                SystemFile folder = ES.Services.EnterpriseStorage.GetEnterpriseFolder(
                    PanelRequest.ItemID, PanelRequest.FolderID);

                litFolderName.Text = string.Format("{0}", folder.Name);

                // bind form
                var esPermissions = ES.Services.EnterpriseStorage.GetFolderOwaAccounts(PanelRequest.ItemID, folder);

                owaUsers.SetUsers(esPermissions);
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("ENETERPRISE_STORAGE_GET_FOLDER_SETTINGS", ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                SystemFile folder = new SystemFile { Name = PanelRequest.FolderID };

                if (!ES.Services.EnterpriseStorage.CheckEnterpriseStorageInitialization(PanelSecurity.PackageId, PanelRequest.ItemID))
                {
                    ES.Services.EnterpriseStorage.CreateEnterpriseStorage(PanelSecurity.PackageId, PanelRequest.ItemID);
                }

                ES.Services.EnterpriseStorage.SetFolderOwaAccounts(
                    PanelRequest.ItemID,
                    folder,
                    owaUsers.GetUsers());


                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "enterprisestorage_folders",
                        "ItemID=" + PanelRequest.ItemID));
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("ENTERPRISE_STORAGE_UPDATE_FOLDER_SETTINGS", ex);
            }
        }
    }
}