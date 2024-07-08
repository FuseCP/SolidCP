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

public partial class CrmuserConfiguration: EntityTypeConfiguration<Crmuser>
{
    public override void Configure() {
        Property(e => e.ChangedDate).HasDefaultValueSql("(getdate())");
        Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

        HasOne(d => d.Account).WithMany(p => p.Crmusers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRMUsers_ExchangeAccounts");
    }
}
