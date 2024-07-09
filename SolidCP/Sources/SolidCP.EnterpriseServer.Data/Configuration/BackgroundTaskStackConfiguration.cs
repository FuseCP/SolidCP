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

public partial class BackgroundTaskStackConfiguration: EntityTypeConfiguration<BackgroundTaskStack>
{
    public override void Configure() {
		HasKey(e => e.TaskStackId).HasName("PK__Backgrou__5E44466FB8A5F217");

#if NetCore
        HasOne(d => d.Task).WithMany(p => p.BackgroundTaskStacks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Backgroun__TaskI__005FFE8A");
#else
		HasRequired(d => d.Task).WithMany(p => p.BackgroundTaskStacks);
#endif

    }
}
