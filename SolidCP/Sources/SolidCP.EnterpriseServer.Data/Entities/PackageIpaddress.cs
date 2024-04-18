#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class PackageIpaddress
{
    public int PackageAddressId { get; set; }

    public int PackageId { get; set; }

    public int AddressId { get; set; }

    public int? ItemId { get; set; }

    public bool? IsPrimary { get; set; }

    public int? OrgId { get; set; }

    public virtual Ipaddress Address { get; set; }

    public virtual ServiceItem Item { get; set; }

    public virtual Package Package { get; set; }
}
#endif