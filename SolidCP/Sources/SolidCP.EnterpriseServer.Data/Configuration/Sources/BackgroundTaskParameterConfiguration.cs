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

using BackgroundTaskParameter = SolidCP.EnterpriseServer.Data.Entities.BackgroundTaskParameter;

public partial class BackgroundTaskParameterConfiguration: Extensions.EntityTypeConfiguration<BackgroundTaskParameter>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public BackgroundTaskParameterConfiguration(): base() { }
    public BackgroundTaskParameterConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.ParameterId).HasName("PK__Backgrou__F80C6297E2E5AF88");

        HasIndex(e => e.TaskId, "BackgroundTaskParametersIdx_TaskID");

        Property(e => e.ParameterId).HasColumnName("ParameterID");
        Property(e => e.Name).HasMaxLength(255);
        Property(e => e.SerializerValue).HasColumnType("ntext");
        Property(e => e.TaskId).HasColumnName("TaskID");
        Property(e => e.TypeName).HasMaxLength(255);

        HasOne(d => d.Task).WithMany(p => p.BackgroundTaskParameters)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Backgroun__TaskI__03D16812");
    }
#endif
}
