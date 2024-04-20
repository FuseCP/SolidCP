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

        HasOne(d => d.Account).WithMany(p => p.WebDavAccessTokens).HasConstraintName("FK_WebDavAccessTokens_UserId");
    }
#endif
}
