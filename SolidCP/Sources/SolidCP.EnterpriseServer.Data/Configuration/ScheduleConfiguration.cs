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

public partial class ScheduleConfiguration : Extensions.EntityTypeConfiguration<Schedule>
{
	public ScheduleConfiguration() : base() { }
	public ScheduleConfiguration(DbFlavor flavor) : base(flavor) { }

#if NetCore || NetFX
	public override void Configure()
	{

#if NetCore
        HasOne(d => d.Package).WithMany(p => p.Schedules)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Schedule_Packages");

        HasOne(d => d.Task).WithMany(p => p.Schedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_ScheduleTasks");
#else
		HasRequired(d => d.Package).WithMany(p => p.Schedules).WillCascadeOnDelete();
		HasRequired(d => d.Task).WithMany(p => p.Schedules);
#endif
	}
#endif
}
