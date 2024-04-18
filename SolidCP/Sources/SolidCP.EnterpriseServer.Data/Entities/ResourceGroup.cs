#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class ResourceGroup
{
    public int GroupId { get; set; }

    public string GroupName { get; set; }

    public int GroupOrder { get; set; }

    public string GroupController { get; set; }

    public bool? ShowGroup { get; set; }

    public virtual ICollection<HostingPlanResource> HostingPlanResources { get; set; } = new List<HostingPlanResource>();

    public virtual ICollection<PackageResource> PackageResources { get; set; } = new List<PackageResource>();

    public virtual ICollection<PackagesBandwidth> PackagesBandwidths { get; set; } = new List<PackagesBandwidth>();

    public virtual ICollection<PackagesDiskspace> PackagesDiskspaces { get; set; } = new List<PackagesDiskspace>();

    public virtual ICollection<Provider> Providers { get; set; } = new List<Provider>();

    public virtual ICollection<Quota> Quota { get; set; } = new List<Quota>();

    public virtual ICollection<ResourceGroupDnsRecord> ResourceGroupDnsRecords { get; set; } = new List<ResourceGroupDnsRecord>();

    public virtual ICollection<Server> Servers { get; set; } = new List<Server>();

    public virtual ICollection<ServiceItemType> ServiceItemTypes { get; set; } = new List<ServiceItemType>();

    public virtual ICollection<StorageSpaceLevelResourceGroup> StorageSpaceLevelResourceGroups { get; set; } = new List<StorageSpaceLevelResourceGroup>();

    public virtual ICollection<VirtualGroup> VirtualGroups { get; set; } = new List<VirtualGroup>();
}
#endif