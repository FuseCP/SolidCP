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
[Index("ItemTypeId", Name = "ServiceItemsIdx_ItemTypeID")]
[Index("PackageId", Name = "ServiceItemsIdx_PackageID")]
[Index("ServiceId", Name = "ServiceItemsIdx_ServiceID")]
#endif
public partial class ServiceItem
{
    [Key]
    [Column("ItemID")]
    public int ItemId { get; set; }

    [Column("PackageID")]
    public int? PackageId { get; set; }

    [Column("ItemTypeID")]
    public int? ItemTypeId { get; set; }

    [Column("ServiceID")]
    public int? ServiceId { get; set; }

    [StringLength(500)]
    public string ItemName { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime? CreatedDate { get; set; }

	[InverseProperty("Item")]
	public virtual ICollection<DmzIpAddress> DmzIpAddresses { get; set; } = new List<DmzIpAddress>();

	[InverseProperty("MailDomain")]
    public virtual ICollection<Domain> DomainMailDomains { get; set; } = new List<Domain>();

    [InverseProperty("WebSite")]
    public virtual ICollection<Domain> DomainWebSites { get; set; } = new List<Domain>();

    [InverseProperty("Zone")]
    public virtual ICollection<Domain> DomainZones { get; set; } = new List<Domain>();

    [InverseProperty("Item")]
    public virtual ICollection<ExchangeAccount> ExchangeAccounts { get; set; } = new List<ExchangeAccount>();
    //TODO make this work for NetFX too
#if NetCore    
    [InverseProperty("Item")]
    public virtual ExchangeOrganization ExchangeOrganization { get; set; }
#endif
    [InverseProperty("Item")]
    public virtual ICollection<ExchangeOrganizationDomain> ExchangeOrganizationDomains { get; set; } = new List<ExchangeOrganizationDomain>();

    [ForeignKey("ItemTypeId")]
    [InverseProperty("ServiceItems")]
    public virtual ServiceItemType ItemType { get; set; }

    [ForeignKey("PackageId")]
    [InverseProperty("ServiceItems")]
    public virtual Package Package { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<PackageIpAddress> PackageIpAddresses { get; set; } = new List<PackageIpAddress>();

    [InverseProperty("Item")]
    public virtual ICollection<PrivateIpAddress> PrivateIpAddresses { get; set; } = new List<PrivateIpAddress>();

    [ForeignKey("ServiceId")]
    [InverseProperty("ServiceItems")]
    public virtual Service Service { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<ServiceItemProperty> ServiceItemProperties { get; set; } = new List<ServiceItemProperty>();
}
#endif