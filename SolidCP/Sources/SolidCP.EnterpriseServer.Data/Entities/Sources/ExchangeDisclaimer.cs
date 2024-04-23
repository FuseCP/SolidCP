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

public partial class ExchangeDisclaimer
{
    [Key]
    public int ExchangeDisclaimerId { get; set; }

    [Column("ItemID")]
    public int ItemId { get; set; }

    [Required]
    [StringLength(300)]
    public string DisclaimerName { get; set; }

    public string DisclaimerText { get; set; }
}
#endif