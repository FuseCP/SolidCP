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

public partial class ServiceItemConfiguration: Extensions.EntityTypeConfiguration<ServiceItem>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public ServiceItemConfiguration(): base() { }
    public ServiceItemConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.ItemId);

        HasIndex(e => e.ItemTypeId, "ServiceItemsIdx_ItemTypeID");

        HasIndex(e => e.PackageId, "ServiceItemsIdx_PackageID");

        HasIndex(e => e.ServiceId, "ServiceItemsIdx_ServiceID");

        Property(e => e.ItemId).HasColumnName("ItemID");
        Property(e => e.CreatedDate).HasColumnType("datetime");
        Property(e => e.ItemName).HasMaxLength(500);
        Property(e => e.ItemTypeId).HasColumnName("ItemTypeID");
        Property(e => e.PackageId).HasColumnName("PackageID");
        Property(e => e.ServiceId).HasColumnName("ServiceID");

        HasOne(d => d.ItemType).WithMany(p => p.ServiceItems)
                .HasForeignKey(d => d.ItemTypeId)
                .HasConstraintName("FK_ServiceItems_ServiceItemTypes");

        HasOne(d => d.Package).WithMany(p => p.ServiceItems)
                .HasForeignKey(d => d.PackageId)
                .HasConstraintName("FK_ServiceItems_Packages");

        HasOne(d => d.Service).WithMany(p => p.ServiceItems)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK_ServiceItems_Services");
    }
#endif
}
