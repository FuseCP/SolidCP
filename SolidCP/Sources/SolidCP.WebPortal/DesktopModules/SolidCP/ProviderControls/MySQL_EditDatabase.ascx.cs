// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SolidCP.EnterpriseServer;
using SolidCP.Providers.Database;

namespace SolidCP.Portal.ProviderControls
{
	public partial class MySQL_EditDatabase : SolidCPControlBase, IDatabaseEditDatabaseControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (ViewState["ControlsToggled"] == null)
			{
				bool editMode = (PanelRequest.ItemID > 0);
				btnBackup.Enabled = editMode;
				btnRestore.Enabled = editMode;
				btnTruncate.Enabled = editMode;
			}
		}

		public void BindItem(SqlDatabase item)
		{
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

		public void SaveItem(SqlDatabase item)
		{
		}

		protected void btnTruncate_Click(object sender, EventArgs e)
		{
			// truncate
			try
			{
				int result = ES.Services.DatabaseServers.TruncateSqlDatabase(PanelRequest.ItemID);
				if (result < 0)
				{
					HostModule.ShowResultMessage(result);
					return;
				}
			}
			catch (Exception ex)
			{
				HostModule.ShowErrorMessage("SQL_TRUNCATE_DATABASE", ex);
				return;
			}

			HostModule.ShowSuccessMessage("SQL_TRUNCATE_DATABASE");
		}

		protected void btnBackup_Click(object sender, EventArgs e)
		{
			Response.Redirect(HostModule.EditUrl("ItemID", PanelRequest.ItemID.ToString(), "backup",
				PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId));
		}

		protected void btnRestore_Click(object sender, EventArgs e)
		{
			Response.Redirect(HostModule.EditUrl("ItemID", PanelRequest.ItemID.ToString(), "restore",
				PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId));
		}
	}
}
