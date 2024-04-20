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

public partial class PackageVlanConfiguration: Extensions.EntityTypeConfiguration<PackageVlan>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public PackageVlanConfiguration(): base() { }
    public PackageVlanConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.PackageVlanId).HasName("PK__PackageV__A9AABBF9C0C25CB3");

        HasOne(d => d.Package).WithMany(p => p.PackageVlans).HasConstraintName("FK_PackageID");

        HasOne(d => d.Vlan).WithMany(p => p.PackageVlans).HasConstraintName("FK_VlanID");
    }
#endif
}
