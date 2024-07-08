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

public partial class VirtualGroupConfiguration: EntityTypeConfiguration<VirtualGroup>
{
    public override void Configure() {

#if NetCore
        HasOne(d => d.Group).WithMany(p => p.VirtualGroups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VirtualGroups_ResourceGroups");

        HasOne(d => d.Server).WithMany(p => p.VirtualGroups).HasConstraintName("FK_VirtualGroups_Servers");
#else
        HasRequired(d => d.Group).WithMany(p => p.VirtualGroups);
        HasRequired(d => d.Server).WithMany(p => p.VirtualGroups);
#endif
    }
}