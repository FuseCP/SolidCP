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

public partial class ExchangeMailboxPlanRetentionPolicyTagConfiguration : EntityTypeConfiguration<ExchangeMailboxPlanRetentionPolicyTag>
{
	public override void Configure()
	{
		HasKey(e => e.PlanTagId).HasName("PK__Exchange__E467073C50CD805B");
    }
}
