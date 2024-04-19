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

public partial class BlackBerryUserConfiguration: Extensions.EntityTypeConfiguration<BlackBerryUser>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public BlackBerryUserConfiguration(): base() { }
    public BlackBerryUserConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasIndex(e => e.AccountId, "BlackBerryUsersIdx_AccountId");

        Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        Property(e => e.ModifiedDate).HasColumnType("datetime");

        HasOne(d => d.Account).WithMany(p => p.BlackBerryUsers)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BlackBerryUsers_ExchangeAccounts");
    }
#endif
}
