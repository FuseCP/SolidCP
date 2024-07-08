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
[Index("GroupId", Name = "VirtualGroupsIdx_GroupID")]
[Index("ServerId", Name = "VirtualGroupsIdx_ServerID")]
#endif
public partial class VirtualGroup
{
    [Key]
    [Column("VirtualGroupID")]
    public int VirtualGroupId { get; set; }

    [Column("ServerID")]
    public int ServerId { get; set; }

    [Column("GroupID")]
    public int GroupId { get; set; }

    public int? DistributionType { get; set; }

    public bool? BindDistributionToPrimary { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("VirtualGroups")]
    public virtual ResourceGroup Group { get; set; }

    [ForeignKey("ServerId")]
    [InverseProperty("VirtualGroups")]
    public virtual Server Server { get; set; }
}
#endif