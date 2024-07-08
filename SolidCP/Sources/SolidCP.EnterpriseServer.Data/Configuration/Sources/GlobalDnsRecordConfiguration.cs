// This file is auto generated, do not edit.
using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
using SolidCP.EnterpriseServer.Data.Extensions;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

using GlobalDnsRecord = SolidCP.EnterpriseServer.Data.Entities.GlobalDnsRecord;

public partial class GlobalDnsRecordConfiguration: EntityTypeConfiguration<GlobalDnsRecord>
{
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
}
