#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

#if NetCore
//[Keyless]
[PrimaryKey("ItemId", "SettingsName")]
[Index("ItemId", Name = "ExchangeOrganizationSettingsIdx_ItemId")]
#endif
public partial class ExchangeOrganizationSetting
{
    [Key]
    [Column(Order = 1)]
    public int ItemId { get; set; }

    [Required]
    [Key]
    [Column(Order = 2)]
    [StringLength(100)]
    public string SettingsName { get; set; }

    [Required]
    public string Xml { get; set; }

    [ForeignKey("ItemId")]
    public virtual ExchangeOrganization Item { get; set; }
}
#endif