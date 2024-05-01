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
[Index("OrganizationId", Name = "IX_ExchangeOrganizations_UniqueOrg", IsUnique = true)]
#endif
public partial class ExchangeOrganization
{
    [Key]
    [Column("ItemID")]
    public int ItemId { get; set; }

    [Required]
    [Column("OrganizationID")]
    [StringLength(128)]
    public string OrganizationId { get; set; }

    [Column("ExchangeMailboxPlanID")]
    public int? ExchangeMailboxPlanId { get; set; }

    [Column("LyncUserPlanID")]
    public int? LyncUserPlanId { get; set; }

    [Column("SfBUserPlanID")]
    public int? SfBuserPlanId { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<ExchangeMailboxPlan> ExchangeMailboxPlans { get; set; } = new List<ExchangeMailboxPlan>();

    [InverseProperty("Item")]
    public virtual ICollection<ExchangeOrganizationSsFolder> ExchangeOrganizationSsFolders { get; set; } = new List<ExchangeOrganizationSsFolder>();

    [ForeignKey("ItemId")]
#if NetCore
    [InverseProperty("ExchangeOrganization")]
#endif
    public virtual ServiceItem Item { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<LyncUserPlan> LyncUserPlans { get; set; } = new List<LyncUserPlan>();
}
#endif