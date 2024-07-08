#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[Table("PackagesTreeCache")]
#if NetCore
[PrimaryKey("ParentPackageId", "PackageId")]
#endif
public partial class PackagesTreeCache
{
    [Key]
    [Column("ParentPackageID", Order = 1)]
    public int ParentPackageId { get; set; }

    [Key]
    [Column("PackageID", Order = 2)]
    public int PackageId { get; set; }

    [ForeignKey("PackageId")]
    public virtual Package Package { get; set; }

    [ForeignKey("ParentPackageId")]
    public virtual Package ParentPackage { get; set; }
}
#endif