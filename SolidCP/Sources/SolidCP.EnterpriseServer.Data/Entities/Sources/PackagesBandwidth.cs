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

[Table("PackagesBandwidth")]
#if NetCore
[PrimaryKey("PackageId", "GroupId", "LogDate")]
#endif
public partial class PackagesBandwidth
{
    [Key]
    [Column("PackageID")]
    public int PackageId { get; set; }

    [Key]
    [Column("GroupID")]
    public int GroupId { get; set; }

    [Key]
    [Column(TypeName = "datetime")]
    public DateTime LogDate { get; set; }

    public long BytesSent { get; set; }

    public long BytesReceived { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("PackagesBandwidths")]
    public virtual ResourceGroup Group { get; set; }

    [ForeignKey("PackageId")]
    [InverseProperty("PackagesBandwidths")]
    public virtual Package Package { get; set; }
}
#endif