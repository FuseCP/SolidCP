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
[PrimaryKey("ServiceId", "PropertyName")]
#endif
public partial class ServiceProperty
{
    [Key]
    [Column("ServiceID")]
    public int ServiceId { get; set; }

    [Key]
    [StringLength(50)]
    public string PropertyName { get; set; }

    public string PropertyValue { get; set; }

    [ForeignKey("ServiceId")]
    [InverseProperty("ServiceProperties")]
    public virtual Service Service { get; set; }
}
#endif