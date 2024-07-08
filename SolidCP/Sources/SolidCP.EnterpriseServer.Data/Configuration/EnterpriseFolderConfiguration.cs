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

public partial class EnterpriseFolderConfiguration: EntityTypeConfiguration<EnterpriseFolder>
{
    public override void Configure() {

#if NetCore
        HasOne(d => d.StorageSpaceFolder).WithMany(p => p.EnterpriseFolders)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_EnterpriseFolders_StorageSpaceFolderId");
#else
        HasOptional(d => d.StorageSpaceFolder).WithMany(p => p.EnterpriseFolders);
#endif
    }
}
