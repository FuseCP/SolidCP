﻿using System;
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

public partial class AuditLogConfiguration: EntityTypeConfiguration<AuditLog>
{
#if NetCore || NetFX
	public override void Configure() {
        HasKey(e => e.RecordId).HasName("PK_Log");
        Property(e => e.RecordId).IsUnicode(false);
        Property(e => e.SourceName).IsUnicode(false);
        Property(e => e.TaskName).IsUnicode(false);
    }
#endif
}