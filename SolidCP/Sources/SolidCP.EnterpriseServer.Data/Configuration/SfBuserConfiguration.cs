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

public partial class SfBUserConfiguration: EntityTypeConfiguration<SfBUser>
{
#if NetCore || NetFX
    public override void Configure() {

		if (IsMsSql)
		{
			Property(e => e.CreatedDate).HasColumnType("datetime");
			Property(e => e.ModifiedDate).HasColumnType("datetime");
		}
	}
#endif
}
