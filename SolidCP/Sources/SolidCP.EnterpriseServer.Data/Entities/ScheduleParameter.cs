#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class ScheduleParameter
{
    public int ScheduleId { get; set; }

    public string ParameterId { get; set; }

    public string ParameterValue { get; set; }

    public virtual Schedule Schedule { get; set; }
}
#endif