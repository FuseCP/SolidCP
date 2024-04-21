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

public partial class ScheduleConfiguration: Extensions.EntityTypeConfiguration<Schedule>
{
    public ScheduleConfiguration(): base() { }
    public ScheduleConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasOne(d => d.Package).WithMany(p => p.Schedules)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Schedule_Packages");

        HasOne(d => d.Task).WithMany(p => p.Schedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_ScheduleTasks");

#region Seed Data
        HasData(
            new Schedule() { ScheduleId = 1, Enabled = true, FromTime = DateTime.Parse("2000-01-01T12:00:00.0000000"), HistoriesNumber = 7, Interval = 0, LastRun = null,
                MaxExecutionTime = 3600, NextRun = DateTime.Parse("2010-07-16T14:53:02.4700000"), PackageId = 1, PriorityId = "Normal", ScheduleName = "Calculate Disk Space", ScheduleTypeId = "Daily",
                StartTime = DateTime.Parse("2000-01-01T12:30:00.0000000"), TaskId = "SCHEDULE_TASK_CALCULATE_PACKAGES_DISKSPACE", ToTime = DateTime.Parse("2000-01-01T12:00:00.0000000"), WeekMonthDay = 1 },
            new Schedule() { ScheduleId = 2, Enabled = true, FromTime = DateTime.Parse("2000-01-01T12:00:00.0000000"), HistoriesNumber = 7, Interval = 0, LastRun = null,
                MaxExecutionTime = 3600, NextRun = DateTime.Parse("2010-07-16T14:53:02.4770000"), PackageId = 1, PriorityId = "Normal", ScheduleName = "Calculate Bandwidth", ScheduleTypeId = "Daily",
                StartTime = DateTime.Parse("2000-01-01T12:00:00.0000000"), TaskId = "SCHEDULE_TASK_CALCULATE_PACKAGES_BANDWIDTH", ToTime = DateTime.Parse("2000-01-01T12:00:00.0000000"), WeekMonthDay = 1 }
        );
#endregion

    }
#endif
}
