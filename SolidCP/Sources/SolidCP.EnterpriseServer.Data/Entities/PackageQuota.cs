// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class PackageQuota
{
    public int PackageId { get; set; }

    public int QuotaId { get; set; }

    public int QuotaValue { get; set; }

    public virtual Package Package { get; set; }

    public virtual Quota Quota { get; set; }
}
#endif