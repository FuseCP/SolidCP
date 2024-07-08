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

public partial class Version
{
    [Key]
    [StringLength(50)]
#if NetCore
    [Unicode(false)]
#endif
    public string DatabaseVersion { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime BuildDate { get; set; }
}
#endif