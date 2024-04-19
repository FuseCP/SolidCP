// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public string TaskId { get; set; }

    public int? PackageId { get; set; }

    public string ScheduleName { get; set; }

    public string ScheduleTypeId { get; set; }

    public int? Interval { get; set; }

    public DateTime? FromTime { get; set; }

    public DateTime? ToTime { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? LastRun { get; set; }

    public DateTime? NextRun { get; set; }

    public bool Enabled { get; set; }

    public string PriorityId { get; set; }

    public int? HistoriesNumber { get; set; }

    public int? MaxExecutionTime { get; set; }

    public int? WeekMonthDay { get; set; }

    public virtual Package Package { get; set; }

    public virtual ICollection<ScheduleParameter> ScheduleParameters { get; set; } = new List<ScheduleParameter>();

    public virtual ScheduleTask Task { get; set; }
}
#endif