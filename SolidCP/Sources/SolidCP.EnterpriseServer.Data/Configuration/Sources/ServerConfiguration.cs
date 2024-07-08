// This file is auto generated, do not edit.
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

public partial class ServerConfiguration: EntityTypeConfiguration<Server>
{
    public override void Configure() {
        Property(e => e.Adenabled).HasDefaultValue(false);
        Property(e => e.ServerUrl).HasDefaultValue("");

        HasOne(d => d.PrimaryGroup).WithMany(p => p.Servers).HasConstraintName("FK_Servers_ResourceGroups");
    }
}
