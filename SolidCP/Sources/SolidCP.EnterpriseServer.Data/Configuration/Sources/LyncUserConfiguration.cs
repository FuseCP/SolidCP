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

public partial class LyncUserConfiguration: Extensions.EntityTypeConfiguration<LyncUser>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public LyncUserConfiguration(): base() { }
    public LyncUserConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasIndex(e => e.LyncUserPlanId, "LyncUsersIdx_LyncUserPlanID");

        Property(e => e.LyncUserId).HasColumnName("LyncUserID");
        Property(e => e.AccountId).HasColumnName("AccountID");
        Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        Property(e => e.LyncUserPlanId).HasColumnName("LyncUserPlanID");
        Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        Property(e => e.SipAddress).HasMaxLength(300);

        HasOne(d => d.LyncUserPlan).WithMany(p => p.LyncUsers)
                .HasForeignKey(d => d.LyncUserPlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LyncUsers_LyncUserPlans");
    }
#endif
}
