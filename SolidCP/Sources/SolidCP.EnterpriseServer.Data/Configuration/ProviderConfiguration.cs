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

public partial class ProviderConfiguration: Extensions.EntityTypeConfiguration<Provider>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public ProviderConfiguration(): base() { }
    public ProviderConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.ProviderId).HasName("PK_ServiceTypes");

        Property(e => e.ProviderId).ValueGeneratedNever();

        HasOne(d => d.Group).WithMany(p => p.Providers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Providers_ResourceGroups");
    }
#endif
}
