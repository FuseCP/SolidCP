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

public partial class HostingPlanResourceConfiguration: EntityTypeConfiguration<HostingPlanResource>
{
    public override void Configure() {
        HasOne(d => d.Group).WithMany(p => p.HostingPlanResources)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HostingPlanResources_ResourceGroups");

        HasOne(d => d.Plan).WithMany(p => p.HostingPlanResources).HasConstraintName("FK_HostingPlanResources_HostingPlans");
    }
}
