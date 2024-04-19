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

public partial class ExchangeOrganizationDomainConfiguration: Extensions.EntityTypeConfiguration<ExchangeOrganizationDomain>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public ExchangeOrganizationDomainConfiguration(): base() { }
    public ExchangeOrganizationDomainConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.OrganizationDomainId);

        HasIndex(e => e.ItemId, "ExchangeOrganizationDomainsIdx_ItemID");

        HasIndex(e => e.DomainId, "IX_ExchangeOrganizationDomains_UniqueDomain").IsUnique();

        Property(e => e.OrganizationDomainId).HasColumnName("OrganizationDomainID");
        Property(e => e.DomainId).HasColumnName("DomainID");
        Property(e => e.DomainTypeId).HasColumnName("DomainTypeID");
        Property(e => e.IsHost).HasDefaultValue(false);
        Property(e => e.ItemId).HasColumnName("ItemID");

        HasOne(d => d.Item).WithMany(p => p.ExchangeOrganizationDomains)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_ExchangeOrganizationDomains_ServiceItems");
    }
#endif
}
