using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class UsersDetailedConfiguration: EntityTypeConfiguration<UsersDetailed>
{
#if NetCore || NetFX
    public override void Configure() {

		if (IsMsSql)
		{
			Property(e => e.Comments).HasColumnType("ntext");
			Property(e => e.Changed).HasColumnType("datetime");
			Property(e => e.Created).HasColumnType("datetime");
		}
		else if (IsMsSql || IsMariaDb || IsSqlite || IsPostgreSql)
		{
#if NetCore
			Property(e => e.Comments).HasColumnType("TEXT");
#endif
		}

#if NetCore
        Core.ToView("UsersDetailed");
#else
		// The db will be created with EF Core 8, not with EF6, so the view will be in the database,
		// so we can use ToTable here
		ToTable("UsersDetailed");
#endif
    }
#endif
		}
