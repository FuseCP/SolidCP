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
[PrimaryKey("SourceName", "TaskName")]
#endif
public partial class AuditLogTask
{
    [Key]
    [Column(Order = 1)]
    [StringLength(100)]
#if NetCore
    [Unicode(false)]
#endif
    public string SourceName { get; set; }

    [Key]
    [Column(Order = 2)]
    [StringLength(100)]
#if NetCore
    [Unicode(false)]
#endif
    public string TaskName { get; set; }

    [StringLength(100)]
    public string TaskDescription { get; set; }
}
#endif