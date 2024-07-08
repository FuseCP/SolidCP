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

#if NetCore
[PrimaryKey("UserId", "SettingsName", "PropertyName")]
#endif
public partial class UserSetting
{
    [Key]
    [Column("UserID")]
    public int UserId { get; set; }

    [Key]
    [StringLength(50)]
    public string SettingsName { get; set; }

    [Key]
    [StringLength(50)]
    public string PropertyName { get; set; }

    [Column(TypeName = "ntext")]
    public string PropertyValue { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserSettings")]
    public virtual User User { get; set; }
}
#endif