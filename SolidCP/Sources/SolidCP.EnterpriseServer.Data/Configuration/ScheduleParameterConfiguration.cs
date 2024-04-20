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

public partial class ScheduleParameterConfiguration : Extensions.EntityTypeConfiguration<ScheduleParameter>
{
	public ScheduleParameterConfiguration() : base() { }
	public ScheduleParameterConfiguration(DbFlavor flavor) : base(flavor) { }

#if NetCore || NetFX
	public override void Configure()
	{

#if NetCore
        HasOne(d => d.Schedule).WithMany(p => p.ScheduleParameters).HasConstraintName("FK_ScheduleParameters_Schedule");
#else
		HasRequired(d => d.Schedule).WithMany(p => p.ScheduleParameters);
#endif
	}
#endif
}
