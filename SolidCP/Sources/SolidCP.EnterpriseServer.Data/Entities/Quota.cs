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
[Index("GroupId", Name = "QuotasIdx_GroupID")]
[Index("ItemTypeId", Name = "QuotasIdx_ItemTypeID")]
#endif
public partial class Quota
{
    [Key]
    [Column("QuotaID")]
    public int QuotaId { get; set; }

    [Column("GroupID")]
    public int GroupId { get; set; }

    public int QuotaOrder { get; set; }

    [Required]
    [StringLength(50)]
    public string QuotaName { get; set; }

    [StringLength(200)]
    public string QuotaDescription { get; set; }

    [Column("QuotaTypeID")]
    public int QuotaTypeId { get; set; }

    public bool? ServiceQuota { get; set; }

    [Column("ItemTypeID")]
    public int? ItemTypeId { get; set; }

    public bool? HideQuota { get; set; }

    public int? PerOrganization { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("Quota")]
    public virtual ResourceGroup Group { get; set; }

    [InverseProperty("Quota")]
    public virtual ICollection<HostingPlanQuota> HostingPlanQuota { get; set; } = new List<HostingPlanQuota>();

    [ForeignKey("ItemTypeId")]
    [InverseProperty("Quota")]
    public virtual ServiceItemType ItemType { get; set; }

    [InverseProperty("Quota")]
    public virtual ICollection<PackageQuota> PackageQuota { get; set; } = new List<PackageQuota>();
}
#endif