#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class AuditLog
{
    public string RecordId { get; set; }

    public int? UserId { get; set; }

    public string Username { get; set; }

    public int? ItemId { get; set; }

    public int SeverityId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime FinishDate { get; set; }

    public string SourceName { get; set; }

    public string TaskName { get; set; }

    public string ItemName { get; set; }

    public string ExecutionLog { get; set; }

    public int? PackageId { get; set; }
}
#endif