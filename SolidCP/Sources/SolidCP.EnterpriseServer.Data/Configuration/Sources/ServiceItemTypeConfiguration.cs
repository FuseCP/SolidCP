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

public partial class ServiceItemTypeConfiguration: EntityTypeConfiguration<ServiceItemType>
{

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
