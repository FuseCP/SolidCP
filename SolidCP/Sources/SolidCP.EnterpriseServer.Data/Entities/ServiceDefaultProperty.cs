// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class ServiceDefaultProperty
{
    public int ProviderId { get; set; }

    public string PropertyName { get; set; }

    public string PropertyValue { get; set; }

    public virtual Provider Provider { get; set; }
}
#endif