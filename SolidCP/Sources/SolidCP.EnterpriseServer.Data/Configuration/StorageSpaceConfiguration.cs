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

public partial class StorageSpaceConfiguration: EntityTypeConfiguration<StorageSpace>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__StorageS__3214EC07B8B9A6D1");

        Property(e => e.Name).IsUnicode(false);
        Property(e => e.Path).IsUnicode(false);
        Property(e => e.UncPath).IsUnicode(false);

#if NetCore
        Property(e => e.IsDisabled).HasDefaultValue(false);

        HasOne(d => d.Server).WithMany(p => p.StorageSpaces).HasConstraintName("FK_StorageSpaces_ServerId");

        HasOne(d => d.Service).WithMany(p => p.StorageSpaces).HasConstraintName("FK_StorageSpaces_ServiceId");
#else
        HasRequired(d => d.Server).WithMany(p => p.StorageSpaces);
        HasRequired(d => d.Service).WithMany(p => p.StorageSpaces);
#endif
    }
}
