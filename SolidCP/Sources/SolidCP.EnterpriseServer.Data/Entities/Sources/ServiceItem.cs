// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities.Sources;

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

    [Column(TypeName = "datetime")]
    public DateTime? CreatedDate { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<DmzIpaddress> DmzIpaddresses { get; set; } = new List<DmzIpaddress>();

    [InverseProperty("MailDomain")]
    public virtual ICollection<Domain> DomainMailDomains { get; set; } = new List<Domain>();

    [InverseProperty("WebSite")]
    public virtual ICollection<Domain> DomainWebSites { get; set; } = new List<Domain>();

    [InverseProperty("ZoneItem")]
    public virtual ICollection<Domain> DomainZoneItems { get; set; } = new List<Domain>();

    [InverseProperty("Item")]
    public virtual ICollection<ExchangeAccount> ExchangeAccounts { get; set; } = new List<ExchangeAccount>();

    [InverseProperty("Item")]
    public virtual ExchangeOrganization ExchangeOrganization { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<ExchangeOrganizationDomain> ExchangeOrganizationDomains { get; set; } = new List<ExchangeOrganizationDomain>();

    [ForeignKey("ItemTypeId")]
    [InverseProperty("ServiceItems")]
    public virtual ServiceItemType ItemType { get; set; }

    [ForeignKey("PackageId")]
    [InverseProperty("ServiceItems")]
    public virtual Package Package { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<PackageIpaddress> PackageIpaddresses { get; set; } = new List<PackageIpaddress>();

    [InverseProperty("Item")]
    public virtual ICollection<PrivateIpaddress> PrivateIpaddresses { get; set; } = new List<PrivateIpaddress>();

    [ForeignKey("ServiceId")]
    [InverseProperty("ServiceItems")]
    public virtual Service Service { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<ServiceItemProperty> ServiceItemProperties { get; set; } = new List<ServiceItemProperty>();
}
#endif