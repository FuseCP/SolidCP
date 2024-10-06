using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

using GlobalDnsRecord = SolidCP.EnterpriseServer.Data.Entities.GlobalDnsRecord;

public partial class GlobalDnsRecordConfiguration : EntityTypeConfiguration<GlobalDnsRecord>
{
	public override void Configure()
	{
		Property(e => e.RecordType).IsUnicode(false);

		if (IsCore && IsSqlite) Property(e => e.RecordType).HasColumnType("TEXT COLLATE NOCASE");

#if NetCore
        HasOne(d => d.IpAddress).WithMany(p => p.GlobalDnsRecords).HasConstraintName("FK_GlobalDnsRecords_IPAddresses");

        HasOne(d => d.Package).WithMany(p => p.GlobalDnsRecords)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GlobalDnsRecords_Packages");

        HasOne(d => d.Server).WithMany(p => p.GlobalDnsRecords).HasConstraintName("FK_GlobalDnsRecords_Servers");

        HasOne(d => d.Service).WithMany(p => p.GlobalDnsRecords)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GlobalDnsRecords_Services");
#else
		HasRequired(d => d.IpAddress).WithMany(p => p.GlobalDnsRecords);
		HasRequired(d => d.Package).WithMany(p => p.GlobalDnsRecords);
		HasRequired(d => d.Server).WithMany(p => p.GlobalDnsRecords);
		HasRequired(d => d.Service).WithMany(p => p.GlobalDnsRecords);
#endif

    }
}
