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

public partial class BackgroundTaskLogConfiguration: EntityTypeConfiguration<BackgroundTaskLog>
{
    public override void Configure() {
        HasKey(e => e.LogId).HasName("PK__Backgrou__5E5499A86067A6E5");

        HasOne(d => d.Task).WithMany(p => p.BackgroundTaskLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Backgroun__TaskI__7D8391DF");
    }
}
