#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class Ipaddress
{
    public int AddressId { get; set; }

    public string ExternalIp { get; set; }

    public string InternalIp { get; set; }

    public int? ServerId { get; set; }

    public string Comments { get; set; }

    public string SubnetMask { get; set; }

    public string DefaultGateway { get; set; }

    public int? PoolId { get; set; }

    public int? Vlan { get; set; }

    public virtual ICollection<GlobalDnsRecord> GlobalDnsRecords { get; set; } = new List<GlobalDnsRecord>();

    public virtual ICollection<PackageIpaddress> PackageIpaddresses { get; set; } = new List<PackageIpaddress>();

    public virtual Server Server { get; set; }
}
#endif