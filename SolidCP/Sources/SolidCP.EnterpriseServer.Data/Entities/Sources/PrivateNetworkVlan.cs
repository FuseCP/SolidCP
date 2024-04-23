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

[Table("PrivateNetworkVLANs")]
#if NetCore
[Index("ServerId", Name = "PrivateNetworkVLANsIdx_ServerID")]
#endif
public partial class PrivateNetworkVlan
{
    [Key]
    [Column("VlanID")]
    public int VlanId { get; set; }

    public int Vlan { get; set; }

    [Column("ServerID")]
    public int? ServerId { get; set; }

    [Column(TypeName = "ntext")]
    public string Comments { get; set; }

    [InverseProperty("Vlan")]
    public virtual ICollection<PackageVlan> PackageVlans { get; set; } = new List<PackageVlan>();

    [ForeignKey("ServerId")]
    [InverseProperty("PrivateNetworkVlans")]
    public virtual Server Server { get; set; }
}
#endif