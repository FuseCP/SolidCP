#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class PackagesTreeCache
{
    public int ParentPackageId { get; set; }

    public int PackageId { get; set; }

    public virtual Package Package { get; set; }

    public virtual Package ParentPackage { get; set; }
}
#endif