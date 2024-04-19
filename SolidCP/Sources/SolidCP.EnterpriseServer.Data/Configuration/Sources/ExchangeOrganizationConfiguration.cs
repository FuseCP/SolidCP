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

public partial class ExchangeOrganizationConfiguration: Extensions.EntityTypeConfiguration<ExchangeOrganization>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public ExchangeOrganizationConfiguration(): base() { }
    public ExchangeOrganizationConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.ItemId);

        HasIndex(e => e.OrganizationId, "IX_ExchangeOrganizations_UniqueOrg").IsUnique();

        Property(e => e.ItemId)
                .ValueGeneratedNever()
                .HasColumnName("ItemID");
        Property(e => e.ExchangeMailboxPlanId).HasColumnName("ExchangeMailboxPlanID");
        Property(e => e.LyncUserPlanId).HasColumnName("LyncUserPlanID");
        Property(e => e.OrganizationId)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnName("OrganizationID");
        Property(e => e.SfBuserPlanId).HasColumnName("SfBUserPlanID");

        HasOne(d => d.Item).WithOne(p => p.ExchangeOrganization)
                .HasForeignKey<ExchangeOrganization>(d => d.ItemId)
                .HasConstraintName("FK_ExchangeOrganizations_ServiceItems");
    }
#endif
}
