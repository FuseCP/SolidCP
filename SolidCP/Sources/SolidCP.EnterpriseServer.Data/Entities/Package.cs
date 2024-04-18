#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class Package
{
    public int PackageId { get; set; }

    public int? ParentPackageId { get; set; }

    public int UserId { get; set; }

    public string PackageName { get; set; }

    public string PackageComments { get; set; }

    public int? ServerId { get; set; }

    public int StatusId { get; set; }

    public int? PlanId { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public bool OverrideQuotas { get; set; }

    public DateTime? BandwidthUpdated { get; set; }

    public bool DefaultTopPackage { get; set; }

    public DateTime StatusIdchangeDate { get; set; }

    public virtual ICollection<Domain> Domains { get; set; } = new List<Domain>();

    public virtual ICollection<GlobalDnsRecord> GlobalDnsRecords { get; set; } = new List<GlobalDnsRecord>();

    public virtual ICollection<HostingPlan> HostingPlans { get; set; } = new List<HostingPlan>();

    public virtual ICollection<Package> InverseParentPackage { get; set; } = new List<Package>();

    public virtual ICollection<PackageAddon> PackageAddons { get; set; } = new List<PackageAddon>();

    public virtual ICollection<PackageIpaddress> PackageIpaddresses { get; set; } = new List<PackageIpaddress>();

    public virtual ICollection<PackageQuota> PackageQuota { get; set; } = new List<PackageQuota>();

    public virtual ICollection<PackageResource> PackageResources { get; set; } = new List<PackageResource>();

    public virtual ICollection<PackageVlan> PackageVlans { get; set; } = new List<PackageVlan>();

    public virtual ICollection<PackagesBandwidth> PackagesBandwidths { get; set; } = new List<PackagesBandwidth>();

    public virtual ICollection<PackagesDiskspace> PackagesDiskspaces { get; set; } = new List<PackagesDiskspace>();

    public virtual Package ParentPackage { get; set; }

    public virtual HostingPlan Plan { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual Server Server { get; set; }

    public virtual ICollection<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

    public virtual User User { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
#endif