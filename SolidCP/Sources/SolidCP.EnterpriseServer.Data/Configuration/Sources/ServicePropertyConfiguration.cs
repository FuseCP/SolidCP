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

public partial class ServicePropertyConfiguration: EntityTypeConfiguration<ServiceProperty>
{
    public override void Configure() {
        HasKey(e => new { e.ServiceId, e.PropertyName }).HasName("PK_ServiceProperties_1");

        HasOne(d => d.Service).WithMany(p => p.ServiceProperties).HasConstraintName("FK_ServiceProperties_Services");
    }
}
