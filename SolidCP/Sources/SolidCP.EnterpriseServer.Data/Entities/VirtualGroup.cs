#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class VirtualGroup
{
    public int VirtualGroupId { get; set; }

    public int ServerId { get; set; }

    public int GroupId { get; set; }

    public int? DistributionType { get; set; }

    public bool? BindDistributionToPrimary { get; set; }

    public virtual ResourceGroup Group { get; set; }

    public virtual Server Server { get; set; }
}
#endif