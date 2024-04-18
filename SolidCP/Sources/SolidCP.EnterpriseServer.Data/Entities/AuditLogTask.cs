#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[PrimaryKey("SourceName", "TaskName")]
public partial class AuditLogTask
{
    [Key]
    [StringLength(100)]
    [Unicode(false)]
    public string SourceName { get; set; }

    [Key]
    [StringLength(100)]
    [Unicode(false)]
    public string TaskName { get; set; }

    [StringLength(100)]
    public string TaskDescription { get; set; }
}
#endif