using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.HostedSolution;

namespace SolidCP.Portal.ExchangeServer
{
    public partial class OrganizationSettingsGeneralSettings : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Organization org = ES.Services.Organizations.GetOrganization(PanelRequest.ItemID);
                litOrganizationName.Text = org.OrganizationId;

                BindSettings();
            }

        }

        private void BindSettings()
        {
            var settings = ES.Services.Organizations.GetOrganizationGeneralSettings(PanelRequest.ItemID);

            if (settings != null)
            {
                txtOrganizationLogoUrl.Text = settings.OrganizationLogoUrl;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            SaveGeneralSettings(GetSettings());
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            if (SaveGeneralSettings(GetSettings()))
            {
                Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "organization_home",
                    "SpaceID=" + PanelSecurity.PackageId));
            }
        }

        private OrganizationGeneralSettings GetSettings()
        {
            var settings = new OrganizationGeneralSettings
            {
                OrganizationLogoUrl = txtOrganizationLogoUrl.Text
            };

            return settings;
        }


        private bool SaveGeneralSettings(OrganizationGeneralSettings settings)
        {
            try
            {
                ES.Services.Organizations.UpdateOrganizationGeneralSettings(PanelRequest.ItemID, GetSettings());
            }
            catch (Exception ex)
            {
                ShowErrorMessage("ORANIZATIONSETTINGS_NOT_UPDATED", ex);
                return false;
            }

            return true;
        }
    }
}