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

public partial class ExchangeMailboxPlanConfiguration: EntityTypeConfiguration<ExchangeMailboxPlan>
{
    public override void Configure() {

        if (IsCore && IsSqlite) Property(e => e.MailboxPlan).HasColumnType("TEXT COLLATE NOCASE");

#if NetCore
        HasOne(d => d.Item).WithMany(p => p.ExchangeMailboxPlans).HasConstraintName("FK_ExchangeMailboxPlans_ExchangeOrganizations");
#else
        HasRequired(d => d.Item).WithMany(p => p.ExchangeMailboxPlans);
#endif
    }
}
