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

public partial class StorageSpaceConfiguration: EntityTypeConfiguration<StorageSpace>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__StorageS__3214EC0756F21A09");

        HasOne(d => d.Server).WithMany(p => p.StorageSpaces).HasConstraintName("FK_StorageSpaces_ServerId");

        HasOne(d => d.Service).WithMany(p => p.StorageSpaces).HasConstraintName("FK_StorageSpaces_ServiceId");
    }
}
