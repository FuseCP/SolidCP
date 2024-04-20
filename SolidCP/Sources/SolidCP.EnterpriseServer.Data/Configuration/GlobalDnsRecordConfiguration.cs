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
        HasOne(d => d.Ipaddress).WithMany(p => p.GlobalDnsRecords).HasConstraintName("FK_GlobalDnsRecords_IPAddresses");

        HasOne(d => d.Package).WithMany(p => p.GlobalDnsRecords)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GlobalDnsRecords_Packages");

        HasOne(d => d.Server).WithMany(p => p.GlobalDnsRecords).HasConstraintName("FK_GlobalDnsRecords_Servers");

        HasOne(d => d.Service).WithMany(p => p.GlobalDnsRecords)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GlobalDnsRecords_Services");
    }
#endif
}
