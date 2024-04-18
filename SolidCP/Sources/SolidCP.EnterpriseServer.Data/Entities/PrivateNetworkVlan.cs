#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class PrivateNetworkVlan
{
    public int VlanId { get; set; }

    public int Vlan { get; set; }

    public int? ServerId { get; set; }

    public string Comments { get; set; }

    public virtual ICollection<PackageVlan> PackageVlans { get; set; } = new List<PackageVlan>();

    public virtual Server Server { get; set; }
}
#endif