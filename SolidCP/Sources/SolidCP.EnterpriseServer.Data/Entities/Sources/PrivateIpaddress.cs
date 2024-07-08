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

[Table("PrivateIPAddresses")]
#if NetCore
[Index("ItemId", Name = "PrivateIPAddressesIdx_ItemID")]
#endif
public partial class PrivateIpaddress
{
    [Key]
    [Column("PrivateAddressID")]
    public int PrivateAddressId { get; set; }

    [Column("ItemID")]
    public int ItemId { get; set; }

    [Required]
    [Column("IPAddress")]
    [StringLength(15)]
#if NetCore
    [Unicode(false)]
#endif
    public string Ipaddress { get; set; }

    public bool IsPrimary { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("PrivateIpaddresses")]
    public virtual ServiceItem Item { get; set; }
}
#endif