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

public partial class ExchangeOrganizationDomainConfiguration : Extensions.EntityTypeConfiguration<ExchangeOrganizationDomain>
{

	public ExchangeOrganizationDomainConfiguration() : base() { }
	public ExchangeOrganizationDomainConfiguration(DbFlavor flavor) : base(flavor) { }

#if NetCore || NetFX
	public override void Configure()
	{

#if NetCore
        Property(e => e.IsHost).HasDefaultValue(false);

        HasOne(d => d.Item).WithMany(p => p.ExchangeOrganizationDomains).HasConstraintName("FK_ExchangeOrganizationDomains_ServiceItems");
#else
		HasRequired(d => d.Item).WithMany(p => p.ExchangeOrganizationDomains);
#endif
	}
#endif
}
