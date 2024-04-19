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

public partial class RdsmessageConfiguration: Extensions.EntityTypeConfiguration<Rdsmessage>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public RdsmessageConfiguration(): base() { }
    public RdsmessageConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        ToTable("RDSMessages");

        HasIndex(e => e.RdscollectionId, "RDSMessagesIdx_RDSCollectionId");

        Property(e => e.Date).HasColumnType("datetime");
        Property(e => e.MessageText)
                .IsRequired()
                .HasColumnType("ntext");
        Property(e => e.RdscollectionId).HasColumnName("RDSCollectionId");
        Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(250)
                .IsFixedLength();

        HasOne(d => d.Rdscollection).WithMany(p => p.Rdsmessages)
                .HasForeignKey(d => d.RdscollectionId)
                .HasConstraintName("FK_RDSMessages_RDSCollections");
    }
#endif
}
