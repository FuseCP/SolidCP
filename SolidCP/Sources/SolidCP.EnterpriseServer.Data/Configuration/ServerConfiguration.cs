// This file is auto generated, do not edit.
using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
#if NetCore
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
#endif
#if NetFX
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Spatial;
using System.Data.Entity.Validation;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class ServerConfiguration: Extensions.EntityTypeConfiguration<Server>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public ServerConfiguration(): base() { }
    public ServerConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        Property(e => e.Adenabled).HasDefaultValue(false);
        Property(e => e.ServerUrl).HasDefaultValue("");

        HasOne(d => d.PrimaryGroup).WithMany(p => p.Servers).HasConstraintName("FK_Servers_ResourceGroups");
    }
#endif
}
