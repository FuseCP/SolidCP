using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class SystemSettingConfiguration: EntityTypeConfiguration<SystemSetting>
{
    public override void Configure() {

		if (IsSqlServer) Property(e => e.PropertyValue).HasColumnType("ntext");
		else if (IsCore && (IsMySql || IsMariaDb || IsSqlite || IsPostgreSql))
		{
			Property(e => e.PropertyValue).HasColumnType("TEXT");
		}

		#region Seed Data
		HasData(() => new SystemSetting[] {
			new SystemSetting() { SettingsName = "AccessIpsSettings", PropertyName = "AccessIps", PropertyValue = "" },
			new SystemSetting() { SettingsName = "AuthenticationSettings", PropertyName = "CanPeerChangeMfa", PropertyValue = "True" },
			new SystemSetting() { SettingsName = "AuthenticationSettings", PropertyName = "MfaTokenAppDisplayName", PropertyValue = "SolidCP" },
			new SystemSetting() { SettingsName = "BackupSettings", PropertyName = "BackupsPath", PropertyValue = "c:\\HostingBackups" },
			new SystemSetting() { SettingsName = "SmtpSettings", PropertyName = "SmtpEnableSsl", PropertyValue = "False" },
			new SystemSetting() { SettingsName = "SmtpSettings", PropertyName = "SmtpPort", PropertyValue = "25" },
			new SystemSetting() { SettingsName = "SmtpSettings", PropertyName = "SmtpServer", PropertyValue = "127.0.0.1" },
			new SystemSetting() { SettingsName = "SmtpSettings", PropertyName = "SmtpUsername", PropertyValue = "postmaster" }
		});
		#endregion
    }
}
