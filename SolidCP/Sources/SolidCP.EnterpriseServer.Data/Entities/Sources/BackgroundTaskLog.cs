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
[Index("TaskId", Name = "BackgroundTaskLogsIdx_TaskID")]
#endif
public partial class BackgroundTaskLog
{
    [Key]
    [Column("LogID")]
    public int LogId { get; set; }

    [Column("TaskID")]
    public int TaskId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Date { get; set; }

    [Column(TypeName = "ntext")]
    public string ExceptionStackTrace { get; set; }

    public int? InnerTaskStart { get; set; }

    public int? Severity { get; set; }

    [Column(TypeName = "ntext")]
    public string Text { get; set; }

    public int? TextIdent { get; set; }

    [Column(TypeName = "ntext")]
    public string XmlParameters { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("BackgroundTaskLogs")]
    public virtual BackgroundTask Task { get; set; }
}
#endif