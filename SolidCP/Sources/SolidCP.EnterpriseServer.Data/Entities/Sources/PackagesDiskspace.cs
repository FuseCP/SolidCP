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

[Table("PackagesDiskspace")]
#if NetCore
[PrimaryKey("PackageId", "GroupId")]
#endif
public partial class PackagesDiskspace
{
    [Key]
    [Column("PackageID")]
    public int PackageId { get; set; }

    [Key]
    [Column("GroupID")]
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