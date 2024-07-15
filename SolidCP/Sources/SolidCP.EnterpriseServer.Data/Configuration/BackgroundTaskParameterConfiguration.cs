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

using BackgroundTaskParameter = SolidCP.EnterpriseServer.Data.Entities.BackgroundTaskParameter;

public partial class BackgroundTaskParameterConfiguration: EntityTypeConfiguration<BackgroundTaskParameter>
{
    public override void Configure() {
		HasKey(e => e.ParameterId).HasName("PK__Backgrou__F80C629777BF580B");
		if (IsSqlServer)
		{
			Property(e => e.SerializerValue).HasColumnType("ntext");
		}
		else if (IsCore && (IsMySql || IsMariaDb || IsSqlite || IsPostgreSql))
		{
			Property(e => e.SerializerValue).HasColumnType("TEXT");
		}

#if NetCore
		HasOne(d => d.Task).WithMany(p => p.BackgroundTaskParameters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Backgroun__TaskI__7AA72534");
#else
		HasRequired(d => d.Task).WithMany(p => p.BackgroundTaskParameters);
#endif
    }
}
