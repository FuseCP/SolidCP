// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities.Sources;

public partial class BackgroundTask
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public Guid Guid { get; set; }

    [Column("TaskID")]
    [StringLength(255)]
    public string TaskId { get; set; }

    [Column("ScheduleID")]
    public int ScheduleId { get; set; }

    [Column("PackageID")]
    public int PackageId { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Column("EffectiveUserID")]
    public int EffectiveUserId { get; set; }

    [StringLength(255)]
    public string TaskName { get; set; }

    [Column("ItemID")]
    public int? ItemId { get; set; }

    [StringLength(255)]
    public string ItemName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FinishDate { get; set; }

    public int IndicatorCurrent { get; set; }

    public int IndicatorMaximum { get; set; }

    public int MaximumExecutionTime { get; set; }

    public string Source { get; set; }

    public int Severity { get; set; }

    public bool? Completed { get; set; }

    public bool? NotifyOnComplete { get; set; }

    public int Status { get; set; }

    [InverseProperty("Task")]
    public virtual ICollection<BackgroundTaskLog> BackgroundTaskLogs { get; set; } = new List<BackgroundTaskLog>();

    [InverseProperty("Task")]
    public virtual ICollection<BackgroundTaskParameter> BackgroundTaskParameters { get; set; } = new List<BackgroundTaskParameter>();

    [InverseProperty("Task")]
    public virtual ICollection<BackgroundTaskStack> BackgroundTaskStacks { get; set; } = new List<BackgroundTaskStack>();
}
#endif