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

public partial class ServiceDefaultPropertyConfiguration: EntityTypeConfiguration<ServiceDefaultProperty>
{

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => new { e.ProviderId, e.PropertyName }).HasName("PK_ServiceDefaultProperties_1");

        HasOne(d => d.Provider).WithMany(p => p.ServiceDefaultProperties)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceDefaultProperties_Providers");
    }
#endif
}
