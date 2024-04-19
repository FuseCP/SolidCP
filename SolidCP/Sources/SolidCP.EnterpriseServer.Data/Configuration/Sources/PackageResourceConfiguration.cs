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

public partial class PackageResourceConfiguration: Extensions.EntityTypeConfiguration<PackageResource>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public PackageResourceConfiguration(): base() { }
    public PackageResourceConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => new { e.PackageId, e.GroupId }).HasName("PK_PackageResources_1");

        Property(e => e.PackageId).HasColumnName("PackageID");
        Property(e => e.GroupId).HasColumnName("GroupID");

        HasOne(d => d.Group).WithMany(p => p.PackageResources)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageResources_ResourceGroups");

        HasOne(d => d.Package).WithMany(p => p.PackageResources)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageResources_Packages");
    }
#endif
}
