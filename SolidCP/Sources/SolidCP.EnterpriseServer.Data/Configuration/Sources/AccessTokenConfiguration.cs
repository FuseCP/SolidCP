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

public partial class AccessTokenConfiguration: Extensions.EntityTypeConfiguration<AccessToken>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public AccessTokenConfiguration(): base() { }
    public AccessTokenConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__AccessTo__3214EC27A32557FE");

        HasIndex(e => e.AccountId, "AccessTokensIdx_AccountID");

        Property(e => e.Id).HasColumnName("ID");
        Property(e => e.AccountId).HasColumnName("AccountID");
        Property(e => e.ExpirationDate).HasColumnType("datetime");
        Property(e => e.SmsResponse)
                .HasMaxLength(100)
                .IsUnicode(false);

        HasOne(d => d.Account).WithMany(p => p.AccessTokens)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_AccessTokens_UserId");
    }
#endif
}
