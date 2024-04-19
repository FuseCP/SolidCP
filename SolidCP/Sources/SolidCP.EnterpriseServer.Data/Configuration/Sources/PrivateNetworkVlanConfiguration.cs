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

public partial class PrivateNetworkVlanConfiguration: Extensions.EntityTypeConfiguration<PrivateNetworkVlan>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public PrivateNetworkVlanConfiguration(): base() { }
    public PrivateNetworkVlanConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.VlanId).HasName("PK__PrivateN__8348135581B53618");

        ToTable("PrivateNetworkVLANs");

        HasIndex(e => e.ServerId, "PrivateNetworkVLANsIdx_ServerID");

        Property(e => e.VlanId).HasColumnName("VlanID");
        Property(e => e.Comments).HasColumnType("ntext");
        Property(e => e.ServerId).HasColumnName("ServerID");

        HasOne(d => d.Server).WithMany(p => p.PrivateNetworkVlans)
                .HasForeignKey(d => d.ServerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ServerID");
    }
#endif
}
