#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[Table("RDSServers")]
#if NetCore
[Index("RdsCollectionId", Name = "RDSServersIdx_RDSCollectionId")]
#endif
public partial class RdsServer
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
    public int? RdsCollectionId { get; set; }

    public bool ConnectionEnabled { get; set; }

    public int? Controller { get; set; }

    [ForeignKey("RdsCollectionId")]
    [InverseProperty("RdsServers")]
    public virtual RdsCollection RdsCollection { get; set; }
}
#endif