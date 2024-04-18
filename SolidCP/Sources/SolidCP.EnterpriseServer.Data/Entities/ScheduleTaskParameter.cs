#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class ScheduleTaskParameter
{
    public string TaskId { get; set; }

    public string ParameterId { get; set; }

    public string DataTypeId { get; set; }

    public string DefaultValue { get; set; }

    public int ParameterOrder { get; set; }

    public virtual ScheduleTask Task { get; set; }
}
#endif