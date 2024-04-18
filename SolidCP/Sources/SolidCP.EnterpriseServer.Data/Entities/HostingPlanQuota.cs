#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class HostingPlanQuota
{
    public int PlanId { get; set; }

    public int QuotaId { get; set; }

    public int QuotaValue { get; set; }

    public virtual HostingPlan Plan { get; set; }

    public virtual Quota Quota { get; set; }
}
#endif