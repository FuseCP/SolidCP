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

public partial class ExchangeAccountConfiguration : EntityTypeConfiguration<ExchangeAccount>
{
#if NetCore || NetFX
	public override void Configure()
	{

#if NetCore
        if (IsMsSql) Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

        HasOne(d => d.Item).WithMany(p => p.ExchangeAccounts).HasConstraintName("FK_ExchangeAccounts_ServiceItems");

        HasOne(d => d.MailboxPlan).WithMany(p => p.ExchangeAccounts).HasConstraintName("FK_ExchangeAccounts_ExchangeMailboxPlans");
#else
		HasRequired(d => d.Item).WithMany(p => p.ExchangeAccounts);
		// TODO optional or required?
		HasOptional(d => d.MailboxPlan).WithMany(p => p.ExchangeAccounts);
#endif
	}
#endif
}
