// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class Quota
{
    public int QuotaId { get; set; }

    public int GroupId { get; set; }

    public int QuotaOrder { get; set; }

    public string QuotaName { get; set; }

    public string QuotaDescription { get; set; }

    public int QuotaTypeId { get; set; }

    public bool? ServiceQuota { get; set; }

    public int? ItemTypeId { get; set; }

    public bool? HideQuota { get; set; }

    public int? PerOrganization { get; set; }

    public virtual ResourceGroup Group { get; set; }

    public virtual ICollection<HostingPlanQuota> HostingPlanQuota { get; set; } = new List<HostingPlanQuota>();

    public virtual ServiceItemType ItemType { get; set; }

    public virtual ICollection<PackageQuota> PackageQuota { get; set; } = new List<PackageQuota>();
}
#endif