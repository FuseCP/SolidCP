// This file is auto generated, do not edit.
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

public partial class OcsuserConfiguration : EntityTypeConfiguration<Ocsuser>
{
#if NetCore || NetFX
	public override void Configure()
	{
#if NetCore
		if (IsMsSql) {
			Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
			Property(e => e.ModifiedDate).HasDefaultValueSql("(getdate())");
		}
#endif
	}
#endif
}