// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class ScheduleTaskViewConfiguration
{
    public string TaskId { get; set; }

    public string ConfigurationId { get; set; }

    public string Environment { get; set; }

    public string Description { get; set; }

    public virtual ScheduleTask Task { get; set; }
}
#endif