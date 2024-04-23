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

[Table("RDSServers")]
#if NetCore
[Index("RdscollectionId", Name = "RDSServersIdx_RDSCollectionId")]
#endif
public partial class Rdsserver
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("ItemID")]
    public int? ItemId { get; set; }

    [StringLength(255)]
    public string Name { get; set; }

    [StringLength(255)]
    public string FqdName { get; set; }

    [StringLength(255)]
    public string Description { get; set; }

    [Column("RDSCollectionId")]
    public int? RdscollectionId { get; set; }

    public bool ConnectionEnabled { get; set; }

    public int? Controller { get; set; }

    [ForeignKey("RdscollectionId")]
    [InverseProperty("Rdsservers")]
    public virtual Rdscollection Rdscollection { get; set; }
}
#endif