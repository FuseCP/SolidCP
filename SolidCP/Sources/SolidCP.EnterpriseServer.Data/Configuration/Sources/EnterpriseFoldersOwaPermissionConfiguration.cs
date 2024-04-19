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

public partial class EnterpriseFoldersOwaPermissionConfiguration: Extensions.EntityTypeConfiguration<EnterpriseFoldersOwaPermission>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public EnterpriseFoldersOwaPermissionConfiguration(): base() { }
    public EnterpriseFoldersOwaPermissionConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__Enterpri__3214EC27D1B48691");

        HasIndex(e => e.AccountId, "EnterpriseFoldersOwaPermissionsIdx_AccountID");

        HasIndex(e => e.FolderId, "EnterpriseFoldersOwaPermissionsIdx_FolderID");

        Property(e => e.Id).HasColumnName("ID");
        Property(e => e.AccountId).HasColumnName("AccountID");
        Property(e => e.FolderId).HasColumnName("FolderID");
        Property(e => e.ItemId).HasColumnName("ItemID");

        HasOne(d => d.Account).WithMany(p => p.EnterpriseFoldersOwaPermissions)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_EnterpriseFoldersOwaPermissions_AccountId");

        HasOne(d => d.Folder).WithMany(p => p.EnterpriseFoldersOwaPermissions)
                .HasForeignKey(d => d.FolderId)
                .HasConstraintName("FK_EnterpriseFoldersOwaPermissions_FolderId");
    }
#endif
}
