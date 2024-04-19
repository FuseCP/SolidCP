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

public partial class ExchangeOrganizationSsFolderConfiguration: Extensions.EntityTypeConfiguration<ExchangeOrganizationSsFolder>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public ExchangeOrganizationSsFolderConfiguration(): base() { }
    public ExchangeOrganizationSsFolderConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__Exchange__3214EC072DDBA072");

        HasIndex(e => e.ItemId, "ExchangeOrganizationSsFoldersIdx_ItemId");

        HasIndex(e => e.StorageSpaceFolderId, "ExchangeOrganizationSsFoldersIdx_StorageSpaceFolderId");

        Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

        HasOne(d => d.Item).WithMany(p => p.ExchangeOrganizationSsFolders)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_ExchangeOrganizationSsFolders_ItemId");

        HasOne(d => d.StorageSpaceFolder).WithMany(p => p.ExchangeOrganizationSsFolders)
                .HasForeignKey(d => d.StorageSpaceFolderId)
                .HasConstraintName("FK_ExchangeOrganizationSsFolders_StorageSpaceFolderId");
    }
#endif
}
