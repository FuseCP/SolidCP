﻿// This file is auto generated, do not edit.
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

public partial class QuotaConfiguration: EntityTypeConfiguration<Quota>
{

#if NetCore || NetFX
    public override void Configure() {
        Property(e => e.QuotaId).ValueGeneratedNever();
        Property(e => e.QuotaOrder).HasDefaultValue(1);
        Property(e => e.QuotaTypeId).HasDefaultValue(2);
        Property(e => e.ServiceQuota).HasDefaultValue(false);

        HasOne(d => d.Group).WithMany(p => p.Quota).HasConstraintName("FK_Quotas_ResourceGroups");

        HasOne(d => d.ItemType).WithMany(p => p.Quota).HasConstraintName("FK_Quotas_ServiceItemTypes");
    }
#endif
}