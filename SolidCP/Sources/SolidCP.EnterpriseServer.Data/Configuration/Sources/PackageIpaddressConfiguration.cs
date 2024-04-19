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

public partial class PackageIpaddressConfiguration: Extensions.EntityTypeConfiguration<PackageIpaddress>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public PackageIpaddressConfiguration(): base() { }
    public PackageIpaddressConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.PackageAddressId);

        ToTable("PackageIPAddresses");

        HasIndex(e => e.AddressId, "PackageIPAddressesIdx_AddressID");

        HasIndex(e => e.ItemId, "PackageIPAddressesIdx_ItemID");

        HasIndex(e => e.PackageId, "PackageIPAddressesIdx_PackageID");

        Property(e => e.PackageAddressId).HasColumnName("PackageAddressID");
        Property(e => e.AddressId).HasColumnName("AddressID");
        Property(e => e.ItemId).HasColumnName("ItemID");
        Property(e => e.OrgId).HasColumnName("OrgID");
        Property(e => e.PackageId).HasColumnName("PackageID");

        HasOne(d => d.Address).WithMany(p => p.PackageIpaddresses)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageIPAddresses_IPAddresses");

        HasOne(d => d.Item).WithMany(p => p.PackageIpaddresses)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_PackageIPAddresses_ServiceItems");

        HasOne(d => d.Package).WithMany(p => p.PackageIpaddresses)
                .HasForeignKey(d => d.PackageId)
                .HasConstraintName("FK_PackageIPAddresses_Packages");
    }
#endif
}
