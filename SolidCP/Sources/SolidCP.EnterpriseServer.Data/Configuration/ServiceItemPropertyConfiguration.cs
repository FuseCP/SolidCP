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

public partial class ServiceItemPropertyConfiguration: EntityTypeConfiguration<ServiceItemProperty>
{
    public override void Configure() {

        if (IsCore && IsSqlite) Property(e => e.PropertyName).HasColumnType("TEXT COLLATE NOCASE");

#if NetCore
        HasOne(d => d.Item).WithMany(p => p.ServiceItemProperties).HasConstraintName("FK_ServiceItemProperties_ServiceItems");
#else
        HasRequired(d => d.Item).WithMany(p => p.ServiceItemProperties);
#endif
    }
}
