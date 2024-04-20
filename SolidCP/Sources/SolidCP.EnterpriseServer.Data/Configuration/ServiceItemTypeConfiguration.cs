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

public partial class ServiceItemTypeConfiguration: Extensions.EntityTypeConfiguration<ServiceItemType>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public ServiceItemTypeConfiguration(): base() { }
    public ServiceItemTypeConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        Property(e => e.ItemTypeId).ValueGeneratedNever();
        Property(e => e.Backupable).HasDefaultValue(true);
        Property(e => e.Importable).HasDefaultValue(true);
        Property(e => e.TypeOrder).HasDefaultValue(1);

        HasOne(d => d.Group).WithMany(p => p.ServiceItemTypes).HasConstraintName("FK_ServiceItemTypes_ResourceGroups");
    }
#endif
}
