﻿using System;
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

public partial class AccessTokenConfiguration: EntityTypeConfiguration<AccessToken>
{
#if NetCore || NetFX
	public override void Configure() {
        HasKey(e => e.Id).HasName("PK__AccessTo__3214EC27A32557FE");

        Property(e => e.SmsResponse).IsUnicode(false);
#if NetCore
        HasOne(d => d.Account).WithMany(p => p.AccessTokens).HasConstraintName("FK_AccessTokens_UserId");
#else
        HasRequired(d => d.Account).WithMany(p => p.AccessTokens);
#endif
    }
#endif
}