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
[PrimaryKey("ItemId", "PropertyName")]
#endif
public partial class ServiceItemProperty
{
    [Key]
    [Column("ItemID")]
    public int ItemId { get; set; }

    [Key]
    [StringLength(50)]
    public string PropertyName { get; set; }

    public string PropertyValue { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("ServiceItemProperties")]
    public virtual ServiceItem Item { get; set; }
}
#endif