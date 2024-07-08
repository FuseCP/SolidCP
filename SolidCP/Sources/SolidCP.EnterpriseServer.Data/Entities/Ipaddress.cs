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
#if NetCore
[Index("ServerId", Name = "IPAddressesIdx_ServerID")]
#endif
public partial class IpAddress
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

    //[Column(TypeName = "ntext")]
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

    [InverseProperty("IpAddress")]
    public virtual ICollection<GlobalDnsRecord> GlobalDnsRecords { get; set; } = new List<GlobalDnsRecord>();

    [InverseProperty("Address")]
    public virtual ICollection<PackageIpAddress> PackageIpAddresses { get; set; } = new List<PackageIpAddress>();

    [ForeignKey("ServerId")]
    [InverseProperty("IpAddresses")]
    public virtual Server Server { get; set; }
}
#endif