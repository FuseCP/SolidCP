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

public partial class BackgroundTaskLogConfiguration: EntityTypeConfiguration<BackgroundTaskLog>
{
#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.LogId).HasName("PK__Backgrou__5E5499A830A1D5BF");

        if (IsMsSql)
        {
            Property(e => e.Date).HasColumnType("datetime");
			Property(e => e.Text).HasColumnType("ntext");
			Property(e => e.ExceptionStackTrace).HasColumnType("ntext");
			Property(e => e.XmlParameters).HasColumnType("ntext");
		} else if (IsCore && (IsMySql || IsMariaDb || IsSqlite || IsPostgreSql))
        {
			Property(e => e.Text).HasColumnType("TEXT");
			Property(e => e.ExceptionStackTrace).HasColumnType("TEXT");
			Property(e => e.XmlParameters).HasColumnType("TEXT");
        }
#if NetCore // EF Core
		HasOne(d => d.Task).WithMany(p => p.BackgroundTaskLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Backgroun__TaskI__06ADD4BD");
#else
        HasRequired(d => d.Task).WithMany(p => p.BackgroundTaskLogs);
#endif
    }
#endif
}
