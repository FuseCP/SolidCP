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

using GlobalDnsRecord = SolidCP.EnterpriseServer.Data.Entities.GlobalDnsRecord;

public partial class GlobalDnsRecordConfiguration: Extensions.EntityTypeConfiguration<GlobalDnsRecord>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public GlobalDnsRecordConfiguration(): base() { }
    public GlobalDnsRecordConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.RecordId);

        HasIndex(e => e.IpaddressId, "GlobalDnsRecordsIdx_IPAddressID");

        HasIndex(e => e.PackageId, "GlobalDnsRecordsIdx_PackageID");

        HasIndex(e => e.ServerId, "GlobalDnsRecordsIdx_ServerID");

        HasIndex(e => e.ServiceId, "GlobalDnsRecordsIdx_ServiceID");

        Property(e => e.RecordId).HasColumnName("RecordID");
        Property(e => e.IpaddressId).HasColumnName("IPAddressID");
        Property(e => e.Mxpriority).HasColumnName("MXPriority");
        Property(e => e.PackageId).HasColumnName("PackageID");
        Property(e => e.RecordData)
                .IsRequired()
                .HasMaxLength(500);
        Property(e => e.RecordName)
                .IsRequired()
                .HasMaxLength(50);
        Property(e => e.RecordType)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);
        Property(e => e.ServerId).HasColumnName("ServerID");
        Property(e => e.ServiceId).HasColumnName("ServiceID");

        HasOne(d => d.Ipaddress).WithMany(p => p.GlobalDnsRecords)
                .HasForeignKey(d => d.IpaddressId)
                .HasConstraintName("FK_GlobalDnsRecords_IPAddresses");

        HasOne(d => d.Package).WithMany(p => p.GlobalDnsRecords)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GlobalDnsRecords_Packages");

        HasOne(d => d.Server).WithMany(p => p.GlobalDnsRecords)
                .HasForeignKey(d => d.ServerId)
                .HasConstraintName("FK_GlobalDnsRecords_Servers");

        HasOne(d => d.Service).WithMany(p => p.GlobalDnsRecords)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GlobalDnsRecords_Services");
    }
#endif
}
