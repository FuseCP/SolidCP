#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class ExchangeRetentionPolicyTag
{
    public int TagId { get; set; }

    public int ItemId { get; set; }

    public string TagName { get; set; }

    public int TagType { get; set; }

    public int AgeLimitForRetention { get; set; }

    public int RetentionAction { get; set; }
}
#endif