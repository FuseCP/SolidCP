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

public partial class HostingPlanConfiguration: Extensions.EntityTypeConfiguration<HostingPlan>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public HostingPlanConfiguration(): base() { }
    public HostingPlanConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasOne(d => d.Package).WithMany(p => p.HostingPlans)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_HostingPlans_Packages");

        HasOne(d => d.Server).WithMany(p => p.HostingPlans).HasConstraintName("FK_HostingPlans_Servers");

        HasOne(d => d.User).WithMany(p => p.HostingPlans).HasConstraintName("FK_HostingPlans_Users");
    }
#endif
}
