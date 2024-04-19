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

public partial class AuditLogConfiguration: Extensions.EntityTypeConfiguration<AuditLog>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public AuditLogConfiguration(): base() { }
    public AuditLogConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.RecordId).HasName("PK_Log");

        ToTable("AuditLog");

        Property(e => e.RecordId)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("RecordID");
        Property(e => e.ExecutionLog).HasColumnType("ntext");
        Property(e => e.FinishDate).HasColumnType("datetime");
        Property(e => e.ItemId).HasColumnName("ItemID");
        Property(e => e.ItemName).HasMaxLength(100);
        Property(e => e.PackageId).HasColumnName("PackageID");
        Property(e => e.SeverityId).HasColumnName("SeverityID");
        Property(e => e.SourceName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        Property(e => e.StartDate).HasColumnType("datetime");
        Property(e => e.TaskName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        Property(e => e.UserId).HasColumnName("UserID");
        Property(e => e.Username).HasMaxLength(50);
    }
#endif
}
