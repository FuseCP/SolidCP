// This file is auto generated, do not edit.
using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
#if NetCore
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
#endif
#if NetFX
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Spatial;
using System.Data.Entity.Validation;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class BackgroundTaskLogConfiguration: Extensions.EntityTypeConfiguration<BackgroundTaskLog>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public BackgroundTaskLogConfiguration(): base() { }
    public BackgroundTaskLogConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.LogId).HasName("PK__Backgrou__5E5499A830A1D5BF");

        HasIndex(e => e.TaskId, "BackgroundTaskLogsIdx_TaskID");

        Property(e => e.LogId).HasColumnName("LogID");
        Property(e => e.Date).HasColumnType("datetime");
        Property(e => e.ExceptionStackTrace).HasColumnType("ntext");
        Property(e => e.TaskId).HasColumnName("TaskID");
        Property(e => e.Text).HasColumnType("ntext");
        Property(e => e.XmlParameters).HasColumnType("ntext");

        HasOne(d => d.Task).WithMany(p => p.BackgroundTaskLogs)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Backgroun__TaskI__06ADD4BD");
    }
#endif
}
