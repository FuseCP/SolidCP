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

public partial class DomainDnsRecordConfiguration: Extensions.EntityTypeConfiguration<DomainDnsRecord>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public DomainDnsRecordConfiguration(): base() { }
    public DomainDnsRecordConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__DomainDn__3214EC2758B0A6F1");

        HasIndex(e => e.DomainId, "DomainDnsRecordsIdx_DomainId");

        Property(e => e.Id).HasColumnName("ID");
        Property(e => e.Date).HasColumnType("datetime");
        Property(e => e.DnsServer).HasMaxLength(255);
        Property(e => e.Value).HasMaxLength(255);

        HasOne(d => d.Domain).WithMany(p => p.DomainDnsRecords)
                .HasForeignKey(d => d.DomainId)
                .HasConstraintName("FK_DomainDnsRecords_DomainId");
    }
#endif
}
