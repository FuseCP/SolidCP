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
    public override void Configure() {

        Property(e => e.ADAuthenticationType).IsUnicode(false);
		if (IsSqlServer) Property(e => e.Comments).HasColumnType("ntext");
		else if (IsCore && (IsMySql || IsMariaDb || IsSqlite || IsPostgreSql))
		{
			Property(e => e.Comments).HasColumnType("TEXT");
			if (IsSqlite) Property(e => e.ADAuthenticationType).HasColumnType("TEXT COLLATE NOCASE");
		}

#if NetCore
        Property(e => e.ADEnabled).HasDefaultValue(false);
		Property(e => e.VirtualServer).HasDefaultValue(false);
        Property(e => e.ServerUrl).HasDefaultValue("");
		Property(e => e.OSPlatform).HasDefaultValue(Providers.OS.OSPlatform.Unknown);
		Property(e => e.PasswordIsSHA256).HasDefaultValue(false);


        HasOne(d => d.PrimaryGroup).WithMany(p => p.Servers).HasConstraintName("FK_Servers_ResourceGroups");
#else
		HasOptional(d => d.PrimaryGroup).WithMany(p => p.Servers);
#endif
    }
}