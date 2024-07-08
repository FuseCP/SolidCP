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

public partial class ExchangeMailboxPlanConfiguration: EntityTypeConfiguration<ExchangeMailboxPlan>
{
    public override void Configure() {
        HasOne(d => d.Item).WithMany(p => p.ExchangeMailboxPlans).HasConstraintName("FK_ExchangeMailboxPlans_ExchangeOrganizations");
    }
}
