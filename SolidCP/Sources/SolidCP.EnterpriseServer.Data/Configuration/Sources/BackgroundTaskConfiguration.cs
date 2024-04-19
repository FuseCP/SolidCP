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

public partial class BackgroundTaskConfiguration: Extensions.EntityTypeConfiguration<BackgroundTask>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public BackgroundTaskConfiguration(): base() { }
    public BackgroundTaskConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__Backgrou__3214EC271AFAB817");

        Property(e => e.Id).HasColumnName("ID");
        Property(e => e.EffectiveUserId).HasColumnName("EffectiveUserID");
        Property(e => e.FinishDate).HasColumnType("datetime");
        Property(e => e.ItemId).HasColumnName("ItemID");
        Property(e => e.ItemName).HasMaxLength(255);
        Property(e => e.PackageId).HasColumnName("PackageID");
        Property(e => e.ScheduleId).HasColumnName("ScheduleID");
        Property(e => e.StartDate).HasColumnType("datetime");
        Property(e => e.TaskId)
                .HasMaxLength(255)
                .HasColumnName("TaskID");
        Property(e => e.TaskName).HasMaxLength(255);
        Property(e => e.UserId).HasColumnName("UserID");
    }
#endif
}
