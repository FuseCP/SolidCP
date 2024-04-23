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

public partial class SupportServiceLevel
{
    [Key]
    [Column("LevelID")]
    public int LevelId { get; set; }

    [Required]
    [StringLength(100)]
    public string LevelName { get; set; }

    [StringLength(1000)]
    public string LevelDescription { get; set; }
}
#endif