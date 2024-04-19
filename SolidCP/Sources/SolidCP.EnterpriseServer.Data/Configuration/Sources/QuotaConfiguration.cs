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

public partial class QuotaConfiguration: Extensions.EntityTypeConfiguration<Quota>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public QuotaConfiguration(): base() { }
    public QuotaConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasIndex(e => e.GroupId, "QuotasIdx_GroupID");

        HasIndex(e => e.ItemTypeId, "QuotasIdx_ItemTypeID");

        Property(e => e.QuotaId)
                .ValueGeneratedNever()
                .HasColumnName("QuotaID");
        Property(e => e.GroupId).HasColumnName("GroupID");
        Property(e => e.ItemTypeId).HasColumnName("ItemTypeID");
        Property(e => e.QuotaDescription).HasMaxLength(200);
        Property(e => e.QuotaName)
                .IsRequired()
                .HasMaxLength(50);
        Property(e => e.QuotaOrder).HasDefaultValue(1);
        Property(e => e.QuotaTypeId)
                .HasDefaultValue(2)
                .HasColumnName("QuotaTypeID");
        Property(e => e.ServiceQuota).HasDefaultValue(false);

        HasOne(d => d.Group).WithMany(p => p.Quota)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK_Quotas_ResourceGroups");

        HasOne(d => d.ItemType).WithMany(p => p.Quota)
                .HasForeignKey(d => d.ItemTypeId)
                .HasConstraintName("FK_Quotas_ServiceItemTypes");
    }
#endif
}
