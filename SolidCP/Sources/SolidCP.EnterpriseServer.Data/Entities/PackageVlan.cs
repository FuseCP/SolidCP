// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class PackageVlan
{
    public int PackageVlanId { get; set; }

    public int VlanId { get; set; }

    public int PackageId { get; set; }

    public virtual Package Package { get; set; }

    public virtual PrivateNetworkVlan Vlan { get; set; }
}
#endif