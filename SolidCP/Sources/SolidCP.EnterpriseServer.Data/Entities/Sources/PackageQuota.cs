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
[PrimaryKey("PackageId", "QuotaId")]
#endif
public partial class PackageQuota
{
    [Key]
    [Column("PackageID")]
    public int PackageId { get; set; }

    [Key]
    [Column("QuotaID")]
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