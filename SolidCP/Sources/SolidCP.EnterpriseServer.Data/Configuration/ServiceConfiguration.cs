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

public partial class ServiceConfiguration: EntityTypeConfiguration<Service>
{
    public override void Configure() {

		if (IsSqlServer) Property(e => e.Comments).HasColumnType("ntext");
		else if (IsCore && (IsMySql || IsMariaDb || IsSqlite || IsPostgreSql))
		{
            Property(e => e.Comments).HasColumnType("TEXT");
		}

#if NetCore
        HasOne(d => d.Cluster).WithMany(p => p.Services).HasConstraintName("FK_Services_Clusters");

        HasOne(d => d.Provider).WithMany(p => p.Services)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Services_Providers");

        HasOne(d => d.Server).WithMany(p => p.Services)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Services_Servers");
#else
		//TODO optional or required?
		HasOptional(d => d.Cluster).WithMany(p => p.Services);
        HasRequired(d => d.Provider).WithMany(p => p.Services);
        HasRequired(d => d.Server).WithMany(p => p.Services);
#endif
    }
}
