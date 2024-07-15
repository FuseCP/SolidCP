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

public partial class SslCertificateConfiguration: EntityTypeConfiguration<SslCertificate>
{
    public override void Configure() {

        Property(e => e.Id).ValueGeneratedOnAdd();

		if (IsSqlServer)
		{
			Property(e => e.Csr).HasColumnType("ntext");
			Property(e => e.Certificate).HasColumnType("ntext");
			Property(e => e.Hash).HasColumnType("ntext");
			Property(e => e.Pfx).HasColumnType("ntext");
			Property(e => e.ValidFrom).HasColumnType("datetime");
			Property(e => e.ExpiryDate).HasColumnType("datetime");
		}
		else if (IsCore && (IsMySql || IsMariaDb || IsSqlite || IsPostgreSql))
		{
			Property(e => e.Csr).HasColumnType("TEXT");
			Property(e => e.Certificate).HasColumnType("TEXT");
			Property(e => e.Hash).HasColumnType("TEXT");
			Property(e => e.Pfx).HasColumnType("TEXT");
		}

    }
}