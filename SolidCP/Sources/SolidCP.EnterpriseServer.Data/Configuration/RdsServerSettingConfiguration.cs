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

public partial class RdsServerSettingConfiguration: EntityTypeConfiguration<RdsServerSetting>
{
    public override void Configure() {

		if (IsSqlServer) Property(e => e.PropertyValue).HasColumnType("ntext");
		else if (IsCore && (IsMySql || IsMariaDb || IsSqlite || IsPostgreSql))
		{
			Property(e => e.PropertyValue).HasColumnType("TEXT");
		}

    }
}
