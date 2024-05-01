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
[PrimaryKey("PackageId", "QuotaId")]
#endif
public partial class PackageQuota
{
    [Key]
    [Column("PackageID", Order = 1)]
    public int PackageId { get; set; }

    [Key]
    [Column("QuotaID", Order = 2)]
    public int QuotaId { get; set; }

    public int QuotaValue { get; set; }

    [ForeignKey("PackageId")]
    [InverseProperty("PackageQuota")]
    public virtual Package Package { get; set; }

    [ForeignKey("QuotaId")]
    [InverseProperty("PackageQuota")]
    public virtual Quota Quota { get; set; }
}
#endif