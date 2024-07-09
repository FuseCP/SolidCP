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

[Table("DmzIPAddresses")]
#if NetCore
[Index("ItemId", Name = "DmzIPAddressesIdx_ItemID")]
#endif
public partial class DmzIpaddress
{
    [Key]
    [Column("DmzAddressID")]
    public int DmzAddressId { get; set; }

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
    [InverseProperty("DmzIpaddresses")]
    public virtual ServiceItem Item { get; set; }
}
#endif