#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[Table("PackageIPAddresses")]
#if NetCore
[Index("AddressId", Name = "PackageIPAddressesIdx_AddressID")]
[Index("ItemId", Name = "PackageIPAddressesIdx_ItemID")]
[Index("PackageId", Name = "PackageIPAddressesIdx_PackageID")]
#endif
public partial class PackageIpAddress
{
    [Key]
    [Column("PackageAddressID")]
    public int PackageAddressId { get; set; }

    [Column("PackageID")]
    public int PackageId { get; set; }

    [Column("AddressID")]
    public int AddressId { get; set; }

    [Column("ItemID")]
    public int? ItemId { get; set; }

    public bool? IsPrimary { get; set; }

    [Column("OrgID")]
    public int? OrgId { get; set; }

    [ForeignKey("AddressId")]
    [InverseProperty("PackageIpAddresses")]
    public virtual IpAddress Address { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("PackageIpAddresses")]
    public virtual ServiceItem Item { get; set; }

    [ForeignKey("PackageId")]
    [InverseProperty("PackageIpAddresses")]
    public virtual Package Package { get; set; }
}
#endif