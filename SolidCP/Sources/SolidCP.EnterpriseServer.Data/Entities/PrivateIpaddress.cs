#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class PrivateIpaddress
{
    public int PrivateAddressId { get; set; }

    public int ItemId { get; set; }

    public string Ipaddress { get; set; }

    public bool IsPrimary { get; set; }

    public virtual ServiceItem Item { get; set; }
}
#endif