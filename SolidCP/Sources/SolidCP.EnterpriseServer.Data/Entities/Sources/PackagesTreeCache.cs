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

[Table("PackagesTreeCache")]
#if NetCore
[Keyless]
#endif
public partial class PackagesTreeCache
{
    [Column("ParentPackageID")]
    public int ParentPackageId { get; set; }

    [Column("PackageID")]
    public int PackageId { get; set; }

    [ForeignKey("PackageId")]
    public virtual Package Package { get; set; }

    [ForeignKey("ParentPackageId")]
    public virtual Package ParentPackage { get; set; }
}
#endif