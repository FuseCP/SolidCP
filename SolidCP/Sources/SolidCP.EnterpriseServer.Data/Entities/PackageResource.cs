#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class PackageResource
{
    public int PackageId { get; set; }

    public int GroupId { get; set; }

    public bool CalculateDiskspace { get; set; }

    public bool CalculateBandwidth { get; set; }

    public virtual ResourceGroup Group { get; set; }

    public virtual Package Package { get; set; }
}
#endif