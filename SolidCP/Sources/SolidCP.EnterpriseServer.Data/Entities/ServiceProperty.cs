#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class ServiceProperty
{
    public int ServiceId { get; set; }

    public string PropertyName { get; set; }

    public string PropertyValue { get; set; }

    public virtual Service Service { get; set; }
}
#endif