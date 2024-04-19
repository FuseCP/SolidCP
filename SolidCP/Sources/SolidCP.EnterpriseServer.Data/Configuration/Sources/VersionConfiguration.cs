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

using Version = SolidCP.EnterpriseServer.Data.Entities.Version;

public partial class VersionConfiguration: Extensions.EntityTypeConfiguration<Version>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public VersionConfiguration(): base() { }
    public VersionConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.DatabaseVersion);

        Property(e => e.DatabaseVersion)
                .HasMaxLength(50)
                .IsUnicode(false);
        Property(e => e.BuildDate).HasColumnType("datetime");
    }
#endif
}
