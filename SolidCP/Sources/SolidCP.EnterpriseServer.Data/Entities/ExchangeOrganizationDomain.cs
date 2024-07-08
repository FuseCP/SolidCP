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
[Index("ItemId", Name = "ExchangeOrganizationDomainsIdx_ItemID")]
[Index("DomainId", Name = "IX_ExchangeOrganizationDomains_UniqueDomain", IsUnique = true)]
#endif
public partial class ExchangeOrganizationDomain
{
    [Key]
    [Column("OrganizationDomainID")]
    public int OrganizationDomainId { get; set; }

    [Column("ItemID")]
    public int ItemId { get; set; }

    [Column("DomainID")]
    public int? DomainId { get; set; }

    public bool? IsHost { get; set; }

    [Column("DomainTypeID")]
    public int DomainTypeId { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("ExchangeOrganizationDomains")]
    public virtual ServiceItem Item { get; set; }
}
#endif