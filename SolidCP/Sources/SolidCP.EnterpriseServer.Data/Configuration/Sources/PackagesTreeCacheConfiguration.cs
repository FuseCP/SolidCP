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

public partial class PackagesTreeCacheConfiguration: Extensions.EntityTypeConfiguration<PackagesTreeCache>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public PackagesTreeCacheConfiguration(): base() { }
    public PackagesTreeCacheConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasNoKey().ToTable("PackagesTreeCache");

        HasIndex(e => new { e.ParentPackageId, e.PackageId }, "PackagesTreeCacheIndex").IsClustered();

        Property(e => e.PackageId).HasColumnName("PackageID");
        Property(e => e.ParentPackageId).HasColumnName("ParentPackageID");

        HasOne(d => d.Package).WithMany()
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagesTreeCache_Packages1");

        HasOne(d => d.ParentPackage).WithMany()
                .HasForeignKey(d => d.ParentPackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagesTreeCache_Packages");
    }
#endif
}
