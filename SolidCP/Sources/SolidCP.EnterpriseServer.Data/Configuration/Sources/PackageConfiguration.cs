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

public partial class PackageConfiguration: Extensions.EntityTypeConfiguration<Package>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public PackageConfiguration(): base() { }
    public PackageConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        ToTable(tb => tb.HasTrigger("Update_StatusIDchangeDate"));

        HasIndex(e => e.ParentPackageId, "PackageIndex_ParentPackageID");

        HasIndex(e => e.PlanId, "PackageIndex_PlanID");

        HasIndex(e => e.ServerId, "PackageIndex_ServerID");

        HasIndex(e => e.UserId, "PackageIndex_UserID");

        Property(e => e.PackageId).HasColumnName("PackageID");
        Property(e => e.BandwidthUpdated).HasColumnType("datetime");
        Property(e => e.PackageComments).HasColumnType("ntext");
        Property(e => e.PackageName).HasMaxLength(300);
        Property(e => e.ParentPackageId).HasColumnName("ParentPackageID");
        Property(e => e.PlanId).HasColumnName("PlanID");
        Property(e => e.PurchaseDate).HasColumnType("datetime");
        Property(e => e.ServerId).HasColumnName("ServerID");
        Property(e => e.StatusId).HasColumnName("StatusID");
        Property(e => e.StatusIdchangeDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("StatusIDchangeDate");
        Property(e => e.UserId).HasColumnName("UserID");

        HasOne(d => d.ParentPackage).WithMany(p => p.InverseParentPackage)
                .HasForeignKey(d => d.ParentPackageId)
                .HasConstraintName("FK_Packages_Packages");

        HasOne(d => d.Plan).WithMany(p => p.Packages)
                .HasForeignKey(d => d.PlanId)
                .HasConstraintName("FK_Packages_HostingPlans");

        HasOne(d => d.Server).WithMany(p => p.Packages)
                .HasForeignKey(d => d.ServerId)
                .HasConstraintName("FK_Packages_Servers");

        HasOne(d => d.User).WithMany(p => p.Packages)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Packages_Users");

        HasMany(d => d.Services).WithMany(p => p.Packages)
            .UsingEntity<Dictionary<string, object>>(
                "PackageService",
                r => r.HasOne<Service>().WithMany()
                        .HasForeignKey("ServiceId")
                        .HasConstraintName("FK_PackageServices_Services"),
                l => l.HasOne<Package>().WithMany()
                        .HasForeignKey("PackageId")
                        .HasConstraintName("FK_PackageServices_Packages"),
                j => {
                    j.HasKey("PackageId", "ServiceId");
                    j.ToTable("PackageServices");
                    j.IndexerProperty<int>("PackageId").HasColumnName("PackageID");
                    j.IndexerProperty<int>("ServiceId").HasColumnName("ServiceID");
                });
    }
#endif
}
