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

public partial class EnterpriseFoldersOwaPermissionConfiguration: EntityTypeConfiguration<EnterpriseFoldersOwaPermission>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__Enterpri__3214EC27136A25A7");

        HasOne(d => d.Account).WithMany(p => p.EnterpriseFoldersOwaPermissions).HasConstraintName("FK_EnterpriseFoldersOwaPermissions_AccountId");

        HasOne(d => d.Folder).WithMany(p => p.EnterpriseFoldersOwaPermissions).HasConstraintName("FK_EnterpriseFoldersOwaPermissions_FolderId");
    }
}
