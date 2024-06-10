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

public partial class CrmUserConfiguration : EntityTypeConfiguration<CrmUser>
{
#if NetCore || NetFX
	public override void Configure()
	{

#if NetCore
		if (IsMsSql)
		{
			Property(e => e.ChangedDate).HasDefaultValueSql("(getdate())");
			Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
		}

		HasOne(d => d.Account).WithMany(p => p.CrmUsers)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_CRMUsers_ExchangeAccounts");
#else
        HasRequired(d => d.Account).WithMany(p => p.CrmUsers);
#endif
	}
#endif
}
