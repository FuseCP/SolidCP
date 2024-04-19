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

public partial class IpaddressConfiguration: Extensions.EntityTypeConfiguration<Ipaddress>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public IpaddressConfiguration(): base() { }
    public IpaddressConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.AddressId);

        ToTable("IPAddresses");

        HasIndex(e => e.ServerId, "IPAddressesIdx_ServerID");

        Property(e => e.AddressId).HasColumnName("AddressID");
        Property(e => e.Comments).HasColumnType("ntext");
        Property(e => e.DefaultGateway)
                .HasMaxLength(15)
                .IsUnicode(false);
        Property(e => e.ExternalIp)
                .IsRequired()
                .HasMaxLength(24)
                .IsUnicode(false)
                .HasColumnName("ExternalIP");
        Property(e => e.InternalIp)
                .HasMaxLength(24)
                .IsUnicode(false)
                .HasColumnName("InternalIP");
        Property(e => e.PoolId).HasColumnName("PoolID");
        Property(e => e.ServerId).HasColumnName("ServerID");
        Property(e => e.SubnetMask)
                .HasMaxLength(15)
                .IsUnicode(false);
        Property(e => e.Vlan).HasColumnName("VLAN");

        HasOne(d => d.Server).WithMany(p => p.Ipaddresses)
                .HasForeignKey(d => d.ServerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_IPAddresses_Servers");
    }
#endif
}
