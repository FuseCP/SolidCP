#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[Table("Schedule")]
#if NetCore
[Index("PackageId", Name = "ScheduleIdx_PackageID")]
[Index("TaskId", Name = "ScheduleIdx_TaskID")]
#endif
public partial class Schedule
{
    [Key]
    [Column("ScheduleID")]
    public int ScheduleId { get; set; }

    [Required]
    [Column("TaskID")]
    [StringLength(100)]
    public string TaskId { get; set; }

    [Column("PackageID")]
    public int? PackageId { get; set; }

    [StringLength(100)]
    public string ScheduleName { get; set; }

    [Column("ScheduleTypeID")]
    [StringLength(50)]
    public string ScheduleTypeId { get; set; }

    public int? Interval { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime? FromTime { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime? ToTime { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime? StartTime { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime? LastRun { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime? NextRun { get; set; }

    public bool Enabled { get; set; }

    [Column("PriorityID")]
    [StringLength(50)]
    public string PriorityId { get; set; }

    public int? HistoriesNumber { get; set; }

    public int? MaxExecutionTime { get; set; }

    public int? WeekMonthDay { get; set; }

    [ForeignKey("PackageId")]
    [InverseProperty("Schedules")]
    public virtual Package Package { get; set; }

    [InverseProperty("Schedule")]
    public virtual ICollection<ScheduleParameter> ScheduleParameters { get; set; } = new List<ScheduleParameter>();

    [ForeignKey("TaskId")]
    [InverseProperty("Schedules")]
    public virtual ScheduleTask Task { get; set; }
}
#endif