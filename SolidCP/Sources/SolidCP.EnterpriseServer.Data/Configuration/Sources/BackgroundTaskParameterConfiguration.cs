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

using BackgroundTaskParameter = SolidCP.EnterpriseServer.Data.Entities.BackgroundTaskParameter;

public partial class BackgroundTaskParameterConfiguration: EntityTypeConfiguration<BackgroundTaskParameter>
{

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.ParameterId).HasName("PK__Backgrou__F80C62971030BD7B");

        HasOne(d => d.Task).WithMany(p => p.BackgroundTaskParameters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Backgroun__TaskI__3F6663D5");
    }
#endif
}