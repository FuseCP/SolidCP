using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
using SolidCP.EnterpriseServer.Data.Extensions;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class DmzIpAddressConfiguration: EntityTypeConfiguration<DmzIpAddress>
{
    public override void Configure() {
        if (IsCore && IsSqlite) Property(e => e.IpAddress).HasColumnType("TEXT COLLATE NOCASE");

#if NetCore
        HasOne(d => d.Item).WithMany(p => p.DmzIpAddresses).HasConstraintName("FK_DmzIPAddresses_ServiceItems");
#else
        HasRequired(d => d.Item).WithMany(p => p.DmzIpAddresses);
#endif
    }
}
