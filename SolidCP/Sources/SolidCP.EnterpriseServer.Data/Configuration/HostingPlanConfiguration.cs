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
	public override void Configure()
	{

		if (IsSqlServer)
		{
			Property(e => e.PlanDescription).HasColumnType("ntext");
			Property(e => e.SetupPrice).HasColumnType("money");
			Property(e => e.RecurringPrice).HasColumnType("money");
		}
		else if (IsCore && (IsMySql || IsMariaDb || IsSqlite || IsPostgreSql))
		{
			Property(e => e.PlanDescription).HasColumnType("TEXT");
		}


#if NetCore
        HasMany(d => d.Packages).WithOne(p => p.HostingPlan)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_HostingPlans_Packages");

        HasOne(d => d.Server).WithMany(p => p.HostingPlans).HasConstraintName("FK_HostingPlans_Servers");

        HasOne(d => d.User).WithMany(p => p.HostingPlans).HasConstraintName("FK_HostingPlans_Users");
#else
		HasMany(d => d.Packages).WithOptional(p => p.HostingPlan).WillCascadeOnDelete();
		HasOptional(d => d.Server).WithMany(p => p.HostingPlans);
		HasOptional(d => d.User).WithMany(p => p.HostingPlans);
#endif
    }
}
