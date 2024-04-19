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

public partial class ServiceItemPropertyConfiguration: Extensions.EntityTypeConfiguration<ServiceItemProperty>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public ServiceItemPropertyConfiguration(): base() { }
    public ServiceItemPropertyConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => new { e.ItemId, e.PropertyName });

        Property(e => e.ItemId).HasColumnName("ItemID");
        Property(e => e.PropertyName).HasMaxLength(50);

        HasOne(d => d.Item).WithMany(p => p.ServiceItemProperties)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_ServiceItemProperties_ServiceItems");
    }
#endif
}
