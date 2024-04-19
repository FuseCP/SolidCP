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

public partial class WebDavAccessTokenConfiguration: Extensions.EntityTypeConfiguration<WebDavAccessToken>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public WebDavAccessTokenConfiguration(): base() { }
    public WebDavAccessTokenConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__WebDavAc__3214EC27B27DC571");

        HasIndex(e => e.AccountId, "WebDavAccessTokensIdx_AccountID");

        Property(e => e.Id).HasColumnName("ID");
        Property(e => e.AccountId).HasColumnName("AccountID");
        Property(e => e.AuthData).IsRequired();
        Property(e => e.ExpirationDate).HasColumnType("datetime");
        Property(e => e.FilePath).IsRequired();

        HasOne(d => d.Account).WithMany(p => p.WebDavAccessTokens)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_WebDavAccessTokens_UserId");
    }
#endif
}
