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

[Table("RDSServerSettings")]
#if NetCore
[PrimaryKey("RdsServerId", "SettingsName", "PropertyName")]
#endif
public partial class RdsserverSetting
{
    [Key]
    public int RdsServerId { get; set; }

    [Key]
    [StringLength(50)]
    public string SettingsName { get; set; }

    [Key]
    [StringLength(50)]
    public string PropertyName { get; set; }

    [Column(TypeName = "ntext")]
    public string PropertyValue { get; set; }

    public bool ApplyUsers { get; set; }

    public bool ApplyAdministrators { get; set; }
}
#endif