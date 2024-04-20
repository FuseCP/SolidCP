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

public partial class ScheduleTaskParameterConfiguration: Extensions.EntityTypeConfiguration<ScheduleTaskParameter>
{
    public ScheduleTaskParameterConfiguration(): base() { }
    public ScheduleTaskParameterConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {

#if NetCore
        HasOne(d => d.Task).WithMany(p => p.ScheduleTaskParameters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScheduleTaskParameters_ScheduleTasks");
#else
        HasRequired(d => d.Task).WithMany(p => p.ScheduleTaskParameters);
#endif
    }
#endif
}
