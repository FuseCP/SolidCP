// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

#if NetCore
[Index("PrimaryGroupId", Name = "ServersIdx_PrimaryGroupID")]
#endif
public partial class Server
{
    [Key]
    [Column("ServerID")]
    public int ServerId { get; set; }

    [Required]
    [StringLength(100)]
    public string ServerName { get; set; }

    [StringLength(255)]
    public string ServerUrl { get; set; }

    [StringLength(100)]
    public string Password { get; set; }

    //[Column(TypeName = "ntext")]
    public string Comments { get; set; }

    public bool VirtualServer { get; set; }

    [StringLength(200)]
    public string InstantDomainAlias { get; set; }

    [Column("PrimaryGroupID")]
    public int? PrimaryGroupId { get; set; }

    [Column("ADRootDomain")]
    [StringLength(200)]
    public string ADRootDomain { get; set; }

    [Column("ADUsername")]
    [StringLength(100)]
    public string ADUsername { get; set; }

    [Column("ADPassword")]
    [StringLength(100)]
    public string ADPassword { get; set; }

    [Column("ADAuthenticationType")]
    [StringLength(50)]
#if NetCore
    [Unicode(false)]
#endif
    public string ADAuthenticationType { get; set; }

    [Column("ADEnabled")]
    public bool? ADEnabled { get; set; }

    [StringLength(200)]
    public string ADParentDomain { get; set; }

    [StringLength(200)]
    public string ADParentDomainController { get; set; }

    [Column("OSPlatform")]
    public Providers.OS.OSPlatform OSPlatform { get; set; }

    public bool? IsCore { get; set; }

    [Column("PasswordIsSHA256")]
    public bool PasswordIsSHA256 { get; set; }

    [InverseProperty("Server")]
    public virtual ICollection<GlobalDnsRecord> GlobalDnsRecords { get; set; } = new List<GlobalDnsRecord>();

    [InverseProperty("Server")]
    public virtual ICollection<HostingPlan> HostingPlans { get; set; } = new List<HostingPlan>();

    [InverseProperty("Server")]
    public virtual ICollection<IpAddress> IpAddresses { get; set; } = new List<IpAddress>();

    [InverseProperty("Server")]
    public virtual ICollection<Package> Packages { get; set; } = new List<Package>();

    [ForeignKey("PrimaryGroupId")]
    [InverseProperty("Servers")]
    public virtual ResourceGroup PrimaryGroup { get; set; }

    [InverseProperty("Server")]
    public virtual ICollection<PrivateNetworkVlan> PrivateNetworkVlans { get; set; } = new List<PrivateNetworkVlan>();

    [InverseProperty("Server")]
    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    [InverseProperty("Server")]
    public virtual ICollection<StorageSpace> StorageSpaces { get; set; } = new List<StorageSpace>();

    [InverseProperty("Server")]
    public virtual ICollection<VirtualGroup> VirtualGroups { get; set; } = new List<VirtualGroup>();

    [InverseProperty("Server")]
    public virtual ICollection<VirtualService> VirtualServices { get; set; } = new List<VirtualService>();
}
#endif