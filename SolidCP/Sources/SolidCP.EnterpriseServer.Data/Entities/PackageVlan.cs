#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[Table("PackageVLANs")]
#if NetCore
[Index("PackageId", Name = "PackageVLANsIdx_PackageID")]
[Index("VlanId", Name = "PackageVLANsIdx_VlanID")]
#endif
public partial class PackageVlan
{
    [Key]
    [Column("PackageVlanID")]
    public int PackageVlanId { get; set; }

    [Column("VlanID")]
    public int VlanId { get; set; }

	[Column("PackageID")]
    public int PackageId { get; set; }
	public bool IsDmz { get; set; }
	
    [ForeignKey("PackageId")]
    [InverseProperty("PackageVlans")]
    public virtual Package Package { get; set; }

    [ForeignKey("VlanId")]
    [InverseProperty("PackageVlans")]
    public virtual PrivateNetworkVlan Vlan { get; set; }
}
#endif