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

public partial class ExchangeRetentionPolicyTagConfiguration: Extensions.EntityTypeConfiguration<ExchangeRetentionPolicyTag>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public ExchangeRetentionPolicyTagConfiguration(): base() { }
    public ExchangeRetentionPolicyTagConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.TagId).HasName("PK__Exchange__657CFA4C02667D37");

        Property(e => e.TagId).HasColumnName("TagID");
        Property(e => e.ItemId).HasColumnName("ItemID");
        Property(e => e.TagName).HasMaxLength(255);
    }
#endif
}
