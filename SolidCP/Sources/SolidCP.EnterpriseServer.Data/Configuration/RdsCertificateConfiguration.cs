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

public partial class RdsCertificateConfiguration: EntityTypeConfiguration<RdsCertificate>
{
    public override void Configure() {

		if (IsSqlServer)
		{
			Property(e => e.Content).HasColumnType("ntext");
			Property(e => e.ValidFrom).HasColumnType("datetime");
			Property(e => e.ExpiryDate).HasColumnType("datetime");
		}
		else if (IsCore && (IsMySql || IsMariaDb || IsSqlite || IsPostgreSql))
		{
			Property(e => e.Content).HasColumnType("TEXT");
		}

    }
}
