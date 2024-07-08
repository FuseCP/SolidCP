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

public partial class EnterpriseFolderConfiguration: EntityTypeConfiguration<EnterpriseFolder>
{
    public override void Configure() {
        HasOne(d => d.StorageSpaceFolder).WithMany(p => p.EnterpriseFolders)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_EnterpriseFolders_StorageSpaceFolderId");
    }
}
