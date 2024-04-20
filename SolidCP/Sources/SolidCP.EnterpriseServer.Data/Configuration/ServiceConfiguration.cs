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

public partial class ServiceConfiguration: Extensions.EntityTypeConfiguration<Service>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public ServiceConfiguration(): base() { }
    public ServiceConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasOne(d => d.Cluster).WithMany(p => p.Services).HasConstraintName("FK_Services_Clusters");

        HasOne(d => d.Provider).WithMany(p => p.Services)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Services_Providers");

        HasOne(d => d.Server).WithMany(p => p.Services)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Services_Servers");
    }
#endif
}
