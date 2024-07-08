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

[Table("RDSCollections")]
public partial class Rdscollection
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

    [InverseProperty("Rdscollection")]
    public virtual ICollection<RdscollectionSetting> RdscollectionSettings { get; set; } = new List<RdscollectionSetting>();

    [InverseProperty("Rdscollection")]
    public virtual ICollection<RdscollectionUser> RdscollectionUsers { get; set; } = new List<RdscollectionUser>();

    [InverseProperty("Rdscollection")]
    public virtual ICollection<Rdsmessage> Rdsmessages { get; set; } = new List<Rdsmessage>();

    [InverseProperty("Rdscollection")]
    public virtual ICollection<Rdsserver> Rdsservers { get; set; } = new List<Rdsserver>();
}
#endif