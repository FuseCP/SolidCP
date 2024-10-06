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

public partial class AdditionalGroupConfiguration: EntityTypeConfiguration<AdditionalGroup>
{
    public override void Configure() {
		if (IsCore && IsSqlite) Property(e => e.GroupName).HasColumnType("TEXT COLLATE NOCASE");
		
		HasKey(e => e.Id).HasName("PK__Addition__3214EC27E665DDE2");
	}
}
