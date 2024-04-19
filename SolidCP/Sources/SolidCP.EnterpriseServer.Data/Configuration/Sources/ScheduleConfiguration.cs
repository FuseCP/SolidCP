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

public partial class ScheduleConfiguration: Extensions.EntityTypeConfiguration<Schedule>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public ScheduleConfiguration(): base() { }
    public ScheduleConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        ToTable("Schedule");

        HasIndex(e => e.PackageId, "ScheduleIdx_PackageID");

        HasIndex(e => e.TaskId, "ScheduleIdx_TaskID");

        Property(e => e.ScheduleId).HasColumnName("ScheduleID");
        Property(e => e.FromTime).HasColumnType("datetime");
        Property(e => e.LastRun).HasColumnType("datetime");
        Property(e => e.NextRun).HasColumnType("datetime");
        Property(e => e.PackageId).HasColumnName("PackageID");
        Property(e => e.PriorityId)
                .HasMaxLength(50)
                .HasColumnName("PriorityID");
        Property(e => e.ScheduleName).HasMaxLength(100);
        Property(e => e.ScheduleTypeId)
                .HasMaxLength(50)
                .HasColumnName("ScheduleTypeID");
        Property(e => e.StartTime).HasColumnType("datetime");
        Property(e => e.TaskId)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("TaskID");
        Property(e => e.ToTime).HasColumnType("datetime");

        HasOne(d => d.Package).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Schedule_Packages");

        HasOne(d => d.Task).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_ScheduleTasks");
    }
#endif
}
