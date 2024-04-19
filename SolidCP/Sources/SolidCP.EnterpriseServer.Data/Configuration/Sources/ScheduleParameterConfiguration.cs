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

public partial class ScheduleParameterConfiguration: Extensions.EntityTypeConfiguration<ScheduleParameter>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public ScheduleParameterConfiguration(): base() { }
    public ScheduleParameterConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => new { e.ScheduleId, e.ParameterId });

        Property(e => e.ScheduleId).HasColumnName("ScheduleID");
        Property(e => e.ParameterId)
                .HasMaxLength(100)
                .HasColumnName("ParameterID");
        Property(e => e.ParameterValue).HasMaxLength(1000);

        HasOne(d => d.Schedule).WithMany(p => p.ScheduleParameters)
                .HasForeignKey(d => d.ScheduleId)
                .HasConstraintName("FK_ScheduleParameters_Schedule");
    }
#endif
}
