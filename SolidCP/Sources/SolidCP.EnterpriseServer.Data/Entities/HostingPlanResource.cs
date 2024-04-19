// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class HostingPlanResource
{
    public int PlanId { get; set; }

    public int GroupId { get; set; }

    public bool? CalculateDiskSpace { get; set; }

    public bool? CalculateBandwidth { get; set; }

    public virtual ResourceGroup Group { get; set; }

    public virtual HostingPlan Plan { get; set; }
}
#endif