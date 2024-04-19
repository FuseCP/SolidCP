// This file is auto generated, do not edit.
using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
#if NetCore
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
#endif
#if NetFX
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Spatial;
using System.Data.Entity.Validation;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class StorageSpaceFolderConfiguration: Extensions.EntityTypeConfiguration<StorageSpaceFolder>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public StorageSpaceFolderConfiguration(): base() { }
    public StorageSpaceFolderConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__StorageS__3214EC07AC0C9EB6");

        HasIndex(e => e.StorageSpaceId, "StorageSpaceFoldersIdx_StorageSpaceId");

        Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(300)
                .IsUnicode(false);
        Property(e => e.Path)
                .IsRequired()
                .IsUnicode(false);
        Property(e => e.UncPath).IsUnicode(false);

        HasOne(d => d.StorageSpace).WithMany(p => p.StorageSpaceFolders)
                .HasForeignKey(d => d.StorageSpaceId)
                .HasConstraintName("FK_StorageSpaceFolders_StorageSpaceId");
    }
#endif
}
