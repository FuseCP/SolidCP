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
[PrimaryKey("PlanId", "QuotaId")]
#endif
public partial class HostingPlanQuota
{
    [Key]
    [Column("PlanID", Order = 1)]
    public int PlanId { get; set; }

    [Key]
    [Column("QuotaID", Order = 2)]
    public int QuotaId { get; set; }

    public int QuotaValue { get; set; }

    [ForeignKey("PlanId")]
    [InverseProperty("HostingPlanQuota")]
    public virtual HostingPlan Plan { get; set; }

    [ForeignKey("QuotaId")]
    [InverseProperty("HostingPlanQuota")]
    public virtual Quota Quota { get; set; }
}
#endif