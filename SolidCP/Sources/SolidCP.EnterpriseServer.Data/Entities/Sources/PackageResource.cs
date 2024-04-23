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
[PrimaryKey("PackageId", "GroupId")]
#endif
public partial class PackageResource
{
    [Key]
    [Column("PackageID")]
    public int PackageId { get; set; }

    [Key]
    [Column("GroupID")]
    public int GroupId { get; set; }

    public bool CalculateDiskspace { get; set; }

    public bool CalculateBandwidth { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("PackageResources")]
    public virtual ResourceGroup Group { get; set; }

    [ForeignKey("PackageId")]
    [InverseProperty("PackageResources")]
    public virtual Package Package { get; set; }
}
#endif