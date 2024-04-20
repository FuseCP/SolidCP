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

public partial class ServicePropertyConfiguration: Extensions.EntityTypeConfiguration<ServiceProperty>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public ServicePropertyConfiguration(): base() { }
    public ServicePropertyConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => new { e.ServiceId, e.PropertyName }).HasName("PK_ServiceProperties_1");

        HasOne(d => d.Service).WithMany(p => p.ServiceProperties).HasConstraintName("FK_ServiceProperties_Services");
    }
#endif
}
