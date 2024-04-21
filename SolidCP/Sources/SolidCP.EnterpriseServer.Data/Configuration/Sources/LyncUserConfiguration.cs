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

public partial class LyncUserConfiguration: Extensions.EntityTypeConfiguration<LyncUser>
{
    public LyncUserConfiguration(): base() { }
    public LyncUserConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
        Property(e => e.ModifiedDate).HasDefaultValueSql("(getdate())");

        HasOne(d => d.LyncUserPlan).WithMany(p => p.LyncUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LyncUsers_LyncUserPlans");
    }
#endif
}
