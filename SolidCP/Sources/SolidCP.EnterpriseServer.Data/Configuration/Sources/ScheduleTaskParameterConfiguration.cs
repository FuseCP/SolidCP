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

public partial class ScheduleTaskParameterConfiguration: EntityTypeConfiguration<ScheduleTaskParameter>
{

#if NetCore || NetFX
    public override void Configure() {
        HasOne(d => d.Task).WithMany(p => p.ScheduleTaskParameters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScheduleTaskParameters_ScheduleTasks");
    }
#endif
}
