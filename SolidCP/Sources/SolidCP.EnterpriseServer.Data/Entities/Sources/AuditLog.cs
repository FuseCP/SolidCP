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

[Table("AuditLog")]
public partial class AuditLog
{
    [Key]
    [Column("RecordID")]
    [StringLength(32)]
#if NetCore
    [Unicode(false)]
#endif
    public string RecordId { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    [StringLength(50)]
    public string Username { get; set; }

    [Column("ItemID")]
    public int? ItemId { get; set; }

    [Column("SeverityID")]
    public int SeverityId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FinishDate { get; set; }

    [Required]
    [StringLength(50)]
#if NetCore
    [Unicode(false)]
#endif
    public string SourceName { get; set; }

    [Required]
    [StringLength(50)]
#if NetCore
    [Unicode(false)]
#endif
    public string TaskName { get; set; }

    [StringLength(100)]
    public string ItemName { get; set; }

    [Column(TypeName = "ntext")]
    public string ExecutionLog { get; set; }

    [Column("PackageID")]
    public int? PackageId { get; set; }
}
#endif