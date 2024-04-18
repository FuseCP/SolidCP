#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class ServiceItemProperty
{
    public int ItemId { get; set; }

    public string PropertyName { get; set; }

    public string PropertyValue { get; set; }

    public virtual ServiceItem Item { get; set; }
}
#endif