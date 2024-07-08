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

public partial class PackageIpAddressConfiguration: EntityTypeConfiguration<PackageIpAddress>
{
    public override void Configure() {

#if NetCore
        HasOne(d => d.Address).WithMany(p => p.PackageIpAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageIPAddresses_IPAddresses");

        HasOne(d => d.Item).WithMany(p => p.PackageIpAddresses).HasConstraintName("FK_PackageIPAddresses_ServiceItems");

        HasOne(d => d.Package).WithMany(p => p.PackageIpAddresses).HasConstraintName("FK_PackageIPAddresses_Packages");
#else
        HasRequired(d => d.Address).WithMany(p => p.PackageIpAddresses);
        HasRequired(d => d.Item).WithMany(p => p.PackageIpAddresses);
        HasRequired(d => d.Package).WithMany(p => p.PackageIpAddresses);
#endif
    }
}
