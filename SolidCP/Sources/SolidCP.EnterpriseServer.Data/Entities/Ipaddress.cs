#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[Table("IPAddresses")]
[Index("ServerId", Name = "IPAddressesIdx_ServerID")]
public partial class Ipaddress
{
    [Key]
    [Column("AddressID")]
    public int AddressId { get; set; }

    [Required]
    [Column("ExternalIP")]
    [StringLength(24)]
    [Unicode(false)]
    public string ExternalIp { get; set; }

    [Column("InternalIP")]
    [StringLength(24)]
    [Unicode(false)]
    public string InternalIp { get; set; }

    [Column("ServerID")]
    public int? ServerId { get; set; }

    [Column(TypeName = "ntext")]
    public string Comments { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string SubnetMask { get; set; }

    [StringLength(15)]
    [Unicode(false)]
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