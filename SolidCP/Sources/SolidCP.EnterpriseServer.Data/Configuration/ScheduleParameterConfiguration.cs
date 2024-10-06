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

public partial class ScheduleParameterConfiguration : EntityTypeConfiguration<ScheduleParameter>
{
	public override void Configure()
	{
		if (IsCore && IsSqlite) Property(e => e.ParameterId).HasColumnType("TEXT COLLATE NOCASE");

#if NetCore
        HasOne(d => d.Schedule).WithMany(p => p.ScheduleParameters).HasConstraintName("FK_ScheduleParameters_Schedule");
#else
		HasRequired(d => d.Schedule).WithMany(p => p.ScheduleParameters);
#endif

		#region Seed Data
		HasData(() => new ScheduleParameter[] {
			new ScheduleParameter() { ScheduleId = 1, ParameterId = "SUSPEND_OVERUSED", ParameterValue = "false" },
			new ScheduleParameter() { ScheduleId = 2, ParameterId = "SUSPEND_OVERUSED", ParameterValue = "false" }
		});
		#endregion
    }
}
