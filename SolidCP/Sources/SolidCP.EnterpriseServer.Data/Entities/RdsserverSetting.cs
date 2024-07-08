#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[Table("RDSServerSettings")]
#if NetCore
[PrimaryKey("RdsServerId", "SettingsName", "PropertyName")]
#endif
public partial class RdsServerSetting
{
    [Key]
    [Column(Order = 1)]
    public int RdsServerId { get; set; }

    [Key]
	[Column(Order = 2)]
	[StringLength(50)]
    public string SettingsName { get; set; }

    [Key]
	[Column(Order = 3)]
	[StringLength(50)]
    public string PropertyName { get; set; }

    //[Column(TypeName = "ntext")]
    public string PropertyValue { get; set; }

    public bool ApplyUsers { get; set; }

    public bool ApplyAdministrators { get; set; }
}
#endif