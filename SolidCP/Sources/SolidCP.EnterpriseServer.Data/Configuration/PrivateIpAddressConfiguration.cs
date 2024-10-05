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

public partial class PrivateIpAddressConfiguration: EntityTypeConfiguration<PrivateIpAddress>
{
    public override void Configure() {

        Property(e => e.IpAddress).IsUnicode(false);

        if (IsCore && IsSqlite) Property(e => e.IpAddress).HasColumnType("TEXT COLLATE NOCASE");

#if NetCore
        HasOne(d => d.Item).WithMany(p => p.PrivateIpAddresses).HasConstraintName("FK_PrivateIPAddresses_ServiceItems");
#else
        HasRequired(d => d.Item).WithMany(p => p.PrivateIpAddresses);
#endif
    }
}
