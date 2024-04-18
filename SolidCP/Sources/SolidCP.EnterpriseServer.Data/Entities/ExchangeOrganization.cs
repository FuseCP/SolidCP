#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class ExchangeOrganization
{
    public int ItemId { get; set; }

    public string OrganizationId { get; set; }

    public int? ExchangeMailboxPlanId { get; set; }

    public int? LyncUserPlanId { get; set; }

    public int? SfBuserPlanId { get; set; }

    public virtual ICollection<ExchangeMailboxPlan> ExchangeMailboxPlans { get; set; } = new List<ExchangeMailboxPlan>();

    public virtual ICollection<ExchangeOrganizationSsFolder> ExchangeOrganizationSsFolders { get; set; } = new List<ExchangeOrganizationSsFolder>();

    public virtual ServiceItem Item { get; set; }

    public virtual ICollection<LyncUserPlan> LyncUserPlans { get; set; } = new List<LyncUserPlan>();
}
#endif