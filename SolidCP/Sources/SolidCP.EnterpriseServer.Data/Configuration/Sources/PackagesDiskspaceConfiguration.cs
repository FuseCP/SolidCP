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

public partial class PackagesDiskspaceConfiguration: Extensions.EntityTypeConfiguration<PackagesDiskspace>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public PackagesDiskspaceConfiguration(): base() { }
    public PackagesDiskspaceConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => new { e.PackageId, e.GroupId });

        ToTable("PackagesDiskspace");

        Property(e => e.PackageId).HasColumnName("PackageID");
        Property(e => e.GroupId).HasColumnName("GroupID");

        HasOne(d => d.Group).WithMany(p => p.PackagesDiskspaces)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagesDiskspace_ResourceGroups");

        HasOne(d => d.Package).WithMany(p => p.PackagesDiskspaces)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagesDiskspace_Packages");
    }
#endif
}
