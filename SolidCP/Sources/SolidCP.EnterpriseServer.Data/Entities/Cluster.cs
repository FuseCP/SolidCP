// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class Cluster
{
    public int ClusterId { get; set; }

    public string ClusterName { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
#endif