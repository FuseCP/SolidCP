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
        HasKey(e => e.PlanId);

        HasIndex(e => e.PackageId, "HostingPlansIdx_PackageID");

        HasIndex(e => e.ServerId, "HostingPlansIdx_ServerID");

        HasIndex(e => e.UserId, "HostingPlansIdx_UserID");

        Property(e => e.PlanId).HasColumnName("PlanID");
        Property(e => e.PackageId).HasColumnName("PackageID");
        Property(e => e.PlanDescription).HasColumnType("ntext");
        Property(e => e.PlanName)
                .IsRequired()
                .HasMaxLength(200);
        Property(e => e.RecurringPrice).HasColumnType("money");
        Property(e => e.ServerId).HasColumnName("ServerID");
        Property(e => e.SetupPrice).HasColumnType("money");
        Property(e => e.UserId).HasColumnName("UserID");

        HasOne(d => d.Package).WithMany(p => p.HostingPlans)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_HostingPlans_Packages");

        HasOne(d => d.Server).WithMany(p => p.HostingPlans)
                .HasForeignKey(d => d.ServerId)
                .HasConstraintName("FK_HostingPlans_Servers");

        HasOne(d => d.User).WithMany(p => p.HostingPlans)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_HostingPlans_Users");
    }
#endif
}
