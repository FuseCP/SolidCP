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

public partial class ScheduleConfiguration : EntityTypeConfiguration<Schedule>
{
#if NetCore || NetFX
	public override void Configure()
	{

		if (IsMsSql)
		{
			Property(e => e.FromTime).HasColumnType("datetime");
			Property(e => e.ToTime).HasColumnType("datetime");
			Property(e => e.StartTime).HasColumnType("datetime");
			Property(e => e.NextRun).HasColumnType("datetime");
			Property(e => e.LastRun).HasColumnType("datetime");
		}

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

		#region Seed Data
		HasData(() => new Schedule[] {
			new Schedule() { ScheduleId = 1, Enabled = true, FromTime = DateTime.Parse("2000-01-01T12:00:00.0000000Z"), HistoriesNumber = 7, Interval = 0, MaxExecutionTime = 3600,
				NextRun = DateTime.Parse("2010-07-16T14:53:02.4700000Z"), PackageId = 1, PriorityId = "Normal", ScheduleName = "Calculate Disk Space", ScheduleTypeId = "Daily", StartTime = DateTime.Parse("2000-01-01T12:30:00.0000000Z"),
				TaskId = "SCHEDULE_TASK_CALCULATE_PACKAGES_DISKSPACE", ToTime = DateTime.Parse("2000-01-01T12:00:00.0000000Z"), WeekMonthDay = 1 },
			new Schedule() { ScheduleId = 2, Enabled = true, FromTime = DateTime.Parse("2000-01-01T12:00:00.0000000Z"), HistoriesNumber = 7, Interval = 0, MaxExecutionTime = 3600,
				NextRun = DateTime.Parse("2010-07-16T14:53:02.4770000Z"), PackageId = 1, PriorityId = "Normal", ScheduleName = "Calculate Bandwidth", ScheduleTypeId = "Daily", StartTime = DateTime.Parse("2000-01-01T12:00:00.0000000Z"),
				TaskId = "SCHEDULE_TASK_CALCULATE_PACKAGES_BANDWIDTH", ToTime = DateTime.Parse("2000-01-01T12:00:00.0000000Z"), WeekMonthDay = 1 }
		});
		#endregion
	}
#endif
}
