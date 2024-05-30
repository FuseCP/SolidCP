#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[Table("RDSCollections")]
public partial class RdsCollection
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("ItemID")]
    public int ItemId { get; set; }

    [StringLength(255)]
    public string Name { get; set; }

    [StringLength(255)]
    public string Description { get; set; }

    [StringLength(255)]
    public string DisplayName { get; set; }

    [InverseProperty("RdsCollection")]
    public virtual ICollection<RdsCollectionSetting> RdsCollectionSettings { get; set; } = new List<RdsCollectionSetting>();

    [InverseProperty("RdsCollection")]
    public virtual ICollection<RdsCollectionUser> RdsCollectionUsers { get; set; } = new List<RdsCollectionUser>();

    [InverseProperty("RdsCollection")]
    public virtual ICollection<RdsMessage> RdsMessages { get; set; } = new List<RdsMessage>();

    [InverseProperty("RdsCollection")]
    public virtual ICollection<RdsServer> RdsServers { get; set; } = new List<RdsServer>();
}
#endif