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

public partial class AuditLogSource
{
    [Key]
    [StringLength(100)]
#if NetCore
    [Unicode(false)]
#endif
    public string SourceName { get; set; }
}
#endif