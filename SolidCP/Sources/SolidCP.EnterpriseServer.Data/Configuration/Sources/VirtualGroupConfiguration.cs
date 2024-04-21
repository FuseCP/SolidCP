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

public partial class VirtualGroupConfiguration: Extensions.EntityTypeConfiguration<VirtualGroup>
{
    public VirtualGroupConfiguration(): base() { }
    public VirtualGroupConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasOne(d => d.Group).WithMany(p => p.VirtualGroups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VirtualGroups_ResourceGroups");

        HasOne(d => d.Server).WithMany(p => p.VirtualGroups).HasConstraintName("FK_VirtualGroups_Servers");
    }
#endif
}
