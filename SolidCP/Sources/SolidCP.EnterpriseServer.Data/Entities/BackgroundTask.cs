#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class BackgroundTask
{
    public int Id { get; set; }

    public Guid Guid { get; set; }

    public string TaskId { get; set; }

    public int ScheduleId { get; set; }

    public int PackageId { get; set; }

    public int UserId { get; set; }

    public int EffectiveUserId { get; set; }

    public string TaskName { get; set; }

    public int? ItemId { get; set; }

    public string ItemName { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? FinishDate { get; set; }

    public int IndicatorCurrent { get; set; }

    public int IndicatorMaximum { get; set; }

    public int MaximumExecutionTime { get; set; }

    public string Source { get; set; }

    public int Severity { get; set; }

    public bool? Completed { get; set; }

    public bool? NotifyOnComplete { get; set; }

    public int Status { get; set; }

    public virtual ICollection<BackgroundTaskLog> BackgroundTaskLogs { get; set; } = new List<BackgroundTaskLog>();

    public virtual ICollection<BackgroundTaskParameter> BackgroundTaskParameters { get; set; } = new List<BackgroundTaskParameter>();

    public virtual ICollection<BackgroundTaskStack> BackgroundTaskStacks { get; set; } = new List<BackgroundTaskStack>();
}
#endif