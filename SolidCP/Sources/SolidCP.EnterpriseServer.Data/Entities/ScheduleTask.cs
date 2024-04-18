#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class ScheduleTask
{
    public string TaskId { get; set; }

    public string TaskType { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<ScheduleTaskParameter> ScheduleTaskParameters { get; set; } = new List<ScheduleTaskParameter>();

    public virtual ICollection<ScheduleTaskViewConfiguration> ScheduleTaskViewConfigurations { get; set; } = new List<ScheduleTaskViewConfiguration>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
#endif