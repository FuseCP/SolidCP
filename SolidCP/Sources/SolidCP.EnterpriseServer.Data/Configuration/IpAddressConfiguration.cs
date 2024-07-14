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

public partial class IpAddressConfiguration: EntityTypeConfiguration<IpAddress>
{
    public override void Configure() {

        Property(e => e.ExternalIp).IsUnicode(false);
        Property(e => e.InternalIp).IsUnicode(false);
        Property(e => e.SubnetMask).IsUnicode(false);
        Property(e => e.DefaultGateway).IsUnicode(false);
		if (IsSqlServer) Property(e => e.Comments).HasColumnType("ntext");
		else if (IsCore && (IsMySql || IsMariaDb || IsSqlite || IsPostgreSql))
		{
            Property(e => e.Comments).HasColumnType("TEXT");
        }


#if NetCore
        HasOne(d => d.Server).WithMany(p => p.IpAddresses)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_IPAddresses_Servers");
#else
		// TODO optional or required and cascade delete?
		HasOptional(d => d.Server).WithMany(p => p.IpAddresses).WillCascadeOnDelete();
#endif
    }
}
