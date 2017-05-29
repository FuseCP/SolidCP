using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.EnterpriseServer.Base.RDS;

namespace SolidCP.Portal.RDS
{
    public partial class RDSEditUserExperience : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var timeouts = RdsServerSettings.ScreenSaverTimeOuts;
                ddTimeout.DataSource = timeouts;
                ddTimeout.DataTextField = "Value";
                ddTimeout.DataValueField = "Key";
                ddTimeout.DataBind();

                var collection = ES.Services.RDS.GetRdsCollection(PanelRequest.CollectionID);
                litCollectionName.Text = collection.DisplayName;
                BindSettings();
            }
        }

        private void BindSettings()
        {
            var serverSettings = ES.Services.RDS.GetRdsServerSettings(PanelRequest.CollectionID, string.Format("Collection-{0}-Settings", PanelRequest.CollectionID));

            if (serverSettings == null || !serverSettings.Settings.Any())
            {
                ddTimeout.SelectedValue = "";
                ddTreshold.SelectedValue = "";
                ddHideCDrive.SelectedValue = "";
            }
            else
            {
                BindSettings(serverSettings);
            }
        }

        private void BindSettings(RdsServerSettings settings)
        {
            var setting = GetServerSetting(settings, RdsServerSettings.LOCK_SCREEN_TIMEOUT);

            if (setting != null)
            {
                ddTimeout.SelectedValue = setting.PropertyValue;
                cbTimeoutAdministrators.Checked = setting.ApplyAdministrators;
                cbTimeoutUsers.Checked = setting.ApplyUsers;
            }

            SetCheckboxes(settings, RdsServerSettings.REMOVE_RUN_COMMAND, cbRunCommandAdministrators, cbRunCommandUsers);
            SetCheckboxes(settings, RdsServerSettings.REMOVE_POWERSHELL_COMMAND, cbPowershellAdministrators, cbPowershellUsers);
            //SetCheckboxes(settings, RdsServerSettings.HIDE_C_DRIVE, cbHideCDriveAdministrators, cbHideCDriveUsers);
            SetCheckboxes(settings, RdsServerSettings.REMOVE_SHUTDOWN_RESTART, cbShutdownAdministrators, cbShutdownUsers);
            SetCheckboxes(settings, RdsServerSettings.DISABLE_TASK_MANAGER, cbTaskManagerAdministrators, cbTaskManagerUsers);
            SetCheckboxes(settings, RdsServerSettings.CHANGE_DESKTOP_DISABLED, cbDesktopAdministrators, cbDesktopUsers);
            SetCheckboxes(settings, RdsServerSettings.SCREEN_SAVER_DISABLED, cbScreenSaverAdministrators, cbScreenSaverUsers);
            SetCheckboxes(settings, RdsServerSettings.RDS_VIEW_WITHOUT_PERMISSION, cbViewSessionAdministrators, cbViewSessionUsers);
            SetCheckboxes(settings, RdsServerSettings.RDS_CONTROL_WITHOUT_PERMISSION, cbControlSessionAdministrators, cbControlSessionUsers);
            SetCheckboxes(settings, RdsServerSettings.DISABLE_CMD, cbDisableCmdAdministrators, cbDisableCmdUsers);

            setting = GetServerSetting(settings, RdsServerSettings.DRIVE_SPACE_THRESHOLD);

            if (setting != null)
            {
                ddTreshold.SelectedValue = setting.PropertyValue;
            }

            setting = GetServerSetting(settings, RdsServerSettings.HIDE_C_DRIVE);

            if (setting != null)
            {
                ddHideCDrive.SelectedValue = setting.PropertyValue;
                cbHideCDriveAdministrators.Checked = setting.ApplyAdministrators;
                cbHideCDriveUsers.Checked = setting.ApplyUsers;
            }
        }

        private void SetCheckboxes(RdsServerSettings settings, string settingName, CheckBox cbAdministrators, CheckBox cbUsers)
        {
            var setting = GetServerSetting(settings, settingName);

            if (setting != null)
            {
                cbAdministrators.Checked = setting.ApplyAdministrators;
                cbUsers.Checked = setting.ApplyUsers;
            }
        }

        private RdsServerSetting GetServerSetting(RdsServerSettings settings, string propertyName)
        {
            return settings.Settings.FirstOrDefault(s => s.PropertyName.Equals(propertyName));
        }

        private RdsServerSettings GetSettings()
        {                                                                                               
            var settings = new RdsServerSettings();

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.LOCK_SCREEN_TIMEOUT,
                PropertyValue = ddTimeout.SelectedValue,
                ApplyAdministrators = cbTimeoutAdministrators.Checked,
                ApplyUsers = cbTimeoutUsers.Checked
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.REMOVE_RUN_COMMAND,
                PropertyValue = "",
                ApplyAdministrators = cbRunCommandAdministrators.Checked,
                ApplyUsers = cbRunCommandUsers.Checked
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.REMOVE_POWERSHELL_COMMAND,
                PropertyValue = "",
                ApplyAdministrators = cbPowershellAdministrators.Checked,
                ApplyUsers = cbPowershellUsers.Checked
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.HIDE_C_DRIVE,
                PropertyValue = ddHideCDrive.SelectedValue,
                ApplyAdministrators = cbHideCDriveAdministrators.Checked,
                ApplyUsers = cbHideCDriveUsers.Checked
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.REMOVE_SHUTDOWN_RESTART,
                PropertyValue = "",
                ApplyAdministrators = cbShutdownAdministrators.Checked,
                ApplyUsers = cbShutdownUsers.Checked
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.DISABLE_TASK_MANAGER,
                PropertyValue = "",
                ApplyAdministrators = cbTaskManagerAdministrators.Checked,
                ApplyUsers = cbTaskManagerUsers.Checked
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.CHANGE_DESKTOP_DISABLED,
                PropertyValue = "",
                ApplyAdministrators = cbDesktopAdministrators.Checked,
                ApplyUsers = cbDesktopUsers.Checked
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.SCREEN_SAVER_DISABLED,
                PropertyValue = "",
                ApplyAdministrators = cbScreenSaverAdministrators.Checked,
                ApplyUsers = cbScreenSaverUsers.Checked
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.DRIVE_SPACE_THRESHOLD,
                PropertyValue = ddTreshold.SelectedValue,
                ApplyAdministrators = true,
                ApplyUsers = true
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.RDS_VIEW_WITHOUT_PERMISSION,
                PropertyValue = "",
                ApplyAdministrators = cbViewSessionAdministrators.Checked,
                ApplyUsers = cbViewSessionUsers.Checked
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.RDS_CONTROL_WITHOUT_PERMISSION,
                PropertyValue = "",
                ApplyAdministrators = cbControlSessionAdministrators.Checked,
                ApplyUsers = cbControlSessionUsers.Checked
            });

            settings.Settings.Add(new RdsServerSetting
            {
                PropertyName = RdsServerSettings.DISABLE_CMD,
                PropertyValue = "",
                ApplyAdministrators = cbDisableCmdAdministrators.Checked,
                ApplyUsers = cbDisableCmdUsers.Checked
            });

            return settings;
        }        

        private bool SaveServerSettings()
        {
            try
            {
                ES.Services.RDS.UpdateRdsServerSettings(PanelRequest.CollectionID, string.Format("Collection-{0}-Settings", PanelRequest.CollectionID), GetSettings());
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

            SaveServerSettings();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            if (SaveServerSettings())
            {
                Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "rds_collections", "SpaceID=" + PanelSecurity.PackageId));
            }
        }
    }
}