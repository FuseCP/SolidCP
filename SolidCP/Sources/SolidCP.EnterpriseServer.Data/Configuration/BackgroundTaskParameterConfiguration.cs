// This file is auto generated, do not edit.
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

using BackgroundTaskParameter = SolidCP.EnterpriseServer.Data.Entities.BackgroundTaskParameter;

public partial class BackgroundTaskParameterConfiguration: Extensions.EntityTypeConfiguration<BackgroundTaskParameter>
{
    public BackgroundTaskParameterConfiguration(): base() { }
    public BackgroundTaskParameterConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.ParameterId).HasName("PK__Backgrou__F80C6297E2E5AF88");

#if NetCore
        HasOne(d => d.Task).WithMany(p => p.BackgroundTaskParameters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Backgroun__TaskI__03D16812");
#else
        HasRequired(d => d.Task).WithMany(p => p.BackgroundTaskParameters);
#endif
    }
#endif
    }
