using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class ServiceItemTypeConfiguration: Extensions.EntityTypeConfiguration<ServiceItemType>
{
    public ServiceItemTypeConfiguration(): base() { }
    public ServiceItemTypeConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {

#if NetCore
        Property(e => e.ItemTypeId).ValueGeneratedNever();
        Property(e => e.Backupable).HasDefaultValue(true);
        Property(e => e.Importable).HasDefaultValue(true);
        Property(e => e.TypeOrder).HasDefaultValue(1);

        HasOne(d => d.Group).WithMany(p => p.ServiceItemTypes).HasConstraintName("FK_ServiceItemTypes_ResourceGroups");
#else
        HasRequired(d => d.Group).WithMany(p => p.ServiceItemTypes);
#endif
    }
#endif
    }
