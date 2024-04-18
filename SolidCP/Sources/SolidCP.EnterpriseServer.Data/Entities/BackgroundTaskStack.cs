#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class BackgroundTaskStack
{
    public int TaskStackId { get; set; }

    public int TaskId { get; set; }

    public virtual BackgroundTask Task { get; set; }
}
#endif