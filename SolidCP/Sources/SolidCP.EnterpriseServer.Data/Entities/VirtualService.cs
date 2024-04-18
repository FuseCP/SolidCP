#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class VirtualService
{
    public int VirtualServiceId { get; set; }

    public int ServerId { get; set; }

    public int ServiceId { get; set; }

    public virtual Server Server { get; set; }

    public virtual Service Service { get; set; }
}
#endif