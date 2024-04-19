// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class PackageAddon
{
    public int PackageAddonId { get; set; }

    public int? PackageId { get; set; }

    public int? PlanId { get; set; }

    public int? Quantity { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public string Comments { get; set; }

    public int? StatusId { get; set; }

    public virtual Package Package { get; set; }

    public virtual HostingPlan Plan { get; set; }
}
#endif