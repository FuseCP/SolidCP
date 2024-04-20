using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class PackageVlanConfiguration: Extensions.EntityTypeConfiguration<PackageVlan>
{
    public PackageVlanConfiguration(): base() { }
    public PackageVlanConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.PackageVlanId).HasName("PK__PackageV__A9AABBF9C0C25CB3");

#if NetCore
        HasOne(d => d.Package).WithMany(p => p.PackageVlans).HasConstraintName("FK_PackageID");

        HasOne(d => d.Vlan).WithMany(p => p.PackageVlans).HasConstraintName("FK_VlanID");
#else
        HasRequired(d => d.Package).WithMany(p => p.PackageVlans);
        HasRequired(d => d.Vlan).WithMany(p => p.PackageVlans);
#endif
    }
#endif
}
