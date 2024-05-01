#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[Table("PackagesDiskspace")]
#if NetCore
[PrimaryKey("PackageId", "GroupId")]
#endif
public partial class PackagesDiskspace
{
    [Key]
    [Column("PackageID", Order = 1)]
    public int PackageId { get; set; }

    [Key]
    [Column("GroupID", Order = 2)]
    public int GroupId { get; set; }

    public long DiskSpace { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("PackagesDiskspaces")]
    public virtual ResourceGroup Group { get; set; }

    [ForeignKey("PackageId")]
    [InverseProperty("PackagesDiskspaces")]
    public virtual Package Package { get; set; }
}
#endif