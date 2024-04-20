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

public partial class ResourceGroupDnsRecordConfiguration : Extensions.EntityTypeConfiguration<ResourceGroupDnsRecord>
{
	public ResourceGroupDnsRecordConfiguration() : base() { }
	public ResourceGroupDnsRecordConfiguration(DbFlavor flavor) : base(flavor) { }

#if NetCore || NetFX
	public override void Configure()
	{

#if NetCore
		Property(e => e.RecordOrder).HasDefaultValue(1);

		HasOne(d => d.Group).WithMany(p => p.ResourceGroupDnsRecords).HasConstraintName("FK_ResourceGroupDnsRecords_ResourceGroups");
#else
		HasRequired(d => d.Group).WithMany(p => p.ResourceGroupDnsRecords);
#endif
	}
#endif
}
