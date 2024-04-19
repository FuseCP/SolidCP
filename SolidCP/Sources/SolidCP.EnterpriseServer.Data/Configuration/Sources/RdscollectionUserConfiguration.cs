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

public partial class RdscollectionUserConfiguration: Extensions.EntityTypeConfiguration<RdscollectionUser>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public RdscollectionUserConfiguration(): base() { }
    public RdscollectionUserConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__RDSColle__3214EC2780141EF7");

        ToTable("RDSCollectionUsers");

        HasIndex(e => e.AccountId, "RDSCollectionUsersIdx_AccountID");

        HasIndex(e => e.RdscollectionId, "RDSCollectionUsersIdx_RDSCollectionId");

        Property(e => e.Id).HasColumnName("ID");
        Property(e => e.AccountId).HasColumnName("AccountID");
        Property(e => e.RdscollectionId).HasColumnName("RDSCollectionId");

        HasOne(d => d.Account).WithMany(p => p.RdscollectionUsers)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_RDSCollectionUsers_UserId");

        HasOne(d => d.Rdscollection).WithMany(p => p.RdscollectionUsers)
                .HasForeignKey(d => d.RdscollectionId)
                .HasConstraintName("FK_RDSCollectionUsers_RDSCollectionId");
    }
#endif
}
