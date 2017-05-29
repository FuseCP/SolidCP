using SolidCP.EnterpriseServer;
using SolidCP.Providers.Database;
using System;

namespace SolidCP.Portal.ProviderControls {
    public partial class MariaDB_EditUser_EditField : SolidCPControlBase, IDatabaseEditDatabaseControl {
        protected void Page_Load(object sender, EventArgs e) {
            if(ViewState["ControlsToggled"] == null) {
                bool editMode = (PanelRequest.ItemID > 0);
                btnBackup.Enabled = editMode;
                btnRestore.Enabled = editMode;
                btnTruncate.Enabled = editMode;
            }
        }

        public void BindItem(SqlDatabase item) {
            litDataSize.Text = item.DataSize.ToString();
            // enable/disable controls according to hosting context
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(item.PackageId);
            string backupQuotaName = item.GroupName + ".Backup";
            string restoreQuotaName = item.GroupName + ".Restore";
            string truncateQuotaName = item.GroupName + ".Truncate";
            btnBackup.Enabled = cntx.Quotas.ContainsKey(backupQuotaName) && !cntx.Quotas[backupQuotaName].QuotaExhausted;
            btnRestore.Enabled = cntx.Quotas.ContainsKey(restoreQuotaName) && !cntx.Quotas[restoreQuotaName].QuotaExhausted;
            btnTruncate.Enabled = cntx.Quotas.ContainsKey(truncateQuotaName) && !cntx.Quotas[truncateQuotaName].QuotaExhausted;

            ViewState["ControlsToggled"] = true;
        }

        public void SaveItem(SqlDatabase item) {
        }

        protected void btnTruncate_Click(object sender, EventArgs e) {
            // truncate
            try {
                int result = ES.Services.DatabaseServers.TruncateSqlDatabase(PanelRequest.ItemID);
                if(result < 0) {
                    HostModule.ShowResultMessage(result);
                    return;
                }
            } catch(Exception ex) {
                HostModule.ShowErrorMessage("SQL_TRUNCATE_DATABASE", ex);
                return;
            }

            HostModule.ShowSuccessMessage("SQL_TRUNCATE_DATABASE");
        }

        protected void btnBackup_Click(object sender, EventArgs e) {
            Response.Redirect(HostModule.EditUrl("ItemID", PanelRequest.ItemID.ToString(), "backup",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId));
        }

        protected void btnRestore_Click(object sender, EventArgs e) {
            Response.Redirect(HostModule.EditUrl("ItemID", PanelRequest.ItemID.ToString(), "restore",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId));
        }
    }
}
