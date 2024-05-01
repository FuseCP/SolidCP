#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class ResourceGroup
{
    [Key]
    [Column("GroupID")]
    public int GroupId { get; set; }

    [Required]
    [StringLength(100)]
    public string GroupName { get; set; }

    public int GroupOrder { get; set; }

    [StringLength(1000)]
    public string GroupController { get; set; }

    public bool? ShowGroup { get; set; }

    [InverseProperty("Group")]
    public virtual ICollection<HostingPlanResource> HostingPlanResources { get; set; } = new List<HostingPlanResource>();

    [InverseProperty("Group")]
    public virtual ICollection<PackageResource> PackageResources { get; set; } = new List<PackageResource>();

    [InverseProperty("Group")]
    public virtual ICollection<PackagesBandwidth> PackagesBandwidths { get; set; } = new List<PackagesBandwidth>();

    [InverseProperty("Group")]
    public virtual ICollection<PackagesDiskspace> PackagesDiskspaces { get; set; } = new List<PackagesDiskspace>();

    [InverseProperty("Group")]
    public virtual ICollection<Provider> Providers { get; set; } = new List<Provider>();

    [InverseProperty("Group")]
    public virtual ICollection<Quota> Quota { get; set; } = new List<Quota>();

    [InverseProperty("Group")]
    public virtual ICollection<ResourceGroupDnsRecord> ResourceGroupDnsRecords { get; set; } = new List<ResourceGroupDnsRecord>();

    [InverseProperty("PrimaryGroup")]
    public virtual ICollection<Server> Servers { get; set; } = new List<Server>();

    [InverseProperty("Group")]
    public virtual ICollection<ServiceItemType> ServiceItemTypes { get; set; } = new List<ServiceItemType>();

    [InverseProperty("Group")]
    public virtual ICollection<StorageSpaceLevelResourceGroup> StorageSpaceLevelResourceGroups { get; set; } = new List<StorageSpaceLevelResourceGroup>();

    [InverseProperty("Group")]
    public virtual ICollection<VirtualGroup> VirtualGroups { get; set; } = new List<VirtualGroup>();
}
#endif