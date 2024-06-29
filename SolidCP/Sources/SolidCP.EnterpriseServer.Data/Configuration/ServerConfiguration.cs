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

public partial class ServerConfiguration: EntityTypeConfiguration<Server>
{
#if NetCore || NetFX
    public override void Configure() {

        Property(e => e.ADAuthenticationType).IsUnicode(false);
		if (IsMsSql) Property(e => e.Comments).HasColumnType("ntext");
		else if (IsMySql || IsMariaDb || IsSqlite || IsPostgreSql)
		{
#if NetCore
			Property(e => e.Comments).HasColumnType("TEXT");
#endif
		}

#if NetCore
        Property(e => e.ADEnabled).HasDefaultValue(false);
        Property(e => e.ServerUrl).HasDefaultValue("");

        HasOne(d => d.PrimaryGroup).WithMany(p => p.Servers).HasConstraintName("FK_Servers_ResourceGroups");
#else
		// TODO optional or required?
		HasOptional(d => d.PrimaryGroup).WithMany(p => p.Servers);
#endif
    }
#endif
		}
