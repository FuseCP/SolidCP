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
[PrimaryKey("SettingsName", "PropertyName")]
#endif
public partial class SystemSetting
{
    [Key]
    [Column(Order = 1)]
    [StringLength(50)]
    public string SettingsName { get; set; }

    [Key]
    [Column(Order = 2)]
    [StringLength(50)]
    public string PropertyName { get; set; }

    //[Column(TypeName = "ntext")]
    public string PropertyValue { get; set; }
}
#endif