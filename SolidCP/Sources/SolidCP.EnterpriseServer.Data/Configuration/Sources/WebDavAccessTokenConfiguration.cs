﻿// This file is auto generated, do not edit.
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

public partial class WebDavAccessTokenConfiguration: EntityTypeConfiguration<WebDavAccessToken>
{

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__WebDavAc__3214EC27E061C787");

        HasOne(d => d.Account).WithMany(p => p.WebDavAccessTokens).HasConstraintName("FK_WebDavAccessTokens_UserId");
    }
#endif
}