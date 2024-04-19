// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class BackgroundTaskParameter
{
    public int ParameterId { get; set; }

    public int TaskId { get; set; }

    public string Name { get; set; }

    public string SerializerValue { get; set; }

    public string TypeName { get; set; }

    public virtual BackgroundTask Task { get; set; }
}
#endif