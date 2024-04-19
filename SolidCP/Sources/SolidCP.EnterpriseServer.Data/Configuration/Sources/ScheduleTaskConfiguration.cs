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

public partial class ScheduleTaskConfiguration: Extensions.EntityTypeConfiguration<ScheduleTask>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public ScheduleTaskConfiguration(): base() { }
    public ScheduleTaskConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.TaskId);

        Property(e => e.TaskId)
                .HasMaxLength(100)
                .HasColumnName("TaskID");
        Property(e => e.RoleId).HasColumnName("RoleID");
        Property(e => e.TaskType)
                .IsRequired()
                .HasMaxLength(500);
    }
#endif
}
