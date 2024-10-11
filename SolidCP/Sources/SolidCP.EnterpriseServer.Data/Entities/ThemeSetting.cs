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
[Index("ThemeId", Name = "ThemeSettingsIdx_ThemeID")]
#endif
public partial class ThemeSetting
{
    [Key]
    [Column("ThemeSettingID")]
    public int ThemeSettingId { get; set; }

    [Column("ThemeID")]
    public int ThemeId { get; set; }

    [Required]
    [StringLength(255)]
    public string SettingsName { get; set; }

    [Required]
    [StringLength(255)]
    public string PropertyName { get; set; }

    [Required]
    [StringLength(255)]
    public string PropertyValue { get; set; }
}
#endif