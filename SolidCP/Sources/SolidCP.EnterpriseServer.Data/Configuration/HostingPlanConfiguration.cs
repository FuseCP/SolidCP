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

public partial class HostingPlanConfiguration : EntityTypeConfiguration<HostingPlan>
{
#if NetCore || NetFX
	public override void Configure()
	{

		if (IsMsSql)
		{
			Property(e => e.PlanDescription).HasColumnType("ntext");
			Property(e => e.SetupPrice).HasColumnType("money");
			Property(e => e.RecurringPrice).HasColumnType("money");
		}
		else if (IsMySql || IsMariaDb || IsSqlite || IsPostgreSql)
		{
			Property(e => e.PlanDescription).HasColumnType("TEXT");
		}


#if NetCore
        HasOne(d => d.Package).WithMany(p => p.HostingPlans)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_HostingPlans_Packages");

        HasOne(d => d.Server).WithMany(p => p.HostingPlans).HasConstraintName("FK_HostingPlans_Servers");

        HasOne(d => d.User).WithMany(p => p.HostingPlans).HasConstraintName("FK_HostingPlans_Users");
#else
		// TODO EF cascade delete?
		HasRequired(d => d.Package).WithMany(p => p.HostingPlans).WillCascadeOnDelete();
		HasRequired(d => d.Server).WithMany(p => p.HostingPlans);
		HasRequired(d => d.User).WithMany(p => p.HostingPlans);
#endif
	}
#endif
}
