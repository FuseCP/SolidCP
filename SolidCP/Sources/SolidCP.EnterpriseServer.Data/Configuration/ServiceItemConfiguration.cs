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

public partial class ServiceItemConfiguration: Extensions.EntityTypeConfiguration<ServiceItem>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public ServiceItemConfiguration(): base() { }
    public ServiceItemConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasOne(d => d.ItemType).WithMany(p => p.ServiceItems).HasConstraintName("FK_ServiceItems_ServiceItemTypes");

        HasOne(d => d.Package).WithMany(p => p.ServiceItems).HasConstraintName("FK_ServiceItems_Packages");

        HasOne(d => d.Service).WithMany(p => p.ServiceItems).HasConstraintName("FK_ServiceItems_Services");
    }
#endif
}
