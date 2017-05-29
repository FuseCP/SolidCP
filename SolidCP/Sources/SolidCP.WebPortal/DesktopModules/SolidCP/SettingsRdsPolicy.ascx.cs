using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.EnterpriseServer.Base.RDS;

namespace SolidCP.Portal
{
    public partial class SettingsRdsPolicy : SolidCPControlBase, IUserSettingsEditorControl
    {        
        public void BindSettings(UserSettings settings)
        {
            var timeouts = RdsServerSettings.ScreenSaverTimeOuts;
            ddTimeout.DataSource = timeouts;
            ddTimeout.DataTextField = "Value";
            ddTimeout.DataValueField = "Key";
            ddTimeout.DataBind();

            ddTimeout.SelectedValue = settings[RdsServerSettings.LOCK_SCREEN_TIMEOUT_VALUE];
            cbTimeoutAdministrators.Checked = Convert.ToBoolean(settings[RdsServerSettings.LOCK_SCREEN_TIMEOUT_ADMINISTRATORS]);
            cbTimeoutUsers.Checked = Convert.ToBoolean(settings[RdsServerSettings.LOCK_SCREEN_TIMEOUT_USERS]);

            cbRunCommandAdministrators.Checked = Convert.ToBoolean(settings[RdsServerSettings.REMOVE_RUN_COMMAND_ADMINISTRATORS]);
            cbRunCommandUsers.Checked = Convert.ToBoolean(settings[RdsServerSettings.REMOVE_RUN_COMMAND_USERS]);

            cbPowershellAdministrators.Checked = Convert.ToBoolean(settings[RdsServerSettings.REMOVE_POWERSHELL_COMMAND_ADMINISTRATORS]);
            cbPowershellUsers.Checked = Convert.ToBoolean(settings[RdsServerSettings.REMOVE_POWERSHELL_COMMAND_USERS]);

            ddHideCDrive.SelectedValue = settings[RdsServerSettings.HIDE_C_DRIVE_VALUE];
            cbHideCDriveAdministrators.Checked = Convert.ToBoolean(settings[RdsServerSettings.HIDE_C_DRIVE_ADMINISTRATORS]);
            cbHideCDriveUsers.Checked = Convert.ToBoolean(settings[RdsServerSettings.HIDE_C_DRIVE_USERS]);

            cbShutdownAdministrators.Checked = Convert.ToBoolean(settings[RdsServerSettings.REMOVE_SHUTDOWN_RESTART_ADMINISTRATORS]);
            cbShutdownUsers.Checked = Convert.ToBoolean(settings[RdsServerSettings.REMOVE_SHUTDOWN_RESTART_USERS]);

            cbTaskManagerAdministrators.Checked = Convert.ToBoolean(settings[RdsServerSettings.DISABLE_TASK_MANAGER_ADMINISTRATORS]);
            cbTaskManagerUsers.Checked = Convert.ToBoolean(settings[RdsServerSettings.DISABLE_TASK_MANAGER_USERS]);

            cbDesktopAdministrators.Checked = Convert.ToBoolean(settings[RdsServerSettings.CHANGE_DESKTOP_DISABLED_ADMINISTRATORS]);
            cbDesktopUsers.Checked = Convert.ToBoolean(settings[RdsServerSettings.CHANGE_DESKTOP_DISABLED_USERS]);

            cbScreenSaverAdministrators.Checked = Convert.ToBoolean(settings[RdsServerSettings.SCREEN_SAVER_DISABLED_ADMINISTRATORS]);
            cbScreenSaverUsers.Checked = Convert.ToBoolean(settings[RdsServerSettings.SCREEN_SAVER_DISABLED_USERS]);

            cbViewSessionAdministrators.Checked = Convert.ToBoolean(settings[RdsServerSettings.RDS_VIEW_WITHOUT_PERMISSION_ADMINISTRATORS]);
            cbViewSessionUsers.Checked = Convert.ToBoolean(settings[RdsServerSettings.RDS_VIEW_WITHOUT_PERMISSION_Users]);
            cbControlSessionAdministrators.Checked = Convert.ToBoolean(settings[RdsServerSettings.RDS_CONTROL_WITHOUT_PERMISSION_ADMINISTRATORS]);
            cbControlSessionUsers.Checked = Convert.ToBoolean(settings[RdsServerSettings.RDS_CONTROL_WITHOUT_PERMISSION_Users]);

            cbDisableCmdAdministrators.Checked = Convert.ToBoolean(settings[RdsServerSettings.DISABLE_CMD_ADMINISTRATORS]);
            cbDisableCmdUsers.Checked = Convert.ToBoolean(settings[RdsServerSettings.DISABLE_CMD_USERS]);

            ddTreshold.SelectedValue = settings[RdsServerSettings.DRIVE_SPACE_THRESHOLD_VALUE];            
        }

        public void SaveSettings(UserSettings settings)
        {
            settings[RdsServerSettings.LOCK_SCREEN_TIMEOUT_VALUE] = ddTimeout.SelectedValue;
            settings[RdsServerSettings.LOCK_SCREEN_TIMEOUT_ADMINISTRATORS] = cbTimeoutAdministrators.Checked.ToString();
            settings[RdsServerSettings.LOCK_SCREEN_TIMEOUT_USERS] = cbTimeoutUsers.Checked.ToString();
            settings[RdsServerSettings.REMOVE_RUN_COMMAND_ADMINISTRATORS] = cbRunCommandAdministrators.Checked.ToString();
            settings[RdsServerSettings.REMOVE_RUN_COMMAND_USERS] = cbRunCommandUsers.Checked.ToString();
            settings[RdsServerSettings.REMOVE_POWERSHELL_COMMAND_ADMINISTRATORS] = cbPowershellAdministrators.Checked.ToString();
            settings[RdsServerSettings.REMOVE_POWERSHELL_COMMAND_USERS] = cbPowershellUsers.Checked.ToString();
            settings[RdsServerSettings.HIDE_C_DRIVE_VALUE] = ddHideCDrive.SelectedValue;
            settings[RdsServerSettings.HIDE_C_DRIVE_ADMINISTRATORS] = cbHideCDriveAdministrators.Checked.ToString();
            settings[RdsServerSettings.HIDE_C_DRIVE_USERS] = cbHideCDriveUsers.Checked.ToString();
            settings[RdsServerSettings.REMOVE_SHUTDOWN_RESTART_ADMINISTRATORS] = cbShutdownAdministrators.Checked.ToString();
            settings[RdsServerSettings.REMOVE_SHUTDOWN_RESTART_USERS] = cbShutdownUsers.Checked.ToString();
            settings[RdsServerSettings.DISABLE_TASK_MANAGER_ADMINISTRATORS] = cbTaskManagerAdministrators.Checked.ToString();
            settings[RdsServerSettings.DISABLE_TASK_MANAGER_USERS] = cbTaskManagerUsers.Checked.ToString();
            settings[RdsServerSettings.CHANGE_DESKTOP_DISABLED_ADMINISTRATORS] = cbDesktopAdministrators.Checked.ToString();
            settings[RdsServerSettings.CHANGE_DESKTOP_DISABLED_USERS] = cbDesktopUsers.Checked.ToString();
            settings[RdsServerSettings.SCREEN_SAVER_DISABLED_ADMINISTRATORS] = cbScreenSaverAdministrators.Checked.ToString();
            settings[RdsServerSettings.SCREEN_SAVER_DISABLED_USERS] = cbScreenSaverUsers.Checked.ToString();
            settings[RdsServerSettings.DRIVE_SPACE_THRESHOLD_VALUE] = ddTreshold.SelectedValue;
            settings[RdsServerSettings.RDS_VIEW_WITHOUT_PERMISSION_ADMINISTRATORS] = cbViewSessionAdministrators.Checked.ToString();
            settings[RdsServerSettings.RDS_VIEW_WITHOUT_PERMISSION_Users] = cbViewSessionUsers.Checked.ToString();
            settings[RdsServerSettings.RDS_CONTROL_WITHOUT_PERMISSION_ADMINISTRATORS] = cbControlSessionAdministrators.Checked.ToString();
            settings[RdsServerSettings.RDS_CONTROL_WITHOUT_PERMISSION_Users] = cbControlSessionUsers.Checked.ToString();
            settings[RdsServerSettings.DISABLE_CMD_ADMINISTRATORS] = cbDisableCmdAdministrators.Checked.ToString();
            settings[RdsServerSettings.DISABLE_CMD_USERS] = cbDisableCmdUsers.Checked.ToString();            
        }
    }
}