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

public partial class StorageSpaceLevelResourceGroupConfiguration: EntityTypeConfiguration<StorageSpaceLevelResourceGroup>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__StorageS__3214EC07623A93FD");

        HasOne(d => d.Group).WithMany(p => p.StorageSpaceLevelResourceGroups).HasConstraintName("FK_StorageSpaceLevelResourceGroups_GroupId");

        HasOne(d => d.Level).WithMany(p => p.StorageSpaceLevelResourceGroups).HasConstraintName("FK_StorageSpaceLevelResourceGroups_LevelId");
    }
}
