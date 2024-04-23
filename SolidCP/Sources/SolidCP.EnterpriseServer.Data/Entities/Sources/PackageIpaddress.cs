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

[Table("PackageIPAddresses")]
#if NetCore
[Index("AddressId", Name = "PackageIPAddressesIdx_AddressID")]
[Index("ItemId", Name = "PackageIPAddressesIdx_ItemID")]
[Index("PackageId", Name = "PackageIPAddressesIdx_PackageID")]
#endif
public partial class PackageIpaddress
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
    [InverseProperty("PackageIpaddresses")]
    public virtual Ipaddress Address { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("PackageIpaddresses")]
    public virtual ServiceItem Item { get; set; }

    [ForeignKey("PackageId")]
    [InverseProperty("PackageIpaddresses")]
    public virtual Package Package { get; set; }
}
#endif