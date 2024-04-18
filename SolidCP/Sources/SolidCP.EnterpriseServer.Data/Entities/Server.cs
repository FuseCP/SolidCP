#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class Server
{
    public int ServerId { get; set; }

    public string ServerName { get; set; }

    public string ServerUrl { get; set; }

    public string Password { get; set; }

    public string Comments { get; set; }

    public bool VirtualServer { get; set; }

    public string InstantDomainAlias { get; set; }

    public int? PrimaryGroupId { get; set; }

    public string AdrootDomain { get; set; }

    public string Adusername { get; set; }

    public string Adpassword { get; set; }

    public string AdauthenticationType { get; set; }

    public bool? Adenabled { get; set; }

    public string AdParentDomain { get; set; }

    public string AdParentDomainController { get; set; }

    public int Osplatform { get; set; }

    public bool? IsCore { get; set; }

    public bool PasswordIsSha256 { get; set; }

    public virtual ICollection<GlobalDnsRecord> GlobalDnsRecords { get; set; } = new List<GlobalDnsRecord>();

    public virtual ICollection<HostingPlan> HostingPlans { get; set; } = new List<HostingPlan>();

    public virtual ICollection<Ipaddress> Ipaddresses { get; set; } = new List<Ipaddress>();

    public virtual ICollection<Package> Packages { get; set; } = new List<Package>();

    public virtual ResourceGroup PrimaryGroup { get; set; }

    public virtual ICollection<PrivateNetworkVlan> PrivateNetworkVlans { get; set; } = new List<PrivateNetworkVlan>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    public virtual ICollection<StorageSpace> StorageSpaces { get; set; } = new List<StorageSpace>();

    public virtual ICollection<VirtualGroup> VirtualGroups { get; set; } = new List<VirtualGroup>();

    public virtual ICollection<VirtualService> VirtualServices { get; set; } = new List<VirtualService>();
}
#endif