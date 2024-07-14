using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class WebDavAccessTokenConfiguration: EntityTypeConfiguration<WebDavAccessToken>
{
    public override void Configure() {

		HasKey(e => e.Id).HasName("PK__WebDavAc__3214EC2708781F08");
		if (IsSqlServer) Property(e => e.ExpirationDate).HasColumnType("datetime");
        else if (IsCore && IsSqliteFX) Property(e => e.AccessToken).HasColumnType("BLOB");

#if NetCore
		HasOne(d => d.Account).WithMany(p => p.WebDavAccessTokens).HasConstraintName("FK_WebDavAccessTokens_UserId");
#else
        HasRequired(d => d.Account).WithMany(p => p.WebDavAccessTokens);
#endif
    }
}