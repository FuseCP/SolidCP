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

public partial class ExchangeDeletedAccountConfiguration : EntityTypeConfiguration<ExchangeDeletedAccount>
{
	public override void Configure()
	{
		HasKey(e => e.Id).HasName("PK__Exchange__3214EC27EF1C22C1");
		if (IsSqlServer) Property(e => e.ExpirationDate).HasColumnType("datetime");
    }
}
