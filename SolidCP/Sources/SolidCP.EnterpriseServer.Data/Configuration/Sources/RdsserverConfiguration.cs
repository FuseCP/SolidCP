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

public partial class RdsserverConfiguration: Extensions.EntityTypeConfiguration<Rdsserver>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public RdsserverConfiguration(): base() { }
    public RdsserverConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__RDSServe__3214EC27DBEBD4B5");

        ToTable("RDSServers");

        HasIndex(e => e.RdscollectionId, "RDSServersIdx_RDSCollectionId");

        Property(e => e.Id).HasColumnName("ID");
        Property(e => e.ConnectionEnabled).HasDefaultValue(true);
        Property(e => e.Description).HasMaxLength(255);
        Property(e => e.FqdName).HasMaxLength(255);
        Property(e => e.ItemId).HasColumnName("ItemID");
        Property(e => e.Name).HasMaxLength(255);
        Property(e => e.RdscollectionId).HasColumnName("RDSCollectionId");

        HasOne(d => d.Rdscollection).WithMany(p => p.Rdsservers)
                .HasForeignKey(d => d.RdscollectionId)
                .HasConstraintName("FK_RDSServers_RDSCollectionId");
    }
#endif
}
