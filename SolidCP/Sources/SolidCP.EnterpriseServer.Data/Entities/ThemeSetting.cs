// This file is auto generated, do not edit.
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
[PrimaryKey("ThemeId", "SettingsName", "PropertyName")]
#endif
public partial class ThemeSetting
{
    [Key]
    [Column("ThemeID", Order = 1)]
    public int ThemeId { get; set; }

    [Required]
    [Key]
    [Column(Order = 2)]
    [StringLength(255)]
    public string SettingsName { get; set; }

    [Required]
    [Key]
    [Column(Order = 3)]
    [StringLength(255)]
    public string PropertyName { get; set; }

    [Required]
    [StringLength(255)]
    public string PropertyValue { get; set; }
}
#endif