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

public partial class BackgroundTaskStackConfiguration: Extensions.EntityTypeConfiguration<BackgroundTaskStack>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public BackgroundTaskStackConfiguration(): base() { }
    public BackgroundTaskStackConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.TaskStackId).HasName("PK__Backgrou__5E44466F62E48BE6");

        ToTable("BackgroundTaskStack");

        HasIndex(e => e.TaskId, "BackgroundTaskStackIdx_TaskID");

        Property(e => e.TaskStackId).HasColumnName("TaskStackID");
        Property(e => e.TaskId).HasColumnName("TaskID");

        HasOne(d => d.Task).WithMany(p => p.BackgroundTaskStacks)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Backgroun__TaskI__098A4168");
    }
#endif
}
