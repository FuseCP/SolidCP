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

public partial class ServiceDefaultPropertyConfiguration: Extensions.EntityTypeConfiguration<ServiceDefaultProperty>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public ServiceDefaultPropertyConfiguration(): base() { }
    public ServiceDefaultPropertyConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => new { e.ProviderId, e.PropertyName }).HasName("PK_ServiceDefaultProperties_1");

        HasOne(d => d.Provider).WithMany(p => p.ServiceDefaultProperties)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceDefaultProperties_Providers");
    }
#endif
}
