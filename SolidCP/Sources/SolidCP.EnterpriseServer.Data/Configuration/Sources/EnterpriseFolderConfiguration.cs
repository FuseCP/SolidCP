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

public partial class EnterpriseFolderConfiguration: Extensions.EntityTypeConfiguration<EnterpriseFolder>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public EnterpriseFolderConfiguration(): base() { }
    public EnterpriseFolderConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasIndex(e => e.StorageSpaceFolderId, "EnterpriseFoldersIdx_StorageSpaceFolderId");

        Property(e => e.EnterpriseFolderId).HasColumnName("EnterpriseFolderID");
        Property(e => e.Domain).HasMaxLength(255);
        Property(e => e.FolderName)
                .IsRequired()
                .HasMaxLength(255);
        Property(e => e.HomeFolder).HasMaxLength(255);
        Property(e => e.ItemId).HasColumnName("ItemID");
        Property(e => e.LocationDrive).HasMaxLength(255);

        HasOne(d => d.StorageSpaceFolder).WithMany(p => p.EnterpriseFolders)
                .HasForeignKey(d => d.StorageSpaceFolderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_EnterpriseFolders_StorageSpaceFolderId");
    }
#endif
}
