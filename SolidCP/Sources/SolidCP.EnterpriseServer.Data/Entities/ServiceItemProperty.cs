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
[PrimaryKey("ItemId", "PropertyName")]
#endif
public partial class ServiceItemProperty
{
    [Key]
    [Column("ItemID", Order = 1)]
    public int ItemId { get; set; }

    [Key]
    [Column(Order = 2)]
    [StringLength(50)]
    public string PropertyName { get; set; }

    public string PropertyValue { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("ServiceItemProperties")]
    public virtual ServiceItem Item { get; set; }
}
#endif