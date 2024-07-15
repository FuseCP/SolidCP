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

using BackgroundTask = SolidCP.EnterpriseServer.Data.Entities.BackgroundTask;

public partial class BackgroundTaskConfiguration: EntityTypeConfiguration<BackgroundTask>
{
    public override void Configure() {
		HasKey(e => e.Id).HasName("PK__Backgrou__3214EC273A1145AC");
		if (IsSqlServer)
        {
            Property(e => e.StartDate).HasColumnType("datetime");
            Property(e => e.FinishDate).HasColumnType("datetime");
        }
        else if (IsCore && IsSqliteFX) Property(e => e.Guid).HasColumnType("BLOB");
	}
}
