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

public partial class PackageAddonConfiguration: Extensions.EntityTypeConfiguration<PackageAddon>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public PackageAddonConfiguration(): base() { }
    public PackageAddonConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasIndex(e => e.PackageId, "PackageAddonsIdx_PackageID");

        HasIndex(e => e.PlanId, "PackageAddonsIdx_PlanID");

        Property(e => e.PackageAddonId).HasColumnName("PackageAddonID");
        Property(e => e.Comments).HasColumnType("ntext");
        Property(e => e.PackageId).HasColumnName("PackageID");
        Property(e => e.PlanId).HasColumnName("PlanID");
        Property(e => e.PurchaseDate).HasColumnType("datetime");
        Property(e => e.StatusId).HasColumnName("StatusID");

        HasOne(d => d.Package).WithMany(p => p.PackageAddons)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PackageAddons_Packages");

        HasOne(d => d.Plan).WithMany(p => p.PackageAddons)
                .HasForeignKey(d => d.PlanId)
                .HasConstraintName("FK_PackageAddons_HostingPlans");
    }
#endif
}
