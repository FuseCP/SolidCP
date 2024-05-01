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
[PrimaryKey("ProviderId", "PropertyName")]
#endif
public partial class ServiceDefaultProperty
{
    [Key]
    [Column("ProviderID", Order = 1)]
    public int ProviderId { get; set; }

    [Key]
    [Column(Order = 2)]
    [StringLength(50)]
    public string PropertyName { get; set; }

    [StringLength(1000)]
    public string PropertyValue { get; set; }

    [ForeignKey("ProviderId")]
    [InverseProperty("ServiceDefaultProperties")]
    public virtual Provider Provider { get; set; }
}
#endif