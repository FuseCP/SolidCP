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

public partial class PackageVlanConfiguration: EntityTypeConfiguration<PackageVlan>
{
    public override void Configure() {
        HasKey(e => e.PackageVlanId).HasName("PK__PackageV__A9AABBF954AA28C0");

        HasOne(d => d.Package).WithMany(p => p.PackageVlans).HasConstraintName("FK_PackageID");

        HasOne(d => d.Vlan).WithMany(p => p.PackageVlans).HasConstraintName("FK_VlanID");
    }
}
