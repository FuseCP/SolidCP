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

[Table("IPAddresses")]
#if NetCore
[Index("ServerId", Name = "IPAddressesIdx_ServerID")]
#endif
public partial class Ipaddress
{
    [Key]
    [Column("AddressID")]
    public int AddressId { get; set; }

    [Required]
    [Column("ExternalIP")]
    [StringLength(24)]
#if NetCore
    [Unicode(false)]
#endif
    public string ExternalIp { get; set; }

    [Column("InternalIP")]
    [StringLength(24)]
#if NetCore
    [Unicode(false)]
#endif
    public string InternalIp { get; set; }

    [Column("ServerID")]
    public int? ServerId { get; set; }

    [Column(TypeName = "ntext")]
    public string Comments { get; set; }

    [StringLength(15)]
#if NetCore
    [Unicode(false)]
#endif
    public string SubnetMask { get; set; }

    [StringLength(15)]
#if NetCore
    [Unicode(false)]
#endif
    public string DefaultGateway { get; set; }

    [Column("PoolID")]
    public int? PoolId { get; set; }

    [Column("VLAN")]
    public int? Vlan { get; set; }

    [InverseProperty("Ipaddress")]
    public virtual ICollection<GlobalDnsRecord> GlobalDnsRecords { get; set; } = new List<GlobalDnsRecord>();

    [InverseProperty("Address")]
    public virtual ICollection<PackageIpaddress> PackageIpaddresses { get; set; } = new List<PackageIpaddress>();

    [ForeignKey("ServerId")]
    [InverseProperty("Ipaddresses")]
    public virtual Server Server { get; set; }
}
#endif