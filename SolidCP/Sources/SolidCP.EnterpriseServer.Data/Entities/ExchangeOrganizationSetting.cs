#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[Keyless]
[Index("ItemId", Name = "ExchangeOrganizationSettingsIdx_ItemId")]
public partial class ExchangeOrganizationSetting
{
    public int ItemId { get; set; }

    [Required]
    [StringLength(100)]
    public string SettingsName { get; set; }

    [Required]
    public string Xml { get; set; }

    [ForeignKey("ItemId")]
    public virtual ExchangeOrganization Item { get; set; }
}
#endif