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

public partial class RdscollectionConfiguration: Extensions.EntityTypeConfiguration<Rdscollection>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public RdscollectionConfiguration(): base() { }
    public RdscollectionConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__RDSColle__3214EC27346D361D");

        ToTable("RDSCollections");

        Property(e => e.Id).HasColumnName("ID");
        Property(e => e.Description).HasMaxLength(255);
        Property(e => e.DisplayName).HasMaxLength(255);
        Property(e => e.ItemId).HasColumnName("ItemID");
        Property(e => e.Name).HasMaxLength(255);
    }
#endif
}
