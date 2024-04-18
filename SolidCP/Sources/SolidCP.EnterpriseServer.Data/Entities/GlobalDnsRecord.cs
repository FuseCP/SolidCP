#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class GlobalDnsRecord
{
    public int RecordId { get; set; }

    public string RecordType { get; set; }

    public string RecordName { get; set; }

    public string RecordData { get; set; }

    public int Mxpriority { get; set; }

    public int? ServiceId { get; set; }

    public int? ServerId { get; set; }

    public int? PackageId { get; set; }

    public int? IpaddressId { get; set; }

    public int? SrvPriority { get; set; }

    public int? SrvWeight { get; set; }

    public int? SrvPort { get; set; }

    public virtual Ipaddress Ipaddress { get; set; }

    public virtual Package Package { get; set; }

    public virtual Server Server { get; set; }

    public virtual Service Service { get; set; }
}
#endif