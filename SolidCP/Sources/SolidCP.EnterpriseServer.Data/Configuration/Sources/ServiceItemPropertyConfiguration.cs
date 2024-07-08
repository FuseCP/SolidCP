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

public partial class ServiceItemPropertyConfiguration: EntityTypeConfiguration<ServiceItemProperty>
{
    public override void Configure() {
        HasOne(d => d.Item).WithMany(p => p.ServiceItemProperties).HasConstraintName("FK_ServiceItemProperties_ServiceItems");
    }
}
