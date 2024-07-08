// This file is auto generated, do not edit.
using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
using SolidCP.EnterpriseServer.Data.Extensions;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class PackageConfiguration: EntityTypeConfiguration<Package>
{
    public override void Configure() {
        ToTable(tb => tb.HasTrigger("Update_StatusIDchangeDate"));

        Property(e => e.StatusIdchangeDate).HasDefaultValueSql("(getdate())");

        HasOne(d => d.ParentPackage).WithMany(p => p.InverseParentPackage).HasConstraintName("FK_Packages_Packages");

        HasOne(d => d.Plan).WithMany(p => p.Packages).HasConstraintName("FK_Packages_HostingPlans");

        HasOne(d => d.Server).WithMany(p => p.Packages).HasConstraintName("FK_Packages_Servers");

        HasOne(d => d.User).WithMany(p => p.Packages)
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

        #region Seed Data
        HasData(() => new Package[] {
            new Package() { PackageId = 1, PackageComments = "", PackageName = "System", StatusId = 1, StatusIdChangeDate = DateTime.Parse("2024-04-20T09:02:58.5600000Z").ToUniversalTime(), UserId = 1 }
        });
        #endregion

    }
}
