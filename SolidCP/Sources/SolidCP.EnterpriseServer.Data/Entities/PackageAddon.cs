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
[Index("PackageId", Name = "PackageAddonsIdx_PackageID")]
[Index("PlanId", Name = "PackageAddonsIdx_PlanID")]
#endif
public partial class PackageAddon
{
    [Key]
    [Column("PackageAddonID")]
    public int PackageAddonId { get; set; }

    [Column("PackageID")]
    public int? PackageId { get; set; }

    [Column("PlanID")]
    public int? PlanId { get; set; }

    public int? Quantity { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime? PurchaseDate { get; set; }

    //[Column(TypeName = "ntext")]
    public string Comments { get; set; }

    [Column("StatusID")]
    public int? StatusId { get; set; }

    [ForeignKey("PackageId")]
    [InverseProperty("PackageAddons")]
    public virtual Package Package { get; set; }

    [ForeignKey("PlanId")]
    [InverseProperty("PackageAddons")]
    public virtual HostingPlan Plan { get; set; }
}
#endif