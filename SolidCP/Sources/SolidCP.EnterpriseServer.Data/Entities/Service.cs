// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class Service
{
    public int ServiceId { get; set; }

    public int ServerId { get; set; }

    public int ProviderId { get; set; }

    public string ServiceName { get; set; }

    public string Comments { get; set; }

    public int? ServiceQuotaValue { get; set; }

    public int? ClusterId { get; set; }

    public virtual Cluster Cluster { get; set; }

    public virtual ICollection<GlobalDnsRecord> GlobalDnsRecords { get; set; } = new List<GlobalDnsRecord>();

    public virtual Provider Provider { get; set; }

    public virtual Server Server { get; set; }

    public virtual ICollection<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

    public virtual ICollection<ServiceProperty> ServiceProperties { get; set; } = new List<ServiceProperty>();

    public virtual ICollection<StorageSpace> StorageSpaces { get; set; } = new List<StorageSpace>();

    public virtual ICollection<VirtualService> VirtualServices { get; set; } = new List<VirtualService>();

    public virtual ICollection<Package> Packages { get; set; } = new List<Package>();
}
#endif