// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class HostingPlan
{
    public int PlanId { get; set; }

    public int? UserId { get; set; }

    public int? PackageId { get; set; }

    public int? ServerId { get; set; }

    public string PlanName { get; set; }

    public string PlanDescription { get; set; }

    public bool Available { get; set; }

    public decimal? SetupPrice { get; set; }

    public decimal? RecurringPrice { get; set; }

    public int? RecurrenceUnit { get; set; }

    public int? RecurrenceLength { get; set; }

    public bool? IsAddon { get; set; }

    public virtual ICollection<HostingPlanQuota> HostingPlanQuota { get; set; } = new List<HostingPlanQuota>();

    public virtual ICollection<HostingPlanResource> HostingPlanResources { get; set; } = new List<HostingPlanResource>();

    public virtual Package Package { get; set; }

    public virtual ICollection<PackageAddon> PackageAddons { get; set; } = new List<PackageAddon>();

    public virtual ICollection<Package> Packages { get; set; } = new List<Package>();

    public virtual Server Server { get; set; }

    public virtual User User { get; set; }
}
#endif