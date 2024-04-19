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

public partial class HostingPlanQuotaConfiguration: Extensions.EntityTypeConfiguration<HostingPlanQuota>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public HostingPlanQuotaConfiguration(): base() { }
    public HostingPlanQuotaConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => new { e.PlanId, e.QuotaId }).HasName("PK_HostingPlanQuotas_1");

        Property(e => e.PlanId).HasColumnName("PlanID");
        Property(e => e.QuotaId).HasColumnName("QuotaID");

        HasOne(d => d.Plan).WithMany(p => p.HostingPlanQuota)
                .HasForeignKey(d => d.PlanId)
                .HasConstraintName("FK_HostingPlanQuotas_HostingPlans");

        HasOne(d => d.Quota).WithMany(p => p.HostingPlanQuota)
                .HasForeignKey(d => d.QuotaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HostingPlanQuotas_Quotas");
    }
#endif
}
