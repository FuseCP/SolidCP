// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities.Sources;

public partial class ExchangeMailboxPlanRetentionPolicyTag
{
    [Key]
    [Column("PlanTagID")]
    public int PlanTagId { get; set; }

    [Column("TagID")]
    public int TagId { get; set; }

    public int MailboxPlanId { get; set; }
}
#endif