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

        HasOne(d => d.Account).WithMany(p => p.EnterpriseFoldersOwaPermissions).HasConstraintName("FK_EnterpriseFoldersOwaPermissions_AccountId");

        HasOne(d => d.Folder).WithMany(p => p.EnterpriseFoldersOwaPermissions).HasConstraintName("FK_EnterpriseFoldersOwaPermissions_FolderId");
    }
#endif
}
