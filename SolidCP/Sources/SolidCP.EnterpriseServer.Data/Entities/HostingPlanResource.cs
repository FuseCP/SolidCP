#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

#if NetCore
[PrimaryKey("PlanId", "GroupId")]
#endif
public partial class HostingPlanResource
{
    [Key]
    [Column("PlanID", Order = 1)]
    public int PlanId { get; set; }

    [Key]
    [Column("GroupID", Order = 2)]
    public int GroupId { get; set; }

    public bool? CalculateDiskSpace { get; set; }

    public bool? CalculateBandwidth { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("HostingPlanResources")]
    public virtual ResourceGroup Group { get; set; }

    [ForeignKey("PlanId")]
    [InverseProperty("HostingPlanResources")]
    public virtual HostingPlan Plan { get; set; }
}
#endif