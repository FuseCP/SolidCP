#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[Table("PackagesBandwidth")]
#if NetCore
[PrimaryKey("PackageId", "GroupId", "LogDate")]
#endif
public partial class PackagesBandwidth
{
    [Key]
    [Column("PackageID", Order = 1)]
    public int PackageId { get; set; }

    [Key]
    [Column("GroupID", Order = 2)]
    public int GroupId { get; set; }

    [Key]
	//[Column(TypeName = "datetime", Order = 3)]
	[Column(Order = 3)]
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