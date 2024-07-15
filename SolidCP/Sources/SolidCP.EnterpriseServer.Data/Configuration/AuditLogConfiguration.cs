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

public partial class AuditLogConfiguration: EntityTypeConfiguration<AuditLog>
{
	public override void Configure() {
		HasKey(e => e.RecordId).HasName("PK_Log");
		Property(e => e.RecordId).IsUnicode(false);
		Property(e => e.SourceName).IsUnicode(false);
		Property(e => e.TaskName).IsUnicode(false);
		if (IsSqlServer)
		{
			Property(e => e.StartDate).HasColumnType("datetime");
			Property(e => e.FinishDate).HasColumnType("datetime");
			Property(e => e.ExecutionLog).HasColumnType("ntext");
		} else if (IsCore && (IsMySql || IsMariaDb || IsSqlite || IsPostgreSql)) {
			Property(e => e.ExecutionLog).HasColumnType("TEXT");
		}
	}
}
