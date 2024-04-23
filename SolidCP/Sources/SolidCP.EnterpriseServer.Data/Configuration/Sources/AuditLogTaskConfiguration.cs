// This file is auto generated, do not edit.
using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
using SolidCP.EnterpriseServer.Data.Extensions;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class AuditLogTaskConfiguration: EntityTypeConfiguration<AuditLogTask>
{

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => new { e.SourceName, e.TaskName }).HasName("PK_LogActions");
    }
#endif
}
