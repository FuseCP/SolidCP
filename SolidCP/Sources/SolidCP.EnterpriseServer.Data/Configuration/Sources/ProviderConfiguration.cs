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

public partial class ProviderConfiguration: EntityTypeConfiguration<Provider>
{

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
