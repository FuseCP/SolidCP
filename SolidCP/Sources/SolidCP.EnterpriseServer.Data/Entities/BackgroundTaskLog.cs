// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class BackgroundTaskLog
{
    public int LogId { get; set; }

    public int TaskId { get; set; }

    public DateTime? Date { get; set; }

    public string ExceptionStackTrace { get; set; }

    public int? InnerTaskStart { get; set; }

    public int? Severity { get; set; }

    public string Text { get; set; }

    public int? TextIdent { get; set; }

    public string XmlParameters { get; set; }

    public virtual BackgroundTask Task { get; set; }
}
#endif