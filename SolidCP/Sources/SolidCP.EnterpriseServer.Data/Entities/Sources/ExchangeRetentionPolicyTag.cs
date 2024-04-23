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

public partial class ExchangeRetentionPolicyTag
{
    [Key]
    [Column("TagID")]
    public int TagId { get; set; }

    [Column("ItemID")]
    public int ItemId { get; set; }

    [StringLength(255)]
    public string TagName { get; set; }

    public int TagType { get; set; }

    public int AgeLimitForRetention { get; set; }

    public int RetentionAction { get; set; }
}
#endif