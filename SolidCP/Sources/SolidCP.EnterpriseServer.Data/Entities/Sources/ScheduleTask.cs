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

public partial class ScheduleTask
{
    [Key]
    [Column("TaskID")]
    [StringLength(100)]
    public string TaskId { get; set; }

    [Required]
    [StringLength(500)]
    public string TaskType { get; set; }

    [Column("RoleID")]
    public int RoleId { get; set; }

    [InverseProperty("Task")]
    public virtual ICollection<ScheduleTaskParameter> ScheduleTaskParameters { get; set; } = new List<ScheduleTaskParameter>();

    [InverseProperty("Task")]
    public virtual ICollection<ScheduleTaskViewConfiguration> ScheduleTaskViewConfigurations { get; set; } = new List<ScheduleTaskViewConfiguration>();

    [InverseProperty("Task")]
    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
#endif