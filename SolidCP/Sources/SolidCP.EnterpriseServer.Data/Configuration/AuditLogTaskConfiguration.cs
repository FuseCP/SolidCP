using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class AuditLogTaskConfiguration: Extensions.EntityTypeConfiguration<AuditLogTask>
{
    public AuditLogTaskConfiguration(): base() { }
    public AuditLogTaskConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => new { e.SourceName, e.TaskName }).HasName("PK_LogActions");
    }
#endif
}
