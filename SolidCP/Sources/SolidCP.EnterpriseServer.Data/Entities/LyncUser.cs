#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class LyncUser
{
    public int LyncUserId { get; set; }

    public int AccountId { get; set; }

    public int LyncUserPlanId { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ModifiedDate { get; set; }

    public string SipAddress { get; set; }

    public virtual LyncUserPlan LyncUserPlan { get; set; }
}
#endif